using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Linq;
using TC.Common;

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
        [HideInInspector]
        public GameUINetworkController GameUINetworkController;
        [HideInInspector]
        public AvatorController AvatorControllerReference;
        [HideInInspector]
        public GameEntityRegister GameEntityRegister;

        private void Awake()
        {
        }

        public bool InitializePlayerEntity(AvatorController _avatorControllerReference, 
            UserInformations _user,
            Camera _cameraOnDestination)
        {
            //Updates attributes of this GameObject
            UpdatePlayerEntityAttributes(_avatorControllerReference);

            //I'm sorry Demetra
            var destinationOwnerNetId = AvatorControllerReference.
                DestinationControllerReference.
                networkObject.NetworkId;

            //Updates attributes of this networkObject
            UpdatePlayerEntityNetworkAttributes(destinationOwnerNetId,
                _user.username,
                _user.password,
                _user.email,
                _user.faction,
                _user.selectedUma);

            //When the avator controller is destroyed, destroy also the player entity
            AvatorControllerReference.networkObject.onDestroy += NetworkObject_onDestroy;

            //Create the Game UI Controller Entity in the network
            var gameUiNetworkController = NetworkManager.Instance.InstantiateGameUINetworkEntity(0, transform.position);
            gameUiNetworkController.networkStarted += GameUiNetworkController_networkStarted;

            networkObject.SendRpc(RPC_UPDATE_PLAYER_ENTITY,
                Receivers.AllBuffered,
            _user.username,
            _user.password,
            _user.email,
            _user.faction,
            _user.selectedUma);

            return true;
        }

        private void GameUiNetworkController_networkStarted(NetworkBehavior behavior)
        {
            GameUINetworkController = behavior.GetComponent<GameUINetworkController>();
            GameUINetworkController.InitializeNetworkGameUI(AvatorControllerReference);
        }

        protected bool UpdatePlayerEntityNetworkAttributes(uint _destinationOwnerNetId,
            string _username,
            string _password,
            string _email,
            string _faction,
            string _selectedUma)
        {
            networkObject.DestinationOwnerNetId = _destinationOwnerNetId;
            username = _username;
            password = _password;
            email = _email;
            faction = _faction;
            selectedUma = _selectedUma;
            return true;
        }

        protected bool UpdatePlayerEntityAttributes(AvatorController _avatorControllerReference)
        {
            AvatorControllerReference = _avatorControllerReference;

            //Gets the AvatorController transform
            var _parentTransform = AvatorControllerReference.gameObject.GetComponent<Transform>();
            transform.SetParent(_parentTransform);

            //Get the GameEntityRegister 
            GameEntityRegister = _avatorControllerReference.GameEntityRegister;
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

            UpdatePlayerEntityNetworkAttributes(networkObject.DestinationOwnerNetId,
                _username,
                _password,
                _email,
                _faction,
                _selectedUma);

            var gameRegister = FindObjectOfType<GameEntityRegister>();
            var destination = (DestinationController)gameRegister.FindEntity(typeof(DestinationController),
                networkObject.DestinationOwnerNetId);

            var avator = destination.AvatorController;
            //sets this player entity as reference for the Avator founded
            avator.PlayerEntity = this;
            UpdatePlayerEntityAttributes(avator);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }
    }
}
