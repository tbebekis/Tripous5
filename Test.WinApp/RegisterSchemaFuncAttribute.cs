using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.WinApp
{
    [AttributeUsageAttribute(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class RegisterSchemaFuncAttribute: Attribute
    {
        public RegisterSchemaFuncAttribute(int Version)
        {
        }

        public int Version { get; set; } = 1;
    }
}
