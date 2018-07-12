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
        public GameObject TowerUIInstantiated;
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

        public void InitializeTowerUINetworkController(TowerEntityController _towerEntityController) {

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
            if (!isServerProcess && !networkObject.IsOwner) {
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
            var towerEntity = (TowerEntityController) gameEntityRegister.FindEntity(typeof(TowerEntityController),towerEntityID);

            UpdateTowerUINetController(towerEntity);
        }
    }

}
