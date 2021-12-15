
tp.Urls.DeskGetMainMenu = '/DeskGetMainMenu';

tp.Classes.Desk = 'tp-Desk';
tp.Classes.DeskMainMenu = 'tp-Desk-MainMenu';
tp.Classes.DeskMainMenuBarItem = 'tp-Desk-MainMenuBarItem';

//#region tp.Command

/** A command class. This is here mostly for reference only. */
tp.Command = class {

    /** constructor */
    constructor() {
        this.Params = {};
        this.Items = [];
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

//#region tp.DesktopAjaxRequest

/** Represents an ajax request */
tp.DesktopAjaxRequest = class {

    /**
     * Constructor
     * @param {string} OperationName The name of the request operation.
     * @param {object} Params Optional. A user defined key-pair object with parameters.
     */
    constructor(OperationName, Params = null) {
        this.OperationName = OperationName;
        this.Params = Params || {};
    }
    /** The name of the request operation.
     * @type {string}
     */
    OperationName = '';
    /** A user defined key-pair object with parameters.
     * @type {object}
     */
    Params = {};
};

//#endregion

//#region tp.DeskInfo

/**
 * Gets or sets a key-value information object to a DOM element. <br />
 * If the second parameter is not passed then it just gets and returns the information object.
 * @param {HTMLElement} el The DOM element to get from or set the info to.
 * @param {object} Info A key-value information object
 */
tp.DeskInfo = function (el, Info = null) {
    if (tp.IsValid(Info)) {
        el.__DeskInfo = Info;
    }
    else {
        if ('__DeskInfo' in el)
            Info = el.__DeskInfo;
    }

    return Info;
};

//#endregion

//#region tp.Desktop


tp.DeskOptions = {

    /** Main menu element
     * @type {HTMLElement}
     */
    elMainMenu: null,
    /** Element
     * @type {HTMLElement}
     */
    elMainPager: null,
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

        this.elMainMenu = tp('.main-menu');
        this.elContainer = tp('.pager-container.main-pager-container');

        //this.elTabBarContainer = tp.Select(this.elContainer, '.tab-container');
        //new app.TabBarHandler(this.elTabBarContainer);

        this.elTabBar = tp.Select(this.elContainer, '.tab-bar');
        this.elViewList = tp.Select(this.elContainer, '.view-list');

        this.CommandExecutors = [];

        //this.CommandExecutors.push(new app.MainMenuCommandExecutor());

        if (this.elMainMenu)
            this.elMainMenu.addEventListener('click', this);

        if (this.elTabBar)
            this.elTabBar.addEventListener('click', this);

        tp.Viewport.AddListener(this.OnScreenSizeChanged, this);
    }

    Options = {};
    /** The main menu container. Not contained by the elContainer of this instance.
     * @type {HTMLDivElement}
     */
    elMainMenu = null;
    /** The main container. 
     * Contains everything except header and footer. 
     * Header contains the  main-menu and main tool-bar.
     * @type {HTMLDivElement} 
     */
    elContainer = null;
    /** Contains the tab-bar and the left and right buttons.
     * @type {HTMLDivElement}
     */
    elTabBarContainer = null;
    /** The tab-bar. Contains the tabs (captions) of the pages (views)
     * @type {HTMLDivElement}
     */
    elTabBar = null;
    /** Contains the pages (views)
     * @type {HTMLDivElement}
     */
    elViewList = null;
    /** A list of registered command executors.
     * @type {array}
     */
    CommandExecutors = [];


    /**
    Notification sent by tp.Viewport when the screen (viewport) size changes. 
    This method is called only if this.IsScreenResizeListener is true.
    @param {boolean} ScreenModeFlag - Is true when the screen mode (XSmall, Small, Medium, Large) is changed as well.
    */
    OnScreenSizeChanged(ScreenModeFlag) {
    }
    /**
     * Event handler. Handles all events
     * @param {Event} e The event.
     */
    async handleEvent(e) {
        let Cmd, A, el;
        switch (e.type) {
            case 'click':
                if (tp.ContainsEventTarget(this.elTabBar, e.target)) {
                    A = this.GetTabList();
                    el = A.find(item => { return tp.ContainsEventTarget(item, e.target); });
                    if (tp.IsHTMLElement(el)) {
                        this.TabClicked(el);
                    }
                }
                else if (tp.HasClass(e.target, 'main-menu-command')) {
                    Cmd = new tp.Command(tp.GetDataSetupObject(e.target));
                    await this.ExecuteCommand(Cmd);
                }
                break;
        }
    }

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
    /**
     * Returns an element (the tab) if a page entry exists under a specified name.
     * @param {string} ViewName The page name
     * @returns {HTMLElement} Returns an elememt (the tab) if a page entry exists under a specified name.
     */
    FindTab(ViewName) {
        let TabList = this.GetTabList();
        let Tab = TabList.find(elTab => {
            let Info = tp.DeskInfo(elTab);
            return tp.IsValid(Info) ? ViewName === Info.Name : false;
        });
        return Tab;
    }
    /**
     * Returns true if a page entry exists under a specified name.
     * @param {string} ViewName The page name
     * @returns {boolean} Returns true if a page entry exists under a specified name.
     */
    TabExists(ViewName) {
        let Tab = this.FindTab(ViewName);
        return tp.IsElement(Tab);
    }
    /** Returns the list of tabs.
     * @returns {HTMLElement[]} Returns the list of tabs.
     * */
    GetTabList() {
        let Result = tp.ChildHTMLElements(this.elTabBar)
        return Result;
    }
    /** Returns the list of pages (views).
     * @returns {HTMLElement[]} Returns the list of pages (views).
     * */
    GetViewList() {
        let Result = tp.ChildHTMLElements(this.elViewList)
        return Result;
    }



    /**
     * Executes an ajax request to the server and returns the Packet as it comes from server.
     * @param {tp.DesktopAjaxRequest} Request The request object.
     * @returns {object} Returns the Packet from the server.
     */
    async AjaxExecute(Request) {
        let Result = null;
        let Url = '/AjaxExecute'

        let Args = await tp.Ajax.PostModelAsync(Url, Request);
        if (Args.ResponseData.IsSuccess === true) {
            let Packet = Args.ResponseData.Packet;
            Result = Packet;
        }
        else {
            tp.DisplayAjaxErrors(Args);
        }

        return Result;
    }
    /**
     * Creates a tab element and a page (view) element and adds both to the DOM.
     * @param {object} Packet The packet as it comes from server
     */
    async CreateViewElement(Packet) {
        let TabHtmlText = `
       <div class="tab-item">
           <div class="tp-Text">${Packet.ViewName}</div>
       </div>
`;

        let elView = tp.ContentWindow.GetContentElement(Packet.HtmlText.trim());
        let elTab = tp.ContentWindow.GetContentElement(TabHtmlText.trim());

        let DeskInfo = {};
        DeskInfo.Name = Packet.ViewName;
        DeskInfo.elTab = elTab;
        DeskInfo.elPage = elView;

        tp.DeskInfo(elTab, DeskInfo);
        tp.DeskInfo(elView, DeskInfo);

        this.elViewList.appendChild(elView);
        this.elTabBar.appendChild(elTab);

        let CreateParams = {
            Name: Packet.ViewName,
            Packet: Packet,
            elTab: elTab
        };

        await this.CreateViewObject(elView, CreateParams);

        this.TabClicked(elTab);
    }
    /**
     * Creates a specified view object on an element that is already added to DOM and returns that view instance. <br />
     * The passed-in element provides a data-setup attribute specifying a javascript file array, a css file array and a view class.  <br />
     * This function dynamically loads the specified files, if not already loaded, and then creates the instance of the view object. <br />
     * @param {HTMLElement} elView The element to create the view object upon.
     * @param {object} Params A key-value object with initialization information.
     * @returns {app.DeskView} Returns the view javascript object instance.
     */
    async CreateViewObject(elView, Params) {
        let Result = null;
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

        Result = new DataSetup.ClassType(elView, Params);
        Result.OnAfterConstruction();

        return Result;
    }
    /**
     * Handles the click on a tab.
     * @param {HTMLElement} elTab The tab
     */
    TabClicked(elTab) {
        let i, ln;

        let TabList = this.GetTabList();
        let PageList = this.GetViewList();

        let Info = tp.DeskInfo(elTab);
        let elPage = Info.elPage;

        for (i = 0, ln = TabList.length; i < ln; i++) {
            tp.RemoveClass(TabList[i], tp.Classes.Selected);
            PageList[i].style.display = 'none';
        }

        tp.AddClass(elTab, tp.Classes.Selected);
        elPage.style.display = '';
    }
};
//#endregion



//#region tp.DesktopCommandExecutor

/** Base command executor class. <br />
 *  A command executor must provide two functions: CanExecuteCommand(Cmd) and async ExecuteCommand(Cmd).
 * */
tp.DesktopCommandExecutor = class {

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
tp.DesktopCommandExecutor.ValidCommands = [];

//#endregion


//---------------------------------------------------------------------------------------
// views
//---------------------------------------------------------------------------------------


//#region tp.DeskView

/** Represents a desk view. Used as base view class. */
tp.DeskView = class extends tp.View {

    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Notification 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        this.IsScreenResizeListener = true;
    }

    Close() {
        this.Dispose();
    }

    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    */
    Dispose() {
        if (this.IsDisposed === false && tp.IsElement(this.Handle)) {

            if (tp.IsElement(this.CreateParams.elTab)) {
                tp.Remove(this.CreateParams.elTab);
                this.CreateParams.elTab = null;
            }

            if (tp.IsArray(this.CreateParams.CSS)) {
                tp.StaticFiles.UnLoadCssFiles(this.CreateParams.CSS);
            }

            if (tp.IsArray(this.CreateParams.JS)) {
                tp.StaticFiles.UnLoadJavascriptFiles(this.CreateParams.JS);
            }

            super.Dispose();
        }
    }


    /**
     * Executes a command that returns a Packet from server for creating a modal dialog.
     * @param {tp.Command} Cmd The command to execute.
     * @returns {object} Returns the Packet from the server.
     */
    async AjaxExecuteDialog(Cmd) {

        let Params = {
            ViewName: Cmd.Name
        };
        Params = tp.MergeQuick(Params, Cmd.Params);
        let Request = new tp.DesktopAjaxRequest("GetHtmlView", Params);
        let Packet = await tp.Desk.AjaxExecute(Request);

        return Packet;
    }

};

//#endregion

//#region tp.DeskDataView

tp.DeskDataView = class extends tp.DataView {
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Notification 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        this.IsScreenResizeListener = true;
    }

    /**
    Closes the view and removes the view from the DOM.
    @protected
    */
    CloseView() {
        this.Dispose();
    }

    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    */
    Dispose() {
        if (this.IsDisposed === false && tp.IsElement(this.Handle)) {

            if (tp.IsElement(this.CreateParams.elTab)) {
                tp.Remove(this.CreateParams.elTab);
                this.CreateParams.elTab = null;
            }

            if (tp.IsArray(this.CreateParams.CSS)) {
                tp.StaticFiles.UnLoadCssFiles(this.CreateParams.CSS);
            }

            if (tp.IsArray(this.CreateParams.JS)) {
                tp.StaticFiles.UnLoadJavascriptFiles(this.CreateParams.JS);
            }

            super.Dispose();
        }
    }


};

