using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;

namespace TC.GPConquest.Server
{
    /**
     * This class is used just for the same kind of initializations that
     * takes place in GameStatusController class
     ***/
    public class ServerProcessController : MonoBehaviour
    {

        protected AssetLoaderController AssetLoaderController;
        protected TowersController TowersController;

        List<Vector2> TowersGPSCoords = new List<Vector2>
        {
            new Vector2(40.856480f, 14.277191f)
            //new Vector2(40.856490f, 14.277192f),
            //new Vector2(40.856485f, 14.277171f)
        };

        private List<TowerEntityController> ListOfTowers;

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


