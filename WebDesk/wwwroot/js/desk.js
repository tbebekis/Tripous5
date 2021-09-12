
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
                tp.Throw('This is an error');
                //tp.InfoNote('hi there');
                //await tp.YesNoBoxAsync('this is a message');
                //let Args = await tp.ContentWindow.ShowModalAsync('Content window', 'this is content');
                break;
        }

        return Result;
    }

};

tp.AppInitializeBefore = function () {
    app.Desk = new app.DeskClass();

    app.Desk.RegisterCommandExecutor(new app.MainMenuCommandExecutor());
};


