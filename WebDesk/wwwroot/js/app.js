var app = app || {};

app.Config = app.Config || {};
app.Config.CurrencySymbol = '€';

app.Resources = app.Resources || {}; 

/**
 * Converts a string to float number. Returns a float number on success or a specified default value on failure. <br />
 * The decimal separator could be point or comma. <br />
 * @param {string} v The string to operate on.
 * @param {number} Default Optional. The default value to return on failure.
 * @returns {number} Returns a float number on success or a specified default value on failure.
 */
app.StrToFloat = function (v, Default = 0) {
    if (tp.IsString(v)) {
        v = v.replace(',', '.');
        let Convertion = tp.TryStrToFloat(v);
        if (Convertion.Result === true) {
            return Convertion.Value;
        }
        return Default;
    };

    return v;
}

/**
 * Waits for a specified number of milli-seconds and then calls a specified function, if passed.
 * @param {number} MSecsToWait Milli-seconds to wait
 * @param {Function} [FuncToCall=null] Function to call
 */
app.WaitAsync = async function (MSecsToWait, FuncToCall = null) {
    function f() { }
    FuncToCall = FuncToCall || f;
    return new Promise((Resolve, Reject) => {
        setTimeout(() => {
            try {
                f();
                Resolve();
            } catch (e) {
                Reject(e);
            }
        }, MSecsToWait);
    })
};


/** A list with the dynamically loaded modules (javascript files). */
app.JavascriptFiles = [];
/** A list with the dynamically loaded css files. */
app.CssFiles = [];

/**
 * Loads a javascript file dynamically.
 * NOTE: Files loaded dynamically are referenced counted. When ref-counting drops to zero the element is deleted from DOM.
 * @param {string} Url The url path of the file.
 * @returns {Promise} Returns a promise.
 */
app.LoadJavascriptFile = async function (Url) {
    // NOTE: It seems there is a severe problem with the Visual Studio debugger 
    // and awaiting the Promise the import() function returns. 
    // It crashes Chrome making debugging impossible.
    // So, no ES6 modules yet. At least dynamically loadable modules.

    // let P = import(Url);
    // return P;

    let FileUrl = Url.toLowerCase();
    let oFile = app.JavascriptFiles.find(item => { return item.FileUrl === FileUrl; });

    if (!tp.IsValid(oFile)) {
        let ExecutorFunc = (Resolve, Reject) => {
            try {
                let el = tp.Doc.createElement("script");
                el.src = Url;
                el.onload = function () {
                    oFile = {
                        FileUrl: FileUrl,
                        Counter: 1,
                        Element: el
                    };
                    app.JavascriptFiles.push(oFile);

                    Resolve();
                };
                el.onerror = function (e) {
                    Reject(e);
                };
                let Head = tp('head');
                Head.appendChild(el);
            } catch (e) {
                Reject(e);
            }
        };

        let Result = new Promise(ExecutorFunc);
        return Result;
    }
    else {
        oFile.Counter += 1;
    }
 
    return Promise.resolve(); 
};
/**
 * Unloads a dynamically loaded javascript file.
 * @param {string} Url The url path of the file.
 */
app.UnLoadJavascriptFile = function (Url) {
    let FileUrl = Url.toLowerCase();
    let oFile = app.JavascriptFiles.find(item => { return item.FileUrl === FileUrl; });

    if (tp.IsValid(oFile)) {
        oFile.Counter -= 1;

        if (oFile.Counter <= 0) {
            let Head = tp('head');
            Head.removeChild(oFile.Element);
            tp.ListRemove(app.JavascriptFiles, oFile);            
        }
    }
};

/**
 * Loads a list of javascript files dynamically.
 * @param {string[]} UrlList An array with the url paths of the files.
 */
app.LoadJavascriptFiles = async function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            await app.LoadJavascriptFile(UrlList[i]);
        }
    }
};
/**
 * Unloads a dynamically loaded list of javascript files 
 * @param {string[]} UrlList An array with the url paths of the files.
 */
app.UnLoadJavascriptFiles = function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            app.UnLoadJavascriptFile(UrlList[i]);
        }
    }
};

/**
 * Loads a css file dynamically.
 * NOTE: Files loaded dynamically are referenced counted. When ref-counting drops to zero the element is deleted from DOM.
 * @param {string} Url The url path of the file.
 * @returns {Promise} Returns a promise.
 */
