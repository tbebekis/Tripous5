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
    public class MetaViews : MetaItems<MetaView>
    {
        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            if (Store == null)
                return;

            DataTable Table = Store.GetViews();
            MetaViewsMap Map = Store.GetViewsMap(Table);

            string Name;
            MetaView Item;


            foreach (DataRow Row in Table.Rows)
            {
                //if (!string.IsNullOrEmpty(Map.TypeKey) && Sys.IsSameText("VIEW", Row.AsString(Map.TypeKey)))
                {
                    // ignore db2 system tables
                    if (!string.IsNullOrEmpty(Map.TypeKey)
                        && !string.IsNullOrEmpty(Map.OwnerKey)
                        && Row.AsString(Map.OwnerKey).StartsWith("SYS", StringComparison.OrdinalIgnoreCase))
                        continue;

                    Name = Row.AsString(Map.Key);
                    if (!string.IsNullOrEmpty(Name))
                    {
                        if (this.Contains(Name))
                            continue;

                        Item = this.Add(Name);

                        if (!string.IsNullOrEmpty(Map.OwnerKey))
                            Item.SchemaOwner = Row.AsString(Map.OwnerKey);

                        if (Map.HasSql)
                            Item.SourceCode = Row.AsString(Map.Definition);
                    }

                }
            }
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaViews()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Views";
            fKind = MetaNodeKind.Views;
        }

        /* properties */
        /// <summary>
        /// Gets the owner metastore
        /// </summary>
        public Metastore Store { get { return Owner as Metastore; } }
    }
}
