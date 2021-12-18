using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace WebLib 
{
    public class AjaxPacket
    {
        Dictionary<string, object> Properties   = new Dictionary<string, object>();

        public AjaxPacket(string OperationName)
        {
            this.OperationName = OperationName;
        }

        public object GetPacketObject()
        {
            JObject Result = new JObject();
            Result["OperationName"] = OperationName;

            if (Properties != null && Properties.Count > 0)
            {
                foreach (var Entry in Properties)
                    Result[Entry.Key] = JToken.FromObject(Entry.Value);
            }

            return Result;
        }
        
        public bool ContainsKey(string Key)
        {
            return Properties.ContainsKey(Key);
        }

        public string OperationName { get; }
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key)? Properties[Key]: null; }
            set { Properties[Key] = value; }
        }


    }
}
