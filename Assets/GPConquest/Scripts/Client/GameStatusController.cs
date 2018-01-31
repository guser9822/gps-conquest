using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;
using TC.GPConquest.Player;

namespace TC.GPConquest {

    public class GameStatusController : MonoBehaviour
    {
        public ClientNetworkController ClientNetworkController;
        public ConnectionInfo ConnectionInfo;
        public AssetLoaderController AssetLoaderController;

        private void Awake()
        {
            AssetLoaderController.CacheAllUMA("umacharactersasset");
            ConnectionInfo.SetConnectionInfo();
            ClientNetworkController.StartCustomNetworkController(ConnectionInfo);
        }

        private void Start()
        {
            //Instantiate Player
            NetworkManager.Instance.InstantiatePlayerDestinationController(0);
        }
    }

}
