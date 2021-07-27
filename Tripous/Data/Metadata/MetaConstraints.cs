/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Represents a meta-data information entity
    /// </summary>
    public class MetaConstraints : MetaItems<MetaConstraint>
    {
        /* private */
        private static string GetDeleteUpdateRule(DataRow Row, string FieldName)
        {
            if (string.IsNullOrEmpty(FieldName))
                return string.Empty;

            string Result = Row.AsString(FieldName);

            //translate DB2 numbers
            if (Result == "0")
                Result = "CASCADE";
            else if (Result == "1")
                Result = "RESTRICT";
            else if (Result == "2")
                Result = "SET NULL";
            else if (Result == "3")
                Result = "NO ACTION";

            if (!string.IsNullOrEmpty(Result) && !Result.Equals("NO ACTION", StringComparison.OrdinalIgnoreCase))
                return Result;

            return string.Empty;
        }
        private void AddCollection(MetaConstraintType ConstraintType)
        {
            // Firebird returns check constraints without TABLE_NAME
            if ((ConstraintType == MetaConstraintType.Check) && (Store.Provider.ServerType == SqlServerType.Firebird))
                return;

            DataTable Table = null;


            switch (ConstraintType)
            {
                case MetaConstraintType.PrimaryKey: Table = this.Table.tblPrimaryKeys; break;
                case MetaConstraintType.ForeignKey: Table = this.Table.tblForeignKeys; break;
                case MetaConstraintType.UniqueKey: Table = this.Table.tblUniqueKeys; break;
                case MetaConstraintType.Check: Table = this.Table.tblConstraints; break;
            }

            MetaConstraintsMap Map = new MetaConstraintsMap(Table, ConstraintType);

            if (string.IsNullOrEmpty(Map.Key) || !Table.ContainsColumn(Map.Key))
                return;

            DataView View = Table.DefaultView;

            if (!string.IsNullOrEmpty(Map.TableKey) && Table.ContainsColumn(Map.TableKey))
                View.RowFilter = string.Format("{0} = '{1}'", Map.TableKey, this.Table.Name);

            if ((ConstraintType != MetaConstraintType.Check) && !string.IsNullOrEmpty(Map.OrdinalKey) && Table.ContainsColumn(Map.OrdinalKey))
                View.Sort = Map.OrdinalKey;

            MetaConstraint Item = null;
            string Name = null;
            DataRow Row;
            foreach (DataRowView RowView in View)
            {
                Row = RowView.Row;

                Name = Row.AsString(Map.Key);
                Item = this.Find(Name);


                if (Item == null)
                {
                    Item = this.Add(Name);
                    Item.ConstraintType = ConstraintType;

                    if (ConstraintType == MetaConstraintType.Check && !string.IsNullOrEmpty(Map.ExpressionKey))
                    {
                        Item.Expression = Row.AsString(Map.ExpressionKey);
                        continue;
                    }

                    if (ConstraintType == MetaConstraintType.ForeignKey)
                    {
                        if (!string.IsNullOrEmpty(Map.RefersToKey))
                            Item.ForeignFields = Row.AsString(Map.RefersToKey);

                        if (!string.IsNullOrEmpty(Map.RefersToTableKey))
                            Item.ForeignTable = Row.AsString(Map.RefersToTableKey);
                    }

                    Item.DeleteRule = GetDeleteUpdateRule(Row, Map.DeleteRuleKey);
                    Item.UpdateRule = GetDeleteUpdateRule(Row, Map.UpdateRuleKey);

                }


                if (ConstraintType != MetaConstraintType.Check && !string.IsNullOrEmpty(Map.ColumnKey))
                {
                    Item.Fields += Row.AsString(Map.ColumnKey) + ";";
                }


            }

        }
        private void AddForeignKeyColumns()
        {
            DataTable Table = this.Table.tblForeignKeyColumns;

            if ((Table == null) || (Table.Rows.Count == 0))
                return;


            string key = "CONSTRAINT_NAME";
            string tableKey = "TABLE_NAME";
            string columnKey = "COLUMN_NAME";

            if (!Table.Columns.Contains(key))
                key = "foreignkey";

            if (!Table.Columns.Contains(tableKey))
                tableKey = "table";

            if (!Table.Columns.Contains(columnKey))
                columnKey = "name";

            if (!Table.Columns.Contains(columnKey))
                columnKey = "FKEY_FROM_COLUMN"; //VistaDB


            string Name;
            MetaConstraint Item = null;

            foreach (DataRow Row in Table.Rows)
            {
                Name = Row.AsString(key);
                if (!string.IsNullOrEmpty(Name))
                {
                    Item = this.Find(Name);
                }

                if (Item == null)
                {
                    Item = this.Add(Name);
                    Item.ConstraintType = MetaConstraintType.ForeignKey;
                }

                Name = Row.AsString(columnKey);
                if (!string.IsNullOrEmpty(Name))
                {
                    if (!Item.Fields.ContainsText(Name))
                        Item.Fields += Name + ";";
                }
            }

        }

        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            AddCollection(MetaConstraintType.PrimaryKey);
            AddCollection(MetaConstraintType.ForeignKey);
            AddCollection(MetaConstraintType.UniqueKey);
            AddCollection(MetaConstraintType.Check);

            AddForeignKeyColumns();
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaConstraints()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Constraints";
            fKind = MetaNodeKind.Constraints;
        }


        /* properties */
        /// <summary>
        /// Gets the owner table
        /// </summary>
        public MetaTable Table { get { return fOwner as MetaTable; } }
        /// <summary>
        /// Gets the owner metadata store
        /// </summary>
        public Metastore Store { get { return Table != null ? Table.Store : null; } }
    }
}
