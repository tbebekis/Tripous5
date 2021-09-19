﻿/** A command class. This is here mostly for reference only. */
app.Command = class {

    /** constructor */
    constructor() {
        this.Params = {};
    }

    /** A name unique among all commands. */
    Name = '';
    /** The command type. What a command does when it is called. */
    Type = 'Ui';
    /** True when this is a single instance Ui command. */
    IsSingleInstance = false;
    /** User defined parameters */
    Params = {};
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
        this.elPager = tp('.pager-container.main-pager');

        this.elTabBarContainer = tp.Select(this.elPager, '.tab-container');
        new app.TabBarHandler(this.elTabBarContainer);

        this.elTabs = tp.Select(this.elPager, '.tab-bar'); 
        this.elPages = tp.Select(this.elPager, '.page-list');
 
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

    elMainMenu = null;
    elPager = null;
    elTabBarContainer = null;
    elTabs = null;          // TabBar
    elPages = null;         // TabPages

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
                    Cmd = app.GetDataObject(e.target, 'setup');
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
    /** Returns the list of pages.
     * @returns {HTMLElement[]} Returns the list of pages.
     * */
    GetPageList() {
        let Result = tp.ChildHTMLElements(this.elPages)
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
     * Creates a tab element and a page element and adds both to the DOM.
     * @param {object} Packet The packet as it comes from server
     */
    async CreatePageElement(Packet) {
        let TabHtmlText = `
       <div class="tab-item">
           <div class="tp-Text">${Packet.ViewName}</div>
       </div>
`;
      
        let elPage = app.GetContentElement(Packet.HtmlText.trim());
        let elTab = app.GetContentElement(TabHtmlText.trim());

        let DeskInfo = {};
        DeskInfo.Name = Packet.ViewName;
        DeskInfo.elTab = elTab;
        DeskInfo.elPage = elPage;

        app.DeskInfo(elTab, DeskInfo);
        app.DeskInfo(elPage, DeskInfo);

        this.elPages.appendChild(elPage);
        this.elTabs.appendChild(elTab);

        let CreateParams = {
            Name: Packet.ViewName,
            Packet: Packet,
            elTab: elTab
        };

        await this.CreatePage(elPage, CreateParams);

        this.TabClicked(elTab);
    }
    /**
     * Creates a specified page object on an element that is already added to DOM and returns that page instance. <br />
     * The created element provides a data-setup attribute specifying a javascript file, a css file and a page class.  <br />
     * This function dynamically loads the specified files, if not already loaded, and then creates the instance of the page object. <br />
     * @param {HTMLElement} elPage
     * @param {object} Params A key-value object with initialization information.
     * @returns {app.Desk.Page} Returns the page instance.
     */
    async CreatePage(elPage, Params) {
        let Page = null;
        let Setup = app.GetDataObject(elPage, 'setup'); 

        if (!tp.IsBlank(Setup.PageCSS)) {
            let P = await app.LoadCssFile(Setup.PageCSS);
        }

        if (!tp.IsBlank(Setup.PageJS)) {
            let P = await app.LoadJavascriptFile(Setup.PageJS);
        }
 
        if (!tp.IsBlank(Setup.PageClass)) {
            let Code = `new ${Setup.PageClass}(elPage, Params)`;
            Page = eval(Code);
        }

        return Page;
    }
    /**
     * Handles the click on a tab.
     * @param {HTMLElement} elTab The tab
     */
    TabClicked(elTab) {
        let i, ln;

        let TabList = this.GetTabList();
        let PageList = this.GetPageList();

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
        'Ui.SysData.List.Table'
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
 
        if (Cmd.Type === 'Ui') {
            if ((Cmd.IsSingleInstance && !app.Desk.Instance.TabExists(Cmd.Name)) || !Cmd.IsSingleInstance) {
                let Params = {
                    ViewName: Cmd.Name
                };
                Params = tp.MergeQuick(Params, Cmd.Params);
                let Request = new app.AjaxRequest("GetHtmlView", Params);
                let Packet = await app.Desk.Instance.AjaxExecute(Request);
                if (tp.IsValid(Packet))
                    Result = await app.Desk.Instance.CreatePageElement(Packet);
            }                
        }
        else {
            tp.Throw('Command not found'); 
        }

        return Result;
    }
};

/** Represents a desk page. Used as base page class. */
app.Desk.Page = class {

    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [CreateParams=null] Optional. A javascript object with initialization parameters. 
     */
    constructor(elPage, CreateParams = null) {
        this.Handle = elPage;
        this.CreateParams = CreateParams || {};
        this.Setup = app.GetDataObject(elPage, 'setup') || {};

        tp.Broadcaster.Add(this);
        this.ScreenResizeListener = tp.Viewport.AddListener(this.OnScreenSizeChanged, this);

        this.InitializeFields();
        this.OnFieldsInitialized();                         // notification

        this.ProcessCreateParams(this.CreateParams);
        this.OnCreateParamsProcessed();                     // notification

        this.OnInitializationCompleted();                   // notification

        this.CreateControls();
    }

    /** The page element.
     * @type {HTMLElement}
     */
    Handle = null;
    /** Listens for screen size and screen size mode (small, large, etc.) changes. <br />
     * Screen size notifications handled by the OnScreenSizeChanged() method.
     * @type{tp.Listener}
     */
    ScreenResizeListener = null;
    /** True when this instance is disposed. 
     * NOTE: Close() disposes this instance.
     * @type {boolean}
     * */
    IsDisposed = false;
    /** A javascript object with initialization parameters. <br />
     * Contains the ViewName property and the Packet property with the Packet as it came from server.
     * @type {object}
     */
    CreateParams = {};
    /** A javascript object with initialization parameters passed to data-setup attribute of the element.
     * @type {object}
     */
    Setup = {};

    /** Returns and array of property names the ProcessCreateParams() should NOT set 
     @returns {string[]} Returns and array of property names the ProcessCreateParams() should NOT set
     */
    GetAvoidParams() {
        return ['Id', 'Name', 'Handle', 'Parent', 'Html', 'CssClasses', 'CssText'];
    }
    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @param {object} [o=null] - Optional. The create params object to processs.
    */
    ProcessCreateParams(o = null) {
        o = o || {};

        let AvoidParams = this.GetAvoidParams();


        let Allowed = false;
        let HasSetter = false;
        let IsWritable = false;

        for (var Prop in o) {
            if (!tp.IsFunction(o[Prop])) {
                Allowed = AvoidParams.indexOf(Prop) === -1;
                HasSetter = tp.GetPropertyInfo(this, Prop).HasSetter;
                IsWritable = tp.IsWritableProperty(this, Prop);

                if (Allowed && (HasSetter || IsWritable)) {
                    this[Prop] = o[Prop];
                }
            }
        }
    }
    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    */
    Dispose() {
        if (this.IsDisposed === false && tp.IsElement(this.Handle)) {

            tp.Broadcaster.Remove(this);

            if (this.ScreenResizeListener) {
                tp.Viewport.RemoveListener(this.ScreenResizeListener);
                this.ScreenResizeListener = null;
            }

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
                } catch  {
                    //
                }
            }

            if (!tp.IsBlank(this.Setup.PageCSS)) {
                app.UnLoadCssFile(this.Setup.PageCSS);
            }

            if (!tp.IsBlank(this.Setup.PageJS)) {
                app.UnLoadJavascriptFile(this.Setup.PageJS);
            }

            this.IsDisposed = true;
        }
    }
    /**
    Initializes fields and properties just before applying the create params.        
    */
    InitializeFields() {
    }
    /** Creates page controls */
    CreateControls() {
    }

    /* notifications */
    /**
    Notification sent by tp.Viewport when the screen (viewport) size changes.  
    @param {boolean} ScreenModeFlag - Is true when the screen mode (XSmall, Small, Medium, Large) is changed as well.
    */
    OnScreenSizeChanged(ScreenModeFlag) { }
    /** 
     * This class is a {@link tp.Broadcaster}   listener.
     * This method is called by tp.Broadcaster to notify a listener about an event.
     * @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
     * @returns {any} Returns a value or null.
     */
    BroadcasterFunc(Args) { return null; }

    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnFieldsInitialized() { }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization and options (CreateParams) processing  
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnCreateParamsProcessed() { }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing 
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() { }

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



