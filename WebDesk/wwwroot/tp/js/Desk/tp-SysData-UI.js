﻿//#region SqlFilterDefEditDialog
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
    DeleteFilterRow() {
        let Row = this.gridFilters.FocusedRow;
        if (tp.IsValid(Row)) {
            tp.YesNoBox('Delete selected row?', (Dialog) => {
                if (Dialog.DialogResult === tp.DialogResult.Yes) {
                    let Item = Row.OBJECT;
                    tp.ListRemove(this.SelectSql.Filters, Item);
                    this.tblFilters.RemoveRow(Row);
                }
            });
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
    return tp.Window.ShowModalForAsync(SqlQuery, tp.SqlBrokerQueryDefEditDialog, 'SqlBrokerQueryDef Definition Editor', WindowArgs);
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