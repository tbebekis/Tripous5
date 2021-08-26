using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

using Tripous.Data;

namespace Tripous.Data
{

    /// <summary>
    /// SqlBroker
    /// </summary>
    public class SqlBroker: Broker
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string GENERIC_SQL_BROKER = "_GENERIC_SQL_BROKER_";

        /// <summary>
        /// Field
        /// </summary>
        protected string fEntityName = string.Empty;

        /* initialization */
        #region Initialization
        /// <summary>
        /// Initializes the broker.
        /// </summary>
        protected override void DoInitialize()
        {
            base.DoInitialize();
            InitializeDescriptor();
            InitializeDatabaseConnection();
            InitializeTables();
        }
       
        /// <summary>
        /// Initializes the descriptor for this broker.
        /// <para>The descriptor may come from a descriptor registry. Otherwise the <see cref="DefineDescriptor"/>() is called. </para>
        /// </summary>
        protected virtual void InitializeDescriptor()
        {
            if (!IsDeclarative)
                DefineDescriptor();
        }
        /// <summary>
        /// Called if no Descriptor is passed as "BrokerDescriptor"
        /// and we are going to have a "manully" defined Descriptor 
        /// </summary>
        protected virtual void DefineDescriptor()
        {
        }

        /// <summary>
        /// Initializes the database connection of this broker. A broker needs to know which is 
        /// the database it operates on.
        /// </summary>
        protected virtual void InitializeDatabaseConnection()
        {
            ConnectionInfo = Db.GetConnectionInfo(Descriptor.ConnectionName);

            if (ConnectionInfo == null)
                Sys.Throw("No database connection defined for Broker. {0}", Descriptor.Name);

            Store = SqlStores.CreateSqlStore(ConnectionInfo);
        }
        /// <summary>
        /// Initializes the Tables that make up the broker. A complex method which "completes"
        /// table descriptor definitions, generates sql statements etc.
        /// </summary>
        protected virtual void InitializeTables()
        {
            this.DataSet.DataSetName = "DS_" + Descriptor.Name;

            MemTable Table;


            // create the main TableDes if not assigned
            if (Descriptor.MainTable == null && !string.IsNullOrWhiteSpace(Descriptor.MainTableName))
            {
                Descriptor.Tables.Add(new SqlBrokerTableDef() { Name = Descriptor.MainTableName }); 
            }

            // ensure that any TableDes is updated with the actual table schema
            foreach (var TableDes in Descriptor.Tables)
            {
                Table = new MemTable();
                Store.GetNativeSchema("", TableDes.Name, TableDes.Name, Table);
                TableDes.UpdateFrom(Table);
            }

            // get the sql generation flags
            BuildSqlFlags SqlFlags = GetBuildSqlFlags();

            // 1. create sql statements for all Tables of the BrokerDescriptor
            // 2. create DataTable objects for all Tables of the BrokerDescriptor    
            TableSqls TempSqlStatements = new TableSqls();
            foreach (var TableDes in Descriptor.Tables)
            {
                TableDes.BuildSql(TempSqlStatements, SqlFlags);
                TableDes.CreateDescriptorTable(Store, Tables, false);
                Table = Tables.Find(item => item.Name.IsSameText(TableDes.Name));
                Table.SqlStatements = TempSqlStatements;
                Table.PrimaryKeyField = TableDes.PrimaryKeyField;
                Table.AutoGenerateGuidKeys = Descriptor.GuidOids;

                Table.ExtendedProperties["Descriptor"] = TableDes;
            }

            tblItem = Tables.Find(item => item.Name.IsSameText(Descriptor.MainTableName));
            tblLines = Tables.Find(item => item.Name.IsSameText(Descriptor.LinesTableName));
            tblSubLines = Tables.Find(item => item.Name.IsSameText(Descriptor.SubLinesTableName));

 
            // Detail Tables    -  find the detail Tables of any table
            InitializeDetails();
            tblItem.Details.Active = true;

            // DataColumn expressions - must be assigned after DataRelations are constructed
            DataColumn Field;
            foreach (var TableDes in Descriptor.Tables)
            {
                Table = Tables.Find(item => item.Name.IsSameText(TableDes.Name));
                if (Table != null)
                {
                    foreach (var FieldDes in TableDes.Fields)
                    {
                        if (!string.IsNullOrEmpty(FieldDes.Expression))
                        {
                            Field = Table.FindColumn(FieldDes.Name);
                            if (Field != null)
                                Field.Expression = FieldDes.Expression;
                        }
                    }
                }
            }

            //  Queries
            CompleteQueryDefs();    // complete any LookUp query missing, that is not explicitly defined
            InitializeQueries();    // select each Query and define field titles }



            // StockTables
            InitializeStockTables();

            // TableSet
            TableSetFlags TableSetFlags = TableSetFlags.None;
 
            if (Descriptor.NoCascadeDeletes)
                TableSetFlags |= TableSetFlags.NoCascadeDeletes;

            // collect query tables
            List<MemTable> QueryTables = new List<MemTable>();
            MemTable QueryTable;

            foreach (var QueryDes in Descriptor.Queries)
            {
                QueryTable = Tables.Find(item => item.Name.IsSameText(QueryDes.Name));

                if (QueryTable != null)
                    QueryTables.Add(QueryTable);
            }


            TableSet = new TableSet(Store, tblItem, QueryTables, TableSetFlags);

            TableSet.TransactionStageCommit += new EventHandler<TransactionStageEventArgs>(TableSet_TransactionStageCommit);
            TableSet.TransactionStageDelete += new EventHandler<TransactionStageEventArgs>(TableSet_TransactionStageDelete);
        }

