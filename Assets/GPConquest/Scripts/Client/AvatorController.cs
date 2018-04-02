using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UMA;
using TC.UM4GPConquest.Utility;
using System;
using System.Linq;

namespace TC.GPConquest.Player
{

    public class AvatorController : PlayerAvatorControllerBehavior
    {
        protected string SelectedUma;
        protected AssetLoaderController AssetLoaderController;
        protected Animator Animator;
        protected CharacterController CharacterController;
        private Transform DestinationTransform;
        public string SpeedParamenter = "Forward";
        private float SpeedDampTime = .00f;
        protected UserInformations CurrentUserInfo;
        protected PlayerEntity PlayerEntity;

        public Camera CameraOnDestination { get; set; }

        private void Awake()
        {
            AssetLoaderController = FindObjectOfType<AssetLoaderController>();
            CharacterController = GetComponent<CharacterController>();
            CharacterController.center = new Vector3(0, 1.0f, 0);
        }

        void Update()
        {
            //We don't check the owner of the object, every client has it's own Avators , so these latters are not synched
            MoveAvator();
        }

        public void CreateAndSpawnAvator(DestinationController _destinationController)
        {
            if (!networkObject.IsOwner)
                return;

            //Update shared attributes according the choosen size of the avator
            if (_destinationController.isGiantMode)
            {
                UpdateAvatorNetAttrbs(new Vector3(25, 25, 25),
                        _destinationController.networkObject.NetworkId,
                        0.5f,
                        0.6f);
            }
            else
            {
                UpdateAvatorNetAttrbs(new Vector3(1, 1, 1),
                    _destinationController.networkObject.NetworkId,
                        0.7f,
                        0.6f);
            }

            //Setups the AvatorController attributes that cames from the DestinationController 
            UpdateAvatorAttributes(_destinationController.gameObject.name + " avator - " + networkObject.NetworkId, 
                _destinationController.selectedUma, 
                _destinationController.CurrentUserInformations,
                _destinationController.gameObject.GetComponent<Transform>(),
                _destinationController.DestinationCamera);

            //When the destination controller is destroyed, destroy also the realated Avator.
            _destinationController.networkObject.onDestroy += NetworkObject_onDestroy;

            //Setup and spawn the UMA character
            CreateAndSpawnUMA();

            //Create the Player Entity in the network
            var playerEntityModelBehavior = NetworkManager.Instance.InstantiatePlayerEntityModel(0,transform.position);
            playerEntityModelBehavior.networkStarted += PlayerEntityModelBehavior_networkStarted;

            networkObject.SendRpc(RPC_UPDATE_AVATOR_ON_NETWORK,
                Receivers.AllBuffered,
                SelectedUma,
                gameObject.name);
        }

        private void PlayerEntityModelBehavior_networkStarted(NetworkBehavior behavior)
        {
            PlayerEntity = behavior.GetComponent<PlayerEntity>();
            PlayerEntity.InitializePlayerEntity(transform,
                CurrentUserInfo,
                networkObject.NetworkId,
                CameraOnDestination);
        }

        //Sets up values for the avator controller based on isGiantMode value
        protected void UpdateAvatorNetAttrbs(Vector3 _avatorDimensions,
            uint _destinationNetworkId,
            float _avatorSpeed,
            float _avatorDestDist)
        {
            //Updates network attributes
            networkObject.avatorNetDims = _avatorDimensions;
            networkObject.destNetwId = _destinationNetworkId;
            networkObject.avatorNetSpeed = _avatorSpeed;
            networkObject.avatorNetDestDistance = _avatorDestDist;
        }

        protected void UpdateAvatorAttributes(string _playerName, 
            string _selectedAvator,
            UserInformations _userInformations,
            Transform _destinationTransform,
            Camera _cameraOnDestination)
        {
            gameObject.name = _playerName;
            SelectedUma = _selectedAvator;
            CharacterController.center = new Vector3(0, 1.0f, 0);
            CurrentUserInfo = _userInformations;
            DestinationTransform = _destinationTransform;
            CameraOnDestination = _cameraOnDestination;
        }


        // Updates the UMA avator attached to this game object
        private void CreateAndSpawnUMA()
        {
            UMADynamicAvatar thisUmaDynamicAvator = gameObject.GetComponent<UMADynamicAvatar>();
            //Create a UMA avator and bind it to the DynamicAvator of this object
            UMAGenericHelper.createUMAAvator(AssetLoaderController, 
                SelectedUma, 
                thisUmaDynamicAvator, 
                AvatorController_OnCharacterCreated);
        }

        //Sets up settings after the creation of the UMA character
        private void AvatorController_OnCharacterCreated(UMAData data)
        {
            //Deactivate shadows production and reception
            data.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            data.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().receiveShadows = false;

            //Once the Avator is created grab the animator
            Animator = GetComponent<Animator>();

            //Assign choosen dimensions for this avator. NOTE: It doesn't works correctly, sometimes the avator doesn't show up
            gameObject.transform.localScale = networkObject.avatorNetDims;

        }

        //This function updates the avators of the other players that you can see by your client
        public override void UpdateAvatorOnNetwork(RpcArgs args)
        {

            string playerName = args.GetNext<string>();
            string selectedUma = args.GetNext<string>();

            MainThreadManager.Run(() =>
            {

                ////I'm the avator on the network, I shall find my destination in the scene using it's network id
                DestinationController[] playersInTheScene = FindObjectsOfType<DestinationController>();

                var destination = playersInTheScene.ToList().
                    Find(x => x.networkObject.NetworkId.Equals(networkObject.destNetwId));

                UpdateAvatorAttributes(selectedUma, 
                    playerName,
                    destination.CurrentUserInformations, 
                    destination.GetComponent<Transform>(),
                    destination.DestinationCamera);

                CreateAndSpawnUMA();
            });

        }

        private void MoveAvator()
        {
            if (Animator && DestinationTransform)
            {
                if (Vector3.Distance(DestinationTransform.position, Animator.rootPosition) > networkObject.avatorNetDestDistance)
                {
                    Animator.SetFloat(SpeedParamenter, networkObject.avatorNetSpeed, SpeedDampTime, Time.deltaTime);

                    Vector3 curentDir = Animator.rootRotation * Vector3.forward;
                    Vector3 wantedDir = (DestinationTransform.position - Animator.rootPosition).normalized;

                    if (Vector3.Dot(curentDir, wantedDir) > 0)
                    {
                        transform.LookAt(DestinationTransform);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                    else
                    {
                        transform.LookAt(DestinationTransform);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }

                }
                else
                {
                    Animator.SetFloat(SpeedParamenter, 0, SpeedDampTime, Time.deltaTime);
                }
            }
        }

        void OnAnimatorMove()
        {
            if (CharacterController)
            {
                CharacterController.Move(Animator.deltaPosition);
                transform.rotation = Animator.rootRotation;
            }
        }

        //When the destination controller is destroyed, destroys the avator
        private void NetworkObject_onDestroy(NetWorker sender)
        {
            Animator = null;
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

    }

}