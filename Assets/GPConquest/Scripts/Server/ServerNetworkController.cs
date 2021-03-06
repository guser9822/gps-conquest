﻿using BeardedManStudios.Forge.Networking;
using TC.GPConquest.Server;
using UnityEngine;

namespace TC.GPConquest
{
    public class ServerNetworkController : CustomNetworkController
    {

        protected ServerProcessController ServerProcessController;

        private void Awake()
        {
            ServerProcessController = GetComponent<ServerProcessController>();
        }

        public override void StartCustomNetworkController(ConnectionInfo _connectionInfo)
        {
            base.StartCustomNetworkController(_connectionInfo);
            Host();
        }

        protected void Host()
        {
            NetWorker server;

            if (useTCP)
            {
                server = new TCPServer(64);
                ((TCPServer)server).Connect();
            }
            else
            {
                server = new UDPServer(64);
                ((UDPServer)server).Connect(ConnectionInfo.IpAddress, ushort.Parse(ConnectionInfo.ServerPort));
            }

            server.playerTimeout += (player, sender) =>
            {
                Debug.Log("Player " + player.NetworkId + " timed out");
            };

            Connected(server);
        }

        protected override void Connected(NetWorker networker)
        {
            base.Connected(networker);
            //if (networker is IServer)
            //    NetworkObject.Flush(networker);
            //TDOD : find a better solution in order to spawn the towers when the server is really started
            ServerProcessController.RequestTowersSpawn();
        }

    }

}


