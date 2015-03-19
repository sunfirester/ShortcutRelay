using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using WindowsInput;
using System.IO;

namespace ShortcutRelayService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false, AddressFilterMode = AddressFilterMode.Any)]
    public class Service1 : IService1
    {
        List<ShortcutData> shortcutList = new List<ShortcutData>();
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public List<ShortcutData> GetShortcutList()
        {
            if (shortcutList.Count > 0)
                return shortcutList;
            else
                return null;
        }

        public void AddShortCut(String shortcut, String name)
        {
            shortcutList.Add(new ShortcutData(shortcut, name));
        }

        public void ActivateShortcut(String shortcutText)
        {
            doActivate(shortcutText);
        }

        public void ActivateShortcutByID(int ID)
        {
            doActivate(shortcutList[ID].shortcut);
        }

        public void ActivateShortcutByName(string _name)
        {
            for (int i = 0; i < shortcutList.Count; i++)
            {
                if (shortcutList[i].name == _name)
                {
                    doDelete(i);
                    return;
                }
            }
        }

        public void DeleteShortcut(String shortcutText)
        {
            for (int i = 0; i < shortcutList.Count; i++)
            {
                if (shortcutList[i].shortcut == shortcutText)
                {
                    doDelete(i);
                    return;
                }
            }
        }

        public void DeleteShortcutByID(int ID)
        {
            doDelete(ID);
        }

        public void DeleteShortcutByName(string _name)
        {
            foreach (ShortcutData data in shortcutList)
            {
                if (data.name == _name)
                {
                    doActivate(data.shortcut);
                    return;
                }
            }
        }

        public void doDelete(int shortcutIndex)
        {
            shortcutList.RemoveAt(shortcutIndex);
        }

        public void doActivate(String shortcutText)
        {
            String[] segmentArray = shortcutText.Split('+');
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
                InputSimulator.SimulateModifiedKeyStroke(Modifiers.ToArray(), lastKey);
            }
            else
            {
                InputSimulator.SimulateKeyPress(keyCodes[0]);
            }
        }
    }
}
