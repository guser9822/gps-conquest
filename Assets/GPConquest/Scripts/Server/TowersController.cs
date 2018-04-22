using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TC.GPConquest.Server
{
    public class TowersController : MonoBehaviour
    {
        private List<TowerEntityController> listOfTowers = new List<TowerEntityController>();
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
            var _tower = behavior.GetComponent<TowerEntityController>();
            _tower.InitTowerEntityController(MAPTowerGPSCoords[_tower]);
        }
    }
}