//---------------------------------------------------------------------------------------
// Urls
//---------------------------------------------------------------------------------------

tp.Urls.BrokerInitialize = '/Broker/Initialize';
tp.Urls.BrokerInsert = '/Broker/Insert';
tp.Urls.BrokerEdit = '/Broker/Edit';
tp.Urls.BrokerDelete = '/Broker/Delete';
tp.Urls.BrokerCommit = '/Broker/Commit';
tp.Urls.BrokerSelectList = '/Broker/SelectList';



 
//---------------------------------------------------------------------------------------
// Broker
//---------------------------------------------------------------------------------------

//#region tp.BrokerAction

/**
Represents a broker action
*/
tp.BrokerAction = class {

    /**
    Constructor.
    @param {tp.Broker} Broker - The broker that executes the action
    @param {string} ActionName - The action name. One of the string constants of this class
    */
    constructor(Broker, ActionName) {

        this.Name = ActionName; 
        this.Broker = Broker; 

        this.Args = new tp.AjaxArgs();
        this.Args.Action = this;
        this.Args.Data = {};

        this.AssignFlag = false; 

        switch (ActionName) {
            case tp.BrokerAction.Initialize:
                this.Args.Url = tp.Urls.BrokerInitialize;
                this.AssignFlag = true;
                break;

            case tp.BrokerAction.Insert:
                this.Args.Url = tp.Urls.BrokerInsert;
                this.AssignFlag = true;
                break;
            case tp.BrokerAction.Edit:
                this.Args.Url = tp.Urls.BrokerEdit;
                this.AssignFlag = true;
                break;
            case tp.BrokerAction.Delete:
                this.Args.Url = tp.Urls.BrokerDelete;
                this.AssignFlag = false;
                break;
            case tp.BrokerAction.Commit:
                this.Args.Url = tp.Urls.BrokerCommit;
                this.AssignFlag = true;
                break;

            case tp.BrokerAction.SelectList:
                this.Args.Url = tp.Urls.BrokerSelectList;
                this.AssignFlag = false;
                break;
        }
    }

    /**
    One of the string constants of this class
    @type {string}
    */
    Name;
    /** The broker that executes this action
    @type {tp.Broker}
    */
    Broker;
    /** The name of the broker that executes this action    
    @type {string}
    */
    get BrokerName() {
        return this.Broker ? this.Broker.Name : null;
    }
    /**
    Indicates whether the broker should call Assign() after executing the action.
    @type {boolean}
    */
    AssignFlag;

    /**
    The SELECT of a List part 
    @type {tp.SelectSql}
    */
    SelectSql;
    /** The {@link tp.AjaxArgs} that carry this action to the server
    @type {tp.AjaxArgs}
    */
    Args;

    /**
    True when this action succeeds.
    @type {boolean}
    */
    get Succeeded() { return this.Args.Result === true; }
 
};

tp.BrokerAction.Initialize = 'Initialize';
tp.BrokerAction.Insert = 'Insert';
tp.BrokerAction.Edit = 'Edit';
tp.BrokerAction.Delete = 'Delete';
tp.BrokerAction.Commit = 'Commit';
tp.BrokerAction.SelectList = 'SelectList';

//#endregion

//#region tp.BrokerEventArgs
/**
EventArgs for the broker class.
*/
tp.BrokerEventArgs = class extends tp.EventArgs {

    /**
    Constructor
    @param {tp.Broker} Broker A {@link tp.Broker}
    @param {tp.BrokerAction} Action A {@link tp.BrokerAction}
    */
    constructor(Broker, Action) {
        super(null, Broker, null);

        this.Broker = Broker;
        this.Action = Action;
    }

    /** Field
     @type {tp.Broker}
     */
    Broker;
    /** Field
     @type {tp.BrokerAction}
     */
    Action;
};
//#endregion

//#region tp.Broker

