
app.DeskClass = class {
    constructor() {

        this.elMainMenu = tp('.main-menu');
        this.elPager = tp('.pager-container.main-pager');

        this.elTabBarContainer = tp.Select(this.elPager, '.tab-container');
        new app.TabBarHandler(this.elTabBarContainer);

        this.elTabs = tp.Select(this.elPager, '.tab-bar'); 
        this.elPages = tp.Select(this.elPager, '.page-list');
 
        this.Views = [];
        this.CommandExecutors = [];

        this.CommandExecutors.push(new app.MainMenuCommandExecutor()); 

        if (this.elMainMenu)
            this.elMainMenu.addEventListener('click', this);

        if (this.elTabBarContainer)
            this.elTabBarContainer.addEventListener('click', this);

        tp.Viewport.AddListener(this.OnScreenSizeChanged, this);
    }

    elMainMenu = null;
    elPager = null;
    elTabBarContainer = null;
    elTabs = null;          // TabBar
    elPages = null;         // TabPages
    CommandExecutors = [];
    Views = [];

    /**
    Notification sent by tp.Viewport when the screen (viewport) size changes. 
    This method is called only if this.IsScreenResizeListener is true.
    @param {boolean} ScreenModeFlag - Is true when the screen mode (XSmall, Small, Medium, Large) is changed as well.
    */
    OnScreenSizeChanged(ScreenModeFlag) { 
    }
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


    RegisterCommandExecutor(Executor) {
        this.CommandExecutors.push(Executor);
    }
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
    CreateTab(Packet, Name, Params = null) {

        let TabHtmlText = `
       <div class="tab-item">
           <div class="tp-Text">${Name}</div>
       </div>
`;
        let PageHtmlText = Packet.HtmlText.trim();

        let elPage = app.GetContentElement(PageHtmlText);
        let elTab = app.GetContentElement(TabHtmlText);

        elTab.__PageInfo = {
            Name: Name,
            Params: Params,
            Page: elPage
        };

        this.elPages.appendChild(elPage);
        this.elTabs.appendChild(elTab);

        this.TabClicked(elTab);
    }

    /**
     * Handles the click on a tab.
     * @param {HTMLElement} elTab The tab
     */
    TabClicked(elTab) {
        let i, ln;

        let TabList = this.GetTabList();
        let PageList = this.GetPageList();

        let elPage = elTab.__PageInfo.Page;

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
            //tp.SuccessNote('OK');
            let Packet = Args.ResponseData.Packet;
            this.CreateTab(Packet, Name, Params);
        }
        else {
            app.DisplayAjaxErrors(Args);
            return;
        }

    }
};

/** 
 *  @type {app.DeskClass} */
app.Desk = null;

app.MainMenuCommandExecutor = class {
    constructor() {
    }

    ValidCommands = [
        'AppTable.Ui.List'
    ];

    CanExecuteCommand(Cmd) {
        return this.ValidCommands.find(item => { return tp.IsSameText(item, Cmd.Name); });  
    }

    async ExecuteCommand(Cmd) {
        let Result = null;
        let Params = {
            S: 'hi there',
            Bool: true,
            Age: 15
        };

        if (Cmd.Type === 'Ui') {
            if ((Cmd.IsSingleInstance && !app.Desk.TabExists(Cmd.Name)) || !Cmd.IsSingleInstance)
                Result = await app.Desk.GetHtmlView(Cmd.Name, Params);
        }
        else {
            tp.Throw('Command not found');
 
        }


        return Result;
    }

 

};

tp.AppInitializeBefore = function () {
    app.Desk = new app.DeskClass();   
};


