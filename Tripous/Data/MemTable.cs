namespace Tripous.Data
{




    /// <summary>
    /// An extended DataTable class
    /// </summary>
    public class MemTable : DataTable, INamedItem
    {

        /* fields */
        static private int tableNameCounter = 0;
        static private int foreignKeyConstraintCounter = 0;

        /// <summary>
        /// Field
        /// </summary>
        protected string fTitle;
        /// <summary>
        /// Field
        /// </summary>
        protected string fPrimaryKeyField;
        /// <summary>
        /// Field
        /// </summary>
        protected string fMasterKeyField;
        /// <summary>
        /// Field
        /// </summary>
        protected string fDetailKeyField;
        /// <summary>
        /// Field
        /// </summary>
        protected MemTable fMasterTable = null;
        /// <summary>
        /// Field
        /// </summary>
        protected DetailsList fDetails;
        /// <summary>
        /// Field
        /// </summary>
        protected NamedItems<MemTable> fStockTables = new NamedItems<MemTable>();

        /*
                /// <summary>
                /// Field
                /// </summary>
                // protected SqlStatements fSqlStatements = new SqlStatements(); 
         */

        /// <summary>
        /// Field
        /// </summary>
        protected string scriptSource;
        /// <summary>
        /// Field
        /// </summary>
        protected int eventsDisableCount = 0;

        /// <summary>
        /// Field
        /// </summary>
        protected bool hasExpressionsChecked;
        /// <summary>
        /// Field
        /// </summary>
        protected bool hasExpressions;


        /// <summary>
        /// Represents a list of <see cref="MemTable"/> instances, detail to a master MemTable.
        /// <para>Both the master and the detail MemTable instances must belong to the
        /// same DataSet, otherwise an exception is thrown.</para>
        /// </summary>
        public class DetailsList : OwnedCollection<MemTable>
        {
            /* fields */
            private MemTable table = null; // the owner table, which becomes the master of any other table added
            private DataRelation relation = null;
            private int activeCount = 0;

            /// <summary>
            /// Throws an exception if the master and the detail MemTable instances
            /// in the list do not belong to the same DataSet.
            /// </summary>
            private void CheckDatasets()
            {
                foreach (MemTable DetailTable in this)
                    CheckDatasets(DetailTable);
            }
            private void CheckDatasets(MemTable DetailTable)
            {
                if (DetailTable == null)
                    throw new ArgumentNullException("DetailTable");

                if (table.DataSet == null)
                    Sys.Throw("MasterTable Table has no DataSet");

                if (DetailTable.DataSet == null)
                    Sys.Throw("A DetailTable Table has no DataSet");

                if (DetailTable.DataSet != table.DataSet)
                    Sys.Throw("MasterTable.DataSet != DetailTable.DataSet");

            }

            /// <summary>
            /// Constructor
            /// </summary>
            internal DetailsList(MemTable Table)
            {
                table = Table;
            }

            /// <summary>
            /// It is called by the Insert method to check the validitiy of the Item before inserting.
            /// </summary>
            protected override void CheckInsert(int Index, MemTable DetailTable)
            {
                base.CheckInsert(Index, DetailTable);
                CheckDatasets(DetailTable);
            }
            /// <summary>
            /// Adds DetailTable to the list
            /// </summary>
            protected override void OnInsertAfter(int Index, MemTable DetailTable)
            {
                base.OnInsertAfter(Index, DetailTable);
                DetailTable.fMasterTable = table;
            }
            /// <summary>
            /// Removes DetailTable from list.
            /// </summary>
            protected override void OnRemoveAfter(int Index, MemTable DetailTable)
            {
                base.OnRemoveAfter(Index, DetailTable);

                while (DetailTable.Details.Active)
                    DetailTable.Details.Active = false;

                DetailTable.fMasterTable = null;
            }


            /// <summary>
            /// Activates and de-activates the master-detail relation-ship between
            /// the master MemTable and the details. 
            /// <para>WARNING: Tables MUST HAVE already columns created.</para>
            /// </summary>
            public bool Active
            {
                get { return activeCount >= 1; }
                set
                {
                    if (value)
                    {
                        activeCount++;
                        if (activeCount == 1)
                        {
                            CheckDatasets();
                            foreach (MemTable DetailTable in this)
                            {
                                DetailTable.Locale = table.Locale;
                                DetailTable.CaseSensitive = table.CaseSensitive;

                                DataColumn ParentColumn = table.Columns[DetailTable.MasterKeyField];
                                DataColumn ChildColumn = DetailTable.Columns[DetailTable.DetailKeyField];

                                /* add a foreign key constraint, so deleting a row in the master results in deleting all child rows */
                                foreignKeyConstraintCounter++;
                                string ObjectName = string.Format("FK_{0}_{1}", DetailTable.TableName, foreignKeyConstraintCounter);
                                /*
                                ForeignKeyConstraint FKC = new ForeignKeyConstraint(ObjectName, ParentColumn, ChildColumn);
                                FKC.DeleteRule = Rule.Cascade;
                                FKC.UpdateRule = Rule.Cascade;
                                FKC.AcceptRejectRule = AcceptRejectRule.Cascade;
                                DetailTable.Constraints.Add(FKC);
                                DetailTable.ExtendedProperties["ForeignKeyConstraint"] = FKC;
                                //*/

                                /* create a DataRelation between master and detail tables */
                                /* WARNING: Any relation creates a UniqueContraint in the master table */
                                ObjectName = MemTable.ConstructRelationName(this.table, DetailTable);
                                DetailTable.Details.relation = table.DataSet.Relations.Add(ObjectName, ParentColumn, ChildColumn);

                                DetailTable.Details.relation.ChildKeyConstraint.DeleteRule = Rule.Cascade;  /* it's set to Cascade by default */
                                DetailTable.Details.relation.ChildKeyConstraint.UpdateRule = Rule.Cascade;  /* it's set to Cascade by default */
                                DetailTable.Details.relation.ChildKeyConstraint.AcceptRejectRule = AcceptRejectRule.Cascade;

                                DetailTable.Details.Active = true;
                            }
                        }
                    }
                    else
                    {
                        activeCount--;
                        if (activeCount == 0)
                        {
                            /* remove any foreign key constraint 
                            if (this.table.ExtendedProperties.ContainsKey("ForeignKeyConstraint"))
                            {
                                ForeignKeyConstraint FKC = this.table.ExtendedProperties["ForeignKeyConstraint"] as ForeignKeyConstraint;
                                if (FKC != null)
                                {
                                    this.table.ExtendedProperties["ForeignKeyConstraint"] = null;
                                    this.table.Constraints.Remove(FKC);
                                }
                            }
                            */

                            foreach (MemTable DetailTable in this)
                            {
                                DetailTable.Details.Active = false;
                            }

                            table.ChildRelations.Clear();

                            relation = null;

                        }

                        if (activeCount < 0)
                            activeCount = 0;
                    }
                }
            }
            /// <summary>
            /// Returns the DataRelation instance, this object represents. 
            /// <para>WARNING: This DataRelation is not null only when <see cref="Active"/> is true.</para>
            /// </summary>
            public DataRelation Relation
            {
                get { return relation; }
            }
        }


        /* private */
        /// <summary>
        /// Calls the <see cref="MemTable.InitializeAutoInc"/>
        /// </summary>
        private void ColumnsCollection_ColumnAddedOrRemoved(object sender, CollectionChangeEventArgs e)
        {
            InitializeAutoInc(this);
        }

        /* overrides - event activation */
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnColumnChanged(DataColumnChangeEventArgs e)
        {
            if (!EventsDisabled)
            {
                base.OnColumnChanged(e);

                if (!hasExpressionsChecked)
                {
                    foreach (DataColumn Field in Columns)
                    {
                        if (!string.IsNullOrEmpty(Field.Expression))
                        {
                            hasExpressions = true;
                            break;
                        }
                    }

                    hasExpressionsChecked = true;
                }

                if (hasExpressions)
                {
                    e.Row.EndEdit();
                }
            }
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnColumnChanging(DataColumnChangeEventArgs e)
        {
            if (!EventsDisabled)
                base.OnColumnChanging(e);
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            if (AutoGenerateGuidKeys && (e.Action == DataRowAction.Add) && (e.Row.RowState != DataRowState.Deleted))
            {
                int Index = Columns.IndexOf(PrimaryKeyField);

                if ((Index >= 0) && (Columns[Index].DataType == typeof(System.String)))
                {
                    if (Sys.IsNull(e.Row[Index]) || (e.Row[Index].ToString() == string.Empty))
                        if ((Columns[Index].MaxLength == -1) || (Columns[Index].MaxLength >= 40))
                            e.Row[Index] = Sys.GenId();
                }
            }


            if (!EventsDisabled)
                base.OnRowChanged(e);
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            if (!EventsDisabled)
                base.OnRowChanging(e);
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            if (!EventsDisabled)
                base.OnRowDeleting(e);

            //DeleteDetailRows(e.Row);
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            if (!EventsDisabled)
                base.OnRowDeleted(e);
        }

        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnTableCleared(DataTableClearEventArgs e)
        {
            if (!EventsDisabled)
                base.OnTableCleared(e);
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnTableClearing(DataTableClearEventArgs e)
        {
            if (!EventsDisabled)
                base.OnTableClearing(e);
        }
        /// <summary>
        /// Calls the base method, only if <see cref="EventsDisabled"/> is false, eventually
        /// deactivating the method.
        /// </summary>
        protected override void OnTableNewRow(DataTableNewRowEventArgs e)
        {
            if (!EventsDisabled)
                base.OnTableNewRow(e);
        }

        /// <summary>
        /// Deletes all detail rows of Row in the detail tree.
        /// </summary>
        protected virtual void DeleteDetailRows(DataRow Row)
        {
            if (Columns.Contains(PrimaryKeyField))
            {
                if (Row.HasVersion(DataRowVersion.Original))
                {
                    object RowId = Row[PrimaryKeyField, DataRowVersion.Original];
                    object DetailRowId = null;

                    if (!Sys.IsNull(RowId))
                    {
                        foreach (MemTable DetailTable in Details)
                        {
                            foreach (DataRow DetailRow in DetailTable.Rows)
                            {
                                DetailRowId = DetailRow[DetailTable.MasterKeyField];
                                if (!Sys.IsNull(DetailRowId) && RowId.Equals(DetailRowId))
                                {
                                    DetailRow.Delete();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MemTable()
            : this(NextTableName())
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public MemTable(string tableName)
            : this(tableName, string.Empty)
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public MemTable(string tableName, string tableNamespace)
            : base(tableName, tableNamespace)
        {
            AutoGenerateGuidKeys = true;
            fDetails = new DetailsList(this);
            Columns.CollectionChanged += new CollectionChangeEventHandler(ColumnsCollection_ColumnAddedOrRemoved);

            /* The virtual OnTableNewRow() is not called if the invocation list of 
               the TableNewRow event is empty. Microsoft says this is by design
                see: http://connect.microsoft.com/VisualStudio/feedback/details/184473/ontablenewrow-is-called-only-when-the-event-delegate-list-for-tablenewrow-event-is-not-empty
               Anyway, just adding an empty event handler, forces the OnTableNewRow() to be called.
             */
            this.TableNewRow += new DataTableNewRowEventHandler(Table_TableNewRow);
        }


        /// <summary>
        /// The virtual OnTableNewRow() is not called if the invocation list of 
        /// the TableNewRow event is empty. Microsoft says this is by design
        /// see: http://connect.microsoft.com/VisualStudio/feedback/details/184473/ontablenewrow-is-called-only-when-the-event-delegate-list-for-tablenewrow-event-is-not-empty
        /// Anyway, just adding an empty event handler, forces the OnTableNewRow() to be called.
        /// </summary>
        void Table_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
        }
 

        /* static */
        /// <summary>
        /// Creates a MemTable with TableName and adds it to the DS tables
        /// </summary>
        static public MemTable Create(DataSet DS, string TableName)
        {
            if (string.IsNullOrWhiteSpace(TableName))
                TableName = NextTableName();

            MemTable Result = new MemTable(TableName);

            if (DS != null)
                DS.Tables.Add(Result);

            return Result;
        }
        /// <summary>
        /// Creates a MemTable and adds it to the DS tables
        /// </summary>
        static public MemTable Create(DataSet DS)
        {
            return Create(DS, "");
        }
        /// <summary>
        /// Creates a MemTable with TableName 
        /// </summary>
        static public MemTable Create(string TableName)
        {
            return Create(null, TableName);
        }
        /// <summary>
        /// Creates a MemTable 
        /// </summary>
        static public MemTable Create()
        {
            return Create(null, "");
        }
        /// <summary>
        /// Constructs and returns a valid and unique <see cref="DataTable.TableName"/>
        /// </summary>
        static public string NextTableName()
        {
            tableNameCounter++;
            return "Table_" + tableNameCounter.ToString();
        }
        /// <summary>
        /// Ensures that Table has a TableName
        /// </summary>
        static public void EnsureTableName(DataTable Table)
        {
            if (string.IsNullOrWhiteSpace(Table.TableName))
                Table.TableName = NextTableName();
        }
        /// <summary>
        /// Constructs and returns a DataRelation name, based on MasterTable and DetailTable
        /// </summary>
        static public string ConstructRelationName(DataTable Master, DataTable Detail)
        {
            EnsureTableName(Master);
            EnsureTableName(Detail);

            return Master.TableName + "_TO_" + Detail.TableName;
        }
        /// <summary>
        /// If Table contains a DataColumn named as Table.PrimaryKeyField and
        /// that DataColumn is of type System.Int32, then initializes the properties
        /// of the DataColumn, so the column to be an autoincrement one (negative).
        /// </summary>
        static public void InitializeAutoInc(MemTable Table)
        {
            if (Table.Columns.Contains(Table.PrimaryKeyField))
                if (Table.Columns[Table.PrimaryKeyField].DataType == typeof(System.Int32))
                {
                    DataColumn Column = Table.Columns[Table.PrimaryKeyField];
                    Column.AutoIncrement = true;
                    Column.AutoIncrementSeed = -1;
                    Column.AutoIncrementStep = -1;
                }
        }
        /// <summary>
        /// Adds Table to the DS.Tables
        /// </summary>
        static public void AddTableTo(DataSet DS, DataTable Table)
        {
            if (Table.DataSet != null)
                Table.DataSet.Tables.Remove(Table);

            DS.Tables.Add(Table);
        }

        /// <summary>
        /// Returns an array of all detail and subdetail etc tables
        /// of the TopTable (including the top table), in a flat way,
        /// from top to bottom of the tree
        /// </summary>
        static public MemTable[] GetDetailTreeAsFlatArray(MemTable TopTable)
        {
            List<MemTable> List = new List<MemTable>();

            Action<MemTable> Proc = null;

            Proc = delegate (MemTable Master)
            {
                List.Add(Master);
                foreach (MemTable Table in Master.Details)
                    Proc(Table);
            };

            Proc(TopTable);

            return List.ToArray();
        }


        /* public */
        /// <summary>
        /// Returns a value from a stock table
        /// </summary>
        public T GetStockTableValue<T>(string StockTableName, string FieldName, T Default)
        {
            MemTable StockTable = this.StockTables.Find(StockTableName);
            if ((StockTable != null) && (StockTable.Rows.Count == 1) && StockTable.ContainsColumn(FieldName))
            {
                object Value = StockTable.Rows[0][FieldName];
                return Sys.AsValue(Value, Default);
            }

            return Default;
        }

        /* properties */
        /// <summary>
        /// Returns true if this table has no rows at all.
        /// </summary>
        public bool IsEmpty
        {
            get { return Rows.Count <= 0; }
        }
        /// <summary>
        /// Gets or sets the Title of this instance. For display purposes
        /// </summary>
        public string Title
        {
            get { return string.IsNullOrWhiteSpace(fTitle) ? TableName : fTitle; }
            set { fTitle = value; }
        }
        /// <summary>
        /// Gets or sets the PrimaryKeyField name
        /// </summary>
        public string PrimaryKeyField
        {
            get { return string.IsNullOrWhiteSpace(fPrimaryKeyField) ? "Id" : fPrimaryKeyField; }
            set
            {
                fPrimaryKeyField = value;
                InitializeAutoInc(this);
            }
        }
        /// <summary>
        /// Gets or sets the MasterKeyField name, that is the DataColumn.ColumnName
        /// of the column in the master table, which is used in the DataRelation
        /// between the two MemTables.
        /// </summary>
        public string MasterKeyField
        {
            get { return string.IsNullOrWhiteSpace(fMasterKeyField) ? "Id" : fMasterKeyField; }
            set { fMasterKeyField = value; }
        }
        /// <summary>
        /// Gets or sets the DetailKeyField name, that is the DataColumn.ColumnName
        /// of the column in this table, which is used in the DataRelation between
        /// the two MemTables.
        /// </summary>
        public string DetailKeyField
        {
            get { return string.IsNullOrWhiteSpace(fDetailKeyField) ? string.Empty : fDetailKeyField; }
            set { fDetailKeyField = value; }
        }
        /// <summary>
        /// Returns true if this instance has an auto-increment primary key column
        /// </summary>
        public bool IsAutoInc
        {
            get
            {
                bool Result = false;

                if (Columns.Contains(PrimaryKeyField))
                    if (Columns[PrimaryKeyField].DataType == typeof(System.Int32))
                        Result = Columns[PrimaryKeyField].AutoIncrement == true;

                return Result;
            }
        }
        /// <summary>
        /// When true it automatically generates Guid keys for the primary key column, when a new row is added
        /// </summary>
        public bool AutoGenerateGuidKeys { get; set; }
        /// <summary>
        /// Returns true if the primary key field of type string
        /// </summary>
        public bool IsStringPrimaryKey { get { return Columns.Contains(PrimaryKeyField) && (Columns[PrimaryKeyField].DataType == typeof(System.String)); } }

        /// <summary>
        /// Gets the master table of this table, when this is a detail. It may return null.
        /// </summary>
        public MemTable MasterTable { get { return fMasterTable; } }
        /// <summary>
        /// Returns true if this is a detail table.
        /// </summary>
        public bool IsDetail { get { return fMasterTable != null; } }

        /// <summary>
        /// Gets a string to be used as the DataMember for data bound controls.
        /// </summary>
        public string DataMember { get { return fMasterTable == null ? this.TableName : ConstructRelationName(fMasterTable, this); } }


        /// <summary>
        /// Gets the Details of this instance.
        /// </summary>
        public DetailsList Details
        {
            get { return fDetails; }
        }
        /// <summary>
        /// Returns the "level" of this table. The level is the position
        /// of this table in a larger master-detail-subdetail tree which
        /// is automatically constructed.
        /// </summary>
        public int Level { get { return fMasterTable == null ? 0 : fMasterTable.Level + 1; } }
        /// <summary>
        /// Gets the StockTables of this instance.
        /// </summary>
        public NamedItems<MemTable> StockTables
        {
            get { return fStockTables; }
        }

        /*
        /// <summary>
        /// Gets or sets (assigns) the SqlStatements of this instance
        /// </summary>
        public SqlStatements SqlStatements
        {
            get { return fSqlStatements; }
            set { fSqlStatements.Assign(value); }
        }         
        // */

        /// <summary>
        /// The Sql statements for the table
        /// </summary>
        public TableSqls SqlStatements { get; set; } = new TableSqls();


        /// <summary>
        /// Enables or disables the OnTableXXX, OnRowXXXX and OnColumnXXXX methods,
        /// to call or not the base version of the method.
        /// </summary>
        public bool EventsDisabled
        {
            get { return eventsDisableCount > 0; }
            set
            {
                if (value)
                    eventsDisableCount++;
                else
                    eventsDisableCount--;

                if (eventsDisableCount < 0)
                    eventsDisableCount = 0;

                for (int i = 0; i < this.Details.Count; i++)
                    this.Details[i].EventsDisabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the Name of the table. It uses the TabelName property.
        /// </summary>
        public string Name
        {
            get { return string.IsNullOrWhiteSpace(TableName) ? string.Empty : TableName; }
            set
            {
                if (value != Name)
                {
                    if (value == null)
                        throw new ArgumentNullException("TableName");

                    if (Collection is IUniqueNamesList)
                        (Collection as IUniqueNamesList).CheckUniqueName(null, value);

                    TableName = value;
                }

            }
        }

        /// <summary>
        ///  Gets or sets the owner collection of this instance.
        /// </summary>
        public IList Collection { get; set; }



    }
}
