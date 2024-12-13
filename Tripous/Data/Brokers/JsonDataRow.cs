namespace Tripous.Data
{

    /// <summary>
    /// JsonDataRow
    /// </summary>
    public class JsonDataRow
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataRow()
        { 
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataRow(DataRow Source)
        {
            this.Data = Source.ItemArray;
            this.State = Source.RowState;
        }

        /* properties */
        /// <summary>
        /// DataRowState.ItemArray
        /// </summary>
        public object[] Data { get; set; }
        /// <summary>
        /// DataRowState as integer
        /// </summary>
        public DataRowState State { get; set; }
    }
}
