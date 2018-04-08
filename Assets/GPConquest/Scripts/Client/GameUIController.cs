﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MarkLight.Views;
using TC.GPConquest.MarkLight4GPConquest.Player;

namespace TC.GPConquest.Player
{
    public class GameUIController : MonoBehaviour
    {

        public GameObject PrefabPlayerUI;//prefabs of the UI, used only for instantiation
        protected GameObject AvatorUIViewPresenter;//instantiated UI
        [HideInInspector]
        public Camera CameraOnDestination;//camera of the DestinationController object
        [HideInInspector]
        public AvatorUI AvatorUI;//UI on the avator/character
        [HideInInspector]
        public PlayerUI PlayerUI;//Fixed 2D UI of the player

        private void Awake()
        {
            //AvatorUI conflict with the UI that reside on the server process.
            //For this particular case  we don't make this UI visible on the server process.
            var server = FindObjectOfType<ServerNetworkController>();
            if (ReferenceEquals(server,null) || server.gameObject.tag != "ServerController")
                AvatorUIViewPresenter = Instantiate<GameObject>(PrefabPlayerUI);
        }

        //This function initialize the GameUI. isTheOwnerOnNetwork states if the caller
        //of this function it's the client owner of the object on the network
        public bool InitializeGameUI(AvatorController _avatorControllerReference)
        {
            //AvatorUI conflict with the UI that reside on the server process.
            //For this particular case  we don't make this UI visible on the server process.
            var server = FindObjectOfType<ServerNetworkController>();
            if ((!ReferenceEquals(server, null) && server.gameObject.tag == "ServerController"))
                return false;

            Transform _parentTransform = _avatorControllerReference.gameObject.GetComponent<Transform>();
            PlayerEntity _playerEntity = _avatorControllerReference.PlayerEntity;

            //assign the camera that ui should point to
            CameraOnDestination = _avatorControllerReference.CameraOnDestination;

            //Puts the UI under the hieararchy of the GameUIController object
            AvatorUIViewPresenter.transform.SetParent(_parentTransform);

            //Sets the correct position for the UI
            AvatorUIViewPresenter.transform.localPosition =
                new Vector3(0.0f,
                0,
                0f);

            //Gets the AvatorUI and initialize it with the camera that it must follow
            AvatorUI = AvatorUIViewPresenter.GetComponentInChildren<AvatorUI>();
            AvatorUI.InitAvatorUI(CameraOnDestination, _avatorControllerReference.PlayerEntity);

            //Gets the AvatorUI and initialize it with the camera that it must follow
            PlayerUI = AvatorUIViewPresenter.GetComponentInChildren<PlayerUI>();

            //If the code isn't executed by the owner, deactivate the 2D GUI
            if (!_playerEntity.networkObject.IsOwner)
            {
                //Deactivates static parts of the UI
                PlayerUI.gameObject.SetActive(false);
                //Deactivates the EventSystem
                var eventSystem = AvatorUIViewPresenter.GetComponentInChildren<EventSystem>();
                eventSystem.gameObject.SetActive(false);
            }
            else // else initialize the PlayerUI
            {
                PlayerUI.InitPlayerUI(_avatorControllerReference);
            }

            return true;
        }


    }
}


