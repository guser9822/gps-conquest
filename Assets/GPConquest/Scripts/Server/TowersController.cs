using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using TC.Common;
using TC.GPConquest.Common;
using TC.GPConquest.Player;
using UnityEngine;
using System.Linq;
using Assets.Helpers;

namespace TC.GPConquest.Server
{
    //This class is used to manage towers
    public class TowersController : MonoBehaviour
    {
        private List<TowerEntityController> listOfTowers = new List<TowerEntityController>();
        //This dictionary contains key-value pairs formed like this (tower->gpscoords) in order to simplfy towers creation 
        private Dictionary<TowerEntityController, Vector2> MAPTowerGPSCoords 
            = new Dictionary<TowerEntityController, Vector2>();
        private GameEntityRegister GameEntityRegister;

        /* At level of zoom 16 we know that the ground resolution  (that is
         the ratio between the metric unit and the pixel) is 2,3887 meter
         per pixel, so 2,3887 * 256 (tile size in pixel) = 611 uu (unity unit) which
         are basically 611 meters*/
        public static readonly float TOWER_SPAWN_DISTANCE = 611*4f;//2 Km and HALF

        public static readonly int NEAR_TOWERS_NUM = 10;

        private void Awake()
        {
            GameEntityRegister = GetComponent<GameEntityRegister>();
        }

        public List<TowerEntityController> ListOfTowers
        {
            get
            {
                return listOfTowers;
            }

            private set
            {
                listOfTowers = value;
            }
        }

        public List<TowerEntityController> SpawnTowers(List<Vector2> towersGPSCoords)
        {
            //Atm we get gps coords from a list and create the associations tower->gpscoords
            towersGPSCoords.ForEach(x => {
                var //Everything You Do is a Balloon
                   _thisTower = NetworkManager.Instance.InstantiateTowerEntityController();
                var _thisTowerEntityController = _thisTower.GetComponent<TowerEntityController>();

                MAPTowerGPSCoords.Add(_thisTowerEntityController, x);

                _thisTower.networkStarted += _thisTower_networkStarted;
                ListOfTowers.Add(_thisTowerEntityController);
            });

            return ListOfTowers;
        }

        private void _thisTower_networkStarted(NetworkBehavior behavior)
        {
            //After that the tower is generated on the network, adjust it's position
            var _tower = behavior.GetComponent<TowerEntityController>();
            _tower.InitTowerEntityController(MAPTowerGPSCoords[_tower]);
        }

        public void DestroyTowers() {
            listOfTowers.ForEach(x =>
            {
                x.networkObject.ClearRpcBuffer();
                x.networkObject.Destroy();
            });
        }

        public void ResetAllTowers()
        {
            //######## to implement
        }

        public void SpawnTowersOnRequest(uint _playerNetId)
        {
            //Find the player that requested towers by it's network id
            var player = GameEntityRegister.FindEntity(typeof(DestinationController),_playerNetId) as DestinationController;
            //Get it's position
            var playerPosition = player.transform.position;
            //Get all the towers
            var towers = GameEntityRegister.GetAllEntity(typeof(TowerEntityController)).Cast<TowerEntityController>();
            //Find towers near the player
            var nearTowers = FindTowersByDistance(towers,TOWER_SPAWN_DISTANCE,playerPosition);
            //spawn a sufficient number of towers
            if (nearTowers.Count <= NEAR_TOWERS_NUM)
            {
                int remaining = Mathf.Abs(nearTowers.Count - NEAR_TOWERS_NUM);
                List<Vector2> newTowersPositions = new List<Vector2>();
                for (int i = 0; i < remaining; i++) {
                    var newRandGPSCoords = RandGPSCoord(playerPosition, TOWER_SPAWN_DISTANCE);
                    newTowersPositions.Add(newRandGPSCoords);
                }
                if(remaining>0)
                    SpawnTowers(newTowersPositions);
            }
        }

        private List<TowerEntityController> FindTowersByDistance(IEnumerable<TowerEntityController> towers, float _dis,Vector3 playerPosition)
        {
            List<TowerEntityController> nearTowers = new List<TowerEntityController>();
            //Check towers proximity respect to the player
            towers.ToList<TowerEntityController>().
                ForEach(tower =>
                {
                    var towerPosition = tower.transform.position;
                    if (Vector3.Distance(playerPosition, towerPosition)
                            <= TOWER_SPAWN_DISTANCE)
                        nearTowers.Add(tower);
                });
            return nearTowers;
        }

        private Vector2 RandGPSCoord(Vector3 _playerPosition, float _dist)
        {
            var randomDistX = Random.Range(.0f, _dist);
            float x = ((int)Random.value) % 2 == 0 ? randomDistX : randomDistX * -1;
            var randomDistZ = Random.Range(.0f, _dist);
            float z = ((int)Random.value) % 2 == 0 ? randomDistZ : randomDistZ * -1;
            var gpsCoords =  GM.MetersToLatLon(new Vector2(_playerPosition.x + x, _playerPosition.z +z));
            return gpsCoords;
        }
    }
}