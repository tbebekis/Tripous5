namespace Tripous.Data
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
        public JsonDataSet(DataSet Source, SqlBrokerDef Descriptor)
        {
            JsonDataTable Table;
            SqlBrokerTableDef TableDes;

            this.Name = Source.DataSetName;
            foreach (DataTable SourceTable in Source.Tables)
            {
                TableDes = null;
                if (Descriptor != null)
                {
                    TableDes = Descriptor.Tables.Find(item => item.Alias.IsSameText(SourceTable.TableName));  
                    if (TableDes == null)
                        TableDes = Descriptor.Tables.Find(item => item.Name.IsSameText(SourceTable.TableName));
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
        /// Converts a DataSet to JsonObject
        /// </summary>
        static public JsonObject ToJObject(DataSet Source, SqlBrokerDef Descriptor)
        {
            JsonDataSet Instance = new JsonDataSet(Source, Descriptor);
            return Instance.ToJObject();
        }
        /// <summary>
        /// Converts a DataSet to JsonObject
        /// </summary>
        static public JsonObject ToJObject(DataSet Source)
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
            JsonObject JO = JsonNode.Parse(JsonText) as JsonObject;
            FromJObject(JO, Target);
        }
        /// <summary>
        /// JsonObject to JsonDataTable
        /// </summary>
        static public JsonDataSet FromJObject(JsonObject JO)
        {
            JsonDataSet Target = new JsonDataSet();
            FromJObject(JO, Target);
            return Target;
        }
        /// <summary>
        /// JsonObject to JsonDataTable
        /// </summary>
        static public void FromJObject(JsonObject JO, JsonDataSet Target)
        {

            Target.Clear();

            dynamic jDS = JO as dynamic;

            // properties
            Target.Name = jDS.Name;

            // Tables
            JsonDataTable Table;
            JsonObject JO2;
            dynamic jTables = jDS.Tables;
            foreach (dynamic C in jTables)
            {
                JO2 = C as JsonObject;
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
        /// Converts this instance to JsonObject
        /// </summary>
        public JsonObject ToJObject()
        {
            string JsonText = Json.Serialize(this);
            return JsonNode.Parse(JsonText) as JsonObject;
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
