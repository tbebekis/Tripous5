namespace Tripous.Data.Metadata
{


    /// <summary>
    /// Represents a meta-data information entity
    /// </summary>
    public class MetaTypes : MetaItems<MetaType>
    {
        /* overrides */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected override void DoLoad()
        {
            if (Store == null)
                return;

            DataTable Table = Store.GetCollection(DbMetaDataCollectionNames.DataTypes, null);

            if (Table.ContainsColumn("TypeName") && Table.ContainsColumn("DataType"))
            {

                bool IsMsAccess = (Store.Provider.ServerType == SqlServerType.Access)
                    && Table.ContainsColumn("NativeDataType");

                MetaType Item;
                foreach (DataRow Row in Table.Rows)
                {
                    Item = this.Add(Row.AsString("TypeName"));
                    Item.NetType = Row.AsString("DataType");

                    if (IsMsAccess)
                    {
                        Item.MetaTag = Row.AsString("NativeDataType");
                    }
                }
            }
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaTypes()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Types";
            fKind = MetaNodeKind.Types;
        }

        /* public */
        /// <summary>
        /// Finds and returns a meta data type by its tag, if any, else null
        /// </summary>
        public MetaType FindByMetaTag(string MetaTag)
        {
            Lock();
            try
            {
                return this.FirstOrDefault(Item => Sys.IsSameText(Item.MetaTag, MetaTag));
            }
            finally
            {
                UnLock();
            }
        }

        /* properties */
        /// <summary>
        /// Gets the owner metastore
        /// </summary>
        public Metastore Store { get { return Owner as Metastore; } }
    }
}
