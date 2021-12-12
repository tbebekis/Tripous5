//#region tp.TabControl
/**
Represents a tab page, that is a child to a TabControl
*/
tp.TabPage = class extends tp.tpElement {

    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /** Gets or sets the title (caption) of the tab 
     * @type {string}
     */
    get Title() {
        return this.Tab.innerHTML;
    }
    set Title(v) {
        this.Tab.innerHTML = v;
    }

};
/**
The tab is the caption container
@type {HTMLElement}
*/
tp.TabPage.prototype.Tab = null;



/**
TabControl <br />
Example markup <br />
<pre>
    <div id="TabControl" class="tp-TabControl" data-setup="{ SelectedIndex: 0 }">
        <div><div>Page 1</div><div>Page 2</div><div>Page 3</div></div>
        <div>
            <div>Content 1</div>
            <div>Content 2</div>
            <div>Content 3</div>
        </div>
    </div>
</pre>
@implements {tp.ISelectedIndex}
*/
tp.TabControl = class extends tp.tpElement {
    /**
    Constructor
    Example markup <br />
    <pre>
        <div id="TabControl" class="tp-TabControl" data-setup="{ SelectedIndex: 0 }">
            <div><div>Page 1</div><div>Page 2</div><div>Page 3</div></div>
            <div>
                <div>Content 1</div>
                <div>Content 2</div>
                <div>Content 3</div>
            </div>
        </div>
</pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }


    /* overridables */
    /**
     * Sets the selected page by index.
     * @protected
     * @param {number} Index The index of the page to set as selected.
     */
    SetSelectedIndex(Index) {

        var elTab = null;     // HTMLElement;
        var i, ln;

        // un-select tab buttons
        let TabElementList = this.GetTabElementList();

        for (i = 0, ln = TabElementList.length; i < ln; i++) {
            if (Index === i) {
                elTab = TabElementList[i];
            }
            tp.RemoveClass(TabElementList[i], tp.Classes.Selected);
        }

        // un-select tab pages
        let PageElementList = this.GetPageElementList();

        for (i = 0, ln = PageElementList.length; i < ln; i++) {
            tp.StyleProp(PageElementList[i], 'display', 'none');
        }

        // selected
        if (elTab && elTab.TabPage) {
            tp.AddClass(elTab, tp.Classes.Selected);
            
            let elPage = elTab.TabPage.Handle;
            elPage.style.display = '';

            if (this.fToggleZone)
                this.fToggleZoneTitle.innerHTML = elTab.innerHTML;
        }

    }
    /**
     * Creates and returns a new page. <br />
     * NOTE: It does NOT add the page to the control.
     * @protected
     * @param {string} [Title] Optional.The title text of the new page.
     * @returns {tp.TabPage} Returns the new page.
     */
    CreatePage(Title) {
        this.OnPageCreating();

        let elTab = this.Document.createElement('div'); // HTMLElement
        let elPage = this.Document.createElement('div'); // HTMLElement

        // tab title
        if (tp.IsString(Title) && !tp.IsBlank(Title))
            elTab.innerHTML = Title;

        let CP = {
            Tab: elTab
        };

        elTab.TabPage = new tp.TabPage(elPage, CP);

        let Result = elTab.TabPage;

        this.OnPageCreated(Result);

        return Result;
    }
    /**
    Shows or hides the "toggle tab list", a drop-down list of tab titles which is used in responsive screen dimensions.
    @protected
    */
    ToggleClicked() {

        if (!this.fToggleDropDownBox) {
            let el = this.Document.createElement('div');
            this.fToggleDropDownBox = new tp.DropDownBox(el, {
                Associate: this.fToggleButton,
                Owner: this,
                Height: 'auto',
                Width: 'auto'
            });

            this.fToggleTabList = this.fToggleDropDownBox.AddChild('div');
            tp.AddClass(this.fToggleTabList, 'tp-TabToggleDropDownList');
            this.fToggleTabList.addEventListener('click', (e) => {
                this.OnAnyDOMEvent(e);
            });
 
        }

        this.fToggleDropDownBox.Toggle();

    }
    /**
    Called by the dropdown box to inform its owner about a stage change.
    @param {tp.DropDownBox} Sender The sender
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants.
    */
    OnDropDownBoxEvent(Sender, Stage) {
        let i, ln, List, ToggleTabItem;

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
                this.fToggleTabList.innerHTML = '';
                break;

            case tp.DropDownBoxStage.Opened:
                List = this.GetTabElementList();
                for (i = 0, ln = List.length; i < ln; i++) {
                    ToggleTabItem = this.Document.createElement('div');
                    this.fToggleTabList.appendChild(ToggleTabItem);
                    ToggleTabItem.innerHTML = this.GetTitleAt(i);
                } 
                break;

            case tp.DropDownBoxStage.Closing:
                break;

            case tp.DropDownBoxStage.Closed:
                break;
        }



    }

    /**
    Gets or sets the selected page index
    @type {number}
    */
    get SelectedIndex() {
        let TabElementList = this.GetTabElementList();
        for (let i = 0, ln = TabElementList.length; i < ln; i++) {
            if (tp.HasClass(TabElementList[i], tp.Classes.Selected)) {
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
    /**
    Gets or sets the selected {@link tp.TabPage} page.
    @type {tp.TabPage}
    */
    get SelectedPage() {
        let Result = null;
        var Index = this.SelectedIndex;
        if (Index >= 0) {
            let PageElementList = this.GetPageElementList();
            let elPage = PageElementList[Index];
            let Result = tp.GetObject(elPage);
        }
        return Result;
    }
    set SelectedPage(v) {
        if (v instanceof tp.TabPage) {
            let PageElementList = this.GetPageElementList();
            let Index = PageElementList.indexOf(v.Handle);
            if (Index >= 0)
                this.SetSelectedIndex(Index);
        }
    }

    /* overridables */
 
    CreateControls() {
        let List = this.GetChildren();

        if (List.length === 2) {
            this.TabContainer = List[0];
            this.PageContainer = List[1];
        }
        else if (List.length === 0) {
            this.TabContainer = this.AddChild('div');
            this.PageContainer = this.AddChild('div');
        }
        else {
            tp.Throw('Wrong TabControl structure. Should be empty or have 2 child DIVs.');
        }

        // add classes to containers
        tp.AddClass(this.TabContainer, tp.Classes.Bar);
        tp.AddClass(this.PageContainer, tp.Classes.List);

        let TabElementList = this.GetTabElementList();
        let PageElementList = this.GetPageElementList();

        if (TabElementList.length !== PageElementList.length)
            tp.Throw('Tabs and Pages should be equal in number.');


        for (let i = 0, ln = PageElementList.length; i < ln; i++) {
            let elTab = TabElementList[i];
            let elPage = PageElementList[i];
            let TabPage;

            let CP = {
                Tab: elTab
            };

            TabPage = new tp.TabPage(elPage, CP);
            elTab.TabPage = TabPage;
        }

        // toggle zone and button
        let HtmlText =
`<div class="${tp.Classes.Toggle}">
    <div class="${tp.Classes.Btn}">
         <img class="${tp.Classes.Img}" src="${tp.tpWindow.ICON_ThreeLines}" />
    </div>
    <div class="${tp.Classes.Text}"></div>    
</div>`;

        this.fToggleZone = tp.Prepend(this.Handle, HtmlText);
        this.fToggleButton = tp.Select(this.fToggleZone, '.' + tp.Classes.Btn);
        this.fToggleZoneTitle = tp.Select(this.fToggleZone, '.' + tp.Classes.Text);


        this.fToggleZone.style.display = 'none';
    }
    SetResponsiveMode(Flag) {
 
        // hide the toggle dropdown if visible
        if (this.fToggleDropDownBox && this.fToggleDropDownBox.IsOpen)
            this.fToggleDropDownBox.Close();

        if (tp.IsEmpty(this.InResponsiveMode) || (tp.IsBoolean(this.InResponsiveMode) && this.InResponsiveMode !== Flag)) {
 
            // if (this.Handle.offsetWidth <= tp.ScreenWidthsMax.Small)
            if (Flag) {

                this.StopIntersectionObserver();

                this.fToggleZone.style.display = '';
                this.TabContainer.style.display = 'none';

                let Index = this.SelectedIndex;
                if (Index !== -1) {
                    this.fToggleZoneTitle.innerHTML = this.GetTitleAt(Index);
                }
            }
            else {
                this.fToggleZone.style.display = 'none';
                this.TabContainer.style.display = '';

                this.StartIntersectionObserver();
            }

            this.InResponsiveMode = Flag;
        }

    }
    ResizeHandler() {

    }

 
 

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.TabControl';
        this.fDefaultCssClasses = tp.Classes.TabControl;
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
    Notification 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();
 

    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();

        this.HookEvent(tp.Events.Click);
        //this.IsScreenResizeListener = true;
        //this.ResizeHandler();

        this.CreateIntersectionObserver();
        this.StartIntersectionObserver();
    }
    /**
    Event trigger
    @param {object} ResizeInfo An object of type <code>{Width: boolean, Height: boolean}</code>
    */
    OnResized(ResizeInfo) {
        super.OnResized(ResizeInfo);
        //this.ResizeHandler();
    }
    /**
    Called becacuse the size of its parent is changed
    */
    ParentSizeChanged() {
        super.ParentSizeChanged();
        //this.ResizeHandler();
    }

    /**
     Notification sent by tp.Viewport when the screen (viewport) size changes. <br />
     This method is called only if this.IsScreenResizeListener is true.
     @protected
     @override
     @param {boolean} ScreenModeFlag - Is true when the screen mode (XSmall, Small, Medium, Large) is changed as well.
     */
    OnScreenSizeChanged(ScreenModeFlag) {
        super.OnScreenSizeChanged(ScreenModeFlag);
        if (ScreenModeFlag === true)
            this.ResizeHandler();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The Event object
    */
    OnAnyDOMEvent(e) {

        let Type = tp.Events.ToTripous(e.type);

        if (tp.Events.Click === Type) {
            if (e.target instanceof HTMLElement) {

                if (tp.ContainsElement(this.TabContainer, e.target)) {
                    let TabElementList = this.GetTabElementList();
                    for (let i = 0, ln = TabElementList.length; i < ln; i++) {
                        if (tp.ContainsElement(TabElementList[i], e.target)) {
                            this.SelectedIndex = i;
                            break;
                        }
                    }
                } else if (this.fToggleZone && tp.ContainsElement(this.fToggleButton, e.target)) {
                    this.ToggleClicked();
                } else if (this.fToggleTabList && tp.ContainsElement(this.fToggleTabList, e.target)) {
                    let List = tp.ChildHTMLElements(this.fToggleTabList);
                    let Index = List.indexOf(e.target);
                    if (Index !== -1) {
                        this.SelectedIndex = Index;
                        this.ToggleClicked();
                    }
                }
            }

        }

        super.OnAnyDOMEvent(e);
    }

    /* public */
    /** Returns an array with the Tab HTML Elements (tab captions). The array may be empty.
     * @returns {HTMLElement[]} Returns an array with the Tab HTML Elements (tab captions). The array may be empty.
     * */
    GetTabElementList() {
        return tp.IsElement(this.TabContainer) ? tp.ChildHTMLElements(this.TabContainer) : [];
    }
    /** Returns an array with the Page HTML Elements (tab pages). The array may be empty.
     * @returns {HTMLElement[]} Returns an array with the Page HTML Elements (tab pages). The array may be empty.
     * */
    GetPageElementList() {
        return tp.IsElement(this.PageContainer) ? tp.ChildHTMLElements(this.PageContainer) : [];
    }
    /** Returns an array of the contained {@link tp.TabPage} pages
     * @returns {tp.TabPage[]} Returns an array of the contained {@link tp.TabPage} pages
     */
    GetPageList() {
        let Result = [];
        let PageElementList = this.GetPageElementList();
        PageElementList.forEach((elPage) => {
            let Page = tp.GetObject(elPage);
            Result.push(Page);
        });

        return Result;
    }

    /**
    Adds and returns a child. 
    @param {string} [Title] Optional. The title text of the page.
    @returns {tp.TabPage} Returns the newly added tp.TabPage child
    */
    AddPage(Title) {
        let PageElementList = this.GetPageElementList();
        return this.InsertPage(PageElementList.length, Title);
    }
    /**
    Inserts a child at a specified index and returns the child.  
    @param {number} Index The index to use.
    @param {string} [Title] Optional. The title text of the page.
    @returns {tp.TabPage} Returns the newly added tp.TabPage child
    */
    InsertPage(Index, Title) {
        if (this.Handle) {
            this.StopIntersectionObserver();

            let PageElementList = this.GetPageElementList();

            let Page = this.CreatePage(Title);

            if (PageElementList.length === 0 || Index < 0 || Index >= PageElementList.length) {
                Index = PageElementList.length === 0 ? 0 : PageElementList.length;

                this.TabContainer.appendChild(Page.Tab);
                this.PageContainer.appendChild(Page.Handle);

            } else {
                this.TabContainer.insertBefore(Page.Tab, this.TabContainer.children[Index]);
                this.PageContainer.insertBefore(Page.Handle, this.PageContainer.children[Index]);
            }

            this.SetSelectedIndex(Index);

            this.StartIntersectionObserver();
            

            return Page;
        }

        return null;
    }

    /**
    Returns a tab page object found at a specified index.
    @param {number} Index The index to use.
    @returns {tp.TabPage}  Returns a tab page object found at a specified index.
    */
    PageAt(Index) {
        let Result = null;
        let List = this.GetPageList();
        if (Index >= 0 && Index <= List.length - 1)
            Result = List[Index];
        return Result;
    }
    /**
    Returns the title text of a child found at a specified index.
    @param {number} Index The index to use.
    @return {string} Returns the title text of a child found at a specified index.
    */
    GetTitleAt(Index) {
        let Page = this.PageAt(Index);
        return Page.Tab.innerHTML;
    }
    /**
    Sets the title text of a child found at a specified index.
    @param {number} Index The index to use.
    @param {string} Text The title text.
    */
    SetTitleAt(Index, Text) {
        let Page = this.PageAt(Index);
        Page.Tab.innerHTML = Text;
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
    Event trigger
    @returns {tp.CreateChildEventArgs} Returns the tp.CreateChildEventArgs event Args.
    */
    OnPageCreating() {
        let Args = new tp.CreateChildEventArgs(null);
        this.Trigger('PageCreating', Args);
        return Args;
    }
    /**
    Event trigger
    @param {tp.TabPage} Page The tp.TabPage created child.
    */
    OnPageCreated(Page) {
        let Args = new tp.CreateChildEventArgs(null);
        Args.Child = Page;
        this.Trigger('PageCreated', Args);
    }


    CreateIntersectionObserver() {
        let Options = {
            // The root to use for intersection.
            // If not provided, use the top-level document’s viewport.
            root: this.TabContainer,
            // Same as margin, can be 1, 2, 3 or 4 components, possibly negative lengths.
            // If an explicit root element is specified, components may be percentages of the
            // root element size.  If no explicit root element is specified, using a percentage
            // is an error.
            rootMargin: "0px",
            // Threshold(s) at which to trigger callback, specified as a ratio, or list of
            // ratios, of (visible area / total area) of the observed element (hence all
            // entries must be in the range [0, 1]).  Callback will be invoked when the visible
            // ratio of the observed element crosses a threshold in the list.
            threshold: 1.0 // []

        };

        let Func = (entries) => {
            if (tp.IsArray(entries) && entries.length > 0) {
                let Flag = entries.some((entry) => { return entry.intersectionRatio < 1.0; });
                if (Flag === true) {
                    log(entries);
                }
                this.SetResponsiveMode(Flag);
            }
        };

        this.IntersectionObserver = new IntersectionObserver(Func, Options);
    }
    StopIntersectionObserver() {
        if (this.IntersectionObserver && this.LastTab) {
            //this.InResponsiveMode.disconnect();
            this.IntersectionObserver.unobserve(this.LastTab);
            this.IntersectionObserver.disconnect();
        }
    }
    StartIntersectionObserver() {
        if (this.IntersectionObserver) {
            this.StopIntersectionObserver();

            let List = this.GetTabElementList();

            if (List.length > 0) {
                this.LastTab = List[List.length - 1];
                if (tp.IsHTMLElement(this.LastTab))
                    this.IntersectionObserver.observe(this.LastTab);
            }
        }

    }
};

/** Private field.
 @type {boolean}
 */
tp.TabControl.prototype.InResponsiveMode;
/** Private field.
 @type {IntersectionObserver}
 */
tp.TabControl.prototype.IntersectionObserver = null;

/** Private field.
 @type {HTMLElement}
 */
tp.TabControl.prototype.TabContainer = null;
/** Private field.
 @type {HTMLElement}
 */
tp.TabControl.prototype.PageContainer = null;
 

/** The zone displayed when in responsive mode
 * @private
 @type {HTMLElement}
 */
tp.TabControl.prototype.fToggleZone = null;
/** The three lines toggle button 
 * @private
 @type {HTMLElement}
 */
tp.TabControl.prototype.fToggleButton = null; 
/** The text of the selected tab. Displayed when in responsive mode
 * @private
 @type {HTMLElement}
 */
tp.TabControl.prototype.fToggleZoneTitle = null;
/** The list of tab titles displayed when in responsive mode
* @type {HTMLElement}
* */
tp.TabControl.prototype.fToggleTabList = null;

/** Private field.
 @type {tp.DropDownBox}
 */
tp.TabControl.prototype.fToggleDropDownBox = null;

//#endregion