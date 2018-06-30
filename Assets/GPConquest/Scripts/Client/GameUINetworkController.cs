using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Linq;
using TC.Common;
using TC.GPConquest.Player;
using TC.GPConquest;
using TC.GPConquest.MarkLight4GPConquest.Player;
using MarkLight.Views;

namespace TC.GPConquest.Player
{
    public class GameUINetworkController : GameUINetworkEntityBehavior
    {

        public GameObject PrefabPlayerUI;//prefabs of the UI, used only for instantiation
        public GameObject AvatorUIViewPresenter;//instantiated UI
        [HideInInspector]
        public Camera CameraOnDestination;//camera of the DestinationController object
        [HideInInspector]
        public AvatorUI AvatorUI;//UI on the avator/character
        [HideInInspector]
        public PlayerUI PlayerUI;//Fixed 2D UI of the player
        [HideInInspector]
        public AvatorController AvatorControllerReference;
        [HideInInspector]
        public PlayerEntity PlayerEntity;
        protected bool ServerProcess;

        public void Awake()
        {
            //AvatorUI conflict with the UI that reside on the server process.
            //For this particular case  we don't make this UI visible on the server process.
            var server = FindObjectOfType<ServerNetworkController>();
            if ((!ReferenceEquals(server, null) && server.gameObject.tag == "ServerController"))
                ServerProcess = true;
        }

        protected void UpdateGameUINetworkControllerAttributes(AvatorController _avatorControllerReference)
        {

            if (ServerProcess) return; //Do not execute any initialization for the ServerController process

            AvatorUIViewPresenter = Instantiate<GameObject>(PrefabPlayerUI);

            //Gets the AvatorUI
            AvatorUI = AvatorUIViewPresenter.GetComponentInChildren<AvatorUI>();

            //Gets the PlayerUI 
            PlayerUI = AvatorUIViewPresenter.GetComponentInChildren<PlayerUI>();

            AvatorControllerReference = _avatorControllerReference;
            PlayerEntity = AvatorControllerReference.PlayerEntity;

            Transform _avatorTransform = AvatorControllerReference.gameObject.GetComponent<Transform>();

            this.transform.SetParent(_avatorTransform);

            //assign the camera that ui should point to
            CameraOnDestination = AvatorControllerReference.CameraOnDestination;

            //Puts the UI under the hieararchy of the GameUIController object
            AvatorUIViewPresenter.transform.SetParent(_avatorTransform);

            //Sets the correct position for the UI
            AvatorUIViewPresenter.transform.localPosition =
                new Vector3(0.0f,
                0,
                0f);

            //If the code isn't executed by the owner or the process is the Server, deactivate the 2D GUI and the avator ui
            if (!networkObject.IsOwner)
            {
                //Deactivates static parts of the UI
                PlayerUI.gameObject.SetActive(false);

                //Deactivates the EventSystem
                var eventSystem = AvatorUIViewPresenter.GetComponentInChildren<EventSystem>();
                eventSystem.gameObject.SetActive(false);

            }
            else PlayerUI.InitPlayerUI(AvatorControllerReference);

            AvatorUI.InitAvatorUI(CameraOnDestination, PlayerEntity);
        }

        public bool InitializeNetworkGameUI(AvatorController _avatorControllerReference)
        {
            UpdateGameUINetworkControllerAttributes(_avatorControllerReference);
            var _destinationNetId = AvatorControllerReference.networkObject.destNetwId;

            networkObject.SendRpc(RPC_UPDATE_ON_NETWORK_GAME_U_I,Receivers.AllBuffered, _destinationNetId);
            return true;
        }

        public override void UpdateOnNetworkGameUI(RpcArgs args)
        {
            uint _destinationNetId = args.GetNext<uint>();

            var gameRegister = FindObjectOfType<GameEntityRegister>();
            var destination = (DestinationController)gameRegister.FindEntity(typeof(DestinationController),
                _destinationNetId);
            var avator = destination.AvatorController;

            UpdateGameUINetworkControllerAttributes(avator);

        }
    }


}