        /// <summary>
        /// Completes query table descriptors by generating sql SELECT statements if needed.
        /// </summary>
        protected virtual void CompleteQueryDefs()
        {
            foreach (var TableDes in Descriptor.Tables)
            {
                foreach (var FieldDes in TableDes.Fields)
                {                   
                    if ((FieldDes.IsForeignKeyField) && Descriptor.Queries.Find(item => item.Name.IsSameText(FieldDes.ForeignTableName)) == null)
                    {
                        Descriptor.Queries.Add(new SqlBrokerQueryDef() { 
                            Name = FieldDes.ForeignTableName,
                            Sql = FieldDes.GetForeignSelectSql()
                        });
                    }
                }
            }
        }
        /// <summary>
        /// Selects each Query and defines query talbes field titles.
        /// <para>Query Tables are used as look up Tables and they do not participate
        /// in the table tree when inserting, editing and deleting.</para>
        /// </summary>
        protected virtual void InitializeQueries()
        {
            MemTable Table;
            foreach (var QueryDes in Descriptor.Queries)
            {
                Table = Tables.Find(item => item.Name.IsSameText(QueryDes.Name));
                if (Table == null)
                {
                    Table = new MemTable(QueryDes.Name);
                    Tables.Add(Table);
                    Table.SqlStatements.SelectSql = QueryDes.Sql;
                    if (QueryDes.FieldTitleKeys != null)
                        Table.SqlStatements.FieldTitleKeys = QueryDes.FieldTitleKeys;
                }
            }
        }

        /// <summary>
        /// Initializes detail data Tables
        /// </summary>
        protected virtual void InitializeDetails()
        {
            // find the detail datasets of any dataset
            CollectDetails(tblItem, Descriptor.MainTable);
        }
        /// <summary>
        /// Constructs the table tree of this broker.
        /// </summary>
        protected virtual void CollectDetails(MemTable MasterTable, SqlBrokerTableDef MasterDes)
        {
            MemTable DetailTable;

            foreach (var DetailDes in Descriptor.Tables)
            {
                DetailTable = Tables.Find(item => item.Name.IsSameText(DetailDes.Name));
                if ((DetailTable != null) && (DetailDes != MasterDes) && Sys.IsSameText(MasterDes.Name, DetailDes.MasterTableName))
                {
                    MasterTable.Details.Add(DetailTable);

                    // do a recursion to add detail Tables to this table
                    CollectDetails(DetailTable, DetailDes);
                }
            }
        }

