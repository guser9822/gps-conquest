using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using TC.Common;
using TC.GPConquest.Common;

namespace TC.GPConquest.Player
{
    public class DestinationController : PlayerDestinationControllerBehavior,
        IEqualityComparer<DestinationController>,
        IRegistrable
    {
        #region Mix
        private Renderer sphereRend;
        [HideInInspector]
        public Camera DestinationCamera;
        protected tileGen TileGen;
        public GameEntityRegister GameEntityRegister { get; private set; }
        #endregion

        #region Attributes dedicated to the avator
        public Vector3 destNetAvatorDims { get; protected set; }
        public float destAvatorSpeed { get; protected set; }
        public float destAvatorDestDist { get; protected set; }
        public string SelectedUma { get; protected set; }
        public UserInformations CurrentUserInformations { get; private set; }
        public AvatorController AvatorController { get; set; }
        #endregion

        #region  Attributes dedicated to UnityEditor
        public bool editorMode;
        public GameObject sphere;
        public Color cursorColor;
        public string PlayerName;
        public Boolean isGiantMode;
        #endregion

        private void Awake()
        {
            sphereRend = sphere.GetComponent<Renderer>();
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();
            InitDestinationController();
        }

        protected void InitDestinationController()
        {
            //This is necessary since we are overriding NetworkStart and the code then will be executed on proprietary and non process
            if (!networkObject.IsOwner)
                return;

            //Set up camera for GPConquest view
            DestinationCamera = FindObjectOfType<Camera>();
            DestinationCamera.gameObject.GetComponent<Transform>().SetParent(transform);

            //Gets a reference to the user account informations
            UsersContainer userInformations = FindObjectOfType<UsersContainer>();
            CurrentUserInformations = userInformations.UserInfos;

            //Selected UMA
            SelectedUma = userInformations.UserInfos.selectedUma;

            //Assign the color
            networkObject.destNetColor = UnityEngine.Random.ColorHSV();

            //Giant mode : Makes the avator bigger than normal (about 60 meters) in order to fit the GPConquest's scene concept
            if (isGiantMode)
            {
                //The assigning of the dimensions of the cursor change based on the choosen dimension
                networkObject.destCursorDims = new Vector3(10, 10, 10);

                UpdateDestinationAttributes(new Vector3(0, 100, -70),
                    Quaternion.AngleAxis(-50.0f, Vector3.left),
                    50.0f,
                    networkObject.destCursorDims,
                    CurrentUserInformations,
                    networkObject.destNetColor);

            }
            else //Normal mode
            {
                networkObject.destCursorDims = new Vector3(0.4f, 0.4f, 0.4f);

                UpdateDestinationAttributes(new Vector3(0, 4.45f, -5),
                    Quaternion.AngleAxis(-40.0f, Vector3.left),
                    10.0f,
                    networkObject.destCursorDims,
                    CurrentUserInformations,
                    networkObject.destNetColor);
            }

            //Create the map
            TileGen = gameObject.GetComponent<tileGen>();
            StartCoroutine(TileGen.StartTiling());

            //Spawn the avator on the network
            var avatorController = NetworkManager.Instance.InstantiatePlayerAvatorController(0, transform.position);
            avatorController.networkStarted += AvatorController_networkStarted;

            networkObject.SendRpc(RPC_INIT_NET_DESTINATION,
                Receivers.AllBuffered,
                PlayerName,
                SelectedUma);
        }

        protected void UpdateDestinationAttributes(string _playerName,
            string _selectedUma,
            Color _cursorColor,
            Vector3 _cursorsDimension)
        {
            //Change the name of the gameObject and make it visible in the editor
            PlayerName = _playerName;
            SelectedUma = _selectedUma;
            gameObject.name = PlayerName;

            //Assign the color/dimension of the cursor and make it visible in the editor
            cursorColor = _cursorColor;
            sphereRend.material.color = _cursorColor;
            sphere.transform.localScale = _cursorsDimension;

            //Find GameEntityRegister
            GameEntityRegister = FindObjectOfType<GameEntityRegister>();

            //Add this Destination controller to the register
            GameEntityRegister.AddEntity(this);

            //Register the callback for deleting the Destination controller from the register when it will disconnect
            networkObject.onDestroy += NetworkObject_onDestroyRemoveFromRegister;
        }

        //Sets up values for the destination controller based on isGiantMode value
        protected void UpdateDestinationAttributes(Vector3 _cameraPosition,
            Quaternion _cameraRotation,
            float _cursorSpeed,
            Vector3 _cursorDimensions,
            UserInformations _currentUserInformations,
            Color _cursorColor)
        {
            //Init base attributes
            UpdateDestinationAttributes(_currentUserInformations.username,
                _currentUserInformations.selectedUma,
                _cursorColor, 
                _cursorDimensions);

            //Sets up the camera attributes
            DestinationCamera.gameObject.GetComponent<Transform>().position = _cameraPosition;
            DestinationCamera.gameObject.GetComponent<Transform>().rotation = _cameraRotation;

            //Sets up cursor attributes
            networkObject.destCursorSpeed = _cursorSpeed;
            networkObject.destCursorDims = _cursorDimensions;
        }

        //Init the destination controller over the network
        public override void InitNetDestination(RpcArgs args)
        {
            string _playerName = args.GetNext<string>();
            string _selectedUma = args.GetNext<string>();
            //Find the main camera. For my entity on the network, the camera will be the same
            //of the owner of the client in order to let the UI (e.g. nickname labels) to point towards
            //the players owning the clients. 
            DestinationCamera = FindObjectOfType<Camera>();

            UpdateDestinationAttributes(_playerName,
                _selectedUma,
                networkObject.destNetColor,
                networkObject.destCursorDims);
        }

        //Once the avator object is created on the network, starts build it's avator character
        private void AvatorController_networkStarted(NetworkBehavior behavior)
        {
            //Sets up name and selected uma character
            var avatorController = behavior.GetComponent<AvatorController>();
            //Passes this destination controller in order to set up correctly the avator
            avatorController.InitAvatorController(this);
        }

        // Update is called once per frame
        void Update()
        {
            //If we are not the owner of this network object then we should
            // move this cube to the position/ rotation dictated by the owner
            if (!networkObject.IsOwner)
            {
                transform.position = networkObject.destNetPosition;
                transform.rotation = networkObject.destNetRotation;
                return;
            }

            /*
            * If we are in editorMode it means that we are testing the game
            * withouth GPS support, so we recalculate the vector _position
            * with acquired commands-
            * **/
            if (editorMode)
            {
                transform.position = new Vector3(Input.GetAxis("Horizontal") * networkObject.destCursorSpeed * Time.deltaTime
                                                , transform.position.y
                                                , Input.GetAxis("Vertical") * networkObject.destCursorSpeed * Time.deltaTime) +
                                                transform.position;
            }

            networkObject.destNetPosition = transform.position;
            networkObject.destNetRotation = transform.rotation;
        }

        /*
         * This function, now is used mainly for initializing the player position
         * through the tileGens script.
         * For moving the character we will use the Update untill the movement with
         * the GPS are off. So Update and this function needs to be refactored into ONE.
         * **/
        public void MovePlayerDestination(Vector3 _position)
        {

            // If we are not the owner of this network object then we should
            // move this cube to the position/rotation dictated by the owner
            if (!networkObject.IsOwner)
            {
                transform.position = networkObject.destNetPosition;
                transform.rotation = networkObject.destNetRotation;
                return;
            }

            transform.position = _position;
            networkObject.destNetPosition = transform.position;
            networkObject.destNetRotation = transform.rotation;
        }

        //Function used to destroy this object. 
        //NOTE 1: It will also destroy the avator connected
        //NOTE 2: It will not be executed on the non owner process
        public void DestroyDestinationController()
        {
            //This will remove the destination controller only from the owner process entities register 
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

        private void NetworkObject_onDestroyRemoveFromRegister(NetWorker sender)
        {
            GameEntityRegister.RemoveEntity(this);
        }

        public bool Equals(DestinationController x, DestinationController y)
        {
            return x.networkObject.NetworkId == y.networkObject.NetworkId &&
               x.PlayerName == y.PlayerName;
        }

        public int GetHashCode(DestinationController obj)
        {
            int result = 89;
            result = 13 * result + obj.networkObject.NetworkId.GetHashCode();
            result = 13 * result + obj.PlayerName.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            var item = obj as DestinationController;

            if (ReferenceEquals(item,null))
            {
                return false;
            }

            return this.networkObject.NetworkId == item.networkObject.NetworkId &&
                this.PlayerName == item.PlayerName;
        }

        public override int GetHashCode()
        {
            int result = 89;
            result = 13 * result + this.networkObject.NetworkId.GetHashCode();
            result = 13 * result + this.PlayerName.GetHashCode();
            return result;
        }

        public uint GetUniqueKey()
        {
            return networkObject.NetworkId;
        }
    }



}
