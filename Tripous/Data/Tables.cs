namespace Tripous.Data
{

    /// <summary>
    /// A list of MemTable instances
    /// </summary>
    public class Tables: NamedItems<MemTable>
    {
 

        /// <summary>
        /// Inserts Item at Index
        /// </summary>
        protected override void InsertItem(int Index, MemTable Item)
        {
            if ((DataSet != null) && (Item.DataSet == null))
            {
                DataSet.Tables.Add(Item);
            }

            base.InsertItem(Index, Item);
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public Tables()
        {
        }
        /// <summary>
        /// Constructor. The DataSet passed here is given as the DataSet to any
        /// added Table which has no DataSet defined.
        /// </summary>
        public Tables(DataSet DataSet)
        {
            this.DataSet = DataSet;
        }

        /// <summary>
        /// Returns the content of this collection as a Dictionary
        /// </summary>
        public Dictionary<string, DataTable> ToDictionary()
        {
            Dictionary<string, DataTable> Result = new Dictionary<string, DataTable>();

            foreach (MemTable Table in this)
                Result[Table.Name] = Table;

            return Result;
        }


        /// <summary>
        /// Returns the DataSet. It may be null. This DataSet is given as the DataSet
        /// to any added Table which has not DataSet defined.
        /// </summary>
        [JsonIgnore]
        public DataSet DataSet { get; protected set; }
    }
}
