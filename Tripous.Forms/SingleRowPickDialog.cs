using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tripous.Data;

namespace Tripous.Forms
{

    /// <summary>
    /// A dialog for selecting a single DataRow
    /// </summary>
    public partial class SingleRowPickDialog : Form
    {
        DataTable Table;
        string[] VisibleColumns;
        object KeyValue;
        string KeyFieldName;
        

        DataRow ResultRow;


        /* private */
        void AnyClick(object sender, EventArgs e)
        {
            if (sender == btnOK)
            {
                ResultRow = Grid.GetSelectedRow();
                if (ResultRow != null)
                    DialogResult = DialogResult.OK;
            }
            else if (sender == btnShowIdColumns)
            {
                ShowHideIdGridColumns();
            }
        }
        void Grid_DoubleClick(object sender, EventArgs e)
        {
            if (btnOK.Enabled)
                btnOK.PerformClick();
        }


        void ShowHideIdGridColumns()
        {
            foreach (DataGridViewColumn C in Grid.Columns)
            {
                if (Db.IsIdColumn(C.DataPropertyName))
                    C.Visible = (btnShowIdColumns != null) && btnShowIdColumns.Checked;
            }
        }
        void PositionToRow()
        {
            if ((KeyValue != null) && !string.IsNullOrWhiteSpace(KeyFieldName) && (Table.Rows.Count > 0))
            {

                DataRow Row = Table.Locate(KeyFieldName, new object[] { KeyValue }, LocateOptions.None);
                CurrencyManager cm = Grid.BindingContext[Table] as CurrencyManager;

                if ((Row != null) && (cm != null) && (cm.Count > 0))
                {
                    object Id = Row[KeyFieldName];
                    object IdB;
                    bool Flag = false;
                    DataRow CurrentRow = null;

                    IList List = cm.List;
                    object Item = List[0];

                    if (Item is DataRow)
                    {
                        for (int i = 0; i < List.Count; i++)
                        {
                            IdB = (List[i] as DataRow)[KeyFieldName];
                            if (object.Equals(Id, IdB))
                            {
                                cm.Position = i;
                                CurrentRow = List[i] as DataRow;
                                Flag = true;
                                break;
                            }
                        }
                    }
                    else if (Item is DataRowView)
                    {
                        for (int i = 0; i < List.Count; i++)
                        {
                            IdB = ((List[i] as DataRowView).Row)[KeyFieldName];
                            if (object.Equals(Id, IdB))
                            {
                                cm.Position = i;
                                CurrentRow = (List[i] as DataRowView).Row;
                                Flag = true;
                                break;
                            }
                        }
                    }


                    if ((Flag) && (CurrentRow != null))
                    {
                        Grid.PositionToRow(CurrentRow);
                    }

                }

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

            Grid.DoubleClick += Grid_DoubleClick;

            btnShowIdColumns.CheckOnClick = true;
            btnShowIdColumns.Checked = false;
 
            Grid.DataSource = Table;
            Grid.InitializeReadOnly();

            if (Table.Rows.Count <= 2500)
                Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (VisibleColumns != null && VisibleColumns.Length > 0)
            {
                foreach (DataGridViewColumn C in Grid.Columns)
                {
                    C.Visible = VisibleColumns.ContainsText(C.DataPropertyName);
                }
            }

            PositionToRow();


            btnOK.Enabled = (Table != null) && (Table.Rows.Count > 0);

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
        public SingleRowPickDialog()
        {
            InitializeComponent();
        }

 


        /* static */
        /// <summary>
        /// Shows the dialog
        /// </summary>
        static public DataRow ShowDialog(DataTable Table, string[] VisibleColumns = null, string Title = null, string KeyFieldName = null, object KeyValue = null)
        {
            if (Table != null)
            {
                using (SingleRowPickDialog F = new SingleRowPickDialog())
                {
                    if (!string.IsNullOrWhiteSpace(Title))
                        F.Text = Title;

                    F.Table = Table;
                    F.VisibleColumns = VisibleColumns;
                    F.KeyFieldName = KeyFieldName;
                    F.KeyValue = KeyValue;

                    if (F.ShowDialog() == DialogResult.OK)
                        return F.ResultRow;
                }
            }

            return null;
        }
        /// <summary>
        /// Shows the dialog
        /// </summary>
        static public DataRow ShowDialog(string ConnectionName, string SqlText, string[] VisibleColumns = null, string Title = null, string KeyFieldName = null, object KeyValue = null)
        {
            DataTable Table = Db.Select(ConnectionName, SqlText);
            return ShowDialog(Table, VisibleColumns, Title, KeyFieldName, KeyValue);
        }

    }
}
