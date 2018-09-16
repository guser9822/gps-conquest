using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using System;
using System.Linq;
using TC.Common;
using TC.GPConquest.Player;
using TC.GPConquest.Common;
using TC.GPConquest.Utility;

namespace TC.GPConquest.Server
{
    public class TowerCaptureController : TowerCaptureNetworkControllerBehavior,ISerializationCallbackReceiver
    {
        public TowerEntityController TowerEntityController { get; set; }
        private string WinningOrNoFaction = GameCommonNames.NO_FACTION;//used only by the owner of the tower to contain the winning faction or the NO_FACTION name

        public static readonly string CAPTURE_IN_PROGRES = "CAPTURE_IN_PROGRESS";
        public static readonly string CAPTURED_BY_FACTION = "CAPTURED_BY_FACTION";
        public static readonly string STARTUP_STATE = "INITIAL_STATE";
        protected string CurrentTowerPhase = STARTUP_STATE;
        //Times expressed in seconds
        private struct TIMES
        {
            public const double FACTION_CONQUEST_TIME = 30;
            public const double WAITING_TIME_BEFORE_NEXT_CAPTURE = 10;
            public const double UPDATE_NETWORK_CAPT_CONTROLLERS_TIME = 0.5;//Timer used to send RPC every half second
        };

        //Timers that keep time passed
        private double WAITING_TIME_BEFORE_NEXT_CAPTURE_PASSED = 0.0;
        private double UPDATE_NETWORK_TOWERS_TIME_PASSED = 0.0;

        //Table <DestinationController,Time spent in the capture of the tower>
        private Dictionary<DestinationController, double> PlayersCaptureTimeTable = new Dictionary<DestinationController, double>();

        /* Dictionary that contains for each faction the amount of time
           that the players of that same faction have spent during tower capturing */
        private Dictionary<string, double> FactionsConquestTime = new Dictionary<string, double>();

        //Just for log through inspector
        public List<DestinationController> PlayerCapturingTowerList = new List<DestinationController>();

        List<CaptureTransferInfo> CaptureInfoList = new List<CaptureTransferInfo>();
        private bool IsCaptured = false;

        void Awake()
        {
            WinningOrNoFaction = GameCommonNames.NO_FACTION;
            FactionsConquestTime = InitializeFactionsConquestTime(FactionsConquestTime);
        }

        //Initialize the map with factions and time set to 0
        protected Dictionary<string, double> InitializeFactionsConquestTime(Dictionary<string, double> _factionsConquestTime)
        {
            //Initialize the map factions/time passed
            GameCommonNames.FACTIONS.ForEach(x =>
            {
                if (!ReferenceEquals(x, null))
                    _factionsConquestTime[x] = 0D;
            });
            return _factionsConquestTime;
        }

