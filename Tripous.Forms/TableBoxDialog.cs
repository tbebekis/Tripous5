using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tripous.Forms
{

    /// <summary>
    /// Displays a DataTable or a DataRow
    /// </summary>
    public partial class TableBoxDialog : Form
    {
        /// <summary>
        /// handles the Grid double click 
        /// </summary>
        void Grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (btnOK.Enabled)
                btnOK.PerformClick();
        }
        /// <summary>
        /// Initializes the dialog box
        /// </summary>
        void Initialize(DataTable Table, string Title)
        {
            this.Text = Title;

            Grid.InitializeReadOnly();
            Grid.DataError += Grid_DataError;
            Grid.DataSource = Table;
            if (Table.Rows.Count <= 5000)
                Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // ColumnHeader DisplayedCells Fill AllCells

            Grid.CellMouseDoubleClick += Grid_CellMouseDoubleClick;

            foreach (DataColumn column in Table.Columns)
            {
                Grid.Columns[column.ColumnName].HeaderText = column.Caption;
                Grid.Columns[column.ColumnName].Visible = column.IsVisible();

                if (Simple.SimpleTypeOf(column.DataType).IsNumeric())
                    Grid.Columns[column.ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            btnOK.Enabled = Grid.RowCount > 0;
        }
        void Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // do nothing
        }

        /// <summary>
        /// Displays the dialog box. Returns true if the user selects a row.
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public bool Execute(DataTable Table, out DataRow Row, string Text)
        {
            bool Result = false;
            Row = null;

            if (Table == null)
                throw new ArgumentNullException("Table", "TableBox can not display a null table");

            using (TableBoxDialog Dlg = new TableBoxDialog())
            {
                Dlg.Initialize(Table, Text);
                Result = Dlg.ShowDialog() == DialogResult.OK;
                if (Result)
                {
                    Row = ((DataRowView)Dlg.Grid.CurrentRow.DataBoundItem).Row;
                }
            }

            return Result;
        }
        /// <summary>
        /// Displays the dialog box. Returns true if the user selects a row.
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public bool Execute(DataTable Table, out DataRow Row)
        {
            return Execute(Table, out Row, !string.IsNullOrEmpty(Table.TableName) ? Table.TableName : "Table box");
        }
        /// <summary>
        /// Displays the dialog box. 
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public void Execute(DataTable Table, string Text)
        {
            DataRow Row;
            Execute(Table, out Row, Text);
        }
        /// <summary>
        /// Displays the dialog box. 
        /// Use the ColumnInfoList to pass setup information for the columns.
        /// </summary>
        static public void Execute(DataTable Table)
        {
            Execute(Table, !string.IsNullOrEmpty(Table.TableName) ? Table.TableName : "Table box");
        }

        /// <summary>
        /// Displays a Row dialog box. 
        /// </summary>
        static public void Execute(DataRow SourceRow, string[] VisibleFields)
        {
            string TableName = "Row";
            if (!string.IsNullOrEmpty(SourceRow.Table.TableName))
                TableName = string.Format("Row: {0}", SourceRow.Table.TableName);

            DataTable Table = new DataTable(TableName);
            Table.Columns.Add("Ordinal", typeof(int));
            Table.Columns.Add("Column", typeof(string));            
            Table.Columns.Add("Caption", typeof(string));
            Table.Columns.Add("Type", typeof(string));
            Table.Columns.Add("Value", typeof(string));
            

            DataRow Row;

            foreach (DataColumn Column in SourceRow.Table.Columns)
            {
                if (VisibleFields.Contains(Column.ColumnName))
                {
                    Row = Table.NewRow();

                    Row["Ordinal"] = Column.Ordinal;
                    Row["Column"] = Column.ColumnName;
                    Row["Caption"] = Column.Caption;
                    Row["Type"] = Column.DataType.ToString();
                    if (!SourceRow.IsNull(Column))
                    {
                        if ((Column.DataType == typeof(string)) && string.IsNullOrEmpty(SourceRow[Column].ToString()))
                            Row["Value"] = string.Empty.QS();
                        else
                            Row["Value"] = SourceRow[Column].ToString();
                    }

                    Table.Rows.Add(Row);
                }
            }

            Execute(Table, "Row box");
        }
        /// <summary>
        /// Displays a Row dialog box. 
        /// </summary>
        static public void Execute(DataRow SourceRow)
        {
            string[] VisibleFields = new string[SourceRow.Table.Columns.Count];

            for (int i = 0; i < SourceRow.Table.Columns.Count; i++)
                VisibleFields[i] = SourceRow.Table.Columns[i].ColumnName;

            Execute(SourceRow, VisibleFields);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TableBoxDialog()
        {
            InitializeComponent();

            if (Ui.MainForm != null)
            {
                this.Owner = Ui.MainForm;
                this.Icon = Ui.MainForm.Icon;
            }
        }
    }
}
