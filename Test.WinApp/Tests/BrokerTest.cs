using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using Tripous;
using Tripous.Data;

namespace Test.WinApp
{
    static public class BrokerTest
    {
        [RegisterSchemaFunc(Version: 2)]
        static void RegisterSchema(Schema Schema, SchemaVersion Version)
        {
            DataTableDef Table;

            string SqlText;

            Table = new DataTableDef() { Name = "Customer" };
            Table.AddId();
            Table.AddString("Name", 96, true);

            SqlText = Table.GetDefText();
            Version.AddTable(SqlText);

            Table = new DataTableDef() { Name = "Address" };
            Table.AddId();
            Table.AddString("CustomerId", 40, true).SetForeign("Customer", "Id");
            Table.AddString("StreetAddress", 96, true);
            Table.AddString("City", 96, false);

            SqlText = Table.GetDefText();
            Version.AddTable(SqlText);
        }

        [RegisterBrokersFunc]
        static void RegisterBrokers()
        {
            SqlBrokerDef BrokerDef;
            SqlBrokerTableDef TableDef;
            SqlBrokerFieldDef FieldDef;

            BrokerDef = SqlBrokerDef.Register("Customer");
            BrokerDef.MainTableName = "Customer";
            BrokerDef.LinesTableName = "Address";

            TableDef = BrokerDef.AddTable("Customer");
            FieldDef = TableDef.AddId();
            FieldDef = TableDef.AddString("Name", 96);

            TableDef = BrokerDef.AddTable("Address").SetMaster("Customer", "Id", "CustomerId"); 
            FieldDef = TableDef.AddId();
            FieldDef = TableDef.AddId("CustomerId");
            FieldDef = TableDef.AddString("StreetAddress", 96, "", FieldFlags.Required);
            FieldDef = TableDef.AddString("City", 96, "", FieldFlags.Required);
        }

        static public void Test1()
        {
            SqlBroker Broker = SqlBroker.Create("Customer", true, false);
            
            Broker.Insert();
            DataRow Row = Broker.Row;
            string Id = Row.AsString("Id");
            Row["Name"] = "Θόδωρος";

            Row = Broker.tblLines.AddNewRow();
            Row["CustomerId"] = Id;
            Row["StreetAddress"] = "Ανδριανουπόλεως 3";
            Row["City"] = "Θεσσαλονίκη";

            Broker.Commit(true); 
          
        }
    }
}
