using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.Player;
using TC.GPConquest.Server;
using UnityEngine;
using System.Linq;
using System;
using TC.GPConquest.Common;

namespace TC.Common
{
    //This class act as a register of specific game objects that are present in the process(server or client) scene.
    /* NOTE : For the moment let's deactivate ISerializationCallbackReceiver, it creates synchronization problems
     *  and sometimes the application doesn't respond correctly. I suspect that the problem is bounded by the
     *  process of serialization/deserialization  and that GameEntityRegister isn't a Singleton. To investigate. 
     */
    public class GameEntityRegister : MonoBehaviour, ISerializationCallbackReceiver
    {
        protected Dictionary<uint, IRegistrable> AllDestinationsControllers = new Dictionary<uint, IRegistrable>();
        protected Dictionary<uint, IRegistrable> AllTowersEntityController = new Dictionary<uint, IRegistrable>();
        protected Dictionary<Type,Dictionary<uint, IRegistrable>> TypeRegisterMap = 
            new Dictionary<Type, Dictionary<uint, IRegistrable>>();

        public List<TowerEntityController> TowersList = new List<TowerEntityController>();
        public List<DestinationController> DestinationsList = new List<DestinationController>();


        private void Awake()
        {
            //Initialize the map of the types with their its own register
            TypeRegisterMap[typeof(DestinationController)] = AllDestinationsControllers;
            TypeRegisterMap[typeof(TowerEntityController)] = AllTowersEntityController;
        }

        public void AddEntity(IRegistrable _entity)
        {
            var aEntityType = _entity.GetType();
            var networkId = _entity.GetUniqueKey();
            TypeRegisterMap[aEntityType][networkId] = _entity;
        }

        public bool RemoveEntity(IRegistrable _entity)
        {
            var aEntityType = _entity.GetType();
            var networkId = _entity.GetUniqueKey();
            return TypeRegisterMap[aEntityType].Remove(networkId);
        }
        /*
         * Return the entity otherwise null
         * **/
        public IRegistrable FindEntity(Type _entityType,uint _gameObjectUniqueKey)
        {
            IRegistrable result = null;
            if (TypeRegisterMap.ContainsKey(_entityType))
            {
                var mapType = TypeRegisterMap[_entityType];
                if (mapType.ContainsKey(_gameObjectUniqueKey))
                    result = mapType[_gameObjectUniqueKey];
                else Debug.LogWarning("Player with netword id " + _gameObjectUniqueKey + " not found");
            }
            else {
                Debug.LogWarning("Type "+ _entityType.ToString() + " not found in the types map");
            }
            return result;
        }
    
        /*
         * Execute this code only in Editor mode, otherwise the application will be unstable
         * **/
        public void OnBeforeSerialize()
        {
    #if UNITY_EDITOR
            TowersList.Clear();
            DestinationsList.Clear();

            AllDestinationsControllers.ToList().ForEach(
            x =>
            {
                DestinationsList.Add((DestinationController)x.Value);
            });

            AllTowersEntityController.ToList().ForEach(
            x =>
            {
                TowersList.Add((TowerEntityController)x.Value);
            });
    #endif
        }

        /*
        * Execute this code only in Editor mode, otherwise the application will be unstable
        * **/
        public void OnAfterDeserialize()
        {
    #if UNITY_EDITOR

            TowersList.ForEach(x =>
            {
                AllTowersEntityController.Add(x.networkObject.NetworkId, x);
            });

            DestinationsList.ForEach(x =>
            {
                AllDestinationsControllers.Add(x.networkObject.NetworkId, x);
            });
    #endif
        }

    }
}