        /// <summary>
        /// Initializes "stock Tables". A stock table is a helper data table.
        /// It is not a table that participates in the table tree when inserting, editing and deleting.
        /// </summary>
        protected virtual void InitializeStockTables()
        {
            //InitializeStockTables(ftblItem, Descriptor.MainTable.StockTables);

            MemTable StockTable;
            foreach (var StockTableDes in Descriptor.MainTable.StockTables)
            {
                StockTable = Tables.Find(item => item.Name.IsSameText(StockTableDes.Name));
                if (StockTable == null)
                {
                    StockTable = new MemTable(StockTableDes.Name);
                    Tables.Add(StockTable);
                    StockTable.SqlStatements.SelectRowSql = StockTableDes.Sql;
                    tblItem.StockTables.Add(StockTable);
                }
            }
        }

        /// <summary>
        /// Returns a bit field (set) of sql generation flags. Used when initializing Tables and
        /// their sql statements.
        /// </summary>
        protected virtual BuildSqlFlags GetBuildSqlFlags()
        {
            BuildSqlFlags Result = BuildSqlFlags.None;

            if (Descriptor.GuidOids)
                Result |= BuildSqlFlags.GuidOids;
            else if (Store.Provider.OidMode == OidMode.Before)
                Result |= BuildSqlFlags.OidModeIsBefore;

            if (IsListBroker)
                Result |= BuildSqlFlags.BrowseBlobFields;

            return Result;
        }
        #endregion


        /* item */
        /// <summary>
        /// Called by the <see cref="Broker.Insert"/> to actually starts an insert operation.
        /// </summary>
        protected override void DoInsert()
        {
            TableSet.ProcessInsert();
        }
        /// <summary>
        /// Called after the <see cref="DoInsert"/>
        /// </summary>
        protected override void DoInsertAfter()
        {
            base.DoInsertAfter();
            SetDefaultValues();
        }
        /// <summary>
        /// Called by the <see cref="Broker.Edit"/> to actually starts an edit operation.
        /// </summary>
        protected override void DoEdit(object RowId)
        {
            TableSet.Select(RowId);
        }

        /// <summary>
        /// Called by the <see cref="Broker.Delete"/> to actually delete a row to the underlying table tree (database).
        /// </summary>
        protected override void DoDelete(object RowId)
        {
            TableSet.Delete(RowId);
        }
        /// <summary>
        /// Called before the <see cref="DoCommit"/>.
        /// <para>Calls the virtual SetDefaultValues(), so inheritors may assign default values. </para>
        /// </summary>
        protected override void DoCommitBefore(bool Reselect)
        {
            base.DoCommitBefore(Reselect);
            SetDefaultValues();
        }
        /// <summary>
        /// Called by the Commit() to actually commit changes made by the Insert or Edit methods,
        /// to the underlying table tree (database).
        /// <para>Returns the row id of the tblItem commited row.</para>
        /// </summary>
        protected override object DoCommit(bool Reselect)
        {
            return TableSet.Commit(Reselect);
        }
        /// <summary>
        /// Called by the Cancel() to actually rollback changes made by the Insert() or Edit() methods
        /// to the in-memory data table tree.
        /// </summary>
        protected override void DoCancel()
        {
            TableSet.ProcessCancel();
        }

        /// <summary>
        /// Called after the <see cref="DoEdit"/>
        /// </summary>
        protected override void DoEditAfter(object RowId)
        {
            // swallow inherited base call
            // SetDefaultValues(); // NO. Calling this sets the Modified flag in the rows of any table
        }
        /// <summary>
        /// Called after the <see cref="DoDelete"/>
        /// </summary>
        protected override void DoDeleteAfter(object RowId)
        {
            // swallow inherited base call
        }
        /// <summary>
        /// Called after the <see cref="DoCommit"/>
        /// </summary>
        protected override void DoCommitAfter(bool Reselect)
        {
            // swallow inherited base call
        }
        /// <summary>
        /// Called after the <see cref="DoCancel"/>
        /// </summary>
        protected override void DoCancelAfter()
        {
            // swallow inherited base call
        }


