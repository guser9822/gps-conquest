using MarkLight.Views.UI;
using MarkLight;

namespace TC.GPConquest.MarkLight4GPConquest
{

    public class ServerOptions : UIView
    {
        public ObservableList<string> InternetProtocols;
        public ComboBox ComboBoxProtocols;
        public InputField InputIp;
        public InputField InputPort;
        [MapTo("InputIp.Text")]
        public _string IpAddress;
        [MapTo("InputPort.Text")]
        public _string ServerPort;
        [MapTo("ComboBoxProtocols.SelectedItem")]
        public _object XProtocol;

        public override void Initialize()
        {
            base.Initialize();
            // initialize and populate ip list
            InternetProtocols = InitializeIPList(UIInfoLayer.InternetProtocolsList);
        }

        private ObservableList<string> InitializeIPList(params string[] ips)
        {
            InternetProtocols = new ObservableList<string>();
            foreach (string ip in ips) {
                InternetProtocols.Add(ip);
            }
            return InternetProtocols;
        }

        //_protocol = 0 = UDP, _protocol = 1 = TCP
        public void SetServerOptions(string _ip = "127.0.0.1", string _port = "15937",int _protocol = 0)
        {
            //Sets up a local UDP server as default
            IpAddress.Value = _ip;
            ServerPort.Value = _port;
            //Sets UDP as default internet protocol
            InternetProtocols.SelectedIndex = _protocol;//sets the list on UDP
            XProtocol = ComboBoxProtocols.SelectedItem;
         }
    }

}
