

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
    <div class="ToolBar ${tp.Classes.SelectSqlListToolBar}">
        <a class="ButtonEx" data-setup="{ Command: 'Execute', Text: 'Execute', ToolTip: 'Execute' , IcoClasses: 'fa fa-bolt',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'ClearFilter', Text: 'Clear', ToolTip: 'Clear Filter' , IcoClasses: 'fa fa-trash-o',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'RowLimit', Text: 'Row Limit', ToolTip: 'Row Limit' , IcoClasses: 'fa fa-compress',  NoText: true, Ico: 'Left'}"></a>
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

        this.ToggleRowLimit();

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
    async AnyClick(Args) {
        if (Args.Handled !== true) {
            /** @type {tp.SelectSql} */
            let SelectSql;
            let SqlText;

            var Command = tp.GetCommand(Args);
            if (!tp.IsBlank(Command)) {
                switch (Command) {

                    case 'Execute':
                        SelectSql = this.CurrentSelectSqlUi.GenerateSql();
                        SqlText = SelectSql.Text;
                         

                        Args.Handled = true;
                        this.OnExecute();
                        break;

                    case 'ShowSql':
                        SelectSql = this.CurrentSelectSqlUi.GenerateSql();
                        SqlText = SelectSql.Text;
                        await tp.InfoBoxAsync(SqlText);
                        //await tp.InfoBoxAsync(this.CurrentSelectSqlUi.FilterValueList.InfoText);
                        Args.Handled = true;
                        break;

                    case 'ClearFilter':
                        this.CurrentSelectSqlUi.ClearControls();
                        Args.Handled = true;
                        break;

                    case 'RowLimit':
                        this.ToggleRowLimit();
                        Args.Handled = true;
                        break;
 
                }
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

    ToggleRowLimit() {
        let Btn = this.ToolBar.FindItemByCommand('RowLimit');
        tp.ToggleClass(Btn.Handle, tp.Classes.Active);
        this.RowLimit = !this.RowLimit;
    }

    /** Returns a {@link tp.SelectSql} instance along with user defined WHERE, if any.
     * @returns {tp.SelectSql} Returns a {@link tp.SelectSql} instance along with user defined WHERE, if any.
     * */
    GenerateSql() {
        return this.CurrentSelectSqlUi.GenerateSql();
    }


    /**
    Event trigger
    */
    OnExecute() { this.Trigger('Execute', {}); }
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
        if (!tp.IsValid(this.FilterValueList)) {
            this.ControlLinks = [];
            this.FilterValueList = new tp.SqlFilterValueList(this.SelectSql);

            if (tp.IsArray(this.FilterValueList.Items)) {
                this.FilterValueList.Items.forEach(item => {
                    let Link = new tp.SqlFilterControlLink(this, item);
                    this.ControlLinks.push(Link);
                    Link.CreateControls();
                });
            }
        } 
    }
    ClearControls() {
        if (tp.IsArray(this.ControlLinks) && this.ControlLinks.length > 0) {
            this.ControlLinks.forEach(item => {
                item.ClearControls();
            });
        }

        this.FilterValueList.ClearValues();
    }
    GenerateSql() {
        return this.FilterValueList.GenerateSql();
    }
 
};

/** A {@link tp.SelectSql} this panel represents. Contains the list for SqlFilter items.
 * NOTE: Comes from CreateParams.
 * @type {tp.SelectSql}
 */
tp.SelectSqlUi.prototype.SelectSql = null;
/** A {@link tp.SqlFilterValueList} 
 * @type {tp.SqlFilterValueList}
 */
tp.SelectSqlUi.prototype.FilterValueList = null;
/** A list {@link tp.SqlFilterControlLink} control links.
 * @type {tp.SqlFilterControlLink[]}
 */
tp.SelectSqlUi.prototype.ControlLinks = [];
//#endregion

//#region tp.SqlFilterValueList

/**
 * A list of {@link tp.SqlFilterValue} items. <br />
 * A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
 * */
tp.SqlFilterValueList = class {
    /**
     * Constructor
     * @param {tp.SelectSql} SelectSql  A {@tp.SelectSql} instance with a  list of {@link tp.SqlFilterDef} items. A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
     */
    constructor(SelectSql) {
        this.Items = [];
        this.SelectSql = SelectSql;
        this.FilterDefs = SelectSql.Filters;

        if (tp.IsArray(this.FilterDefs)) {
            this.FilterDefs.forEach(item => {
                let Filter = new tp.SqlFilterValue(this.SelectSql, item);
                this.Items.push(Filter);
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
    /** A list of {@link tp.SqlFilterValue} items.
     * @type {tp.SqlFilterValue[]}
     */
    Items = [];
    /** The informational text. Infotext is a user friendly version of the generated Sql.
     * @type {string}
     */
    InfoText = '';  


    ClearValues() {
        this.Items.forEach(item => { item.Clear(); });
    }
    GenerateSql() {
        this.Items.forEach(item => {
            item.ControlLink.InputFromControls();
        });

        let Ref = {
            sWhere: '',
            sHaving: ''
        };

        /** @type {tp.SelectSql} */
        let Result = this.GenerateSqlWhereAndHaving(Ref);
        return Result;
    }

    GenerateSqlWhereAndHaving(Ref) {
        this.InfoText = '';

        Ref.sWhere = tp.TrimEnd(Ref.sWhere);
        Ref.sHaving = tp.TrimEnd(Ref.sHaving); 

        let SS = this.SelectSql;  

        this.DoGenerateSql(SS, Ref, true);
        this.DoGenerateSql(SS, Ref, false);

        SS.WhereUser = Ref.sWhere;
        SS.Having = Ref.sHaving;

        return SS; 
    }

    /**
     * Private method. Prepares a clause (UserWhere or Having).
     * @param {tp.SelectSql} SelectSql A {@tp.SelectSql} to operate on
     * @param {object} Ref An object of type { sWhere: '', sHaving: '' }
     * @param {boolean} IsWhere A boolean value indicating what property of the 'Ref' value to use as clause.
     */
    DoGenerateSql(SelectSql, Ref, IsWhere) {

        let sClause = IsWhere === true ? Ref.sWhere : Ref.sHaving;

        /** @type {tp.SqlFilterValue[]} */
        let List = [];

        // get the right items
        this.Items.forEach(item => {
            if (IsWhere === true) {
                if (item.FilterDef.PutInHaving !== true)
                    List.push(item);
            }
            else {
                if (item.FilterDef.PutInHaving == true)
                    List.push(item);
            }
        });

        let sInfoText, S, PrefixConcat;
        List.forEach(item => {
            sInfoText = '';
            S = item.GenerateSql();

            PrefixConcat = sClause.length > 0
                || (sClause.length === 0 && List.indexOf(item) === 0 && IsWhere === true && !tp.IsBlank(SelectSql.Where));


            if (S.length > 0) {

                switch (item.WhereOperator) {
                    case tp.WhereOperator.And:
                        if (PrefixConcat) {
                            sClause = sClause.length == 0 ? ` and ${S} ` : `${sClause} ${tp.LB}  and ${S} `;  
                            sInfoText += `and ${S}`;
                        }
                        else {
                            sClause = `      ${S}  `; 
                            sInfoText += S ; 
                        }
                        break;

                    case tp.WhereOperator.Not:
                        if (PrefixConcat) {
                            sClause = sClause.length == 0 ? ` and not ${S} ` : `${sClause} ${tp.LB}  and not ${S} `; 
                            sInfoText += `and not ${S}`;  
                        }
                        else {
                            sClause =  `      not ${S}  ` ;  
                            sInfoText += `not ${S}`; 
                        }
                        break;

                    case tp.WhereOperator.Or:
                        if (PrefixConcat) {
                            sClause = sClause.length == 0 ? ` or ${S} ` : `${sClause} ${tp.LB}  or ${S} `;
                            sInfoText += `or ${S}`;
                        }
                        else {
                            sClause = `      ${S}  `; 
                            sInfoText += S;
                        }
                        break;
                }

                if (IsWhere === true)
                    Ref.sWhere = sClause;
                else
                    Ref.sHaving = sClause;

                if (sInfoText.length > 0)
                    this.InfoText += `${item.FilterDef.Title} : ${sInfoText}  ${tp.LB}`;   

            }


        }); 

    }
};
//#endregion

//#region tp.SqlFilterValue
/** A Sql Filter item. <br />
 * Represents a single data column that participates in a WHERE clause. <br />
 * It is actually an Sql generator, regarding that data column.  <br />
 * The data column can be of a string, integer, float, datetime or boolean-integer type.
 * An instance of this class uses a {@link tp.SqlFilterControlLink} instance to get control values.
 * */
tp.SqlFilterValue = class {
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
        this.DateRange = tp.DateRange.Custom;
        this.SelectedOptionsList = [];
        this.WhereOperator = tp.WhereOperator.And;
    }
    /** Generates and returns the sql text for the data column this instance represents.
     * @returns {string} Returns the sql text for the data column this instance represents.
     * */
    GenerateSql() {
        if (tp.IsEmpty(this.FilterDef))
            return '';

        let Result = '';
        let DataType = this.FilterDef.DataType;
        //let QuotedDateTimes = true;

        let FullName = this.FilterDef.FieldPath;
        //let FromName = FullName + tp.FromField;
        //let ToName = FullName + tp.ToField;

        if (this.FilterDef.PutInHaving === true && !tp.IsBlank(this.FilterDef.AggregateFunc))
            FullName = ` ${this.FilterDef.AggregateFunc}(${FullName}) `;


        // WHERE clause generation
        let S = '';
        if (this.FilterDef.Mode == tp.SqlFilterMode.Simple) {
            
            // no range
            if (this.FilterDef.UseRange !== true && (DataType === tp.DataType.String || tp.DataType.IsNumber(DataType))) {

                if (DataType === tp.DataType.String && !tp.IsBlank(this.Value)) {
                    S = tp.Sql.NormalizeMaskForce(this.Value);
                    S = S.trim();
                    Result = ` (${FullName} ${S}) `;
                }
                else if (tp.DataType.IsNumber(DataType) && !tp.IsBlank(this.Value)) {
                    S = ' = ' + this.Value.trim();
                    Result = ` (${FullName}${S}) `;
                }
            }
            // range
            else if (tp.DataType.IsDateTime(DataType) || (this.FilterDef.UseRange === true && (DataType === tp.DataType.String || tp.DataType.IsNumber(DataType)))) {

                let sG = '';
                let sL = '';

                // range, for all types except Date
                if (!tp.IsBlank(this.Greater)) {
                    switch (DataType) {
                        case tp.DataType.Integer: sG = this.Greater; break;
                        case tp.DataType.Float:
                        case tp.DataType.Decimal: sG = this.Greater.replace(',', '.'); break;
                        case tp.DataType.String: sG = `'${this.Greater}'`; break;
                    }
                }

                if (!tp.IsBlank(this.Less)) {
                    switch (DataType) {
                        case tp.DataType.Integer: sL = this.Less; break;
                        case tp.DataType.Float:
                        case tp.DataType.Decimal: sL = this.Less.replace(',', '.'); break;
                        case tp.DataType.String: sL = `'${this.Less}'`; break;
                    }
                }

                // range for Dates (Date is always a range)
                if (tp.DataType.IsDateTime(DataType)) {

                    let FromDate = null;
                    let ToDate = null;

                    if (this.DateRange !== tp.DateRange.Custom) {
                        let o = tp.DateRanges.ToDates(this.DateRange, tp.Today());
                        if (o.Result === true) {
                            FromDate = o.FromDate;
                            ToDate = o.ToDate;
                        }
                    }
                    else {

                        if (!tp.IsBlank(this.Greater)) {
                            let o = tp.TryParseDateTime(this.Greater);
                            if (o.Result === true) {
                                FromDate = o.Value;
                            }
                        }

                        if (!tp.IsBlank(this.Less)) {
                            let o = tp.TryParseDateTime(this.Less);
                            if (o.Result === true) {
                                ToDate = o.Value;
                            }
                        }
                    }


                    if (tp.IsDate(FromDate)) {
                        FromDate = tp.StartOfDay(FromDate);
                        sG = tp.FormatDateTime(FromDate, 'yyyy-MM-dd HH:mm:ss'); // ISO date
                        sG = `'${sG}'`;
                    }

                    if (tp.IsDate(ToDate)) {
                        ToDate = tp.EndOfDay(ToDate);
                        sL = tp.FormatDateTime(ToDate, 'yyyy-MM-dd HH:mm:ss'); // ISO date
                        sL = `'${sL}'`;
                    }

                }            

                if (!tp.IsBlank(sG))
                    sG = ` (${FullName} >= ${sG}) `

                if (!tp.IsBlank(sL))
                    sL = ` (${FullName} <= ${sL}) `;

                if (!tp.IsBlank(sG) && !tp.IsBlank(sL))
                    Result = ` (${sG} and ${sL}) `; 
                else if (!tp.IsBlank(sG))
                    Result = sG;
                else if (!tp.IsBlank(sL))
                    Result = sL;

            }
            // boolean
            else if (DataType === tp.DataType.Boolean) {
                if (this.BoolValue !== tp.TriState.Default) {
                    S = this.BoolValue == tp.TriState.True ? '= 1' : '<> 1';        // assumes that boolean values are actually integer values  
                    Result = ` (${FullName} ${S}) `;
                }
            }
        }
        else if (tp.Bf.In(this.FilterDef.Mode, tp.SqlFilterMode.EnumConst | tp.SqlFilterMode.EnumQuery) && (DataType === tp.DataType.String || DataType === tp.DataType.Integer)) {

            S = '';

            if (tp.IsArray(this.SelectedOptionsList) && this.SelectedOptionsList.length > 0) {
                if (this.SelectedOptionsList.length === 1) {
                    S = DataType === tp.DataType.String ? `'${this.SelectedOptionsList[0]}'` : this.SelectedOptionsList[0];
                    if (!tp.IsBlank(S))
                        Result = ` (${FullName} = ${S}) `;
                }
                else {
                    this.SelectedOptionsList.forEach(item => {
                        S += DataType === tp.DataType.String ? `'${item}', ` : `${item}, `;
                    });

                    S = tp.RemoveLastComma(S);
                    if (!tp.IsBlank(S))
                        Result = ` (${FullName} in (${S})) `; 
                }
            }
 
        }



        return Result; 
    }

};
//#endregion


//#region tp.SqlFilterControlLink

/** Represents a link between a {@link tp.SqlFilterValue} and a control on a Ui.
 * */
tp.SqlFilterControlLink = class {
    /**
     * 
     * @param {tp.SelectSql} SelectSql  A {@link tp.SelectSql} the filter of this link belongs to.
     * @param {tp.SqlFilterValue} SqlFilterValue A {@link tp.SqlFilterValue} filter item, representing a single data column that participates in a WHERE clause.
     */
    constructor(ParentObject, SqlFilterValue) {
        this.ParentObject = ParentObject;
        this.SelectSql = this.ParentObject.SelectSql;
        this.FilterDef = SqlFilterValue.FilterDef;
        this.FilterValue = SqlFilterValue;
        this.FilterValue.ControlLink = this;
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
    /** The {@link tp.SqlFilterValue} filter item, representing a single data column that participates in a WHERE clause. 
     * This is where values of a control link are placed.
     * @type {tp.SqlFilterValue}
     */
    FilterValue = null;

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

    /** Container of the 'from' and 'to' containers
     * @type {HTMLDivElement}
     */
    elRangeContainer = null;
    /** Container of the 'from' control
     * @type {HTMLDivElement}
     */
    elFromControlContainer = null;
    /** Container of the 'to' control
     * @type {HTMLDivElement}
     */
    elToControlContainer = null;

    elDateRangeSelectorContainer = null;

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

    lblDateRange = null;

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
    /** A combo-box displaying  
     * @type {tp.HtmlComboBox}
     */
    cboDateRange = null;  
 
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
                        this.ControlClassType = tp.Ui.Types.HtmlComboBox;
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

    CreateFilterRow() {

/*
<div class="tp-SqlFilterRow">
    <div class="tp-Text"></div>
    <div class="tp-SqlFilter-DateRangeSelector">
        <div class="tp-Text"></div>
        @* DateRange combo-box here *@
    </div>
    <div class="tp-SqlFilter-Ctrl">
        @* one or two controls here *@
    </div>
</div>
 */
        let HtmlText =
`<div class="${tp.Classes.SqlFilterRow}">
    <div class="${tp.Classes.Text}">${this.FilterDef.Title}</div>    
</div>`;

        this.elFilterRow = tp.Append(this.ParentObject.Handle, HtmlText);
        this.lblTitle = tp.Select(this.elFilterRow, '.' + tp.Classes.Text);

        // date-range selector
        if (tp.DataType.IsDateTime(this.FilterDef.DataType)) {
            let DateRangeSelectorHtmlText =
`<div class="${tp.Classes.SqlFilterDateRangeSelector}">
</div>`;

            this.elDateRangeSelectorContainer = tp.Append(this.elFilterRow, DateRangeSelectorHtmlText);
        }
 

        // control container
        let ControlContainerHtmlText =
            `<div class="${tp.Classes.SqlFilterCtrl}"></div>`;

        this.elControlContainer = tp.Append(this.elFilterRow, ControlContainerHtmlText);
    }

    CreateControl_Simple() {
        let i, ln;

        let UseRange = this.FilterDef.UseRange === true || tp.DataType.IsDateTime(this.FilterDef.DataType); // this.FilterDef.DataType === tp.DataType.DateTime || this.FilterDef.DataType === tp.DataType.Date;
 

        if (!UseRange) {
            this.edtBox = new this.ControlClassType(null, { Parent: this.elControlContainer });

            if (this.FilterDef.DataType === tp.DataType.Boolean) {
                this.edtBox.Add('', tp.TriState.Default);
                this.edtBox.Add('False', tp.TriState.False);
                this.edtBox.Add('True', tp.TriState.True);
            }
        }
        else {

            if (tp.DataType.IsDateTime(this.FilterDef.DataType)) {
 
                this.lblDateRange = tp.Append(this.elDateRangeSelectorContainer, `<div class="${tp.Classes.Text}">Range</div>`);
                this.cboDateRange = new tp.HtmlComboBox(null, { Parent: this.elDateRangeSelectorContainer });

                for (i = 0, ln = tp.DateRanges.WhereRanges.length; i < ln; i++) {
                    this.cboDateRange.Add(tp.DateRanges.WhereRangesTexts[i], tp.DateRanges.WhereRanges[i]);
                }

                this.cboDateRange.SelectedIndex = 0;

                this.cboDateRange.On('SelectedIndexChanged', this.cboDateRange_SelectedIndexChanged, this);
            } 
                
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
    /**
     * Event handler. Handles the SelectedIndexChanged event of the Associate.
     * @protected
     * @param {tp.EventArgs} Args A tp.EventArgs object.
     */
    cboDateRange_SelectedIndexChanged(Args) {
        let v = tp.StrToInt(this.cboDateRange.SelectedValue);
        let IsCustom = v === tp.DateRange.Custom;

        if (IsCustom) {
            this.edtFrom.Text = '';
            this.edtTo.Text = '';
        } else {
            let RangeResult = tp.DateRanges.ToDates(v);
            if (RangeResult.Result === true) {
                this.edtFrom.Date = RangeResult.FromDate;
                this.edtTo.Date = RangeResult.ToDate;
            }
        }

        this.edtFrom.Enabled = IsCustom;
        this.edtTo.Enabled = IsCustom;

        this.edtFrom.ReadOnly = !IsCustom;
        this.edtTo.ReadOnly = !IsCustom;
    }

    InputFromControls() {

        // -------------------------------------------------------------------------------------
        let GetControlValue = (Control) => {
            let Result = null;

            if (this.FilterDef.Mode === tp.SqlFilterMode.Simple) {

                if (this.FilterDef.DataType !== tp.DataType.Boolean) {
                    Result = Control.Text;
                    Result = tp.IsString(Result) ? Result.trim() : '';
                    if (Result !== '') {
                        if (tp.Bf.In(this.FilterDef.DataType, tp.DataType.Float | tp.DataType.Decimal)) {
                            Result = Result.replace(',', '.');
                        }
                    }
                }
                else {
                    Result = Control.SelectedValue; // edtBox is a tp.HtmlComboBox
                }
            }
            else {
                Result = [];
 
                let ResultField = this.FilterDef.Enum.ResultField;
                if (tp.IsString(ResultField) && !tp.IsBlank(ResultField) && this.tblEnum.ContainsColumn(ResultField)) {
                    let v;
                    this.tblEnum.Rows.forEach(Row => {
                        if (Row.Get('Include', false) === true) {
                            v = Row.Get(ResultField, null);
                            if (tp.IsValid(v))
                                Result.push(!tp.IsString(v)? v.toString(): v);
                        }
                    });
                }

            }

            return Result;
        };
        // -------------------------------------------------------------------------------------

        
        switch (this.FilterDef.Mode) {

            case tp.SqlFilterMode.Simple:
                switch (this.FilterDef.DataType) {
                    case tp.DataType.String:
                    case tp.DataType.Integer:
                    case tp.DataType.Float:
                    case tp.DataType.Decimal:
                        if (this.FilterDef.UseRange !== true) {
                            this.FilterValue.Value = GetControlValue(this.edtBox);
                        }
                        else {
                            this.FilterValue.Greater = GetControlValue(this.edtFrom);  
                            this.FilterValue.Less = GetControlValue(this.edtTo);
                        }
                        break;

                    case tp.DataType.Date:
                    case tp.DataType.DateTime:
                        this.FilterValue.Greater = GetControlValue(this.edtFrom);
                        this.FilterValue.Less = GetControlValue(this.edtTo);
                        break;

                    case tp.DataType.Boolean:
                        let S = GetControlValue(this.edtBox);
                        if (!tp.IsBlank(S)) {
                            this.FilterValue.BoolValue = tp.StrToInt(S);
                        }
                        break;
                }
                break;

            case tp.SqlFilterMode.EnumQuery:
            case tp.SqlFilterMode.EnumConst:
                this.FilterValue.SelectedOptionsList = GetControlValue(null);
                break;
        }
    }
    ClearControls() {  
        if (tp.IsValid(this.edtBox)) {
            if (this.FilterDef.DataType === tp.DataType.Boolean)
                this.edtBox.SelectedIndex = 0;
            else
                this.edtBox.Text = '';
        }

        if (tp.IsValid(this.edtFrom))
            this.edtFrom.Text = '';

        if (tp.IsValid(this.edtTo))
            this.edtTo.Text = '';

        if (tp.IsValid(this.tblEnum)) {
            let Flag = this.FilterDef.Enum.IsMultiChoise === true && this.FilterDef.Enum.IncludeAll === true;
            this.tblEnum.Rows.forEach(Row => {
                Row.Set('Include', Flag);
            });

            this.gridEnum.RepaintRows();
        }
    }
 
};



//#endregion
 