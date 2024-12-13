namespace Tripous.Data.Metadata
{


    /// <summary>
    /// Represents a meta-data information entity
    /// </summary>
    public class MetaTriggers : MetaItems<MetaTrigger>
    {
        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            if (Store == null)
                return;

            DataTable Table = this.Table.tblTriggers;

            if ((Table.Columns.Count > 0) && (Table.Rows.Count > 0))
            {
                //sql server
                string key = "TRIGGER_NAME";
                string tableKey = "TABLE_NAME";
                string bodyKey = "TRIGGER_BODY";
                string eventKey = "TRIGGERING_EVENT";
                string triggerTypeKey = "TRIGGER_TYPE";
                string ownerKey = "OWNER";

                //firebird
                if (!Table.Columns.Contains(ownerKey))
                    ownerKey = null;

                if (!Table.Columns.Contains(bodyKey))
                    bodyKey = "SOURCE";

                if (!Table.Columns.Contains(eventKey))
                    eventKey = "TRIGGER_TYPE";

                if (!Table.Columns.Contains(bodyKey))
                    bodyKey = "BODY";


                if (!Table.Columns.Contains(tableKey))
                    tableKey = null;
                if (!Table.Columns.Contains(bodyKey))
                    bodyKey = null;
                if (!Table.Columns.Contains(eventKey))
                    eventKey = null;
                if (!Table.Columns.Contains(triggerTypeKey))
                    triggerTypeKey = null;

                string Name;
                MetaTrigger Item;

                DataView View = Table.DefaultView;
                if (!string.IsNullOrEmpty(tableKey) && Table.ContainsColumn(tableKey))
                {
                    View.RowFilter = string.Format("{0} = '{1}'", tableKey, this.Table.Name);
                    View.Sort = tableKey;
                }

                DataRow Row;
                //foreach (DataRow Row in Table.Rows)
                foreach (DataRowView RowView in View)
                {
                    Row = RowView.Row;

                    Name = Row.AsString(key);
                    if (!string.IsNullOrEmpty(Name))
                    {

                        Item = this.Find(Name);

                        if (Item == null)
                        {
                            Item = this.Add(Name);
                        }

                        if (!string.IsNullOrEmpty(bodyKey))
                            Item.SourceCode = Row.AsString(bodyKey);

                        if (!string.IsNullOrEmpty(eventKey))
                            Item.TriggerEvent = Row.AsString(eventKey);

                        if (!string.IsNullOrEmpty(triggerTypeKey))
                        {
                            Item.TriggerType = Row.AsString(triggerTypeKey);

                            if (this.Store.Provider.ServerType == SqlServerType.Firebird)
                            {
                                if (!string.IsNullOrEmpty(Item.TriggerType))
                                {
                                    switch (Item.TriggerType)
                                    {
                                        case "1":
                                            Item.TriggerType = "BEFORE";
                                            Item.TriggerEvent = "INSERT";
                                            break;
                                        case "2":
                                            Item.TriggerType = "AFTER";
                                            Item.TriggerEvent = "INSERT";
                                            break;
                                        case "3":
                                            Item.TriggerType = "BEFORE";
                                            Item.TriggerEvent = "UPDATE";
                                            break;
                                        case "4":
                                            Item.TriggerType = "AFTER";
                                            Item.TriggerEvent = "UPDATE";
                                            break;
                                        case "5":
                                            Item.TriggerType = "BEFORE";
                                            Item.TriggerEvent = "DELETE";
                                            break;
                                        case "6":
                                            Item.TriggerType = "AFTER";
                                            Item.TriggerEvent = "DELETE";
                                            break;
                                    }
                                }

                            }
                        }
                    }


                }
            }
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaTriggers()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Triggers";
            fKind = MetaNodeKind.Triggers;
        }

        /* properties */
        /// <summary>
        /// Gets the owner table
        /// </summary>
        public MetaTable Table { get { return fOwner as MetaTable; } }
        /// <summary>
        /// Gets the owner metastore
        /// </summary>
        public Metastore Store { get { return Table != null ? Table.Store : null; } }
    }
}