app.LoadCssFile = async function (Url) {

    let FileUrl = Url.toLowerCase();
    let oFile = app.CssFiles.find(item => { return item.FileUrl === FileUrl; });

    if (!tp.IsValid(oFile)) {
        let ExecutorFunc = (Resolve, Reject) => {
            try {
                let el = tp.Doc.createElement("link");
                el.href = Url;
                el.rel = 'stylesheet';
                el.type = 'text/css';

                el.onload = function () {
                    oFile = {
                        FileUrl: FileUrl,
                        Counter: 1,
                        Element: el
                    };
                    app.CssFiles.push(oFile);
 
                    Resolve();
                };
                el.onerror = function (e) {
                    Reject(e);
                };
                let Head = tp('head');
                Head.appendChild(el);
            } catch (e) {
                Reject(e);
            }
        };

        let Result = new Promise(ExecutorFunc);
        return Result;
    }
    else {
        oFile.Counter += 1;
    }

    return Promise.resolve();
};
/**
 * Unloads a dynamically loaded css file.
 * @param {string} Url The url path of the file.
 */
app.UnLoadCssFile = function (Url) {
    let FileUrl = Url.toLowerCase();
    let oFile = app.CssFiles.find(item => { return item.FileUrl === FileUrl; });

    if (tp.IsValid(oFile)) {
        oFile.Counter -= 1;

        if (oFile.Counter <= 0) {
            let Head = tp('head');
            Head.removeChild(oFile.Element);
            tp.ListRemove(app.CssFiles, oFile);
        }
    }
};

/**
 * Loads a list of css files dynamically.
 * @param {string[]} UrlList An array with the url paths of the files.
 */
app.LoadCssFiles = async function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            await app.LoadCssFile(UrlList[i]);
        }
    }
};
/**
 * Unloads a dynamically loaded list of css files
 * @param {string[]} UrlList An array with the url paths of the files.
 */
app.UnLoadCssFiles = function (UrlList) {
    if (tp.IsArray(UrlList)) {
        let i, ln;

        for (i = 0, ln = UrlList.length; i < ln; i++) {
            app.UnLoadCssFile(UrlList[i]);
        }
    }
};

/**
 * Should be called after a tp ajax call having errors. Displays the errors using notification boxes.
 * @param {tp.AjaxArgs} Args An instance of tp ajax arguments
 */
app.DisplayAjaxErrors = function (Args) {
    let i, ln, Packet;

    if (Args.ResponseData.IsSuccess === false) {
        if (tp.IsString(Args.ResponseData.ErrorText) && !tp.IsBlank(Args.ResponseData.ErrorText)) {
            tp.ErrorNote(Args.ResponseData.ErrorText);
        }
        if (tp.IsString(Args.ResponseData.Packet) && !tp.IsBlank(Args.ResponseData.Packet)) {
            tp.ErrorNote(Args.ResponseData.Packet);
        }
        else if (tp.IsArray(Args.ResponseData.Packet)) {
            Packet = Args.ResponseData.Packet;
            for (i = 0, ln = Packet.length; i < ln; i++) {
                tp.ErrorNote(Packet[i]);
            }
        }
    }
};

/**
 * Gets or sets an key-value information object to a DOM element. <br />
 * If the second parameter is not passed the it just gets and returns the information object.
 * @param {HTMLElement} el The DOM element to get from or set the info to.
 * @param {object} Info A key-value information object
 */
app.DeskInfo = function (el, Info = null) {
    if (tp.IsValid(Info)) {
        el.__DeskInfo = Info;
    }
    else {
        if ('__DeskInfo' in el)
            Info = el.__DeskInfo;
    }

    return Info;
};

 

/** Creates and returns an overlay div.
 * @returns {HTMLElement} Returns the overlay div. */
app.CreateOverlay = function () {
    let Result = tp.Div(tp.Doc.body);
    Result.id = tp.SafeId('Overlay');
    tp.AddClass(Result, 'overlay');
    tp.BringToFront(Result);
    return Result;
};

/**
 * Handles a drop-down operation by toggling the visibility and positioning of the drop-down list. <br />
 * The list element could be in absolute or fixed position.
 * @param {string|Element} Button Selector or Element. The element which displays the drop-down when is clicked
 * @param {string|Element} List Selector or Element. The drop-down list
 * @param {string} CssClass The css class to toggle to the drop-down list
 */
