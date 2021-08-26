using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;


namespace Tripous.Data
{

    /// <summary>
    /// Performs SELECTs using SqlFilters
    /// </summary>
    [TypeStoreItem]
    public class SqlBrowser
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string GENERIC_SQL_BROKER = "_GENERIC_SQL_BROWSER_";

        /* overridables */
        /// <summary>
        /// Called by Initialize()
        /// </summary>
        protected virtual void DoInitialize()
        {
            InitializeDescriptor();
            InitializeDatabaseConnection();

        }
        /// <summary>
        /// Called by DoInitialize()
        /// </summary>
        protected virtual void InitializeDescriptor()
        {
            if (!IsDeclarative)
                DefineDescriptor();
        }
        /// <summary>
        /// Called by InitializeDescriptor()
        /// </summary>
        protected virtual void DefineDescriptor()
        {
        }
        /// <summary>
        /// Initializes the database connection of this browser. A browser needs to know which is 
        /// the database it operates on.
        /// </summary>
        protected virtual void InitializeDatabaseConnection()
        {
            var ConnectionInfo = Db.GetConnectionInfo(Descriptor.ConnectionName);

            if (ConnectionInfo == null)
                Sys.Throw("No Sql connection defined for Browser. {0}", Descriptor.Name);

            Store = SqlStores.CreateSqlStore(ConnectionInfo);
        }


        /// <summary>
        /// Triggers the associated event.
        /// <para>The SelectSql is a copy of the SelectDes.SelectSql since it is not wise to alter
        /// the descriptor in any way</para>
        /// <para>Returning true, cancels the SELECT execution</para>
        /// </summary>
        protected virtual bool OnSelectBefore(SelectSql SelectSql)
        {
            if (SelectBefore != null)
            {
                SelectSqlEventArgs ea = new SelectSqlEventArgs(SelectSql);
                SelectBefore(this, ea);
                return ea.Cancel;
            }

            return false;
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrowser()
        {
        }

        /* public */
        /// <summary>
        /// Initializes this instance
        /// </summary>
        public void Initialize()
        {
            if (!Initialized)
            {
                DoInitialize();
                Initialized = true;
            }
        }

        /// <summary>
        /// Executes a SELECT statements and puts the returned data rows to the Table.
        /// <para>SqlText could be the statement text or a SelectSql Name found in Descriptor.SelectList.</para>
        /// <para>RowLimit greater than zero, is an instruction to apply a row limit to the SELECT statement</para>
        /// <para>NOTE: This method is used when selecting for the browse part of a data form. 
        ///  Normally the Table passed to this method is not part of the table tree of the TableSet.</para>
        /// </summary>
        public virtual int Select(MemTable Table, string SqlText, int RowLimit)
        {
            SelectSql SS = null;
            string SelectSqlName = SqlText.Trim();

            if (!SelectSqlName.StartsWithText("SELECT")) // it's a SelectSql name
            {
                SS = this.Descriptor.SelectList.Find(item => item.Name == SelectSqlName);
                SqlText = SS.Text;
            }

            SS = new SelectSql(SqlText);

            if (!OnSelectBefore(SS))                    // OnSelectBefore() returning true, cancels the SELECT execution
            {
                if (RowLimit > 0)
                {
                    Store.Provider.ApplyRowLimit(SS, RowLimit);
                }

                SqlText = SS.Text;
                Store.SelectTo(Table, SqlText);
            }

            return Table.Rows.Count;
        }
        /// <summary>
        /// Executes the SELECT SqlText and puts the returned data rows to the Table.
        /// <para>It is used when selecting for the browse part of a data form.</para>
        /// <para>Normally the Table passed to this method is not part of the table tree of the TableSet.</para>
        /// </summary>
        public virtual int Select(MemTable Table, string SqlText)
        {
            return Select(Table, SqlText, SysConfig.SelectSqlRowsLimit);
        }

        /* properties */
        /// <summary>
        /// True after the Initialize() is called.
        /// </summary>
        public bool Initialized { get; protected set; }
        /// <summary>
        /// True means that Descriptor is assigned by the user before the Initialize()
        /// </summary>
        public bool IsDeclarative => Descriptor.Name != GENERIC_SQL_BROKER;

        /// <summary>
        /// Gets the variables of the browser.
        /// </summary>
        public Dictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// Gets or sets the descriptor of this instance
        /// </summary>
        public SqlBrowserDef Descriptor { get; set; } = new SqlBrowserDef() { Name = GENERIC_SQL_BROKER };



        /// <summary>
        /// Returns the Executor
        /// </summary>
        public virtual SqlStore Store { get; protected set; }


        /* events */
        /// <summary>
        /// Occurs just before a SELECT statement goes for execution.
        /// <para>Client code may link to this event in order to display a Criteria dialog</para>
        /// </summary>
        public event EventHandler<SelectSqlEventArgs> SelectBefore;

    }
}