        private void Update()
        {
            if (networkObject.IsOwner)
            {
                if (PlayersCaptureTimeTable.Count > 0 && !IsCaptured)
                {
                    UPDATE_NETWORK_TOWERS_TIME_PASSED += Time.deltaTime;

                    //This check and this call are used to update just once the color of the tower effect.
                    //If the tower is under capture, call repeatly NotifyChangeInTower consumes bandwidth 
                    //because some RPCs are involded.
                    if (!CurrentTowerPhase.Equals(CAPTURE_IN_PROGRES))
                    {
                        CurrentTowerPhase = CAPTURE_IN_PROGRES;
                        NotifyChangeInTower(TowerEntityController, WinningOrNoFaction, CurrentTowerPhase);
                    }

                    //Count the time spent by each player during the capture
                    PlayersCaptureTimeTable = PlayersCapturingCounter(PlayersCaptureTimeTable);
                    if (UPDATE_NETWORK_TOWERS_TIME_PASSED >= TIMES.UPDATE_NETWORK_CAPT_CONTROLLERS_TIME)
                    {
                        //Let the owner of the towers fill the list and sends updates on the network
                        CaptureInfoList = FillCaptureInfoList(PlayersCaptureTimeTable, CaptureInfoList);
                        CaptureInfoList = SendCaptureInfoOverTheNetwork(CaptureInfoList);
                        UPDATE_NETWORK_TOWERS_TIME_PASSED = 0D;
                    }
                    //Sums the time spent by players for each faction
                    FactionsConquestTime = FactionsCapturingCounter(FactionsConquestTime, PlayersCaptureTimeTable);
                    //Checks if one faction reached the winning time
                    IsCaptured = CheckWinningConditions(FactionsConquestTime, PlayersCaptureTimeTable, WinningOrNoFaction);

                    //If the tower have been captured, sets a timer that blocks the capture game untill it's value will be 0
                    if (IsCaptured)
                    {
                        WAITING_TIME_BEFORE_NEXT_CAPTURE_PASSED = TIMES.WAITING_TIME_BEFORE_NEXT_CAPTURE;
                        CurrentTowerPhase = CAPTURED_BY_FACTION;
                        //Sets the winner : update the faction name and the color of the tower effect
                        //NOTE : the TowerUINetworkController have an internal dictionary that ,for each faction, have the corresponding color
                        NotifyChangeInTower(TowerEntityController, WinningOrNoFaction, WinningOrNoFaction);
                    }

                }

                //If the tower have been captured, starts the countdown in order to reactivate the capture game
                if (IsCaptured)
                {
                    WAITING_TIME_BEFORE_NEXT_CAPTURE_PASSED -= Time.deltaTime;
                    //If the timer gets azzerated, reactivates the capture game
                    if (WAITING_TIME_BEFORE_NEXT_CAPTURE_PASSED <= 0.0f)
                    {
                        IsCaptured = false;
                        CurrentTowerPhase = STARTUP_STATE;
                        //Activate the tower capture game, but keep the tower faction and that faction color
                        NotifyChangeInTower(TowerEntityController, WinningOrNoFaction, WinningOrNoFaction);
                    }
                }

            }
        }

        /// <summary>
        /// Notify the tower entity to change it's state.
        /// </summary>
        /// <param name="_towerEntityController"></param>
        /// <param name="_winner"></param>
        /// <param name="_actionName"></param>
        protected void NotifyChangeInTower(TowerEntityController _towerEntityController, string _winner, string _actionName)
        {
            if (!ReferenceEquals(_towerEntityController, null) && 
                !ReferenceEquals(_winner, null) &&
                !ReferenceEquals(_actionName,null))
            {
                _towerEntityController.ChangeTowerEntityStatus(_winner,_actionName);
            }
            else Debug.LogWarning("Winner faction or tower entity are null");
        }

        protected bool CheckWinningConditions(Dictionary<string, double> _factionsConquestTime,
            Dictionary<DestinationController, double> _playerCaptureTimeTable,
            string _winningOrNoFaction)
        {
            bool _isCaptured = false;

            //Find the first faction that reached the conquest time
            var winningFaction = _factionsConquestTime.
                Where(x => IsWinningFaction(x.Key, x.Value)).
                FirstOrDefault();

            var winningFactionName = winningFaction.Key;
            if (!ReferenceEquals(null, winningFactionName))
            {
                WinningOrNoFaction = winningFactionName;
                //Find the player of that faction with the more time spent on the capture of the tower
                var winningPlayerPair = _playerCaptureTimeTable.Where(
                    x =>
                    {
                        var avatorController = x.Key.AvatorController;
                        var playerEntity = avatorController.PlayerEntity;
                        return playerEntity.faction.Equals(winningFactionName);
                    }).
                    OrderByDescending<KeyValuePair<DestinationController, double>, double>(x => x.Value).
                    FirstOrDefault();

                var winningPlayer = winningPlayerPair.Key;
                var gameEntities = TowerEntityController.GameEntityRegister;
                if (!ReferenceEquals(null, winningPlayer) &&
                    !ReferenceEquals(gameEntities.FindEntity(typeof(DestinationController), winningPlayer.networkObject.NetworkId), null))
                {
                    Debug.Log("[OWNER] We have the Winning Faction : "
                        + winningFactionName +
                        ", won by " + winningPlayer.PlayerName);
                    _isCaptured = true;
                }
                else {
                    Debug.LogWarning("The strange happened no winning player found for faction " + winningFactionName);
                    CheckForCapture(winningPlayer, false);
                }
            }
            return _isCaptured;
        }

