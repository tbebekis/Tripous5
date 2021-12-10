
tp.SqlFilterDialog = class extends tp.tpWindow {
    /**
    Constructor
    @param {tp.WindowArgs} Args - Setup options
    */
    constructor(Args) {
        super(Args);
    }


    /* overrides */
    /**
    Override
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SqlFilterDialog';
    }
    /**
    Override
    @protected
    @override
    */
    CreateControls() {
        super.CreateControls();

/*
        this.Table = this.Args.Table;
        this.VisibleColumns = this.Args.VisibleColumns || [];
        this.SelectSql = this.Args.SelectSql;
        this.SqlText = this.Args.SqlText;
        this.KeyValue = this.Args.KeyValue;
        this.KeyFieldName = this.Args.KeyFieldName;
 */

        this.ContentWrapper.StyleProp('display', 'flex');
        this.ContentWrapper.StyleProp('flex-direction', 'column');
        this.ContentWrapper.StyleProp('gap', '2px');
 
        // tool-bar 
        this.ToolBar = new tp.ToolBar();
        this.ToolBar.Parent = this.ContentWrapper;
        this.ToolBar.AddClass(tp.Classes.ToolBar);        

        // Command, Text, ToolTip, IcoClasses, CssClasses, ToRight
        this.btnExecute = this.ToolBar.AddButton('Execute', 'Execute', 'Execute', 'fa fa-bolt');
        this.btnClearFilter = this.ToolBar.AddButton('ClearFilter', 'Clear', 'Clear Filter', 'fa fa-trash-o');
        this.btnShowSql = this.ToolBar.AddButton('ShowSql', 'Show Sql', 'Show Sql', 'fa fa-file-text-o');
        this.btnShowIdColumns = this.ToolBar.AddButton('ShowIdColumns', 'Show/Hide Id Columns', 'Show/Hide Id Columns', 'fa fa-th-list');  
        
        this.ToolBar.On('ButtonClick', this.AnyClick, this);


        this.ToolBar.SetNoText(true);
 

        // footer buttons
        this.btnOK = this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK, false);
        this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel, false);

        // tab control
        this.Pager = new tp.TabControl();
        this.Pager.Parent = this.ContentWrapper;
        this.Pager.Height = '100%';
        this.tabFilter = this.Pager.AddPage('Filter');
        this.tabGrid = this.Pager.AddPage('Data');

        alert(this.tabGrid instanceof tp.tpElement);


        ///////////////////////////////////////////
        this.Grid = new tp.Grid();
        this.Grid.Parent = this.tabGrid;
        this.Args.Grid = this.Grid;
 
        this.Grid.Height = '100%';
        this.Grid.Width = '100%';

        this.Grid.ReadOnly = true;
        this.Grid.ToolBarVisible = false;
        this.Grid.GroupsVisible = true;
        this.Grid.FilterVisible = true;
        this.Grid.FooterVisible = false;

        //this.Grid.On(tp.Events.DoubleClick, this.DoubleClick, this);

 
        //this.LoadDataAsync();

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
                tp.InfoNote(Command);
                /*
                switch (Command) {
                    case 'Execute':
                        SelectSql = this.GenerateSql();
                        SqlText = SelectSql.Text;
 
                        Args.Handled = true;
                        this.OnExecute();
                        break;

                    case 'ShowSql':
                        SelectSql = this.GenerateSql();
                        SqlText = SelectSql.Text;
                        await tp.InfoBoxAsync(SqlText);
                        Args.Handled = true;
                        break;

                    case 'ClearFilter':
                        this.SelectedFilterPanel.ClearControls();
                        Args.Handled = true;
                        break;

                    case 'ShowIdColumns':
                        this.Grid.ShowHideIdGridColumns();
                        break;
                }
                */
            }
        }
    }
};


/** Field
 * @field
 * @type {tp.ControlToolBar}
 */
tp.SqlFilterDialog.prototype.ToolBar = null;

/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnExecute = false;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnClearFilter = false;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnShowSql = false;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnShowIdColumns = false;
/** Field
 * @field
 * @type {tp.tpElement}
 */
tp.SqlFilterDialog.prototype.btnOK = null;
/** Field
 * @field
 * @type {tp.TabControl}
 */
tp.SqlFilterDialog.prototype.Pager = null;
/** Field
 * @field
 * @type {tp.TabPage}
 */
tp.SqlFilterDialog.prototype.tabFilter = null;
/** Field
 * @field
 * @type {tp.TabPage}
 */
tp.SqlFilterDialog.prototype.tabGrid = null;

/** Field
 * @field
 * @type {tp.Grid}
 */
tp.SqlFilterDialog.prototype.Grid = null;
/** Field
 * @field
 * @type {tp.DataRow}
 */
tp.SqlFilterDialog.prototype.SelectedRow = null;
/** Field
 * @field
 * @type {tp.DataTable}
 */
tp.SqlFilterDialog.prototype.Table = null;

/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object. Must have (mandatory), at least, defined either a  <code>Table: tp.DataTable</code> or a <code>SqlText: string </code> or a <code>SelectSql: tp.SelectSql </code> property.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.SqlFilterBox = function (Args) {
    let Result = new tp.SqlFilterDialog(Args);
    Result.ShowModal();
    return Result;
};
/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object. Must have (mandatory), at least, defined either a  <code>Table: tp.DataTable</code> or a <code>SqlText: string </code> or a <code>SelectSql: tp.SelectSql </code> property.
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.SqlFilterBoxAsync = function (Args) { 

    let Result = new Promise((Resolve, Reject) => {
        Args.CloseFunc = (Args) => {
            Resolve(Args);
        };
        tp.SqlFilterBox(Args);
    });

    return Result;
 
};