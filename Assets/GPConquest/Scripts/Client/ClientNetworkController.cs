using BeardedManStudios.Forge.Networking;
using TC.GPConquest.MarkLight4GPConquest;
using UnityEngine;

namespace TC.GPConquest
{
    public class ClientNetworkController : CustomNetworkController
    {
        public override void StartCustomNetworkController(ConnectionInfo _connectionInfo)
        {
            base.StartCustomNetworkController(_connectionInfo);
            Connect();
        }

        public void Connect()
        {
            ushort port;
            if (!ushort.TryParse(ConnectionInfo.ServerPort, out port))
            {
                Debug.LogError("The supplied port number is not within the allowed range 0-" + ushort.MaxValue);
                return;
            }

            NetWorker client;

            if (useTCP)
            {
                client = new TCPClient();
                ((TCPClient)client).Connect(ConnectionInfo.IpAddress, (ushort)port);
            }
            else
            {
                client = new UDPClient();
                ((UDPClient)client).Connect(ConnectionInfo.IpAddress, (ushort)port);
            }

            Connected(client);
        }

    }
}