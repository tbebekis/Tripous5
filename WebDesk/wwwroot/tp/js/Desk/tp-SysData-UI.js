
//---------------------------------------------------------------------------------------
// Database Definition dialogs
//---------------------------------------------------------------------------------------

//#region DatabaseFieldEditDialog

tp.DatabaseFieldEditDialog = class extends tp.Window {
    /**
     * Constructor
     * @param {tp.WindowArgs} Args The window args
     */
    constructor(Args) {
        super(Args);
    }

    /** The parent element of controls
    * @type {HTMLElement}
    */
    ContentRow = null;
    /** The datasource controls are bind to.
    * @type {tp.DataSource}
    */
    DataSource = null;

    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.DatabaseFieldEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.tblField = this.Args.Instance;
        this.IsInsertMode = this.Args.IsInsertMode;
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', _L('Cancel'), tp.DialogResult.Cancel);

        let ContentHtmlText;
        let HtmlText;
        let HtmlRowList = [];

        let ColumnNames = [];       // Visible Controls
        let EditableColumns = []    // Editable Controls

        ColumnNames = ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'ForeignKeyConstraintName', 'Unique', 'UniqueConstraintName'];

        // inserting or editing?
        EditableColumns = this.IsInsertMode === true ?
            ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique'] :
            ['Name', 'TitleKey', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique'];


        this.DataSource = new tp.DataSource(this.tblField);

        // Editable Controls
        this.tblField.Columns.forEach((column) => {
            if (column.Name === 'Name')
                column.MaxLength = 30;

            column.ReadOnly = EditableColumns.indexOf(column.Name) < 0;
        });

        // Visible Controls
        // prepare HTML text for each column in tblFields
        ColumnNames.forEach((ColumnName) => {
            let Column = this.tblField.FindColumn(ColumnName);
            let IsCheckBox = Column.DataType === tp.DataType.Boolean;

            let Text = Column.Title;
            let Ctrl = {
                TypeName: Column.Name === 'DataType' ? 'ComboBox' : tp.DataTypeToUiType(Column.DataType),
                TableName: this.tblField.Name,
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
        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480]}'>
    <div class="Col" data-setup='{ControlWidthPercents: [100, 60, 60, 60, 60]}'>
        ${HtmlText}
    </div>
</div>
`;
        this.ContentRow = tp.HtmlToElement(RowHtmlText);
        this.ContentWrapper.Handle.appendChild(this.ContentRow);

        tp.Ui.CreateContainerControls(this.ContentRow.parentElement);

    }


    /**
     * To be used by modal dialogs in passing values from the object being edited to controls.
     * This method is called after the window becomes visible.
     */
    ItemToControls() {

        tp.StyleProp(this.ContentWrapper.Handle, 'padding', '5px');

        // force tp-Cols to adjust
        this.BroadcastSizeModeChanged();

        // bind dialog controls
        tp.BindAllDataControls(this.ContentRow, (DataSourceName) => {
            if (DataSourceName === 'Field')
                return this.DataSource;

            if (DataSourceName === 'DataType') {
                let Result = new tp.DataSource(tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]));
                return Result;
            }

            return null;
        });
    }
    /**
     * To be used by modal dialogs in passing values from controls to the object being edited, in case of a valid DialogResult.
     * This method is called just before setting the DialogResult property.
     * NOTE: Throwing an exception from inside this method cancels the setting of the DialogResult property.
     */
    ControlsToItem() {

        /** @type {tp.DataRow} */
        let Row = this.tblField.Rows[0];

        let Errors = [];

        // Name
        let v = Row.Get('Name', '');
        if (tp.IsBlank(v)) {
            Errors.push('\nName is required');
        }

        if (tp.IsString(v) && !tp.IsValidIdentifier(v, '$')) {
            Errors.push('\nName should start with _ or letter \nand cannot contain spaces, special characters and punctuation.');
        }

        // Length
        v = Row.Get('DataType', '');
        if (v === tp.DataType.String) {
            v = Row.Get('Length', 0);
            if (v <= 0) {
                Errors.push('\nInvalid Length');
            }
        }

        if (Errors.length > 0) {
            let Message = Errors.join('\n');
            Message += '\n'
            tp.Throw(Message);
        }


        v = Row.Get('TitleKey', '');
        if (tp.IsBlank(v)) {
            Row.Set('TitleKey', Row.Get('Name', ''));
        }
    }

}

/** When true then dialog is in insert mode, else in edit mode
 * @type {boolean}
 * */
tp.DatabaseFieldEditDialog.prototype.IsInsertMode = false;
/** The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
 * @type {tp.DataTable}   
 */
tp.DatabaseFieldEditDialog.prototype.tblField = null;



/**
Displays a modal dialog box for editing a {@link tp.DataFieldDef} field of a {@link tp.DataTableDef} object
@static
@param {tp.DataTable} tblField The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
@param {boolean} IsInsertMode When true then dialog is in insert mode, else in edit mode
@returns {tp.DatabaseFieldEditDialog} Returns the {@link tp.Window}  dialog box
*/
tp.DatabaseFieldEditDialog.ShowModalAsync = function (tblField, IsInsertMode) {

    let BodyWidth = tp.Doc.body.offsetWidth
    let w = BodyWidth <= 580 ? BodyWidth - 6 : 580;

    let WindowArgs = new tp.WindowArgs({ Text: 'Edit Database Field', Width: w, Height: 'auto', IsInsertMode: IsInsertMode });

    return tp.Window.ShowModalForAsync(tblField, tp.DatabaseFieldEditDialog, WindowArgs);
};

//#endregion

//---------------------------------------------------------------------------------------
// SelectSql Definition dialogs
//---------------------------------------------------------------------------------------

//#region SqlFilterDefEditDialog
/** Modal dialog box for editing the Enum part (the EnumXXX properties) of a {@link tp.SqlFilterDef} descriptor */
tp.SqlFilterDefEditDialog = class extends tp.Window {
    /**
     * Constructor
     * @param {tp.WindowArgs} Args The window args
     */
    constructor(Args) {
        super(Args);
    }

    tabGeneral = null;
    tabEnum = null;
    tabEnumSql = null;

    edtFieldPath = null;
    edtTitleKey = null;
    cboDataType = null;
    cboMode = null;
    chUseRange = null;
    edtLocator = null;
    chPutInHaving = null;
    edtAggregateFunc = null;

    edtEnumResultField = null;
    chEnumIsMultiChoise = null;
    chEnumIncludeAll = null;
    /**
     * @type {tp.Memo}
     */
    edtEnumOptionList = null;
    /**
     * @type {tp.Memo}
     */
    edtEnumDisplayLabels = null;

    /** The element upon Ace Editor is created. 
     * The '__Editor' property of the element points to Ace Editor object.
     * @type {HTMLElement}
     */
    elSqlEditor = null;

    /* overrides */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SqlFilterDefEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.FilterDef = this.Args.FilterDef;
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        let LayoutRow, elRow, elCol, elCol2, el, CP, i, ln, Index;

        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480], Height: "auto"}'>
    <div class="Col" data-setup='{WidthPercents: [100, 100, 50, 33.33, 33.33], ControlWidthPercents: [100, 60, 60, 60, 60]}'>
    </div>
</div>
`;

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel);

        this.Pager = new tp.TabControl(null, { Height: '100%' });
        this.Pager.Parent = this.ContentWrapper;

        this.tabGeneral = this.Pager.AddPage('General');
        this.tabEnum = this.Pager.AddPage('Enum');
        this.tabEnumSql = this.Pager.AddPage('Enum Sql');

        setTimeout(() => { this.Pager.SelectedPage = this.tabGeneral; }, 100);

        // General Page
        // ---------------------------------------------------------------------------------
        elRow = tp.HtmlToElement(RowHtmlText);
        this.tabGeneral.Handle.appendChild(elRow);
        tp.Ui.CreateContainerControls(elRow.parentElement);

        elCol = elRow.children[0];
        tp.StyleProp(elCol, 'padding-left', '2px');

        // controls 
        this.edtFieldPath = tp.CreateControlRow(tp.Div(elCol), false, 'Field Path', { TypeName: 'TextBox' }).Control;
        this.edtTitleKey = tp.CreateControlRow(tp.Div(elCol), false, 'Title Key', { TypeName: 'TextBox' }).Control;
        this.cboDataType = tp.CreateControlRow(tp.Div(elCol), false, 'DataType', { TypeName: 'ComboBox', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: tp.DataType.ToList([]) }).Control;
        this.cboMode = tp.CreateControlRow(tp.Div(elCol), false, 'Mode', { TypeName: 'ComboBox', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: tp.SqlFilterMode.ToList([]) }).Control;
        this.chUseRange = tp.CreateControlRow(tp.Div(elCol), true, 'Use Range', { TypeName: 'CheckBox' }).Control;
        this.edtLocator = tp.CreateControlRow(tp.Div(elCol), false, 'Locator', { TypeName: 'TextBox' }).Control;
        this.chPutInHaving = tp.CreateControlRow(tp.Div(elCol), true, 'Put in Having', { TypeName: 'CheckBox' }).Control;
        this.edtAggregateFunc = tp.CreateControlRow(tp.Div(elCol), false, 'Aggregate Func', { TypeName: 'TextBox' }).Control;

        // item to controls 
        this.edtFieldPath.Text = this.FilterDef.FieldPath;
        this.edtTitleKey.Text = this.FilterDef.TitleKey;
        this.cboDataType.SelectedIndex = this.cboDataType.Items.findIndex((item) => item.Id === this.FilterDef.DataType);       
        this.cboMode.SelectedIndex = this.cboMode.Items.indexOf(this.FilterDef.Mode);
        this.chUseRange.Checked = this.FilterDef.UseRange === true;
        this.edtLocator.Text = this.FilterDef.Locator;
        this.chPutInHaving.Checked = this.FilterDef.PutInHaving === true;
        this.edtAggregateFunc.Text = this.FilterDef.AggregateFunc;

        // Enum Page
        // ---------------------------------------------------------------------------------
        elRow = tp.HtmlToElement(RowHtmlText);
        this.tabEnum.Handle.appendChild(elRow);
        tp.Ui.CreateContainerControls(elRow.parentElement);

        elCol = elRow.children[0];
        tp.StyleProp(elCol, 'padding-left', '2px');

        // controls
        this.edtEnumResultField = tp.CreateControlRow(tp.Div(elCol), false, 'Result Field', { TypeName: 'TextBox' }).Control;
        this.edtEnumOptionList = tp.CreateControlRow(tp.Div(elCol), false, 'Options', { TypeName: 'Memo' }).Control;
        this.chEnumIsMultiChoise = tp.CreateControlRow(tp.Div(elCol), true, 'Is Multi Choise', { TypeName: 'CheckBox' }).Control;
        this.chEnumIncludeAll = tp.CreateControlRow(tp.Div(elCol), true, 'Include All', { TypeName: 'CheckBox' }).Control;

        elCol = elRow.children[1];
        tp.StyleProp(elCol, 'padding-left', '2px');
        this.edtEnumDisplayLabels = tp.CreateControlRow(tp.Div(elCol), false, 'Display Labels', { TypeName: 'Memo' }).Control;

        this.edtEnumOptionList.Height = '10em';
        this.edtEnumDisplayLabels.Height = '10em';

        // item to controls
        this.edtEnumResultField.Text = this.FilterDef.EnumResultField;
        this.chEnumIsMultiChoise.Checked = this.FilterDef.EnumIsMultiChoise === true;
        this.chEnumIncludeAll.Checked = this.FilterDef.EnumIncludeAll === true;
        this.edtEnumOptionList.AppendLines(this.FilterDef.EnumOptionList);
        this.edtEnumDisplayLabels.Text = this.FilterDef.EnumDisplayLabels;

        // Sql Page
        // ---------------------------------------------------------------------------------
        this.elSqlEditor = tp.CreateSourceCodeEditor(this.tabEnumSql.Handle, 'sql', this.FilterDef.EnumSql);
    }
    /** Called just before a modal window is about to set its DialogResult property. <br />
     * Returning false cancels the setting of the property and the closing of the modal window. <br />
     * NOTE: Setting the DialogResult to any value other than <code>tp.DialogResult.None</code> closes a modal dialog window.
     * @override
     * @param {any} DialogResult
     */
    CanSetDialogResult(DialogResult) {
        if (DialogResult === tp.DialogResult.OK) {

            this.FilterDef.FieldPath = this.edtFieldPath.Text;
            this.FilterDef.TitleKey = this.edtTitleKey.Text;
            this.FilterDef.DataType = this.cboDataType.SelectedValue;
            this.FilterDef.Mode = this.cboMode.SelectedValue;
            this.FilterDef.UseRange = this.chUseRange.Checked;
            this.FilterDef.Locator = this.edtLocator.Text;
            this.FilterDef.PutInHaving = this.chPutInHaving.Checked;
            this.FilterDef.AggregateFunc = this.edtAggregateFunc.Text;

            this.FilterDef.EnumSql = this.elSqlEditor.__Editor.getValue();

            this.FilterDef.EnumResultField = this.edtEnumResultField.Text;
            this.FilterDef.EnumIsMultiChoise = this.chEnumIsMultiChoise.Checked;
            this.FilterDef.EnumIncludeAll = this.chEnumIncludeAll.Checked;
            this.FilterDef.EnumOptionList = this.edtEnumOptionList.GetLines(true);

            this.FilterDef.EnumDisplayLabels = this.edtEnumDisplayLabels.Text;

            // check if is valid before closing
            this.FilterDef.CheckDescriptor();
        }

        return true;
    }


};

