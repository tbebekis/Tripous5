/** Indicates how to render an item-bar
 @class
 @enum {number}
 */
tp.ItemBarRenderMode = {
    /** none  */
    None: 0,
    /** Items do not exceed the item-bar width.  */
    Normal: 1,
    /** Items exceed the item-bar width. Display a toogle button and render items in a drop-down */
    Toggle: 2,
    /** Items exceed the item-bar width. Display a Next and Prev button in the item-bar edges. */
    NextPrev: 4,
};
Object.freeze(tp.TabRenderMode);


tp.ItemBar = class extends tp.tpElement {

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

    /* properties */
    /**
     Gets or sets the render mode. One of the {@link tp.ItemBarRenderMode} constants
     @type {number}
     @private
     */
    get RenderMode() {
        return this.fRenderMode;
    }
    set RenderMode(v) {

        if (tp.IsNumber(v) && (v !== this.fRenderMode) && (this.ChangingMode === false)) {
            this.ChangingMode = true;
            try {
                this.fRenderMode = v;

                // hide the toggle dropdown if visible
                if (this.ToggleDropDownBox && this.ToggleDropDownBox.IsOpen)
                    this.ToggleDropDownBox.Close();

                // set all visible
                let List = this.GetItemElementList();
                List.forEach(item => item.style.display = '' );
 
                switch (this.RenderMode) {
                    case tp.ItemBarRenderMode.Normal:
                        this.ToggleContainer.style.display = 'none';
                        this.btnToLeft.style.display = 'none';
                        this.btnToRight.style.display = 'none';

                        this.ItemContainer.style.display = '';
                        break;

                    case tp.ItemBarRenderMode.Toggle:
                        this.ItemContainer.style.display = 'none';
                        this.btnToLeft.style.display = 'none';
                        this.btnToRight.style.display = 'none';

                        this.ToggleContainer.style.display = '';
                        let Index = this.SelectedIndex;
                        this.ToggleTextZone.innerHTML = this.GetItemTextAt(Index); 
                        break;

                    case tp.ItemBarRenderMode.NextPrev:
                        this.ToggleContainer.style.display = 'none';                        

                        this.btnToLeft.style.display = '';
                        this.btnToRight.style.display = '';
                        this.ItemContainer.style.display = '';

                        this.Arrange();
                        break;
                }

                this.OnRenderModeChanged();

            } finally {
                this.ChangingMode = false
            }
        }
    }
    /**
    Gets or sets the selected page index
    @type {number}
    */
    get SelectedIndex() {
        let List = this.GetItemElementList();
        for (let i = 0, ln = List.length; i < ln; i++) {
            if (tp.HasClass(List[i], tp.Classes.Selected)) {
                return i;
            }
        }

        return -1;
    }
    set SelectedIndex(v) {
        let CurrentIndex = this.SelectedIndex;
        if (v !== CurrentIndex) {
            this.OnSelectedIndexChanging(CurrentIndex, v);
            this.SetSelectedIndex(v);
            this.OnSelectedIndexChanged(v);
        }
    }


    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ItemBar';
        this.fDefaultCssClasses = tp.Classes.ItemBar;
    }
    /**
   Initializes fields and properties just before applying the create params.
   @protected
   @override
   */
    InitializeFields() {
        super.InitializeFields();
        this.CreateControls();
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();

        this.HookEvent(tp.Events.Click);

        this.IsElementResizeListener = true;
        this.RenderMode = tp.ItemBarRenderMode.Normal;
    }
    /**
    Event trigger
    @protected
    @param {object} ResizeInfo An object of type <code>{Width: boolean, Height: boolean}</code>
    */
    OnResized(ResizeInfo) {
        super.OnResized(ResizeInfo);

        let List;
        let ItemTotalWidth = 0;
        let HiddenList = [];
        let Style = tp.GetComputedStyle(this.ItemContainer);
        let Gap = parseInt(Style.gap, 10);

        if (this.RenderMode === tp.ItemBarRenderMode.Toggle)
            this.ItemContainer.style.display = '';

        let ContainerWidth = this.Handle.offsetWidth;
        List = this.GetItemElementList();

        let IsHidden;
        List.forEach((item) => {
            IsHidden = item.style.display === 'none';
            if (IsHidden)
                item.style.display = ''

            ItemTotalWidth += (item.offsetWidth + Gap);

            if (IsHidden)
                item.style.display = 'none'
        });

        if (this.RenderMode === tp.ItemBarRenderMode.Toggle)
            this.ItemContainer.style.display = 'none';

        if (this.RenderMode === tp.ItemBarRenderMode.Toggle || this.RenderMode === tp.ItemBarRenderMode.NextPrev) {

        }

        switch (this.RenderMode) {

            case tp.ItemBarRenderMode.Toggle:
            case tp.ItemBarRenderMode.NextPrev:
                if (ContainerWidth > ItemTotalWidth) {
                    this.RenderMode = tp.ItemBarRenderMode.Normal;
                }
                break;

            case tp.ItemBarRenderMode.Normal:
                if (ItemTotalWidth > ContainerWidth) {
                    this.RenderMode = this.ResponsiveMode;
                }
                break;

        }

 
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The Event object
    */
    OnAnyDOMEvent(e) {
        let i, ln, Index, List, EventArgs;
        let Type = tp.Events.ToTripous(e.type);

        if (tp.Events.Click === Type) {
            if (e.target instanceof HTMLElement) {
                if (tp.ContainsElement(this.ItemContainer, e.target)) {
                    List = this.GetItemElementList();
                    for (i = 0, ln = List.length; i < ln; i++) {
                        if (tp.ContainsElement(List[i], e.target)) {
                            this.SelectedIndex = i;
                            EventArgs = new tp.EventArgs(Type, List[i], e);
                            this.OnItemClicked(EventArgs);
                            break;
                        }
                    }
                } else if (this.ToggleContainer && tp.ContainsElement(this.ToggleButton, e.target)) {
                    this.ToggleClicked();
                } else if (this.ToggleItemList && tp.ContainsElement(this.ToggleItemList, e.target)) {
                    List = tp.ChildHTMLElements(this.ToggleItemList);
                    Index = List.indexOf(e.target);
                    if (Index !== -1) {
                        this.SelectedIndex = Index;
                        this.ToggleClicked();

                        List = this.GetItemElementList();
                        EventArgs = new tp.EventArgs(Type, List[Index], e);
                        this.OnItemClicked(EventArgs);
                    }
                }
                else if (tp.ContainsEventTarget(this.btnToLeft, e.target)) {
                    if (this.CanHideNext())
                        this.HideNext();
                }
                else if (tp.ContainsEventTarget(this.btnToRight, e.target)) {
                    if (this.CanShowNext())
                        this.ShowNext();
                }
            }

        }

        super.OnAnyDOMEvent(e);
    }

    /* private */
    /**
     * Sets the selected item by index.
     * @private
     * @param {number} Index The index of the item to set as selected.
     */
    SetSelectedIndex(Index) {

        var elSelectedItem = null;     // HTMLElement;
        var i, ln;

        // un-select items
        let List = this.GetItemElementList();

        for (i = 0, ln = List.length; i < ln; i++) {
            if (Index === i) {
                elSelectedItem = List[i];
            }
            tp.RemoveClass(List[i], tp.Classes.Selected);
        } 

        // selected
        if (elSelectedItem) {
            tp.AddClass(elSelectedItem, tp.Classes.Selected);
 
            if (this.ToggleTextZone)
                this.ToggleTextZone.innerHTML = this.GetItemTextAt(Index);
        }

    } 
    /**
    Shows or hides the "toggle tab list", a drop-down list of tab titles which is used in responsive screen dimensions.
    @private
    */
    ToggleClicked() {
        if (!this.ToggleDropDownBox) {
            let el = this.Document.createElement('div');
            this.ToggleDropDownBox = new tp.DropDownBox(el, {
                Associate: this.ToggleButton,
                Owner: this,
                Height: 'auto',
                Width: 'auto'
            });

            this.ToggleItemList = this.ToggleDropDownBox.AddChild('div');
            tp.AddClass(this.ToggleItemList, tp.Classes.ToggleItemList);
            this.ToggleItemList.addEventListener('click', (e) => {
                this.OnAnyDOMEvent(e);
            });
        }

        this.ToggleDropDownBox.Toggle();

    }
    /**
    Called by the dropdown box to inform its owner about a stage change.
    @private
    @param {tp.DropDownBox} Sender The sender
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants.
    */
    OnDropDownBoxEvent(Sender, Stage) {
        let i, ln, List, ToggleItem;

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
                this.ToggleItemList.innerHTML = '';
                break;

            case tp.DropDownBoxStage.Opened:
                List = this.GetItemElementList();
                for (i = 0, ln = List.length; i < ln; i++) {
                    ToggleItem = this.Document.createElement('div');
                    this.ToggleItemList.appendChild(ToggleItem);
                    ToggleItem.innerHTML = this.GetItemTextAt(i);
                }
                break;

            case tp.DropDownBoxStage.Closing:
                break;

            case tp.DropDownBoxStage.Closed:
                break;
        }
    }
    /** Creates child controls of this instance. Called in construction sequence.
    * @private
    * */
    CreateControls() {
        let List = this.GetChildren();

        // remove any elements
        List.forEach((item) => { tp.Remove(item); });

        // toggle button and text zone
        let HtmlText =
`<div class="${tp.Classes.Toggle}">
    <div class="${tp.Classes.Btn}">
         <img class="${tp.Classes.Img}" src="${tp.tpWindow.ICON_ThreeLines}" />
    </div>
    <div class="${tp.Classes.Text}"></div>    
</div>`;

        this.ToggleContainer = tp.Append(this.Handle, HtmlText);
        this.ToggleButton = tp.Select(this.ToggleContainer, '.' + tp.Classes.Btn);
        this.ToggleTextZone = tp.Select(this.ToggleContainer, '.' + tp.Classes.Text);

        this.ToggleContainer.style.display = 'none';

        // button to-left
        HtmlText = `<div class="${tp.Classes.Prev}">◂</div>`;
        this.btnToLeft = tp.Append(this.Handle, HtmlText);

        this.btnToLeft.style.display = 'none';

        // item container
        HtmlText = `<div class="${tp.Classes.ItemList}"></div>`;
        this.ItemContainer = tp.Append(this.Handle, HtmlText);

        // button to-right
        HtmlText = `<div class="${tp.Classes.Next}">▸</div>`;
        this.btnToRight = tp.Append(this.Handle, HtmlText);

        this.btnToRight.style.display = 'none';

        // now add items to container
        List.forEach((item) => { tp.Append(this.ItemContainer, item); });
    }


    IsItemVisible(el) {
        let display = tp.GetComputedStyle(el).display;
        return !tp.IsSameText('none', display);
    }
    GetVisibleItemTotalWidth() {
        let Style = tp.GetComputedStyle(this.ItemContainer);
        let Gap = parseInt(Style.gap, 10);

        let List = this.GetItemElementList();
        let i, ln, el, R, Result = 0;
        let Items = List.slice().reverse();

        for (i = 0, ln = Items.length; i < ln; i++) {
            el = Items[i];
            if (!this.IsItemVisible(el))
                break;
 
            Result += (el.offsetWidth + Gap);
        }

        return Result;
    }
    CanHideNext() {
        let List = this.GetItemElementList();
        if (List.length > 2) {
            let el = List[List.length - 2];
            return this.IsItemVisible(el);
        }
        return false;
    }
    CanShowNext() {
        let List = this.GetItemElementList();
        if (List.length > 0) {
            let el = List[0];
            return !this.IsItemVisible(el);
        }
        return false;
    }
    HideNext() {
        let List = this.GetItemElementList();
        let i, ln, el;
        for (i = 0, ln = List.length; i < ln; i++) {
            el = List[i];
            if (this.IsItemVisible(el)) {
                el.style.display = 'none';
                break;
            }
        }
    }
    ShowNext() {
        let List = this.GetItemElementList();
        let i, ln, el, Items = List.slice().reverse();
        for (i = 0, ln = Items.length; i < ln; i++) {
            el = Items[i];
            if (!this.IsItemVisible(el)) {
                el.style.display = '';
                break;
            }
        }
    }
    Arrange() {
        let List = this.GetItemElementList();
        let i, ln, el, ContainerWidth, ItemTotalWidth;
        ContainerWidth = this.Handle.offsetWidth;

        // show all
        for (i = 0, ln = List.length; i < ln; i++) {
            el = List[i];
            el.style.display = '';
        }

        ItemTotalWidth = this.GetVisibleItemTotalWidth();
        while (ContainerWidth < ItemTotalWidth) {
            this.HideNext();
            ItemTotalWidth = this.GetVisibleItemTotalWidth();
        }
    }
    /** Returns the display text (caption) of an item found at a specified index.
     * @param {number} Index The item index
     * @returns {string} Returns the display text (caption) of an item found at a specified index.
     */
    GetItemTextAt(Index) {       
        let List = this.GetItemElementList();

        if (Index >= 0 && Index <= List.length - 1) {
            let el = List[Index];

            let o = tp.GetObject(el);
            if (o && 'Text' in o) {
                return o.Text;
            }

            if (el.hasChildNodes()) {
                let NodeList = el.childNodes;
                if (NodeList.length === 1 && tp.IsText(NodeList[0])) {
                    return el.innerHTML;
                }
            }
        }

        return '';
    }

    /* public */
    /** Returns an array with the Item HTML Elements. The array may be empty.
     * @returns {HTMLElement[]} Returns an array with the Item HTML Elements. The array may be empty.
     * */
    GetItemElementList() {
        return tp.IsElement(this.ItemContainer) ? tp.ChildHTMLElements(this.ItemContainer) : [];
    }


    /* event triggers */
    /**
    Event trigger
    @param {number} CurrentIndex The current index.
    @param {number} NewIndex The new index.
    */
    OnSelectedIndexChanging(CurrentIndex, NewIndex) {
        let Args = new tp.IndexChangeEventArgs(CurrentIndex, NewIndex);
        this.Trigger('SelectedIndexChanging', Args);
    }
    /**
    Event trigger
    @param {number} CurrentIndex The current index.
    */
    OnSelectedIndexChanged(CurrentIndex) {
        let Args = new tp.IndexChangeEventArgs(CurrentIndex);
        this.Trigger('SelectedIndexChanged', Args);
    }
    /**
    Event trigger. <br />
    NOTE: This event is triggered because of a click on an item and just after the "SelectedIndexChanged" event is triggered.
    @param {tp.EventArgs} Args Event arguments. Sender is the clicked HTMElement and e the DOM Event.
    */
    OnItemClicked(Args) {
        this.Trigger('ItemClicked', Args);
        alert('ItemClicked ');
    }
    /**
    Event trigger
    */
    OnRenderModeChanged() {
        let Args = new tp.EventArgs(null);
        this.Trigger('RenderModeChanged', Args);
        return Args;
    }

};

tp.ItemBar.prototype.ChangingMode = false;
/** Private field.
 @type {number}
 */
tp.ItemBar.prototype.fRenderMode = tp.ItemBarRenderMode.None;

/** Private field.
 @type {number}
 */
tp.ItemBar.prototype.ResponsiveMode = tp.ItemBarRenderMode.NextPrev;
 
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.ToggleContainer = null;
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.ToggleButton = null;
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.ToggleTextZone = null;
/** Private field.
 @type {tp.DropDownBox}
 */
tp.ItemBar.prototype.ToggleDropDownBox = null;
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.ToggleItemList = null;

/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.btnToLeft = null;
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.btnToRight = null;
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.ItemContainer = null;
/** Private field.
 @type {HTMLElement}
 */
tp.ItemBar.prototype.LastItem = null;

