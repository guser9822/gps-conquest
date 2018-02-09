﻿using System.Collections;
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
        public tileGen TileGen;

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
            /*
             * Chain of Events : 
             * NetworkTileGen instantiates PlayerDestinationController because it dictate player position 
             * PlayerDestinationController instantiates AvatorController because of the old design 
             * of the game, it should be changed.
             */
            //NetworkManager.Instance.InstantiateNetworkTileGen(0);
            Instantiate<tileGen>(TileGen);
        }
    }

}
