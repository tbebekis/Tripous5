﻿namespace WebLib
{
    static public partial class DataStore
    {
        static void RegisterBroker_Trader()
        { 
            SqlBrokerDef Broker = SqlBrokerDef.Register("Trader");
            Broker.TitleKey = "Traders";
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
 
            MainSelect.Filters.Add("Trader.Name", "Trader", DataFieldType.String);
            MainSelect.Filters.Add("Trader.Salary", "Salary", DataFieldType.Decimal).SetUseRange(true);
            MainSelect.Filters.Add("Trader.BirthDate", "BirthDate", DataFieldType.Date);
            MainSelect.Filters.Add("Trader.Married", "Married", DataFieldType.Boolean);



            /*
            SqlFilterDef FilterDef;

            FilterDef = MainSelect.Filters.Add("Trader.X1", "X1", DataFieldType.String, SqlFilterMode.EnumQuery);
            FilterDef.Enum.ResultField = "Id";
            FilterDef.Enum.Sql = "select * from Trader";
            FilterDef.Enum.IsMultiChoise = false;

            FilterDef = MainSelect.Filters.Add("Trader.X2", "X2", DataFieldType.String, SqlFilterMode.EnumConst);
            FilterDef.Enum.ResultField = "Id";
            FilterDef.Enum.IsMultiChoise = true;
            FilterDef.Enum.IncludeAll = true;
            FilterDef.Enum.OptionList.AddRange(new string[] { "one", "two", "three" });
            */
 

            MainSelect.Filters.CheckDescriptors();

            SqlBrokerTableDef Table = Broker.AddTable(Broker.MainTableName, Broker.Title);
            Table.AddId();
            Table.Add("Code", 40, "Code", FieldFlags.Required | FieldFlags.Searchable | FieldFlags.ReadOnlyUI).SetCodeProviderName(CodeProviderDef.Simple6_3);
            Table.Add("Name", 96, "Trader", FieldFlags.Required | FieldFlags.Searchable);
            Table.AddDate("BirthDate", "BirthDate", FieldFlags.Searchable);
            Table.AddDecimal("Salary", 2, "Salary", FieldFlags.Required | FieldFlags.Searchable).SetDefaultValue("0");
            Table.AddTextBlob("Notes");
            Table.AddBoolean("Married", "Married", FieldFlags.Required | FieldFlags.Searchable).SetDefaultValue("0");

            //string JsonText = Json.Serialize(Broker, true);

        }

        static void RegisterBrokers()
        {
            RegisterBroker_Trader();
        }
    }
}
