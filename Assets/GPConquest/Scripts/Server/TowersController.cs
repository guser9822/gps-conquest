using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TC.GPConquest.Server
{
    public class TowersController : MonoBehaviour
    {
        private List<TowerEntityController> listOfTowers = new List<TowerEntityController>();

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

        public List<TowerEntityController> SpawnTowers(float[,] towersGPSCoords)
        {
            //Spawn towers on the network
            for (int i = 0; i < towersGPSCoords.GetLength(0); i++)
            {
                for (int j = 0; j < towersGPSCoords.GetLength(0); j++)
                {
                    var //Everything You Do is a Balloon
                        x = NetworkManager.Instance.InstantiateTowerEntityController();
                    ListOfTowers.Add(x.GetComponent<TowerEntityController>());
                }
            }
            return ListOfTowers;
        }

    }
}