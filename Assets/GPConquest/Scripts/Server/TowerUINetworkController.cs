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
            }
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

            // Set Tower UI only for non owner process
            if (!isServerProcess && !networkObject.IsOwner)
            {
                TowerUIInstantiated.gameObject.transform.SetParent(this.gameObject.transform);
                //Deactivates the EventSystem
                var eventSystem = TowerUIInstantiated.GetComponentInChildren<EventSystem>();
                eventSystem.gameObject.SetActive(false);
            }
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

        public override void UpdateTowerUINetControllerOnNetwork(RpcArgs args)
        {
            var towerEntityID = args.GetNext<uint>();

            //Find GameEntityRegister
            var gameEntityRegister = FindObjectOfType<GameEntityRegister>();
            var towerEntity = (TowerEntityController)gameEntityRegister.FindEntity(typeof(TowerEntityController), towerEntityID);

            UpdateTowerUINetController(towerEntity);
        }

        public void CallChangeUIStatus(string _factionName)
        {
            if (!ReferenceEquals(_factionName, null) && _factionName.Length>0)
            {
                /*Call an RPC in order to update TowerUIController on the network
                 * since, on the server process, there's no UI prefab instantiated
                 */
                networkObject.SendRpc(RPC_CHANGE_U_I_STATUS_ON_NETWORK,
                    true,
                    Receivers.AllBuffered,
                    _factionName);
            }
            else
            {
                Debug.LogError("Faction name cannot be null or empty. ");
            }
        }

        public override void ChangeUIStatusOnNetwork(RpcArgs args)
        {
            //this if statement is just for fortify the assertion that this code must be executed only on non owner process
            //but it's not necessary since the nature of this call imply that
            if (!networkObject.IsOwner)
            {
                if (!ReferenceEquals(args, null))
                {
                    var factionName = args.GetNext<string>();
                    if (!ReferenceEquals(factionName, null) && factionName.Length > 0)
                    {
                        TowerEntityGameUI.ChangeTowerUIStatus(factionName);
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
