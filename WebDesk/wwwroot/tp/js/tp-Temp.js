

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
    /** The alias of this field
     * @type {string}
     */
    Alias = '';

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
     * Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
     * and a COUNTRY table with ID and NAME fields. To establish a foreign relation
     *     this field          = "COUNTRY_ID";     // CUSTOMER.COUNTRY_ID
     *     ForeignTableName    = "COUNTRY";                           
     *     ForeignKeyField     = "ID";             // COUNTRY.ID         
     *     ForeignFieldList    = "ID;NAME";        // COUNTRY.ID, COUNTRY.NAME 
     * @type {string}
     */
    ForeignTableName = '';
    /** The alias of a foreign table this field points to, if any, else null.
     * @type {string}
     */
    ForeignTableAlias = '';
    /** The name of the field of the foreign table that becomes the result of a look-up operation
     * @type {string}
     */
    ForeignKeyField = '';
    /** A semi-colon separated list of field names, e.g. Id;Name
     * The fields in this list are used in constructing a SELECT statement. <br />
     * NOTE: The ForeignKeyField must be included in this list. <br />
     * NOTE: When this property has a value then the ForeignTableSql is not used.
     * @type {string}
     */
    ForeignFieldList = '';
    /** A SELECT statement to be used instead of the ForeignFieldList. <br />
     * NOTE: The ForeignKeyField must be included in this SELECT statement.
     * @type {string}
     */
    ForeignTableSql = '';

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

        this.ForeignTableName = Source.ForeignTableName || '';
        this.ForeignTableAlias = Source.ForeignTableAlias || '';
        this.ForeignKeyField = Source.ForeignKeyField || '';
        this.ForeignFieldList = Source.ForeignFieldList || '';
        this.ForeignTableSql = Source.ForeignTableSql || '';

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
};
 
tp.SqlBrokerFieldDef.prototype.fTitle = '';
tp.SqlBrokerFieldDef.prototype.fTitleKey = '';

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
    /** The alias of this field
     * @type {string}
     */
    Alias = '';

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

};

tp.SqlBrokerTableDef.prototype.fTitle = '';
tp.SqlBrokerTableDef.prototype.fTitleKey = '';
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


//#region SqlBrokerTableDefEditDialog
/** Modal dialog box for editing a {@link tp.SqlBrokerTableDef} descriptor
 */
tp.SqlBrokerTableDefEditDialog = class extends tp.Window {
    /**
     * Constructor
     * @param {tp.WindowArgs} Args The window args
     */
    constructor(Args) {
        super(Args);
    }

    /**
     * @type {tp.TabControl}
     */
    Pager = null;
    /**
     * @type {tp.TabPage}
     */
    tabGeneral = null;
 
    /**
     * @type {tp.TextBox}
     */
    edtName = null;
    /**
     * @type {tp.TextBox}
     */
    edtAlias = null;
    /**
     * @type {tp.TextBox}
     */
    edtTitleKey = null;
    /**
     * @type {tp.TextBox}
     */
    edtPrimaryKeyField = null;
    /**
     * @type {tp.TextBox}
     */
    edtMasterTableName = null;
    /**
     * @type {tp.TextBox}
     */
    edtMasterKeyField = null;
    /**
     * @type {tp.TextBox}
     */
    edtDetailKeyField = null;


    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.SqlBrokerTableDefEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.TableDef = this.Args.TableDef;
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        let LayoutRow, elRow, elCol, el, CP, i, ln, Index;

        // , Height: "100%"
        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480], Height: "100%"}'>
    <div class="Col" data-setup='{WidthPercents: [100, 100, 50, 33.33, 33.33], ControlWidthPercents: [100, 60, 60, 60, 60]}'>
    </div>
