tp.Urls.SysDataSelectList = '/SysData/SelectList';
tp.Urls.SysDataSelectItemById = '/SysData/SelectItemById';
tp.Urls.SysDataSaveItem = '/SysData/SaveItem';

tp.Urls.LocatorGetDef = '/Locator/GetDef';
tp.Urls.LocatorSqlSelect = '/Locator/SqlSelect';


//#region SysDataItem

/** 
 * Represents the data of a row in the 'system data' table.
 * System data table is a database table which stores information 
 * regarding system data, such as reports, descriptors, resources etc.
 */
tp.SysDataItem = class extends tp.Object {
    /** Constructor.
     * Assigns this instance's properties from a specified source, if not null.
     * @param {objec} Source Optional.
     */
    constructor(Source = null) {
        super();

        this.fTitle = '';
        this.fTitleKey = '';

        this.Assign(Source);
    }

    /** Title (caption) of this instance, used for display purposes.
     * @type {string}
     */
    get Title() {
        return !tp.IsBlankString(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        this.fTitle = v;
    }
    /** A resource Key used in returning a localized version of Title
     * @type {string}
     */
    get TitleKey() {
        return !tp.IsBlankString(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }


    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.DataType = Source.DataType || '';
        this.DataName = Source.DataName || '';

        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        this.Notes = Source.Notes || '';
        this.Owner = Source.Owner || '';        

        this.Tag1 = Source.Tag1 || '';
        this.Tag2 = Source.Tag2 || '';
        this.Tag3 = Source.Tag3 || '';
        this.Tag4 = Source.Tag4 || '';

        this.Data1 = Source.Data1 || '';
        this.Data2 = Source.Data2 || '';
        this.Data3 = Source.Data3 || '';
        this.Data4 = Source.Data4 || '';
    }

    /** Assigns this instance's properties from a specified {@link tp.DataRow}.
     * @param {tp.DataRow} Row The row to load from.
     */
    FromDataRow(Row) {
        this.DataName = Row.Get('DataName', '');
        this.DataType = Row.Get('DataType', '');

        this.Title = Row.Get('Title', '');
        this.TitleKey = Row.Get('TitleKey', '');

        this.Notes = Row.Get('Notes', '');
        this.Owner = Row.Get('Owner', '');

        this.Tag1 = Row.Get('Tag1', '');
        this.Tag2 = Row.Get('Tag2', '');
        this.Tag3 = Row.Get('Tag3', '');
        this.Tag4 = Row.Get('Tag4', '');

        this.Data1 = Row.Get('Data1', '');
        this.Data2 = Row.Get('Data2', '');
        this.Data3 = Row.Get('Data3', '');
        this.Data4 = Row.Get('Data4', '');
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}.
     * @param {tp.DataRow} Row The row to save to.
     */
    ToDataRow(Row) {
        Row.Set('DataName', this.DataName);
        Row.Set('DataType', this.DataType);
   
        Row.Set('Title', this.Title);
        Row.Set('TitleKey', this.TitleKey);

        Row.Set('Notes', this.Notes);
        Row.Set('Owner', this.Owner);
  
        Row.Set('Tag1', this.Tag1 );
        Row.Set('Tag2', this.Tag2 );
        Row.Set('Tag3', this.Tag3 );
        Row.Set('Tag4', this.Tag4 );
  
        Row.Set('Data1', this.Data1);
        Row.Set('Data2', this.Data2);
        Row.Set('Data3', this.Data3);
        Row.Set('Data4', this.Data4);
    }

    /** Assigns this instance's properties from the first {@link tp.DataRow} row of a specified {@link tp.DataTable}.
     * @param {tp.DataTable} Table
     */
    FromDataTable(Table) {
        this.FromDataRow(Table.Rows[0]);
    }
    /** Saves this instance's properties to a {@link tp.DataRow} of a specified {@link tp.DataTable}.
     * A boolean parameter controls whether to add a new row or use the first row of the table.
     * @param {tp.DataTable} [Table=null] Optional. If null a new {@link tp.DataTable} is created.
     * @param {boolean} [AddRow=true] Optional. If true, the default, adds a new row and saves this instance to that row. Else uses the first row of the table.
     * @returns {tp.DataTable} Returns the {@link tp.DataTable}
     */
    ToDataTable(Table = null, AddRow = true) {
        if (tp.IsEmpty(Table)) {
            Table = tp.SysDataItem.CreateTable();
            AddRow = true;
        }

        let Row = AddRow === true ? Table.AddEmptyRow(): Table.Rows[0];
        this.ToDataRow(Row);

        return Table;
    }
};

tp.SysDataItem.prototype.fTitle = '';
tp.SysDataItem.prototype.fTitleKey = '';

/** The data type, i.e. Report, Broker, Table, etc.
 * NOTE: The pair DataType + DataName must be unique among all SysData items.
 * @type {string}
 * */
tp.SysDataItem.prototype.DataType = '';
/** The data name, i.e. Report.Customer, Broker.Customer, etc.
 * NOTE: The pair DataType + DataName must be unique among all SysData items.
 * @type {string}
 * */
tp.SysDataItem.prototype.DataName = '';

/** Notes
 * @type {string}
 */
tp.SysDataItem.prototype.Notes = '';
/** A string indicating the owner of the entry in the table.
 * @type {string}
 */
tp.SysDataItem.prototype.Owner = '';

/** A user defined string tag.
 * @type {string}
 */
tp.SysDataItem.prototype.Tag1 = '';
/** A user defined string tag.
 * @type {string}
 */
tp.SysDataItem.prototype.Tag2 = '';
/** A user defined string tag.
 * @type {string}
 */
tp.SysDataItem.prototype.Tag3 = '';
/** A user defined string tag.
 * @type {string}
 */
tp.SysDataItem.prototype.Tag4 = '';

/** Text blob data
 * @type {string}
 */
tp.SysDataItem.prototype.Data1 = '';
/** Text blob data
 * @type {string}
 */
tp.SysDataItem.prototype.Data2 = '';
/** Text blob data
 * @type {string}
 */
tp.SysDataItem.prototype.Data3 = '';
/** Text blob data
 * @type {string}
 */
tp.SysDataItem.prototype.Data4 = '';

tp.SysDataItem.CreateTable = function () {
    let Table = new tp.DataTable('SysData');

    Table.AddColumn('Id', tp.DataType.String, 40);

    Table.AddColumn('DataType', tp.DataType.String, 96);
    Table.AddColumn('DataName', tp.DataType.String, 96);

    Table.AddColumn('TitleKey', tp.DataType.String, 96);
    Table.AddColumn('Notes', tp.DataType.String, 255);

    Table.AddColumn('Owner', tp.DataType.String, 96);

    Table.AddColumn('Tag1', tp.DataType.String, 96);
    Table.AddColumn('Tag2', tp.DataType.String, 96);
    Table.AddColumn('Tag3', tp.DataType.String, 96);
    Table.AddColumn('Tag4', tp.DataType.String, 96);

    Table.AddColumn('Data1', tp.DataType.TextBlob);
    Table.AddColumn('Data2', tp.DataType.TextBlob);
    Table.AddColumn('Data3', tp.DataType.TextBlob);
    Table.AddColumn('Data4', tp.DataType.TextBlob); 

    return Table;
};

//#endregion


//---------------------------------------------------------------------------------------
// Database related Definition classes
//---------------------------------------------------------------------------------------

//#region UniqueConstraintDef

/** For table-wise unique constraints, possibly on multiple fields.
 * */
tp.UniqueConstraintDef = class extends tp.Object {
    /** Constructor.
     * Assigns this instance's properties from a specified source, if not null.
     * @param {objec} Source Optional.
     */
    constructor(Source = null) {
        super();

        this.Name = '';
        this.Assign(Source);
    }
    /** A proper string, e.g. <code>Field1, Field2</code>
     * @type {string}
     */
    get FieldNames() { return this.fFieldNames; }
    set FieldNames(v) {
        if (this.fFieldNames != v) {
            if (!tp.IsBlankString(v) && tp.IsBlankString(this.Name))
                this.Name = "UC_" + tp.GenerateRandomString(tp.SysConfig.DbIdentifierMaxLength - 3);

            this.fFieldNames = v;
        }
    }

    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.FieldNames = Source.FieldNames || '';
        this.Name = Source.Name || '';
    }

};
/** The constraint name
 * @type {string}
 */
