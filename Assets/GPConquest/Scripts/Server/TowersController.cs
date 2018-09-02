using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TC.GPConquest.Server
{
    //This class is used to manage towers
    public class TowersController : MonoBehaviour
    {
        private List<TowerEntityController> listOfTowers = new List<TowerEntityController>();
        //This dictionary contains key-value pairs formed like this (tower->gpscoords) in order to simplfy towers creation 
        private Dictionary<TowerEntityController, Vector2> MAPTowerGPSCoords 
            = new Dictionary<TowerEntityController, Vector2>();

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

    }
}