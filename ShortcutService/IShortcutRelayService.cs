using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace ShortcutService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShortcutRelayService" in both code and config file together.
    [ServiceContract]
    public interface IShortcutRelayService
    {
        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetShortcutList")]
        //List<ShortcutData> GetShortcutList();

        //[OperationContract]
        //void ActivateShortcut(String shortcutText);

        [OperationContract]
        [WebInvoke(Method = "SET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddShortCut/{shortcut}/{name}")]
        void AddShortCut(String shortcut, String name);

        [OperationContract]
        string test(string test);
    }
}
