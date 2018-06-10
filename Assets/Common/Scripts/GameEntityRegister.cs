using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.Player;
using TC.GPConquest.Server;
using UnityEngine;

namespace TC.Common
{
    //This class act as a register of specific game objects that are present in the process(server or client) scene.
    public class GameEntityRegister : MonoBehaviour
    {

        public List<DestinationController> DestinationsControllerList = new List<DestinationController>();
        //public List<TowerEntityController> towersControllerList = new List<TowerEntityController>();

        public void AddDestinationController(DestinationController _destinationController)
        {
            DestinationsControllerList.Add(_destinationController);
        }

        public bool RemoveDestinationController(DestinationController _destinationController)
        {
            return DestinationsControllerList.Remove(_destinationController);
        }

    }
}