/**
The base broker class. A broker handles a tree of data-tables that make an entity (e.g. Customer and CustomerAddress tables)
and the data operations (INSERT, UPDATE, DELETE, SELECT) for that tree.    
*/
tp.Broker = class extends tp.Object {

    /**
    Constructor.
    @param {string} Name - The broker name.
    */
    constructor(Name = '') {
        super();

        this.fInitialized = false;
        this.InitializeFields();

        this.Name = Name || this.Name;
    }


    /**
    Returns true if this instance is initialized.
    @type {boolean}  
    */
    get Initialized() {
        return this.fInitialized;
    }
    /**
    Returns the top data-table. This is always a table with a single row.
    @type {tp.DataTable}
    */
    get tblItem() {
        return this.FindTable(this.MainTableName);
    }
    /**
    Returns the first level {@link tp.DataTable} detail table, if defined.
    This is a handy property and can be used when there is only a single detail table in the first level and the LinesTableName property has a valid table name value.
    @type {tp.DataTable}
    */
    get tblLines() {
        return this.FindTable(this.LinesTableName);
    }
    /**
    Returns the second level {@link tp.DataTable} detail table, if defined.
    This is a handy property and can be used when there is only a single detail table in the second level and the SubLinesTableName property has a valid table name value.
    @type {tp.DataTable}
    */
    get tblSubLines() {
        return this.FindTable(this.SubLinesTableName);
    }
    /**
    Returns the sole {@link tp.DataRow} row of the tblItem {@link tp.DataTable}, if any, else null
    @type {tp.DataRow}
    */
    get Row() {
        if (!tp.IsEmpty(this.tblItem) && (this.tblItem.RowCount > 0))
            return this.tblItem.Rows[0];
        return null;
    }
    /**
    Returns the value of the PrimaryKeyField of the sole row of the tblItem {@link tp.DataTable}, if any, else null.
    @type {any}
    */
    get Id() {
        var Row = this.Row;
        if (!tp.IsEmpty(Row) && !tp.IsBlank(this.PrimaryKeyField)) {
            return Row.Get(this.PrimaryKeyField, null);
        }
        return null;
    }


    /* protected */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.Broker';
    }
    /**
    Initializes fields and properties.      
    @protected
    @override
    */
    InitializeFields() {
        this.Name = tp.NextName('Broker');
        this.QueryNames = [];
        this.SelectSqlList = [];

        this.tblList = null;
        this.DataSet = new tp.DataSet();

        this.fInitialized = false;
        this.IsListBroker = false;
        this.IsMasterBroker = true;
        this.State = tp.DataMode.None;
        this.EntityId = 0;
        this.GuidOids = true;
    }
    /**
    Throws an exception if this instance is not initialized.
    @protected
    */
    CheckInitialized() {
        if (this.Initialized === false) {
            tp.Throw('Broker is NOT initialized: ' + this.Name);
        }
    }

    /**
    Creates and returns the List {@link tp.DataTable} data table
    @protected
    @param {object} Source The source object to copy property values from.
    @returns {tp.DataTable} Returns the List {@link tp.DataTable} data table
    */
    CreateListTable(Source) {
        this.tblList = new tp.DataTable("List");
        this.tblList.Assign(Source);
        return this.tblList;
    }
    /**
    Assigns the select list, that is the array of {@link tp.SelectSql} items.
    @protected
    @param {any[]} Source An array of {@link tp.SelectSql} items to copy items from.
    */
    AssignSelectSqlList(Source) {
        this.SelectSqlList = [];
        let SS; // tp.SelectSql;
        for (let i = 0, ln = Source.length; i < ln; i++) {
            SS = new tp.SelectSql();
            this.SelectSqlList.push(SS);
            SS.Assign(Source[i]);
        }
    }
    /**
    Finds and returns a {@link tp.SelectSql} item by name, if any, else null
    @protected
    @param {string} Name The name of the {@link tp.SelectSql} item.
    @returns {tp.SelectSql} Returns a {@link tp.SelectSql} item by name, if any, else null
    */
    FindSelectSql(Name) {
        if (!tp.IsEmpty(this.SelectSqlList) && tp.IsString(Name) && !tp.IsBlank(Name)) {
            Name = tp.Trim(Name);
            if (!tp.StartsWith(Name, "SELECT ")) {
                return tp.FirstOrDefault(this.SelectSqlList, (item) => {
                    return tp.IsSameText(Name, item.Name);
                });
            }
        }

        return null;
    }
    /**
    Returns true if a {@link tp.DataColumn} column should be excluded from required columns. E.g. the tblItem Code column with a Code producer.
    @protected
    @param {tp.DataColumn} Column The {@link tp.DataColumn} column
    @returns {boolean} Returns true if a {@link tp.DataColumn} column should be excluded from required columns.
    */
    ExcludeRequiredColumn(Column) {
        return (Column.Table === this.tblItem) && (tp.IsSameText('Code', Column.Name));
    }
    /**
    Returns a list with {@link tp.DataColumn} required columns without a value, in a {@link tp.DataRow} row of a {@link tp.DataTable}.
    @protected
    @param {tp.DataTable} Table The {@link tp.DataTable} table containing the {@link tp.DataRow} row
    @param {tp.DataRow} Row The {@link tp.DataRow} row to check.
    @returns {tp.DataColumn[]} Returns a list with {@link tp.DataColumn} required columns without a value, in a {@link tp.DataRow} row of a {@link tp.DataTable}.
    */
    GetTableEmptyRequiredFields(Table, Row) {

        let Column,     // tp.DataColumn
            List = [];  // tp.DataColumn[]

        for (let i = 0, ln = Table.Columns.length; i < ln; i++) {
            Column = Table.Columns[i];
            if (Column.IsRequired && Row.IsNullOrEmpty(Column) && !this.ExcludeRequiredColumn(Column)) {
                List.push(Column);
            }
        }

        return List;
    }
    /**
    Returns a list with {@link tp.DataColumn}  required columns without a value, of the sole {@link tp.DataRow} row of the main {@link tp.DataTable} (tblItem).
    @protected
    @returns {tp.DataColumn[]} Returns a list with {@link tp.DataColumn}  required columns without a value, of the sole {@link tp.DataRow} row of the main {@link tp.DataTable} (tblItem).
    */
    GetMainTableEmptyRequiredFields() {

        var Table = this.tblItem;
        var Row = this.Row;
        var List = this.GetTableEmptyRequiredFields(Table, Row);
        return List;
    }

 

    /* public */
    /**
    If an object being stringified has a property named toJSON whose value is a function,
    then the toJSON method customizes JSON stringification behavior: instead of the object
    being serialized, the value returned by the toJSON method when called will be serialized.
    @see {@link http://www.ecma-international.org/ecma-262/5.1/#sec-15.12.3|specification}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify|mdn}
    @returns {object} Returns an object
     */
    toJSON() {
        var Result = {};

        Result.Name = this.Name;
        Result.IsListBroker = this.IsListBroker;
        Result.IsMasterBroker = this.IsMasterBroker;
        Result.State = this.State;
        Result.ConnectionName = this.ConnectionName;
        Result.MainTableName = this.MainTableName;
        Result.LinesTableName = this.LinesTableName;
        Result.SubLinesTableName = this.SubLinesTableName;
        Result.EntityId = this.EntityId;
        Result.EntityName = this.EntityName;
        Result.PrimaryKeyField = this.PrimaryKeyField;
        Result.GuidOids = this.GuidOids;
        Result.QueryNames = this.QueryNames;
        Result.Tables = this.DataSet.toJSON().Tables;

        return Result;
    }
    /**
    Assigns properties of this instance using as source a specified source object.
    @param {object} Source The source object to copy property values from.
    */
    Assign(Source) {

        var SetupColumns = function (Table) {
            var i, ln, Column;
            for (i = 0, ln = Table.Columns.length; i < ln; i++) {
                Column = Table.Columns[i];
            }
        };

        this.Name = Source.Name;
        this.IsListBroker = Source.IsListBroker;
        this.IsMasterBroker = Source.IsMasterBroker;
        this.State = Source.State;
        this.ConnectionName = Source.ConnectionName;
        this.MainTableName = Source.MainTableName;
        this.LinesTableName = Source.LinesTableName;
        this.SubLinesTableName = Source.SubLinesTableName;
        this.EntityId = Source.EntityId;
        this.EntityName = Source.EntityName;
        this.PrimaryKeyField = Source.PrimaryKeyField;
        this.GuidOids = Source.GuidOids;
        this.QueryNames = Source.QueryNames;

        var SourceTables = Source.DataSet ? Source.DataSet.Tables : Source.Tables;
        var Table;
        for (var i = 0, ln = SourceTables.length; i < ln; i++) {
            Table = this.DataSet.FindTable(SourceTables[i].Name);
            if (tp.IsEmpty(Table)) {
                Table = new tp.DataTable(SourceTables[i].Name);
                this.DataSet.AddTable(Table);
                Table.Assign(SourceTables[i]);
            } else {
                Table.AssignRows(SourceTables[i].Rows, true);
            }

            if (!tp.IsEmpty(Table) && !tp.IsEmpty(Table.Columns))
                SetupColumns(Table);
        }


        if (!this.fInitialized) {
            this.AssignSelectSqlList(Source.SelectSqlList);
        }

        this.fInitialized = true;
    }
    /**
    Removes all rows from all the tables
    */
    Clear() {
        if (!tp.IsEmpty(this.tblItem))
            this.tblItem.Clear();
    }

    /**
    Finds and returns a {@link tp.DataTable} table by name, if any, else null
    @param {string} TableName The table name
    @returns {tp.DataTable} A  {@link tp.DataTable} table or null.
    */
    FindTable(TableName) {
        return this.DataSet.FindTable(TableName);
    }

    /**
    Executes an action (Insert, Edit, Delete, Commit, SelectList) and retuns a {@link tp.BrokerAction} {@link Promise}.
    @protected
    @param {tp.BrokerAction} Action - An information object regarding the action. It also contains the tp.AjaxArgs of the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Action(Action) {

        let Args = await tp.Ajax.Async(Action.Args);
        Action.Args = Args; 

        if (Action.AssignFlag === true) {
            this.Clear();
            this.Assign(Args.Packet);
        }

        this.OnAction(Action);

        return Action;        

    }


    /**
    Before function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoInitializeBefore(Action) {
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoInitialize(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoInitializeAfter(Action) {
    }


    /**
    Before function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoInsertBefore(Action) {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    CheckCanInsert(Action) {
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoInsert(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoInsertAfter(Action) {
    }



    /**
    Before function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoEditBefore(Action) {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    CheckCanEdit(Action) {
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoEdit(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoEditAfter(Action) {
    }



    /**
    Before function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoDeleteBefore(Action) {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    CheckCanDelete(Action) {
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoDelete(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoDeleteAfter(Action) {
    }




    /**
    Before function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoCommitBefore(Action) {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    CheckCanCommit(Action) {

        var Flag = tp.Bf.In(this.State, tp.DataMode.Insert | tp.DataMode.Edit);
        if (!Flag)
            tp.Throw('Broker can not commit because not in Insert or Edit mode');

        var List = this.GetMainTableEmptyRequiredFields();
        if (List.length > 0) {
            var S, Column, SB = new tp.StringBuilder();
            SB.AppendLine('Required fields without values:');
            for (var i = 0, ln = List.length; i < ln; i++) {
                Column = List[i];
                S = tp.Format('{0} - ({1}.{2})', Column.Title, Column.Table.Name, Column.Name);
                SB.AppendLine(S);
            }

            tp.Throw(SB.ToString());
        }
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoCommit(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoCommitAfter(Action) {
    }



    /**
    Before function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoSelectListBefore(Action) {
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoSelectList(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoSelectListAfter(Action) {
    }


    /* standard actions */
    /**
    Initializes the broker.  Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise}. <br />
    CAUTION: Returns a null {@link Promise} if this broker is already initialized.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Initialize() {
        if (this.fInitialized === false) {
            let Action = new tp.BrokerAction(this, tp.BrokerAction.Initialize);
            Action.Args.Data.BrokerName = this.Name;

            this.DoInitializeBefore(Action);
            Action = await this.DoInitialize(Action);
            this.DoInitializeAfter(Action);

            return Action;
        }

        return null; 
    }
    /**
    Sets the broker in the insert mode. Initializes the broker if needed. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Insert() {
        if (this.fInitialized === false) {
            await this.Initialize();
        }

        let Action = new tp.BrokerAction(this, tp.BrokerAction.Insert);
        Action.Args.Data.BrokerName = this.Name;

        this.DoInsertBefore(Action);
        this.CheckCanInsert(Action);
        Action = await this.DoInsert(Action);
        this.DoInsertAfter(Action);

        return Action;
    }
    /**
    Loads an entity this broker represents, by Id. Initializes the broker if needed. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {any} Id - The Id of the entity in the main table (top table, i.e. tblItem)
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Edit(Id) {
        if (this.fInitialized === false) {
            await this.Initialize();
        }

        let Action = new tp.BrokerAction(this, tp.BrokerAction.Edit);
        Action.Args.Data.BrokerName = this.Name;
        Action.Args.Data.Id = Id;     

        this.DoEditBefore(Action);
        this.CheckCanEdit(Action);
        Action = await this.DoEdit(Action);
        this.DoEditAfter(Action);

        return Action;
    }
    /**
    Deletes an entity this broker represents, by Id. Initializes the broker if needed. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {any} Id - The Id of the entity in the main table (top table, i.e. tblItem)
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Delete(Id) {
        if (this.fInitialized === false) {
            await this.Initialize();
        }

        let Action = new tp.BrokerAction(this, tp.BrokerAction.Delete);
        Action.Args.Data.BrokerName = this.Name;
        Action.Args.Data.Id = Id; 

        this.DoDeleteBefore(Action);
        this.CheckCanDelete(Action);
        Action = await this.DoDelete(Action);

        // delete row from List table
        if (Action.Succeeded === true) {
            if (this.tblList instanceof tp.DataTable) {
                let Row = this.tblList.FindRow(this.PrimaryKeyField, Id);
                if (!tp.IsEmpty(Row)) {
                    this.tblList.RemoveRow(Row);
                }
            }
        }

        this.DoDeleteAfter(Action);

        return Action;
    }
    /**
    Commits the entity this broker represents (commits an insert or edit). The broker must already be initialized or an exception is thrown. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Commit() {
        this.CheckInitialized();

        let Action = new tp.BrokerAction(this, tp.BrokerAction.Commit);

        Action.Args.EncodeArgs = false;
        
        Action.Args.UriEncodeData = false;

        this.DoCommitBefore(Action);
        this.CheckCanCommit(Action);

        // WARNING: The serialization must be done here, at the last moment
        // so any code can have the chance to alter table data until that last moment
        var Model = this.toJSON();
        let JsonText = JSON.stringify(Model);
        Action.Args.Data = JsonText;

        Action.Args.ContentType = 'application/json; charset=utf-8';
 
        Action = await this.DoCommit(Action);
        this.DoCommitAfter(Action);

        return Action; 
    }
    /**
    Executes a SELECT for the List grid. Initializes the broker if needed. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {string | tp.SelectSql} SelectSql - The SELECT statement to execute
    @param {bool} [UseRowLimit] - Optional. True to use the default row limit
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async SelectList(SelectSql, UseRowLimit = true) {
        if (this.fInitialized === false) {
            await this.Initialize();
        }

        this.LastSelectSql = null;
 
        let Action = new tp.BrokerAction(this, tp.BrokerAction.SelectList);
        Action.Args.Data.BrokerName = this.Name;       
        Action.Args.Data.SqlText = SelectSql.Text;
        Action.Args.Data.UseRowLimit = tp.IsBoolean(UseRowLimit) ? UseRowLimit : true;

        Action.SelectSql = tp.IsString(SelectSql) ? new tp.SelectSql(SelectSql) : SelectSql;

        this.DoSelectListBefore(Action);
        Action = await this.DoSelectList(Action);

        if (Action.Succeeded === true) {
            if (!tp.IsEmpty(Action) && !tp.IsEmpty(Action.Args.Packet)) {
                let Packet = Action.Args.Packet;
                this.CreateListTable(Packet);

                this.LastSelectSql = Action.SelectSql;             
                Action.SelectSql.Table = this.tblList;
            }
        }

        this.DoSelectListAfter(Action);

        return Action; 
    }

    /* Event triggers */
    /**
    Event trigger
    @param {tp.BrokerAction} Action The {@link tp.BrokerAction} action
    */
    OnAction(Action) {
        var Args = new tp.BrokerEventArgs(this, Action);
        this.Trigger('Action', Args);
    }

};


