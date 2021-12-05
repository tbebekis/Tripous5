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

            FieldDef = Def.Add("Id", "Id").SetListVisible(false);
            FieldDef = Def.Add("Code", "Code").SetVisible(true);
            FieldDef = Def.Add("Name", "Name").SetVisible(true);
 
        }

        static void RegisterLocators()
        {
            RegisterLocator_Trader();
        }
    }
}
