
tp.Urls.DeskGetMainMenu = '/Desk/GetMainMenu';

tp.Classes.Desk = 'tp-Desk';
tp.Classes.DeskMainMenu = 'tp-Desk-MainMenu';
tp.Classes.DeskMainMenuBarItem = 'tp-Desk-MainMenuBarItem';
tp.Classes.DeskViewPager = 'tp-Desk-ViewPager';
tp.Classes.DeskView = 'tp-Desk-View';
tp.Classes.DeskSysDataView = 'tp-Desk-SysData-View';

//#region tp.Command

/** A command class. This is here mostly for reference only. */
tp.Command = class {

    /** constructor */
    constructor(Source = null) {
        this.Params = {};
        this.Items = [];

        if (Source)
            this.Assign(Source);
    }

    /** Database Id
     * @type {string}
     */
    Id = '';
    /** The command type. What a command does when it is called. 
     * @type {string}
     */
    Type = 'Ui';
    /** A name unique among all commands.
     * @type {string}
     * */
    Name = '';

    /** The text title of this instance, used for display purposes.
     * @type {string}
     * */
    Title = '';

    /** Icon key
     * @type {string}
     * */
    IconKey = '';
    /** True when this is a single instance Ui command. */
    IsSingleInstance = false;

    /** The list of child commands, if any, else null. */
    Items = [];
    /** User defined parameters */
    Params = {};

    /** Assigns this instance's properties from a specified source object.
     * @param {object} Source
     */
    Assign(Source) {
        if (tp.IsValid(Source)) {
            this.Id = Source.Id || '';
            this.Type = Source.Type || '';
            this.Name = Source.Name || '';
            this.Title = Source.Title || '';
            this.IconKey = Source.IconKey || '';
            this.IsSingleInstance = Source.IsSingleInstance === true;
            this.Params = {};
            if (Source.Params)
                tp.MergeQuick(this.Params, Source.Params);

            this.Items = [];

            if (tp.IsArray(Source.Items)) {
                Source.Items.forEach((item) => {
                    let Cmd = new tp.Command();
                    this.Items.push(Cmd);
                    Cmd.Assign(item);
                });
            } 
        }
    }
    IsUiCommand() {
        return this.Type === 'Ui' || tp.StartsWith(this.Name, 'Ui.', true);
    }
};

//#endregion
 
//---------------------------------------------------------------------------------------
// desktop
//---------------------------------------------------------------------------------------

//#region tp.AjaxRequest
/** Creates and returns an instance based on a specified {@link tp.Command}
 * @param {tp.Command} Cmd
 */
tp.AjaxRequest.CreateFromCommand = function (Cmd) {
 
    let Params = {
        Type: Cmd.Type,
        IsSingleInstance: Cmd.IsSingleInstance === true
    };
    if (Cmd.Params)
        Params = tp.MergeQuick(Params, Cmd.Params);

    let Result = new tp.AjaxRequest(Cmd.Name, Params);
    return Result;
};
//#endregion

//#region tp.Desktop

tp.DeskOptions = {

    /**Header
     * @type {tp.tpElement}
     */
    Header: null,
    /** Footer
     * @type {tp.tpElement}
     */
    Footer: null,
    /** MainMenu
     * @type {tp.DeskMainMenu}
     */
    MainMenu: null,
    /** MainCommandExecutor
     * @type {tp.DeskCommandExecutor}
     */
    MainCommandExecutor: null,
    /** ViewPager
     * @type {tp.DeskViewPager}
     */
    ViewPager: null, 
};

/**
 * @type {tp.Desktop}
 * */
tp.Desk = null;


