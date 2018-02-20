using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UMA;
using System;

namespace TC.GPConquest.Player
{

    public class AvatorController : PlayerAvatorControllerBehavior
    {
        public string selectedUma;
        private AssetLoaderController assetLoaderController;
        public Animator animator;
        public CharacterController characterController;
        private Transform destinationTransform;
        public string speedParamenter = "Forward";
        private float SpeedDampTime = .00f;

        private void Awake()
        {
            /*NOTE For the Server process : 
            * The server process doesn't have AssetLoaderController object in it's scene
            * because we don't need any player in that process, so, basically,
            * assetLoaderController will always null
            * TO DO : Find a better solution to isolate the server from the rest of the client
            * **/
            assetLoaderController = FindObjectOfType<AssetLoaderController>();
            characterController = GetComponent<CharacterController>();
            characterController.center = new Vector3(0, 1.0f, 0);
        }

        void Update()
        {
            //We don't check the owner of the object, every client has it's own Avators , so these latters are not synched
            MoveAvator();
        }

        public void CreateAndSpawnAvator(DestinationController _destinationController)
        {
            if (!networkObject.IsOwner) return;

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

            UpdateAvatorAttributes(_destinationController.gameObject.name + " avator - " + networkObject.NetworkId, _destinationController.selectedUma);

            //Bind this Avator to the calling destination controller
            _destinationController.AvatorController = this;
            _destinationController.networkObject.onDestroy += NetworkObject_onDestroy;
            destinationTransform = _destinationController.gameObject.GetComponent<Transform>();

            //Setup and spawn the UMA character
            UpdateUMA_Avator();

            networkObject.SendRpc(RPC_UPDATE_AVATOR_ON_NETWORK,
                Receivers.AllBuffered,
                selectedUma,
                gameObject.name);
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

        protected void UpdateAvatorAttributes(string _playerName, string _selectedAvator)
        {
            gameObject.name = _playerName;
            selectedUma = _selectedAvator;
            characterController.center = new Vector3(0, 1.0f, 0);
        }

        //Updates the UMA avator attached to this game object
        private void UpdateUMA_Avator()
        {
            /**
             * Here we check assetLoadController against null because the server process,
             * which doesn't need any player, don't have anyone of this object.
             * **/
            if (assetLoaderController != null)
            {
                //Set/spawn a UMA Avator
                GameObject thisUma = gameObject.GetComponent<UMADynamicAvatar>().gameObject;
                UMADynamicAvatar thisUmaDynamicAvator = gameObject.GetComponent<UMADynamicAvatar>();

                thisUmaDynamicAvator.context = assetLoaderController.context;
                thisUmaDynamicAvator.umaGenerator = assetLoaderController.generator;
                thisUmaDynamicAvator.loadOnStart = false;
                thisUmaDynamicAvator.Initialize();
                thisUmaDynamicAvator.animationController = assetLoaderController.thirdPersonController;
                UMATextRecipe recipe = assetLoaderController.umaCharactersTemplates[selectedUma];
                thisUmaDynamicAvator.Load(recipe);

                thisUma.GetComponent<UMAData>().OnCharacterCreated += AvatorController_OnCharacterCreated;
            }
        }

        //Sets up settings after the creation of the UMA character
        private void AvatorController_OnCharacterCreated(UMAData data)
        {
            //Deactivate shadows production and reception
            data.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            data.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().receiveShadows = false;

            //Once the Avator is created grab the animator
            animator = GetComponent<Animator>();

            //Assign choosen dimensions for this avator. NOTE: It doesn't works correctly, sometimes the avator doesn't show up
            gameObject.transform.localScale = networkObject.avatorNetDims;

        }


        public override void UpdateAvatorOnNetwork(RpcArgs args)
        {

            string playerName = args.GetNext<string>();
            string selectedUma = args.GetNext<string>();

            MainThreadManager.Run(() =>
            {
                UpdateAvatorAttributes(selectedUma, playerName);
                UpdateUMA_Avator();

                //I'm the avator on the network, I shall find my destination in the scene using it's network id
                DestinationController[] playersInTheScene = FindObjectsOfType<DestinationController>();
                foreach (DestinationController dst in playersInTheScene)
                {
                    if (dst.networkObject.NetworkId.Equals(networkObject.destNetwId))
                        destinationTransform = dst.GetComponent<Transform>();
                }
            });

        }

        private void MoveAvator()
        {
            if (animator && destinationTransform)
            {
                if (Vector3.Distance(destinationTransform.position, animator.rootPosition) > networkObject.avatorNetDestDistance)
                {
                    animator.SetFloat(speedParamenter, networkObject.avatorNetSpeed, SpeedDampTime, Time.deltaTime);

                    Vector3 curentDir = animator.rootRotation * Vector3.forward;
                    Vector3 wantedDir = (destinationTransform.position - animator.rootPosition).normalized;

                    if (Vector3.Dot(curentDir, wantedDir) > 0)
                    {
                        transform.LookAt(destinationTransform);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                    else
                    {
                        transform.LookAt(destinationTransform);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }

                }
                else
                {
                    animator.SetFloat(speedParamenter, 0, SpeedDampTime, Time.deltaTime);
                }
            }
        }

        void OnAnimatorMove()
        {
            if (characterController)
            {
                characterController.Move(animator.deltaPosition);
                transform.rotation = animator.rootRotation;
            }
        }

        //When the destination controller is destroyed, destroys the avator
        private void NetworkObject_onDestroy(NetWorker sender)
        {
            animator = null;
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

    }

}