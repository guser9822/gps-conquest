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

        private void ActivateBoxColliders(bool _activate)
        {
            var boxColls = GetComponents<BoxCollider>();
            boxColls.ToList<BoxCollider>().ForEach(x => { x.enabled = _activate; });
        }

        public void InitTowerEntityController(Vector2 _GPSCoords)
        {
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
            ManageCollision(other, true);
        }

        void OnTriggerExit(Collider other)
        {
            ManageCollision(other, false);
        }

        private void ManageCollision(Collider other, bool _isCapturing)
        {

            if (networkObject.IsOwner)
            {
                if (other.CompareTag(CommonNames.DESTINATION_TAG))
                {
                    var playerDestinationComponent = other.GetComponent<DestinationController>();
                    TowerCaptureController.CheckForCapture(playerDestinationComponent, _isCapturing);
                }

            }
        }

    }
}
