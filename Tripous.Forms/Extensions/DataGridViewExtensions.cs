/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
 
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections.Generic;

 

namespace Tripous.Forms
{
 
    /// <summary>
    /// A helper class for a DataGridView bound to a DataTable
    /// </summary>
    static public class DataGridViewExtensions
    {
        /// <summary>
        /// Returns the currency manager of Grid, if any, else null
        /// </summary>
        static public CurrencyManager GetManager(this DataGridView Grid)
        {
            if (Grid.DataSource != null)
            {
                if (Grid.DataSource is BindingSource)
                    return (Grid.DataSource as BindingSource).CurrencyManager;

                if (!string.IsNullOrEmpty(Grid.DataMember))
                    if (Grid.BindingContext.Contains(Grid.DataSource, Grid.DataMember))
                        return Grid.BindingContext[Grid.DataSource, Grid.DataMember] as CurrencyManager;

                if (Grid.BindingContext.Contains(Grid.DataSource))
                    return Grid.BindingContext[Grid.DataSource] as CurrencyManager;
            }

            return null;
        }
        /// <summary>
        /// Gets the DataTable of Grid
        /// </summary>
        static public DataTable GetDataTable(this DataGridView Grid)
        {
            if (Grid.DataSource is BindingSource)
                return Ui.DataTableOf((Grid.DataSource as BindingSource).DataSource, (Grid.DataSource as BindingSource).DataMember);

            return Ui.DataTableOf(Grid.DataSource, Grid.DataMember);
        }
        /// <summary>
        /// Returns true if Grid's DataSource is a DataTable
        /// </summary>
        static public bool IsDataTableBound(this DataGridView Grid)
        {
            return (Grid.DataSource != null) && (Grid.DataSource is DataTable);
        }
        /// <summary>
        /// Returns true if Grid has a current <see cref="DataRow"/>
        /// </summary>
        static public bool HasCurrentDataRow(this DataGridView Grid)
        {
            return Grid.CurrentDataRow() != null;
        }
        /// <summary>
        /// Returns the current  <see cref="DataRow"/> of Grid
        /// </summary>
        static public DataRow CurrentDataRow(this DataGridView Grid)
        {
            if ((Grid.CurrentRow != null) && (Grid.CurrentRow.DataBoundItem is DataRowView))
                return (Grid.CurrentRow.DataBoundItem as DataRowView).Row;

            return null;
        }
        /// <summary>
        /// Positions the grid to the specified row.
        /// <para>Returns true on success.</para>
        /// </summary>
        static public bool PositionToRow(this DataGridView Grid, DataRow Row)
        {
            CurrencyManager Manager = Grid.GetManager();
            if ((Manager != null) && (Manager.Count > 0))
                return Manager.PositionToRow(Row);

            return false;
        }

        /// <summary>
        /// Returns an array of the currently selected DataRows in the grid.
        /// <para>The array is empty if no rows are selected.</para>
        /// </summary>
        static public DataRow[] GetSelectedRows(this DataGridView Grid)
        {
            List<DataRow> List = new List<DataRow>();

            foreach (DataGridViewRow DGVR in Grid.SelectedRows)
            {
                if (DGVR.DataBoundItem is DataRow)
                    List.Add(DGVR.DataBoundItem as DataRow);
                else if (DGVR.DataBoundItem is DataRowView)
                    List.Add((DGVR.DataBoundItem as DataRowView).Row);
            }

            return List.ToArray();
        }
        /// <summary>
        /// Returs the selected DataRow if any, else null
        /// </summary>
        static public DataRow GetSelectedRow(this DataGridView Grid)
        {
            DataRow[] Rows = Grid.GetSelectedRows();
            if ((Rows != null) && (Rows.Length > 0))
                return Rows[0];

            return Grid.CurrentDataRow();
        }
        /// <summary>
        /// Returns an array of the currently visible DataRows in the grid.
        /// <para>The array is empty if no rows are selected.</para>
        /// </summary>
        static public DataRow[] GetVisibleRows(this DataGridView Grid)
        {
            List<DataRow> List = new List<DataRow>();

            foreach (DataGridViewRow DGVR in Grid.Rows)
            {
                if (DGVR.DataBoundItem is DataRow)
                    List.Add(DGVR.DataBoundItem as DataRow);
                else if (DGVR.DataBoundItem is DataRowView)
                    List.Add((DGVR.DataBoundItem as DataRowView).Row);
            }

            return List.ToArray();
        }

