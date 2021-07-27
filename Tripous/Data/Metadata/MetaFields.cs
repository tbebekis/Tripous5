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
    public class MetaFields : MetaItems<MetaField>
    {
        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            DataTable Table = this.Table.tblColumns;
            MetaFieldsMap Map = Store.GetFieldsMap(Table);

            DataView View = Table.DefaultView;
            if (!string.IsNullOrEmpty(Map.TableKey) && Table.ContainsColumn(Map.TableKey))
            {
                View.RowFilter = string.Format("{0} = '{1}'", Map.TableKey, this.Table.Name);
                View.Sort = Map.TableKey;
            }

            MetaField Item;
            int Value;
            string S;

            string TrueData = "YES;Y;TRUE;1;";

            bool OrdinalFlag = !string.IsNullOrEmpty(Map.OrdinalKey) && Table.ContainsColumn(Map.OrdinalKey);
            bool DataTypeFlag = !string.IsNullOrEmpty(Map.DatatypeKey) && Table.ContainsColumn(Map.DatatypeKey);
            bool NullFlag = !string.IsNullOrEmpty(Map.NullableKey) && Table.ContainsColumn(Map.NullableKey);
            bool LengthFlag = !string.IsNullOrEmpty(Map.LengthKey) && Table.ContainsColumn(Map.LengthKey);
            bool DefaultFlag = !string.IsNullOrEmpty(Map.DefaultKey) && Table.ContainsColumn(Map.DefaultKey);
            bool PrimaryFlag = !string.IsNullOrEmpty(Map.PrimaryKeyKey) && Table.ContainsColumn(Map.PrimaryKeyKey);
            bool AutoIncrementFlag = !string.IsNullOrEmpty(Map.AutoIncrementKey) && Table.ContainsColumn(Map.AutoIncrementKey);
            bool UniqueFlag = !string.IsNullOrEmpty(Map.UniqueKey) && Table.ContainsColumn(Map.UniqueKey);
            bool PrecisionFlag = !string.IsNullOrEmpty(Map.PrecisionKey) && Table.ContainsColumn(Map.PrecisionKey);
            bool ScaleFlag = !string.IsNullOrEmpty(Map.ScaleKey) && Table.ContainsColumn(Map.ScaleKey);

            DataRow Row;
            //foreach (DataRow Row in Table.Rows)
            foreach (DataRowView RowView in View)
            {
                Row = RowView.Row;

                Item = this.Add(Row[Map.ColumnName].ToString());

                /* ordinal */
                if (OrdinalFlag)
                    if (int.TryParse(Row.AsString(Map.OrdinalKey), out Value))
                        Item.Ordinal = Value;

                /* data type */
                if (DataTypeFlag)
                {
                    S = Row.AsString(Map.DatatypeKey);
                    Item.MetaType = Store.Types.Find(S);
                    if (Item.MetaType == null)
                        Item.MetaType = Store.Types.FindByMetaTag(S);
                }

                /* is nullable */
                if (NullFlag)
                    Item.IsNullable = TrueData.ContainsText(Row.AsString(Map.NullableKey));

                /* max length */
                if (LengthFlag && (Item.MetaType != null) && (Item.MetaType.IsString))
                    if (int.TryParse(Row.AsString(Map.LengthKey), out Value))
                        Item.MaxLength = Value.ToString();

                /* default value */
                if (DefaultFlag)
                {
                    S = Row.AsString(Map.DefaultKey);
                    if (!string.IsNullOrEmpty(S))
                    {
                        /* firebird prepends the word default, i.e. default '' or default -1 */
                        S = S.Replace("default", string.Empty);
                        S = S.Trim();
                        if (S == "''")
                            Item.DefaultValue = string.Empty;
                        else
                        {
                            S = S.Replace("(", string.Empty);
                            S = S.Replace(")", string.Empty);
                            S = S.Trim(new[] { ' ', '\'', '=' });

                            if (Sys.IsSameText(S, "newid"))
                                Item.DefaultValue = "Guid";
                            else if (!string.IsNullOrEmpty(S))
                                Item.DefaultValue = S;
                        }
                    }
                }

                /* primary key */
                if (PrimaryFlag)
                    Item.IsPrimaryKey = TrueData.ContainsText(Row.AsString(Map.PrimaryKeyKey));

                /* identity - autoincrement */
                if (AutoIncrementFlag)
                    Item.IsIdentity = TrueData.ContainsText(Row.AsString(Map.AutoIncrementKey));

                /* unique */
                if (UniqueFlag)
                    Item.IsUniqueKey = TrueData.ContainsText(Row.AsString(Map.UniqueKey));

                /* precision */
                if (PrecisionFlag)
                    if (int.TryParse(Row.AsString(Map.PrecisionKey), out Value))
                        Item.Precision = Value.ToString();

                /* scale */
                if (ScaleFlag)
                    if (int.TryParse(Row.AsString(Map.ScaleKey), out Value))
                        Item.Scale = Value.ToString();

            }
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaFields()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Fields";
            fKind = MetaNodeKind.Fields;
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
