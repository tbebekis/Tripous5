using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk.Models
{
    public class AjaxRequest
    {
        public AjaxRequest()
        {
        }

        public string OperationName { get; set; }
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }


}
