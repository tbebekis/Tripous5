/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous.Data;

namespace Tripous.Model
{
    /// <summary>
    /// JsonDataSet
    /// </summary>
    public class JsonDataSet
    {       

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataSet()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataSet(DataSet Source, BrokerDescriptor Descriptor)
        {
            JsonDataTable Table;
            TableDescriptor TableDes;

            this.Name = Source.DataSetName;
            foreach (DataTable SourceTable in Source.Tables)
            {
                TableDes = null;
                if (Descriptor != null)
                {
                    TableDes = Descriptor.Tables.FindByAlias(SourceTable.TableName);
                    if (TableDes == null)
                        TableDes = Descriptor.Tables.Find(SourceTable.TableName);
                }

                Table = new JsonDataTable(SourceTable, TableDes);
                Tables.Add(Table);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataSet(DataSet Source)
        {
            JsonDataTable Table;


            this.Name = Source.DataSetName;
            foreach (DataTable SourceTable in Source.Tables)
            {
                Table = new JsonDataTable(SourceTable);
                Tables.Add(Table);
            }
        }

        /* static */
        /// <summary>
        /// Converts a DataSet to JObject
        /// </summary>
        static public JObject ToJObject(DataSet Source, BrokerDescriptor Descriptor)
        {
            JsonDataSet Instance = new JsonDataSet(Source, Descriptor);
            return Instance.ToJObject();
        }
        /// <summary>
        /// Converts a DataSet to JObject
        /// </summary>
        static public JObject ToJObject(DataSet Source)
        {
            JsonDataSet Instance = new JsonDataSet(Source);
            return Instance.ToJObject();
        }
        /// <summary>
        /// Json text to JsonDataTable
        /// </summary>
        static public JsonDataSet FromJson(string JsonText)
        {
            JsonDataSet Target = new JsonDataSet();
            FromJson(JsonText, Target);
            return Target;
        }
        /// <summary>
        /// Json text to JsonDataTable
        /// </summary>
        static public void FromJson(string JsonText, JsonDataSet Target)
        {
            JObject JO = JObject.Parse(JsonText);
            FromJObject(JO, Target);
        }
        /// <summary>
        /// JObject to JsonDataTable
        /// </summary>
        static public JsonDataSet FromJObject(JObject JO)
        {
            JsonDataSet Target = new JsonDataSet();
            FromJObject(JO, Target);
            return Target;
        }
        /// <summary>
        /// JObject to JsonDataTable
        /// </summary>
        static public void FromJObject(JObject JO, JsonDataSet Target)
        {

            Target.Clear();

            dynamic jDS = JO as dynamic;

            // properties
            Target.Name = jDS.Name;

            // Tables
            JsonDataTable Table;
            JObject JO2;
            dynamic jTables = jDS.Tables;
            foreach (dynamic C in jTables)
            {
                JO2 = C as JObject;
                Table = new JsonDataTable(JO2);
                Target.Tables.Add(Table);
            }


        }


        /* public */
        /// <summary>
        /// Clears this instance from all data in properties and lists
        /// </summary>
        public void Clear()
        {
            Name = string.Empty;

            Tables.Clear();
        }
        /// <summary>
        /// Converts this instance to JObject
        /// </summary>
        public JObject ToJObject()
        {
            return JObject.FromObject(this);
        }
        /// <summary>
        /// Converts this to DataSet
        /// </summary>
        public DataSet ToDataSet()
        {
            DataSet ds = new DataSet();
            this.ToDataSet(ds);
            return ds;
        }
        /// <summary>
        /// Converts this to DataSet
        /// </summary>
        public void ToDataSet(DataSet Dest)
        {
            Dest.DataSetName = this.Name;

            // add the tables first
            foreach (JsonDataTable SourceTable in this.Tables)
                Dest.Tables.Add(SourceTable.ToTable());

            // then resolve detail tables
            MemTable Table;
            foreach (JsonDataTable SourceTable in this.Tables)
            {
                if (Dest.Tables.Contains(SourceTable.Name))
                {
                    Table = Dest.Tables[SourceTable.Name] as MemTable;
                    foreach (string TableName in SourceTable.Details)
                        Table.Details.Add(Dest.Tables[TableName] as MemTable);
                }
                
            }
        }

        /* properties */
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tables
        /// </summary>
        public List<JsonDataTable> Tables { get; set; } = new List<JsonDataTable>();
 
    }
}
