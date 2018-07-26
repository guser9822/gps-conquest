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
using TC.GPConquest.Common;
using TC.GPConquest.MarkLight4GPConquest.Common;

namespace TC.GPConquest.Server
{

    public class TowerEntityController : TowerEntityControllerBehavior, 
        IEqualityComparer<TowerEntityController>,
        IRegistrable
    {

        public string OwnerFaction;
        public Vector2 GPSCoords;//used just for visualization in the inspector
        public TowerCaptureController TowerCaptureController {get;set;}
        public GameEntityRegister GameEntityRegister { get; private set; }
        public TowerUINetworkController TowerUINetworkController { get; set; }

        private void ActivateBoxColliders(bool _activate)
        {
            //Activate box colliders only for the tower owner, because the latter it's designated 
            //to send update to the towers on the network
            if (networkObject.IsOwner) {
                var boxColls = GetComponents<BoxCollider>();
                boxColls.ToList<BoxCollider>().ForEach(x => { x.enabled = _activate; });
            }

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
            towerCaptureController.networkStarted += TowerComponents_networkStarted;

            //Spawn on the network a NetworkUI contrller
            var towerNetUIController = NetworkManager.Instance.InstantiateTowerUIEntity(0, transform.position);
            towerNetUIController.networkStarted += TowerNetUIController_networkStarted;

            networkObject.SendRpc(RPC_UPDATE_TOWER_ATTRRIBUTES,
               Receivers.AllBuffered,
               OwnerFaction);
        }

        private void TowerNetUIController_networkStarted(NetworkBehavior behavior)
        {
            TowerUINetworkController = behavior.GetComponent<TowerUINetworkController>();
            TowerUINetworkController.InitializeTowerUINetworkController(this);
        }

        private void TowerComponents_networkStarted(NetworkBehavior behavior)
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
            ActivateBoxColliders(true);//This call isn't necessary for the towers on the network but we don't care

            //Find GameEntityRegister
            GameEntityRegister = FindObjectOfType<GameEntityRegister>();

            //Add this tower controller to the register
            GameEntityRegister.AddEntity(this);

            //Register the callback for deleting the Destination controller from the register when it will disconnect
            networkObject.onDestroy += NetworkObject_onDestroyRemoveFromRegister;
        }

        private void NetworkObject_onDestroyRemoveFromRegister(NetWorker sender)
        {
            GameEntityRegister.RemoveEntity(this);
        }

        public override bool Equals(object obj)
        {
            var item = obj as TowerEntityController;

            if (ReferenceEquals(item,null))
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

        public uint GetUniqueKey()
        {
            return networkObject.NetworkId;
        }

        private void OnMouseDown()
        {
            if (!networkObject.IsOwner /* and not serverProcess */)
            {
                TowerUINetworkController.TowerEntityGameUI.ToggleWindow();
            }
         }

        /// <summary>
        /// This function is called from outside (e.g. TowerCaptureController) in order to 
        /// change the state of the tower entity
        /// </summary>
        /// <param name="_winningFaction"></param>
        /// <param name="_actionName"></param>
        public void ChangeTowerEntityStatus(string _winningFaction, string _actionName)
        {
            if (networkObject.IsOwner)
            {
                if (!ReferenceEquals(_winningFaction, null) && 
                    !ReferenceEquals(_actionName, null))
                {
                    ApplyChangesToTowerEntity(_winningFaction);

                    //Update tower entities on network
                    networkObject.SendRpc(RPC_CHANGE_TOWER_STATUS_ON_NETWORK,
                        true,
                        Receivers.AllBuffered,
                        _winningFaction);

                    //Update UI
                    TowerUINetworkController.CallChangeUIStatus(_winningFaction,_actionName);
                }
                else
                {
                    Debug.LogError("Faction name cannot be null");
                }
            }
        }

        /// <summary>
        /// RPC used to alter the tower status on the network
        /// </summary>
        /// <param name="args"></param>
        public override void ChangeTowerStatusOnNetwork(RpcArgs args)
        {
            if (!ReferenceEquals(args, null))
            {
                var factionName = args.GetNext<string>();
                if (!ReferenceEquals(factionName, null) && factionName.Length > 0)
                {
                    ApplyChangesToTowerEntity(factionName);
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

        /// <summary>
        /// This is an internal function used for changing attributes of the tower
        /// after the capture and it is invoked by both owner e non owner of the entity
        /// </summary>
        protected void ApplyChangesToTowerEntity(string factionName)
        {
            if (!ReferenceEquals(factionName, null) && factionName.Length > 0)
            {
                OwnerFaction = factionName;
            }
            else
            {
                Debug.LogError("Faction name is null or empty string. ");
            }
        }

        public void RequestChangeTowerUI(string _actionName)
        {
            if (!ReferenceEquals(_actionName, null) && _actionName.Length > 0)
            {
            }
            else Debug.LogError("Action name is null or empty");
        }
    }
}
