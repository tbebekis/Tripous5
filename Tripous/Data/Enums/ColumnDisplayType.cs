namespace Tripous.Data 
{
    /// <summary>
    /// The display type of a column. Used with grids.
    /// </summary>
    public enum ColumnDisplayType
    {
        /// <summary>
        /// Whatever the underlying field is
        /// </summary>
        Default = 0,
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime = 1,
        /// <summary>
        /// Date
        /// </summary>
        Date = 2,
        /// <summary>
        /// Time
        /// </summary>
        Time = 3,
        /// <summary>
        /// CheckBox
        /// </summary>
        CheckBox = 4,
        /// <summary>
        /// Memo
        /// </summary>
        Memo = 5,
        /// <summary>
        /// Image
        /// </summary>
        Image = 6,
    }
}
