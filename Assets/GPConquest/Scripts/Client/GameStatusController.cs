using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;


namespace TC.GPConquest {

    public class GameStatusController : MonoBehaviour
    {
        public ClientNetworkController ClientNetworkController;
        public ConnectionInfo ConnectionInfo;

        private void Awake()
        {
            ConnectionInfo.SetConnectionInfo();
            ClientNetworkController.StartCustomNetworkController(ConnectionInfo);
        }

        private void Start()
        {
            //Instantiate Player
            NetworkManager.Instance.InstantiatePlayerDestinationController(0);
        }
    }

}
