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

    public class TowerEntityController : TowerEntityControllerBehavior, IEqualityComparer<TowerEntityController>
    {

        public string OwnerFaction;
        public Vector2 GPSCoords;//used just for visualization in the inspector
        public TowerCaptureController TowerCaptureController {get;set;}

        //Table <Tuple2<NetworkId,Nickname>,Time spent in the capture of the tower>>
        private Dictionary<GPSConqTuple2<string, uint>,double> PlayerNetIdNameCaptureTimeTable = 
            new Dictionary<GPSConqTuple2<string, uint>,double>();

        //Map that contains the amount of time passed since each faction is trying to capture the tower
        private Dictionary<string, double> FactionsCaptureTime = 
            new Dictionary<string, double>();

        public List<string> PlayerCapturingTowerList = new List<string>();

        private void ActivateBoxColliders(bool _activate)
        {
            var boxColls = GetComponents<BoxCollider>();
            boxColls.ToList<BoxCollider>().ForEach(x => { x.enabled = _activate; });
        }

        public void InitTowerEntityController(Vector2 _GPSCoords)
        {

            if (!networkObject.IsOwner) return;

            //Box colliders are disabled by default in the prefab in order to avoid to add them to the tower capture at spawn time
            ActivateBoxColliders(true);

            GPSCoords = networkObject.towerGPSCoords = _GPSCoords;

            //Calculates the tower position in Unity given the GPS coordinates
            var _towerPos = GPSHelper.LatLongToUnityCoords(_GPSCoords.x, _GPSCoords.y);

            //Updates tower position on network
            networkObject.towerNetPosition = _towerPos;

            //Update tower position on client
            transform.position = _towerPos;

            //Spawn tower capture controller on the network on the same position of the tower
            var towerCaptureController = NetworkManager.Instance.InstantiateTowerCaptureNetworkController(0,transform.position);
            towerCaptureController.networkStarted += TowerCaptureController_networkStarted;

            networkObject.SendRpc(RPC_UPDATE_TOWER_ATTRRIBUTES,
               Receivers.AllBuffered,
               OwnerFaction);
        }

        private void TowerCaptureController_networkStarted(NetworkBehavior behavior)
        {
            TowerCaptureController = behavior.GetComponent<TowerCaptureController>();
            TowerCaptureController.InitTowerCaptureController(this);
        }

        public override void UpdateTowerAttrributes(RpcArgs args)
        {
            OwnerFaction = args.GetNext<string>();
            GPSCoords = networkObject.towerGPSCoords;
            //Update tower position on network
            transform.position = networkObject.towerNetPosition;
            ActivateBoxColliders(true);
        }

        public override bool Equals(object obj)
        {
            var item = obj as TowerEntityController;

            if (item == null)
            {
                return false;
            }

            return this.networkObject.NetworkId == item.networkObject.NetworkId &&
                this.networkObject.towerGPSCoords == item.networkObject.towerGPSCoords;
        }

        public override int GetHashCode()
        {
            int result = 89;
            result = 13 * result + this.networkObject.NetworkId.GetHashCode();
            result = 13 * result + this.networkObject.towerGPSCoords.GetHashCode();
            return result;
        }

        public bool Equals(TowerEntityController x, TowerEntityController y)
        {
            return x.networkObject.NetworkId == y.networkObject.NetworkId &&
                x.networkObject.towerGPSCoords == y.networkObject.towerGPSCoords;
        }

        public int GetHashCode(TowerEntityController obj)
        {
            int result = 89;
            result = 13 * result + obj.networkObject.NetworkId.GetHashCode();
            result = 13 * result + obj.networkObject.towerGPSCoords.GetHashCode();
            return result;
        }

        void OnTriggerEnter(Collider other)
        {
            MangeCollision(other, true);
        }

        void OnTriggerExit(Collider other)
        {
            MangeCollision(other, false);
        }

        private void MangeCollision(Collider other, bool _isCapturing)
        {

            if (networkObject.IsOwner)
            {
                if (other.CompareTag(CommonNames.DESTINATION_TAG))
                {
                    var playerDestinationComponent = other.GetComponent<DestinationController>();
                    var playerNetId = playerDestinationComponent.networkObject.NetworkId;
                    var playerNickname = playerDestinationComponent.AvatorController.PlayerEntity.username;

                    AddOrDeletePlayerToTheCapturing(playerNickname
                        , playerNetId
                        , _isCapturing);

                    networkObject.SendRpc(RPC_SEND_PLAYER_INFO
                        , Receivers.AllBuffered
                        , playerNetId
                        , _isCapturing
                        , playerNickname);
                }
            }


        }

        protected void AddOrDeletePlayerToTheCapturing(string _name
                                                        ,uint _playerNetId
                                                        ,bool _isCapturing)
        {
            GPSConqTuple2<string, uint> playerKey = new GPSConqTuple2<string,uint>(_name,_playerNetId);
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

        public override void SendPlayerInfo(RpcArgs args)
        {
            var playerNetId = args.GetNext<uint>();
            var isCapturing = args.GetNext<bool>();
            var playerNickname = args.GetNext<string>();

            AddOrDeletePlayerToTheCapturing(playerNickname
                                            ,playerNetId
                                            ,isCapturing);
        }

    }
}
