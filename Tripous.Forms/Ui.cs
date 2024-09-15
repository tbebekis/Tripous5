using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Data;

 

using Tripous.Data;

namespace Tripous.Forms
{
    /// <summary>
    /// Helper
    /// </summary>
    static public partial class Ui
    {
        static int fWaiting;
        static int fWaitCursor;
 

        static object syncLock = new LockObject();

 

        /// <summary>
        /// The id of the primary thread
        /// </summary>
        static public readonly int PrimaryThreadId = Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// Fake initialization
        /// </summary>
        static public void Initialize()
        {
            // fake
        }

        /* message boxes */
        /// <summary>
        /// Displays a message dialog box
        /// </summary>
        static private DialogResult Box(string Message, MessageBoxType Kind, string MemoFonts)
        {
            //Wait.ForceClose();

            return MessageBoxDialog.Show(Message, Kind, MemoFonts); 
        }
        /// <summary>
        /// Displays a message dialog box
        /// </summary>
        static private bool Box(string Message, MessageBoxType Kind)
        {
            DialogResult Result = Box(Message, Kind, "Tahoma");
            return (Result == DialogResult.OK) || (Result == DialogResult.Yes);
        }

        /// <summary>
        /// Displays an information dialog box
        /// </summary>
        static public void InfoBox(string Message)
        {
            Box(Message, MessageBoxType.Info);
        }
        /// <summary>
        /// Displays an information dialog box
        /// </summary>
        static public void InfoBox(string Message, params object[] Args)
        {
            InfoBox(string.Format(Message, Args));
        }

        /// <summary>
        /// Displays an error dialog box
        /// </summary>
        static public void ErrorBox(Exception Exception)
        {
            ErrorBox(Sys.ExceptionText(Exception));
        }
        /// <summary>
        /// Displays an error dialog box
        /// </summary>
        static public void ErrorBox(string Message)
        {
            Box(Message, MessageBoxType.Error, "Courier New");
        }
        /// <summary>
        /// Displays an error dialog box
        /// </summary>
        static public void ErrorBox(string Message, params object[] Args)
        {
            ErrorBox(string.Format(Message, Args));
        }

        /// <summary>
        /// Displays a yes-no dialog box
        /// </summary>
        static public bool YesNoBox(string Message)
        {
            return Box(Message, MessageBoxType.YesNo);
        }
        /// <summary>
        /// Displays a yes-no dialog box
        /// </summary>
        static public bool YesNoBox(string Message, params object[] Args)
        {
            return YesNoBox(string.Format(Message, Args));
        }

        /// <summary>
        /// Displays an information dialog box with Courier New font.
        /// </summary>
        static public bool CourierBox(string Text)
        {
            return Box(Text, MessageBoxType.Info, "Courier New") == DialogResult.OK;
        }
        /// <summary>
        /// Displays an information dialog box with Courier New font.
        /// </summary>
        static public bool CourierBox(string Text, params object[] Args)
        {
            return CourierBox(string.Format(Text, Args));
        }

        /// <summary>
        /// Displays the dialog box. Returns true if the user selects a row.
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public bool TableBox(DataTable Table, out DataRow Row, string Text)
        {
            Row = null;
            return TableBoxDialog.Execute(Table, out Row, Text);
        }
        /// <summary>
        /// Displays the dialog box. Returns true if the user selects a row.
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public bool TableBox(DataTable Table, out DataRow Row)
        {
            Row = null;
            return TableBoxDialog.Execute(Table, out Row);
        }
        /// <summary>
        /// Displays the dialog box. 
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public void TableBox(DataTable Table, string Text)
        {
            DataRow Row;
            TableBox(Table, out Row, Text);
        }
        /// <summary>
        /// Displays the dialog box. 
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public void TableBox(DataTable Table)
        {
            DataRow Row;
            TableBox(Table, out Row);
        }
        /// <summary>
        /// Displays the dialog box. 
        /// </summary>
        static public void RowBox(DataRow SourceRow)
        {
            TableBoxDialog.Execute(SourceRow);
        }

