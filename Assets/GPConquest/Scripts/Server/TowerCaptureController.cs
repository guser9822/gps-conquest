using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Linq;
using TC.GPConquest.GpsMap.Helpers;
using TC.Common;
using TC.GPConquest.Player;


namespace TC.GPConquest.Server
{
    public class TowerCaptureController : TowerCaptureNetworkControllerBehavior
    {
        public TowerEntityController TowerEntityController { get; set; }

        //Capture Times expressed in milliseconds
        private struct TIMES {
            public const double FACTION_CONQUEST_TIME = 30000;
            public const double UPDATE_NETWORK_TOWERS_TIME = 0.5;//Timer used to send RPC every half second
        };
        //RPC send counter
        private double RPCSendCounter = 0.0;

        //Table <Tuple2<NetworkId,Nickname>,Time spent in the capture of the tower>>
        private Dictionary<GPSConqTuple2<string, uint>, double> PlayerNetIdNameCaptureTimeTable =
            new Dictionary<GPSConqTuple2<string, uint>, double>();

        //Dictionary that contains for each faction the amount of time
        //that the players of that same faction have spent during tower capturing
        private Dictionary<string, double> FactionsConquestTimeDict = new Dictionary<string, double>();

        //Just for log through inspector
        public List<string> PlayerCapturingTowerList = new List<string>();

        //Table <Tuple2<NetworkId,Nickname>,DestinationController>> NOTE : USED ONLY BY THE OWNER
        private Dictionary<GPSConqTuple2<string, uint>, DestinationController> PlayerNetIdNameDestinationObj =
            new Dictionary<GPSConqTuple2<string, uint>, DestinationController>();

        private void Update()
        {

            if (networkObject.IsOwner)
            {
                PlayerNetIdNameCaptureTimeTable = PlayerCapturingCounters(PlayerNetIdNameCaptureTimeTable);
                FactionsConquestTimeDict = FactionsCaptureCounter(FactionsConquestTimeDict,
                    PlayerNetIdNameCaptureTimeTable,
                    PlayerNetIdNameDestinationObj);
            }

        }

        protected Dictionary<string, double> FactionsCaptureCounter(Dictionary<string, double> _factionsConquestTimeDict,
            Dictionary<GPSConqTuple2<string, uint>, double> _playerNetIdNameCaptureTimeTable,
            Dictionary<GPSConqTuple2<string, uint>, DestinationController> _playerNetIdNameDestinationObj)
        {
            var playerList = _playerNetIdNameDestinationObj.ToList();

            playerList.ForEach(s =>
            {
                var playerNameNetid = s.Key;
                var destinationController = s.Value;
                double timePlayerSpent;

                if (_playerNetIdNameCaptureTimeTable.TryGetValue(playerNameNetid, out timePlayerSpent))
                {
                    var playerEntity = destinationController.gameObject.GetComponent<PlayerEntity>();
                    var faction = playerEntity.faction;
                    double factionTime;

                    if (_factionsConquestTimeDict.TryGetValue(faction, out factionTime))
                    {
                        factionTime += timePlayerSpent;
                        _factionsConquestTimeDict.Add(faction, factionTime);
                    }

                }

            });

            return _factionsConquestTimeDict;
        }

        protected Dictionary<GPSConqTuple2<string, uint>, double> PlayerCapturingCounters(
            Dictionary<GPSConqTuple2<string, uint>, double> _playerNetIdNameCaptureTimeTable) {

            var playersCapturingTower = _playerNetIdNameCaptureTimeTable.ToList();
            if (playersCapturingTower.Count > 0)
            {
                playersCapturingTower.ForEach(
                    s => {

                        double timePassed = s.Value; //Time passed since the player is stayed in the capture zone
                        //  OLD FORMULA double timeAdd = +timePassed + Time.deltaTime; //Add time passed since last update 

                        double timeAdd = Time.deltaTime; //Now we will sum the amount that the player spent in the capture of the tower
                                                         //of time directly in the total time of the faction
                        _playerNetIdNameCaptureTimeTable[s.Key] = timeAdd;

                        if(RPCSendCounter >= TIMES.UPDATE_NETWORK_TOWERS_TIME)
                            networkObject.SendRpc(RPC_UPDATE_CAPTURE_TIME_FOR_PLAYER,
                                Receivers.AllBuffered,
                                s.Key.GetFrist(),
                                s.Key.GetSecond(),
                                timeAdd);
                       
                    });
            }

            //Increment the counter, else azzerate it.
            RPCSendCounter = RPCSendCounter >= TIMES.UPDATE_NETWORK_TOWERS_TIME ? 
                0.0 : RPCSendCounter + Time.deltaTime;

            return _playerNetIdNameCaptureTimeTable;
        }

