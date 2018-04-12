using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;
using TC.GPConquest.Player;
using TC.Common;
using BeardedManStudios.Forge.Networking.Generated;

namespace TC.GPConquest {

    public class GameStatusController : MonoBehaviour
    {
        protected ClientNetworkController ClientNetworkController;
        protected ConnectionInfo ConnectionInfo;
        protected AssetLoaderController AssetLoaderController;
        public tileGen TileGen;
        public PlayerDestinationControllerBehavior PlayerClient { get; private set; }

        private void Awake()
        {
            ClientNetworkController = GetComponent<ClientNetworkController>();
            ConnectionInfo = GetComponent<ConnectionInfo>();
            AssetLoaderController = GetComponent<AssetLoaderController>();

            AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
            ConnectionInfo.SetConnectionInfo();
            ClientNetworkController.StartCustomNetworkController(ConnectionInfo);
        }

        private void Start()
        {
            PlayerClient = NetworkManager.Instance.InstantiatePlayerDestinationController(0);
        }

    }

}