app.DropDownHandler = function (Button, List, CssClass = 'tp-Visible') {
    Button = tp(Button);
    List = tp(List);
 
    Button.addEventListener('click', (ev) => {
        let Position = tp.ComputedStyle(List).position;
        let R = Button.getBoundingClientRect();
        let P; // tp.Point

        // toggle() returns true if adds the class
        if (List.classList.toggle(CssClass)) {
            if (tp.IsSameText('absolute', Position)) {
                P = tp.ToParent(Button);
                List.X = P.X;
                List.Y = P.Y + R.height;
            } else if (tp.IsSameText('fixed', Position)) {
                P = tp.ToViewport(Button);
                List.X = P.X;
                List.Y = P.Y + R.height;
            }
        }
    });

    window.addEventListener('click', (ev) => {
        if (List.classList.contains(CssClass) && !tp.ContainsEventTarget(Button, ev.target)) {
            List.classList.remove(CssClass);
        }
    });

    window.addEventListener('scroll', (ev) => {
        if (List.classList.contains(CssClass) && !tp.ContainsEventTarget(Button, ev.target)) {
            List.classList.remove(CssClass);
        }
    });
};

/**
 * Returns an HTMLElement based on a specified Selector or HtmlText. Throws an exception on failure. <br />
 * Used when he have to display a content div inside another div, such as in dialog boxes.
 * @param {HTMLElement|string} ElementOrSelectorOrHtmlText Could be an HTMLElement, a selector or just plain HTML text.
 * @returns {HTMLElement} Returns an HTMLElement based on a specified Selector or HtmlText
 */
app.GetContentElement = function (ElementOrSelectorOrHtmlText) {
    let Result = null;
    if (tp.IsElement(ElementOrSelectorOrHtmlText)) {
        Result = ElementOrSelectorOrHtmlText;
    }

    if (Result === null) {
        if (tp.ContainsText(ElementOrSelectorOrHtmlText, '<div',  true)) {
            // create a temp div
            let div = tp.Div(tp.Doc.body);
            div.innerHTML = ElementOrSelectorOrHtmlText.trim();
            Result = div.firstChild;
            div.parentNode.removeChild(div);
        }
        else {
            Result = tp(ElementOrSelectorOrHtmlText);
        }
    }

    if (Result === null) {
        tp.Throw('Can not extract the Content element');
    }

    return Result;

};

/** Options class for the modal dialog */
app.ModalDialogOptions = function () {

    return {
        /** The caption title of the dialog */
        Text: '',
        /** When true the dialog closes with Cancel() when the user clicks on the underlying overlay DIV */
        CloseOnOverlayClick: true,
        /** When true the dialog is a non-resizable one */
        NonResizable: true,
        /** The context (this) of the call-back functions */
        Context: null,
        /** Call-back function, called after the dialog is initialized */
        OnInitialized: (Box) => { },
        /** Call-back function, called just before the dialog closes */
        OnClosing: (Box) => { },
        /** Call-back function, called after the dialog is closed */
        OnClosed: (Box) => { }
    };

};