        /// <summary>
        /// Sets default values for all Tables.
        /// <para>It is called by the DoInsertAfter() and DoCommitBefore() </para>
        /// </summary>
        protected virtual void SetDefaultValues()
        {
            if ((this.State == DataMode.Insert) || (IsListBroker && Commiting))
            {
                MemTable Table;
                foreach (var TableDes in Descriptor.Tables)
                {
                    Table = Tables.Find(item => item.Name.IsSameText(TableDes.Name));
                    if (Table != null)
                    {
                        SetDefaultValues(Table);
                        SqlValueProviders.Process(Table, Store);
                    }
                }
            }
        }
        /// <summary>
        /// Sets default values to the Table. It is called when an commit operation starts.
        /// </summary>
        protected virtual void SetDefaultValues(DataTable Table)
        {
            if ((this.State == DataMode.Insert) || (IsListBroker && Commiting))
            {
                foreach (DataRow Row in Table.Rows)
                    SetDefaultValues(Row);
            }
        }
        /// <summary>
        /// Sets default values to the Row. It is called when an commit operation starts.
        /// </summary>
        protected virtual void SetDefaultValues(DataRow Row)
        {

            if (Row.RowState == DataRowState.Deleted)
                return;

            if (Descriptor.MainTableName.IsSameText(Row.Table.TableName))
            {
                if (IsListBroker)
                {
                    if (!Commiting)
                        return;
                }
                else if (this.State != DataMode.Insert)
                {
                    return;
                }
            }

            bool Flag = ((this.State == DataMode.Insert) || (IsListBroker && Commiting)) && (Row.RowState != DataRowState.Deleted);



            var TableDes = Descriptor.Tables.Find(item => item.Name.IsSameText(Row.Table.TableName));
            //FieldDescriptorBase FieldBaseDes;
            Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> Pair;
            SqlBrokerFieldDef FieldDes;

            foreach (DataColumn Column in Row.Table.Columns)
            {
                if (!Column.ReadOnly)
                {
                    if (Sys.IsNull(Row[Column]) || (Simple.SimpleTypeOf(Column.DataType).IsString() && (Row[Column].ToString() == string.Empty)))
                    {
                        if (TableDes != null)
                        {
                            Pair = TableDes.FindAnyField(Column.ColumnName);
                            if (Pair != null)
                            {
                                FieldDes = Pair.Item2;

                                if (FieldDes != null)
                                {
                                    /* skip the column if the column descriptor is marked as read-only */
                                    if (FieldDes.IsReadOnly)
                                        continue;

                                    /* FieldDescriptor.DefaultValue */
                                    SqlValueProviders.Process(Row, Column, FieldDes.DefaultValue, Store);

                                    /* if still is null */
                                    if (Sys.IsNull(Row[Column]) && FieldDes.IsBoolean)
                                    {
                                        Row[Column] = 0;
                                    }
                                }
                            }

                        }

                        /* if still is null */
                        if (Sys.IsNull(Row[Column]) && (Column.DataType == typeof(System.Boolean)))
                            Row[Column] = false;
                        else if (Sys.IsNull(Row[Column]) || (Simple.SimpleTypeOf(Column.DataType).IsString() && (Row[Column].ToString() == string.Empty)))
                        {
                            if (Sys.IsSameText(SysConfig.CompanyFieldName, Column.ColumnName)) // ColumnName is CompanyId
                                Row[Column] = SysConfig.CompanyId;
                        }
                    }

                }

            }

        }

        /* item checks */
        /// <summary>
        /// Called by the Commit and throws an exception if, for some reason,
        /// commiting item is considered invalid.
        /// </summary>
        public override void CheckCanCommit(bool Reselect)
        {
            CheckRequiredFields();
        }

        /* tableset event handlers */
        /// <summary>
        /// Gets a notification from the TableSet when deleting
        /// </summary>
        protected virtual void TableSet_TransactionStageDelete(object sender, TransactionStageEventArgs e)
        {
        }
        /// <summary>
        /// Gets a notification from the TableSet when commiting
        /// </summary>
        protected virtual void TableSet_TransactionStageCommit(object sender, TransactionStageEventArgs e)
        {
            // ** in web applications we do NOT have TableSet state
            if (/*(TableSet.IsInsert) && */(e.Stage == TransactionStage.Start) && (e.ExecTime == ExecTime.After))
            {
                AssignCodeValue(e.Store, e.Transaction);
            }
        }