tp.Desktop = class {
    /**
     * 
     * @param {tp.DeskOptions} Options
     */
    constructor(Options) {
        if (tp.Desk)
            tp.Throw('Desk is already created');

        tp.Desk = this;

        this.Options = Options || {};
        tp.MergeQuick(this, this.Options);

        this.CommandExecutors = [];
        this.RegisterCommandExecutor(this.MainCommandExecutor);
    }

    Options = {};

    /** A list of registered command executors.
     * @type {array}
     */
    CommandExecutors = [];

    /**Header
     * @type {tp.tpElement}
     */
    Header = null;
    /** Footer
     * @type {tp.tpElement}
     */
    Footer = null;
    /** MainMenu. A {@link tp.ItemBar} descendant. Displays drop-downs with menu items. Each menu-item contains a {@link tp.Command}
     * @type {tp.DeskMainMenu}
     */
    MainMenu = null;
    /** MainCommandExecutor
     * @type {tp.DeskCommandExecutor}
     */
    MainCommandExecutor = null;
    /** ViewPager. A composite control. Handles the tab-control-like Ui of creating, displaying and destroying views.
     * Contains the TabBar, a {@link tp.ItemBar}, with an item for each created page.
     * Contains the PageList, a (@link tp.tpElement), with the created pages.
     * @type {tp.DeskViewPager}
     */
    ViewPager = null;

    /**
     * Registers a command executor
     * @param {object} Executor The instance to register. Must provider a CanExecuteCommand(Cmd) and an async ExecuteCommand(Cmd) functions.
     */
    RegisterCommandExecutor(Executor) {
        this.CommandExecutors.push(Executor);
    }

 
    /**
     * Calls all registered command executors until it finds one that can execute the command. Then it passes the command to that executor.
     * @param {tp.Command} Cmd
     */
    async ExecuteCommand(Cmd) {
        let i, ln, Executor;
        let Result = null;
        for (i = 0, ln = this.CommandExecutors.length; i < ln; i++) {
            Executor = this.CommandExecutors[i];
            if (Executor.CanExecuteCommand(Cmd)) {
                Result = await Executor.ExecuteCommand(Cmd);
                break;
            }
        }
        return Result;
    }

};
//#endregion

//#region tp.DeskMainMenu

/** The desktop main menu
 * */
tp.DeskMainMenu = class extends tp.ItemBar {

    /**
    Constructor <br />
    Example markup:
    <pre>
 
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.DeskMainMenu';
        this.fDefaultCssClasses.push(tp.Classes.DeskMainMenu);
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        this.ResponsiveMode = tp.ItemBarRenderMode.NextPrev;
        super.OnInitializationCompleted();
    }
    /** Creates child controls of this instance. Called in construction sequence.
    * @private
    * */
    async CreateControls() {
        super.CreateControls();

        let Url = tp.Urls.DeskGetMainMenu;

        let Args = await tp.Ajax.GetAsync(Url);
        let Packet = Args.Packet;

        let BarItemList = [];
        let BarItem;
        if (tp.IsArray(Packet)) {
            Packet.forEach((cmd) => {
                BarItem = new tp.DeskMainMenuBarItem(new tp.Command(cmd), this);
                BarItemList.push(BarItem);
            });
        }

        this.AddRange(BarItemList);

    }

    /** Event handler for the menu items, other than the menu-bar items.
     * When a menu-bar item is clicked presents a drop-down list with menu items.
     * Whe any of the items in that list is clicked, calls this handler.
     * @param {tp.EventArgs} Args
     */
    async MenuItemClicked(Args) {
        /** @type {tp.tpElement} */
        let E = Args.Sender;

        if (E instanceof tp.tpElement) {
            let Cmd = E.Cmd;
            await tp.Desk.ExecuteCommand(Cmd); 
        }
    }

};


//#endregion

//#region tp.DeskMainMenuBarItem

/** Represents a menu item of the desktop main menu, other than the menu-bar items. 
 * Items like this are presented to the user in a drop-down list when any of the menu-bar items is clicked.
 */
