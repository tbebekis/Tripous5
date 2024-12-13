namespace Tripous.Data.Metadata
{


    /// <summary>
    /// Provides a unified way in accessing the DataTable columns, 
    /// the GetSchema() returns for a Collection such as Tables, Columns, etc.
    /// <para>NOTE: this code is taken from the excellent http://dbschemareader.codeplex.com/</para>
    /// </summary>
    public class MetaFieldsMap
    {
        private void CheckDb2(DataTable Table)
        {
            if (Table.Columns.Contains("data_type_name")) DatatypeKey = "data_type_name";
            if (!Table.Columns.Contains(PrecisionKey)) PrecisionKey = "column_size";
            if (!Table.Columns.Contains(ScaleKey)) ScaleKey = "decimal_digits";
            if (!Table.Columns.Contains(DefaultKey)) DefaultKey = "column_def";
        }
        private void CheckDevartPostgreSql(DataTable Table)
        {
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = "position";
            if (!Table.Columns.Contains(TableKey)) TableKey = "table";
            if (!Table.Columns.Contains(ColumnName)) ColumnName = "name";
            if (!Table.Columns.Contains(DatatypeKey)) DatatypeKey = "typename";
            if (!Table.Columns.Contains(UniqueKey)) UniqueKey = "isunique";
            if (!Table.Columns.Contains(DefaultKey)) DefaultKey = "defaultvalue";
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaFieldsMap(DataTable Table)
        {
 

            //sql server
            ColumnName = "column_name";
            TableKey = "table_name";
            OrdinalKey = "ordinal_position";
            DatatypeKey = "data_type";
            NullableKey = "is_nullable";
            LengthKey = "character_maximum_length";
            PrecisionKey = "numeric_precision";
            ScaleKey = "numeric_scale";
            DateTimePrecision = "datetime_precision";
            DefaultKey = "column_default";

            //oracle
            if (!Table.Columns.Contains(OrdinalKey)) 
                OrdinalKey = "id";

            if (!Table.Columns.Contains(DatatypeKey)) 
                DatatypeKey = "datatype";

            if (!Table.Columns.Contains(NullableKey)) 
                NullableKey = "nullable";

            if (!Table.Columns.Contains(LengthKey)) 
                LengthKey = "length";

            if (!Table.Columns.Contains(PrecisionKey)) 
                PrecisionKey = "precision";

            if (!Table.Columns.Contains(ScaleKey)) 
                ScaleKey = "scale";

            if (!Table.Columns.Contains(DateTimePrecision)) 
                DateTimePrecision = null;

            //sqlite
            AutoIncrementKey = "AUTOINCREMENT";
            PrimaryKeyKey = "PRIMARY_KEY";
            UniqueKey = "UNIQUE";

            //firebird
            if (!Table.Columns.Contains(DatatypeKey)) 
                DatatypeKey = "column_data_type";

            if (!Table.Columns.Contains(LengthKey)) 
                LengthKey = "COLUMN_SIZE";

            //devart.Data.PostgreSql
            CheckDevartPostgreSql(Table);

            //db2
            CheckDb2(Table);

            //Intersystems Cache
            if (Table.Columns.Contains("TYPE_NAME")) 
                DatatypeKey = "TYPE_NAME";

            //sybase ultralite
            if (!Table.Columns.Contains(DefaultKey)) 
                DefaultKey = "default";

            if (!Table.Columns.Contains(NullableKey)) 
                NullableKey = "nulls";

            if (!Table.Columns.Contains(LengthKey)) 
                LengthKey = null;

            if (!Table.Columns.Contains(PrecisionKey)) 
                PrecisionKey = null;

            if (!Table.Columns.Contains(ScaleKey)) 
                ScaleKey = null;




            if (!Table.Columns.Contains(DefaultKey)) 
                DefaultKey = null; //not in Oracle catalog

            if (!Table.Columns.Contains(AutoIncrementKey)) 
                AutoIncrementKey = null;

            if (!Table.Columns.Contains(PrimaryKeyKey)) 
                PrimaryKeyKey = null;

            if (!Table.Columns.Contains(UniqueKey)) 
                UniqueKey = null;

            if (!Table.Columns.Contains(OrdinalKey)) 
                OrdinalKey = null;

            if (!Table.Columns.Contains(DatatypeKey)) 
                DatatypeKey = null;
        }

        /* properties */
 
        /// <summary>
        /// Gets the name of the ColumnName column
        /// </summary>
        public string ColumnName { get; private set; }
        /// <summary>
        /// Gets the name of the Default column
        /// </summary>
        public string DefaultKey { get; private set; }
        /// <summary>
        /// Gets the name of the DateTimePrecision column
        /// </summary>
        public string DateTimePrecision { get; private set; }
        /// <summary>
        /// Gets the name of the Precision column
        /// </summary>
        public string PrecisionKey { get; private set; }
        /// <summary>
        /// Gets the name of the Scale column
        /// </summary>
        public string ScaleKey { get; private set; }
        /// <summary>
        /// Gets the name of the Unique column
        /// </summary>
        public string UniqueKey { get; private set; }
        /// <summary>
        /// Gets the name of the PrimaryKey column
        /// </summary>
        public string PrimaryKeyKey { get; private set; }
        /// <summary>
        /// Gets the name of the AutoIncrement column
        /// </summary>
        public string AutoIncrementKey { get; private set; }
        /// <summary>
        /// Gets the name of the Ordinal column
        /// </summary>
        public string OrdinalKey { get; private set; }
        /// <summary>
        /// Gets the name of the TableName column
        /// </summary>
        public string TableKey { get; set; }
        /// <summary>
        /// Gets the name of the Nullabl column
        /// </summary>
        public string NullableKey { get; private set; }
        /// <summary>
        /// Gets the name of the Length column
        /// </summary>
        public string LengthKey { get; private set; }        
        /// <summary>
        /// Gets the name of the Datatype column
        /// </summary>
        public string DatatypeKey { get; private set; }
    }
}
