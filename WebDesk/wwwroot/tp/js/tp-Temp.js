tp.SelectSqlEditDialog = class extends tp.Window {
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
     * @type {tp.TabPage}
     */
    tabSql = null;
    /**
     * @type {tp.TabPage}
     */
    tabColumns = null;
    /**
     * @type {tp.TabPage}
     */
    tabFilters = null;

    /**
     * @type {tp.TextBox}
     */
    edtName = null;
    /**
     * @type {tp.TextBox}
     */
    edtTitleKey = null;
    /**
     * @type {tp.CheckBox}
     */
    chCompanyAware = null;
    /**
     * @type {tp.TextBox}
     */
    edtConnectionName = null;
    /**
     * @type {tp.TextBox}
     */
    edtDateRangeColumn = null;
    /**
     * @type {tp.HtmlComboBox}
     */
    cboDateRange = null;

    /** The element upon Ace Editor is created. 
     * The '__Editor' property of the element points to Ace Editor object.
     * @type {HTMLElement}
     */
    elSqlEditor = null;
    /**
     * @type {tp.Grid}
     */
    gridColumns = null;

    /**
     * @type {tp.DataTable}
     */
    tblColumns = null;

    /**
     * @type {tp.ToolBar}
     */
    tbFilters = null;
    /**
    * @type {tp.HtmlListBox}
    */
    lboFilters = null;

    /* overrides */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SelectSqlEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.SelectSql = this.Args.SelectSql;
        //this.BoxType = this.Args['BoxType'] || ''; 
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        let elRow, elCol, el, CP, i, ln;

        // , Height: "100%"
        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480], Height: "100%"}'>
    <div class="Col" data-setup='{WidthPercents: [100, 100, 50, 33.33, 33.33], ControlWidthPercents: [100, 60, 60, 60, 60]}'>
    </div>
