using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLib
{
    public class AjaxViewInfo
    {
        public AjaxViewInfo()
        {
        }

        public string RazorViewNameOrPath { get; set; }
        public object Model { get; set; }
        public Dictionary<string, object> ViewData { get; } = new Dictionary<string, object>();
    }
}
