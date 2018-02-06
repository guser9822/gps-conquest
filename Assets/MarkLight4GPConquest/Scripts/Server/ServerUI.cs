using MarkLight.Views.UI;

using UnityEngine;

namespace TC.GPConquest.MarkLight4GPConquest {

    public class ServerUI : UIView{

        public ViewSwitcher ContentViewSwitcher;
        public ServerStart ServerStart;
        public ServerOptions ServerOptions;
        public Button StartServerButton;
        public Button ServerOptionsButton;
        public Button ServerDisconnectButton;

        public override void Initialize()
        {
            base.Initialize();
            //Sets references
            ServerStart.ServerOptions = ServerOptions;
            ServerDisconnectButton.IsVisible.Value = false;
        }

        private void Start()
        {
            //When ServerUI starts(ServerOptions is already initialized), uses default settings for the server
            ServerOptions.SetServerOptions();
        }

        public void StartServerActivation()
        {
            string label = StartServerButton.Text;
            if (label.Equals(UIInfoLayer.StartServerLabel))
            {
                ToggleUIElements();
                ContentViewSwitcher.SwitchTo(1);
                ServerStart.StartServer();
            }
            else if(label.Equals(StartServerButton.Text.Value = UIInfoLayer.ServerStatusLabel))
            {
                ContentViewSwitcher.SwitchTo(1);
            }
        }

        public void CallServerOptions()
        {
            ContentViewSwitcher.SwitchTo(3);
        }

        public void CallServerDisconnect()
        {
            ToggleUIElements();
            ServerStart.StopServer();
            ContentViewSwitcher.SwitchTo(0);
        }

        public void CallCloseServer()
        {
            ContentViewSwitcher.SwitchTo(0);
            Application.Quit();
        }

        protected void ToggleUIElements()
        {
            StartServerButton.Text.Value = ToggleLabelStart();
            ServerOptionsButton.IsVisible.Value = ServerOptionsButton.IsVisible.Value ? false : true;
            ServerDisconnectButton.IsVisible.Value = ServerDisconnectButton.IsVisible.Value ? false : true;
        }

        private string ToggleLabelStart()
        {
            return StartServerButton.Text.Value.Equals(UIInfoLayer.ServerStatusLabel) ? 
                UIInfoLayer.StartServerLabel : UIInfoLayer.ServerStatusLabel;
        }

  }

}