/* fields */

/** Field
@private
@type {boolean}
*/
tp.Broker.prototype.fInitialized = false;
/** Field
@private
@type {tp.BrokerView}
*/
tp.Broker.prototype.fView = null;
/** Field
@type {tp.DataSet}
*/
tp.Broker.prototype.DataSet = null;
/** Field
@type {tp.DataTable}
*/
tp.Broker.prototype.tblList = null;
/** Field
@type {string[]}
*/
tp.Broker.prototype.QueryNames = [];
/** Field
@type {tp.SelectSql[]}
*/
tp.Broker.prototype.SelectSqlList = [];
/** Field. The last SelectSql statement executed for the List (not for the Item) 
@type {tp.SelectSql}
*/
tp.Broker.prototype.LastSelectSql = null;
/** Field
@type {tp.BrokerAction}
*/
tp.Broker.prototype.LastAction = tp.BrokerAction.None;
/** Field
@type {string}
*/
tp.Broker.prototype.Name = '';
/** Field
@type {boolean}
*/
tp.Broker.prototype.IsListBroker = false;
/** Field
@type {boolean}
*/
tp.Broker.prototype.IsMasterBroker = false;
/** Field
@type {tp.DataMode}
*/
tp.Broker.prototype.State = tp.DataMode.None;
/** Field
@type {string}
*/
tp.Broker.prototype.ConnectionName = '';
/** Field
@type {string}
*/
tp.Broker.prototype.MainTableName = '';
/** Field
@type {string}
*/
tp.Broker.prototype.LinesTableName = '';
/** Field
@type {string}
*/
tp.Broker.prototype.SubLinesTableName = '';
/** Field
@type {number}
*/
tp.Broker.prototype.EntityId;
/** Field
@type {string}
*/
tp.Broker.prototype.EntityName = '';
/** Field
@type {string}
*/
tp.Broker.prototype.PrimaryKeyField = '';
/** Field
@type {boolean}
*/
tp.Broker.prototype.GuidOids = true;

