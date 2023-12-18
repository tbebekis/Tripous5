tp.Urls.SysDataSelectList = '/SysData/SelectList';
tp.Urls.SysDataSelectItemById = '/SysData/SelectItemById';
tp.Urls.SysDataSaveItem = '/SysData/SaveItem';


//#region SysDataItem
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
            this.Title = Row.Get('Title', '');
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
        Row.Set('Title', this.Title);
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



//#region SysDataHandler

tp.SysDataHandler = class {

    /** Constructor
     * @param {tp.View} View The view that calls and owns this handler
     * @param {string} DataType The SysData DataType this handler can handle.
     */
    constructor(View, DataType) {
        this.View = View;
        this.DataType = DataType;
    }

    /** Returns true if the owner {@link tp.View}  is in insert mode
     * @type {boolean}
     * */
    get IsInsertItem() { return this.View.ViewMode === tp.DataViewMode.Insert; }
    /** Returns true if the owner {@link tp.View}  is in edit mode
     * @type {boolean}
     * */
    get IsEditItem() { return this.View.ViewMode === tp.DataViewMode.Edit; }

    /* overridables */

    /** If not already created, then creates any control this handler needs in order to edit a SysDataItem.
    * */
    CreateEditControls() {
    }

    /** Called before the Insert() operation of the owner View.
    */
    InsertItemBefore() {
    }
    /** Called after the Insert() operation of the owner View.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     */
    InsertItemAfter(tblSysDataItem) {
    }

    /** Called before the Edit() operation of the owner View.
     * The View is about to load in its Edit part a SysData Item from server.
     * @param {string} Id
     */
    EditItemBefore(Id) {
    }
    /** Called after the Edit() operation of the owner View.
     * The View is just loaded in its Edit part a SysData Item from server.
     * @param {string} Id
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     */
    EditItemAfter(Id, tblSysDataItem) {
    }

    /** Called before the Commit() operation of the owner View.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     */
    CommitItemBefore(tblSysDataItem) {
    }
    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @returns {boolean} Returns true if commit is allowed, else false.
     */
    CanCommitItem(tblSysDataItem) {
        return true;
    }

    /**
    Event handler. Called by the owner View when the Edit Pager changes Page.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    EditPager_PageChanged(Args) {
 
    }

};

/**
 @type {string}
 */
tp.SysDataHandler.prototype.DataType = 'Table';
/**
 @type {tp.DeskSysDataView}
 */
tp.SysDataHandler.prototype.View = null;

//#endregion

//#region SysDataHandlerTable

tp.SysDataHandlerTable = class extends tp.SysDataHandler {

    /** Constructor
     * @param {tp.View} View The view that calls and owns this handler
     */
    constructor(View) {
        super(View, 'Table')
    }

    /** A {@link tp.DataTable} for handling the fields of a table def.
     @type {tp.DataTable}
     */
    tblFields = null;
    /** A {@link tp.Grid} for handling the fields of a table def.
     @type {tp.Grid}
     */
    gridFields = null;
 

    /* overrides */
    /** If not already created, then creates any control this handler needs in order to edit a SysDataItem.
     * @override
    * */
    CreateEditControls() {
        if (this.View.pagerEdit.GetPageCount() === 1) {

            // add a tp.TabPage to View's pagerEdit
            let FieldsPage = this.View.pagerEdit.AddPage('Fields');
            tp.Data(FieldsPage.Handle, 'Name', 'Fields');

            // add a tp.Row to the tab page
            let Row = new tp.Row(null, { Height: '100%' });
            FieldsPage.AddComponent(Row);

            // add a DIV for the gridFields tp.Grid in the row
            let el = Row.AddDivElement();
            let CP = {
                Name: "gridFields",
                Height: '100%',

                ToolBarVisible: true,
                GroupsVisible: false,
                FilterVisible: false,
                FooterVisible: false,
                GroupFooterVisible: false,

                ButtonInsertVisible: true,
                ButtonEditVisible: true,
                ButtonDeleteVisible: true,
                ConfirmDelete: true,

                AllowUserToAddRows: true,
                AllowUserToDeleteRows: true,
                AutoGenerateColumns: false,

                Columns: [
                    { Name: 'Name' },
                    { Name: 'TitleKey' },
                    { Name: 'IsPrimaryKey' },
                    { Name: 'DataType', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]) },

                    { Name: 'Length' },
                    { Name: 'Required' },
                    { Name: 'DefaultExpression' },

                    { Name: 'Unique' },
                    { Name: 'UniqueConstraintName' },
                    { Name: 'ForeignKey' },
                    { Name: 'ForeignKeyConstraintName' },
                ]
            };

            // create the grid
            this.gridFields = new tp.Grid(el, CP);
            this.gridFields.On("ToolBarButtonClick", this.GridFields_AnyButtonClick, this);
            this.gridFields.On(tp.Events.DoubleClick, this.GridFields_DoubleClick, this);
        }

    }

    /** Called before the Insert() operation of the owner View.
     * @override
    */
    InsertItemBefore() {
    }
    /** Called after the Insert() operation of the owner View.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @override
     */
    InsertItemAfter(tblSysDataItem) {
        this.SetupItem(true, tblSysDataItem);

        let Row = this.tblFields.AddEmptyRow();
        Row.Set('Name', 'Id');
        Row.Set('TitleKey', 'Id');
        Row.Set('IsPrimaryKey', true);
        Row.Set('DataType', tp.DataType.String);
        Row.Set('Length', 40);
        Row.Set('Required', true);

    }

    /** Called before the Edit() operation of the owner View. <br />
     * The View is about to load in its Edit part a SysData Item from server.
     * @param {string} Id
     * @override
     */
    EditItemBefore(Id) {
    }
    /** Called after the Edit() operation of the owner View. <br />
     * The View is just loaded in its Edit part a SysData Item from server.
     * @param {string} Id
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @override
     */
    EditItemAfter(Id, tblSysDataItem) {
        this.SetupItem(false, tblSysDataItem);
    }

    /** Called before the Commit() operation of the owner View.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @override
     */
    CommitItemBefore(tblSysDataItem) {
        let Row = tblSysDataItem.Rows[0];

        let TableDef = new tp.DataTableDef();
        TableDef.Name = Row.Get('DataName', '');
        TableDef.TitleKey = Row.Get('TitleKey', '');
        TableDef.FieldsFromDataTable(this.tblFields);

        let JsonText = tp.ToJson(TableDef, true);
        Row.Set('Data1', JsonText);
    }
    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @returns {boolean} Returns true if commit is allowed, else false.
     * @override
     */
    CanCommitItem(tblSysDataItem) {
        let Result = true;

        if (tp.IsEmpty(this.tblFields) || this.tblFields.RowCount <= 1) {
            tp.WarningNote('Cannot save changes.\nNo fields defined in the table');
            Result = false;
        }

        let Row = tblSysDataItem.Rows[0];
        let v = Row.Get('DataName', '');

        if (tp.IsBlankString(v)) {
            tp.WarningNote('Cannot save changes.\nNo Table Name (DataName)');
            Result = false;
        }

        if (tp.IsString(v) && !tp.IsValidIdentifier(v, '$')) {
            tp.WarningNote('Cannot save changes.\nTable Name should start with _ or letter \nand cannot contain spaces, special characters and punctuation.');
            Result = false;
        }

        return Result;
    }

    /**
    Event handler. Called by the owner View when the Edit Pager changes Page.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    EditPager_PageChanged(Args) {
    }

    /* private */
    /** Creates and assigns the tblFields. Sets tblFields a grid's data-source.
     * @param {boolean} IsInsertItem True when is an Insert new SysDataItem operation. False when is an Edit an existing SysDataItem operation.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * */
    SetupItem(IsInsertItem, tblSysDataItem) {

        // create an empty tp.DataTableDef
        let TableDef = new tp.DataTableDef();

        // if editing an already existing table definition
        // then read the json from Data1 field and load the TableDef
        if (IsInsertItem === false) {
            let Text = tblSysDataItem.Rows[0].Get('Data1');
            let Source = eval("(" + Text + ")");
            log(Source);
            TableDef.Assign(Source);
        }

        // create the tblFields
        this.tblFields = TableDef.FieldsToDataTable();
        this.tblFields.AcceptChanges();

        this.gridFields.DataSource = this.tblFields;
        this.gridFields.BestFitColumns();
 
    }
 
    /** Creates and returns a clone of the tblFields with just a single row, in order to be passed to the edit dialog.
     * The row is either empty, on insert, or a clone of a tblFields row, on edit.
     * @param {tp.DataRow} SourceRow The row is either empty, on insert, or a clone of a tblFields row, on edit.
     * @returns {tp.DataTable} Returns a clone of the tblFields with just a single row, in order to be passed to the edit dialog.
     */
    CreateEditFieldTable(SourceRow = null) {
        let FieldRow;
        let IsInsertField = tp.IsEmpty(SourceRow);    

        // create the tblField, used in editing a single field
        let tblField = this.tblFields.Clone();
        tblField.Name = 'Field';

        // add the single row in tblField
        FieldRow = tblField.AddEmptyRow();

        if (IsInsertField) {
            FieldRow.Set('DataType', tp.DataType.String);
            FieldRow.Set('Length', 0);
            FieldRow.Set('IsPrimaryKey', false);
            FieldRow.Set('Required', false);
            FieldRow.Set('Unique', false);
        }
        else {
            FieldRow.CopyFromRow(SourceRow);
        } 

        return tblField;
    }

    /** Displays an edit dialog box for editing an existing or new row of the tblFields.
     * The passed {@link tp.DataTable} is a clone of tblFields with just a single row.
     * That single row is either empty, on insert, or a clone of a tblFields row, on edit.
     * @param {boolean} IsInsertField True when is an Insert Field operation. False when is an Edit Field operation.
     * @param {tp.DataTable} tblField The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
     */
    async ShowEditFieldDialog(IsInsertField, tblField) {
        let DialogBox = null;
        let ContentHtmlText;
        let HtmlText;
        let HtmlRowList = [];

        
        let ColumnNames = [];       // Visible Controls
        let EditableColumns = []    // Editable Controls

        // inserting a Table
        if (this.IsInsertItem) {
            ColumnNames = ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'ForeignKeyConstraintName', 'Unique', 'UniqueConstraintName'];
            EditableColumns = ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique'];
        }
        // editing a Table
        else {
            ColumnNames = ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'ForeignKeyConstraintName', 'Unique', 'UniqueConstraintName'];
            EditableColumns = IsInsertField === true ?
                ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique']:
                ['Name', 'TitleKey', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique'];
        }

        let DataSource = new tp.DataSource(tblField);

        // Editable Controls
        tblField.Columns.forEach((column) => {
            if (column.Name === 'Name')
                column.MaxLength = 30;

            column.ReadOnly = EditableColumns.indexOf(column.Name) < 0;
        });

        // Visible Controls
        // prepare HTML text for each column in tblFields
        ColumnNames.forEach((ColumnName) => {
            let Column = this.tblFields.FindColumn(ColumnName);
            let IsCheckBox = Column.DataType === tp.DataType.Boolean;

            let Text = Column.Title;
            let Ctrl = {
                TypeName: Column.Name === 'DataType' ? 'ComboBox' : tp.DataTypeToUiType(Column.DataType),
                TableName: tblField.Name,
                DataField: Column.Name
            };

            if (ColumnName === 'DataType') {
                Ctrl.ListOnly = true;
                Ctrl.ListValueField = 'Id';
                Ctrl.ListDisplayField = 'Name';
                Ctrl.ListSourceName = 'DataType';
            }

            // <div class="tp-CtrlRow" data-setup="{Text: 'Id', Control: { TypeName: 'TextBox', Id: 'Code', DataField: 'Code', ReadOnly: true } }"></div>
            HtmlText = tp.CtrlRow.GetHtml(IsCheckBox, Text, Ctrl);
            HtmlRowList.push(HtmlText);
        });


        // join html text for all control rows
        HtmlText = HtmlRowList.join('\n');

        // content
        ContentHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480]}'>
    <div class="Col" data-setup='{ControlWidthPercents: [100, 60, 60, 60, 60]}'>
        ${HtmlText}
    </div>