        public void InitTowerCaptureController(TowerEntityController _towerEntityController)
        {
            UpdateTowerCaptureControllerAttributes(_towerEntityController);

            //Destroy this object when the Tower is destroyed
            _towerEntityController.networkObject.onDestroy += NetworkObject_onDestroy;

            networkObject.SendRpc(RPC_UPDATE_CAPTURE_CONTROLLER_ON_NETWORK
                , Receivers.AllBuffered);
        }

        private void UpdateTowerCaptureControllerAttributes(TowerEntityController _towerEntityController)
        {
            networkObject.TowerEntityNetId = _towerEntityController.networkObject.NetworkId;
            TowerEntityController = _towerEntityController;
            _towerEntityController.TowerCaptureController = this;
            transform.SetParent(TowerEntityController.transform);
        }

        public override void UpdateCaptureControllerOnNetwork(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                TowerEntityController[] playersInTheScene = FindObjectsOfType<TowerEntityController>();

                var tower = playersInTheScene.ToList().
                    Find(x => x.networkObject.NetworkId.Equals(networkObject.TowerEntityNetId));

                UpdateTowerCaptureControllerAttributes(tower);
            });
        }

        //This method checks if the player is capturing the tower and updates this object internal structures
        public void CheckForCapture(DestinationController _playerDestinationComponent, bool _isCapturing)
        {
            var playerNetId = _playerDestinationComponent.networkObject.NetworkId;
            var playerNickname = _playerDestinationComponent.AvatorController.PlayerEntity.username;

            AddOrDeletePlayerToTheCapturing(playerNickname,
                playerNetId,
                _isCapturing,
                _playerDestinationComponent);

            //Keep updated also the network entities of this object
            networkObject.SendRpc(RPC_UPDATE_CAPTURE_ON_NETWORK
                , Receivers.AllBuffered
                , playerNetId
                , _isCapturing
                , playerNickname);
        }

        protected void AddOrDeletePlayerToTheCapturing(string _name,
            uint _playerNetId,
            bool _isCapturing,
            DestinationController _playerDestinationComponent)
        {
            GPSConqTuple2<string, uint> playerKey = new GPSConqTuple2<string, uint>(_name, _playerNetId);
            var playerKeyString = playerKey.ToString();
            double val = 0d;
            bool exists;

            if (_isCapturing)
            {
                exists = PlayerNetIdNameCaptureTimeTable.TryGetValue(playerKey, out val);
                if (!exists)
                {
                    PlayerNetIdNameCaptureTimeTable.Add(playerKey, 0d);
                    PlayerNetIdNameDestinationObj.Add(playerKey, _playerDestinationComponent);
                    PlayerCapturingTowerList.Add(playerKeyString);
                }
            }
            else
            {
                exists = PlayerNetIdNameCaptureTimeTable.TryGetValue(playerKey, out val);
                if (exists)
                {
                    PlayerNetIdNameCaptureTimeTable.Remove(playerKey);
                    PlayerNetIdNameDestinationObj.Remove(playerKey);
                    PlayerCapturingTowerList.Remove(playerKeyString);
                }
            }
        }

        public override void UpdateCaptureOnNetwork(RpcArgs args)
        {
            var playerNetId = args.GetNext<uint>();
            var isCapturing = args.GetNext<bool>();
            var playerNickname = args.GetNext<string>();

            //TODO Fix
            //For the moment, the network towers will jsut put null as destination controller
            AddOrDeletePlayerToTheCapturing(playerNickname,
                playerNetId,
                isCapturing,
                null);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

        public override void UpdateCaptureTimeForPlayer(RpcArgs args)
        {
            var playerUsername =  args.GetNext<string>();
            var playerNetId = args.GetNext<uint>();
            var captureTimePassed = args.GetNext<double>();
            GPSConqTuple2<string, uint> thisPlayerKey = new GPSConqTuple2<string, uint>(playerUsername, playerNetId);
            PlayerNetIdNameCaptureTimeTable[thisPlayerKey] = captureTimePassed;
            var previousElapsedTime = PlayerNetIdNameCaptureTimeTable[thisPlayerKey];
            Debug.Log("[Proxy]The player " + thisPlayerKey.GetFrist() + " spent secs " + previousElapsedTime);
        }

        public double TimeISpentForCapturingThisTower(string _playerUsername, uint _playerNetId)
        {
            GPSConqTuple2<string, uint> thisPlayerKey = new GPSConqTuple2<string, uint>(_playerUsername, _playerNetId);
            return PlayerNetIdNameCaptureTimeTable[thisPlayerKey];
        }

        public double TimeSpentByMyFunctionForCapturingTheTower(string _myfaction)
        {
            double timeSpent;
            FactionsConquestTimeDict.TryGetValue(_myfaction,out timeSpent);
            return timeSpent;
        }
    }

}
