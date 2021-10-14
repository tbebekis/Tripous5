
/** Handles the Filters panel of a {@link tp.DataView}. <br />
 * Displays a combo-box with the available {@link tp.SelectSql} statements for the user to choose one, define filters and execute it.
* */
tp.SqlFilterListUi = class extends tp.tpElement {

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

        this.tpClass = 'tp.SqlFilterListUi';
        this.fDefaultCssClasses = tp.Classes.SqlFilterListUi;
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
        let PanelInfo = elOption.PanelInfo;
        if (!tp.IsValid(PanelInfo.Panel)) {
            let el = this.PanelList.AddChild();
            let CP = {
                SelectSql: PanelInfo.SelectSql 
            }
            PanelInfo.Panel = new tp.SqlFilterPanel(el, CP);
        }

        this.PanelList.SelectedPanel = PanelInfo.Panel.Handle;
    }
};

/** The DIV where to build the Filters panel Ui.
 * @type {HTMLElement}
 */
tp.SqlFilterListUi.prototype.elPanel = null; 
/** A list of {@link tp.SelectSql} items to display. The first item must be named 'Main' is it is non-editable and non-deletable.
 * @type {tp.SelectSql[]}
 */
tp.SqlFilterListUi.prototype.SelectList = [];
/** ToolBar
 * @type {tp.ToolBar}
 */
tp.SqlFilterListUi.prototype.ToolBar = null;
/** ComboBox displaying the select statements
 * @type {tp.HtmlComboBox}
 */
tp.SqlFilterListUi.prototype.cboSelectList = null;
/** A panel list. Each panel corresponds to single {@link tp.SelectSql} instance.
 * @type {tp.PanelList}
 */
tp.SqlFilterListUi.prototype.PanelList = null;





// tp.SqlFilterPanel
// tp.SqlFilterControlLink


/** The container of the Ui of the filters of a single {@link tp.SelectSql} item.
 * */
tp.SqlFilterPanel = class extends tp.tpElement {
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

        this.tpClass = 'tp.SqlFilterPanel';
        this.fDefaultCssClasses = tp.Classes.SqlFilterPanel;
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
        // TODO: EDW, πρέπει να φτιάξουμε τα controls των φίλτρων.
        // SEE: Tripous2: SelectSqlBrowserUi ICriterionLink CriterionLink
    }
};