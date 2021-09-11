/** Handles the main menu bar 
 * Example markup <br />
 * <pre>
 *    <div class="main-menu"> *
 *        <div class="btn-to-left">◂</div>
 *        <div class="menu-bar">
 *
 *           <div class="bar-item">
 *               <div class="tp-Text">@BarItem.Title</div>
 *               <div class="tp-List">...</div>
 *           </div>
 *
 *           <div class="bar-item">
 *               <div class="tp-Text">@BarItem.Title</div>
 *               <div class="tp-List">...</div>
 *           </div>
 *        </div> *
 *        <div class="btn-to-right">▸</div>
 *    </div>
 * </pre> 
 */
app.MainMenuHandler = function () {
    const CssClass = 'tp-Visible';   

    let elMainMenu = tp('.main-menu');
    let elMenuBar = tp.Select(elMainMenu, '.menu-bar');
    let btnToLeft = tp.Select(elMainMenu, '.btn-to-left');
    let btnToRight = tp.Select(elMainMenu, '.btn-to-right');

    new app.BarHandler(elMenuBar, btnToLeft, btnToRight);

    this.BarItems = tp.ChildHTMLElements(elMenuBar);

    this.BarItems.forEach((elBarItem) => {
        let Button = tp.Select(elBarItem, '.tp-Text');
        let List = tp.Select(elBarItem, '.tp-List');

        app.DropDownHandler(Button, List, CssClass);
    });
};

app.DeskClass = class {
    constructor() {

        this.elMainMenu = tp('.main-menu');
        this.CommandExecutors = [];

        if (this.elMainMenu)
            this.elMainMenu.addEventListener('click', this);

        tp.Viewport.AddListener(this.OnScreenSizeChanged, this);
    }

    elMainMenu = null;
    CommandExecutors = [];

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
                if (tp.HasClass(e.target, 'main-menu-command')) {
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
};

app.MainMenuCommandExecutor = class {
    constructor() {
    }

    CanExecuteCommand(Cmd) {
        return Cmd.Name === 'New';
    }

    async ExecuteCommand(Cmd) {
        let Result = null;
        switch (Cmd.Name) {
            case 'New':
                tp.InfoNote('hi there');
                break;
        }

        return Result;
    }

};

tp.AppInitializeBefore = function () {
    app.Desk = new app.DeskClass();

    app.Desk.RegisterCommandExecutor(new app.MainMenuCommandExecutor());
};


