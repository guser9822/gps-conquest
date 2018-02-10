using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;
using TC.GPConquest.Player;
using TC.Common;

namespace TC.GPConquest {

    public class GameStatusController : MonoBehaviour
    {
        protected ClientNetworkController ClientNetworkController;
        protected ConnectionInfo ConnectionInfo;
        protected AssetLoaderController AssetLoaderController;

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
            NetworkManager.Instance.InstantiatePlayerDestinationController(0);
        }
    }

}
