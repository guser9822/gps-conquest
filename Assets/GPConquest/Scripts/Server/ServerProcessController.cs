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
        private List<TowerEntityController> ListOfTowers;

        List<Vector2> TowersGPSCoords = new List<Vector2>
        {
            new Vector2(40.856480f, 14.277191f)
            //new Vector2(40.856490f, 14.277192f),
            //new Vector2(40.856485f, 14.277171f)
        };

        private void Awake()
        {
            AssetLoaderController = GetComponent<AssetLoaderController>();
            AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
            TowersController = GetComponent<TowersController>();
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
            ListOfTowers = TowersController.SpawnTowers(TowersGPSCoords);
        }
    }
}