        protected bool IsWinningFaction(string _factionName, double _captureTime)
        {
            return _captureTime >= TIMES.FACTION_CONQUEST_TIME ? true : false;
        }

        protected List<CaptureTransferInfo> SendCaptureInfoOverTheNetwork(List<CaptureTransferInfo> _captureInfoList)
        {
            //Convert the list of the informations into an array of bytes
            var bytes = ByteArrayUtils.ObjectToByteArray(_captureInfoList);
            if (!ReferenceEquals(bytes, null) && bytes.Length > 0)
            {
                networkObject.SendRpc(RPC_UPDATE_CAPTURES_TIMES_ON_NETWORK,
                                        true,
                                        Receivers.All,
                                        new object[] { bytes });

                _captureInfoList.Clear();
            }
            else Debug.LogWarning("Capture infos array of bytes is null or empty");
            return _captureInfoList;
        }

        protected List<CaptureTransferInfo> FillCaptureInfoList(Dictionary<DestinationController, double> _playerCaptureTimeTable,
            List<CaptureTransferInfo> _captureInfoList)
        {
            var playerList = _playerCaptureTimeTable.ToList();
            if (!ReferenceEquals(playerList, null) && playerList.Count > 0)
            {
                playerList.ForEach(x =>
                {
                    if (!ReferenceEquals(x, null))
                    {
                        var player = x.Key;
                        var gameEntities = TowerEntityController.GameEntityRegister;
                        if (!ReferenceEquals(player, null) &&
                                !ReferenceEquals(gameEntities.FindEntity(typeof(DestinationController), player.networkObject.NetworkId), null))
                        {
                            CaptureTransferInfo newCaptureInfo = new CaptureTransferInfo()
                            {
                                uniqueId = player.networkObject.NetworkId,
                                captureTime = x.Value
                            };
                            _captureInfoList.Add(newCaptureInfo);
                        }
                        else
                        {
                            Debug.LogWarning("Player is null while filling capture infos");
                            CheckForCapture(player, false);
                        }
                    }
                    else Debug.LogWarning("PlayerKeyPair is null while filling capture infos");
                });
            }
            else Debug.LogWarning("Players list is empty while filling capture informations list");

            return _captureInfoList;
        }

        protected Dictionary<string, double> FactionsCapturingCounter(Dictionary<string, double> _factionsTime,
            Dictionary<DestinationController, double> _playerCaptureTimeTable)
        {
            //Set the timer at 0 for each faction
            _factionsTime = InitializeFactionsConquestTime(_factionsTime);
            var players = _playerCaptureTimeTable.Keys.ToList<DestinationController>();
            if (!ReferenceEquals(players, null) && players.Count > 0)
            {
                players.ForEach(player =>
                {
                    if (!ReferenceEquals(player, null))
                    {
                        var playerEntity = player.AvatorController.PlayerEntity;
                        var faction = playerEntity.faction;

                        var gameEntities = TowerEntityController.GameEntityRegister;
                        if (_playerCaptureTimeTable.ContainsKey(player) &&
                                !ReferenceEquals(gameEntities.FindEntity(typeof(DestinationController), player.networkObject.NetworkId), null))
                        {
                            var playerTime = _playerCaptureTimeTable[player];
                            var newTime = _factionsTime[faction] + playerTime;
                            _factionsTime[faction] = newTime;
                        }
                        else {
                            Debug.LogWarning("Players/times table doesn't contain the player");
                            CheckForCapture(player, false);
                        }
                    }
                    else
                        Debug.LogWarning("Found a destination controller null");
                });
            }
            else Debug.LogWarning("Player list is null or empty");

            return _factionsTime;
        }

