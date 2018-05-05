using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using TC.GPConquest.GpsMap.Helpers;

namespace TC.GPConquest.Server {

    public class TowerEntityController : TowerEntityControllerBehavior,IEqualityComparer<TowerEntityController>
    {

        public string OwnerFaction;
        public Vector2 GPSCoords;//used just for visualization in the inspector

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
    }
}