        /* miscs */
        /// <summary>
        /// Called from inside a commit transaction in order to assign the Code column
        /// </summary>
        protected virtual void AssignCodeValue(SqlStore Store, DbTransaction Transaction)
        {
            // ** in web applications we do NOT have TableSet state
            if /*(TableSet.IsInsert) && */(CodeProducer != null)
            {
                string CodeFieldName = CodeProducer.CodeFieldName;

                if (tblItem != null && tblItem.Rows.Count > 0 && tblItem.Columns.Contains(CodeFieldName))
                {
                    foreach (DataRow Row in tblItem.Rows)
                    {
                        if (Sys.IsNull(Row[CodeFieldName]) || string.IsNullOrEmpty(Sys.AsString(Row[CodeFieldName], string.Empty).Trim()))
                        {
                            Row[CodeFieldName] = CodeProducer.Execute(Row, this.Store, Transaction);
                        }
                    }
                }
            } 
        }


        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlBroker()
        {
        }

 
        /* browser select */
        /// <summary>
        /// Executes a SELECT statements and puts the returned data rows to the Table.
        /// <para>SqlText could be the statement text or a SelectSql Name found in Descriptor.SelectList.</para>
        /// <para>RowLimit greater than zero, is an instruction to apply a row limit to the SELECT statement</para>
        /// <para>NOTE: This method is used when selecting for the browse part of a data form. 
        ///  Normally the Table passed to this method is not part of the table tree of the TableSet.</para>
        /// </summary>
        public virtual int SelectBrowser(MemTable Table, string SqlText, int RowLimit)
        {
            SelectSql SS = null;
            string SelectSqlName = SqlText.Trim();

            if (!SelectSqlName.StartsWithText("SELECT")) // it's a SelectSql name
            {
                SS = this.Descriptor.SelectList.Find(item => item.Name.IsSameText(SelectSqlName));
                SqlText = SS.Text;
            }

            if (RowLimit > 0)
            {
                SS = new SelectSql(SqlText);
                Store.Provider.ApplyRowLimit(SS, RowLimit);
                SqlText = SS.Text;
            }

            return TableSet.SelectBrowser(Table, SqlText);
        }
        /// <summary>
        /// Executes the SELECT SqlText and puts the returned data rows to the Table.
        /// <para>It is used when selecting for the browse part of a data form.</para>
        /// <para>Normally the Table passed to this method is not part of the table tree of the TableSet.</para>
        /// </summary>
        public virtual int SelectBrowser(MemTable Table, string SqlText)
        {
            return TableSet.SelectBrowser(Table, SqlText);
        }
 
        /* batch commits */
        /// <summary>
        /// A Commit() version for batch operations.
        /// <para>Starts a transaction and keeps on calling a specified Proc while it returns true.</para>
        /// <para>It commits the transaction each time the TransLimit is reached.</para>
        /// </summary>
        public virtual void CommitBatch(BatchCommitArgs Args)
        {
            bool TriggerScriptEvents = Bf.In(BatchCommitFlags.TriggerScriptEvents, Args.Flags);
            bool CallCustomActions = Bf.In(BatchCommitFlags.CallCustomActions, Args.Flags);

            //ArgList ParamList = null;
            bool Reselect = false;
            bool ShouldPost;


            tblItem.EventsDisabled = true;
            try
            {
                int Counter = 0;
                int PostCounter = 0;


                while (true)
                {



                    Args.Counter = Counter;
                    Args.PostCounter = PostCounter;

                    ShouldPost = Args.BeforeFunc();

                    if (ShouldPost)
                    {
                        // ------------------------------------------------------------------------------
                        // before
                        if (TableSet.Transaction == null)
                        {
                            TableSet.Transaction = TableSet.Store.BeginTransaction();
                        }


                        DoCommitBefore(Reselect);

                        /* TODO: TriggerScriptEvents || CallCustomActions
                        if (TriggerScriptEvents || CallCustomActions)
                        {
                            ParamList = new ArgList(new string[] { "Reselect" }, new object[] { Reselect });
                            if (TriggerScriptEvents)
                                TriggerScriptEvent("CommitBefore", ParamList);
                            if (CallCustomActions)
                                ExecuteCustomActions("CommitBefore", ParamList);
                        }
                        */

                        CheckCanCommit(Reselect);
                        AssignCodeValue(TableSet.Store, TableSet.Transaction);

                        // ------------------------------------------------------------------------------
                        // post
                        TableSet.PostChanges();
                        tblItem.DataSet.AcceptChanges();   // clear logs  
                        LastCommitedId = tblItem.Rows.Count > 0 ? tblItem.Rows[0][tblItem.PrimaryKeyField] : null;

                        // ------------------------------------------------------------------------------
                        // after
                        DoCommitAfter(Reselect);

                        /* TODO: TriggerScriptEvents || CallCustomActions
                        if (TriggerScriptEvents || CallCustomActions)
                        {
                            ParamList["LastCommitedId"] = LastCommitedId;
                            if (TriggerScriptEvents)
                                TriggerScriptEvent("CommitAfter", ParamList);
                            if (CallCustomActions)
                                ExecuteCustomActions("CommitAfter", ParamList);
                        }
                        */
                    }



                    if (!Args.AfterFunc(LastCommitedId))
                        break;


                    if (ShouldPost)
                    {
                        PostCounter++;

                        if (PostCounter % Args.TransLimit == 0)
                        {
                            TableSet.Transaction.Commit();
                            TableSet.Transaction.Dispose();
                            TableSet.Transaction = null;
                        }
                    }

                    Counter++;
                }

                // after the loop
                if (TableSet.Transaction != null)
                {
                    TableSet.Transaction.Commit();
                    TableSet.Transaction.Dispose();
                    TableSet.Transaction = null;
                }

            }
            finally
            {
                tblItem.EventsDisabled = false;
            }








        }

