using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest {

    public class ConnectionInfo : MonoBehaviour
    {

        public string IpAddress;
        public string ServerPort;
        public string InternetProtocol;

        public void SetConnectionInfo(string _ip = "127.0.0.1", string _port = "15937", string _protocol = "UDP")
        {
            IpAddress = _ip;
            ServerPort = _port;
            InternetProtocol = _protocol;
        }
    }

}