        /* PickRows dialog */
        /// <summary>
        /// Displays a row pick list dialog box to the user. Returns true if the user clicks on the OK button of the dialog.
        /// <para>Table is the Table to pick up rows from. </para>
        /// <para>SelectedRows contains the rows that are initially displayed as selected to the user. Up on succesful
        /// return it contains the rows selected by the user.</para>
        /// <para>VisibleColumns is an array of column names of the Table.</para>
        /// </summary>
        static public bool PickRows(DataTable Table, List<DataRow> SelectedRows, string[] VisibleColumns, string Title = null)
        {
            return MultiRowPickDialog.ShowDialog(Table, SelectedRows, VisibleColumns, Title);
        }
        /// <summary>
        /// Displays a row pick list dialog box to the user. Returns true if the user clicks on the OK button of the dialog.
        /// <para>ConnectionName and SqlText are used to select a Table in order to pick up rows from. </para>
        /// <para>SelectedRows contains the rows that are initially displayed as selected to the user. Up on succesful
        /// return it contains the rows selected by the user.</para>
        /// <para>VisibleColumns is an array of column names of the Table.</para>
        /// </summary>
        static public bool PickRows(string ConnectionName, string SqlText, List<DataRow> SelectedRows, string[] VisibleColumns, string Title = null)
        {
            return MultiRowPickDialog.ShowDialog(ConnectionName, SqlText, SelectedRows, VisibleColumns, Title);
        }
        /// <summary>
        /// Displays a dialog box for the user to pick up rows. Returns the selected rows of null if no selection is made.
        /// <para>tblSource is the DataTable where selected rows come from.</para>
        /// <para>tblTarget is the DataTable where selected rows are going to be inserted some how.</para>
        /// <para>VisibleColumns is an array of tblSource visible comlumn names.</para>
        /// <para>TargetKeyName and SourceKeyName are key field names used to initially find what source rows are already selected in target.</para>
        /// </summary>
        static public List<DataRow> PickRows(DataTable tblTarget, DataTable tblSource, string[] VisibleColumns, string TargetKeyName, string SourceKeyName, string Title = null)
        {
            List<DataRow> SelectedRows = new List<DataRow>();

            DataRow OutRow = null;
            foreach (DataRow SourceRow in tblSource.Rows)
            {
                if (tblTarget.Locate(TargetKeyName, new object[] { SourceRow[SourceKeyName] }, LocateOptions.None, out OutRow))
                    SelectedRows.Add(SourceRow);
            }

            if (MultiRowPickDialog.ShowDialog(tblSource, SelectedRows, VisibleColumns, Title)) 
                return SelectedRows;

            return null;
        }

        /// <summary>
        /// Displays a dialog box for selecting a single DataRow.
        /// </summary>
        static public DataRow PickRow(DataTable Table, string[] VisibleColumns = null, string Title = null, string KeyFieldName = null, object KeyValue = null)
        {
            return SingleRowPickDialog.ShowDialog(Table, VisibleColumns, Title, KeyFieldName, KeyValue);
        }
        /// <summary>
        /// Displays a dialog box for selecting a single DataRow.
        /// </summary>
        static public DataRow PickRow(string ConnectionName, string SqlText, string[] VisibleColumns = null, string Title = null, string KeyFieldName = null, object KeyValue = null)
        {
            return SingleRowPickDialog.ShowDialog(ConnectionName, SqlText, VisibleColumns, Title, KeyFieldName, KeyValue);
        }