tp.DeskMainMenuBarItem = class {

    /** Constructor
     * @param {tp.Command|object} Cmd A {@link tp.Command} associated to the menu item.
     * @param {tp.DeskMainMenu} DeskMainMenu The desktop main menu.
     */
    constructor(Cmd, DeskMainMenu) {

        this.Cmd = Cmd;
        let Text = Cmd.Title ? Cmd.Title : Cmd.Name;

        let BarItemHtmlText = `
<div class="${tp.Classes.BarItem}">
    <div class="${tp.Classes.Text}">${Text}</div>
    <div class="${tp.Classes.List}"> 
    </div>
</div>
`;
        this.Handle = tp.HtmlToElement(BarItemHtmlText);
        let List = tp.ChildHTMLElements(this.Handle);
        this.elText = List[0];
        this.elList = List[1];

        tp.DropDownHandler(this.elText, this.elList, tp.Classes.Visible);

        if (tp.IsArray(Cmd.Items) && Cmd.Items.length > 0) {
            Cmd.Items.forEach((childCmd) => {
                let el = tp.Doc.createElement('div');
                let Text = childCmd.Title ? childCmd.Title : childCmd.Name;

                let E = new tp.tpElement(el, { Parent: this.elList, Text: Text });
                E.Cmd = childCmd;
                E.On(tp.Events.Click, (Args) => {
                    DeskMainMenu.MenuItemClicked(Args);
                });
            });
        }
    }

    /** The HTMLElement this instance is created upon.
     * @type {HTMLElement}
     * */
    Handle = null;
    /** The HTMLElement holding the text of this item
     * @type {HTMLElement}
     */
    elText = null;
    /** The HTMLElement used as the drop-down container for the child items of this menu item.
     * @type {HTMLElement}
     */
    elList = null;
    /** The {@link tp.Command} this instance is associated to.
     * @type {tp.Command}
     */
    Cmd = null;

    /** Gets or sets the text of the menu item
     * @type {string}
     */
    get Text() {
        return elText.innerHTML;
    }
    set Text(v) {
        elText.innerHTML = v;
    }


};
//#endregion

//#region tp.DeskCommandExecutor

/** Base command executor class. <br />
 *  A command executor must provide two functions: CanExecuteCommand(Cmd) and async ExecuteCommand(Cmd).
 * */
tp.DeskCommandExecutor = class {

    /** Constructor */
    constructor() {
    }

    /**
     * Returns true if this executor can handle the specified command.
     * @param {tp.Command} Cmd The command to check.
     * @returns {boolean} Returns true if this executor can handle the specified command.
     */
    CanExecuteCommand(Cmd) {
        return this.ValidCommands.find(item => { return tp.IsSameText(item, Cmd.Name); });
    }
    /**
     * Executes a specified command. The CanExecuteCommand() is called just before this call.
     * @param {tp.Command} Cmd The command to execute.
     * @returns {Promise} Returns whatever the specified command dictates to return. For a Ui command returns the Packet from the server.
     */
    async ExecuteCommand(Cmd) {
        return null;
    }
};
tp.DeskCommandExecutor.ValidCommands = [];

//#endregion

//#region tp.DeskViewPager

