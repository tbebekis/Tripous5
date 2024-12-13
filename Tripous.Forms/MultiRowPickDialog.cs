namespace Tripous.Forms
{

    /// <summary>
    /// A dialog box that allows the selection of multiple DataRow objects
    /// </summary>
    public partial class MultiRowPickDialog : Form
    { 
        DataTable tblSource;
        List<DataRow> SelectedRows;
        string[] VisibleColumns;
        DataTable Table = new DataTable("RowPickList");

        /* private */
        void AnyClick(object sender, EventArgs e)
        {
            if (sender == btnIncludeAll)
            {
                IncludeAll(true);
            }
            else if (sender == btnExcludeAll)
            {
                IncludeAll(false);
            }
            else if (sender == btnShowIdColumns)
            {
                ShowHideIdGridColumns();
            }
            else if (sender == btnOK)
            {
                SelectedRows.Clear();

                foreach (DataRow Row in Table.Rows)
                {
                    if (Sys.AsBoolean(Row["Include"], false))
                        SelectedRows.Add(Row["SourceRow"] as DataRow);
                }

                DialogResult = DialogResult.OK;
            }
 
        }
        void Grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataRow Row = Grid.GetSelectedRow();
            if (Row != null)
            {
                Row["Include"] = !Sys.AsBoolean(Row["Include"], false);
            }
        }
        void Grid_DoubleClick(object sender, EventArgs e)
        {
            DataRow Row = Grid.GetSelectedRow();
            if (Row != null)
            {
                Row["Include"] = !Sys.AsBoolean(Row["Include"], false);
            }
        }

        void IncludeAll(bool Value)
        {
            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                    Row[0] = Value;
            }
        }
        void ShowHideIdGridColumns()
        { 
            foreach (DataGridViewColumn C in Grid.Columns)
            {
                if (Db.IsIdColumn(C.DataPropertyName))
                    C.Visible = (btnShowIdColumns != null) && btnShowIdColumns.Checked;
            }
        }
        void FormInitialize()
        {
            if (Ui.MainForm != null)
            {
                this.Owner = Ui.MainForm;
                this.Icon = Ui.MainForm.Icon;
            }

            this.CancelButton = btnCancel;
            this.AcceptButton = btnOK;

            btnOK.Click += AnyClick;
            btnShowIdColumns.Click += AnyClick;
            btnIncludeAll.Click += AnyClick;
            btnExcludeAll.Click += AnyClick;

            Grid.DoubleClick += Grid_DoubleClick;
            //Grid.CellMouseDoubleClick += Grid_CellMouseDoubleClick;

            btnShowIdColumns.CheckOnClick = true;
            btnShowIdColumns.Checked = false;
 
            DataColumn Column = new DataColumn("Include", typeof(System.Boolean));
            Column.Caption = "+/-";

            Table.Columns.Add(Column);

            foreach (DataColumn SourceColumn in tblSource.Columns)
            {
                Column = SourceColumn.CloneColumn();
                Table.Columns.Add(Column);
            }

            Column = new DataColumn("SourceRow", typeof(object));
            Table.Columns.Add(Column);


            DataRow Row;
            foreach (DataRow SourceRow in tblSource.Rows)
            {
                if (SourceRow.RowState != DataRowState.Deleted)
                {
                    Row = Table.NewRow();
                    Table.Rows.Add(Row);
                    Row["Include"] = SelectedRows.Contains(SourceRow);
                    Row["SourceRow"] = SourceRow;

                    foreach (DataColumn SourceColumn in tblSource.Columns)
                        Row[SourceColumn.ColumnName] = SourceRow[SourceColumn];
                }
            }

            Grid.DataSource = Table;
            Grid.InitializeReadOnly();

            if (Table.Rows.Count <= 2500)
                Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            foreach (DataGridViewColumn C in Grid.Columns)
            {
                if (!C.DataPropertyName.IsSameText("Include"))
                {
                    C.ReadOnly = true;

                    if (VisibleColumns != null && VisibleColumns.Length > 0)
                    {
                        C.Visible = VisibleColumns.ContainsText(C.DataPropertyName);
                    }                    
                }
            } 
 
            btnOK.Enabled = (Table != null) && (Table.Rows.Count > 0);
            btnIncludeAll.Enabled = btnOK.Enabled;
            btnExcludeAll.Enabled = btnOK.Enabled;
        }




        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            if (!DesignMode)
                FormInitialize();
            base.OnShown(e);
        }

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public MultiRowPickDialog()
        {
            InitializeComponent();
        }


        /* static */
        /// <summary>
        /// Displays a row pick list dialog box to the user. Returns true if the user clicks on the OK button of the dialog.
        /// <para>Table is the Table to pick up rows from. </para>
        /// <para>SelectedRows contains the rows that are initially displayed as selected to the user. Up on succesful
        /// return it contains the rows selected by the user.</para>
        /// <para>VisibleColumns is an array of column names of the Table.</para>
        /// </summary>
        static public bool ShowDialog(DataTable Table, List<DataRow> SelectedRows, string[] VisibleColumns, string Title = null)
        {
            if ((Table != null) && (SelectedRows != null))
            {
                using (MultiRowPickDialog F = new MultiRowPickDialog())
                {
                    if (!string.IsNullOrWhiteSpace(Title))
                        F.Text = Title;

                    F.VisibleColumns = VisibleColumns;
                    F.SelectedRows = SelectedRows;
                    F.tblSource = Table;

                    return (F.ShowDialog() == DialogResult.OK);
                }
            }

            return false;
        }
        /// <summary>
        /// Displays a row pick list dialog box to the user. Returns true if the user clicks on the OK button of the dialog.
        /// <para>ConnectionName and SqlText are used to select a Table in order to pick up rows from. </para>
        /// <para>SelectedRows contains the rows that are initially displayed as selected to the user. Up on succesful
        /// return it contains the rows selected by the user.</para>
        /// <para>VisibleColumns is an array of column names of the Table.</para>
        /// </summary>
        static public bool ShowDialog(string ConnectionName, string SqlText, List<DataRow> SelectedRows, string[] VisibleColumns, string Title = null)
        {
            DataTable Table = Db.Select(ConnectionName, SqlText);
            return ShowDialog(Table, SelectedRows, VisibleColumns, Title);
        }
    }
}
