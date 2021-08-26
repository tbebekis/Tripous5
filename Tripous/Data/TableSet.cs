/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a set of correlated tables belonging to a certain table tree, such as Customers, Suppliers, Trades etc.
    /// </summary>
    public class TableSet
    {
 

        /// <summary>
        /// Field
        /// </summary>
        protected MemTable topTable;
        /// <summary>
        /// Field
        /// </summary>
        protected List<MemTable> tableTree = new List<MemTable>();
        /// <summary>
        /// Field
        /// </summary>
        protected List<MemTable> Queries;
 

        /* flags */
        /// <summary>
        /// Field
        /// </summary>
        protected bool generateSql = false;
        /// <summary>
        /// Field
        /// </summary>
        protected bool pessimisticMode = false;
        /// <summary>
        /// Field
        /// </summary>
        protected bool cascadeDeletes = false;


        /// <summary>
        /// Field
        /// </summary>
        protected Guid stamp;



        /* initialization */
        /// <summary>
        /// Adds Table an all of its detail tables to tableTree.
        /// </summary>
        protected virtual void AddTableToTree(MemTable Table)
        {
            if (tableTree.IndexOf(Table) == -1)
                tableTree.Add(Table);

            for (int i = 0; i < Table.Details.Count; i++)
                AddTableToTree(Table.Details[i]);
        }
        /// <summary>
        /// Constructs the table tree
        /// </summary>
        protected virtual void ConstructTableTree()
        {
            AddTableToTree(topTable);
        }
        /// <summary>
        /// Sets the MaxDetailLevel, that is the depth of the details.
        /// </summary>
        protected virtual void SetMaxDetailLevel()
        {
            MaxDetailLevel = 0;

            foreach (MemTable Table in tableTree)
                MaxDetailLevel = Math.Max(MaxDetailLevel, Table.Level);

        }
        /// <summary>
        /// Queries in TableSet is a flagList of DataTables used as look-ups etc. This method
        /// executes the SELECT S statement for each of those queries.
        /// </summary>
        protected virtual void SelectQueries()
        {
            if (Queries != null)
            {
                MemTable Table;
                for (int i = 0; i < Queries.Count; i++)
                {
                    Table = Queries[i];

                    if (string.IsNullOrWhiteSpace(Table.SqlStatements.SelectSql))
                        Table.SqlStatements.SelectSql = "select * from " + Table.TableName; 

                    Store.SelectTo(Table, Table.SqlStatements.SelectSql);

                    if (Table.SqlStatements.HasTitleKeys())
                    {
                        Table.SetColumnCaptionsFrom(Table.SqlStatements.FieldTitleKeys.ToDictionary(), true);
                    }
                    else
                    {
                        for (int j = 0; j < Table.Columns.Count; j++)
                        {
                            Table.Columns[j].IsVisible(!Sys.IsSameText("ID", Table.Columns[j].ColumnName));
                        }
                    }

                }
            }


        }
        /// <summary>
        /// Generates text statements
        /// </summary>
        protected virtual void GenerateSqlStatements()
        {
            if (generateSql)
            {
                for (int i = 0; i < tableTree.Count; i++)
                    Db.BuildSql(tableTree[i], Store, tableTree[i] == topTable);
            }
        }
 

        /* edit operation */
        /// <summary>
        /// Cancels any pending edit operation in the whole table tree.
        /// </summary>
        protected virtual void InternalCancel()
        {
            int Level = MaxDetailLevel;

            // in reverse order
            while (Level >= topTable.Level)
            {
                foreach (MemTable Table in tableTree)
                {
                    if (Table.Level == Level)
                    {
                        foreach (DataRow Row in Table.Rows)
                        {
                            if (Row.HasVersion(DataRowVersion.Proposed))
                            {
                                Row.EndEdit();
                            }
                        }
                    }
                }

                Level--;
            }
        }

        /* database SELECT tree */
        /// <summary>
        /// Executes the SELECT SqlText and appends the resulted rows to the DetailTable MemTable.
        /// </summary>
        private void Select_DoAddToDetail(string SqlText, MemTable Detail)
        {
            DataTable Source = Store.Select(SqlText);

            if (Detail.Columns.Count == 0)
                Source.CopyStructureTo(Detail);

            Detail.BeginLoadData();
            try
            {
                for (int i = 0; i < Source.Rows.Count; i++)
                {
                    if (Detail.Locate(Detail.PrimaryKeyField, new object[] { Source.Rows[i][Detail.PrimaryKeyField] }, LocateOptions.None) == null)
                        Source.Rows[i].AppendTo(Detail);
                }
            }
            finally
            {
                Detail.EndLoadData();
            }

        }
        /// <summary>
        /// Executes the SELECT of the DetailTable.  
        /// </summary>
        private void Select_DoDetail(MemTable MasterTable, MemTable Detail)
        {
            if (!string.IsNullOrWhiteSpace(Detail.SqlStatements.SelectSql))
            {
                // 1. SqlText execution ===================================================
                if ((MasterTable.Rows.Count > 0) && (MasterTable.Columns.Contains(Detail.MasterKeyField)))
                {
                    /*  limit the number of elements inside the in (...),  in order
                        to avoid problems with database servers that have such a limit.    */
                    string[] KeyValuesList = MasterTable.GetKeyValuesList(Detail.MasterKeyField, 100, false);

                    StringBuilder SB = new StringBuilder();
                    for (int i = 0; i < KeyValuesList.Length; i++)
                    {
                        SB.Clear();
                        SB.AppendLine(Detail.SqlStatements.SelectSql);
                        SB.AppendLine($"where ");
                        SB.AppendLine($"{Detail.TableName}.{Detail.DetailKeyField} in {KeyValuesList[i]}");

                        Select_DoAddToDetail(SB.ToString(), Detail);
                    }
                }

                Detail.SetColumnCaptionsFrom(Detail.SqlStatements.FieldTitleKeys.ToDictionary(), HideUntitleDisplayLabels);

                if (!Detail.IsEmpty)
                    Select_DoDetails(Detail);
            }


 

        }
        /// <summary>
        /// Executes the SELECT of Details of the MasterTable.
        /// </summary>
        private void Select_DoDetails(MemTable MasterTable)
        {
            foreach (MemTable DetailTable in MasterTable.Details)
            {
                Select_DoDetail(MasterTable, DetailTable);
                DetailTable.AcceptChanges();
            }
        }

        // TODO: pessimistic stamp handling
        /* pessimistic mode */
        /// <summary>
        /// Not yet
        /// </summary>
        protected virtual void SavePessimisticStamp(object RowId)
        {
        }
        /// <summary>
        /// Not yet
        /// </summary>
        protected virtual void DeletePessimisticStamp(object RowId)
        {
        }
        /// <summary>
        /// Not yet
        /// </summary>
        protected virtual bool PessimisticStampExists(object RowId)
        {
            return false;
        }

        /* Oids and Guid */
        /// <summary>
        /// Returns the next id value of a generator named after the TableName table.
        /// <para>It should be used only with databases that support generators or when a CustomOid object is used.</para>
        /// </summary>
        protected virtual int NextId(string TableName)
        {
            if (Transaction != null)
                return Store.NextId(Transaction, TableName);
            return Store.NextId(TableName);
        }
        /// <summary>
        /// Returns the last id produced by an INSERT statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        protected virtual int LastId(string TableName)
        {
            if (Transaction != null)
                return Store.LastId(Transaction, TableName);
            return Store.LastId(TableName);
        }
        /// <summary>
        /// Creates and returns a new Guid surrounded by {}
        /// </summary>
        protected string GetGuid()
        {
            return Sys.GenId();
        }

        /// <summary>
        /// Returns true if Oids are needed before commiting a row to the database
        /// </summary>
        protected virtual bool OidIsBefore { get { return Store.Provider.OidMode == OidMode.Before; } }
        /// <summary>
        /// Returns true if Oids are needed after commiting a row to the database
        /// </summary>
        protected virtual bool OidIsAfter { get { return !OidIsBefore; } }

        /* event triggers */
        /// <summary>
        /// Triggers the TransactionDelete event.
        /// </summary>
        protected void OnTransactionStageDelete(TransactionStage Stage, ExecTime ExecTime, object RowId)
        {
            if (TransactionStageDelete != null)
                TransactionStageDelete(this, new TransactionStageEventArgs(Store, Transaction, Stage, ExecTime, RowId));
        }
        /// <summary>
        /// Triggers the TransactionCommit event.
        /// </summary>
        protected void OnTransactionStageCommit(TransactionStage Stage, ExecTime ExecTime)
        {
            if (TransactionStageCommit != null)
                TransactionStageCommit(this, new TransactionStageEventArgs(Store, Transaction, Stage, ExecTime, -1));
        }

        /* miscs */
        /// <summary>
        /// Removes all data rows from Table and its details
        /// </summary>
        protected virtual void Empty(MemTable Table)
        {
            for (int i = 0; i < Table.Details.Count; i++)
                Empty(Table.Details[i]);

            Table.Rows.Clear();
            Table.AcceptChanges();
        }
        /// <summary>
        /// Puts Variable values into the S by replacing value placeholders.
        /// <para>The default prefix for a Variable inside CommandText text is :@</para>
        /// </summary>
        protected virtual void ResolveSql(ref string SqlText)
        {
            SqlValueProviders.Process(ref SqlText, Store);
        }
 


        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public TableSet(SqlStore Store, MemTable TopTable, List<MemTable> Queries, TableSetFlags Flags = TableSetFlags.GenerateSql)
        {
            if (TopTable == null)
                throw new ArgumentNullException("TopTable"); 

            this.Store = Store;
            this.topTable = TopTable;
            this.Queries = Queries;

            generateSql = (Flags & TableSetFlags.GenerateSql) == TableSetFlags.GenerateSql;
            pessimisticMode = (Flags & TableSetFlags.PessimisticMode) == TableSetFlags.PessimisticMode;
            cascadeDeletes = !((Flags & TableSetFlags.NoCascadeDeletes) == TableSetFlags.NoCascadeDeletes);

            ConstructTableTree();
            SetMaxDetailLevel();
            GenerateSqlStatements();
            SelectQueries();
        }

        /* database operations */
        /// <summary>
        /// Selects the whole table tree from the database starting from the top table (which is a single-row table).
        /// <para>RowId could be string or integer and is the primary key value of the top table.</para>
        /// </summary>
        public void Select(object RowId)
        {
            ProcessEmpty();

            topTable.EventsDisabled = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(topTable.SqlStatements.SelectRowSql))
                {
                    topTable.BeginLoadData();
                    try
                    {
                        Store.SelectTo(topTable, topTable.SqlStatements.SelectRowSql, RowId);
                    }
                    finally
                    {
                        topTable.EndLoadData();
                    }

                    topTable.AcceptChanges();
                }


                // select stock tables
                if ((RowId != null) && (topTable.Rows.Count >= 1))
                {
                    MemTable StockTable;
                    for (int i = 0; i < topTable.StockTables.Count; i++)
                    {
                        StockTable = topTable.StockTables[i];
                        Store.SelectTo(StockTable, StockTable.SqlStatements.SelectRowSql, topTable.Rows[0]);
                    }
                }

                Select_DoDetails(topTable); 

                topTable.SetColumnCaptionsFrom(topTable.SqlStatements.FieldTitleKeys.ToDictionary(), HideUntitleDisplayLabels);

                if (!topTable.IsEmpty)
                    SavePessimisticStamp(RowId);

            }
            finally
            {
                topTable.EventsDisabled = false;
            }

            IsInsert = false;
        }
        /// <summary>
        /// Deletes the whole table tree to the database. The way this method process table deletes, depends on the cascadeDeletes flag.
        /// <para>RowId could be string or integer and is the primary key value of the top table.</para>
        /// </summary>
        public void Delete(object RowId)
        {
            if (RowId == null)
                return;

            // first, select the top table and the detail tables
            Select(RowId);
            if (topTable.Rows.Count != 1)
            {
                DeletePessimisticStamp(RowId);
                return;
            }

            bool Error = false;

            topTable.EventsDisabled = true;
            //topTable.Details.Active = false;
            try
            {
                // delete the top row, which deletes all detail rows too.
                topTable.Rows[0].Delete();

                // then, inside a Transaction
                OnTransactionStageDelete(TransactionStage.Start, ExecTime.Before, RowId);

                using (Transaction = Store.BeginTransaction())
                {
                    OnTransactionStageDelete(TransactionStage.Start, ExecTime.After, RowId);
                    try
                    {
                        PostDeletes();

                        OnTransactionStageDelete(TransactionStage.Commit, ExecTime.Before, RowId);
                        Transaction.Commit();
                        OnTransactionStageDelete(TransactionStage.Commit, ExecTime.After, RowId);

                        DeletePessimisticStamp(RowId);
                    }
                    catch
                    {
                        Error = true;
                        topTable.DataSet.RejectChanges();
                        OnTransactionStageDelete(TransactionStage.Rollback, ExecTime.Before, RowId);
                        Transaction.Rollback();
                        OnTransactionStageDelete(TransactionStage.Rollback, ExecTime.After, RowId);
                        throw;
                    }
                }


            }
            finally
            {
                //topTable.Details.Active = true;
                topTable.EventsDisabled = false;
                Transaction = null;
            }

            if (Error)
            {
                //Select(RowId);
            }

        }
        /// <summary>
        /// Commits the whole table tree to the database. It can be either an insert or an update.
        /// </summary>
        public object Commit(bool Reselect)
        {
            topTable.EventsDisabled = true;
            //topTable.Details.Active = false;
            try
            {
                // inside a single Transaction
                OnTransactionStageCommit(TransactionStage.Start, ExecTime.Before);


                using (Transaction = Store.BeginTransaction())
                {
                    OnTransactionStageCommit(TransactionStage.Start, ExecTime.After);

                    try
                    {
                        PostChanges();

                        OnTransactionStageCommit(TransactionStage.Commit, ExecTime.Before);
                        Transaction.Commit();
                        topTable.DataSet.AcceptChanges();   // clear logs    
                        OnTransactionStageCommit(TransactionStage.Commit, ExecTime.After);
                    }
                    catch
                    {
                        OnTransactionStageCommit(TransactionStage.Rollback, ExecTime.Before);
                        Transaction.Rollback();
                        OnTransactionStageCommit(TransactionStage.Rollback, ExecTime.After);
                        throw;
                    }
                }

            }
            finally
            {
                //topTable.Details.Active = true;
                topTable.EventsDisabled = false;
                Transaction = null;
            }



            LastCommitedId = null;

            if (topTable.Rows.Count > 0)
                LastCommitedId = topTable.Rows[0][topTable.PrimaryKeyField];

            /*
            if (IsInsert)
            {
                if (topTable.IsStringField(topTable.PrimaryKeyField))
                {
                    if (topTable.Rows.Count > 0)
                        Result = topTable.Rows[0][topTable.PrimaryKeyField];
                }
                else
                {
                    if (OidIsAfter)
                        Result = LastId(topTable.TableName);
                    else if (OidIsBefore && (topTable.Rows.Count > 0))
                        Result = topTable.Rows[0][topTable.PrimaryKeyField];
                }
            }
            else if (!IsInsert && (topTable.Rows.Count > 0))
            {
                Result = topTable.Rows[0][topTable.PrimaryKeyField];
            }
            //*/

            //if ((IsInsert || Reselect) && !Sys.IsNull(Result))
            if (Reselect && !Sys.IsNull(LastCommitedId))
                Select(LastCommitedId);

            IsInsert = false;
            return LastCommitedId;

        }
        /// <summary>
        /// A Commit() version for batch operations.
        /// <para>Starts a transaction and keeps on calling CommitProc() while it returns true.</para>
        /// <para>It commits the transaction each time the TransLimit is reached.</para>
        /// <para>Info is a user defined object.</para>
        /// </summary>
        public void CommitBatch(BatchCommitArgs Args)
        {
            topTable.EventsDisabled = true;
            try
            {
                int Counter = 0;
                int PostCounter = 0;
                bool ShouldPost;

                while (true)
                {
                    Args.Counter = Counter;
                    Args.PostCounter = PostCounter;

                    ShouldPost = Args.BeforeFunc();

                    if (ShouldPost)
                    {
                        if (Transaction == null)
                        {
                            Transaction = Store.BeginTransaction();
                        }

                        PostChanges();

                        topTable.DataSet.AcceptChanges();   // clear logs   

                        LastCommitedId = null;

                        if (topTable.Rows.Count > 0)
                            LastCommitedId = topTable.Rows[0][topTable.PrimaryKeyField];
                    }


                    if (!Args.AfterFunc(LastCommitedId))
                        break;


                    if (ShouldPost)
                    {
                        PostCounter++;

                        if (PostCounter % Args.TransLimit == 0)
                        {
                            Transaction.Commit();
                            Transaction.Dispose();
                            Transaction = null;
                        }
                    }

                    Counter++;
                }

                if (Transaction != null)
                {
                    Transaction.Commit();
                    Transaction.Dispose();
                    Transaction = null;
                }
            }
            finally
            {
                topTable.EventsDisabled = false;
            }
        }
        /// <summary>
        /// Posts any changes (deletes, updates, inserts) to the database
        /// </summary>
        public void PostChanges()
        {
            PostDeletes();
            PostUpdates();
            PostInserts();
        }
        /// <summary>
        /// Executes the SELECT SqlText and puts the returned data rows to the Table.
        /// <para>It is used when selecting for the browse part of a data form.</para>
        /// <para>Normally the Table passed to this method is not part of the table tree of the TableSet.</para>
        /// </summary>
        public int SelectBrowser(MemTable Table, string SqlText)
        {
            int Result = 0;

            if (Table != null)
            {
                if (string.IsNullOrWhiteSpace(SqlText))
                    SqlText = Table.SqlStatements.SelectSql;

                if (SqlText.Trim() != "")
                {
                    Table.BeginLoadData();
                    Table.EventsDisabled = true;
                    try
                    {
                        Result = Store.SelectTo(Table, SqlText);
                        Table.SetColumnCaptionsFrom(Table.SqlStatements.FieldTitleKeys.ToDictionary(), HideUntitleDisplayLabels);
                    }
                    finally
                    {
                        Table.EventsDisabled = false;
                        Table.EndLoadData();
                    }
                }
            }

            return Result;

        }


        /// <summary>
        /// It is used to form an anonymous method.
        /// </summary>
        private delegate void TableDelegate(MemTable Table);

        /* database commit the whole tree: INSERT-UPDATE-DELETE */
        /// <summary>
        /// Processes the DELETE part of a commit
        /// </summary>
        private void PostDeletes()
        {
            /* an anonymous method in order to avoid a separate method, since this code is called twice, below */
            TableDelegate DeleteTable = delegate (MemTable Table)
            {

                DataTable Source = Table.GetDeletedRows();

                if (Source != null)
                {
                    string SqlText = "";

                    /* delete the rows */
                    string[] KeyValuesList = Source.GetKeyValuesList(Table.PrimaryKeyField, 100, true);

                    foreach (string KeyValues in KeyValuesList)
                    {
                        SqlText = string.Format("delete from {0} where {1} in {2}", Table.TableName, Table.PrimaryKeyField, KeyValues);
                        Store.ExecSql(Transaction, SqlText);
                    }
                }
            };

            int Level;

            // deletes in reverse order
            if (cascadeDeletes)
            {
                Level = MaxDetailLevel;
                while (Level >= topTable.Level)
                {
                    foreach (MemTable Table in tableTree)
                    {
                        if (Table.Level == Level)
                        {
                            DeleteTable(Table);
                        }
                    }

                    Level--;
                }
            }
            else // deletes in normal order: let any constraint throw an exception
            {
                Level = topTable.Level;
                while (Level <= MaxDetailLevel)
                {
                    foreach (MemTable Table in tableTree)
                    {
                        if (Table.Level == Level)
                        {
                            DeleteTable(Table);
                        }
                    }

                    Level++;
                }
            }
        }
        /// <summary>
        /// Processes the UPDATE part of a commit
        /// </summary>
        private void PostUpdates()
        {
            object Value;
            int Level = topTable.Level;

            // updates with normal order
            while (Level <= MaxDetailLevel)
            {
                foreach (MemTable Table in tableTree)
                {
                    if ((Table.Level == Level) && !Table.IsEmpty && Table.Columns.Contains(Table.PrimaryKeyField))
                    {
                        foreach (DataRow Row in Table.Rows)
                        {
                            if (Row.RowState == DataRowState.Modified)
                            {
                                if (!Row.IsNull(Table.PrimaryKeyField))
                                {
                                    Value = Row[Table.PrimaryKeyField];
                                    Store.ExecSql(Transaction, Table.SqlStatements.UpdateRowSql, Row);
                                }
                            }
                        }
                    }
                }

                Level++;
            }

        }
        /// <summary>
        /// Processes the INSERT part of a commit
        /// </summary>
        private void PostInserts()
        {
            bool IsString;

            object Value;
            int OldId;
            int NewId;

            // inserts with normal order
            int Level = topTable.Level;
            while (Level <= MaxDetailLevel)
            {
                foreach (MemTable Table in tableTree)
                {
                    if ((Table.Level == Level) && !Table.IsEmpty && Table.Columns.Contains(Table.PrimaryKeyField))
                    {
                        IsString = Table.IsStringField(Table.PrimaryKeyField);

                        foreach (DataRow Row in Table.Rows)
                        {
                            if ((Row.RowState == DataRowState.Added) && !Row.IsNull(Table.PrimaryKeyField))
                            {
                                Value = Row[Table.PrimaryKeyField];

                                /* primary key is a Guid */
                                if (IsString)
                                {
                                    Store.ExecSql(Transaction, Table.SqlStatements.InsertRowSql, Row);
                                }
                                /* primary key is an integer, autoincremented or provided by a generator/sequencer */
                                else
                                {
                                    OldId = Sys.AsInteger(Value, -1);
                                    NewId = OldId;

                                    // generator/sequencer, so get the "correct" Id
                                    if (OidIsBefore)
                                    {
                                        NewId = NextId(Table.TableName);
                                        Row[Table.PrimaryKeyField] = NewId;
                                    }

                                    try
                                    {
                                        Store.ExecSql(Transaction, Table.SqlStatements.InsertRowSql, Row);
                                    }
                                    catch
                                    {
                                        Row[Table.PrimaryKeyField] = OldId;
                                        throw;
                                    }


                                    if (OidIsAfter)
                                    {
                                        NewId = LastId(Table.TableName);
                                        Row[Table.PrimaryKeyField] = NewId;
                                    }


                                    // update Table detail tables with the "correct" master Id.
                                    foreach (MemTable DetailTable in Table.Details)
                                    {
                                        foreach (DataRow DetailRow in DetailTable.Rows)
                                        {
                                            if (!DetailRow.IsNull(DetailTable.DetailKeyField) && (Sys.AsInteger(DetailRow[DetailTable.DetailKeyField], -1) == OldId))
                                                DetailRow[DetailTable.DetailKeyField] = NewId;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Level++;
            }




        }

        /* edit operation handling */
        /// <summary>
        /// Removes all data rows from all tables in the tableTree
        /// </summary>
        public void ProcessEmpty()
        {
            InternalCancel();

            //topTable.ControlsEnabled = false;
            topTable.EventsDisabled = true;
            //topTable.Details.Active = false;
            try
            {
                Empty(topTable);
            }
            finally
            {
                //topTable.Details.Active = true;
                topTable.EventsDisabled = false;
                //topTable.ControlsEnabled = true;
            }
        }
        /// <summary>
        /// Prepares the TableSet for an insert operation (in the tables, NOT the database)
        /// </summary>
        public void ProcessInsert()
        {
            ProcessEmpty();

            //topTable.ControlsEnabled = false;
            topTable.EventsDisabled = true;
            //topTable.Details.Active = false;
            try
            {
                if (topTable.Rows.Count == 0)
                {
                    DataRow Row = topTable.NewRow();
                    topTable.Rows.Add(Row);
                }
            }
            finally
            {
                //topTable.Details.Active = true;
                topTable.EventsDisabled = false;
                //topTable.ControlsEnabled = true;
            }

            IsInsert = true;
        }
        /// <summary>
        /// Cancels an edit operation and re-initializes the table tree.
        /// </summary>
        public void ProcessCancel()
        {
            if (IsInsert)
                ProcessInsert();
            else if (topTable.Rows[0].RowState != DataRowState.Deleted)
                Select(topTable.Rows[0][topTable.PrimaryKeyField]);
        }

        /* properties */
        /// <summary>
        /// Returns the executor
        /// </summary>
        public SqlStore Store { get; protected set; }
        /// <summary>
        /// Returns the current Transaction
        /// </summary>
        public DbTransaction Transaction { get; set; }
        /// <summary>
        /// True when inserting
        /// </summary>
        public bool IsInsert { get; protected set; }
        /// <summary>
        /// If true, then when SELECTing to a MemTable, hides any column not found in the table's SqlStatements.BrowseSelect.DisplayLabels  
        /// </summary>
        public bool HideUntitleDisplayLabels { get; set; }
        /// <summary>
        /// Returns the maximum detail level
        /// </summary>
        public int MaxDetailLevel { get; protected set; }
        /// <summary>
        /// Returns the Id of the last commit
        /// </summary>
        public object LastCommitedId { get; protected set; }

        /* events */
        /// <summary>
        /// Occurs when <see cref="Delete"/>(object RowId) method is called.
        /// </summary>
        public event EventHandler<TransactionStageEventArgs> TransactionStageDelete;
        /// <summary>
        /// Occurs when <see cref="Commit"/>() method is called.
        /// </summary>
        public event EventHandler<TransactionStageEventArgs> TransactionStageCommit;

    }

}