</div>
`;

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', _L('Cancel'), tp.DialogResult.Cancel);

        this.Pager = new tp.TabControl(null, { Height: '100%' });
        this.Pager.Parent = this.ContentWrapper;

        this.tabGeneral = this.Pager.AddPage(_L('General'));
 


        setTimeout(() => { this.Pager.SelectedPage = this.tabGeneral; }, 100);

        // General Page
        // ---------------------------------------------------------------------------------
        elRow = tp.HtmlToElement(RowHtmlText);
        this.tabGeneral.Handle.appendChild(elRow);
        tp.Ui.CreateContainerControls(elRow.parentElement);

        elCol = elRow.children[0];
        tp.StyleProp(elCol, 'padding-left', '2px');

        // controls
        this.edtName = tp.CreateControlRow(tp.Div(elCol), false, 'Name', { TypeName: 'TextBox' }).Control;

        this.edtAlias           = tp.CreateControlRow(tp.Div(elCol), false, 'Alias', { TypeName: 'TextBox' }).Control;
        this.edtTitleKey        = tp.CreateControlRow(tp.Div(elCol), false, 'TitleKey', { TypeName: 'TextBox' }).Control;
        this.edtPrimaryKeyField = tp.CreateControlRow(tp.Div(elCol), false, 'PrimaryKeyField', { TypeName: 'TextBox' }).Control;
        this.edtMasterTableName = tp.CreateControlRow(tp.Div(elCol), false, 'MasterTableName', { TypeName: 'TextBox' }).Control;
        this.edtMasterKeyField  = tp.CreateControlRow(tp.Div(elCol), false, 'MasterKeyField', { TypeName: 'TextBox' }).Control;
        this.edtDetailKeyField  = tp.CreateControlRow(tp.Div(elCol), false, 'DetailKeyField', { TypeName: 'TextBox' }).Control;

        // item to controls
        this.edtName.Text = this.TableDef.Name;
        this.edtAlias.Text = this.TableDef.Alias;          
        this.edtTitleKey.Text = this.TableDef.TitleKey;      
        this.edtPrimaryKeyField.Text = this.TableDef.PrimaryKeyField;
        this.edtMasterTableName.Text = this.TableDef.MasterTableName;
        this.edtMasterKeyField.Text = this.TableDef.MasterKeyField; 
        this.edtDetailKeyField.Text = this.TableDef.DetailKeyField;  

    }


    /** Called just before a modal window is about to set its DialogResult property. <br />
     * Returning false cancels the setting of the property and the closing of the modal window. <br />
     * NOTE: Setting the DialogResult to any value other than <code>tp.DialogResult.None</code> closes a modal dialog window.
     * @override
     * @param {any} DialogResult
     */
    CanSetDialogResult(DialogResult) {
        if (DialogResult === tp.DialogResult.OK) { 
            this.TableDef.Name = this.edtName.Text;
            this.TableDef.Alias = this.edtAlias.Text;
            this.TableDef.TitleKey = this.edtTitleKey.Text;
            this.TableDef.PrimaryKeyField = this.edtPrimaryKeyField.Text;
            this.TableDef.MasterTableName = this.edtMasterTableName.Text;
            this.TableDef.MasterKeyField = this.edtMasterKeyField.Text;
            this.TableDef.DetailKeyField = this.edtDetailKeyField.Text;  
        }

        return true;
    }

 

    // EDW SqlBrokerTableDef EditDialog
}


/** The instance to be edited by this dialog box.
 * @type {tp.SqlBrokerTableDef}
 * */
tp.SqlBrokerTableDefEditDialog.prototype.TableDef = null;


/**
Displays a modal dialog box for editing a {@link tp.SqlBrokerTableDef} object
@static
@param {tp.SqlBrokerTableDef} TableDef The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlBrokerTableDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlBrokerTableDefEditDialog.ShowModal = function (TableDef, WindowArgs = null) {

    let Args = WindowArgs || {};
    Args.Text = Args.Text || 'Table Definition editor';

    Args = new tp.WindowArgs(Args);
    Args.AsModal = true;
    Args.DefaultDialogResult = tp.DialogResult.Cancel;
    Args.TableDef = TableDef;

    let Result = new tp.SqlBrokerTableDefEditDialog(Args);
    Result.ShowModal();

    return Result;
};
/**
Displays a modal dialog box for editing a {@link tp.SqlBrokerTableDef} object
@static
@param {tp.SqlBrokerTableDef} TableDef The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlBrokerTableDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlBrokerTableDefEditDialog.ShowModalAsync = function (TableDef, WindowArgs = null) {
    return new Promise((Resolve, Reject) => {
        WindowArgs = WindowArgs || {};
        let CloseFunc = WindowArgs.CloseFunc;

        WindowArgs.CloseFunc = (Window) => {
            tp.Call(CloseFunc, Window.Args.Creator, Window);
            Resolve(Window);
        };

        tp.SqlBrokerTableDefEditDialog.ShowModal(TableDef, WindowArgs);
    });
};



//#endregion