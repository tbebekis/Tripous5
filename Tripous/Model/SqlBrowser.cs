/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Tripous.Data;


namespace Tripous.Model
{



    /// <summary>
    /// Performs SELECTs using SqlFilters
    /// </summary>
    [TypeStoreItem]
    public class SqlBrowser
    {
  
        /// <summary>
        /// Field
        /// </summary>
        protected SqlBrowserDescriptor fDescriptor = new SqlBrowserDescriptor();


        /* overridables */
        /// <summary>
        /// Called by Initialize()
        /// </summary>
        protected virtual void DoInitialize()
        {
            InitializeDescriptor();
            if (Descriptor != null)
                Descriptor.EnsureMainSelect();
            InitializeDatabaseConnection();
 
        }
        /// <summary>
        /// Called by DoInitialize()
        /// </summary>
        protected virtual void InitializeDescriptor()
        {
            if (!IsDeclarative)
            {
                if (Variables.ContainsKey("BrowserDescriptor") && (Variables["BrowserDescriptor"] is SqlBrowserDescriptor))
                    Descriptor.Assign(Variables["BrowserDescriptor"] as SqlBrowserDescriptor);
                else
                    DefineDescriptor();
            }
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
        /// Constructor.
        /// </summary>
        public SqlBrowser()
        {
        }
 
        /* static */
        /// <summary>
        /// Finds a descriptor based on DataName
        /// </summary>
        static public SqlBrowserDescriptor FindDescriptor(string DataName)
        {
            SqlBrowserDescriptor Result = null; //TODO:  FormDescriptorSysDataItem.FindDescriptor(DataName);
            if (Result != null)
            {
                Result.DescriptorMode = DescriptorMode.Custom;
            }

            if (Result == null)
                Result = Registry.Browsers.Find(DataName);

            return Result;
        }
        /// <summary>
        /// Creates and returns an instance based on the specified descriptor
        /// </summary>
        static public SqlBrowser Create(SqlBrowserDescriptor Des)
        {
            if (Des != null)
            {
                SqlBrowser Result = TypeStore.Create(Des.TypeClassName) as SqlBrowser;
                Result.Descriptor = Des;
                Result.Initialize();
                return Result;
            }

            return null;
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
                SS = this.Descriptor.SelectList.Find(SelectSqlName); 
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
        /// Means that Descriptor is assigned by the user before the Initialize()
        /// </summary>
        public bool IsDeclarative { get; protected set; }
 
        /// <summary>
        /// Gets the variables of the browser.
        /// </summary>
        public Dictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// Gets or sets the descriptor of this instance
        /// </summary>
        public SqlBrowserDescriptor Descriptor
        {
            get { return fDescriptor; }
            set
            {
                if (!Initialized)
                {
                    fDescriptor.Assign(value);
                    IsDeclarative = value != null;
                }
            }
        }

 
        /// <summary>
        /// Returns the Executor
        /// </summary>
        public virtual SqlStore Store { get; protected set; }
 

        /// <summary>
        /// Gets or sets the "onwer" of this broker
        /// </summary>
        public object Owner { get; set; }

        /* events */
        /// <summary>
        /// Occurs just before a SELECT statement goes for execution.
        /// <para>Client code may link to this event in order to display a Criteria dialog</para>
        /// </summary>
        public event EventHandler<SelectSqlEventArgs> SelectBefore;
    }




}
