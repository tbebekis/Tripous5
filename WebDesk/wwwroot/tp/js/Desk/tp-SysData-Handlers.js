//---------------------------------------------------------------------------------------
// SysData handler classes
//---------------------------------------------------------------------------------------

//#region SysDataHandler

/** The ancestor of all classes that handle {@link tp.SysDataItem} instances.
 * The {@link tp.DeskSysDataView} selects the handler to use, according to the DataType, e.g. Broker, Table, Locator, etc.
 * A concrete handler knows how to handle the {@link tp.SysDataItem} of the specified DataType, 
 * i.e. insert, edit, delete, etc.
 */
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
    /** Called by the InsertItemAfter() and EditItemAfter() methods. It either creates a new xxxxDef instance or loads an existing one.
     * Creates and assigns the tblFields. Sets tblFields a grid's data-source.
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
        tblField.Name = 'Field';                // WARNING: This name is used to differentiate the DataTables in DatabaseFieldEditDialog()

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

 
    /** Called when inserting a single row of the tblFields and displays the edit dialog 
     */
    async InsertFieldRow() {
        let tblField = this.CreateEditFieldTable(null);
        let DialogBox = await tp.DatabaseFieldEditDialog.ShowModalAsync(tblField, true);  
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
                let DialogBox = await tp.DatabaseFieldEditDialog.ShowModalAsync(tblField, false);  

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
                this.gridFields.DeleteFocusedRow(); 
                // EDW : continue adding GridRowDelete for all grids
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

//#region SysDataHandlerBroker
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
    /** Called by the InsertItemAfter() and EditItemAfter() methods. It either creates a new xxxxDef instance or loads an existing one.
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

            // Tables Page
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

//#region SysDataHandlerLocator
tp.SysDataHandlerLocator = class extends tp.SysDataHandler {
    /** Constructor
     * @param {tp.View} View The view that calls and owns this handler
     */
    constructor(View) {
        super(View, 'Locator')
    }


    /** A C# LocatorDef instance as it comes from server. <br /> 
     * @type {tp.LocatorDef}
     */
    LocatorDef = null;
    /**
     * @type {tp.DataTable}
     */
    tblLocatorDef = null;
    /**
     * @type {tp.DataSource}
     */
    dsLocatorDef = null;
 
    /** The broker General tab page
     * @type {tp.TabPage}
     */
    tabGeneral = null;
    /** The Sql tab page
     * @type {tp.TabPage}
     */
    tabSql = null;
    /** The element upon Ace Editor is created. 
     * The '__Editor' property of the element points to Ace Editor object.
     * @type {HTMLElement}
     */
    elSqlEditor = null; 

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

            let LayoutRow,  // A responsive row which is the container of controls bound to tblLocatorDef.
                el, CP;

            // General Page
            // ---------------------------------------------------------------------------------
            // add a tp.TabPage to View's pagerEdit
            this.tabGeneral = this.View.pagerEdit.AddPage(_L('General'));
            tp.Data(this.tabGeneral.Handle, 'Name', 'General');

            // create the data table
            this.tblLocatorDef = tp.LocatorDef.CreateDataTable();
            this.tblLocatorDef.Name = 'LocatorDef';
 
            let HtmlText;
            let HtmlRowList = [];

            // for each table field, produce html text for control rows and add the text to a string-list
            this.tblLocatorDef.Columns.forEach((Column) => {
                if (Column.Name !== 'SqlText') {
                    let IsCheckBox = Column.DataType === tp.DataType.Boolean;

                    let Text = Column.Title;
                    let Ctrl = {
                        TypeName: tp.DataTypeToUiType(Column.DataType),
                        TableName: this.tblLocatorDef.Name,
                        DataField: Column.Name
                    };

                    // <div class="tp-CtrlRow" data-setup="{Text: 'Id', Control: { TypeName: 'TextBox', Id: 'Code', DataField: 'Code', ReadOnly: true } }"></div>
                    HtmlText = tp.CtrlRow.GetHtml(IsCheckBox, Text, Ctrl);
                    HtmlRowList.push(HtmlText)
                }
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

            this.dsLocatorDef = new tp.DataSource(this.tblLocatorDef);

            tp.BindAllDataControls(elRow, () => { return this.dsLocatorDef; });  

            // SQL Page
            // ---------------------------------------------------------------------------------
            this.tabSql = this.View.pagerEdit.AddPage('Sql');
            this.elSqlEditor = tp.CreateSourceCodeEditor(this.tabSql.Handle, 'sql', '');

            // Fields Page
            // ---------------------------------------------------------------------------------
            // add a tp.TabPage to View's pagerEdit
            let FieldsPage = this.View.pagerEdit.AddPage('Fields');
            tp.Data(FieldsPage.Handle, 'Name', 'Fields');

            // add a tp.Row to the tab page
            let Row = new tp.Row(null, { Height: '100%' });
            FieldsPage.AddComponent(Row);

            // add a DIV for the gridFields tp.Grid in the row
            el = Row.AddDivElement();
            CP = {
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
                    { Name: 'TableName' },
                    { Name: 'DataField' },
                    { Name: 'DataType', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]) },

                    { Name: 'TitleKey' },

                    { Name: 'Visible' },
                    { Name: 'Searchable' },
                    { Name: 'ListVisible' },
                    { Name: 'IsIntegerBoolean' },

                    { Name: 'Width' },
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
        let Row = this.tblLocatorDef.Rows[0];
        Row.Set('SqlText', this.elSqlEditor.__Editor.getValue());
        this.LocatorDef.FromDataRow(Row);
        this.LocatorDef.FieldsFromDataTable(this.tblFields);

        let JsonText = tp.ToJson(this.LocatorDef, true);
        Row = tblSysDataItem.Rows[0];
        Row.Set('DataName', this.LocatorDef.Name);
        Row.Set('TitleKey', this.LocatorDef.TitleKey);
        Row.Set('Data1', JsonText);
    }
    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @returns {boolean} Returns true if commit is allowed, else false.
     * @override
     */
    CanCommitItem(tblSysDataItem) {
        let List = this.LocatorDef.GetDescriptorErrors();
        if (List.length === 0)
            return true;

        let S = List.join('\n');
        tp.ErrorBoxAsync(S);
        return false;
    }

    /**
    Event handler. Called by the owner View when the Edit Pager changes Page.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    EditPager_PageChanged(Args) {
    }

    /* private */
    /** Called by the InsertItemAfter() and EditItemAfter() methods. It either creates a new xxxxDef instance or loads an existing one.
     * @param {boolean} IsInsertItem True when is an Insert new SysDataItem operation. False when is an Edit an existing SysDataItem operation.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * */
    SetupItem(IsInsertItem, tblSysDataItem) {
        let Row;

        this.View.FindControlByDataField('DataName').ReadOnly = true;
        this.View.FindControlByDataField('TitleKey').ReadOnly = true;
 

        // create an empty Def instance
        this.LocatorDef = new tp.LocatorDef();

        // if editing an already existing definition
        // then read the json from Data1 field and load the Def instance
        if (IsInsertItem === false) {
            let Text = tblSysDataItem.Rows[0].Get('Data1');
            let Source = eval("(" + Text + ")");
            log(Source);
            this.LocatorDef.Assign(Source);

            this.elSqlEditor.__Editor.setValue(this.LocatorDef.SqlText, -1);
        }
        else {
            // Insert
            this.elSqlEditor.__Editor.setValue('', -1);
        }

        this.tblLocatorDef.ClearRows();
        this.tblLocatorDef.AcceptChanges();

        Row = this.tblLocatorDef.AddEmptyRow();

        this.LocatorDef.ToDataRow(Row); 

        // create the tblFields
        this.tblFields = this.LocatorDef.FieldsToDataTable();
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
        let Row;
        let IsInsertField = tp.IsEmpty(SourceRow);

        // create the tblField, used in editing a single field
        let tblField = this.tblFields.Clone();
        tblField.Name = 'Field';    // WARNING: This name is used to differentiate the DataTables in LocatorFieldEditDialog()

        // add the single row in tblField
        Row = tblField.AddEmptyRow();

        if (IsInsertField) { 
            Row.Set('DataType', tp.DataType.String);
 
            Row.Set('Visible', true);
            Row.Set('Searchable', true);
            Row.Set('ListVisible', true);
            Row.Set('IsIntegerBoolean', false);

            Row.Set('Width', 70);
 
        }
        else {
            Row.CopyFromRow(SourceRow);
        }

        return tblField;
    }
 
    /** Called when inserting a single row of the tblFields and displays the edit dialog 
     */
    async InsertFieldRow() {
        let tblField = this.CreateEditFieldTable(null);
        let DialogBox = await tp.LocatorFieldEditDialog.ShowModalAsync(tblField, true);  
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
            let tblField = this.CreateEditFieldTable(Row);
            let DialogBox = await tp.LocatorFieldEditDialog.ShowModalAsync(tblField, false);  

            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                let FieldRow = tblField.Rows[0];
                Row.CopyFromRow(FieldRow);
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

//#region SysDataHandlerCodeProvider
tp.SysDataHandlerCodeProvider = class extends tp.SysDataHandler {
    /** Constructor
     * @param {tp.View} View The view that calls and owns this handler
     */
    constructor(View) {
        super(View, 'CodeProvider')
    }


    /** A C# CodeProviderDef instance as it comes from server. <br />
     * @type {tp.CodeProviderDef}
     */
    CodeProviderDef = null;
    /**
     * @type {tp.DataTable}
     */
    tblCodeProviderDef = null;
    /**
     * @type {tp.DataSource}
     */
    dsCodeProviderDef = null;

    /** The General tab page
     * @type {tp.TabPage}
     */
    tabGeneral = null;

    /* overrides */
    /** If not already created, then creates any control this handler needs in order to edit a SysDataItem.
     * @override
    * */
    CreateEditControls() {
        if (this.View.pagerEdit.GetPageCount() === 1) {

            let LayoutRow,  // A responsive row which is the container of controls bound to table.
                el, CP;

            // General Page
            // ---------------------------------------------------------------------------------
            // add a tp.TabPage to View's pagerEdit
            this.tabGeneral = this.View.pagerEdit.AddPage(_L('General'));
            tp.Data(this.tabGeneral.Handle, 'Name', 'General');

            // create the data table
            this.tblCodeProviderDef = tp.CodeProviderDef.CreateDataTable();
            this.tblCodeProviderDef.Name = 'CodeProviderDef';

            let HtmlText;
            let HtmlRowList = [];

            // for each table field, produce html text for control rows and add the text to a string-list
            this.tblCodeProviderDef.Columns.forEach((Column) => {
                let IsCheckBox = Column.DataType === tp.DataType.Boolean;

                let Text = Column.Title;
                let Ctrl = {
                    TypeName: tp.DataTypeToUiType(Column.DataType),
                    TableName: this.tblCodeProviderDef.Name,
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

            this.dsCodeProviderDef = new tp.DataSource(this.tblCodeProviderDef);

            tp.BindAllDataControls(elRow, () => { return this.dsCodeProviderDef; });
             
 
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
        let Row = this.tblCodeProviderDef.Rows[0]; 
        this.CodeProviderDef.FromDataRow(Row); 

        let JsonText = tp.ToJson(this.CodeProviderDef, true);
        Row = tblSysDataItem.Rows[0];
        Row.Set('DataName', this.CodeProviderDef.Name);
        Row.Set('TitleKey', this.CodeProviderDef.Name);
        Row.Set('Data1', JsonText);
    }
    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * @returns {boolean} Returns true if commit is allowed, else false.
     * @override
     */
    CanCommitItem(tblSysDataItem) {
        let List = this.CodeProviderDef.GetDescriptorErrors();
        if (List.length === 0)
            return true;

        let S = List.join('\n');
        tp.ErrorBoxAsync(S);
        return false;
    }

    /**
    Event handler. Called by the owner View when the Edit Pager changes Page.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    EditPager_PageChanged(Args) {
    }

    /* private */
    /** Called by the InsertItemAfter() and EditItemAfter() methods. It either creates a new xxxxDef instance or loads an existing one.
     * @param {boolean} IsInsertItem True when is an Insert new SysDataItem operation. False when is an Edit an existing SysDataItem operation.
     * @param {tp.DataTable} tblSysDataItem The Edit (data) table. Results from a convertion of a {@link tp.SysDataItem} to a {@link tp.DataTable}.
     * */
    SetupItem(IsInsertItem, tblSysDataItem) {

        this.View.FindControlByDataField('DataName').ReadOnly = true;
        this.View.FindControlByDataField('TitleKey').ReadOnly = true;

        // create an empty Def instance
        this.CodeProviderDef = new tp.CodeProviderDef();

        // if editing an already existing definition
        // then read the json from Data1 field and load the Def instance
        if (IsInsertItem === false) {
            let Text = tblSysDataItem.Rows[0].Get('Data1');
            let Source = eval("(" + Text + ")");
            log(Source);
            this.CodeProviderDef.Assign(Source);
        } 


        let Row;

        this.tblCodeProviderDef.ClearRows();
        this.tblCodeProviderDef.AcceptChanges();

        Row = this.tblCodeProviderDef.AddEmptyRow();

        this.CodeProviderDef.ToDataRow(Row);

    }
}
//#endregion