namespace WebLib.Models
{
    /// <summary>
    /// A model for a row
    /// </summary>
    public class ViewRowModel
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewRowModel(ViewDef ViewDef, ViewRowDef Def)
        {
            this.ViewDef = ViewDef;
            this.Def = Def;

        }

        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The definition of the row.
        /// </summary>
        public ViewRowDef Def { get; }
    }
}
