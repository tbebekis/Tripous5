namespace Tripous.Data
{

    /// <summary>
    /// Represents an accordeon control
    /// </summary>
    public class ViewAccordionDef : ViewDefContainer<ViewAccordionPanelDef>
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordionDef()
        {
        }
    }


    /// <summary>
    /// Represents a panel in a <see cref="ViewAccordionDef"/>
    /// </summary>
    public class ViewAccordionPanelDef: ViewDefContainerPanel
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordionPanelDef()
        {
        }

    }
}
