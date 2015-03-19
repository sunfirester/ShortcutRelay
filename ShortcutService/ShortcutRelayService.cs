using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WindowsInput;
using System.ServiceModel.Web;


namespace ShortcutService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShortcutRelayService" in both code and config file together.
    public class ShortcutRelayService : IShortcutRelayService
    {
        public List<ShortcutData> shortcutList = new List<ShortcutData>();

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetShortcutList")]
        public List<ShortcutData> GetShortcutList()
        {
            if (shortcutList.Count > 0)
                return shortcutList;
            else
                return null;
        }

        [WebInvoke(Method = "SET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddShortCut/{shortcut}/{name}")]
        public void AddShortCut(String shortcut, String name)
        {
            shortcutList.Add(new ShortcutData(shortcut, name));
        }

        public string test(string test)
        {
            return test;
        }
        public void ActivateShortcut(String shortcutText)
        {
            String[] segmentArray = shortcutText.Split('+');
            List<VirtualKeyCode> keyCodes = new List<VirtualKeyCode>();
            for (int i = 0; i < segmentArray.Length; i++)
            {
                VirtualKeyCode keyCode = (VirtualKeyCode)System.Enum.Parse(typeof(VirtualKeyCode), segmentArray[i].Replace(" ", String.Empty));
                keyCodes.Add(keyCode);
                //System.Windows.Forms.MessageBox.Show(Convert.ChangeType(keyCode, keyCode.GetTypeCode()).ToString());
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
    }
    public class ShortcutData
    {
        public String shortcut { get; set; }
        public String name { get; set; }

        public ShortcutData(String _shortcut, String _name)
        {
            this.shortcut = _shortcut;
            this.name = _name;
        }
    }
}
