using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;

namespace TC.GPConquest.Player
{
    public class DestinationController : PlayerDestinationControllerBehavior
    {
        #region Mix
        public AssetLoaderController AssetLoaderController { get; private set; }
        private Renderer sphereRend;
        public Camera cam { get; private set; }
        protected tileGen TileGen;
        #endregion

        #region Attributes dedicated to the avator
        public Vector3 destNetAvatorDims { get; protected set; }
        public float destAvatorSpeed { get; protected set; }
        public float destAvatorDestDist { get; protected set; }
        public string selectedUma { get; protected set; }
        public AvatorController AvatorController { get; set; }
        private Vector3 avatorSpawnPosition;
        #endregion

        #region  Attributes dedicated to UnityEditor
        public bool editorMode;
        public GameObject sphere;
        public Color cursorColor;
        public string playerName;
        public Boolean isGiantMode;
        #endregion

        private string[] nameParts = new string[] { "crazy", "cat", "dog", "homie",
            "bobble", "mr", "ms", "mrs", "castle", "flip", "flop" };

        private string[] avators = new string[] { "HumanMale", "HumanMale 1", "HumanMale 2", "HumanMale 3", "HumanMale 4"
            , "HumanFemale" , "HumanFemale 1", "HumanFemale 2", "HumanFemale 3", "HumanFemale 4"};

        private void Awake()
        {
            AssetLoaderController = FindObjectOfType<AssetLoaderController>();
            sphereRend = sphere.GetComponent<Renderer>();
            TileGen = GetComponent<tileGen>();
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();
            InitDestinationController();
        }

        protected void InitDestinationController()
        {

            if (!networkObject.IsOwner)
                return;

            //Set up camera for GPConquest view
            cam = FindObjectOfType<Camera>();
            cam.gameObject.GetComponent<Transform>().SetParent(transform);

            // Get a random index for the first name
            int first = UnityEngine.Random.Range(0, nameParts.Length - 1);
            // Get a random index for the last name
            int last = UnityEngine.Random.Range(0, nameParts.Length - 1);

            //Selected UMA
            int randomAvator = UnityEngine.Random.Range(0, avators.Length - 1);
            selectedUma = avators[randomAvator];

            //Assign the color
            networkObject.destNetColor = UnityEngine.Random.ColorHSV();

            //Giant mode : Makes the avator bigger than normal (about 60 meters) in order to fit the GPConquest's scene concept
            if (isGiantMode)
            {
                //The assigning of the dimensions of the cursor change based on the choosen dimension
                networkObject.destCursorDims = new Vector3(10, 10, 10);

                //Assign the spawn position for the avator
                avatorSpawnPosition = new Vector3(gameObject.transform.position.x - 20,
                gameObject.transform.position.y,
                gameObject.transform.position.z - 20);

                UpdateDestinationAttributes(new Vector3(0, 100, -70),
                    Quaternion.AngleAxis(-50.0f, Vector3.left),
                    50.0f,
                    networkObject.destCursorDims,
                    nameParts[first] + " " + nameParts[last],
                    networkObject.destNetColor);

            }
            else //Normal mode
            {
                networkObject.destCursorDims = new Vector3(0.4f, 0.4f, 0.4f);

                avatorSpawnPosition = new Vector3(gameObject.transform.position.x - 2,
                    gameObject.transform.position.y,
                    gameObject.transform.position.z - 2);

                UpdateDestinationAttributes(new Vector3(0, 4.45f, -5),
                    Quaternion.AngleAxis(-40.0f, Vector3.left),
                    10.0f,
                    networkObject.destCursorDims,
                    nameParts[first] + " " + nameParts[last],
                    networkObject.destNetColor);
            }

            //Spawn the avator on the network
            var avatorController = NetworkManager.Instance.InstantiatePlayerAvatorController(0, avatorSpawnPosition);

            avatorController.networkStarted += AvatorController_networkStarted;

            networkObject.SendRpc(RPC_INIT_NET_DESTINATION,
                Receivers.AllBuffered,
                playerName);
        }