        /// <summary>
        /// Returns the DataRow under the mouse cursor, if any, else null.
        /// <para>To be used with drag n drop operations</para>
        /// </summary>
        static public DataRow RowUnderMouse(this DataGridView Grid)
        {
            Point P = Control.MousePosition;
            P = Grid.PointToClient(P);


            int RowIndex = Grid.HitTest(P.X, P.Y).RowIndex;

            if (RowIndex != -1)
            {
                DataGridViewRow DGVR = Grid.Rows[RowIndex];

                if (DGVR.DataBoundItem is DataRow)
                    return DGVR.DataBoundItem as DataRow;
                else if (DGVR.DataBoundItem is DataRowView)
                    return (DGVR.DataBoundItem as DataRowView).Row;
            }

            return null;
        }

        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public object AsObject(this DataGridView Grid, string ColumnName, object Default)
        {
            DataRow Row = CurrentDataRow(Grid);
            if (Row != null)
                return DataRowExtensions.AsObject(Row, ColumnName, Default);

            return Default;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public int AsInteger(this DataGridView Grid, string ColumnName, int Default)
        {
            DataRow Row = CurrentDataRow(Grid);
            if (Row != null)
                return DataRowExtensions.AsInteger(Row, ColumnName, Default);

            return Default;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public int AsInteger(this DataGridView Grid, string ColumnName)
        {
            return AsInteger(Grid, ColumnName, 0);
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public string AsString(this DataGridView Grid, string ColumnName, string Default)
        {
            DataRow Row = CurrentDataRow(Grid);
            if (Row != null)
                return DataRowExtensions.AsString(Row, ColumnName, Default);

            return Default;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public string AsString(this DataGridView Grid, string ColumnName)
        {
            return AsString(Grid, ColumnName, "");
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public double AsFloat(this DataGridView Grid, string ColumnName, double Default)
        {
            DataRow Row = CurrentDataRow(Grid);
            if (Row != null)
                return DataRowExtensions.AsFloat(Row, ColumnName, Default);

            return Default;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public double AsFloat(this DataGridView Grid, string ColumnName)
        {
            return AsFloat(Grid, ColumnName, 0);
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public bool AsBoolean(this DataGridView Grid, string ColumnName, bool Default)
        {
            DataRow Row = CurrentDataRow(Grid);
            if (Row != null)
                return DataRowExtensions.AsBoolean(Row, ColumnName, Default);

            return Default;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public bool AsBoolean(this DataGridView Grid, string ColumnName)
        {
            return AsBoolean(Grid, ColumnName, false);
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public DateTime AsDateTime(this DataGridView Grid, string ColumnName, DateTime Default)
        {
            DataRow Row = CurrentDataRow(Grid);
            if (Row != null)
                return DataRowExtensions.AsDateTime(Row, ColumnName, Default);

            return Default;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public DateTime AsDateTime(this DataGridView Grid, string ColumnName)
        {
            return AsDateTime(Grid, ColumnName, DateTime.Now);
        }
        
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public DataGridViewColumn FindColumn(this DataGridView Grid, string FieldName)
        {
            foreach (DataGridViewColumn Column in Grid.Columns)
                if (Sys.IsSameText(FieldName, Column.DataPropertyName))
                    return Column;

            return null;
        }
        /// <summary>
        /// Returns the value of the field with ColumnName of the current  <see cref="DataRow"/> of Grid, if any, else Default.
        /// </summary>
        static public bool ContainsColumn(this DataGridView Grid, string FieldName)
        {
            return FindColumn(Grid, FieldName) != null;
        }
        /// <summary>
        /// Initializes the Grid as a read-only one
        /// </summary>
        /// <param name="Grid"></param>
        static public void InitializeReadOnly(this DataGridView Grid)
        {
            if (Grid != null)
            {
                Grid.ReadOnly = true;
                Grid.AllowUserToAddRows = false;
                Grid.AllowUserToDeleteRows = false;
                Grid.AlternatingRowsDefaultCellStyle.BackColor = UiConfig.AlternativeRowsBackColor;
                //Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }

        }
        
        /// <summary>
        /// Sets the HeaderText of Grid.Columns.
        /// <para>List is a FIELD_NAME=TITLE list.</para>
        /// <para>If HideUntitled is true, then any column not contained in List becomes invisible.</para>
        /// </summary>
        static public void SetColumnCaptionsFrom(this DataGridView Grid, NameValueStringList List, bool HideUntitled)
        {
            if (Grid != null)
            {
                foreach (DataGridViewColumn Column in Grid.Columns)
                {
                    if (List.ContainsName(Column.DataPropertyName))
                    {
                        Column.HeaderText = List.Values[Column.DataPropertyName];
                    }
                    else if (HideUntitled)
                        Column.Visible = false;
                }
            }

        }
        /// <summary>
        /// Sets the HeaderText of Grid.Columns.
        /// <para>List is a FIELD_NAME=TITLE list.</para>
        /// <para>If HideUntitled is true, then any column not contained in List becomes invisible.</para>
        /// </summary>
        static public void SetColumnCaptionsFrom(this DataGridView Grid, string DisplayLabels, bool HideUntitled)
        {
            SetColumnCaptionsFrom(Grid, new NameValueStringList(DisplayLabels), HideUntitled);
        }
        /// <summary>
        /// Sets the HeaderText of Grid.Columns using Table.Columns
        /// </summary>
        static public void SetColumnCaptionsFrom(this DataGridView Grid, DataTable Table)
        {
            NameValueStringList List = new NameValueStringList();
            foreach (DataColumn Column in Table.Columns)
            {
                List.Values[Column.ColumnName] = Column.Caption;
            }

            SetColumnCaptionsFrom(Grid, List, false);
        }


        /// <summary>
        /// Sets the visibility of a column
        /// </summary>
        static public void SetColumnVisible(this DataGridView Grid, string FieldName, bool Visible)
        {
            DataGridViewColumn C = Grid.FindColumn(FieldName);
            if (C != null)
                C.Visible = Visible;
        }
        /// <summary>
        /// Sets the visibility of a number of columns
        /// </summary>
        static public void SetVisibleColumns(this DataGridView Grid, params string[] FieldNames)
        {
            if (FieldNames != null && FieldNames.Length > 0)
            {
                foreach (DataGridViewColumn C in Grid.Columns)
                {
                    C.Visible = FieldNames.ContainsText(C.DataPropertyName);
                }
            }
        }
        /// <summary>
        /// Sets the read-only property of a number of columns
        /// </summary>
        static public void SetReadOnlyColumns(this DataGridView Grid, params string[] FieldNames)
        {
            if (FieldNames != null && FieldNames.Length > 0)
            {
                foreach (DataGridViewColumn C in Grid.Columns)
                {
                    C.ReadOnly = FieldNames.ContainsText(C.DataPropertyName);
                }
            }
        }

        /// <summary>
        /// Setups Grid columns based on Table columns
        /// <para>Returns a List of columns set as non-Visible. See: ForceNonVisibleDataGridViewColumns()</para>
        /// </summary>
        static public List<DataGridViewColumn> SetupGridColumns(this DataGridView Grid, DataTable Table)
        {
            List<DataGridViewColumn> NonVisibleList = new List<DataGridViewColumn>();

            Grid.SuspendLayout();

            DataGridViewColumn Column;

            foreach (DataColumn Field in Table.Columns)
            {
                Column = Grid.Columns.Contains(Field.ColumnName) ? Grid.Columns[Field.ColumnName] : null;
                if (Column != null)
                {
                    Column.HeaderText = Field.Caption;
                    if (string.IsNullOrEmpty(Column.Name))
                        Column.Name = Column.DataPropertyName;
                    Column.Visible = Field.IsVisibleSet() ? Field.IsVisible() : Ui.IsVisibleColumn(Field.ColumnName);

                    if (!Column.Visible)
                        NonVisibleList.Add(Column);

                    if (Simple.SimpleTypeOf(Field.DataType).IsNumeric())
                        Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    if (Simple.SimpleTypeOf(Field.DataType).IsFloat())
                        Column.DefaultCellStyle.Format = "F2";
                }
            }



            Grid.ResumeLayout();

            return NonVisibleList;
        }
    }
 
}

 