</div>
`;
        let elContent = tp.HtmlToElement(ContentHtmlText);

        // show the dialog
        if (tp.IsHTMLElement(elContent)) {


            let BodyWidth = tp.Doc.body.offsetWidth
            let w = BodyWidth <= 580 ? BodyWidth - 6 : 580;

            let WindowArgs = new tp.WindowArgs({ Text: 'Edit Field', Width: w, Height: 'auto' });

            //----------------------------------------------------- 
            /** Callback to be called after the dialog shows itself (i.e. OnShown())
             * @param {tp.Window} Window
             */
            WindowArgs.ShowFunc = (Window) => {
                tp.StyleProp(elContent.parentElement, 'padding', '5px');

                // force tp-Cols to adjust
                Window.BroadcastSizeModeChanged();

                // bind dialog controls
                tp.BindAllDataControls(elContent, (DataSourceName) => {
                    if (DataSourceName === 'Field')
                        return DataSource;

                    if (DataSourceName === 'DataType') {
                        let Result = new tp.DataSource(tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]));
                        return Result;
                    }

                    return null;
                });
            };
            //----------------------------------------------------- 
            /** Callback to be called just before a modal window is about to set its DialogResult property. <br />
             *  Returning false from the call-back cancels the setting of the property and the closing of the modal window. <br />
             * NOTE: Setting the DialogResult to any value other than <code>tp.DialogResult.None</code> closes a modal dialog window.
             * @param {tp.Window} Window
             * @param {number} DialogResult One of the {@link tp.DialogResult} constants
             * */
            WindowArgs.CanSetDialogResultFunc = (Window, DialogResult) => {
                if (DialogResult === tp.DialogResult.OK) {
                
                    let Row = tblField.Rows[0];

                    // Name
                    let v = Row.Get('Name', '');
                    if (tp.IsBlank(v)) {
                        tp.WarningNote('Name is required');
                        return false;
                    }

                    if (tp.IsString(v) && !tp.IsValidIdentifier(v, '$')) {
                        tp.WarningNote('Name should start with _ or letter \nand cannot contain spaces, special characters and punctuation.');
                        return false;
                    }

                    // Length
                    v = Row.Get('DataType', '');
                    if (v === tp.DataType.String) {
                        v = Row.Get('Length', 0);
                        if (v <= 0) {
                            tp.WarningNote('Invalid Length');
                            return false;
                        }
                    }


                }
                
                return true;
            };
            //----------------------------------------------------- 
            /**  Callback to be called when the dialog is about to close (i.e. OnClosing())
             * @param {tp.Window} Window
             */
            WindowArgs.CloseFunc = (Window) => {
                let Row = tblField.Rows[0];
                let v = Row.Get('TitleKey', '');
                if (tp.IsBlank(v)) {
                    Row.Set('TitleKey', Row.Get('Name', ''));
                }
            };
            //----------------------------------------------------- 

            tp.Ui.CreateContainerControls(elContent.parentElement);
            DialogBox = await tp.ContentWindow.ShowModalAsync(elContent, WindowArgs);
        }

        return DialogBox;
    }
    /** Called when inserting a single row of the tblFields and displays the edit dialog 
     */
    async InsertFieldRow() {
        let tblField = this.CreateEditFieldTable(null);
        let DialogBox = await this.ShowEditFieldDialog(true, tblField);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let FieldRow = tblField.Rows[0];
            let Row = this.tblFields.AddEmptyRow();
            Row.CopyFromRow(FieldRow);
        }
    }
    /** Called when editing a single row of the tblFields and displays the edit dialog 
     */
    async EditFieldRow() {
        let Row = this.gridFields.FocusedRow;
        if (tp.IsValid(Row)) {
            if (Row.Get('IsPrimaryKey', false) === true) {
                tp.WarningNote('Editing Primary Key is not allowed.');
            }
            else {
                let tblField = this.CreateEditFieldTable(Row);
                let DialogBox = await this.ShowEditFieldDialog(false, tblField);

                if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                    let FieldRow = tblField.Rows[0];
                    Row.CopyFromRow(FieldRow);
                }
            }
        }
    }
 
    /* event handlers */
    /** Event handler
     * @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
     */
    GridFields_AnyButtonClick(Args) {
        Args.Handled = true;

        switch (Args.Command) {
            case 'GridRowInsert':
                this.InsertFieldRow();
                break;
            case 'GridRowEdit':
                this.EditFieldRow();
                break;
            case 'GridRowDelete':
                tp.InfoNote('Clicked: ' + Args.Command);
                break;
        }


    }
    /**
    Event handler
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    GridFields_DoubleClick(Args) {
        Args.Handled = true;
        this.EditFieldRow();
    }
};

//#endregion

//#region tp.SysDataHandlerBroker
tp.SysDataHandlerBroker = class extends tp.SysDataHandler {
    /** Constructor
     * @param {tp.View} View The view that calls and owns this handler
     */
    constructor(View) {
        super(View, 'Broker')
    }

    /** A C# SqlBrokerDef instance as it comes from server. <br /> 
     * @type {tp.SqlBrokerDef}
     */
    BrokerDef = null;
    /**
     * @type {tp.DataTable}
     */
    tblBrokerDef = null;
    /**
     * @type {tp.DataSource}
     */
    dsBrokerDef = null;
    /** A responsive row which is the container of controls bound to tblBrokerDef.
     * @type {tp.Row}
     */
    BrokerLayoutRow = null;

    /** The broker General tab page
     * @type {tp.TabPage}
     */
    tabGeneral = null;
    /**  
     * @type {tp.TabPage}
     */
    tabSelectSqlList = null;
    /**  
     * @type {tp.TabPage}
    */
    tabQueryList = null;
    /**  
     * @type {tp.TabPage}
    */
    tabTableList = null;
 
    /**
     * @type {tp.DataTable}
     */
    tblSelectSqlList = null;  
    /**
     * @type {tp.DataTable}
     */
    tblQueryList = null;  
    /**
     * @type {tp.DataTable}
     */
    tblTableList = null;  

    /**
     * @type {tp.Grid}
     */
    gridSelectSqlList = null;
    /**
     * @type {tp.Grid}
     */
    gridQueryList = null;
    /**
     * @type {tp.Grid}
     */
    gridTableList = null;

    /* private */
    /** 
     * @param {boolean} IsInsertItem True when is an Insert new SysDataItem operation. False when is an Edit an existing SysDataItem operation.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * */
    SetupItem(IsInsertItem, tblSysDataItem) {
        let Row = tblSysDataItem.Rows[0];
 
        // create an empty def
        this.BrokerDef = new tp.SqlBrokerDef(); 
        this.BrokerDef.Name = Row.Get('DataName', this.BrokerDef.Name);
        this.BrokerDef.TitleKey = Row.Get('TitleKey', this.BrokerDef.TitleKey);
        

        // Edit
        // if editing an already existing broker definition
        // then read the json from Data1 field and load the BrokerDef
        if (IsInsertItem === false) {
            let Text = tblSysDataItem.Rows[0].Get('Data1');
            let Source = eval("(" + Text + ")");
            log(Source);
            this.BrokerDef.Assign(Source);
        }
        else {
           // Insert
        }

        this.tblBrokerDef.ClearRows();
        this.tblBrokerDef.AcceptChanges();

        Row = this.tblBrokerDef.AddEmptyRow();

        Row.Set('Name', this.BrokerDef.Name);
        Row.Set('TypeClassName', this.BrokerDef.TypeClassName);
        //Row.Set('Title', this.BrokerDef.Title);
        Row.Set('TitleKey', this.BrokerDef.TitleKey);
        Row.Set('ConnectionName', this.BrokerDef.ConnectionName);
        Row.Set('MainTableName', this.BrokerDef.MainTableName);
        Row.Set('LinesTableName', this.BrokerDef.LinesTableName);
        Row.Set('SubLinesTableName', this.BrokerDef.SubLinesTableName);
        Row.Set('EntityName', this.BrokerDef.EntityName);
        Row.Set('GuidOids', this.BrokerDef.GuidOids);
        Row.Set('CascadeDeletes', this.BrokerDef.CascadeDeletes); 
    }

    /* overrides */
    /** If not already created, then creates any control this handler needs in order to edit a SysDataItem.
    * */
    CreateEditControls() {

        if (this.View.pagerEdit.GetPageCount() === 1) {

            let LayoutRow, el, CP;

            // General Page
            // ---------------------------------------------------------------------------------
            // add a tp.TabPage to View's pagerEdit
            this.tabGeneral = this.View.pagerEdit.AddPage(_L('General'));
            tp.Data(this.tabGeneral.Handle, 'Name', 'General');
 
            // create the data table
            this.tblBrokerDef = new tp.DataTable();
            this.tblBrokerDef.Name = 'BrokerDef';

            this.tblBrokerDef.AddColumn('Name');
            this.tblBrokerDef.AddColumn('TypeClassName');
            //this.tblBrokerDef.AddColumn('Title');
            this.tblBrokerDef.AddColumn('TitleKey');
            this.tblBrokerDef.AddColumn('ConnectionName');
            this.tblBrokerDef.AddColumn('MainTableName');
            this.tblBrokerDef.AddColumn('LinesTableName');
            this.tblBrokerDef.AddColumn('SubLinesTableName');
            this.tblBrokerDef.AddColumn('EntityName');
            this.tblBrokerDef.AddColumn('GuidOids', tp.DataType.Boolean);
            this.tblBrokerDef.AddColumn('CascadeDeletes', tp.DataType.Boolean);

            this.tblBrokerDef.SetColumnReadOnly('Name', true);
            this.tblBrokerDef.SetColumnReadOnly('TitleKey', true);

            let HtmlText;
            let HtmlRowList = [];

            // for each table field, produce html text for control rows and add the text to a string-list
            this.tblBrokerDef.Columns.forEach((Column) => {
                let IsCheckBox = Column.DataType === tp.DataType.Boolean;

                let Text = Column.Title;
                let Ctrl = {
                    TypeName: tp.DataTypeToUiType(Column.DataType),
                    TableName: this.tblBrokerDef.Name,
                    DataField: Column.Name
                };

                // <div class="tp-CtrlRow" data-setup="{Text: 'Id', Control: { TypeName: 'TextBox', Id: 'Code', DataField: 'Code', ReadOnly: true } }"></div>
                HtmlText = tp.CtrlRow.GetHtml(IsCheckBox, Text, Ctrl);
                HtmlRowList.push(HtmlText)
            });


            // join html text for all control rows
            HtmlText = HtmlRowList.join('\n');

            // content
            HtmlText = `