        protected Dictionary<DestinationController, double> PlayersCapturingCounter(
            Dictionary<DestinationController, double> _playerNetIdNameCaptureTimeTable)
        {
            var playersCapturingTower = _playerNetIdNameCaptureTimeTable.ToList();
            if (!ReferenceEquals(playersCapturingTower, null) && playersCapturingTower.Count > 0)
            {
                playersCapturingTower.ForEach(
                    s =>
                    {
                        if (!ReferenceEquals(s, null))
                        {
                            var player = s.Key;
                            var gameEntities = TowerEntityController.GameEntityRegister;
                            if (!ReferenceEquals(player, null) &&
                                    !ReferenceEquals(gameEntities.FindEntity(typeof(DestinationController), player.networkObject.NetworkId), null))
                            {
                                var playerNetworkkId = s.Key.networkObject.NetworkId;
                                double timePassed = s.Value; //Time passed since the player is stayed in the capture zone
                                double timeAdd = timePassed + Time.deltaTime; //Add time passed since last update 

                                _playerNetIdNameCaptureTimeTable[s.Key] = timeAdd;
                            }
                            else
                            {
                                Debug.LogWarning("Null player in the players/times table");
                                CheckForCapture(player,false);
                            }
                        }
                        else Debug.LogWarning("Null KeyPair in the players/times table");

                    });
            }

            return _playerNetIdNameCaptureTimeTable;
        }

        public void InitTowerCaptureController(TowerEntityController _towerEntityController)
        {
            if (!ReferenceEquals(_towerEntityController, null))
            {
                UpdateTowerCaptureControllerAttributes(_towerEntityController);

                //Destroy this object when the Tower is destroyed
                _towerEntityController.networkObject.onDestroy += NetworkObject_onDestroy;

                networkObject.SendRpc(RPC_UPDATE_CAPTURE_CONTROLLER_ON_NETWORK, Receivers.AllBuffered);
            }
            else Debug.LogWarning("Can't use a null tower entity controller for initializations");

        }

        private void UpdateTowerCaptureControllerAttributes(TowerEntityController _towerEntityController)
        {
            if (!ReferenceEquals(_towerEntityController, null))
            {
                networkObject.TowerEntityNetId = _towerEntityController.networkObject.NetworkId;
                TowerEntityController = _towerEntityController;
                _towerEntityController.TowerCaptureController = this;
                transform.SetParent(TowerEntityController.transform);
            }
            else Debug.LogWarning("Tower entity controller null");

        }

        //Update the capture controller on the network after initialization 
        public override void UpdateCaptureControllerOnNetwork(RpcArgs args)
        {
            /*
             * The tower entity has a connection to the GameRegisterEntity,
             * but for finding the tower entity of this capture controller
             * we must firstly find the GameRegister
             * **/
            var gameRegister = FindObjectOfType<GameEntityRegister>();

            var tower = (TowerEntityController)gameRegister.FindEntity(typeof(TowerEntityController),
                networkObject.TowerEntityNetId);
            UpdateTowerCaptureControllerAttributes(tower);
        }

        //This method checks if the player is capturing the tower and updates this object internal structures
        public void CheckForCapture(DestinationController _playerDestinationComponent, bool _isCapturing)
        {
            if (!ReferenceEquals(_playerDestinationComponent, null))
            {
                var playerNetId = _playerDestinationComponent.networkObject.NetworkId;
                AddOrDeletePlayerToTheCapturing(_playerDestinationComponent, _isCapturing);

                //Keep updated also the network entities of this object
                networkObject.SendRpc(RPC_UPDATE_CAPTURE_ON_NETWORK,
                    Receivers.AllBuffered,
                    playerNetId,
                    _isCapturing);
            }
            else Debug.LogWarning("Can't check the capture status of a null destination controller");

        }

        protected void AddOrDeletePlayerToTheCapturing(DestinationController _playerDestinationComponent,
            bool _isCapturing)
        {
            bool exists;
            if (!ReferenceEquals(_playerDestinationComponent, null))
            {
                if (_isCapturing)
                {
                    exists = PlayersCaptureTimeTable.ContainsKey(_playerDestinationComponent);
                    if (!exists)
                        PlayersCaptureTimeTable.Add(_playerDestinationComponent, 0d);
                }
                else
                {
                    exists = PlayersCaptureTimeTable.ContainsKey(_playerDestinationComponent);
                    if (exists)
                        PlayersCaptureTimeTable.Remove(_playerDestinationComponent);
                }
            }
            else Debug.LogWarning("Can't add or remove a null destination controller");
        }

