namespace WebLib.Models
{
    /// <summary>
    /// A model for an Accordion control
    /// </summary>
    public class ViewAccordionModel
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordionModel(ViewDef ViewDef, ViewAccordionDef Def)
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
        /// The definition associated to this model
        /// </summary>
        public ViewAccordionDef Def { get; }
    }
}