//#endregion

//#region tp.SysDataViewList

/** Represents a view. Displays a list of items of a certain DataType. */
tp.SysDataViewList = class extends tp.DeskView {
    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [Params=null] Optional. A javascript object with initialization parameters.
     */
    constructor(elPage, CreateParams = null) {
        super(elPage, CreateParams);
    }

    /**
     * @type {tp.ToolBar}
     */
    ToolBar = null;
    /**
     * @type {tp.Grid}
     */
    Grid = null;
    /**
     * @type {tp.DataTable}
     */
    Table = null;

    /** Creates page controls */
    CreateControls() {
        super.CreateControls();
        let el = tp.Select(this.Handle, '.tp-ToolBar');
        this.ToolBar = new tp.ToolBar(el);

        this.ToolBar.On('ButtonClick', this.AnyToolBarButtonClick, this);

        this.Table = new tp.DataTable();
        this.Table.Assign(this.CreateParams.Packet.Table);

        el = tp.Select(this.Handle, '.tp-Grid');

        let CP = {
            DataSource: this.Table,
            ReadOnly: true,
            ToolBarVisible: false,
            GroupsVisible: false,
            FilterVisible: false,
            FooterVisible: false,
            Columns: [
                { Name: 'DataName' },
                { Name: 'TitleKey' },
                { Name: 'Owner' }
            ]
        };

        this.Grid = new tp.Grid(el, CP);
    }

    async ShowModal(IsInsert) {

        // get the packet
        let DataType = this.Setup.DataType;
        let Cmd = new tp.Command();
        Cmd.Name = IsInsert === true ? `Ui.SysData.Insert.${DataType}` : `Ui.SysData.Edit.${DataType}`;
        if (IsInsert !== true) {
            let Row = this.Grid.DataSource.Current;
            Cmd.Params.Id = Row.GetByName('Id');
        }

        let Packet = await this.AjaxExecuteDialog(Cmd);

        // create the page element
        let elPage = tp.ContentWindow.GetContentElement(Packet.HtmlText.trim());

        let DeskInfo = {};
        DeskInfo.Name = Packet.ViewName;
        DeskInfo.elPage = elPage;
        tp.DeskInfo(elPage, DeskInfo);

        // create the page instance
        let CreateParams = {
            Name: Packet.ViewName,
            Packet: Packet,
        };

        let Page = await tp.Desk.CreateViewObject(elPage, CreateParams);

        // display the dialog
        let WindowArgs = await tp.ContentWindow.ShowModalAsync(Packet.ViewName, elPage);

        let DialogResult = WindowArgs.Window.DialogResult;
        let S = tp.EnumNameOf(tp.DialogResult, DialogResult);
        tp.InfoNote('DialogResult = ' + S);

        return DialogResult;
    }
    async AnyToolBarButtonClick(Args) {
        let Command = Args.Command;
        switch (Command) {
            case 'Close':
                this.Close();
                break;
            case 'Insert':
                await this.ShowModal(true);
                break;
            default:
                tp.InfoNote('Command: ' + Command);
                break;
        }
    }
};

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