/**
 * @type {tp.SqlFilterDef}
 * */
tp.SqlFilterDefEditDialog.prototype.FilterDef = null;


/**
Displays a modal dialog box for editing a {@link tp.SqlFilterDef} descriptor
@static
@param {tp.SqlFilterDef} FilterDef The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlFilterDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlFilterDefEditDialog.ShowModal = function (FilterDef, WindowArgs = null) {

    let Args = WindowArgs || {};
    Args.Text = Args.Text || 'SqlFilterDef editor';

    Args = new tp.WindowArgs(Args);
    Args.AsModal = true;
    Args.DefaultDialogResult = tp.DialogResult.Cancel;
    Args.FilterDef = FilterDef;

    let Result = new tp.SqlFilterDefEditDialog(Args);
    Result.ShowModal();

    return Result;
};
/**
Displays a modal dialog box for a {@link tp.SqlFilterDef} descriptor
@static
@param {tp.SqlFilterDef} FilterDef The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlFilterDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlFilterDefEditDialog.ShowModalAsync = function (FilterDef, WindowArgs = null) {
    return new Promise((Resolve, Reject) => {
        WindowArgs = WindowArgs || {};
        let CloseFunc = WindowArgs.CloseFunc;

        WindowArgs.CloseFunc = (Window) => {
            tp.Call(CloseFunc, Window.Args.Creator, Window);
            Resolve(Window);
        };

        tp.SqlFilterDefEditDialog.ShowModal(FilterDef, WindowArgs);
    });
};
//#endregion

//#region SelectSqlEditDialog

/** Modal dialog box for editing a {@link tp.SelectSql} descriptor
 *  */
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
     * @type {tp.Grid}
     */
    gridFilters = null;
    /**
     * @type {tp.DataTable}
     */
    tblFilters = null;


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
        this.tabSql = this.Pager.AddPage('Sql');
        this.tabColumns = this.Pager.AddPage(_L('Columns'));
        this.tabFilters = this.Pager.AddPage(_L('Filters'));

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

        Index;
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
        LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
        this.tabColumns.AddComponent(LayoutRow);
 
        // columns grid
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
                { Name: 'DisplayType', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.ColumnDisplayType) },

                { Name: 'Width' },
                { Name: 'ReadOnly' },

                { Name: 'DisplayIndex' },
                { Name: 'GroupIndex' },
                { Name: 'Decimals' },
                { Name: 'FormatString' },
                { Name: 'Aggregate', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.AggregateType) },
                { Name: 'AggregateFormat' },
            ]
        };

        this.gridColumns = new tp.Grid(el, CP);
        //this.gridColumns.On("ToolBarButtonClick", this.GridColumns_AnyButtonClick, this);
        //this.gridColumns.On(tp.Events.DoubleClick, this.GridColumns_DoubleClick, this);

        // columns table
        this.tblColumns = this.SelectSql.ColumnsToDataTable();

        this.gridColumns.DataSource = this.tblColumns;
        this.gridColumns.BestFitColumns();


        // Filters Page
        // ---------------------------------------------------------------------------------
        LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
        this.tabFilters.AddComponent(LayoutRow);

        // filters table
        this.tblFilters = this.SelectSql.FiltersToDataTable();

        // filters grid
        let AggregateFuncList = [
            { Id: '', Name: '' },
            { Id: 'count', Name: 'count' },
            { Id: 'avg', Name: 'avg' },
            { Id: 'sum', Name: 'sum' },
            { Id: 'max', Name: 'max' },
            { Id: 'min', Name: 'min' }
        ];

        // add a DIV for the gridFields tp.Grid in the row
        el = LayoutRow.AddDivElement();
        CP = {
            Name: "gridFilters",
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
                { Name: 'FieldPath' },
                { Name: 'TitleKey' },

                { Name: 'DataType', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]) },
                { Name: 'Mode', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.SqlFilterMode, [tp.SqlFilterMode.None]) },

                { Name: 'UseRange' },
                { Name: 'Locator' },
                { Name: 'PutInHaving' },
                { Name: 'AggregateFunc', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: AggregateFuncList },
            ]
        };

        this.gridFilters = new tp.Grid(el, CP);
        //this.gridFilters.AddToolBarButton('EditEnum', '', 'Edit Enum part', 'fa fa-sticky-note-o', '', false);    // Command, Text, ToolTip, IcoClasses, CssClasses, ToRight
        this.gridFilters.On('ToolBarButtonClick', this.gridFilters_ToolBarButtonClick, this);

        this.gridFilters.DataSource = this.tblFilters;
        this.gridFilters.BestFitColumns();

        this.tblFilters.On('RowCreated', this.tblFilters_RowCreated, this);
        this.tblFilters.On('RowModified', this.tblFilters_RowModified, this);


    }
    /** Called just before a modal window is about to set its DialogResult property. <br />
     * Returning false cancels the setting of the property and the closing of the modal window. <br />
     * NOTE: Setting the DialogResult to any value other than <code>tp.DialogResult.None</code> closes a modal dialog window.
     * @override
     * @param {any} DialogResult
     */
    CanSetDialogResult(DialogResult) {
        if (DialogResult === tp.DialogResult.OK) {
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

            this.SelectSql.CheckDescriptor();
        }

        return true;
    }
 
    /* event handlers */

    /* NOT USED - we use tp.Grid's built-in functionality
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
    GridColumns_DoubleClick(Args) {
        //Args.Handled = true;
        //this.EditFieldRow();
    }
    */

    /** Called when a new data row is created and it is about to be added to the table
     * @param {tp.DataTableEventArgs} Args
     */
    tblFilters_RowCreated(Args) {
        //Args.Row.FilterDef = new tp.SqlFilterDef();
    }
    /** Called when a column in a data row is modified.
     * @param {tp.DataTableEventArgs} Args
     */
    tblFilters_RowModified(Args) {
        // nothing yet
    }

    /** Creates and returns a clone of the tblFilters with just a single row, in order to be passed to the edit dialog.
     * The row is either empty, on insert, or a clone of a tblFilters row, on edit.
     * @param {tp.DataRow} SourceRow The row is either empty, on insert, or a clone of a tblFilters row, on edit.
     * @returns {tp.DataTable} Returns a clone of the tblFilters with just a single row, in order to be passed to the edit dialog.
     */
    CreateEditFilterTable(SourceRow = null) {
        let Row;
        let IsInsert = tp.IsEmpty(SourceRow);

        // create the table, used in editing a single row
        let Table = this.tblFilters.Clone();
        Table.Name = 'Filter';

        // add the single row in table
        Row = Table.AddEmptyRow();

        if (IsInsert) {
            let Item = new tp.SqlFilterDef();
            Item.ToDataRow(Row);
        }
        else {
            Row.CopyFromRow(SourceRow);
        }

        return Table;
    }
    /** Called when inserting a single row of the tblFilters and displays the edit dialog
    */
    async InsertFilterRow() {
        let Item = new tp.SqlFilterDef();

        let DialogBox = await tp.SqlFilterDefEditDialog.ShowModalAsync(Item);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblFilters.AddEmptyRow();
            Item.ToDataRow(Row);
            this.SelectSql.Filters.push(Item);
        }
    }
    /** Called when editing a single row of the tblFilters and displays the edit dialog
     */
    async EditFilterRow() {
        let Row = this.gridFilters.FocusedRow;
        if (tp.IsValid(Row)) {
            let Item = Row.OBJECT;
            let DialogBox = await this.ShowEditFilterDialog(Item);
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Item.ToDataRow(Row);
            }
        }
    }
    /** Deletes a single row of the tblSelectSqlList 
    */
    async DeleteFilterRow() {
        let Row = this.gridFilters.FocusedRow;

        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                let Item = Row.OBJECT;
                tp.ListRemove(this.SelectSql.Filters, Item);
                this.tblFilters.RemoveRow(Row);
            }
        }
    }

    /** Called when a button in the filters grid tool-bar is clicked. 
     * @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
     */
    async gridFilters_ToolBarButtonClick(Args) {
        Args.Handled = true;

        switch (Args.Command) {
            case 'GridRowInsert':
                this.InsertFilterRow();
                break;
            case 'GridRowEdit':
                this.EditFilterRow();
                break;
            case 'GridRowDelete':
                this.DeleteFilterRow();
                break;
        }

        /*
        if (Args.Command === 'EditEnum') {
            Args.Handled = true;

            let Row = this.gridFilters.FocusedRow;
            if (!tp.IsEmpty(Row)) {
                let FilterDef = Row.FilterDef;


                //         this.tblFilters.AddColumn('Mode', tp.DataType.Integer).DefaultValue = tp.SqlFilterMode.Simple;

                let Res = await tp.SqlFilterDefEnumDialog.ShowModalAsync(FilterDef);
                let o = Res;
            } 
        }
        */
    }
};