/** A composite control. Handles the tab-control-like Ui of creating, displaying and destroying views  */
tp.DeskViewPager = class extends tp.tpElement {

    /** Constructor
     * @param {string|HTMLElement} ElementOrSelector
     * @param {object|tp.CreateParams} CreateParams
     */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.DeskViewPager';
        this.fDefaultCssClasses = [tp.Classes.DeskViewPager];
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.CreateControls();
    }
    /** Creates child controls of this instance. Called in construction sequence.
    * @private
    * */
    CreateControls() { 
        this.TabBar = new tp.ItemBar(this.AddDivElement());
        this.PageListContainer = new tp.tpElement(this.AddDivElement());
        this.PageListContainer.AddClass(tp.Classes.List);
 
        this.TabBar.On('SelectedIndexChanged', this.TabBar_SelectedIndexChanged, this);
    }

    /** Creates and adds a view and a tab-item, based on a packet from the server.
     * @param {object} Packet
     */
    async AddView(Packet) {
        /** @type {tp.View} */
        let View = null;

        // tab
        let elTab = tp.Doc.createElement('div');
        let TabItem = new tp.tpElement(elTab);
        TabItem.Text = !tp.IsBlankString(Packet.ViewTitle) ? Packet.ViewTitle: Packet.ViewName;

        // view
        let elView = tp.HtmlToElement(Packet.HtmlText);
        let DataSetup = tp.GetDataSetupObject(elView);

        if (tp.IsArray(DataSetup.CSS)) {
            await tp.StaticFiles.LoadCssFiles(DataSetup.CSS);
        }

        if (tp.IsArray(DataSetup.JS)) {
            await tp.StaticFiles.LoadJavascriptFiles(DataSetup.JS);
        }

        if (tp.IsString(DataSetup['ClassType'])) {
            DataSetup.ClassType = eval(DataSetup.ClassType);
        }

        if (!tp.IsFunction(DataSetup.ClassType))
            tp.Throw('No class to create a view');

        let CreateParams = {
            ViewName: Packet.ViewName,
            Packet: Packet
        };

        View = new DataSetup.ClassType(elView, CreateParams);
        View.On('Disposing', this.AnyView_Disposing, this);

        TabItem.DeskView = View;
        View.TabItem = TabItem;

        this.PageListContainer.AddControl(View);
        this.TabBar.AddItem(TabItem);

        this.TabBar.SelectedItem = TabItem;

        if (View instanceof tp.View)
            View.OnAfterConstruction();

        return View;
    }
    /** Shows a specified view. 
     * @param {tp.tpElement|number} ViewOrIndex The view instance or the view index
     */
    ShowView(ViewOrIndex) {
        let TabList = this.TabBar.GetItemElementList();
        let ViewList = this.GetViewList();

        let View = tp.IsNumber(ViewOrIndex) ? ViewList[ViewOrIndex] : ViewOrIndex;

        for (let i = 0, ln = TabList.length; i < ln; i++) {
            tp.RemoveClass(TabList[i], tp.Classes.Selected);
            ViewList[i].Handle.style.display = 'none';
        }

        tp.AddClass(View.TabItem.Handle, tp.Classes.Selected);
        View.Handle.style.display = '';
    }
    /** Event handler. Called when any view is about to be destroyed.
     * @param {tp.EventArgs} Args
     */
    AnyView_Disposing(Args) {
        let View = Args.Sender;

        let Index = this.TabBar.IndexOfItem(View.TabItem.Handle);
        this.TabBar.RemoveItemAt(Index);

        if (tp.IsArray(View.CreateParams.CSS)) {
            tp.StaticFiles.UnLoadCssFiles(View.CreateParams.CSS);
        }

        if (tp.IsArray(View.CreateParams.JS)) {
            tp.StaticFiles.UnLoadJavascriptFiles(View.CreateParams.JS);
        }

        // we have to wait for the page to be complete its Dispose() operation. 
        setTimeout(() => {
            let List = this.GetViewList();

            let NewIndex = Index - 1;
            let NewIndex2 = Index + 1;
            if (NewIndex >= 0 && NewIndex <= List.length - 1) {
                this.TabBar.SelectedIndex = NewIndex;
            }
            else if (NewIndex2 >= 0 && NewIndex2 <= List.length - 1) {
                this.TabBar.SelectedIndex = NewIndex;
            }
            else {
                this.TabBar.SelectedIndex = List.length > 0 ? List.length - 1 : -1;
            }

        }, 0);
    }

    /** Returns an array with {@link tp.View} items contained by this pager.
     * @returns {tp.View[]} Returns an array with {@link tp.View} items contained by this pager.
     * */
    GetViewList() {
        let ResultList = this.PageListContainer.GetControlList();  
        return ResultList;
    }
    /** Finds and returns a contained view by a specified name, if any, else null/undefined.
     * @param {string} ViewName The name of the view
     */
    FindViewByName(ViewName) {
        let List = this.GetViewList();
        let Result = List.find((view) => {
            return tp.IsSameText(view.ViewName, ViewName);
        });
        return Result;
    }
    /** Returns true if a view exists under a specified name.
     * @param {string} ViewName The name of the view
     */
    ViewExists(ViewName) {
        return tp.IsValid(this.FindViewByName(ViewName));
    }


    /** Event handler.
     * @param {tp.IndexChangeEventArgs} Args
     */
    TabBar_SelectedIndexChanged(Args) {
        this.ShowView(Args.CurrentIndex); 
    }

};

/**
 * @type {tp.ItemBar}
 * */
tp.DeskViewPager.prototype.TabBar = null;
/**
 * @type {tp.tpElement}
 * */
tp.DeskViewPager.prototype.PageListContainer = null;


//#endregion

