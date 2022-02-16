using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Tripous;
using Tripous.Data;


namespace WebLib
{
    static public partial class DataStore
    {
        static internal class CacheKeys
        {
            public const string Settings = "Settings";
            public const string LanguageList = "LanguageList";
            public const string LanguageResourceStrings = "LanguageResourceStrings";
        }
    }
}