/** The instance to be edited by this dialog box.
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
//#endregion

//---------------------------------------------------------------------------------------
// Locator Definition dialogs
//---------------------------------------------------------------------------------------

//#region LocatorFieldEditDialog

tp.LocatorFieldEditDialog = class extends tp.Window {
    /**
     * Constructor
     * @param {tp.WindowArgs} Args The window args
     */
    constructor(Args) {
        super(Args);
    }

    /** The parent element of controls
    * @type {HTMLElement}
    */
    ContentRow = null;
    /** The datasource controls are bind to.
    * @type {tp.DataSource}
    */
    DataSource = null;

    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.LocatorFieldEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.tblField = this.Args.Instance;
        this.IsInsertMode = this.Args.IsInsertMode;
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', _L('Cancel'), tp.DialogResult.Cancel);

        let ContentHtmlText;
        let HtmlText;
        let HtmlRowList = [];

        let ColumnNames = [];       // Visible Controls
        let EditableColumns = []    // Editable Controls

        ColumnNames = ['Name', 'TableName', 'DataField', 'DataType', 'TitleKey', 'Visible', 'Searchable', 'ListVisible', 'IsIntegerBoolean', 'Width'];
        EditableColumns = ColumnNames;

        this.DataSource = new tp.DataSource(this.tblField);

        // Visible Controls
        // prepare HTML text for each column in tblFields
        ColumnNames.forEach((ColumnName) => {
            let Column = this.tblField.FindColumn(ColumnName);
            let IsCheckBox = Column.DataType === tp.DataType.Boolean;

            let Text = Column.Title;
            let Ctrl = {
                TypeName: Column.Name === 'DataType' ? 'ComboBox' : tp.DataTypeToUiType(Column.DataType),
                TableName: this.tblField.Name,
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
        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480]}'>
    <div class="Col" data-setup='{ControlWidthPercents: [100, 60, 60, 60, 60]}'>
        ${HtmlText}
    </div>
</div>
`;
        this.ContentRow = tp.HtmlToElement(RowHtmlText);
        this.ContentWrapper.Handle.appendChild(this.ContentRow);

        tp.Ui.CreateContainerControls(this.ContentRow.parentElement);
    }


    /**
     * To be used by modal dialogs in passing values from the object being edited to controls.
     * This method is called after the window becomes visible.
     */
    ItemToControls() {

        tp.StyleProp(this.ContentWrapper.Handle, 'padding', '5px');

        // force tp-Cols to adjust
        this.BroadcastSizeModeChanged();

        // bind dialog controls
        tp.BindAllDataControls(this.ContentRow, (DataSourceName) => {
            if (DataSourceName === 'Field')
                return this.DataSource;

            if (DataSourceName === 'DataType') {
                let Result = new tp.DataSource(tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]));
                return Result;
            }

            return null;
        });
    }
    /**
     * To be used by modal dialogs in passing values from controls to the object being edited, in case of a valid DialogResult.
     * This method is called just before setting the DialogResult property.
     * NOTE: Throwing an exception from inside this method cancels the setting of the DialogResult property.
     */
    ControlsToItem() {

        /** @type {tp.DataRow} */
        let Row = this.tblField.Rows[0];

        let Errors = [];

        // Name
        let v = Row.Get('Name', '');
        if (tp.IsBlank(v)) {
            Errors.push('\nName is required');
        }

        if (Errors.length > 0) {
            let Message = Errors.join('\n');
            Message += '\n'
            tp.Throw(Message);
        }

        v = Row.Get('TitleKey', '');
        if (tp.IsBlank(v)) {
            Row.Set('TitleKey', Row.Get('Name', ''));
        }
    }

}

/** When true then dialog is in insert mode, else in edit mode
 * @type {boolean}
 * */
tp.LocatorFieldEditDialog.prototype.IsInsertMode = false;
/** The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
 * @type {tp.DataTable}   
 */
tp.LocatorFieldEditDialog.prototype.tblField = null;



/**
Displays a modal dialog box for editing a {@link tp.LocatorFieldDef} field of a {@link tp.LocatorDef} object
@static
@param {tp.DataTable} tblField The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
@param {boolean} IsInsertMode When true then dialog is in insert mode, else in edit mode
@returns {tp.LocatorFieldEditDialog} Returns the {@link tp.Window}  dialog box
*/
tp.LocatorFieldEditDialog.ShowModalAsync = function (tblField, IsInsertMode) {

    let BodyWidth = tp.Doc.body.offsetWidth
    let w = BodyWidth <= 580 ? BodyWidth - 6 : 580;

    let WindowArgs = new tp.WindowArgs({ Text: _L('LocatorFieldDialogTitle', 'Edit Locator field'), Width: w, Height: 'auto', IsInsertMode: IsInsertMode });

    return tp.Window.ShowModalForAsync(tblField, tp.LocatorFieldEditDialog, WindowArgs);
};

//#endregion

//---------------------------------------------------------------------------------------
// Broker Definition dialogs
//---------------------------------------------------------------------------------------

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
     * @type {tp.TabPage}
     */
    tabFields = null;
    /**
     * @type {tp.TabPage}
     */
    tabJoinTables = null;
    /**
     * @type {tp.TabPage}
     */
    tabStockTables = null;


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


    /**
     * @type {tp.Grid}
     */
    gridFields = null;
    /**
     * @type {tp.DataTable}
     */
    tblFields = null;

    /**
     * @type {tp.Grid}
     */
    gridJoinTables = null;
    /**
     * @type {tp.DataTable}
     */
    tblJoinTables = null;

    /**
     * @type {tp.Grid}
     */
    gridStockTables = null;
    /**
     * @type {tp.DataTable}
     */
    tblStockTables = null;

    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.SqlBrokerTableDefEditDialog';
    }
    /**
     * Override
     */
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.TableDef = this.Args.Instance;
        this.IsJoinTable = this.Args.IsJoinTable || false;
        this.OwnKeyFieldName = this.Args.OwnKeyFieldName || '';
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        this.IsJoinTable = this.IsJoinTable === true;

        let LayoutRow, elRow, elCol, el, CP;

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
        this.tabFields = this.Pager.AddPage(_L('Fields'));
        this.tabJoinTables = this.Pager.AddPage(_L('JoinTables'));
        this.tabStockTables = this.Pager.AddPage(_L('StockTables'));



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

        this.edtAlias = tp.CreateControlRow(tp.Div(elCol), false, 'Alias', { TypeName: 'TextBox' }).Control;
        this.edtTitleKey = tp.CreateControlRow(tp.Div(elCol), false, 'TitleKey', { TypeName: 'TextBox' }).Control;
        this.edtPrimaryKeyField = tp.CreateControlRow(tp.Div(elCol), false, 'PrimaryKeyField', { TypeName: 'TextBox' }).Control;
        this.edtMasterTableName = tp.CreateControlRow(tp.Div(elCol), false, 'MasterTableName', { TypeName: 'TextBox' }).Control;
        this.edtMasterKeyField = tp.CreateControlRow(tp.Div(elCol), false, this.IsJoinTable ? 'Own Key Field' : 'MasterKeyField', { TypeName: 'TextBox' }).Control;
        this.edtDetailKeyField = tp.CreateControlRow(tp.Div(elCol), false, 'DetailKeyField', { TypeName: 'TextBox' }).Control; 
 
        this.edtMasterTableName.Enabled = this.IsJoinTable !== true;
        this.edtDetailKeyField.Enabled = this.IsJoinTable !== true;




        // Fields Page
        // ---------------------------------------------------------------------------------
        LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
        this.tabFields.AddComponent(LayoutRow);

        // columns grid
        // add a DIV for the gridFields tp.Grid in the row
        el = LayoutRow.AddDivElement();

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

            //ReadOnly: true,
            AllowUserToAddRows: true,
            AllowUserToDeleteRows: true,
            AutoGenerateColumns: false,

            Columns: [
                { Name: 'Name' },
                { Name: 'Alias' },
                { Name: 'TitleKey' },
                { Name: 'DataType', ListValueField: 'Id', ListDisplayField: 'Name', ListSource: tp.EnumToLookUpTable(tp.DataType) },

                { Name: 'MaxLength' },
                { Name: 'Decimals' },
            ],
            CustomButtons: [
                { Command: 'Field.JoinTable', Text: '', ToolTip: 'Join Table to Field', IcoClasses: 'fa fa-table', CssClasses: '', ToRight: false }, 
            ]
        };

        // table and grid
        this.tblFields = this.TableDef.FieldsToDataTable();

        this.gridFields = new tp.Grid(el, CP);
        this.gridFields.On("ToolBarButtonClick", this.AnyGrid_AnyButtonClick, this);
        this.gridFields.On(tp.Events.DoubleClick, this.AnyGrid_DoubleClick, this);

        this.gridFields.DataSource = this.tblFields;
        this.gridFields.BestFitColumns();

        // JoinTables Page
        // ---------------------------------------------------------------------------------
        LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
        this.tabJoinTables.AddComponent(LayoutRow);

        // add a DIV for the gridJoinTables tp.Grid in the row
        el = LayoutRow.AddDivElement();
        CP = {
            Name: "gridJoinTables",
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

            // string OwnKeyField, string ForeignTable, string ForeignAlias = "", string ForeignPrimaryKey

            Columns: [
                { Name: 'OwnKeyField' },
                { Name: 'ForeignTable' },
                { Name: 'ForeignAlias' },
                { Name: 'ForeignPrimaryKey' },
            ]
        };

        // table and grid
        this.tblJoinTables = this.TableDef.JoinTablesToDataTable();

        this.gridJoinTables = new tp.Grid(el, CP);
        this.gridJoinTables.On("ToolBarButtonClick", this.AnyGrid_AnyButtonClick, this);
        this.gridJoinTables.On(tp.Events.DoubleClick, this.AnyGrid_DoubleClick, this);

        this.gridJoinTables.DataSource = this.tblJoinTables;
        this.gridJoinTables.BestFitColumns();

        // StockTables Page
        // ---------------------------------------------------------------------------------
        LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
        this.tabStockTables.AddComponent(LayoutRow);


        // add a DIV for the gridStockTables tp.Grid in the row
        el = LayoutRow.AddDivElement();
        CP = {
            Name: "gridStockTables",
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

        // table and grid
        this.tblStockTables = this.TableDef.StockTablesToDataTable();

        this.gridStockTables = new tp.Grid(el, CP);
        this.gridStockTables.DataSource = this.tblStockTables;
        this.gridStockTables.BestFitColumns();

        this.gridStockTables.On("ToolBarButtonClick", this.AnyGrid_AnyButtonClick, this);
        this.gridStockTables.On(tp.Events.DoubleClick, this.AnyGrid_DoubleClick, this);

    }






    /** Called when inserting a single row of the tblFields and displays the edit dialog 
     */
    async InsertFieldRow() {
        let FieldDef = new tp.SqlBrokerFieldDef();

        let DialogBox = await tp.SqlBrokerFieldDefEditDialog.ShowModalAsync(FieldDef, null);
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblFields.AddEmptyRow();
            FieldDef.ToDataRow(Row);
        }
    }
    /** Called when editing a single row of the tblFields and displays the edit dialog 
     */
    async EditFieldRow() {
        let Row = this.gridFields.FocusedRow;

        if (tp.IsValid(Row)) {
            let FieldDef = new tp.SqlBrokerFieldDef();
            FieldDef.Assign(Row.OBJECT);
            let DialogBox = await tp.SqlBrokerFieldDefEditDialog.ShowModalAsync(FieldDef, null);
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                FieldDef.ToDataRow(Row);
            }
        }
    }
    /** Called when deleting a single row of the tblFields 
     */
    async DeleteFieldRow() {
        let Row = this.gridFields.FocusedRow;

        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                this.tblFields.RemoveRow(Row);
            }
        }
    }


    async JoinTableToField() {
        let Row = this.gridFields.FocusedRow;

        if (tp.IsValid(Row)) {
            /** @type {tp.SqlBrokerFieldDef}*/
            let FieldDef = Row.OBJECT;
            let FieldName = FieldDef.Name;
            let JoinTableRow = this.tblJoinTables.FindRow('OwnKeyField', FieldName);
            if (tp.IsValid(JoinTableRow)) {
                await this.EditJoinTableRow(JoinTableRow);
            }
            else {
                await this.InsertJoinTableRow(FieldName);
            }             
        }
    }
 
    /**
     * Called when inserting a single row of the tblJoinTables and displays the edit dialog
     * @param {string} OwnKeyFieldName Used when the table being edited is a JoinTable. When it is not null or empty, goes to MasterKeyField
     */
    async InsertJoinTableRow(OwnKeyFieldName = null) {
        let Instance = new tp.SqlBrokerTableDef();
        Instance.MasterKeyField = OwnKeyFieldName || Instance.MasterKeyField;

        let DialogBox = await tp.SqlBrokerTableDefEditDialog.ShowModalAsync(Instance, { Text: 'Join Table Definition Editor', IsJoinTable: true });
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblJoinTables.AddEmptyRow();
            Instance.ToJoinTableDataRow(Row);
        }
    } 
    /**
     * Called when editing a single row of the tblJoinTables and displays the edit dialog
     * @param {tp.DataRow} RowToEdit When not null then is the row to edit
     */
    async EditJoinTableRow(RowToEdit = null) {
        let Row = RowToEdit || this.gridJoinTables.FocusedRow;

        if (tp.IsValid(Row)) {
            let Instance = new tp.SqlBrokerTableDef();
            Instance.Assign(Row.OBJECT);
            let DialogBox = await tp.SqlBrokerTableDefEditDialog.ShowModalAsync(Instance, { Text: 'Join Table Definition Editor', IsJoinTable: true });
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Instance.ToJoinTableDataRow(Row);
            }
        }
    }
    /**  Called when deleting a single row of the tblJoinTables
    */
    async DeleteJoinTableRow() {
        let Row = this.gridJoinTables.FocusedRow;
        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                this.tblJoinTables.RemoveRow(Row);
            }
        }
    }

    /** Called when inserting a single row of the tblStockTables and displays the edit dialog
     */
    async InsertStockTableRow() {
        let Instance = new tp.SqlBrokerQueryDef();

        let DialogBox = await tp.SqlBrokerQueryDefEditDialog.ShowModalAsync(Instance, { Text: 'Stock Table Definition Editor' });
        if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
            let Row = this.tblStockTables.AddEmptyRow();
            Instance.ToDataRow(Row);
        }
    }
    /** Called when editing a single row of the tblStockTables and displays the edit dialog
     */
    async EditStockTableRow() {
        let Row = this.gridStockTables.FocusedRow;

        if (tp.IsValid(Row)) {
            let Instance = new tp.SqlBrokerQueryDef();
            Instance.Assign(Row.OBJECT);
            let DialogBox = await tp.SqlBrokerQueryDefEditDialog.ShowModalAsync(Instance, { Text: 'Stock Table Definition Editor' });
            if (tp.IsValid(DialogBox) && DialogBox.DialogResult === tp.DialogResult.OK) {
                Instance.ToDataRow(Row);
            }
        }
    }
    /**  Called when deleting a single row of the tblStockTables
     */
    async DeleteStockTableRow() {
        let Row = this.gridStockTables.FocusedRow;

        if (tp.IsValid(Row)) {
            let Flag = await tp.YesNoBoxAsync('Delete selected row?');
            if (Flag === true) {
                this.tblStockTables.RemoveRow(Row);
            }
        }
    }


    /** Override */
    ItemToControls() {
        // item to controls
        this.edtName.Text = this.TableDef.Name;
        this.edtAlias.Text = this.TableDef.Alias;
        this.edtTitleKey.Text = this.TableDef.TitleKey;
        this.edtPrimaryKeyField.Text = this.TableDef.PrimaryKeyField;
        this.edtMasterTableName.Text = this.TableDef.MasterTableName;
        this.edtMasterKeyField.Text = this.TableDef.MasterKeyField;
        this.edtDetailKeyField.Text = this.TableDef.DetailKeyField;
    }
    /** Override */
    ControlsToItem() {
        this.TableDef.Name = this.edtName.Text;
        this.TableDef.Alias = this.edtAlias.Text || this.edtName.Text;
        this.TableDef.TitleKey = this.edtTitleKey.Text;
        this.TableDef.PrimaryKeyField = this.edtPrimaryKeyField.Text;
        this.TableDef.MasterTableName = this.edtMasterTableName.Text;
        this.TableDef.MasterKeyField = this.edtMasterKeyField.Text;
        this.TableDef.DetailKeyField = this.edtDetailKeyField.Text;

        this.TableDef.CheckDescriptor();
    }


    /* event handlers */
    /** Event handler
     * @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
     */
    AnyGrid_AnyButtonClick(Args) {
        Args.Handled = true;

        if (Args.Sender === this.gridFields) {
            switch (Args.Command) {
                case 'GridRowInsert':
                    this.InsertFieldRow();
                    break;
                case 'GridRowEdit':
                    this.EditFieldRow();
                    break;
                case 'GridRowDelete':
                    this.DeleteFieldRow();
                    break;
                case 'Field.JoinTable': // custom button
                    this.JoinTableToField();
                    break;
 
            }
        }
        else if (Args.Sender === this.gridStockTables) {
            switch (Args.Command) {
                case 'GridRowInsert':
                    this.InsertStockTableRow();
                    break;
                case 'GridRowEdit':
                    this.EditStockTableRow();
                    break;
                case 'GridRowDelete':
                    this.DeleteStockTableRow();
                    break;
            }
        }
        else if (Args.Sender === this.gridJoinTables) {
            switch (Args.Command) {
                case 'GridRowInsert':
                    this.InsertJoinTableRow();
                    break;
                case 'GridRowEdit':
                    this.EditJoinTableRow();
                    break;
                    this.DeleteJoinTableRow();
                    break;
            }
        }
    }
    /**
    Event handler
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    AnyGrid_DoubleClick(Args) {
        Args.Handled = true;

        if (Args.Sender === this.gridFields) {
            this.EditFieldRow();
        }
        else if (Args.Sender === this.gridStockTables) {
            this.EditStockTableRow();
        }
    }


}


/** The instance to be edited by this dialog box.
 * @type {tp.SqlBrokerTableDef}
 * */
tp.SqlBrokerTableDefEditDialog.prototype.TableDef = null;
/** When true it means that the dialog is used in editing a Join Table
 * @type {boolean}
 * */
tp.SqlBrokerTableDefEditDialog.prototype.IsJoinTable = false;
 

/**
Displays a modal dialog box for editing a {@link tp.SqlBrokerTableDef} object
@static
@param {tp.SqlBrokerTableDef} TableDef The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlBrokerTableDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlBrokerTableDefEditDialog.ShowModalAsync = function (TableDef, WindowArgs = null) {
    WindowArgs = WindowArgs || {};
    WindowArgs.Text = WindowArgs.Text || 'Table Definition Editor';
    return tp.Window.ShowModalForAsync(TableDef, tp.SqlBrokerTableDefEditDialog, WindowArgs);
};


//#endregion

//#region SqlBrokerFieldDefEditDialog
/** Modal dialog box for editing a {@link tp.SqlBrokerFieldDef} descriptor
 */
tp.SqlBrokerFieldDefEditDialog = class extends tp.Window {
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
    tabLookUp = null;
    /**
     * @type {tp.TabPage}
     */
    tabLookUpTableSql = null;

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
     * @type {tp.ComboBox}
     */
    cboDataType = null;
    /**
     * @type {tp.TextBox}
     */
    edtMaxLength = null;
    /**
     * @type {tp.TextBox}
     */
    edtDecimals = null;
    /**
     * @type {tp.TextBox}
     */
    edtCodeProviderName = null;
    /**
     * @type {tp.TextBox}
     */
    edtDefaultValue = null;
    /**
     * @type {tp.TextBox}
     */
    edtExpression = null;
    /**
     * @type {tp.TextBox}
     */
    edtLocatorName = null;
    /**
     * @type {tp.CheckListBox}
     */
    lboFlags = null;


    /**
     * @type {tp.TextBox}
     */
    edtLookUpTableName = null;
    /**
     * @type {tp.TextBox}
     */
    edtLookUpTableAlias = null;
    /**
     * @type {tp.TextBox}
     */
    edtLookUpKeyField = null;
    /**
     * @type {tp.TextBox}
     */
    edtLookUpFieldList = null;


    /** The element upon Ace Editor is created. 
    * The '__Editor' property of the element points to Ace Editor object.
    * @type {HTMLElement}
    */
    elSqlEditor = null;

    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.SqlBrokerFieldDefEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.FieldDef = this.Args.Instance;
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', _L('Cancel'), tp.DialogResult.Cancel);

        this.Pager = new tp.TabControl(null, { Height: '100%' });
        this.Pager.Parent = this.ContentWrapper;

        this.tabGeneral = this.Pager.AddPage(_L('General'));
        this.tabLookUp = this.Pager.AddPage(_L('LookUp'));
        this.tabLookUpTableSql = this.Pager.AddPage(_L('LookUp Table Sql'));

        setTimeout(() => { this.Pager.SelectedPage = this.tabGeneral; }, 100);


        let LayoutRow, elRow, elCol, elCol2, el, CP;

        //
        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480], Height: "100%"}'>
    <div class="Col" data-setup='{WidthPercents: [100, 100, 50, 33.33, 33.33], ControlWidthPercents: [100, 60, 60, 60, 60]}'>
    </div>
    <div class="Col" data-setup='{WidthPercents: [100, 100, 50, 33.33, 33.33], ControlWidthPercents: [100, 60, 60, 60, 60]}'>
    </div>
</div>
`;

        // General Page
        // ---------------------------------------------------------------------------------
        elRow = tp.HtmlToElement(RowHtmlText);
        this.tabGeneral.Handle.appendChild(elRow);
        tp.Ui.CreateContainerControls(elRow.parentElement);

        elCol = elRow.children[0];
        tp.StyleProp(elCol, 'padding-left', '2px');

        elCol2 = elRow.children[1];
        tp.StyleProp(elCol2, 'padding-left', '2px');

        // controls
        // col 1
        this.edtName = tp.CreateControlRow(tp.Div(elCol), false, 'Name', { TypeName: 'TextBox' }).Control;
        this.edtAlias = tp.CreateControlRow(tp.Div(elCol), false, 'Alias', { TypeName: 'TextBox' }).Control;
        this.edtTitleKey = tp.CreateControlRow(tp.Div(elCol), false, 'TitleKey', { TypeName: 'TextBox' }).Control;
        this.cboDataType = tp.CreateControlRow(tp.Div(elCol), false, 'DataType', { TypeName: 'ComboBox', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: tp.DataType.ToList([]) }).Control;
        this.edtMaxLength = tp.CreateControlRow(tp.Div(elCol), false, 'MaxLength', { TypeName: 'TextBox' }).Control;
        this.edtDecimals = tp.CreateControlRow(tp.Div(elCol), false, 'Decimals', { TypeName: 'TextBox' }).Control;

        this.edtCodeProviderName = tp.CreateControlRow(tp.Div(elCol), false, 'CodeProviderName', { TypeName: 'TextBox' }).Control;
        this.edtDefaultValue = tp.CreateControlRow(tp.Div(elCol), false, 'DefaultValue', { TypeName: 'TextBox' }).Control;
        this.edtExpression = tp.CreateControlRow(tp.Div(elCol), false, 'Expression', { TypeName: 'TextBox' }).Control;

        this.edtLocatorName = tp.CreateControlRow(tp.Div(elCol), false, 'LocatorName', { TypeName: 'TextBox' }).Control;

        // col 2
        this.lboFlags = tp.CreateControlRow(tp.Div(elCol2), false, 'Flags', { TypeName: 'CheckListBox', ListValueField: 'Id', ListDisplayField: 'Name', List: tp.FieldFlags.ToList([]) }).Control;

        // LookUp Page
        // ---------------------------------------------------------------------------------
        elRow = tp.HtmlToElement(RowHtmlText);
        this.tabLookUp.Handle.appendChild(elRow);
        tp.Ui.CreateContainerControls(elRow.parentElement);

        elCol = elRow.children[0];
        tp.StyleProp(elCol, 'padding-left', '2px');


        this.edtLookUpTableName = tp.CreateControlRow(tp.Div(elCol), false, 'LookUpTableName', { TypeName: 'TextBox' }).Control;
        this.edtLookUpTableAlias = tp.CreateControlRow(tp.Div(elCol), false, 'LookUpTableAlias', { TypeName: 'TextBox' }).Control;
        this.edtLookUpKeyField = tp.CreateControlRow(tp.Div(elCol), false, 'LookUpKeyField', { TypeName: 'TextBox' }).Control;
        this.edtLookUpFieldList = tp.CreateControlRow(tp.Div(elCol), false, 'LookUpFieldList', { TypeName: 'TextBox' }).Control;

        // LookUp Table Sql Page
        // ---------------------------------------------------------------------------------
        this.elSqlEditor = tp.CreateSourceCodeEditor(this.tabLookUpTableSql.Handle, 'sql', this.FieldDef.LookUpTableSql);

    }

    /** Override */
    ItemToControls() {
        this.edtName.Text = this.FieldDef.Name;
        this.edtAlias.Text = this.FieldDef.Alias;
        this.edtTitleKey.Text = this.FieldDef.TitleKey;
        this.cboDataType.SelectedIndex = this.cboDataType.Items.findIndex((item) => item.Id === this.FieldDef.DataType);
        this.edtMaxLength.Text = this.FieldDef.MaxLength;
        this.edtDecimals.Text = this.FieldDef.Decimals;

        this.edtCodeProviderName.Text = this.FieldDef.CodeProviderName;
        this.edtDefaultValue.Text = this.FieldDef.DefaultValue;
        this.edtExpression.Text = this.FieldDef.Expression;

        this.edtLocatorName.Text = this.FieldDef.LocatorName;

        this.edtLookUpTableName.Text = this.FieldDef.LookUpTableName;
        this.edtLookUpTableAlias.Text = this.FieldDef.LookUpTableAlias;
        this.edtLookUpKeyField.Text = this.FieldDef.LookUpKeyField;
        this.edtLookUpFieldList.Text = this.FieldDef.LookUpFieldList;


        let Flags = tp.Bf.SetValueToIntegerArray(tp.FieldFlags, this.FieldDef.Flags)
        this.lboFlags.SelectedValues = Flags;
    }
    /** Override */
    ControlsToItem() {
        this.FieldDef.Name = this.edtName.Text;
        this.FieldDef.Alias = this.edtAlias.Text || this.edtName.Text;
        this.FieldDef.TitleKey = this.edtTitleKey.Text;
        this.FieldDef.DataType = this.cboDataType.SelectedValue;
        this.FieldDef.MaxLength = tp.StrToInt(this.edtMaxLength.Text);
        this.FieldDef.Decimals = tp.StrToInt(this.edtDecimals.Text);

        this.FieldDef.CodeProviderName = this.edtCodeProviderName.Text;
        this.FieldDef.DefaultValue = this.edtDefaultValue.Text;
        this.FieldDef.Expression = this.edtExpression.Text;

        this.FieldDef.LocatorName = this.edtLocatorName.Text;

        this.FieldDef.LookUpTableName = this.edtLookUpTableName.Text;
        this.FieldDef.LookUpTableAlias = this.edtLookUpTableAlias.Text;
        this.FieldDef.LookUpKeyField = this.edtLookUpKeyField.Text;
        this.FieldDef.LookUpFieldList = this.edtLookUpFieldList.Text;
        this.FieldDef.LookUpTableSql = this.elSqlEditor.__Editor.getValue();

        this.FieldDef.Flags = tp.Bf.IntegerArrayToSetValue(this.lboFlags.SelectedValues);

        this.FieldDef.CheckDescriptor();

    }
}


