namespace Tripous.Data.Metadata
{

    /// <summary>
    /// A base class for the Field, Column, Trigger etc classes
    /// </summary>
    public class MetaItem : NamedItem
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaItem()
        {
        }


        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
    }
 
}
