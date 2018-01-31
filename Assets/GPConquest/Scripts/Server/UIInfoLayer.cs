using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TC.GPConquest.MarkLight4GPConquest
{
    public static class UIInfoLayer
    {

        #region Error messages
        public static readonly string ServerOptsNullMessage = "Server options can't be null.";
        public static readonly string ConnectionInfoNullMessage = "Connection infomartion are null.";
        #endregion
        #region UI user messages/label
        public static readonly string StartServerLabel = "Start Server";
        public static readonly string ServerStatusLabel = "Server Status";
        public static readonly string UDPString = "UDP";
        public static readonly string TCPString = "TCP";
        public static readonly string[] InternetProtocolsList = { UDPString, TCPString}; 
        #endregion
    }

}