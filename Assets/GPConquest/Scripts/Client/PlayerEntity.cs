using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Linq;

namespace TC.GPConquest.Player
{
    /*
     * This class is used to manage the player information in the network.
     * Instances of this class will create the main player ui through the method
     * 'InitializePlayerEntity' .
     * **/
    public class PlayerEntity : PlayerEntityModelBehavior
    {

        public string username;
        public string password;
        public string email;
        public string faction;
        public string selectedUma;
        public GameUIController GameUIController;
        
        private void Awake()
        {
            GameUIController = GetComponent<GameUIController>();
        }

        public bool InitializePlayerEntity(AvatorController _avatorControllerReference, 
            UserInformations _user, 
            uint avatorNetId, 
            Camera _cameraOnDestination)
        {
            //Gets the AvatorController transform
            Transform parentTransform = _avatorControllerReference.gameObject.GetComponent<Transform>();

            //Updates attributes of this networkObject
            UpdatePlayerEntityNetworkAttributes(avatorNetId,
                _user.username,
                _user.password,
                _user.email,
                _user.faction,
                _user.selectedUma);

            //Updates attributes of this GameObject
            UpdatePlayerEntityAttributes(parentTransform);

            //When the avator controller is destroyed, destroy also the player entity
            _avatorControllerReference.networkObject.onDestroy += NetworkObject_onDestroy;

            //Init and create the game UI
            GameUIController.InitializeGameUI(_avatorControllerReference);

            networkObject.SendRpc(RPC_UPDATE_PLAYER_ENTITY,
                Receivers.AllBuffered,
            _user.username,
            _user.password,
            _user.email,
            _user.faction,
            _user.selectedUma);

            return true;
        }

        protected bool UpdatePlayerEntityNetworkAttributes(uint _avatorNetId,
            string _username,
            string _password,
            string _email,
            string _faction,
            string _selectedUma)
        {
            networkObject.avatorOwnerNetId = _avatorNetId;
            username = _username;
            password = _password;
            email = _email;
            faction = _faction;
            selectedUma = _selectedUma;
            return true;
        }

        protected bool UpdatePlayerEntityAttributes(Transform _parentTransform)
        {
            transform.SetParent(_parentTransform);
            return true;
        }

        public override void UpdatePlayerEntity(RpcArgs args)
        {
            //Update attributes on the network
            string _username = args.GetNext<string>();
            string _password = args.GetNext<string>();
            string _email = args.GetNext<string>();
            string _faction = args.GetNext<string>();
            string _selectedUma = args.GetNext<string>();

            UpdatePlayerEntityNetworkAttributes(networkObject.avatorOwnerNetId,
                _username,
                _password,
                _email,
                _faction,
                _selectedUma);

            //Searches the owner avator in the scene and do setups
            MainThreadManager.Run(() =>
            {
                AvatorController[] avatorsInTheScene = FindObjectsOfType<AvatorController>();

                 var avator = avatorsInTheScene.ToList().
                    Find(x => x.networkObject.NetworkId.Equals(networkObject.avatorOwnerNetId));

                //sets this player entity as reference for the Avator founded
                avator.PlayerEntity = this;
                UpdatePlayerEntityAttributes(avator.GetComponent<Transform>());
                GameUIController.InitializeGameUI(avator);

            });
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }
    }
}
