//---------------------------------------------------------------------------------------
// registry
//---------------------------------------------------------------------------------------

tp.Urls.GetRegistryItem = '/App/GetRegistryItem';
tp.Urls.GetRegistryList = '/App/GetRegistryList';

tp.Urls.BrokerInitialize = '/Broker/Initialize';
tp.Urls.BrokerInsert = '/Broker/Insert';
tp.Urls.BrokerEdit = '/Broker/Edit';
tp.Urls.BrokerDelete = '/Broker/Delete';
tp.Urls.BrokerCommit = '/Broker/Commit';
tp.Urls.BrokerSelectBrowser = '/Broker/SelectBrowser'; 

/**
 A registry for keeping descriptors.
@static
*/
tp.Registry = class {
 
    /**
     * Returns a registry item, usually a descriptor, base on a specified item type and name.
     * @private
     * @param {string} ItemType The type of the item, e.g. Locator. Case insensitive
     * @param {string} ItemName The name of the item, e.g. Customer. Case insensitive
     * @returns {object} Returns a registry item, usually a descriptor, base on a specified item type and name.
     */
    static async GetRegistryItemAsync(ItemType, ItemName) { 
        let Result = null;

        switch (ItemType.toLowerCase()) {
            case "locator":
                Result = tp.FirstOrDefault(this.Locators, (item) => { return tp.IsSameText(ItemName, item.Name); });
                break;
        }

        if (tp.IsEmpty(Result)) {

            let Args = new tp.AjaxArgs();
            Args.Url = tp.Urls.GetRegistryItem;
            Args.Data = {
                ItemType: ItemType,
                ItemName: ItemName
            };

            Args = await tp.Ajax.Async(Args);

            var o = JSON.parse(Args.ResponseText);
            if (o.Result === false)
                tp.Throw(o.ErrorText);

            switch (ItemType.toLowerCase()) {
                case "locator":
                    Result = new tp.LocatorDescriptor('');
                    Result.Assign(o.Packet);
                    this.Locators.push(Result);
                    break;
            }
        }

        return Result;

    }

    /**
     * Finds and returns a {@link tp.LocatorDescriptor} locator
     * @param {string} Name The name of the locator
     * @returns {tp.LocatorDescriptor} Finds and returns a {@link tp.LocatorDescriptor} locator.
     */
    static async FindLocatorAsync(Name) {
        return await this.GetRegistryItemAsync("Locator", Name);
    }
};

