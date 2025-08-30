namespace Tripous.Data
{


    /// <summary>
    /// JsonDataTable
    /// </summary>
    public class JsonDataTable
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataTable()
        { 
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataTable(DataTable Source, SqlBrokerTableDef Descriptor)
        {
            this.Name = Source.TableName;

            if (Source is Tripous.Data.MemTable)
            {                
                Tripous.Data.MemTable T = Source as Tripous.Data.MemTable;
                this.PrimaryKeyField = T.PrimaryKeyField;
                this.MasterKeyField = T.MasterKeyField;
                this.DetailKeyField = T.DetailKeyField;
                this.MasterTableName = T.MasterTable != null ? T.MasterTable.TableName : "";
                this.AutoGenerateGuidKeys = T.AutoGenerateGuidKeys;

                foreach (MemTable DetailTable in T.Details)
                {
                    this.Details.Add(DetailTable.TableName);
                }

                foreach (MemTable StockTable in T.StockTables)
                {
                    this.StockTables.Add(StockTable.TableName);
                }
            }

            // columns
            SqlBrokerFieldDef FieldDescriptor;
            JsonDataColumn Column;
            foreach (DataColumn SourceColumn in Source.Columns)
            {
                FieldDescriptor = null;
                if (Descriptor != null)
                {
                    Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> Pair = Descriptor.FindAnyField(SourceColumn.ColumnName);
                    FieldDescriptor = Pair != null ? Pair.Item2 : null;
                }

                Column = new JsonDataColumn(SourceColumn, FieldDescriptor);
                Columns.Add(Column);
            }

            // rows
            JsonDataRow Row;
            foreach (DataRow SourceRow in Source.Rows)
            {
                Row = new JsonDataRow(SourceRow);
                Rows.Add(Row);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataTable(DataTable Source)
        {
            this.Name = Source.TableName;

            if (Source is Tripous.Data.MemTable)
            {
                Tripous.Data.MemTable T = Source as Tripous.Data.MemTable;
                this.PrimaryKeyField = T.PrimaryKeyField;
                this.MasterKeyField = T.MasterKeyField;
                this.DetailKeyField = T.DetailKeyField;
                this.MasterTableName = T.MasterTable != null ? T.MasterTable.TableName : "";
                this.AutoGenerateGuidKeys = T.AutoGenerateGuidKeys;

                foreach (MemTable DetailTable in T.Details)
                {
                    this.Details.Add(DetailTable.TableName);
                }

                foreach (MemTable StockTable in T.StockTables)
                {
                    this.StockTables.Add(StockTable.TableName);
                }
            }

            // columns 
            JsonDataColumn Column;
            foreach (DataColumn SourceColumn in Source.Columns)
            {
                Column = new JsonDataColumn(SourceColumn);
                Columns.Add(Column);
            }

            // rows
            JsonDataRow Row;
            foreach (DataRow SourceRow in Source.Rows)
            {
                Row = new JsonDataRow(SourceRow);
                Rows.Add(Row);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataTable(JsonObject JO)
        {
            FromJObject(JO, this);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataTable(string JsonText)
        {
            FromJson(JsonText, this);
        }

        /* static */
        /// <summary>
        /// Converts a DataTable to JsonObject
        /// </summary>
        static public JsonObject ToJObject(DataTable Source, SqlBrokerTableDef Descriptor)
        {
            JsonDataTable Instance = new JsonDataTable(Source, Descriptor);
            return Instance.ToJObject();
        }
        /// <summary>
        /// Converts a DataTable to JsonObject
        /// </summary>
        static public JsonObject ToJObject(DataTable Source)
        {
            JsonDataTable Instance = new JsonDataTable(Source);
            return Instance.ToJObject();
        }
        /// <summary>
        /// Json text to JsonDataTable
        /// </summary>
        static public JsonDataTable FromJson(string JsonText)
        {
            JsonDataTable Table = new JsonDataTable();
            FromJson(JsonText, Table);
            return Table;
        }
        /// <summary>
        /// Json text to JsonDataTable
        /// </summary>
        static public void FromJson(string JsonText, JsonDataTable Table)
        {
            JsonObject JO = JsonNode.Parse(JsonText) as JsonObject;
            FromJObject(JO, Table);
        }
        /// <summary>
        /// JsonObject to JsonDataTable
        /// </summary>
        static public JsonDataTable FromJObject(JsonObject JO)
        {
            JsonDataTable Table = new JsonDataTable();
            FromJObject(JO, Table);
            return Table;
        }
        /// <summary>
        /// JsonObject to JsonDataTable
        /// </summary>
        static public void FromJObject(JsonObject JO, JsonDataTable Table)
        {

            Table.Clear();

            dynamic jTable = JO as dynamic;

            // properties
            Table.Name = jTable.Name;

            Table.PrimaryKeyField = jTable.PrimaryKeyField;
            Table.MasterKeyField = jTable.MasterKeyField;
            Table.DetailKeyField = jTable.DetailKeyField;
            Table.MasterTableName = jTable.MasterTableName;
            Table.AutoGenerateGuidKeys = jTable.AutoGenerateGuidKeys;

            string S;

            // Details
            dynamic jDetails = jTable.Details;
            foreach (dynamic D in jDetails)
            {
                S = D.ToObject(typeof(string)) as string;
                if (!string.IsNullOrWhiteSpace(S))
                    Table.Details.Add(S);
            }

            // StockTables
            dynamic jStockTables = jTable.StockTables;
            foreach (dynamic ST in jStockTables)
            {
                S = ST.ToObject(typeof(string)) as string;
                if (!string.IsNullOrWhiteSpace(S))
                    Table.StockTables.Add(S);
            }

            // Columns
            JsonDataColumn Column;
            dynamic jColumns = jTable.Columns;
            foreach (dynamic C in jColumns)
            {
                Column = new JsonDataColumn();

                Column.Name = C.Name;
                Column.Title = C.Title;
                //Column.TitleKey = C.TitleKey;
                Column.DataType = C.DataType;
                Column.Expression = C.Expression;
                Column.DefaultValue = C.DefaultValue;
                Column.MaxLength = C.MaxLength;
                Column.Decimals = C.Decimals;
                Column.ReadOnly = C.ReadOnly;
                Column.Visible = C.Visible;
                Column.Required = C.Required;
                Column.Unique = C.Unique;

                Table.Columns.Add(Column);
            }


            // Rows
            dynamic Data;
            object[] items;

            JsonDataRow Row;
            dynamic jRows = jTable.Rows;

            foreach (dynamic R in jRows)
            {
                Data = R.Data;
                items = (object[])Data.ToObject(typeof(object[]));

                Row = new JsonDataRow();
                Row.Data = items;
                Row.State = (DataRowState)R.State;

                Table.Rows.Add(Row);
            }


            // Deleted Rows
            dynamic jDeleted = jTable.Deleted;

            foreach (dynamic DR in jDeleted)
            {
                Data = DR.Data;
                items = (object[])Data.ToObject(typeof(object[]));

                Row = new JsonDataRow();
                Row.Data = items;
                Row.State = DataRowState.Deleted;

                Table.Deleted.Add(Row);
            }


        }

        /* public */
        /// <summary>
        /// Clears this instance from all data in properties and lists
        /// </summary>
        public void Clear()
        {
            Name = string.Empty;
            PrimaryKeyField = string.Empty;
            MasterKeyField = string.Empty;
            DetailKeyField = string.Empty;
            MasterTableName = string.Empty;
            AutoGenerateGuidKeys = false;

            Details.Clear();
            StockTables.Clear();
            Columns.Clear();
            Rows.Clear();
            Deleted.Clear();
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
        /// Copies rows from this JsonDataTable to a real DataTable, preserving row state (even for deleted rows).
        /// </summary>
        public void RowsTo(DataTable Table)
        {
            DataRow Row;

            if ((Rows != null) && (Rows.Count > 0))
            {                
                foreach (JsonDataRow JRow in Rows)
                {
                    Row = Table.NewRow();
                    Row.ItemArray = JRow.Data;
                    Table.Rows.Add(Row);

                    Row.AcceptChanges();

                    if (JRow.State == DataRowState.Added)
                        Row.SetAdded();
                    else if (JRow.State == DataRowState.Modified)
                        Row.SetModified();
                }
            }

            if ((Deleted != null) && (Deleted.Count > 0))
            {
                foreach (JsonDataRow JRow in Rows)
                {
                    Row = Table.NewRow();
                    Row.ItemArray = JRow.Data;
                    Table.Rows.Add(Row);

                    Row.AcceptChanges();

                    Row.Delete();
                } 
            }
            
        }
        /// <summary>
        /// Converts this to a MemTable
        /// </summary>
        public DataTable ToTable()
        {
            DataTable Table = new DataTable(this.Name);
            this.ToTable(Table);
            return Table;
        }
        /// <summary>
        /// Converts this to a MemTable
        /// </summary>
        public void ToTable(DataTable Table)
        {
            Table.TableName = this.Name;
            if (Table is MemTable)
            {
                (Table as MemTable).PrimaryKeyField = this.PrimaryKeyField;
                (Table as MemTable).MasterKeyField = this.MasterKeyField;
                (Table as MemTable).DetailKeyField = this.DetailKeyField;
                (Table as MemTable).AutoGenerateGuidKeys = this.AutoGenerateGuidKeys;
            }

            foreach (JsonDataColumn SourceColumn in this.Columns)
                Table.Columns.Add(SourceColumn.ToColumn());

            this.RowsTo(Table);
        }


        /* properties */
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// PrimaryKeyField
        /// </summary>
        public string PrimaryKeyField { get; set; }
        /// <summary>
        /// MasterKeyField
        /// </summary>
        public string MasterKeyField { get; set; }
        /// <summary>
        /// DetailKeyField
        /// </summary>
        public string DetailKeyField { get; set; }
        /// <summary>
        /// MasterTableName
        /// </summary>
        public string MasterTableName { get; set; }
        /// <summary>
        /// AutoGenerateGuidKeys
        /// </summary>
        public bool AutoGenerateGuidKeys { get; set; }
        /// <summary>
        /// Details
        /// </summary>
        public List<string> Details { get; set; } = new List<string>();
        /// <summary>
        /// StockTables
        /// </summary>
        public List<string> StockTables { get; set; } = new List<string>();
        /// <summary>
        /// Columns
        /// </summary>
        public List<JsonDataColumn> Columns { get; set; } = new List<JsonDataColumn>();
        /// <summary>
        /// Rows
        /// </summary>
        public List<JsonDataRow> Rows { get; set; } = new List<JsonDataRow>();
        /// <summary>
        /// Deleted
        /// </summary>
        public List<JsonDataRow> Deleted { get; set; } = new List<JsonDataRow>();


    }
}
