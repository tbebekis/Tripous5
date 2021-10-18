//#region tp.SelectSqlListUi
/** Handles the combo-box with the available {@link tp.SelectSql} items for the user to choose one, define filters and execute it. <br />
 * Displays a panel list where each {@link tp.SelectSql} item has its own panel displaying its filter controls. <br />
 * A {@link tp.SelectSql} contains a list of {@link tp.SqlFilterDef} descriptor items. <br />
 * A filter descriptor is used in creating a filter item. <br />
 * A control link associates a Ui control and a filter item.
* */
tp.SelectSqlListUi = class extends tp.tpElement {

    /**
    Constructor <br />
    The passed-in element is DIV where to build the Filters panel Ui. <br />
    The CreateParams passed-in MUST contain a property named SelectList, 
    which is a list of {@link tp.SelectSql} items to display. The first item must be named 'Main' is it is non-editable and non-deletable.
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SelectSqlListUi';
        this.fDefaultCssClasses = tp.Classes.SelectSqlListUi;
    }


    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing 
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() {
        let InnerHTML =
`<div class="top-zone">
    <div class="select-list-container">
    </div>
    <div class="ToolBar">
        <a class="ButtonEx" data-setup="{ Command: 'Execute', Text: 'Execute', ToolTip: 'Execute' , IcoClasses: 'fa fa-bolt',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Clear', Text: 'Clear', ToolTip: 'Clear Filter' , IcoClasses: 'fa fa-trash-o',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'RowLimit', Text: 'Row Limit', ToolTip: 'Row Limit Active' , IcoClasses: 'fa fa-compress',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'ShowSql', Text: 'Show Sql', ToolTip: 'Show Sql' , IcoClasses: 'fa fa-file-text-o',  NoText: true, Ico: 'Left'}"></a>
    </div>
</div>
<div class="tp-PanelList">
</div>`;

        tp.Html(this.Handle, InnerHTML);

        let el, CP;

 
        // combo-box with SelectSql items
        el = tp.Select(this.Handle, '.select-list-container');
        CP = {
            Parent: el,
            Width: 240
        };

        this.cboSelectList = new tp.HtmlComboBox(null, CP);

        // SelectList comes from CreateParams.
        this.SelectList.forEach(item => {
            let elOption = this.cboSelectList.Add(item.Name, '');

            elOption.PanelInfo = {
                SelectSql: item,
                Panel: null
            }
        });

        this.cboSelectList.On('SelectedIndexChanged', this.cboSelectList_SelectedIndexChanged, this);

        // ToolBar
        el = tp.Select(this.Handle, '.ToolBar');
        this.ToolBar = new tp.ToolBar(el);
        this.ToolBar.On('ButtonClick', this.AnyClick, this);


        // PanelList
        el = tp.Select(this.Handle, '.tp-PanelList');
        this.PanelList = new tp.PanelList(el);
 

        this.cboSelectList.OnSelectedIndexChanged();

    }

 
    /* Event triggers */

    /**
    Event handler. If a Command exists in the clicked element then the Args.Command is assigned.
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    AnyClick(Args) {
        if (Args.Handled !== true) {
            var Command = tp.GetCommand(Args);
            if (!tp.IsBlank(Command)) {
                //this.ExecuteCommand(Command);
                switch (Command) {
                    case 'Execute':

                        break;
                }
                //alert(Command);
            }                
        }
    }
    /**
    Event handler. 
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    cboSelectList_SelectedIndexChanged(Args) {
        Args.Handled = true;
        let elOption = Args.Sender.SelectedItem;

        // create panels on-demand only
        let PanelInfo = elOption.PanelInfo;
        if (!tp.IsValid(PanelInfo.Panel)) {
            let el = this.PanelList.AddChild();
            let CP = {
                SelectSql: PanelInfo.SelectSql 
            }
            PanelInfo.Panel = new tp.SelectSqlUi(el, CP);
        }

        this.CurrentSelectSqlUi = PanelInfo.Panel;             // set the selected panel object
        this.PanelList.SelectedPanel = PanelInfo.Panel.Handle;
    }


    GetSelectedSelectSqlInfo() {
        let Result = {
            SelectSql: this.CurrentSelectSqlUi.GenerateSql(),
            RowLimit: this.RowLimit
        };
        return Result;
    }

};

/** The DIV where to build the Filters panel Ui.
 * @type {HTMLElement}
 */
tp.SelectSqlListUi.prototype.elPanel = null; 
/** A list of {@link tp.SelectSql} items to display. The first item must be named 'Main' is it is non-editable and non-deletable.
 * NOTE: Comes from CreateParams.
 * @type {tp.SelectSql[]}
 */
tp.SelectSqlListUi.prototype.SelectList = [];
/** ToolBar
 * @type {tp.ToolBar}
 */
tp.SelectSqlListUi.prototype.ToolBar = null;
/** ComboBox displaying the select statements
 * @type {tp.HtmlComboBox}
 */
tp.SelectSqlListUi.prototype.cboSelectList = null;
/** A panel list. Each panel corresponds to single {@link tp.SelectSql} instance.
 * @type {tp.PanelList}
 */
tp.SelectSqlListUi.prototype.PanelList = null;
/** The currently selected {@link tp.SelectSqlUi} panel object.
 * @type {tp.SelectSqlUi}
 */
tp.SelectSqlListUi.prototype.CurrentSelectSqlUi = null;
/** When true a row limit is applied to the current Sql statement.
 * @type {boolean}
 */
tp.SelectSqlListUi.prototype.RowLimit = false;
//#endregion


//#region tp.SelectSqlUi
/** The Ui built upon a single {@link tp.SelectSql} item and its filters list.
 * Actually this is a DIV displaying a control for each filter item defined in a {@link tp.SelectSql} instance.
 * */
tp.SelectSqlUi = class extends tp.tpElement {
    /**
    Constructor <br />
    The passed-in element is a DIV where to build the controls of the filter list. <br />
    The CreateParams passed-in MUST contain a property named SelectSql of type {@link tp.SelectSql}.
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SelectSqlUi';
        this.fDefaultCssClasses = tp.Classes.SelectSqlUi;
    }

    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing 
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() {
        this.CreateControls();
    }

    CreateControls() {
        if (!tp.IsValid(this.Filters)) {

            this.Filters = new tp.SqlFilters(this.SelectSql);

            if (tp.IsArray(this.Filters.List)) {
                this.Filters.List.forEach(item => {
                    let Link = new tp.SqlFilterControlLink(this, item);
                    Link.CreateControls();
                });
            }
        } 
    }

    GenerateSql() {
        return this.Filters.GenerateSql();
    }
 
};

/** A {@link tp.SelectSql} this panel represents. Contains the list for SqlFilter items.
 * NOTE: Comes from CreateParams.
 * @type {tp.SelectSql}
 */
tp.SelectSqlUi.prototype.SelectSql = null;
/** A {@link tp.SqlFilters} 
 * @type {tp.SqlFilters}
 */
tp.SelectSqlUi.prototype.Filters = null;

//#endregion



//#region tp.SqlFilters

/**
 * A list of {@link tp.SqlFilter} items. <br />
 * A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
 * */
tp.SqlFilters = class {
    /**
     * Constructor
     * @param {tp.SelectSql} SelectSql  A {@tp.SelectSql} instance with a  list of {@link tp.SqlFilterDef} items. A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
     */
    constructor(SelectSql) {
        this.List = [];
        this.SelectSql = SelectSql;
        this.FilterDefs = SelectSql.Filters;

        if (tp.IsArray(this.FilterDefs)) {
            this.FilterDefs.forEach(item => {
                let Filter = new tp.SqlFilter(this.SelectSql, item);
                this.List.push(Filter);
            });
        }
    }

    /** A {@link tp.SelectSql} this panel represents. Contains the list for SqlFilter items.
     * NOTE: Comes from CreateParams.
     * @type {tp.SelectSql}
     */
    SelectSql = null;
    /** A list of {@link tp.SqlFilterDef} items. A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
     * @type {tp.SqlFilterDef[]}
     */
    FilterDefs = null;
    /** A list of {@link tp.SqlFilter} items.
     * @type {tp.SqlFilter[]}
     */
    List = [];

    InfoText = '';

 
    GenerateSql() {
        this.List.forEach(item => {
            item.ControlLink.InputFromControls();
        });

        let Ref = {
            sWhere: '',
            sHaving: ''
        };

        let Result = this.GenerateSqlWhereAndHaving(Ref);
        return Result;
    }

    GenerateSqlWhereAndHaving(Ref) {
        this.InfoText = '';

        Ref.sWhere = tp.TrimEnd(Ref.sWhere);
        Ref.sHaving = tp.TrimEnd(Ref.sHaving); 

        let SS = new tp.SelectSql(this.SelectSql.Text);

        DoGenerateSql(SS, Ref, true);
        DoGenerateSql(SS, Ref, false);

        SS.WhereUser = Ref.sWhere;
        SS.Having = Ref.sHaving;

        return SS;
 
    }
    //DoGenerateSql(SelectSql SelectSql, ref string sClause, bool IsWhere)
    DoGenerateSql(SelectSql, Ref, IsWhere) {
        // EDW
    }
};
//#endregion


//#region tp.SqlFilter
/** A Sql Filter item. <br />
 * Represents a single data column that participates in a WHERE clause. <br />
 * It is actually an Sql generator, regarding that data column.  <br />
 * The data column can be of a string, integer, float, datetime or boolean-integer type.
 * An instance of this class uses a {@link tp.SqlFilterControlLink} instance to get control values.
 * */
tp.SqlFilter = class {
    /**
     * Constructor
     * @param {tp.SelectSql} SelectSql A {@link tp.SelectSql} instance this filter is part of.
     * @param {tp.SqlFilterDef} FilterDef The filter descriptor this filter represents.
     */
    constructor(SelectSql, FilterDef) {
        this.SelectSql = SelectSql;
        this.FilterDef = FilterDef;
        this.SelectedOptionsList = [];
    }


    /** A {@link tp.SelectSql} instance this filter is part of. 
     * @type {tp.SelectSql}
     */
    SelectSql = null;
    /** The filter descriptor this filter represents.
     * @type {tp.SqlFilterDef}
     */
    FilterDef = null;

    /** The value of the filter
     * @type {string}
     */
    Value = '';
    /** The "greater or equal" value as text.
     * @type {string}
     */
    Greater = '';
    /** The "less or equal" value as text.
     * @type {string}
     */
    Less = '';
    /** The boolean value used. One of the {@link tp.TriState} constants. Default means no value at all.
     * @type {number}
     */
    BoolValue = tp.TriState.Default;
    /** The date range. One of the {@link tp.DateRange} constants.
     * @type {number}
     */
    DateRange = tp.DateRange.Custom;
    /** The list of the selected options. 
     * It is used when the "data type" is "EnumQuery" or "EnumConst", when a list or a grid is presented to the user in order to make multiple choises.
     * @type {string[]}
     */
    SelectedOptionsList = [];
    /** The where operator used (and, not, or). One of the {@link tp.WhereOperator} constants.
     * @type {}
     */
    WhereOperator = tp.WhereOperator.And;

    /** Links this filter and a filter control.
     * @type {tp.SqlFilterControlLink}
     */
    ControlLink = null;

    /** Resets the values of the properties of this instance. */
    Clear() {
        this.Value = '';
        this.Greater = '';
        this.Less = '';
        this.BoolValue = tp.TriState.Default;
        this.this.DateRange = tp.DateRange.Custom;
        this.SelectedOptionsList = [];
        this.WhereOperator = tp.WhereOperator.And;
    }
    /** Generates and returns the sql text for the data column this instance represents.
     * @returns {string} Returns the sql text for the data column this instance represents.
     * */
    GenerateSql() {
        return ''; // TODO: GenerateSql
    }

};
//#endregion



//#region tp.SqlFilterControlLink

/** Represents a link between a {@link tp.SqlFilter} and a control on a Ui.
 * */
tp.SqlFilterControlLink = class {
    /**
     * 
     * @param {tp.SelectSql} SelectSql  A {@link tp.SelectSql} the filter of this link belongs to.
     * @param {tp.SqlFilter} SqlFilter A {@link tp.SqlFilter} filter item, representing a single data column that participates in a WHERE clause.
     */
    constructor(ParentObject, SqlFilter) {
        this.ParentObject = ParentObject;
        this.SelectSql = this.ParentObject.SelectSql;
        this.FilterDef = SqlFilter.FilterDef;
        this.SqlFilter = SqlFilter;
        this.SqlFilter.ControlLink = this;
    }

    /** A {@link tp.SelectSqlUi} instance. Provides the parent DOM element of the control of this instance.
     * @type {tp.SelectSqlUi}
     */
    ParentObject = null;
    /** A {@link tp.SelectSql} the filter of this link belongs to. 
     * @type {tp.SelectSql}
     */
    SelectSql = null;
    /** The filter descriptor this link is associated to.
     * @type {tp.SqlFilterDef}
     */
    FilterDef = null;
    /** The {@link tp.SqlFilter} filter item, representing a single data column that participates in a WHERE clause. 
     * This is where values of a control link are placed.
     * @type {tp.SqlFilter}
     */
    SqlFilter = null;

    /** The tripous script class type used in creating the filter controls (edtBox, edtFrom and edtTo) of this filter
     * @type {object}
     */
    ControlClassType = null;

    /** The DOM element of the whole filter row
     * @type {HTMLElement}
     * */
    elFilterRow = null;
    /** The DOM element containing the filter control
     * @type {HTMLElement}
     * */
    elControlContainer = null;

    /** Container of the 'from' control
     * @type {HTMLDivElement}
     */
    elFromControlContainer = null;
    /** Container of the 'to' control
     * @type {HTMLDivElement}
     */
    elToControlContainer = null;


    /** Label displaying the title of the filter, i.e. the field name.  
     * @type {HTMLDivElement}
     */
    lblTitle = null;
    /** Label displaying 'From'. Use when filter range is in use.
     * @type {HTMLDivElement}
     */
    lblFrom = null;
    /** Label displaying 'To'. Use when filter range is in use.
     * @type {HTMLDivElement}
     */
    lblTo = null;

    /** The main input control of this filter. Used when UseRange of the filter def is false. 
     * @type {tp.tpElement}
     */
    edtBox = null;
    /** The 'from' input control of this filter. Used when UseRange of the filter def is true.
     * @type {tp.tpElement}
     */
    edtFrom = null;
    /** The 'to' input control of this filter. Used when UseRange of the filter def is true.
     * @type {tp.tpElement}
     */
    edtTo = null;

    /** The data-table of an enum filter
     * @type {tp.DataTable}
     */
    tblEnum = null;
    /** A {@link tp.Grid} used with filters of type enum
     * @type {tp.Grid}
     */
    gridEnum = null;
    /** Internal. Used when this is an single-choise enum filter, in order to get notified when a row is selected and act accordingly.
     * @type {tp.DataSourceListener}
     */
    gridEnumDatasourceListener = null;
    /** Internal flag. Used when this is an single-choise enum filter, in order to prevent multiple choises
     * @type {boolean}
     */
    SettingSingleChoise = false;


    CreateControls() {

        this.CreateFilterRow();

        switch (this.FilterDef.Mode) {

            case tp.SqlFilterMode.Simple:
                switch (this.FilterDef.DataType) {
                    case tp.DataType.String:
                        this.ControlClassType = tp.Ui.Types.TextBox
                        break;
                    case tp.DataType.Integer:
                    case tp.DataType.Float:
                    case tp.DataType.Decimal:
                        this.ControlClassType = tp.Ui.Types.NumberBox
                        break;
                    case tp.DataType.Date:
                    case tp.DataType.DateTime:
                        this.ControlClassType = tp.Ui.Types.DateBox;
                              break;
                    case tp.DataType.Boolean:
                        tp.Throw("Boolean filters not yet ready");
                        //this.ControlClassType = tp.Ui.Types.CheckBox;
                        // TODO: we need a combo-box here with 3 states: all, true, false
                        break;
                }
                this.CreateControl_Simple();
                break;

            case tp.SqlFilterMode.EnumQuery:
            case tp.SqlFilterMode.EnumConst:
                this.CreateControl_Enum();
                break;
        }
    }
    GetControlCssClass() {
        switch (this.FilterDef.Mode) {
            case tp.SqlFilterMode.Simple:
                switch (this.FilterDef.DataType) {
                    case tp.DataType.Integer: return tp.Classes.SqlFilterInteger;
                    case tp.DataType.Float: return tp.Classes.SqlFilterFloat;
                    case tp.DataType.Decimal: return tp.Classes.SqlFilterDecimal;
                    case tp.DataType.Date: 
                    case tp.DataType.DateTime: return tp.Classes.SqlFilterDate;
                    case tp.DataType.Boolean: return tp.Classes.SqlFilterBoolean; 
                }
                break;
            case tp.SqlFilterMode.EnumQuery: return tp.Classes.SqlFilterEnumQuery;
            case tp.SqlFilterMode.EnumConst: return tp.Classes.SqlFilterEnumConst;
            case tp.SqlFilterMode.Locator: return tp.Classes.SqlFilterLocator;
        }

        return tp.Classes.SqlFilterString;
    }
    CreateFilterRow() {
        let HtmlText =
`<div class="${tp.Classes.SqlFilterRow}">
    <div class="${tp.Classes.Text}">${this.FilterDef.Title}</div>
    <div class="${tp.Classes.SqlFilterCtrl} ${this.GetControlCssClass()}"></div>
</div>`;

        this.elFilterRow = tp.Append(this.ParentObject.Handle, HtmlText);
        this.lblTitle =  tp.Select(this.elFilterRow, '.' + tp.Classes.Text);
        this.elControlContainer = tp.Select(this.elFilterRow, '.' + tp.Classes.SqlFilterCtrl);
    }
    CreateControl_Simple() {
 
        let UseRange = this.FilterDef.UseRange === true || this.FilterDef.DataType === tp.DataType.DateTime || this.FilterDef.DataType === tp.DataType.Date;
        if (UseRange)
            tp.AddClass(this.elControlContainer, tp.Classes.SqlFilterRange);

        if (!UseRange) {
            this.edtBox = new this.ControlClassType(null, { Parent: this.elControlContainer });
        }
        else {
            this.elFromControlContainer = tp.Append(this.elControlContainer, `<div class="${tp.Classes.From}"></div>`);
            this.lblFrom = tp.Append(this.elFromControlContainer, `<div>From</div>`);
            this.edtFrom = new this.ControlClassType(null, { Parent: this.elFromControlContainer });

            this.elToControlContainer = tp.Append(this.elControlContainer, `<div class="${tp.Classes.To}"></div>`);
            this.lblTo = tp.Append(this.elToControlContainer, `<div>To</div>`);
            this.edtTo = new this.ControlClassType(null, { Parent: this.elToControlContainer });
        }
    }
    async CreateControl_Enum() {

        let TableColumn, GridColumn;
        if (this.FilterDef.Mode === tp.SqlFilterMode.EnumQuery) {
            this.tblEnum = await tp.Db.SelectAsync(this.FilterDef.Enum.Sql);
        }
        else {
            this.FilterDef.Enum.ResultField = this.FilterDef.Enum.ResultField || 'Result';

            this.tblEnum = new tp.DataTable('tblEnum');
            this.tblEnum.AddColumn(this.FilterDef.Enum.ResultField, this.FilterDef.Enum.DataType, 140);

            this.FilterDef.Enum.OptionList.forEach(item => {
                this.tblEnum.AddRow([item]);
            });
        }

        if (!this.tblEnum.ContainsColumn('Include')) {
            TableColumn = this.tblEnum.AddColumn('Include', tp.DataType.Boolean, 0, 0);
            TableColumn.Title = '+/-';
        }

        if (this.FilterDef.Enum.IsMultiChoise === true && this.FilterDef.Enum.IncludeAll === true) {
            this.tblEnum.Rows.forEach(Row => {
                Row.Set('Include', true);
            });
        }

        this.gridEnum = new tp.Grid(null, { Parent: this.elControlContainer });
        this.gridEnum.ReadOnly = false;
        this.gridEnum.ToolBarVisible = false;
        this.gridEnum.GroupsVisible = false;
        this.gridEnum.FilterVisible = false;
        this.gridEnum.FooterVisible = false;
        this.gridEnum.ButtonInsertVisible = false;
        this.gridEnum.ButtonDeleteVisible = false;

        this.gridEnum.DataSource = this.tblEnum;

        GridColumn = this.gridEnum.GetColumn('Include');
        GridColumn.Width = 36;
        GridColumn.Text = '+/-';
        GridColumn.ToolTip = 'Include';

        this.gridEnum.SetColumnListWritable(['Include']);

        // when not multi-choise, ensure only a single row is selected
        if (this.FilterDef.Enum.IsMultiChoise !== true) {
            this.gridEnumDatasourceListener = new tp.DataSourceListener();            
            this.gridEnum.DataSource.AddDataListener(this.gridEnumDatasourceListener);
            this.gridEnumDatasourceListener.DataSourceRowModified = (Table, Row, Column, OldValue, NewValue) => { this.gridEnum_DataSourceRowModified(Table, Row, Column, OldValue, NewValue); };
        }
    }


    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    gridEnum_DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        let Flag = false;
        if (tp.IsSameText('Include', Column.Name)) {

            // when not multi-choise, ensure only a single row is selected
            if (this.SettingSingleChoise === false) {
                this.SettingSingleChoise = true;
                try {
                    let Index = Row.Table.IndexOfColumn(Column);
                    let v = Row.Get(Index);
                    let v2;
                    if (v === true) {
                        this.tblEnum.Rows.forEach((Row2) => {
                            if (Row !== Row2) {
                                v2 = Row2.Get(Index);
                                if (v2 === true) {
                                    Row2.Set(Index, false);
                                    Flag = true;
                                }
                            }
                        });
                    }
                } finally {
                    this.SettingSingleChoise = false;
                }
            }

        }

        if (Flag)
            this.gridEnum.RepaintRows();
    }

    GetControlValue(Control) {
        let Result = '';

        if (this.FilterDef.Mode === tp.SqlFilterMode.Simple) {

            if (this.FilterDef.DataType !== tp.DataType.Boolean) {
                Result = Control.Text;
                Result = tp.IsString(Result) ? Result.trim() : '';
                if (Result !== '') {
                    if (Bf.In(this.FilterDef.DataType, tp.DataType.Float | tp.DataType.Decimal)) {
                        Result = Result.replace(',', '.');
                    }
                }
            }
            else {
                // TODO: Boolean
            }
        }
        else {

        }

 

        return Result;
    }
    InputFromControls() {

        switch (this.FilterDef.Mode) {

            case tp.SqlFilterMode.Simple:
                switch (this.FilterDef.DataType) {
                    case tp.DataType.String:
                    case tp.DataType.Integer:
                    case tp.DataType.Float:
                    case tp.DataType.Decimal:
                        if (this.FilterDef.UseRange !== true) {
                            this.SqlFilter.Value = this.GetControlValue(this.edtBox);
                        }
                        else {
                            this.SqlFilter.Greater = this.GetControlValue(this.edtFrom);
                            this.SqlFilter.Less = this.GetControlValue(this.edtTo);
                        }
                        break;

                    case tp.DataType.Date:
                    case tp.DataType.DateTime:
                        this.SqlFilter.Greater = this.GetControlValue(this.edtFrom);
                        this.SqlFilter.Less = this.GetControlValue(this.edtTo);
                        break;

                    case tp.DataType.Boolean:
                        break;
                }
                break;

            case tp.SqlFilterMode.EnumQuery:
            case tp.SqlFilterMode.EnumConst:

                break;
        }
    }
    CleanControls() {
        if (tp.IsValid(this.edtBox)) {
            this.edtBox.Text = '';
        }

        if (tp.IsValid(this.tblEnum)) {
            let Flag = this.FilterDef.Enum.IsMultiChoise === true && this.FilterDef.Enum.IncludeAll === true;
            this.tblEnum.Rows.forEach(Row => {
                Row.Set('Include', Flag);
            });
        }
    }
 
};



//#endregion
 