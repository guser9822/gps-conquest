using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using TC.GPConquest.Common;
using System.Linq;
using System;

namespace TC.GPConquest.Server
{
    public class GameFactionsController : GameFactionsNetworkControllerBehavior
    {
        public Dictionary<string, float> FactionsPoints = new Dictionary<string, float>();
        public static readonly float CHECK_WINNER_TIMER = 60 * 60 * 24 * 60 * 30;
        private float WINNER_CHECK_TIME_PASSED;
        private ServerProcessController ServerProcessController;

        private void Awake()
        {
            //Initialize the factions points
            GameCommonNames.FACTIONS.ForEach(x =>
            {
                FactionsPoints.Add(x, 0.0f);
            });
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            WINNER_CHECK_TIME_PASSED += Time.deltaTime;
            if (WINNER_CHECK_TIME_PASSED >= CHECK_WINNER_TIMER)
            {
                WINNER_CHECK_TIME_PASSED = 0;

                //Find the faction with the highest score
                var winningFactionRes = FactionsPoints.
                    OrderByDescending<KeyValuePair<string, float>, string>(x => x.Key).
                    FirstOrDefault();

                //Reset the factions points to 0
                ResetScores();

                var winningFaction = winningFactionRes.Key;
                //Inform the Server Process
                ServerProcessController.DeclareWinnerFaction(winningFaction);
            }
        }

        /// <summary>
        /// This function is used for invoke the RPC used for adding points to a faciton
        /// </summary>
        /// <param name="_factionName">Name of the faction</param>
        /// <param name="_pointsAmount">Amount of the poin</param>
        public void RequestAddingPointsFaction(String _factionName, float _pointsAmount)
        {
            if (!ReferenceEquals(_factionName, null) && _factionName.Length>0 && !ReferenceEquals(_pointsAmount, null))
            {
                networkObject.SendRpc(RPC_ADD_POINTS_TO_FACTION_NETWORK,
                    true,
                    Receivers.AllBuffered,
                    _factionName,
                    _pointsAmount);
            }
            else Debug.LogError("Network arguments are null.");
        }

        /// <summary>
        /// RPC that add an amount of points to a faction
        /// </summary>
        /// <param name="args"></param>
        public override void AddPointsToFactionNetwork(RpcArgs args)
        {
            if (!ReferenceEquals(args, null))
            {
                var factionName = args.GetNext<string>();
                var pointsAmount = args.GetNext<float>();
                if (!ReferenceEquals(factionName, null) && !ReferenceEquals(pointsAmount, null))
                {
                    if (FactionsPoints.ContainsKey(factionName))
                        FactionsPoints[factionName] += pointsAmount;
                    else Debug.LogError("Non corresponding faction to " + factionName);
                }
                else Debug.LogError("Network arguments are null.");                  
            }
            else Debug.LogError("Network parameters are null");
        }

        private void ResetScores()
        {
            FactionsPoints.ToList().ForEach(x =>
            {
                FactionsPoints[x.Key] = 0.0f;
            });
        }

    }
}
