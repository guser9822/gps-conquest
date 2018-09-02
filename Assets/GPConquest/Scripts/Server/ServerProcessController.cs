using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;
using BeardedManStudios.Forge.Networking.Unity;
using System.Linq;

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
        private GameFactionsController GameFactionsController;
        private GameEntityRegister GameEntityRegister;

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
            //Instantiates towers
            TowersController.SpawnTowers(TowersGPSCoords);

        }

        public void RequestServerConnection(ConnectionInfo _connectionInfo) {
            ServerNetworkController.StartCustomNetworkController(_connectionInfo);
            //Instantiate factions controller
            GameFactionsController = NetworkManager.Instance.InstantiateGameFactionsNetworkController() as GameFactionsController;
        }

        public void RequestServerDisconnection() {
            TowersController.DestroyTowers();
            ServerNetworkController.CloseMultiplayerBase();
            ClearFactionsController();
        }

        private void ClearFactionsController() {
            GameFactionsController.networkObject.ClearRpcBuffer();
            GameFactionsController.networkObject.Destroy();
        }

        /// <summary>
        /// This function is used in order to send a message to all the players
        /// that are connect that the winner faction for this period is declared
        /// and reset for all towers the owner 
        /// </summary>
        public void DeclareWinnerFaction(string winningFaction)
        {
            var destIregistrable = GameEntityRegister.GetAllEntity(typeof(DestinationController));
            var connectedPlayerDest = destIregistrable.Cast<DestinationController>();

            if (!ReferenceEquals(winningFaction, null) && winningFaction.Length > 0 && destIregistrable.Count > 0)
            {
                //Notify all players
                connectedPlayerDest.ToList().ForEach(x =>
                {
                    x.NotifyWinningFaction(winningFaction);
                });

                //Reset the owner for all towers
                TowersController.ResetAllTowers();
            }
            else Debug.LogError("Invalid faction name or empty player list");
        }

    }
}


