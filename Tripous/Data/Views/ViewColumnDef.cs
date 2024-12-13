namespace Tripous.Data
{


    /// <summary>
    /// Represents a column in any ui container.
    /// <para>May contain: Controls.</para>
    /// </summary>
    public class ViewColumnDef: ViewDefComponent
    {
        /// <summary>
        /// Constant. Default column and width classes
        /// </summary>
        public const string DefaultCssClasses = "Col ";

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewColumnDef()
        {
        }

        /// <summary>
        /// Returns the content of the <see cref="ViewDefComponent.CssClasses"/> list as text.
        /// </summary>
        public override string GetCssClassesText()
        {
            string Text = base.GetCssClassesText();
            return !string.IsNullOrWhiteSpace(Text) ? Text : DefaultCssClasses;            
        }

        /// <summary>
        /// Adds and returns a <see cref="ViewControlDef"/>
        /// </summary>
        public ViewControlDef AddControl(string TypeName, string TitleKey, string DataField = "", string TableName = "", object Properties = null)
        {
            ViewControlDef Result = new ViewControlDef(TypeName, TitleKey, DataField, TableName, Properties);
            Controls.Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewControlDef"/>
        /// </summary>
        public ViewControlDef AddControl(ViewControlDef ControlDef)
        {
            Controls.Add(ControlDef);
            return ControlDef;
        }
 
        /* properties */
        /// <summary>
        /// A list of control rows.  
        /// </summary>
        public List<ViewControlDef> Controls { get; } = new List<ViewControlDef>();
    }


 
}
