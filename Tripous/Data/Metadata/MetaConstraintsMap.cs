namespace Tripous.Data.Metadata
{
    /// <summary>
    /// Provides a unified way in accessing the DataTable columns, 
    /// the GetSchema() returns for a Collection such as Tables, Columns, etc.
    /// <para>NOTE: this code is taken from the excellent http://dbschemareader.codeplex.com/</para>
    /// </summary>
    public class MetaConstraintsMap
    {
        private void CheckSqLite(DataTable Table)
        {
            if (!Table.Columns.Contains(ColumnKey)) ColumnKey = "FKEY_FROM_COLUMN";
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = "FKEY_FROM_ORDINAL_POSITION";
            if (!Table.Columns.Contains(RefersToTableKey)) RefersToTableKey = "FKEY_TO_TABLE";
        }
        private void CheckDb2(DataTable Table, MetaConstraintType constraintType)
        {
            if (constraintType == MetaConstraintType.PrimaryKey && !Table.Columns.Contains(Key)) Key = "PK_NAME";
            if (constraintType == MetaConstraintType.ForeignKey && !Table.Columns.Contains(Key)) Key = "FK_NAME";
            if (!Table.Columns.Contains(TableKey)) TableKey = "FKTABLE_NAME";
            if (!Table.Columns.Contains(RefersToTableKey)) RefersToTableKey = "PKTABLE_NAME";
            if (!Table.Columns.Contains(RefersToKey)) RefersToKey = "PK_NAME";
            if (!Table.Columns.Contains(ColumnKey)) ColumnKey = "FKCOLUMN_NAME";
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = "KEY_SEQ";
            if (!Table.Columns.Contains(ExpressionKey)) ExpressionKey = "CHECK_CLAUSE";
        }
        private void CheckFirebird(DataTable Table, MetaConstraintType constraintType)
        {
            if (constraintType == MetaConstraintType.PrimaryKey && !Table.Columns.Contains(Key)) Key = "PK_NAME";
            if (constraintType == MetaConstraintType.ForeignKey && !Table.Columns.Contains(Key)) Key = "UK_NAME";
            if (!Table.Columns.Contains(RefersToTableKey)) RefersToTableKey = "REFERENCED_TABLE_NAME";
            //a firebird typo!
            if (Table.Columns.Contains("CHECK_CLAUSULE")) ExpressionKey = "CHECK_CLAUSULE";
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaConstraintsMap(DataTable Table, MetaConstraintType constraintType)
        {
            //all same, my custom sql
            Key = "CONSTRAINT_NAME";
            TableKey = "TABLE_NAME";
            ColumnKey = "COLUMN_NAME";
            OrdinalKey = "ORDINAL_POSITION";
            RefersToKey = "UNIQUE_CONSTRAINT_NAME";
            RefersToTableKey = "FK_TABLE";
            ExpressionKey = "EXPRESSION";
            DeleteRuleKey = "DELETE_RULE";
            UpdateRuleKey = "UPDATE_RULE";
            //oracle
            if (!Table.Columns.Contains(Key)) Key = "FOREIGN_KEY_CONSTRAINT_NAME";
            if (!Table.Columns.Contains(TableKey)) TableKey = "FOREIGN_KEY_TABLE_NAME";
            if (!Table.Columns.Contains(RefersToTableKey)) RefersToTableKey = "PRIMARY_KEY_TABLE_NAME";
            if (!Table.Columns.Contains(RefersToKey)) RefersToKey = "PRIMARY_KEY_CONSTRAINT_NAME";
            //devart.data.postgresql
            if (!Table.Columns.Contains(Key)) Key = "NAME";
            if (!Table.Columns.Contains(TableKey)) TableKey = "TABLE";

            //firebird
            CheckFirebird(Table, constraintType);
            //sqlite
            CheckSqLite(Table);

            //db2
            CheckDb2(Table, constraintType);

            //oledb
            if (!Table.Columns.Contains(Key)) Key = "FK_NAME";
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = "ORDINAL";
            if (!Table.Columns.Contains(TableKey)) TableKey = "FK_TABLE_NAME";
            if (!Table.Columns.Contains(ColumnKey)) ColumnKey = "FK_COLUMN_NAME";
            if (!Table.Columns.Contains(RefersToTableKey)) RefersToTableKey = "PK_TABLE_NAME";
            if (!Table.Columns.Contains(RefersToKey)) RefersToKey = "PK_NAME";

            if (!Table.Columns.Contains(RefersToKey)) RefersToKey = null;
            if (!Table.Columns.Contains(RefersToTableKey)) RefersToTableKey = null;
            if (!Table.Columns.Contains(DeleteRuleKey)) DeleteRuleKey = null;
            if (!Table.Columns.Contains(UpdateRuleKey)) UpdateRuleKey = null;
            //not present if separate foreign key columns
            if (!Table.Columns.Contains(ColumnKey)) ColumnKey = null;
            if (!Table.Columns.Contains(OrdinalKey)) OrdinalKey = null;
            if (!Table.Columns.Contains(ExpressionKey)) ExpressionKey = null;
            if (!Table.Columns.Contains(Key)) Key = null;
        }


        /* properties */
        /// <summary>
        /// Gets the name of the foreign table column
        /// </summary>
        public string RefersToTableKey { get; private set; }
        /// <summary>
        /// Gets the name of the unique constraint name column
        /// </summary>
        public string RefersToKey { get; private set; }
        /// <summary>
        /// Gets the name of the expression column
        /// </summary>
        public string ExpressionKey { get; private set; }
        /// <summary>
        /// Gets the name of the update rule column
        /// </summary>
        public string UpdateRuleKey { get; private set; }
        /// <summary>
        /// Gets the name of the ColumnName column
        /// </summary>
        public string ColumnKey { get; private set; }
        /// <summary>
        /// Gets the name of the delete rule column
        /// </summary>
        public string DeleteRuleKey { get; private set; }
        /// <summary>
        /// Gets the name of the ordinal position column
        /// </summary>
        public string OrdinalKey { get; private set; }
        /// <summary>
        /// Gets the name of the owner table name column
        /// </summary>
        public string TableKey { get; private set; }
        /// <summary>
        /// Gets the name of the foreign key constraint name column
        /// </summary>
        public string Key { get; private set; }
    }

 
}