/** The instance to be edited by this dialog box.
 * @type {tp.SqlBrokerFieldDef}
 * */
tp.SqlBrokerFieldDefEditDialog.prototype.FieldDef = null;



/**
Displays a modal dialog box for editing a {@link tp.SqlBrokerFieldDef} object
@static
@param {tp.SqlBrokerFieldDef} FieldDef The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlBrokerFieldDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlBrokerFieldDefEditDialog.ShowModalAsync = function (FieldDef, WindowArgs = null) {
    WindowArgs = WindowArgs || {};
    WindowArgs.Height = 550;
    WindowArgs.Text = WindowArgs.Text || 'Field Definition Editor';

    return tp.Window.ShowModalForAsync(FieldDef, tp.SqlBrokerFieldDefEditDialog, WindowArgs);
};

//#endregion

//#region SqlBrokerQueryDefEditDialog
/** Modal dialog box for editing a {@link tp.SqlBrokerQueryDef} descriptor
 */
tp.SqlBrokerQueryDefEditDialog = class extends tp.Window {
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
    tabFieldTitleList = null;


    /**
     * @type {tp.TextBox}
     */
    edtName = null;
    /** The element upon Ace Editor is created. 
     * The '__Editor' property of the element points to Ace Editor object.
     * @type {HTMLElement}
     */
    elSqlEditor = null;
    /**
     * @type {tp.Memo}
     */
    mmoFieldTitleList = null;

    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.SqlBrokerQueryDefEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.SqlQuery = this.Args.Instance;
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
        this.tabSql = this.Pager.AddPage('Sql');
        this.tabFieldTitleList = this.Pager.AddPage(_L('FieldTitles', 'Field Titles'));
        this.tabFieldTitleList.ToolTip = _L('QueryFieldTitles_ToolTip', `
A string list, where each string  has the format FIELD_NAME=TitleKey.
Determines the visibility of the fields in the drop-down grids:
if it is empty then all fields are visible
else only the included fields are visible
        `);


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

        // Sql Page
        // ---------------------------------------------------------------------------------
        this.elSqlEditor = tp.CreateSourceCodeEditor(this.tabSql.Handle, 'sql', this.SqlQuery.SqlText);


        // FieldTitleList Page
        // --------------------------------------------------------------------------------- 
        LayoutRow = new tp.Row(null, { Height: '100%' }); // add a tp.Row to the tab page
        this.tabFieldTitleList.AddComponent(LayoutRow);

        this.mmoFieldTitleList = new tp.Memo(null);
        this.mmoFieldTitleList.SetParent(LayoutRow);

        this.mmoFieldTitleList.CssText = `
width: calc(100% - 6px);
height: calc(100% - 6px);
font-family: monospace;
white-space: pre;
overflow: auto;

outline: none;
resize: none;
padding: 4px;
`;
 
    }

    /** Called just before a modal window is about to set its DialogResult property. <br />
     * Returning false cancels the setting of the property and the closing of the modal window. <br />
     * NOTE: Setting the DialogResult to any value other than <code>tp.DialogResult.None</code> closes a modal dialog window.
     * @override
     * @param {any} DialogResult
     */
    CanSetDialogResult(DialogResult) {
        if (DialogResult === tp.DialogResult.OK) {
            this.SqlQuery.Name = this.edtName.Text;
            this.SqlQuery.SqlText = this.elSqlEditor.__Editor.getValue();
            this.SqlQuery.FieldTitleKeys = this.mmoFieldTitleList.GetLines(true);

            this.SqlQuery.CheckDescriptor();
        }

        return true;
    }

    ItemToControls() {
        this.edtName.Text = this.SqlQuery.Name;
        if (tp.IsArray(this.SqlQuery.FieldTitleKeys))
            this.mmoFieldTitleList.Text = this.SqlQuery.FieldTitleKeys.join('\n');

    }
    ControlsToItem() {
        this.SqlQuery.Name = this.edtName.Text;
        this.SqlQuery.SqlText = this.elSqlEditor.__Editor.getValue();
        this.SqlQuery.FieldTitleKeys = this.mmoFieldTitleList.GetLines(true);

        this.SqlQuery.CheckDescriptor();
    }
}


