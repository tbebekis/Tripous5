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
            Table = new tp.DataTable();
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
     * @type {tp.Grid}
     */
    gridSelectSqlList = null;
    /**
     * @type {tp.DataTable}
     */
    tblSelectSqlList = null;  

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
        // if editing an already existing table definition
        // then read the json from Data1 field and load the TableDef
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
                Name: "gridSelectSqlList",
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

            // create the columns grid
            this.gridSelectSqlList = new tp.Grid(el, CP);

            this.tblSelectSqlList = new tp.DataTable();
            this.tblSelectSqlList.AddColumn('Name').DefaultValue = '';
            this.tblSelectSqlList.AddColumn('ConnectionName').DefaultValue = tp.SysConfig.DefaultConnection;
            this.tblSelectSqlList.AddColumn('CompanyAware', tp.DataType.Boolean).DefaultValue = false;
 
            this.tblSelectSqlList.AcceptChanges();

            this.gridSelectSqlList.DataSource = this.tblSelectSqlList;
            this.gridSelectSqlList.BestFitColumns();

            this.gridSelectSqlList.On("ToolBarButtonClick", this.GridSelectSqlList_AnyButtonClick, this);
            this.gridSelectSqlList.On(tp.Events.DoubleClick, this.GridSelectSqlList_DoubleClick, this);
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
 
        this.BrokerDef.Name.Name = Row.Get('DataName', '');
        this.BrokerDef.Name.TitleKey = Row.Get('TitleKey', ''); 

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

        let SS = new tp.SelectSql();

        let DialogBox = await tp.SelectSqlEditDialog.ShowModalAsync(SS);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblSelectSqlList.AddEmptyRow();
            Row.Set('Name', SS.Name);
            Row.Set('ConnectionName', SS.ConnectionName);
            Row.Set('CompanyAware', SS.CompanyAware);
            Row.SelectSql = SS;
            this.BrokerDef.SelectSqlList.push(SS);
        }
    }
    /** Called when editing a single row of the tblSelectSqlList and displays the edit dialog
     */
    async EditSelectSqlRow() {       
        let Row = this.gridSelectSqlList.FocusedRow;
        if (tp.IsValid(Row)) {
            let SS = Row.SelectSql;
            let DialogBox = await tp.SelectSqlEditDialog.ShowModalAsync(SS);
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Row.Set('Name', SS.Name);
                Row.Set('ConnectionName', SS.ConnectionName);
                Row.Set('CompanyAware', SS.CompanyAware);
            }
        } 
    }
    /** Deletes a single row of the tblSelectSqlList 
     */
    DeleteSelectSqlRow() {
        let Row = this.gridSelectSqlList.FocusedRow;
        if (tp.IsValid(Row)) {
            tp.YesNoBox('Delete selected row?', (Dialog) => {
                if (Dialog.DialogResult === tp.DialogResult.Yes) {
                    let SS = Row.SelectSql;
                    tp.ListRemove(this.BrokerDef.SelectSqlList, SS);
                    this.tblBrokerDef.RemoveRow(Row);
                }
            });
        }
    }


    /* event handlers */
    /** Event handler
     * @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
     */
    GridSelectSqlList_AnyButtonClick(Args) {
        Args.Handled = true;

        switch (Args.Command) {
            case 'GridRowInsert':
                this.InsertSelectSqlRow();
                break;
            case 'GridRowEdit':
                this.EditSelectSqlRow();
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
    GridSelectSqlList_DoubleClick(Args) {
        this.EditSelectSqlRow();
    }

    // EDW next
    //      Queries
    //      Tables

};
//#endregion

//#region tp.DeskSysDataView

/** Represents a view. Displays a list of items of a certain DataType. */
tp.DeskSysDataView = class extends tp.DeskView {
    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [Params=null] Optional. A javascript object with initialization parameters.
     */
    constructor(elPage, CreateParams = null) {
        super(elPage, CreateParams);
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

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.DeskSysDataView';
        this.fDefaultCssClasses = [tp.Classes.View, tp.Classes.DeskSysDataView];
    }
    /**
    Initializes fields and properties just before applying the create params.      
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        this.fName = tp.NextName('DeskSysDataView');
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

        this.DataType = this.CreateParams.DataType
        switch (this.DataType) {
            case 'Table':
                this.Handler = new tp.SysDataHandlerTable(this);
                break;
            case 'Broker':
                this.Handler = new tp.SysDataHandlerBroker(this);
                break;
            default:
                tp.Throw(`SysData DataType not supported: ${this.DataType}`);
                break;
        }

        this.CreateToolBar();
        this.CreateTabControl();
        this.CreateListGrid();

        this.ListSelect();
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


    /**
    Returns the Id (value of the primary key field) of the selected data-row of the List (browser) grid, if any, else null.
    @protected
    @returns {any} Returns the Id (value of the primary key field) of the selected data-row of the browser grid, if any, else null.
    */
    GetListSelectedId() {

        if (!tp.IsBlank(this.PrimaryKeyField) && !tp.IsEmpty(this.gridList)) {
            var Row = this.gridList.FocusedRow;
            if (!tp.IsEmpty(Row) && Row.Table.ContainsColumn(this.PrimaryKeyField)) {
                return Row.Get(this.PrimaryKeyField);
            }
        }

        return null;
    }

    /* overridables */
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
            this.gridList.GroupsVisible = false;
            this.gridList.GroupFooterVisible = false;
            this.gridList.AutoGenerateColumns = false;

            this.gridList.AddColumn('Owner');
            //this.gridList.AddColumn('DataType');
            this.gridList.AddColumn('DataName');
            this.gridList.AddColumn('TitleKey');

            this.gridList.AddColumn('Tag1');
            this.gridList.AddColumn('Tag2');
            this.gridList.AddColumn('Tag3');
            this.gridList.AddColumn('Tag4');

            this.gridList.On(tp.Events.DoubleClick, this.ListGrid_DoubleClick, this);
        }


    }

    /** Creates and binds the controls of the edit part, if not already created.
    * */
    CreateEditControls() {
        if (!tp.IsValid(this.pagerEdit)) {
            let el = this.GetEditPageElement();

            let ControlList = tp.Ui.CreateContainerControls(el);
            this.pagerEdit = ControlList.find(item => item instanceof tp.TabControl);

            this.Handler.CreateEditControls();

            if (this.pagerEdit.GetPageCount() === 1) {
                this.pagerEdit.ShowTabBar(false);   // hide tab-bar if we have only a single page
            }

            this.pagerEdit.On('SelectedIndexChanged', this.pagerEdit_PageChanged, this);

        }
    }
    /**
    Displays the main local menu
    @protected
    */
    DisplayHomeLocalMenu() {
    }
    // commands/modes
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
 
    /** Returns true if can commit changes, else false.
     * @returns {boolean} Returns true if can commit changes, else false.
     * */
    CanCommit() {
        if (!this.Handler.CanCommitItem(this.tblSysDataItem))
            return false;

        let Row = this.tblSysDataItem.Rows[0];
        let v = Row.Get('DataName', '');
        if (!tp.IsValid(v) || tp.IsBlankString(v)) {
            tp.ErrorNote('Cannot save changes.\nNo value in field: DataName');
            return false;
        }

        return true;
    }

    async ListSelect() {
        let Url = tp.Urls.SysDataSelectList;
        let Data = {
            DataType: this.DataType,
            NoBlobs: true
        }

        let Args = await tp.Ajax.GetAsync(Url, Data);
        this.tblList = new tp.DataTable();
        this.tblList.Assign(Args.Packet);

        this.dsList = new tp.DataSource(this.tblList);
        this.gridList.DataSource = this.dsList;

        this.ForceSelect = false;
        this.ViewMode = tp.DataViewMode.List;
    }
 
    async Insert() {
        this.CreateEditControls();        

        this.pagerEdit.SelectedIndex = 0;

        this.Handler.InsertItemBefore();

        let Item = new tp.SysDataItem();
        this.tblSysDataItem = Item.ToDataTable();

        this.tblSysDataItem.SetColumnListReadOnly(['DataType', 'Owner']);
        let Row = this.tblSysDataItem.RowCount === 0 ? this.tblSysDataItem.AddEmptyRow() : this.tblSysDataItem.Rows[0];

        Row.Set('DataType', this.DataType);
        Row.Set('Owner', 'App');
        Row.Set('Tag4', 'Custom');

        this.DataSources.length = 0;
        this.DataSources.push(new tp.DataSource(this.tblSysDataItem));

        let DataControlList = this.GetDataControlList();
        this.BindControls(DataControlList);

        this.Handler.InsertItemAfter(this.tblSysDataItem);

        this.ViewMode = tp.DataViewMode.Insert;
    }
    async Edit() {
        this.CreateEditControls();

        let Id = this.GetListSelectedId();

        if (tp.IsEmpty(Id)) {
            tp.ErrorNote('No selected row');
        } else {
            let Url = tp.Urls.SysDataSelectItemById;
            let Data = {
                Id: Id
            }

            this.Handler.EditItemBefore(Id);

            let Args = await tp.Ajax.GetAsync(Url, Data);

            log(Args.Packet);

            let Item = new tp.SysDataItem(Args.Packet);
            this.tblSysDataItem = Item.ToDataTable();

            this.tblSysDataItem.SetColumnListReadOnly(['DataType', 'Owner']);

            this.DataSources.length = 0;
            this.DataSources.push(new tp.DataSource(this.tblSysDataItem));

            let DataControlList = this.GetDataControlList();
            this.BindControls(DataControlList);

            this.Handler.EditItemAfter(Id, this.tblSysDataItem);

            this.ViewMode = tp.DataViewMode.Edit;
        }
    }
    async Delete() {

    }
    async Commit() {

        // default values
        let Row = this.tblSysDataItem.Rows[0];
        let v = Row.Get('TitleKey', '');
        if (!tp.IsValid(v) || tp.IsBlankString(v))
            Row.Set('TitleKey', Row.Get('DataName', ''));

        this.Handler.CommitItemBefore(this.tblSysDataItem);

        // checks
        if (!this.CanCommit())
            return;        

        let Item = new tp.SysDataItem();
        Item.FromDataTable(this.tblSysDataItem);

        let Url = tp.Urls.SysDataSaveItem;
 
        let Args = await tp.Ajax.PostModelAsync(Url, Item);

        if (Args.ResponseData.IsSuccess === true) {
            tp.SuccessNote('OK');
        }

        this.ForceSelect = true;
        this.ViewMode = tp.DataViewMode.Edit;
    }




    /* data-binding */
    /**
    Finds and returns a {@link tp.DataSource} data-source by name, if any, else null. <br />
    @protected
    @param {string} SourceName The data-source by name
    @returns {tp.DataSource} Returns a {@link tp.DataSource} data-source or null
    */
    GetDataSource(SourceName) {
        return super.GetDataSource(SourceName);
    }
    /** Returns the list of data controls.
     * @returns {tp.Control[]}  Returns the list of data controls.
     * */
    GetDataControlList() {
        let ElementList = this.pagerEdit.GetPageElementList();
        let Result = tp.GetAllDataControls(ElementList[0]);     // controls bound to SysData table are in the first page only
        return Result;
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
            if (!tp.IsBlank(Command)) {
                this.ExecuteCommand(Command);
            }
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
    /**
    Event handler. Called when the Edit Pager changes Page.
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    pagerEdit_PageChanged(Args) {
        if (this.Handler) {
            this.Handler.EditPager_PageChanged(Args);
        }
    }
};

tp.DeskSysDataView.prototype.PrimaryKeyField = 'Id';
/** The DataType, i.e. Table, Broker, Report, etc.
 * @type {string}
 */
tp.DeskSysDataView.prototype.DataType = '';
/** The tool-bar
 * @type {tp.ToolBar}
 */
tp.DeskSysDataView.prototype.ToolBar = null;
/** Field
 @protected
 @type {tp.TabControl}
 */
tp.DeskSysDataView.prototype.MainPager = null;

/** The List (browser) grid
 * @type {tp.Grid}
 */
tp.DeskSysDataView.prototype.gridList = null;
/** Field
 @protected
 @type {tp.DataSource}
 */
tp.DeskSysDataView.prototype.dsList = null;
/** The List (browser) table
 * @type {tp.DataTable}
 */
tp.DeskSysDataView.prototype.tblList = null;
/** The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
 * @type {tp.DataTable}
 */
tp.DeskSysDataView.prototype.tblSysDataItem = null;
/** Field. One of the  {@link tp.DataViewMode} constants
 @protected
 @type {number}
 */
tp.DeskSysDataView.prototype.fViewMode = tp.DataViewMode.None;
/** Field. One of the  {@link tp.DataViewMode} constants
 @protected
 @type {number}
 */
tp.DeskSysDataView.prototype.fLastViewMode = tp.DataViewMode.None;
/** Field
@protected
@type {boolean}
*/
tp.DeskSysDataView.prototype.ForceSelect = false;
/** The {@link tp.TabControl} in the Edit part. That control is the container of the edit controls. Its first page contains the data-bound controls.
@protected
@type {tp.TabControl}
*/
tp.DeskSysDataView.prototype.pagerEdit = null;
/** An object that handles a specific DataType, e.g. Table, Broker, Report, etc.
@protected
@type {tp.SysDataHandler}
*/
tp.DeskSysDataView.prototype.Handler = null;


//#endregion
