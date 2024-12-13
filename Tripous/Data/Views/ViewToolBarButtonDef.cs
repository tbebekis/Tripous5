namespace Tripous.Data
{

    /// <summary>
    /// Indicates the mode (existence and position) of the icon of a button.
    /// </summary>
    public enum ViewToolBarButtonDefIcoMode
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Left
        /// </summary>
        Left = 1,
        /// <summary>
        /// Top
        /// </summary>
        Top = 2,
    }



    /// <summary>
    /// Represents a tool-bar button in a view
    /// </summary>
    public class ViewToolBarButtonDef
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewToolBarButtonDef()
        {
        }
 


        /* public */
        /// <summary>
        /// Override
        /// </summary>
        public override string ToString()
        {
            string S = !string.IsNullOrWhiteSpace(Text) ? Text : Command;
            return base.ToString();
        }
        /// <summary>
        /// Creates and returns the value of the data-setup HTML attribute of the button
        /// </summary>
        public string GetDataSetupText()
        {
            Dictionary<string, object> Setup = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(Command))
                Setup["Command"] = Command;

            if (!string.IsNullOrWhiteSpace(Text))
                Setup["Text"] = Text;

            if (!string.IsNullOrWhiteSpace(ToolTip))
                Setup["ToolTip"] = ToolTip;

            if (!string.IsNullOrWhiteSpace(Url))
                Setup["Url"] = Url;

            if (!string.IsNullOrWhiteSpace(IcoClasses))
                Setup["IcoClasses"] = IcoClasses;

            if (!string.IsNullOrWhiteSpace(ImageUrl))
                Setup["ImageUrl"] = ImageUrl;

            Setup["IcoMode"] = IcoMode.ToString();
            Setup["NoText"] = NoText;

            string Result = Json.Serialize(Setup);
            return Result;
        }

        /* properties */
        /// <summary>
        /// A user defined string. The command to execute when clicked.
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Caption text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Tool tip text 
        /// </summary>
        public string ToolTip { get; set; }
        /// <summary>
        /// The value of the href property when this button ends up creating an anchor element.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Ico css classes, e.g. fa fa-xxxxx, for the ico
        /// </summary>
        public string IcoClasses { get; set; }
        /// <summary>
        /// The url for an ico
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Indicates whether the text is visible. Defaults to true.
        /// </summary>
        public bool NoText { get; set; } = true;
        /// <summary>
        /// Indicates the mode (existence and position) of the icon of a button.
        /// </summary>
        public ViewToolBarButtonDefIcoMode IcoMode { get; set; } = ViewToolBarButtonDefIcoMode.Left;


        /* static properties */
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonHome
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Home",
                    Text = Res.GS("Home"),
                    ToolTip = Res.GS("Home"),
                    Url = "",
                    IcoClasses = "fa fa-home",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonList
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "List",
                    Text = Res.GS("List"),
                    ToolTip = Res.GS("List"),
                    Url = "",
                    IcoClasses = "fa fa-list-alt",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonFilters
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Filters",
                    Text = Res.GS("Filters"),
                    ToolTip = Res.GS("Filters"),
                    Url = "",
                    IcoClasses = "fa fa-search",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }

        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonFirst
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "First",
                    Text = Res.GS("First"),
                    ToolTip = Res.GS("First"),
                    Url = "",
                    IcoClasses = "fa fa-step-backward",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonPrior
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Prior",
                    Text = Res.GS("Prior"),
                    ToolTip = Res.GS("Prior"),
                    Url = "",
                    IcoClasses = "fa fa-caret-left",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonNext
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Next",
                    Text = Res.GS("Next"),
                    ToolTip = Res.GS("Next"),
                    Url = "",
                    IcoClasses = "fa fa-caret-right",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonLast
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Last",
                    Text = Res.GS("Last"),
                    ToolTip = Res.GS("Last"),
                    Url = "",
                    IcoClasses = "fa fa-step-forward",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }

        /// <summary>
        /// Creates and returns a list of the four navigation buttons
        /// </summary>
        static public List<ViewToolBarButtonDef> NavigationButtons
        {
            get
            {
                return new List<ViewToolBarButtonDef>
                {
                    ButtonFirst,
                    ButtonPrior,
                    ButtonNext,
                    ButtonLast
                };
            }
        }

        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonEdit
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Edit",
                    Text = Res.GS("Edit"),
                    ToolTip = Res.GS("Edit"),
                    Url = "",
                    IcoClasses = "fa fa-edit",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonInsert
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Insert",
                    Text = Res.GS("Insert"),
                    ToolTip = Res.GS("Insert"),
                    Url = "",
                    IcoClasses = "fa fa-plus",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonDelete
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Delete",
                    Text = Res.GS("Delete"),
                    ToolTip = Res.GS("Delete"),
                    Url = "",
                    IcoClasses = "fa fa-minus",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonSave
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Save",
                    Text = Res.GS("Save"),
                    ToolTip = Res.GS("Save"),
                    Url = "",
                    IcoClasses = "fa fa-floppy-o",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }

        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonCancel
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Cancel",
                    Text = Res.GS("Cancel"),
                    ToolTip = Res.GS("Cancel"),
                    Url = "",
                    IcoClasses = "fa fa-times",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
        /// <summary>
        /// Creates and returns a standard button
        /// </summary>
        static public ViewToolBarButtonDef ButtonClose
        {
            get
            {
                return new ViewToolBarButtonDef()
                {
                    Command = "Close",
                    Text = Res.GS("Close"),
                    ToolTip = Res.GS("Close"),
                    Url = "",
                    IcoClasses = "fa fa-sign-out",
                    ImageUrl = "",
                    NoText = true,
                    IcoMode = ViewToolBarButtonDefIcoMode.Left
                };
            }
        }
    }
}