<div class="Row" data-setup='{Height: "100%", Breakpoints: [450, 768, 1050, 1480]}'>
    <div class="Col" data-setup='{ControlWidthPercents: [100, 60, 60, 60, 60]}'>
        ${HtmlText}
    </div>
</div>
`;
            let elRow = tp.HtmlToElement(HtmlText);

            LayoutRow = new tp.Row(elRow, { Height: '100%' });
            this.tabGeneral.AddComponent(LayoutRow);

            tp.Ui.CreateContainerControls(elRow);

            this.dsBrokerDef = new tp.DataSource(this.tblBrokerDef);

            tp.BindAllDataControls(elRow, () => { return this.dsBrokerDef; });        

            // SelectSqlList Page
            // ---------------------------------------------------------------------------------
            this.tabSelectSqlList = this.View.pagerEdit.AddPage(_L('SelectSqlList'));
            tp.Data(this.tabSelectSqlList.Handle, 'Name', 'SelectSqlList');

            LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
            this.tabSelectSqlList.AddComponent(LayoutRow);

            // add a DIV for the gridSelectSqlList tp.Grid in the row
            el = LayoutRow.AddDivElement();
            CP = {
                NameTag: "gridSelectSqlList",
                Height: '100%',

                ToolBarVisible: true,
                GroupsVisible: false,
                FilterVisible: false,
                FooterVisible: false,
                GroupFooterVisible: false,

                ButtonInsertVisible: true,
                ButtonEditVisible: true,
                ButtonDeleteVisible: true,
                ConfirmDelete: true,

                ReadOnly: true,
                AllowUserToAddRows: true,
                AllowUserToDeleteRows: true,
                AutoGenerateColumns: false,

                Columns: [
                    { Name: 'Name' },
                    { Name: 'ConnectionName' },
                    { Name: 'CompanyAware' },                     
                ]
            };

            // create the grid
            this.gridSelectSqlList = new tp.Grid(el, CP);

            this.tblSelectSqlList = new tp.DataTable();
            this.tblSelectSqlList.AddColumn('Name').DefaultValue = '';
            this.tblSelectSqlList.AddColumn('ConnectionName').DefaultValue = tp.SysConfig.DefaultConnection;
            this.tblSelectSqlList.AddColumn('CompanyAware', tp.DataType.Boolean).DefaultValue = false;
 
            this.tblSelectSqlList.AcceptChanges();

            this.gridSelectSqlList.DataSource = this.tblSelectSqlList;
            this.gridSelectSqlList.BestFitColumns();

            this.gridSelectSqlList.On("ToolBarButtonClick", this.AnyGridButtonClick, this);
            this.gridSelectSqlList.On(tp.Events.DoubleClick, this.AnyGridDoubleClick, this);


            // Queries Page
            // ---------------------------------------------------------------------------------
            this.tabQueryList = this.View.pagerEdit.AddPage(_L('QueryList'));
            tp.Data(this.tabQueryList.Handle, 'Name', 'QueryList');

            LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
            this.tabQueryList.AddComponent(LayoutRow);

            // add a DIV for the gridQueryList tp.Grid in the row
            el = LayoutRow.AddDivElement();
            CP = {
                NameTag: "gridQueryList",
                Height: '100%',

                ToolBarVisible: true,
                GroupsVisible: false,
                FilterVisible: false,
                FooterVisible: false,
                GroupFooterVisible: false,

                ButtonInsertVisible: true,
                ButtonEditVisible: true,
                ButtonDeleteVisible: true,
                ConfirmDelete: true,

                ReadOnly: true,
                AllowUserToAddRows: true,
                AllowUserToDeleteRows: true,
                AutoGenerateColumns: false,

                Columns: [
                    { Name: 'Name' }, 
                ]
            };

            // create the grid
            this.gridQueryList = new tp.Grid(el, CP);

            this.tblQueryList = new tp.DataTable();
            this.tblQueryList.AddColumn('Name').DefaultValue = ''; 

            //this.tblQueryList.AcceptChanges();

            this.gridQueryList.DataSource = this.tblQueryList;
            this.gridQueryList.BestFitColumns();

            this.gridQueryList.On("ToolBarButtonClick", this.AnyGridButtonClick, this);
            this.gridQueryList.On(tp.Events.DoubleClick, this.AnyGridDoubleClick, this);

            // Queries Page
            // ---------------------------------------------------------------------------------
            this.tabTableList = this.View.pagerEdit.AddPage(_L('TableList'));
            tp.Data(this.tabTableList.Handle, 'Name', 'TableList');

            LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
            this.tabTableList.AddComponent(LayoutRow);

            // add a DIV for the gridTableList tp.Grid in the row
            el = LayoutRow.AddDivElement();
            CP = {
                NameTag: "gridTableList",
                Height: '100%',

                ToolBarVisible: true,
                GroupsVisible: false,
                FilterVisible: false,
                FooterVisible: false,
                GroupFooterVisible: false,

                ButtonInsertVisible: true,
                ButtonEditVisible: true,
                ButtonDeleteVisible: true,
                ConfirmDelete: true,

                ReadOnly: true,
                AllowUserToAddRows: true,
                AllowUserToDeleteRows: true,
                AutoGenerateColumns: false,

                Columns: [
                    { Name: 'Name' },
                    { Name: 'Alias' },
                    { Name: 'TitleKey' },
                    { Name: 'PrimaryKeyField' },
                    { Name: 'MasterTableName' },
                    { Name: 'MasterKeyField' },
                    { Name: 'DetailKeyField' }, 
                ]
            };

            // create the grid
            this.gridTableList = new tp.Grid(el, CP);

            this.tblTableList = new tp.DataTable();
            this.tblTableList.AddColumn('Name');
            this.tblTableList.AddColumn('Alias');
            this.tblTableList.AddColumn('TitleKey');
            this.tblTableList.AddColumn('PrimaryKeyField'); //.DefaultValue = 'Id';
            this.tblTableList.AddColumn('MasterTableName');
            this.tblTableList.AddColumn('MasterKeyField');
            this.tblTableList.AddColumn('DetailKeyField');
 
            this.gridTableList.DataSource = this.tblTableList;
            this.gridTableList.BestFitColumns();

            this.gridTableList.On("ToolBarButtonClick", this.AnyGridButtonClick, this);
            this.gridTableList.On(tp.Events.DoubleClick, this.AnyGridDoubleClick, this);

        }
    }

    /** Called before the Insert() operation of the owner View.
    */
    InsertItemBefore() {
    }
    /** Called after the Insert() operation of the owner View.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     */
    InsertItemAfter(tblSysDataItem) {
        this.SetupItem(true, tblSysDataItem);
    }

    /** Called before the Edit() operation of the owner View.
     * The View is about to load in its Edit part a SysData Item from server.
     * @param {string} Id
     */
    EditItemBefore(Id) {
    }
    /** Called after the Edit() operation of the owner View.
     * The View is just loaded in its Edit part a SysData Item from server.
     * @param {string} Id
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     */
    EditItemAfter(Id, tblSysDataItem) {
        this.SetupItem(false, tblSysDataItem);
    }

    /** Called before the Commit() operation of the owner View.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     */
    CommitItemBefore(tblSysDataItem) {
        let Row = tblSysDataItem.Rows[0];
 
        this.BrokerDef.Name = Row.Get('DataName', '');
        this.BrokerDef.TitleKey = Row.Get('TitleKey', ''); 

        let JsonText = tp.ToJson(this.BrokerDef, true);
 
        //Row.Set('Data1', JsonText);
    }
    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @returns {boolean} Returns true if commit is allowed, else false.
     */
    CanCommitItem(tblSysDataItem) {
        return true;
    }

    /**
    Event handler. Called by the owner View when the Edit Pager changes Page.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    EditPager_PageChanged(Args) {
        let CurrentPage = this.View.pagerEdit.SelectedPage;
        if (CurrentPage === this.tabGeneral) {

            let SourceRow = this.View.tblSysDataItem.Rows[0];
            let Row = this.tblBrokerDef.Rows[0];

            Row.Set('Name', SourceRow.Get('DataName', ''));
            Row.Set('TitleKey', SourceRow.Get('TitleKey', ''));
        }
    }


    /** Called when inserting a single row of the tblSelectSqlList and displays the edit dialog
    */
    async InsertSelectSqlRow() {

        let Instance = new tp.SelectSql();

        let DialogBox = await tp.SelectSqlEditDialog.ShowModalAsync(Instance);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblSelectSqlList.AddEmptyRow();
            Row.Set('Name', Instance.Name);
            Row.Set('ConnectionName', Instance.ConnectionName);
            Row.Set('CompanyAware', Instance.CompanyAware);
            Row.OBJECT = Instance;
            this.BrokerDef.SelectSqlList.push(Instance);
        }
    }
    /** Called when editing a single row of the tblSelectSqlList and displays the edit dialog
     */
    async EditSelectSqlRow() {       
        let Row = this.gridSelectSqlList.FocusedRow;
        if (tp.IsValid(Row)) {
            let Instance = Row.OBJECT;
            let DialogBox = await tp.SelectSqlEditDialog.ShowModalAsync(Instance);
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Row.Set('Name', Instance.Name);
                Row.Set('ConnectionName', Instance.ConnectionName);
                Row.Set('CompanyAware', Instance.CompanyAware);
            }
        } 
    }
    /** Deletes a single row of the tblSelectSqlList 
     */
    async DeleteSelectSqlRow() {
        let Row = this.gridSelectSqlList.FocusedRow;
        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                let Instance = Row.OBJECT;
                tp.ListRemove(this.BrokerDef.SelectSqlList, Instance);
                this.tblSelectSqlList.RemoveRow(Row);
            } 
        }
    }

    /** Called when inserting a single row of the tblQueryList and displays the edit dialog
    */
    async InsertQuerylRow() {
        let Instance = new tp.SqlBrokerQueryDef();

        let DialogBox = await tp.SqlBrokerQueryDefEditDialog.ShowModalAsync(Instance);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblQueryList.AddEmptyRow();
            Row.Set('Name', Instance.Name);
            Row.OBJECT = Instance;
            this.BrokerDef.Queries.push(Instance);
        }
    }
    /** Called when editing a single row of the tblQueryList and displays the edit dialog
     */
    async EditQueryRow() {
        let Row = this.gridQueryList.FocusedRow;
        if (tp.IsValid(Row)) {
            let Instance = Row.OBJECT;
            let DialogBox = await tp.SqlBrokerQueryDefEditDialog.ShowModalAsync(Instance);
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Row.Set('Name', Instance.Name); 
            }
        } 
    }
    /** Deletes a single row of the tblQueryList
     */
    async DeleteQueryRow() {
        let Row = this.gridQueryList.FocusedRow;
        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                let Instance = Row.OBJECT;
                tp.ListRemove(this.BrokerDef.Queries, Instance);
                this.tblQueryList.RemoveRow(Row);
            }
        }
    }


    /** Called when inserting a single row of the tblTableList and displays the edit dialog
    */
    async InsertTableRow() {
 
        let Instance = new tp.SqlBrokerTableDef();

        let DialogBox = await tp.SqlBrokerTableDefEditDialog.ShowModalAsync(Instance);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblTableList.AddEmptyRow();
            Row.Set('Name', Instance.Name);
            Row.Set('Alias', Instance.Alias);
            Row.Set('TitleKey', Instance.TitleKey);
            Row.Set('PrimaryKeyField', Instance.PrimaryKeyField);
            Row.Set('MasterTableName', Instance.MasterTableName);
            Row.Set('MasterKeyField', Instance.MasterKeyField);
            Row.Set('DetailKeyField', Instance.DetailKeyField);
            Row.OBJECT = Instance;
            this.BrokerDef.Tables.push(Instance);
        } 
    }
    /** Called when editing a single row of the tblTableList and displays the edit dialog
     */
    async EditTableRow() {
        let Row = this.gridTableList.FocusedRow;
        if (tp.IsValid(Row)) {
            let Instance = Row.OBJECT;
            let DialogBox = await tp.SqlBrokerTableDefEditDialog.ShowModalAsync(Instance);
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Row.Set('Name', Instance.Name);
                Row.Set('Alias', Instance.Alias);
                Row.Set('TitleKey', Instance.TitleKey);
                Row.Set('PrimaryKeyField', Instance.PrimaryKeyField);
                Row.Set('MasterTableName', Instance.MasterTableName);
                Row.Set('MasterKeyField', Instance.MasterKeyField);
                Row.Set('DetailKeyField', Instance.DetailKeyField);
            }
        } 
    }
    /** Deletes a single row of the tblTableList
     */
    async DeleteTableRow() {
        let Row = this.gridTableList.FocusedRow;
        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                let Instance = Row.OBJECT;
                tp.ListRemove(this.BrokerDef.Tables, Instance);
                this.tblTableList.RemoveRow(Row);
            }
        } 
    }

    /* event handlers */
    /** Event handler
     * @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
     */
    AnyGridButtonClick(Args) {
        Args.Handled = true;

        let NameTag = Args.Sender.NameTag;

        if (NameTag === 'gridSelectSqlList') {
            switch (Args.Command) {
                case 'GridRowInsert':
                    this.InsertSelectSqlRow();
                    break;
                case 'GridRowEdit':
                    this.EditSelectSqlRow();
                    break;
                case 'GridRowDelete':
                    this.DeleteSelectSqlRow();
                    break;
            }
        }
        else if (NameTag === 'gridQueryList') {
            switch (Args.Command) {
                case 'GridRowInsert':
                    this.InsertQuerylRow();
                    break;
                case 'GridRowEdit':
                    this.EditQueryRow();
                    break;
                case 'GridRowDelete':
                    this.DeleteQueryRow();
                    break;
            }
        }
        
        else if (NameTag === 'gridTableList') {
            switch (Args.Command) {
                case 'GridRowInsert':
                    this.InsertTableRow(); 
                    break;
                case 'GridRowEdit':
                    this.EditTableRow();
                    break;
                case 'GridRowDelete':
                    this.DeleteTableRow();
                    break;
            }
        }    

    }
    /**
    Event handler
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    async AnyGridDoubleClick(Args) {

        Args.Handled = true;

        let NameTag = Args.Sender.NameTag;

        if (NameTag === 'gridSelectSqlList') {
            await this.EditSelectSqlRow();
        }
        else if (NameTag === 'gridQueryList') {
            await this.EditQueryRow();
        }
        else if (NameTag === 'gridTableList') {
            await this.EditTableRow();
        }
    }

   
    

};
//#endregion


