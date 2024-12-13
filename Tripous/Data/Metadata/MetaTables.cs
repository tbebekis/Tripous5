namespace Tripous.Data.Metadata
{


    /// <summary>
    /// Represents a meta-data information entity
    /// </summary>
    public class MetaTables : MetaItems<MetaTable>
    {
        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            if (Store == null)
                return;


            DataTable Table = Store.GetTables();
            MetaTablesMap Map = Store.GetTablesMap(Table);

            if (string.IsNullOrEmpty(Map.TableName) || !Table.ContainsColumn(Map.TableName))
                return;



            DataView View = Table.DefaultView;
            View.RowFilter = string.Format("{0} LIKE '%'", Map.TableName);
            View.Sort = Map.TableName;

            //string Filter = string.Format("{0} LIKE '%'", Map.TableName);
            //string Sort = Map.TableName;

            //DataRow[] Rows = Table.Select(Filter, Sort);

            string TableType;
            string TableName;
            string SchemaOwner;
            MetaTable Item;
            DataRow Row;

            //foreach (DataRow Row in Rows)
            foreach (DataRowView RowView in View)
            {
                Row = RowView.Row;

                TableName = Row[Map.TableName].ToString();
                if (this.Contains(TableName))
                    continue;

                TableType = Row[Map.TypeKey].ToString();

                // may be a VIEW or a system table - Sql server has base tables and views. Oracle has system and user
                if (!TableType.Equals("TABLE", StringComparison.OrdinalIgnoreCase)
                     && !TableType.Equals("BASE", StringComparison.OrdinalIgnoreCase)        // sybase
                     && !TableType.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)
                     && !TableType.Equals("User", StringComparison.OrdinalIgnoreCase)
                     && !TableType.Equals("InnoDB", StringComparison.OrdinalIgnoreCase)      // MySQL types are something different
                     && !TableType.Equals("MyISAM", StringComparison.OrdinalIgnoreCase))     // MySQL types are something different
                    continue;

                // exclude Oracle bin tables
                if (TableName.StartsWith("BIN$", StringComparison.OrdinalIgnoreCase))
                    continue;

                SchemaOwner = !string.IsNullOrEmpty(Map.OwnerKey) ? Row[Map.OwnerKey].ToString() : string.Empty;

                // Db2 system tables creeping in
                if (Map.IsDb2 && SchemaOwner.Equals("SYSTOOLS", StringComparison.OrdinalIgnoreCase))
                    continue;

                Item = new MetaTable();
                Item.Name = TableName;
                Item.SchemaOwner = SchemaOwner;

                this.Add(Item);
            }
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaTables()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Tables";
            fKind = MetaNodeKind.Tables;
        }


        /* properties */
        /// <summary>
        /// Gets the owner meta metastore
        /// </summary>
        public Metastore Store { get { return Owner as Metastore; } }
    }
}
