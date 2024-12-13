namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Provides a unified way in accessing the DataTable columns, 
    /// the GetSchema() returns for a Collection such as Tables, Columns, etc.
    /// <para>NOTE: this code is taken from the excellent http://dbschemareader.codeplex.com/</para>
    /// </summary>
    public class MetaIndexesMap
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaIndexesMap(DataTable Table)
        {
            UniqueKey = "UNIQUE";
            PrimaryKey = "PRIMARY";

            //sql server
            Key = "CONSTRAINT_NAME";
            TableKey = "TABLE_NAME";
            SchemaKey = "TABLE_SCHEMA";
            ColumnKey = "COLUMN_NAME";
            OrdinalKey = "ORDINAL_POSITION";
            //oracle
            Typekey = "INDEX_TYPE";

            if (!Table.Columns.Contains(SchemaKey)) SchemaKey = "INDEX_SCHEMA";
            if (!Table.Columns.Contains(SchemaKey)) SchemaKey = "OWNER";
            if (!Table.Columns.Contains(Key)) Key = "INDEX_NAME";
            if (!Table.Columns.Contains(SchemaKey)) SchemaKey = "INDEX_OWNER";
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = "COLUMN_POSITION";
            if (!Table.Columns.Contains(UniqueKey)) UniqueKey = "UNIQUENESS";
            //mysql
            if (!Table.Columns.Contains(SchemaKey)) SchemaKey = "INDEX_SCHEMA";
            //Devart.Data.Oracle
            if (!Table.Columns.Contains(Key)) Key = "INDEX"; //IndexColumns
            if (!Table.Columns.Contains(Key)) Key = "NAME"; //Indexes
            if (!Table.Columns.Contains(UniqueKey)) UniqueKey = "ISUNIQUE";
            if (!Table.Columns.Contains(SchemaKey)) SchemaKey = "SCHEMA";
            if (!Table.Columns.Contains(TableKey)) TableKey = "TABLE";
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = "POSITION";
            if (!Table.Columns.Contains(ColumnKey)) ColumnKey = "NAME";
            //devart.data.postgresql
            if (!Table.Columns.Contains(Key)) Key = "indexname";
            //sqlite
            if (!Table.Columns.Contains(PrimaryKey)) PrimaryKey = "PRIMARY_KEY";
            //postgresql
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = null;
            //sqlserver 2008 - HEAP CLUSTERED NONCLUSTERED XML SPATIAL
            //sys_indexes has is_unique but it's not exposed. 
            if (!Table.Columns.Contains(Typekey)) Typekey = "type_desc";

            //pre 2008 sql server
            if (!Table.Columns.Contains(Typekey)) Typekey = null;

            //indexes and not indexcolumns
            if (!Table.Columns.Contains(ColumnKey)) ColumnKey = null;
            if (!Table.Columns.Contains(UniqueKey)) UniqueKey = null;
            if (!Table.Columns.Contains(PrimaryKey)) PrimaryKey = null;
            if (!Table.Columns.Contains(SchemaKey)) SchemaKey = null;
        }

        /* properties */
        /// <summary>
        /// Gets the name of the index type column
        /// </summary>
        public string Typekey { get; private set; }
        /// <summary>
        /// Gets the name of the unique key column
        /// </summary>
        public string UniqueKey { get; private set; }
        /// <summary>
        /// Gets the name of the primary key column
        /// </summary>
        public string PrimaryKey { get; private set; }
        /// <summary>
        /// Gets the name of the column name column
        /// </summary>
        public string ColumnKey { get; private set; }
        /// <summary>
        /// Gets the name of the ordinal position  column
        /// </summary>
        public string OrdinalKey { get; private set; }
        /// <summary>
        /// Gets the name of the owner table name  column
        /// </summary>
        public string TableKey { get; private set; }
        /// <summary>
        /// Gets the name of the table schema column
        /// </summary>
        public string SchemaKey { get; private set; }
        /// <summary>
        /// Gets the name of the constraint name  column
        /// </summary>
        public string Key { get; private set; }
    }
}
