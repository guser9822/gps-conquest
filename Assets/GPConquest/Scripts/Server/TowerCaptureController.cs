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
using TC.GPConquest.Common;
using TC.GPConquest.Utility;

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

        //Table <DestinationController,Time spent in the capture of the tower>
        private Dictionary<DestinationController, double> PlayerCaptureTimeTable = new Dictionary<DestinationController, double>();

        /* Dictionary that contains for each faction the amount of time
           that the players of that same faction have spent during tower capturing */
        private Dictionary<string, double> FactionsConquestTime = new Dictionary<string, double>();

        //Just for log through inspector
        public List<string> PlayerCapturingTowerList = new List<string>();

        List<CaptureTransferInfo> CaptureInfoList = new List<CaptureTransferInfo>();

        void Awake()
        {
            FactionsConquestTime = InitializeFactionsConquestTime(FactionsConquestTime);
        }

        //Initialize the map with factions and time set to 0
        protected Dictionary<string, double> InitializeFactionsConquestTime(Dictionary<string, double> _factionsConquestTime)
        {
            //Initialize the map factions/time passed
            GameCommonNames.FACTIONS.ForEach(x =>
            {
                _factionsConquestTime[x] = 0D;
            });
            return _factionsConquestTime;
        }

        private void Update()
        {

            if (networkObject.IsOwner && PlayerCaptureTimeTable.Count>0) 
            {
                PlayerCaptureTimeTable = PlayersCapturingCounter(PlayerCaptureTimeTable);
                //Let the owner of the towers fill the list and send updates on the network
                CaptureInfoList = FillCaptureInfoList(PlayerCaptureTimeTable, CaptureInfoList);
                CaptureInfoList = SendCaptureInfoOverTheNetwork(CaptureInfoList);
                FactionsConquestTime = FactionsCapturingCounter(FactionsConquestTime,PlayerCaptureTimeTable);
            }

        }

        protected List<CaptureTransferInfo> SendCaptureInfoOverTheNetwork(List<CaptureTransferInfo> _captureInfoList)
        {
            //Convert the list of the informations into an array of bytes
            var bytes = ByteArrayUtils.ObjectToByteArray(_captureInfoList);

            networkObject.SendRpc(RPC_UPDATE_CAPTURES_TIMES_ON_NETWORK,
                true,
                Receivers.All,
                new object[] { bytes });

            _captureInfoList.Clear();
            return _captureInfoList;
        }

        protected List<CaptureTransferInfo> FillCaptureInfoList(Dictionary<DestinationController, double> _playerCaptureTimeTable,
            List<CaptureTransferInfo> _captureInfoList) 
        {
            _playerCaptureTimeTable.ToList().ForEach(x =>
            {
                var player = x.Key;
                CaptureTransferInfo newCaptureInfo = new CaptureTransferInfo()
                {
                    uniqueId = player.networkObject.NetworkId,
                    captureTime = x.Value
                };

                _captureInfoList.Add(newCaptureInfo);
            });
            return _captureInfoList;
        }

        protected Dictionary<string, double> FactionsCapturingCounter(Dictionary<string, double> _factionsTime,
            Dictionary<DestinationController, double> _playerCaptureTimeTable)
        {
            //Set the timer at 0 for each faction
            _factionsTime = InitializeFactionsConquestTime(_factionsTime);

            var players = _playerCaptureTimeTable.Keys.ToList<DestinationController>();
            players.ForEach(player =>
            {
                var playerEntity = player.AvatorController.PlayerEntity;
                var faction = playerEntity.faction;
                var playerTime = _playerCaptureTimeTable[player];
                var newTime = _factionsTime[faction] + playerTime;
                _factionsTime[faction] = newTime;
            });
            return _factionsTime;
        }

        protected Dictionary<DestinationController, double> PlayersCapturingCounter(
            Dictionary<DestinationController, double> _playerNetIdNameCaptureTimeTable) {

            var playersCapturingTower = _playerNetIdNameCaptureTimeTable.ToList();
            if (playersCapturingTower.Count > 0)
            {
                playersCapturingTower.ForEach(
                    s => {
                        var playerNetworkkId = s.Key.networkObject.NetworkId;
                        double timePassed = s.Value; //Time passed since the player is stayed in the capture zone
                        double timeAdd = timePassed + Time.deltaTime; //Add time passed since last update 

                        _playerNetIdNameCaptureTimeTable[s.Key] = timeAdd;
                   });
            }
      
            return _playerNetIdNameCaptureTimeTable;
        }

        public void InitTowerCaptureController(TowerEntityController _towerEntityController)
        {
            UpdateTowerCaptureControllerAttributes(_towerEntityController);

            //Destroy this object when the Tower is destroyed
            _towerEntityController.networkObject.onDestroy += NetworkObject_onDestroy;

            networkObject.SendRpc(RPC_UPDATE_CAPTURE_CONTROLLER_ON_NETWORK,Receivers.AllBuffered);
        }

        private void UpdateTowerCaptureControllerAttributes(TowerEntityController _towerEntityController)
        {
            networkObject.TowerEntityNetId = _towerEntityController.networkObject.NetworkId;
            TowerEntityController = _towerEntityController;
            _towerEntityController.TowerCaptureController = this;
            transform.SetParent(TowerEntityController.transform);
        }

        //Update the capture controller on the network after initialization 
        public override void UpdateCaptureControllerOnNetwork(RpcArgs args)
        {
            var gameRegister = FindObjectOfType<GameEntityRegister>();

            var tower = (TowerEntityController)gameRegister.FindEntity(typeof(TowerEntityController),
                networkObject.TowerEntityNetId);
            UpdateTowerCaptureControllerAttributes(tower);
        }

        //This method checks if the player is capturing the tower and updates this object internal structures
        public void CheckForCapture(DestinationController _playerDestinationComponent, bool _isCapturing)
        {
            var playerNetId = _playerDestinationComponent.networkObject.NetworkId;
            AddOrDeletePlayerToTheCapturing(_playerDestinationComponent, _isCapturing);

            //Keep updated also the network entities of this object
            networkObject.SendRpc(RPC_UPDATE_CAPTURE_ON_NETWORK ,
                Receivers.AllBuffered,
                playerNetId,
                _isCapturing);
        }

        protected void AddOrDeletePlayerToTheCapturing(DestinationController _playerDestinationComponent,
            bool _isCapturing)
        {
            double val = 0d;
            bool exists;

            var playerKeyString = 
                _playerDestinationComponent.networkObject.NetworkId.ToString() + 
                " - " 
                + _playerDestinationComponent.PlayerName;

            if (_isCapturing)
            {
                exists = PlayerCaptureTimeTable.TryGetValue(_playerDestinationComponent, out val);
                if (!exists)
                {
                    PlayerCaptureTimeTable.Add(_playerDestinationComponent, 0d);
                    PlayerCapturingTowerList.Add(playerKeyString);
                }
            }
            else
            {
                exists = PlayerCaptureTimeTable.TryGetValue(_playerDestinationComponent, out val);
                if (exists)
                {
                    PlayerCaptureTimeTable.Remove(_playerDestinationComponent);
                    PlayerCapturingTowerList.Remove(playerKeyString);
                }
            }
        }

        //Update the list of the players on the network that are capturing the tower
        public override void UpdateCaptureOnNetwork(RpcArgs args)
        {
            var playerNetId = args.GetNext<uint>();
            var isCapturing = args.GetNext<bool>();

            var player = FindPlayerByNetId(playerNetId);
            AddOrDeletePlayerToTheCapturing(player, isCapturing);
        }

        private void NetworkObject_onDestroy(NetWorker sender)
        {
            networkObject.ClearRpcBuffer();
            networkObject.Destroy();
        }

        protected DestinationController FindPlayerByNetId(uint _playerNetId)
        {
            var gameRegister = TowerEntityController.GameEntityRegister;
            return (DestinationController)gameRegister.FindEntity(typeof(DestinationController), _playerNetId);
        }

        public double TimeISpentForCapturingThisTower(DestinationController _destinationController)
        {
            return PlayerCaptureTimeTable[_destinationController];
        }

        public double TimeSpentByMyFactionForCapturingTheTower(string _myfaction)
        {
            double timeSpent;
            FactionsConquestTime.TryGetValue(_myfaction,out timeSpent);
            return timeSpent;
        }

        public override void UpdateCapturesTimesOnNetwork(RpcArgs args)
        {
            var bytes = args.GetNext<Byte[]>();
            List<CaptureTransferInfo> captureInfos = (List<CaptureTransferInfo>)ByteArrayUtils.ByteArrayToObject(bytes);

            //Update the capture time of the player on the CaptureController over the network
            captureInfos.ForEach(x =>
            {
                var uniqueId = x.uniqueId;
                var capturetime = x.captureTime;
                //Find the player with this networkid
                var keyValuePlayer = PlayerCaptureTimeTable.First(y =>
                {
                    return y.Key.networkObject.NetworkId.Equals(uniqueId);
                });
                var player = keyValuePlayer.Key;
                //Set the new calculated time
                PlayerCaptureTimeTable[player] = capturetime;
                var time = PlayerCaptureTimeTable[player];
                Debug.Log("[Proxy] The player " + player.PlayerName + " spent " + time + " secs.");
            });

            //Recalculate time for the factions
            FactionsConquestTime = FactionsCapturingCounter(FactionsConquestTime, PlayerCaptureTimeTable);
        }
    }

}
