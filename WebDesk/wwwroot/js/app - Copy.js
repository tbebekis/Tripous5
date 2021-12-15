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

 

// TODO: with InterectionObserver

/** Handles a bar of items, like a menu bar or a tab bar, and the left or right scrolling of bar items. <br />
 * The bar provides a to-left and a to-right button in order to scroll the items. <br />
 * Example markup <br />
 * <pre>
 *    <div class="container">
 *        <div class="btn-to-left">◂</div>
 *        <div class="tab-bar">
 *           <div class="item"></div>          
 *           <div class="item"></div>
 *        </div>    
 *        <div class="btn-to-right">▸</div>
 *    </div>
 * </pre>
 * */
app.TabBarHandler = class {
    constructor(Container) {
        this.elContainer = tp(Container);

        this.btnToLeft = tp.Select(this.elContainer, '.btn-to-left');
        this.TabBar = tp.Select(this.elContainer, '.tab-bar');
        this.btnToRight = tp.Select(this.elContainer, '.btn-to-right');

        this.btnToLeft.addEventListener('click', this);
        this.btnToRight.addEventListener('click', this);

        this.Arrange();

        tp.Viewport.AddListener(this.OnScreenSizeChanged, this);
    }

    elContainer = null;
    TabBar = null;
    btnToLeft = null;
    btnToRight = null;
    

    IsItemVisible(el) {
        let display = tp.GetComputedStyle(el).display;
        return !tp.IsSameText('none', display);
    }
    GetBarWidth() {
        let Parent, Result = 0;
        Parent = this.TabBar.parentNode;
        Result = tp.BoundingRect(Parent).Width;
        Result -= (tp.BoundingRect(this.btnToLeft).Width * 2);

        return Result;
    }
    GetTabBarItems() {
        return tp.ChildHTMLElements(this.TabBar);
    }
    GetVisibleItemTotalWidth() {
        let BarItems = this.GetTabBarItems();
        let i, ln, el, R, Result = 0;
        let Items = BarItems.slice().reverse();

        for (i = 0, ln = Items.length; i < ln; i++) {
            el = Items[i];
            if (!this.IsItemVisible(el))
                break;

            R = tp.BoundingRect(el);

            Result += R.Width;
        }

        return Result;
    }
    CanHideNext() {
        let BarItems = this.GetTabBarItems();
        if (BarItems.length > 2) {
            let el = BarItems[BarItems.length - 2];
            return this.IsItemVisible(el);
        }
        return false;
    }
    CanShowNext() {
        let BarItems = this.GetTabBarItems();
        if (BarItems.length > 0) {
            let el = BarItems[0];
            return !this.IsItemVisible(el);
        }
        return false;
    }
    HideNext() {
        let BarItems = this.GetTabBarItems();
        let i, ln, el;
        for (i = 0, ln = BarItems.length; i < ln; i++) {
            el = BarItems[i];
            if (this.IsItemVisible(el)) {
                el.style.display = 'none';
                break;
            }                 
        }
    }
    ShowNext() {
        let BarItems = this.GetTabBarItems();
        let i, ln, el, Items = BarItems.slice().reverse();
        for (i = 0, ln = Items.length; i < ln; i++) {
            el = Items[i];
            if (!this.IsItemVisible(el)) {
                el.style.display = '';
                break;
            }
        }
    }
    Arrange() {
        let BarItems = this.GetTabBarItems();
        let i, ln, el, BarWidth, ItemTotalWidth; 
        BarWidth = this.GetBarWidth();

        // show all
        for (i = 0, ln = BarItems.length; i < ln; i++) {
            el = BarItems[i];
            el.style.display = '';
        }

        ItemTotalWidth = this.GetVisibleItemTotalWidth();
        while (BarWidth < ItemTotalWidth) {
            this.HideNext();
            ItemTotalWidth = this.GetVisibleItemTotalWidth();
        }
    }
    /**
    Notification sent by tp.Viewport when the screen (viewport) size changes. 
    This method is called only if this.IsScreenResizeListener is true.
    @param {boolean} ScreenModeFlag - Is true when the screen mode (XSmall, Small, Medium, Large) is changed as well.
    */
    OnScreenSizeChanged(ScreenModeFlag) {
        this.Arrange();
    }
    handleEvent(e) {
        switch (e.type) {
            case 'click':
                if (tp.ContainsEventTarget(this.btnToLeft, e.target)) {
                    if (this.CanHideNext())
                        this.HideNext();
                }
                else if (tp.ContainsEventTarget(this.btnToRight, e.target)) {
                    if (this.CanShowNext())
                        this.ShowNext();
                } 
                
                break;
        }
    }

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
    new app.Desk();
};


 
