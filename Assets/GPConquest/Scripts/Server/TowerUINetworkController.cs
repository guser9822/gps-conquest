using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Linq;
using TC.Common;
using MarkLight.Views;
using TC.GPConquest.MarkLight4GPConquest.Server;
using TC.GPConquest.Common;

namespace TC.GPConquest.Server {

    public class TowerUINetworkController : TowerUIEntityBehavior
    {

        public TowerEntityController TowerEntityController { get; set; }
        public GameEntityRegister GameEntityRegister { get; set; }
        public GameObject TowerUIPrefab;
        [HideInInspector]
        public GameObject TowerUIInstantiated;
        [HideInInspector]
        public TowerEntityGameUI TowerEntityGameUI;
        private bool isServerProcess;
        public GameObject TowerEffect;
        [HideInInspector]
        public GameObject InstantiatedTowerEffect;

        //List of particle systems used for changing the color of the tower effect
        protected List<ParticleSystem> TowerEffectParticleSystems = new List<ParticleSystem>();

        //Keep in one place the color associated with each faction or actions like "capture of the tower"
        public static readonly Dictionary<string, Color> ColorsDictionary =
            new Dictionary<string, Color>()
            {
                { GameCommonNames.RED_FACTION,Color.red },
                { GameCommonNames.GREEN_FACTION, Color.green},
                { GameCommonNames.NO_FACTION, Color.cyan},
                { TowerCaptureController.STARTUP_STATE, Color.cyan},
                { TowerCaptureController.CAPTURE_IN_PROGRES, Color.yellow}
            };

        private void Awake()
        {
            //AvatorUI conflict with the UI that reside on the server process.
            //For this particular case  we don't make this UI visible on the server process.
            var server = FindObjectOfType<ServerNetworkController>();
            if ((!ReferenceEquals(server, null) && server.gameObject.tag == "ServerController"))
                isServerProcess = true;

            if (!isServerProcess)
            {
                TowerUIInstantiated = Instantiate<GameObject>(TowerUIPrefab);
                TowerEntityGameUI = TowerUIInstantiated.GetComponentInChildren<TowerEntityGameUI>();

                InstantiatedTowerEffect = Instantiate(TowerEffect);
                //Collects all the particle systems in order change the color of the tower effect
                TowerEffectParticleSystems.Add(InstantiatedTowerEffect.GetComponent<ParticleSystem>());
                TowerEffectParticleSystems.AddRange(InstantiatedTowerEffect.GetComponentsInChildren<ParticleSystem>());
                SetTowerEffectColor(GameCommonNames.NO_FACTION);
            }
        }

        /// <summary>
        /// Given a string, picks the color used for the effect of the tower.
        /// The string can be used for getting the faction color, or, for example
        /// the color associated with the capture of the tower.
        /// </summary>
        /// <param name="_actionName"></param>
        public void SetTowerEffectColor(string _actionName)
        {
            if (!ReferenceEquals(_actionName, null))
            {
                Color outColor;
                ColorsDictionary.TryGetValue(_actionName, out outColor);
                if (!ReferenceEquals(outColor, null))
                {
                    TowerEffectParticleSystems.ForEach(x => {
                        var mainModule = x.main;
                        mainModule.startColor = outColor;
                    });
                }
            }
            else Debug.LogError("Action given in input is null.");
        }

        public void InitializeTowerUINetworkController(TowerEntityController _towerEntityController)
        {

            //Update all attributes of this UI
            UpdateTowerUINetController(_towerEntityController);

            networkObject.SendRpc(RPC_UPDATE_TOWER_U_I_NET_CONTROLLER_ON_NETWORK,
                Receivers.AllBuffered,
                networkObject.TowerEntityNetID);

            //Destroy this object when the Tower is destroyed
            _towerEntityController.networkObject.onDestroy += NetworkObject_onDestroy;
        }

        private void UpdateTowerUINetController(TowerEntityController _towerEntityController)
        {
            _towerEntityController.TowerUINetworkController = this;
            TowerEntityController = _towerEntityController;
            networkObject.TowerEntityNetID = TowerEntityController.networkObject.NetworkId;
            var towerEntityTransf = _towerEntityController.GetComponent<Transform>();
            GameEntityRegister = _towerEntityController.GameEntityRegister;
            transform.SetParent(towerEntityTransf);

            // Set Tower UI only for non owner process (client of the player)
            if (!isServerProcess && !networkObject.IsOwner)
            {
                TowerUIInstantiated.gameObject.transform.SetParent(this.gameObject.transform);
                //Deactivates the EventSystem
                var eventSystem = TowerUIInstantiated.GetComponentInChildren<EventSystem>();
                eventSystem.gameObject.SetActive(false);
                //set the towerEntity as the parent of tower effect
                InstantiatedTowerEffect.transform.SetParent(TowerEntityController.transform);
                InstantiatedTowerEffect.transform.localPosition = Vector3.zero;
            }
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

        /// <summary>
        /// RPC used for update the tower ui network controller on the network
        /// </summary>
        /// <param name="args"></param>
        public override void UpdateTowerUINetControllerOnNetwork(RpcArgs args)
        {
            var towerEntityID = args.GetNext<uint>();

            //Find GameEntityRegister
            var gameEntityRegister = FindObjectOfType<GameEntityRegister>();
            var towerEntity = (TowerEntityController)gameEntityRegister.FindEntity(typeof(TowerEntityController), towerEntityID);

            UpdateTowerUINetController(towerEntity);
        }

        /// <summary>
        /// This method updates the Tower UI and visual effect of the towers on non 
        /// owners processes (on players client) using an RPC.
        /// </summary>
        /// <param name="_factionName"></param>
        /// <param name="_actionName"></param>
        public void CallChangeUIStatus(string _factionName, string _actionName)
        {
            if (!ReferenceEquals(_factionName, null) && _factionName.Length>0 &&
                !ReferenceEquals(_actionName, null) && _actionName.Length > 0)
            {
                /*Call an RPC in order to update TowerUIController on the network
                 * since, on the server process, there's no UI prefab instantiated
                 */
                networkObject.SendRpc(RPC_CHANGE_U_I_STATUS_ON_NETWORK,
                    true,
                    Receivers.AllBuffered,
                    _factionName,
                    _actionName);
            }
            else
            {
                Debug.LogError("Faction name cannot be null or empty. ");
            }
        }

        /// <summary>
        /// RPC used by CallChangeUIStatus for changing Towers state on the network.
        /// </summary>
        /// <param name="args"></param>
        public override void ChangeUIStatusOnNetwork(RpcArgs args)
        {
            //this if statement is just for fortify the assertion that this code must be executed only on non owner process
            //but it's not necessary since the nature of this call imply that
            if (!networkObject.IsOwner)
            {
                if (!ReferenceEquals(args, null))
                {
                    var factionName = args.GetNext<string>();
                    var actionName = args.GetNext<string>();
                    if (!ReferenceEquals(factionName, null) && factionName.Length > 0 &&
                        !ReferenceEquals(actionName, null) && actionName.Length > 0)
                    {
                        //Sets the winner faction in the pop of the tower
                        TowerEntityGameUI.ChangeTowerUIStatus(factionName);
                        SetTowerEffectColor(actionName);
                    }
                    else
                    {
                        Debug.LogError("Faction name is null or empty string");
                    }
                }
                else
                {
                    Debug.LogError("Arguments array from the nwetwork is null.");
                }
            }
        }

    }
}
