/** A command class. This is here mostly for reference only. */
app.Command = class {

    /** constructor */
    constructor(Source) {
        this.Params = {};

        if (tp.IsValid(Source))
            tp.MergeQuick(this, Source);
    }

    /** A name unique among all commands. */
    Name = '';
    /** The command type. What a command does when it is called. */
    Type = 'Ui';
    /** True when this is a single instance Ui command. */
    IsSingleInstance = false;
    /** User defined parameters */
    Params = {};

    IsUiCommand() {
        return this.Type === 'Ui' || tp.StartsWith(this.Name, 'Ui.', true);
    }
};

/** Represents an ajax request */
app.AjaxRequest = class {

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

/** A single instance class. It handles the desk. */
app.Desk = class {
    constructor() {
        if (app.Desk.Instance)
            tp.Throw('Desk is already created');

        app.Desk.Instance = this;

        this.elMainMenu = tp('.main-menu');
        this.elContainer = tp('.pager-container.main-pager-container');

        this.elTabBarContainer = tp.Select(this.elContainer, '.tab-container');
        new app.TabBarHandler(this.elTabBarContainer);

        this.elTabs = tp.Select(this.elContainer, '.tab-bar'); 
        this.elViews = tp.Select(this.elContainer, '.view-list');
 
        this.CommandExecutors = [];

        this.CommandExecutors.push(new app.MainMenuCommandExecutor()); 

        if (this.elMainMenu)
            this.elMainMenu.addEventListener('click', this);

        if (this.elTabBarContainer)
            this.elTabBarContainer.addEventListener('click', this);

        tp.Viewport.AddListener(this.OnScreenSizeChanged, this);
    }

    /** The single instance of this class
     * @type {app.Desk}
     */
    static Instance = null;
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
    elTabs = null;           
    /** Contains the pages (views)
     * @type {HTMLDivElement}
     */
    elViews = null;          
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
        let Cmd;
        switch (e.type) {
            case 'click':
                if (tp.ContainsEventTarget(this.elTabs, e.target)) {
                    // tp.ChildHTMLElements(this.Handle)
                }
                else if (tp.HasClass(e.target, 'main-menu-command')) {
                    Cmd = new app.Command(tp.GetDataSetupObject(e.target));
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
     * @param {app.Command} Cmd
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
            let Info = app.DeskInfo(elTab);
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
        let Result = tp.ChildHTMLElements(this.elTabs)
        return Result;
    }
    /** Returns the list of pages (views).
     * @returns {HTMLElement[]} Returns the list of pages (views).
     * */
    GetViewList() {
        let Result = tp.ChildHTMLElements(this.elViews)
        return Result;
    }



    /**
     * Executes an ajax request to the server and returns the Packet as it comes from server.
     * @param {app.AjaxRequest} Request The request object.
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
            app.DisplayAjaxErrors(Args);           
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
      
        let elView = app.GetContentElement(Packet.HtmlText.trim());
        let elTab = app.GetContentElement(TabHtmlText.trim());

        let DeskInfo = {};
        DeskInfo.Name = Packet.ViewName;
        DeskInfo.elTab = elTab;
        DeskInfo.elPage = elView;

        app.DeskInfo(elTab, DeskInfo);
        app.DeskInfo(elView, DeskInfo);

        this.elViews.appendChild(elView);
        this.elTabs.appendChild(elTab);

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
     * @returns {app.Desk.View} Returns the view javascript object instance.
     */
    async CreateViewObject(elView, Params) {
        let Result = null;
        let DataSetup = tp.GetDataSetupObject(elView);

        if (tp.IsArray(DataSetup.CSS)) {
            await app.LoadCssFiles(DataSetup.CSS);
        }

        if (tp.IsArray(DataSetup.JS)) {
            await app.LoadJavascriptFiles(DataSetup.JS);
        }
 
        if (tp.IsString(DataSetup['ClassType'])) {
            DataSetup.ClassType = eval(DataSetup.ClassType);
        }

        if (!tp.IsFunction(DataSetup.ClassType))
            tp.Throw('No class to create a view');

        Result = new DataSetup.ClassType(elView, Params);
 

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

        let Info = app.DeskInfo(elTab);
        let elPage = Info.elPage;

        for (i = 0, ln = TabList.length; i < ln; i++) {
            tp.RemoveClass(TabList[i], tp.Classes.Selected);
            PageList[i].style.display = 'none';
        }

        tp.AddClass(elTab, tp.Classes.Selected);
        elPage.style.display = '';
    }
};

/** A command executor class. <br />
 *  A command executor must provide two functions: CanExecuteCommand(Cmd) and async ExecuteCommand(Cmd).
 * */
app.MainMenuCommandExecutor = class {
    constructor() {
    }

    /** A list of command names this executor can handle.
     * @type {string[]}
     */
    ValidCommands = [
        'Ui.SysData.Tables',
        'Ui.Traders'
    ];

    /**
     * Returns true if this executor can handle the specified command.
     * @param {app.Command} Cmd The command to check.
     * @returns {boolean} Returns true if this executor can handle the specified command.
     */
    CanExecuteCommand(Cmd) {
        return this.ValidCommands.find(item => { return tp.IsSameText(item, Cmd.Name); });  
    }
    /**
     * Executes a specified command. The CanExecuteCommand() is called just before this call.
     * @param {app.Command} Cmd The command to execute.
     * @returns {Promise} Returns whatever the specified command dictates to return. For a Ui command returns the Packet from the server.
     */
    async ExecuteCommand(Cmd) {
        let Result = null;

        if (Cmd.IsUiCommand()) {
            if ((Cmd.IsSingleInstance && !app.Desk.Instance.TabExists(Cmd.Name)) || !Cmd.IsSingleInstance) {
                let Params = {
                    Type: Cmd.Type,
                    IsSingleInstance: Cmd.IsSingleInstance
                };
                Params = tp.MergeQuick(Params, Cmd.Params);
                let Request = new app.AjaxRequest(Cmd.Name, Params);
                let Packet = await app.Desk.Instance.AjaxExecute(Request);
                if (tp.IsValid(Packet))
                    Result = await app.Desk.Instance.CreateViewElement(Packet);
            }                
        }
        else {
            tp.Throw('Command not found'); 
        }

        return Result;
    }
};


/** Represents a desk view. Used as base view class. */
app.Desk.View = class extends tp.View {

    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    */
    Dispose() {
        if (this.IsDisposed === false && tp.IsElement(this.Handle)) {

            tp.Broadcaster.Remove(this);
 
            if (tp.IsElement(this.CreateParams.elTab)) {
                tp.Remove(this.CreateParams.elTab);
                this.CreateParams.elTab = null;
            }

            if (tp.IsValid(this.Handle)) {
                try {
                    let el = this.Handle;
                    tp.SetObject(this.Handle, null);
                    this.Handle = null;
                    if (tp.IsElement(el.parentNode)) {
                        el.parentNode.removeChild(el);
                    }
                } catch {
                    //
                }
            }

            if (tp.IsArray(this.CreateParams.CSS)) {
                app.UnLoadCssFiles(this.CreateParams.CSS);
            }

            if (tp.IsArray(this.CreateParams.JS)) {
                app.UnLoadJavascriptFiles(this.CreateParams.JS);
            } 

            super.Dispose();
        }
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
     * Executes a command that returns a Packet from server for creating a modal dialog.
     * @param {app.Command} Cmd The command to execute.
     * @returns {object} Returns the Packet from the server.
     */
    async AjaxExecuteDialog(Cmd) {

        let Params = {
            ViewName: Cmd.Name
        };
        Params = tp.MergeQuick(Params, Cmd.Params);
        let Request = new app.AjaxRequest("GetHtmlView", Params);
        let Packet = await app.Desk.Instance.AjaxExecute(Request);

        return Packet;
    }

};

 

/** Tripous notification function. <br />
 * NOTE: It is executed before any ready listeners. */
tp.AppInitializeBefore = function () {
    new app.Desk();
};



