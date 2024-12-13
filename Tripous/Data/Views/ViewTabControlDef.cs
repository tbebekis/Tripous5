namespace Tripous.Data
{

    /// <summary>
    /// Represents a tab control
    /// </summary>
    public class ViewTabControlDef : ViewDefContainer<ViewTabPageDef>
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewTabControlDef()
        {
        }
    }



    /// <summary>
    /// Represents a page in a <see cref="ViewTabControlDef"/>
    /// </summary>
    public class ViewTabPageDef: ViewDefContainerPanel
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewTabPageDef()
        {
        }

    }
}