/** The instance to be edited by this dialog box.
 * @type {tp.SqlBrokerQueryDef}
 * */
tp.SqlBrokerQueryDefEditDialog.prototype.SqlQuery = null;


 
/**
Displays a modal dialog box for editing a {@link tp.SqlBrokerQueryDef} object
@static
@param {tp.SqlBrokerQueryDef} SqlQuery The object to edit
@param {tp.WindowArgs} [WindowArgs=null] Optional.
@returns {tp.SqlBrokerQueryDefEditDialog} Returns the {@link tp.ContentWindow}  dialog box
*/
tp.SqlBrokerQueryDefEditDialog.ShowModalAsync = function (SqlQuery, WindowArgs = null) { 
    WindowArgs = WindowArgs || {};
    WindowArgs.Text = WindowArgs.Text || 'SqlBrokerQueryDef Definition Editor';
    return tp.Window.ShowModalForAsync(SqlQuery, tp.SqlBrokerQueryDefEditDialog, WindowArgs);
};

//#endregion

//---------------------------------------------------------------------------------------
// SysData View
//---------------------------------------------------------------------------------------

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
            case 'Locator':
                this.Handler = new tp.SysDataHandlerLocator(this);
                break;
            case 'CodeProvider':
                this.Handler = new tp.SysDataHandlerCodeProvider(this);
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
        this.Handler.CommitItemBefore(this.tblSysDataItem); // must be before any further check

        // default values
        let Row = this.tblSysDataItem.Rows[0];
        let v = Row.Get('TitleKey', '');
        if (!tp.IsValid(v) || tp.IsBlankString(v))
            Row.Set('TitleKey', Row.Get('DataName', ''));
 
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