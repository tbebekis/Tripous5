/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
 
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
  
namespace Tripous.Forms
{
    /// <summary>
    /// Extensions
    /// </summary>
    static public class CurrencyManagerExtensions
    {
        /// <summary>
        /// Gets the Current object of the Manager in a safe manner
        /// </summary>
        static public object ManagerCurrent(this CurrencyManager Manager)
        {
            try
            {
                if ((Manager != null) && (Manager.Position != -1) && (Manager.Current != null))
                    return Manager.Current;
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Tries to find the CurrencyManager of Instance.
        /// </summary>
        static public CurrencyManager ManagerOf(object Instance)
        {
            if (Instance != null)
            {
                if (Instance is BindingSource)
                    return (Instance as BindingSource).CurrencyManager;

 
                if (Instance is DataGridView)
                    return (Instance as DataGridView).GetManager();
 

 

                if (Instance is Control)
                {
                    Control Control = Instance as Control;
                    BindingContext BindingContext = Control.BindingContext;

                    object DataSource = Ui.DataSourceOf(Instance);

                    /* if it has a DataSource property */
                    if (DataSource != null)
                    {
                        if (DataSource is BindingSource)
                            return (DataSource as BindingSource).CurrencyManager;

                        string DataMember = Ui.DataMemberOf(Instance);

                        /* go to the BindingContext with the DataSource and DataMember, if valid */
                        if (!string.IsNullOrEmpty(DataMember))
                            if (BindingContext.Contains(DataSource, DataMember))
                                return BindingContext[DataSource, DataMember] as CurrencyManager;

                        if (BindingContext.Contains(DataSource))
                            return BindingContext[DataSource] as CurrencyManager;

                        if ((DataSource is IListSource) || (DataSource is IBindingListView) || (DataSource is IBindingList))
                            return BindingContext[DataSource] as CurrencyManager;

                    }


                    /* get the first Binding in the DataBindings */
                    if (Control.DataBindings.Count > 0)
                    {
                        DataSource = Control.DataBindings[0].DataSource;
                        if (DataSource is BindingSource)
                            return (DataSource as BindingSource).CurrencyManager;

                        if (BindingContext.Contains(DataSource))
                            return BindingContext[DataSource] as CurrencyManager;
                    }

                }

            }


            return null;
        }
        /// <summary>
        /// Returns the DataTable of the Manager, if any, else null
        /// </summary>
        static public DataTable GetDataTable(this CurrencyManager Manager)
        {
            if (Manager != null)
            {
                if (Manager.List is DataTable)
                    return Manager.List as DataTable;

                if (Manager.List is DataView)
                    return (Manager.List as DataView).Table;

                if (Manager.List is BindingSource)
                    return Ui.DataTableOf((Manager.List as BindingSource).DataSource, (Manager.List as BindingSource).DataMember);
            }


            return null;
        }

        /// <summary>
        /// Returns true if Manager has a current DataRow
        /// </summary>
        static public bool HasCurrentRow(this CurrencyManager Manager)
        {
            return (Manager != null) && (CurrentRow(Manager) != null);
        }
        /// <summary>
        /// Returns true if Manager has a current DataRowView
        /// </summary>
        static public bool HasCurrentRowView(this CurrencyManager Manager)
        {
            return (Manager != null) && (CurrentRowView(Manager) != null);
        }
        /// <summary>
        /// Returns true if Manager is in editing mode
        /// </summary>
        static public bool InEditing(this CurrencyManager Manager)
        {
            return (Manager != null) && HasCurrentRowView(Manager) && CurrentRowView(Manager).IsEdit;
        }
        /// <summary>
        /// Returns true if Manager is in inserting mode
        /// </summary>
        static public bool InInserting(this CurrencyManager Manager)
        {
            return (Manager != null) && HasCurrentRowView(Manager) && CurrentRowView(Manager).IsNew;
        }
        /// <summary>
        /// Returns the current DataRowView of the Manager, if any, else null.
        /// </summary>
        static public DataRowView CurrentRowView(this CurrencyManager Manager)
        {
            object Current = ManagerCurrent(Manager);

            if (Current is DataRowView)
                return Current as DataRowView;

            return null;
        }
        /// <summary>
        /// Returns the current DataRow of the Manager, if any, else null.
        /// </summary>
        static public DataRow CurrentRow(this CurrencyManager Manager)
        {
            object Current = ManagerCurrent(Manager);

            if (Current is DataRowView)
                return (Current as DataRowView).Row;

            if (Current is DataRow)
                return Current as DataRow;

            return null;
        }
        /// <summary>
        /// Gets the position of the Row in the Manager, if exists, else -1.
        /// </summary>
        static public int PositionOf(this CurrencyManager Manager, DataRow Row)
        {
            if ((Manager != null) && (Manager.Count > 0) && /*!InEditing(Manager) && !InInserting(Manager) &&*/ (Row != null))
            {

                IList List = Manager.List;

                object Item = List[0];

                if (Item is DataRow)
                    return List.IndexOf(Row);
                else if (Item is DataRowView)
                {
                    for (int i = 0; i < List.Count; i++)
                    {
                        if (object.ReferenceEquals(Row, (List[i] as DataRowView).Row))
                        {
                            return i;
                        }
                    }
                }

            }

            return -1;

        }
        /// <summary>
        /// Returns true if Row exists in Manager.
        /// </summary>
        static public bool RowExists(this CurrencyManager Manager, DataRow Row)
        {
            return (Manager != null) && (PositionOf(Manager, Row) != -1);
        }
        /// <summary>
        /// Sets Manager <see cref="CurrencyManager.Position"/> position to the Row, if Row belongs to the Manager.
        /// </summary>
        static public bool PositionToRow(this CurrencyManager Manager, DataRow Row)
        {
            if ((Manager != null) && (Row != null))
            {
                int Position = PositionOf(Manager, Row);
                if (Position != -1)
                {
                    Manager.Position = Position;
                    return true;
                }
            }


            return false;
        }
    }
}

 