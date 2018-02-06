using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using TC.GPConquest.MarkLight4GPConquest;
using UnityEngine;

namespace TC.GPConquest {

    public class CustomNetworkController : MonoBehaviour
    {
        protected ConnectionInfo ConnectionInfo;
        protected bool useTCP;
        protected NetworkManager mgr;
        public GameObject networkManager;

        private void InitMultiplayerBaseOptions(ConnectionInfo _connectionInfo)
        {
            if (_connectionInfo != null)
            {
                ConnectionInfo = _connectionInfo;
                useTCP = ConnectionInfo.InternetProtocol.Equals(UIInfoLayer.TCPString) ? true : false;
            }
            else throw new System.ArgumentNullException(UIInfoLayer.ConnectionInfoNullMessage);
        }

        protected void InitMultiplayerBase(ConnectionInfo _connectionInfo)
        {
            InitMultiplayerBaseOptions(_connectionInfo);
            if (!useTCP)// Do any firewall opening requests on the operating system
                NetWorker.PingForFirewall(ushort.Parse(ConnectionInfo.ServerPort));

            Rpc.MainThreadRunner = MainThreadManager.Instance;
        }

        public virtual void StartCustomNetworkController(ConnectionInfo _connectionInfo)
        {
            InitMultiplayerBase(_connectionInfo);
        }

        protected virtual void Connected(NetWorker networker)
        {
            if (!networker.IsBound)
            {
                Debug.LogError("NetWorker failed to bind");
                return;
            }

            if (mgr == null && networkManager == null)
            {
                Debug.LogWarning("A network manager was not provided, generating a new one instead");
                networkManager = new GameObject("Network Manager");
                mgr = networkManager.AddComponent<NetworkManager>();
            }
            else mgr = Instantiate(networkManager).GetComponent<NetworkManager>();

            mgr.Initialize(networker);

            //if (networker is IServer)
            //    NetworkObject.Flush(networker);

        }

        public void CloseMultiplayerBase()
        {
            mgr.Disconnect();
            Destroy(mgr.gameObject);
        }

    }

}

