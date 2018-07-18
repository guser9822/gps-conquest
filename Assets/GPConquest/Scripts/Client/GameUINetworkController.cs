using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using TC.Common;
using TC.GPConquest.MarkLight4GPConquest.Player;
using MarkLight.Views;

namespace TC.GPConquest.Player
{
    /// <summary>
    /// This class is used in order to manage the Game UI for the player in a networked manner
    /// </summary>
    public class GameUINetworkController : GameUINetworkEntityBehavior
    {

        public GameObject PrefabPlayerUI;//prefabs of the UI, used only for instantiation
        public GameObject PrefabAvatorUI;//prefabs of the UI, used only for instantiation
        //[HideInInspector]
        public GameObject InstantiatedPlayerUI;//instantiated UI
        //[HideInInspector]
        public GameObject InstantiatedAvatorUI;//instantiated UI
        //[HideInInspector]
        public Camera CameraOnDestination;//camera of the DestinationController object
        //[HideInInspector]
        public AvatorUI AvatorUI;//UI on the avator/character
        //[HideInInspector]
        public PlayerUI PlayerUI;//Fixed 2D UI of the player
        //[HideInInspector]
        public AvatorController AvatorControllerReference;
        //[HideInInspector]
        public PlayerEntity PlayerEntity;
        protected bool ServerProcess;

        public void Awake()
        {
            //AvatorUI conflict with the UI that reside on the server process.
            //For this particular case  we don't make this UI visible on the server process.
            var server = FindObjectOfType<ServerNetworkController>();
            if ((!ReferenceEquals(server, null) && server.gameObject.tag == "ServerController"))
                ServerProcess = true;
            /*
             *  Putting the Instantiate of a normal object (not networked like the Forge ones) seems to be 
             *  the only way in order to avoid strange initialization and instantiation errors on the owner 
             *  and non owner process
             * **/
            if (!ServerProcess) {
                InstantiatedPlayerUI = Instantiate<GameObject>(PrefabPlayerUI);
                InstantiatedAvatorUI = Instantiate<GameObject>(PrefabAvatorUI);
            }

        }

        protected void UpdateGameUINetworkControllerAttributes(AvatorController _avatorControllerReference)
        {

            _avatorControllerReference.networkObject.onDestroy += NetworkObject_onDestroy;

            if (ServerProcess) return; //Do not execute any initialization for the ServerController process

            //Gets the AvatorUI
            AvatorUI = InstantiatedAvatorUI.GetComponentInChildren<AvatorUI>();

            //Gets the PlayerUI 
            PlayerUI = InstantiatedPlayerUI.GetComponentInChildren<PlayerUI>();

            AvatorControllerReference = _avatorControllerReference;
            PlayerEntity = AvatorControllerReference.PlayerEntity;

            Transform _avatorTransform = AvatorControllerReference.gameObject.GetComponent<Transform>();

            this.transform.SetParent(_avatorTransform);

            //assign the camera that ui should point to
            CameraOnDestination = AvatorControllerReference.CameraOnDestination;

            //Puts the UI under the hieararchy of the GameUIController object
            InstantiatedPlayerUI.transform.SetParent(_avatorTransform);

            //Puts AvatorUI under avator hierarchy
            InstantiatedAvatorUI.transform.SetParent(_avatorTransform);
            //Set the AvatorUI(nickname) on the head of the avator character
            InstantiatedAvatorUI.transform.localPosition = new Vector3(0, 2.3f, 0);

            //Sets the correct position for the UI
            InstantiatedPlayerUI.transform.localPosition =
                new Vector3(0.0f,
                0,
                0f);

            //If the code isn't executed by the owner or the process is the Server, deactivate the 2D GUI and the avator ui
            if (!networkObject.IsOwner)
            {
                //Deactivates static parts of the UI
                PlayerUI.gameObject.SetActive(false);

                //Deactivates the EventSystem
                var eventSystem = InstantiatedPlayerUI.GetComponentInChildren<EventSystem>();
                eventSystem.gameObject.SetActive(false);

            }
            else PlayerUI.InitPlayerUI(AvatorControllerReference);

            if(!ReferenceEquals(AvatorUI,null))
                AvatorUI.InitAvatorUI(CameraOnDestination, PlayerEntity);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
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
            //set this ui to the PLayerEntity in the non owner process 
            avator.PlayerEntity.GameUINetworkController = this;
            UpdateGameUINetworkControllerAttributes(avator);

        }
    }


}
