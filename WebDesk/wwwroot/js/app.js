var app = app || {};
app.Config = app.Config || {};
app.Config.CurrencySymbol = '€';

app.Resources = app.Resources || {};




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
 * Returns the value of the data-XXXX custom attribute of a specified element as a javascript object, or an empty object.
 * @param {string|Element} el The element to operate on.
 * @param {string} [DataName='setup'] The name of the data-XXXX attribute (without the data- prefix)
 * @returns {object} Returns the value of the data-setup custom attribute of a specified element as a javascript object, or an empty object.
 */
app.GetDataObject = function (el, DataName = 'setup') {
    let A = tp.Data(el, DataName);
    if (!tp.IsBlank(A)) {
        A = eval("(" + A + ")");
    }
    else {
        A = {};
    }

    return A;
};

/** Creates and returns an overlay div */
app.CreateOverlay = function () {
    let Overlay = tp.Div(tp.Doc.body);
    Overlay.id = tp.SafeId('Overlay');
    tp.AddClass(Overlay, 'overlay');
    tp.BringToFront(Overlay);
    return Overlay;
};

/**
 * Handles a drop-down operation by toggling the visibility of the drop-down list.
 * @param {string|Element} Button Selector or Element. The element which displays the drop-down when is clicked
 * @param {string|Element} List Selector or Element. The drop-down list
 * @param {string} CssClass The css class to toggle to the drop-down list
 */