        protected void UpdateDestinationAttributes(string _playerName,
            Color _cursorColor,
            Vector3 _cursorsDimension)
        {
            //Change the name of the gameObject and make it visible in the editor
            playerName = _playerName;
            gameObject.name = playerName;

            //Assign the color/dimension of the cursor and make it visible in the editor
            cursorColor = _cursorColor;
            sphereRend.material.color = _cursorColor;
            sphere.transform.localScale = _cursorsDimension;
        }

        //Sets up values for the destination controller based on isGiantMode value
        protected void UpdateDestinationAttributes(Vector3 _cameraPosition,
            Quaternion _cameraRotation,
            float _cursorSpeed,
            Vector3 _cursorDimensions,
            string _playerName,
            Color _cursorColor)
        {
            //Init base attributes
            UpdateDestinationAttributes(_playerName, _cursorColor, _cursorDimensions);

            //Sets up the camera attributes
            cam.gameObject.GetComponent<Transform>().position = _cameraPosition;
            cam.gameObject.GetComponent<Transform>().rotation = _cameraRotation;

            //Sets up cursor attributes
            networkObject.destCursorSpeed = _cursorSpeed;
            networkObject.destCursorDims = _cursorDimensions;

        }

        //Init the destination controller over the network
        public override void InitNetDestination(RpcArgs args)
        {
            string playerName = args.GetNext<string>();

            MainThreadManager.Run(() =>
            {
                UpdateDestinationAttributes(playerName,
                    networkObject.destNetColor,
                    networkObject.destCursorDims);
            });
        }

        //Once the avator object is created on the network, starts build it's avator character
        private void AvatorController_networkStarted(NetworkBehavior behavior)
        {
            //Sets up name and selected uma character
            var avatorController = behavior.GetComponent<AvatorController>();
            //Passes this destination controller in order to set up correctly the avator
            avatorController.CreateAndSpawnAvator(this);
            /*
             * After that destination and avator controller are generated on the network,
             * we can start the map creation. NOTE: tileGen.cs script will dictate the
             * destination controller position and so that of the avator
             * **/
            StartCoroutine(TileGen.StartTiling());
        }

        // Update is called once per frame
        void Update()
        {
            /* TODO : Refactor this code into a function for moving the destinationa controller.
             * The funciton should be use also by tileGen script for moving this object.
             * **/

            // If we are not the owner of this network object then we should
            // move this cube to the position/rotation dictated by the owner
            if (!networkObject.IsOwner)
            {
                transform.position = networkObject.destNetPosition;
                transform.rotation = networkObject.destNetRotation;
                return;
            }

            //NOTE : The code for moving the character utilizing GEO LOCALIZATION is in tileGen.cs
            if (editorMode)
            {
                transform.position = new Vector3(Input.GetAxis("Horizontal") * networkObject.destCursorSpeed * Time.deltaTime
                                                , transform.position.y
                                                , Input.GetAxis("Vertical") * networkObject.destCursorSpeed * Time.deltaTime) + transform.position;
            }

            networkObject.destNetPosition = transform.position;
            networkObject.destNetRotation = transform.rotation;

        }

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

            //NOTE : The code for moving the character utilizing GEO LOCALIZATION is in tileGen.cs
            if (editorMode)
            {
                _position = new Vector3(Input.GetAxis("Horizontal") * networkObject.destCursorSpeed * Time.deltaTime
                                                , transform.position.y
                                                , Input.GetAxis("Vertical") * networkObject.destCursorSpeed * Time.deltaTime) + transform.position;
            }

            networkObject.destNetPosition = _position;
            networkObject.destNetRotation = transform.rotation;
        }

        void OnGUI()
        {
            if (!networkObject.IsOwner)
                return;

            GUI.Label(new Rect(10, 10, 100, 20), playerName);
            if (GUI.Button(new Rect(10, 30, 100, 20), "Exit"))
                DestroyDestinationController();
        }

        //Function used to destroy this object. NOTE : It will also destroy the avator connected
        private void DestroyDestinationController()
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }
    }
}