tp.Registry.Locators = []; // tp.LocatorDescriptor[]


 

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

            case tp.BrokerAction.SelectBrowser:
                this.Args.Url = tp.Urls.BrokerSelectBrowser;
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
    The SELECT of a SelectBrowser
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
tp.BrokerAction.SelectBrowser = 'SelectBrowser';

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
tp.Broker = class extends tp.tpObject {

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

    /* fields */

    /** Field
    @private
    @type {boolean}
    */
    fInitialized;
    /** Field
    @private
    @type {tp.DataView}
    */
    fView;
    /** Field
    @type {tp.DataSet}
    */
    DataSet;
    /** Field
    @type {tp.DataTable}
    */
    tblBrowser;
    /** Field
    @type {string[]}
    */ 
    QueryNames;
    /** Field
    @type {tp.SelectSql[]}
    */
    SelectList;
    /** Field. The last SelectSql statement executed for the browser
    @type {tp.SelectSql}
    */
    LastSelectSql;   
    /** Field
    @type {tp.BrokerAction}
    */
    LastAction;
    /** Field
    @type {string}
    */
    Name;
    /** Field
    @type {boolean}
    */
    IsListBroker;
    /** Field
    @type {boolean}
    */
    IsMasterBroker;
    /** Field
    @type {tp.DataMode}
    */
    State;
    /** Field
    @type {string}
    */
    ConnectionName;
    /** Field
    @type {string}
    */
    MainTableName;
    /** Field
    @type {string}
    */
    LinesTableName;
    /** Field
    @type {string}
    */
    SubLinesTableName;
    /** Field
    @type {number}
    */
    EntityId;
    /** Field
    @type {string}
    */
    EntityName;
    /** Field
    @type {string}
    */
    PrimaryKeyField;
    /** Field
    @type {boolean}
    */
    GuidOids;

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
        this.SelectList = [];

        this.tblBrowser = null;
        this.SelectList = null;
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
    Creates and returns the browser {@link tp.DataTable} data table
    @protected
    @param {object} Source The source object to copy property values from.
    @returns {tp.DataTable} Returns the browser {@link tp.DataTable} data table
    */
    CreateBrowserTable(Source) {
        this.tblBrowser = new tp.DataTable("Browser");
        this.tblBrowser.Assign(Source);
        return this.tblBrowser;
    }
    /**
    Assigns the select list, that is the array of {@link tp.SelectSql} items.
    @protected
    @param {any[]} SourceList An array of {@link tp.SelectSql} items to copy items from.
    */
    AssignSelectList(SourceList) {
        this.SelectList = [];
        let SS; // tp.SelectSql;
        for (let i = 0, ln = SourceList.length; i < ln; i++) {
            SS = new tp.SelectSql();
            this.SelectList.push(SS);
            SS.Assign(SourceList[i]);
        }
    }
    /**
    Finds and returns a {@link tp.SelectSql} item by name, if any, else null
    @protected
    @param {string} Name The name of the {@link tp.SelectSql} item.
    @returns {tp.SelectSql} Returns a {@link tp.SelectSql} item by name, if any, else null
    */
    FindSelectSql(Name) {
        if (!tp.IsEmpty(this.SelectList) && tp.IsString(Name) && !tp.IsBlank(Name)) {
            Name = tp.Trim(Name);
            if (!tp.StartsWith(Name, "SELECT ")) {
                return tp.FirstOrDefault(this.SelectList, (item) => {
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
            this.AssignSelectList(Source.SelectList);
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
    Executes an action (Insert, Edit, Delete, Commit, SelectBrowser) and retuns a {@link tp.BrokerAction} {@link Promise}.
    @protected
    @param {tp.BrokerAction} Action - An information object regarding the action. It also contains the tp.AjaxArgs of the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async Action(Action) {

        let Args = await tp.Ajax.Async(Action.Args);

        if (Args.ResponseData.IsSuccess === false) {
            let ErrorText = 'Action failed. Unknown error.';
            if (tp.IsString(Args.ResponseData.ErrorText) && !tp.IsBlank(Args.ResponseData.ErrorText)) {
                ErrorText = Args.ResponseData.ErrorText;
            }
            tp.Throw(ErrorText);
        }

        Action.Args = Args; 

        if (Action.AssignFlag === true) {
            this.Clear();
            this.Assign(Args.ResponseData.Packet);
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
    DoSelectBrowserBefore(Action) {
    }
    /**
    Do function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async DoSelectBrowser(Action) {
        return this.Action(Action);
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoSelectBrowserAfter(Action) {
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

        // delete row from browser table
        if (Action.Succeeded === true) {
            if (this.tblBrowser instanceof tp.DataTable) {
                let Row = this.tblBrowser.FindRow(this.PrimaryKeyField, Id);
                if (!tp.IsEmpty(Row)) {
                    this.tblBrowser.RemoveRow(Row);
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
    Executes a SELECT for the browser grid. Initializes the broker if needed. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {string | tp.SelectSql} SelectSql - The SELECT statement to execute
    @param {number} [RowLimit] - Optional. The row limit
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async SelectBrowser(SelectSql, RowLimit = null) {
        if (this.fInitialized === false) {
            await this.Initialize();
        }

        this.LastSelectSql = null;
 
        let Action = new tp.BrokerAction(this, tp.BrokerAction.SelectBrowser);
        Action.Args.Data.BrokerName = this.Name;       
        Action.Args.Data.SqlText = SelectSql.Text;
        Action.Args.Data.RowLimit = RowLimit || -1;     

        Action.SelectSql = tp.IsString(SelectSql) ? new tp.SelectSql(SelectSql) : SelectSql;

        this.DoSelectBrowserBefore(Action);
        Action = await this.DoSelectBrowser(Action);

        if (Action.Succeeded === true) {
            if (!tp.IsEmpty(Action) && !tp.IsEmpty(Action.Args.ResponseData.Packet)) {
                let Packet = Action.Args.ResponseData.Packet;
                this.CreateBrowserTable(Packet);

                this.LastSelectSql = Action.SelectSql;
                this.LastSelectSql.SetupTable(this.tblBrowser);

                Action.SelectSql.Table = this.tblBrowser;
            }
        }

        this.DoSelectBrowserAfter(Action);

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

//#endregion

//#region tp.DataViewCommands

/**
Standard commands
*/
tp.DataViewCommands = {
    Home: 'Home',

    List: 'List',
    Filter: 'Filter',

    First: 'First',
    Prior: 'Prior',
    Next: 'Next',
    Last: 'Last',

    Edit: 'Edit',
    Insert: 'Insert',
    Delete: 'Delete',
    Save: 'Save',
    Cancel: 'Cancel',

    Close: 'Close' 
};
Object.freeze(tp.DataViewCommands);
//#endregion

//#region tp.DataView
/**
A data view represents a control container that automatically can create its controls from the provided markup
and bind to data provided by data-tables of a broker. <br />
Example markup
<pre>
    <div class="tp-View" data-setup="{ ClassType: tp.DataView, BrokerName: 'Customer' }"> ... </div>
    or
    <div class="tp-View" data-setup="{ ClassType: tp.DataView, BrokerClass: tp.App.CustomerBroker }"> ... </div>
    or
    <div class="tp-View" data-setup="{ ClassType: tp.DataView, BrokerName: 'Customer', BrokerClass: tp.App.CustomerBroker }"> ... </div>
</pre>
*/
tp.DataView = class extends tp.View {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <div class="tp-View" data-setup="{ ClassType: tp.DataView, BrokerName: 'Customer' }">
            ...
        </div>

        or

        <div class="tp-View" data-setup="{ ClassType: tp.DataView, BrokerClass: tp.App.CustomerBroker }">
            ...
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* fields */
    /** Field
     @protected
     @type {tp.DataSource[]}
     */
    DataSources = [];
    /** Field
     @protected
     @type {tp.Broker}
     */
    Broker = null;
    /** Field
     @protected
     @type {tp.DataSource}
     */
    dsBrowser = null;
    /** Field
     @protected
     @type {tp.Grid}
     */
    gridBrowser = null;
    /** Field
     @protected
     @type {string}
     */
    BrokerName = '';
    /** Field
     @protected
     @type {tp.DataTable}
     */
    ftblItem = null;
    /** Field
     @protected
     @type {tp.DataTable}
     */
    ftblLines = null;
    /** Field
     @protected
     @type {tp.DataTable}
     */
    ftblSubLines = null;
    /** Field
     @protected
     @type {tp.DataMode}
     */
    fDataMode = tp.DataMode.None;
    /** Field
     @protected
     @type {tp.DataMode}
     */
    fLastDataMode = tp.DataMode.None;
    /** Field
     @protected
     @type {tp.ToolBar}
     */
    ToolBar = null;
    /** Field
     @protected
     @type {tp.PanelList}
     */
    PanelList = null;
    /** Field
     @protected
     @type {string[]}
     */
    ValidCommands = [];
    /** Field
     @protected
     @type {boolean}
     */
    ForceSelect = false;

    /** Field
     @protected
     @type {boolean}
     */
    EditControlsCreated = false;
    /** Field
     @protected
     @type {boolean}
     */
    FilterControlsCreated = false;


    /* properties */
    /**
    The primary key field name. Defaults to Id
    @public
    @type {string}
     */
    PrimaryKeyField = '';



    /**
    Gets or sets the data mode. One of the {@link tp.DataMode} constants.
    @type {tp.DataMode}
    */
    get DataMode() {
        return this.fDataMode;
    }
    set DataMode(v) {
        if (this.fDataMode !== v) {
            this.fLastDataMode = this.fDataMode;
            this.fDataMode = v;
            this.OnDataModeChanged();
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

        this.tpClass = 'tp.DataView';
        this.fDefaultCssClasses = tp.Classes.View + ' ' + tp.Classes.DataView;
    }
    /**
    Initializes fields and properties just before applying the create params.      
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        this.fName = tp.NextName('DataView');
        this.DataSources = [];
        this.Broker = null;

        this.fDataMode = tp.DataMode.None;
        this.fLastDataMode = tp.DataMode.None;

        this.PrimaryKeyField = 'Id';
        this.ForceSelect = false;
    }
 
    /**
    * This should be called OUTSIDE of any constructor, after the constructor finishes. <br />
    * 
    * WARNING
    * 
    * In javascript fields contained in a derived class declaration, are initialized after the base constructor finishes.
    * So, if the base constructor calls a virtual method where the derived class initializes a field,
    * when the constructor finishes, the field is assigned again whatever value the class declaration dictates.
    *
    * A = class {
    *     constructor() {
    *         this.SetFields();
    *     }
    *
    *     SetFields() { }
    * }
    *
    * B = class extends A {
    *     constructor() {
    *         super();
    *         this.Display();
    *     }
    *
    *     Field2 = 123;	// this is assigned again after the SetFields() call, when the constructor completes.
    *
    *     SetFields() {
    *         super.SetFields();
    *         this.Field2 = 987;
    *     }
    *     Display() {
    *         alert(this.Field2);
    *     }
    * }
    *
    * The result is that the field retains the value it had in the class declaration, the null value.
    *
    * 
    * */
    OnAfterConstruction() {
        this.InitializeView();
    }

    /* overridables */

    /** This is called after the base class initialization completes and creates just the tool-bar, the main panel-list and the browser grid. <br />
     * It also creates the broker. <br /> 
     * NOTE: Controls of the edit part are created and bound the first time an insert or edit is requested.
     * @protected
     * @override
    */
    InitializeView() {
        this.CreateToolBar();
        this.CreatePanelList();
        this.CreateBrowserGrid();

        this.CreateBroker();

        if (!tp.IsEmpty(this.Broker)) {
            this.Broker.fView = this;
            this.BrokerInitialize();
        }
    }
    /** Returns a DOM Element contained by this view.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewToolBarElement() { return tp.Select(this.Handle, '.ViewToolBar'); }
    /** Returns a DOM Element contained by this view. Returns the main panel-list which in turn contains the three part panels: Brower, Edit and Filters.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewPanelListElement() { return tp.Select(this.Handle, '.ViewPanelList'); }
    /** Returns a DOM Element contained by this view. Returns the Browser Panel, the container of the browser grid, which displays the results of the various SELECTs of the broker.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewBrowserPanelElement() { return tp.Select(this.Handle, '.ViewBrowserPanel'); }
    /** Returns a DOM Element contained by this view. Returns the Edit Panel, which is the container for all edit controls bound to broker datasources.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewEditPanelElement() { return tp.Select(this.Handle, '.ViewEditPanel'); }
    /** Returns a DOM Element contained by this view. Returns the Filters panel, the container of the filter controls.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewFilterPanelElement() { return tp.Select(this.Handle, '.ViewFilterPanel'); }

    /** Returns a DOM Element contained by this view. Returns the element upon to create the browser grid.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewBrowserGridElement() { return tp.Select(this.Handle, '.ViewBrowserGrid'); }

    /** Creates the toolbar of the view 
     @protected
     */
    CreateToolBar() {
        if (tp.IsEmpty(this.ToolBar)) {
            let el = this.GetViewToolBarElement();
            this.ToolBar = new tp.ToolBar(el);
            this.ToolBar.On('ButtonClick', this.AnyClick, this);
        }
    }
    /** Creates the panel-list of the view, the one with the 3 panels: Browser, Edit and Filters panels.
     @protected
     */
    CreatePanelList() {
        if (tp.IsEmpty(this.PanelList)) {
            let el = this.GetViewPanelListElement();
            this.PanelList = new tp.PanelList(el);
        }        
    }
    /**
    Creates the browser grid. <br />
    NOTE: For the browser grid to be created automatically by this method, a div marked with the ViewBrowserGrid is required.
    @protected
    */
    CreateBrowserGrid() {
        if (tp.IsEmpty(this.gridBrowser)) {
            let el = this.GetViewBrowserGridElement();
            this.gridBrowser = new tp.Grid(el);
        }

        let o = this.FindControlByCssClass(tp.Classes.ViewBrowserGrid);
        this.gridBrowser = o instanceof tp.Grid ? o : null;

        if (this.gridBrowser) {
            this.gridBrowser.ReadOnly = true;
            this.gridBrowser.AllowUserToAddRows = false;
            this.gridBrowser.AllowUserToDeleteRows = false;
            this.gridBrowser.ToolBarVisible = false;

            this.gridBrowser.On(tp.Events.DoubleClick, this.BrowserGrid_DoubleClick, this);
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

    /** Creates and binds the controls of the edit part, if not already created.
     * */
    CreateEditControls() {
        if (!this.EditControlsCreated) {
            let el = this.GetViewEditPanelElement();
            let ControlList = tp.Ui.CreateControls(el);
            this.BindControls(ControlList);
            this.EditControlsCreated = true;
        }
    }
    /** Creates the controls of the filter part, if not already created. 
     * */
    CreateFilterControls() {
        if (!this.FilterControlsCreated) {
            // TODO: create filter controls
            this.FilterControlsCreated = true;
        }
    }

    /**
    Returns a control belonging to this instance and bound to a specified field, if any, else null
    @protected
    @param {string | tp.DataColumn} v - The field name or the {@link tp.Control} Column
    @returns {tp.Control} Returns the {@link tp.Control} control if found, or null.
    */
    FindControlByDataField(v) {
        let Control; // tp.Control
        let List = this.GetControls();
        let FieldName = (v instanceof tp.DataColumn) ? v.Name : v;

        for (var i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof tp.Control) {
                Control = List[i];
                if (tp.IsSameText(FieldName, Control.DataField))
                    return Control;
            }
        }
        return null;
    }
    /**
    Finds and returns a {@link tp.DataSource} data-source by name, if any, else null.
    @protected
    @param {string} SourceName The data-source by name
    @returns {tp.DataSource} Returns a {@link tp.DataSource} data-source or null
    */
    GetDataSource(SourceName) {
        if (!tp.IsEmpty(this.Broker)) {
            if (tp.IsSameText('Item', SourceName) || tp.IsBlank(SourceName)) {
                SourceName = this.Broker.MainTableName;
            } else if (tp.IsSameText('Lines', SourceName)) {
                SourceName = this.Broker.LinesTableName;
            } else if (tp.IsSameText('SubLines', SourceName)) {
                SourceName = this.Broker.SubLinesTableName;
            }
        }

        var Table, DataSource;
        var Result = tp.FirstOrDefault(this.DataSources, (item) => {
            return tp.IsSameText(SourceName, item.Name);
        });

        if (tp.IsEmpty(Result) && !tp.IsEmpty(this.Broker)) {
            Table = this.Broker.DataSet.FindTable(SourceName);
            if (!tp.IsEmpty(Table)) {
                DataSource = new tp.DataSource(Table);
                this.DataSources.push(DataSource);
                return DataSource;
            }
        }

        return Result;
    }
    /**
    Returns the index of a data-source, if any, else -1.
    @protected
    @param {string} SourceName The data-source name
    @returns {number} Returns the index of a data-source, if any, else -1.
    */
    IndexOfDataSource(SourceName) {
        for (var i = 0, ln = this.DataSources.length; i < ln; i++) {
            if (tp.IsSameText(SourceName, this.DataSources[i].Name)) {
                return i;
            }
        }

        return -1;
    }

 

    /**
     * Binds controls of the Edit part. <br />
     * NOTE:  Do NOT call this method directly. <br />
     * This method is called automatically the first time an an Insert() or Edit() is requested.
     * @param {tp.tpElement[]} ControlList The list of controls to bind
     * @protected
     */
    BindControls(ControlList) {

        var i, ln, Control;
 
        for (i = 0, ln = ControlList.length; i < ln; i++) {
            Control = ControlList[i];
            if (this.CanBindControl(Control))
                this.BindControl(Control);
        }
    }
    /**
    Returns true if a specified control can be bound to data
    @protected
    @param {tp.Control} Control A {@link tp.Control}
    @returns {boolean} Returns true if a specified control can be bound to data
    */
    CanBindControl(Control) {
        if (!tp.IsEmpty(Control) && (Control instanceof tp.Control) && (Control.DataBindMode !== tp.ControlBindMode.None) && !Control.IsDataBound) {
            switch (Control.DataBindMode) {
                case tp.ControlBindMode.Simple:
                    return !tp.IsBlank(Control.DataField);
                case tp.ControlBindMode.List:
                    return !tp.IsBlank(Control.DataField);
                case tp.ControlBindMode.Grid:
                    return !tp.IsBlank(Control.SourceName) && !tp.HasClass(Control.Handle, tp.Classes.ViewBrowserGrid);
            }
        }

        return false;
    }
    /**
    Binds a specified control to data
    @protected
    @param {tp.Control} Control A {@link tp.Control}
    */
    BindControl(Control) { 
        Control.DataSource = this.GetDataSource(Control.SourceName);
 
        if ((Control.DataBindMode === tp.ControlBindMode.List) && tp.IsEmpty(Control['ListSource']) && !tp.IsEmpty(Control.DataColumn)) {
            let Column = Control.DataColumn;
            if (!tp.IsBlank(Control['ListSourceName'])) {
                var ListSource = this.GetDataSource(Control['ListSourceName']);
                Control['ListSource'] = ListSource;
            }
        }

        this.SetupControl(Control);
    }
    /**
    Sets up a control after data-binding
    @protected
    @param {tp.Control} Control A {@link tp.Control}
    */
    SetupControl(Control) {
        var i, ln,
            CtrlRow,            // HTMLElement
            Column,             // tp.DataColumn,
            Grid,               // tp.Grid,
            Table,              // tp.DataTable,
            ReadOnlyColumns,    // string[],
            VisibleColumns      // string[]
            ;

        if ((Control.DataBindMode === tp.ControlBindMode.Simple) || (Control.DataBindMode === tp.ControlBindMode.List)) {
            Column = Control.DataColumn;
            if (!tp.IsEmpty(Column)) {
                if ('ReadOnly' in Control) {
                    Control.ReadOnly = Column.ReadOnly || Column.IsReadOnly || Column.IsReadOnlyUI;
                }
                if ('MaxLength' in Control) {
                    if (Column.MaxLength !== -1) {
                        Control['MaxLength'] = Column.MaxLength;
                    }
                }
                if ((Column.IsRequired === true) && ('Required' in Control)) {
                    Control.Required = false;
                    Control.Required = true;
                }

                if (Control instanceof tp.CheckBox) {
                    Control.Text = Column.Title;
                } else if (tp.IsElement(Control.elText)) {
                    tp.val(Control.elText, Column.Title);
                }

                if (!Column.IsVisible) {
                    CtrlRow = tp.Ui.GetCtrlRow(Control.Handle);
                    if (tp.IsHTMLElement(CtrlRow))
                        tp.Visible(CtrlRow, false);
                }
            }
        } else if (Control.DataBindMode === tp.ControlBindMode.Grid) {
            Grid = Control;
            Table = Grid.DataSource.Table;

            ReadOnlyColumns = [];
            VisibleColumns = [];
            for (i = 0, ln = Table.Columns.length; i < ln; i++) {
                Column = Table.Columns[i];
                if (Column.IsVisible)
                    VisibleColumns.push(Column.Name);
                if (Column.IsReadOnly || Column.IsReadOnlyUI)
                    ReadOnlyColumns.push(Column.Name);
            }

            Grid.SetColumnListReadOnly(ReadOnlyColumns);
            Grid.SetColumnListVisible(VisibleColumns);
        }
    }
    /**
    Sets or un-sets the read-only flag to this view controls
    @protected
    @param {boolean} Flag A flag
    */
    ApplyReadOnlyEditToControls(Flag) {
        var i, ln,
            Control;    // tp.Control;
        var List = this.GetControls();

        for (i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof tp.Control) {
                Control = List[i];
                if (('DataColumn' in Control) && (Control.DataColumn instanceof tp.DataColumn) && (Control.DataColumn.IsReadOnlyEdit)) {
                    Control.ReadOnly = Flag === true;
                }
            }
        }
    }
    /**
    Calls the Update() method of all data-sources of this view
    @protected
    */
    UpdateDataSources() {
        for (var i = 0, ln = this.DataSources.length; i < ln; i++)
            this.DataSources[i].Update();
    }
    /**
    Sets the data-source of the browser grid
    @protected
    */
    UpdateBrowserDataSource() {
        if (!tp.IsEmpty(this.Broker)) {
            this.dsBrowser = new tp.DataSource(this.Broker.tblBrowser);
        }

        if (!tp.IsEmpty(this.dsBrowser) && !tp.IsEmpty(this.gridBrowser)) {
            this.gridBrowser.DataSource = this.dsBrowser;
        }
    }


    /**
    Returns the Id (value of the primary key field) of the selected data-row of the browser grid, if any, else null.
    @protected
    @returns {any} Returns the Id (value of the primary key field) of the selected data-row of the browser grid, if any, else null.
    */
    GetBrowserSelectedId() {
        if (tp.IsBlank(this.PrimaryKeyField)) {
            if (this.tblItem instanceof tp.DataTable) {
                this.PrimaryKeyField = this.tblItem.PrimaryKeyField;
            }
        }

        if (!tp.IsBlank(this.PrimaryKeyField) && !tp.IsEmpty(this.gridBrowser)) {
            var Row = this.gridBrowser.FocusedRow;
            if (!tp.IsEmpty(Row) && Row.Table.ContainsColumn(this.PrimaryKeyField)) {
                return Row.Get(this.PrimaryKeyField);
            }
        }

        return null;
    }
    /**
    Executes the start command. Called by the DoInitializeAfter(). <br />
    Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise} if broker is already initialized. <br />
    @protected
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise} or a null {@link Promise} if broker is already initialized. <br />
    */
    async ExecuteStartCmd() {
        if (!tp.IsEmpty(this.gridBrowser) && !tp.IsEmpty(this.Broker) && this.Broker.Initialized)
            await this.BrowserSelect();
        return null;
    }
    /**
    Returns a string array with all standard commands
    @protected
    @returns {string[]} Returns a string array with all standard commands
    */
    GetStandardCommandList() {
        let List = [];
        for (let Prop in tp.DataViewCommands) {
            if (tp.IsString(tp.DataViewCommands[Prop])) {
                List.push(tp.DataViewCommands[Prop]);
            }
        }
        return List;
    }
    /**
    Validates standard commands, that is decides which is or not valid at the moment of the call.
    @protected
    */
    ValidateCommands() {

        let DisableList = [];  // string[]

        switch (this.DataMode) {
            case tp.DataMode.Browse:
                DisableList = [
                    tp.DataViewCommands.List,
                    tp.DataViewCommands.First,
                    tp.DataViewCommands.Prior,
                    tp.DataViewCommands.Next,
                    tp.DataViewCommands.Last,
                    tp.DataViewCommands.Save,
                    tp.DataViewCommands.Cancel];
                break;

            case tp.DataMode.Insert:
                DisableList = [
                    tp.DataViewCommands.Filter,
                    tp.DataViewCommands.Insert,
                    tp.DataViewCommands.Edit,
                    tp.DataViewCommands.Delete                    
                ];
                break;
            case tp.DataMode.Edit:
                DisableList = [
                    tp.DataViewCommands.Filter,
                    tp.DataViewCommands.Edit
                ];
                break;
            case tp.DataMode.Delete:
                //
                break;

            case tp.DataMode.Cancel:

                break;

            case tp.DataMode.Commit:
                //
                break;
        }



        this.ValidCommands = this.GetStandardCommandList();

        for (let i = 0, ln = DisableList.length; i < ln; i++) {
            tp.ListRemove(this.ValidCommands, DisableList[i]);
        }


        if (this.dsBrowser) {
            if (!this.dsBrowser.CanFirst())
                tp.ListRemove(this.ValidCommands, tp.DataViewCommands.First);
            if (!this.dsBrowser.CanPrior())
                tp.ListRemove(this.ValidCommands, tp.DataViewCommands.Prior);
            if (!this.dsBrowser.CanNext())
                tp.ListRemove(this.ValidCommands, tp.DataViewCommands.Next);
            if (!this.dsBrowser.CanLast())
                tp.ListRemove(this.ValidCommands, tp.DataViewCommands.Last);
        }
    }
    /**
    Enables/disables buttons and menu items.
    @protected
    */
    EnableCommands() {
        if (tp.IsArray(this.ValidCommands)) {
            if (this.ToolBar) {

                let ControlList = this.ToolBar.GetControls(),
                    c,          // tp.tpElement,
                    Command     // string
                    ;

                for (let i = 0, ln = ControlList.length; i < ln; i++) {
                    c = ControlList[i];
                    if (tp.HasCommandProperty(c)) {
                        Command = c.Command;
                        if (!tp.IsBlank(Command)) {
                            c.Enabled = tp.ListContainsText(this.ValidCommands, Command);
                        }
                    }
                }
            }
        }

    }
    /**
    Sets the visible panel index in the panel list.
    @protected
    @param {number} PanelIndex The panel index
    */
    SetVisiblePanel(PanelIndex) {
        if (this.PanelList) {
            this.PanelList.SelectedIndex = PanelIndex;
        }
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
    }
    /**
    Closes the view and removes the view from the DOM.
    @protected
    */
    CloseView() {
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
    DoSelectBrowserBefore() {
    }
    /**
    After function
    @protected
    @param {tp.BrokerAction} Action - An {@link tp.BrokerAction} information object regarding the action.
    */
    DoSelectBrowserAfter(Action) {
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

                    this.DataMode = tp.DataMode.Browse;
                    this.ValidateCommands();
                    this.EnableCommands();

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

                    this.DataMode = tp.DataMode.Insert;
                    this.DoInsertAfter(Action);

                    this.ValidateCommands();
                    this.EnableCommands();
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

        Id = Id || this.GetBrowserSelectedId();

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

                    this.DataMode = tp.DataMode.Edit;
                    this.ValidateCommands();
                    this.EnableCommands();
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

        Id = Id || this.GetBrowserSelectedId();

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

                        this.DataMode = tp.DataMode.Browse;
                        this.ValidateCommands();
                        this.EnableCommands();
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

                    this.DataMode = tp.DataMode.Edit;
                    this.ValidateCommands();
                    this.EnableCommands();
                } else {
                    this.DisplayActionError(Action);
                }
            } 
        }

        return Action;
    }
    /**
    Executes a SELECT for the browser grid. Retuns a {@link tp.BrokerAction} {@link Promise}.
    @param {string | tp.SelectSql} [SelectSql] - Optional. The SELECT statement to execute
    @param {number} [RowLimit] - Optional. The row limit
    @returns {tp.BrokerAction} Retuns a {@link tp.BrokerAction} {@link Promise}.
    */
    async BrowserSelect(SelectSql = null, RowLimit = null) {
        let Action = null;

        if (!tp.IsEmpty(this.Broker)) {

            if (tp.IsString(SelectSql)) {
                SelectSql = new tp.SelectSql(SelectSql);
            } else if (tp.IsEmpty(SelectSql) && this.Broker.SelectList.length > 0) {
                SelectSql = this.Broker.SelectList[0];
            }

            if (tp.IsEmpty(SelectSql) && !tp.IsEmpty(this.gridBrowser) && !tp.IsEmpty(this.gridBrowser.DataSource)) {
                this.DataMode = tp.DataMode.Browse;
                this.ValidateCommands();
                this.EnableCommands();
            } else {
                this.DoSelectBrowserBefore();

                Action = await this.Broker.SelectBrowser(SelectSql, RowLimit);

                if (Action instanceof tp.BrokerAction) {
                    if (Action.Succeeded === true) {
                        this.UpdateBrowserDataSource();

                        if (this.dsBrowser) {
                            if (this.dsBrowser.CanFirst())
                                this.dsBrowser.First();
                            if (this.dsBrowser.Rows.length > 0 && this.gridBrowser) {
                                this.gridBrowser.FocusedRow = this.dsBrowser.Rows[0];
                            }
                        }

                        this.DoSelectBrowserAfter(Action);
                        this.ForceSelect = false;

                        this.DataMode = tp.DataMode.Browse;
                        this.ValidateCommands();
                        this.EnableCommands();
                    } else {
                        this.DisplayActionError(Action);
                    }
                }
            }
        } 

        return Action;
    }

    /* miscs */
    /**
    Executes a standard command by name
    @param {string} Command - One of the {@link tp.DataViewCommands} constants
    */
    ExecuteCommand(Command) {
        if (tp.IsBlank(Command))
            return;

        switch (Command) {
            case tp.DataViewCommands.Home:
                this.DisplayHomeLocalMenu();
                break;

            case tp.DataViewCommands.List:
                this.BrowserSelect();
                break;
            case tp.DataViewCommands.Filter:
                this.DisplayFilterPanel();
                break;

            case tp.DataViewCommands.First:
                if (this.dsBrowser && this.dsBrowser.CanFirst()) {
                    this.dsBrowser.First();
                    this.Edit();
                }
                break;
            case tp.DataViewCommands.Prior:
                if (this.dsBrowser && this.dsBrowser.CanPrior()) {
                    this.dsBrowser.Prior();
                    this.Edit();
                }
                break;
            case tp.DataViewCommands.Next:
                if (this.dsBrowser && this.dsBrowser.CanNext()) {
                    this.dsBrowser.Next();
                    this.Edit();
                }
                break;
            case tp.DataViewCommands.Last:
                if (this.dsBrowser && this.dsBrowser.CanLast()) {
                    this.dsBrowser.Last();
                    this.Edit();
                }
                break;


            case tp.DataViewCommands.Edit:
                this.Edit();
                break;
            case tp.DataViewCommands.Insert:
                this.Insert();
                break;
            case tp.DataViewCommands.Delete:
                this.Delete();
                break;
            case tp.DataViewCommands.Save:
                this.Commit();
                break;
            case tp.DataViewCommands.Cancel:
                if (this.ForceSelect !== true) {
                    this.DataMode = tp.DataMode.Cancel;
                    this.DataMode = tp.DataMode.Browse;
                } else {
                    this.BrowserSelect();
                }

                break;

            case tp.DataViewCommands.Close:
                this.CloseView();
                break;
        }
    }

    /* Event triggers */
    /**
    Event trigger
    */
    OnDataModeChanged() {
        switch (this.DataMode) {
            case tp.DataMode.Browse:
                this.SetVisiblePanel(0);
                break;

            case tp.DataMode.Insert:
                this.SetVisiblePanel(1);
                break;
            case tp.DataMode.Edit:
                this.SetVisiblePanel(1);
                break;
            case tp.DataMode.Delete:
                //
                break;

            case tp.DataMode.Cancel:
                this.SetVisiblePanel(0);
                break;

            case tp.DataMode.Commit:
                //
                break;
        }

        this.ValidateCommands();
        this.EnableCommands();

        this.Trigger('DataModeChanged', {});
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
    BrowserGrid_DoubleClick(Args) {
        this.ExecuteCommand(tp.DataViewCommands.Edit);
    }
};



//#endregion