        /* data-binding */
        /// <summary>
        /// Creates and returns a <see cref="Binding"/> object on PropName, having the DataSource as 
        /// its datasource and FieldName as the DataMember. After that, the Binding may added to
        /// the bindings collection of a Control.
        /// </summary>
        static public Binding Bind(string PropName, object DataSource, string FieldName)
        {
            Binding Result = new Binding(PropName, DataSource, FieldName, true);
            //Result.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            return Result;
        }
        /// <summary>
        /// Binds Control to DataSource.
        /// </summary>
        static public Binding Bind(Control Control, string PropertyName, object DataSource, string DataMember)
        {
            try
            {
                Binding Result = new Binding(PropertyName, DataSource, DataMember, true);
                Result.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                Control.DataBindings.Add(Result);
                return Result;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Binds Control to DataSource.
        /// <para>Automatically selects the "right" property of the Control based on its Type.</para>
        /// </summary>
        static public Binding Bind(Control Control, object DataSource, string DataMember)
        {
            Binding binding = null;

            if ((DataSource != null) && !string.IsNullOrEmpty(DataMember))
            {

                string PropertyName = string.Empty;

                if ((Control is TextBox)
                    || (Control is RichTextBox)
                    || (Control is Label))
                    PropertyName = "Text";
                else if ((Control is NumericUpDown)
                    || (Control is DateTimePicker)
                    //|| (Control is DateTimePickerEx)
                    )
                    PropertyName = "Value";
                else if ((Control is PictureBox)
                    //|| (Control is PictureBoxEx)
                    )
                    PropertyName = "Image";
                else if (Control is CheckBox)
                    PropertyName = Ui.CheckBoxBindProperty;
                //else if (Control is HtmlTextBox)
                //    PropertyName = "Html";
                else if (Control is ListControl)
                {
                    PropertyName = "SelectedItem";  // normal binding - NOT a look-up

                    if (Control is ComboBox)
                        (Control as ComboBox).DropDownStyle = ComboBoxStyle.DropDownList;
                }

                if (!string.IsNullOrWhiteSpace(PropertyName))
                {
                    binding = Bind(Control, PropertyName, DataSource, DataMember);
                    if (binding != null)
                    {
                        if (PropertyName.IsSameText("Text") || PropertyName.IsSameText("Html"))
                            binding.NullValue = string.Empty;
                        else if (PropertyName.IsSameText(Ui.CheckBoxBindProperty))
                            binding.NullValue = false;

                        /*
                                                if ((Control is PictureBox) || (Control is PictureBoxEx))
                                                {
                                                    binding.Parse += new ConvertEventHandler(Ui.ImageBinding_Parse);
                                                    binding.Format += new ConvertEventHandler(Ui.ImageBinding_Format);
                                                } 
                         */
                    }
                }
            }

            return binding;
        }
        /// <summary>
        /// Binds a ComboBox as a look-up control
        /// </summary>
        static public Binding BindLookUp(ComboBox Control, object DataSource, string DataMember, object ListSource, string DisplayMember, string ValueMember)
        {
            Binding binding = null;

            Control.DropDownStyle = ComboBoxStyle.DropDownList;

            Control.DataSource = ListSource;
            Control.DisplayMember = DisplayMember;
            Control.ValueMember = ValueMember;

            binding = Bind(Control, "SelectedValue", DataSource, DataMember);

            return binding;
        }
        /// <summary>
        /// Handler for an image Binding
        /// </summary>
        static public void ImageBinding_Format(object sender, ConvertEventArgs e)
        {
            if (!Sys.IsNull(e.Value) && (e.DesiredType == typeof(System.Drawing.Image)))
            {
                byte[] Bytes = (byte[])e.Value;

                MemoryStream MS = new MemoryStream(Bytes);
                e.Value = Image.FromStream(MS);
            }
        }
        /// <summary>
        /// Handler for an image Binding
        /// </summary>
        static public void ImageBinding_Parse(object sender, ConvertEventArgs e)
        {
            if (!Sys.IsNull(e.Value) && ((e.DesiredType == typeof(byte[])) || (e.DesiredType == typeof(object))))
            {
                Image img = e.Value as Image;
                if (img != null)
                {
                    MemoryStream MS = new MemoryStream();
                    img.Save(MS, System.Drawing.Imaging.ImageFormat.Gif);
                    e.Value = MS.ToArray();
                }
            }
        }

        /// <summary>
        /// Returns the value of the DataSource property of Instance, if it has a DataSource property.
        /// </summary>
        static public object DataSourceOf(object Instance)
        {
            if (Instance != null)
            {
                PropertyInfo PropInfo = Instance.GetType().GetProperty("DataSource");
                if (PropInfo != null)
                {
                    return PropInfo.GetValue(Instance, null);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the value of the DataMember property of Instance, if it has a DataMember property.
        /// </summary>
        static public string DataMemberOf(object Instance, string Default)
        {
            if (Instance != null)
            {
                PropertyInfo PropInfo = Instance.GetType().GetProperty("DataMember");
                if ((PropInfo != null) && (PropInfo.PropertyType == typeof(System.String)))
                {
                    object Value = PropInfo.GetValue(Instance, null);
                    return Value == null ? Default : Value.ToString();
                }
            }

            return Default;
        }
        /// <summary>
        /// Returns the value of the DataMember property of Instance, if it has a DataMember property.
        /// </summary>
        static public string DataMemberOf(object Instance)
        {
            return DataMemberOf(Instance, string.Empty);
        }

        /// <summary>
        /// Tries to extract the DataTable from the passed in DataSource and DataMember.
        /// Returns null on failure.
        /// </summary>
        static public DataTable DataTableOf(object DataSource, string DataMember)
        {
            while (DataSource != null)
            {
                if (DataSource is DataTable)
                    return DataSource as DataTable;

                if (DataSource is DataSet)
                {
                    if (!string.IsNullOrEmpty(DataMember))
                    {
                        if ((DataSource as DataSet).Tables.Contains(DataMember))
                            return (DataSource as DataSet).Tables[DataMember];

                        if ((DataSource as DataSet).Relations.Contains(DataMember))
                            return (DataSource as DataSet).Relations[DataMember].ChildTable;
                    }

                    return null;
                }

                /* it might be a BindingSource or something with a DataSource property */
                DataSource = DataSourceOf(DataSource);
                if (DataSource != null)
                {
                    if (string.IsNullOrEmpty(DataMember))
                        DataMember = DataMemberOf(DataSource, DataMember);
                }
            }

            return null;
        }
        
        /// <summary>
        /// Returns true if the Table tree (details - subdetails) is modified.
        /// </summary>
        static public bool IsTreeModified(DataTable Table)
        {
            if (Table != null)
            {
                if (Table.GetChanges() != null)
                    return true;
 


                /*  
                                foreach (DataRow Row in Table.Rows)
                                {
                                    if (Bf.Member(Row.RowState, DataRowState.Detached | DataRowState.Added | DataRowState.Modified | DataRowState.Deleted))
                                        return true;
                                }

                                if (Table is MemTable)
                                {
                                    foreach (MemTable DetailTable in (Table as MemTable).Details)
                                    {
                                        if (IsTreeModified(DetailTable))
                                            return true;
                                    }
                                } 
                 */

            }

            return false;
        }

        /* tool-strips */
        /// <summary>
        /// Creates a toolbar separator.
        /// <para>It sets up the separator and adds it to the toolbar.</para>
        /// </summary>
        static public ToolStripSeparator CreateToolStripSeparator(string SepName, ToolStripItemCollection Items)
        {
            ToolStripSeparator Result = new ToolStripSeparator();
            Result.Name = SepName;
            Items.Add(Result);
            return Result;
        }
        /// <summary>
        /// Sets the Visible property of each separator in a toolbar's items
        /// based on the visible buttons found before the separator.
        /// </summary>
        static public void ShowHideSeparators(ToolStripItemCollection Items)
        {
            int VisibleCount = 0;

            foreach (ToolStripItem Item in Items)
            {
                if (Item is ToolStripSeparator)
                {
                    (Item as ToolStripSeparator).Visible = VisibleCount > 0;
                    VisibleCount = 0;
                }
                else if (Item.Visible)
                {
                    VisibleCount++;
                }
            }
        }
        /// <summary>
        /// Creates a toolbar button (dropdown or normal button) based on the specified arguments.
        /// <para>It sets up the button properly and adds it to the specified Items (ToolStripItemCollection, could be a toolbar Items or a dropdown button DropDownItems).</para>
        /// <para>Tag, Items and Click can be null.</para>
        /// </summary>
        static public ToolStripItem CreateToolStripButton(string ButtonName,
                                                        ToolStripItemDisplayStyle DisplayStyle = ToolStripItemDisplayStyle.Image,
                                                        string Text = "",
                                                        Image Image = null,
                                                        EventHandler Click = null,
                                                        ToolStripItemCollection Items = null,
                                                        object Tag = null,
                                                        bool DropDown = false,
                                                        bool Checkable = false,
                                                        bool Checked = false)
        {
            ToolStripItem Result = DropDown ? (ToolStripItem)new ToolStripDropDownButton() : (ToolStripItem)new ToolStripButton();

            if (!string.IsNullOrWhiteSpace(ButtonName))
                Result.Name = ButtonName;
            if (!string.IsNullOrWhiteSpace(Text))
                Result.Text = Text;
            if (Image != null)
                Result.Image = Image;

            Result.DisplayStyle = DisplayStyle;
            Result.Tag = Tag;

            if (Checkable && (Result is ToolStripButton))
            {
                (Result as ToolStripButton).CheckOnClick = true;
                (Result as ToolStripButton).Checked = Checked;
                (Result as ToolStripButton).CheckState = Checked ? CheckState.Checked : CheckState.Unchecked;
            }

            if (Click != null)
                Result.Click += Click;

            if (Items != null)
                Items.Add(Result);

            return Result;
        }

        /* image handling */
        /// <summary>
        /// Converts an image to a base64 string
        /// </summary>
        static public string ImageToBase64(Image Image, bool InsertLineBreaks = true)
        {
            if (Image != null)
            {
                Base64FormattingOptions Options = InsertLineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None;

                using (MemoryStream MS = new MemoryStream())
                {
                    Image.Save(MS, Image.RawFormat);
                    byte[] Bytes = MS.ToArray();
                    return Convert.ToBase64String(Bytes, Options);
                }
            }

            return null;
        }
        /// <summary>
        /// Converts a base64 string back to an image
        /// </summary>
        static public Image Base64ToImage(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                byte[] Bytes = Convert.FromBase64String(Text);

                if ((Bytes != null) && (Bytes.Length > 0))
                {
                    MemoryStream MS = new MemoryStream(Bytes, 0, Bytes.Length);
                    MS.Write(Bytes, 0, Bytes.Length);
                    return Image.FromStream(MS, true);
                }
            }

            return null;
        }

        /* from DataRow extensions */
        /// <summary>
        /// Saves an image into a blob field
        /// </summary>
        static public void ImageToBlob(this DataRow Row, string FieldName, Image Image)
        {
            if (Image != null)
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    Image.Save(MS, Image.RawFormat);
                    Row.StreamToBlob(FieldName, MS);
                }
            }
        }
        /// <summary>
        /// Reads a blob field and returns an image. 
        /// <para>WARNING: Returns null if field is null</para>
        /// </summary>
        static public Image BlobToImage(this DataRow Row, string FieldName)
        {
            MemoryStream MS = Row.BlobToStream(FieldName);
            if (MS.Length > 0)
            {
                MS.Position = 0;
                Image Result = Image.FromStream(MS);
                return Result;
            }
            return null;
        }

        /* miscs */
        /// <summary>
        /// Triggers the UnhandledExceptionThrown event. This gives the chance to any listener code,
        /// such as the WaitForm or the Ui class that displays a wait curstor or any other else, 
        /// to ajust itself and the ui it controls.
        /// </summary>
        static public void OnUnhandledExceptionThrown(Exception Ex)
        {
            if (InMainThread && (UnhandledExceptionThrown != null))
            {
                UnhandledExceptionThrown(null, Ex);
            }
        }

        /// <summary>
        /// Returns true if the ColumnName is Id or ends with Id
        /// </summary>
        static public bool IsIdColumn(string ColumnName)
        {
            return !string.IsNullOrEmpty(ColumnName) && ColumnName.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// Returns true if ColumnName is not Id or ends with Id
        /// </summary>
        static public bool IsVisibleColumn(string ColumnName)
        {
            return !IsIdColumn(ColumnName);
        }

        /// <summary>
        /// Finds and returns the focused control
        /// </summary>
        static public Control FindFocusedControl(Control control)
        {
            var container = control as ContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as ContainerControl;
            }
            return control;
        }

        /// <summary>
        /// Returns the "screen mode" of a container control.
        /// </summary>
        static public ScreenMode GetScreenMode(Control Container)
        {
            /*
None: 0,
XSmall: 1,     //    0 ..  767
Small: 2,      //  768 ..  991
Medium: 4,     //  992 .. 1200
Large: 8       // 1201 .. 
*/

            ScreenMode Result = ScreenMode.None;

            if (Container != null)
            {
                int W = Container.ClientRectangle.Width;

                if (W <= 767)
                    Result = ScreenMode.XSmall;
                else if (W <= 991)
                    Result = ScreenMode.Small;
                else if (W <= 1200)
                    Result = ScreenMode.Medium;
                else
                    Result = ScreenMode.Large;
            }

            return Result;
        }
        /* properties */
        /// <summary>
        /// The main form of the application, if any, else null
        /// </summary>
        static public Form MainForm { get; set; }



        /// <summary>
        /// Gets a value indicating whether the caller must call an invoke method when making method calls 
        /// </summary>
        static public bool InvokeRequired { get { return PrimaryThreadId != Thread.CurrentThread.ManagedThreadId; } }
        /// <summary>
        /// Returns true if it is called by a code which is executed by the main thread.
        /// </summary>
        static public bool InMainThread { get { return !InvokeRequired; } }

        /// <summary>
        /// The property of a checkbox to use, when binding a checkbox
        /// </summary>
        static public string CheckBoxBindProperty { get; set; } = "CheckState";


        /* wait and cursors */
        /// <summary>
        /// Gets or sets a boolean value indicating whether the application performs a lengthy operation
        /// </summary>
        static public bool Waiting
        {
            get { lock(syncLock) return fWaiting > 0; }
            set
            {
                lock (syncLock)
                {
                    if (value)
                    {
                        fWaiting++;
                        if (fWaiting == 1)
                        {
                            WaitCursor = true;
                            Broadcaster.Post(ApplicationWaiting, new Dictionary<string, object>() { { "Value", true } });
                        }
                    }
                    else
                    {
                        fWaiting--;
                        if (fWaiting < 0)
                            fWaiting = 0;
                        if (fWaiting == 0)
                        {
                            WaitCursor = false;
                            Broadcaster.Post(ApplicationWaiting, new Dictionary<string, object>() { { "Value", false } });
                        }
                    }
                }

            }
        }
        /// <summary>
        /// Gets or sets the wait cursor.
        /// </summary>
        static bool WaitCursor
        {
            get { return fWaitCursor > 0; }
            set
            {
                if (InMainThread)
                {
                    if (value)
                    {
                        fWaitCursor++;
                        if (fWaitCursor == 1)
                            Cursor.Current = Cursors.WaitCursor;
                    }
                    else
                    {
                        fWaitCursor--;
                        if (fWaitCursor < 0)
                            fWaitCursor = 0;
                        if (fWaitCursor == 0)
                            Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        /// <summary>
        /// Handles string dates and partial dates written in a text box.
        /// <para>Assumes that date format is <c>yyyy-MM-dd</c> or <c>dd-MM-yyyy</c>.</para>
        /// <para>User may use <c>-, ., or /</c> as date separators.</para>
        /// </summary>
        static public void HandleDateTextBoxLeave(TextBoxBase Box)
        {
            void Handler(TextBoxBase Box)
            {
                const int PartTypeNone = 0;
                const int PartTypeYear = 1;
                const int PartTypeMonth = 2;
                const int PartTypeDay = 3;

                // --------------------------------------------
                int AsNumber(string v)
                {
                    int Result = -1;
                    if (!string.IsNullOrWhiteSpace(v) && int.TryParse(v, out Result))
                        return Result;
                    return -1;
                }
                // --------------------------------------------
                int GetPartType(string v)
                {
                    int Result = PartTypeNone;
                    int N = AsNumber(v);
                    if (N > 0)
                    {
                        if (N > 31)
                            return PartTypeYear;
                        if (N > 12)
                            return PartTypeDay;
                        return PartTypeMonth;
                    }


                    return Result;
                }
                // --------------------------------------------

                string Text = Box.Text;

                if (!string.IsNullOrWhiteSpace(Text))
                {
                    string Year = "";
                    string Month = "";
                    string Day = "";

                    string S;
                    int partType;

                    char Sep = Text.Contains('-') ? '-' : (Text.Contains('/') ? '/' : (Text.Contains('.') ? '.' : '-'));
                    string[] Parts = Text.Split(Sep);

                    if (Parts.Length >= 1)
                    {
                        S = Parts[0];
                        partType = GetPartType(S);
                        if (partType == PartTypeYear)
                            Year = S;
                        else // PartTypeDay
                            Day = S;
                    }

                    if (Parts.Length >= 2)
                    {
                        S = Parts[1];
                        partType = GetPartType(S);
                        if (partType == PartTypeMonth)
                            Month = S;
                    }

                    if (Parts.Length >= 3)
                    {
                        S = Parts[2];
                        partType = GetPartType(S);
                        if (partType == PartTypeYear)
                            Year = S;
                        else // PartTypeDay
                            Day = S;
                    }

                    if (string.IsNullOrWhiteSpace(Year))
                        Year = DateTime.Now.Year.ToString();

                    if (string.IsNullOrWhiteSpace(Month))
                        Month = DateTime.Now.Month.ToString();
                    if (Month.Length == 1)
                        Month = "0" + Month;

                    if (string.IsNullOrWhiteSpace(Day))
                        Day = DateTime.Now.Day.ToString();
                    if (Day.Length == 1)
                        Day = "0" + Day;

                    Box.Text = $"{Year}-{Month}-{Day}";
                }
            }
            // --------------------------------------------

            Box.Leave += (s, e) => Handler(Box);
        }

        /* events */
        /// <summary>
        /// Occurs when an unhandled exception is thrown in the primary (UI) thread.
        /// </summary>
        static public event EventHandler<Exception> UnhandledExceptionThrown;
    }
}