        /// <summary>
        /// This function is used in order to remove player from PlayersCaptureTimeTable in the case
        /// them have been disconnected or their client crashed while they where in capture zone
        /// of a tower.
        /// </summary>
        /// <param name="networkId">Network id of the player to delete</param>
        /// <returns>true if the player was rimoved,false otherwise</returns>
        protected bool TryRemovePlayer(uint networkId)
        {
            bool removed = false;
            var playerToDeletePair = PlayersCaptureTimeTable.Where(x =>
            {
                var player = x.Key;
                var netId = player.networkObject.NetworkId;
                return netId.Equals(networkId);
            }).
            SingleOrDefault();

            var playerToDelete = playerToDeletePair.Key;
            if (!ReferenceEquals(playerToDelete, null))
            {
                PlayersCaptureTimeTable.Remove(playerToDelete);
                removed = true;
            }
            return removed;
        }

        //Update the list of the players on the network that are capturing the tower
        public override void UpdateCaptureOnNetwork(RpcArgs args)
        {
            var playerNetId = args.GetNext<uint>();
            var isCapturing = args.GetNext<bool>();

            var player = FindPlayerByNetId(playerNetId);
            if (!ReferenceEquals(player, null))
                AddOrDeletePlayerToTheCapturing(player, isCapturing);
            else if(isCapturing==false)//this ensure that the player have been crashed or disconnected in the capture zone
            {
                /*
                 * This is the case that occur when a player for example crash in the
                 * capture zone of the tower, we have to remove manually the player
                 * from the table by networkId (couse if the player doesn't exists
                 * anymore in the scene, FindPlayerByNetId result is null)
                 * **/
                TryRemovePlayer(playerNetId);
            }
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
            return PlayersCaptureTimeTable[_destinationController];
        }

        public double TimeSpentByMyFactionForCapturingTheTower(string _myfaction)
        {
            double timeSpent;
            FactionsConquestTime.TryGetValue(_myfaction, out timeSpent);
            return timeSpent;
        }

        public override void UpdateCapturesTimesOnNetwork(RpcArgs args)
        {
            var bytes = args.GetNext<Byte[]>();

            if (!ReferenceEquals(bytes, null))
            {

                List<CaptureTransferInfo> captureInfos = (List<CaptureTransferInfo>)ByteArrayUtils.ByteArrayToObject(bytes);
                if (!ReferenceEquals(captureInfos, null) && captureInfos.Capacity > 0)
                {
                    //Update the capture time of the players on the CaptureController over the network
                    captureInfos.ForEach(x =>
                    {
                        if (!ReferenceEquals(x, null))
                        {
                            var uniqueId = x.uniqueId;
                            var capturetime = x.captureTime;
                            //Find the player with this networkid
                            var keyValuePlayer = PlayersCaptureTimeTable.FirstOrDefault(y =>
                            {
                                return y.Key.networkObject.NetworkId.Equals(uniqueId);
                            });

                            if (!ReferenceEquals(keyValuePlayer, null))
                            {
                                var player = keyValuePlayer.Key;
                                if (!ReferenceEquals(player, null))
                                {
                                    //Set the new calculated time
                                    PlayersCaptureTimeTable[player] = capturetime;
                                }
                                else Debug.LogWarning("Player with networkId " + uniqueId + " not found");
                            }
                            else Debug.LogWarning("KeyValuePair player is null");
                        }
                        else Debug.LogWarning("Capture null is null");
                    });
                    captureInfos = null; //I hope this helps garbage collection
                                         //Recalculate time for the factions
                    FactionsConquestTime = FactionsCapturingCounter(FactionsConquestTime, PlayersCaptureTimeTable);
                }
                else Debug.LogWarning("Capture infos list is empty or null");
            }
            else Debug.LogWarning("Bytes array is null");
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            PlayerCapturingTowerList.Clear();
            PlayersCaptureTimeTable.Keys.ToList().ForEach(x => PlayerCapturingTowerList.Add(x));
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            Debug.Log("OnAfterDeserialize not implemented for TowerCaptureController");
#endif
        }
    }

}