tp.UniqueConstraintDef.prototype.Name = '';
/** A proper string, e.g. <code>Field1, Field2</code>
 * @type {string}
 */
tp.UniqueConstraintDef.prototype.fFieldNames = '';

//#endregion

//#region DataFieldDef

/** Database table field definition
 * */
tp.DataFieldDef = class extends tp.Object {

    /** Constructor.
     * Assigns this instance's properties from a specified source, if not null.
     * @param {objec} Source Optional.
     */
    constructor(Source = null) {
        super();

        this.Id = tp.Guid(true);
        this.fTitle = '';
        this.fTitleKey = '';

        this.Assign(Source);
    }

    /** A GUID string
     * @type {string}
     */
    Id = '';
    /** Title (caption) of this instance, used for display purposes.
     * @type {string}
     */
    get Title() {
        return !tp.IsBlankString(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        this.fTitle = v;
    }
    /** A resource Key used in returning a localized version of Title
     * @type {string}
     */
    get TitleKey() {
        return !tp.IsBlankString(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }

    /** When true denotes a field upon which a unique constraint is applied
     * @type {boolean}
     */
    get Unique() { return this.fUnique; }
    set Unique(v) {
        if (this.fUnique != v) {
            if (v === true && tp.IsBlankString(this.UniqueConstraintName))
                this.UniqueConstraintName = "UC_" + tp.GenerateRandomString(tp.SysConfig.DbIdentifierMaxLength - 3);

            this.fUnique = v;
        }
    }
    /** A string of the form <code>TableName.ColumnName</code> for creating a foreign key constraint on this field.
     * @type {string}
     */
    get ForeignKey() { return this.fForeignKey; }
    set ForeignKey(v) {
        if (this.fForeignKey != v) {
            if (!tp.IsBlankString(v) && tp.IsBlankString(this.ForeignKeyConstraintName))
                this.ForeignKeyConstraintName = "FC_" + tp.GenerateRandomString(tp.SysConfig.DbIdentifierMaxLength - 3);

            this.fForeignKey = v;
        }
    }

    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Id = Source.Id || tp.Guid(true);
        this.Name = Source.Name || '';
        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        this.IsPrimaryKey = Source.IsPrimaryKey === true;
        this.DataType = Source.DataType;
        this.Length = Source.Length;
        this.Required = Source.Required === true;
        this.DefaultExpression = Source.DefaultExpression || null;
        this.UniqueConstraintName = Source.UniqueConstraintName || '';
        this.Unique = Source.Unique === true;

        this.ForeignKeyConstraintName = Source.ForeignKeyConstraintName || '';
        this.ForeignKey = Source.ForeignKey || '';
    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {
            this.Id = Row.Get('Id', '');
            this.Name = Row.Get('Name', '');
            this.TitleKey = Row.Get('TitleKey', '');
            this.IsPrimaryKey = Row.Get('IsPrimaryKey', false);
            this.DataType = Row.Get('DataType', tp.DataType.String);
            this.Length = Row.Get('Length', 0);
            this.Required = Row.Get('Required', false);
            this.DefaultExpression = Row.Get('DefaultExpression', null);
            this.UniqueConstraintName = Row.Get('UniqueConstraintName', '');
            this.Unique = Row.Get('Unique', false);
            this.ForeignKeyConstraintName = Row.Get('ForeignKeyConstraintName', '');
            this.ForeignKey = Row.Get('ForeignKey', '');
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {
        Row.Set('Id', this.Id);
        Row.Set('Name', this.Name);
        Row.Set('TitleKey', this.TitleKey);
        Row.Set('IsPrimaryKey', this.IsPrimaryKey);
        Row.Set('DataType', this.DataType);
        Row.Set('Length', this.Length);
        Row.Set('Required', this.Required);
        Row.Set('DefaultExpression', this.DefaultExpression);
        Row.Set('Unique', this.Unique);
        Row.Set('UniqueConstraintName', this.UniqueConstraintName);
        Row.Set('ForeignKey', this.ForeignKey);
        Row.Set('ForeignKeyConstraintName', this.ForeignKeyConstraintName);
    }
    /** Creates and returns a {@link tp.DataTable} used in moving around instances of this class.
     */
    static CreateDataTable() {
        let Table = new tp.DataTable();
        Table.Name = 'Fields';

        Table.AddColumn('Id', tp.DataType.String, 40);
        Table.AddColumn('Name', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);
        Table.AddColumn('TitleKey', tp.DataType.String, 96);
        Table.AddColumn('IsPrimaryKey', tp.DataType.Boolean);
        Table.AddColumn('DataType', tp.DataType.String, 40);
        Table.AddColumn('Length', tp.DataType.Integer);
        Table.AddColumn('Required', tp.DataType.Boolean);
        Table.AddColumn('DefaultExpression', tp.DataType.String, 80);
        Table.AddColumn('Unique', tp.DataType.Boolean);
        Table.AddColumn('UniqueConstraintName', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);
        Table.AddColumn('ForeignKey', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);
        Table.AddColumn('ForeignKeyConstraintName', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);

        return Table;
    }
};

tp.DataFieldDef.prototype.fTitle = '';
tp.DataFieldDef.prototype.fTitleKey = '';

/** A name unique among all instances of this type
 * @type {string}
 */
tp.DataFieldDef.prototype.Name = '';
/** True when the field is a primary key
 * @type {boolean}
 */
tp.DataFieldDef.prototype.IsPrimaryKey = false;
/** The data-type of the field. One of the {@link tp.DataType} constants.
 * @type {string}
 */
tp.DataFieldDef.prototype.DataType = tp.DataType.Unknown;
/** Field length. Applicable to varchar fields only.
 * @type {number}
 */
tp.DataFieldDef.prototype.Length = 0;
/** True when the field is NOT nullable.
 * NOTE: when true then produces 'not null'
 * @type {boolean}
 */
tp.DataFieldDef.prototype.Required = false;
/** The default expression, if any. E.g. 0, or ''. Defaults to null.
 * NOTE:  e.g. produces default 0, or default ''
 * @type {object}
 */
tp.DataFieldDef.prototype.DefaultExpression = null;
/** When true denotes a field upon which a unique constraint is applied
 * @type {boolean}
 */
tp.DataFieldDef.prototype.fUnique = false;
/** The unique constraint name to create when Unique is set to true.
 * @type {string}
 */
tp.DataFieldDef.prototype.UniqueConstraintName = '';
/** A string of the form <code>TableName.ColumnName</code> for creating a foreign key constraint on this field.
 * @type {string}
 */
tp.DataFieldDef.prototype.fForeignKey = '';
/** When not null/empty indicates that this field has a foreign key constraint.
 * @type {string}
 */
tp.DataFieldDef.prototype.ForeignKeyConstraintName = '';

//#endregion

//#region DataTableDef

/** Database table definition
 * */
tp.DataTableDef = class extends tp.Object {

    /** Constructor.
     * Assigns this instance's properties from a specified source, if not null.
     * @param {objec} Source Optional.
     */
    constructor(Source = null) {
        super();

        this.Id = tp.Guid(true);
        this.fTitle = '';
        this.fTitleKey = '';
        this.Fields = [];
        this.UniqueConstraints = [];

        this.Assign(Source);
    }

    /** A GUID string
     * @type {string}
     */
    Id = '';
    /** Title (caption) of this instance, used for display purposes.
     * @type {string}
     */
    get Title() {
        return !tp.IsBlankString(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        this.fTitle = v;
    }
    /** A resource Key used in returning a localized version of Title
     * @type {string}
     */
    get TitleKey() {
        return !tp.IsBlankString(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }
 
    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Id = Source.Id || tp.Guid(true);
        this.Name = Source.Name || '';
        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        if (tp.IsArray(Source.Fields)) {
            this.Fields = [];
            Source.Fields.forEach((item) => {
                let FieldDef = new tp.DataFieldDef(item);
                this.Fields.push(FieldDef);
            });
        }

        if (tp.IsArray(Source.UniqueConstraints)) {
            this.UniqueConstraints = [];
            Source.UniqueConstraints.forEach((item) => {
                let UC = new tp.UniqueConstraintDef(item);
                this.UniqueConstraints.push(UC);
            });
        }
 
    }

    /** Loads this instance's fields from a specified {@link tp.DataTable}
     * @param {tp.DataTable} Table The table to load fields from.
     */
    FieldsFromDataTable(Table) {
        this.Fields.length = 0;
        if (Table instanceof tp.DataTable) {
            Table.Rows.forEach((Row) => {
                let FieldDef = new tp.DataFieldDef();
                this.Fields.push(FieldDef);
                FieldDef.FromDataRow(Row);
            });
        }
    }
    /** Saves this instance's fields to a {@link tp.DataTable} and returns the table.
     * @param {tp.DataTable} [Table=null] Optional. The table to save fields to.
     * @returns {tp.DataTable} Returns the {@link tp.DataTable} table.
     * */
    FieldsToDataTable(Table = null) {
        if (!(Table instanceof tp.DataTable)) {
            Table = tp.DataFieldDef.CreateDataTable();
        }           
 
        this.Fields.forEach((FieldDef) => {
            let Row = Table.AddEmptyRow();
            FieldDef.ToDataRow(Row);
        });        

        return Table;
    }
};



tp.DataTableDef.prototype.fTitle = '';
tp.DataTableDef.prototype.fTitleKey = '';

/** A name unique among all instances of this type
 * @type {string}
 */
tp.DataTableDef.prototype.Name = '';
/** The list of fields
 * @type {tp.DataFieldDef[]}
 */
tp.DataTableDef.prototype.Fields = [];
/** For multi-field unique constraints.
 * Use it when a unique constraint is required on more than a single field adding a proper string, e.g. Field1, Field2
 * @type {string[]}
 */
tp.DataTableDef.prototype.UniqueConstraints = [];







//#endregion


//---------------------------------------------------------------------------------------
// Locator related Definition classes
//---------------------------------------------------------------------------------------

//#region tp.Locators
/** A helper static class for locators */
tp.Locators = class {

    /**
     * Returns a locator definition registered under a specified name, if any, else null/undefined. <br />
     * It first searches the already downloaded definitions and if the requested is not found then calls the server.
     * @param {string} Name The name of the locator definition.
     * @returns {tp.LocatorDef} Returns a locator definition registered under a specified name, if any, else null/undefined.
     */
    static async GetDefAsync(Name) {
        let Result = this.Descriptors.find(item => tp.IsSameText(Name, item.Name));

        if (!tp.IsValid(Result)) {

            let Url = tp.Urls.LocatorGetDef;
            let Data = {
                LocatorName: Name,
            };

            let Args = await tp.Ajax.GetAsync(Url, Data);

            Result = new tp.LocatorDef();
            Result.Assign(Args.Packet);
            this.Descriptors.push(Result);
        }

        return Result;
    }
    /**
     * Executes the SELECT statement of a specified Locator, along with the specified where clause and returns the result {@link tp.DataTable}
     * @param {string} Name The name of the locator definition.
     * @param {string} WhereSql The where clause text to add to the SELECT statement of a specified Locator.
     * @returns {tp.DataTable} Returns the result {@link tp.DataTable}
     */
    static async SqlSelectAsync(Name, WhereSql) {
        let Url = tp.Urls.LocatorSqlSelect;
        let Data = {
            LocatorName: Name,
            WhereSql: WhereSql || ''
        };

        let Args = await tp.Ajax.GetAsync(Url, Data);
        let Table = new tp.DataTable();
        Table.Assign(Args.Packet);

        return Table;
    }
};

/** A list of locator definitions already downloaded from server.
 * @static
 * @type {tp.LocatorDef[]} 
 * */
tp.Locators.Descriptors = [];
//#endregion

//#region tp.LocatorDef

/**
Describes a {@link tp.Locator}. <br />

A locator represents (returns) a single value, but it can handle and display multiple values
in order to help the end user in identifying and locating that single value.  <br />

For example, a TRADES table has a CUSTOMER_ID column, representing that single value, but the user interface
has to display information from the CUSTOMERS table, specifically, the ID, CODE and NAME columns.

The TRADES table is the data table and the CUSTOMER_ID is the DataField field name.
The CUSTOMERS table is the list table, denoted by the ListTableName, and the ID is the ListKeyField field name.

The fields, ID, CODE and NAME, may be described by individual {@link tp.LocatorFieldDef} field items.  <br />

In other words, the TRADES.CUSTOMER_ID value comes from the CUSTOMERS.ID value. <br />
The TRADES is the data-table where the CUSTOMERS is the list table. <br />
A locator provides a filtering mechanism for the user to locate a CUSTOMERS row. <br />
Also a locator displays the CUSTOMERS rows when the filtering returns more than one row. <br />

A locator can be used either as a single-row control, as the {@link tp.LocatorBox}  does, or as a group of
related columns in a {@link tp.Grid}.  <br />

NOTE: A locator of a {@link tp.LocatorBox} type, may or may not define the tp.LocatorFieldDef.DataField field names. <br />
Usually in a case like that, the data table contains just the key field, the LocatorBox.DataField. <br />
A locator of a grid-type must define the names of those fields always and the data table must contain DataColumn columns
on those fields.

NOTE: The SqlText is mandatory.
 * */
tp.LocatorDef = class {

    /** Contructor */
    constructor() {
        this.Fields = [];
    }


    /** Field
    * @private
    * @type {string}
    */
    fTitleKey = '';


    /* properties */
    /**
    Gets or sets the locator descriptor name
    @type {string}
    */
    Name = '';
    /**
    Gets or sets a resource Key used in returning a localized version of Title
    @type {string}
    */
    get TitleKey() {
        return !tp.IsNullOrWhiteSpace(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }
    /**
    The connection name 
    @type {string}
    */
    ConnectionName = tp.SysConfig.DefaultConnection;
    /**
    Gets or sets the name of the list table
    @type {string}
    */
    ListTableName = '';
    /**
    Gets or sets the key field of the list table. The value of this field goes to the DataField
    */
    ListKeyField = 'Id';
    /**
    Gets or sets the zoom command name. A user inteface (form) can use this name to call the command.
    @type {string}
    */
    ZoomCommand = '';
    /**
    The SELECT statement to execute
    @type {string}
    */
    SqlText = '';

    /**
    Indicates whether the locator is readonly, meaning the user cannot select from the drop-down list
    @type {boolean}
    */
    ReadOnly = false;
    /**
    Gets the list of descriptor fields.
    @type {tp.LocatorFieldDef[]}
    */
    Fields = [];

    /**
    Assigns the properties of this instance from a specified source object.
    @param {object} Source The source to copy property values from.
    */
    Assign(Source) {
        if (tp.IsValid(Source)) {
            for (var Prop in Source) {
                if (!tp.IsFunction(Source[Prop]) && tp.HasWritableProperty(this, Prop)) {
                    if (Prop === 'Fields' && tp.IsArray(Source[Prop])) {
                        this.Fields = [];
                        let FieldDef;
                        Source[Prop].forEach((SourceFieldDef) => {
                            FieldDef = new tp.LocatorFieldDef();
                            FieldDef.Assign(SourceFieldDef);
                            this.Fields.push(FieldDef);
                        });
                    }
                    else {
                        this[Prop] = Source[Prop];
                    }
                }
            }
        }

    }
    /** Returns a string list with possible errors in this descriptor. If no errors the array is empty. 
     * @param {string[]} [List=null] Optional. The string list to place the error strings. 
     * @returns {string[]} A string list with error texts or empty.
     */
    GetDescriptorErrors(List = null) {
        List = List || [];

        if (tp.IsNullOrWhiteSpace(this.Name))
            List.push(_L("E_LocatorDef_NameIsEmpty", "LocatorDef: Name is empty"));

        if (tp.IsNullOrWhiteSpace(this.ConnectionName))
            List.push(_L("E_LocatorDef_ConnectionNameIsEmpty", `LocatorDef ${this.Name}: ConnectionName is empty`));

        if (tp.IsNullOrWhiteSpace(this.SqlText))
            List.push(_L("E_LocatorDef_SqlTextIsEmpty", `LocatorDef ${this.Name}: SqlText is empty`));

        if (!tp.IsValid(this.Fields) || this.Fields.Count === 0)
            List.push(_L("E_LocatorDef_NoFields", `LocatorDef ${this.Name}: No fields defined`));

        this.Fields.forEach((item) => { item.GetDescriptorErrors(List); });

        return List;
    }
    /** Throws exception if this instance is not a valid one.
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {
        let List = this.GetDescriptorErrors();
        if (List.length > 0) {
            let S = List.join('\n');
            tp.Throw(S);
        }
    }

    /**
    Finds a {@link tp.LocatorFieldDef}  field descriptor by list field name and returns the field or null if not found
    @param {string} ListField - The name of the ListField.
    @returns {tp.LocatorFieldDef} Finds a {@link tp.LocatorFieldDef}  field descriptor by list field name and returns the field or null if not found
    */
    Find(ListField) {
        return this.Fields.find((item) => { return tp.IsSameText(ListField, item.Name); });
    }
    /**
    Finds a {@link tp.LocatorFieldDef} field descriptor by data field and returns the field or null if not found
    @param {string} DataField - The field name of the field in the target data-source
    @returns {tp.LocatorFieldDef} Finds a {@link tp.LocatorFieldDef} field descriptor by data field and returns the field or null if not found
    */
    FindByDataField(DataField) {
        return this.Fields.find((item) => { return tp.IsSameText(DataField, item.DataField); });
    }

    /** Loads this instance's fields from a specified {@link tp.DataTable}
     * @param {tp.DataTable} Table The table to load fields from.
     */
    FieldsFromDataTable(Table) {
        this.Fields.length = 0;
        if (Table instanceof tp.DataTable) {
            Table.Rows.forEach((Row) => {
                let FieldDef = new tp.LocatorFieldDef();
                this.Fields.push(FieldDef);
                FieldDef.FromDataRow(Row);
            });
        }
    }
    /** Saves this instance's fields to a {@link tp.DataTable} and returns the table.
     * @param {tp.DataTable} [Table=null] Optional. The table to save fields to.
     * @returns {tp.DataTable} Returns the {@link tp.DataTable} table.
     * */
    FieldsToDataTable(Table = null) {
        if (!(Table instanceof tp.DataTable)) {
            Table = tp.LocatorFieldDef.CreateDataTable();
        }

        this.Fields.forEach((FieldDef) => {
            let Row = Table.AddEmptyRow();
            FieldDef.ToDataRow(Row);
        });

        return Table;
    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {

            this.Name = Row.Get('Name', '');
            this.Title = Row.Get('TitleKey', '');
            this.ConnectionName = Row.Get('ConnectionName', '');
            this.ListTableName = Row.Get('ListTableName', '');
            this.ListKeyField = Row.Get('ListKeyField');
            this.ZoomCommand = Row.Get('ZoomCommand', '');
            this.SqlText = Row.Get('SqlText', '');
            this.ReadOnly = Row.Get('ReadOnly', false);
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {

        Row.Set('Name', this.Name);
        Row.Set('TitleKey', this.TitleKey);
        Row.Set('ConnectionName', this.ConnectionName);
        Row.Set('ListTableName', this.ListTableName);
        Row.Set('ListKeyField', this.ListKeyField);
        Row.Set('ZoomCommand', this.ZoomCommand);
        Row.Set('SqlText', this.SqlText);
        Row.Set('ReadOnly', this.ReadOnly);
    }
    /** Creates and returns a {@link tp.DataTable} used in moving around instances of this class.
     */
    static CreateDataTable() {
        let Table = new tp.DataTable();
        Table.Name = 'Locators';

        Table.AddColumn('Name');
        Table.AddColumn('TitleKey');
        Table.AddColumn('ConnectionName').DefaultValue = tp.SysConfig.DefaultConnection;
        Table.AddColumn('ListTableName');
        Table.AddColumn('ListKeyField').DefaultValue = 'Id';
        Table.AddColumn('ZoomCommand');
        Table.AddColumn('SqlText');
        Table.AddColumn('ReadOnly', tp.DataType.Boolean).DefaultValue = false;
        return Table;
    }
};
//#endregion 

//#region tp.LocatorFieldDef
/** Describes the "field" (text box or grid column) of a Locator. <br />
 * A field such that associates a column in the data table (the target) to a column in the list table (the source).
 * */
tp.LocatorFieldDef = class {

    /** Contructor */
    constructor() {
    }

    /**
    The field name in the list (source) table
    @type {string}
    */
    Name = '';
    /**
    The table name of the list (source) table
    @type {string}
    */
    TableName = '';

    /** When not empty/null then it denotes a field in the dest data table where to put the value of this field.
     * @type {string}
     */
    DataField = '';
    /**
    Gets or sets the data type of the field. One of the tp.DataType constants
    @type {string}
    */
    DataType = tp.DataType.String;

    /**
    Gets or sets tha Title of this descriptor, used for display purposes.
    @type {string}
    */
    Title = '';
    /**
    Gets or sets a resource Key used in returning a localized version of Title.
    @type {string}
    */
    get TitleKey() {
        return tp.IsBlank(this.fTitleKey) ? this.Name : this.fTitleKey;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }

    /**
    Indicates whether a TextBox for this field is visible in a LocatorBox
    @type {boolean}
    */
    Visible = true;
    /**
    When true the field can be part in a where clause in a select statement.
    @type {boolean}
    */
    Searchable = true;
    /**
    Indicates whether the field is visible when the list table is displayed
    @type {boolean}
    */
    ListVisible = true

    /**
    Used to notify criterial links to treat the field as an integer boolea fieldn (1 = true, 0 = false)
    @type {boolean}
    */
    IsIntegerBoolean = false;
    /**
    Controls the width of the text box in a LocatorBox. In pixels.
    @type {number}
    */
    Width = 70;

    /**
    Assigns the properties of this instance from a specified source object.
    @param {object} Source The source to copy property values from.
    */
    Assign(Source) {
        if (tp.IsValid(Source)) {
            for (let Prop in Source) {
                if (!tp.IsFunction(Source[Prop]) && tp.HasWritableProperty(this, Prop)) {
                    this[Prop] = Source[Prop];
                }
            }
        }
    }
    /** Returns a string list with possible errors in this descriptor. If no errors the array is empty. 
     * @param {string[]} [List=null] Optional. The string list to place the error strings. 
     * @returns {string[]} A string list with error texts or empty.
     */
    GetDescriptorErrors(List = null) {
        List = List || [];

        if (tp.IsNullOrWhiteSpace(this.Name))
            List.push(_L("E_LocatorFieldDef_NameIsEmpty", "LocatorFieldDef: Name is empty"));

        if (tp.IsNullOrWhiteSpace(this.TableName))
            List.push(_L("E_LocatorFieldDef_TableNameIsEmpty", `LocatorFieldDef ${this.Name}:TableName is empty`));
    }
    /** Throws exception if this instance is not a valid one.
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {
        let List = this.GetDescriptorErrors();
        if (List.length > 0) {
            let S = List.join('\n');
            tp.Throw(S);
        }
    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {

            this.Name = Row.Get('Name', '');
            this.TableName = Row.Get('TableName', '');
            this.DataField = Row.Get('DataField', '');
            this.DataType = Row.Get('DataType', tp.DataType.String);

            this.TitleKey = Row.Get('TitleKey', '');

            this.Visible = Row.Get('Visible', true);
            this.Searchable = Row.Get('Searchable', true);
            this.ListVisible = Row.Get('ListVisible', true);
            this.IsIntegerBoolean = Row.Get('IsIntegerBoolean', false);

            this.Width = Row.Get('Width', 70);
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {

        Row.Set('Name', this.Name);
        Row.Set('TableName', this.TableName);
        Row.Set('DataField', this.DataField);
        Row.Set('DataType', this.DataType);

        Row.Set('TitleKey', this.TitleKey);

        Row.Set('Visible', this.Visible);
        Row.Set('Searchable', this.Searchable);
        Row.Set('ListVisible', this.ListVisible);
        Row.Set('IsIntegerBoolean', this.IsIntegerBoolean);

        Row.Set('Width', this.Width);
    }
    /** Creates and returns a {@link tp.DataTable} used in moving around instances of this class.
     */
    static CreateDataTable() {
        let Table = new tp.DataTable();
        Table.Name = 'Fields';

        Table.AddColumn('Name', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);
        Table.AddColumn('TableName', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);
        Table.AddColumn('DataField', tp.DataType.String, tp.SysConfig.DbIdentifierMaxLength);
        Table.AddColumn('DataType', tp.DataType.String, 40);

        Table.AddColumn('TitleKey', tp.DataType.String, 96);

        Table.AddColumn('Visible', tp.DataType.Boolean);
        Table.AddColumn('Searchable', tp.DataType.Boolean);
        Table.AddColumn('ListVisible', tp.DataType.Boolean);
        Table.AddColumn('IsIntegerBoolean', tp.DataType.Boolean);

        Table.AddColumn('Width', tp.DataType.Integer);
        return Table;
    }
};

tp.LocatorFieldDef.BoxDefaultWidth = 70;
//#endregion

//---------------------------------------------------------------------------------------
// Broker related Definition classes
//---------------------------------------------------------------------------------------

//#region CodeProviderDef

/** Describes the production of a unique Code.
 */
tp.CodeProviderDef = class {

    /** Constructor */
    constructor() {
    }

    /** A unique name for this instance
     * @type {string}
     */
    Name = '';
    /** The definition text, e.g. XXX-XXX
     * @type {string}
     */
    Text = '';
    /** A character that used in separating the parts of the produced Code.
     * @type {string}
     */
    PartSeparator = '-';
    /** The C# class name of the type this descriptor describes.
     * NOTE: The value of this property may be a string returned by the Type.AssemblyQualifiedName property of the type.
     * Otherwise it must be a type name registered to the TypeStore either directly or just by using the TypeStoreItemAttribute attribute.
     * In the case of a type registered with the TypeStore, a safe way is to use a Namespace.TypeName combination both, when registering and when retreiving a type.
     * Regarding types belonging to the various Tripous namespaces, using just the TypeName is enough.
     * Most of the Tripous types are already registered to the TypeStore with just their TypeName.
     * @type {string}
     */
    TypeClassName = 'CodeProvider';


    /** Returns a string list with possible errors in this descriptor. If no errors the array is empty. 
     * @param {string[]} [List=null] Optional. The string list to place the error strings. 
     * @returns {string[]} A string list with error texts or empty.
     */
    GetDescriptorErrors(List = null) {
        List = List || [];

        if (tp.IsNullOrWhiteSpace(this.Name))
            List.push(_L("E_CodeProviderDef_NameIsEmpty", "CodeProviderDef: Name is empty"));

        if (tp.IsNullOrWhiteSpace(this.Text))
            List.push(_L("E_CodeProviderDef_TextIsEmpty", `CodeProviderDef ${this.Name}: Text is empty. Must be something like XXX-XXX`));

        return List;
    }
    /** Throws exception if this instance is not a valid one.
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {
        let List = this.GetDescriptorErrors();
        if (List.length > 0) {
            let S = List.join('\n');
            tp.Throw(S);
        }
    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {

            this.Name = Row.Get('Name', '');
            this.Text = Row.Get('Text', '');
            this.PartSeparator = Row.Get('PartSeparator', '');
            this.TypeClassName = Row.Get('TypeClassName', '');
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {
        Row.Set('Name', this.Name);
        Row.Set('Text', this.Text);
        Row.Set('PartSeparator', this.PartSeparator);
        Row.Set('TypeClassName', this.TypeClassName);
    }
    /** Creates and returns a {@link tp.DataTable} used in moving around instances of this class.
     */
    static CreateDataTable() {
        let Table = new tp.DataTable();
        Table.Name = 'CodeProviders';

        Table.AddColumn('Name');
        Table.AddColumn('Text');
        Table.AddColumn('PartSeparator');
        Table.AddColumn('TypeClassName');
        return Table;
    }

};

//#endregion


//#region SqlBrokerQueryDef

/** Describes a SELECT statement.
 * */
tp.SqlBrokerQueryDef = class {
    /** Constructor */
    constructor() {
        this.FieldTitleKeys = [];
    }

    /** The name
     * @type {string}
     */
    Name = '';
    /** The SELECT statement
     * @type {string}
     */
    SqlText = '';
    /** A string list, where each string  has the format FIELD_NAME=TitleKey. <br />
     * Determines the visibility of the fields in the drop-down grids:
     * if it is empty then all fields are visible
     * else only the included fields are visible
     * @type {string[]}
     */
    FieldTitleKeys = [];

    /** Assigns this instance's properties from a specified source.
    * @param {objec} Source
    */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Name = Source.Name || '';
        this.SqlText = Source.SqlText || '';

        if (tp.IsArray(Source.FieldTitleKeys)) {
            this.FieldTitleKeys = [];
            Source.FieldTitleKeys.forEach((item) => {
                this.FieldTitleKeys.push(item);
            });
        }
    }
    /** Throws exception if this instance is not a valid one. 
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {

        if (tp.IsNullOrWhiteSpace(this.Name))
            tp.Throw(_L("E_SqlBrokerQueryDef_NoName", "SqlBrokerQueryDef must have a Name"));

        if (tp.IsNullOrWhiteSpace(this.SqlText))
            tp.Throw(_L("E_SqlBrokerQueryDef_NoSql", "SqlBrokerQueryDef must have an SQL statement"));

    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {
            let Source = Row.OBJECT;
            this.Assign(Source);
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {
        Row.Set('Name', this.Name);

        Row.OBJECT = this;
    }
    /** Creates and returns a {@link tp.DataTable} used in moving around instances of this class.
     */
    static CreateDataTable() {
        let Table = new tp.DataTable();

        Table.AddColumn('Name').DefaultValue = 'New Item';

        return Table;
    }
};

//#endregion

//#region SqlBrokerFieldDef
tp.SqlBrokerFieldDef = class {

    /** Constructor */
    constructor() {
    }

    /** The field name
     * @type {string}
     */
    Name = '';
    /** The alias of this instance
     * @type {string}
     */
    get Alias() {
        return !tp.IsBlankString(this.fAlias) ? this.fAlias : this.Name;
    }
    set Alias(v) {
        this.fAlias = v;
    }

    /** Title (caption) of this instance, used for display purposes.
     * @type {string}
     */
    get Title() {
        return !tp.IsBlankString(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        // nothing
    }
    /** A resource Key used in returning a localized version of Title
     * @type {string}
     */
    get TitleKey() {
        return !tp.IsBlankString(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }

    /** The data-type of the field. One of the {@link tp.DataType} constants.
     * @type {string}
     */
    DataType = tp.DataType.Unknown;
    /** The max length of a string field
     * @type {number}
     */
    MaxLength = 0;
    /** The decimals of the field. Used when is a float field. -1 means is not set.
     * @type {number}
     */
    Decimals = -1;
    /**
    The flags bit-field.
    @default tp.FieldFlags.None
    @type {tp.FieldFlags}
    */
    Flags = tp.FieldFlags.None;

    /** The name of the code producer descriptor associated to this field.
     * @type {string}
     */
    CodeProviderName = '';
    /** The default value of the field.
     * @type {string}
     */
    DefaultValue = null;
    /** The expression used to calculate the values in a column, or create an aggregate column
     * @type {string}
     */
    Expression = null;



    /** The name of a foreign table this field points to, if any, else null. <br />
     * NOTE: This idea comes from old Tripous versions where it was used with LookUp controls such as ComboBox.
     * For examples of use in UIs check the Tripous2 ControlHandlerStandard class the Bind() method.
     * 
     * Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
     * and a COUNTRY table with ID and NAME fields. To establish a foreign relation
     *     this field          = "COUNTRY_ID";     // CUSTOMER.COUNTRY_ID
     *     LookUpTableName     = "COUNTRY";                           
     *     LookUpKeyField      = "ID";             // COUNTRY.ID         
     *     LookUpFieldList     = "ID;NAME";        // COUNTRY.ID, COUNTRY.NAME 
     * @type {string}
     */
    LookUpTableName = '';
    /** The alias of a foreign table this field points to, if any, else null.
     * @type {string}
     */
    LookUpTableAlias = '';
    /** The name of the field of the foreign table that becomes the result of a look-up operation
     * @type {string}
     */
    LookUpKeyField = '';
    /** A semi-colon separated list of field names, e.g. Id;Name
     * The fields in this list are used in constructing a SELECT statement. <br />
     * NOTE: The LookUpKeyField must be included in this list. <br />
     * NOTE: When this property has a value then the LookUpTableSql is not used.
     * @type {string}
     */
    LookUpFieldList = '';
    /** A SELECT statement to be used instead of the LookUpFieldList. <br />
     * NOTE: The LookUpKeyField must be included in this SELECT statement.
     * @type {string}
     */
    LookUpTableSql = '';

    /** The name of a LocatorDef to be used with this field.
     * @type {string}
     */
    LocatorName = '';

    /** Assigns this instance's properties from a specified source.
    * @param {objec} Source
    */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Name = Source.Name || '';
        this.Alias = Source.Alias || '';

        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        this.DataType = Source.DataType || tp.DataType.Unknown;
        this.MaxLength = Source.MaxLength || 0;
        this.Decimals = Source.Decimals || -1;
        this.Flags = Source.Flags || tp.FieldFlags.None;

        this.CodeProviderName = Source.CodeProviderName || '';
        this.DefaultValue = Source.DefaultValue || '';
        this.Expression = Source.Expression || '';

        this.LookUpTableName = Source.LookUpTableName || '';
        this.LookUpTableAlias = Source.LookUpTableAlias || '';
        this.LookUpKeyField = Source.LookUpKeyField || '';
        this.LookUpFieldList = Source.LookUpFieldList || '';
        this.LookUpTableSql = Source.LookUpTableSql || '';

        this.LocatorName = Source.LocatorName || '';

    }
    /** Throws exception if this instance is not a valid one. 
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {
        if (tp.IsNullOrWhiteSpace(this.Name))
            tp.Throw(_L("E_SqlBrokerFieldDef_NameIsEmpty", "SqlBrokerFieldDef Name is empty"));

        if (tp.IsNullOrWhiteSpace(this.Alias))
            tp.Throw(_L("E_SqlBrokerFieldDef_TextIsEmpty", "SqlBrokerFieldDef Alias  is empty. "));

        if (this.DataType === tp.DataType.Unknown)
            tp.Throw(_L("E_SqlBrokerFieldDef_DataTypeIsEmpty", "SqlBrokerFieldDef DataType is Unknown. "));
    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {
            let Source = Row.OBJECT;
            this.Assign(Source);
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {
        Row.Set('Name', this.Name);
        Row.Set('Alias', this.Alias);
        Row.Set('TitleKey', this.TitleKey);
        Row.Set('DataType', this.DataType);
        Row.Set('MaxLength', this.MaxLength);
        Row.Set('Decimals', this.Decimals);

        Row.OBJECT = this;

    }
    /** Creates and returns a {@link tp.DataTable} used in moving around instances of this class.
     */
    static CreateDataTable() {
        let Table = new tp.DataTable();

        Table.AddColumn('Name').DefaultValue = 'NewField';
        Table.AddColumn('Alias').DefaultValue = 'NewField';
        Table.AddColumn('TitleKey').DefaultValue = 'NewField';
        Table.AddColumn('DataType', tp.DataType.Integer).DefaultValue = tp.DataType.Unknown;
        Table.AddColumn('MaxLength', tp.DataType.Integer).DefaultValue = 0;
        Table.AddColumn('Decimals', tp.DataType.Integer).DefaultValue = -1;

        return Table;
    }



};

tp.SqlBrokerFieldDef.prototype.fTitle = '';
tp.SqlBrokerFieldDef.prototype.fTitleKey = '';
tp.SqlBrokerFieldDef.prototype.fAlias = '';

//#endregion

//#region SqlBrokerTableDef
tp.SqlBrokerTableDef = class {

    /** Constructor */
    constructor() {
        this.Fields = [];
        this.JoinTables = [];
        this.StockTables = [];
    }

    /** The field name
     * @type {string}
     */
    Name = '';
    /** The alias of this instance
     * @type {string}
     */ 
    get Alias() {
        return !tp.IsBlankString(this.fAlias) ? this.fAlias : this.Name;
    }
    set Alias(v) {
        this.fAlias = v;
    }
    /** Title (caption) of this instance, used for display purposes.
     * @type {string}
     */
    get Title() {
        return !tp.IsBlankString(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        // nothing
    }
    /** A resource Key used in returning a localized version of Title
     * @type {string}
     */
    get TitleKey() {
        return !tp.IsBlankString(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }

    /** The name of the primary key field
     * @type {string}
     */
    PrimaryKeyField = 'Id';

    /** The  name of the master table.
     * It is used when this table is a detail table in a master-detail relation.
     * @type {string}
     */
    MasterTableName = '';
    /** The field name of a field belonging to a master table.
     * Used when this table is a detail table in a master-detail relation or when this is a join table.
     * @type {string}
     */
    MasterKeyField = 'Id';
    /** The the detail key field. A field that belongs to this table and mathes the MasterTableName primary key field.
     * It is used when this table is a detail table in a master-detail relation.
     * @type {string}
     */
    DetailKeyField = '';

    /** The list of the fields
     * @type {tp.SqlBrokerFieldDef[]}
     */
    Fields = [];
    /** The list of join tables
     * @type {tp.SqlBrokerTableDef[]}
     */
    JoinTables = [];
    /** The main table of a Broker (Item) is selected as <c>select * from TABLE_NAME where ID = :ID</c> <br />
     * If the table contains foreign keys, for instance CUSTOMER_ID etc, then those foreign tables are NOT joined.
     * The programmer who designs the UI just creates a Locator where needed.
     * But there is always the need to have data from those foreign tables in many situations, i.e. in reports.
     * StockTables are used for that. StockTables are selected each time after the select of the main broker table (Item)
     * @type {tp.SqlBrokerQueryDef[]}
     */
    StockTables = [];

    /** Assigns this instance's properties from a specified source.
    * @param {objec} Source
    */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Name = Source.Name || '';
        this.Alias = Source.Alias || '';

        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        this.PrimaryKeyField = Source.PrimaryKeyField || 'Id';

        this.MasterTableName = Source.MasterTableName || '';
        this.MasterKeyField = Source.MasterKeyField || 'Id';
        this.DetailKeyField = Source.DetailKeyField || '';

        if (tp.IsArray(Source.Fields)) {
            this.Fields = [];
            Source.Fields.forEach((item) => {
                let FieldDef = new tp.SqlBrokerFieldDef();
                FieldDef.Assign(item);
                this.Fields.push(FieldDef);
            });
        }

        if (tp.IsArray(Source.JoinTables)) {
            this.JoinTables = [];
            Source.JoinTables.forEach((item) => {
                let TableDef = new tp.SqlBrokerTableDef();
                TableDef.Assign(item);
                this.JoinTables.push(TableDef);
            });
        }

        if (tp.IsArray(Source.StockTables)) {
            this.StockTables = [];
            Source.StockTables.forEach((item) => {
                let QueryDef = new tp.SqlBrokerQueryDef();
                QueryDef.Assign(item);
                this.StockTables.push(QueryDef);
            });
        }
    }
    /** Throws exception if this instance is not a valid one. 
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {
        if (tp.IsNullOrWhiteSpace(this.Name))
            tp.Throw(_L("E_SqlBrokerTableDef_NameIsEmpty", "SqlBrokerTableDef Name is empty"));

        if (tp.IsNullOrWhiteSpace(this.Alias))
            tp.Throw(_L("E_SqlBrokerTableDef_AliasIsEmpty", "SqlBrokerTableDef Alias is empty."));

        if (tp.IsNullOrWhiteSpace(this.PrimaryKeyField))
            tp.Throw(_L("E_SqlBrokerTableDef_PrimaryKeyFieldIsEmpty", "SqlBrokerTableDef PrimaryKeyField is empty."));

        if (!tp.IsValid(this.Fields) || this.Fields.length === 0)
            tp.Throw(_L("E_SqlBrokerTableDef_NoFieldsDefined", "SqlBrokerTableDef Fields not defined."));
    }

    /** Loads this instance's fields from a specified {@link tp.DataTable}
     * @param {tp.DataTable} Table The table to load fields from.
     */
    FieldsFromDataTable(Table) {
        this.Fields.length = 0;
        if (Table instanceof tp.DataTable) {
            Table.Rows.forEach((Row) => {
                let Item = new tp.SqlBrokerFieldDef();
                this.Fields.push(Item);
                Item.FromDataRow(Row);
            });
        }
    }
    /** Saves this instance's fields to a specified {@link tp.DataTable} and returns the table. If no table is specified a new one is created.
     * @param {tp.DataTable} [Table=null] Optional. The table to save fields to.
     * @returns {tp.DataTable} Returns the {@link tp.DataTable} table.
     * */
    FieldsToDataTable(Table = null) {
        if (!(Table instanceof tp.DataTable)) {
            Table = tp.SqlBrokerFieldDef.CreateDataTable();
        }

        this.Fields.forEach((Item) => {
            let Row = Table.AddEmptyRow();
            Item.ToDataRow(Row);
        });

        Table.AcceptChanges();

        return Table;
    }

    /** Loads this instance's JoinTables from a specified {@link tp.DataTable}
     * @param {tp.DataTable} Table The table to load JoinTables from.
     */
    JoinTablesFromDataTable(Table) {
        this.JoinTables.length = 0;
        if (Table instanceof tp.DataTable) {
            Table.Rows.forEach((Row) => {
                let Item = new tp.SqlBrokerTableDef();
                this.JoinTables.push(Item);
                Item.FromJoinTableDataRow(Row);
            });
        }
    }
    /** Saves this instance's JoinTables to a specified {@link tp.DataTable} and returns the table. If no table is specified a new one is created.
     * @param {tp.DataTable} [Table=null] Optional. The table to save JoinTables to.
     * @returns {tp.DataTable} Returns the {@link tp.DataTable} table.
     * */
    JoinTablesToDataTable(Table = null) {
        if (!(Table instanceof tp.DataTable)) {
            Table = tp.SqlBrokerTableDef.CreateJoinTableDataTable();
        }

        this.JoinTables.forEach((Item) => {
            let Row = Table.AddEmptyRow();
            Item.ToJoinTableDataRow(Row);
        });

        Table.AcceptChanges();

        return Table;
    }

    static CreateJoinTableDataTable() {
        let Table = new tp.DataTable();

        Table.AddColumn('OwnKeyField').DefaultValue = 'Own Key Field';
        Table.AddColumn('ForeignTable').DefaultValue = 'Foreign Table Name to Join';
        Table.AddColumn('ForeignAlias').DefaultValue = '';
        Table.AddColumn('ForeignPrimaryKey').DefaultValue = 'Id';

        return Table;
    }
    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromJoinTableDataRow(Row) {
        if (Row instanceof tp.DataRow) {
            let Source = Row.OBJECT;

            Source.Name = Row.Get('ForeignTable', '');
            Source.Alias = Row.Get('ForeignAlias', '');
            Source.PrimaryKeyField = Row.Get('ForeignPrimaryKey', '');
            Source.MasterKeyField = Row.Get('OwnKeyField', '');

            this.Assign(Source);
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToJoinTableDataRow(Row) {
        Row.Set('ForeignTable', this.Name);
        Row.Set('ForeignAlias', this.Alias);
        Row.Set('ForeignPrimaryKey', this.PrimaryKeyField);
        Row.Set('OwnKeyField', this.MasterKeyField);

        Row.OBJECT = this;
    }


    /** Loads this instance's StockTables from a specified {@link tp.DataTable}
     * @param {tp.DataTable} Table The table to load StockTables from.
     */
    StockTablesFromDataTable(Table) {
        this.StockTables.length = 0;
        if (Table instanceof tp.DataTable) {
            Table.Rows.forEach((Row) => {
                let Item = new tp.SqlBrokerQueryDef();
                this.StockTables.push(Item);
                Item.FromDataRow(Row);
            });
        }
    }
    /** Saves this instance's StockTables to a specified {@link tp.DataTable} and returns the table. If no table is specified a new one is created.
     * @param {tp.DataTable} [Table=null] Optional. The table to save StockTables to.
     * @returns {tp.DataTable} Returns the {@link tp.DataTable} table.
     * */
    StockTablesToDataTable(Table = null) {
        if (!(Table instanceof tp.DataTable)) {
            Table = tp.SqlBrokerQueryDef.CreateDataTable();
        }

        this.Fields.forEach((Item) => {
            let Row = Table.AddEmptyRow();
            Item.ToDataRow(Row);
        });

        Table.AcceptChanges();

        return Table;
    }

};

tp.SqlBrokerTableDef.prototype.fTitle = '';
tp.SqlBrokerTableDef.prototype.fTitleKey = '';
tp.SqlBrokerTableDef.prototype.fAlias = '';
//#endregion

//#region SqlBrokerDef

tp.SqlBrokerDef = class {

    /** Constructor */
    constructor() {
    }

    /** The field name
     * @type {string}
     */
    Name = '';
    /** The C# class name of the type this descriptor describes.
     * NOTE: The valus of this property may be a string returned by the Type.AssemblyQualifiedName property of the type.
     * Otherwise it must be a type name registered to the TypeStore either directly or just by using the TypeStoreItemAttribute attribute.
     * In the case of a type registered with the TypeStore, a safe way is to use a Namespace.TypeName combination both, when registering and when retreiving a type.
     * Regarding types belonging to the various Tripous namespaces, using just the TypeName is enough.
     * Most of the Tripous types are already registered to the TypeStore with just their TypeName.
     * @type {string}
     */
    TypeClassName = 'SqlBroker';

    /** Title (caption) of this instance, used for display purposes.
     * @type {string}
     */
    get Title() {
        return !tp.IsBlankString(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        // nothing
    }
    /** A resource Key used in returning a localized version of Title
     * @type {string}
     */
    get TitleKey() {
        return !tp.IsBlankString(this.fTitleKey) ? this.fTitleKey : this.Name;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }

    /** The connection name (database)
     * @type {string}
     */
    ConnectionName = 'Default';

    /** The name of the main table
     * @type {string}
     */
    MainTableName = '';
    /** The name of the detail table, if any
     * @type {string}
     */
    LinesTableName = '';
    /** The name of the sub-detail table, if any
     * @type {string}
     */
    SubLinesTableName = '';

    /** The name of the Entity this broker represents
     * @type {string}
     */
    EntityName = '';

    /** When is true indicates that the OID is a Guid string. 
     * Defaults to true.
     * @type {boolean}
     */
    GuidOids = true;
    /** When true indicates that deletes should happen bottom to top, i.e. starting from the bottom table.
     *  When false indicates that deletes should happen top to bottom, so if any database foreign constraint exists, then let an exception to be thrown. 
     *  Defaults to true.
     * @type {boolean}
     */
    CascadeDeletes = true;

    /** The list of select statements of the list (browser) part.
     * @type {tp.SelectSql[]}
     */
    SelectSqlList = [];
    /** The list of table descriptors.
     * @type {tp.SqlBrokerTableDef[]}
     */
    Tables = [];
    /** A list of SELECT Sql statements that executed once at the initialization of the broker and may be used in various situations, i.e. Locators
     * @type {tp.SqlBrokerQueryDef[]}
     */
    Queries = [];

    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Name = Source.Name || '';
        this.TypeClassName = Source.TypeClassName || 'SqlBroker';

        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        this.ConnectionName = Source.ConnectionName || 'Default';

        this.MainTableName = Source.MainTableName || '';
        this.LinesTableName = Source.LinesTableName || '';
        this.SubLinesTableName = Source.SubLinesTableName || '';

        this.EntityName = Source.EntityName || '';

        this.GuidOids = Source.GuidOids === true;
        this.CascadeDeletes = Source.CascadeDeletes === true;

        if (tp.IsArray(Source.SelectSqlList)) {
            this.SelectSqlList = [];
            Source.SelectSqlList.forEach((item) => {
                let SelectSql = new tp.SelectSql();
                SelectSql.Assign(item);
                this.SelectSqlList.push(SelectSql);
            });
        }


        if (tp.IsArray(Source.Tables)) {
            this.Tables = [];
            Source.Tables.forEach((item) => {
                let TableDef = new tp.SqlBrokerTableDef();
                TableDef.Assign(item);
                this.Tables.push(TableDef);
            });
        }

        if (tp.IsArray(Source.Queries)) {
            this.Queries = [];
            Source.Queries.forEach((item) => {
                let QueryDef = new tp.SqlBrokerQueryDef();
                QueryDef.Assign(item);
                this.Queries.push(QueryDef);
            });
        }

    }
    /** Throws exception if this instance is not a valid one. 
     *  The following code must be the exact copy of the corresponding C# class CheckDescriptor() function
     */
    CheckDescriptor() {

        if (tp.IsNullOrWhiteSpace(this.Name))
            tp.Throw(_L("E_SqlBrokerDef_NameIsEmpty", "SqlBrokerDef Name is empty"));

        if (tp.IsNullOrWhiteSpace(this.ConnectionName))
            tp.Throw(_L("E_SqlBrokerDef_ConnectionNameIsEmpty", "SqlBrokerDef ConnectionName is empty"));

        if (!tp.IsValid(this.Tables) || this.Tables.length === 0)
            tp.Throw(_L("E_SqlBrokerDef_TablesIsEmpty", "SqlBrokerDef Tables is empty"));

        this.Tables.forEach((item) => { item.CheckDescriptor(); });

        if (tp.IsValid(this.SelectSqlList) && this.SelectSqlList.length > 0)
            this.SelectSqlList.forEach((item) => { item.CheckDescriptor(); });

        if (tp.IsValid(this.Queries) && this.Queries.length > 0)
            this.Queries.forEach((item) => { item.CheckDescriptor(); });
    }





};

tp.SqlBrokerDef.prototype.fTitle = '';
tp.SqlBrokerDef.prototype.fTitleKey = '';

//#endregion

 