</div>
`;

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel);
 
        this.Pager = new tp.TabControl(null, { Height: '100%' });
        this.Pager.Parent = this.ContentWrapper;

        this.tabGeneral = this.Pager.AddPage('General');
        this.tabSql = this.Pager.AddPage('Sql');
        this.tabColumns = this.Pager.AddPage('Columns');
        this.tabFilters = this.Pager.AddPage('Filters');

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
        this.edtTitleKey = tp.CreateControlRow(tp.Div(elCol), false, 'Title Key', { TypeName: 'TextBox' }).Control;        
        this.edtConnectionName = tp.CreateControlRow(tp.Div(elCol), false, 'Connection', { TypeName: 'TextBox' }).Control;
        this.edtDateRangeColumn = tp.CreateControlRow(tp.Div(elCol), false, 'Date Range Column', { TypeName: 'TextBox' }).Control;
        this.cboDateRange = tp.CreateControlRow(tp.Div(elCol), false, 'Date Range', { TypeName: 'HtmlComboBox' }).Control;
        this.chCompanyAware = tp.CreateControlRow(tp.Div(elCol), true, 'Company Aware', { TypeName: 'CheckBox' }).Control;

        // item to controls
        this.edtName.Text = this.SelectSql.Name;
        this.edtTitleKey.Text = this.SelectSql.TitleKey;
        this.edtConnectionName.Text = this.SelectSql.ConnectionName;
        this.edtDateRangeColumn.Text = this.SelectSql.DateRangeColumn;
        this.chCompanyAware.Checked = this.SelectSql.CompanyAware === true;

        for (i = 0, ln = tp.DateRanges.WhereRanges.length; i < ln; i++) {
            this.cboDateRange.Add(tp.DateRanges.WhereRangesTexts[i], tp.DateRanges.WhereRanges[i]);
        }

        let Index;
        if (tp.IsNumber(this.SelectSql.DateRange)) {
            let v = this.SelectSql.DateRange.toString();
            Index = this.cboDateRange.IndexOfValue(v);            
        }

        Index = Index >= 0 ? Index : 0;
        this.cboDateRange.SelectedIndex = Index;


        // Sql Page
        // ---------------------------------------------------------------------------------
        this.elSqlEditor = tp.CreateSourceCodeEditor(this.tabSql.Handle, 'sql', this.SelectSql.Text);

        // Columns Page
        // ---------------------------------------------------------------------------------

        // add a tp.Row to the tab page
        let LayoutRow = new tp.Row(null, { Height: '100%' });
        this.tabColumns.AddComponent(LayoutRow);

        // add a DIV for the gridFields tp.Grid in the row
        el = LayoutRow.AddDivElement();
        CP = {
            Name: "gridColumns",
            Height: '100%',
            

            ToolBarVisible: true,
            GroupsVisible: false,
            FilterVisible: false,
            FooterVisible: false,
            GroupFooterVisible: false,

            ButtonInsertVisible: true,
            //ButtonEditVisible: true,
            ButtonDeleteVisible: true,
            ConfirmDelete: true,

            ReadOnly: false,
            AllowUserToAddRows: true,
            AllowUserToDeleteRows: true,
            AutoGenerateColumns: false,

            Columns: [
                { Name: 'Name' },
                { Name: 'TitleKey' },
                { Name: 'DisplayType', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.ColumnDisplayTypeToLookUpTable() },
 
                { Name: 'Width' },
                { Name: 'ReadOnly' },

                { Name: 'DisplayIndex' },
                { Name: 'GroupIndex' },
                { Name: 'Decimals' },
                { Name: 'FormatString' },
                { Name: 'Aggregate', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.AggregateTypeToLookUpTable() },
                { Name: 'AggregateFormat' },
            ]
        };

        // create the columns grid
        this.gridColumns = new tp.Grid(el, CP);
        //this.gridColumns.On("ToolBarButtonClick", this.GridColumns_AnyButtonClick, this);
        //this.gridColumns.On(tp.Events.DoubleClick, this.GridColumns_DoubleClick, this);

        // columns table
        this.tblColumns = new tp.DataTable();
        this.tblColumns.AddColumn('Name');
        this.tblColumns.AddColumn('TitleKey');
        this.tblColumns.AddColumn('DisplayType', tp.DataType.Boolean);
        this.tblColumns.AddColumn('Width', tp.DataType.Integer);
        this.tblColumns.AddColumn('ReadOnly', tp.DataType.Boolean);  
        this.tblColumns.AddColumn('DisplayIndex', tp.DataType.Integer);
        this.tblColumns.AddColumn('GroupIndex', tp.DataType.Integer);
        this.tblColumns.AddColumn('Decimals', tp.DataType.Integer);
        this.tblColumns.AddColumn('FormatString');
        this.tblColumns.AddColumn('Aggregate', tp.DataType.Integer);
        this.tblColumns.AddColumn('AggregateFormat');

        this.SelectSql.Columns.forEach((Column) => {
            let DataRow = this.tblColumns.AddEmptyRow();
            DataRow.Set('Name', Column.Name);
            DataRow.Set('TitleKey', Column.TitleKey);
            DataRow.Set('DisplayType', Column.DisplayType);
            DataRow.Set('Width', Column.Width);
            DataRow.Set('ReadOnly', Column.ReadOnly);
            DataRow.Set('DisplayIndex', Column.DisplayIndex);
            DataRow.Set('GroupIndex', Column.GroupIndex);
            DataRow.Set('Decimals', Column.Decimals);
            DataRow.Set('FormatString', Column.FormatString);
            DataRow.Set('Aggregate', Column.Aggregate);
            DataRow.Set('AggregateFormat', Column.AggregateFormat);
        });

        this.tblColumns.AcceptChanges();

        this.gridColumns.DataSource = this.tblColumns;
        this.gridColumns.BestFitColumns();
 
        this.tblColumns.On('RowCreated', this.tblColumns_RowCreated, this);


        // Filters Page
        // ---------------------------------------------------------------------------------
        el = this.tabFilters.AddDivElement();
        tp.SetStyle(el, {
            'position': 'relative',
            'display': 'flex',
            'gap': '2px',
            'flex-direction': 'column',
            'height': '100%'
        }); 

        this.tbFilters = new tp.ToolBar();
        this.tbFilters.ParentHandle = el;
        this.tbFilters.AddButton('Insert', 'Insert', 'Insert', 'fa fa-plus');
        this.tbFilters.AddButton('Edit', 'Edit', 'Edit', 'fa fa-edit');
        this.tbFilters.AddButton('Delete', 'Delete', 'Delete', 'fa fa-minus');
        this.tbFilters.SetNoText(true);

        el = tp.Div(el);
        tp.SetStyle(el, {
            'position': 'relative',
            'flex-grow': 1,
            'padding' : '0 0 1px 1px'
        }); 

        this.lboFilters = new tp.HtmlListBox();
        this.lboFilters.ParentHandle = el;
        this.lboFilters.Width = '240px';
        this.lboFilters.Height = '100%';

        this.tbFilters.On('ButtonClick', this.tbFilters_ButtonClick, this);

        this.SelectSql.Filters.forEach((FilterDef) => {
            this.lboFilters.Add(FilterDef.FieldPath, FilterDef.FieldPath);
        });
 

    }
    /** Can be used in passing the results back to the caller code. 
     * On modal dialogs the code should examine the DialogResult to decide what to do.
     * @override
     * */
    PassBackResult() {
        if (this.DialogResult === tp.DialogResult.OK) {
            this.SelectSql.Name = this.edtName.Text;
            this.SelectSql.TitleKey = this.edtTitleKey.Text;
            this.SelectSql.ConnectionName = this.edtConnectionName.Text;
            this.SelectSql.DateRangeColumn = this.edtDateRangeColumn.Text;
            this.SelectSql.DateRange = tp.StrToInt(this.cboDateRange.SelectedValue);
            this.SelectSql.CompanyAware = this.chCompanyAware.Checked;

            this.SelectSql.Text = this.elSqlEditor.__Editor.getValue();

            this.SelectSql.Columns.length = 0;

            this.tblColumns.Rows.forEach((Row) => {
                let Column = new tp.SelectSqlColumn();
                this.SelectSql.Columns.push(Column);

                Column.Name = Row.Get('Name', '');
                Column.TitleKey = Row.Get('TitleKey', '');
                Column.DisplayType = Row.Get('DisplayType', Column.DisplayType);
                Column.Width = Row.Get('Width', 90);
                Column.ReadOnly = Row.Get('ReadOnly', false);
                Column.DisplayIndex = Row.Get('DisplayIndex', 0);
                Column.GroupIndex = Row.Get('GroupIndex', -1);
                Column.Decimals = Row.Get('Decimals', -1);
                Column.FormatString = Row.Get('FormatString', '');
                Column.Aggregate = Row.Get('Aggregate', Column.Aggregate);
                Column.AggregateFormat = Row.Get('AggregateFormat', '');

            });
        }
    }



    /* event handlers */
    /** Called when a new data row is created and it is about to added to the table
     * @param {tp.DataTableEventArgs} Args
     */
    tblColumns_RowCreated(Args) {
        let Row = Args.Row;
        Row.Set('Name', 'NewColumn');
        Row.Set('TitleKey', 'New Column');
        Row.Set('DisplayType', tp.ColumnDisplayType.Default);
        Row.Set('Width', 90);
        Row.Set('ReadOnly', false);
        Row.Set('GroupIndex', -1);
        Row.Set('Decimals', -1);
        Row.Set('FormatString', '');
        Row.Set('Aggregate', tp.AggregateType.None);
        Row.Set('AggregateFormat', '');
    }
    /** Event handler
     * @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
     */
    GridColumns_AnyButtonClick(Args) {
        Args.Handled = true;

        switch (Args.Command) {
            case 'GridRowInsert':
                //this.InsertFieldRow();
                break;
            case 'GridRowEdit':
                //this.EditFieldRow();
                break;
            case 'GridRowDelete':
                //tp.InfoNote('Clicked: ' + Args.Command);
                break;
        }
    }
    /**
    Event handler
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    GridColumns_DoubleClick(Args) {
        //Args.Handled = true;
        //this.EditFieldRow();
    }
    /** Event handler. Filters tool-bar button click.
     * @param {tp.ToolBarItemClickEventArgs} Args
     */
    tbFilters_ButtonClick(Args) {
        tp.InfoNote(Args.Command);
    }
};


/**
 * @type {tp.SelectSql}
 * */
tp.SelectSqlEditDialog.prototype.SelectSql = null;

/**
Displays a modal dialog box for editing a {@link tp.SelectSql} object
@static
@param {tp.SelectSql} SelectSql The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SelectSqlEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SelectSqlEditDialog.ShowModal = function (SelectSql, WindowArgs = null) {

    let Args = WindowArgs || {};
    Args.Text = Args.Text || 'SelectSql editor';

    Args = new tp.WindowArgs(Args);
    Args.AsModal = true;
    Args.DefaultDialogResult = tp.DialogResult.Cancel;
    Args.SelectSql = SelectSql;

    let Result = new tp.SelectSqlEditDialog(Args);
    Result.ShowModal();

    return Result;
};
/**
Displays a modal dialog box for editing a {@link tp.SelectSql} object
@static
@param {tp.SelectSql} SelectSql The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SelectSqlEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SelectSqlEditDialog.ShowModalAsync = function (SelectSql, WindowArgs = null) {
    return new Promise((Resolve, Reject) => {
        WindowArgs = WindowArgs || {};
        let CloseFunc = WindowArgs.CloseFunc;

        WindowArgs.CloseFunc = (Window) => {
            tp.Call(CloseFunc, Window.Args.Creator, Window);
            Resolve(Window);
        }; 

        tp.SelectSqlEditDialog.ShowModal(SelectSql, WindowArgs);
    });
 
};





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

        // EDW

    }



};

tp.SqlBrokerDef.prototype.fTitle = '';
tp.SqlBrokerDef.prototype.fTitleKey = '';

//#endregion
 