        /* query Tables */
        /// <summary>
        /// Re-executes the SELECT for all query Tables.
        /// </summary>
        public virtual void ReselectQueries()
        {
            foreach (var QueryDes in Descriptor.Queries)
            { 
                MemTable Table = Tables.Find(item => item.Name.IsSameText(QueryDes.Name));
                if (Table != null)
                    Store.SelectTo(Table, QueryDes.Sql);
            }
        }
        /// <summary>
        /// Re-executes the SELECT for a query Table.
        /// </summary>
        public virtual void ReselectQuery(string QueryName)
        {
            MemTable Table = Tables.Find(item => item.Name.IsSameText(QueryName));
            SqlBrokerQueryDef QueryDes = Descriptor.Queries.Find(item => item.Name.IsSameText(QueryName));
            if (QueryDes != null && Table != null)
            {
                Store.SelectTo(Table, QueryDes.Sql);
            }
        }

        /* miscs */
        /// <summary>
        /// Throws an exception if any of the required (not null) fields, is null
        /// </summary>
        public virtual void CheckRequiredFields()
        {
            MemTable Table;
            foreach (SqlBrokerTableDef TableDes in Descriptor.Tables)
            {
                Table = Tables.Find(item => item.Name.IsSameText(TableDes.Name));
                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                        CheckRequiredFields(Row, TableDes);
                }
            }
        }
        /// <summary>
        /// Throws an exception if any of the required (not null) fields, is null
        /// </summary>
        public virtual void CheckRequiredFields(DataRow Row, SqlBrokerTableDef TableDes)
        {
            if ((Row == null) || (TableDes == null) || Bf.Member(DataRowState.Deleted, Row.RowState))
                return;


            SqlBrokerFieldDef FieldDes;

            if (TableDes != null)
            {
                bool IsCodeColumn;

                StringBuilder SB = new StringBuilder();

                foreach (DataColumn Column in Row.Table.Columns)
                {
                    FieldDes = TableDes.Fields.Find(item => item.Name.IsSameText(Column.ColumnName));

                    if ((!Column.AllowDBNull || ((FieldDes != null) && FieldDes.IsRequired)) && Db.IsNullOrEmpty(Row, Column))
                    {
                        /* skip the column if it is the CodeProducer target column. That column 
                           is assigned later from insided the TableSet commit transaction */
                        IsCodeColumn = (State == DataMode.Insert) && Sys.IsSameText(CodeProducer.CodeFieldName, Column.ColumnName) && (Column.Table == tblItem) && (CodeProducer != null);
                        if (!IsCodeColumn)
                            SB.AppendLine(string.Format("  {0} -> {1} ({2}.{3})", TableDes.Title, Column.Caption, TableDes.Name, Column.ColumnName));
                    }
                }

                if (SB.Length > 0)
                {
                    SB.Insert(0, Res.GS("E_InvalidFieldValues", "Invalid field values") + Environment.NewLine);
                    Sys.Throw(SB.ToString());
                }
            }
        }

        /// <summary>
        /// Locates and returns a row or null. 
        /// <para>FieldName is the column name to search and Value the value to locate</para>
        /// </summary>
        public override DataRow Locate(string FieldName, object Value)
        {
            if (!Sys.IsNull(Value)
                && !string.IsNullOrWhiteSpace(FieldName)
                && !string.IsNullOrWhiteSpace(MainTableName))
            {
                string SqlText = string.Format("select * from {0} where {1} = {2}{1}",
                    MainTableName, FieldName, SqlProvider.GlobalPrefix);

                DataTable Table = Store.Select(SqlText, Value);
                if (Table.Rows.Count > 0)
                {
                    return Table.Rows[0];
                }
            }

            return null;
        }
        /// <summary>
        /// Returns the Table Descriptor of TableName, if any, else null.
        /// <para>If TableName is null or empty, it returns the MainTable (tblItem) descriptor.</para>
        /// </summary>
        public virtual SqlBrokerTableDef TableDescriptorOf(string TableName)
        {
            if (string.IsNullOrWhiteSpace(TableName))
                return Descriptor.MainTable;
            return Descriptor.Tables.Find(item => item.Name.IsSameText(TableName));
        }
        /// <summary>
        /// Returns the Field Descriptor of TableName.FieldName, if any, else null.
        /// <para>If TableName is null or empty, it returns the MainTable (tblItem) descriptor.</para>
        /// </summary>
        public virtual SqlBrokerFieldDef FieldDescriptorOf(string TableName, string FieldName)
        {
            SqlBrokerTableDef TableDes = TableDescriptorOf(TableName);
            if (TableDes != null)
                return TableDes.Fields.Find(item => item.Name.IsSameText(FieldName));
            return null;
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
                JsonDataTable JTable = JB.Tables.Find(item => Table.TableName.IsSameText(item.Name));
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
        /// SelectBrowser json counterpart
        /// <para>SqlText could be the statement text or a SelectSql Name found in Descriptor.SelectList.</para>
        /// <para>RowLimit greater than zero, is an instruction to apply a row limit to the SELECT statement</para>
        /// </summary>
        public virtual JsonDataTable JsonSelectBrowser(string SqlText, int RowLimit)
        {
            MemTable Table = new MemTable();
            SelectBrowser(Table, SqlText, RowLimit);
            JsonDataTable JTable = new JsonDataTable(Table, null);
            return JTable;
        }

        /* properties */
        /// <summary>
        /// Gets or sets the descriptor of this broker
        /// </summary>
        public SqlBrokerDef Descriptor { get; set; } = new SqlBrokerDef() { Name = GENERIC_SQL_BROKER };

        /// <summary>
        /// Returns the connection info
        /// </summary>
        public SqlConnectionInfo ConnectionInfo { get; protected set; }
        /// <summary>
        /// Returns the Executor
        /// </summary>
        public SqlStore Store { get; protected set; }

        /// <summary>
        /// Gets the TableSet
        /// </summary>
        public TableSet TableSet { get; protected set; }

        /// <summary>
        /// Gets the code producer of the broker.
        /// </summary>
        public CodeProvider CodeProducer { get; set; }

        /// <summary>
        /// Returns the table name of the main table
        /// </summary>
        public override string MainTableName { get { return Descriptor.MainTableName; } }
        /// <summary>
        /// Returns the table name of the Lines table
        /// </summary>
        public override string LinesTableName { get { return Descriptor.LinesTableName; } }
        /// <summary>
        /// Returns the table name of the SubLines table
        /// </summary>
        public override string SubLinesTableName { get { return Descriptor.SubLinesTableName; } }
  
        /// <summary>
        /// The name of the Entity this broker represents
        /// </summary>
        public override string EntityName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(fEntityName))
                    return fEntityName;

                return Descriptor.EntityName;
            }
            set { fEntityName = value; }
        }

        /// <summary>
        /// True means that Descriptor is assigned by the user before the Initialize()
        /// </summary>
        public bool IsDeclarative => Descriptor.Name != GENERIC_SQL_BROKER;
    }
}
