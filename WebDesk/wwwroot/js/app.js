var app = app || {};

app.Config = app.Config || {};
app.Config.CurrencySymbol = '€';

app.Resources = app.Resources || {}; 

//#region app.Header
/** Handles controls and operations of the header */
app.Header = class extends tp.tpElement {

    /** Constructor
     * @param {string|HTMLElement} ElementOrSelector
     * @param {object|tp.CreateParams} CreateParams
     */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /** Returns the ReturnUrl of the current page, if any, else empty string
     * @type {string}
     * */
    get ReturnUrl() {
        return !tp.IsBlankString(this.CreateParams.ReturnUrl) ? this.CreateParams.ReturnUrl : '';
    }

    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'app.Header'; 
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        // handles the language selection drop-down
        tp.DropDownHandler('#btnLanguage', '#cboLanguage', 'tp-Visible');
    }
 
};
//#endregion

//#region app.Footer
/** Handles controls and operations of the footer */
app.Footer = class extends tp.tpElement {

    /** Constructor
     * @param {string|HTMLElement} ElementOrSelector
     * @param {object|tp.CreateParams} CreateParams
     */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
};
//#endregion

 
//#region app.MainCommandExecutor
/** A command executor class. <br />
 *  A command executor must provide two functions: CanExecuteCommand(Cmd) and async ExecuteCommand(Cmd).
 * */
app.MainCommandExecutor = class extends tp.DeskCommandExecutor {
    constructor() {
        super();

        this.ValidCommands = [
            'Ui.SysData.Table',
            'Ui.Data.Trader'
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
            if (Cmd.IsSingleInstance && tp.Desk.ViewPager.ViewExists(Cmd.Name)) {
                let View = tp.Desk.ViewPager.FindViewByName(Cmd.Name);
                tp.Desk.ViewPager.ShowView(View);
                Result = View;
            }
            else {
                let Request = tp.DeskAjaxRequest.CreateFromCommand(Cmd);  
                let Packet = await tp.Desk.AjaxExecute(Request);
                if (tp.IsValid(Packet))
                    Result = await tp.Desk.ViewPager.AddView(Packet);
            }
        }
        else {
            tp.Throw('Command not found');
        }

        return Result;
    }
};
//#endregion


/** Tripous notification function. <br />
 * NOTE: It is executed before any ready listeners. */
tp.AppInitializeBefore = function () {
 
    let el;

    let GetElement = (Selector) => {
        let Result = tp(Selector);
        if (!tp.IsHTMLElement(Result))
            tp.Throw(`Element not found. Selector: ${Selector}`);
        return Result;
    };

    // header
    el = GetElement('.header');
    tp.DeskOptions.Header = new app.Header(el);

    // footer
    el = GetElement('.footer');
    tp.DeskOptions.Footer = new app.Footer(el);

    // main menu
    el = GetElement('.' + tp.Classes.DeskMainMenu);
    tp.DeskOptions.MainMenu = new tp.DeskMainMenu(el);

    // main command executor
    tp.DeskOptions.MainCommandExecutor = new app.MainCommandExecutor();

    // desk view main pager  
    el = GetElement('.' + tp.Classes.DeskViewPager);

    tp.DeskOptions.ViewPager = new tp.DeskViewPager(el);

    // desktop
    tp.Desk = new tp.Desktop(tp.DeskOptions);
};


 
