using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Linq;
using TC.GPConquest.GpsMap.Helpers;
using TC.Common;
using TC.GPConquest.Player;


namespace TC.GPConquest.Server
{
    public class TowerCaptureController : TowerCaptureNetworkControllerBehavior
    {

        public TowerEntityController TowerEntityController ; 

        public void InitTowerCaptureController(TowerEntityController _towerEntityController)
        {
            UpdateTowerCaptureControllerAttributes(_towerEntityController);

            //Destroy this object when the Tower is destroyed
            _towerEntityController.networkObject.onDestroy += NetworkObject_onDestroy;

            networkObject.SendRpc(RPC_UPDATE_CAPTURE_CONTROLLER_ON_NETWORK
                , Receivers.AllBuffered);
        }

        public override void UpdateCaptureControllerOnNetwork(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                TowerEntityController[] playersInTheScene = FindObjectsOfType<TowerEntityController>();

                var tower = playersInTheScene.ToList().
                    Find(x => x.networkObject.NetworkId.Equals(networkObject.TowerEntityNetId));

                UpdateTowerCaptureControllerAttributes(tower);
            });
        }

        private void UpdateTowerCaptureControllerAttributes(TowerEntityController _towerEntityController)
        {
            networkObject.TowerEntityNetId = _towerEntityController.networkObject.NetworkId;
            TowerEntityController = _towerEntityController;
            transform.SetParent(TowerEntityController.transform);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

    }

}
