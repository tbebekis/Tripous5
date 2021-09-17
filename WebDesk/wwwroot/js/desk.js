

/** A command class. This is here mostly for reference only.
 * */
app.Command = class {

    /** constructor */
    constructor() {
    }

    /** A name unique among all commands. */
    Name = '';
    /** The command type. What a command does when it is called. */
    Type = 'Ui';
    /** True when this is a single instance Ui command. */
    IsSingleInstance = false;
    /** Gets meaning according to command's type */
    Tag = '';
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
 
        this.Pages = [];
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
    Pages = [];

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
        let Tab = TabList.find(elTab => { return tp.IsValid(elTab.__PageInfo) ? ViewName === elTab.__PageInfo.Name : false; });
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
     * Creates a tab and a page and displays both in the Ui.
     * @param {object} Packet The packet as it comes from server
     * @param {string} Name The page name
     * @param {object} Params A key-pair object
     */
    async CreateTab(Packet, Name, Params = null) {

        let TabHtmlText = `
       <div class="tab-item">
           <div class="tp-Text">${Name}</div>
       </div>
`;
        let PageHtmlText = Packet.HtmlText.trim();

        let elPage = app.GetContentElement(PageHtmlText);
        let elTab = app.GetContentElement(TabHtmlText);

        Params = Params || {};
        Params.Name = Name;
        Params.elTab = elTab;
        Params.elPage = elPage;

        elTab.__PageInfo = Params;

        this.elPages.appendChild(elPage);
        this.elTabs.appendChild(elTab);

        await this.StartPage(elPage, Params);
 
        this.TabClicked(elTab);
    }
    /**
     * "Starts" a specified page that is already added to DOM.. <br />
     * This function dynamically imports a javascript module specified by the markup of the page and then calls the StartPage() of that module. <br />
     * @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/import#dynamic_import|Dynamic Import}
     * @param {HTMLElement} elPage
     * @param {object} Params A key-value object with initialization information.
     */
    async StartPage(elPage, Params) {
        let Setup = app.GetDataObject(elPage, 'setup');
        let ModulePath = Setup.PageModule;

        Setup = tp.MergeQuick(Setup, Params || {});

        let P = await import(ModulePath)
            .then(module => {
                module.StartPage(elPage);
            });

        return P;
    }
    /**
     * Handles the click on a tab.
     * @param {HTMLElement} elTab The tab
     */
    TabClicked(elTab) {
        let i, ln;

        let TabList = this.GetTabList();
        let PageList = this.GetPageList();

        let elPage = elTab.__PageInfo.elPage;

        for (i = 0, ln = TabList.length; i < ln; i++) {
            tp.RemoveClass(TabList[i], tp.Classes.Selected);
            PageList[i].style.display = 'none';
        }

        tp.AddClass(elTab, tp.Classes.Selected);
        elPage.style.display = '';
    }
    /**
     * Gets a HTML view from server
     * @param {string} Name Required. A name uniquely identifying the Ui request.
     * @param {object} Params Optional. A key-value object.
     */
    async GetHtmlView(Name, Params = null) {
        let Url = '/GetHtmlView'
        let Model = {
            Name: Name,
            Params: Params
        };
        let Args = await tp.Ajax.PostModelAsync(Url, Model);
        if (Args.ResponseData.IsSuccess === true) {
            let Packet = Args.ResponseData.Packet;
            await this.CreateTab(Packet, Name, Params);
        }
        else {
            app.DisplayAjaxErrors(Args);
            return;
        }

    }
};

 

/** A command executor class.
 *  A command executor must provide two functions: CanExecuteCommand(Cmd) and async ExecuteCommand(Cmd).
 * */
app.MainMenuCommandExecutor = class {
    constructor() {
    }

    /** A list of command names this executor can handle.
     * @type {string[]}
     */
    ValidCommands = [
        'AppTable.Ui.List'
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
     * @returns {Promise} Returns whatever the specified command dictates to return.
     */
    async ExecuteCommand(Cmd) {
        let Result = null;
        let Params = tp.MergeQuick({}, Cmd);
        

        if (Cmd.Type === 'Ui') {
            if ((Cmd.IsSingleInstance && !app.Desk.Instance.TabExists(Cmd.Name)) || !Cmd.IsSingleInstance)
                Result = await app.Desk.Instance.GetHtmlView(Cmd.Name, Params);
        }
        else {
            tp.Throw('Command not found');
 
        }


        return Result;
    }
};

/** Represents a desk page */
app.Desk.Page = class {

    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [CreateParams=null] Optional. A javascript object with initialization parameters. 
     */
    constructor(elPage, CreateParams = null) {
        this.Handle = elPage;
        this.CreateParams = CreateParams || {};

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
    IsDisposed = false;
    /** A javascript object with initialization parameters.
     * @type {object}
     */
    CreateParams = {};


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
            //this.IsElementResizeListener = false;
            //this.IsScreenResizeListener = false;

            var el = this.Handle;
            tp.SetObject(this.Handle, null);
            this.Handle = null;
            if (tp.IsElement(el.parentNode)) {
                el.parentNode.removeChild(el);
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
};

tp.AppInitializeBefore = function () {
    new app.Desk();   
};


