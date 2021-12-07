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
        static void RegisterLocator_Trader()
        {
            LocatorDef Def = LocatorDef.Register("Trader");
            Def.SqlText = @"
select
    Id
    ,Code
    ,Name
from
    Trader
";
            Def.TitleKey = Def.Name;
            Def.ListTableName = "Trader";
            Def.ListKeyField = "Id";
            Def.ReadOnly = false;

            LocatorFieldDef FieldDef;

            FieldDef = Def.Add("Id", "TraderId", "Id",  DataFieldType.String, false, false, true, "");
            FieldDef = Def.Add("Code", "Trader__Code", "Code", DataFieldType.String, true, true, true, "");
            FieldDef = Def.Add("Name", "Trader__Name", "Name", DataFieldType.String, true, true, true, "");

        }

        static void RegisterLocators()
        {
            RegisterLocator_Trader();
        }
    }
}
