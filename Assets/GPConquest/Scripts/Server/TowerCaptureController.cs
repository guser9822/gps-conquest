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

        public TowerEntityController TowerEntityController { get; set; }

        //Table <Tuple2<NetworkId,Nickname>,Time spent in the capture of the tower>>
        private Dictionary<GPSConqTuple2<string, uint>, double> PlayerNetIdNameCaptureTimeTable =
            new Dictionary<GPSConqTuple2<string, uint>, double>();

        //Map that contains the amount of time passed since each faction is trying to capture the tower
        private Dictionary<string, double> FactionsCaptureTime =
            new Dictionary<string, double>();

        //Just for log through inspector
        public List<string> PlayerCapturingTowerList = new List<string>();

        public void InitTowerCaptureController(TowerEntityController _towerEntityController)
        {
            UpdateTowerCaptureControllerAttributes(_towerEntityController);

            //Destroy this object when the Tower is destroyed
            _towerEntityController.networkObject.onDestroy += NetworkObject_onDestroy;

            networkObject.SendRpc(RPC_UPDATE_CAPTURE_CONTROLLER_ON_NETWORK
                , Receivers.AllBuffered);
        }

        private void UpdateTowerCaptureControllerAttributes(TowerEntityController _towerEntityController)
        {
            networkObject.TowerEntityNetId = _towerEntityController.networkObject.NetworkId;
            TowerEntityController = _towerEntityController;
            _towerEntityController.TowerCaptureController = this;
            transform.SetParent(TowerEntityController.transform);
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

        //This method checks if the player is capturing the tower and updates this object internal structures
        public void CheckForCapture(DestinationController _playerDestinationComponent, bool _isCapturing)
        {
            var playerNetId = _playerDestinationComponent.networkObject.NetworkId;
            var playerNickname = _playerDestinationComponent.AvatorController.PlayerEntity.username;

            AddOrDeletePlayerToTheCapturing(playerNickname
                , playerNetId
                , _isCapturing);

            //Keep updated also the network entities of this object
            networkObject.SendRpc(RPC_UPDATE_CAPTURE_ON_NETWORK
                , Receivers.AllBuffered
                , playerNetId
                , _isCapturing
                , playerNickname);
        }

        protected void AddOrDeletePlayerToTheCapturing(string _name
                                                , uint _playerNetId
                                                , bool _isCapturing)
        {
            GPSConqTuple2<string, uint> playerKey = new GPSConqTuple2<string, uint>(_name, _playerNetId);
            var playerKeyString = playerKey.ToString();
            double val = 0d;
            bool exists;

            if (_isCapturing)
            {
                exists = PlayerNetIdNameCaptureTimeTable.TryGetValue(playerKey, out val);
                if (!exists)
                {
                    PlayerNetIdNameCaptureTimeTable.Add(playerKey, 0d);
                    PlayerCapturingTowerList.Add(playerKeyString);
                }
            }
            else
            {
                exists = PlayerNetIdNameCaptureTimeTable.TryGetValue(playerKey, out val);
                if (exists)
                {
                    PlayerNetIdNameCaptureTimeTable.Remove(playerKey);
                    PlayerCapturingTowerList.Remove(playerKeyString);
                }
            }
        }

        public override void UpdateCaptureOnNetwork(RpcArgs args)
        {
            var playerNetId = args.GetNext<uint>();
            var isCapturing = args.GetNext<bool>();
            var playerNickname = args.GetNext<string>();

            AddOrDeletePlayerToTheCapturing(playerNickname
                                            , playerNetId
                                            , isCapturing);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }
    }

}
