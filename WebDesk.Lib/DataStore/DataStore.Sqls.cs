using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;

namespace WebDesk
{
    static public partial class DataStore
    {
        static readonly string SSelectLanguages = $@"select * from {SysTables.Lang}";
    }
}
