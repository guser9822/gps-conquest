﻿using System.Collections;
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
        public GameUIController GameUIController;
        
        private void Awake()
        {
            GameUIController = GetComponent<GameUIController>();
        }

        public bool InitializePlayerEntity(AvatorController _avatorControllerReference, 
            UserInformations _user,
            Camera _cameraOnDestination)
        {
            //Gets the AvatorController transform
            Transform parentTransform = _avatorControllerReference.gameObject.GetComponent<Transform>();

            //I'm sorry Demetra
            var destinationOwnerNetId = _avatorControllerReference.
                DestinationControllerReference.
                networkObject.NetworkId;

            //Updates attributes of this networkObject
            UpdatePlayerEntityNetworkAttributes(destinationOwnerNetId,
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
            UpdatePlayerEntityAttributes(avator.GetComponent<Transform>());
            GameUIController.InitializeGameUI(avator);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }
    }
}
