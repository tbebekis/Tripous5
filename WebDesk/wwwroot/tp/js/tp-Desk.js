
tp.Urls.DeskGetMainMenu = '/DeskGetMainMenu';

tp.Classes.Desk = 'tp-Desk';
tp.Classes.DeskMainMenu = 'tp-Desk-MainMenu';
tp.Classes.DeskMainMenuBarItem = 'tp-Desk-MainMenuBarItem';
tp.Classes.DeskViewPager = 'tp-Desk-ViewPager';

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

//#region tp.DeskAjaxRequest

/** Represents an ajax request */
tp.DeskAjaxRequest = class {

    /**
     * Constructor
     * @param {string} OperationName The name of the request operation.
     * @param {object} Params Optional. A user defined key-pair object with parameters.
     */
    constructor(OperationName, Params = null) {
        this.OperationName = OperationName;
        this.Params = Params || {};

        tp.DeskAjaxRequest.Counter++;
        this.Id = tp.DeskAjaxRequest.Counter.toString();
    }

    /** Optional. The id of the request.
     * @type {string}
     */
    Id = '';
    /** Required. The name of the request operation.
     * @type {string}
     */
    OperationName = '';
    /** Optional. A user defined key-pair object with parameters.
     * @type {object}
     */
    Params = {};
};

/** Creates and returns an instance based on a specified {@link tp.Command}
 * @param {tp.Command} Cmd
 */
tp.DeskAjaxRequest.CreateFromCommand = function (Cmd) {
 
    let Params = {
        Type: Cmd.Type,
        IsSingleInstance: Cmd.IsSingleInstance === true
    };
    if (Cmd.Params)
        Params = tp.MergeQuick(Params, Cmd.Params);

    let Result = new tp.DeskAjaxRequest(Cmd.Name, Params);
    return Result;
};
/** Id counter.
 * @param {number}  
 */
tp.DeskAjaxRequest.Counter = 0;

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
     * Executes an ajax request to the server and returns the Packet as it comes from server.
     * @param {tp.DeskAjaxRequest} Request The request object.
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

        var o = JSON.parse(Args.ResponseText);
        if (o.IsSuccess === false)
            tp.Throw(o.ErrorText);

        let Packet = JSON.parse(o.Packet);

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

        if (View instanceof tp.DataView)
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

    /**
     * Executes a command that returns a Packet from server for creating a modal dialog.
     * @param {tp.Command} Cmd The command to execute.
     * @returns {object} Returns the Packet from the server.
     */
    async AjaxExecuteDialog(Cmd) { 
        let Request = tp.DeskAjaxRequest.CreateFromCommand(Cmd);   
        let Packet = await tp.Desk.AjaxExecute(Request);
        return Packet;
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

    /** Creates child controls of this instance. Called in construction sequence.
    * @private
    * */
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
        let DataType = this.CreateParams.DataType;

        let Cmd = new tp.Command();
        Cmd.Name = IsInsert === true ? `Ui.SysData.Insert.${DataType}` : `Ui.SysData.Edit.${DataType}`;
        Cmd.Type = 'Ui';

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
                this.CloseView();
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