/** Used by app.ModalDialog class, see below. */
app.InternalModalDialogClass = class {
    constructor(elContent, Options) {

        // options
        let DefaultOptions = new app.ModalDialogOptions();

        Options = Options || {};
        this.Options = {};
        for (var Prop in DefaultOptions) {
            this.Options[Prop] = Prop in Options ? Options[Prop] : DefaultOptions[Prop];
        }

        // overlay
        this.Overlay = app.CreateOverlay();
        this.Overlay.style.justifyContent = 'center';
        this.Overlay.style.alignItems = 'center';

        // dialog
        this.elDialog = tp('.dialog-box-template');
        this.elDialog = this.elDialog.cloneNode(true);
        this.elDialog.className = 'dialog-box';
        this.Overlay.appendChild(this.elDialog);

        // title
        this.elTitle = tp.Select(this.elDialog, '.title');
        this.btnClose = tp.Select(this.elDialog, '.close');

        // content container  and content
        this.elContentContainer = tp.Select(this.elDialog, '.content-container');
        this.elContent = tp(elContent);
        this.OldParent = this.elContent.parentNode;

        // events
        this.btnClose.addEventListener('click', this);      // close button        
        tp.Doc.body.addEventListener('keyup', this);        // close on ESCAPE key        
        if (this.Options.CloseOnOverlayClick === true)      // close on overlay click
            this.Overlay.addEventListener('click', this);

        // title and content        
        this.elTitle.innerHTML = this.Options.Text;
        this.elContentContainer.appendChild(this.elContent);

        // location
        tp.Viewport.CenterInWindow(this.elDialog);

        // Dragger setup
        let Mode = this.Options.NonResizable === true ? tp.DraggerMode.Drag : tp.DraggerMode.Both;
        this.Dragger = new tp.Dragger(Mode, this.elDialog, this.elTitle);
        this.Dragger.On(tp.Events.DragStart, (Args) => {
            if (this.Dragger) {
                let X = this.elDialog.offsetLeft - 5;
                let Y = this.elDialog.offsetTop - 5;

                requestAnimationFrame(() => {
                    this.elDialog.style.top = tp.px(Y);
                    this.elDialog.style.left = tp.px(X);
                });
            }
        }, null);


        // call on initialized
        tp.Call(this.Options.OnInitialized, this.Options.Context, this);

        // re-center
        tp.Viewport.CenterInWindow(this.elDialog);

    }

    Options = {};

    Overlay = null;
    elDialog = null;
    elTitle = null;
    btnClose = null;
    elContentContainer = null;
    elContent = null;

    OldParent = null;
    Cancelled = false;
    ResolveFunc = null;


    handleEvent(e) {
        switch (e.type) {
            case 'click':
                if (this.btnClose === e.target || tp.ContainsElement(this.btnClose, e.target)) {
                    e.stopPropagation();
                    this.Cancel();
                }
                else if (this.Overlay === e.target && !tp.ContainsEventTarget(this.elDialog, e.target)) {
                    e.stopPropagation();
                    this.Cancel();
                }
                break;
            case 'keyup':
                if (e.keyCode === tp.Keys.Escape && !tp.IsEmpty(this.Overlay)) {
                    this.Cancel();
                }
                break;
        }
    }

    Cancel() {
        if (tp.IsValid(this.Overlay)) {
            this.Cancelled = true;
            this.Close();
        }
    }
    Close(DialogResult = null) {
        if (tp.IsValid(this.Overlay)) {

            // remove handlers
            this.btnClose.removeEventListener('click', this);

            if (this.Options.CloseOnOverlayClick === true)
                this.Overlay.removeEventListener('click', this);

            tp.Doc.body.removeEventListener('keyup', this);

            // handle the results
            this.DialogResult = DialogResult;
            tp.Call(this.Options.OnClosing, this.Options.Context, this);
            if (this.OldParent)
                this.OldParent.appendChild(this.elContent)
            this.Overlay.parentNode.removeChild(this.Overlay);
            this.Overlay = null;
            tp.Call(this.Options.OnClosed, this.Options.Context, this);

            // The promise resolves to either a this instance or a javascript object passed to the Close() method.
            let Result = tp.IsValid(this.DialogResult) ? this.DialogResult : this;
            tp.Call(this.ResolveFunc, null, Result);

            // remove from boxes
            tp.ListRemove(app.ModalDialog.Boxes, this);
        }
    }

};

/**
 * Async function. Displays a modal dialog box. The dialog displays a specified content element. <br />
 * CAUTION: The width and height of the passed-in content element determines the dimensions of the dialog box.
 * @param {string|HTMLElement} elContent - A selector or a HTMLElement. The content element.
 * @param {app.ModalDialogOptions} Options - Options object. See the app.ModalDialogOptions class.
 * @returns {Promise} Returns a promise which resolves when the dialog closes for any reason. <br /> 
 * That promise resolves to either an app.InternalModalDialogClass instance or a javascript object passed to the Close() method.
 */
app.ModalDialog = async function (elContent, Options) {

    // create the dialog
    let Box = new app.InternalModalDialogClass(elContent, Options); //new BoxClass(elContent, Options);
    app.ModalDialog.Boxes.push(Box);

    // return a promise
    return new Promise((Resolve, Reject) => {
        Box.ResolveFunc = Resolve;
    });
};

/** Internal property. A stack where all opened dialogs are placed.  */
app.ModalDialog.Boxes = [];
/**
 * Closes the (top) dialog with success
 * @param {any} DialogResult The result to return on success.
 */
app.ModalDialog.Close = function (DialogResult = null) {
    if (this.Boxes.length > 0) {
        let Box = this.Boxes[this.Boxes.length - 1];
        Box.Close(DialogResult);
    }
};
/** 
 *  Closes the (top) dialog with cancel. The dialog Cancelled property is set to true. 
 */
app.ModalDialog.Cancel = function () {
    if (this.Boxes.length > 0) {
        let Box = this.Boxes[this.Boxes.length - 1];
        Box.Cancel();
    }
};

 
 
/** Handles controls and operations of the header */
app.Header = class {
    constructor(Options) {
        let i, ln, List;
        if (app.Header.Instance !== null)
            tp.Throw('app.Header is a single-instance class');

        app.Header.Instance = this;
        this.Options = Options;

        // handles the language selection drop-down
        app.DropDownHandler('#btnLanguage', '#cboLanguage', 'tp-Visible');



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
        let display = tp.ComputedStyle(el).display;
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


 
