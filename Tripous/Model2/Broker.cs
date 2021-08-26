using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using Tripous.Data;

namespace Tripous.Model2
{
    /// <summary>
    /// Broker
    /// </summary>
    public class Broker
    {
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
            if ((tblItem != null) && (tblItem.DataSet != null))
                tblItem.DataSet.AcceptChanges();
        }
        /// <summary>
        /// Called by the <see cref="RejectChanges"/> to actually rollback changes made to the tblItem
        /// </summary>
        protected virtual void DoRejectChanges()
        {
            if ((tblItem != null) && (tblItem.DataSet != null))
                tblItem.DataSet.RejectChanges();
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Broker()
        {
        }


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
                    //OnStateChange(ExecTime.Before, State, OldState);
                    CheckCanInsert();
                    DoInsert();
                    DoInsertAfter();
                    //OnStateChange(ExecTime.After, State, OldState);
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
                    //OnStateChange(ExecTime.Before, State, OldState, RowId);
                    CheckCanEdit(RowId);
                    DoEdit(RowId);
                    LastEditedId = RowId;
                    DoEditAfter(RowId);
                    //OnStateChange(ExecTime.After, State, OldState, RowId);
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
                //OnStateChange(ExecTime.Before, DataMode.Delete, State, RowId);
                CheckCanDelete(RowId);
                DoDelete(RowId);
                LastDeletedId = RowId;
                DoDeleteAfter(RowId);
                //OnStateChange(ExecTime.After, DataMode.Delete, State, RowId);
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

                    //OnStateChange(ExecTime.Before, DataMode.Commit, State, null, Reselect);
                    CheckCanCommit(Reselect);
                    LastCommitedId = DoCommit(Reselect);
                    DoCommitAfter(Reselect);
                    //OnStateChange(ExecTime.After, DataMode.Commit, State, null, Reselect);
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
            //OnStateChange(ExecTime.Before, DataMode.Cancel, State);
            DoCancel();
            DoCancelAfter();
            //OnStateChange(ExecTime.After, DataMode.Cancel, State);
        }

        /* item checks */
        /// <summary>
        /// Called by the <see cref="Insert"/> and throws an exception if, for some reason,
        /// starting an insert operation is considered invalid.
        /// </summary>
        public virtual void CheckCanInsert()
        {
            if (IsListBroker)
                Sys.Throw("Can not insert item in a list broker.");
        }
        /// <summary>
        /// Called by the <see cref="Edit"/> and throws an exception if, for some reason,
        /// starting an edit operation is considered invalid.
        /// </summary>
        public virtual void CheckCanEdit(object RowId)
        {
            if (IsListBroker)
                Sys.Throw("Can not edit item in a list broker.");

            if (Sys.IsNull(RowId))
                Sys.Throw("Can not edit item. Invalid RowId");

        }
        /// <summary>
        /// Called by the <see cref="Delete"/> and throws an exception if, for some reason,
        /// deleting the row in the database is considered invalid.
        /// </summary>
        public virtual void CheckCanDelete(object RowId)
        {
            if (IsListBroker)
                Sys.Throw("Can not delete item in a list broker.");

            if (Sys.IsNull(RowId))
                Sys.Throw("Can not delete item. Invalid RowId");
        }
        /// <summary>
        /// Called by the <see cref="Commit"/> and throws an exception if, for some reason,
        /// commiting item is considered invalid.
        /// </summary>
        public virtual void CheckCanCommit(bool Reselect)
        {
            if (IsListBroker)
                Sys.Throw("Can not commit item in a list broker.");
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
        /// Finds and returns a DataTable by TableName, if any, else null.
        /// </summary>
        public MemTable FindTable(string TableName)
        {
            MemTable Result = null;
            
            if (!string.IsNullOrWhiteSpace(TableName))
            {
                Tables.Find(item => TableName.IsSameText(item.TableName));

                if (Result == null)
                {
                    if (Sys.IsSameText(TableName, "Item") || Sys.IsSameText(TableName, SqlBrokerTableDef.ITEM))
                        return Tables.Find(item => MainTableName.IsSameText(item.TableName));    
                    else if (Sys.IsSameText(TableName, "Lines") || Sys.IsSameText(TableName, SqlBrokerTableDef.LINES))
                        return Tables.Find(item => LinesTableName.IsSameText(item.TableName));     
                    else if (Sys.IsSameText(TableName, "SubLines") || Sys.IsSameText(TableName, SqlBrokerTableDef.SUBLINES))
                        return Tables.Find(item => SubLinesTableName.IsSameText(item.TableName));   
                }
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
        /// Returns the "data State" of the broker. It could be Insert, Edit or None.
        /// <para>The State remains Insert or Edit after the Insert() or Edit() is called. 
        /// A call to Commit() sets the State to Edit. </para>
        /// </summary>
        public DataMode State { get; protected set; } = DataMode.None;
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
        /// Gets the Tables list of tables. All data tables of the broker are included in that list.
        /// </summary>
        public List<MemTable> Tables { get; protected set; } = new List<MemTable>();    //public Tables Tables { get; protected set; }
        /// <summary>
        /// Gets the item table which is the top in the tree of the correlated tables.
        /// </summary>
        public MemTable tblItem { get; protected set; }
        /// <summary>
        /// Gets the detail table, if any.
        /// </summary>
        public MemTable tblLines { get; protected set; }
        /// <summary>
        /// Gets the sub-detail table, if any
        /// </summary>
        public MemTable tblSubLines { get; protected set; }

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
        /// Returns the first row of the tblItem.
        /// <para>WARNING: Valid only in insert and edit mode.</para>
        /// </summary>
        public virtual DataRow Row { get { return tblItem.Rows.Count > 0 ? tblItem.Rows[0] : null; } }
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
        /// The name of the Entity this broker represents
        /// </summary>
        public virtual string EntityName { get; set; }
    }
}