//#endregion

//---------------------------------------------------------------------------------------
// BrokerView
//---------------------------------------------------------------------------------------
 
//#region tp.BrokerView
/**
A data view represents a control container that automatically can create its controls from the provided markup
and bind to data provided by data-tables of a broker. <br />
Example markup
<pre>
    <div class="tp-View" data-setup="{ ClassType: tp.BrokerView, BrokerName: 'Customer' }"> ... </div>
    or
    <div class="tp-View" data-setup="{ ClassType: tp.BrokerView, BrokerClass: tp.App.CustomerBroker }"> ... </div>
    or
    <div class="tp-View" data-setup="{ ClassType: tp.BrokerView, BrokerName: 'Customer', BrokerClass: tp.App.CustomerBroker }"> ... </div>
</pre>
*/
tp.BrokerView = class extends tp.View {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <div class="tp-View" data-setup="{ ClassType: tp.BrokerView, BrokerName: 'Customer' }">
            ...
        </div>

        or

        <div class="tp-View" data-setup="{ ClassType: tp.BrokerView, BrokerClass: tp.App.CustomerBroker }">
            ...
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /**
    Gets or sets the data mode. One of the {@link tp.DataViewMode} constants.
    @type {number}
    */
    get ViewMode() {
        return this.fViewMode;
    }
    set ViewMode(v) {
        if (this.fViewMode !== v) {
            this.fLastViewMode = this.fViewMode;
            this.fViewMode = v;
            this.OnViewModeChanged();
            this.EnableCommands();
        }
    }
    /**
    Returns the sole {@link tp.DataRow} row of the tblItem, if any, else null
    @type {tp.DataRow}
    */
    get Row() {
        if (!tp.IsEmpty(this.tblItem) && (this.tblItem.RowCount > 0))
            return this.tblItem.Rows[0];
        return null;
    }
    /**
    Returns the top data-table. This is always a {@link tp.DataTable} table with a single row.
    @type {tp.DataTable}
    */
    get tblItem() {
        if (!tp.IsEmpty(this.ftblItem))
            return this.ftblItem;
        if (!tp.IsEmpty(this.Broker))
            return this.Broker.tblItem;
        return null;
    }
    /**
    Returns the first level detail {@link tp.DataTable} table, if defined.
    This is a handy property and can be used when there is only a single detail table in the first level and the LinesTableName property has a valid table name value.
    @type {tp.DataTable}
    */
    get tblLines() {
        if (!tp.IsEmpty(this.ftblLines))
            return this.ftblLines;
        if (!tp.IsEmpty(this.Broker))
            return this.Broker.tblLines;
        return null;
    }
    /**
    Returns the second level detail {@link tp.DataTable} table, if defined.
    This is a handy property and can be used when there is only a single detail table in the second level and the SubLinesTableName property has a valid table name value.
    @type {tp.DataTable}
    */
    get tblSubLines() {
        if (!tp.IsEmpty(this.ftblSubLines))
            return this.ftblSubLines;
        if (!tp.IsEmpty(this.Broker))
            return this.Broker.tblSubLines;
        return null;
    }


    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.BrokerView';
        this.fDefaultCssClasses = [tp.Classes.View, tp.Classes.BrokerView];
    }
    /**
    Initializes fields and properties just before applying the create params.      
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        this.fName = tp.NextName('BrokerView');
        this.Broker = null;

        this.fViewMode = tp.DataViewMode.None;
        this.fLastViewMode = tp.DataViewMode.None;

        this.PrimaryKeyField = 'Id';
        this.ForceSelect = false;
    }

 

    /* overridables */

    /** This is called after the base class initialization completes and creates just the tool-bar, the main panel-list and the List grid. <br />
     * It also creates the broker. <br /> 
     * NOTE: Controls of the edit part are created and bound the first time an insert or edit is requested.
     * @protected
     * @override
    */
    InitializeView() {
        super.InitializeView();

        this.CreateToolBar();
        this.CreateTabControl();
        this.CreateListGrid();

        this.CreateBroker();

        if (!tp.IsEmpty(this.Broker)) {
            this.Broker.fView = this;
            this.BrokerInitialize();
        }
    }

    /**
    Sets the visible panel index in the panel list.
    @protected
    @param {number} PageIndex The panel index
    */
    SetVisiblePage(PageIndex) {
        if (this.MainPager) {
            this.MainPager.SelectedIndex = PageIndex;
        }
    }
    /**
     * Sets the visible panel of the main pager (a PanelList) by its 'PanelMode'.
     * NOTE: Each panel of the main pager (a PanelList) may have a data-setup with a 'PanelMode' string property indicating the 'mode' of the panel.
     * @param {string} PageName The panel mode to check for.
     */
    SetVisiblePageByName(PageName) {
        let elPanel = this.FindPageByName(PageName);
        let Index = -1;
        if (elPanel) {
            let Panels = this.GetTabPageElements();
            Index = Panels.indexOf(elPanel);
        }

        if (Index >= 0) {
            this.SetVisiblePage(Index);
        }
    }
    /**
     * Each panel of the main pager (a PanelList) MUST have a data-setup with a 'PanelMode' string property indicating the 'mode' of the panel.
     * This function returns a panel found having a specified PanelMode, or null if not found.
     * @param {string} PageName The panel mode to check for.
     * @returns {HTMLElement} Returns a panel found having a specified PanelMode, or null if not found.
     */
    FindPageByName(PageName) {
        if (tp.IsValid(this.MainPager)) {
            let Panels = this.GetTabPageElements();

            let i, ln, elPanel, Setup;

            for (i = 0, ln = Panels.length; i < ln; i++) {
                elPanel = Panels[i];
                Setup = tp.GetDataSetupObject(elPanel);
                if (tp.IsValid(Setup)) {
                    if (PageName === Setup.Name) {
                        return elPanel;
                    }
                }
            }
        }

        return null;
    }

    /** Returns a DOM Element contained by this view.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetToolBarElement() { return tp.Select(this.Handle, '.ToolBar'); }
    /** Returns a DOM Element contained by this view. Returns the main panel-list which in turn contains the three part panels: Brower, Edit and Filters.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetTabControlElement() { return tp.Select(this.Handle, '.MainContainer'); }
    /** Returns an array with the panels of the panel list
     * @returns {HTMLElement[]}
     * */
    GetTabPageElements() {
        if (tp.IsValid(this.MainPager)) {
            let List = tp.ChildHTMLElements(this.GetTabControlElement());
            if (List && List.length === 2) {
                return tp.ChildHTMLElements(List[1])
            }               
        }
        return [];
    }
    /** Returns a DOM Element contained by this view. Returns the Filters panel, the container of the filter controls.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetFilterPageElement() { return this.FindPageByName('Filters'); }
    /** Returns a DOM Element contained by this view. Returns the List (browser) Panel, the container of the List (browser) grid, which displays the results of the various SELECTs of the broker.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetListPageElement() { return this.FindPageByName('List'); }
    /** Returns a DOM Element contained by this view. Returns the Edit Panel, which is the container for all edit controls bound to broker datasources.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetEditPageElement() { return this.FindPageByName('Edit'); }
    /** Returns a DOM Element contained by this view. Returns the element upon to create the List (browser) grid.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetListGridElement() { return tp.Select(this.GetListPageElement(), '.Grid'); }

    /** Creates the toolbar of the view 
     @protected
     */
    CreateToolBar() {
        if (tp.IsEmpty(this.ToolBar)) {
            let el = this.GetToolBarElement();
            this.ToolBar = new tp.ToolBar(el);
            this.ToolBar.On('ButtonClick', this.AnyClick, this);
        }
    }
    /** Creates the panel-list of the view, the one with the 3 panels: List, Edit and Filters panels.
     @protected
     */
    CreateTabControl() {
        if (tp.IsEmpty(this.MainPager)) {
            let el = this.GetTabControlElement();
            this.MainPager = new tp.TabControl(el);
            this.MainPager.ShowTabBar(false);   // hide tab-bar 
        }
    }
    /**
    Creates the List (browser) grid. <br />
    NOTE: For the List (browser) grid to be created automatically by this method, a div marked with the Grid class is required in the List panel.
    @protected
    */
    CreateListGrid() {
        if (tp.IsEmpty(this.gridList)) {
            let el = this.GetListGridElement();
            this.gridList = new tp.Grid(el);
        }

        if (tp.IsEmpty(this.gridList)) {
            let o = tp.FindComponentByCssClass(tp.Classes.Grid, this.GetListPageElement());
            this.gridList = o instanceof tp.Grid ? o : null;
        }

        if (this.gridList) {
            this.gridList.ReadOnly = true;
            this.gridList.AllowUserToAddRows = false;
            this.gridList.AllowUserToDeleteRows = false;
            this.gridList.ToolBarVisible = false;

            this.gridList.On(tp.Events.DoubleClick, this.ListGrid_DoubleClick, this);
        }
    }
    /** Creates and binds the controls of the edit part, if not already created.
     * */
    CreateEditControls() {
        if (!this.EditControlsCreated) {
            let el = this.GetEditPageElement();
            let ControlList = tp.Ui.CreateContainerControls(el);

            let List = tp.ChildHTMLElements(el);
            if (List.length === 1) {
                let o = tp.GetComponent(List[0]);
                if (o instanceof tp.TabControl) {
                    this.EditPager = o;
                    if (this.EditPager.GetPageCount() === 1) {
                        this.EditPager.ShowTabBar(false);   // hide tab-bar if we have only a single page
                    }
                }
            }

            this.BindControls(ControlList);
            this.EditControlsCreated = true;
        }
    }
    /** Creates the controls of the filter part, if not already created. 
     * */
    CreateFilterControls() {

        if (!tp.IsValid(this.SelectSqlListUi)) {
            let elParent = this.FindPageByName('Filters');
            if (elParent) {
                let el = tp.Div(elParent);
                let CP = {};
                CP.SelectSqlList = this.Broker.SelectSqlList;
                this.SelectSqlListUi = new tp.SqlFilterSelectSqlListUi(el, CP);
                this.SelectSqlListUi.On('Execute', (Args) => {
                    this.ListSelect();
                });
            }
        }

    }

    /**
    Creates the broker based on CreateParams settings
    @protected
    */
    CreateBroker() {
        let BrokerName = '';

        if (!tp.IsBlank(this.BrokerName))
            BrokerName = this.BrokerName;
        else if ('BrokerName' in this.CreateParams && tp.IsString(this.CreateParams.BrokerName))
            BrokerName = this.CreateParams.BrokerName;

        if (tp.IsEmpty(this.Broker)) {
            if ('BrokerClass' in this.CreateParams) {

                if (tp.IsString(this.CreateParams.BrokerClass) && !tp.IsBlank(this.CreateParams.BrokerClass))
                    this.CreateParams.BrokerClass = tp.StrToClass(this.CreateParams.BrokerClass, true);

                if (!tp.IsFunction(this.CreateParams.BrokerClass))
                    tp.Throw(`Cannot create broker. No broker class for a view: ${this.Name}`);

                this.Broker = new this.CreateParams.BrokerClass(BrokerName);
            } else if (!tp.IsBlank(BrokerName)) {
                this.Broker = new tp.Broker(BrokerName);
                this.BrokerName = BrokerName;
            }
        }
    }

    /* data-binding */
    /**
    Finds and returns a {@link tp.DataSource} data-source by name, if any, else null.
    @protected
    @param {string} SourceName The data-source by name
    @returns {tp.DataSource} Returns a {@link tp.DataSource} data-source or null
    */
    GetDataSource(SourceName) {
        let Result = null;

        if (!tp.IsEmpty(this.Broker)) {
            if (tp.IsSameText('Item', SourceName) || tp.IsBlank(SourceName)) {
                SourceName = this.Broker.MainTableName;
            } else if (tp.IsSameText('Lines', SourceName)) {
                SourceName = this.Broker.LinesTableName;
            } else if (tp.IsSameText('SubLines', SourceName)) {
                SourceName = this.Broker.SubLinesTableName;
            }
        }

        if (tp.IsArray(this.DataSources)) {
            Result = this.DataSources.find((item) => {
                return tp.IsSameText(SourceName, item.Name);
            });
        } 

        if (tp.IsEmpty(Result) && !tp.IsEmpty(this.Broker)) {
            let Table = this.Broker.DataSet.FindTable(SourceName);
            if (!tp.IsEmpty(Table)) {
                let DataSource = new tp.DataSource(Table);
                this.DataSources.push(DataSource);
                return DataSource;
            }
        }

        return Result;
    }
 
    /**
    Returns the Id (value of the primary key field) of the selected data-row of the List (browser) grid, if any, else null.
    @protected
    @returns {any} Returns the Id (value of the primary key field) of the selected data-row of the browser grid, if any, else null.
    */
    GetListSelectedId() {
        if (tp.IsBlank(this.PrimaryKeyField)) {
            if (this.tblItem instanceof tp.DataTable) {
                this.PrimaryKeyField = this.tblItem.PrimaryKeyField;
            }
        }

        if (!tp.IsBlank(this.PrimaryKeyField) && !tp.IsEmpty(this.gridList)) {
            var Row = this.gridList.FocusedRow;
            if (!tp.IsEmpty(Row) && Row.Table.ContainsColumn(this.PrimaryKeyField)) {
                return Row.Get(this.PrimaryKeyField);
            }
        }

        return null;
    }
 
    /**
    Displays the main local menu
    @protected
    */
    DisplayHomeLocalMenu() {
    }
    /**
    Displays the filter panel of the current SelectSql
    @protected
    */
    DisplayFilterPanel() {
        this.CreateFilterControls();
        this.ViewMode = tp.DataViewMode.Filters;
    }



    /* action do methods */
    /**
    Before function
    @protected
    */
    DoInitializeBefore() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoInitializeAfter(Action) {
    }

    /**
    Before function
    @protected
    */
    DoInsertBefore() {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    */
    CheckCanInsert() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoInsertAfter(Action) {
    }

    /**
    Before function 
    @protected
    */
    DoEditBefore() {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    */
    CheckCanEdit() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoEditAfter(Action) {
    }

    /**
    Before function
    @protected
    */
    DoDeleteBefore() {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    */
    CheckCanDelete() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoDeleteAfter(Action) {
    }

    /**
    Before function
    @protected
    */
    DoCommitBefore() {
    }
    /**
    Check function. It may check a number of conditions and throw an exception.
    @protected
    */
    CheckCanCommit() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoCommitAfter(Action) {
    }

    /**
    Before function
    @protected
    */
    DoSelectListBefore() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoSelectListAfter(Action) {
    }

    /**
    Displays an error note
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DisplayActionError(Action) {
        if (tp.IsBlank(Action.ErrorText))
            Action.ErrorText = tp.Format('{0}.{1} failed. Reason: Unknown', Action.BrokerName, Action.Name);
        tp.ErrorNote(Action.ErrorText);
    }

    /* broker actions */
    /**
    Initializes the broker, if not already initialized. 
    Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise} if broker is already initialized. <br />
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise} if broker is already initialized.
    */
    async BrokerInitialize() {
        let Action = null;

        if (!tp.IsEmpty(this.Broker) && !this.Broker.Initialized) {
            this.DoInitializeBefore();

            Action = await this.Broker.Initialize();

            if (Action instanceof tp.BrokerAction) {
                if (Action.Succeeded === true) {
                    this.DoInitializeAfter(Action);

                    this.ViewMode = tp.DataViewMode.List;

                    await this.ExecuteStartCmd();
                } else {
                    this.DisplayActionError(Action);
                }
            }
        }

        return Action;
    }
    /**
    Sets the broker in the insert mode. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Insert() {
        this.CreateEditControls();

        let Action = null;

        if (!tp.IsEmpty(this.Broker)) {
            this.DoInsertBefore();
            this.CheckCanInsert();

            Action = await this.Broker.Insert();
            if (Action instanceof tp.BrokerAction) {
                if (Action.Succeeded === true) {
                    this.UpdateDataSources();
                    this.ApplyReadOnlyEditToControls(false);

                    this.ViewMode = tp.DataViewMode.Insert;
                    this.DoInsertAfter(Action);

                } else {
                    this.DisplayActionError(Action);
                }
            }
        }

        return Action;
    }
    /**
    Loads an entity this view represents, by Id. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {any} [Id] - Optional. The Id of the entity in the main table (top table, i.e. tblItem)
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Edit(Id = null) {
        this.CreateEditControls();

        let Action = null;

        Id = Id || this.GetListSelectedId();

        if (tp.IsEmpty(Id)) {
            tp.ErrorNote('No selected row');
        } else if (!tp.IsEmpty(this.Broker)) {
            this.DoEditBefore();
            this.CheckCanEdit();

            Action = await this.Broker.Edit(Id);
            if (Action instanceof tp.BrokerAction) {
                if (Action.Succeeded === true) {
                    this.UpdateDataSources();
                    this.ApplyReadOnlyEditToControls(true);

                    this.DoEditAfter(Action);

                    this.ViewMode = tp.DataViewMode.Edit;

                } else {
                    this.DisplayActionError(Action);
                }
            }
        }

        return Action;
    }
    /**
    Deletes an entity this view represents, by Id.  Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {any} [Id] - Optional. The Id of the entity in the main table (top table, i.e. tblItem)
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Delete(Id = null) {
        let Action = null;

        Id = Id || this.GetListSelectedId();

        if (tp.IsEmpty(Id)) {
            tp.ErrorNote('No selected row');
        } else if (!tp.IsEmpty(this.Broker)) {

            let Flag = await tp.YesNoBoxAsync('Delete row?');

            if (Flag === true) {
                this.DoDeleteBefore();
                this.CheckCanDelete();

                Action = await this.Broker.Delete(Id);

                if (Action instanceof tp.BrokerAction) {
                    if (Action.Succeeded === true) {
                        tp.SuccessNote('Deleted successfully');

                        this.DoDeleteAfter(Action);

                        this.ViewMode = tp.DataViewMode.List;

                    } else {
                        this.DisplayActionError(Action);
                    }
                }
            }
        }

        return Action;
    }
    /**
    Commits the entity this view represents (commits an insert or edit). The broker must already be initialized or an exception is thrown. <br />
    Retuns a {@link tp.BrokerAction} {@link Promise}.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Commit() {
        let Action = null;

        if (!tp.IsEmpty(this.Broker)) {

            this.DoCommitBefore();
            this.CheckCanCommit();

            Action = await this.Broker.Commit();

            if (Action instanceof tp.BrokerAction) {
                if (Action.Succeeded === true) {
                    this.UpdateDataSources();
                    this.ApplyReadOnlyEditToControls(true);

                    tp.SuccessNote('Saved successfully');

                    this.DoCommitAfter(Action);
                    this.ForceSelect = true;

                    this.ViewMode = tp.DataViewMode.Edit;

                } else {
                    this.DisplayActionError(Action);
                }
            }
        }

        return Action;
    }

    /**
    Sets the data-source of the List (browser) grid
    @param {tp.SelectSql} SelectSql - The SelectSql instance containing the DataTable.
    @protected
    */
    UpdateListDataSource(SelectSql) {
        this.gridList.DataSource = null;

        if (tp.IsValid(this.Broker) && (SelectSql instanceof tp.SelectSql)) {
            let Table = this.Broker.tblList;

            SelectSql.SetupTable(Table);

            this.gridList.AutoGenerateColumns = !tp.IsValid(SelectSql.Columns) || SelectSql.Columns.length === 0;
            this.gridList.ClearColumns();

            this.dsList = new tp.DataSource(Table);

            // grid columns should be defined before binding
            if (!this.gridList.AutoGenerateColumns)
                this.gridList.AddSelectSqlColumns(Table, SelectSql.Columns);

            this.gridList.DataSource = this.dsList;
        }
    }

    /**
    Executes a SELECT for the List (browser) grid. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {string | tp.SelectSql} [SelectSql] - Optional. The SELECT statement to execute
    @param {number} [RowLimit] - Optional. The row limit
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async ListSelect() {
        let Action = null;

        if (!tp.IsEmpty(this.Broker)) {

            // SelectSql
            /** @type {tp.SelectSql} */
            let SelectSql = null;

            if (tp.IsValid(this.SelectSqlListUi)) {
                SelectSql = this.SelectSqlListUi.GenerateSql();
            }

            if (tp.IsEmpty(SelectSql) && this.Broker.SelectSqlList.length > 0) {
                SelectSql = this.Broker.SelectSqlList[0];
            }

            // RowLimit
            /** @type {boolean} */
            let UseRowLimit = tp.IsValid(this.SelectSqlListUi) ? this.SelectSqlListUi.UseRowLimit : true;


            let SqlText = SelectSql.Text;
            let ForceSelect = this.ForceSelect || SqlText !== this.LastSelectSqlText || UseRowLimit !== this.LastUseRowLimit;

            if (ForceSelect) {
                this.LastSelectSqlText = SqlText;
                this.LastUseRowLimit = UseRowLimit;

                this.DoSelectListBefore();

                Action = await this.Broker.SelectList(SelectSql, UseRowLimit);

                if (Action instanceof tp.BrokerAction) {
                    if (Action.Succeeded === true) {

                        this.UpdateListDataSource(SelectSql);

                        if (this.dsList) {
                            if (this.dsList.CanFirst())
                                this.dsList.First();
                            if (this.dsList.Rows.length > 0 && this.gridList) {
                                this.gridList.FocusedRow = this.dsList.Rows[0];
                            }
                        }

                        this.DoSelectListAfter(Action);
                        this.ForceSelect = false;

                        this.ViewMode = tp.DataViewMode.List;

                    } else {
                        this.DisplayActionError(Action);
                    }
                }
            }
            else {
                this.ViewMode = tp.DataViewMode.List;
            }


        }

        return Action;
    }

    /* commands/modes */
    /**
Executes the start command. Called by the DoInitializeAfter(). <br />
Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise} if broker is already initialized. <br />
@protected
@returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise} if broker is already initialized. <br />
*/
    async ExecuteStartCmd() {
        if (!tp.IsEmpty(this.gridList) && !tp.IsEmpty(this.Broker) && this.Broker.Initialized)
            await this.ListSelect();
        return null;
    }

    /**
    Returns the bit-field (set) of the valid commands for this view. <br />
    @protected
    @returns {number} Returns the bit-field (set) of the valid commands for this view. <br />
    */
    GetValidCommands() {
        let Result = tp.DataViewMode.None;

        for (var PropName in tp.DataViewMode) {
            if (tp.IsInteger(tp.DataViewMode[PropName])) {
                Result |= tp.DataViewMode[PropName];
            }
        }

        return Result;
    }
    /**
    Validates standard commands, that is decides which is or not valid at the moment of the call.
    @protected
    */
    ValidateCommands() {

        let Navigation = tp.DataViewMode.First |
            tp.DataViewMode.Prior |
            tp.DataViewMode.Next |
            tp.DataViewMode.Last;

        this.ValidCommands = this.GetValidCommands();
        this.ValidCommands = tp.Bf.Subtract(this.ValidCommands, Navigation);

        switch (this.ViewMode) {
            case tp.DataViewMode.List:
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands,
                    tp.DataViewMode.List |
                    tp.DataViewMode.Save |
                    tp.DataViewMode.Cancel
                );
                break;
            case tp.DataViewMode.Insert:
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands,
                    tp.DataViewMode.Insert |
                    tp.DataViewMode.Edit |
                    tp.DataViewMode.Delete
                );
                break;
            case tp.DataViewMode.Edit:
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands,
                    tp.DataViewMode.Edit
                );
                break;
            case tp.DataViewMode.Delete:
                break;
            case tp.DataViewMode.Cancel:
                break;
            case tp.DataViewMode.Save:
                break;

            case tp.DataViewMode.Filters:
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands,
                    tp.DataViewMode.Filters |
                    tp.DataViewMode.Insert |
                    tp.DataViewMode.Edit |
                    tp.DataViewMode.Delete |
                    tp.DataViewMode.Save |
                    tp.DataViewMode.Cancel
                );
                break;
        }


        if (this.dsList) {
            if (!this.dsList.CanFirst())
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands, tp.DataViewMode.First);
            if (!this.dsList.CanPrior())
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands, tp.DataViewMode.Prior);
            if (!this.dsList.CanNext())
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands, tp.DataViewMode.Next);
            if (!this.dsList.CanLast())
                this.ValidCommands = tp.Bf.Subtract(this.ValidCommands, tp.DataViewMode.Last);
        }
    }
    /**
    Enables/disables buttons and menu items.
    @protected
    */
    EnableCommands() {
        this.ValidateCommands();
        if (tp.IsNumber(this.ValidCommands)) {
            if (this.ToolBar) {

                let ControlList = tp.GetAllComponents(this.ToolBar.Handle),
                    c,          // tp.Component,
                    Command,    // string
                    ViewMode    // integer
                    ;

                for (let i = 0, ln = ControlList.length; i < ln; i++) {
                    c = ControlList[i];
                    if (tp.HasCommandProperty(c)) {
                        Command = c.Command;

                        if (!tp.IsBlank(Command) && Command in tp.DataViewMode) {
                            ViewMode = tp.DataViewMode[Command];
                            c.Enabled = tp.Bf.In(ViewMode, this.ValidCommands);
                        }
                    }
                }
            }
        }

    }
    /**
    Executes a standard command by name
    @param {number | string} Command - One of the {@link tp.DataViewMode} constants, either the name or the value.
    */
    ExecuteCommand(Command) {
        let ViewMode = tp.DataViewMode.None;

        if (tp.IsString(Command) && !tp.IsBlank(Command)) {
            ViewMode = Command in tp.DataViewMode ? tp.DataViewMode[Command] : tp.DataViewMode.None;
        }
        else if (tp.IsInteger(Command)) {
            ViewMode = Command;
        }
 
        switch (ViewMode) {
            case tp.DataViewMode.None:
                this.ExecuteCustomCommand(Command);
                break;

            case tp.DataViewMode.Home:
                this.DisplayHomeLocalMenu();
                break;

            case tp.DataViewMode.List:
                this.ListSelect();
                break;
            case tp.DataViewMode.Filters:
                this.DisplayFilterPanel();
                break;

            case tp.DataViewMode.First:
                if (this.dsList && this.dsList.CanFirst()) {
                    this.dsList.First();
                    this.Edit();
                }
                break;
            case tp.DataViewMode.Prior:
                if (this.dsList && this.dsList.CanPrior()) {
                    this.dsList.Prior();
                    this.Edit();
                }
                break;
            case tp.DataViewMode.Next:
                if (this.dsList && this.dsList.CanNext()) {
                    this.dsList.Next();
                    this.Edit();
                }
                break;
            case tp.DataViewMode.Last:
                if (this.dsList && this.dsList.CanLast()) {
                    this.dsList.Last();
                    this.Edit();
                }
                break;


            case tp.DataViewMode.Edit:
                this.Edit();
                break;
            case tp.DataViewMode.Insert:
                this.Insert();
                break;
            case tp.DataViewMode.Delete:
                this.Delete();
                break;
            case tp.DataViewMode.Save:
                this.Commit();
                break;
            case tp.DataViewMode.Cancel:
                if (this.ForceSelect !== true) {
                    this.ViewMode = tp.DataViewMode.Cancel;
                    this.ViewMode = tp.DataViewMode.List;
                } else {
                    this.ListSelect();
                }

                break;

            case tp.DataViewMode.Close:
                this.CloseView();
                break;
        }
    }
    /**
    Executes a custom command by name specified by a command name.
    @param {string} Command - A string denoting the custom command.
    */
    ExecuteCustomCommand(Command) {
    }



    /* Event triggers */
    /**
    Event trigger
    */
    OnViewModeChanged() {
        switch (this.ViewMode) {
            case tp.DataViewMode.List:
            case tp.DataViewMode.Cancel:
                this.SetVisiblePageByName('List');
                break;

            case tp.DataViewMode.Insert:
            case tp.DataViewMode.Edit:
                this.SetVisiblePageByName('Edit');
                break;

            case tp.DataViewMode.Filters:
                this.SetVisiblePageByName('Filters');
                break;
        }

        this.Trigger('OnViewModeChanged', {});
    }

    /**
    Event handler. If a Command exists in the clicked element then the Args.Command is assigned.
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    AnyClick(Args) {
        if (Args.Handled !== true) {
            var Command = tp.GetCommand(Args);
            if (!tp.IsBlank(Command))
                this.ExecuteCommand(Command);
        }
    }
    /**
    Event handler
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    ListGrid_DoubleClick(Args) {
        this.ExecuteCommand(tp.DataViewMode.Edit);
    }
};

/* fields */

