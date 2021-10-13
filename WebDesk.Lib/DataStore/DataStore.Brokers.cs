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
            SelectSql MainSelect = Broker.MainSelect;
            MainSelect.Text = @"
select 
   Trader.Id            as Id
  ,Trader.Code          as Code
  ,Trader.Name          as Name
  ,Trader.Married       as Married
  ,Trader.BirthDate     as BirthDate
  ,Trader.Salary        as Salary
from 
  Trader
";

            MainSelect.AddColumn("Id", "Id").SetVisible(false);
            MainSelect.AddColumn("Code", "Code");
            MainSelect.AddColumn("Name", "Name");
            MainSelect.AddColumn("Married", "Married", ColumnDisplayType.CheckBox);
            MainSelect.AddColumn("BirthDate", "BirthDate", ColumnDisplayType.Date);
            MainSelect.AddColumn("Salary", "Salary").SetDecimals(2); 

            SqlBrokerTableDef Table = Broker.AddTable(Broker.MainTableName, Broker.Title);
            Table.AddId();
            Table.Add("Code", 40, "Code", FieldFlags.Required | FieldFlags.Searchable | FieldFlags.ReadOnlyUI).SetCodeProviderName(CodeProviderDef.Simple6_3);
            Table.Add("Name", 96, "Trader", FieldFlags.Required | FieldFlags.Searchable);
            Table.AddDate("BirthDate", "BirthDate", FieldFlags.Searchable);
            Table.AddDecimal("Salary", 2, "Salary", FieldFlags.Required | FieldFlags.Searchable);
            Table.AddTextBlob("Notes");
            Table.AddBoolean("Married", "Married", FieldFlags.Required | FieldFlags.Searchable);

        }

        static void RegisterBrokers()
        {
            RegisterBroker_Trader();
        }
    }
}
