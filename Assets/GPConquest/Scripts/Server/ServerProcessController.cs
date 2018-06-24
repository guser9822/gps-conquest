using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;

namespace TC.GPConquest.Server
{
    /**
     * This class is used for initialize and manage the server process
     ***/
    public class ServerProcessController : MonoBehaviour
    {

        protected AssetLoaderController AssetLoaderController;
        protected TowersController TowersController;
        private ServerNetworkController ServerNetworkController;

        List<Vector2> TowersGPSCoords = new List<Vector2>
        {
            new Vector2(40.856480f, 14.277191f),
            new Vector2(40.857330f, 14.278447f),
            new Vector2(40.857128f, 14.278398f)
        };

        private void Awake()
        {
            AssetLoaderController = GetComponent<AssetLoaderController>();
            AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
            TowersController = GetComponent<TowersController>();
            ServerNetworkController = GetComponent<ServerNetworkController>();
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RequestTowersSpawn()
        {
            TowersController.SpawnTowers(TowersGPSCoords);
        }

        public void RequestServerConnection(ConnectionInfo _connectionInfo) {
            ServerNetworkController.StartCustomNetworkController(_connectionInfo);
        }

        public void RequestServerDisconnection() {
            TowersController.DestroyTowers();
            ServerNetworkController.CloseMultiplayerBase();
        }

    }
}


