﻿//#region DataTableDef

/** Database table definition
 * */
tp.DataTableDef = class {

    /** Constructor.
     * Assigns this instance's properties from a specified source, if not null.
     * @param {objec} Source Optional.
     */
    constructor(Source = null) {
        this.fTitle = '';
        this.fTitleKey = '';
        this.Fields = [];
        this.UniqueConstraints = [];

        this.Assign(Source);
    }

    /** A name unique among all instances of this type
     * @type {string}
     */
    Name = '';
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

    /** The list of fields
     * @type {tp.DataFieldDef[]}
     */
    Fields = [];
    /** For multi-field unique constraints.
     * Use it when a unique constraint is required on more than a single field adding a proper string, e.g. Field1, Field2
     * @type {string[]}
     */
    UniqueConstraints = [];

    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

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

        if (tp.IsArray(Source.UniqueConstraints))
            this.UniqueConstraints = Source.UniqueConstraints;
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
            Table.AddColumn('Name', tp.DataType.String, 32);
            Table.AddColumn('TitleKey', tp.DataType.String, 96);
            Table.AddColumn('IsPrimaryKey', tp.DataType.Boolean);
            Table.AddColumn('DataType', tp.DataType.String, 40);
            Table.AddColumn('Length', tp.DataType.Integer);
            Table.AddColumn('Required', tp.DataType.Boolean);
            Table.AddColumn('DefaultValue');
            Table.AddColumn('Unique', tp.DataType.Boolean);
            Table.AddColumn('ForeignTableName', tp.DataType.String, 32);
            Table.AddColumn('ForeignFieldName', tp.DataType.String, 32);
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

/** Database table field definition
 * */
tp.DataFieldDef = class {

    /** Constructor.
     * Assigns this instance's properties from a specified source, if not null.
     * @param {objec} Source Optional.
     */
    constructor(Source = null) {
        this.fTitle = '';
        this.fTitleKey = '';

        this.Assign(Source);
    }

    /** A name unique among all instances of this type
     * @type {string}
     */
    Name = '';
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

    /** True when the field is a primary key
     * @type {boolean}
     */
    IsPrimaryKey = false;
    /** The data-type of the field. One of the {@link tp.DataType} constants.
     * @type {string}
     */
    DataType = tp.DataType.Unknown;
    /** Field length. Applicable to varchar fields only.
     * @type {number}
     */
    Length = 0;
    /** True when the field is NOT nullable.
     * NOTE: when true then produces 'not null'
     * @type {boolean}
     */
    Required = false;
    /** The default expression, if any. E.g. 0, or ''. Defaults to null.
     * NOTE:  e.g. produces default 0, or default ''
     * @type {object}
     */
    DefaultValue = null;
    /** When true denotes a field upon which a unique constraint is applied
     * @type {boolean}
     */
    Unique = false;

    /** When not empty is the name of a foreign table which this field references.
     * NOTE: Used in creating a foreign key constraint.
     * @type {string}
     */
    ForeignTableName = '';
    /** When not empty is the name of a foreign field which this field references.
     * NOTE: Used in creating a foreign key constraint.
     * @type {string}
     */
    ForeignFieldName = '';

    /** Assigns this instance's properties from a specified source.
     * @param {objec} Source
     */
    Assign(Source) {
        if (!tp.IsValid(Source))
            return;

        this.Name = Source.Name || '';
        this.Title = Source.Title || '';
        this.TitleKey = Source.TitleKey || '';

        this.IsPrimaryKey = Source.IsPrimaryKey === true;
        this.DataType = Source.DataType;
        this.Length = Source.Length;
        this.Required = Source.Required === true;
        this.DefaultValue = Source.DefaultValue;
        this.Unique = Source.Unique === true;

        this.ForeignTableName = Source.ForeignTableName || '';
        this.ForeignFieldName = Source.ForeignFieldName || '';
    }

    /** Loads this instance's properties from a specified {@link tp.DataRow}
     * @param {tp.DataRow} Row The {@link tp.DataRow} to load from.
     */
    FromDataRow(Row) {
        if (Row instanceof tp.DataRow) {
            this.Name = Row.Get('Name', '');
            this.Title = Row.Get('Title', '');
            this.TitleKey = Row.Get('TitleKey', '');
            this.IsPrimaryKey = Row.Get('IsPrimaryKey', false);
            this.DataType = Row.Get('DataType', tp.DataType.String);
            this.Length = Row.Get('Length', 0);
            this.Required = Row.Get('Required', false);
            this.DefaultValue = Row.Get('DefaultValue', null);
            this.Unique = Row.Get('Unique', false);
            this.ForeignTableName = Row.Get('ForeignTableName', '');
            this.ForeignFieldName = Row.Get('ForeignFieldName', '');
        }
    }
    /** Saves this instance's properties to a specified {@link tp.DataRow}
     * @param {tp.DataRow}  Row The {@link tp.DataRow} to save to.
     */
    ToDataRow(Row) {
        Row.Set('Name', this.Name);
        Row.Set('Title', this.Title);
        Row.Set('TitleKey', this.TitleKey);
        Row.Set('IsPrimaryKey', this.IsPrimaryKey);
        Row.Set('DataType', this.DataType);
        Row.Set('Length', this.Length);
        Row.Set('Required', this.Required);
        Row.Set('DefaultValue', this.DefaultValue);
        Row.Set('Unique', this.Unique);
        Row.Set('ForeignTableName', this.ForeignTableName);
        Row.Set('ForeignFieldName', this.ForeignFieldName);
    }
};

tp.DataFieldDef.prototype.fTitle = '';
tp.DataFieldDef.prototype.fTitleKey = '';

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


    /** Called before the Insert() operation of the owner View.
    */
    InsertItemBefore() {
    }
    /** Called after the Insert() operation of the owner View.
     * @param {tp.DataTable} tblData
     */
    InsertItemAfter(tblData) {
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
     * @param {tp.DataTable} tblData
     */
    EditItemAfter(Id, tblData) {
    }

    /** Called before the Commit() operation of the owner View.
     * @param {tp.DataTable} tblData
     */
    CommitItemBefore(tblData) {
    }

    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblData
     * @returns {boolean} Returns true if commit is allowed, else false.
     */
    CanCommitItem(tblData) {
        return true;
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
     * @param {string} DataType The SysData DataType this handler can handle.
     */
    constructor(View, DataType = 'Table') {
        super(View, DataType)
    }

    /** A {@link tp.DataTable} for handling the fields of a table def.
     @type {tp.DataTable}
     */
    tblFields = null;
    /** A {@link tp.Grid} for handling the fields of a table def.
     @type {tp.Grid}
     */
    gridFields = null;
    /** A C# DataTableDef instance as it comes from server. <br />
     <pre>
        "Id": "{A2485753-8C03-4863-903C-5054E29F5330}",
        "Name": "AppUser",
        "TitleKey": "AppUser",
        "Fields": [
            {
                "Id": "{0A28CC30-7971-4771-91AE-3F7616FFB480}",
                "Name": "Id",
                "TitleKey": "Id",
                "IsPrimaryKey": true,
                "DataType": "String",
                "Length": 40,
                "Required": true,
                "DefaultValue": null,
                "Unique": false,
                "ForeignTableName": null,
                "ForeignFieldName": null
            },
            ...
        ]
     </pre>
     * @type {object}
     */
    TableDef = null;
 
    /** Creates and assigns the tblFields.
     * @param {boolean} IsInsert True when is an Insert operation. False when is an Edit operation.
     * @param {tp.DataTable} tblData
     * */
    SetupFieldsTable(IsInsert, tblData) {
        let TableDef = new tp.DataTableDef();

        // read the json from Data1 field and load the tblFields
        if (IsInsert === false) {
            let Text = tblData.Rows[0].Get('Data1');
            //log(Text);
            let Source = eval("(" + Text + ")");
            TableDef.Assign(Source);
        }

        this.tblFields = TableDef.FieldsToDataTable();
        this.tblFields.AcceptChanges();
    }
    /** Assigns the gridFields property and sets up the grid.
     * */
    SetupFieldsGrid() {
        if (tp.IsEmpty(this.gridFields)) {
            this.gridFields = tp.FindComponentByName('gridFields', this.View.Handle);
            this.gridFields.On("ToolBarButtonClick", this.GridFields_AnyButtonClick, this);
            this.gridFields.On(tp.Events.DoubleClick, this.GridFields_DoubleClick, this);
        }

        this.gridFields.DataSource = this.tblFields;
        this.gridFields.BestFitColumns();
    }

    /** Creates and returns a clone of the tblFields with just a single row, in order to be passed to the edit dialog.
     * The row is either empty, on insert, or a clone of a tblFields row, on edit.
     * @param {tp.DataRow} SourceRow The row is either empty, on insert, or a clone of a tblFields row, on edit.
     * @returns Returns a clone of the tblFields with just a single row, in order to be passed to the edit dialog.
     */
    CreateEditTable(SourceRow = null) {
        let IsInsert = tp.IsEmpty(SourceRow);
        let EditableColumns = ['TitleKey', 'Length', 'DefaultValue'];

        let Result = this.tblFields.Clone();
        Result.Name = 'Fields';

        if (tp.IsEmpty(SourceRow)) {
            Result.AddEmptyRow();
        }
        else {
            Result.AddRow(SourceRow.Data);
        }

        Result.Columns.forEach((column) => {
            if (column.Name === 'Name')
                column.MaxLength = 32;

            column.ReadOnly = !(IsInsert || (EditableColumns.indexOf(column.Name) !== -1));

        });

        return Result;
    }

    /** Displays an edit dialog box for editing an existing or new row of the tblFields.
     * The passed {@link tp.DataTable} is a clone of tblFields with just a single row.
     * That single row is either empty, on insert, or a clone of a tblFields row, on edit.
     * @param {boolean} IsInsert True when is an Insert operation. False when is an Edit operation.
     * @param {tp.DataTable} Table The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
     */
    async ShowEditDialog(IsInsert, Table) {
        let DialogBox = null;
        let ContentHtmlText;
        let HtmlText;
        let HtmlRowList = [];

        let DataSource = new tp.DataSource(Table);
        let ColumnNames = ['Name', 'TitleKey', 'DataType', 'Length', 'DefaultValue', 'ForeignTableName', 'ForeignFieldName', 'Required', 'Unique'];


        // prepare HTML text for each column in tblFields
        ColumnNames.forEach((ColumnName) => {
            let Column = this.tblFields.FindColumn(ColumnName);
            let IsCheckBox = Column.DataType === tp.DataType.Boolean;

            let Text = Column.Title;
            let Ctrl = {
                TypeName: Column.Name === 'DataType' ? 'ComboBox' : tp.DataTypeToUiType(Column.DataType),
                TableName: Column.Table.Name,
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
        let elContent = tp.ContentWindow.GetContentElement(ContentHtmlText);

        // show the dialog
        if (tp.IsHTMLElement(elContent)) {


            let BodyWidth = tp.Doc.body.offsetWidth
            let w = BodyWidth <= 580 ? BodyWidth - 6 : 580;

            let WindowArgs = new tp.WindowArgs({ Text: 'Fields', Width: w, Height: 'auto' });

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
                    if (DataSourceName === 'Fields')
                        return DataSource;

                    if (DataSourceName === 'DataType') {
                        let Result = new tp.DataSource(tp.DataType.ToLookupTable([tp.DataType.Unknown]));
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
                // EDW: validate user input in dialog box
                return true;
            };
            //----------------------------------------------------- 
            /**  Callback to be called when the dialog is about to close (i.e. OnClosing())
             * @param {tp.Window} Window
             */
            WindowArgs.CloseFunc = (Window) => {
                //
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
        let tblField = this.CreateEditTable(null);
        let DialogBox = await this.ShowEditDialog(true, tblField);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblFields.AddEmptyRow();
            Row.CopyFromRow(tblField.Rows[0]);
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
                let tblField = this.CreateEditTable(Row);
                let DialogBox = await this.ShowEditDialog(false, tblField);

                if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                    Row.CopyFromRow(tblField.Rows[0]);
                }
            }
        }
    }


    /** Called before the Insert() operation of the owner View.
    */
    InsertItemBefore() {
    }
    /** Called after the Insert() operation of the owner View.
     * @param {tp.DataTable} tblData
     */
    InsertItemAfter(tblData) {
        this.SetupFieldsTable(true, tblData);
        this.SetupFieldsGrid();

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
     */
    EditItemBefore(Id) {
    }
    /** Called after the Edit() operation of the owner View. <br />
     * The View is just loaded in its Edit part a SysData Item from server.
     * @param {string} Id
     * @param {tp.DataTable} tblData
     */
    EditItemAfter(Id, tblData) {
        this.SetupFieldsTable(false, tblData);
        this.SetupFieldsGrid();
    }

    /** Called before the Commit() operation of the owner View.
     * @param {tp.DataTable} tblData
     */
    CommitItemBefore(tblData) {
        let Row = tblData.Rows[0];

        let TableDef = new tp.DataTableDef();
        TableDef.Name = Row.Get('DataName', '');
        TableDef.TitleKey = Row.Get('TitleKey', '');
        TableDef.FieldsFromDataTable(this.tblFields);

        let JsonText = tp.ToJson(TableDef, true);
        log(JsonText)
    }

    /** Called before the Commit() operation of the owner View. Returns true if commit is allowed, else false.
     * @param {tp.DataTable} tblData
     * @returns {boolean} Returns true if commit is allowed, else false.
     */
    CanCommitItem(tblData) {
        if (tp.IsEmpty(this.tblFields) || this.tblFields.RowCount <= 1) {
            tp.WarningNote('Cannot save changes.\nNo fields defined in the table');
            return false;
        }

        return true;
    }

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
                this.Handler = new tp.SysDataHandlerTable(this, this.DataType);
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

            if (tp.IsArray(ControlList)) {
                ControlList.forEach((c) => {
                    if (c instanceof tp.TabControl) {
                        this.pagerEdit = c;
                        if (this.pagerEdit.GetPageCount() === 1) {
                            this.pagerEdit.ShowTabBar(false);   // hide tab-bar if we have only a single page
                        }
                    }
                });
            }

        }
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

        let Url = tp.Urls.SysDataSelectEmptyItem;

        this.pagerEdit.SelectedIndex = 0;

        this.Handler.InsertItemBefore();

        let Args = await tp.Ajax.GetAsync(Url);

        this.tblData = new tp.DataTable();
        this.tblData.Assign(Args.Packet);
        this.tblData.Name = 'SysData';

        this.tblData.SetColumnListReadOnly(['DataType', 'Owner']);

        if (this.tblData.RowCount === 0) {
            let Row = this.tblData.AddEmptyRow();
            Row.Set('DataType', this.DataType);
            Row.Set('Owner', 'App');
            Row.Set('Tag4', 'Custom');
        }

        this.DataSources.length = 0;
        this.DataSources.push(new tp.DataSource(this.tblData));

        let DataControlList = this.GetDataControlList();
        this.BindControls(DataControlList);

        this.Handler.InsertItemAfter(this.tblData);

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

            this.tblData = new tp.DataTable();
            this.tblData.Assign(Args.Packet);
            this.tblData.Name = 'SysData';

            this.tblData.SetColumnListReadOnly(['DataType', 'Owner']);

            this.DataSources.length = 0;
            this.DataSources.push(new tp.DataSource(this.tblData));

            let DataControlList = this.GetDataControlList();
            this.BindControls(DataControlList);

            this.Handler.EditItemAfter(Id, this.tblData);

            this.ViewMode = tp.DataViewMode.Edit;
        }
    }
    async Delete() {

    }
    async Commit() {
        // checks
        if (!this.CanCommit())
            return;

        this.Handler.CommitItemBefore(this.tblData);

        // default values
        let Row = this.tblData.Rows[0];
        let v = Row.Get('TitleKey', '');
        if (!tp.IsValid(v) || tp.IsBlankString(v))
            Row.Set('TitleKey', Row.Get('DataName', ''));

    }


    CreateEditTable() {

    }
    /** Returns true if can commit changes, else false.
     * @returns {boolean} Returns true if can commit changes, else false.
     * */
    CanCommit() {
        if (!this.Handler.CanCommitItem(this.tblData))
            return false;

        let Row = this.tblData.Rows[0];
        let v = Row.Get('DataName', '');
        if (!tp.IsValid(v) || tp.IsBlankString(v)) {
            tp.ErrorNote('Cannot save changes.\nNo value in field: DataName');
            return false;
        }

        return true;
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
/** The Edit (data) table
 * @type {tp.DataTable}
 */
tp.DeskSysDataView.prototype.tblData = null;
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