/** Field
 @protected
 @type {tp.Broker}
 */
tp.BrokerView.prototype.Broker = null;
/** Field
 @protected
 @type {tp.DataSource}
 */
tp.BrokerView.prototype.dsList = null;
/** Field
 @protected
 @type {tp.Grid}
 */
tp.BrokerView.prototype.gridList = null;
/** Field
 @protected
 @type {string}
 */
tp.BrokerView.prototype.BrokerName = '';
/** Field
 @protected
 @type {tp.DataTable}
 */
tp.BrokerView.prototype.ftblItem = null;
/** Field
 @protected
 @type {tp.DataTable}
 */
tp.BrokerView.prototype.ftblLines = null;
/** Field
 @protected
 @type {tp.DataTable}
 */
tp.BrokerView.prototype.ftblSubLines = null;
/** Field. One of the  {@link tp.DataViewMode} constants
 @protected
 @type {number}
 */
tp.BrokerView.prototype.fViewMode = tp.DataViewMode.None;
/** Field. One of the  {@link tp.DataViewMode} constants
 @protected
 @type {number}
 */
tp.BrokerView.prototype.fLastViewMode = tp.DataViewMode.None;
/** Field
 @protected
 @type {tp.ToolBar}
 */
tp.BrokerView.prototype.ToolBar = null;
/** Field
 @protected
 @type {tp.TabControl}
 */
tp.BrokerView.prototype.MainPager = null;
/** Field. The tab-control where edit part controls are reside
 @protected
 @type {tp.TabControl}
 */
tp.BrokerView.prototype.EditPager = null;
/** Field. 
 @protected
 @type {tp.SqlFilterSelectSqlListUi}
 */
tp.BrokerView.prototype.SelectSqlListUi = null;


/** Field. A bit-field (set) build using the {@link tp.DataViewMode} constants
 @protected
 @type {number}
 */
tp.BrokerView.prototype.ValidCommands = tp.DataViewMode.None;
/** Field
 @protected
 @type {boolean}
 */
tp.BrokerView.prototype.ForceSelect = false;

/** Field
 @protected
 @type {string}
 */
tp.BrokerView.prototype.LastSelectSqlText = '';
/** Field
 @protected
 @type {boolean}
 */
tp.BrokerView.prototype.LastUseRowLimit = true;

/** Field
 @protected
 @type {boolean}
 */
tp.BrokerView.prototype.EditControlsCreated = false;



/* properties */
/**
The primary key field name. Defaults to Id
@public
@type {string}
 */
tp.BrokerView.prototype.PrimaryKeyField = '';

//#endregion