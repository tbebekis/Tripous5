namespace Tripous.Data
{

    /// <summary>
    /// Represents a panel in a container.
    /// <para>Container could be a <see cref="ViewPanelListDef"/>, a <see cref="ViewTabControlDef"/> or a <see cref="ViewAccordionDef"/> </para>
    /// </summary>
    public class ViewDefContainerPanel : ViewDefComponent
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDefContainerPanel()
        {
        }

        /* public */
        /// <summary>
        /// Adds and returns a <see cref="ViewTabPageDef"/>
        /// <para>NOTE: Creates the container if is null.</para>
        /// </summary>
        public ViewTabPageDef AddTabPage(string TitleKey, string Name = "")
        {
            if (TabControl == null)
                TabControl = new ViewTabControlDef();

            ViewTabPageDef Result = TabControl.Add(TitleKey, Name);
            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewAccordionPanelDef"/>
        /// <para>NOTE: Creates the container if is null.</para>
        /// </summary>
        public ViewAccordionPanelDef AddGroup(string TitleKey, string Name = "")
        {
            if (Accordion == null)
                Accordion = new ViewAccordionDef();

            ViewAccordionPanelDef Result = Accordion.Add(TitleKey, Name);
            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewRowDef"/>
        /// <para>NOTE: Creates the rows list if is null.</para>
        /// </summary>
        public ViewRowDef AddRow(string TableName = "")
        {
            if (Rows == null)
                Rows = new List<ViewRowDef>();

            ViewRowDef Result = new ViewRowDef();
            Result.TableName = TableName;
            Rows.Add(Result);
            return Result;
        }


        /* properties */
        /// <summary>
        /// A tab control
        /// </summary>
        public ViewTabControlDef TabControl { get; set; }
        /// <summary>
        /// An accordeon control
        /// </summary>
        public ViewAccordionDef Accordion { get; set; }
        /// <summary>
        /// A list of rows. Could be empty.
        /// <para>A row is a panel. It may contain a grid or columns with controls (control rows).</para>
        /// </summary>
        public List<ViewRowDef> Rows { get; set; }
    }

}
