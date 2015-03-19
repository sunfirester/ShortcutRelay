using ShortcutRelayService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using WindowsInput;
//using System.ServiceModel.Discovery;

namespace ShortcutRelay
{
    class RelayServiceWrapper
    {
        public ServiceHost host { get; set; }
        public IService1 client { get; set; }
        public List<ShortcutData> shortcutList { get; set; }

        public RelayServiceWrapper()
        {
            shortcutList = new List<ShortcutData>();
            host = new ServiceHost(typeof(ShortcutRelayService.Service1));
            //host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            //serviceHost.AddServiceEndpoint(new UdpDiscoveryEndpoint());

        }

        public void createServiceClient()
        {
            var myBinding = new BasicHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8734/ShortcutRelay/ShortcutService");
            var myChannelFactory = new ChannelFactory<IService1>(myBinding, myEndpoint);

            try
            {
                client = myChannelFactory.CreateChannel();
                //client.AddShortCut("CONTROL + MENU + F11", "MUSIC");
                //client.ActivateShortcutByName("NAME");
                //client.ActivateShortcutByID(0);
                //((ICommunicationObject)client).Close();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                if (client != null)
                {
                    ((ICommunicationObject)client).Abort();
                }
            }
        }

        public void activateShortcut(String shortText)
        {
            String[] segmentArray = shortText.Split('+');
            List<VirtualKeyCode> keyCodes = new List<VirtualKeyCode>();
            for (int i = 0; i < segmentArray.Length; i++)
            {
                VirtualKeyCode keyCode = (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), segmentArray[i].Replace(" ", String.Empty));
                keyCodes.Add(keyCode);
            }
            if (segmentArray.Length > 1)
            {
                List<VirtualKeyCode> Modifiers = new List<VirtualKeyCode>(keyCodes);
                VirtualKeyCode lastKey = Modifiers[Modifiers.Count - 1];
                Modifiers.RemoveAt(Modifiers.Count - 1);
                //System.Windows.Forms.MessageBox.Show(Modifiers.ToArray().Length.ToString());

                InputSimulator.SimulateModifiedKeyStroke(Modifiers.ToArray(), lastKey);
            }
            else
            {
                InputSimulator.SimulateKeyPress(keyCodes[0]);
            }

        }

        public void recreateService()
        {
            host = new ServiceHost(typeof(ShortcutRelayService.Service1));
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //smb.HttpGetEnabled = true;
            //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            //host.Description.Behaviors.Add(smb);
            //host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
        }

        public void startService()
        {
            if (host.State == CommunicationState.Opened)
                return;
            try
            {
                host.Open();
            }
            catch
            {
                recreateService();
                host.Open();
            }
            createServiceClient();
        }

        public void stopService()
        {
            if (host.State == CommunicationState.Closed)
                return;
            host.Close();
        }

        public void loadShortcutList()
        {
            shortcutList = client.GetShortcutList();
        }

        public void addShortcutToSerivce(string shortcut, string name)
        {
            client.AddShortCut(shortcut, name);
        }

        public void ActivateShortcut(string shortString)
        {
            client.ActivateShortcut(shortString);
        }

        public void DeleteShortcut(String shortcutText)
        {
            client.DeleteShortcut(shortcutText);
        }

        public void DeleteShortcutByID(int ID)
        {
            client.DeleteShortcutByID(ID);
        }

        public void DeleteShortcutByName(string _name)
        {
            client.DeleteShortcutByName(_name);
        }

        public string LocalIPAddress()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                return localIP;
            }
            else
                return "Network Unavailable";
        }
    }
}
