using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.Player;
using TC.GPConquest.Server;
using UnityEngine;
using System.Linq;


namespace TC.Common
{
    //This class act as a register of specific game objects that are present in the process(server or client) scene.
    public class GameEntityRegister : MonoBehaviour
    {

        public List<object> DestinationsControllerList = new List<object>();
        public List<object> TowersControllerList = new List<object>();

        public void AddEntity(object _entity)
        {
            var aEntityType = _entity.GetType();
            if ( aEntityType.Equals(typeof(DestinationController)))
            {
                AddDestinationController(_entity);
            }
            else if (aEntityType.Equals(typeof(TowerEntityController)))
            {
                AddTowerController(_entity);
            }
        }

        public bool RemoveEntity(object _entity)
        {
            var aEntityType = _entity.GetType();
            return aEntityType.Equals(typeof(DestinationController)) ?
                DestinationsControllerList.Remove((DestinationController)_entity):
                aEntityType.Equals(typeof(TowerEntityController)) ?
                TowersControllerList.Remove((TowerEntityController)_entity) : false;

        }

        public object FindEntity(System.Type _entityType,System.Predicate<object> match)
        {
            return _entityType.Equals(typeof(DestinationController)) ?
                DestinationsControllerList.Find(x => {return match(x);}) :
                _entityType.Equals(typeof(TowerEntityController)) ? 
                TowersControllerList.Find(x => { return match(x); }) : null;
        }

        protected void AddTowerController(object _towerEntityControllerInput)
        {
            TowerEntityController _towerEntityController = (TowerEntityController) _towerEntityControllerInput;
            var towerToAddNetId = _towerEntityController.networkObject.NetworkId;
            var towerToAddGPSCoords = _towerEntityController.GPSCoords;

            if (!TowersControllerList.Any<object>(xx =>
            {
                TowerEntityController x = (TowerEntityController)xx;
                var towerInListNetId = x.networkObject.NetworkId;
                var towerInListGPSCoors = x.GPSCoords;
                return towerInListNetId.Equals(towerToAddNetId) && towerInListGPSCoors.Equals(towerToAddGPSCoords);
            }))
            {
                TowersControllerList.Add(_towerEntityController);
            }
        }

        protected void AddDestinationController(object _destinationControllerInput)
        {
            DestinationController _destinationController = (DestinationController)_destinationControllerInput;
            var playerToAddNetId = _destinationController.networkObject.NetworkId;
            var playerToAddUsername = _destinationController.PlayerName;

            /*
             * We have to check if the destination controller is already in the list before add it,
             * because for some strange reason on the owner process the destination controller is 
             * added two times
             * **/
            if (!DestinationsControllerList.Any<object>(xx =>
            {
                DestinationController x = (DestinationController)xx;
                var playerInListNetId = x.networkObject.NetworkId;
                var playerInListUsername = x.GetComponent<DestinationController>().PlayerName;
                return playerInListNetId.Equals(playerInListNetId) && playerInListUsername.Equals(playerToAddUsername);
            }))
            {
                DestinationsControllerList.Add(_destinationController);
            }
        }


    }
}