//---------------------------------------------------------------------------------------
// views
//---------------------------------------------------------------------------------------


//#region tp.DeskView

/** A {@link tp.View} view for the desktop.  */
tp.DeskView = class extends tp.View {

    /** Constructor
     * @param {string|HTMLElement} ElementOrSelector
     * @param {object|tp.CreateParams} CreateParams
     */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 

    /** Closes and disposes the view
     * */
    CloseView() {
        this.Dispose();
    }
 

};

//#endregion

//#region tp.DeskDataView

/** A {@link tp.DataView} view for the desktop */
tp.DeskDataView = class extends tp.DataView {

    /** Constructor
     * @param {string|HTMLElement} ElementOrSelector
     * @param {object|tp.CreateParams} CreateParams
     */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /**
    Closes the view and removes the view from the DOM.
    @protected
    */
    CloseView() {
        this.Dispose();
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
    Gets or sets the data mode. One of the {@link tp.DataMode} constants.
    @type {tp.DataViewMode}
    */
    get ViewMode() {
        return this.fViewMode;
    }
    /**
    Gets or sets the data mode. One of the {@link tp.DataMode} constants.
    @type {tp.DataViewMode}
    */
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

        this.fName = tp.NextName('DataView');
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

        this.DataType = this.CreateParams.Packet.DataType

        this.CreateToolBar();
        this.CreatePanelList();
        this.CreateListGrid();

        this.ListSelect();
    }

    /**
    Sets the visible panel index in the panel list.
    @protected
    @param {number} PanelIndex The panel index
    */
    SetVisiblePanel(PanelIndex) {
        if (this.PanelList) {
            this.PanelList.SelectedIndex = PanelIndex;
        }
    }
    /**
     * Sets the visible panel of the main pager (a PanelList) by its 'PanelMode'.
     * NOTE: Each panel of the main pager (a PanelList) may have a data-setup with a 'PanelMode' string property indicating the 'mode' of the panel.
     * @param {string} PanelMode The panel mode to check for.
     */
    SetVisiblePanelByPanelMode(PanelMode) {
        let elPanel = this.FindPanelByPanelMode(PanelMode);
        let Index = -1;
        if (elPanel) {
            let Panels = this.PanelList.GetPanels();
            Index = Panels.indexOf(elPanel);
        }

        if (Index >= 0) {
            this.SetVisiblePanel(Index);
        }
    }
    /**
     * Each panel of the main pager (a PanelList) MUST have a data-setup with a 'PanelMode' string property indicating the 'mode' of the panel.
     * This function returns a panel found having a specified PanelMode, or null if not found.
     * @param {string} PanelMode The panel mode to check for.
     * @returns {HTMLElement} Returns a panel found having a specified PanelMode, or null if not found.
     */
    FindPanelByPanelMode(PanelMode) {
        if (tp.IsValid(this.PanelList)) {
            let Panels = this.PanelList.GetPanels();

            let i, ln, elPanel, Setup;

            for (i = 0, ln = Panels.length; i < ln; i++) {
                elPanel = Panels[i];
                Setup = tp.GetDataSetupObject(elPanel);
                if (tp.IsValid(Setup)) {
                    if (PanelMode === Setup.PanelMode) {
                        return elPanel;
                    }
                }
            }
        }

        return null;
    }

    /** Returns an array with the panels of the panel list
     * @returns {HTMLElement[]}
     * */
    GetPanelListElements() { return tp.IsValid(this.PanelList) ? this.PanelList.GetPanels() : tp.ChildHTMLElements(this.GetViewPanelListElement()); }
    /** Returns a DOM Element contained by this view.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewToolBarElement() { return tp.Select(this.Handle, '.ToolBar'); }
    /** Returns a DOM Element contained by this view. Returns the main panel-list which in turn contains the three part panels: Brower, Edit and Filters.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewPanelListElement() { return tp.Select(this.Handle, '.PanelList'); }
    /** Returns a DOM Element contained by this view. Returns the List (browser) Panel, the container of the List (browser) grid, which displays the results of the various SELECTs of the broker.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewListPanelElement() { return this.FindPanelByPanelMode('List'); }
    /** Returns a DOM Element contained by this view. Returns the Edit Panel, which is the container for all edit controls bound to broker datasources.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewEditPanelElement() { return this.FindPanelByPanelMode('Edit'); }
    /** Returns a DOM Element contained by this view. Returns the element upon to create the List (browser) grid.
     * @returns {HTMLElement} Returns a DOM Element contained by this view.
     *  */
    GetViewListGridElement() { return tp.Select(this.GetViewListPanelElement(), '.Grid'); }


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
            let el = this.GetViewToolBarElement();
            this.ToolBar = new tp.ToolBar(el);
            this.ToolBar.On('ButtonClick', this.AnyClick, this);
        }

        
    }
    /** Creates the panel-list of the view, the one with the 3 panels: List, Edit and Filters panels.
     @protected
     */
    CreatePanelList() {
        if (tp.IsEmpty(this.PanelList)) {
            let el = this.GetViewPanelListElement();
            this.PanelList = new tp.PanelList(el);
        }
    }
    /**
    Creates the List (browser) grid. <br />
    NOTE: For the List (browser) grid to be created automatically by this method, a div marked with the Grid class is required in the List panel.
    @protected
    */
    CreateListGrid() {
        if (tp.IsEmpty(this.gridList)) {
            let el = this.GetViewListGridElement();
            this.gridList = new tp.Grid(el);
        }

        if (tp.IsEmpty(this.gridList)) {
            let o = this.FindControlByCssClass(tp.Classes.Grid, this.GetViewListPanelElement());
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

/*
        if (this.gridList && !this.tblList) {
            this.tblList = new tp.DataTable();
            this.tblList.Assign(this.CreateParams.Packet.ListTable);

            this.dsList = new tp.DataSource(this.tblList);
            this.gridList.DataSource = this.dsList;
        }
 */
    }
 
    /** Creates and binds the controls of the edit part, if not already created.
     * */
    CreateEditControls() {

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

                let ControlList = this.ToolBar.GetControls(),
                    c,          // tp.tpElement,
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

    // EDW: 1. Να οριστούν τα edit controls στο ViewDef (DataStore.Views -> RegisterView_SysData_Table() )
    // 2. Να φτιαχτεί κλάση SysDataTypeHandler που θα χειρίζεται το Edit part κάθε ξεχωριστού DataType
    // 3. Να γραφτούν οι Edit(), Insert() κλπ με 


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
        //this.CreateEditControls();
        //this.SetVisiblePanelByPanelMode('Edit');
    }
    async Edit() {
        this.CreateEditControls();

        let Id = this.GetListSelectedId();

        if (tp.IsEmpty(Id)) {
            tp.ErrorNote('No selected row');
        } else {
            //this.DoEditBefore();
            //this.CheckCanEdit();
            let Url = tp.Urls.SysDataSelectById;
            let Data = {
                Id: Id
            }

            let Args = await tp.Ajax.GetAsync(Url, Data);
 
            this.tblData = new tp.DataTable();
            this.tblData.Assign(Args.Packet);

            this.ViewMode = tp.DataViewMode.Edit;
        }
    }
    async Delete() {

    }
    async Commit() {

    }


    /* Event triggers */
    /**
    Event trigger
    */
    OnViewModeChanged() {
        switch (this.ViewMode) {
            case tp.DataViewMode.List:
            case tp.DataViewMode.Cancel:
                this.SetVisiblePanelByPanelMode('List');
                break;

            case tp.DataViewMode.Insert:
            case tp.DataViewMode.Edit:
                this.SetVisiblePanelByPanelMode('Edit');
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
/** The panel list. Contains the List and Edit panels
 @protected
 @type {tp.PanelList}
 */
tp.DeskSysDataView.prototype.PanelList = null;
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


//#endregion




 

//#endregion

//#region tp.SysDataViewEditTable

/** Represents a page. Edit/Insert view of the Table DataType. */
tp.SysDataViewEditTable = class extends tp.DeskView {
    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [Params=null] Optional. A javascript object with initialization parameters.
     */
    constructor(elPage, CreateParams = null) {
        super(elPage, CreateParams);
    }
};

//#endregion




