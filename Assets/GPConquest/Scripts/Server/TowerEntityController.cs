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
        private Dictionary<uint, string> DictPlayerNameCapturing = new Dictionary<uint, string>();
        public List<string> PlayerCapturingTowerList = new List<string>();

        private void ActivateBoxColliders(bool _activate)
        {
            var boxColls = GetComponents<BoxCollider>();
            boxColls.ToList<BoxCollider>().ForEach(x => { x.enabled = _activate; });
        }

        public void InitTowerEntityController(Vector2 _GPSCoords)
        {

            if (!networkObject.IsOwner) return;

            ActivateBoxColliders(true);

            GPSCoords = networkObject.towerGPSCoords = _GPSCoords;

            //Calculates the tower position in Unity given the GPS coordinates
            var _towerPos = GPSHelper.LatLongToUnityCoords(_GPSCoords.x, _GPSCoords.y);

            //Updates tower position on network
            networkObject.towerNetPosition = _towerPos;

            //Update tower position on client
            transform.position = _towerPos;

            networkObject.SendRpc(RPC_UPDATE_TOWER_ATTRRIBUTES,
               Receivers.AllBuffered,
               OwnerFaction);
        }

        public override void UpdateTowerAttrributes(RpcArgs args)
        {
            OwnerFaction = args.GetNext<string>();

            GPSCoords = networkObject.towerGPSCoords;

            //Update tower position on network
            transform.position = networkObject.towerNetPosition;

            ActivateBoxColliders(true);

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

            if (other.CompareTag(CommonNames.AVATOR_TAG))
            {
                var playerAvatorComponent = other.GetComponent<AvatorController>();
                var playerNetId = playerAvatorComponent.networkObject.NetworkId;
                var playerNickname = playerAvatorComponent.PlayerEntity.username;

                AddOrDeletePlayerToTheCapturing(playerNickname
                    , playerNetId
                    , _isCapturing);

                networkObject.SendRpc(RPC_SEND_PLAYER_NET_ID
                    , Receivers.AllBuffered
                    , playerNetId
                    , _isCapturing
                    , playerNickname);
            }

        }

        protected void AddOrDeletePlayerToTheCapturing(string _name
                                                        ,uint _playerNetId
                                                        ,bool _isCapturing)
        {
            string playerNicknameNetId;
            string nickName;
            bool exists;

            if (_isCapturing)
            {
                exists = DictPlayerNameCapturing.TryGetValue(_playerNetId, out nickName);
                if (!exists)
                {
                    DictPlayerNameCapturing.Add(_playerNetId, _name);
                    playerNicknameNetId = _name.ToString() + " - " + _playerNetId.ToString();
                    PlayerCapturingTowerList.Add(playerNicknameNetId);
                }
            }
            else
            {
                exists = DictPlayerNameCapturing.TryGetValue(_playerNetId, out nickName);
                if (exists)
                {
                    DictPlayerNameCapturing.Remove(_playerNetId);
                    playerNicknameNetId = nickName.ToString() + " - " + _playerNetId.ToString();
                    PlayerCapturingTowerList.Remove(playerNicknameNetId);
                }
            }
        }

        public override void SendPlayerNetId(RpcArgs args)
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