app.DropDownHandler = function (Button, List, CssClass = 'tp-Visible') {
    Button = tp(Button);
    List = tp(List);

    Button.addEventListener('click', (ev) => {
        List.classList.toggle(CssClass);
    });

    window.addEventListener('click', (ev) => {
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
        if (tp.ContainsText('<div', ElementOrSelectorOrHtmlText, true)) {
            // create a temp div
            let div = tp.Div(tp.Doc.body);
            div.innerHTML = ElementOrSelectorOrHtmlText;
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

/**
 * A Yes-No dialog box
 * @param {string} Title The dialog title
 * @param {string} MessageText The message text to display
 * @returns {Promise} Returns true/false as a Promise
 */
app.YesNoBox = async function (Title, MessageText) {

    let Result = false;

    let elContainer = tp('.yes-no-box-content-template');
    elContainer = elContainer.cloneNode(true);
    elContainer.className = 'yes-no-box-content';

    let elTextArea = tp.Select(elContainer, '.message-text-area');
    elTextArea.innerHTML = MessageText;

    let btnYes = tp.Select(elContainer, '.btn-yes');
    btnYes.addEventListener('click', (e) => {
        e.stopPropagation();
        Result = true;
        app.ModalDialog.Close(Result);
    });

    let btnNo = tp.Select(elContainer, '.btn-no');
    btnNo.addEventListener('click', (e) => {
        e.stopPropagation();
        app.ModalDialog.Close(Result);
    });

    let Options = {
        Text: Title
    };

    // return the promise
    let P = await app.ModalDialog(elContainer, Options);
    return P;
};


/** Implementation of Tripous Notification Box.
 * A notification box is displayed by calling tp.InfoNote(Message), tp.ErrorNote(Message), etc.
 * */
app.NotificationBox = class extends tp.tpElement {
    /**
     * Constructor
     * @param {string} Message The message
     * @param {tp.NotificationType} Type The type
     */
    constructor(Message, Type) {

        let el = tp('.notification-box-template');
        el = el.cloneNode(true);
        let S = el.className;
        S = S.replace('-template', '');
        el.className = S;

        // append to the body
        tp.Doc.body.appendChild(el);

        // call the super constructor
        super(el, null);

        this.Type = Type;
        if (this.Type === tp.NotificationType.Error)
            this.AddClass('error');
        else if (this.Type === tp.NotificationType.Success)
            this.AddClass('success');
        else
            this.AddClass('info');

        this.btnClose = tp.Select(this.Handle, '.close');
        this.divCaption = tp.Select(this.Handle, '.caption');
        this.divCaption.tabIndex = 0;

        el = tp.Select(this.Handle, '.left');
        el.tabIndex = 0;

        el = tp.Select(this.Handle, '.right');
        el.tabIndex = 0;

        let sType = tp.EnumNameOf(tp.NotificationType, Type);
        let Title = tp.NotificationBoxSetup.Titles[sType];

        el = tp.Select(this.divCaption, '.title');
        el.innerHTML = Title;

        el = tp.Select(this.Handle, '.right .text');
        el.innerHTML = Message;

        // style
        let CssStyle = {
            'opacity': '0',
            'transition': tp.Format('opacity {0}s ease-in 0s', tp.NotificationBoxSetup.DurationSecs),  // google : css transition fade out  
        };

        if (tp.Viewport.IsXSmall) {
            CssStyle['left'] = '2px';
            CssStyle['right'] = '2px';
        } else {
            CssStyle['width'] = tp.px(tp.NotificationBoxSetup.Width);
        }
        this.SetStyle(CssStyle);

        tp.NotificationBoxes.Place(this.Handle);

        // add to the notification boxes
        tp.NotificationBoxes.Add(this.Handle);

        // set a close timeout
        setTimeout(function (box) {
            if (!box.Clicked) {
                box.Dispose();
            }
        }, tp.NotificationBoxSetup.DurationSecs * 1000, this);

        // Dragger setup
        this.Dragger = new tp.Dragger(tp.DraggerMode.Both, this.Handle, this.divCaption);
        this.Dragger.On(tp.Events.DragStart, this.OnAnyEvent, this);

        // events
        this.HookEvent(tp.Events.Click);
        this.HookEvent(tp.Events.KeyDown);
        this.HookEvent(tp.Events.MouseDown);

        this.BringToFront();
    }

    Type = tp.NotificationType.Information;
    Clicked = false;
    Dragger = null;       // tp.Dragger = null;

    btnClose = null;      // HTMLImageElement    
    divCaption = null;    // HTMLElement; 

    /**
     * Initializes the 'static' and 'read-only' class fields
     * @protected 
     * @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'app.NotificationBox';
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event}  e The Event object
    */
    OnAnyDOMEvent(e) {
        let Type = tp.Events.ToTripous(e.type);

        if (tp.IsSameText(tp.Events.Click, Type)) {
            if (this.Clicked !== true) {
                this.Clicked = true;
                this.Handle.style.transition = 'none';
                this.Handle.style.opacity = '1';
            }

            if (e.target === this.btnClose) {
                this.Dispose();
            }
        } else if (e instanceof KeyboardEvent) {
            if (tp.IsSameText(tp.Events.KeyDown, Type)) {
                var Key = e.keyCode;
                if (tp.Keys.Escape === Key) {
                    this.Dispose();
                }
            }
        } else if (tp.IsSameText(tp.Events.MouseDown, Type)) {
            this.BringToFront();
        }

        if (!this.IsDisposed)
            super.OnAnyDOMEvent(e);
    }
    /**
    Handles any event. Even DOM events are send in this method.
    @protected
    @override
    @param {tp.EventArgs} Args The tp.EventArgs object
    */
    OnAnyEvent(Args) {
        if (tp.IsSameText(tp.Events.DragStart, Args.EventName)) {
            if (this.Dragger) {
                tp.NotificationBoxes.Remove(this.Handle);

                let X = this.Handle.offsetLeft - 5;
                let Y = this.Handle.offsetTop - 5;

                requestAnimationFrame(() => {
                    //this.Position = 'absolute';
                    this.X = X;
                    this.Y = Y;
                });
            }
        }
    }
    /**        
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    @protected
    @override
    */
    Dispose() {
        tp.NotificationBoxes.Remove(this.Handle);
        super.Dispose();
    }

};

tp.NotificationBoxSetup.Titles = {
    Information: 'Information',
    Warning: 'Warning',
    Error: 'Error',
    Success: 'Success'
};

/** Replace the above tp.NotifyFunc <br />
Displays a notification message to the user.
@param {string} Message The notification message
@param {NotificationType} Type The type of notification, i.e. Warning, Error, etc
 */
tp.NotifyFunc = (Message, Type) => {
    let Box = new app.NotificationBox(Message, Type);
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

app.MainMenu = class {

    constructor() {
        const CssClass = 'tp-Visible';
        let i, ln;

        this.elMainMenu = tp('.main-menu');
        this.elMenuBar = tp.Select(this.elMainMenu, '.menu-bar');

        this.BarItems = tp.ChildHTMLElements(this.elMenuBar);

        this.BarItems.forEach((elBarItem) => {
            let elText, elList;

            elText = tp.Select(elBarItem, '.tp-Text');
            elList = tp.Select(elBarItem, '.tp-List');

            elText.addEventListener('click', (ev) => {
                elList.classList.toggle(CssClass);
            });

            window.addEventListener('click', (ev) => {
                if (elList.classList.contains(CssClass) && !tp.ContainsEventTarget(elText, ev.target)) {
                    elList.classList.remove(CssClass);
                }
            });

        });

    }

    elMainMenu = null;
    elMenuBar = null;
    BarItems = [];
};