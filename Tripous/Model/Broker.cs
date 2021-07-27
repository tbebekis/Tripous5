/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using Tripous.Data;
 


namespace Tripous.Model
{

    /// <summary>
    /// Broker class is the base class for creating Tripous business objects.
    /// <para>A CustomerBroker, a SupplierBroker or a TradeBroker, for example, all should inherit from this base class.</para>
    /// <para>A broker represents a group of correlated tables which belong to a certain "module". A Customer module, for example,
    /// may use the CUSTOMER, CUSTOMER_ADDRESS and COUNTRY tables.</para>
    /// <para>A broker offers two "views" of the data it represents: a browse (or list) view and an edit (or item) view and provides a set of
    /// methods for handling each "view". </para>
    /// <para>It also functions in one of two modes: list broker or master broker. A master broker is the most common scenario. </para>
    /// <para>A master broker provides a whole table tree, where the tblItem is the top, 
    /// of master-detail-subdetail tables to insert, edit and delete data. The tblItem must have always just a single row.</para>
    /// <para>A list broker uses the tblBrowser for inserting, editing and deleting data. Changes to different rows are commited all at once. 
    /// In short a list broker utilizes the "cached updates" scenario, something similar to AcceptChanges .</para>
    /// </summary>
    [TypeStoreItem]
    public class Broker
    {
        /* static private */
        static void OnBrokerCreated(Broker Broker)
        {
            if (BrokerCreated != null)
            {
                try
                {
                    BrokerCreated(Broker, new BrokerEventArgs(Broker));
                }
                catch
                {
                }
            }
        }
        static void OnBrokerInitialized(Broker Broker)
        {
            if (BrokerInitialized != null)
            {
                try
                {
                    BrokerInitialized(Broker, new BrokerEventArgs(Broker));
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// Field
        /// </summary>
        static public readonly string BrowserPostfix = "__BROWSER";
        /// <summary>
        /// Field. Broker "event" names
        /// </summary>
        static public readonly string[] EventNames = {
                                                        "InsertBefore",
                                                        "InsertAfter",
                                                        "EditBefore",
                                                        "EditAfter",
                                                        "DeleteBefore",
                                                        "DeleteAfter",
                                                        "CommitBefore.Insert",
                                                        "CommitAfter.Insert",
                                                        "CommitBefore.Edit",
                                                        "CommitAfter.Edit",
                                                        "CancelBefore",
                                                        "CancelAfter",
                                                     };

 
 

        /* protected */
 


        /// <summary>
        /// Field
        /// </summary>
        protected MemTable ftblItem;                // ref
        /// <summary>
        /// Field
        /// </summary>
        protected MemTable ftblLines;               // ref
        /// <summary>
        /// Field
        /// </summary>
        protected MemTable ftblSubLines;            // ref    

        /* operation flags */
        /// <summary>
        /// Field
        /// </summary>
        protected int fInserting;
        /// <summary>
        /// Field
        /// </summary>
        protected int fLoading;
        /// <summary>
        /// Field
        /// </summary>
        protected int fDeleting;
        /// <summary>
        /// Field
        /// </summary>
        protected int fCommiting;


        /* initialization */
        /// <summary>
        /// Initializes the broker.
        /// </summary>
        public virtual void Initialize(bool AsListBroker)
        {
            if (!Initialized)
            {
                Initializing = true;
                try
                {
                    IsListBroker = AsListBroker || this.IsListBroker;
                    DoInitializeBefore();
                    DoInitialize();
                    Initialized = true;
                    DoInitializeAfter();
                }
                finally
                {
                    Initializing = false;
                }
            }
        }
        /// <summary>
        /// Called before the <see cref="DoInitialize"/>
        /// </summary>
        protected virtual void DoInitializeBefore()
        {
        }
        /// <summary>
        /// Initializes the broker.
        /// </summary>
        protected virtual void DoInitialize()
        {
        }
        /// <summary>
        /// Called after the <see cref="DoInitialize"/>
        /// </summary>
        protected virtual void DoInitializeAfter()
        {
        }

 



        /* item */
        /// <summary>
        /// Starts an insert operation. Valid with master brokers only.
        /// </summary>
        public void Insert()
        {
            DataMode OldState = State;
            State = DataMode.Insert;
            try
            {
                Inserting = true;
                try
                {
                    DoInsertBefore();
                    OnStateChange(ExecTime.Before, State, OldState);
                    CheckCanInsert();
                    DoInsert();
                    DoInsertAfter();
                    OnStateChange(ExecTime.After, State, OldState);
                }
                finally
                {
                    Inserting = false;
                }
            }
            catch
            {
                State = OldState;
                throw;
            }

        }
        /// <summary>
        /// Starts an edit operation. Valid with master brokers only.
        /// </summary>
        public void Edit(object RowId)
        {
            DataMode OldState = State;
            State = DataMode.Edit;
            try
            {
                Loading = true;
                try
                {
                    DoEditBefore(RowId);
                    OnStateChange(ExecTime.Before, State, OldState, RowId);
                    CheckCanEdit(RowId);
                    DoEdit(RowId);
                    LastEditedId = RowId;
                    DoEditAfter(RowId);
                    OnStateChange(ExecTime.After, State, OldState, RowId);
                }
                finally
                {
                    Loading = false;
                }

            }
            catch
            {
                State = OldState;
                throw;
            }

        }
        /// <summary>
        /// Deletes a row. Valid with master brokers only.
        /// </summary>
        public void Delete(object RowId)
        {
            Deleting = true;
            try
            {
                DoDeleteBefore(RowId);
                OnStateChange(ExecTime.Before, DataMode.Delete, State, RowId);
                CheckCanDelete(RowId);
                DoDelete(RowId);
                LastDeletedId = RowId;
                DoDeleteAfter(RowId);
                OnStateChange(ExecTime.After, DataMode.Delete, State,  RowId);
            }
            finally
            {
                Deleting = false;
            }
        }
        /// <summary>
        /// Commits changes after an insert or edit. Valid with master brokers only.
        /// <para>Returns the row id of the tblItem commited row.</para>
        /// </summary>
        public object Commit(bool Reselect)
        {
            DataMode OldState = State;
            try
            {
                Commiting = true;
                try
                {
                    string PlusEventName = "." + OldState.ToString();
                    DoCommitBefore(Reselect);

                    OnStateChange(ExecTime.Before, DataMode.Commit, State,  null, Reselect);
                    CheckCanCommit(Reselect);
                    LastCommitedId = DoCommit(Reselect);
                    DoCommitAfter(Reselect);
                    OnStateChange(ExecTime.After, DataMode.Commit, State,  null, Reselect);
                }
                finally
                {
                    Commiting = false;
                }


                State = DataMode.Edit;
                return LastCommitedId;
            }
            catch
            {
                State = OldState;
                throw;
            }
        }
        /// <summary>
        /// Cancels changes after an insert or edit. Valid with master brokers only.
        /// </summary>
        public void Cancel()
        {
            DoCancelBefore();
            OnStateChange(ExecTime.Before, DataMode.Cancel, State);
            DoCancel();
            DoCancelAfter();
            OnStateChange(ExecTime.After, DataMode.Cancel, State);
        }

        /* json */  
        /// <summary>
        /// JsonBroker action
        /// </summary>
        public virtual JsonBroker JsonInitialize()
        {
            JsonBroker Result = new JsonBroker(this);
            foreach (SelectSql SS in Result.SelectList)
                SS.TranslateColumnCaptions();

            return Result;
        }
        /// <summary>
        /// JsonBroker action
        /// </summary>
        public virtual JsonBroker JsonInsert()
        {
            Insert();
            JsonBroker Result = new JsonBroker(this);
            return Result;
        }
        /// <summary>
        /// JsonBroker action
        /// </summary>
        public virtual JsonBroker JsonEdit(string Id)
        {
            object oId = Id;
            DataColumn Column = tblItem.Columns[tblItem.PrimaryKeyField];
            if (Column.DataType == typeof(int))
                oId = Convert.ToInt32(Id);
            Edit(oId);
            JsonBroker Result = new JsonBroker(this);
            return Result;
        }
        /// <summary>
        /// JsonBroker action
        /// </summary>
        public virtual void JsonDelete(string Id)
        {
            object oId = Id;
            DataColumn Column = tblItem.Columns[tblItem.PrimaryKeyField];
            if (Column.DataType == typeof(int))
                oId = Convert.ToInt32(Id);
            Delete(oId);
        }
        /// <summary>
        /// JsonBroker action
        /// </summary>
        public virtual JsonBroker JsonCommit(JsonBroker JB)
        {

            this.State = JB.State;

            Action<MemTable> ProcessTable = null;
            ProcessTable = delegate (MemTable Table)
            {
                JsonDataTable JTable = JB.Tables.FirstOrDefault(item => Table.TableName.IsSameText(item.Name));
                if (JTable != null)
                {
                    JTable.RowsTo(Table);
                    foreach (MemTable DetailTable in Table.Details)
                    {
                        ProcessTable(DetailTable);
                    }
                }
            };


            ProcessTable(tblItem);

            Commit(true);

            JsonBroker Result = new JsonBroker(this);
            return Result;
        }


        /// <summary>
        /// Event trigger
        /// </summary>
        protected virtual void OnStateChange(ExecTime Stage, DataMode State, DataMode OldState, object RowId = null, bool Reselect = false)
        {
            if (StateChange != null)
            {
                BrokerEventArgs Args = new BrokerEventArgs(this, Stage, State, OldState, RowId, Reselect);
                StateChange(this, Args);
            }
        }


        /* item DoXXX before and after methods */
        /// <summary>
        /// Called before the <see cref="DoInsert"/>
        /// </summary>
        protected virtual void DoInsertBefore()
        {
        }
        /// <summary>
        /// Called by the <see cref="Insert"/> to actually starts an insert operation.
        /// </summary>
        protected virtual void DoInsert()
        {
        }
        /// <summary>
        /// Called after the <see cref="DoInsert"/>
        /// </summary>
        protected virtual void DoInsertAfter()
        {
        }
        /// <summary>
        /// Called before the <see cref="DoEdit"/>
        /// </summary>
        protected virtual void DoEditBefore(object RowId)
        {
        }
        /// <summary>
        /// Called by the <see cref="Edit"/> to actually starts an edit operation.
        /// </summary>
        protected virtual void DoEdit(object RowId)
        {
        }
        /// <summary>
        /// Called after the <see cref="DoEdit"/>
        /// </summary>
        protected virtual void DoEditAfter(object RowId)
        {
            AcceptChanges();
        }
        /// <summary>
        /// Called before the <see cref="DoDelete"/>
        /// </summary>
        protected virtual void DoDeleteBefore(object RowId)
        {
        }
        /// <summary>
        /// Called by the <see cref="Delete"/> to actually delete a row to the underlying table tree (database).
        /// </summary>
        protected virtual void DoDelete(object RowId)
        {
        }
        /// <summary>
        /// Called after the <see cref="DoDelete"/>
        /// </summary>
        protected virtual void DoDeleteAfter(object RowId)
        {
            AcceptChanges();
        }
        /// <summary>
        /// Called before the <see cref="DoCommit"/>
        /// </summary>
        protected virtual void DoCommitBefore(bool Reselect)
        {
        }
        /// <summary>
        /// Called by the <see cref="Commit"/> to actually commit changes made by the <see cref="Insert"/> or <see cref="Edit"/> methods,
        /// to the underlying table tree (database).
        /// <para>Returns the row id of the tblItem commited row.</para>
        /// </summary>
        protected virtual object DoCommit(bool Reselect)
        {
            return null;
        }
        /// <summary>
        /// Called after the <see cref="DoCommit"/>
        /// </summary>
        protected virtual void DoCommitAfter(bool Reselect)
        {
            AcceptChanges();
        }
        /// <summary>
        /// Called before the <see cref="DoCancel"/>
        /// </summary>
        protected virtual void DoCancelBefore()
        {
        }
        /// <summary>
        /// Called by the <see cref="Cancel"/> to actually rollback changes made by the <see cref="Insert"/> or <see cref="Edit"/> methods
        /// to the in-memory data table tree.
        /// </summary>
        protected virtual void DoCancel()
        {
        }
        /// <summary>
        /// Called after the <see cref="DoCancel"/>
        /// </summary>
        protected virtual void DoCancelAfter()
        {
            RejectChanges();
        }

        /* item checks */
        /// <summary>
        /// Called by the <see cref="Insert"/> and throws an exception if, for some reason,
        /// starting an insert operation is considered invalid.
        /// </summary>
        public virtual void CheckCanInsert()
        {
            if (IsListBroker)
                Sys.Error("Can not insert item in a list broker.");
        }
        /// <summary>
        /// Called by the <see cref="Edit"/> and throws an exception if, for some reason,
        /// starting an edit operation is considered invalid.
        /// </summary>
        public virtual void CheckCanEdit(object RowId)
        {
            if (IsListBroker)
                Sys.Error("Can not edit item in a list broker.");

            if (Sys.IsNull(RowId))
                Sys.Error("Can not edit item. Invalid RowId");

        }
        /// <summary>
        /// Called by the <see cref="Delete"/> and throws an exception if, for some reason,
        /// deleting the row in the database is considered invalid.
        /// </summary>
        public virtual void CheckCanDelete(object RowId)
        {
            if (IsListBroker)
                Sys.Error("Can not delete item in a list broker.");

            if (Sys.IsNull(RowId))
                Sys.Error("Can not delete item. Invalid RowId");
        }
        /// <summary>
        /// Called by the <see cref="Commit"/> and throws an exception if, for some reason,
        /// commiting item is considered invalid.
        /// </summary>
        public virtual void CheckCanCommit(bool Reselect)
        {
            if (IsListBroker)
                Sys.Error("Can not commit item in a list broker.");
        }

        /* item miscs */
        /// <summary>
        /// Called to commit changes made to the tblItem
        /// </summary>
        protected void AcceptChanges()
        {
            DoAcceptChanges();
        }
        /// <summary>
        /// Called to rollback changes made to the tblItem
        /// </summary>
        protected void RejectChanges()
        {
            DoRejectChanges();
        }
        /// <summary>
        /// Called by the <see cref="AcceptChanges"/> to actually commit changes made to the tblItem
        /// </summary>
        protected virtual void DoAcceptChanges()
        {
            if ((ftblItem != null) && (ftblItem.DataSet != null))
                ftblItem.DataSet.AcceptChanges();
        }
        /// <summary>
        /// Called by the <see cref="RejectChanges"/> to actually rollback changes made to the tblItem
        /// </summary>
        protected virtual void DoRejectChanges()
        {
            if ((ftblItem != null) && (ftblItem.DataSet != null))
                ftblItem.DataSet.RejectChanges();
        }



 

        /* constructors */
        /// <summary>
        /// Constructor
        /// </summary>
        public Broker()
        {
            DataSet = Db.CreateDataset("Broker");
            Tables = new Tables(DataSet);
        }



        /* static */
        /// <summary>
        /// Creates and returns a broker based on the specified BrokerDescriptor.
        /// <para>The Initialized flag controls the initialization of the broker.</para>
        /// </summary>
        static public Broker Create(BrokerDescriptor BrokerDes, bool Initialized, bool AsListBroker)
        {
            if (BrokerDes != null)
            {
                Broker Result = TypeStore.Create(BrokerDes.TypeClassName) as Broker;

                if (Result != null)
                {
                    /* pass a clone of the broker descriptor */
                    Result.Variables["BrokerDescriptor"] = BrokerDes.Clone();

                    if ((Result.CodeProducer == null) && !string.IsNullOrEmpty(BrokerDes.CodeProducerName))
                        Result.CodeProducer = CodeProducer.Create(BrokerDes.CodeProducerName, BrokerDes.MainTableName);

                    OnBrokerCreated(Result);

                    if (Initialized)
                        Result.Initialize(AsListBroker || Result.IsListBroker);

                    OnBrokerInitialized(Result);

                    return Result;
                }
            }

            return null;

        }
        /// <summary>
        /// Creates and returns a broker based on the specified BrokerDescriptor.
        /// <para>The Initialized flag controls the initialization of the broker.</para>
        /// </summary>
        static public Broker Create(BrokerDescriptor BrokerDes, bool Initialized)
        {
            return Create(BrokerDes, Initialized, false);
        }
        /// <summary>
        /// Creates and returns a broker, based on Name. 
        /// <para>For this method to suceed, a BrokerDescriptor with the specified name
        /// must be either registered with the Registry, or saved to the SysData table.</para>
        /// <para>The Initialized flag controls the initialization of the broker.</para>
        /// </summary>
        static public Broker Create(string Name, bool Initialized, bool AsListBroker)
        {
            BrokerDescriptor Descriptor = FindDescriptor(Name);
            if (Descriptor != null)
                return Create(Descriptor, Initialized, AsListBroker);

            Sys.Error("BrokerDescriptor not found: {0}", Name);

            return null;

        }
        /// <summary>
        /// Creates and returns a broker, based on Name. 
        /// <para>For this method to suceed, a BrokerDescriptor with the specified name
        /// must be either registered with the Registry, or saved to the SysData table.</para>
        /// <para>The Initialized flag controls the initialization of the broker.</para>
        /// </summary>
        static public Broker Create(string Name, bool Initialized)
        {
            return Create(Name, Initialized, false);
        }
        /// <summary>
        /// Searches the Registry and the SysData table for a BrokerDescriptor
        /// with the specified Name (DataName). Returns null on failure.
        /// </summary>
        static public BrokerDescriptor FindDescriptor(string DataName)
        {
            BrokerDescriptor Result = null;
            try
            {
                // system tables may not present, because not all applications need them
                // so guard the statement and swallow any exception
                Result = BrokerSysDataItem.FindDescriptor(DataName);
            }
            catch  
            {
            }
            
            if (Result == null)
                Result = Registry.Brokers.Find(DataName);
            return Result;
        }

 

        /* copy-paste */
        /// <summary>
        /// Not yet.
        /// </summary>
        public virtual bool Copy()
        {
            return false;
        }
        /// <summary>
        /// Not yet.
        /// </summary>
        public virtual bool Paste()
        {
            return false;
        }
        /// <summary>
        /// Not yet.
        /// </summary>
        public virtual bool CanPaste()
        {
            return false;
        }

        /* miscs */
        /// <summary>
        /// Locates and returns a row or null. 
        /// <para>FieldName is the column name to search and Value the value to locate</para>
        /// </summary>
        public virtual DataRow Locate(string FieldName, object Value)
        {
            return null;
        }
        /// <summary>
        /// Backs-up the current item into a temp database table.
        /// <para>Used with Compact Framework to prevent data loss.</para>
        /// </summary>
        public virtual void BackUp()
        {
        }
        /// <summary>
        /// Returns the TableDescriptor of TableName, if any, else null.
        /// <para>If TableName is null or empty, it returns the MainTable (tblItem) descriptor.</para>
        /// </summary>
        public virtual TableDescriptor TableDescriptorOf(string TableName)
        {
            return null;
        }
        /// <summary>
        /// Returns the FieldDescriptor of TableName.FieldName, if any, else null.
        /// <para>If TableName is null or empty, it returns the MainTable (tblItem) descriptor.</para>
        /// </summary>
        public virtual FieldDescriptor FieldDescriptorOf(string TableName, string FieldName)
        {
            return null;
        }
        /// <summary>
        /// Finds and returns a DataTable by TableName, if any, else null.
        /// </summary>
        public MemTable FindTable(string TableName)
        {
            MemTable Result = Tables.Find(TableName);

            if (Result == null)
            {
                if (string.IsNullOrEmpty(TableName) || Sys.IsSameText(TableName, "Item") || Sys.IsSameText(TableName, TableDescriptors.ITEM))
                    return this.Tables[MainTableName];
                else if (Sys.IsSameText(TableName, "Lines") || Sys.IsSameText(TableName, TableDescriptors.LINES))
                    return this.Tables[LinesTableName];
                else if (Sys.IsSameText(TableName, "SubLines") || Sys.IsSameText(TableName, TableDescriptors.SUBLINES))
                    return this.Tables[SubLinesTableName];
            }

            return Result;
        }


        /* properties */
        /// <summary>
        /// True while broker is in the initialization phase.
        /// </summary>
        public bool Initializing { get; protected set; }
        /// <summary>
        /// True after the broker is Initialized.
        /// </summary>
        public bool Initialized { get; protected set; }
        /// <summary>
        /// True while inserting, that is while Insert() executes.
        /// </summary>
        public bool Inserting
        {
            get { return fInserting > 0; }
            protected set
            {
                if (value)
                    fInserting++;
                else
                    fInserting--;

                if (fInserting < 0)
                    fInserting = 0;
            }
        }
        /// <summary>
        /// True while loading, that is while Edit() executes.
        /// </summary>
        public bool Loading
        {
            get { return fLoading > 0; }
            protected set
            {
                if (value)
                    fLoading++;
                else
                    fLoading--;

                if (fLoading < 0)
                    fLoading = 0;
            }
        }
        /// <summary>
        /// True while deleting, that is while Delete() executes.
        /// </summary>
        public bool Deleting
        {
            get { return fDeleting > 0; }
            protected set
            {
                if (value)
                    fDeleting++;
                else
                    fDeleting--;

                if (fDeleting < 0)
                    fDeleting = 0;
            }
        }
        /// <summary>
        /// True while commiting, that is while Commit() executes.
        /// </summary>
        public bool Commiting
        {
            get { return fCommiting > 0; }
            protected set
            {
                if (value)
                    fCommiting++;
                else
                    fCommiting--;

                if (fCommiting < 0)
                    fCommiting = 0;
            }
        }

        /// <summary>
        /// Gets or set a flag indicating whether this broker is in batch mode.
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool BatchMode { get; set; }

        /// <summary>
        /// Gets the DataSet of the broker. All data tables of the broker belong to the same DataSet.
        /// </summary>
        public DataSet DataSet { get; protected set; } 
        /// <summary>
        /// Gets the item table which is the top in the tree of the correlated tables.
        /// </summary>
        public MemTable tblItem { get { return ftblItem; } }
        /// <summary>
        /// Gets the detail table, if any.
        /// </summary>
        public MemTable tblLines { get { return ftblLines; } }
        /// <summary>
        /// Gets the sub-detail table, if any
        /// </summary>
        public MemTable tblSubLines { get { return ftblSubLines; } }


        /// <summary>
        /// Gets the Tables list of tables. All data tables of the broker are included in that list.
        /// </summary>
        public Tables Tables { get; protected set; } 
        /// <summary>
        /// Gets the code producer of the broker.
        /// </summary>
        public CodeProducer CodeProducer { get; protected set; }
        /// <summary>
        /// Gets the variables of the broker.
        /// </summary>
        public Dictionary<string, object> Variables { get; protected set; } = new Dictionary<string, object>();
        /// <summary>
        /// True if this is a list broker
        /// </summary>
        public virtual bool IsListBroker { get; protected set; }
        /// <summary>
        /// True if this is a master broker.
        /// </summary>
        public virtual bool IsMasterBroker { get { return !IsListBroker; } }
        /// <summary>
        /// Gets or sets the "onwer" of this broker
        /// </summary>
        public object Owner { get; set; }
        /// <summary>
        /// Returns the "data State" of the broker. It could be Insert, Edit or None.
        /// <para>The State remains Insert or Edit after the Insert() or Edit() is called. 
        /// A call to Commit() sets the State to Edit. </para>
        /// </summary>
        public DataMode State { get; protected set; } = DataMode.None;
        /// <summary>
        /// Returns the value of the Id field of the tblItem
        /// </summary>
        public virtual object Id { get { return Row != null ? Row[tblItem.PrimaryKeyField] : DBNull.Value; } }
        /// <summary>
        /// Returns the id of the item the last Edit() operation has loaded
        /// </summary>
        public virtual object LastEditedId { get; protected set; }
        /// <summary>
        /// Returns the Id of the last commit
        /// </summary>
        public virtual object LastCommitedId { get; protected set; }
        /// <summary>
        /// Returns the Id of the last delete
        /// </summary>
        public virtual object LastDeletedId { get; protected set; }

        /// <summary>
        /// Returns the table name of the main table
        /// </summary>
        public virtual string MainTableName { get; protected set; }
        /// <summary>
        /// Returns the table name of the Lines table
        /// </summary>
        public virtual string LinesTableName { get; protected set; }
        /// <summary>
        /// Returns the table name of the SubLines table
        /// </summary>
        public virtual string SubLinesTableName { get; protected set; }
        /// <summary>
        /// Returns the table name of the Backup table
        /// </summary>
        public virtual string BackupTableName { get; protected set; }
        /// <summary>
        /// An integer Id from the SYS_ENTITY table 
        /// <para>It may points to an application Entity (for example Customer, Order, Employee, etc)</para>
        /// <para>Defaults to 0, meaning no entity Id.</para>
        /// <para>NOTE: EntityId is used by forms in order to call SysAction and Document services. No EntityId, no such services.</para>
        /// </summary>
        public virtual int EntityId { get; set; }
        /// <summary>
        /// The name of the Entity this broker represents
        /// </summary>
        public virtual string EntityName { get; set; }
        /// <summary>
        /// Returns the first row of the tblItem.
        /// <para>WARNING: Valid only in insert and edit mode.</para>
        /// </summary>
        public virtual DataRow Row { get { return tblItem.Rows.Count > 0 ? tblItem.Rows[0] : null; } }


        /* events */
        /// <summary>
        /// Occurs in any state change, i.e. Insert, Edit, Delete, etc.
        /// </summary>
        public event EventHandler<BrokerEventArgs> StateChange;

        /* static events */
        /// <summary>
        /// Occurs just after a broker is created
        /// </summary>
        static public event EventHandler<BrokerEventArgs> BrokerCreated;
        /// <summary>
        /// Occurs just after a broker is initialized
        /// </summary>
        static public event EventHandler<BrokerEventArgs> BrokerInitialized;

    }












}
