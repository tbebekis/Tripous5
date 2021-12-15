var app = app || {};

app.Config = app.Config || {};
app.Config.CurrencySymbol = '€';

app.Resources = app.Resources || {}; 


/** Handles controls and operations of the header */
app.Header = class {
    constructor(Options) {
        let i, ln, List;
        if (app.Header.Instance !== null)
            tp.Throw('app.Header is a single-instance class');

        app.Header.Instance = this;
        this.Options = Options;

        // handles the language selection drop-down
        tp.DropDownHandler('#btnLanguage', '#cboLanguage', 'tp-Visible');



        // events
        tp.Doc.body.addEventListener('click', this);

        tp.Viewport.AddListener(this.ScreenSizeChanged, this);
    }

    Options = {
        ReturnUrl: '',
    }; 

    handleEvent(e) {
        switch (e.type) {
            case 'click':

                break;

        }
    }

    /**
     * Called when the screen size changes.
     * @param {bool} ScreenModeFlag True when screen mode has changed too (i.e. from Large to Medium)
     */
    ScreenSizeChanged(ScreenModeFlag) {
        if (ScreenModeFlag === true) {
            if (tp.Viewport.IsXSmall || tp.Viewport.IsSmall) {
                //
            }
            else {
                //
            }
        }
    }
};
app.Header.Instance = null;

/** Handles controls and operations of the header */
app.Footer = {
};



 
 

/** A command executor class. <br />
 *  A command executor must provide two functions: CanExecuteCommand(Cmd) and async ExecuteCommand(Cmd).
 * */
app.MainMenuCommandExecutor = class extends tp.DesktopCommandExecutor {
    constructor() {
        super();

        this.ValidCommands = [
            'Ui.SysData.Tables',
            'Ui.Traders'
        ];
    } 
 
    /**
     * Executes a specified command. The CanExecuteCommand() is called just before this call.
     * @param {tp.Command} Cmd The command to execute.
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
                let Request = new tp.DesktopAjaxRequest(Cmd.Name, Params);
                let Packet = await tp.Desk.AjaxExecute(Request);
                if (tp.IsValid(Packet))
                    Result = await tp.Desk.CreateViewElement(Packet);
            }
        }
        else {
            tp.Throw('Command not found');
        }

        return Result;
    }
};



/** Tripous notification function. <br />
 * NOTE: It is executed before any ready listeners. */
tp.AppInitializeBefore = function () {
 
    let el;
    //new app.Desk();
    el = tp('.' + tp.Classes.DeskMainMenu);
    new tp.DeskMainMenu(el);
};


 
