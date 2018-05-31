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

namespace TC.GPConquest.Server {

    public class TowerEntityController : TowerEntityControllerBehavior,IEqualityComparer<TowerEntityController>
    {

        public string OwnerFaction;
        public Vector2 GPSCoords;//used just for visualization in the inspector
        public HashSet<uint> PlayersCapturingTheTower = new HashSet<uint>();

        public void InitTowerEntityController(Vector2 _GPSCoords)
        {
            if (!networkObject.IsOwner) return;

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
            if (other.CompareTag(CommonNames.AVATOR_TAG))
            {
                Debug.Log("Collision enter : " + other.gameObject.name);
                var playerNetObj = other.GetComponent<PlayerAvatorControllerBehavior>().networkObject;
                networkObject.SendRpc(RPC_SEND_PLAYER_NET_ID,Receivers.AllBuffered,playerNetObj.NetworkId,true);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(CommonNames.AVATOR_TAG))
            {
                Debug.Log("Collision exit : " + other.gameObject.name);
                var playerNetObj = other.GetComponent<PlayerAvatorControllerBehavior>().networkObject;
                networkObject.SendRpc(RPC_SEND_PLAYER_NET_ID, Receivers.AllBuffered, playerNetObj.NetworkId, false);
            }
        }

        public override void SendPlayerNetId(RpcArgs args)
        {
            var playerNetId = args.GetNext<uint>();
            bool isCapturing = args.GetNext<bool>();

            if (isCapturing)
            {
                if (!PlayersCapturingTheTower.Contains(playerNetId))
                    PlayersCapturingTheTower.Add(playerNetId);
            }
            else PlayersCapturingTheTower.Remove(playerNetId);
            if (PlayersCapturingTheTower.Count > 0)
            {
                foreach (var x in PlayersCapturingTheTower)
                    Debug.Log("Player Id : "+x);
            }
        }
    }
}
