namespace Tripous.Data
{
    /// <summary>
    /// Base class for all view component defs
    /// </summary>
    public class ViewDefComponent
    {
        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDefComponent()
        {
        }


        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name) ? Name : (!string.IsNullOrWhiteSpace(Title) ? Title : base.ToString());
        }

        /// <summary>
        /// Assigns properties to a data-setup object
        /// </summary>
        public virtual void AssignTo(Dictionary<string, object> DataSetup)
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                DataSetup["Name"] = Name;
            }
                

            if (!string.IsNullOrWhiteSpace(Title))
            {
                DataSetup["Title"] = Title;
            }                

            if (!string.IsNullOrWhiteSpace(TableName))
                DataSetup["TableName"] = TableName;

            if (CssClasses != null & CssClasses.Count > 0)
                DataSetup["CssClasses"] = string.Join(" ", CssClasses.ToArray());

            foreach (var Entry in Properties)
                DataSetup[Entry.Key] = Entry.Value;
        }
        /// <summary>
        /// Serializes this instance in order to properly used as a data-setup html attribute.
        /// </summary>
        public virtual string GetDataSetupText()
        {
            Dictionary<string, object> Temp = new Dictionary<string, object>();
            AssignTo(Temp);
            string JsonText = Json.Serialize(Temp, Json.CreateDefaultSettings(false));
            return JsonText;
        }
        
 
        /// <summary>
        /// Adds one or more CSS classes to the <see cref="CssClasses"/> list.
        /// <para>Css classes are specified as a list of space delimited strings. </para>
        /// </summary>
        public void AddCssClasses(string Text)
        {
            if (CssClasses == null)
                CssClasses = new List<string>();

            if (!string.IsNullOrWhiteSpace(Text))
            {
                string[] Parts = Text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (Parts != null && Parts.Length > 0)
                {
                    foreach (string CssClass in Parts)
                        if (!string.IsNullOrWhiteSpace(CssClass) && !CssClasses.Contains(CssClass))
                            CssClasses.Add(CssClass);
                }
            } 
        }
        /// <summary>
        /// Removes one or more CSS classes from the <see cref="CssClasses"/> list.
        /// <para>Css classes are specified as a list of space delimited strings. </para>
        /// </summary>
        public void RemoveCssClasses(string Text)
        {
            if (CssClasses == null)
                CssClasses = new List<string>();

            if (!string.IsNullOrWhiteSpace(Text))
            {
                string[] Parts = Text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (Parts != null && Parts.Length > 0)
                {
                    foreach (string CssClass in Parts)
                        if (!string.IsNullOrWhiteSpace(CssClass) && !CssClasses.Contains(CssClass))
                            CssClasses.Remove(CssClass);
                }
            } 
        }
        /// <summary>
        /// Returns the content of the <see cref="CssClasses"/> list as text.
        /// </summary>
        public virtual string GetCssClassesText()
        {
            return CssClasses != null & CssClasses.Count > 0 ? string.Join(" ", CssClasses.ToArray()) : "";
        }
        /// <summary>
        /// Returns the content of the <see cref="Style"/> list as text, e.g <code>style="width: 100%; height: 200px; color: black;"</code>.
        /// <para>The result text becomes the inline css style of the element this instance represents.</para>
        /// </summary>
        public virtual string GetCssStyleText()
        {
            string Result = "";
            
            if (Style != null && Style.Count > 0)
            {
                StringBuilder SB = new StringBuilder();

                foreach (var Entry in Style)
                    SB.Append($"{Entry.Key}: {Entry.Value}; ");

                if (SB.Length > 0)
                    Result = $"style=\"{SB}\"";
            }
 

            return Result;
        }

        /* properties */
        /// <summary>
        /// A unique name among all siblings. 
        /// <para>This unique identifier is used by the serialization system.</para>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public virtual string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : (!string.IsNullOrWhiteSpace(Name)? Name: Sys.None); }
            set { }
        }

        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// A list of css classes for the view
        /// </summary>
        public List<string> CssClasses { get; set; } = new List<string>();

        /// <summary>
        /// Helper indexer for the Properties property.
        /// <para>Properties property is a Dictionary with properties of the data-setup attribute of this component. </para>
        /// <para>NOTE: The data-setup of a control row has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
        /// </summary>
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key) ? Properties[Key] : null; }
            set { Properties[Key] = value; }
        }
        /// <summary>
        /// Dictionary with properties of the data-setup attribute of this component. 
        /// <para>NOTE: The data-setup of a control row has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();
        /// <summary>
        /// Dictionary with css style properties
        /// </summary>
        public Dictionary<string, object> Style { get; } = new Dictionary<string, object>();
    }
 
}
