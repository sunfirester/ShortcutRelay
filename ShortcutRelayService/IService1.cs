using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;


namespace ShortcutRelayService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet]
        string GetData(string value);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetShortcutList")]
        List<ShortcutData> GetShortcutList();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ActivateShortcut/{shortcut}")]
        void ActivateShortcut(String shortcutText);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ActivateShortcutByID/{ID}")]
        void ActivateShortcutByID(int shortcutID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ActivateShortcutByName/{name}")]
        void ActivateShortcutByName(string shortcutName);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteShortcut/{shortcut}")]
        void DeleteShortcut(String shortcutText);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteShortcutByID/{ID}")]
        void DeleteShortcutByID(int shortcutID);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "DeleteShortcutByName/{name}")]
        void DeleteShortcutByName(string shortcutName);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "AddShortCut/{shortcut}/{name}")]
        void AddShortCut(String shortcut, String name);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "ShortcutRelayService.ContractType".
    [DataContract]
    public class ShortcutData
    {
        [DataMember]
        public String shortcut { get; set; }
        [DataMember]
        public String name { get; set; }

        public ShortcutData(String _shortcut, String _name)
        {
            this.shortcut = _shortcut;
            this.name = _name;
        }
    }
}
