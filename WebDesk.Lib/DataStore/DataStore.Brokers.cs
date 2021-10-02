using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Tripous;
using Tripous.Data;

using WebLib.Models;

namespace WebLib
{
    static public partial class DataStore
    {
        static void RegisterBroker_Trader()
        { 
            SqlBrokerDef Broker = SqlBrokerDef.Register("Trader");
            //Broker.CodeProducerName = SysCodeProducers.Simple6_3;
            Broker.MainSelect.Text = @"
select 
   Trader.Id            as Id
  ,Trader.Code          as Code
  ,Trader.Name          as Name
from 
  Trader
";

            NameValueStringList DisplayLabels = new NameValueStringList();
            DisplayLabels.Clear();
            DisplayLabels.Values["Code"] = "Code";
            DisplayLabels.Values["Name"] = "Name";
            Broker.MainSelect.DisplayLabels = DisplayLabels.Text;

            SqlBrokerTableDef Table = Broker.AddTable(Broker.MainTableName, Broker.Title);
            Table.AddId();
            Table.Add("Code", 40, "Code", FieldFlags.Required | FieldFlags.Searchable | FieldFlags.ReadOnlyUI).SetCodeProviderName(CodeProviderDef.Simple6_3);
            Table.Add("Name", 96, "Trader", FieldFlags.Required | FieldFlags.Searchable);
 
        }

        static void RegisterBrokers()
        {
            RegisterBroker_Trader();
        }
    }
}
