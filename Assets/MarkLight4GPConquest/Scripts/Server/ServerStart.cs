using MarkLight.Views.UI;
using MarkLight;
using System;
using TC.GPConquest.Server;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class ServerStart : UIView
    {
        public _string ServerStatus;
        public GenericPopUp GenericPopUp;
        public ServerOptions ServerOptions;
        private ConnectionInfo ConnectionInfo; 

        public void Awake()
        {
            /* We add this components here because when you update the view,
            all references setted by the inspector are lost */
            ConnectionInfo = gameObject.AddComponent<ConnectionInfo>();
        }

        public void StartServer()
        {
            if (ServerOptions != null)
            {
               ConnectionInfo.SetConnectionInfo(ServerOptions.IpAddress,
                    ServerOptions.ServerPort,
                    (string)ServerOptions.XProtocol.Value);

                FindMultiplayerController().RequestServerConnection(ConnectionInfo);
            }
            else
            {
                GenericPopUp.GenericPopUpMessage.Value = UIInfoLayer.ServerOptsNullMessage;
                GenericPopUp.ToggleWindow();
            }
        }

        public void StopServer()
        {
            FindMultiplayerController().RequestServerDisconnection();
        }

        /** :::: NOTE ::::
         * The reference to an object is continously lost due, I believe, the refresh  meccanism  on the views of 
         * MarkLightUI; so, whenever we will need inside our UView of a particular object active in the scene it 
         * will need to be tagged first(not necessary but It's better), in order to find that specific object and then 
         * 'find' everytime.
        */
        private ServerProcessController FindMultiplayerController()
        {
            return Array.Find<ServerProcessController>(
                    FindObjectsOfType<ServerProcessController>(),
                        s => s.gameObject.tag.Equals("ServerController") &&
                        s.gameObject.GetComponent<ServerProcessController>()
                    );
        }

    }

}
