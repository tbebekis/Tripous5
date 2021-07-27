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
    public class MetaIndexes : MetaItems<MetaIndex>
    {

        /* private */
        private bool GetBooleanValue(DataRow Row, string FieldName, string Text)
        {
            try
            {
                if (!string.IsNullOrEmpty(FieldName))
                {
                    object Value = Row[FieldName];
                    if (!Sys.IsNull(Value))
                    {
                        if (Value is string)
                            return Sys.IsSameText(Value.ToString(), Text);
                        else
                            return Row.AsBoolean(FieldName);
                    }
                }
            }
            catch
            {
            }

            return false;
        }
        private void AddIndexes(DataTable Table)
        {
            MetaIndexesMap Map = new MetaIndexesMap(Table);

            DataView View = Table.DefaultView;
            if (!string.IsNullOrEmpty(Map.TableKey) && Table.ContainsColumn(Map.TableKey))
            {
                View.RowFilter = string.Format("{0} = '{1}'", Map.TableKey, this.Table.Name);
                View.Sort = Map.TableKey;
            }

            string Name;
            MetaIndex Item;

            DataRow Row;
            //foreach (DataRow Row in Table.Rows)
            foreach (DataRowView RowView in View)
            {
                Row = RowView.Row;

                Name = Row.AsString(Map.Key);
                if (!string.IsNullOrEmpty(Name))
                {

                    Item = this.Find(Name);
                    if (Item == null)
                    {
                        Item = this.Add(Name);

                        if (!string.IsNullOrEmpty(Map.Typekey))
                            Item.IndexType = Row.AsString(Map.Typekey);

                        if (GetBooleanValue(Row, Map.UniqueKey, "UNIQUE"))
                        {
                            Item.IsUnique = true;
                            Item.IndexType = "UNIQUE";
                        }

                        if (GetBooleanValue(Row, Map.PrimaryKey, String.Empty))
                            Item.IndexType = "PRIMARY";

                    }


                    if (!string.IsNullOrEmpty(Map.ColumnKey))
                    {
                        Name = Row.AsString(Map.ColumnKey);
                        if (!string.IsNullOrEmpty(Name))
                        {
                            Item.Fields += Row.AsString(Map.ColumnKey) + ";";

                        }
                    }
                }
            }

        }


        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            if (Store == null)
                return;

            AddIndexes(this.Table.tblIndexes);
            AddIndexes(this.Table.tblIndexColumns);
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaIndexes()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Indexes";
            fKind = MetaNodeKind.Indexes;
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
