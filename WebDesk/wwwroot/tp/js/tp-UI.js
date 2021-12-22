 

/* eslint no-constant-condition: ["error", { "checkLoops": false }] */

//#region tp.Classes
tp.Classes = {

    NoBrowserAppearance: 'tp-NoBrowserAppearance',

    Normal: 'tp-Normal',
    XSmall: 'tp-XSmall',
    Small: 'tp-Small',
    Medium: 'tp-Medium',
    Large: 'tp-Large',

    /* states           ----------------------------------------------------------------- */
    Focused: 'tp-Focused',
    Selected: 'tp-Selected',
    Active: 'tp-Active',
    Inactive: 'tp-Inactive',
    Visible: 'tp-Visible',
    Expanded: 'tp-Expanded',
    Collapsed: 'tp-Collapsed',
    Grouped: 'tp-Grouped',
    Marked: 'tp-Marked',
    Disabled: 'tp-Disabled',
    Wrapped: 'tp-Wrapped',

    Horizontal: 'tp-Horizontal',
    Vertical: 'tp-Vertical',

    Selectable: 'tp-Selectable',
    UnSelectable: 'tp-UnSelectable',

    Hide: 'tp-Hide',
    HideSmall: 'tp-Hide-s',
    HideMid: 'tp-Hide-m',
    HideLarge: 'tp-Hide-l',

    /* general          ----------------------------------------------------------------- */
    Item: 'tp-Item',
    ItemList: 'tp-ItemList',
    Text: 'tp-Text',
    Ctrl: 'tp-Ctrl',
    CText: 'tp-CText',         // label text of a control
    Content: 'tp-Content',
    List: 'tp-List',
    ListItem: 'tp-ListItem',

    Edit: 'tp-Edit',
 

    Viewport: 'tp-Viewport',
    Container: 'tp-Container',

    Btn: 'tp-Btn',
    Ico: 'tp-Ico',
    Img: 'tp-Img',
    Separator: 'tp-Separator',

    Strip: 'tp-Strip',
    TabBar: 'tp-TabBar',
    Bar: 'tp-Bar',
    BarItem: 'tp-BarItem',
    Zone: 'tp-Zone',
    Toggle: 'tp-Toggle',
    ToggleItemList: 'tp-ToggleItemList',
 
    Handle: 'tp-Handle',

    Plus: 'tp-Plus',
    Minus: 'tp-Minus',

    Min: 'tp-Min',
    Max: 'tp-Max',

    From: 'tp-From',
    To: 'tp-To',

    Next: 'tp-Next',
    Prev: 'tp-Prev',

    First: 'tp-First',
    Last: 'tp-Last',

    Close: 'tp-Close',
    Open: 'tp-Open',

    WrapFirst: 'tp-WrapFirst',
    WrapLast: 'tp-WrapLast',

    Left: 'tp-Left',
    Right: 'tp-Right',
    Center: 'tp-Center',

    Top: 'tp-Top',
    Mid: 'tp-Mid',
    Bottom: 'tp-Bottom',   

    Main: 'tp-Main',
    Site: 'tp-Site',

    Header: 'tp-Header',
    Footer: 'tp-Footer',

    Group: 'tp-Group',

    SiteMenu: 'tp-SiteMenu',
    PageMenu: 'tp-PageMenu',
    MenuStrip: 'tp-MenuStrip',

    Title: 'tp-Title',
    Caption: 'tp-Caption',
    Logo: 'tp-Logo',

    FlexFill: 'tp-FlexFill',

    IcoLeft: 'tp-IcoLeft',
    IcoTop: 'tp-IcoTop',

    TextTop: 'tp-TextTop',

    None: 'tp-None',
    NoIco: 'tp-NoIco',
    NoText: 'tp-NoText',

    /* etc.             ----------------------------------------------------------------- */
    DropDown: 'tp-DropDown',
    Overlay: 'tp-Overlay',
    CenterInParent: 'tp-CenterInParent',

    Shadow: 'tp-Shadow',
    ClickBox: 'tp-ClickBox',

    TextCenter: 'tp-TextCenter',
    TextRight: 'tp-TextRight',

    RightAligner: 'tp-RightAligner',
    RightAligned: 'tp-RightAligned',

    RequiredMark: 'tp-RequiredMark',

    DragSource: 'tp-DragSource',
    DropTarget: 'tp-DropTarget',


    /* spinner          ----------------------------------------------------------------- */
    Spinner: 'tp-Spinner-Snake',  // Snake Wheel
    SpinnerContainer: 'tp-SpinnerContainer',


    /* borders          ----------------------------------------------------------------- */
    Border: 'tp-BorderBox',
    BorderLeft: 'tp-BorderLeft',
    BorderTop: 'tp-BorderTop',
    BorderRight: 'tp-BorderRight',
    BorderBottom: 'tp-BorderBottom',

    /* flex             ----------------------------------------------------------------- */
    FlexWrap: 'tp-FlexWrap',
    FlexNoWrap: 'tp-FlexNoWrap',
    FlexRowReverse: 'tp-FlexRowReverse',
    FlexColumnReverse: 'tp-FlexColumnReverse',
    FlexCenterH: 'tp-FlexCenterH',
    FlexCenterV: 'tp-FlexCenterV',
    FlexCenter: 'tp-FlexCenter',

    /* layout           ----------------------------------------------------------------- */
    Limiter: 'tp-Limiter',
    Col: 'tp-Col',                             // a responsive column, actually a parent class where child could be any s, m or l combination 
    Row: 'tp-Row',                             // a responsive row 

    RowCenter: 'tp-RowCenter',                 // children vertical alignment
    RowTop: 'tp-RowTop',                       // children vertical alignment

    Control: 'tp-Col tp-Control',
    ControlLabel: 'tp-Col tp-ControlLabel',

    CtrlRow: 'tp-CtrlRow',
    CheckBoxRow: 'tp-CheckBoxRow',

    /* containers       ----------------------------------------------------------------- */
    Block: 'tp-Block',
 
    View: 'tp-View',
    BrokerView: 'tp-BrokerView',

    Window: 'tp-Window',
    WindowCaption: 'tp-WindowCaption',
    WindowCaptionText: 'tp-WindowCaptionText',
    WindowCaptionButtonBar: 'tp-WindowCaptionButtonBar',
    WindowCaptionButton: 'tp-WindowCaptionButton',
    WindowContentContainer: 'tp-WindowContentContainer',
    WindowContent: 'tp-WindowContent',
    WindowFooter: 'tp-WindowFooter',

    Panel: 'tp-Panel',
    FlexPanel: 'tp-FlexPanel',
    FillPanel: 'tp-FillPanel',              // the last panel in a container

    PanelList: 'tp-PanelList',
    PanelListItem: 'tp-PanelListItem',

    Frame: 'tp-Frame',

    GroupBox: 'tp-GroupBox',
    Splitter: 'tp-Splitter',
    Accordion: 'tp-Accordion',
    TabControl: 'tp-TabControl',
    TreeView: 'tp-TreeView',

    Node: 'tp-Node',
    Leaf: 'tp-Leaf',

    /* Sql Filters          ----------------------------------------------------------------- */
    SelectSqlListUi: 'tp-SelectSqlListUi',
    SqlFilterPanel: 'tp-SqlFilterPanel',
    SelectSqlListToolBar: 'tp-SelectSqlList-ToolBar',

    SqlFilterRow: 'tp-SqlFilterRow',
    SqlFilterCtrl: 'tp-SqlFilter-Ctrl',
    SqlFilterDateRangeSelector: 'tp-SqlFilter-DateRangeSelector',
 

    
    

 
    /* menus                ----------------------------------------------------------------- */
    Menu: 'tp-Menu',
    ContextMenu: 'tp-ContextMenu',
    MenuItem: 'tp-MenuItem',

    MenuSeparator: 'tp-MenuSeparator',

    MenuItemImage: 'tp-MenuItemImage',
    MenuItemText: 'tp-MenuItemText',
    MenuItemArrow: 'tp-MenuItemArrow',
    MenuItemList: 'tp-MenuItemList',
    MenuItemSeparator: 'tp-MenuItemSeparator',

    /* html controls        ----------------------------------------------------------------- */
    HtmlComboBox: 'tp-HtmlComboBox',
    HtmlListBox: 'tp-HtmlListBox',
    HtmlNumberBox: 'tp-HtmlNumberBox',
    HtmlDateBox: 'tp-HtmlDateBox',


    /* controls             ----------------------------------------------------------------- */
    ControlStrip: 'tp-ControlStrip',         // for combo-boxes, buttoned edit-boxes etc.
    ControlText: 'tp-ControlText',           // a flex container of text in a control
    ControlButtons: 'tp-ControlButtons',     // a bar of buttons in a control
    ControlButton: 'tp-ControlButton',       // for combo-boxes, buttoned edit-boxes etc.

    DropDownBox: 'tp-DropDownBox',
    DropDownBoxItem: 'tp-DropDownBoxItem',

    ItemBar: 'tp-ItemBar',
    Button: 'tp-Button',
    ButtonEx: 'tp-ButtonEx',    
    ToolBar: 'tp-ToolBar',
    ToolButton: 'tp-ToolButton',
    ButtonStrip: 'tp-ButtonStrip',

    ControlToolBar: 'tp-ControlToolBar',
    ControlToolButton: 'tp-ControlToolButton',

    Label: 'tp-Label',
    TextBox: 'tp-TextBox',
    AutocompleteList: 'tp-AutocompleteList',
    Memo: 'tp-Memo',
    CheckBox: 'tp-CheckBox',
    RadioGroup: 'tp-RadioGroup',
    DateBox: 'tp-DateBox',
    NumberBox: 'tp-NumberBox',

    ListControl: 'tp-ListControl',
    ComboBox: 'tp-ComboBox',
    ListBox: 'tp-ListBox',

    CheckListBox: 'tp-CheckListBox',
    CheckComboBox: 'tp-CheckComboBox',
 
    ProgressBar: 'tp-ProgressBar',

    ValueSlider: 'tp-ValueSlider',
    RangeLabel: 'tp-RangeLabel',

    Calendar: 'tp-Calendar',
    CalendarHeaderRow: 'tp-CalendarHeaderRow',
    CalendarDaysRow: 'tp-CalendarDaysRow',
    CalendarWeekRow: 'tp-CalendarWeekRow',
    CalendarDateCell: 'tp-CalendarDateCell',

    LocatorBox: 'tp-LocatorBox',


    /* date-box             ----------------------------------------------------------------- */


    /* image and slider     ----------------------------------------------------------------- */
    ImageBox: 'tp-ImageBox',
    ImageSlider: 'tp-ImageSlider',


    /* grid                 ----------------------------------------------------------------- */
    Grid: 'tp-Grid',

    GridColumn: 'tp-Grid-Col',
    GridRow: 'tp-Grid-Row',
    //GridNode: 'tp-Grid-Node', // 

    CellText: 'tp-Cell-Text',

    Sorter: 'tp-Sorter',
    Resizer: 'tp-Resizer',
    Expander: 'tp-Expander',

    GridCell: 'tp-Grid-Cell',
    GroupCell: 'tp-Group-Cell',
    FilterCell: 'tp-Filter-Cell',
    SummaryCell: 'tp-Summary-Cell',

    FilterTextBox: 'tp-Filter-TextBox',
    GridInplaceEditor: 'tp-Grid-InplaceEditor',
    GridInplaceEditorCheckBox: 'tp-Grid-InplaceEditor-CheckBox',
    GridInplaceEditorLocator: 'tp-Grid-InplaceEditor-Locator',

    Columns: 'tp-Columns',
    Groups: 'tp-Groups',
    Filters: 'tp-Filters',
    //GridFooter: 'tp-Grid-Footer',
    Summaries: 'tp-Summaries',

    GridToolBar: 'tp-Grid-ToolBar',
    GridToolButton: 'tp-Grid-ToolButton',

    GridGroup: 'tp-Grid-Group',
    GridGroupText: 'tp-Grid-Group-Text',
    GridGroupFooter: 'tp-Grid-Group-Footer',
    //GridGroupExpander: 'tp-Grid-Group-Expander',

    Summary: 'tp-Summary',

    CellTube: 'tp-Cell-Tube',

    /* icons                ----------------------------------------------------------------- */
    IcoArrowDown: 'fa fa-caret-down', // 'fa fa-caret-down'  ti ti-ArrowDown
    IcoCalendar: 'fa fa-calendar',
    IcoCheck: 'fa fa-check-square-o',
    IcoUnCheck: 'fa fa-square-o',

    IcoToolBarInsert: 'fa fa-plus-circle',
    IcoToolBarDelete: 'fa fa-minus-circle',
    IcoToolBarEdit: 'fa fa-pencil-square-o',         // fa fa-pencil
    IcoToolBarFind: 'fa fa-search '                 // fa fa-filter
};
//#endregion
 

//#region tp.ImageSizeMode
/** Indicates how to handle an image resizing
 @class
 @enum {number}
 */
tp.ImageSizeMode = {
    /** none  */
    Unknown: 0,
    /** background-size: cover */
    Crop: 1,
    /** background-size: contain */
    Scale: 2,
    /** background-size: 100% 100% */
    Stretch: 4
};
Object.freeze(tp.ImageSizeMode);
//#endregion

 
//#region tp.ICommandProperty

/** Represents an object with a Command string property. 
 @interface
 */
tp.ICommandProperty = class {
    /** The command
     @type {string}
     */
    Command;
};

/**
Type-guard function
@param {any} v The object to check
@returns {boolean} Returns true if the specified object provides a Command string property.
*/
tp.HasCommandProperty = function (v) { return v instanceof tp.tpObject && 'Command' in v; };
//#endregion

//#region tp.ISelectedIndex
/**
Represents an object that provides a SelectedIndex property and triggers SelectedIndexChanged events
 @interface
*/
tp.ISelectedIndex = class {
    /**
    The selected index of the object
    @type {number}
    */
    SelectedIndex;
};

/**
Type-guard function
@param {any} v The object to check
@returns {boolean} Returns true if the specified object provides a SelectedIndex numeric property.
*/
tp.IsISelectedIndex = function (v) { return v instanceof tp.tpObject && 'SelectedIndex' in v; };
//#endregion

//#region tp.IDropDownBoxListener
/**
A listener for dropdown box events. The owner of a dropdown box has to implement this interface if it is to get notified about dropdown box stage changes.
 @interface
*/
tp.IDropDownBoxListener = class {
    /**
        Called by the dropdown box to inform its owner about a stage change.
        @param {tp.DropDownBox} Sender The sender
        @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants.
        */
    OnDropDownBoxEvent(Sender, Stage) { }
};

/**
Type guard function for the IDropDownBoxListener
@param {any} v - The object to check
@return {boolean} Returns true if the specified object passes the test.
*/
tp.IsDropDownBoxListener = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['OnDropDownBoxEvent']); };

//#endregion

//---------------------------------------------------------------------------------------
// Containers
//---------------------------------------------------------------------------------------

//#region tp.IndexChangeEventArgs
/**
EventArgs for events notifying about index changes
*/
tp.IndexChangeEventArgs = class extends tp.EventArgs {

    /**
    Constructor 
    @param {number} CurrentIndex - Optional.
    @param {number} NewIndex - Optional.
    */
    constructor(CurrentIndex, NewIndex) {
        super('', null);

        this.CurrentIndex = CurrentIndex;
        this.NewIndex = NewIndex;
    }
 
};
/**
 The current index
 @type {number}
 */
tp.IndexChangeEventArgs.prototype.CurrentIndex = -1;
/**
 The new index
 @type {number}
 */
tp.IndexChangeEventArgs.prototype.NewIndex = -1;
//#endregion 

//#region tp.CreateChildEventArgs
/**
EventArgs for events notifying about child creation
*/
tp.CreateChildEventArgs = class extends tp.EventArgs {
    /**
    Constructor 
    @param {HTMLElement|tp.tpElement} Child The child element this event is about. Could be HTMLElement or tp.tpElement
    */
    constructor(Child) {
        super('', null);

        this.elChild = null;
        this.tpChild = null;

        if (Child instanceof tp.tpElement) {
            this.tpChild = Child;
            this.elChild = Child.Handle;
        }

        if (!tp.IsHTMLElement(this.elChild) && (Child instanceof HTMLElement))
            this.elChild = Child;

        this.Child = null;
    }

};
/**
Used when child is a single DOM element
@type {HTMLElement}
*/
tp.CreateChildEventArgs.prototype.elChild = null;
/**
Used when child is a single tp.tpElement
@type {tp.tpElement}
*/
tp.CreateChildEventArgs.prototype.tpChild = null;
/**
Used when child is complex object
@type {object}
*/
tp.CreateChildEventArgs.prototype.Child = null;
//#endregion 


//#region tp.GroupBox
/**
A group-box container, that is a fieldset element with a legend title element. <br />
Example markup:
<pre>
    <fieldset> </fieldset>

    <fieldset>
        <legend>Title</legend>
    </fieldset>
</pre>
*/
tp.GroupBox = class extends tp.tpElement {

    /**
    Constructor <br />

    Example markup:
    <pre>
        <fieldset> </fieldset>

        <fieldset>
            <legend>Title</legend>
        </fieldset>
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional. 
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /**
    Gets or sets the title of the group box
    @type {string}
    */
    get Text() {
        return this.fLegend instanceof HTMLLegendElement ? this.fLegend.innerHTML : '';
    }
    set Text(v) {
        if (tp.IsString(v) && this.fLegend instanceof HTMLLegendElement) {
            this.fLegend.innerHTML = v;
            this.fLegend.style.display = tp.IsBlank(v) ? 'none' : '';
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

        this.tpClass = 'tp.GroupBox';

        this.fElementType = 'fieldset';
        this.fDisplayType = '';         // by default, either the css defined or block
        this.fDefaultCssClasses = [tp.Classes.GroupBox];
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        let el = tp.Select(this.Handle, 'legend');
        if (!tp.IsEmpty(el)) {
            this.fLegend = el;
        } else {
            this.fLegend = this.Document.createElement('legend');
            tp.InsertNode(this.Handle, 0, this.fLegend);            
        }

        this.fLegend.style.display = tp.IsBlank(this.Text) ? 'none' : '';

    }
};
/** The legend element
 @protected
 @type {HTMLLegendElement}*/
tp.GroupBox.prototype.fLegend = null;
//#endregion  

//#region tp.Accordion
/**
Accordion <br />
Each child is a div containing two divs, a title and a content div.
Example markup <br />
<pre>
    <div id="Accordion" class="tp-Accordion" data-setup="{AllowMultiExpand: false}">
        <div>
            <div>Item 1</div>
            <div>
                <p>Hi there</p>
                <p>Content of the first item</p>
            </div>
        </div>
        <div>
            <div>Item 2</div>
            <div>
                <p>This content belongs to</p>
                <p>the second accordion item</p>
            </div>
        </div>
    </div>
</pre>

*/
tp.Accordion = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup <br />
    <pre>
        <div id="Accordion" class="tp-Accordion" data-setup="{AllowMultiExpand: false}">
            <div>
                <div>Item 1</div>
                <div>
                    <p>Hi there</p>
                    <p>Content of the first item</p>
                </div>
            </div>
            <div>
                <div>Item 2</div>
                <div>
                    <p>This content belongs to</p>
                    <p>the second accordion item</p>
                </div>
            </div>
        </div>
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /* protected */
    /**
     * Finds and returns a clicked child element, if any, else returns null.
     * @protected
     * @param {Event} e An Event object
     * @returns {HTMLElement} Finds and returns a clicked child element, if any, else returns null.
     */
    FindClickedChild(e) {

        let el,
            elCaption,
            List = this.GetElementList();

        for (let i = 0, ln = List.length; i < ln; i++) {
            el = List[i];
            elCaption = (el.children.length > 0 ? el.children[0] : null);
            if (elCaption instanceof HTMLElement) {
                if (tp.ContainsEventTarget(elCaption, e.target))
                    return el;
            }
        }

        return null;
    }
    /**
     * Creates and returns a child. <br />
     * It first triggers the ChildCreating event giving a chance to client code to create the child and return the div.
     * @protected
     * @param {string} [Title] Optional. The title of the new child.
     * @returns {HTMLDivElement} Returns the newly created div element.
     */
    CreateChild(Title) {
  
       this.OnChildCreating();
    
       let Result = this.Document.createElement('div');

        // title
        let el = this.Document.createElement('div');
        Result.appendChild(el);
        el.innerHTML = tp.IsString(Title) && !tp.IsBlank(Title) ? Title : tp.NO_NAME;
 
        // content
        el = this.Document.createElement('div');
        Result.appendChild(el);

        this.OnChildCreated(Result);

        return Result;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.Accordion';
        this.fDefaultCssClasses = [tp.Classes.Accordion];
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        if (tp.IsEmpty(this.AllowMultiExpand))
            this.AllowMultiExpand = false;
    }
    /**
    Event trigger
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        this.HookEvent(tp.Events.Click);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The Event object.
    */
    OnAnyDOMEvent(e) {

        var el, i, ln, List, Type = tp.Events.ToTripous(e.type);

        if (tp.Events.Click === Type) {
            el = this.FindClickedChild(e);
            if (el) {
                i = this.IndexOfElement(el);
                var IsExpanded = tp.HasClass(el, tp.Classes.Expanded);
                this.Expand(!IsExpanded, i);
            }
        }

        super.OnAnyDOMEvent(e);
    }

    /* public */
    /**
     Expands or collapses one or all items.
     @param {boolean} Flag - True expands, false collapses
     @param {number} [ChildIndex=-1] - To expand/collapse all items pass -1, else pass the item index
     */
    Expand(Flag, ChildIndex = -1) {
        let i, ln;

        if (tp.IsEmpty(ChildIndex))
            ChildIndex = -1;

        var List = this.GetElementList();

        if (ChildIndex < 0) {
            for (i = 0, ln = List.length; i < ln; i++) {
                tp.RemoveClass(List[i], tp.Classes.Expanded);
            }

            if (Flag) {
                for (i = 0, ln = List.length; i < ln; i++) {
                    tp.AddClass(List[i], tp.Classes.Expanded);
                }
            }
        } else {
            if (!this.AllowMultiExpand) {
                for (i = 0, ln = List.length; i < ln; i++) {
                    tp.RemoveClass(List[i], tp.Classes.Expanded);
                }
            }

            var el = List[ChildIndex];
            if (Flag) {
                tp.AddClass(el, tp.Classes.Expanded);
            } else {
                tp.RemoveClass(el, tp.Classes.Expanded);
            }
        }

    }
    /**
    Toggles the expansion of a child item at a specified index.
    @param {number} Index The child index.
    */
    Toggle(Index) {
        let el = this.GetElementAt(Index);
        if (el) {
            this.Expand(!this.IsExpanded(Index), Index);
        }
    }
    /**
    Returns true if an child item at a specified index is expanded.
    @param {number} Index The index to check.
    @returns {boolean} Returns true if an child item at a specified index is expanded.
    */
    IsExpanded(Index) {
        let el = this.GetElementAt(Index);
        if (el) {
            return tp.HasClass(el, tp.Classes.Expanded);
        }

        return false;
    }

    /**
    Adds and returns a child. A child is a div with two children divs, a title div and a content div.
    @param {string} [Title] Optional. The title of the new child.
    @returns {HTMLElement} Returns the newly added child
    */
    AddChild(Title)  {
        return this.InsertChild(this.GetElementList().length, Title);
    }
    /**
    Inserts a child at a specified index and returns the child. A child is a div with two children divs, a title div and a content div.
    @param {number} Index The index to use
    @param {string} [Title] Optional. The title of the new child.
    @returns {HTMLElement} Returns the newly added child
    */
    InsertChild(Index, Title) {
        if (this.Handle) {
            var List = this.GetElementList();

            let Child = this.CreateChild(Title);

            if (List.length === 0 || Index < 0 || Index >= List.length) {
                Index = List.length === 0 ? 0 : List.length;
                this.Handle.appendChild(Child);
            } else {
                var RefChild = List[Index];
                this.Handle.insertBefore(Child, RefChild);
            }

            this.Expand(true, Index);
            return Child;
        }

        return null;
    }

    /**
    Returns the title HTMLElement of a child found at a specified index.
    A child is a div with two children divs, a title div and a content div.
    @param {number} Index The index to use
    @return {HTMLElement} Returns the title HTMLElement if found, else null
    */
    TitleChildAt(Index) {
        let el = this.GetElementAt(Index);
        if ((el instanceof HTMLElement) && el.children.length > 0) {
            return el.children[0];
        }

        return null;
    }
    /**
    Returns the content HTMLElement of a child found at a specified index.
    A child is a div with two children divs, a title div and a content div.
    @param {number} Index The index to use
    @return {HTMLElement} Returns the title HTMLElement if found, else null
    */
    ContentChildAt(Index)  {
        let el = this.GetElementAt(Index);
        if ((el instanceof HTMLElement) && el.children.length > 1) {
            return el.children[1];
        }

        return null;
    }

    /**
    Returns the title text of a child found at a specified index.
    @param {number} Index The index to use
    @return {string} Returns the title text of a child found at a specified index.
    */
    GetTitleAt(Index) {
        let el = this.TitleChildAt(Index);
        if (el instanceof HTMLElement) {
            return el.innerHTML;
        }

        return '';
    }
    /**
    Sets the title text of a child found at a specified index.
    @param {number} Index The index to use
    @param {string} Text The title text.
    */
    SetTitleAt(Index, Text) {
        let el = this.TitleChildAt(Index);
        if (el instanceof HTMLElement) {
            el.innerHTML = Text;
        }
    }

    /** Returns all the title elements
     * @returns {HTMLElement[]} Returns all the title elements
     * */
    GetTitleElements() {
        let ElementList = this.GetElementList();
        let Result = [];

        ElementList.forEach((item) => {
            let List = tp.ChildHTMLElements(item);
            if (List.length === 2) {
                Result.push(List[0]);
            }
        });

        return Result;
    }
    /** Returns all the panel elements
     * @returns {HTMLElement[]} Returns all the title elements
     * */
    GetPanelElements() {
        let ElementList = this.GetElementList();
        let Result = [];

        ElementList.forEach((item) => {
            let List = tp.ChildHTMLElements(item);
            if (List.length === 2) {
                Result.push(List[1]);
            }
        });

        return Result;
    }

    /* event triggers */
    /**
    Event trigger. <br />
    Gives a chance to client code to create and return the new child.
    @returns {tp.CreateChildEventArgs} Returns the tp.CreateChildEventArgs Args of the event.
    */
    OnChildCreating() {
        let Args = new tp.CreateChildEventArgs(null);
        this.Trigger('ChildCreating', Args);
        return Args;
    }
    /**
    Event trigger
    @param {HTMLElement} Child The newly created child.
    */
    OnChildCreated(Child) {
        let Args = new tp.CreateChildEventArgs(Child);
        this.Trigger('ChildCreated', Args);
    }

};
/**
When true, then multiple child items can be expanded at the same time. Defaults to false.
@type {boolean}
*/
tp.Accordion.prototype.AllowMultiExpand = false;
//#endregion  

//#region tp.PanelList
/**
A list of panels. Only one panel is visible at the same time. <br />
An Associate object, one that provides a SelectedIndexChanged event and a SelectedIndex property, can automatically control the selected panel of this panel list. <br />
The Associate object can be passed in CreateParams or in the declarative (markup) create params of the data-setup attribute. <br />
Example markup <br />
<pre>
    <div id="PanelList" data-setup="{ SelectedIndex: 0 }">
        <div>Panel 1</div>
        <div>Panel 2</div>
        <div>Panel 3</div>
    </div>
</pre>
@implements {tp.ISelectedIndex}
*/
tp.PanelList = class extends tp.tpElement {


    /**
    Constructor <br />
    Example markup <br />
    <pre>
        <div id="PanelList" data-setup="{ SelectedIndex: 0 }">
            <div>Panel 1</div>
            <div>Panel 2</div>
            <div>Panel 3</div>
        </div>
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or set the associate element, the element that controls the selected index of this panel list. <br />
    The Associate must provide a SelectedIndex property and perhaps a SelectedIndexChanged event. <br />
    Setting accepts a string selector, a DOM element or a tp.tpElement.
    @type {string|HTMLElement|tp.tpElement}
    */
    get Associate() {
        return this.fAssociate;
    }
    set Associate(v) {
        if (v !== this.fAssociate) {

            if (this.fAssociate instanceof tp.tpObject && this.fSelectedIndexListener instanceof tp.Listener) {
                this.fAssociate.Off('SelectedIndexChanged', this.fSelectedIndexListener);
            }

            this.fAssociate = null;
            this.SelectedIndex = -1;

            if (tp.IsISelectedIndex(v)) {
                this.fAssociate = v;
            } else if (tp.IsString(v) || tp.IsNode(v)) {
                let o = tp.GetObject(v);
                if (tp.IsISelectedIndex(o))
                    this.fAssociate = o;
            }

            if (tp.IsISelectedIndex(this.fAssociate) && this.fAssociate instanceof tp.tpObject) {
                this.fAssociate.On('SelectedIndexChanged', this.Associate_SelectedIndexChanged, this);
                this.SelectedIndex = this.Associate.SelectedIndex;
            }
        }

    }
    /**
    Gets or sets the selected index
    @type {number}
    */
    get SelectedIndex() {
        var el = tp.Select(this.Handle, '.' + tp.Classes.Selected);
        if (tp.IsHTMLElement(el)) {
            let List = tp.ChildHTMLElements(this.Handle);
            return List.indexOf(el);
        }
        return -1;
    }
    set SelectedIndex(v) {
        var SelectedIndex = this.SelectedIndex;

        if (v !== SelectedIndex) {
            this.SetSelectedIndex(v);
        }
    }
    /**
    Gets or sets the selected panel
    @type {HTMLElement}
    */
    get SelectedPanel() {
        let Index = this.SelectedIndex;
        if (Index >= 0) {
            let Panels = this.GetPanels();
            return Panels[Index];
        }

        return null;
    }
    set SelectedPanel(v) {
        if (tp.IsElement(v)) {
            let Panels = this.GetPanels();
            let Index = Panels.indexOf(v);
            if (Index >= 0) {
                this.SelectedIndex = Index;
            }
        }
    }

    /* protected */
    /**
     * Sets the selected index. 
     * @protected
     * @param {number} Index The index to set.
     */
    SetSelectedIndex(Index) {
        let List = this.GetPanels();
        if (tp.InRange(List, Index)) {
            for (var i = 0, ln = List.length; i < ln; i++) {
                tp.RemoveClass(List[i], tp.Classes.Selected);
                List[i].style.display = 'none';
            }

            tp.AddClass(List[Index], tp.Classes.Selected);
            List[Index].style.display = '';

            if (tp.IsISelectedIndex(this.fAssociate) && (this.fUpdateAssociate === true)) {
                this.Associate.SelectedIndex = Index;
            }

            this.OnSelectedIndexChanged();
        }
    }
    /**
     * Event handler. Handles the SelectedIndexChanged event of the Associate.
     * @protected
     * @param {tp.EventArgs} Args A tp.EventArgs object.
     */
    Associate_SelectedIndexChanged(Args) {
        if (tp.IsISelectedIndex(this.fAssociate)) {
            this.fUpdateAssociate = false;
            try {
                this.SelectedIndex = this.Associate.SelectedIndex;
            } finally {
                this.fUpdateAssociate = true;
            }
        }
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @override
    @protected
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.PanelList';
        this.fDefaultCssClasses = [tp.Classes.PanelList];
    }
    /**
   Initializes fields and properties just before applying the create params.
   @override
   @protected
   */
    InitializeFields() {
        super.InitializeFields();
        this.fUpdateAssociate = true;
    }


    /* public */
    /**
    Adds and returns a child. 
    @returns {HTMLElement} Returns the newly added child
    */
    AddChild() {
        return this.InsertChild(this.Count);
    }
    /**
    Inserts a child at a specified index and returns the child.  
    @param {number} Index The index to use.
    @returns {HTMLElement} Returns the newly added child
    */
    InsertChild(Index)  {
        if (this.Handle) {
            var List = this.GetElementList();

            let Child = this.Document.createElement('div');

            if (List.length === 0 || Index < 0 || Index >= List.length) {
                Index = List.length === 0 ? 0 : List.length;

                this.Handle.appendChild(Child);
            } else {
                this.Handle.insertBefore(Child, List[Index]);
            }

            this.SetSelectedIndex(Index);
            return Child;
        }

        return null;
    }
    /**
    Returns an array with the panel elements
    @returns {HTMLElement[]} Returns an array with the panel elements
    */
    GetPanels()  {
        return this.GetElementList();
    }

    /* event triggers */
    /**
    Event trigger
    */
    OnSelectedIndexChanged() {
        this.Trigger('SelectedIndexChanged', {});
    }


};
/** Private field for the Associate object 
 @private
 @type {object}
 */
tp.PanelList.prototype.fAssociate = null;
/** Private field. A flag used internally. 
 @private
 @type {boolean}
 */
tp.PanelList.prototype.fUpdateAssociate = false;
/** Private field. 
 @private
 @type {tp.Listener}
 */
tp.PanelList.prototype.fSelectedIndexListener = null; //: tp.Listener;
//#endregion  

//#region tp.TabControl


/**
Represents a tab page, that is a child to a TabControl
*/
tp.TabPage = class extends tp.tpElement {

    /** Constructor.
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
     */
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

    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    @protected
    */
    DoDispose() {
        try {
            if (tp.IsHTMLElement(this.Tab) && tp.IsHTMLElement(this.Tab.parentNode)) {
                this.Tab.parentNode.removeChild(this.Tab);
            }

        } catch (e) {
            //
        }

        super.DoDispose();
    }
};
/**
The tab is the caption text container
@type {HTMLElement}
*/
tp.TabPage.prototype.Tab = null;


/**
TabControl <br />
This control uses a ResizeObserver (actually a {@link tp.ResizeDetector}) to detect whether tabs fit in tab-bar width. <br />
Provides three rendering modes: Normal, Toggle and NextPrev. The rendering mode changes automatically from Normal to the others and back. <br />
The user defines just the <code>ResponsiveMode</code> and then the mode goes from Normal to that <code>ResponsiveMode</code> depending on the width.
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


    /* properties */
    /**
     Gets or sets the render mode. One of the {@link tp.ItemBarRenderMode} constants
     @type {number}
     @private
     */
    get RenderMode() {
        return this.TabBar.RenderMode;
    }
    /** The rendering mode to go to when total item width exceeds this instance width.
      One of the {@link tp.ItemBarRenderMode} constants.
      @type {number}
     */
    get ResponsiveMode() {
        return this.TabBar.ResponsiveMode;
    }
    set ResponsiveMode(v) {
        this.TabBar.ResponsiveMode = v;
    }


    /**
    Gets or sets the selected page index
    @type {number}
    */
    get SelectedIndex() {
        return this.TabBar.SelectedIndex;
    }
    set SelectedIndex(v) {
        this.TabBar.SelectedIndex = v;
    }
    /**
    Gets or sets the selected {@link tp.TabPage} page.
    @type {tp.TabPage}
    */
    get SelectedPage() {
        let Result = null;
        let elPage = null;
        let Index = this.SelectedIndex;

        let PageElementList = this.GetPageElementList();
        if (Index >= 0 && Index <= PageElementList.length - 1) {
            elPage = PageElementList[Index];
            Result = tp.GetObject(elPage);
        }
        return Result;
    }
    set SelectedPage(v) {
        if (v instanceof tp.TabPage) {
            let PageElementList = this.GetPageElementList();
            let Index = PageElementList.indexOf(v.Handle);
            this.SelectedIndex = Index;
        }
    }


    /* private */
    /**
     * Sets the selected page by index.
     * @private
     * @param {number} Index The index of the page to set as selected.
     */
    SetSelectedIndex(Index) {

        var elTab = null;     // HTMLElement;
        var i, ln, Index;

        this.TabBar.SetSelectedIndex(Index);

        let TabElementList = this.GetTabElementList();
        elTab = TabElementList[Index];

        // un-select tab pages
        let PageElementList = this.GetPageElementList();

        for (i = 0, ln = PageElementList.length; i < ln; i++) {
            tp.StyleProp(PageElementList[i], 'display', 'none');
        }

        // selected
        if (elTab && elTab.TabPage) {
            let elPage = elTab.TabPage.Handle;
            elPage.style.display = '';
        }

    }

    /**
     * Creates and returns a new page. <br />
     * NOTE: It does NOT add the page to the control.
     * @private
     * @param {string} [Title] Optional.The title text of the new page.
     * @returns {tp.TabPage} Returns the new page.
     */
    CreatePage(Title) {

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

        return Result;
    }

    /** Creates child controls of this instance. Called in construction sequence.
     * @private
     * */
    CreateControls() {
        let List = this.GetElementList();
        let elTabBar;

        if (List.length === 2) {
            elTabBar = List[0];
            this.PageContainer = List[1];
        }
        else if (List.length === 0) {
            elTabBar = this.AddDivElement();
            this.PageContainer = this.AddDivElement();
        }
        else {
            tp.Throw('Wrong TabControl structure. Should be empty or have 2 child DIVs.');
        }

        this.TabBar = new tp.ItemBar(elTabBar);
        this.TabBar.On('SelectedIndexChanging', this.OnSelectedIndexChanging, this);
        this.TabBar.On('SelectedIndexChanged', this.OnSelectedIndexChanged, this);

        // add classes to containers
        //tp.AddClass(this.TabContainer, tp.Classes.Bar);
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
        this.fDefaultCssClasses = [tp.Classes.TabControl];
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
    }


    /* public */
    /** Shows or hides the tab-bar according to a specified flag
     * @param {boolean} Flag 
     */
    ShowTabBar(Flag) {
        this.TabBar.Visible = Flag === true;
    }
    /** Returns an array with the Tab HTML Elements (tab captions). The array may be empty.
     * @returns {HTMLElement[]} Returns an array with the Tab HTML Elements (tab captions). The array may be empty.
     * */
    GetTabElementList() {
        return this.TabBar.GetItemElementList();
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
    /** Returns the number of pages
     * @returns {number} Returns the number of pages
     * */
    GetPageCount() {
        return this.GetPageList().length;
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
            let PageElementList = this.GetPageElementList();

            let Page = this.CreatePage(Title);

            if (PageElementList.length === 0 || Index < 0 || Index >= PageElementList.length) {
                Index = PageElementList.length === 0 ? 0 : PageElementList.length;

                this.TabBar.AddItem(Page.Tab);
                this.PageContainer.appendChild(Page.Handle);

            } else {
                this.TabBar.InsertAt(Index, Page.Tab);
                this.PageContainer.insertBefore(Page.Handle, this.PageContainer.children[Index]);
            }

            this.OnPageAdded(Page);

            this.SelectedIndex = Index;

            return Page;
        }

        return null;
    }
    /** Removes a page at a specified index
     * @param {number} Index The index of the page in the list of pages
     */
    RemovePageAt(Index) {
        if (tp.IsNumber(Index)) {
            let List = this.GetPageList();
            if (Index >= 0 && Index <= List.length - 1) {

                let Page = List[Index];

                this.OnPageRemoving(Page);
                this.TabBar.RemoveItemAt(Index);
                Page.Dispose();

                this.OnPageRemoved(Page);

                // new index
                List = this.GetPageList();
                let NewIndex = Index - 1;
                let NewIndex2 = Index + 1;
                if (NewIndex >= 0 && NewIndex <= List.length - 1) {
                    this.SelectedIndex = NewIndex;
                }
                else if (NewIndex2 >= 0 && NewIndex2 <= List.length - 1) {
                    this.SelectedIndex = NewIndex;
                }
                else {
                    this.SelectedIndex = List.length > 0 ? List.length - 1 : -1;
                }

            }
        }
    }
    /** Removes a specified page.
     * @param {tp.TabPage} Page
     */
    RemovePage(Page) {
        if (Page instanceof tp.TabPage) {
            let List = this.GetPageList();
            let Index = List.indexOf(Page);
            this.RemovePageAt(Index);
        }
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
    OnSelectedIndexChanging(Args) {
        this.Trigger('SelectedIndexChanging', Args);
    }
    /**
    Event trigger
    @param {number} CurrentIndex The current index.
    */
    OnSelectedIndexChanged(Args) {
        this.SetSelectedIndex(Args.CurrentIndex);
        this.Trigger('SelectedIndexChanged', Args);
    }
    /**
    Event trigger
    @param {tp.TabPage} Page A tp.TabPage 
    */
    OnPageAdded(Page) {
        let Args = new tp.CreateChildEventArgs(null);
        Args.Child = Page;
        this.Trigger('PageAdded', Args);
    }
    /**
    Event trigger
    @param {tp.TabPage} Page A tp.TabPage 
    */
    OnPageRemoving(Page) {
        let Args = new tp.CreateChildEventArgs(null);
        Args.Child = Page;
        this.Trigger('PageAdded', Args);
    }
    /**
    Event trigger
    @param {tp.TabPage} Page A tp.TabPage 
    */
    OnPageRemoved(Page) {
        let Args = new tp.CreateChildEventArgs(null);
        Args.Child = Page;
        this.Trigger('PageRemoved', Args);
    }

};



/** Private field.
 @type {tp.ItemBar}
 */
tp.TabControl.prototype.TabBar = null;
/** Private field.
 @type {HTMLElement}
 */
tp.TabControl.prototype.PageContainer = null;




//#endregion

//#region tp.ImageSlider

/**
Image slider
Example markup
<pre>
    <div id="ImageSlider" data-setup='{ Height: 500, Width: 700, Images: ["/Images/Obama.jpg", "/Images/Kefalonia.jpg", "/Images/Garden.jpg"], SelectedIndex: 2 }'></div>
</pre>
@implements {tp.ISelectedIndex}
*/
tp.ImageSlider = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup
    <pre>
        <div id="ImageSlider" data-setup='{ Height: 500, Width: 700, Images: ["/Images/Obama.jpg", "/Images/Kefalonia.jpg", "/Images/Garden.jpg"], SelectedIndex: 2 }'></div>
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Returns the number of images
    @type {number}
    */
    get ImageCount() {
        return this.fImages.length;
    }
    /**
    Gets or sets the array of image urls.
    @type {string[]}
    */
    get Images() {
        if (tp.IsEmpty(this.fImages))
            this.fImages = [];
        return this.fImages;
    }
    set Images(v) {
        if (tp.IsArray(v))
            this.fImages = v;
    }
    /**
    Gets or sets the selected index
    @default -1
    @type {number}
    */
    get SelectedIndex() {
        return this.fSelectedIndex;
    }
    set SelectedIndex(v) {
        if (tp.IsNumber(v) && tp.InRange(this.Images, v)) {
            this.fSelectedIndex = v;
            let S = this.Images[v];

            if (!tp.IsBlank(S) && !tp.StartsWith(S, 'url(')) {
                S = 'url(' + S + ')';
            }

            this.StyleProp('background-image', S);

            this.OnSelectedIndexChanged();
        }
    }
    /**
    Gets or sets the size mode. <br />
    It always returns one of the {@link tp.ImageSizeMode} constants. Accepts either a tp.ImageSizeMode or the corresponding enum name string.
    @type {number}
    */
    get SizeMode() {
        var S = this.StyleProp('background-size');
        if (S === 'cover')
            return tp.ImageSizeMode.Crop;
        if (S === 'contain')
            return tp.ImageSizeMode.Scale;
        if (S === '100% 100%')
            return tp.ImageSizeMode.Stretch;
        return tp.ImageSizeMode.Unknown;
    }
    set SizeMode(v) {
        if (tp.IsString(v)) {
            if (tp.IsSameText(v, 'Crop'))
                v = tp.ImageSizeMode.Crop;
            else if (tp.IsSameText(v, 'Scale'))
                v = tp.ImageSizeMode.Scale;
            else if (tp.IsSameText(v, 'Stretch'))
                v = tp.ImageSizeMode.Stretch;
            else
                v = tp.ImageSizeMode.Stretch;
        }

        if (v === tp.ImageSizeMode.Crop)
            this.StyleProp('background-size', 'cover');
        else if (v === tp.ImageSizeMode.Scale)
            this.StyleProp('background-size', 'contain');
        else if (v === tp.ImageSizeMode.Stretch)
            this.StyleProp('background-size', '100% 100%');
    }
    /**
    Gets or sets a boolean value indicating whether to automatically cycle image display
    @default true
    @type {boolean}
    */
    get AutoCycle() {
        return this.fAutoCycle;
    }
    set AutoCycle(v) {
        v = v === true;
        if (v !== this.AutoCycle) {
            this.fAutoCycle = v === true;
            this.DoAutoCycle(this.fAutoCycle);
        }
    }
    /**
    Gets or sets the interval in milliseconds of the auto-cycling
    @default 6000
    @type {number}
    */
    get AutoCycleMSecs() {
        return this.fAutoCycleInterval;
    }
    set AutoCycleMSecs(v) {
        if (v !== this.fAutoCycleInterval) {
            this.fAutoCycleInterval = v;
            if (this.AutoCycle === true) {
                this.AutoCycle = false;
                this.AutoCycle = true;
            }
        }
    }

    get DisplayCycleButtons() {
        return this.fDisplayCycleButtons === true;
    }
    set DisplayCycleButtons(v) {
        v = Boolean(v);
        if (v !== this.DisplayCycleButtons) {
            this.fDisplayCycleButtons = v;
            this.DisplayCycleButtonsChanged();
        }        
    }

    /* protected */
    /**
     * Starts or stops the auto-cycling of images according to a specified flag.
     * @protected
     * @param {boolean} Flag The flag.
     */
    DoAutoCycle(Flag) {
        if (!tp.IsEmpty(this.fAutoCycleId)) {
            clearInterval(this.fAutoCycleId);
            this.fAutoCycleId = null;
        }

        if (Flag === true) {
            this.fAutoCycleId = setInterval(function (Context) {
                var Index = Context.SelectedIndex + 1;
                if (Index > (Context.Images.length - 1)) {
                    Index = 0;
                }
                Context.SelectedIndex = Index;
            }, this.AutoCycleMSecs, this);
        }
    }
    /** Hides or shows the prev and next buttons according to DisplayCycleButtons property value. */
    DisplayCycleButtonsChanged() {
        if (tp.IsElement(this.fNext)) {
            this.fNext.style.display = this.DisplayCycleButtons === true ? '' : 'none';
            this.fPrev.style.display = this.DisplayCycleButtons === true ? '' : 'none';
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

        this.tpClass = 'tp.ImageSlider';
        this.fDefaultCssClasses = [tp.Classes.ImageSlider];
    }
    /**
   Initializes fields and properties just before applying the create params.
   @protected
   @override
   */
    InitializeFields() {
        super.InitializeFields();

        this.fImages = [];
        this.fSelectedIndex = -1;
        this.fAutoCycleInterval = 6000;
        this.PauseOnHover = true;
        this.AutoCycle = true;
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        this.HookEvent(tp.Events.MouseEnter);
        this.HookEvent(tp.Events.MouseLeave);
        this.HookEvent(tp.Events.Click);

        this.fNext = this.Document.createElement('div');
        this.Handle.appendChild(this.fNext);
        this.fNext.className = tp.Classes.Next;

        this.fPrev = this.Document.createElement('div');
        this.Handle.appendChild(this.fPrev);
        this.fPrev.className = tp.Classes.Prev;
    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.DoAutoCycle(this.AutoCycle);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The Event object.
    */
    OnAnyDOMEvent(e) {

        var Index, Type = tp.Events.ToTripous(e.type);

        switch (Type) {
            case tp.Events.MouseEnter:
                if (this.AutoCycle && this.PauseOnHover) {
                    this.DoAutoCycle(false);
                }
                break;

            case tp.Events.MouseLeave:
                if (this.AutoCycle && this.PauseOnHover) {
                    this.DoAutoCycle(true);
                }
                break;

            case tp.Events.Click:
                if (this.Images.length > 1) {
                    if (this.fPrev === e.target || this.ChangeOnClick === true) {
                        Index = this.SelectedIndex - 1;
                        if (Index < 0) {
                            Index = this.Images.length - 1;
                        }
                    } else if (this.fNext === e.target || this.ChangeOnClick === true) {
                        Index = this.SelectedIndex + 1;
                        if (Index > this.Images.length - 1) {
                            Index = 0;
                        }
                    }

                    this.SelectedIndex = Index;

                } 

                break;

        }

        super.OnAnyDOMEvent(e);
    }

    /* event triggers */
    /**
    Event trigger
    */
    OnSelectedIndexChanged() {
        this.Trigger('SelectedIndexChanged', {});
    }
};
/**
Gets or sets a boolean value indicating whether to pause auto-cycling while mouse is over the control.
@default true
@public
@type {boolean}
*/
tp.ImageSlider.prototype.PauseOnHover = true;
/** 
 Private field.
 @private
 @type {string[]}
 */
tp.ImageSlider.prototype.fImages = [];
/**
 Private field.
 @private
 @type {number}
 */
tp.ImageSlider.prototype.fSelectedIndex = -1;
/**
 Private field.
 @private
 @type {boolean}
 */
tp.ImageSlider.prototype.fAutoCycle = true;
/**
 Private field. Returned by the setInterval()
 @private
 @type {number}
 */
tp.ImageSlider.prototype.fAutoCycleId = null;
/**
 Private field.
 @private
 @type {number}
 */
tp.ImageSlider.prototype.fAutoCycleInterval = 0;
/**
Private field.
@private
@type {HTMLElement}
*/
tp.ImageSlider.prototype.fNext = null;
/**
 Private field.
 @private
 @type {HTMLElement}
 */
tp.ImageSlider.prototype.fPrev = null;
/**
 Private field.
 @private
 @type {boolean}
 */
tp.ImageSlider.prototype.fDisplayCycleButtons = true;

/** When true, clicking on an image displays the next image.
@default true
@public
 @type {boolean}
 */
tp.ImageSlider.prototype.ChangeOnClick = true;
//#endregion  

//#region tp.Splitter
/**
A splitter bar. <br />
Place the splitter bar between two divs an let it auto-config. Or define a sibling div as the Associate.
Example markup <br />
<pre>
    <div id="Container" style="display: flex;">
        <div id="A">This is the A panel</div>
        <div class="tp-Splitter"></div>
        <div id="B">This is the B panel</div>
    </div>
</pre>
*/
tp.Splitter = class extends tp.tpElement {

    /**
    Constructor.
    Example markup <br />
    <pre>
        <div id="Container" style="display: flex;">
            <div id="A">This is the A panel</div>
            <div class="tp-Splitter"></div>
            <div id="B">This is the B panel</div>
        </div>
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /* protected */
    /**
     * Adjusts the orientation according to IsHorizontal property value.
     * @protected
     * */
    SetHorizontal() {
        if (this.Handle) {
            if (this.IsHorizontal) {
                this.RemoveClass(tp.Classes.Vertical);
                this.AddClass(tp.Classes.Horizontal);
            } else {
                this.RemoveClass(tp.Classes.Horizontal);
                this.AddClass(tp.Classes.Vertical);
            }
        }
    }
 
    /**
    Gets or sets a boolean value indicating whether this is a horizontal splittr. Defaults to false.
    Setting this property sets a proper css class to the element.
    @type {boolean}
    */
    get IsHorizontal() {
        return this.fIsHorizontal === true;
    }
    set IsHorizontal(v) {
        if (v !== this.IsHorizontal) {
            this.fIsHorizontal = Boolean(v);
            this.SetHorizontal();
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
        this.tpClass = 'tp.Splitter';
        this.fDefaultCssClasses = [tp.Classes.Splitter];
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();

        this.SetHorizontal();

        if (tp.IsString(this.Associate))
            this.Associate = tp.Select(this.Associate);

        if (!tp.IsHTMLElement(this.Associate) && tp.IsHTMLElement(this.ParentHandle)) {
            let List = tp.ChildHTMLElements(this.ParentHandle);
            let Index = List.indexOf(this.Handle);
            if (Index > 0) {
                this.Associate = List[Index - 1];
            }
        }

        this.AssociateMinSize = tp.IsEmpty(this.AssociateMinSize) ? 100 : this.AssociateMinSize;
        this.AssociateMaxSize = tp.IsEmpty(this.AssociateMaxSize) ? 500 : this.AssociateMaxSize;

        this.fDragContext = new tp.DragContext(this.Handle, this);
    }

    /* public */
    /**
    Called by the tp.DragContext. The listener returns true for a dragging operation to start. <br />
    The may called either on mouse-down, or on mouse-move while the mouse button is down.
    @protected
    @param {MouseEvent} e A MouseEvent object.
    @returns {boolean} Returning true starts the dragging operation.
    */
    IsDragStart(e) {
        return true;
    }

    /**
    Called by the tp.DragContext.
    @protected
    @param {MouseEvent} e A MouseEvent object.
    */
    DragStart(e) {
        tp.AddClass(this.Document.body, tp.Classes.UnSelectable);

        this.fOldCursor = this.Document.body.style.cursor;
        this.Document.body.style.cursor = this.IsHorizontal ? tp.Cursors.ResizeRow : tp.Cursors.ResizeCol;
    }
    /**
    Called by the tp.DragContext.
    @protected
    @param {MouseEvent} e A MouseEvent object.
    */
    DragMove(e) {
        if (tp.IsHTMLElement(this.Associate)) {

            let P = tp.Mouse.ToElement(e, this.ParentHandle);

            if (this.IsHorizontal) {
                let Y = P.Y;

                Y = Math.max(this.AssociateMinSize, Y);
                Y = Math.min(this.AssociateMaxSize, Y);

                this.Associate.style.height = Y + 'px';
            } else {
                let X = P.X;

                X = Math.max(this.AssociateMinSize, X);
                X = Math.min(this.AssociateMaxSize, X);

                this.Associate.style.width = X + 'px';
            }
        }
    }
    /**
    Called by the tp.DragContext.
    @protected
    @param {MouseEvent} e A MouseEvent object.
    */
    DragEnd(e) {
        tp.RemoveClass(this.Document.body, tp.Classes.UnSelectable);
        this.Document.body.style.cursor = this.fOldCursor;
    }
};
/**
 Private field.
 @private
 @type {tp.DragContext}
 */
tp.Splitter.prototype.fDragContext = null;
/**
 Private field.
 @private
 @type {boolean}
 */
tp.Splitter.prototype.fIsHorizontal = false;
/**
 Private field.
 @private
 @type {string}
 */
tp.Splitter.prototype.fOldCursor = '';

/**
 Public field.
 @public
 @type {HTMLElement}
 */
tp.Splitter.prototype.Associate = null;
/**
 Public field.
 @public
 @type {number}
 */
tp.Splitter.prototype.AssociateMinSize = 100;
/**
 Public field.
 @public
 @type {number}
 */
tp.Splitter.prototype.AssociateMaxSize = 500;
//#endregion  

//#region tp.IFrame
/**
An iframe element <br />
Example markup: <br />
<pre>
    <div>
        <iframe id="IFrame" data-setup="{Width: 800, Height: 600,  Url: 'https://jsdoc.app/'}"></iframe>
    </div>
</pre>
*/
tp.IFrame = class extends tp.tpElement {

    /**
    Constructor
    Example markup: <br />
    <pre>
        <div>
            <iframe id="IFrame" data-setup="{Width: 800, Height: 600,  Url: 'https://jsdoc.app/'}"></iframe>
        </div>
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /**
     * Event handler
     * @protected
     * @param {Event} e An Event object.
     */
    DocumentLoaded(e) {
        if (this.UseSpinner === true) {
            tp.ShowSpinner(false);
        }
    }

    /* properties */
    /**
    Retrieves the document object of the page or frame.
    @type {Document}
    */
    get ContentDoc()  {
        return this.Handle instanceof HTMLIFrameElement ? this.Handle.contentDocument : null;
    }
    /**
    The contentWindow property returns the Window object of an iframe element. You can use this Window object to access the iframe's document and its internal DOM.
    @type {Window}
    */
    get Window()  {
        return this.Handle instanceof HTMLIFrameElement ? this.Handle.contentWindow : null;
    }
    /**
    Gets of sets the width style property. <br />
    Could be string or number. If a number is passed then it is considered as pixels.
    @type {string|number}
    */
    get Width() {
        return this.Handle instanceof HTMLIFrameElement ? this.Handle.width : null;
    }
    set Width(v) {
        if (this.Handle instanceof HTMLIFrameElement) {
            this.Handle.width = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }
    /**
    Gets of sets the height style property. <br />
    Could be string or number. If a number is passed then it is considered as pixels
    @type {string|number}
    */
    get Height() {
        return this.Handle instanceof HTMLIFrameElement ? this.Handle.height : null;
    }
    set Height(v) {
        if (this.Handle instanceof HTMLIFrameElement) {
            this.Handle.height = tp.IsNumber(v) ? tp.px(v) : v;
        }
    }

    /**
    Gets or sets the url to be loaded by this iframe
    @type {string}
    */
    get Url()  {
        return this.fUrl;
    }
    set Url(v) {
        if (this.Handle instanceof HTMLIFrameElement) {

            let SpinnerFlag = tp.IsString(v) && !tp.IsBlank(v) && this.UseSpinner === true;

            if (SpinnerFlag) {
                tp.ShowSpinner(true);
                this.Handle.addEventListener('load', this.FuncBind(this.DocumentLoaded));
            }

            this.fUrl = v;
            this.Handle.src = v;

            if (!tp.IsBlank(v)) {
                // Remove the srcdoc attribute because the presence of the srcdoc overrides any src setting
                // see: http://stackoverflow.com/questions/19739001/which-is-the-difference-between-srcdoc-and-src-datatext-html-in-an
                this.Handle.removeAttribute("srcdoc");
            }
        }

    }
    /**
    Returns a boolean value when the browser supports the srcdoc attribute of the iframe.
    @type {boolean}
    @see {@link https://www.w3schools.com/jsref/prop_frame_srcdoc.asp|w3schools}
    @see {@link https://stackoverflow.com/questions/19739001/which-is-the-difference-between-srcdoc-and-src-datatext-html-in-an|stackoverflow}
    */
    get SupportsSrcDoc() {
        return (this.Handle instanceof HTMLIFrameElement) && ('srcdoc' in this.Handle);
    }
    /**
    Gets or sets the HTML content of the page to show in the iframe.
    @type {string}
    @see {@link https://www.w3schools.com/jsref/prop_frame_srcdoc.asp|w3schools}
    @see {@link https://stackoverflow.com/questions/19739001/which-is-the-difference-between-srcdoc-and-src-datatext-html-in-an|stackoverflow}
    */
    get Content() {
        return this.SupportsSrcDoc ? this.Handle['srcdoc'] : this.Handle.src;
    }
    set Content(v) {
        if (this.Handle instanceof HTMLIFrameElement) {
            if ('srcdoc' in this.Handle) {
                this.Handle['srcdoc'] = v;
            } else {
                var Doc = this.ContentDoc;
                if (Doc) {
                    Doc.open();
                    Doc.write(v);
                    Doc.close();
                } 
            }
        }
    }

    /* oerrrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.IFrame';
        this.fElementType = 'iframe';          // inline frame
        this.fDefaultCssClasses = [tp.Classes.Frame];
    }
    /**
    Notification
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        tp.FrameRemoveBorder(this.Handle);
    }

}; 
/**
 Private field.
 @private
 @type {string}
 */
tp.IFrame.prototype.fUrl = '';
/**
A value indicating whether to display a global spinner while loading a document to the iframe
@public
@type {boolean}
*/
tp.IFrame.prototype.UseSpinner = true;
//#endregion  

//#region tp.DropDownBox
/**
The stage of a dropdown box
@class
@enum {number}
*/
tp.DropDownBoxStage = {
    Opening: 1,
    Opened: 2,
    Closing: 4,
    Closed: 8
};
Object.freeze(tp.DropDownBoxStage);

/**
A resizable dropdown box that can serve any block, inline-block or flex element (Associate) and is controlled by an owner (Owner) object. <br />
The dropdown box it always gets 'fixed' position. <br />
It also recalculates its location in order to be inside the boundaries of the viewport. <br />
It recalculates its location even when its Associate changes location.  <br />
The initialization can be done by passing an object to the CreateParams parameter of the constructor as
<pre>
    var Owner = {
        OnDropDownBoxEvent: function (Sender, Stage) {
            log(tp.EnumNameOf(tp.DropDownBoxStage, Stage));
        },
    };

    var box1 = new tp.DropDownBox('#DropDownBox1', {
        Associate: '#btn1',
        Owner: Owner,
        Width: tp.Select('#btn1').getBoundingClientRect().width,
        Height: 200,
    });
</pre>
*/
tp.DropDownBox = class extends tp.tpElement {

    /**
    Constructor <br />
    The initialization can be done by passing an object to the CreateParams parameter of the constructor as
    <pre>
        var Owner = {
            OnDropDownBoxEvent: function (Sender, Stage) {
                log(tp.EnumNameOf(tp.DropDownBoxStage, Stage));
            },
        };

        var box1 = new tp.DropDownBox('#DropDownBox1', {
            Associate: '#btn1',
            Owner: Owner,
            Width: tp.Select('#btn1').getBoundingClientRect().width,
            Height: 200,
        });
    </pre>
    @param {string|HTMLElement} ElementOrSelector - Optional.
    @param {object} CreateParams - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* protected */


    /* properties */
    /**
    Gets or set the associate element, the element that displays the box.
    @type {HTMLElement}
    */
    get Associate() {
        return this.fAssociate;
    }
    set Associate(v) {
        if (tp.IsString(v)) {
            this.fAssociate = tp.Select(v);
        } else if (tp.IsHTMLElement(v)) {
            this.fAssociate = v;
        }

        let Style = this.GetComputedStyle();
        if (Style.width !== 'auto') {
            if (tp.IsHTMLElement(this.Associate)) {
                this.Width = this.Associate.getBoundingClientRect().width;
            }
        }
    }
    /**
    Gets or sets the owner, an object that gets notified regarding dropdown box stage changes. <br />
    The owner is an object that provides a method as <code>function OnDropDownBoxEvent(Sender, Stage)</code>
    @type {object}
    */
    get Owner() {
        return this.fOwner;
    }
    set Owner(v) {
        if (tp.IsDropDownBoxListener(v)) {
            this.fOwner = v;
        } else {
            let o = null;

            if (tp.IsString(v)) {
                o = tp.Select(v);
            } else if (tp.IsHTMLElement(v)) {
                o = v;
            }

            o = tp.GetObject(o);
            if (tp.IsDropDownBoxListener(o))
                this.fOwner = o;
        }
    }
    /**
    Returns true while the dropdown box is visible
    @type {boolean}
    */
    get IsOpen() {
        return this.HasClass(tp.Classes.Visible);
    }
    /**
    Returns true while the dropdown box is resizing.
    @type {boolean}
    */
    get Resizing() {
        return this.fResizing === true;
    }
    /**
    Returns the dragge, the resizer of the dropdown box.
    @type {tp.Dragger}
    */
    get Dragger() {
        return this.fDragger;
    }

    /* overridables */
    /**
    Called by the Open()/Close() methods and notifies the owner, if any, about a stage change
    @protected
    @param {tp.DropDownBoxStage} Stage The stage of the drop-down
    */
    OnOwnerEvent(Stage) {
        //log(tp.EnumNameOf(tp.DropDownBoxStage, Stage));
        if (tp.IsDropDownBoxListener(this.Owner))
            this.Owner.OnDropDownBoxEvent(this, Stage);
    }
    /**
    Event handler for the dragger. It sets/unsets the resizing flag.
    @protected
    @param {tp.EventArgs} Args The tp.EventArgs args object.
    */
    AnyDraggerEvent(Args) {
        //log(Args.EventName);
        if (tp.IsSameText(tp.Events.DragStart, Args.EventName)) {
            this.fResizing = true;
        } else if (tp.IsSameText(tp.Events.DragEnd, Args.EventName)) {
            setTimeout(function (self) {
                self.fResizing = false;
            }, 600, this);
        }
    }
    /**
    Event handler
    @protected
    @param {Event} e An Event object.
    */
    Window_Scroll(e) {
        if (!tp.ContainsEventTarget(this.Handle, e.target)) {
            if (this.IsOpen && this.StyleProp('position') === 'fixed')
                this.Close();
        }
    }
    /**
    Event handler
    @protected
    @param {Event} e An Event object.
    */
    Document_Click(e) {
        if (!tp.ContainsEventTarget(this.Handle, e.target)) {
            if (this.IsOpen) {
                this.Close();
                e.stopPropagation();
            }                
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

        this.tpClass = 'tp.DropDownBox';
        this.fDefaultCssClasses = [tp.Classes.DropDownBox];
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {

        this.fIsFirstOpen = true;

        this.TabIndex = -1;

        this.fDragger = new tp.Dragger(tp.DraggerMode.Resize, this.Handle);
        this.Dragger.Edges = tp.Edge.Bottom | tp.Edge.Right;
        this.Dragger.MinHeight = 100;
        this.Dragger.MinWidth = 100;

        this.Dragger.On(tp.Events.DragStart, this.FuncBind(this.AnyDraggerEvent));
        this.Dragger.On(tp.Events.DragEnd, this.FuncBind(this.AnyDraggerEvent));

        super.OnInitializationCompleted();

        this.Id = tp.SafeId('DropDown');
    }

    /* public */
    // http://tlindig.github.io/position-calculator/
    // https://github.com/tlindig/position-calculator/blob/master/src/position-calculator.js

    /**
    Displays the dropdown box
    */
    Open() {
        if (!this.IsOpen && tp.IsHTMLElement(this.Associate)) {

            if (tp.IsEmpty(this.ParentHandle)) {
                tp.Doc.body.appendChild(this.Handle);                
            } 
            
            this.Handle.style.font = 'inherit';
            this.Position = 'fixed';            

            if (this.fIsFirstOpen === true) {
                let Style = this.GetComputedStyle();
                if (Style.width !== 'auto')
                    this.Width = this.Associate.getBoundingClientRect().width;
                this.fIsFirstOpen = false;
            }

            this.OnOwnerEvent(tp.DropDownBoxStage.Opening);
 
            this.AddClass(tp.Classes.Visible);

            this.UpdateTop();

            this.ZIndex = tp.MaxZIndexOf(tp.Doc.body);

            this.OnOwnerEvent(tp.DropDownBoxStage.Opened);

            // reposition if outside of viewport
            if (tp.IsSameText('fixed', this.Position)) {
                let Size = tp.Viewport.GetSize();
                let Rect = tp.BoundingRect(this.Handle);      // location (relative to viewport) and size of this instance
 
                if (Rect.Bottom > Size.Height) {
                    this.Y = Rect.Y - Rect.Height;
                }

                if (Rect.Right > Size.Width) {
                    this.X = Size.Width - Rect.Width;
                }
            }              

            setTimeout(() => {
                window.addEventListener('scroll', this.FuncBind(this.Window_Scroll));
                document.addEventListener('click', this.FuncBind(this.Document_Click), true);
            }, 0);


        }
    }
    /**
    Hides the dropdown box
    */
    Close() {
        if (this.IsOpen && !this.Resizing) {
            this.OnOwnerEvent(tp.DropDownBoxStage.Closing);
            this.RemoveClass(tp.Classes.Visible);
            this.OnOwnerEvent(tp.DropDownBoxStage.Closed);

            try {
                window.removeEventListener('scroll', this.FuncBind(this.Window_Scroll));
            } catch (e) {
                //
            }

            try {
                document.removeEventListener('click', this.FuncBind(this.Document_Click), true);
            } catch (e) {
                //
            }
 
        }
    }
    /**
    Displays or hides the dropdown box.
    */
    Toggle() {
        if (!this.IsOpen)
            this.Open();
        else
            this.Close();
    }
    /**
    Updates the top position of the drop-down box
    */
    UpdateTop() {
        if (this.IsOpen === true) {
            let R = this.Associate.getBoundingClientRect();
            let P; // tp.Point

            P = tp.ToViewport(this.Associate);
            this.X = P.X;
            this.Y = P.Y + R.height;

        }
    }
};
/** 
 Private field.
 @private
 @type {HTMLElement}
 */
tp.DropDownBox.prototype.fAssociate = null;
/**
 Private field. An object that provides a method as <code>function OnDropDownBoxEvent(Sender, Stage)</code>
 @private
 @type {object}
 */
tp.DropDownBox.prototype.fOwner = null;
/**
 Private field.
 @private
 @type {tp.Dragger}
 */
tp.DropDownBox.prototype.fDragger = null;
/**
 Private field.
 @private
 @type {boolean}
 */
tp.DropDownBox.prototype.fResizing = false;
/**
 Private field.
 @private
 @type {boolean}
 */
tp.DropDownBox.prototype.fIsFirstOpen = false;
//#endregion  

//#region  tp.VirtualScroller


/**
A virtual scroller helper class for rows with fixed height.
The scroller consists of a viewport HTMLElement which contains and scrolls a child container upon which rows are rendered. The container must be the single child of the viewport.
The RowHeight property defines the height of each row and if it is left unspecified then the height of the first row is used.
@example
var RowCount = 1000;
//var RowHeight = 50;
var ViewportHeight = 400;

var divViewport = tp.Select('#Viewport');
//var divContainer = tp.Select('#Container');
var divContainer = null;

tp.SetStyle(divViewport, {
    position: 'relative',
    left: '40px',
    width: '200px',
    height: ViewportHeight + 'px',
});

var A = [];
for (var i = 0, ln = RowCount; i < ln; i++) {
    var o = { Name: 'row ' + i, Num: i };
    A.push(o);
}

vs = new tp.VirtualScroller(divViewport, divContainer);
//vs.RowHeight = RowHeight;
vs.SetRowList(A);

*/
tp.VirtualScroller = class  {

    /**
    Constructor
    @param {HTMLElement} Viewport - The view-port of the scroller. Contains and scrolls the row container. Must be positioned, i.e. relative or absolute.
    @param {HTMLElement} [Container] - Optional. If not specified the scroller creates a div in its place. The row container where rows are rendered. The container is a child to viewport.
    @param {any[]} [RowList] - Optional. It can be defined later, after the construction. The list of rows to render.
    */
    constructor(Viewport, Container, RowList) {
        this.fViewport = Viewport;
        if (!Container) {
            Container = Viewport.ownerDocument.createElement('div');
            Container.tabIndex = -1;
            Viewport.appendChild(Container);
        }
        this.fContainer = Container;

        this.RowCache = {};
        this.RowList = RowList || [];

        //this.fViewport.style.cssFloat = 'left';
        this.fViewport.style.overflow = 'auto';

        this.fContainer.style.position = 'relative';
        this.fContainer.style.overflow = 'hidden';

        this.RenderBind = this.Render.bind(this);
        this.fViewport.addEventListener('scroll', this.RenderBind, false);
    }

 

    /** 
     Gets the viewport HTMLElement.
     @type {HTMLElement}
     */
    get Viewport() {
        return this.fViewport;
    }
    /**
     Gets the container HTMLElement.
     @type {HTMLElement}
     */
    get Container() {
        return this.fContainer;
    }
    /**
    Returns the number of row in the internal row list.
    @type {number}
    */
    get RowListCount() {
        return tp.IsArray(this.RowList) ? this.RowList.length : 0;
    }

    /**
    Callback that renders a row and returns an HTMLElement
    @param {any} Row - An element from the RowList array
    @param {number} RowIndex - The index of the Row in the RowList array
    @param {number} RowHeight - The height of the row as a number (in pixels)
    @returns {HTMLElement} Returns an HTMLElement
    */
    RenderRowFunc(Row, RowIndex, RowHeight) {
        let div = document.createElement('div');
        div.tabIndex = -1;
        tp.SetStyle(div, {
            'border-bottom': '1px dotted lightgray',
            left: '0',
            'font-size': '9pt',
            'width': '100%',
            'height': RowHeight + 'px',
            'padding': '4px'
        });

        div.innerHTML = 'row ' + RowIndex;
        return div;
    }
    /**
    Callback that is called before and after scroll.
    @param {number} Phase - Denotes the call phase. 1 means before scroll where 2 means after scroll.
    */
    ScrollFunc(Phase) {
    }



    /* private */
    /**
     * Clears the row cache
     * @private
     * */
    ClearCache() {
        var el, // HTMLElement
            Prop;

        for (Prop in this.RowCache) {
            el = this.RowCache[Prop];
            el.parentNode.removeChild(el);
            delete this.RowCache[Prop];
        }
    }
    /**
     * Renders the scroller
     * @private
     * */
    Render() {
        //requestAnimationFrame(this.RenderBind);

        if (this.RowList.length > 0 && this.LastScrollTop !== this.fViewport.scrollTop) {
            tp.Call(this.ScrollFunc, this.Context, 1);

            this.LastScrollTop = this.fViewport.scrollTop;

            // calculate the viewport elevator  
            var H = this.ContainerHeight;
            var T = this.fViewport.scrollTop;
            var R = this.fViewport.getBoundingClientRect();
            var B = T + R.height;

            var Top = Math.abs(Math.floor(T / this.RowHeight)) - 5;
            Top = Math.max(0, Top);

            var Bottom = Math.abs(Math.ceil(B / this.RowHeight)) + 5;
            Bottom = Math.min(this.ContainerHeight / this.RowHeight, Bottom);

            // remove rows no longer in the viewport
            var el,  // HTMLElement
                Prop,
                Index;

            for (Prop in this.RowCache) {
                Index = Number(Prop);
                if (Index < Top || Index > Bottom) {
                    el = this.RowCache[Prop];
                    el.parentNode.removeChild(el);
                    delete this.RowCache[Prop];
                }
            }

            // add new rows
            var Length = this.RowList.length;
            for (Index = Top; Index <= Bottom; Index++) {
                if ((Index >= 0) && (Index <= Length - 1) && !this.RowCache[Index]) {
                    el = this.RenderRow(Index);

                    el.style.position = 'absolute';
                    el.style.top = (Index * this.RowHeight) + 'px';
                    el.style.height = this.RowHeight + 'px';

                    this.fContainer.appendChild(el);
                    this.RowCache[Index] = el;
                }
            }

            this.IndexTop = Top;
            this.IndexBottom = Bottom;

            tp.Call(this.ScrollFunc, this.Context, 2);
        }
    }
    /**
     * Renders a row and returns the HTMLELement.
     * @param {number} RowIndex The index of row.
     * @returns {HTMLElement} Returns the new HTMLElement.
     */
    RenderRow(RowIndex) {
        var Row = this.RowList[RowIndex];
        var div = tp.Call(this.RenderRowFunc, this.Context, Row, RowIndex, this.RowHeight);
        return div;
    }

    /* methods */
    /**
    Sets the list of the rows to display.
    @param {any[]} RowList - An array with user data
    */
    SetRowList(RowList) {

        this.ClearCache();

        this.IndexTop = 0;
        this.IndexBottom = 0;
        this.LastScrollTop = null;
        this.fViewport.scrollTop = 0;

        this.RowList = RowList || [];

        // calculate the row height
        if (tp.IsEmpty(this.RowHeight) && RowList.length > 0) {
            let el = this.RenderRow(0);
            this.fContainer.appendChild(el);
            this.RowHeight = el.getBoundingClientRect().height;
            this.fContainer.removeChild(el);
        }

        if (tp.IsEmpty(this.RowHeight))
            this.RowHeight = 32;

        if (!tp.IsEmpty(this.RowList)) {
            this.ContainerHeight = this.RowList.length * this.RowHeight;
            this.fContainer.style.height = this.ContainerHeight + 'px';

            this.Render();
        }
    }
    /**
     * Returns the array of rows.
     * @returns {any[]} Returns the array of rows.
     * */
    GetRowList() {
        return !tp.IsEmpty(this.RowList) ? this.RowList.slice() : [];
    }
    /** Forces a re-render of the scroller. */
    Update() {
        //requestAnimationFrame(this.RenderBind);
        this.ClearCache();

        this.IndexTop = 0;
        this.IndexBottom = 0;
        this.LastScrollTop = null;
        this.fViewport.scrollTop = 0;

        if (!tp.IsEmpty(this.RowList)) {
            this.ContainerHeight = this.RowList.length * this.RowHeight;
            this.fContainer.style.height = this.ContainerHeight + 'px';

            this.Render();
        }
    }


};
/**
 Private field.
 @private
 @type {HTMLElement}
 */
tp.VirtualScroller.prototype.fViewport = null;
/**
 Private field.
 @private
 @type {HTMLElement}
 */
tp.VirtualScroller.prototype.fContainer = null;
/**
 Private field.
 @private
 @type {any[]}
 */
tp.VirtualScroller.prototype.RowList = [];
/**
 Private field.
 @private
 @type {number}
 */
tp.VirtualScroller.prototype.ContainerHeight = 0;
/**
 Private field.
 @private
 @type {number}
 */
tp.VirtualScroller.prototype.LastScrollTop = 0;
/**
 Private field. A literal object as { [Key: number]: HTMLElement }
 @private
 @type {object}
 */
tp.VirtualScroller.prototype.RowCache = { }; // { [Key: number]: HTMLElement }
/**
 Private field.
 @private
 @type {number}
 */
tp.VirtualScroller.prototype.IndexTop = 0;
/**
 Private field.
 @private
 @type {number}
 */
tp.VirtualScroller.prototype.IndexBottom = 0;
/**
 Private field.
 @private
 @type {any}
 */
tp.VirtualScroller.prototype.RenderBind = null;

/**
The height of a row. If not specified then the height of the first row is used.
 @public
 @type {number}
*/
tp.VirtualScroller.prototype.RowHeight = null;
/**
Context (this) for the callbacks
 @public
 @type {object}
*/
tp.VirtualScroller.prototype.Context = null;
//#endregion  

//---------------------------------------------------------------------------------------
// menus
//---------------------------------------------------------------------------------------

//#region  tp.MenuItemType

/**
A static enum-like class. Indicates the type of a menu item
 @class
 @enum {number}
*/
tp.MenuItemType = {
    /**
    MenuItem
    @type {number}
    */
    Item: 1,
    /**
    MenuSeparator
    @type {number}
    */
    Separator: 2
};
Object.freeze(tp.MenuItemType);
//#endregion

//#region  tp.MenuEventArgs
/**
EventArgs for menu events
*/
tp.MenuEventArgs = class extends tp.EventArgs {

    /**
    Constructor 
    @param {tp.MenuItem} MenuItem - The menu item
    @param {Event} [e] Optional. The event.
    */
    constructor(MenuItem, e = null) {
        super('', null);

        this.MenuItem = MenuItem;
        this.e = e;
        this.Command = MenuItem.Command;
    }

    /* properties */

    /**
    The menu item
    @type {tp.MenuItem}
    */
    MenuItem = null;

};
//#endregion

//#region  tp.MenuItemList
/**
Internal private class. Is the child menu item container or a parent menu item.
*/
tp.MenuItemList = class {

    /**
    Constructor.
    @param {HTMLElement} Handle - The HTMLElement that is the container for the child menu items
    @param {Object} Owner - An IMenuItemParent implementor that acts as the owner of this list.
    */
    constructor(Handle, Owner) {
        this.fHandle = Handle;
        this.fOwner = Owner;
        this.fItems = []; // tp.MenuItemBase[]

        if (!Owner.IsMenu)
            this.fHandle.style.display = 'none';
    }

    fOwner = null;  // IMenuItemParent;
    fHandle = null; // HTMLElement;
    fItems = [];    // tp.MenuItemBase[]

    /**
    Returns the handle of this instance
    @type {HTMLElement}
    */
    get Handle() {
        return this.fHandle;
    }
    /**
    Returns the owner of this instance
    @type {object}
    */
    get Owner() {
        return this.fOwner;
    }
    /**
    Returns the number of menu items in this list
    @type {number}
    */
    get Count() {
        return this.fItems.length;
    }

    /* protected */
    /**
    Throws an exception if a specified menu item has not a Handle property referencing a valid HTMLElement
    @protected
    @param {tp.MenuItemBase} Item The item to check.
    */
    CheckHandle(Item) {
        if (!tp.IsHTMLElement(Item.Handle)) {
            tp.Throw('Can not operate on a menu item without handle');
        }
    }

    /* public */
    /**
   Returns the index of a specified child menu item, if any, else -1.
   @param {tp.MenuItemBase} Item The item
   @returns {number} Returns the index of a specified child menu item, if any, else -1.
   */
    IndexOf(Item) {
        return this.fItems.indexOf(Item);
    }
    /**
    Returns a child menu item found at a specified index, if any, else null
    @param {number} Index The index
    @returns {tp.MenuItemBase} Returns a child menu item found at a specified index, if any, else null
    */
    ByIndex(Index) {
        return this.fItems[Index];
    }
    /**
    Returns a child menu item having a specified Command, if any, else null
    @param {string} Command The command
    @returns {tp.MenuItem} Returns a child menu item having a specified Command, if any, else null
    */
    ByCommand(Command) {
        let Result = tp.FirstOrDefault(this.fItems, (item) => { return tp.IsSameText(Command, item['Command']); });
        return Result instanceof tp.MenuItem ? Result : null;
    }

    /**
    Returns true if this parent contains a specified child menu item
    @param {tp.MenuItemBase} Item The item
    @returns {boolean} Returns true if this parent contains a specified child menu item
    */
    Contains(Item) {
        return this.fItems.indexOf(Item) !== -1;
    }
    /**
    Removes a specified child menu item
    @param {tp.MenuItemBase} Item The item
    */
    Remove(Item) {
        if (this.Contains(Item)) {
            this.CheckHandle(Item);
            tp.ListRemove(this.fItems, Item);
            Item.Handle.parentNode.removeChild(Item.Handle);
        }
    }
    /**
    Removes all child menu items
    */
    Clear() {
        for (let i = 0, ln = this.fItems.length; i < ln; i++) {
            this.Remove(this.fItems[i]);
        }
    }

    /**
    Adds a specified menu item as a child to this parent
    @param {tp.MenuItemBase} Item The item
    */
    Add(Item) {
        this.Insert(this.fItems.length, Item);
    }
    /**
    Adds a menu item as a child to this parent
    @param {string} Text - The display text of the menu item
    @param {string} [Command] Optional. The command of the menu item
    @returns {tp.MenuItemBase} Returns the newly added menu item
    */
    AddMenuItem(Text, Command) {
        let Result = tp.MenuItemBase.CreateMenuItem(Text);
        Result.Command = Command;
        this.Add(Result);
        return Result;
    }
    /**
    Adds and returns a separator
    @returns {tp.MenuSeparator} Returns the separator
    */
    AddSeparator() {
        let Result = tp.MenuItemBase.CreateSeparator();
        this.Add(Result);
        return Result;
    }

    /**
    Inserts a specified menu item as a child to this parent, at a specified index.
    @param {number} Index The index
    @param {tp.MenuItemBase} Item The item
    */
    Insert(Index, Item) {
        if (!this.Contains(Item)) {
            this.CheckHandle(Item);

            Item.Parent = null;

            if (this.fItems.length === Index) {
                this.fItems.push(Item);
                this.Handle.appendChild(Item.Handle);
            } else {
                let RefNode = this.Handle.children[Index];
                this.Handle.insertBefore(Item.Handle, RefNode);
                tp.ListInsert(this.fItems, Index, Item);
            }

            Item.Parent = this.Owner;
        }

    }
    /**
    Inserts a menu item as a child to this parent, at a specified index.
    @param {number} Index The index
    @param {string} Text - The display text of the menu item
    @param {string} [Command] - Optional. The command of the menu item
    @returns {tp.MenuItemBase} Returns the newly added menu item
    */
    InsertMenuItem(Index, Text, Command)  {
        let Result = tp.MenuItemBase.CreateMenuItem(Text);
        Result.Command = Command;
        this.Insert(Index, Result);
        return Result;
    }
    /**
    Inserts and returns a separator, at a specified index.
    @param {number} Index The index
    @returns {tp.MenuSeparator} Returns the separator
    */
    InsertSeparator(Index)  {
        let Result = tp.MenuItemBase.CreateSeparator();
        this.Insert(Index, Result);
        return Result;
    }
};
//#endregion

//#region  tp.MenuItemBase
/**
Base class for the MenuItem and MenuSeparator
*/
tp.MenuItemBase = class extends tp.tpObject {

    /**
    Constructor
    @param {tp.MenuItemType} Type - Indicates the type of the menu item. One of the {@link tp.MenuItemType} constants
    @param {HTMLElement} [Handle] - Optional. The HTMLElement this menu item represents
    */
    constructor(Type, Handle) {
        super();

        this.fType = Type;
        this.Handle = Handle;

        if (this.fType === tp.MenuItemType.Separator && this.Handle) {
            this.Handle.innerHTML = '';
        }

        if (this.Handle)
            this.NormalizeHandle();
    }

    /* private */
    fParent = null; // tp.IMenuItemParent;
    fType = tp.MenuItemType.Item;
    fItems = null; // tp.MenuItemList;

    /* protected */
    fImageElement = null;       // HTMLElement;
    fTextElement = null;        // HTMLAnchorElement;
    fArrowElement = null;       // HTMLElement;
    fListElement = null;        // HTMLElement;
    fSeparatorElement = null;   // HTMLElement;

    /* properties */
    /**
    The HTMLElement this menu item represents. Treat it as a read-ony property
    @type {HTMLElement}
    */
    Handle = null;

    /**
    Gets or sets the display text of the item. Valid ony for a tp.MenuItem
    @type {string}
    */
    get Text() {
        return this.IsMenuItem ? this.fTextElement.innerHTML : '-';
    }
    set Text(v) {
        if (this.IsMenuItem)
            this.fTextElement.innerHTML = v;
    }
    /**
    Hides or displays the menu item
    @type {boolean}
    */
    get Visible() {
        return this.Handle.style.display === '';
    }
    set Visible(v) {
        this.Handle.style.display = v === true ? '' : 'none';
    }
    /**
    Enables or disables the menu item
    @type {boolean}
    */
    get Enabled() {
        return tp.Enabled(this.Handle) === true;
    }
    set Enabled(v) {
        tp.Enabled(this.Handle, v);
    }

    /**
    Gets or sets the parent of this menu item
    @type {tp.IMenuItemParent}
    */
    get Parent() {
        return this.fParent;
    }
    set Parent(v) {
        if (!tp.IsEmpty(this.fParent)) {
            this.fParent.Remove(this);
            this.fParent = null;
        }

        this.fParent = v;

        if (!tp.IsEmpty(this.fParent)) {
            this.fParent.Add(this);
        }
    }
    /**
    Returns the menu or context menu this menu item belongs to.
    @type {tp.MenuBase}
    */
    get Menu() {

        let Parent = this.Parent;
        while (true) {

            if (tp.IsEmpty(Parent))
                break;

            if (Parent instanceof tp.MenuBase)
                return Parent;

            if (Parent instanceof tp.MenuItemBase)
                Parent = Parent.Parent;
        }

        return null;
    }

    /**
    Returns a boolean value indicating whether this instance is a menu item
    @type {boolean}
    */
    get IsMenuItem() {
        return !this.IsSeparator;
    }
    /**
    Returns a boolean value indicating whether this instance is a menu separator
    @type {boolean}
    */
    get IsSeparator() {
        return this.fType === tp.MenuItemType.Separator;
    }
    /**
    Returns a boolean value indicating whether this instance is a main menu
    @type {boolean}
    */
    get IsMenu() {
        return false;
    }
    /**
    Returns a boolean value indicating whether this instance is a context menu
    @type {boolean}
    */
    get IsContextMenu() {
        return false;
    }

    /**
    Returns a boolean value indicating whether this instance is has child menu items
    @type {boolean}
    */
    get HasChildren() {
        return this.Count > 0;
    }
    /**
    Returns the number of the child menu items
    @type {number}
    */
    get Count() {
        return this.IsMenuItem ? this.fItems.Count : 0;
    }
    /**
    Gets or sets a user defined value
    @type {any}
    */
    Tag = null;

    /* static private */


    /**
     * Ensures that a specified text is not null or empty
     * @private
     * @param {string} Text The text to check
     * @return {string} Returns the text 
     */
    static EnsureMenuItemText(Text) {
        if (tp.IsBlank(Text)) {
            Text = 'MenuItem ' + this.MenuItemCounter++;
        }
        return Text;
    }

    /*static */
    /**
    Creates and returns a menu item
    @param {string} Text - The display text of the menu item
    @returns {tp.MenuItem} Returns the newly create item
    */
    static CreateMenuItem(Text) {
        let el = document.createElement('div');
        el.innerHTML = this.EnsureMenuItemText(Text);
        let Result = new tp.MenuItem(el);
        return Result;
    }
    /**
    Creates and returns a menu separator
    @returns {tp.MenuSeparator} Returns the newly create menu separator
    */
    static CreateSeparator() {
        let el = document.createElement('div');
        let Result = new tp.MenuSeparator(el);
        return Result;
    }
    /**
    Returns true if a specified element is a menu separator, that is it just contains a - character.
    Used in the construction phase, when reading the provided markup.
    @param {HTMLElement} el The element to check
    @returns {boolean} Returns true if a specified element is a menu separator, that is it just contains a - character.
    */
    static IsSeparator(el) {
        let T = tp.FindTextNode(el);
        let S = T ? T.nodeValue : '';
        return S === '-';
    }

    /* private */
    /**
    Throws an excepction if this menu item base is NOT a tp.MenuItem
    @private
    */
    CheckIsMenuItem() {
        if (!this.IsMenuItem) {
            tp.Throw('Invalid menu operation. This is not a menu item');
        }
    }
    /**
    Creates the HTMLElement elements needed for this menu item.
    @protected
    @param {string} Text - The display text of the menu item.
    */
    CreateElements(Text) {
        // image
        this.fImageElement = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.fImageElement);

        if (this.IsSeparator) {
            // separator
            this.fSeparatorElement = this.Handle.ownerDocument.createElement('hr');
            this.Handle.appendChild(this.fSeparatorElement);
        } else {

            // text
            this.fTextElement = this.Handle.ownerDocument.createElement('a');
            this.Handle.appendChild(this.fTextElement);
            this.fTextElement.href = 'javascript:void(0);';
            if (!tp.IsBlank(Text))
                this.fTextElement.innerHTML = Text;

            // arrow
            this.fArrowElement = this.Handle.ownerDocument.createElement('div');
            this.Handle.appendChild(this.fArrowElement);

            // list
            this.fListElement = this.Handle.ownerDocument.createElement('div');
            this.Handle.appendChild(this.fListElement);

            this.fItems = new tp.MenuItemList(this.fListElement, this);
        }
    }
    /**
    Reads the provided markup, creates the HTMLElements needed and applies any supplied create params
    @protected
    */
    NormalizeHandle() {
        let T = tp.FindTextNode(this.Handle);
        let S = T ? T.nodeValue || '' : '';
        if (T) {
            T.nodeValue = '';
        }

        S = tp.Trim(S);

        this.Handle.className = this.IsSeparator ? tp.Classes.MenuSeparator : tp.Classes.MenuItem;
        tp.SetObject(this.Handle, this);

        let List = tp.ChildHTMLElements(this.Handle);

        if (List.length === 0) {
            this.CreateElements(S);
        } else {

            for (let i = 0, ln = List.length; i < ln; i++) {
                List[i].parentNode.removeChild(List[i]);
            }

            this.CreateElements(S);

            if (!this.IsSeparator) {
                let MI = null; // tp.MenuItemBase;

                for (let i = 0, ln = List.length; i < ln; i++) {
                    MI = tp.MenuItem.IsSeparator(List[i]) ? new tp.MenuSeparator(List[i]) : new tp.MenuItem(List[i]);
                    MI.fParent = this;
                    this.fItems.Add(MI);
                }
            }
        }



    }

    /**
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() {
        return this.Text;
    }

    /**
    Returns the index of a specified child menu item, if any, else -1.
    @param {tp.MenuItemBase} Item The menu item
    @returns {number} Returns the index of a specified child menu item, if any, else -1.
    */
    IndexOf(Item) {
        this.CheckIsMenuItem();
        return this.fItems.IndexOf(Item);
    }
    /**
    Returns a child menu item found at a specified index, if any, else null
    @param {number} Index The index
    @returns {tp.MenuItemBase} Returns a child menu item found at a specified index, if any, else null
    */
    ByIndex(Index) {
        this.CheckIsMenuItem();
        return this.fItems.ByIndex(Index);
    }
    /**
    Returns a child menu item having a specified Command, if any, else null
    @param {string} Command The command
    @returns {tp.MenuItem} Returns a child menu item having a specified Command, if any, else null
    */
    ByCommand(Command) {
        return this.fItems.ByCommand(Command);
    }

    /**
    Returns true if this parent contains a specified child menu item
    @param {tp.MenuItemBase} Item The menu item
    @returns {boolean} Returns true if this parent contains a specified child menu item
    */
    Contains(Item) {
        this.CheckIsMenuItem();
        return this.fItems.Contains(Item);
    }
    /**
    Removes a specified child menu item
    @param {tp.MenuItemBase} Item The menu item
    */
    Remove(Item) {
        this.CheckIsMenuItem();
        this.fItems.Remove(Item);
    }
    /**
    Removes all child menu items
    */
    Clear() {
        this.CheckIsMenuItem();
        this.fItems.Clear();
    }

    /**
    Adds a specified menu item as a child to this parent
    @param {tp.MenuItemBase} Item The menu item
    */
    Add(Item) {
        this.CheckIsMenuItem();
        this.fItems.Add(Item);
    }
    /**
    Adds a menu item as a child to this parent
    @param {string} Text - The display text of the menu item
    @param {string} [Command] - Optional. The command of the menu item
    @returns {tp.MenuItemBase} Returns the newly added menu item
    */
    AddMenuItem(Text, Command) {
        this.CheckIsMenuItem();
        return this.fItems.AddMenuItem(Text, Command);
    }
    /**
    Adds and returns a separator
    @returns {tp.MenuSeparator} Returns the separator
    */
    AddSeparator() {
        this.CheckIsMenuItem();
        return this.fItems.AddSeparator();
    }

    /**
    Inserts a specified menu item as a child to this parent, at a specified index.
    @param {number} Index The item index
    @param {tp.MenuItemBase} Item The menu item
    @return {tp.MenuItemBase} Returns the newly added menu item
    */
    Insert(Index, Item) {
        this.CheckIsMenuItem();
        return this.fItems.Insert(Index, Item);
    }
    /**
    Inserts a menu item as a child to this parent, at a specified index.
    @param {number} Index The item index
    @param {string} Text - The display text of the menu item
    @param {string} [Command] - Optional. The command of the menu item
    @returns {tp.MenuItemBase} Returns the newly added menu item
    */
    InsertMenuItem(Index, Text, Command) {
        this.CheckIsMenuItem();
        return this.fItems.InsertMenuItem(Index, Text, Command);
    }
    /**
    Inserts and returns a separator, at a specified index.
    @param {number} Index The item index
    @returns {tp.MenuSeparator} Returns the separator
    */
    InsertSeparator(Index) {
        this.CheckIsMenuItem();
        return this.fItems.InsertSeparator(Index);
    }

    /**
    Creates the handle (HTMLElement) of this instance if not already created
    */
    CreateHandle() {
        if (tp.IsEmpty(this.Handle)) {
            this.Handle = document.createElement('div');
            this.NormalizeHandle();
        }
    }
};

// Firefox v.69 does NOT support static keyword in fields

/** MenuItemCounter
    @private
    @type {number}*/
tp.MenuItemBase.MenuItemCounter = 0;
//#endregion

//#region  tp.MenuItem
/**
Represents a menu item <br />
@implements {tp.ICommandProperty}
Example markup
@example
<pre>
    <div>
        File
        <div>Open</div>
        <div>New</div> 
    </div>
</pre>
For each menu item provided by markup, an element with 4 containers is produced: 1. Image, 2. Text, 3. Arrow, 4. List <br />
Example of produced markup
@example
<pre>
    <div class="tp-MenuItem">
        <div></div>
        <a>File</a>
        <div></div>
        <div style="display: none;">
            <div class="tp-MenuItem">
                <div></div>
                <a>Open</a>
                <div></div>
                <div style="display: none;"></div>
            </div>
            <div class="tp-MenuItem">
                <div></div>
                <a>New</a>
                <div></div>
                <div style="display: none;"></div>
            </div>
        </div>
    </div>
</pre>
*/
tp.MenuItem = class extends tp.MenuItemBase {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <div>
            File
            <div>Open</div>
            <div>New</div>
        </div>
    </pre>
    @param {HTMLElement} [Handle] - Optional. The handle of the element
    */
    constructor(Handle) {
        super(tp.MenuItemType.Item, Handle);
    }

    /* protected */

    /**
     Ico classes
     @protected
     @type {string}
     */
    fIcoClasses = null;
    /** Image Url
     @protected
     @type {string}
     */
    fImageUrl = null;

    /** Controls the visibility of the drop-down list
     * @protected
     * @type {boolean}
     */
    get IsListVisible() {
        return this.fListElement.style.display === '';
    }
    set IsListVisible(v) {
        let Flag = v === true;
        this.fListElement.style.display = Flag ? '' : 'none';
        if (Flag)
            tp.BringToFront(this.fListElement);
    }

    /* properties */
    /**
    Gets or sets a value indicating whether this menu item is enabled
    @type {boolean}
    */
    get Enabled() {
        return tp.Enabled(this.Handle);
    }
    set Enabled(v) {
        tp.Enabled(this.Handle, v);
        if (this.Handle) {
            tp.Enabled(this.fImageElement, v);
            tp.Enabled(this.fTextElement, v);
        }
    }
    /**
    The text element of a menu item is an anchor element.
    This property gets or sets the value of the href property of that anchor element.
    @type {string}
    */
    get Url() {
        if (this.fTextElement instanceof HTMLAnchorElement) {
            return this.fTextElement.href !== 'javascript:void(0);' ? this.fTextElement.href : '';
        }

        return '';
    }
    set Url(v) {
        if (this.fTextElement instanceof HTMLAnchorElement) {
            if (tp.IsEmpty(v) || tp.IsBlank(v)) {
                this.fTextElement.href = 'javascript:void(0);';
            } else {
                this.fTextElement.href = v;
            }
        }
    }
    /**
    Gets or sets the ico css classes, e.g. fa fa-xxxxx, for the ico
    @type {string}
    */
    get IcoClasses() {
        return this.fIcoClasses;
    }
    set IcoClasses(v) {
        if (tp.IsHTMLElement(this.fImageElement)) {
            tp.RemoveClasses(this.fImageElement, this.fIcoClasses);
            this.fImageElement.style.background = '';
            this.fImageUrl = '';
            tp.AddClasses(this.fImageElement, v);
        }

        this.fIcoClasses = v;
    }
    /**
    Gets or sets a url for the item ico
    @type {string}
    */
    get ImageUrl() {
        return this.fImageUrl;
    }
    set ImageUrl(v) {
        if (tp.IsHTMLElement(this.fImageElement)) {
            tp.RemoveClasses(this.fImageElement, this.fIcoClasses);
            this.fImageElement.style.background = '';
            this.fIcoClasses = '';

            tp.SetStyle(this.fImageElement, {
                'background-image': tp.Format('url("{0}")', v),
                'background-repeat': 'no-repeat',
                'background-position': 'center center',
                'background-size': '75%'
            });
        }
        this.fImageUrl = v;
    }
    /**
    Gets or sets a user defined string 
    @type {string}
    */
    Command = null;
    /**
    Returns the text element which is a {@link HTMLAnchorElement}
    @type {HTMLAnchorElement}
    */
    get TextElement() {
        return this.fTextElement;
    }

    /* protected */
    /**
    Reads the provided markup, creates the HTMLElements needed and applies any supplied create params
    @protected
    @override
    */
    NormalizeHandle() {
        super.NormalizeHandle();

        // process create params
        let CreateParams = tp.Data(this.Handle, 'setup');
        if (!tp.IsBlank(CreateParams)) {
            CreateParams = eval("(" + CreateParams + ")");                            // compile it

            this.Handle.removeAttribute('data-setup');

            let A = ['Enabled', 'IcoClasses', 'ImageUrl', 'Command', 'Url'];
            for (let i = 0, ln = A.length; i < ln; i++) {
                if (A[i] in CreateParams) {
                    this[A[i]] = CreateParams[A[i]];
                }
            }
        }

        tp.On(this.Handle, tp.Events.Click, this.FuncBind(this.OnItemClick));
        tp.On(this.Handle, tp.Events.MouseEnter, this.FuncBind(this.OnMouseEnter));
        tp.On(this.Handle, tp.Events.MouseLeave, this.FuncBind(this.OnMouseLeave));
    }


    /* Event handlers */
    /**
    Event handler
    @protected
    @param {MouseEvent} e The event
    */
    OnItemClick(e) {
        //e.preventDefault(); // NO, let the click do its default because the menu item may have a href defined

        if (this.Menu instanceof tp.MenuBase) {


            if (this.IsListVisible) {
                this.OnMouseLeave(e);
            }
            else {
                this.OnMouseEnter(e);
            }

            let Parent = this.Parent;
            while (true) {

                if (tp.IsEmpty(Parent))
                    break;

                if (Parent instanceof tp.MenuBase) {
                    if (Parent instanceof tp.ContextMenu) {
                        Parent.Visible = false;
                    }

                    break;
                }


                if (Parent instanceof tp.MenuItem) {
                    if (Parent.IsListVisible)
                        Parent.IsListVisible = false;

                    Parent = Parent.Parent;
                }

            }

            if (!tp.IsString(this.Url) || tp.IsBlank(this.Url)) {
                e.preventDefault();
            }

            this.Menu.OnItemClick(e, this);

            e.stopPropagation();

        }
        else {
            tp.CancelEvent(e);
        }

    }
    /**
    Event handler
    @protected
    @param {MouseEvent} e The event
    */
    OnMouseEnter(e) {

        if (this.Enabled && this.Menu && this.Menu.Enabled && this.Menu.HasFocused && this.HasChildren && this.IsListVisible === false) {
            this.IsListVisible = true;

            if (this.Menu.IsMenu && this.Menu.Contains(this)) {
                this.fListElement.style.left = '1px';
                this.fListElement.style.top = tp.px(this.Menu.OffsetSize.Height - 4);
            } else {
                this.fListElement.style.left = '100%';
                tp.StyleProp(this.fListElement, 'margin-top', '2px');
                tp.StyleProp(this.fListElement, 'margin-left', '-3px');
                //this.ListContainer.ZIndex = this.ZIndex + 1;
            }
        }
    }
    /**
    Event handler
    @protected
    @param {MouseEvent} e The event
    */
    OnMouseLeave(e) {

        if (this.Enabled && this.Menu && this.Menu.Enabled && this.HasChildren && this.IsListVisible === true) {
            this.IsListVisible = false;
        }
    }


    /* public */
    /**
    Removes a specified child menu item
    @param {tp.MenuItemBase} Item The menu item
    */
    Remove(Item) {
        // 
    }

};
//#endregion

//#region  tp.MenuSeparator
/**
Represents a menu item separator <br />
Example markup
@example
<pre>
    <div>
        File
        <div>Open</div>
        <div>-</div>
        <div>Exit</div>
    </div>
</pre>
For each menu item provided by markup, an element with 2 containers is produced: 1. Image, 2. Separator (a hr element) <br />
Example of produced markup
@example
<pre>
    <div class="tp-MenuItem">
        <div></div>
        <a>File</a>
        <div></div>
        <div style="display: none;">
            <div class="tp-MenuItem">
                <div></div>
                <a>Open</a>
                <div></div>
                <div style="display: none;"></div>
            </div> 
            <div class="tp-MenuSeparator">
                <div></div>
                <hr>
            </div>
            <div class="tp-MenuItem">
                <div></div>
                <a>Exit</a>
                <div></div>
                <div style="display: none;"></div>
            </div>
        </div>
    </div>
</pre>
*/
tp.MenuSeparator = class extends tp.MenuItemBase {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <div>
            File
            <div>Open</div>
            <div>-</div>
            <div>Exit</div>
        </div>
    </pre>
    @param {HTMLElement} [Handle] - Optional. The handle of the element
    */
    constructor(Handle) {
        super(tp.MenuItemType.Separator, Handle);
    }
};
//#endregion

//#region  tp.MenuBase
/**
Base class for the {@link tp.Menu} (main menu) and {@link tp.ContextMenu} (local menu)
*/
tp.MenuBase = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup
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
    Returns a boolean value indicating whether this instance is a menu item
    @type {boolean}
    */
    get IsMenuItem() {
        return false;
    }
    /**
    Returns a boolean value indicating whether this instance is a menu separator
    @type {boolean}
    */
    get IsSeparator() {
        return false;
    }
    /**
    Returns a boolean value indicating whether this instance is a main menu
    @type {boolean}
    */
    get IsMenu() {
        return false;
    }
    /**
    Returns a boolean value indicating whether this instance is a context menu
    @type {boolean}
    */
    get IsContextMenu() {
        return false;
    }

    /**
    Returns a boolean value indicating whether this instance is has child menu items
    @type {boolean}
    */
    get HasChildren() {
        return this.Count > 0;
    }
    /**
    Returns the number of the child menu items
    @type {number}
    */
    get Count() {
        return !tp.IsEmpty(this.fItems) ? this.fItems.Count : 0;
    }
    /**
    Gets or sets a user defined value
    */
    Tag = null;

    /* overrides */
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.TabIndex = 0;

        this.fItems = new tp.MenuItemList(this.Handle, this);

        let List = tp.ChildHTMLElements(this.Handle);

        for (let i = 0, ln = List.length; i < ln; i++) {
            List[i].parentNode.removeChild(List[i]);
        }

        let MI = null; // tp.MenuItemBase;

        for (let i = 0, ln = List.length; i < ln; i++) {
            MI = tp.MenuItem.IsSeparator(List[i]) ? new tp.MenuSeparator(List[i]) : new tp.MenuItem(List[i]);
            MI.Parent = this;
        }

    }

    /* overridables */
    /**
     * Finds a {@link tp.MenuItem} based on a specified {@link HTMLElement}
     * @protected
     * @param {HTMLElement} el The element
     * @return {tp.MenuItem} Returns the menu itme or null.
     */
    FindMenuItemByElement(el) {
        el = tp.Closest(el, '.' + tp.Classes.MenuItem);

        if (el && tp.ContainsElement(this.Handle, el)) {
            let o = tp.GetObject(el);
            if (o instanceof tp.MenuItem)
                return o;
        }

        return null;
    }
    /** Event handler
     * @protected
     * @param {MouseEvent} e The event object
     */
    OnDocumentClick(e) {
    }

    /* public */
    /**
    Returns the index of a specified child menu item, if any, else -1.
    @param {tp.MenuItemBase} Item The menu item
    @returns {number} Returns the index of a specified child menu item, if any, else -1.
    */
    IndexOf(Item) {
        return this.fItems.IndexOf(Item);
    }
    /**
    Returns a child menu item found at a specified index, if any, else null
    @param {number} Index The index
    @returns {tp.MenuItemBase} Returns a child menu item found at a specified index, if any, else null
    */
    ByIndex(Index) {
        return this.fItems.ByIndex(Index);
    }
    /**
    Returns a child menu item having a specified Command, if any, else null
    @param {string} Command The command
    @returns {tp.MenuItem} Returns a child menu item having a specified Command, if any, else null
    */
    ByCommand(Command) {
        let A = tp.SelectAll(this.Handle, '.' + tp.Classes.MenuItem);
        let List = tp.ToArray(A);
        let Result = tp.FirstOrDefault(List, (item) => { return tp.IsSameText(Command, item['Command']); });
        return Result instanceof tp.MenuItem ? Result : null;
    }

    /**
    Returns true if this parent contains a specified child menu item
    @param {tp.MenuItemBase} Item The menu item
    @returns {boolean} Returns true if this parent contains a specified child menu item
    */
    Contains(Item) {
        return this.fItems.Contains(Item);
    }
    /**
    Removes a specified child menu item
    @param {tp.MenuItemBase} Item The item to remove
    */
    Remove(Item) {
        this.fItems.Remove(Item);
    }
    /**
    Removes all child menu items
    */
    Clear() {
        this.fItems.Clear();
    }

    /**
    Adds a specified menu item as a child to this parent
    @param {tp.MenuItemBase} Item The item to add
    */
    Add(Item) {
        this.fItems.Add(Item);
    }
    /**
    Adds a menu item as a child to this parent
    @param {string} Text - The display text of the menu item
    @param {string} [Command]  - Optional. The command of the menu item
    @returns {tp.MenuItemBase} Returns the newly added menu item
    */
    AddMenuItem(Text, Command) {
        return this.fItems.AddMenuItem(Text, Command);
    }
    /**
    Adds and returns a separator
    @returns {tp.MenuSeparator} Returns the separator
    */
    AddSeparator() {
        return this.fItems.AddSeparator();
    }

    /**
    Inserts a specified menu item as a child to this parent, at a specified index.
    @param {number} Index The item index
    @param {tp.MenuItemBase} Item The menu item to insert
    @return {tp.MenuItemBase} Returns the newly added menu item
    */
    Insert(Index, Item) {
        return this.fItems.Insert(Index, Item);
    }
    /**
    Inserts a menu item as a child to this parent, at a specified index.
    @param {number} Index The item index
    @param {string} Text - The display text of the menu item
    @param {string} [Command]  - Optional.  The command of the menu item
    @returns {tp.MenuItemBase} Returns the newly added menu item
    */
    InsertMenuItem(Index, Text, Command) {
        return this.fItems.InsertMenuItem(Index, Text, Command);
    }
    /**
    Inserts and returns a separator, at a specified index.
    @param {number} Index The item index
    @returns {tp.MenuSeparator} Returns the separator
    */
    InsertSeparator(Index) {
        return this.fItems.InsertSeparator(Index);
    }

    /* Event triggers */
    /**
    Event trigger
    @param {MouseEvent} e The event object
    @param {tp.MenuItem} Item The menu item
    */
    OnItemClick(e, Item) {
        let Args = new tp.MenuEventArgs(Item, e);
        this.Trigger('ItemClick', Args);
        //log(Item);
    }
};
/* CAUTION: Keep this field here. It seems that fields are initialized after constructors are called. */
tp.MenuBase.prototype.fItems; // tp.MenuItemList;
//#endregion

//#region  tp.Menu
/**
Represents a desktop-like main menu <br />
Example markup
@example
<pre>
    <div id="MainMenu">
        <div>
            File
            <div>Open</div>
            <div>New</div>
        </div>
    </div>
</pre>
For each menu item provided by markup, an element with 4 containers is produced: 1. Image, 2. Text, 3. Arrow, 4. List <br />
Example of produced markup
@example
<pre>
    <div id="MainMenu" class="tp-Menu tp-Object" tabindex="0">
        <div class="tp-MenuItem">
            <div></div>
            <a>File</a>
            <div></div>
            <div style="display: none;">
                <div class="tp-MenuItem">
                    <div></div>
                    <a>Open</a>
                    <div></div>
                    <div style="display: none;"></div>
                </div>
                <div class="tp-MenuItem">
                    <div></div>
                    <a>New</a>
                    <div></div>
                    <div style="display: none;"></div>
                </div>
            </div>
        </div>
    </div>
</pre>
*/
tp.Menu = class extends tp.MenuBase {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
         <div id="MainMenu">
            <div>
                File
                <div>Open</div>
                <div>New</div>
            </div>
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Returns a boolean value indicating whether this instance is a main menu
    @type {boolean}
    */
    get IsMenu()  {
        return true;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.Menu';
        this.fDefaultCssClasses = [tp.Classes.Menu];
    }

};
//#endregion

//#region  tp.ContextMenu
/**
Represents a desktop-like context (local) menu <br />
Example markup
@example
<pre>
    <div id="ContextMenu">
        <div>
            File
            <div>Open</div>
            <div>New</div>
        </div>
    </div>
</pre>
For each menu item provided by markup, an element with 4 containers is produced: 1. Image, 2. Text, 3. Arrow, 4. List <br />
Example of produced markup
@example
<pre>
    <div id="ContextMenu" class="tp-ContextMenu tp-Object" tabindex="0">
        <div class="tp-MenuItem">
            <div></div>
            <a>File</a>
            <div></div>
            <div style="display: none;">
                <div class="tp-MenuItem">
                    <div></div>
                    <a>Open</a>
                    <div></div>
                    <div style="display: none;"></div>
                </div>
                <div class="tp-MenuItem">
                    <div></div>
                    <a>New</a>
                    <div></div>
                    <div style="display: none;"></div>
                </div>
            </div>
        </div>
    </div>
</pre>
*/
tp.ContextMenu = class extends tp.MenuBase {
    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <div id="ContextMenu">
            <div>
                File
                <div>Open</div>
                <div>New</div>
            </div>
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Returns a boolean value indicating whether this instance is a context menu
    @type {boolean}
    */
    get IsContextMenu() {
        return true;
    }


    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ContextMenu';
        this.fDefaultCssClasses = [tp.Classes.ContextMenu];
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        tp.On(this.Document, tp.Events.KeyUp, this);

        this.Visible = false;
    }
    /**
    Handles any DOM event
    @protected
    @param {Event} e The event object
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);

        switch (Type) {

            case tp.Events.Click:
                this.Visible = false;
                break;

            case tp.Events.KeyUp:
                if (e instanceof KeyboardEvent) {
                    if (e.keyCode === tp.Keys.Escape) {
                        this.Visible = false;
                    }
                }
                break;



        }


        super.OnAnyDOMEvent(e);
    }
    /**
    Event trigger
    @protected
    */
    OnVisibleChanged() {
        if (this.Visible) {
            setTimeout((self) => {
                tp.On(self.Document, tp.Events.Click, self);
            }, 0, this);

        } else {

            setTimeout((self) => {
                tp.Off(self.Document, tp.Events.Click, self);
            }, 0, this);

        }
        super.OnVisibleChanged();
    }

    /* methods */
    /**
    Shows the context menu at event coordinates (viewport coordinates).
    NOTE: This method prevents the default browser menu.
    @param {MouseEvent} e The event object
    */
    Show(e) {
        e.preventDefault();
        var X = e.clientX + 1;
        var Y = e.clientY + 1;
        this.ShowAt(X, Y);
    }
    /**
    Shows the context menu at viewport coordinates. <br />
    It also recalculates the location in order to be inside the boundaries of the viewport. <br />
    NOTE: This method does NOT prevent the default browser menu.
    @param {number} X The x coordination
    @param {number} Y The y coordination
    */
    ShowAt(X, Y) {
        if (tp.IsEmpty(this.ParentHandle)) {
            this.Document.body.appendChild(this.Handle);            
        }

        this.Position = 'fixed';
        this.ZIndex = tp.MaxZIndexOf(this.Document.body);        
        
        this.X = X;
        this.Y = Y;
        this.Visible = true;

        // reposition if outside of viewport
        let Size = tp.Viewport.GetSize();
        let Rect = tp.BoundingRect(this.Handle);      // location (relative to viewport) and size of this instance
 
        if (Rect.Bottom > Size.Height) {
            this.Y = Y - Rect.Height;
        }

        if (Rect.Right > Size.Width) {
            this.X = Size.Width - Rect.Width;
        }        

        this.Focus();
 

    }
};
//#endregion

//#region tp.SiteMenuEventArgs

/**
EventArgs for events for the tp.SiteMenu
*/
tp.SiteMenuEventArgs = class extends tp.EventArgs {

    /**
    Constructor 
    @param {HTMLElement} el - The clicked element
    @param {Event} [e] - Optional. The event
    */
    constructor(el, e) {
        super('', null);

        this.el = el;
        this.e = e;

        this.ItemText = '';

        if (tp.IsHTMLElement(el)) {
            this.Command = tp.Data(el, 'command') || '';


            let t = tp.FindTextNode(el);
            if (t) {
                this.ItemText = tp.Trim(t.nodeValue || '');
            }
        }
    }

    /**
    The text of the clicked element (menu item), if any, else empty string.
    @type {string}
    */
    ItemText = '';
};
//#endregion

//#region  tp.SiteMenu
/**
A document-like menu, with a toggle button for small srceens, that supports columns on menu items. <br />
Example markup
<pre>
    <div id="SiteMenu" class="tp-SiteMenu" data-setup="{ BreakPoint: 768 }">

        <div class="tp-Toggle" style="order: 1;">
            <div class="tp-Btn">Menu</div>                         <!-- toggle button &#8801; &#9776; -->
            <div class="tp-FlexFill"></div>
        </div>


        <div class="tp-Strip tp-Normal" style="order: 2;">

            <div class="tp-Item">MenuBarItem 1</div>                <!-- menu bar item -->

            <div class="tp-Item">
                MenuBarItem 2                      <!-- menu bar item with child menu items -->
                <div class="tp-Content">
                    <div class="tp-Item tp-Title">Title</div>
                    <div class="tp-Item" data-command="Command1">Item 1</div>
                    <div class="tp-Item">Item 2</div>
                </div>
            </div>

            <div class="tp-Item">
                MenuBarItem 3                      <!-- menu bar item with child menu items in columns -->
                <div class="tp-Content">
                    <div class="tp-Columns">
                        <div class="tp-Column">
                            <div class="tp-Item tp-Title">Title</div>
                            <div class="tp-Item">Item 1</div>
                            <div class="tp-Item">Item 2</div>
                        </div>
                        <div class="tp-Column">
                            <div class="tp-Item">Item 1</div>
                            <div class="tp-Item">Item 2</div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>
</pre>

*/
tp.SiteMenu = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <div id="SiteMenu" class="tp-SiteMenu" data-setup="{ BreakPoint: 768 }">

            <div class="tp-Toggle" style="order: 1;">
                <div class="tp-Btn">Menu</div>                         <!-- toggle button &#8801; &#9776; -->
                <div class="tp-FlexFill"></div>
            </div>

            <div class="tp-Strip tp-Normal" style="order: 2;">

                <div class="tp-Item">MenuBarItem 1</div>                <!-- menu bar item -->

                <div class="tp-Item">
                    MenuBarItem 2                      <!-- menu bar item with child menu items -->
                    <div class="tp-Content">
                        <div class="tp-Item tp-Title">Title</div>
                        <div class="tp-Item" data-command="Command1">Item 1</div>
                        <div class="tp-Item">Item 2</div>
                    </div>
                </div>

                <div class="tp-Item">
                    MenuBarItem 3                      <!-- menu bar item with child menu items in columns -->
                    <div class="tp-Content">
                        <div class="tp-Columns">
                            <div class="tp-Column">
                                <div class="tp-Item tp-Title">Title</div>
                                <div class="tp-Item">Item 1</div>
                                <div class="tp-Item">Item 2</div>
                            </div>
                            <div class="tp-Column">
                                <div class="tp-Item">Item 1</div>
                                <div class="tp-Item">Item 2</div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional. If a CreateParams is passed having its DeferHandleCreation to true, then then the
    constructor does NOT call the CreateHandle() and it is the caller responsibility to call it at a later time..
    When this property is set to true, then the constructor does NOT call the CreateHandle() and it is the caller responsibility to call it at a later time.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* protected */
    fMenuStrip = null;      // HTMLElement;
    fLastActive = null;
    fIsSmallScreen = false; // boolean;

    /* properties */
    /**
    The break-point. When the width of the viewport is smaller that this break-point, then the menu displays the toggle button. Defaults to 768.
    @type {number}
    */
    BreakPoint = 768;
    /**
    Returns the active element (menu item), if any, else null.
    @type {HTMLElement} 
    */
    get ActiveItem() {
        if (tp.IsHTMLElement(this.MenuStrip)) {
            return tp.Select(this.MenuStrip, '.' + tp.Classes.Item + '.' + tp.Classes.Active);
        }

        return null;
    }
    /**
    Returns the menu strip element, that is the menu bar where top menu items reside.
    @type {HTMLElement}
    */
    get MenuStrip() {
        if (tp.IsHTMLElement(this.Handle) && tp.IsEmpty(this.fMenuStrip)) {
            this.fMenuStrip = tp.Select(this.Handle, '.' + tp.Classes.Strip);
        }

        return this.fMenuStrip;
    }
    /**
    Returns true when the viewport width is smaller than the BreakPoint.
    @type {boolean}
    */
    get IsSmallScreen() {
        return this.fIsSmallScreen;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SiteMenu';
        this.fDefaultCssClasses = [tp.Classes.SiteMenu];
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.IsScreenResizeListener = true;

        this.HookEvent(tp.Events.Click);
        tp.On(this.Document, tp.Events.Click, this, false);

        tp.On(this.Document, tp.Events.MouseEnter, this, false);
    }
    /**
    Initializes fields and properties just before applying the create params.   
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.BreakPoint = 768;
    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.OnScreenSizeChanged(true);
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

        let VS = tp.Viewport.GetSize();

        if (VS.Width <= this.BreakPoint) {
            if (tp.IsEmpty(this.IsSmallScreen) || this.IsSmallScreen === false) {
                this.fIsSmallScreen = true;
                this.ScreenModeChanged();
            }
        } else {
            if (tp.IsEmpty(this.IsSmallScreen) || this.IsSmallScreen === true) {
                this.fIsSmallScreen = false;
                this.ScreenModeChanged();
            }
        }

        if (ScreenModeFlag === true) {

            switch (tp.Viewport.Mode) {
                case tp.ScreenMode.XSmall:
                case tp.ScreenMode.Small:
                case tp.ScreenMode.Medium:
                case tp.ScreenMode.Large:

                    break;
            }
        }
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The event object
    */
    OnAnyDOMEvent(e) {

        var Args, i, ln, S, el, List,
            Type = tp.Events.ToTripous(e.type);

        switch (Type) {
            case tp.Events.Click:
                if (tp.HasClass(e.target, tp.Classes.Btn) && tp.ContainsEventTarget(this.Handle, e.target)) {                // toggle button
                    this.ClearActiveItem();
                    tp.CancelEvent(e);
                    tp.ToggleClass(this.MenuStrip, tp.Classes.Hide);
                } else if (tp.HasClass(e.target, tp.Classes.Item) && tp.ContainsEventTarget(this.Handle, e.target)) {         // drop-down
                    tp.CancelEvent(e);
                    if (tp.HasClass(e.target, tp.Classes.Active)) {
                        this.ClearActiveItem();
                    } else {
                        this.ClearActiveItem();
                        tp.AddClass(e.target, tp.Classes.Active);
                    }
                    this.OnItemClick(e.target, e);
                } else if (e.currentTarget === this.Document) {
                    this.ClearActiveItem();
                }
                break;

            case tp.Events.MouseEnter:
                el = this.ActiveItem;

                // we have an active top menu item, and the mouse enters in another top menu item
                if (el && this.MenuStrip && tp.ListContains(this.MenuStrip.children, e.target) && (el !== e.target) && tp.HasClass(e.target, tp.Classes.Item)) {
                    this.ClearActiveItem();
                    tp.AddClass(e.target, tp.Classes.Active);
                    tp.CancelEvent(e);
                }
                // the mouse moves outside of this control
                else if (!tp.ContainsEventTarget(this.Handle, e.target)) {
                    tp.CancelEvent(e);
                    this.ClearActiveItem();
                }
                break;

        }

        super.OnAnyDOMEvent(e);
    }

    /* protected */
    /**
    Notification. Called when the screen changes from small to non-small.
    @protected
    */
    ScreenModeChanged() {
        this.ClearActiveItem();
        if (this.IsSmallScreen === true) {
            tp.AddClass(this.MenuStrip, tp.Classes.Hide);
        } else {
            tp.RemoveClass(this.MenuStrip, tp.Classes.Hide);
        }
    }
    /**
    Removes a css class from the currently active menu item.
    @protected
    */
    ClearActiveItem() {
        var el = this.ActiveItem;
        if (el) {
            tp.RemoveClass(el, tp.Classes.Active);
        }
    }
    /**
    Called when a menu item element is clicked.
    @protected
    @param {HTMLElement} el The element
    @param {Event} e The event object
    */
    OnItemClick(el, e) {
        let Args = new tp.SiteMenuEventArgs(el, e);
        this.Trigger('ItemClick', Args);
    }
};
//#endregion

//---------------------------------------------------------------------------------------
// tool-bars and buttons
//---------------------------------------------------------------------------------------

//#region tp.ItemBar

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


/** A bar (a zone) that can display items. Can be used as base class for tool-bars, tab-bars, etc. <br />
This controls uses a {@link https://developer.mozilla.org/en-US/docs/Web/API/ResizeObserver|ResizeObserver}, actually a <code>tp.ResizeDetector</code>,
to detect whether items fit in item-bar width. <br />
Provides three rendering modes: Normal, Toggle and NextPrev. The rendering mode changes automatically from Normal to the others and back. <br />
The user defines just the <code>ResponsiveMode</code> and then the mode goes from Normal to that <code>ResponsiveMode</code> depending on the width.
 * Properties: SelectedIndex, ResponsiveMode <br />
 * Methods: AddItem(), InsertItem(), RemoveItemAt() <br />
 * Events: RenderModeChanged, SelectedIndexChanging, SelectedIndexChanged, ItemClicked
 * */
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
                List.forEach(item => item.style.display = '');

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

    /** Get or sets the selected HTMLElement.
     * The setter may be passed an HTMLElement or an object with a Handle property of type HTMLElement.
     * @type {HTMLElement}
     */
    get SelectedItem() {
        let Index = this.SelectedIndex;

        if (Index >= 0) {
            let List = this.GetItemElementList();
            return List[Index];
        }

        return null;
    }
    set SelectedItem(v) {
        if (tp.IsValid(v)) {
            let el = tp.IsHTMLElement(v) ? v : (tp.IsHTMLElement(v.Handle) ? v.Handle : null);
            if (el) {
                let Index = this.IndexOfItem(el);
                if (Index >= 0)
                    this.SelectedIndex = Index;
            }
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
        this.fDefaultCssClasses = [tp.Classes.ItemBar];
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
                            this.OnItemClicked(List[i], e);
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
                        this.OnItemClicked(List[Index], e);
                    }
                }
                else if (tp.ContainsEventTarget(this.btnToLeft, e.target)) {
                    if (this.CanShowNext())
                        this.ShowNext();
                }
                else if (tp.ContainsEventTarget(this.btnToRight, e.target)) {
                    if (this.CanHideNext())
                        this.HideNext();
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

            this.ToggleItemList = this.ToggleDropDownBox.AddDivElement();
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
        let List = this.GetElementList();

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
    /** Called when an item is added, inserted or removed. 
     */
    ItemListChanged() {
        if (this.RenderMode === tp.ItemBarRenderMode.NextPrev)
            this.Arrange();
        else
            this.OnResized(null);
    }

    /** Returns true if a specified item is visible.
     * @private
     * @param {HTMLElement} el
     * @returns {boolean} Returns true if a specified item is visible.
     */
    IsItemVisible(el) {
        let display = tp.GetComputedStyle(el).display;
        return !tp.IsSameText('none', display);
    }
    /** Returns the total width of the visible items.
     * NOTE: Used only when mode is PrevNext.
     * @private
     * @returns {number} Returns the total width of the visible items.
     * */
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
    /** Returns true the next item can be hidden.
     * NOTE: Used only when mode is PrevNext.
     * @private
     * @returns {boolean} Returns true the Next button can be hidden.
     */
    CanHideNext() {
        if (this.RenderMode === tp.ItemBarRenderMode.NextPrev) {
            let List = this.GetItemElementList();
            if (List.length > 2) {
                let el = List[List.length - 2];
                return this.IsItemVisible(el);
            }
        }
        return false;
    }
    /** Returns true the previous item can be hidden.
     * NOTE: Used only when mode is PrevNext.
     * @private
     * @returns {boolean} Returns true the previous item can be hidden.
     */
    CanShowNext() {
        if (this.RenderMode === tp.ItemBarRenderMode.NextPrev) {
            let List = this.GetItemElementList();
            if (List.length > 0) {
                let el = List[0];
                return !this.IsItemVisible(el);
            }
        }
        return false;
    }
    /** Hides the next item
     * NOTE: Used only when mode is PrevNext.
     * @private
     * */
    HideNext() {
        if (this.RenderMode === tp.ItemBarRenderMode.NextPrev) {
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

    }
    /** Hides the previous item
     * NOTE: Used only when mode is PrevNext.
     * @private
     * */
    ShowNext() {
        if (this.RenderMode === tp.ItemBarRenderMode.NextPrev) {
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
    }
    /** Arranges the visible and non visible items.
     * NOTE: Used only when mode is PrevNext.
     * @private
     * */
    Arrange() {
        if (this.RenderMode === tp.ItemBarRenderMode.NextPrev) {
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
    }
    /** Returns the display text (caption) of an item found at a specified index.
     * @private
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

    /** Adds an item (which is an HTMLElement or an object with a Handle property of type HTMLElement) at the end of items.
     * @param {HTMLElement|object} el The item to add. An HTMLElement or an object with a Handle property of type HTMLElement.
     */
    AddItem(el) {
        if (tp.IsValid(el)) {
            if (!tp.IsHTMLElement(el) && tp.IsHTMLElement(el.Handle))
                el = el.Handle;

            if (tp.IsHTMLElement(el)) {
                this.ItemContainer.appendChild(el);
                this.ItemListChanged();
            }
        } 
    }
    /** Adds an array of items (where each item is an HTMLElement or an object with a Handle property of type HTMLElement) at the end of items
     * @param {HTMLElement[]|object[]} ItemList The array of items to add. Each item is an HTMLElement or an object with a Handle property of type HTMLElement.
     */
    AddRange(ItemList) {
        if (tp.IsArray(ItemList)) {

            ItemList.forEach((el) => {
                if (!tp.IsHTMLElement(el) && tp.IsHTMLElement(el.Handle))
                    el = el.Handle;
 
                if (tp.IsHTMLElement(el)) {
                    this.ItemContainer.appendChild(el);                    
                }
            });

            this.ItemListChanged();
        }
    }
    /** Adds an item at a specified index
     * @param {HTMLElement} el The element to insert
     * @param {number} Index The index
     */
    InsertItem(el, Index) {
        let List = this.GetItemElementList();
        if (tp.IsElement(el) && Index >= 0 && Index <= List.length - 1) {

            this.IsElementResizeListener = false;
            try {
                let TempList = [];
                List.forEach((item) => {
                    TempList.push(item);
                    item.parentNode.removeChild(item);
                });

                tp.ListInsert(TempList, Index, el);

                TempList.forEach(item => this.ItemContainer.appendChild(item));
            } finally {
                this.IsElementResizeListener = true;
            }

            this.ItemListChanged();
        }
    }
    /** Removes an item at a specified index
     * @param {number} Index The index
     */
    RemoveItemAt(Index) {
        let List = this.GetItemElementList();
        if (Index >= 0 && Index <= List.length - 1) {

            this.IsElementResizeListener = false;
            try {
                let el = List[Index];
                el.parentNode.removeChild(el);
            } finally {
                this.IsElementResizeListener = true;
            }

            this.ItemListChanged();
        }
    }
    /** Returns the index of a specified item.
     * @param {HTMLElement} el
     * @returns {number} Returns the index of a specified item.
     */
    IndexOfItem(el) {
        let List = this.GetItemElementList();
        return List.indexOf(el);
    }

    /* event triggers */
    /**
    Event trigger
    */
    OnRenderModeChanged() {
        let Args = new tp.EventArgs(null);
        this.Trigger('RenderModeChanged', Args);
        return Args;
    }
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
    /** Event trigger. <br />
     * NOTE: This event is triggered because of a click on an item and just after the "SelectedIndexChanged" event is triggered.
     * The arguments passed to the event is of type {@link tp.EventArgs}.  Sender is this instance, Item is the clicked HTMElement, ItemIndex its index, and e the DOM Event.
     * @param {HTMLElement} Item The clicked item
     * @param {Event} e The Event
     */
    OnItemClicked(Item, e) {
        let List = this.GetItemElementList();
        let Index = List.indexOf(Item);

        let Args = new tp.EventArgs('ItemClicked', this, e);
        Args.Item = Item;
        Args.el = Args.Item;
        Args.ItemIndex = Index;

        this.Trigger('ItemClicked', Args);
    }


};

tp.ItemBar.prototype.ChangingMode = false;
/** The rendering mode to go to when total item width exceeds this instance width.
 One of the {@link tp.ItemBarRenderMode} constants.
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

//#endregion

//#region  tp.Button
/**
Button class <br />
@implements {tp.ICommandProperty}
Example markup:
<pre>
    <button>Button1</button>
</pre>
*/
tp.Button = class extends tp.tpElement {
    /**
    Constructor <br />
    Example markup:
    <pre>
        <button>Button1<button>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    } 

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.Button';
        this.fElementType = 'button';
        this.fElementSubType = 'button';
        this.fDefaultCssClasses = [tp.Classes.Button];
    }

};
/**
Gets or sets the button command. A user defined string.
@type {string}
*/
tp.Button.prototype.Command = '';
/**
A user defined value
@type {any}
*/
tp.Button.prototype.Tag = null;
//#endregion

//#region tp.ToolBarItemClickEventArgs
/**
EventArgs for events for the tool-bar
*/
tp.ToolBarItemClickEventArgs = class extends tp.EventArgs {

    /**
    Constructor 
    @param {tp.tpElement} Item - The clicked {@link tp.tpElement} item
    @param {string} [Command] Optional
    */
    constructor(Item, Command = '') {
        super('', null);

        this.Item = Item;
        this.Command = Command;
    }

    /* properties */

    /**
    The item clicked.
    @type {tp.tpElement}
    */
    Item = null;

};
//#endregion

//#region tp.ControlToolButton

/**
Button for control tool-bars. Control tool-bars used by controls such as the Grid and some dialogs.
@implements {tp.ICommandProperty}
Example markup:
<pre>
    <div>Button1<div>
</pre>
*/
tp.ControlToolButton = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <div>Button1<div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or sets the display text of the node. 
    @type {string}
    */
    get Text() {
        return tp.IsHTMLElement(this.fTextElement) ? this.fTextElement.textContent : '';
    }
    set Text(v) {
        if (tp.IsHTMLElement(this.fTextElement)) {
            this.fTextElement.textContent = v;
            this.UpdatePadding();
        }
    }
    /**
    Gets or sets the ico css classes, e.g. fa fa-xxxxx, for the ico
    @type {string}
    */
    get IcoClasses() {
        return tp.IsHTMLElement(this.fImageElement) ? this.fImageElement.textContent : '';
    }
    set IcoClasses(v) {
        if (this.fImageElement) {
            this.fImageElement.className = v;
            this.UpdatePadding();
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

        this.tpClass = 'tp.ControlToolButton';
        this.fDefaultCssClasses = [tp.Classes.ControlToolButton];
    }
    /**
   Notification <br />
   Initialization steps:
   <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
   </ul>
   @protected
    @override
   */
    OnHandleCreated() {
        super.OnHandleCreated();

        let T = tp.FindTextNode(this.Handle);
        let S = T ? T.nodeValue || '' : '';
        if (T) {
            T.nodeValue = '';
        }

        S = tp.Trim(S);

        this.fImageElement = this.Document.createElement('div');
        this.Handle.appendChild(this.fImageElement);

        tp.StyleProp(this.fImageElement, 'font-size', '14px');

        this.fTextElement = this.Document.createElement('div');
        this.Handle.appendChild(this.fTextElement);
        this.fTextElement.innerHTML = S;

        this.HookEvent(tp.Events.Click);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The event object
    */
    OnAnyDOMEvent(e) {

        var Type = tp.Events.ToTripous(e.type);

        if (tp.Events.Click === Type) {
            if (this.Enabled) {
                if (this.Parent instanceof tp.ControlToolBar) {
                    this.Parent.OnButtonClick(new tp.ToolBarItemClickEventArgs(this, this.Command));
                }
            }
        }

        super.OnAnyDOMEvent(e);

    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.UpdatePadding();
    }


    /* overridables */

    /** Updates padding
     */
    UpdatePadding() {
        var Flag = !tp.IsBlank(this.IcoClasses) && !tp.IsBlank(this.Text);
        var S = Flag ? '4px' : '0';
        tp.StyleProp(this.fTextElement, 'padding-left', S);
    }
};

/* protected */

/** Field
@protected
@type {HTMLElement}
 */
tp.ControlToolButton.prototype.fTextElement = null;
/** Field
@protected
@type {HTMLElement}
 */
tp.ControlToolButton.prototype.fImageElement = null;


/**
Gets or sets the button command. A user defined string.
@type {string}
*/
tp.ControlToolButton.prototype.Command = '';
/**
A user defined value
@type {any}
*/
tp.ControlToolButton.prototype.Tag = null;

//#endregion

//#region tp.ControlToolBar

/**
A control tool-bar. Used by the grid and some dialogs
*/
tp.ControlToolBar = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <button>Button1<button>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

     
 

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ControlToolBar';
        this.fDefaultCssClasses = [tp.Classes.ControlToolBar];
        this.ButtonClass = tp.ControlToolButton;
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.fRightAligner = this.Document.createElement('div');
        this.Handle.appendChild(this.fRightAligner);
        this.fRightAligner.className = tp.Classes.FlexFill;
    }

    /* public */

    /**
     * Adds and returns a new tool-button to the tool-bar
     * @param {string} Command The button command
     * @param {string} Text The text
     * @param {string} ToolTip The tooltip
     * @param {string} IcoClasses The ico classes
     * @param {string} CssClasses Css classes
     * @param {boolean} ToRight Defaults to false. When true the button aligns to the right.
     * @returns {tp.ControlToolButton} Returns a new tool-button to the tool-bar
     */
    AddButton(Command, Text, ToolTip = '', IcoClasses = '', CssClasses = '', ToRight = false) {
        let CP = {};
        if (!tp.IsBlank(Command))
            CP.Command = Command;

        if (!tp.IsBlank(Text))
            CP.Text = Text;

        if (!tp.IsBlank(ToolTip))
            CP.ToolTip = ToolTip;

        if (!tp.IsBlank(IcoClasses))
            CP.IcoClasses = IcoClasses;

        if (!tp.IsBlank(CssClasses))
            CP.CssClasses = CssClasses;

        var btn = new this.ButtonClass(null, CP);
        btn.AddClass(tp.Classes.ToolButton);

        if (ToRight === true) {
            this.Handle.appendChild(btn.Handle);
        } else {
            this.Handle.insertBefore(btn.Handle, this.fRightAligner);
        }

        return btn;
    }
    /**
     * Adds a control to the toolbar
     * @param {tp.Control} Control The control.
     * @param {boolean} ToRight Defaults to false. When true the button aligns to the right.
     */
    AddItem(Control, ToRight = false) {
        if (ToRight === true) {
            this.Handle.appendChild(Control.Handle);
        } else {
            this.Handle.insertBefore(Control.Handle, this.fRightAligner);
        }
    }

    /* Event triggers */
    /**
    Event trigger
    @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
    */
    OnButtonClick(Args) {
        this.Trigger('ButtonClick', Args);
    }

};

/** Field
@protected
@type {HTMLElement}
 */
tp.ControlToolBar.prototype.fRightAligner = null;

/**
 The button class
 @default {@link tp.ControlToolButton}
 @type {any}
 */
tp.ControlToolBar.prototype.ButtonClass = null;
//#endregion

//#region tp.ButtonExIcoMode
/**
Indicates the mode (existence and position) of the icon of a button.
@class
@enum {number}
*/
tp.ButtonExIcoMode = {
    None: 0,
    Left: 1,
    Top: 2 
};
Object.freeze(tp.ButtonExIcoMode);
//#endregion

//#region tp.ButtonEx

/**
A button class with ico and text. It actually is an anchor element. <br />
@implements {tp.ICommandProperty}
Example markup
<pre>
    <a data-setup="{ IcoClasses: 'fa fa-university'}">Button Caption</a>
</pre>
Produced markup
<pre>
    <a class="tp-ButtonEx">
        <div class="fa fa-university" ></div>
        <div>Button Caption</div>
    </a>
</pre>
*/
tp.ButtonEx = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <button>Button1<button>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or sets the display text . 
    @type {string}
    */
    get Text() {
        return tp.IsHTMLElement(this.fTextElement) ? this.fTextElement.textContent : '';
    }
    set Text(v) {
        if (tp.IsHTMLElement(this.fTextElement)) {
            this.fTextElement.textContent = v;
        }
    }
    /**
    The handle of this class is an anchor element. <br />
    This property gets or sets the value of the href property of that anchor element.
    @type {string}
    */
    get Url() {
        if (this.Handle instanceof HTMLAnchorElement) {
            return this.Handle.href !== 'javascript:void(0);' ? this.Handle.href : '';
        }

        return '';
    }
    set Url(v) {
        if (this.Handle instanceof HTMLAnchorElement) {
            if (tp.IsEmpty(v) || tp.IsBlank(v)) {
                this.Handle.href = 'javascript:void(0);';
            } else {
                this.Handle.href = v;
            }
        }
    }
    /**
    Gets or sets the ico css classes, e.g. fa fa-xxxxx, for the ico
    @type {string}
    */
    get IcoClasses() {
        return this.fIcoClasses;
    }
    set IcoClasses(v) {
        if (tp.IsHTMLElement(this.fImageElement)) {
            tp.RemoveClasses(this.fImageElement, this.fIcoClasses);
            this.fImageElement.style.background = '';
            this.fImageUrl = '';
            tp.AddClasses(this.fImageElement, v);
        }

        this.fIcoClasses = v;
    }
    /**
    Gets or sets a url for the ico
    @type {string}
    */
    get ImageUrl() {
        return this.fImageUrl;
    }
    set ImageUrl(v) {
        if (tp.IsHTMLElement(this.fImageElement)) {
            tp.RemoveClasses(this.fImageElement, this.fIcoClasses);
            this.fImageElement.style.background = '';
            this.fIcoClasses = '';

            tp.SetStyle(this.fImageElement, {
                'background-image': tp.Format('url("{0}")', v),
                'background-repeat': 'no-repeat',
                'background-position': 'center center',
                'background-size': '90%'
            });
        }
        this.fImageUrl = v;
    }
    /**
    Gets or sets a value that controls the existence and position of the ico. One of the {@link tp.ButtonExIcoMode} constants.
    @type {tp.ButtonExIcoMode}
    */
    get IcoMode()  {
        return this.fIcoMode;
    }
    set IcoMode(v) {
        this.fIcoMode = v;
        this.RemoveClasses(tp.Classes.NoIco, tp.Classes.IcoTop);
        if (this.fIcoMode === tp.ButtonExIcoMode.None) {
            this.AddClass(tp.Classes.NoIco);
        } else if (this.fIcoMode === tp.ButtonExIcoMode.Top) {
            this.AddClass(tp.Classes.IcoTop);
        }
    }
    /**
    Get or sets a value indicating whether the text is visible. Defaults to false.
    @type {boolean}
    */
    get NoText() {
        return this.fNoText;
    }
    set NoText(v) {
        v = v === true;
        this.RemoveClass(tp.Classes.NoText);
        if (v) {
            this.AddClass(tp.Classes.NoText);
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

        this.tpClass = 'tp.ButtonEx';
        this.fElementType = 'a';

        this.fDefaultCssClasses = [tp.Classes.ButtonEx];
    }
    /**
   Notification <br />
   Initialization steps:
   <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
   </ul>
   @protected
   @override
   */
    OnHandleCreated() {
        super.OnHandleCreated();

        let T = tp.FindTextNode(this.Handle);
        let S = T ? T.nodeValue || '' : '';
        if (T) {
            T.nodeValue = '';
        }

        S = tp.Trim(S);

        if (this.Handle instanceof HTMLAnchorElement) {
            if (tp.IsBlank(this.Handle.href) || this.Handle.href === '#')
                this.Handle.href = 'javascript:void(0);';
        }

        this.fImageElement = this.Document.createElement('div');
        this.Handle.appendChild(this.fImageElement);

        this.fTextElement = this.Document.createElement('div');
        this.Handle.appendChild(this.fTextElement);
        this.fTextElement.innerHTML = S;

        this.HookEvent(tp.Events.Click);
    }
    /**
    Initializes fields and properties just before applying the create params.     
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.fIcoMode = tp.ButtonExIcoMode.Left;
        this.fNoText = false;
    }
    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @param {object} o - Optional. The create params object to processs.
    @protected
    @override
    */
    ProcessCreateParams(o) {
        o = o || {};

        if (tp.IsBlank(o.IcoClasses) && tp.IsBlank(o.ImageUrl)) {
            this.IcoMode = tp.ButtonExIcoMode.None;
        } else if (!tp.IsBlank(o.IcoClasses) || !tp.IsBlank(o.ImageUrl)) {
            this.IcoMode = tp.ButtonExIcoMode.Left;
        }

        for (var Prop in o) {
            if (!tp.IsFunction(o[Prop]) /* NO  && (Prop in this) */) {
                if (Prop === 'Ico') {
                    if (tp.IsSameText('Top', o[Prop])) {
                        this.IcoMode = tp.ButtonExIcoMode.Top;
                    } else if (tp.IsSameText('No', o[Prop]) || tp.IsSameText('NoIco', o[Prop])) {
                        this.IcoMode = tp.ButtonExIcoMode.None;
                    }
                } else {
                    this[Prop] = o[Prop];
                }

            }
        }
    }
    /**
    Handles any DOM event
    @param {Event} e The event object
    @protected
    @override
    */
    OnAnyDOMEvent(e) {

        var Type = tp.Events.ToTripous(e.type);

        if (tp.Events.Click === Type) {
            if (this.Enabled) {
                if (this.Parent instanceof tp.ToolBar) {
                    this.Parent.OnButtonClick(new tp.ToolBarItemClickEventArgs(this, this.Command));
                }
            }
        }

        super.OnAnyDOMEvent(e);

    }

};

/* private */

/**
Gets or sets a user defined string 
@type {string}
*/
tp.ButtonEx.prototype.Command = '';
/**
Gets or sets a user defined value
@type {any}
*/
tp.ButtonEx.prototype.Tag = null;


/* fields */
/**
 Field
 @protected
 @type {HTMLElement}
 */
tp.ButtonEx.prototype.fImageElement = null;
/**
 Field
 @protected
 @type {HTMLElement}
 */
tp.ButtonEx.prototype.fTextElement = null;
/**
 Field
 @protected
 @type {tp.ButtonExIcoMode}
 */
tp.ButtonEx.prototype.fIcoMode = tp.ButtonExIcoMode.Left;
/**
 Field
 @protected
 @type {boolean}
 */
tp.ButtonEx.prototype.fNoText = false;
/**
 Field
 @protected
 @type {string}
 */
tp.ButtonEx.prototype.fIcoClasses = '';
/**
 Field
 @protected
 @type {string}
 */
tp.ButtonEx.prototype.fImageUrl = '';

//#endregion

//#region tp.ToolBar
/**
A tool-bar
*/
tp.ToolBar = class extends tp.tpElement {

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
    The button class 
    */
    ButtonClass = null;

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ToolBar';
        this.fDefaultCssClasses = [tp.Classes.ToolBar];
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.fRightAligner = this.Document.createElement('div');
        this.Handle.appendChild(this.fRightAligner);
        this.fRightAligner.className = tp.Classes.FlexFill;

        tp.Ui.CreateControls(this.Handle);
 
    }

    /* public */
    /**
    Adds and returns a new tool-button to the tool-bar
     * @param {string} Command The button command
     * @param {string} Text The text
     * @param {string} ToolTip The tooltip
     * @param {string} IcoClasses The ico classes
     * @param {string} CssClasses Css classes
     * @param {boolean} ToRight Defaults to false. When true the button aligns to the right.
     * @returns {tp.ButtonEx} Returns a new tool-button to the tool-bar
     */
    AddButton(Command, Text, ToolTip = '', IcoClasses = '', CssClasses = '', ToRight = false) {
        let CP = {};
        if (!tp.IsBlank(Command))
            CP.Command = Command;

        if (!tp.IsBlank(Text))
            CP.Text = Text;

        if (!tp.IsBlank(ToolTip))
            CP.ToolTip = ToolTip;

        if (!tp.IsBlank(IcoClasses))
            CP.IcoClasses = IcoClasses;

        if (!tp.IsBlank(CssClasses))
            CP.CssClasses = CssClasses;

        var btn = new tp.ButtonEx(null, CP);
        btn.AddClass(tp.Classes.ToolButton);

        if (ToRight === true) {
            this.Handle.appendChild(btn.Handle);
        } else {
            this.Handle.insertBefore(btn.Handle, this.fRightAligner);
        }

        return btn;
    }
    /**
     * Adds a control to the toolbar
     * @param {tp.Control} Control The control.
     * @param {boolean} ToRight Defaults to false. When true the button aligns to the right.
     */
    AddItem(Control, ToRight = false) {
        if (ToRight === true) {
            this.Handle.appendChild(Control.Handle);
        } else {
            this.Handle.insertBefore(Control.Handle, this.fRightAligner);
        }
    }

    /**
    Sets the ico mode (None, Left, Top) to all of the buttons of this toolbar
    @param {tp.ButtonExIcoMode} v The mode to set. One of {@link tp.ButtonExIcoMode} constants.
    */
    SetIcoMode(v) {
        let o;
        let List = this.GetControls();

        for (let i = 0, ln = List.length; i < ln; i++) {
            o = List[i];
            if (o instanceof tp.ButtonEx) {
                o.IcoMode = v;
            }
        }
    }
    /**
    Sets the NoText flag to all of the buttons of this toolbar
    @param {boolean} Flag The flag value
    */
    SetNoText(Flag) {
        Flag = Flag === true;
        let o;
        let List = this.GetControls();

        for (let i = 0, ln = List.length; i < ln; i++) {
            o = List[i];
            if (o instanceof tp.ButtonEx) {
                o.NoText = Flag;
            }
        }
    }

    /**
     * Returns a child of this tool-bar having a specified command, if any, else null.
     * @param {string} Command The command to look for, e.g. 'Execute'.
     * @returns {tp.ButtonEx | tp.tpElement}  Returns a child of this tool-bar having a specified command, if any, else null.
     */
    FindItemByCommand(Command) {
        let List = this.GetControls();

        let Result = List.find(item => {
            return tp.IsString(item.Command) && tp.IsSameText(item.Command, Command);                 
        });

        return Result;
    }

    /* Event triggers */
    /**
    Event trigger
    @param {tp.ToolBarItemClickEventArgs} Args The {@link tp.ToolBarItemClickEventArgs} arguments
    */
    OnButtonClick(Args) {
        this.Trigger('ButtonClick', Args);
    }

};

/** Field
 @type {HTMLElement}
 @protected
 */
tp.ToolBar.prototype.fRightAligner = null;
//#endregion

//---------------------------------------------------------------------------------------
// controls
//---------------------------------------------------------------------------------------

//#region  tp.ControlBindMode
/**
The type of a control data-binding
@class
@enum {number}
*/
tp.ControlBindMode = {
    None: 0,
    Simple: 1,
    List: 2,
    Grid: 4
};
Object.freeze(tp.ControlBindMode);

//#endregion

//#region  tp.CalenderClickChangeType
/**
Indicates the type of date change that happened in a tp.Calender because of a mouse click.
@class
@enum {number}
*/
tp.CalenderClickChangeType = {
    Date: 1,
    Month: 2,
    Year: 4
};
Object.freeze(tp.CalenderClickChangeType);
//#endregion

//#region  tp.CalendarClickChangeEventArgs
/**
Used by the tp.Calendar OnClickChange() event trigger method.
Notifies listeners about the type of date change that happened in a tp.Calender because of a mouse click.
*/
tp.CalendarClickChangeEventArgs = class extends tp.EventArgs {
    /**
    Constructor.
    @param {tp.CalenderClickChangeType} ChangeType - Denotes the type of date change that happened in a {@link tp.Calender} because of a mouse click. One of the {@link tp.CalenderClickChangeType} constants.
    */
    constructor(ChangeType) {
        super('ClickChange', null, null);
        this.ChangeType = ChangeType;
    }

    /**
    Denotes the type of date change that happened in a {@link tp.Calender} because of a mouse click. One of the {@link tp.CalenderClickChangeType} constants.
    @type {tp.CalenderClickChangeType}
    */
    ChangeType = tp.CalenderClickChangeType.Date;
};
//#endregion

//#region  tp.TreeViewEventArgs
/**
EventArgs for events for the tp.TreeView
*/
tp.TreeViewEventArgs = class extends tp.EventArgs {

    /**
    Constructor 
    @param {tp.TreeNode} [Node] - Optional.
    */
    constructor(Node) {
        super('', null);

        this.Node = Node;
    }

/* properties */
    /** The tree-view node
     @type {tp.TreeNode}
     */
    Node = null;

};
//#endregion


//#region  tp.AutocompleteList

/**
Repesents an autocomplete list. To be used with text-boxes. The data list could be asked from the server on every search. <br />
The data list of the autocomplete could be a list of primitives (string, boolean, number), or just plain javascript objects. <br />
When the autocomplete data list is a list of objects then the ListDisplayField MUST be specified. <br />
The height of the dropdown list is controlled by the MaxDropdownItems numeric property.
@example
var text1, a1, cb;

function AdjustAutocompleteData() {
    if (cb.checked === true) {
        a1.ServerFunc = '/Test/GetAutocompleteList';
    } else {
        a1.ServerFunc = '';
    }
}

tp.Ready(function () {
    cb = tp.Select('#cb');
    text1 = tp.Select('#text1');
    a1 = new tp.AutocompleteList(text1);

    a1.DataList = [
        { Id: 1, Name: 'javascript' },
        { Id: 2, Name: 'typescript' },
        { Id: 3, Name: 'script' },
        { Id: 4, Name: 'scriptable' },
        { Id: 5, Name: 'html' },
        { Id: 6, Name: 'css rules' },
    ];

    a1.ListDisplayField = 'Name';
    a1.Active = true;
    text1.focus();

    AdjustAutocompleteData();
});

function CheckBoxClick() {
    AdjustAutocompleteData();
}
*/
tp.AutocompleteList = class extends tp.DropDownBox {

    /**
    Constructor
    @param {string | HTMLElement} Associate The associate element, the element that displays the box.
    */
    constructor(Associate) {
        super(null, null); // let the dropdown box to create its handle

        if (!tp.IsHTMLElement(Associate))
            tp.Throw('No Associate defined for AutocompleteList');

        this.Active = false;
        this.fDisplayList = [];

        this.Associate = tp.Select(Associate);

        this.fDocument = this.Associate.ownerDocument;

        this.fContainer = this.Document.createElement('div');
        this.Handle.appendChild(this.fContainer);
        this.fContainer.className = tp.Classes.List;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        this.fContainer.tabIndex = -1;

        this.fDropDownScroller = new tp.VirtualScroller(this.Handle, this.fContainer);

        this.fDropDownScroller.Context = this;
        this.fDropDownScroller.RenderRowFunc = this.ItemRenderFunc;

        tp.On(this.Document, tp.Events.Click, this, false);
        tp.On(this.Associate, tp.Events.KeyUp, this, true);
        tp.On(this.Associate, tp.Events.KeyDown, this, false);
    }

    /* private */

    /** Field
     @private
     @type {HTMLElement}
     */
    fContainer = null;
    /** Field
     @private
     @type {tp.VirtualScroller}
     */
    fDropDownScroller = null;
    /** Field
     @private
     @type {number}
     */
    fItemHeight = 0;
    /** Field
     @private
     @type {string}
     */
    fListDisplayField = null;
    /** Field
     @private
     @type {any}
     */
    fSelectedItem = null;
    /** Field
     @private
     @type {number}
     */
    fMaxDropdownItems = 10;
    /** Field
     @private
     @type {any[]}
     */
    fDisplayList = null;

    /* properties */
    /**
    The height of an item (line) in the dropdown box. If not specified then it results from a calculation.
    @type {number}
    */
    get ItemHeight() {
        if (tp.IsEmpty(this.fItemHeight) || this.fItemHeight <= 0)
            this.fItemHeight = tp.GetLineHeight(this.Associate);
        return this.fItemHeight;
    }
    set ItemHeight(v) {
        this.fItemHeight = v;
    }

    /**
    The name of value "field-name"/property of objects contained in the data list. <br />
    The autocomplete box may diplay a list of objects. In that case this property specifies which property of those objects to display.
    @type {string}
    */
    get ListDisplayField() {
        return this.fListDisplayField;
    }
    set ListDisplayField(v) {
        this.fListDisplayField = v;
    }
    /**
    Gets or sets the maximum number of items to be shown in the dropdown list
    @type {number}
    */
    get MaxDropdownItems() {
        let Result = this.fMaxDropdownItems || 10;
        if (Result > 40) {
            Result = 40;
        }
        return Result;
    }
    set MaxDropdownItems(v) {
        this.fMaxDropdownItems = v;
    }
    /**
    Returns the selected item from the list, if any, else null.
    @type {any}
    */
    get SelectedItem() {
        return this.fSelectedItem;
    }
    /**
    Returns true if the selected item is valid. That is if the text displayed by the associate equals to the text of the selected item, if any.
    @type {boolean}
    */
    get IsSelectedItemValid() {
        if (!tp.IsEmpty(this.SelectedItem)) {
            var S = this.GetItemText(this.SelectedItem);
            var S2 = !tp.IsEmpty(this.Associate) ? tp.Trim(tp.val(this.Associate)) || '' : '';

            return !tp.IsBlank(S2) && tp.IsSameText(S, S2);
        }
        return false;
    }
    /**
    True to activate the autocomplete. Defaults to false.
    @default false
    @type {boolean}
    */
    Active = false;
    /**
    A url for a server action to call for filtering. When this is set, then the DataList local data is ignored.
    @type {string}
    */
    ServerFunc = null;
    /**
    A local list of data to user when filtering
    @type {any[]}
    */
    DataList = null;
    /**
    If true, then the "starts with" logic is used when filtering. Defaults to false.
    @default false
    @type {boolean}
    */
    UseStartsWithFilter = false;
    /**
    The number of characters typed in the associated, that trigger the autocompletion operation. Defaults to 3.
    @default 3
    @type {number}
    */
    AutocompleteCharCount = 3;


    /* private */
    /**
    Called by the virtual scroller for rendering list items
    @private
    @param {any} Row - An element from the RowList array
    @param {number} RowIndex - The index of the Row in the RowList array
    @returns {HTMLElement} Returns an {@link HTMLElement}
    */
    ItemRenderFunc(Row, RowIndex)  {
        var Result = this.Document.createElement('div');
        Result.className = tp.Classes.Item;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        Result.tabIndex = -1;
        tp.SetElementInfo(Result, {
            Item: Row,
            Index: RowIndex
        });

        Result.innerHTML = this.GetItemText(Row);
        return Result;
    }
    /** 
     * Returns the text of a specified item.
     * @private
     * @param {any} Item The item
     * @returns {string} Returns the text of a specified item.
     */
    GetItemText(Item) {

        if (!tp.IsEmpty(Item)) {
            if (tp.IsPrimitive(Item)) {
                return Item.toString();
            }

            if (!tp.IsBlank(this.ListDisplayField) && (this.ListDisplayField in Item)) {
                return Item[this.ListDisplayField];
            }

            if (typeof Item.ToString === "function")
                return Item.ToString();
        }

        return '';

    }
    /**
     * Sets the scroller list.
     * @private
     * @param {string} Text The text
     */
    SetScrollerList(Text) {
        let List = this.fDisplayList;
        if (tp.IsEmpty(List) || (tp.IsArray(List) && List.length === 0)) {
            this.Close();
        } else if (tp.IsArray(List)) {
            if (List.length === 1 && Text === this.GetItemText(List[0])) {
                this.Close();
            } else if (List.length > 0) {
                this.fDropDownScroller.RowHeight = this.ItemHeight;
                this.fDropDownScroller.SetRowList(List);
                this.Open();
            }
        }
    }
    /** Returns the {@link HTMLElement} that is marked as selected.
     * @private
     * @returns {HTMLElement}  Returns the {@link HTMLElement} that is marked as selected.
     * */
    GetItemWithSelectionIndication() {
        return tp.Select(this.fContainer, '.' + tp.Classes.Selected);
    }
    /** Sets a specified {@link HTMLElement} as the selected item.
     * @private
     * @param {HTMLElement} el The {@link HTMLElement} to set as selected
     */
    SetSelectionIndicationTo(el) {
        // remove any selection indication
        var el2 = tp.Select(this.fContainer, '.' + tp.Classes.Selected);
        if (el2) {
            tp.RemoveClass(el2, tp.Classes.Selected);
        }

        if (tp.IsElement(el) && tp.ContainsElement(this.fContainer, el)) {
            tp.AddClass(el, tp.Classes.Selected);
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

        this.tpClass = 'tp.AutocompleteList';
        this.fDefaultCssClasses.push(tp.Classes.AutocompleteList);  
    }
    /**
    Called by the Open()/Close() methods and notifies the owner, if any, about a stage change
    @protected
    @override
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants
    */
    OnOwnerEvent(Stage) {
        let n = Math.min(this.MaxDropdownItems, this.fDisplayList.length);
        n = (n * this.ItemHeight) + 5;
        this.Height = n;
        super.OnOwnerEvent(Stage);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {

        if (this.Active === true) {

            var Flag, el, Type = tp.Events.ToTripous(e.type);
            let S, n;

            if (tp.Events.Click === Type) {
                if (this.Resizing === true)
                    return;
                if (e.currentTarget === this.Document) {
                    if (tp.ContainsEventTarget(this.fContainer, e.target)) {
                        Flag = tp.HasClass(e.target, tp.Classes.Item)
                            && tp.HasElementInfo(e.target);  

                        if (Flag) {
                            this.fSelectedItem = tp.GetElementInfo(e.target).Item;  
                            this.Close();
                            tp.val(this.Associate, this.GetItemText(this.fSelectedItem));
                            this.Associate.focus();
                        }
                    } else {
                        this.Close();
                    }
                }
            } else if (e.target === this.Associate) {

                if (tp.Events.KeyUp === Type) {
                    n = tp.IsNumber(this.AutocompleteCharCount) ? this.AutocompleteCharCount : 3;
                    S = tp.val(this.Associate) || '';
                    if (n > 0 && S.length >= n) {
                        if (tp.IsPrintableKey(e)) {
                            this.FilterAsync(S);
                        }

                    } else {
                        this.Close();
                    }
                }

                if (tp.Events.KeyDown === Type && e instanceof KeyboardEvent && this.IsOpen === true) {
                    switch (e.keyCode) {
                        case tp.Keys.Up:
                            el = this.GetItemWithSelectionIndication();
                            if (el && tp.IsElement(el.previousElementSibling)) {
                                this.SetSelectionIndicationTo(el.previousElementSibling);
                                //e.preventDefault();
                            }
                            return;
                        case tp.Keys.Down:
                            el = this.GetItemWithSelectionIndication();
                            if (el && tp.IsElement(el.nextElementSibling)) {
                                this.SetSelectionIndicationTo(el.nextElementSibling);
                            } else if (this.fContainer.children.length > 0) {
                                this.SetSelectionIndicationTo(this.fContainer.children[0]);
                            }
                            return;

                        case tp.Keys.Enter:
                            el = this.GetItemWithSelectionIndication();
                            if (el && tp.HasElementInfo(el)) {
                                this.fSelectedItem = tp.GetElementInfo(el).Item;
                                this.Close();
                                tp.val(this.Associate, this.GetItemText(this.fSelectedItem));
                            }
                            return;

                        case tp.Keys.Escape:
                            this.Close();
                            return;
                    }
                }

            }
        }

        super.OnAnyDOMEvent(e);

    }



    /* methods */
    /**
    Filters the data list, based on the specified text, and displays the dropdown box.
    @param {string} Text The filter text
    */
    async FilterAsync(Text) {
        this.fSelectedItem = null;

        let self = this;
        this.fDisplayList = [];
        let o, i, ln, Item, ItemText, FilterFunc;


        if (!tp.IsBlank(this.ServerFunc)) {                     // user server data

            let Data = {
                Text: Text,
                UseStartsWith: this.UseStartsWithFilter === true
            };

            let Args = await tp.Ajax.PostAsync(this.ServerFunc, Data);

            self.fDisplayList = Args.Packet;
            self.SetScrollerList(Text);

        } else {                                                // use local data

            if (tp.IsArray(this.DataList)) {

                FilterFunc = this.UseStartsWithFilter === true ? tp.StartsWith : tp.ContainsText;

                for (i = 0, ln = this.DataList.length; i < ln; i++) {
                    Item = this.DataList[i];
                    ItemText = this.GetItemText(Item);
                    if (FilterFunc(ItemText, Text, true)) {
                        this.fDisplayList.push(Item);
                    }
                }
            }

            this.SetScrollerList(Text);
        }

    }


};
//#endregion


//#region tp.Control
/**
The ultimate ancestor class of controls.
@implements {tp.IDataSourceListener}
*/
tp.Control = class extends tp.tpElement  {

    /**
    Constructor
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /* static */

     /**
      Adds the "required" mark
      @param {tp.Control} Control The control to add the mark
      @param {HTMLElement} Mark The SPAN element, which is the required mark.
      @returns {HTMLElement} Returns the mark element
      */
     static AddRequiredMark(Control, Mark) {
        if (!(Mark instanceof HTMLElement) && Control.ParentHandle instanceof HTMLElement) {
            Mark = Control.Document.createElement('span');
            Control.ParentHandle.appendChild(Mark);
            Mark.className = tp.Classes.RequiredMark;
            Mark.innerHTML = '*';
            tp.Display(Mark, 'none');
        }
        return Mark;
    }

    /* static-like properties */
    /**
    Returns the name of the property this control class uses with data-binding.
    A property to get/set the data value (i.e. this.Text or this.Value or this.Date)
    @type {string}
    */
    get DataValueProperty() {
        return !tp.IsBlank(this.fDataValueProperty) ? this.fDataValueProperty : '';
    }
    /**
    Returns the mode of the data-binding this control class supports. One of the {@link tp.ControlBindMode} constants
    @type {tp.ControlBindMode}
    */
    get DataBindMode() {
        return this.fDataBindMode;
    }
    /**
    Returns true if this control class supports simple data-binding
    @type {boolean}
    */
    get IsDataBindSimple() {
        return this.DataBindMode === tp.ControlBindMode.Simple;
    }
    /**
    Returns true if this control class supports list data-binding
    @type {boolean}
    */
    get IsDataBindList() {
        return this.DataBindMode === tp.ControlBindMode.List;
    }
    /**
    Returns true if this control class supports grid data-binding
    @type {boolean}
    */
    get IsDataBindGrid() {
        return this.DataBindMode === tp.ControlBindMode.Grid;
    }

    /**
    Gets or sets the value or textContent or innerHTML, depending on the node type
    @type {string}
    */
    get Text() { return super.Text; }
    set Text(v) {
        tp.val(this.Handle, '');
        super.Text = v;
    }

    /* properties */
    /**
    Gets or sets the DataSource, if any, this control is bound to.
    @type {tp.DataSource}
    */
    get DataSource() {
        return this.fDataSource;
    }
    set DataSource(v) {
        if (v !== this.fDataSource) {
            var WasDataBound = this.IsDataBound;
            this.OnDataSourceChanging(v);

            if (v instanceof tp.DataTable) {
                if (v.BindingSource instanceof tp.DataSource)
                    v = v.BindingSource;
                else
                    v = new tp.DataSource(v);
            }

            this.fDataSource = v;

            var IsRemoved = tp.IsEmpty(v) && (WasDataBound === true);
            if (IsRemoved === true)
                this.OnClearDataDisplay();

            this.OnDataSourceChanged();

            if (v) {
                this.Bind();

                if (this.IsDataBound === true) {
                    this.OnBindCompleted();
                }
            }
        }
    }
    /**
    Gets or sets the data field (column name), if any, this control is bound to.
    @type {string}
    */
    get DataField() {
        return this.fDataField;
    }
    set DataField(v) {
        if (v !== this.DataField) {
            this.fDataField = v;
            this.OnDataFieldChanged();
            this.Bind();
        }
    }


    /**
    Returns true if this instance is bound to a DataSource
    @type {boolean}
    */
    get IsDataBound() {
        var Result = false;

        if (!tp.IsEmpty(this.DataSource)) {
            if (tp.ControlBindMode.Simple === this.DataBindMode) {
                Result = !tp.IsBlank(this.DataField) && !tp.IsBlank(this.DataValueProperty) && !tp.IsEmpty(this.DataColumn);
            } else if (tp.ControlBindMode.List === this.DataBindMode) {
                Result = !tp.IsBlank(this.DataField) && !tp.IsBlank(this.DataValueProperty) && !tp.IsEmpty(this.DataColumn);
            } else if (tp.ControlBindMode.Grid === this.DataBindMode) {
                Result = true;
            }
        }

        return Result;
    }
    /**
    Returns the tp.DataColumn this instance is bound to, if any, else null
    @type {tp.DataColumn}
    */
    get DataColumn() {
        if (!tp.IsEmpty(this.DataSource) && !tp.IsBlank(this.DataField)) {
            return this.DataSource.Table.FindColumn(this.DataField);
        }

        return null;
    }
    /**
    Gets or sets a boolean value indicating whether the control is required to have a value
    @type {boolean}
    */
    get Required() {
        return this.fRequired === true;
    }
    set Required(v) {
        if (this.Required !== Boolean(v)) {
            this.fRequired = Boolean(v);
            this.OnRequiredChanged();
        }
    }
    /**
    Gets or sets a boolean value indicating whether the control is read-only
    @type {boolean}
    */
    get ReadOnly() {
        return this.fReadOnly === true;
    }
    set ReadOnly(v) {
        if (this.ReadOnly !== Boolean(v)) {
            this.fReadOnly = Boolean(v);
            this.OnReadOnlyChanged();
        }
    }

    /**
    Gets or sets a single character that acts as a shortcut key to activate/focus an element.
    @type {string}
    @see {@link http://www.w3schools.com/tags/att_global_accesskey.asp|w3schools}
    */
    get AccessKey() {
        return this.Handle ? this.Handle.accessKey : '';
    }
    set AccessKey(v) {
        if (this.Handle) {
            this.Handle.accessKey = v;
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

        this.tpClass = 'tp.Control';

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.None;
        this.fDataValueProperty = '';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.fDataSource = null;
        this.fDataField = '';
    }
    /**
   Notification <br />
   Initialization steps:
   <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
   </ul>
   @protected
   @override
   */
    OnHandleCreated() {
        super.OnHandleCreated();

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        this.Handle.tabIndex = 0;
    }

    /* overridables */
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    */
    Bind() {
    }
    /**
    Displays or hides the required mark when the required property changes.
    @protected
    @param {Element} el The element whose value is required (or not) 
    */
    SetRequiredMark(el) {
        if (tp.IsFormElement(el) || !tp.IsEmpty(this.elRequiredMark)) {

            if (tp.IsEmpty(this.elRequiredMark)) {
                let CtrlRow = tp.Ui.GetCtrlRow(this.Handle);
                if (tp.IsHTMLElement(CtrlRow)) {
                    this.elRequiredMark = tp.Select(CtrlRow, '.' + tp.Classes.RequiredMark);
                }
            }

            //if (tp.IsEmpty(this.elRequiredMark)) {
            //    this.elRequiredMark = tp.Control.AddRequiredMark(this, this.elRequiredMark);
            //}

            if (!tp.IsEmpty(this.elRequiredMark)) {
                if (this.Required === true) {
                    if (el)
                        el.required = true;

                    this.elRequiredMark.style.display = '';

                } else {
                    if (el)
                        el.required = false;

                    this.elRequiredMark.style.display = 'none';
                }
            }

        }
    }

    /* event triggers */
    /**
    Event trigger. Called just before the datasource is assigned
    @protected
    @param {tp.DataSource} NewDataSource - The new datasource that is going to be assigned
    */
    OnDataSourceChanging(NewDataSource) {
        this.Trigger('DataSourceChanging', { NewDataSource: NewDataSource });
        if (this.DataSource) {
            this.DataSource.RemoveDataListener(this);
        }
    }
    /**
    Event trigger. Called right after the datasource is assigned
    @protected
    */
    OnDataSourceChanged() {
        if (this.DataSource) {
            this.DataSource.AddDataListener(this);
            this.Trigger('DataSourceChanged');
        }
    }
    /**
    Event trigger. Called right after the data field is changed
    @protected
    */
    OnDataFieldChanged() {
        this.Trigger('DataFieldChanged');
    }
    /**
    Event trigger. Called right after the DataSource is set to a new value and the control should clear its display.
    @protected
    */
    OnClearDataDisplay() {

        this.Trigger('ClearDataDisplay');
    }
    /**
    Event trigger. Called right after the data-binding is completed
    @protected
    */
    OnBindCompleted() {
        this.Trigger('BindCompleted');
    }
    /**
    Event trigger. Called right after the read-only property is changed
    @protected
    */
    OnReadOnlyChanged() {
        this.Trigger('ReadOnlyChanged', {});
    }

    /* IDataSourceListener implementation */
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable}
    @param {tp.DataRow} Row The {@link tp.DataRow}
    */
    DataSourceRowCreated(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable}
    @param {tp.DataRow} Row The {@link tp.DataRow}
    */
    DataSourceRowAdded(Table, Row) {
        if (this.IsDataBindSimple || this.IsDataBindList) {
            this.ReadDataValue();
        }
    }
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        if (this.IsDataBindSimple || this.IsDataBindList) {
            this.ReadDataValue();
        }
    }
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable}
    @param {tp.DataRow} Row The {@link tp.DataRow}
    */
    DataSourceRowRemoved(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {number} Position The new position
    */
    DataSourcePositionChanged(Table, Row, Position) {
        if (this.IsDataBindSimple || this.IsDataBindList) {
            this.ReadDataValue();
        }
    }

    /**
    Notification
    */
    DataSourceSorted() {
    }
    /**
    Notification
    */
    DataSourceFiltered() {
    }
    /**
    Notification
    */
    DataSourceUpdated() {
        if (this.IsDataBindSimple || this.IsDataBindList) {
            this.ReadDataValue();
        }
    }

    /* public */
    /**
    Returns a specified value formatted as text, according to its data column, if any, else return the value un-formatted. <br />
    NOTE: Use this method with simple and list controls only
    @param {any} v - The value to format.
    @returns {string} The formatted text 
    */
    Format(v) {
        if (this.DataColumn instanceof tp.DataColumn) {
            var Col = this.DataColumn;
            return Col ? Col.Format(v, this.IsDataBindList || this.IsDataBindGrid) : v;
        }

        return '';
    }
    /**
    Converts a specified string into a primitive value (or a date-time),  according to its data column, and returns the value.
    @param {string|any} v - The text or anything convertible to text, that should be converted to a value according to the data-column of this control.
    @returns {any} The result of the convertion
    */
    Parse(v) {
        if (this.DataColumn instanceof tp.DataColumn) {
            if (!tp.IsEmpty(v)) {
                v = !tp.IsString(v) ? v.toString() : v;

                var Col = this.DataColumn;
                return Col ? Col.Parse(v) : v;
            }  
        }

        return null;
    }


    /**
     * Converts a value coming from data-source's column to a value assignable to the data-property of this instance.
     * NOTE: Use this method with simple and list controls only
     * @param {any} v - The value to operate on.
     * @returns {any} The result of the convertion
    */
    DataValueToDataProperty(v) {
        return v;
    }
    /**
     * Converts the value of the data-property of this instance to a value assignable to data-source's column.
     * NOTE: Use this method with simple and list controls only
     * @param {any} v - The value to operate on.
     * @returns {any} The result of the convertion
    */
    DataPropertyToDataValue(v) {
        return v;
    }


    /**
    Reads the value from data-source and assigns the control's data value property
    */
    ReadDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        if (this.IsDataBound && this.DataSource.Position >= 0) {
            this.ReadingDataValue = true;
            try {
                let v = this.DataSource.Get(this.DataField);
                v = this.DataValueToDataProperty(v);

                this[this.DataValueProperty] = v;
            } finally {
                this.ReadingDataValue = false;
            }
        }
    }
    /**
    Writes the value from the control's data value property to the data-source.
    */
    WriteDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        if (this.IsDataBound && this.DataSource.Position >= 0) {
            this.WritingDataValue = true;
            try {
                var v = this[this.DataValueProperty];
                v = this.DataPropertyToDataValue(v);

                this.DataSource.Set(this.DataField, v);
            } finally {
                this.WritingDataValue = false;
            }
        }
    }

    /* validation */
    /**
    Event trigger
    */
    OnRequiredChanged() {
        this.Trigger('RequiredChanged');
    }
    /**
    Returns true if a form will validate when it is submitted, without having to submit it.
    @returns {boolean} Returns true if a form will validate when it is submitted, without having to submit it.
    */
    CheckValidity() {
        return tp.IsValidatableElement(this.Handle) ? this.Handle.checkValidity() : true;
    }
    /**
    Sets a custom error message that is displayed when a form is submitted.
    @param {string} MessageText - Sets a custom error message that is displayed when a form is submitted.
    */
    SetValidationMessage(MessageText) {
        if (tp.IsValidatableElement(this.Handle))
            this.Handle.setCustomValidity(MessageText);
    }


};



/** The bind mode of this class.One of the {@link tp.ControlBindMode} constants <br />
 NOTE: Class data-bind setup (treat this as a read-only class/static field)
 @protected
 @type {tp.ControlBindMode}
 */
tp.Control.prototype.fDataBindMode = tp.ControlBindMode.None;
/** a property to get/set the data value (i.e. this.Text or this.Value or this.Date)
 NOTE: Class data-bind setup (treat this as a read-only class/static field)
 @protected
 @type {string}
 */
tp.Control.prototype.fDataValueProperty = '';


/** Field
 @protected
 @type {tp.DataSource}
 */
tp.Control.prototype.fDataSource = null;
/**
Get or sets the table name, a string value used for declarative data-binding.
@type {string}
*/
tp.Control.prototype.TableName = "";
/** Field
@protected
@type {string}
*/
tp.Control.prototype.fDataField = "";
/** Field
 @protected
 @type {boolean}
 */
tp.Control.prototype.fRequired; // = false;
/** Field
@protected
@type {boolean}
*/
tp.Control.prototype.fReadOnly; // = false;

/** True while reading from datasource and writing to control DataValue Property
@protected
@type {boolean}
*/
tp.Control.prototype.ReadingDataValue = false;
/** True while reading from control DataValue Property and writing to datasource
@protected
@type {boolean}
*/
tp.Control.prototype.WritingDataValue = false;


//#endregion  

//#region tp.Label
/**
Label control <br />
Example markup:
<pre>
    <label>Label text</label>
</pre>
*/
tp.Label = class extends tp.Control {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <label>Label text</label>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Returns the FIRST text node of this control, if any, else null.
    @type {Text}
    */
    get TextNode() {
        return tp.FindTextNode(this.Handle);
    }
    /**
    Gets or sets the text of this instance
    @type {string}
    */
    get Text() {
        var t = this.TextNode;
        return t ? t.nodeValue : '';
    }
    set Text(v) {
        var t = this.TextNode;
        if (!t) {
            t = this.Document.createTextNode(v);
            this.Handle.appendChild(t);
        }
        if (t) {
            t.nodeValue = v;
        }
    }
    /**
    Gets or sets a string value indicating the id attribute of a labeled control. Reflects the for html attribute.
    @type {string}
    */
    get AssociateId() {
        return this.Handle instanceof HTMLLabelElement ? this.Handle.htmlFor : '';
    }
    set AssociateId(v) {
        if (this.Handle instanceof HTMLLabelElement) {
            this.Handle.htmlFor = v;
        }
    }
    /**
    Returns a reference to the HTMLElement control with which this label is associated.
    @type {HTMLElement}
    */
    get Associate() {
        return this.Handle instanceof HTMLLabelElement ? this.Handle['control'] : null;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.Label';
        this.fElementType = 'label';
        this.fDisplayType = 'inline-block';
        this.fDefaultCssClasses = [tp.Classes.Label];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Text';
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
};
//#endregion


//#region tp.InputControl
/**
A base input control (form control).
*/
tp.InputControl = class extends tp.Control {
    /**
    Constructor
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Gets or sets a boolean value indicating that a form control should have input focus when the page loads. Defaults to false.
    @type {boolean}
    */
    get AutoFocus() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.autofocus : false;
    }
    set AutoFocus(v) {
        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.autofocus = v;
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

        this.tpClass = 'tp.InputControl';
        this.fElementType = 'input';

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.HookEvent(tp.Events.Change);
        this.HookEvent(tp.Events.InputChanged);
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
    /**
    Event trigger. Called right after the read-only property is changed
    @protected
    @override
    */
    OnReadOnlyChanged() {
        if (this.Handle instanceof HTMLInputElement)
            tp.ReadOnly(this.Handle, this.ReadOnly);
        super.OnReadOnlyChanged();
    }
    /**
    Event trigger
    @protected
    @override
   */
    OnRequiredChanged() {
        this.SetRequiredMark(this.Handle);
        super.OnRequiredChanged();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);
        if (tp.IsSameText(tp.Events.InputChanged, Type) || tp.IsSameText(tp.Events.Change, Type)) {
            this.WriteDataValue();
            this.OnValueChanged();
        }

        super.OnAnyDOMEvent(e);
    }

    /* Event triggers */
    /**
    Called on any value change
    */
    OnValueChanged() {
        if (!this.ReadingDataValue) {
            this.Trigger('ValueChanged', {});
        }
    }
};
//#endregion

//#region tp.TextBox
/**
A textbox control. <br />
Example markup:
<pre>
    <input type='text' value='text 1'/>
</pre>
*/
tp.TextBox = class extends tp.InputControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <input type='text' value='text 1'/>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

/* private */

    /** Field
     @private
     @type {tp.AutocompleteList}
     */
    fAutocompleteList;

    /* properties */
    /**
    Gets or sets the maximum length of text (in Unicode code points) that the value can be changed to.
    @type {number}
    */
    get MaxLength() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.maxLength : 0;
    }
    set MaxLength(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.maxLength = v;
    }
    /**
    Gets or sets a text the control displays as a hint to the user of what can be entered in the control. <br /> 
    The placeholder text must not contain carriage returns or line-feeds.
    @type {string}
    */
    get Placeholder() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.placeholder : '';
    }
    set Placeholder(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.placeholder = v;
    }
    /**
    Gets or sets a regular expression that an <input> element's value is checked against
    @type {string}
    @see {@link http://www.w3schools.com/tags/att_input_pattern.asp|w3schools}
    */
    get RegexPattern() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.pattern : '';
    }
    set RegexPattern(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.pattern = v;
    }

    /**
    Gets or sets the start position of the selected text
    @type {number}
    */
    get SelectionStart() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.selectionStart : 0;
    }
    set SelectionStart(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.selectionStart = v;
    }
    /**
    Gets or sets the end position of the selected text
    @type {number}
    */
    get SelectionEnd() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.selectionEnd : 0;
    }
    set SelectionEnd(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.selectionEnd = v;
    }
    /**
    Returns true if the direction in which selection occurred is forward. <br />
    The input attribute selectionDirection is set to "forward" if selection was performed in the start-to-end direction of the current locale,
    and is set to "backward" for the opposite direction.
    @type {boolean}
    */
    get IsForwardSelection() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.selectionDirection === 'forward' : false;
    }

    /**
    Gets or sets a boolean value indicating whether the autocomplete attribute is set to on or to off.
    @type {boolean}
    */
    get DOMAutocomplete() {
        return this.Handle instanceof HTMLInputElement ? this.Handle.autocomplete === 'on' : false;
    }
    set DOMAutocomplete(v) {
        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.autocomplete = v === true ? 'on' : 'off';
        }
    }
    /**
    Returns the Tripous tp.AutocompleteList instance associated to this instance.
    @type {tp.AutocompleteList}
    */
    get AutocompleteList() {
        if (this.Handle instanceof HTMLInputElement) {
            if (tp.IsEmpty(this.fAutocompleteList))
                this.fAutocompleteList = new tp.AutocompleteList(this.Handle);
        }
        return this.fAutocompleteList;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.TextBox';
        this.fElementSubType = 'text';
        this.fDefaultCssClasses = [tp.Classes.TextBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Text';
    }
    /**
    Event trigger
    @protected
    @override
    */
    OnHandleCreated() {
        this.HookEventGroups(tp.EventGroup.Keyboard);
        super.OnHandleCreated();
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.SpellCheck = false;
    }
    /**
     * Processes an entry of the this.CreateParams.
     * @param {string} Name The name of the property in this.CreateParams
     * @param {any} Value The value of the property in this.CreateParams
     * @param {string[]} AvoidParams A string array of property names the ProcessCreateParam() should NOT set
     */
    ProcessCreateParam(Name, Value, AvoidParams) {
        if (tp.IsSameText('AutocompleteList', Name)) {
            this.AutocompleteList.DataList = Value;
            this.AutocompleteList.Active = true;
        }
        else {
            super.ProcessCreateParam(Name, Value, AvoidParams);
        }
    }
    /**
    Event trigger. Called right after the data-binding is completed
    @protected
    @override
    */
    OnBindCompleted() {
        var Alignment = tp.DataType.DefaultAlignment(this.DataColumn.DataType);
        this.TextAlign = tp.Alignment.ToText(Alignment);
        super.OnBindCompleted();
    }
 
    /* public */
    /**
    Focuses an element and selects all the text in it
    */
    Select() {
        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.select();
        }
    }
    /**
    Sets the start and end positions of the current text selection.
    @param {number} Start The start position
    @param {number} End The end position
    */
    SetSelectionRange(Start, End) {
        if (this.Handle instanceof HTMLInputElement) {
            Start = Start || 0;
            End = End || 0;
            this.Handle.setSelectionRange(Start, End);
        }
    }
};
//#endregion

//#region tp.Memo 
/**
A memo (textarea) control <br />
Example markup:
<pre>
    <textarea cols='20' rows='5'>memo text</textarea>
</pre>
*/
tp.Memo = class extends tp.Control {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <textarea cols='20' rows='5'>memo text</textarea>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Gets or sets the visible width of the text control, in average character widths. If it is specified, it must be a positive integer. If it is not specified, the default value is 20 (HTML5).
    @type {number}
    */
    get Cols() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.cols : 0;
    }
    set Cols(v) {
        if (this.Handle instanceof HTMLTextAreaElement)
            this.Handle.cols = v;
    }
    /**
    Gets or sets the number of visible text lines for the control.
    @type {number}
    */
    get Rows() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.rows : 0;
    }
    set Rows(v) {
        if (this.Handle instanceof HTMLTextAreaElement)
            this.Handle.cols = v;
    }
    /**
    Gets or sets a boolean value indicating that a form control should have input focus when the page loads. Defaults to false.
    @type {boolean}
    */
    get AutoFocus() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.autofocus : false;
    }
    set AutoFocus(v) {
        if (this.Handle instanceof HTMLTextAreaElement) {
            this.Handle.autofocus = v;
        }
    }
    /**
    Gets or sets the maximum length of text (in Unicode code points) that the value can be changed to.
    @type {number}
    */
    get MaxLength() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.maxLength : 0;
    }
    set MaxLength(v) {
        if (this.Handle instanceof HTMLTextAreaElement)
            this.Handle.maxLength = v;
    }
    /**
    Gets or sets a text the control displays as a hint to the user of what can be entered in the control. <br /> 
    The placeholder text must not contain carriage returns or line-feeds.
    @type {string}
    */
    get Placeholder() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.placeholder : '';
    }
    set Placeholder(v) {
        if (this.Handle instanceof HTMLTextAreaElement)
            this.Handle.placeholder = v;
    }

    /**
    Gets or sets the start position of the selected text
    @type {number}
    */
    get SelectionStart() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.selectionStart : 0;
    }
    set SelectionStart(v) {
        if (this.Handle instanceof HTMLTextAreaElement)
            this.Handle.selectionStart = v;
    }
    /**
    Gets or sets the end position of the selected text
    @type {number}
    */
    get SelectionEnd() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle.selectionEnd : 0;
    }
    set SelectionEnd(v) {
        if (this.Handle instanceof HTMLTextAreaElement)
            this.Handle.selectionEnd = v;
    }
    /**
    Returns true if the direction in which selection occurred is forward. <br />
    The input attribute selectionDirection is set to "forward" if selection was performed in the start-to-end direction of the current locale,
    and is set to "backward" for the opposite direction.
    @type {boolean}
    */
    get IsForwardSelection() {
        return this.Handle instanceof HTMLTextAreaElement ? this.Handle['selectionDirection'] === 'forward' : false;
    }

    /**
    Gets or sets a boolean value indicating whether the control applies word-wrap logic.
    @type {boolean}
    @see {@link http://stackoverflow.com/questions/657795/how-remove-word-wrap-from-textarea|stackoverflow}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Element/textarea#attr-wrap|mdn}
    */
    get WordWrap() {
        if (this.Handle instanceof HTMLTextAreaElement) {
            //var S = this.StyleProp('white-space');
            //return S === 'normal';
            return this.Handle.wrap === 'hard';
        }
        return true;
    }
    set WordWrap(v) {
        if (this.Handle instanceof HTMLTextAreaElement) {
            //this.StyleProp('white-space', v === true ? 'normal' : 'pre');
            this.Handle.wrap = v === true ? 'hard' : 'off';
        }
    }
    /**
    Gets or sets a boolean value indicating whether the control displays a resize handle.
    @type {boolean}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/CSS/resize|mdn}
    */
    get Resizable() {
        let S = this.StyleProp('resize');
        return S !== '' && S !== 'none';
    }
    set Resizable(v) {
        this.StyleProp('resize', v === true ? 'both' : 'none');
    }



    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.Memo';
        this.fElementType = 'textarea';
        //this.fDisplayType = 'inline';
        this.fDefaultCssClasses = [tp.Classes.Memo];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Text';
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();        
        this.HookEvent(tp.Events.Change);
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.Cols = 20;
        this.Rows = 2;
        this.SpellCheck = false;
    }
    /**
    Event trigger. Called right after the read-only property is changed
    @protected
    @override
    */
    OnReadOnlyChanged() {
        if (this.Handle instanceof HTMLTextAreaElement)
            tp.ReadOnly(this.Handle, this.ReadOnly);
        super.OnReadOnlyChanged();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);
        if (tp.IsSameText(tp.Events.InputChanged, Type) || tp.IsSameText(tp.Events.Change, Type)) {
            this.WriteDataValue();
            this.OnValueChanged();
        }

        super.OnAnyDOMEvent(e);
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }

    /* public */
    /**
    Focuses an element and selects all the text in it
    */
    Select() {
        if (this.Handle instanceof HTMLTextAreaElement) {
            this.Handle.select();
        }
    }
    /**
    Sets the start and end positions of the current text selection.
    @param {number} Start The start position
    @param {number} End The end position
    */
    SetSelectionRange(Start, End) {
        if (this.Handle instanceof HTMLTextAreaElement) {
            Start = Start || 0;
            End = End || 0;
            this.Handle.setSelectionRange(Start, End);
        }
    }

    /**
    Appends a specified text in the memo text
    @param {string} Text The text to append
    */
    Append(Text) {
        var S = this.Text + Text;
        this.Text = S;
    }
    /**
    Appends a specified text as a new line in the memo text
    @param {string} Text The text to append
    */
    AppendLine(Text) {
        var S = this.Text;
        S = tp.IsBlank(S) ? Text : S + '\n' + Text;
        this.Text = S;
    }
    /**
    Returns a string array with the text lines in the memo.
    @returns {string[]} Returns a string array with the text lines in the memo.
    */
    GetLines() {
        var S = this.Text;
        var Parts = tp.Split(S, '\n', false);
        return Parts;
    }

    /* Event triggers */
    /**
    Called on any value change
    @protected
    @override
    */
    OnValueChanged() {
        if (!this.ReadingDataValue) {
            this.Trigger('ValueChanged', {});
        }
    }
    /**
    Event trigger
    @protected
    @override
    */
    OnRequiredChanged() {
        this.SetRequiredMark(this.Handle);
        super.OnRequiredChanged();
    }
};
//#endregion

//#region tp.CheckBox 
/**
A check-box control. Is actually a label element surrounding a input[type=checkbox] element. <br />
Example markup
<pre>
    <label><input type="checkbox" /><span class='tp-Text'> This is the text of the checkbox</span></label>
</pre>
*/
tp.CheckBox = class extends tp.Control {

    /**
    Constructor <br />
    Example markup
    <pre>
        <label><input type="checkbox" /><span class='tp-Text'> This is the text of the checkbox</span></label>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /* properties */ 
    /**
    Gets or sets the text of this instance
    @type {string}
    */
    get Text() {
        if (tp.IsHTMLElement(this.elText))
            return this.elText.innerHTML;

        // Returns the FIRST text node of this control, if any, else null.
        var t = tp.FindTextNode(this.Handle);
        return t ? t.nodeValue : '';
    }
    set Text(v) {
        if (tp.IsHTMLElement(this.elText)) {
            this.elText.innerHTML = v;
        }
        else {
            // Returns the FIRST text node of this control, if any, else null.
            let t = tp.FindTextNode(this.Handle);
            if (!t) {
                t = this.Document.createTextNode(v);
                this.Handle.appendChild(t);
            }
            if (t) {
                t.nodeValue = v;
            }
        }
    }
    /**
    Gets or sets a boolean value indicating whether this controls is checked.
    @type {boolean}
    */
    get Checked() {
        return this.fCheckBox instanceof HTMLInputElement ? this.fCheckBox.checked : false;
    }
    set Checked(v) {
        if (this.fCheckBox instanceof HTMLInputElement)
            this.fCheckBox.checked = v === true;
    }


    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.CheckBox';
        this.fElementType = 'label';
        this.fDefaultCssClasses = [tp.Classes.CheckBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Checked';
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        // check-box
        let el = tp.Select(this.Handle, 'input[type=checkbox]');

        if (!tp.IsHTMLElement(el)) {
            this.Handle.innerHTML = "<input type='checkbox' />" + (this.Handle.innerHTML || '');
        }

        this.fCheckBox = tp.Select(this.Handle, 'input[type="checkbox"]');

        // text
        if (!tp.IsHTMLElement(this.elText)) {
            this.elText = tp.Select(this.Handle, '.' + tp.Classes.Text);
        }

        if (!tp.IsHTMLElement(this.elText)) {
            this.elText = this.Document.createElement('span');
            tp.AddClass(this.elText, tp.Classes.Text);
            this.Handle.appendChild(this.elText);
        }
 
        super.OnHandleCreated();
    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {        
        if (this.fCheckBox instanceof HTMLInputElement) {
            tp.On(this.fCheckBox, tp.Events.Change, this);
        }

        super.OnInitializationCompleted();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);
        if (tp.IsSameText(tp.Events.Change, Type) && e.target === this.fCheckBox) {
            this.WriteDataValue();
            this.OnValueChanged();
        }

        super.OnAnyDOMEvent(e);
    }

    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
    /**
    Reads the value from data-source and assigns the control's data value property
    */
    ReadDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        if (this.IsDataBound && this.DataSource.Position >= 0) {
            this.ReadingDataValue = true;
            try {
                let v = this.DataSource.Get(this.DataField);
                this[this.DataValueProperty] = v === true || v === 1;
            } finally {
                this.ReadingDataValue = false;
            }
        }
    }
    /**
    Writes the value from the control's data value property to the data-source.
    */
    WriteDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        if (this.IsDataBound && this.DataSource.Position >= 0) {
            this.WritingDataValue = true;
            try {
                var v = this[this.DataValueProperty];
                this.DataSource.Set(this.DataField, v === true);
            } finally {
                this.WritingDataValue = false;
            }
        }
    }

    /* validation */
    /**
    Event trigger
    @protected
    @override
    */
    OnRequiredChanged() {
        this.SetRequiredMark(this.Handle);
        super.OnRequiredChanged();
    }

    /** Makes the control the focused control */
    Focus() {
        this.fCheckBox.focus();
    }

    /* Event triggers */
    /**
    Called on any value change
    @protected
    @override
    */
    OnValueChanged() {
        if (!this.ReadingDataValue) {
            this.Trigger('ValueChanged', {});
        }
    }
};

/** Field
 @private
 @type {HTMLInputElement}
 */
tp.CheckBox.fCheckBox = null;
//#endregion

//#region tp.NumberBox
/**
A custom number box control. Contains a styled input type="text" element (NOT type="number").    <br />
NOTE: decimal places may be set using the relevant property or the DataColumn of this control.
 
Example markup:
<pre>
    <input type="text" >
</pre>
 
*/
tp.NumberBox = class extends tp.InputControl {
    /**
    Constructor <br />
    Example markup:
    <pre>
        <input type='text' />
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }


    /**
     * Gets the number value of this instance.
     * @type {number}
     */
    get Value() {        
        let S = this.Text;
        let v = this.Parse(S);
        return v;
    }
    /** Sets the value of this instance as a number.
     * @param {number} v The value to set.
     */
    set Value(v) {
        if (v !== this.Value) {
            this.Text = tp.IsNumber(v) ? this.Format(v) : '';
        } 
    }


    get Decimals() {
        return this.DataColumn instanceof tp.DataColumn ? this.DataColumn.Decimals : this.fDecimals;
    }
    set Decimals(v) {
        if (tp.IsInteger(v) && v >= 0 && v !== this.Decimals) {
            this.fDecimals = v;
            this.NormalizeText();
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

        this.tpClass = 'tp.NumberBox';
        this.fElementSubType = 'text';
        this.fDefaultCssClasses = [tp.Classes.NumberBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Value';
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();         

        // events
        this.HookEvent(tp.Events.LostFocus); 
    }
    /**
    Event trigger
    @protected
    @override
    */
    OnHandleCreated() {
        this.HookEventGroups(tp.EventGroup.Keyboard);
        super.OnHandleCreated();
    }

    /**
    Event trigger. Called right after the data-binding is completed
    @protected
    @override
    */
    OnBindCompleted() {
        //this.TextAlign = tp.Alignment.ToText(tp.Alignment.Right);
        super.OnBindCompleted();
    }
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);
        if (tp.IsSameText(tp.Events.LostFocus, Type)) {
            this.NormalizeText();
            this.WriteDataValue();
            this.OnValueChanged();
        }

        super.OnAnyDOMEvent(e);
    }
    /** Called when the control losts focus in order to normalize the text */
    NormalizeText() {
        let S = this.Text;
        let v = this.Parse(S);

        let Result = !tp.IsEmpty(v);

        if (Result && this.Decimals > 0) {
            let S2 = tp.FormatNumber(v, `F${this.Decimals}`);
            if (S !== S2)
                this.Text = S2;
        }

        return Result;
    }

    /**
    Returns a specified value formatted as text, according to its data column, if any, else return the value un-formatted. <br />
    NOTE: Use this method with simple and list controls only
    @param {any} v - The value to format.
    @returns {string} The formatted text 
    */
    Format(v) {
        if (tp.IsValid(v)) {
            if (this.DataColumn instanceof tp.DataColumn) {
                return this.DataColumn.Format(v, false);
            }
            else if (tp.IsNumber(v)) {
                return tp.FormatNumber2(v, this.Decimals);
            }
        }

        return '';
    }
    /**
    Converts a specified string into a primitive value (or a date-time),  according to its data column, and returns the value.
    @param {string} S - The text to convert to a value
    @returns {any} The result of the convertion
    */
    Parse(S) {
        if (tp.IsString(S) && !tp.IsBlank(S)) {
            let Func = this.Decimals > 0 ? tp.TryStrToFloat : tp.TryStrToInt;
            let ConvertionResult = Func(S);
            return ConvertionResult.Result === true ? ConvertionResult.Value : null;
        }

        return null;
    }
};
tp.NumberBox.prototype.fDecimals = 0;
//#endregion

//#region  tp.ListControl
/**
Base class for ComboBox and ListBox controls
@implements {tp.ISelectedIndex}
*/
tp.ListControl = class extends tp.Control {
    /**
    Constructor  
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /**
    Gets or sets the selected index
    @type {number}
    */
    get SelectedIndex() {
        return this.fSelectedIndex;
    }
    set SelectedIndex(v) {
        if ((v !== this.SelectedIndex) && tp.InRange(this.Items, v)) {
            this.fSelectedIndex = v;
            this.DoSelectedIndexChanged();
        }
    }
    /**
    Gets or sets the selected value
    @type {any}
    */
    get SelectedValue() {
        return this.fSelectedValue;
    }
    set SelectedValue(v) {
        if (v !== this.SelectedValue) {
            this.fSelectedValue = v;
            this.DoSelectedValueChanged();
        }
    }
    /**
    Gets or sets the selected item (object or row)
    @type {any}
    */
    get SelectedItem() {
        return this.fSelectedItem;
    }
    set SelectedItem(v) {
        if ((v !== this.SelectedItem) && (this.Items.indexOf(v) !== -1)) {
            this.fSelectedItem = v;
            this.DoSelectedItemChanged();
        }
    }

    /**
    Gets or sets the array of the items. Used when the control is NOT data-bound.
    @type {any[]}
    */
    get Items()  {
        return this.fListSource instanceof tp.DataSource ? this.fListSource.Rows : this.fItems;
    }
    set Items(v) {
        this.Clear();
        if (tp.IsArray(v)) {
            this.fItems.AddRange(v);
        }
        this.SelectedIndex = 0;
    }
    /**
    Gets or sets a {@link tp.DataSource} to be to be used as the source for the item list. It accepts a {@link tp.DataTable} too and uses that table to create the {@link tp.DataSource}.
    @type {tp.DataSource}
    */
    get ListSource() {
        return this.fListSource;
    }
    set ListSource(v) {
        if (v !== this.fListSource) {

            if (this.fListSource instanceof tp.DataSource) {
                this.fListSource.RemoveDataListener(this.fListSourceListener);
            }

            if (v instanceof tp.DataTable) {
                v = new tp.DataSource(v);
            }

            this.fListSource = v instanceof tp.DataSource ? v : null;

            if (this.fListSource instanceof tp.DataSource) {
                this.fListSource.AddDataListener(this.fListSourceListener);
                this.ListSourceBind();
            } else {
                this.Clear();
            }

        }

    }
    /**
    Gets or sets the name of the list datasource. Used in declarative scenarios.
    @type {string}
    */
    get ListSourceName() {
        return this.fListSourceName;
    }
    set ListSourceName(v) {
        this.fListSourceName = v;
    }

    /**
    Gets or sets the name of the value "field-name"/property of objects contained in the item list. <br />
    @type {string}
    */
    get ListValueField() {
        return !tp.IsBlank(this.fListValueField) ? this.fListValueField : this.ListDisplayField;
    }
    set ListValueField(v) {
        this.fListValueField = v;
    }
    /**
    Gets or sets the name of the display "field-name"/property of objects contained in the data list. <br />
    @type {string}
    */
    get ListDisplayField() {
        return this.fListDisplayField;
    }
    set ListDisplayField(v) {
        this.fListDisplayField = v;
    }


    /**
    The height of an item (line) in the dropdown box. If not specified then it results from a calculation.
    @type {number}
    */
    get ItemHeight() {
        if (tp.IsEmpty(this.fItemHeight) || this.fItemHeight <= 0)
            this.fItemHeight = tp.GetLineHeight(this.Handle);
        return this.fItemHeight;
    }
    set ItemHeight(v) {
        this.fItemHeight = v;
    }

    /* protected - overridables */

    /** Sets the text
     * @protected
     * @param {string} Text The text
     */
    DoSetText(Text) {
    }
    /** Updates the scroller
     @protected
     */
    UpdateScroller() {
        if (this.fScroller)
            this.fScroller.Update();
    }
    /** Sets the scroller list
     @protected
     */
    SetScrollerList() {
        if (this.fScroller) {
            this.fScroller.RowHeight = this.ItemHeight;
            this.fScroller.SetRowList(this.Items);
            this.fScroller.Update();
        }
    }
    /**  Returns true if the list contains a specified field name
     * @protected
     * @param {string} FieldName The field name
     * @returns {boolean} Returns true if the list contains a specified field name
     */
    ListContainsField(FieldName) {
        if (!tp.IsEmpty(this.ListSource)) {
            if (this.ListSource.Table instanceof tp.DataTable) {
                let Table = this.ListSource.Table;
                return Table.ContainsColumn(FieldName);
            }
        }

        if (!tp.IsEmpty(this.Items)) {
            if (this.Items.length > 0) {
                var Item = this.Items[0];
                if (!tp.IsPrimitive(Item))
                    return FieldName in Item;
            }
        }

        return false;
    }
    /** Returns the ListValue field name  
     @protected
     @returns {string} Returns the ListValue field name
     */    
    GetListValueField() {
        var Result = this.ListValueField;
        if (tp.IsBlank(Result)) {
            if (this.ListContainsField('Id'))
                Result = 'Id';
        }
        return Result;
    }
    /** Returns the ListDisplay field name 
     @protected
     @returns {string} Returns the ListDisplay field name
     */
    GetListDisplayField() {
        var Result = this.ListDisplayField;
        if (tp.IsBlank(Result)) {
            if (this.ListContainsField('Name'))
                Result = 'Name';
        }
        return Result;
    }
    /** Returns the text of a specified item
     * @protected
     * @param {any} Item The item to operate on.
     * @returns {string} Returns the text of a specified item
     */
    GetItemText(Item) {

        if (!tp.IsEmpty(Item)) {
            if (tp.IsPrimitive(Item)) {
                return Item.toString();
            }

            let DisplayField = this.GetListDisplayField();

            if (!tp.IsBlank(DisplayField)) {
                if (this.ListSource instanceof tp.DataSource && Item instanceof tp.DataRow)
                    return this.ListSource.GetValue(Item, DisplayField, '');
                return Item[DisplayField];
            }

            if (typeof Item.ToString === "function")
                return Item.ToString();
        }

        return '';

    }
    /** Returns the value of a specified item
     * @protected
     * @param {any} Item The item to operate on.
     * @returns {any} Returns the value of a specified item
     */
    GetItemValue(Item) {
        if (!tp.IsEmpty(Item)) {
            if (tp.IsPrimitive(Item))
                return Item;

            let ValueField = this.GetListValueField();

            if (!tp.IsBlank(ValueField)) {
                if (this.ListSource instanceof tp.DataSource && Item instanceof tp.DataRow)
                    return this.ListSource.GetValue(Item, ValueField, null);
                return Item[ValueField];
            }
        }

        return null;
    }
    /** Returns true if the list is an object list.
     * @protected
     * @returns {boolean} Returns true if the list is an object list.
     */
    IsObjectItemList() {
        return tp.All(this.Items, (Item) => {
            return !tp.IsEmpty(Item) && !tp.IsPrimitive(Item);
        });
    }
    /** Returns the index of a specified text item
     * @protected
     * @param {string} S The text
     * @returns {number} Returns the index of a specified text item
     */
    IndexOfText(S) {
        var Result = -1;

        if (this.IsObjectItemList() && !tp.IsBlank(this.GetListDisplayField())) {
            let i, ln, List = this.Items;
            for (i = 0, ln = List.length; i < ln; i++) {
                if (tp.IsSameText(S, this.GetItemText(List[i]))) {
                    Result = i;
                    break;
                }
            }
        } else {
            Result = tp.ListIndexOfText(this.Items, S);
        }

        return Result;
    }
    /** Called when the selected index is changed 
     @protected
     */
    DoSelectedIndexChanged() {
        var Item = this.Items[this.SelectedIndex];

        this.fSelectedItem = Item;
        this.DoSetText(this.GetItemText(Item));

        if (!tp.IsEmpty(Item)) {
            if (tp.IsPrimitive(Item)) {
                this.fSelectedValue = Item;
            } else {
                this.fSelectedValue = this.GetItemValue(Item);
            }
        } else {
            this.fSelectedValue = null;
            this.DoSetText('');
        }

        this.DoPost();
        this.OnSelectedIndexChanged();
    }
    /** Called when the selected value is changed
     @protected
     */
    DoSelectedValueChanged() {
        if (this.Items.length > 0) {
            let i, ln, Item, v, Found,
                IsObjectList = this.IsObjectItemList();

            if (IsObjectList) {
                let List = this.Items;
                Found = false;
                for (i = 0, ln = List.length; i < ln; i++) {
                    Item = List[i];
                    v = this.GetItemValue(Item);
                    if (v === this.SelectedValue) {
                        this.fSelectedIndex = i;
                        this.fSelectedItem = Item;
                        this.DoSetText(this.GetItemText(Item));
                        Found = true;
                        break;
                    }
                }

                if (!Found) {
                    this.fSelectedIndex = -1;
                    this.fSelectedItem = null;
                    this.DoSetText('');
                }
            } else {
                this.fSelectedIndex = this.Items.indexOf(this.SelectedValue);
                this.fSelectedItem = this.SelectedValue;
                this.DoSetText(tp.IsPrimitive(this.SelectedValue) ? this.SelectedValue.toString() : '');
            }
        } else {
            this.fSelectedIndex = -1;
            this.fSelectedItem = null;
            this.DoSetText('');
        }

        this.DoPost();
        this.OnSelectedIndexChanged();
    }
    /** Called when the selected item is changed
     @protected
     */
    DoSelectedItemChanged() {
        var Item = this.SelectedItem;

        if (!tp.IsEmpty(Item)) {

            var IsObjectList = this.IsObjectItemList();
            if (IsObjectList) {
                this.fSelectedValue = this.GetItemValue(Item);
            } else {
                this.fSelectedValue = Item;
            }

            this.fSelectedIndex = this.Items.indexOf(Item);
            this.DoSetText(this.GetItemText(Item));

        } else {
            this.fSelectedIndex = -1;
            this.fSelectedValue = null;
            this.DoSetText('');
        }

        this.DoPost();
        this.OnSelectedIndexChanged();
    }
    /** Posts the change to the underlying datasource 
     @protected
     */
    DoPost() {
        // post the change
        if (this.IsDataBound && this.fCanPostDataValue) {
            this.WriteDataValue();
        }
    }
    /** Clears the selected item, index and value. Optionally posts the change to the underlying datasource
     * @protected
     * @param {boolean} PostFlag If true then it posts the change to the underlying datasource
     */
    DoClearValue(PostFlag) {
        this.fSelectedIndex = -1;
        this.fSelectedValue = null;
        this.fSelectedItem = null;

        if (PostFlag === true) {
            this.DoPost();
        }
    }
    /** Adds or removes the tp-Selected css class from/to the proper element
     * @protected
     * @param {boolean} Flag Controls the setting of the css class.
     */
    SetSelectionIndication(Flag) {
        var Index,
            el; // Element, 

        if (Flag === true) {
            // set the selection indication, if needed
            Index = this.SelectedIndex;
            el = this.GetElementByIndex(Index);
            if (el instanceof HTMLElement) {
                tp.AddClass(el, tp.Classes.Selected);
            }
        } else {
            // remove any selection indication
            el = tp.Select(this.fScroller.Container, '.' + tp.Classes.Selected);
            if (el) {
                tp.RemoveClass(el, tp.Classes.Selected);
            }
        }
    }

    /* notifications from ListItems */

    /** Notification from ListItems
     * @protected
     * @param {tp.ListEventArgs} Args The arguments
     */
    ListChanging(Args) {
        if (!tp.IsEmpty(this.ListSource)) {
            tp.Throw('ListItems modification not allowed.');
        } else {
            //
        }
    }
    /** Notification from ListItems
     * @protected
     * @param {tp.ListEventArgs} Args The arguments
     */
    ListChanged(Args) { 

        switch (Args.Action) {

            case tp.ListChangeType.Insert:
                this.UpdateScroller();
                break;

            case tp.ListChangeType.Remove:
                if (Args.Index === this.SelectedIndex)
                    this.SelectedIndex = 0;
                this.UpdateScroller();
                break;

            case tp.ListChangeType.Clear:
                this.SelectedValue = null;
                this.SetScrollerList();
                break;

            case tp.ListChangeType.Assign:
                if (this.IsDataBound) {
                    this.fCanPostDataValue = false;
                    try {
                        this.DoSelectedValueChanged();
                    } finally {
                        this.fCanPostDataValue = true;
                    }
                } else {
                    this.SelectedValue = null;
                    if (this.Items.length > 0)
                        this.SelectedIndex = 0;
                }
                this.SetScrollerList();
                break;

            case tp.ListChangeType.Update:
            case tp.ListChangeType.AddRange:
                if (this.IsDataBound) {
                    this.fCanPostDataValue = false;
                    try {
                        this.DoSelectedValueChanged();
                    } finally {
                        this.fCanPostDataValue = true;
                    }
                }
                this.SetScrollerList();
                break;

        }

    }

    /* notifications from ListSource */

    /** Notification from ListSource
     @protected
     @param {tp.DataTable} Table The table
     @param {tp.DataRow} Row The row
     */
    ListSourceRowCreated(Table, Row) {
    }
    /** Notification from ListSource
     @protected
     @param {tp.DataTable} Table The table
     @param {tp.DataRow} Row The row
     */
    ListSourceRowAdded(Table, Row) {
        this.UpdateScroller();
    }
    /**
    Notification from ListSource
    @protected
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    ListSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        this.UpdateScroller();
    }
    /** Notification from ListSource
     @protected
     @param {tp.DataTable} Table The table
     @param {tp.DataRow} Row The row
     */
    ListSourceRowRemoved(Table, Row) {
        this.UpdateScroller();
        var Index = this.Items.indexOf(Row);
        if (Index === this.SelectedIndex)
            this.SelectedIndex = 0;

    }
    /** Notification from ListSource
     @protected
     @param {tp.DataTable} Table The table
     @param {tp.DataRow} Row The row
     @param {number} Position The position
     */
    ListSourcePositionChanged(Table, Row, Position) {
    }
    /** Notification from ListSource
     @protected
     */
    ListSourceSorted() {
        this.UpdateScroller();
    }
    /** Notification from ListSource
     @protected
     */
    ListSourceFiltered() {
        if (!tp.InRange(this.Items, this.SelectedIndex)) {
            this.SelectedIndex = 0;
        }
        this.SetScrollerList();
    }
    /** Notification from ListSource. Occurs after a full update of the datasource (e.g. after a commit)
     @protected
     */
    ListSourceUpdated() {
        this.SetScrollerList();
    }
    /** Notification from ListSource
     @protected
     */
    ListSourceBind() {
        this.SetScrollerList();
        this.DoSelectedValueChanged();
        /*
        if (this.IsDataBound) {
            this.fCanPostDataValue = false;
            try {
                this.DoSelectedValueChanged();
            } finally {
                this.fCanPostDataValue = true;
            }
        }
        */
    }

    /* drop-down scrolling */

    /**
     * Returns a {@link HTMLElement} found at a specified index, if any, else null.
     * @protected
     * @param {number} Index The index
     * @returns {HTMLElement} Returns a {@link HTMLElement}
     */
    GetElementByIndex(Index) {
        if (tp.InRange(this.Items, Index)) {
            let Selector = tp.Format('div[data-index="{0}"]', Index);
            let el = tp.Select(this.fScroller.Container, Selector);

            if (el instanceof HTMLElement) {
                return el;
            }
        }

        return null;
    }
    /** Scrolls a row
     * @protected
     * @param {boolean} Up Controls the direction
     */
    RowScroll(Up) {
        if (this.Items.length > 0) {
            Up = Up === true;

            if (tp.ContainsElement(this.fScroller.Container, this.Document.activeElement)) {
                var el = this.Document.activeElement;
                if (el instanceof HTMLElement) {
                    let P; // Point
                    let sIndex = tp.Data(el, 'index');
                    let Index = tp.StrToInt(sIndex, -1);
                    Index = Up ? Index - 1 : Index + 1;

                    if (Index >= 0) {
                        let PageHeight = this.fScroller.Viewport.getBoundingClientRect().height;
                        P = tp.ToParent(el);
                        let Y = Up ? P.Y - this.ItemHeight : P.Y + this.ItemHeight;
                        if (Y < this.fScroller.Viewport.scrollTop || Y + this.ItemHeight > this.fScroller.Viewport.scrollTop + PageHeight) {
                            this.PageScroll(Up);
                        }

                        let el2 = this.GetElementByIndex(Index);
                        if (tp.IsHTMLElement(el2)) {
                            this.fScroller.Viewport.focus();
                            el.blur();
                            el2.focus();
                        }
                    }

                }
            }
        }
    }
    /** Scrolls a page
     * @protected
     * @param {boolean} Up Controls the direction
     */
    PageScroll(Up) {
        if (this.Items.length > 0) {
            Up = Up === true;

            let PageHeight = this.fScroller.Viewport.getBoundingClientRect().height;
            let n = Up ? this.fScroller.Viewport.scrollTop - PageHeight : this.fScroller.Viewport.scrollTop + PageHeight;
            this.fScroller.Viewport.scrollTop = tp.Truncate(n);
            this.fScroller.Container.focus();
        }
    }
    /** Scroll the control
     * @protected
     * @param {boolean} Start If true, then it scrolls from the start of the control
     */
    ControlScroll(Start) {
        if (this.Items.length > 0) {

            Start = Start === true;

            var PageHeight = this.fScroller.Viewport.getBoundingClientRect().height;
            let n = Start ? 1 : this.fScroller.Viewport.scrollTop = (this.ItemHeight * this.Items.length) - PageHeight; // this.fScroller.Viewport.scrollHeight;
            this.fScroller.Viewport.scrollTop = tp.Truncate(n);
            this.fScroller.Container.focus();
        }
    }

    /* drop-down call-backs */

    /** Drop-down call-back
     * @protected
     * @param {any} Row The Row
     * @param {number} RowIndex The row index
     * @returns {HTMLElement} Returns the newly added {@link HTMLElement}
     */
    ItemRenderFunc(Row, RowIndex) {
        var Result = this.Document.createElement('div');
        Result.className = tp.Classes.Item + ' ' + tp.Classes.UnSelectable;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        Result.tabIndex = -1;
        tp.SetElementInfo(Result, {
            Item: Row,
            Index: RowIndex
        });
        tp.Data(Result, 'Index', RowIndex);

        Result.innerHTML = this.GetItemText(Row);
        return Result;
    }

    /** Drop-down call-back
     * @protected
     * @param {number} Phase Indicates the phase. 1 = before scroll, 2 = after scroll
     */
    ScrollFunc(Phase) {
        if (Phase === 1) {                  // before scroll
            this.SetSelectionIndication(false);
        } else if (Phase === 2) {           // after scroll
            this.SetSelectionIndication(true);
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

        this.tpClass = 'tp.ListControl';
        this.fDefaultCssClasses = [tp.Classes.ListControl];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedValue';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.fSelectedIndex = -1;
        this.fItems = new tp.List();
    }
    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @protected
    @override
    @param {object} [o=null] - Optional. The create params object to processs.
    */
    ProcessCreateParams(o) {
        o = o || {};

        for (var Prop in o) {
            if (Prop === 'List' || Prop === 'ListItems' || Prop === 'Items') {
                this.fItems.AddRange(o[Prop]);
            } else if (!tp.IsFunction(o[Prop])) {
                this[Prop] = o[Prop];
            }
        }
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        // list items
        this.fItems.On('Changing', this.ListChanging, this);
        this.fItems.On('Changed', this.ListChanged, this);
        this.fItems.EventsEnabled = true;

        // list-data-source listener
        this.fListSourceListener = new tp.DataSourceListener(this);
        this.fListSourceListener.DataSourceRowCreated = this.ListSourceRowCreated;
        this.fListSourceListener.DataSourceRowAdded = this.ListSourceRowAdded;
        this.fListSourceListener.DataSourceRowModified = this.ListSourceRowModified;
        this.fListSourceListener.DataSourceRowRemoved = this.ListSourceRowRemoved;
        this.fListSourceListener.DataSourcePositionChanged = this.ListSourcePositionChanged;

        this.fListSourceListener.DataSourceSorted = this.ListSourceSorted;
        this.fListSourceListener.DataSourceFiltered = this.ListSourceFiltered;
        this.fListSourceListener.DataSourceUpdated = this.ListSourceUpdated;


    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.SetScrollerList();
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
    /**
    Reads the value from data-source and assigns the control's data value property
    @protected
    */
    ReadDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        this.fCanPostDataValue = false;
        try {
            if (this.IsDataBound && this.DataSource.Position >= 0) {
                this.ReadingDataValue = true;
                try {
                    var v = this.DataSource.Get(this.DataField);
                    this[this.DataValueProperty] = v;
                } finally {
                    this.ReadingDataValue = false;
                }
            }
        } finally {
            this.fCanPostDataValue = true;
        }
    }


    /* public */

    /** Clears the control */
    Clear() {
        this.fItems.Clear();
        this.DoSetText('');
    }
    /**
    Appends the items of a specified source array to this instance.
    @param  {any[]} Items - The source array.
    */
    AddRange(Items) {
        this.fItems.AddRange(Items);
    }

    /* notifications */
    /**
    Occurs when the selection is changed (index, value or item)
    */
    OnSelectedIndexChanged() {
        this.SetSelectionIndication(false);
        this.SetSelectionIndication(true);
        this.Trigger('SelectedIndexChanged', {});
    }

};

/** Field 
 @protected
 @type {tp.VirtualScroller}
 */
tp.ListControl.prototype.fScroller;
/** Field
 @protected
 @type {HTMLElement}
 */
tp.ListControl.prototype.fContainer;
/** Field
 @protected
 @type {any[]}
 */
tp.ListControl.prototype.fItems;
/** Field. An object that implements the DataSourceListener interface
 @protected
 @type {object}
 */
tp.ListControl.prototype.fListSourceListener;
/** Field
 @protected
 @type {tp.DataSource}
 */
tp.ListControl.prototype.fListSource;
/** Field
 @protected
 @type {string}
 */
tp.ListControl.prototype.fListSourceName;
/** Field
 @protected
 @type {string}
 */
tp.ListControl.prototype.fListValueField;
/** Field
 @protected
 @type {string}
 */
tp.ListControl.prototype.fListDisplayField;
/** Field
 @protected
 @type {number}
 */
tp.ListControl.prototype.fSelectedIndex = -1;
/** Field
 @protected
 @type {any}
 */
tp.ListControl.prototype.fSelectedValue;
/** Field
 @protected
 @type {any}
 */
tp.ListControl.prototype.fSelectedItem;
/** Field. Controls the writing of SelectedValue to the DataSource
 @protected
 @type {boolean}
 */
tp.ListControl.prototype.fCanPostDataValue;  
/** Field
 @protected
 @type {number}
 */
tp.ListControl.prototype.fItemHeight;

//#endregion

//#region tp.ComboBox
/**
   A combo-box control. <br />
   Example markup:
   <pre>
       <div data-setup="{ListOnly: true, ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0 }"></div>
   </pre>
   Example of the produced markup.
   <pre>
       <div class="tp-ComboBox">
           <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
       </div>
   </pre>
@implements {tp.IDropDownBoxListener}
*/
tp.ComboBox = class extends tp.ListControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <div data-setup="{ListOnly: true, ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0 }"></div>
    </pre>
    Example of the produced markup.
    <pre>
        <div class="tp-ComboBox">
            <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or sets a boolean value indicating whether the text-box portion of the control is editable
    @type {boolean}
    */
    get ListOnly() {
        return this.fListOnly === true;
    }
    set ListOnly(v) {
        if (this.fListOnly !== Boolean(v)) {
            this.fListOnly = Boolean(v);
            this.fTextBox.readOnly = this.fListOnly;
        }
    }
    /**
    Gets or sets, when ListOnly is false, the text of the control. 
    @type {string}
    */
    get Text() {
        return this.fTextBox instanceof HTMLInputElement ? this.fTextBox.value : '';
    }
    set Text(v) {
        if (!this.ListOnly && v !== this.Text && this.fTextBox instanceof HTMLInputElement) {
            this.fTextBox.value = v;
        }
    }
    /**
    Gets or sets a text the control displays as a hint to the user of what can be entered in the control. <br /> 
    The placeholder text must not contain carriage returns or line-feeds.
    @type {string}
    */
    get Placeholder() {
        return this.fTextBox instanceof HTMLInputElement ? this.fTextBox.placeholder : '';
    }
    set Placeholder(v) {
        if (this.fTextBox instanceof HTMLInputElement) {
            this.fTextBox.placeholder = v;
        }
    }
    /**
    Gets or sets the maximum number of items to be shown in the dropdown list
    @type {number}
    */
    get MaxDropdownItems() {
        let Result = this.fMaxDropdownItems || 10;
        if (Result > 30) {
            Result = 30;
        }
        return Result;
    }
    set MaxDropdownItems(v) {
        this.fMaxDropdownItems = v;
    }
    /**
    Returns true while the dropdown box is visible
    @type {boolean}
    */
    get IsOpen() {
        return this.fDropDownBox ? this.fDropDownBox.IsOpen : false;
    }

    /* protected */

    /** Sets a specified text
     * @protected
     * @param {string} Text The text
     */
    DoSetText(Text) {
        if (this.fTextBox instanceof HTMLInputElement)
            this.fTextBox.value = Text;
    }
    /** Returns the text of an item starting with a specified text.
     * @protected
     * @param {string} Text The text
     * @returns {string} Returns the text of the item, if found, else null.
     */
    GetItemTextStartingWith(Text) {
        let S,
            List = this.Items;

        for (var i = 0, ln = List.length; i < ln; i++) {
            S = this.GetItemText(List[i]);
            if (!tp.IsBlank(S) && tp.StartsWith(S, Text, true)) {
                return S;
            }
        }

        return null;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ComboBox';
        this.fDefaultCssClasses.push(tp.Classes.ComboBox);  

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedValue';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        this.fListOnly = true;

    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        // create the inner controls
        // <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
        let ControlContainer = tp.AppendElement(this.Handle, 'div');
        ControlContainer.className = tp.Classes.Strip;

        this.fTextBox = tp.AppendElement(ControlContainer, 'input');
        this.fTextBox.type = 'text';
        this.fTextBox.spellcheck = false;
        this.fTextBox.className = tp.Classes.Text;
        this.fTextBox.readOnly = this.ListOnly;

        this.fButton = tp.AppendElement(ControlContainer, 'div');
        this.fButton.className = tp.Classes.Btn;
        this.fButton.innerHTML = '&#9662;';

        // drop-down box (is the viewport for the scroller)
        this.fDropDownBox = new tp.DropDownBox(null, {
            Associate: ControlContainer,
            Owner: this,
            Parent: this.Handle
        });

        // items/rows container (is the container for the scroller)
        this.fContainer = this.Document.createElement('div');
        this.fDropDownBox.Handle.appendChild(this.fContainer);
        this.fContainer.className = tp.Classes.List;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        this.fContainer.tabIndex = -1;

        // virtual scroller
        this.fScroller = new tp.VirtualScroller(this.fDropDownBox.Handle, this.fContainer);
        this.fScroller.RowHeight = this.ItemHeight;
        this.fScroller.Context = this;
        this.fScroller.RenderRowFunc = this.ItemRenderFunc;
        this.fScroller.ScrollFunc = this.ScrollFunc;

        // events
        this.HookEvent(tp.Events.Change);
        this.HookEvent(tp.Events.KeyDown);
        this.HookEvent(tp.Events.KeyPress);
        tp.On(this.Document, tp.Events.Click, this, false);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {

        var Info, el, Index, c, ln, S, S2, Flag = false;
        var Type = tp.Events.ToTripous(e.type);


        switch (Type) {
            case tp.Events.LostFocus:
            case tp.Events.Change:
                tp.CancelEvent(e);

                S = tp.Trim(this.Text);
                if (!tp.IsBlank(S)) {
                    Index = this.IndexOfText(S);
                    if (Index >= 0) {
                        this.SelectedIndex = Index;
                    } else {
                        if (this.IsDataBound)
                            this.DoClearValue(true);
                        this.OnTextNotFound(S);
                    }
                } else {
                    if (this.IsDataBound)
                        this.DoClearValue(true);
                }
                break;

            case tp.Events.Click:
                if (this.fDropDownBox.Resizing !== true && this.Enabled === true) {
                    if (e.currentTarget === this.Document) {

                        Flag = (tp.ContainsEventTarget(this.fScroller.Container, e.target))
                            && tp.HasClass(e.target, tp.Classes.Item)
                            && tp.HasElementInfo(e.target);

                        if (Flag) {
                            Info = tp.GetElementInfo(e.target);
                            this.SelectedIndex = Info.Index;
                            this.Close();
                            this.fTextBox.focus();
                        } else if (e.target === this.fTextBox && this.ListOnly === true) {
                            this.Toggle();
                        } else if (e.target === this.fButton) {
                            this.Toggle();
                            if (this.IsOpen === false)
                                this.fTextBox.focus();
                        } else {
                            this.Close();
                        }

                    }
                }

                break;

            case tp.Events.KeyDown:
                if (e instanceof KeyboardEvent) {
                    if (this.Enabled && e.target === this.fTextBox) {

                        switch (e.keyCode) {
                            case tp.Keys.Up:
                            case tp.Keys.Down:
                                tp.CancelEvent(e);
                                e.preventDefault();
                                if (!tp.IsEmpty(e.altKey) && e.altKey) {
                                    if (!this.IsOpen)
                                        this.Open();
                                } else {
                                    this.SelectedIndex = e.keyCode === tp.Keys.Up ? this.SelectedIndex - 1 : this.SelectedIndex + 1;
                                }

                                return;
                        }
                    }

                    Flag = tp.ContainsEventTarget(this.fScroller.Container, e.target)
                        && tp.HasClass(e.target, tp.Classes.Item);

                    // WARNING: NOT AROW KEY SCROLLING NOT WORKING
                    // Set the container tabindex = -1 and each row tabindex = -1, and let the browser do the scrolling
                    if (Flag) {
                        switch (e.keyCode) {
                            case tp.Keys.Up:
                            case tp.Keys.Down:
                                //tp.CancelEvent(e);
                                //e.preventDefault();
                                //this.RowScroll(e.keyCode === tp.Keys.Up);
                                return;

                            case tp.Keys.PageUp:
                            case tp.Keys.PageDown:
                                //tp.CancelEvent(e);
                                //e.preventDefault();
                                //this.PageScroll(e.keyCode === tp.Keys.PageUp);
                                return;

                            case tp.Keys.Home:
                            case tp.Keys.End:
                                //tp.CancelEvent(e);
                                //e.preventDefault();
                                //this.ControlScroll(e.keyCode === tp.Keys.Home);
                                return;

                            case tp.Keys.Enter:
                                tp.CancelEvent(e);
                                Info = tp.GetElementInfo(e.target);
                                this.SelectedIndex = Info.Index;
                                this.Close();
                                this.fTextBox.focus();
                                return;

                            case tp.Keys.Escape:
                                e.preventDefault();
                                this.Close();
                                this.fTextBox.focus();
                                return;
                        }
                    }
                }

                break;

            case tp.Events.KeyPress:
                if (e instanceof KeyboardEvent) {
                    if (this.Enabled && !this.ReadOnly && (e.target === this.fTextBox) && this.ListOnly === false && tp.IsPrintableKey(e)) {
                        e.preventDefault();
                        el = this.fTextBox;
                        c = 'charCode' in e ? e.charCode : e.keyCode;
                        c = String.fromCharCode(c);
                        tp.TextBoxReplaceSelectedText(el, c);
                        ln = el.value.length;

                        S = this.GetItemTextStartingWith(el.value);

                        if (!tp.IsBlank(S)) {
                            el.value = S;
                            tp.TextBoxSelectText(el, ln, el.value.length);
                        }
                    }
                }


                break;

        }


        super.OnAnyDOMEvent(e);
    }

    /* public */
    /**
    Displays the dropdown box
    */
    Open() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Open();
    }
    /**
    Hides the dropdown box
    */
    Close() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Close();
    }
    /**
    Displays or hides the dropdown box
    */
    Toggle() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Toggle();
    }


    /**
    Called by the dropdown box to inform its owner about a stage change.
    @param {tp.DropDownBox} Sender The sender
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants.
    */
    OnDropDownBoxEvent(Sender, Stage) {
        let count,
            n,
            Index;

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
                tp.ZIndex(this.fScroller.Viewport, this.ZIndex + 1);
                this.fScroller.RowHeight = this.ItemHeight;
                //this.fScroller.SetRowList(this.Items);                    
                break;

            case tp.DropDownBoxStage.Opened:
                count = this.Items.length;
                n = count <= 0 ? 2 : (count < this.MaxDropdownItems ? count + 1 : this.MaxDropdownItems);
                n = (n * this.ItemHeight) + 5;
                this.fDropDownBox.Height = n;

                this.UpdateScroller();

                // remove any selection indication
                this.SetSelectionIndication(false);

                // scroll selected index into view
                Index = this.SelectedIndex;
                if (tp.InRange(this.Items, Index)) {
                    n = Index * this.fScroller.RowHeight;
                    this.fScroller.Viewport.scrollTop = n;
                }

                // set the selection indication, if needed
                this.SetSelectionIndication(true);

                this.fScroller.Viewport.focus();
                break;

            case tp.DropDownBoxStage.Closing:
                break;

            case tp.DropDownBoxStage.Closed:
                break;
        }



    }

    /* validation */
    /**
    Event trigger
    @protected
    @override
    */
    OnRequiredChanged() {
        this.SetRequiredMark(this.fTextBox);
        super.OnRequiredChanged();
    }
    /**
    Returns true if a form will validate when it is submitted, without having to submit it.
    @returns {boolean} Returns true if a form will validate when it is submitted, without having to submit it.
    */
    CheckValidity() {
        return tp.IsValidatableElement(this.fTextBox) ? this.fTextBox.checkValidity() : true;
    }
    /**
    Sets a custom error message that is displayed when a form is submitted.
    @param {string} MessageText - Sets a custom error message that is displayed when a form is submitted.
    */
    SetValidationMessage(MessageText) {
        if (tp.IsValidatableElement(this.fTextBox))
            this.fTextBox.setCustomValidity(MessageText);
    }

    /* notifications */
    /**
    Occurs when the text the user is typed in the text-box, does not exist in the item list.
    @param {string} Text The text the user types.
    */
    OnTextNotFound(Text) {
        this.Trigger('TextNotFound', { Text: Text });
    }
    /**
    Occurs when the selection is changed (index, value or item)
    */
    OnSelectedIndexChanged() {
        this.Trigger('SelectedIndexChanged', {});
    }

};

/** Field
 @protected
 @type {HTMLInputElement}
 */
tp.ComboBox.prototype.fTextBox;
/** Field
 @protected
 @type {HTMLElement}
 */
tp.ComboBox.prototype.fButton;
/** Field
 @protected
 @type {tp.DropDownBox}
 */
tp.ComboBox.prototype.fDropDownBox;
/** Field
 @protected
 @type {number}
 */
tp.ComboBox.prototype.fMaxDropdownItems;
/** Field
 @protected
 @type {boolean}
 */
tp.ComboBox.prototype.fListOnly;


//#endregion

//#region tp.ListBox
/**
A list-box control. <br />
Example markup:
<pre>
    <div data-setup="{ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0 }"></div>
</pre>
Example of the produced markup.
<pre>
    <div class="tp-ListBox">
         
    </div>
</pre>
*/
tp.ListBox = class extends tp.ListControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <div data-setup="{ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0 }"></div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ListBox';
        this.fDefaultCssClasses.push(tp.Classes.ListBox); 

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedValue';
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        // items/rows container (is the container for the scroller)
        this.fContainer = this.Document.createElement('div');
        this.Handle.appendChild(this.fContainer);
        this.fContainer.className = tp.Classes.List;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        this.fContainer.tabIndex = -1;

        // virtual scroller
        this.fScroller = new tp.VirtualScroller(this.Handle, this.fContainer);
        this.fScroller.RowHeight = this.ItemHeight;
        this.fScroller.Context = this;
        this.fScroller.RenderRowFunc = this.ItemRenderFunc;
        this.fScroller.ScrollFunc = this.ScrollFunc;

        // events
        this.HookEvent(tp.Events.Click);
        //this.HookEvent(tp.Events.KeyDown);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {

        var Info, Flag = false;
        var Type = tp.Events.ToTripous(e.type);


        switch (Type) {

            case tp.Events.Click:
                Flag = (tp.ContainsEventTarget(this.fScroller.Container, e.target))
                    && tp.HasClass(e.target, tp.Classes.Item)
                    && tp.HasElementInfo(e.target);

                if (Flag) {
                    Info = tp.GetElementInfo(e.target);
                    this.SelectedIndex = Info.Index;
                    this.fScroller.Viewport.focus();
                }

                break;

            // WARNING: ARROW KEY SCROLLING NOT WORKING
            // Set the container tabindex = -1 and each row tabindex = -1, and let the browser do the scrolling
            case tp.Events.KeyDown:
                if (e instanceof KeyboardEvent) {

                    Flag = tp.ContainsEventTarget(this.fScroller.Container, e.target)
                        && tp.HasClass(e.target, tp.Classes.Item);

                    if (Flag) {
                        switch (e.keyCode) {
                            case tp.Keys.Up:
                            case tp.Keys.Down:
                                //e.preventDefault();
                                //e.stopPropagation();
                                //this.RowScroll(e.keyCode === tp.Keys.Up);
                                return;

                            case tp.Keys.PageUp:
                            case tp.Keys.PageDown:
                                //e.preventDefault();
                                //e.stopPropagation();
                                //this.PageScroll(e.keyCode === tp.Keys.PageUp);
                                return;

                            case tp.Keys.Home:
                            case tp.Keys.End:
                                //e.preventDefault();
                                //e.stopPropagation();
                                //this.ControlScroll(e.keyCode === tp.Keys.Home);
                                return;

                            case tp.Keys.Enter:
                                //tp.CancelEvent(e);
                                //Info = tp.GetElementInfo(e.target as Node);
                                //this.SelectedIndex = Info.Index;
                                return;
                        }
                    }
                }

                break;



        }


        super.OnAnyDOMEvent(e);
    }

};
//#endregion

//#region tp.CheckListControl
/**
Base class for CheckComboBox and CheckListBox controls
*/
tp.CheckListControl = class extends tp.Control {

    /**
    Constructor <br /> 
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }


    /* properties */
    /**
    Gets or sets the array of selected indexes
    @type {number[]}
    */
    get SelectedIndexes() {
        return this.fSelectedIndexes;
    }
    set SelectedIndexes(v) {
        if (!tp.IsArray(v))
            v = [];

        this.fSelectedIndexes = v;
        this.OnSelectionChanged();

    }
    /**
    Gets or sets the array of selected values
    @type {any[]}
    */
    get SelectedValues() {
        let Item, Value, Result = [];

        let List = this.GetSelectedItems();

        for (let i = 0, ln = List.length; i < ln; i++) {
            Item = List[i];
            Value = this.GetItemValue(Item);
            Result.push(Value);
        }

        return Result;
    }
    set SelectedValues(v) {
        if (!tp.IsArray(v))
            v = [];

        this.fSelectedIndexes.length = 0;

        let Index;
        for (let i = 0, ln = v.length; i < ln; i++) {
            Index = this.IndexOfValue(v[i]);
            if (Index !== -1)
                this.fSelectedIndexes.push(Index);
        }

        this.OnSelectionChanged();
    }

    /**
    Gets or sets the array of the items. Used when the control is NOT data-bound.
    @type {any[]}
    */
    get Items() {
        return this.fListSource instanceof tp.DataSource ? this.fListSource.Rows : this.fItems;
    }
    set Items(v) {
        this.Clear();
        if (tp.IsArray(v)) {
            this.fItems.length = 0;
            this.fItems.AddRange(v);
        }
    }
    /**
    Gets or sets a tp.DataSource to be to be used as the source for the item list. It accepts a tp.DataTable too and uses that table to create the tp.DataSource.
    @type {any}
    */
    get ListSource() {
        return this.fListSource;
    }
    set ListSource(v) {
        if (v !== this.fListSource) {

            if (this.fListSource instanceof tp.DataSource) {
                this.fListSource.RemoveDataListener(this.fListSourceListener);
            }

            if (v instanceof tp.DataTable) {
                v = new tp.DataSource(v);
            }

            this.fListSource = v instanceof tp.DataSource ? v : null;

            if (this.fListSource instanceof tp.DataSource) {
                this.fListSource.AddDataListener(this.fListSourceListener);
                this.ListSourceBind();
            } else {
                this.Clear();
            }

        }

    }
    /**
    Gets or sets the name of the list datasource. Used in declarative scenarios.
    @type {string}
    */
    get ListSourceName() {
        return this.fListSourceName;
    }
    set ListSourceName(v) {
        this.fListSourceName = v;
    }

    /**
    Gets or sets the name of the value "field-name"/property of objects contained in the item list. <br />
    @type {string}
    */
    get ListValueField() {
        return !tp.IsBlank(this.fListValueField) ? this.fListValueField : this.ListDisplayField;
    }
    set ListValueField(v) {
        this.fListValueField = v;
    }
    /**
    Gets or sets the name of the display "field-name"/property of objects contained in the data list. <br />
    @type {string}
    */
    get ListDisplayField() {
        return this.fListDisplayField;
    }
    set ListDisplayField(v) {
        this.fListDisplayField = v;
    }

    /**
    The height of an item (line) in the dropdown box. If not specified then it results from a calculation.
    @type {number}
    */
    get ItemHeight() {
        if (tp.IsEmpty(this.fItemHeight) || this.fItemHeight <= 0)
            this.fItemHeight = tp.GetLineHeight(this.Handle);
        return this.fItemHeight;
    }
    set ItemHeight(v) {
        this.fItemHeight = v;
    }

    /* protected */

    /** Updates the scroller 
     @protected
     */
    UpdateScroller() {
        if (this.fScroller)
            this.fScroller.Update();
    }
    /** Sets the scroller list to display the list of items
     @protected
     */
    SetScrollerList() {
        if (this.fScroller) {
            this.fScroller.RowHeight = this.ItemHeight;
            this.fScroller.SetRowList(this.Items);
            this.fScroller.Update();
        }
    }
    /** Returns true if the list contains a specified field name
     @protected
     @param {string} FieldName The field name
     @returns {boolean} Returns true if the list contains a specified field name
     */
    ListContainsField(FieldName) {
        if (!tp.IsEmpty(this.ListSource)) {
            if (this.ListSource.Table instanceof tp.DataTable) {
                let Table = this.ListSource.Table;
                return Table.ContainsColumn(FieldName);
            }
        }

        if (!tp.IsEmpty(this.Items)) {
            if (this.Items.length > 0) {
                var Item = this.Items[0];
                if (!tp.IsPrimitive(Item))
                    return FieldName in Item;
            }
        }

        return false;
    }
    /** Returns the name of the ListValueField
     @protected
     @returns {string} Returns the name of the ListValueField
     */
    GetListValueField() {
        var Result = this.ListValueField;
        if (tp.IsBlank(Result)) {
            if (this.ListContainsField('Id'))
                Result = 'Id';
        }
        return Result;
    }
    /** Returns the name of the ListDisplayField
     @protected
     @returns {string} Returns the name of the ListDisplayField
     */
    GetListDisplayField() {
        var Result = this.ListDisplayField;
        if (tp.IsBlank(Result)) {
            if (this.ListContainsField('Name'))
                Result = 'Name';
        }
        return Result;
    }
    /** Returns the text of a specified item
     * @protected
     * @param {any} Item The item
     * @returns {string} Returns the text of a specified item
     */
    GetItemText(Item) {

        if (!tp.IsEmpty(Item)) {
            if (tp.IsPrimitive(Item)) {
                return Item.toString();
            }

            let DisplayField = this.GetListDisplayField();

            if (!tp.IsBlank(DisplayField)) {
                if (this.ListSource instanceof tp.DataSource && Item instanceof tp.DataRow)
                    return this.ListSource.GetValue(Item, DisplayField, '');
                return Item[DisplayField];
            }

            if (typeof Item.ToString === "function")
                return Item.ToString();
        }

        return '';

    }
    /** Returns the value of a specified item
     * @protected
     * @param {any} Item The item
     * @returns {any} Returns the value of a specified item
     */
    GetItemValue(Item) {
        if (!tp.IsEmpty(Item)) {
            if (tp.IsPrimitive(Item))
                return Item;

            let ValueField = this.GetListValueField();

            if (!tp.IsBlank(ValueField)) {
                if (this.ListSource instanceof tp.DataSource && Item instanceof tp.DataRow)
                    return this.ListSource.GetValue(Item, ValueField, null);
                return Item[ValueField];
            }

        }

        return null;
    }
    /** Returns true the list contains objects
     * @protected
     * @returns {boolean} Returns true the list contains objects
     * */
    IsObjectItemList() {
        return tp.All(this.Items, (Item) => {
            return !tp.IsEmpty(Item) && !tp.IsPrimitive(Item);
        });
    }
    /** Returns the index of a specified text
     * @protected
     * @param {string} S The text
     * @returns {number} Returns the index of a specified text
     */
    IndexOfText(S) {
        var Result = -1;

        if (this.IsObjectItemList() && !tp.IsBlank(this.GetListDisplayField())) {
            let i, ln, List = this.Items;
            for (i = 0, ln = List.length; i < ln; i++) {
                if (tp.IsSameText(S, this.GetItemText(List[i]))) {
                    Result = i;
                    break;
                }
            }
        } else {
            Result = tp.ListIndexOfText(this.Items, S);
        }

        return Result;
    }

    /* notifications from ListItems */

    /** Notification from ListItems
     * @protected
     * @param {tp.ListEventArgs} Args The {@link tp.ListEventArgs} arguments
     */
    ListChanging(Args) {
        if (!tp.IsEmpty(this.ListSource)) {
            tp.Throw('ListItems modification not allowed.');
        } else {
            //
        }
    }
    /** Notification from ListItems
     * @protected
     * @param {tp.ListEventArgs} Args The {@link tp.ListEventArgs} arguments
     */
    ListChanged(Args) {

        switch (Args.Action) {

            case tp.ListChangeType.Insert:
                this.UpdateScroller();
                break;

            case tp.ListChangeType.Remove:
                this.CheckIndex(Args.Index, false);
                this.UpdateScroller();
                this.OnSelectionChanged();
                break;

            case tp.ListChangeType.Clear:
            case tp.ListChangeType.Assign:
                this.Clear();
                this.SetScrollerList();
                this.OnSelectionChanged();
                break;

            case tp.ListChangeType.AddRange:
            case tp.ListChangeType.Update:
                this.SetScrollerList();
                this.OnSelectionChanged();
                break;
        }

    }

    /* notifications from ListSource */

    /** Notification from ListSource
     * @protected
     * @param {tp.DataTable} Table The {@link tp.DataTable}
     * @param {tp.DataRow} Row The {@link tp.DataRow}
     */
    ListSourceRowCreated(Table, Row) {
    }
    /** Notification from ListSource
     * @protected
     * @param {tp.DataTable} Table The {@link tp.DataTable}
     * @param {tp.DataRow} Row The {@link tp.DataRow}
     */
    ListSourceRowAdded(Table, Row) {
        this.UpdateScroller();
    }
    /** Notification from ListSource
     * @protected
     * @param {tp.DataTable} Table The {@link tp.DataTable}
     * @param {tp.DataRow} Row The {@link tp.DataRow}
     * @param {tp.DataColumn} Column The {@link tp.DataColumn}
     * @param {any} OldValue The old value
     * @param {any} NewValue The new value
     */
    ListSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        this.UpdateScroller();
    }
    /** Notification from ListSource
     * @protected
     * @param {tp.DataTable} Table The {@link tp.DataTable}
     * @param {tp.DataRow} Row The {@link tp.DataRow}
     */
    ListSourceRowRemoved(Table, Row) {
        this.UpdateScroller();
        var Index = this.Items.indexOf(Row);
        this.CheckIndex(Index, false);
    }
    /** Notification from ListSource
     * @protected
     * @param {tp.DataTable} Table The {@link tp.DataTable}
     * @param {tp.DataRow} Row The {@link tp.DataRow}
     * @param {number} Position The position
     */
    ListSourcePositionChanged(Table, Row, Position) {
    }
    /** Notification from ListSource
     * @protected
     */
    ListSourceSorted() {
        this.UpdateScroller();
    }
    /** Notification from ListSource
     * @protected
     */
    ListSourceFiltered() {
        this.Clear();
        this.SetScrollerList();
    }
    /** Notification from ListSource
     * @protected
     */
    ListSourceUpdated() {
        /// <summary>Occurs after a full update of the datasource (e.g. after a commit)</summary>
        this.SetScrollerList();
    }
    /** Notification from ListSource
     * @protected
     */
    ListSourceBind() {
        this.Clear();
    }

    /* drop-down call-backs */

    /** Drop-down call-back
     * @protected
     * @param {any} Row The row
     * @param {number} RowIndex The row index
     * @returns {HTMLLabelElement} Returns a {@link HTMLLabelElement}
     */
    ItemRenderFunc(Row, RowIndex) {
        var Result = this.Document.createElement('label');
        Result.className = tp.Classes.Item + ' ' + tp.Classes.UnSelectable;

        let cb = this.Document.createElement('input');
        cb.type = 'checkbox';
        Result.appendChild(cb);
        cb.checked = tp.Any(this.SelectedIndexes, (item) => item === RowIndex);

        tp.SetElementInfo(cb, {
            Item: Row,
            Index: RowIndex
        });

        if (cb.checked === true)
            tp.AddClass(Result, tp.Classes.Selected);

        tp.On(cb, tp.Events.Change, this);

        let S = this.GetItemText(Row);
        let text = this.Document.createTextNode(S);
        Result.appendChild(text);

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        Result.tabIndex = -1;
        //tp.SetElementInfo(Result, {
        //    Item: Row,
        //    Index: RowIndex,
        //});
        tp.Data(Result, 'Index', RowIndex);

        return Result;
    }
    /** Drop-down call-back
     * @protected
     * @param {number} Phase Indicates the phase. 1 = before scroll, 2 = after scroll
     */
    ScrollFunc(Phase) {
        if (Phase === 1) {                  // before scroll

        } else if (Phase === 2) {           // after scroll

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

        this.tpClass = 'tp.CheckListControl';
        this.fDefaultCssClasses = [tp.Classes.ListControl];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedValue';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.fItems = new tp.List();
        this.fSelectedIndexes = [];
    }
    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @protected
    @override
    @param {object} o - Optional. The create params object to processs.
    */
    ProcessCreateParams(o = null) {
        o = o || {};

        for (var Prop in o) {
            if (Prop === 'List' || Prop === 'ListItems' || Prop === 'Items') {
                this.fItems.AddRange(o[Prop]);
            } else if (!tp.IsFunction(o[Prop])) {
                this[Prop] = o[Prop];
            }
        }
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        // list items
        this.fItems.On('Changing', this.ListChanging, this);
        this.fItems.On('Changed', this.ListChanged, this);
        this.fItems.EventsEnabled = true;

        // list-data-source listener
        this.fListSourceListener = new tp.DataSourceListener(this);
        this.fListSourceListener.DataSourceRowCreated = this.ListSourceRowCreated;
        this.fListSourceListener.DataSourceRowAdded = this.ListSourceRowAdded;
        this.fListSourceListener.DataSourceRowModified = this.ListSourceRowModified;
        this.fListSourceListener.DataSourceRowRemoved = this.ListSourceRowRemoved;
        this.fListSourceListener.DataSourcePositionChanged = this.ListSourcePositionChanged;

        this.fListSourceListener.DataSourceSorted = this.ListSourceSorted;
        this.fListSourceListener.DataSourceFiltered = this.ListSourceFiltered;
        this.fListSourceListener.DataSourceUpdated = this.ListSourceUpdated;


    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();
        this.SetScrollerList();
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
    /**
    Reads the value from data-source and assigns the control's data value property
    */
    ReadDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        this.fCanPostDataValue = false;
        try {
            if (this.IsDataBound && this.DataSource.Position >= 0) {
                this.ReadingDataValue = true;
                try {
                    var v = this.DataSource.Get(this.DataField);
                    this[this.DataValueProperty] = v;
                } finally {
                    this.ReadingDataValue = false;
                }
            }
        } finally {
            this.fCanPostDataValue = true;
        }
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Info, Index, Type = tp.Events.ToTripous(e.type);

        if (tp.IsSameText(tp.Events.Change, Type)

            && tp.ContainsEventTarget(this.fContainer, e.target)
            && e.target instanceof HTMLInputElement
            && e.target.type === 'checkbox') {

            Info = tp.GetElementInfo(e.target);
            Index = Info.Index;

            this.CheckIndex(Index, e.target.checked);

            let Parent = tp.Closest(e.target, 'label');
            if (Parent instanceof HTMLElement) {
                if (e.target.checked === true)
                    tp.AddClass(Parent, tp.Classes.Selected);
                else
                    tp.RemoveClass(Parent, tp.Classes.Selected);
            }


            this.fScroller.Viewport.focus();
        }

        super.OnAnyDOMEvent(e);
    }

    /* public */

    /** Clears the controls
     * @override
     */
    Clear() {
        super.Clear();
        this.SelectedIndexes.length = 0;
    }

    /**
    Returns the index of a value, if any, else -1
    @param {any} v The value
    @returns {number} Returns the index of a value, if any, else -1
    */
    IndexOfValue(v) {
        let Item, Value, List = this.Items;
        for (let i = 0, ln = List.length; i < ln; i++) {
            Item = List[i];
            Value = this.GetItemValue(Item);
            if (v === Value)
                return i;
        }

        return -1;
    }

    /**
    Checks or un-checks an item by index and according to a specified flag.
    @param {number} Index The index of item
    @param {boolean} Flag True makes a checked item
    */
    CheckIndex(Index, Flag) {
        if (tp.InRange(this.Items, Index)) {
            if (Flag === true) {
                if (tp.Any(this.fSelectedIndexes, (item) => item === Index) === false) {
                    this.fSelectedIndexes.push(Index);
                    this.OnSelectionChanged();
                }
            }
            else {
                for (let i = 0, ln = this.fSelectedIndexes.length; i < ln; i++) {
                    if (this.fSelectedIndexes[i] === Index) {
                        tp.ListRemoveAt(this.fSelectedIndexes, i);
                        this.OnSelectionChanged();
                        break;
                    }
                }
            }
        }
    }
    /**
    Checks or un-checks an item by value and according to a specified flag.
    @param {any} v The value
    @param {boolean} Flag True makes a checked item
    */
    CheckValue(v, Flag) {
        let Index = this.IndexOfValue(v);
        this.CheckIndex(Index, Flag);
    }
    /**
    Checks or un-checks an item according to a specified flag.
    @param {any} Item The item
    @param {boolean} Flag True makes a checked item
    */
    CheckItem(Item, Flag) {
        let Index = this.Items.indexOf(Item);
        this.CheckIndex(Index, Flag);
    }

    /**
    Returns true if an item by a specified index is checked
    @param {number} Index The item index
    @returns {boolean} Returns true if an item by a specified index is checked
    */
    IsChecked(Index) {
        return tp.Any(this.fSelectedIndexes, (item) => item === Index);
    }
    /**
    Returns true if an item by a specified value is checked
    @param {any} v The value
    @returns {boolean} Returns true if an item by a specified value is checked
    */
    IsValueChecked(v)  {
        let Index = this.IndexOfValue(v);
        return this.IsChecked(Index);
    }
    /**
    Returns true if a specified item is checked
    @param {any} Item The item
    @returns {boolean} Returns true if a specified item is checked
    */
    IsItemChecked(Item) {
        let Index = this.Items.indexOf(Item);
        return this.IsChecked(Index);
    }

    /**
    Returns an array with the selected items
    @returns {any[]} Returns an array with the selected items
    */
    GetSelectedItems() {
        let Item, Index, Value,
            List = this.Items,
            Result = [];

        for (let i = 0, ln = this.fSelectedIndexes.length; i < ln; i++) {
            Index = this.fSelectedIndexes[i];
            Item = List[Index];
            Result.push(Item);
        }

        return Result;
    }

    /* event triggers */
    /**
    Event trigger. Occurs whenever the selection changes
    */
    OnSelectionChanged() {
        this.Trigger('SelectionChanged', {});
    }
};

/** Field
 @protected
 @type {tp.VirtualScroller}
 */
tp.CheckListControl.prototype.fScroller;
/** Field
 @protected
 @type {HTMLElement}
 */
tp.CheckListControl.prototype.fContainer;
/** Field
 @protected
 @type {tp.List}
 */
tp.CheckListControl.prototype.fItems;
/** Field
 @protected
 @type {number[]}
 */
tp.CheckListControl.prototype.fSelectedIndexes;
/** Field - a DataSourceListener
 @protected
 @type {object}
 */
tp.CheckListControl.prototype.fListSourceListener;
/** Field
 @protected
 @type {tp.DataSource}
 */
tp.CheckListControl.prototype.fListSource;
/** Field
 @protected
 @type {string}
 */
tp.CheckListControl.prototype.fListSourceName;
/** Field
 @protected
 @type {string}
 */
tp.CheckListControl.prototype.fListValueField;
/** Field
 @protected
 @type {string}
 */
tp.CheckListControl.prototype.fListDisplayField;
/** Field, controls the writing of SelectedValue to the DataSource
 @protected
 @type {boolean}
 */
tp.CheckListControl.prototype.fCanPostDataValue;
/** Field
 @protected
 @type {number}
 */
tp.CheckListControl.prototype.fItemHeight;

//#endregion

//#region tp.CheckListBox
/**
A multi-select list-box control with check-boxes. <br />
Example markup:
<pre>
   <div data-setup="{ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}] }"></div>
</pre>
*/
tp.CheckListBox = class extends tp.CheckListControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <div data-setup="{ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}] }"></div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.CheckListBox';
        this.fDefaultCssClasses.push(tp.Classes.CheckListBox);  

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedValues';
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        // items/rows container (is the container for the scroller)
        this.fContainer = this.Document.createElement('div');
        this.Handle.appendChild(this.fContainer);
        this.fContainer.className = tp.Classes.List;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        this.fContainer.tabIndex = -1;

        // virtual scroller
        this.fScroller = new tp.VirtualScroller(this.Handle, this.fContainer);
        this.fScroller.RowHeight = this.ItemHeight;
        this.fScroller.Context = this;
        this.fScroller.RenderRowFunc = this.ItemRenderFunc;
        this.fScroller.ScrollFunc = this.ScrollFunc;

        // events
        //this.HookEvent(tp.Events.Click);
        //this.HookEvent(tp.Events.KeyDown);
    }

};
//#endregion

//#region  tp.CheckComboBox
/**
A multi-select combo-box control with check-boxes. <br />
Example markup:
<pre>
    <div id="lbo" data-setup="{ Width: 300, ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}]  }"></div>
</pre>
@implements {tp.IDropDownBoxListener}
*/
tp.CheckComboBox = class extends tp.CheckListControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <div id="lbo" data-setup="{ Width: 300, ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}]  }"></div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or sets a boolean value indicating whether the text-box portion of the control is editable
    @type {boolean}
    */
    get ListOnly() {
        return this.fListOnly === true;
    }
    set ListOnly(v) {
        if (this.fListOnly !== Boolean(v)) {
            this.fListOnly = Boolean(v);
            this.fTextBox.readOnly = this.fListOnly;
        }
    }

    /**
    Gets or sets the maximum number of items to be shown in the dropdown list
    @type {number}
    */
    get MaxDropdownItems() {
        let Result = this.fMaxDropdownItems || 10;
        if (Result > 30) {
            Result = 30;
        }
        return Result;
    }
    set MaxDropdownItems(v) {
        this.fMaxDropdownItems = v;
    }
    /**
    Returns true while the dropdown box is visible
    @type {boolean}
    */
    get IsOpen() {
        return this.fDropDownBox ? this.fDropDownBox.IsOpen : false;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.CheckComboBox';
        this.fDefaultCssClasses.push(tp.Classes.CheckComboBox); 

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedValues';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.fLabels = []; // HTMLElement[]
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        // create the inner controls
        // <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
        this.fControlContainer = tp.AppendElement(this.Handle, 'div');
        this.fControlContainer.className = tp.Classes.Strip;

        this.fTextBox = tp.AppendElement(this.fControlContainer, 'input');
        this.fTextBox.type = 'text';
        this.fTextBox.className = tp.Classes.Text;
        this.fTextBox.spellcheck = false;
        //this.fTextBox.readOnly = this.ListOnly;


        // drop-down box (is the viewport for the scroller)
        this.fDropDownBox = new tp.DropDownBox(null, {
            Associate: this.fControlContainer,
            Owner: this,
            Parent: this.Handle
        });

        // items/rows container (is the container for the scroller)
        this.fContainer = this.Document.createElement('div');
        this.fDropDownBox.Handle.appendChild(this.fContainer);
        this.fContainer.className = tp.Classes.List;

        // -1 = still focusable by code,  0 = tab order relative to element's position in document, > 0 according to tab-order
        this.fContainer.tabIndex = -1;

        // virtual scroller
        this.fScroller = new tp.VirtualScroller(this.fDropDownBox.Handle, this.fContainer);
        this.fScroller.RowHeight = this.ItemHeight;
        this.fScroller.Context = this;
        this.fScroller.RenderRowFunc = this.ItemRenderFunc;
        this.fScroller.ScrollFunc = this.ScrollFunc;

        // events
        this.HookEvent(tp.Events.Change);
        this.HookEvent(tp.Events.KeyDown);
        this.HookEvent(tp.Events.KeyPress);

        tp.On(this.fTextBox, tp.Events.LostFocus, this, false);
        tp.On(this.Document, tp.Events.Click, this, false);
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {

        var Info, Index, Item, el, c, Flag = false;
        let List;
        var Type = tp.Events.ToTripous(e.type);


        switch (Type) {

            case tp.Events.Click:
                if (this.fDropDownBox.Resizing !== true && this.Enabled === true) {
                    if (e.currentTarget === this.Document) {

                        Flag = (!tp.ContainsEventTarget(this.Handle, e.target));

                        if (Flag) {
                            this.Close();
                        } else if (e.target === this.fTextBox) {
                            this.Toggle();
                        } else if (tp.HasClass(e.target, tp.Classes.Close)) {
                            Info = tp.GetElementInfo(e.target);
                            Index = Info.Index;
                            this.CheckIndex(Index, false);
                            this.Close();
                        }

                    }
                }

                break;

            case tp.Events.LostFocus:
                if (this.Enabled && !this.ReadOnly && (e.target === this.fTextBox)) {
                    this.fTextBox.value = '';
                    this.ResetScrollerList(this.Items, false);
                }
                break;

            case tp.Events.KeyDown:
                if (e instanceof KeyboardEvent) {
                    if (this.Enabled && !this.ReadOnly && (e.target === this.fTextBox)) {
                        switch (e.keyCode) {

                            case tp.Keys.Enter:
                                if (this.fTextBox.value.length > 2) {
                                    List = this.fScroller.GetRowList();
                                    if (List.length > 0) {
                                        Item = List[0];
                                        Index = this.Items.indexOf(Item);
                                        if (Index !== -1) {
                                            this.CheckIndex(Index, true);
                                            this.fTextBox.value = '';
                                            this.ResetScrollerList(this.Items, true);
                                        }
                                    }
                                }
                                break;
                            case tp.Keys.Delete:
                                if (this.fTextBox.value.length < 2) {
                                    this.ResetScrollerList(this.Items, true);
                                } else {
                                    this.FilterScrollerList();
                                }
                                break;
                            case tp.Keys.Backspace:
                                if (this.fTextBox.value.length === 0) {
                                    if (this.SelectedIndexes.length > 0) {
                                        e.preventDefault();
                                        Index = this.SelectedIndexes[this.SelectedIndexes.length - 1];
                                        this.CheckIndex(Index, false);
                                        setTimeout((self) => { self.fTextBox.focus(); }, 500, this);
                                    }
                                } else if (this.fTextBox.value.length < 2) {
                                    this.ResetScrollerList(this.Items, true);
                                } else {
                                    this.FilterScrollerList();
                                }
                                break;
                        }
                    }
                }
                break;

            case tp.Events.KeyPress:
                if (e instanceof KeyboardEvent) {
                    if (this.Enabled && !this.ReadOnly && (e.target === this.fTextBox) && this.ListOnly === false) {
                        if (tp.IsPrintableKey(e)) {
                            e.preventDefault();
                            el = this.fTextBox;
                            c = 'charCode' in e ? e.charCode : e.keyCode;
                            c = String.fromCharCode(c);
                            tp.TextBoxReplaceSelectedText(el, c);

                            this.FilterScrollerList();

                        } else {
                            //
                        }
                    }
                }
                break;

        }


        super.OnAnyDOMEvent(e);
    }

    /* protected */
    /** Updates label elements
     @protected
     */
    UpdateLabels() {
        let i, ln, Index, Item,
            elItem, //: HTMLElement,
            elText, //: HTMLElement,
            elBtn; //: HTMLElement;

        for (i = 0, ln = this.fLabels.length; i < ln; i++) {
            this.fLabels[i].parentNode.removeChild(this.fLabels[i]);
        }

        this.fLabels.length = 0;

        this.fTextBox.parentNode.removeChild(this.fTextBox);

        let List = this.SelectedIndexes;

        for (i = 0, ln = List.length; i < ln; i++) {
            Index = List[i];
            Item = this.Items[Index];

            elItem = this.Document.createElement('div');
            elItem.className = tp.Classes.Item;

            elText = this.Document.createElement('div');
            elItem.appendChild(elText);
            elText.className = tp.Classes.Text;
            elText.innerHTML = this.GetItemText(Item);

            elBtn = this.Document.createElement('div');
            elItem.appendChild(elBtn);
            elBtn.className = tp.Classes.Close;
            elBtn.innerHTML = '✕'; // '×'; // '✕';
            tp.SetElementInfo(elBtn, {
                Index: Index
            });

            //tp.InsertNode(this.fControlContainer, 0, elItem);
            this.fControlContainer.appendChild(elItem);
            this.fLabels.push(elItem);

            if (this.IsOpen) {
                this.fDropDownBox.UpdateTop();
            }
        }

        this.fControlContainer.appendChild(this.fTextBox);

    }
    /** Updates the drop-down's height
     * @protected
     * */
    UpdateDropdownHeight() {
        let count,
            n;

        count = this.fScroller.RowListCount;
        n = count <= 0 ? 2 : (count < this.MaxDropdownItems ? count + 1 : this.MaxDropdownItems);
        n = (n * this.ItemHeight) + 5;
        this.fDropDownBox.Height = n;
    }
    /** Returns a list of items containing a specified text
     * @protected
     * @param {string} Text The text
     * @returns {any[]}  Returns a list of items containing a specified text
     */
    GetItemsContainingText(Text) {
        let S,
            List = this.Items,
            Result = [];

        for (var i = 0, ln = List.length; i < ln; i++) {
            S = this.GetItemText(List[i]);
            if (!tp.IsBlank(S) && tp.ContainsText(S, Text, true)) {
                Result.push(List[i]);
            }
        }

        return Result;
    }
    /** Filters the scroller list
     * @protected
     */
    FilterScrollerList() {
        let List;

        if (this.fTextBox.value.length === 0 || this.fTextBox.value.length >= 3) {
            List = tp.IsBlank(this.fTextBox.value) ? this.Items : this.GetItemsContainingText(this.fTextBox.value);
            this.Open();
            this.fScroller.SetRowList(List);
            this.UpdateDropdownHeight();
            this.UpdateScroller();
        }
    }
    /** Resets the scroller list
     * @protected
     * @param {any[]} List The list
     * @param {boolean} FocusToTextBox A boolean value whether to set focus to the text box
     */
    ResetScrollerList(List, FocusToTextBox) {
        this.fScroller.SetRowList(List);
        this.UpdateDropdownHeight();
        this.UpdateScroller();

        if (FocusToTextBox === true)
            setTimeout((self) => { self.fTextBox.focus(); }, 500, this);
    }

    /* public */
    /**
    Displays the dropdown box
    */
    Open() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Open();
    }
    /**
    Hides the dropdown box
    */
    Close() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Close();
    }
    /**
    Displays or hides the dropdown box
    */
    Toggle() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Toggle();
    }

    /**
    Called by the dropdown box to inform its owner about a stage change.
    @param {tp.DropDownBox} Sender The sender {@link tp.DropDownBox}
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants
    */
    OnDropDownBoxEvent(Sender, Stage) { 

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
                tp.ZIndex(this.fScroller.Viewport, this.ZIndex + 1);
                this.fScroller.RowHeight = this.ItemHeight;
                break;

            case tp.DropDownBoxStage.Opened:
                this.UpdateDropdownHeight();
                this.UpdateScroller();

                if (this.ListOnly === true)
                    this.fScroller.Viewport.focus();
                break;

            case tp.DropDownBoxStage.Closing:
                break;

            case tp.DropDownBoxStage.Closed:
                break;
        }



    }

    /* event triggers */
    /**
    Event trigger. Occurs whenever the selection changes
    @override
    */
    OnSelectionChanged() {
        this.UpdateLabels();
        super.OnSelectionChanged();
    }
};

 

/** Field
 * @protected
 * @type {HTMLElement}
 * */
tp.CheckComboBox.prototype.fControlContainer;
/** Field
 * @protected
 * @type {HTMLInputElement}
 * */
tp.CheckComboBox.prototype.fTextBox;
/** Field
 * @protected
 * @type {tp.DropDownBox}
 * */
tp.CheckComboBox.prototype.fDropDownBox;
/** Field
 * @protected
 * @type {number}
 * */
tp.CheckComboBox.prototype.fMaxDropdownItems;
/** Field
 * @protected
 * @type {HTMLElement[]}
 * */
tp.CheckComboBox.prototype.fLabels;
/** Field
 * @protected
 * @type {boolean}
 * */
tp.CheckComboBox.prototype.fListOnly;
//#endregion


//#region tp.HtmlListControl
/** 
Base class for the HTMLComboBox and HTMLListBox
@implements {tp.ISelectedIndex}
*/
tp.HtmlListControl = class extends tp.Control {

    /**
    Constructor
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Returns the number of items (options)
    @type {number}
    */
    get Count() {
        return this.Handle instanceof HTMLSelectElement ? this.Handle.length : 0;
    }
    /**
    Gets or sets the selected index, if any, else -1.
    @type {number}
    */
    get SelectedIndex() {
        return this.Handle instanceof HTMLSelectElement ? this.Handle.selectedIndex : -1;
    }
    set SelectedIndex(v) {
        if (this.Handle instanceof HTMLSelectElement) {
            this.Handle.selectedIndex = v;
        }
    }
    /**
    Returns the selected item, if any, else null
    @type {HTMLOptionElement}
    */
    get SelectedItem() {
        if (this.Handle instanceof HTMLSelectElement) {
            if (this.Handle.selectedIndex > -1) {
                return this.Handle.options[this.Handle.selectedIndex];
            }
        }

        return null;
    }
    /** Returns the selected value, if any, else null
     @type {string}
     */
    get SelectedValue() {
        var o = this.SelectedItem;
        return o ? o.value : null;
    }
    /**
    Returns the collection of items (options)
    @type {HTMLOptionsCollection}
    */
    get Items() {
        if (this.Handle instanceof HTMLSelectElement) {
            return this.Handle.options;
        }

        return null;
    }



    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.HtmlListControl';
        this.fElementType = 'select';

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.List;
        this.fDataValueProperty = 'SelectedIndex';
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        this.HookEvent(tp.Events.Change);
    }

    /**
    Processes the this.CreateParams by applying its properties to the properties of this instance
    @protected
    @override
    @param {object} [o] - Optional. The create params object to processs.
    */
    ProcessCreateParams(o) {
        o = o || {};

        for (var Prop in o) {
            if (Prop === 'List' || Prop === 'ListItems' || Prop === 'Items') {
                this.AddRange(o[Prop]);
            } else if (!tp.IsFunction(o[Prop])) {
                this[Prop] = o[Prop];
            }
        }
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);

        switch (Type) {
            case tp.Events.Change:
                this.OnSelectedIndexChanged();
                break;
        }

        super.OnAnyDOMEvent(e);
    }

    /* public */
    /**
    Removes all items (options)
    */
    Clear() {
        if (this.Handle instanceof HTMLSelectElement) {
            for (let i = this.Handle.options.length - 1; i >= 0; i--) {
                this.Handle.remove(i);
            }
        }

        super.Clear();
    }
    /**
    Adds and returns a {@link HTMLOptionElement} item (option)
    @param {string} Text The text of the item
    @param {string} Value The value of the item
    @returns {HTMLOptionElement} Returns the newly added {@link HTMLOptionElement} item (option)
    */
    Add(Text, Value) {
        if (this.Handle instanceof HTMLSelectElement) {
            let Result = this.Handle.ownerDocument.createElement('option');
            Result.text = Text;
            Result.value = Value;
            this.Handle.add(Result);    // add(element: HTMLElement, before?: HTMLElement | number): void;
            return Result;
        }

        return null;
    }
    /**
    Inserts an item at a specified index and returns the {@link HTMLOptionElement} item (option)
    @param {number} Index The item index
    @param {string} Text The item text
    @param {string} Value The item value
    @returns {HTMLOptionElement} Returns the newly added {@link HTMLOptionElement} item (option)
    */
    Insert(Index, Text, Value) {
        if (this.Handle instanceof HTMLSelectElement) {
            let Result = this.Handle.ownerDocument.createElement('option');
            Result.text = Text;
            Result.value = Value;
            this.Handle.add(Result, Index); // add(element: HTMLElement, before?: HTMLElement | number): void;
            return Result;
        }

        return null;
    }
    /**
    Removes an item (option) by index
    @param {number} Index The item index
    */
    RemoveAt(Index) {
        if (this.Handle instanceof HTMLSelectElement) {
            this.Handle.remove(Index);
        }
    }

    /**
    Adds a range of items to the list, where each item is an object of <code> { Id: '', Name: '' } or { Value: '', Text: '' } </code>
    @param {any[]} List - Array of <code> { Id: '', Name: '' } or { Value: '', Text: '' } </code>
    */
    AddRange(List) {
        if (this.Handle instanceof HTMLSelectElement) {
            let o, Text, Value;
            for (let i = 0, ln = List.length; i < ln; i++) {
                o = List[i];

                Value = '';
                Text = '';

                if ('Id' in o) {
                    Value = o['Id'];
                } else if ('Value' in o) {
                    Value = o['Value'];
                }

                if ('Name' in o) {
                    Text = o['Name'];
                } else if ('Text' in o) {
                    Text = o['Text'];
                }

                if (Text === '') {
                    Text = i.toString();
                }

                if (Value === '') {
                    Value = Text;
                }

                this.Add(Text, Value);
            }
        }
    }



    /**
    Returns the index of an item with a specified text, if any, else -1
    @param {string} Text The text
    @returns {number} Returns the index of an item with a specified text, if any, else -1
    */
    IndexOfText(Text) {
        if (this.Handle instanceof HTMLSelectElement) {
            let List = this.Handle.options;
            for (let i = 0, ln = List.length; i < ln; i++) {
                if (Text === (List[i]).text)
                    return i;
            }
        }

        return -1;
    }
    /**
    Returns the index of an item with a specified value, if any, else -1
    @param {string} Value The value
    @returns {number} Returns the index of an item with a specified value, if any, else -1
    */
    IndexOfValue(Value) {
        if (this.Handle instanceof HTMLSelectElement) {
            let List = this.Handle.options;
            for (let i = 0, ln = List.length; i < ln; i++) {
                if (Value === (List[i]).value)
                    return i;
            }
        }

        return -1;
    }


    /**
    Returns an item (option) by index
    @param {number} Index The item index
    @returns {HTMLOptionElement} Returns a {@link HTMLOptionElement}
    */
    ItemAt(Index) {
        if (this.Handle instanceof HTMLSelectElement) {
            return this.Handle.options[Index];
        }
        return null;
    }
    /**
    Returns the text of an item (option) at a specified index
    @param {number} Index The item index
    @returns {string} Returns the text of an item (option) at a specified index
    */
    GetTextAt(Index) {
        return this.ItemAt(Index).text;
    }
    /**
    Sets the text of an item (option) at a specified index
    @param {number} Index The item index
    @param {string} Text The text used in locating the item index
    */
    SetTextAt(Index, Text) {
        this.ItemAt(Index).text = Text;
    }
    /**
    Returns the value of an item (option) at a specified index
    @param {number} Index The item index
    @returns {string} Returns the value of an item (option) at a specified index
    */
    GetValueAt(Index)  {
        return this.ItemAt(Index).value;
    }
    /**
    Sets the value of an item (option) at a specified index
    @param {number} Index The item index
    @param {string} Value The text value to set to the item
    */
    SetValueAt(Index, Value) {
        this.ItemAt(Index).value = Value;
    }


    /* notifications */
    /**
    Occurs when the selection is changed (index, value or item)
    */
    OnSelectedIndexChanged() {
        this.Trigger('SelectedIndexChanged', {});
    }
};
//#endregion

//#region  tp.HtmlComboBox
/**
A combo-box build up-on a select tag (HTMLSelectElement) <br />
Example markup:
<pre>
    <select id="cbo" data-setup="{ Width: 140,  List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}]  }"></select>
</pre>
*/
tp.HtmlComboBox = class extends tp.HtmlListControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <select id="cbo" data-setup="{ Width: 140,  List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}]  }"></select>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.HtmlComboBox';
        this.fDefaultCssClasses = [tp.Classes.HtmlComboBox];
    }
};
//#endregion

//#region  tp.HtmlListBox
/**
A single-select or multi-select list-box build up-on a select tag (HTMLSelectElement)
Example markup:
<pre>
    <select id="lbo" data-setup="{ Width: 140, Height: 240,  List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}]  }"></select>
</pre>
*/
tp.HtmlListBox = class extends tp.HtmlListControl {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <select id="lbo" data-setup="{ Width: 140, Height: 240,  List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}]  }"></select>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Gets or sets the number of visible list-box items, actually its height
    @type {number}
    */
    get VisibleItemCount() {
        return this.Handle instanceof HTMLSelectElement ? this.Handle.size : 0;
    }
    set VisibleItemCount(v) {
        if (this.Handle instanceof HTMLSelectElement) {
            this.Handle.size = v;
        }
    }
    /**
    Sets or sets the boolean value indicating whether multiple items can be selected.
    @type {boolean}
    */
    get MultiSelect() {
        return this.Handle instanceof HTMLSelectElement ? this.Handle.multiple : false;
    }
    set MultiSelect(v) {
        if (this.Handle instanceof HTMLSelectElement) {
            this.Handle.multiple = v;
        }
    }
    /**
    Gets or sets the selected indexes of a multi-select list
    @type { number[]}
    */
    get SelectedIndexes() {
        let Result = [];
        if (this.Handle instanceof HTMLSelectElement) {
            var A = this.Handle.options;
            var o, // HTMLOptionElement
                i, ln;
            for (i = 0, ln = A.length; i < ln; i++) {
                o = A[i];
                if (o.selected === true) {
                    Result.push(i);
                }
            }
        }
        return Result;
    }
    set SelectedIndexes(v) {
        if (this.Handle instanceof HTMLSelectElement) {
            this.SelectAll(false);
            v = tp.IsArray(v) ? v : [];
            var i, ln;
            for (i = 0, ln = v.length; i < ln; i++) {
                this.ItemAt(v[i]).selected = true;
            }
        }
    }
    /**
    Gets or sets the selected values of a multi-select list
    @type {string[]}
    */
    get SelectedValues() {
        let Result = [];
        if (this.Handle instanceof HTMLSelectElement) {
            var A = this.Handle.options;
            var o, // HTMLOptionElement
                i, ln;
            for (i = 0, ln = A.length; i < ln; i++) {
                o = A[i];
                if (o.selected === true) {
                    Result.push(o.value);
                }
            }
        }
        return Result;
    }
    set SelectedValues(v) {
        if (this.Handle instanceof HTMLSelectElement) {
            this.SelectAll(false);
            v = tp.IsArray(v) ? v : [];
            var i, ln, Index;
            for (i = 0, ln = v.length; i < ln; i++) {
                Index = this.IndexOfValue(v[i]);
                if (Index !== -1)
                    this.ItemAt(Index).selected = true;
            }
        }
    }

    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        if (this.Handle instanceof HTMLSelectElement) {
            this.Handle.size = 8;
        }
    }

    /* public */
    /**
    Selects or de-selects all items (options) according to a specified flag. To be used with multi-select lists
    @param {boolean} Flag The flag that controls whether to select items
    */
    SelectAll(Flag) {
        if (this.Handle instanceof HTMLSelectElement) {
            var A = this.Handle.options;
            var i, ln;
            for (i = 0, ln = A.length; i < ln; i++) {
                (A[i]).selected = Flag;
            }
        }
    }
    /**
    Returns an array with all selected items (options)
    @returns {HTMLOptionElement[]} Returns an array of {@link HTMLOptionElement} items with all selected items (options)
    */
    GetSelectedItems() {
        let Result = [];
        if (this.Handle instanceof HTMLSelectElement) {
            var A = this.Handle.options;
            var i, ln;
            for (i = 0, ln = A.length; i < ln; i++) {
                if ((A[i]).selected)
                    Result.push(A[i]);
            }
        }
        return Result;
    }
};
//#endregion

//#region tp.HtmlNumberBox
/**
A number box control built on top of a input type="number" element.
@see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/number|mdn}
Example markup:
<pre>
    <input type="number" id="HtmlNumberBox"  step="0.100"  value='50' />
</pre>
*/
tp.HtmlNumberBox = class extends tp.InputControl {
    /**
    Constructor <br />
    Example markup:
    <pre>
        <input type="number" id="HtmlNumberBox"  step="0.1" , value='50' />
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Gets or sets the minimum value of the control. <br />
    @type {number}
    */
    get Min() {
        return this.Handle instanceof HTMLInputElement ? (tp.IsBlank(this.Handle.min) ? null : Number(this.Handle.min)) : 0;
    }
    set Min(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.min = tp.IsEmpty(v) ? '' : v.toString();
    }
    /**
    Gets or sets the maximum value of the control. <br />
    @type {number}
    */
    get Max() {
        return this.Handle instanceof HTMLInputElement ? (tp.IsBlank(this.Handle.max) ? null : Number(this.Handle.max)) : 0;
    }
    set Max(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.max = tp.IsEmpty(v) ? '' : v.toString();
    }
    /**
    Gets or sets the step of a value change. <br />
    CAUTION: Set it as string in order to infer the decimal places to be used when formatting the value.
    NOTE: One issue with number inputs is that their step size is 1 by default — if you try to enter a number with a decimal, such as "1.0",
    it will be considered invalid. If you want to enter a value that requires decimals, you'll need to reflect this in the step value
    (e.g. step="0.01" to allow decimals to two decimal places). Here's a simple example:
    @see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/number|mdn}
    @type {string}
    */
    get Step() {
        return this.Handle instanceof HTMLInputElement && !isNaN(this.Handle.step) ? this.Handle.step : 1;
    }
    set Step(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.step = v.toString();
    }


    /**
    Gets or sets the value of the control.  
    @type {number}
    */
    get Value() {
        if (this.Handle instanceof HTMLInputElement) {
            return tp.IsBlank(this.Handle.value) ? null : Number(this.Handle.value);
        }
        return null;
    }
    set Value(v) {
        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.value = tp.IsEmpty(v) ? '' : v.toString();
            this.OnValueChanged();
        }
    }
 
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.HtmlNumberBox';
        this.fElementSubType = 'number';
        this.fDefaultCssClasses = [tp.Classes.HtmlNumberBox];

        // data-bind
        this.fDataValueProperty = 'Value';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.min = !tp.IsBlank(this.Handle.min) ? this.Handle.min : '';
            this.Handle.max = !tp.IsBlank(this.Handle.max) ? this.Handle.max : '';
            this.Handle.value = !tp.IsBlank(this.Handle.value) ? this.Handle.value : '0';
            this.Handle.step = !tp.IsBlank(this.Handle.step) ? this.Handle.step : '0.1';
        }
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }

 
};

/** Field
 * @protected
 * @type {any[]}
 */
tp.HtmlNumberBox.prototype.fValueList;

//#endregion

//#region tp.HtmlNumberBoxEx

/**
A custom number box control. Contains a styled input type="number" element. Provides minus and plus buttons.  <br />
NOTE: adding the css class tp-MinusFirst places the minus sign first, that is at the left of the number input control <br />
NOTE2: decimal places are set by setting the decimal places of the value of the Step property
@see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/number|mdn}
Example markup:
<pre>
    <div id="Box" class="tp-MinusFirst" data-setup="{ Width: 200, Min: 0, Max: 200, Value: 100, Step: '0.500'  }"></div>
</pre>
Example of the produced markup.
<pre>
    <div id="Box" class="tp-MinusFirst tp-NumberBox tp-Object" tabindex="0" style="width: 200px;">
        <input type="number" class="tp-Text" min="0" max="200" step="0.500">
        <div class="tp-Minus">▾</div>
        <div class="tp-Plus">▴</div>
    </div>
</pre>
*/
tp.HtmlNumberBoxEx = class extends tp.Control {

    /**
    Constructor <br />
    Example markup:
    <pre>
         <div>
            <div id="Box" data-setup="{ Min: 0, Max: 200, Value: 100, Step: 0.5 }"></div>
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Gets or sets the minimum value of the control. <br />
    @type {number}
    */
    get Min() {
        return this.fTextBox instanceof HTMLInputElement ? (tp.IsBlank(this.fTextBox.min) ? null : Number(this.fTextBox.min)) : 0;
    }
    set Min(v) {
        if (this.fTextBox instanceof HTMLInputElement)
            this.fTextBox.min = tp.IsEmpty(v) ? '' : v.toString();
    }
    /**
    Gets or sets the maximum value of the control. <br />
    @type {number}
    */
    get Max() {
        return this.fTextBox instanceof HTMLInputElement ? (tp.IsBlank(this.fTextBox.max) ? null : Number(this.fTextBox.max)) : 0;
    }
    set Max(v) {
        if (this.fTextBox instanceof HTMLInputElement)
            this.fTextBox.max = tp.IsEmpty(v) ? '' : v.toString();
    }
    /**
    Gets or sets the step of a value change. <br />
    CAUTION: Set it as string in order to infer the decimal places to be used when formatting the value.
    NOTE: One issue with number inputs is that their step size is 1 by default — if you try to enter a number with a decimal, such as "1.0",
    it will be considered invalid. If you want to enter a value that requires decimals, you'll need to reflect this in the step value
    (e.g. step="0.01" to allow decimals to two decimal places). Here's a simple example:
    @see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/number|mdn}
    @type {string}
    */
    get Step() {
        return this.fTextBox instanceof HTMLInputElement && !isNaN(this.fTextBox.step) ? this.fTextBox.step : 1;
    }
    set Step(v) {
        if (this.fTextBox instanceof HTMLInputElement)
            this.fTextBox.step = v.toString();
    }

    /**
    Gets or sets the value of the control.  
    @type {number}
    */
    get Value() {
        if (this.fTextBox instanceof HTMLInputElement) {
            return tp.IsBlank(this.fTextBox.value) ? null : Number(this.fTextBox.value);
        }
        return null;
    }
    set Value(v) {
        if (this.fTextBox instanceof HTMLInputElement) {
            this.fTextBox.value = tp.IsEmpty(v) ? '' : v.toString();
            this.OnValueChanged();
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

        this.tpClass = tp.HtmlNumberBoxEx;

        this.fDefaultCssClasses = [tp.Classes.NumberBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Value';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        // input type="number"
        this.fTextBox = tp.AppendElement(this.Handle, 'input');
        this.fTextBox.type = 'number';
        this.fTextBox.className = tp.Classes.Text;
        this.fTextBox.min = !tp.IsBlank(this.fTextBox.min) ? this.fTextBox.min : '';
        this.fTextBox.max = !tp.IsBlank(this.fTextBox.max) ? this.fTextBox.max : '';
        this.fTextBox.value = !tp.IsBlank(this.fTextBox.value) ? this.fTextBox.value : '0';
        this.fTextBox.step = !tp.IsBlank(this.fTextBox.step) ? this.fTextBox.step : '0.1';

        this.fMinus = tp.AppendElement(this.Handle, 'div');
        this.fMinus.className = tp.Classes.Minus;
        this.fMinus.innerHTML = tp.HtmlNumberBoxEx.MinusSymbol;

        this.fPlus = tp.AppendElement(this.Handle, 'div');
        this.fPlus.className = tp.Classes.Plus;
        this.fPlus.innerHTML = tp.HtmlNumberBoxEx.PlusSymbol;

        this.fPlus.addEventListener("click", () => { this.StepUp(); this.OnValueChanged(); });
        this.fMinus.addEventListener("click", () => { this.StepDown(); this.OnValueChanged(); });

        // NOTE: the change event is not triggered by the stepUp() and stepDown() methods.
        this.fTextBox.addEventListener("change", () => { this.OnValueChanged(); });
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }

    /** Increases the value by the defined step */
    StepUp() {
        this.fTextBox.stepUp();
    }
    /** Decreases the value by the defined step */
    StepDown() {
        this.fTextBox.stepDown();
    }

    /* Event triggers */
    /**
    Called on any value change
    */
    OnValueChanged() {
        if (!this.ReadingDataValue) {
            this.Trigger('ValueChanged', {});
        }
    }
};


/** This is the real input type="number" element
* @protected
* @type {HTMLInputElement}
*/
tp.HtmlNumberBoxEx.prototype.fTextBox;
/** The div with the minus sign
* @protected
* @type {HTMLDivElement}
*/
tp.HtmlNumberBoxEx.prototype.fMinus;
/** The div with the plus sign
* @protected
* @type {HTMLDivElement}
*/
tp.HtmlNumberBoxEx.prototype.fPlus;
/** Field. The expand symbol.
 * @static
 * @type {string}
 */
tp.HtmlNumberBoxEx.PlusSymbol = '▴';      // ➕ + ▲ ▴
/** Field. The collapse symbol.
 * @static
 * @type {string}
 */
tp.HtmlNumberBoxEx.MinusSymbol = '▾';    // ➖ - ▼ ▾
//#endregion

//#region tp.HtmlDateBox
/** A date control built on top of a input type="date" element.*/
tp.HtmlDateBox = class extends tp.InputControl {

    /**
    Constructor <br />
    SEE: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/date#browser_compatibility
    Example markup:
    <pre>
        <input type="date" value='2021-12-25'  />
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }


    /**
    Gets or sets the value of the control.  
    @type {Date}
    */
    get Date() {
        if (this.Handle instanceof HTMLInputElement) {
            return tp.IsBlank(this.Handle.value) ? null : Date(this.Handle.value);
        }
        return null;
    }
    set Date(v) {
        if (this.Handle instanceof HTMLInputElement && tp.IsValid(v)) {
            let dValue = null;
            if (tp.IsString(v) && !tp.IsBlank(v)) {
                let ParseResult = tp.TryParseDateTime(v);
                if (ParseResult.Result === true)
                    dValue = ParseResult.Value;
            }
            else if (tp.IsDate(v)) {
                dValue = v;
            }

            if (tp.IsValid(dValue)) {
                let S = dValue.toLocaleDateString(); // dValue.toISOString().substring(0, 10);
                this.Handle.value = S;
                this.OnValueChanged();
            }

        }
    }

    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.HtmlDateBox';
        this.fElementSubType = 'date';
        this.fDefaultCssClasses = [tp.Classes.HtmlDateBox];

        // data-bind
        this.fDataValueProperty = 'Date';
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
 
    OnHandleCreated() {
        super.OnHandleCreated();
 
    }
 
};
//#endregion


//#region  tp.Calendar
/**
A calendar control. <br />
Example markup
<pre>
    <table data-setup="{ Date: '2000-11-18'}"></table>
</pre>
*/
tp.Calendar = class extends tp.Control {

    /**
    Constructor <br />
    Example markup
    <pre>
        <table data-setup="{ Date: '2000-11-18'}"></table>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or sets the date. It always returns a {@link Date} value. Setting accepts either string or Date value.
    @type {Date}
    */
    get Date() {
        return this.fDate;
    }
    set Date(v) {
        if (tp.IsString(v)) {
            try {
                let o = tp.TryParseDateTime(v);
                if (o.Result === true) {
                    v = o.Value;
                }
            } catch (e) {
                v = new Date();
            }
        }

        if (v instanceof Date) {
            v = tp.ClearTime(v);
        } else {
            v = new Date();
            v = tp.ClearTime(v);
        }

        if (tp.DateCompare(this.fDate, v) !== 0) {
            this.fDate = new Date(v.getTime());
            this.fMonth = this.fDate.getMonth();
            this.fYear = this.fDate.getFullYear();
            this.Update();

            this.OnDateChanged();
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

        this.tpClass = 'tp.Calendar';
        this.fElementType = 'table';
        this.fDefaultCssClasses = [tp.Classes.Calendar];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Date';
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.elTable = this.Handle;
        this.HookEvent(tp.Events.Click);
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        //this.fDate = tp.ClearTime(new Date());

        this.fWeekRows = [];
        this.fDayCells = [];


        this.fYear = 0;
        this.fMonth = 0;
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        this.fWeekRows = [];
        this.fDayCells = [];

        var i, ln, j, jln, el;
        this.Attribute('border', 0);

        // table    : rows
        // row      : rowIndex cells
        // cell     : cellIndex colSpan rowSpan

        var S = tp.Trim(tp.Calendar.Markup);
        S = tp.Replace(S, 'Header', tp.Classes.CalendarHeaderRow);
        S = tp.Replace(S, 'Days', tp.Classes.CalendarDaysRow);
        S = tp.Replace(S, 'Week', tp.Classes.CalendarWeekRow);

        this.Html = S;
 
        // rows
        this.fDateRow = this.elTable.rows[0]; // HTMLTableRowElement
        this.fDaysRow = this.elTable.rows[1]; // HTMLTableRowElement

        for (i = 2, ln = 8; i < ln; i++) {
            this.fWeekRows.push(this.elTable.rows[i]);
        }

        // top cells
        this.fPrevYearCell = this.fDateRow.cells[0];
        this.fPrevMonthCell = this.fDateRow.cells[1];
        this.fCurDateCell = this.fDateRow.cells[2];
        this.fNextMonthCell = this.fDateRow.cells[3];
        this.fNextYearCell = this.fDateRow.cells[4];

        // arrows
        this.fPrevYearCell.innerHTML = '&#10094;';
        this.fNextYearCell.innerHTML = '&#10095;';

        this.fPrevMonthCell.innerHTML = '&#9668;';
        this.fNextMonthCell.innerHTML = '&#9658;';

        // day literals
        this.fDaysRow.cells[6].innerHTML = tp.DayNames[0].substring(0, 3);
        for (i = 1, ln = tp.DayNames.length; i < ln; i++) {
            S = tp.DayNames[i].substring(0, 3);
            this.fDaysRow.cells[i - 1].innerHTML = S;
        }


        // day cells
        for (i = 0, ln = this.fWeekRows.length; i < ln; i++) {
            for (j = 0, jln = this.fWeekRows[i].cells.length; j < jln; j++) {
                this.fDayCells.push(this.fWeekRows[i].cells[j]);
            }
        }



        this.Date = new Date();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Item = null;
        var Type = tp.Events.ToTripous(e.type);
        var ChangeType = null; // tp.CalenderClickChangeType

        if (Type === tp.Events.Click) {

            if (this.fCurDateCell === e.target) {
                var v = new Date();
                this.fMonth = v.getMonth();
                this.fYear = v.getFullYear();
                this.Update();
                ChangeType = tp.CalenderClickChangeType.Date;
            } else if (this.fPrevYearCell === e.target) {
                this.fYear--;
                this.Update();
                ChangeType = tp.CalenderClickChangeType.Year;
            } else if (this.fNextYearCell === e.target) {
                this.fYear++;
                this.Update();
                ChangeType = tp.CalenderClickChangeType.Year;
            } else if (this.fPrevMonthCell === e.target) {
                this.fMonth--;
                ChangeType = tp.CalenderClickChangeType.Month;
                if (this.fMonth < 0) {
                    this.fMonth = 11;
                    this.fYear--;
                    ChangeType = tp.CalenderClickChangeType.Year;
                }
                this.Update();
            } else if (this.fNextMonthCell === e.target) {
                this.fMonth++;
                ChangeType = tp.CalenderClickChangeType.Month;
                if (this.fMonth > 11) {
                    this.fMonth = 0;
                    this.fYear++;
                    ChangeType = tp.CalenderClickChangeType.Year;
                }
                this.Update();

            } else if (e.target instanceof HTMLTableCellElement && this.fDayCells.indexOf(e.target) !== -1) {
                let Info = tp.GetElementInfo(e.target);
                this.Date = new Date(Info.Date.getTime());
                ChangeType = tp.CalenderClickChangeType.Date;
            }


            if (ChangeType !== null)
                this.OnClickChange(ChangeType);

        }
    }

    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }

    /* methods */
    /**
    Returns the zero-based index of a cell in the table row. The returnded index indicates the Day of a specified {@link Date}
    @param {Date} DT The Date the get the day from
    @returns {number} Returns the zero-based index of a cell in the table row
    */
    GetDayCellIndex(DT)  {
        var D = DT.getDay();
        D = D === 0 ? 6 : D - 1;
        return D;
    }
    /** Updates the control */
    Update() {
        var i, ln, j, jln, el;

        var Today = tp.Today();
        var Month = this.fMonth;
        var Year = this.fYear;

        this.fCurDateCell.innerHTML = tp.Format('{0} {1}', tp.MonthNames[Month].substring(0, 3), Year);

        for (i = 0, ln = this.fDayCells.length; i < ln; i++) {
            this.fDayCells[i].className = '';
            this.fDayCells[i].innerHTML = '';
        }

        var FirstDate = new Date(Year, Month, 1);
        var LastDate = new Date(Year, Month, tp.DaysInMonth(Year, Month));

        var FirstDay = this.GetDayCellIndex(FirstDate);
        var LastDay = this.GetDayCellIndex(LastDate);

        var DT = new Date(FirstDate.getTime());
        tp.AddDays(DT, -FirstDay);


        //var LastRowVisible = (FirstDay + tp.DaysInMonth(Year, Month)) > 35;

        var DayOfMonth, DT2;
        for (i = 0, ln = this.fDayCells.length; i < ln; i++) {
            DayOfMonth = DT.getDate();
            this.fDayCells[i].innerHTML = DayOfMonth;

            DT2 = new Date(DT.getTime());
            tp.ClearTime(DT2);
            tp.SetElementInfo(this.fDayCells[i], { Date: DT2 });


            this.fDayCells[i].className = tp.Classes.CalendarDateCell;
            if (!tp.DateBetween(DT2, FirstDate, LastDate)) {
                this.fDayCells[i].className += ' ' + tp.Classes.Inactive;
            }

            if (tp.DateCompare(DT2, Today) === 0) {
                this.fDayCells[i].className += ' ' + tp.Classes.Marked;
            } else if (tp.DateCompare(DT2, this.Date) === 0) {
                this.fDayCells[i].className += ' ' + tp.Classes.Selected;
            }

            tp.AddDays(DT, 1);
        }

    }

    /* event triggers */
    /**
    Event trigger
    */
    OnDateChanged() {
        this.Trigger('DateChanged', {});
    }
    /**
    Event trigger. Notifies listeners about the type of date change that happened in a tp.Calender because of a mouse click.
    @param {tp.CalenderClickChangeType} ChangeType - Denotes the type of date change that happened in a tp.Calender because of a mouse click. One of the {@link tp.CalenderClickChangeType} constants.
    */
    OnClickChange(ChangeType) {
        let Args = new tp.CalendarClickChangeEventArgs(ChangeType);
        this.Trigger('ClickChange', Args);
    }

};



tp.Calendar.Markup = '                                                                      \
<tbody>                                                                                     \
<tr class="Header"><th></th><th></th><th colspan="3"></th><th></th><th></th></tr>             \
<tr class="Days"><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr>       \
<tr class="Week"><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      \
<tr class="Week"><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      \
<tr class="Week"><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      \
<tr class="Week"><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      \
<tr class="Week"><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      \
<tr class="Week"><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      \
</tbody>                                                                                    \
';



/** Field
 @protected
 @type {Date}
 */
tp.Calendar.prototype.fDate;
/** Field
 @protected
 @type {HTMLTableElement}
 */
tp.Calendar.prototype.elTable;
/** Field
 @protected
 @type {HTMLTableRowElement}
 */
tp.Calendar.prototype.fDateRow;
/** Field
 @protected
 @type {HTMLTableRowElement}
 */
tp.Calendar.prototype.fDaysRow;
/** Field
 @protected
 @type {HTMLTableRowElement[]}
 */
tp.Calendar.prototype.fWeekRows;
/** Field
 @protected
 @type {HTMLTableCellElement[]}
 */
tp.Calendar.prototype.fDayCells;
/** Field
 @protected
 @type {HTMLTableCellElement}
 */
tp.Calendar.prototype.fPrevYearCell;
/** Field
 @protected
 @type {HTMLTableCellElement}
 */
tp.Calendar.prototype.fPrevMonthCell;
/** Field
 @protected
 @type {HTMLTableCellElement}
 */
tp.Calendar.prototype.fCurDateCell;
/** Field
 @protected
 @type {HTMLTableCellElement}
 */
tp.Calendar.prototype.fNextMonthCell;
/** Field
 @protected
 @type {HTMLTableCellElement}
 */
tp.Calendar.prototype.fNextYearCell;
/** Field
 @protected
 @type {number}
 */
tp.Calendar.prototype.fYear;
/** Field
 @protected
 @type {number}
 */
tp.Calendar.prototype.fMonth;


//#endregion

//#region  tp.DateBox
/**
A date-box control. <br />
Example markup:
<pre>
    <div data-setup="{ Date: '2000-11-18'}"></div>
</pre>
Example of the produced markup.
<pre>
    <div class="tp-DateBox">
        <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
    </div>
</pre>
@implements {tp.IDropDownBoxListener}
*/
tp.DateBox = class extends tp.Control {

    /**
    Constructor <br />
    Example markup:
    <pre>
        <div data-setup="{ Date: '2000-11-18'}"></div>
    </pre>
    Example of the produced markup.
    <pre>
        <div class="tp-DateBox">
            <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Gets or sets the text of the control. 
    @type {string}
    */
    get Text() {
        return this.fTextBox instanceof HTMLInputElement ? this.fTextBox.value : '';
    }
    set Text(v) {
        if (v !== this.Text && this.fTextBox instanceof HTMLInputElement) {
            this.fTextBox.value = v;
            this.Date = v;
        }
    }
    /**
    Gets or sets the date. It always returns a Date value. Setting accepts either string or Date value.
    @type {Date}
    */
    get Date() {
        return this.fDate;
    }
    set Date(v) {
        if (tp.IsString(v)) {
            try {
                let o = tp.TryParseDateTime(v);
                v = o.Result === true ? o.Value : null;
            } catch (e) {
                v = null;
            }
        }

        if (v instanceof Date)
            v = tp.ClearTime(v);

        if ((this.fTextBox && tp.IsBlank(this.fTextBox.value)) || tp.DateCompare(this.fDate, v) !== 0) {
            if (v instanceof Date) {
                this.fDate = v;
                if (this.fTextBox)
                    this.fTextBox.value = tp.FormatDateTime(v, tp.GetDateFormat()); //  'yyyy-MM-dd'
                if (this.fCalendar) {
                    this.fCalendar.Date = v;
                    this.fCalendar.Update();
                }
            } else {
                this.fDate = null;
                if (this.fTextBox)
                    this.fTextBox.value = '';
            }

            this.WriteDataValue();
            this.OnDateChanged();
        }
    }
 
    /**
    Gets or sets a text the control displays as a hint to the user of what can be entered in the control. <br /> 
    The placeholder text must not contain carriage returns or line-feeds.
    @type {string}
    */
    get Placeholder() {
        return this.fTextBox instanceof HTMLInputElement ? this.fTextBox.placeholder : '';
    }
    set Placeholder(v) {
        if (this.fTextBox instanceof HTMLInputElement) {
            this.fTextBox.placeholder = v;
        }
    }
    /**
    Returns true while the dropdown box is visible
    @type {boolean}
    */
    get IsOpen() {
        return this.fDropDownBox ? this.fDropDownBox.IsOpen : false;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.DateBox';
        this.fElementType = 'div';
        this.fDefaultCssClasses = [tp.Classes.DateBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Date';
    }
    /**
    Notification. Called by CreateHandle() after handle creation and field initialization but BEFORE options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnFieldsInitialized() {
        super.OnFieldsInitialized();

        this.fDate = new Date();

        // create the inner controls
        // <div class="tp-Strip"><input type="text" class="tp-Text" /><div class="tp-Btn">&#9662;</div></div>
        let ControlContainer = tp.AppendElement(this.Handle, 'div');
        ControlContainer.className = tp.Classes.Strip;

        this.fTextBox = tp.AppendElement(ControlContainer, 'input');
        this.fTextBox.type = 'text';
        this.fTextBox.className = tp.Classes.Text;
        this.fTextBox.spellcheck = false;

        this.fButton = tp.AppendElement(ControlContainer, 'div');
        this.fButton.className = tp.Classes.Btn;
        this.fButton.innerHTML = '&#9662;';

        // drop-down box (is the viewport for the scroller)
        this.fDropDownBox = new tp.DropDownBox(null, {
            Associate: ControlContainer,
            Owner: this,
            Parent: this.Handle.parentElement
        });

        //this.fDropDownBox.StyleProp('border', 'none');
        this.fDropDownBox.Dragger.Active = false;

        this.fCalendar = new tp.Calendar();
        this.fCalendarSize = tp.SizeOf(this.fCalendar.Handle);
        this.fDropDownBox.Handle.appendChild(this.fCalendar.Handle);

        // events
        this.HookEvent(tp.Events.Change);
        this.HookEvent(tp.Events.KeyDown);
        this.HookEvent(tp.Events.KeyUp);
        this.HookEvent(tp.Events.KeyPress);

        tp.On(this.Document, tp.Events.Click, this, false);

        this.fCalendar.On('ClickChange', this.Calendar_OnClickChange, this);
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
    /**
    Event trigger. Called right after the read-only property is changed
    @protected
    @override
    */
    OnReadOnlyChanged() {
        if (this.fTextBox instanceof HTMLInputElement)
            tp.ReadOnly(this.fTextBox, this.ReadOnly);
        super.OnReadOnlyChanged();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var S;
        var Type = tp.Events.ToTripous(e.type);

        switch (Type) {
            case tp.Events.LostFocus:
            case tp.Events.Change:
                tp.CancelEvent(e);

                S = tp.Trim(this.Text); 
                let CR = tp.TryParseDateTime(S);
                if (CR.Result === true) {
                    this.Date = CR.Value;
                }
                //else if (!tp.IsEmpty(this.Date)) {
                //    this.Text = tp.FormatDateTime(this.Date);
                //}
                else {
                    this.Date = null;
                    this.Text = '';
                }

                break;

            case tp.Events.Click:
                if (e.currentTarget === this.Document) {

                    if (tp.ContainsEventTarget(this.fCalendar.Handle, e.target) && (e.target instanceof Node)) {
                        // Calendar click, let the  Calendar_OnClickChange() handle it.
                    } else if (e.target === this.fTextBox) {
                        this.fDropDownBox.Close();
                    } else if (e.target === this.fButton) {
                        this.fDropDownBox.Toggle();
                        if (!this.fDropDownBox.IsOpen) {
                            this.fTextBox.focus();
                        }
                    } else {
                        this.fDropDownBox.Close();
                    }
                }


                break;

            case tp.Events.KeyDown:
                break;


            case tp.Events.KeyPress:
                break;

        }
    }
    /**
    Notification from Calender because of a click.
    @param {tp.CalendarClickChangeEventArgs} Args The {@link tp.CalendarClickChangeEventArgs} arguments
    */
    Calendar_OnClickChange(Args) {
        if (tp.Bf.In(tp.CalenderClickChangeType.Date, Args.ChangeType)) {
            this.Date = this.fCalendar.Date;

            this.fDropDownBox.Close();
            this.fTextBox.focus();
        }
    }

    /**
    Called by the dropdown box to inform its owner about a stage change.
    @param {tp.DropDownBox} Sender The sender
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants
    */
    OnDropDownBoxEvent(Sender, Stage) {

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
                break;

            case tp.DropDownBoxStage.Opened: 
                this.fCalendar.Date = this.Date;
                this.fDropDownBox.Height = this.fCalendarSize.Height;
                this.fDropDownBox.Width = this.fCalendarSize.Width;
                break;

            case tp.DropDownBoxStage.Closing:
                break;

            case tp.DropDownBoxStage.Closed:
                break;
        }



    }

    /* public */
    /**
    Displays the dropdown box
    */
    Open() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Open();
    }
    /**
    Hides the dropdown box
    */
    Close() {
        if (this.ReadOnly !== true && this.Enabled === true)
            this.fDropDownBox.Close();
    }
    /**
    Displays or hides the dropdown box
    */
    Toggle() {
        if (this.ReadOnly !== true && this.Enabled === true) {
            if (!this.IsOpen)
                this.Open();
            else
                this.Close();
        }

    }

    /* validation */
    /**
    Event trigger
    */
    OnRequiredChanged() {
        this.SetRequiredMark(this.fTextBox);
        super.OnRequiredChanged();
    }
    /**
    Returns true if a form will validate when it is submitted, without having to submit it.
    @returns {boolean} Returns true if a form will validate when it is submitted, without having to submit it.
    */
    CheckValidity() {
        return tp.IsValidatableElement(this.fTextBox) ? this.fTextBox.checkValidity() : true;
    }
    /**
    Sets a custom error message that is displayed when a form is submitted.
    @param {string} MessageText - Sets a custom error message that is displayed when a form is submitted.
    */
    SetValidationMessage(MessageText) {
        if (tp.IsValidatableElement(this.fTextBox))
            this.fTextBox.setCustomValidity(MessageText);
    }

    /* event triggers */
    /**
    Event trigger
    */
    OnDateChanged() {
        this.Trigger('DateChanged', {});
    }

};

/** Field
 * @protected
 * @type {HTMLInputElement}
 */
tp.DateBox.prototype.fTextBox;
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.DateBox.prototype.fButton;
/** Field
 * @protected
 * @type {tp.DropDownBox}
 */
tp.DateBox.prototype.fDropDownBox;
/** Field
 * @protected
 * @type {tp.Calendar}
 */
tp.DateBox.prototype.fCalendar;
/** Field
 * @protected
 * @type {Date}
 */
tp.DateBox.prototype.fDate;
/** Field
 * @protected
 * @type {tp.Size}
 */
tp.DateBox.prototype.fCalendarSize;
//#endregion

//#region  tp.ImageBox
/**
An image control. <br />
The new css property object-fit, of the img tag, is NOT supported by Internet Explorer. This control tries to mimic that object-fit property behavior using a div and the background-size css property.
Example markup
<pre>
    <div id="ImageBox" data-setup='{ Url: "/Images/Garden.jpg", Height: 200, Width: 300 }'></div>
</pre>
*/
tp.ImageBox = class extends tp.Control {

    /**
    Constructor <br />
    Example markup
    <pre>
        <div id="Image" data-setup='{ Url: "/Content/Images/Garden.jpg", Height: 200, Width: 300 }'></div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /**
    Gets or sets the url of the image. It could be either the path to the image or url-data, e.g. data:image/gif;base64,.... 
    @type {string}
    */
    get Url() {
        return this.StyleProp('background-image');
    }
    set Url(v) {
        if (!tp.IsBlank(v) && !tp.StartsWith(v, 'url(')) {
            v = 'url(' + v + ')';
        }

        this.StyleProp('background-image', v);
    }
    /**
    Gets or sets the size mode. It always returns one of the {@link tp.ImageSizeMode} constants. Accepts either a {@link tp.ImageSizeMode} or the corresponding enum name string.
    @type {tp.ImageSizeMode}
    */
    get SizeMode() {
        var S = this.StyleProp('background-size');
        if (S === 'cover')
            return tp.ImageSizeMode.Crop;
        if (S === 'contain')
            return tp.ImageSizeMode.Scale;
        if (S === '100% 100%')
            return tp.ImageSizeMode.Stretch;
        return tp.ImageSizeMode.Unknown;
    }
    set SizeMode(v) {
        if (tp.IsString(v)) {
            if (tp.IsSameText(v, 'Crop'))
                v = tp.ImageSizeMode.Crop;
            else if (tp.IsSameText(v, 'Scale'))
                v = tp.ImageSizeMode.Scale;
            else if (tp.IsSameText(v, 'Stretch'))
                v = tp.ImageSizeMode.Stretch;
            else
                v = tp.ImageSizeMode.Stretch;
        }

        if (v === tp.ImageSizeMode.Crop)
            this.StyleProp('background-size', 'cover');
        else if (v === tp.ImageSizeMode.Scale)
            this.StyleProp('background-size', 'contain');
        else if (v === tp.ImageSizeMode.Stretch)
            this.StyleProp('background-size', '100% 100%');
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ImageBox';
        this.fElementType = 'div';
        this.fDefaultCssClasses = [tp.Classes.ImageBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Url';
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
};
//#endregion

//#region  tp.RadioGroup
/**
A radio-group control <br />
@implements {tp.ISelectedIndex}
*/
tp.RadioGroup = class extends tp.Control {

    /**
     Constructor <br />
     Example markup
     @example
     <pre>
        <fieldset id="RadioGroup" data-setup="{Name: 'group1', SelectedIndex: 0, Width: 200, Height: 300}">
            <legend>Radio List Title</legend>
            <label><input type="radio" name="group1" value="Male" />Male    </label>
            <label><input type="radio" name="group1" value="Female" />Female  </label>
            <label><input type="radio" name="group1" value="NotSet" />Not Set </label>
        </fieldset>
     </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }




    /* property */
    /**
    Returns the number of radio buttons in this group
    @type {number}
    */
    get Count() {
        return this.GetRadioList().length;
    }
    /**
    Gets or sets the title of the group box
    @type {string}
    */
    get Text() {
        return this.fLegend instanceof HTMLLegendElement ? this.fLegend.innerHTML : '';
    }
    set Text(v) {
        if (tp.IsString(v) && this.fLegend instanceof HTMLLegendElement)
            this.fLegend.innerHTML = v;
    }
    /**
    Gets or sets the group name. This is the name for all radio-buttons in this group
    @type {string}
    */
    get Name() {
        let List = this.GetRadioList();
        return List.length > 0 ? List[0].name : '';
    }
    set Name(v) {
        let List = this.GetRadioList();
        for (let i = 0, ln = List.length; i < ln; i++)
            List[i].name = v;
    }
    /**
    Gets or sets the selected index
    @type {number}
    */
    get SelectedIndex() {
        var el = this.FindCheckedItem();
        var List = this.GetRadioList();
        return List.indexOf(el);
    }
    set SelectedIndex(v) {
        this.CheckItemByIndex(v);
    }
    /**
    Gets or sets the selected radio-button by value
    @type {any}
    */
    get SelectedValue() {
        return this.FindCheckedValue();
    }
    set SelectedValue(v) {
        this.CheckItemByValue(v);
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.RadioGroup';
        this.fElementType = 'fieldset';
        this.fDefaultCssClasses = [tp.Classes.RadioGroup];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'SelectedValue';
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        // ensure legend element exists
        let el = tp.Select(this.Handle, 'legend');
        if (!tp.IsEmpty(el)) {
            this.fLegend = el; // HTMLLegendElement;
        } else {
            this.fLegend = this.Document.createElement('legend');
            tp.InsertNode(this.Handle, 0, this.fLegend);
        }

        // create the radio-button container div
        let List = tp.ChildHTMLElements(this.Handle);
        if (List.length > 1 && !(List[1] instanceof HTMLLabelElement || List[1] === this.fLegend)) {
            this.fContainer = List[1];
        } else {
            this.fContainer = this.Document.createElement('div');
        }


        for (let i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof HTMLLabelElement && List[i] !== this.fLegend && List[i] !== this.fContainer) {
                if (List[i].parentNode !== this.fContainer) {
                    List[i].parentNode.removeChild(List[i]);
                }
            }
        }

        tp.AddClass(this.fContainer, tp.Classes.List);
        let o = {
            position: 'absolute',
            overflow: 'auto',
            left: '6px',            
            top: '10px',   
            right: '6px',
            bottom: '6px'
        };
        tp.SetStyle(this.fContainer, o);

        if (tp.IsEmpty(this.fContainer.parentNode))
            this.Handle.appendChild(this.fContainer);

        for (let i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof HTMLLabelElement && List[i] !== this.fLegend && List[i] !== this.fContainer) {
                if (List[i].parentNode !== this.fContainer) {
                    this.fContainer.appendChild(List[i]);
                }
            }
        }

        this.HookEvent(tp.Events.Change);

    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var el, Type = tp.Events.ToTripous(e.type);

        if (tp.Events.Change === Type) {
            el = e.target;
            if (tp.ElementIs(el, 'input') && tp.IsSameText(el.type, 'radio')) {
                this.OnSelectedIndexChanged();
            }
        }

    }

    /* public */
    /**
    Returns the list of {@link HTMLInputElement} child radio-buttons
    @returns {HTMLInputElement[]} Returns the list of {@link HTMLInputElement} child radio-buttons
    */
    GetRadioList() {
        var Result = [];
        if (this.Handle) {
            var List = tp.SelectAll(this.Handle, 'input[type="radio"]');

            for (var i = 0, ln = List.length; i < ln; i++) {
                Result.push(List[i]);
            }
        }

        return Result;
    }
    /**
    Finds and returns the checked {@link HTMLInputElement} radio button, if any, else null
    @returns {HTMLInputElement} Finds and returns the checked {@link HTMLInputElement} radio button, if any, else null
    */
    FindCheckedItem() {
        var el, i, ln, List = this.GetRadioList();
        for (i = 0, ln = List.length; i < ln; i++) {
            el = List[i];
            if (el.checked === true) {
                return el;
            }
        }
        return null;
    }
    /**
    Returns the value of the checked radio-button, if any, else null
    @returns {string} Returns the value of the checked {@link HTMLInputElement} radio-button, if any, else null
    */
    FindCheckedValue() {
        var el = this.FindCheckedItem();
        if (el) {
            return el.value;
        }

        return null;
    }
    /**
    Finds and returns a radio-button by value, if any, else null
    @param {any} v The value
    @return {HTMLInputElement} Finds and returns a {@link HTMLInputElement} radio-button by value, if any, else null
    */
    FindItemByValue(v) {
        if (!tp.IsEmpty(v)) {
            v = v.toString();

            var el, i, ln, List = this.GetRadioList();
            for (i = 0, ln = List.length; i < ln; i++) {
                el = List[i];
                if (el.value === v) {
                    return el;
                }
            }

        }

        return null;
    }
    /**
    Finds and returns a {@link HTMLInputElement} radio-button by index, if any, else null
    @param {number} Index The index
    @returns {HTMLInputElement} Finds and returns a {@link HTMLInputElement}  radio-button by index, if any, else null
    */
    FindItemByIndex(Index) {
        var el, i, ln, List = this.GetRadioList();
        for (i = 0, ln = List.length; i < ln; i++) {
            el = List[i];
            if (i === Index) {
                return el;
            }
        }
        return null;
    }
    /**
    Checks a radio button by value
    @param {any} v The value
    */
    CheckItemByValue(v) {
        if (!tp.IsEmpty(v)) {
            v = v.toString();

            var el = this.FindItemByValue(v);
            if (el && !el.checked) {
                el.checked = true;
                this.OnSelectedIndexChanged();
            }
        }

    }
    /**
    Checks a radio button by index
    @param {number} Index The index
    */
    CheckItemByIndex(Index) {
        var el = this.FindItemByIndex(Index);
        if (el && !el.checked) {
            el.checked = true;
            this.OnSelectedIndexChanged();
        }
    }

    /**
    Removes all radio-buttons from this group
    */
    Clear() {
        this.fLastName = !tp.IsBlank(this.Name) ? this.Name : this.fLastName;
        var List = tp.SelectAll(this.Handle, 'label');

        for (var i = 0, ln = List.length; i < ln; i++) {
            List[i].parentNode.removeChild(List[i]);
        }
    }
    /**
    Adds a radio-button to this group and returns the newly added {@link HTMLInputElement} element
    @param {any} Value - The value of the radio-button
    @param {string} Text - The text title of the radio-button
    @returns {HTMLInputElement} Returns the newly added {@link HTMLInputElement} element
    */
    AddItem(Value, Text) {
        return this.InsertItem(this.Count, Value, Text);
    }
    /**
    Inserts a radio-button to this group, at a specified index, and returns the newly added {@link HTMLInputElement} element
    @param {number} Index The index
    @param {any} Value - The value of the radio-button
    @param {string} Text - The text title of the radio-button
    @returns {HTMLInputElement} Returns the newly added {@link HTMLInputElement} element
    */
    InsertItem(Index, Value, Text) {
        if (this.Handle) {

            let sName = !tp.IsBlank(this.Name) ? this.Name : (!tp.IsBlank(this.fLastName) ? this.fLastName : 'group');

            // prepare the new child
            let Label = this.Document.createElement('label');
            let S = tp.Format('<input type="radio" name="{0}" value="{1}" />{2}', sName, Value, Text);
            Label.innerHTML = S;

            var List = tp.SelectAll(this.Handle, 'label');
            if (List.length === 0 || Index < 0 || Index >= List.length) {
                Index = List.length === 0 ? 0 : List.length;

                this.fContainer.appendChild(Label);
            } else {
                this.fContainer.insertBefore(Label, List[Index]);
            }

            let rb = tp.Select(Label, 'input[type="radio"]');
            return rb;
        }

        return null;
    }

    /**
    Returns the title text of a radio-button found at a specified index.
    @param {number} Index The index
    @return {string} Returns the title text of a radio-button found at a specified index.
    */
    GetTitleAt(Index) {
        var List = tp.SelectAll(this.Handle, 'label');
        let Node = tp.FindTextNode(List[Index]);
        return Node.nodeValue;
    }
    /**
    Sets the title text of a radio-button found at a specified index.
    @param {number} Index The index
    @param {string} Text The text
    */
    SetTitleAt(Index, Text) {
        var List = tp.SelectAll(this.Handle, 'label');
        let Node = tp.FindTextNode(List[Index]);
        Node.nodeValue = Text;
    }

    /**
    Clears all radio-buttons and loads from a specified enum type
    @param {Object} EnumType The enumeration type
    */
    LoadFromEnumType(EnumType) {
        let List = [];
        let v;
        let n;
        for (let Prop in EnumType) {
           
            n = null;
            if (!tp.IsFunction(EnumType[Prop])) {
                v = EnumType[Prop];
                if (tp.IsNumber(v)) {
                    n = Number(v);
                } else if (tp.IsString(v)) {
                    if (tp.TryStrToInt(v).Result === true) {
                        n = tp.StrToInt(v);
                    }
                }

                if (tp.IsNumber(n)) {
                    let o = {
                        Id: n,
                        Name: Prop
                    };

                    List.push(o);
                }
            }


        }

        if (List.length > 0) {
            this.LoadFrom(List);
        }
    }
    /**
    Clears all radio-buttons and loads from a specified object array. Each object in the array must provide an Id and a Name property.
    @param {object[]} List A list of object of type <code>{ Id: any, Name: string }</code>
    */
    LoadFrom(List) {
        this.Clear();
        for (let i = 0, ln = List.length; i < ln; i++) {
            this.AddItem(List[i].Id, List[i].Name);
        }
 
        if (List.length > 0)
            this.SelectedIndex = 0;
    }

    /* notifications */
    /**
    Occurs when the selected radio button is changed
    */
    OnSelectedIndexChanged() {
        this.Trigger('SelectedIndexChanged', {});
    }
};

/** Field
* @protected
* @type {HTMLLegendElement}
*/
tp.RadioGroup.prototype.fLegend;
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.RadioGroup.prototype.fContainer;
/** Field
 * @protected
 * @type {string}
 */
tp.RadioGroup.prototype.fLastName;

//#endregion

//#region tp.ValueSlider
/**
A value slider control, is an input[type='range'] html control. <br />
It can be used either as a position selector between a minimum and maximum number (range)
or as a value selector where values come from a specified array of values and min/max are the first and the last array index.
*/
tp.ValueSlider = class extends tp.InputControl {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <input type='range' min='1' max='100' step='1', value='50' />
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }



    /* properties */
    /**
    Gets or sets the minimum value of the control. <br />
    When the control uses a value list, this property property is read-only and it returns the first value of the list.
    @type {any}
    */
    get Min() {
        return this.UsesValueList ? 0 : (this.Handle instanceof HTMLInputElement ? this.Handle.min : 0);
    }
    set Min(v) {
        if (!this.UsesValueList && this.Handle instanceof HTMLInputElement)
            this.Handle.min = v;
    }
    /**
    Gets or sets the maximum value of the control. <br />
    When the control uses a value list, this property property is read-only and it returns the last value of the list.
    @type {any}
    */
    get Max() {
        return this.UsesValueList ? this.ValueList.length - 1 : (this.Handle instanceof HTMLInputElement ? this.Handle.max : 0);
    }
    set Max(v) {
        if (!this.UsesValueList && this.Handle instanceof HTMLInputElement)
            this.Handle.max = v;
    }
    /**
    Gets or sets the step of a position change. 
    @type {number}
    */
    get Step() {
        return this.Handle instanceof HTMLInputElement ? Number(this.Handle.step) : 1;
    }
    set Step(v) {
        if (this.Handle instanceof HTMLInputElement)
            this.Handle.step = v.toString();
    }
    /**
    Gets or sets the value (position) of the control. It's a number when no ValueList is used.
    @type {any}
    */
    get Value() {
        if (this.Handle instanceof HTMLInputElement) {
            return this.UsesValueList ? this.GetValue() : this.Handle.value;
        }
        return null;
    }
    set Value(v) {
        if (this.Handle instanceof HTMLInputElement) {

            if (!this.UsesValueList) {
                let min = Number(this.Handle.min);
                let max = Number(this.Handle.max);
                let n = Number(v);
                if (min <= n && n <= max) {
                    this.Handle.value = n.toString();
                    this.OnValueChanged();
                }
            } else {
                this.SetValue(v);
            }
        }
    }
    /**
    Gets or sets the value list.
    When a value list is used then a position change it actually changes the selected index in that list.
    @type {any[]}
    */
    get ValueList() {
        return !tp.IsEmpty(this.fValueList) ? this.fValueList : [];
    }
    set ValueList(v) {
        if (this.Handle instanceof HTMLInputElement) {
            if (tp.IsArray(v)) {
                this.Handle.min = '0';
                this.Handle.max = (v.length - 1).toString();
                this.fValueList = v;
            } else {
                this.Handle.min = '0';
                this.Handle.max = '100';
                this.fValueList = null;
            }

            this.OnValueChanged();
        }
    }
    /**
    Returns true when a value list is used
    @type {boolean}
    */
    get UsesValueList() {
        return tp.IsArray(this.fValueList) && (this.fValueList.length > 0);
    }
    /**
    A user defined value getter, to be used when a value list is provided. <br />
    A function as <code>any GetValueFunc(Slider: tp.ValueSlider)</code>
    @type {function}
    */
    GetValueFunc;
    /**
    A user defined value setter, to be used when a value list is provided. <br />
    A function as <code>void SetValueFunc(Slider: tp.ValueSlider, v: any)</code>
    @type {function}
    */
    SetValueFunc;

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ValueSlider';
        this.fElementSubType = 'range';
        this.fDefaultCssClasses = [tp.Classes.ValueSlider];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Value';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.min = !tp.IsBlank(this.Handle.min) ? this.Handle.min : '0';
            this.Handle.max = !tp.IsBlank(this.Handle.max) ? this.Handle.max : '100';
            this.Handle.value = !tp.IsBlank(this.Handle.value) ? this.Handle.value : '0';
            this.Handle.step = !tp.IsBlank(this.Handle.step) ? this.Handle.step : '1';
        }
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned.
    @protected
    @override
    */
    Bind() {
        super.Bind();
        this.ReadDataValue();
    }

/* protected */

    /** Returns the value
     * @protected
     * @return {any} Returns the value
     * */
    GetValue() {
        if (tp.IsFunction(this.GetValueFunc))
            return tp.Call(this.GetValueFunc, null, this);

        if (this.Handle instanceof HTMLInputElement)
            return this.ValueList[Number(this.Handle.value)];

        return null;
    }
    /**
     * Sets the value
     * @protected
     * @param {any} v The value
     */
    SetValue(v) {
        if (tp.IsFunction(this.SetValueFunc)) {
            tp.Call(this.SetValueFunc, null, this, v);
            this.OnValueChanged();
        } else if (this.Handle instanceof HTMLInputElement) {
            let Index = this.ValueList.indexOf(v);
            if (Index !== -1) {
                this.Handle.value = Index.toString();
                this.OnValueChanged();
            }
        }

    }

    /* public */
    /**
    Increments the position of the control by a value, if specified, else by the Step value.
    @param  {number} [n] The value to increment the position
    */
    StepUp(n) {
        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.stepUp(n);
            this.OnValueChanged();
        }
    }
    /**
    Decrements the position of the control by a value, if specified, else by the Step value.
    @param  {number} [n] The value to decrement the position
    */
    StepDown(n) {
        if (this.Handle instanceof HTMLInputElement) {
            this.Handle.stepDown(n);
            this.OnValueChanged();
        }

    }

};

/** Field
 * @protected
 * @type {any[]}
 */
tp.ValueSlider.prototype.fValueList;

//#endregion

//#region tp.ProgressBar
/**
A progress-bar control
*/
tp.ProgressBar = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup
    @example
    <pre>
        <progress data-setup="{ Width: 200, Max: 10, Value: 0 }"></progress>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Get or sets the maximum value (position actually)
    @type {number}
    */
    get Max() {
        return this.Handle instanceof HTMLProgressElement ? this.Handle.max : 0;
    }
    set Max(v) {
        if (this.Handle instanceof HTMLProgressElement)
            this.Handle.max = v;
    }
    /**
    Gets or sets the current value (position actually)
    @type {number}
    */
    get Value() {
        return this.Handle instanceof HTMLProgressElement ? this.Handle.value : 0;
    }
    set Value(v) {
        if (this.Handle instanceof HTMLProgressElement)
            this.Handle.value = v;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.ProgressBar';
        this.fElementType = 'progress';
        this.fDefaultCssClasses = [tp.Classes.ProgressBar];
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();

        if (this.Handle instanceof HTMLProgressElement) {
            this.Handle.max = !tp.IsEmpty(this.Handle.max) ? this.Handle.max : 100;
            this.Handle.value = !tp.IsEmpty(this.Handle.value) ? this.Handle.value : 0;
        }
    }
};
//#endregion

//#region  tp.TreeNode
/**
Represents a tree-view node

*/
tp.TreeNode = class extends tp.tpObject {

    /**
    Constructor  <br />       
    @example
    <pre>
        <!-- Example markup -->
        <div>Node
            <div>Leaf</div>
        </div>
    </pre>    
    @example
    <pre>
        <!-- Example of produced markup -->
        <div class="tp-Node">
            <div class="tp-Strip">
                <div></div>                 // +/-
                <div></div>                 // image
                <a>Node</a>                 // text
            </div>
            <div>                           // items element
                <div class="tp-Leaf">
                    <div class="tp-Strip">
                        <div></div>         // +/-
                        <div></div>         // image
                        <a>Leaf</a>         // text
                    </div>
                </div>
            </div>
        </div>
    </pre>
    @param {HTMLElement} Handle The {@link HTMLElement} element representing this tree-node
    */
    constructor(Handle) {
        super();
        if (!tp.IsHTMLElement(Handle))
            tp.Throw('Can not create a TreeNode without handle (HTMLElement)');
        this.fHandle = Handle;
        this.NormalizeHandle();
    }
 
    /* properties */
    /**
    Returns the tree-view this node belongs to.
    @type {tp.TreeView}
    */
    get TreeView() {
        let Current = this;
        let CurrentParent = this.fParentTreeNode;

        while (!tp.IsEmpty(CurrentParent)) {
            Current = CurrentParent;
            CurrentParent = Current.fParent;
        }

        return Current instanceof tp.TreeView ? Current : null;
    }

    /**
    Gets or sets the display text of the node. 
    @type {string}
    */
    get Text() {
        return tp.IsHTMLElement(this.fTextElement) ? this.fTextElement.innerHTML : '';
    }
    set Text(v) {
        if (tp.IsHTMLElement(this.fTextElement))
            this.fTextElement.innerHTML = v;
    }
    /**
    Gets or sets the title attribute
    @type {string}
    */
    get ToolTip() {
        return this.Handle ? this.Handle.title : '';
    }
    set ToolTip(v) {
        if (tp.IsString(v) && this.Handle)
            this.Handle.title = v;
    }
    /**
    The text element of a node/leaf is an anchor element.
    This property gets or sets the value of the href property of that anchor element.
    @type {string}
    */
    get Url() {
        if (this.fTextElement instanceof HTMLAnchorElement) {
            return this.fTextElement.href !== 'javascript:void(0);' ? this.fTextElement.href : '';
        }

        return '';
    }
    set Url(v) {
        if (this.fTextElement instanceof HTMLAnchorElement) {
            if (tp.IsEmpty(v) || tp.IsBlank(v)) {
                this.fTextElement.href = 'javascript:void(0);';
            } else {
                this.fTextElement.href = v;
            }
        }
    }
    /**
    Gets or sets the ico css classes, e.g. fa fa-xxxxx, for the ico
    @type {string}
    */
    get IcoClasses() {
        return this.fIcoClasses;
    }
    set IcoClasses(v) {
        if (tp.IsHTMLElement(this.fImageElement)) {
            tp.RemoveClasses(this.fImageElement, this.fIcoClasses);
            this.fImageElement.style.background = '';
            this.fImageUrl = '';
            tp.AddClasses(this.fImageElement, v);
        }

        this.fIcoClasses = v;
        this.IcoChanged();
    }
    /**
    Gets or sets a url for the item ico
    @type {string}
    */
    get ImageUrl() {
        return this.fImageUrl;
    }
    set ImageUrl(v) {
        if (tp.IsHTMLElement(this.fImageElement)) {
            tp.RemoveClasses(this.fImageElement, this.fIcoClasses);
            this.fImageElement.style.background = '';
            this.fIcoClasses = '';

            tp.SetStyle(this.fImageElement, {
                'background-image': tp.Format('url("{0}")', v),
                'background-repeat': 'no-repeat',
                'background-position': 'center center',
                'background-size': '75%'
            });
        }
        this.fImageUrl = v;
        this.IcoChanged();
    }

    /**
    Gets or sets the parent node
    @type {tp.TreeNode}
    */
    get ParentTreeNode() {
        return this.fParentTreeNode;
    }
    set ParentTreeNode(v) {
        this.fParentTreeNode = v;
    }
    /**
    Returns the handle of this node
    @type {HTMLElement}
    */
    get Handle() {
        return this.fHandle;
    }
    /**
    Returns the text element which is a HTMLAnchorElement
    @type {HTMLAnchorElement}
    */
    get TextElement() {
        return this.fTextElement;
    }
    /**
    Returns the number of child nodes
    @type {number}
    */
    get Count() {
        return !tp.IsEmpty(this.fItems) ? this.fItems.length : 0;
    }
    /**
    Returns a boolean value indicating whether this instance has child nodes
    @type {boolean}
    */
    get HasChildren() {
        return this.Count > 0;
    }

    /**
    Returns true if this is the root node, that is the tree-view itself
    @type {boolean}
    */
    get IsRoot() {
        return tp.HasClass(this.Handle, tp.Classes.TreeView);
    }
    /**
    Returns true if this node has child nodes (it is an actual node)
    @type {boolean}
    */
    get IsNode() {
        return !this.IsRoot && this.Count > 0;
    }
    /**
    Returns true if this node has no child nodes (it is a leaf)
    @type {boolean}
    */
    get IsLeaf() {
        return !this.IsRoot && this.Count === 0;
    }

    /**
    Returns true if the node is expanded.
    @type {boolean}
    */
    get IsExpanded() {
        return tp.IsHTMLElement(this.Handle) && tp.HasClass(this.Handle, tp.Classes.Expanded);
    }

    /**
    Returns the level of this Node. A root node has level 0, its children have level 1 and so on.
    @type {number}
    */
    get Level() {
        return tp.IsEmpty(this.fParentTreeNode) ? 0 : this.fParentTreeNode.Level + 1;
    }
    /**
    Returns the index of this Node in the list of its parent, if any, else -1
    @type {number}
    */
    get Index() {
        return tp.IsEmpty(this.fParentTreeNode) ? -1 : this.fParentTreeNode.IndexOf(this);
    }



    /* protected */
    /**
    Initializes this node
    @protected
    */
    NormalizeHandle() {
        this.fIcoClasses = '';
        this.fImageUrl = '';

        let T = tp.FindTextNode(this.Handle);
        let S = T ? T.nodeValue || '' : '';
        if (T) {
            T.nodeValue = '';
        }
        S = tp.Trim(S);

        tp.SetObject(this.Handle, this);

        let List = tp.ChildHTMLElements(this.Handle);



        for (let i = 0, ln = List.length; i < ln; i++) {
            List[i].parentNode.removeChild(List[i]);
        }

        this.CreateElements(S);

        let Node; // tp.TreeNode;
        for (let i = 0, ln = List.length; i < ln; i++) {
            Node = new tp.TreeNode(List[i]);
            this.Add(Node);
        }

        tp.AddClass(this.Handle, this.Count > 0 ? tp.Classes.Node : tp.Classes.Leaf);

        // start collapsed
        if (this.IsNode)
            this.fPlusMinusElement.innerHTML = tp.TreeNode.CollapseSymbol; // '+';

        // process create params
        let CreateParams = tp.Data(this.Handle, 'setup');
        if (!tp.IsBlank(CreateParams)) {
            CreateParams = eval("(" + CreateParams + ")");                            // compile it

            this.Handle.removeAttribute('data-setup');

            let A = ['IcoClasses', 'ImageUrl', 'Url', 'ToolTip'];
            for (let i = 0, ln = A.length; i < ln; i++) {
                if (A[i] in CreateParams) {
                    this[A[i]] = CreateParams[A[i]];
                }
            }
        }

        this.IcoChanged();

        if (tp.IsBlank(this.ToolTip))
            this.ToolTip = S;

    }
    /** Creates the elements of this instance
     * @protected
     * @param {string} Text The display text
     */
    CreateElements(Text) {
        this.fStripElement = this.Handle.ownerDocument.createElement('div');
        this.Handle.appendChild(this.fStripElement);
        this.fStripElement.className = tp.Classes.Strip;

        // +/-
        this.fPlusMinusElement = this.Handle.ownerDocument.createElement('div');
        this.fStripElement.appendChild(this.fPlusMinusElement);

        // image
        this.fImageElement = this.Handle.ownerDocument.createElement('div');
        this.fStripElement.appendChild(this.fImageElement);

        // text
        this.fTextElement = this.Handle.ownerDocument.createElement('a');
        this.fTextElement.href = 'javascript:void(0);';
        this.fStripElement.appendChild(this.fTextElement);
        if (!tp.IsBlank(Text))
            this.fTextElement.innerHTML = Text;


    }
    /** Called when the ico changes 
     @protected
     */
    IcoChanged() {
        if (tp.IsHTMLElement(this.fImageElement)) {
            this.fImageElement.style.display = tp.IsBlank(this.IcoClasses) && tp.IsBlank(this.ImageUrl) ? 'none' : '';
        }
    }


    /* public */
    /**
    Removes all child nodes from this node
    */
    Clear() {
        tp.RemoveClass(this.Handle, tp.Classes.Expanded);
        tp.RemoveClass(this.Handle, tp.Classes.Node);
        tp.AddClass(this.Handle, tp.Classes.Leaf);
        this.fPlusMinusElement.innerHTML = '';

        if (this.fItemsElement) {
            tp.RemoveChildren(this.fItemsElement);
        }

        if (this.Count > 0) {
            for (let i = 0, ln = this.Count; i < ln; i++) {
                this.fItems[i].fParentTreeNode = null;
            }

            this.fItems.length = 0;
        }

        super.Clear();

    }
    /**
    Returns true if this node is the parent node for a specified child node.
    @param {tp.TreeNode} Node The node
    @returns {boolean} Returns true if this node is the parent node for a specified child node.
    */
    Contains(Node) {
        return this.IndexOf(Node) >= 0;
    }
    /**
    Returns the index of a specified child node, if any, else -1.
    @param {tp.TreeNode} Node The node
    @returns {number} Returns the index of a specified child node, if any, else -1.
    */
    IndexOf(Node) {
        return !tp.IsEmpty(this.fItems) ? this.fItems.indexOf(Node) : -1;
    }
    /**
    Returns a child node found at a specified index, if any, else null
    @param {number} Index The index
    @returns {tp.TreeNode} Returns a child node found at a specified index, if any, else null
    */
    ByIndex(Index) {
        return !tp.IsEmpty(this.fItems) ? this.fItems[Index] : null;
    }

    /**
    Adds a child node
    @param {tp.TreeNode} Node The node to add
    */
    Add(Node) {
        this.Insert(this.Count, Node);
    }
    /**
    Inserts a child node at a specified index
    @param {number} Index The index
    @param {tp.TreeNode} Node The node to add
    */
    Insert(Index, Node) {
        if (!this.Contains(Node)) {

            let WasLeaf = this.IsLeaf;

            if (!tp.IsEmpty(Node.ParentTreeNode)) {
                Node.ParentTreeNode.Remove(Node);
            }

            if (tp.IsEmpty(this.fItemsElement)) {
                this.fItemsElement = this.Handle.ownerDocument.createElement('div');
                this.Handle.appendChild(this.fItemsElement);
            }

            if (tp.IsEmpty(this.fItems)) {
                this.fItems = []; // tp.TreeNode[]
            }

            if (Index >= this.Count) {
                this.fItems.push(Node);
                this.fItemsElement.appendChild(Node.Handle);
            } else if (Index >= 0) {
                let RefNode = this.fItems[Index];
                tp.ListInsert(this.fItems, Index, Node);
                this.fItemsElement.insertBefore(Node.Handle, RefNode.Handle);
            }

            Node.fParentTreeNode = this;

            tp.RemoveClass(this.Handle, tp.Classes.Leaf);
            tp.AddClass(this.Handle, tp.Classes.Node);

            // start collapsed
            if (WasLeaf)
                this.fPlusMinusElement.innerHTML = tp.TreeNode.CollapseSymbol; //  '+';
        }
    }
    /**
    Removes a specified node from child node list
    @param {tp.TreeNode} Node The node to remove
    */
    Remove(Node) {
        if (this.Contains(Node)) {
            Node.Handle.parentNode.removeChild(Node.Handle);
            tp.ListRemove(this.fItems, Node);
            Node.fParentTreeNode = null;

            if (this.Count === 0) {
                tp.RemoveClass(this.Handle, tp.Classes.Expanded);
                tp.RemoveClass(this.Handle, tp.Classes.Node);
                tp.AddClass(this.Handle, tp.Classes.Leaf);
                this.fPlusMinusElement.innerHTML = '';
            }
        }
    }
    /**
    Removes a node found at a specified index
    @param {number} Index The node index
    */
    RemoveAt(Index) {
        if (this.Count > 0 && tp.InRange(this.fItems, Index)) {
            this.Remove(this.fItems[Index]);
        }
    }


    /**
    Creates and adds a new node.
    @param {string} Text - The display text of the node
    @returns {tp.TreeNode} Returns the newly added {@link tp.TreeNode} node
    */
    AddNode(Text) {
        return this.InsertNode(this.Count, Text);
    }
    /**
    Creates and inserts a new node, at a specified index.
    @param {number} Index The node index
    @param {string} Text - The display text of the node
    @returns {tp.TreeNode} Returns the newly added {@link tp.TreeNode} node
    */
    InsertNode(Index, Text) {
        let el = this.Handle.ownerDocument.createElement('div');
        let Result = new tp.TreeNode(el);
        Result.Text = Text;
        this.Insert(Index, Result);
        return Result;
    }

    /**
    Collapses this node
    */
    Collapse() {
        if (tp.IsHTMLElement(this.Handle) && this.IsNode) {
            this.fPlusMinusElement.innerHTML = tp.TreeNode.ExpandSymbol; // +
            if (this.IsExpanded) {
                this.OnCollapsing();
                tp.RemoveClass(this.Handle, tp.Classes.Expanded);
                this.OnCollapsed();
            }
        }

    }
    /**
    Expands this nodes
    */
    Expand() {
        if (tp.IsHTMLElement(this.Handle) && this.IsNode) {
            this.fPlusMinusElement.innerHTML = tp.TreeNode.CollapseSymbol; // -   
            if (this.IsExpanded === false) {
                this.OnExpanding();
                tp.AddClass(this.Handle, tp.Classes.Expanded);
                this.OnExpanded();
            }

        }
    }
    /**
    Expands or collapses a node or the whole tree-view
    */
    Toggle() {
        if (tp.IsHTMLElement(this.Handle)) {
            if (this.IsExpanded) {
                this.Collapse();
            } else {
                this.Expand();
            }
        }

    }


    /**
    Collapses all nodes in all levels
    */
    CollapseAll() {
        if (this.IsNode) {
            for (let i = 0, ln = this.fItems.length; i < ln; i++) {
                this.fItems[i].CollapseAll();
            }

            this.Collapse();
        }
    }
    /**
    Expands all nodes in all levels
    */
    ExpandAll() {
        if (this.IsNode) {
            for (let i = 0, ln = this.fItems.length; i < ln; i++) {
                this.fItems[i].ExpandAll();
            }

            this.Expand();
        }
    }


    /* Event triggers */

    /** Event trigger
     * @protected
     * */
    OnCollapsing() {
        let tv = this.TreeView;
        if (tv)
            tv.OnCollapsing(this);
    }
    /** Event trigger
    * @protected
    * */
    OnCollapsed() {
        let tv = this.TreeView;
        if (tv)
            tv.OnCollapsed(this);
    }
    /** Event trigger
    * @protected
    * */
    OnExpanding() {
        let tv = this.TreeView;
        if (tv)
            tv.OnExpanding(this);
    }
    /** Event trigger
    * @protected
    * */
    OnExpanded() {
        let tv = this.TreeView;
        if (tv)
            tv.OnExpanded(this);
    }
};

/**
Field. A user defined value.
@type {any}
*/
tp.TreeNode.Tag = null;

/** Field. The expand symbol.
 * @static
 * @type {string}
 */
tp.TreeNode.ExpandSymbol = '▸';      // + ▸ ► 
/** Field. The collapse symbol.
 * @static
 * @type {string}
 */
tp.TreeNode.CollapseSymbol = '▾';    // - ▾ ▼   
 
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.TreeNode.prototype.fHandle;
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.TreeNode.prototype.fStripElement;
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.TreeNode.prototype.fPlusMinusElement;
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.TreeNode.prototype.fImageElement;
/** Field
 * @protected
 * @type {HTMLAnchorElement}
 */
tp.TreeNode.prototype.fTextElement; // HTMLElement;
/** Field
 * @protected
 * @type {HTMLElement}
 */
tp.TreeNode.prototype.fItemsElement;
/** Field
 * @protected
 * @type {tp.TreeNode}
 */
tp.TreeNode.prototype.fParentTreeNode;
/** Field
 * @protected
 * @type {tp.TreeNode[]}
 */
tp.TreeNode.prototype.fItems;
/** Field
 * @protected
 * @type {string}
 */
tp.TreeNode.prototype.fIcoClasses;
/** Field
 * @protected
 * @type {string}
 */
tp.TreeNode.prototype.fImageUrl;

//#endregion

//#region  tp.TreeView
/**
A tree-view control <br />
*/
tp.TreeView = class extends tp.tpElement {

    /**
    Constructor <br />
    @example
    <pre>
        <!-- Example markup -->
        <div id="tv">
            <div>Leaf</div>
            <div>Node
                <div>Leaf</div>
                <div>Node
                    <div>Leaf</div>
                </div>
            </div>
        </div>
    </pre>
    @example
    <pre>
        <!-- Example of produced markup -->
        <div class="tp-TreeView">
            <div>
                <div class="tp-Leaf">
                    <div class="tp-Strip">
                        <div></div>
                        <div></div>
                        <div>Leaf</div>
                    </div>
                </div>
                <div class="tp-Node">
                    <div class="tp-Strip">
                        <div></div>
                        <div></div>
                        <div>Node</div>
                    </div>
                    <div>
                        <div class="tp-Leaf">
                            <div class="tp-Strip">
                                <div></div>
                                <div></div>
                                <div>Leaf</div>
                            </div>
                        </div>
                        <div class="tp-Node">
                            <div class="tp-Strip">
                                <div></div>
                                <div></div>
                                <div>Node</div>
                            </div>
                            <div>
                                <div class="tp-Leaf">
                                    <div class="tp-Strip">
                                        <div></div>
                                        <div></div>
                                        <div>Leaf</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }




    /**
    Gets or sets the parent node <br />
    NOTE: Treeview is a TreeNode too.
    */
    get ParentTreeNode() {
        return null;
    }
    set ParentTreeNode(v) {
    }
    /**
    Returns the number of child nodes
    @type {number}
    */
    get Count() {
        return !tp.IsEmpty(this.fItems) ? this.fItems.length : 0;
    }
    /**
    Returns a boolean value indicating whether this instance has child nodes
    @type {boolean}
    */
    get HasChildren() {
        return this.Count > 0;
    }

    /**
    Returns true if this is the root node, that is the tree-view itself
    @type {boolean}
    */
    get IsRoot() {
        return true;
    }
    /**
    Returns true if this node has child nodes (it is an actual node)
    @type {boolean}
    */
    get IsNode() {
        return false;
    }
    /**
    Returns true if this node has no child nodes (it is a leaf)
    @type {boolean}
    */
    get IsLeaf() {
        return false;
    }

    /**
    Returns true if the node is expanded.
    @type {boolean}
    */
    get IsExpanded() {
        return tp.IsHTMLElement(this.Handle) && tp.HasClass(this.Handle, tp.Classes.Expanded);
    }

    /**
    Returns the level of this Node. A root node has level 0, its children have level 1 and so on.
    @type {number}
    */
    get Level() {
        return 0;
    }
    /**
    Returns the index of this Node in the list of its parent, if any, else -1
    @type {number}
    */
    get Index() {
        return -1;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.TreeView';
        this.fDefaultCssClasses = [tp.Classes.TreeView];
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();

        this.HookEvent(tp.Events.Click);
    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();

        this.NormalizeNodes();
        this.CollapseAll();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);

        switch (Type) {
            case tp.Events.Click:
                if (tp.IsHTMLElement(e.target)) {
                    let Strip = tp.Closest(e.target, '.' + tp.Classes.Strip);
                    if (tp.IsHTMLElement(Strip) && tp.IsHTMLElement(Strip.parentNode)) {
                        let o = tp.GetObject(Strip.parentNode);
                        if (o instanceof tp.TreeNode) {
                            this.OnNodeClick(o);    // VS2017 BrowserLink throws an exception here, for some reason
                            //setTimeout((self) => { self.OnNodeClick(o); }, 0, this);
                        }
                    }
                }

                break;
        }
    }


    /* overridables */
    /**
    Adds the proper css classes to tree-nodes
    @protected
    */
    NormalizeNodes() {
        let List = tp.ChildHTMLElements(this.Handle);

        for (let i = 0, ln = List.length; i < ln; i++) {
            List[i].parentNode.removeChild(List[i]);
        }

        let Node; // tp.TreeNode;
        for (let i = 0, ln = List.length; i < ln; i++) {
            Node = new tp.TreeNode(List[i]);
            this.Add(Node);
        }
    }
    /** Sets the focused node
     * @protected
     * @param {tp.TreeNode} Node The {@link tp.TreeNode} node to set the focus to
     */
    SetFocusedNode(Node) {
        let List = tp.SelectAll(this.Handle, '.' + tp.Classes.Focused);
        for (let i = 0, ln = List.length; i < ln; i++) {
            tp.RemoveClass(List[i], tp.Classes.Focused);
        }

        if (!tp.IsEmpty(Node))
            tp.AddClass(Node.Handle, tp.Classes.Focused);
    }

    /* public */
    /**
    Removes all child nodes from this node
    */
    Clear() {
        tp.RemoveClass(this.Handle, tp.Classes.Expanded);

        if (this.fItemsElement) {
            tp.RemoveChildren(this.fItemsElement);
        }

        if (this.Count > 0) {
            for (let i = 0, ln = this.Count; i < ln; i++) {
                this.fItems[i].ParentTreeNode = null;
            }

            this.fItems.length = 0;
        }

        super.Clear();

    }
    /**
    Returns true if this node is the parent node for a specified child node.
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    @returns {boolean} Returns true if this node is the parent node for a specified child node.
    */
    Contains(Node) {
        return this.IndexOf(Node) >= 0;
    }
    /**
    Returns the index of a specified child node, if any, else -1.
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    @returns {number} Returns the index of a specified child node, if any, else -1.
    */
    IndexOf(Node) {
        return !tp.IsEmpty(this.fItems) ? this.fItems.indexOf(Node) : -1;
    }
    /**
    Returns a child node found at a specified index, if any, else null
    @param {number} Index The index
    @returns {tp.TreeNode} Returns a {@link tp.TreeNode} child node found at a specified index, if any, else null
    */
    ByIndex(Index) {
        return !tp.IsEmpty(this.fItems) ? this.fItems[Index] : null;
    }

    /**
    Adds a child node
    @param {tp.TreeNode} Node The {@link tp.TreeNode} Node to add
    */
    Add(Node) {
        this.Insert(this.Count, Node);
    }
    /**
    Inserts a child node at a specified index
    @param {number} Index The index
    @param {tp.TreeNode} Node The {@link tp.TreeNode} Node to add
    */
    Insert(Index, Node) {
        if (!this.Contains(Node)) {

            if (!tp.IsEmpty(Node.ParentTreeNode)) {
                Node.ParentTreeNode.Remove(Node);
            }

            if (tp.IsEmpty(this.fItemsElement)) {
                this.fItemsElement = this.Handle.ownerDocument.createElement('div');
                this.Handle.appendChild(this.fItemsElement);
            }

            if (tp.IsEmpty(this.fItems)) {
                this.fItems = []; // tp.TreeNode[]
            }

            if (Index >= this.Count) {
                this.fItems.push(Node);
                this.fItemsElement.appendChild(Node.Handle);
            } else if (Index >= 0) {
                let RefNode = this.fItems[Index];
                tp.ListInsert(this.fItems, Index, Node);
                this.fItemsElement.insertBefore(Node.Handle, RefNode.Handle);
            }

            Node.ParentTreeNode = this;
        }
    }
    /**
    Removes a specified node from child node list
    @param {tp.TreeNode} Node The {@link tp.TreeNode} Node to remove
    */
    Remove(Node) {
        if (this.Contains(Node)) {
            this.Handle.removeChild(Node.Handle);
            tp.ListRemove(this.fItems, Node);
            Node.ParentTreeNode = null;

            if (this.Count === 0) {
                tp.RemoveClass(this.Handle, tp.Classes.Expanded);
            }
        }
    }
    /**
    Removes a node found at a specified index
    @param {number} Index The node index
    */
    RemoveAt(Index) {
        if (this.Count > 0 && tp.InRange(this.fItems, Index)) {
            this.Remove(this.fItems[Index]);
        }
    }

    /**
    Creates and adds a new node.
    @param {string} Text - The display text of the node
    @returns {tp.TreeNode} Returns the newly added {@link tp.TreeNode} node
    */
    AddNode(Text) {
        return this.InsertNode(this.Count, Text);
    }
    /**
    Creates and inserts a new node, at a specified index.
    @param {number} Index The index
    @param {string} Text - The display text of the node
    @returns {tp.TreeNode} Returns the newly added {@link tp.TreeNode} node
    */
    InsertNode(Index, Text) {
        let el = this.Handle.ownerDocument.createElement('div');
        let Result = new tp.TreeNode(el);
        Result.Text = Text;
        this.Insert(Index, Result);
        return Result;
    }

    /**
    Collapses this node
    */
    Collapse() {
        if (tp.IsHTMLElement(this.Handle)) {
            for (let i = 0, ln = this.fItems.length; i < ln; i++)
                this.fItems[i].Collapse();
            tp.RemoveClass(this.Handle, tp.Classes.Expanded);
        }
    }
    /**
    Expands this node
    */
    Expand() {
        if (tp.IsHTMLElement(this.Handle) && this.IsExpanded === false && this.HasChildren === true) {
            for (let i = 0, ln = this.fItems.length; i < ln; i++)
                this.fItems[i].Expand();
            tp.RemoveClass(this.Handle, tp.Classes.Expanded);

        }
    }
    /**
    Expands or collapses a node or the whole tree-view
    */
    Toggle() {
        if (tp.IsHTMLElement(this.Handle)) {
            if (this.IsExpanded) {
                this.Collapse();
            } else {
                this.Expand();
            }
        }

    }

    /**
    Collapses all nodes in all levels
    */
    CollapseAll() {
        if (this.IsNode) {
            for (let i = 0, ln = this.fItems.length; i < ln; i++) {
                this.fItems[i].CollapseAll();
            }

            this.Collapse();
        }
    }
    /**
    Expands all nodes in all levels
    */
    ExpandAll() {
        if (this.IsNode) {
            for (let i = 0, ln = this.fItems.length; i < ln; i++) {
                this.fItems[i].ExpandAll();
            }

            this.Expand();
        }
    }

    /* Event triggers */
    /**
    Event trigger
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    */
    OnNodeClick(Node) {
        if (Node instanceof tp.TreeNode) {
            this.SetFocusedNode(Node);
            let Args = new tp.TreeViewEventArgs(Node);
            this.Trigger('NodeClick', Args);
            if (Node.IsNode)
                Node.Toggle();
        }
    }
    /**
    Event trigger
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    */
    OnCollapsing(Node) {
        if (Node instanceof tp.TreeNode) {
            let Args = new tp.TreeViewEventArgs(Node);
            this.Trigger('Collapsing', Args);
        }
    }
    /**
    Event trigger
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    */
    OnCollapsed(Node) {
        if (Node instanceof tp.TreeNode) {
            let Args = new tp.TreeViewEventArgs(Node);
            tp.RemoveClass(this.Handle, tp.Classes.Expanded);
            this.Trigger('Collapsed', Args);
        }
    }
    /**
    Event trigger
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    */
    OnExpanding(Node) {
        if (Node instanceof tp.TreeNode) {
            let Args = new tp.TreeViewEventArgs(Node);
            this.Trigger('Expanding', Args);
        }
    }
    /**
    Event trigger
    @param {tp.TreeNode} Node The {@link tp.TreeNode} node
    */
    OnExpanded(Node) {
        if (Node instanceof tp.TreeNode) {
            let Args = new tp.TreeViewEventArgs(Node);
            tp.AddClass(this.Handle, tp.Classes.Expanded);
            this.Trigger('Expanded', Args);
        }
    }
};

/** Field
 @protected
 @type {HTMLElement}
 */
tp.TreeView.prototype.fItemsElement;
/** Field
 @protected
 @type {tp.TreeNode[]}
 */
tp.TreeView.prototype.fItems;
//#endregion


//---------------------------------------------------------------------------------------
// locator and locator box
//---------------------------------------------------------------------------------------

//#region tp.LocatorEventType
/**
Indicates the type of a locator event
 @class
 @enum {number}
*/
tp.LocatorEventType = {
    /**
    Occurs when the DataValue property has chanded its value
    */
    DataValueChanged: 1,
    /**
    In both modes. Occurs when the SelectSql is already constructed.
    Gives a chance to any client code to add special where to the passed SelectSql just before execution
    */
    AddToSelectSqlWhere: 2,
    /**
    In both modes. Occurs when the locator needs to execute a SELECT.
    The client may execute the passed SelectSql, or any other statement, and assign the Locator.ListTable
    */
    SelectListTable: 4,
    /**
    In LookUp mode ONLY. Gives a chance to a client code to configure list table columns, titles, visibility etc.
    */
    SetupListTable: 8,
    /**
    Occurs when the ListTable.DefaultView must be filtered.
    */
    FilterListTable: 0x10
};
Object.freeze(tp.LocatorEventType);
//#endregion

//#region tp.LocatorEventArgs
/**
EventArgs for the locator class
*/
tp.LocatorEventArgs = class extends tp.EventArgs {

    /**
    Constructor.
    @param {tp.Locator} Locator The locator
    @param {tp.LocatorEventType} EventType The event type. One of the {@link tp.LocatorEventType} constants.
    */
    constructor(Locator, EventType) {
        super(null, Locator, null);
    }

    /* private */
    /** Field
     @private
     @type {tp.Locator} */
    fLocator = null;
    /** Field
     @private
     @type {tp.LocatorEventType} */
    fEventType = tp.LocatorEventType.DataValueChanged;


    /* properties */
    /**
    Returns the locator event type
    @type {tp.LocatorEventType}
    */
    get EventType() {
        return this.fEventType;
    }
    /**
    Returns the locator
    @type {tp.Locator}
    */
    get Locator() {
        return this.fLocator;
    }
    /**
    Returns the control associated to locator
    @type {tp.Control}
    */
    get Control() {
        return this.Locator.Control;
    }
    /**
    Returns the locator descriptor
    @type {tp.LocatorDescriptor}
    */
    get Descriptor() {
        return this.Locator.Descriptor;
    }
    /**
    Gets or sets the list table
    @type {tp.DataTable}
    */
    get ListTable() {
        return this.Locator.ListTable;
    }
    set ListTable(v) {
        this.Locator.ListTable = v;
    }

 
    /** The value of the where clause 
     @type {string}
     */
    WhereSql = '';
    /**
    Gets or sets the filter to apply to ListTable.DefaultView.RowFilter
    @type {string}
    */
    ListTableFilter = '';

    /** Adds a condition text to this instance WhereSql and returns the new WhereSql.
     * @param {string} Text The condition to be added to WhereSql.
     * @returns {string} Returns the new WhereSql.
     */
    AddToWhereSql(Text) {
        if (tp.IsEmpty(this.WhereSql) || !tp.IsString(this.WhereSql))
            this.WhereSql = '';

        if (tp.IsString(Text) && !tp.IsBlank(Text)) {
            this.WhereSql = tp.IsBlank(this.WhereSql) ? Text : `${this.WhereSql} and ${Text}`;
        }

        return this.WhereSql;
    }
};

//#endregion


//#region tp.ILocatorLink
/**
A link between a locator and a control
@interface
*/
tp.ILocatorLink = class {
    /**
    Sets the Text to Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    @param {string} Text The text to set
    */
    BoxSetText(Locator, Box, Text) { }
    /**
    Returns the Text of the Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    @returns {string} Returns the Text of the Box
    */
    BoxGetText(Locator, Box) { return ''; }
    /**
    Selects all text in the Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    */
    BoxSelectAll(Locator, Box) { }

};
//#endregion


tp.Urls.LocatorGetDef = '/Locator/GetDef';
tp.Urls.LocatorSqlSelect = '/Locator/SqlSelect';

//#region tp.Locators
/** A helper static class for locators */
tp.Locators = class {

    /**
     * Returns a locator definition registered under a specified name, if any, else null/undefined. <br />
     * It first searches the already downloaded definitions and if the requested is not found then calls the server.
     * @param {string} Name The name of the locator definition.
     * @returns {tp.LocatorDef} Returns a locator definition registered under a specified name, if any, else null/undefined.
     */
    static async GetDefAsync(Name) {
        let Result = this.Descriptors.find(item => tp.IsSameText(Name, item.Name));

        if (!tp.IsValid(Result)) {

            let Url = tp.Urls.LocatorGetDef;
            let Data = {
                LocatorName: Name,
            };

            let Args = await tp.Ajax.GetAsync(Url, Data);  
 
            Result = new tp.LocatorDef();
            Result.Assign(Args.Packet);
            this.Descriptors.push(Result);
        }

        return Result;
    }
    /**
     * Executes the SELECT statement of a specified Locator, along with the specified where clause and returns the result {@link tp.DataTable}
     * @param {string} Name The name of the locator definition.
     * @param {string} WhereSql The where clause text to add to the SELECT statement of a specified Locator.
     * @returns {tp.DataTable} Returns the result {@link tp.DataTable}
     */
    static async SqlSelectAsync(Name, WhereSql) {
        let Url = tp.Urls.LocatorSqlSelect;
        let Data = {
            LocatorName: Name,
            WhereSql: WhereSql || ''
        };

        let Args = await tp.Ajax.GetAsync(Url, Data); 
        let Table = new tp.DataTable();
        Table.Assign(Args.Packet);

        return Table;
    }
};

/** A list of locator definitions already downloaded from server.
 * @static
 * @type {tp.LocatorDef[]} 
 * */
tp.Locators.Descriptors = [];
//#endregion 


//#region tp.LocatorDef

/**
Describes a {@link tp.Locator}. <br />

A locator represents (returns) a single value, but it can handle and display multiple values
in order to help the end user in identifying and locating that single value.  <br />

For example, a TRADES table has a CUSTOMER_ID column, representing that single value, but the user interface
has to display information from the CUSTOMERS table, specifically, the ID, CODE and NAME columns.

The TRADES table is the data table and the CUSTOMER_ID is the DataField field name.
The CUSTOMERS table is the list table, denoted by the ListTableName, and the ID is the ListKeyField field name.

The fields, ID, CODE and NAME, may be described by individual {@link tp.LocatorFieldDef} field items.  <br />

In other words, the TRADES.CUSTOMER_ID value comes from the CUSTOMERS.ID value. <br />
The TRADES is the data-table where the CUSTOMERS is the list table. <br />
A locator provides a filtering mechanism for the user to locate a CUSTOMERS row. <br />
Also a locator displays the CUSTOMERS rows when the filtering returns more than one row. <br />

A locator can be used either as a single-row control, as the {@link tp.LocatorBox}  does, or as a group of
related columns in a {@link tp.Grid}.  <br />

NOTE: A locator of a {@link tp.LocatorBox} type, may or may not define the tp.LocatorFieldDef.DataField field names. <br />
Usually in a case like that, the data table contains just the key field, the LocatorBox.DataField. <br />
A locator of a grid-type must define the names of those fields always and the data table must contain DataColumn columns
on those fields.
 * */
tp.LocatorDef = class {

    /** Contructor */
    constructor() {
        this.Fields = [];
    }

    /** Field
    * @private
    * @type {string}
    */
    fName = '';
    /** Field
    * @private
    * @type {string}
    */
    fTitle = '';
    /** Field
    * @private
    * @type {string}
    */
    fListKeyField = '';
    /** Field
    * @private
    * @type {string}
    */
    fZoomCommand = '';


    /* properties */
    /**
    Gets or sets the locator descriptor name
    @type {string}
    */
    get Name() {
        return tp.IsString(this.fName) && !tp.IsBlank(this.fName) ? this.fName : tp.NO_NAME;
    }
    set Name(v) {
        this.fName = v;
    }
    /**
    Gets or sets the title of the locator descriptor.
    @type {string}
    */
    get Title() {
        return tp.IsString(this.fTitle) && !tp.IsBlank(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        this.fTitle = v;
    }
    /**
    Gets or sets the name of the list table
    @type {string}
    */
    ListTableName = '';
    /**
    Gets or sets the key field of the list table. The value of this field goes to the DataField
    */
    get ListKeyField() {
        return tp.IsString(this.fListKeyField) && !tp.IsBlank(this.fListKeyField) ? this.fListKeyField : 'Id';
    }
    set ListKeyField(v) {
        this.fListKeyField = v;
    }
    /**
    Gets or sets the zoom command name. A user inteface (form) can use this name to call the command.
    @type {string}
    */
    get ZoomCommand() {
        return tp.IsString(this.fZoomCommand) && !tp.IsBlank(this.fZoomCommand) ? this.fZoomCommand : '';
    }
    set ZoomCommand(v) {
        this.fZoomCommand = v;
    }

    /**
    The where clause that is produced by a locator algorithm.
    @type {string}
    */
    WhereSql = '';
    /**
    The order by clause to append to the statement
    @type {string}
    */
    OrderBySql = '';

    /**
    Indicates whether the locator is readonly
    @type {boolean}
    */
    ReadOnly = false;
    /**
    Gets the list of descriptor fields.
    @type {tp.LocatorFieldDef[]}
    */
    Fields = [];

    /**
    Assigns the properties of this instance from a specified source object.
    @param {object} Source The source to copy property values from.
    */
    Assign(Source) {
        if (tp.IsValid(Source)) {
            for (var Prop in Source) {
                if (!tp.IsFunction(Source[Prop]) && tp.HasWritableProperty(this, Prop)) {
                    if (Prop === 'Fields' && tp.IsArray(Source[Prop])) {
                        this.Fields = [];
                        let FieldDef;
                        Source[Prop].forEach((SourceFieldDef) => {
                            FieldDef = new tp.LocatorFieldDef();
                            FieldDef.Assign(SourceFieldDef);
                            this.Fields.push(FieldDef);
                        });
                    }
                    else {
                        this[Prop] = Source[Prop];
                    }
                }
            }
        }

    }

    /**
    Finds a {@link tp.LocatorFieldDef}  field descriptor by list field name and returns the field or null if not found
    @param {string} ListField - The name of the ListField.
    @returns {tp.LocatorFieldDef} Finds a {@link tp.LocatorFieldDef}  field descriptor by list field name and returns the field or null if not found
    */
    Find(ListField) {
        return this.Fields.find((item) => { return tp.IsSameText(ListField, item.Name); });
    }
    /**
    Finds a {@link tp.LocatorFieldDef} field descriptor by data field and returns the field or null if not found
    @param {string} DataField - The field name of the field in the target data-source
    @returns {tp.LocatorFieldDef} Finds a {@link tp.LocatorFieldDef} field descriptor by data field and returns the field or null if not found
    */
    FindByDataField(DataField) {
        return this.Fields.find((item) => { return tp.IsSameText(DataField, item.DataField); });
    }

};
//#endregion 

//#region tp.LocatorFieldDef
/** Describes the "field" (text box or grid column) of a Locator. <br />
 * A field such that associates a column in the data table (the target) to a column in the list table (the source).
 * */
tp.LocatorFieldDef = class {

    /** Contructor */
    constructor() {
    }

    /**
    The field name in the list (source) table
    @type {string}
    */
    Name = '';
    /**
    The table name of the list (source) table
    @type {string}
    */
    TableName = '';

    /** When not empty/null then it denotes a field in the dest data table where to put the value of this field.
     * @type {string}
     */
    DataField = '';
    /**
    Gets or sets the data type of the field. One of the tp.DataType constants
    @type {string}
    */
    DataType = tp.DataType.String;

    /**
    Gets or sets tha Title of this descriptor, used for display purposes.
    @type {string}
    */
    Title = '';
    /**
    Gets or sets a resource Key used in returning a localized version of Title.
    @type {string}
    */
    get TitleKey() {
        return tp.IsBlank(this.fTitleKey) ? this.Name : this.fTitleKey;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }

    /**
    Indicates whether a TextBox for this field is visible in a LocatorBox
    @type {boolean}
    */
    Visible = true;
    /**
    When true the field can be part in a where clause in a select statement.
    @type {boolean}
    */
    Searchable = true;
    /**
    Indicates whether the field is visible when the list table is displayed
    @type {boolean}
    */
    ListVisible = true

    /**
    Used to notify criterial links to treat the field as an integer boolea fieldn (1 = true, 0 = false)
    @type {boolean}
    */
    IsIntegerBoolean = false;
    /**
    Controls the width of the text box in a LocatorBox. In pixels.
    @type {number}
    */
    Width = 70;

    /**
    Assigns the properties of this instance from a specified source object.
    @param {object} Source The source to copy property values from.
    */
    Assign(Source) {
        if (tp.IsValid(Source)) {
            for (let Prop in Source) {
                if (!tp.IsFunction(Source[Prop]) && tp.HasWritableProperty(this, Prop)) {
                    this[Prop] = Source[Prop];
                }
            }
        }
    }
};

tp.LocatorFieldDef.BoxDefaultWidth = 70;
//#endregion 

//#region tp.Locator
/** The locator represents (returns) a single value. <br />

A locator acts similarly to a lookup combobox, in helping the user to select and return a single value. <br />

It can be used with large data-sets because it constructs a WHERE clause, asks the server to execute a named SELECT statement and displays the result.  <br />

A locator is just a class, not a user interface control.  <br />
There are special user interface controls, such as the {@link tp.Grid} and the {@link tp.LocatorBox},
that connect to a locator and help the user to select a value using the locator. <br />

The programmer never creates a {@link tp.Locator} instance.
A locator instance is created automatically when a {@link tp.LocatorBox} is created and initialized and when a locator column is added to a {@link tp.Grid}. <br />

A locator always works in conjuction with a control, a {@link tp.LocatorBox} or a {@link tp.Grid}, and its Control property returns that control.
From that associated control the locator knows its DataSource, DataTable, DataColumn and DataField. <br />

The locator provides the Descriptor property or type {@link tp.LocatorDef}.
That descriptor contains a list of {@link tp.LocatorFieldDef} fields. <br />
The fields are used in constructing a WHERE clause and when the SELECT statement is executed a ListTable is returned to the locator.
The ListTable is used as a source by the locator. The locator locates the ListRow and then uses that row in assigning its data fields.
@implements {tp.IDataSourceListener}
@implements {tp.IDropDownBoxListener} 
 * */
tp.Locator = class extends tp.tpObject {

    /**
    Constructor
    */
    constructor() {
        super();

        this.fDetailLocators = [];
        this.fActive = true;
        this.fAssigningCount = 0;

        this.fInitialized = false;
        this.fControls = new tp.Dictionary(); // Key = tp.LocatorFieldDef, Value = HTMLInputElement 

        this.fListRow = null;
        this.fListKeyFieldDef = null;

        this.fInDataSourceEvent = false;
        this.fKeyValue = null;

        this.fInitialized = false;
        this.fControl = null;

        // drop-down box
        this.fDropDownBox = new tp.DropDownBox();
        this.fDropDownBox.Owner = this;

        // this listbox is displayed by the drop-down box
        this.fListBox = new tp.ListBox();
        this.fListBox.ParentHandle = this.fDropDownBox.Handle;
        this.fListBox.Position = 'absolute';
        this.fListBox.Width = '100%';
        this.fListBox.Height = '100%';
        this.fListBox.Handle.style.border = 'none';

        this.fListBox.On(tp.Events.Click, this.ListBox_Click, this);
    }


    /** Field
     * @private
     * @type {tp.Locator}
     */
    fMaster = null;
    /** Field
     * @private
     * @type {tp.Locator[]}
     */
    fDetailLocators = [];

    /** Field. The located DataRow in ListTable
     * @private
     * @type {tp.DataRow}
     */
    fListRow = null;
    /** Field. The field descriptor of the key field of the ListTable
     * @private
     * @type {tp.LocatorFieldDef}
     */
    fListKeyFieldDef = null;

    /** Field
    * @private
    * @type {boolean}
    */
    fActive = false;
    /** Field
    * @private
    * @type {number}
    */
    fAssigningCount = 0;
    /** Field
    * @private
    * @type {boolean}
    */
    fInDataSourceEvent = false;

    /** Field
    * @private
    * @type {any}
    */
    fKeyValue = null;

    /** Field
    * @private
    * @type {boolean}
    */
    fInitialized = false;
    /** Field
    * @private
    * @type {tp.Control}
    */
    fControl = null;

    /** Field. A {@link tp.Dictionary} where Key = tp.LocatorFieldDescriptor and Value = HTMLInputElement
    * @private
    * @type {tp.Dictionary}
    */
    fControls = null;
    /** Field
    * @private
    * @type {tp.DropDownBox}
    */
    fDropDownBox = null;
    /** Field. This listbox is displayed by the drop-down box
    * @private
    * @type {tp.ListBox}
    */
    fListBox = null;


    /** Field
    * @private
    * @type {number}
    */
    fMaxDropdownItems = 10;
    /** Field
    * @private
    * @type {tp.LocatorDef}
    */
    fDescriptor = null;


    /* properties */
    /**
    True while setting values to the Datasource fields, that is while the SetDataValues() is executing.
    @private
    @type {boolean}
    */
    get Assigning() {
        return this.fAssigningCount > 0;
    }
    set Assigning(v) {
        if (v === true)
            this.fAssigningCount++;
        else
            this.fAssigningCount--;
    }
    /**
    Activates or deactivates the locator. Returns true when is active AND initialized
    @type {boolean}
    */
    get Active() {
        return this.fActive && this.Initialized;
    }
    set Active(v) {
        this.fActive = v === true;
        this.Initialize();
    }
    /**
    Returns true if this instance is initialized
    @type {boolean}
    */
    get Initialized() {
        return this.fInitialized === true;
    }
    /**
    Gets or sets the LocatorDef of this locator
    @type {tp.LocatorDef}
    */
    get Descriptor() {
        if (tp.IsEmpty(this.fDescriptor)) {
            this.fDescriptor = new tp.LocatorDef();
        }

        return this.fDescriptor;
    }
    set Descriptor(v) {
        this.fDescriptor = v;
    }

    /**
    Gets or sets the associated control, must be a LocatorBox or a Grid and implement the ILocatorLink.
    @type {tp.Control}
    */
    get Control() {
        return this.fControl;
    }
    set Control(v) {
        if (v !== this.fControl) {
            if (!(v instanceof tp.LocatorBox || v instanceof tp.Grid)) {
                tp.Throw('Locator.Control must be a tp.LocatorBox or tp.Grid');
            }

            this.fControl = v;

            this.fListBox.DataSource = !tp.IsValid(v) ? null : this.fControl.DataSource;
        }
    }
    /**
    True when this is a locator on a Grid. A multi-line locator
    @type {boolean}
    */
    get IsGridMode() {
        return this.Control instanceof tp.Grid;
    }
    /**
    True when this is a locator on a locator box control. A single-line locator
    @type {boolean}
    */
    get IsBoxMode() {
        return this.Control instanceof tp.LocatorBox;
    }

    /**
    Returns the current DataRow of the Control.Datasource  
    @type {tp.DataRow}
    */
    get CurrentRow() {
        return this.Initialized ? this.DataSource.Current : null;
    }
    /**
    Returns the target data-source, the data-source associated to control
    @type {tp.DataSource}
    */
    get DataSource() {
        return this.Control ? this.Control.DataSource : null;
    }
    /**
    Returns the target data table
    */
    get DataTable() {
        return this.DataSource ? this.DataSource.Table : null;
    }
    /**
    Returns the target data column
    @type {tp.DataColumn}
    */
    get DataColumn() {
        if (this.DataTable) {
            if (!tp.IsBlank(this.DataField)) {
                let Result = this.DataTable.FindColumn(this.DataField);
                return Result;
            }
        }
        return null;
    }
    /**
    Gets or sets the name of the field this instance is bound to
    @type {string}
    */
    get DataField() {
        return this.fListBox.DataField;
    }
    set DataField(v) {
        this.fListBox.DataField = v;
    }

    /**
    Gets or set the list DataTable, that is the source table where data values come from.
    @type {tp.DataTable}
    */
    get ListTable() {
        let Result = null;

        if (tp.IsValid(this.fListBox) && this.fListBox.ListSource instanceof tp.DataSource)
            Result = this.fListBox.ListSource.Table;

        return Result;
    }
    set ListTable(v) {
        if (!tp.IsValid(v))
            v = null;

        if (v === null || v instanceof tp.DataTable)
            this.fListBox.ListSource = v;
    }

    /**
    A {@link tp.Dictionary} dictionary, where Key = {@link tp.LocatorFieldDef}  and Value = {@link HTMLInputElement}, 
    that associates fields to sub-controls of the Control (i.e. boxes of a locator box control)
    @type {tp.Dictionary}
    */
    get Controls() {
        return this.fControls;
    }

    /**
    Returns true if Locator is on a new row, not yet attached to the underlying DataTable
    @type {boolean}
    */
    get InNewRow() {
        return (this.CurrentRow === null) || (this.CurrentRow.State === tp.DataRowState.Detached);
    }
    /**
    For creating cascade lookups. The master locator this locator uses to get the value of the DetailKey.
    @type {tp.Locator}
    */
    get Master() {
        return this.fMaster;
    }
    set Master(v) {
        if (v !== this.Master) {
            if (this.Master instanceof tp.Locator) {
                tp.ListRemove(this.Master.fDetailLocators, this);
            }

            this.fMaster = v;

            if (this.Master instanceof tp.Locator) {
                this.Master.fDetailLocators.push(this);
            }
        }
    }
    /** 
    For creating cascade lookups. <br />
    A field name of the ListTable (i.e. CountryId) used in getting a value
     to be used in filtering the ListTable.DefaultView based on that value coming from the Master.KeyValue.
     @type {string}
    */
    DetailKey = '';

    DropDownBoxStage = tp.DropDownBoxStage.Closed;

    /**
    Gets or sets the maximum number of items to be shown in the dropdown list
    @type {number}
    */
    get MaxDropdownItems() {
        let Result = this.fMaxDropdownItems || 10;
        if (Result > 30) {
            Result = 30;
        }
        return Result;
    }
    set MaxDropdownItems(v) {
        this.fMaxDropdownItems = v;
    }

    /** Returns the key value, the value the locator represents. Could be null. <br />
    This value comes from the list table.   
    @type {any}
    */
    get KeyValue() {
        return !tp.IsEmpty(this.fKeyValue) ? this.fKeyValue : null;
    }
    /** Sets the key value.
     * NOTE: Treat this setter as private and with extreme care.
     * CAUTION: Setting this property, ends up setting all DataFields, the DataValue and the text of the controls. <br />
     * It may end up calling the server and re-selecting the {@link tp.Locator.ListTable}.
     * @private
     * @param {any} v The value to set as key value.
     */
    SetKeyValue(v) {
        if (this.Active) {

            this.fKeyValue = v;

            if (tp.IsEmpty(v)) {
                this.fListRow = null;
                this.SetDataFieldValues(null);
            }
            else {
                this.LocateKeyAsync()
                    .then((Result) => {
                        if (Result === true)
                            this.SetDataFieldValues(this.fListRow);
                    });
            }
        }
    }

    /** Gets or sets the value of the DataField <br />
     * This value comes from the {@link tp.DataSource} of the control of this locator.
     * @type {any}
     */
    get DataValue() {
        return this.Active && !tp.IsEmpty(this.CurrentRow) ? this.CurrentRow.Get(this.DataColumn) : null;
    }
    /** Sets the values of all DataFields, except of the DataField, which is the key field, and the text of the controls, based on a specified {@link tp.DataRow} that comes from the ListTable.
    * @private
    * @param {tp.DataRow} SourceListRow A {@link tp.DataRow} that comes from the ListTable.
    */
    SetDataFieldValues(SourceListRow) {
        if (this.Active && !this.Assigning && !tp.IsEmpty(this.fListKeyFieldDef)) {
            this.Assigning = true;
            try {
                //let S = this.IsGridMode ? "Grid" : "LocatorBox";

                let Row = this.CurrentRow;

                if (!tp.IsEmpty(Row)) {

                    let Clearing = tp.IsEmpty(SourceListRow);

                    let i, ln, v,
                        SubControl,         // HTMLInputElement
                        FieldDef,           // tp.LocatorFieldDef
                        Column,             // tp.DataColumn
                        Dest,               // string
                        Source;             // string                    

                    for (i = 0, ln = this.Descriptor.Fields.length; i < ln; i++) {

                        FieldDef = this.Descriptor.Fields[i];

                        // data field
                        if (FieldDef !== this.fListKeyFieldDef && !tp.IsBlank(FieldDef.DataField) && Row.Table.ContainsColumn(FieldDef.DataField)) {
                            if (Clearing) {
                                Row.Set(FieldDef.DataField, null);
                            } else {
                                if (SourceListRow.Table.ContainsColumn(FieldDef.Name)) {
                                    v = SourceListRow.Get(FieldDef.Name);
                                    Row.Set(FieldDef.DataField, v);
                                }
                            }
                        }

                        // sub-controls, i.e. boxes of a locator box control
                        SubControl = this.Controls.ContainsKey(FieldDef) ? this.Controls.Get(FieldDef) : null;

                        if (SubControl) {
                            if (Clearing) {
                                SubControl.value = '';
                            } else {
                                Column = SourceListRow.Table.FindColumn(FieldDef.Name);
                                if (Column) {
                                    v = SourceListRow.Get(FieldDef.Name);
                                    Dest = SubControl.value;
                                    Source = Column.Format(v, false);
                                    if (!tp.IsSameText(Dest, Source)) {
                                        SubControl.value = Source;
                                    }
                                }
                            }
                        }

                    } // for

                }
            }
            finally {
                this.Assigning = false;
            }
        }
    }
    /** Clears the data value
     * */
    ClearDataValue() {
        if (this.Active && !tp.IsEmpty(this.CurrentRow)) {
            this.CurrentRow.Set(this.DataColumn, null);
            this.SetDataFieldValues(null);
        }
    }

    /**
    Locates the key value in the ListTable, based on {@link KeyValue} value, and assigns the ListRow. Returns true on success.  <br />
    NOTE: It does NOT SELECT the list table, it just performs the search into the existing rows.  <br />
    NOTE: This should be called ONLY by the LocateKeyAsync() function.
    @private
    @returns {boolean} Locates the key value in the ListTable and assigns the ListRow. Returns true on success.
    */
    LocateListRowInternal() {
        if (!tp.IsEmpty(this.ListTable)) {
            try {
                this.fListRow = this.ListTable.Locate(this.fListKeyFieldDef.Name, this.KeyValue);
                return !tp.IsEmpty(this.fListRow);
            }
            catch (e) {
                //
            }
        }

        return false;
    }
    /**
    Locates the key value in the ListTable, based on {@link KeyValue} value, and assigns the ListRow. Returns true on success in a boolean {@link Promise}. <br />
    NOTE: It SELECTs the list table. <br />
    Returns a boolean {@link Promise}
    @private
    @returns {boolean} Returns a boolean {@link Promise}
    */
    async LocateKeyAsync() {

        if (this.Active && !tp.IsEmpty(this.KeyValue) && !tp.IsEmpty(this.fListKeyFieldDef)) {

            this.fListRow = null;

            if (this.LocateListRowInternal()) {
                return true;
            }
            else {

                /*
                // TODO: 
                tp.InfoNote("Re-select");

                // then re-select
                try {
                    let WhereUser = tp.Format("{0}.{1}", this.fListKeyFieldDef.TableName, this.fListKeyFieldDef.ListField);
                    let v = this.KeyValue;

                    if (this.fListKeyFieldDef.DataType === tp.DataType.String)
                        WhereUser += tp.Format(" = '{0}' ", v.toString());
                    else if (tp.DataType.IsDateTime(this.fListKeyFieldDef.DataType))
                        WhereUser += tp.Format(" = '{0}'", v.toString());
                    else
                        WhereUser += tp.Format(" = {0}", v); // number

                    let RowCount = await this.SelectListTableAsync(WhereUser);

                    // and then look again into the new data 
                    if (RowCount > 0) {
                        let Result = this.LocateListRowInternal();
                        return Result;
                    }

                }
                catch (e) {
                    //
                }
                */

            }
        }

        return false;

    }



    /** Sets-up the list table
     * @private
     * */
    SetupListTable() {
        if (!tp.IsEmpty(this.ListTable)) {
            this.FilterListTable();

            let FieldDef; // LocatorFieldDef;
            let Column; // tp.DataColumn;
            for (let i = 0, ln = this.ListTable.Columns.length; i < ln; i++) {
                Column = this.ListTable.Columns[i];
                FieldDef = this.Descriptor.Find(Column.Name);

                Column.Visible = (FieldDef !== null) && FieldDef.ListVisible;
                if (Column.Visible === true) {
                    Column.Title = '';
                    Column.Title = FieldDef.Title;
                }
            }

            let Args = new tp.LocatorEventArgs(this, tp.LocatorEventType.SetupListTable);
            this.OnAnyEvent(Args);
        }

    }
    /** Filters the list table
     * @private
     * */
    FilterListTable() {
        // TODO:
    }
    /**
    Reselects the list table and returns a promise with the row count of the table.
    @private
    @returns {number} Returns a number {@link Promise}
    */
    ResetListData() {
        this.ListTable = null;
        let Result = this.SelectListTableAsync("");
        return Result;
    }

    /**
    Executes the SELECT statement, assigns the ListTable and returns a number {@link Promise} with the row count of the table.
    @private
    @param {string} [WhereUser=''] Optional. It is the WHERE added by the user in a locator control or column. Could be null or empty
    @returns {number} Returns a number {@link Promise} with the row count of the table.
    */
    async SelectListTableAsync(WhereUser = '') {
        if (this.Active) {

            this.ListTable = null;

            let Args = new tp.LocatorEventArgs(this, tp.LocatorEventType.SelectListTable);
            Args.WhereSql = WhereUser || '';

            // this may assign the ListTable
            this.OnAnyEvent(Args);

            let Table = await tp.Locators.SqlSelectAsync(this.Descriptor.Name, WhereUser);

            this.ListTable = Table;

            this.SetupListTable();

            return this.ListTable.Rows.length;
        }

        return 0;
    }
    /**
    Displays the result of a SELECT either as drop-down list or in a locator dialog
    @private
    @param {HTMLElement} Associate The associate control to set to the drop-down box
    */
    DisplayListTable(Associate) {
        if (!tp.IsEmpty(this.ListTable)) {

            // display a drop-down box for the user to select the list row
            if (this.ListTable.Rows.length <= tp.SysConfig.LocatorShowDropDownRowCountLimit) {

                if (this.fDropDownBox.IsOpen)
                    this.fDropDownBox.Close();

                this.LocateKeyAsync()
                    .then(() => {
                        this.fDropDownBox.Associate = Associate;
                        this.fDropDownBox.Open();
                    });

            }
            // display an SqlFilterBox dialog box for the user to select the list row
            else {

                /** @type {tp.SqlFilterDef[]} */
                let FilterDefs = [];

                // prepare FilterDefs for the the tp.SqlFilterBox dialog
                this.fDescriptor.Fields.forEach((item) => {
                    if (item.Searchable) {
                        let FieldPath = tp.FieldPath(item.TableName, item.Name);
                        let FilterDef = tp.SqlFilterDef.Create(FieldPath, item.Title || item.Name, item.DataType);
                        FilterDefs.push(FilterDef);
                    }
                });

                if (FilterDefs.length > 0) {

                    /** Selects and returns the list table. Called by the tp.SqlFilterBox dialog.
                     * @param {string} WhereSql The where clause text to add to the SELECT statement of a specified Locator.
                     * @returns {tp.DataTable} Returns a tp.DataTable
                     */
                    let SelectFunc = async (WhereSql) => {
                        let Table = await tp.Locators.SqlSelectAsync(this.fDescriptor.Name, WhereSql);
                        return Table;
                    };

                    /** Callback to call when the tp.SqlFilterBox dialog window closes. A function as function(Args: tp.WindowArgs)
                     * @param {any} WindowArgs
                     */
                    let CloseFunc = (WindowArgs) => {
                        if (WindowArgs.Window.DialogResult === tp.DialogResult.OK) {

                            /** @type {tp.DataRow} */
                            let SelectedRow = WindowArgs.SelectedRow;
                            let Table = WindowArgs.Table;

                            if (SelectedRow && Table) {

                                let v = SelectedRow.Get(this.fDescriptor.ListKeyField, null);
                                if (v) {
                                    this.ListTable = Table;
                                    this.SetupListTable();
                                    this.DataSource.Current.Set(this.DataField, v);
                                    this.fListRow = SelectedRow;
                                    this.SetDataFieldValues(this.fListRow);
                                }
                            }
                        }

                    };

                    // call the dialog
                    tp.SqlFilterBox(FilterDefs, SelectFunc, this.ListTable, CloseFunc);
                }
            }
        }

    }

    /**
    Forces a re-filtering of list table
    @private
    */
    MasterDataValueChanged() {
        this.FilterListTable();
        this.SetDataFieldValues(null);
    }

    /** Event trigger
     * @private
     * @param {tp.LocatorEventArgs} Args The {@link tp.LocatorEventArgs} arguments
     */
    OnAnyEvent(Args) {
        this.Trigger('AnyEvent', Args);
    }
    /**
    Called by the dropdown box to inform its owner about a stage change.
    @param {tp.DropDownBox} Sender The sender
    @param {tp.DropDownBoxStage} Stage One of the {@link tp.DropDownBoxStage} constants.
    */
    OnDropDownBoxEvent(Sender, Stage) {
        let count,
            n;

        this.DropDownBoxStage = Stage;

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
                if (!tp.IsValid(this.fListBox.DataSource))
                    this.fListBox.DataSource = this.fControl.DataSource;

                break;

            case tp.DropDownBoxStage.Opened:
                if (!tp.IsEmpty(this.ListTable) && this.ListTable.RowCount > 0) {
                    count = this.ListTable.RowCount;

                    n = count <= 0 ? 2 : (count < this.MaxDropdownItems ? count + 1 : this.MaxDropdownItems);
                    n = (n * this.fListBox.ItemHeight) + 5;
                    this.fDropDownBox.Height = n;
                }

                break;

            case tp.DropDownBoxStage.Closing:
                break;

            case tp.DropDownBoxStage.Closed:
                break;
        }
    }
    /** Event handler
     * @private
     * @param {tp.EventArgs} Args The {@link tp.EventArgs} argument
     */
    ListBox_Click(Args) {
        if (!this.fDropDownBox.Resizing)
            this.fDropDownBox.Close();
    }
    /** Event trigger
     * @private
     * */
    OnDataValueChanged() {
        // trigger events
        for (let i = 0, ln = this.fDetailLocators.length; i < ln; i++) {
            this.fDetailLocators[i].MasterDataValueChanged();
        }

        let Args = new tp.LocatorEventArgs(this, tp.LocatorEventType.DataValueChanged);
        this.OnAnyEvent(Args);
    }

    /**
    Initializes the locator, if the Initialized flag is false.
    */
    Initialize() {
        if (!this.Initialized && !tp.IsEmpty(this.Control) && !tp.IsBlank(this.DataField)) {
            if (this.Control.DataSource && this.Control.DataSource.Table instanceof tp.DataTable) {
                this.fListKeyFieldDef = this.Descriptor.Find(this.Descriptor.ListKeyField);

                if (this.fListKeyFieldDef) {
                    let Column = this.Control.DataSource.Table.FindColumn(this.DataField);
                    if (Column) {
                        this.DataSource.AddDataListener(this);
                        this.fInitialized = true;
                    }
                }
            }
        }
    }

    /**
    Called by a locator control. Executes the list operation.
    Displays a drop-down or a dialog for the user to select a data row.
    @param {HTMLElement} Associate The control to set as the associate control of the drop-down.
    */
    ShowList(Associate) {
        if (!tp.IsValid(Associate))
            tp.Throw("No Associate");

        if (tp.IsValid(this.ListTable)) {
            this.DisplayListTable(Associate);
        }
        else {
            this.SelectListTableAsync('')
                .then((RowCount) => {
                    if (RowCount > 0) {
                        this.DisplayListTable(Associate);
                    } else {
                        // nothing here
                    }
                });
        }


        /*
                let RowCount = await this.SelectListTableAsync('');
                if (RowCount > 0) {
                    this.DisplayListTable(Associate);
                } else {
                    // nothing here
                }
                // ---------------------------
                this.SelectListTableAsync('')
                    .then((RowCount) => {
                        if (RowCount > 0) {
                            this.DisplayListTable(Associate);
                        } else {
                            // nothing here
                        }
                    });
         */
    }
    HideList() {
        if (this.fDropDownBox.IsOpen)
            this.fDropDownBox.Close();
    }

    /* boxes */
    /**
     * Event handler. Called focus enters a box. 
     * Reads from current row and sets the value of the box.
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDef} FieldDef The {@link tp.LocatorFieldDef} descriptor
     */
    Box_Enter(LocatorLink, Box, FieldDef) {
        if (!this.Initialized)
            this.Initialize();

        if (this.Active && (FieldDef instanceof tp.LocatorFieldDef)) {
            let S = this.DataValueAsTextOf(FieldDef.DataField);
            LocatorLink.BoxSetText(this, Box, S);

            if (this.IsBoxMode)
                LocatorLink.BoxSelectAll(this, Box);
        }
    }
    /**
     * Event handler. Called when focus leaves a box. 
     * If the user alters the text and then just leaves the text box, re-reads from current row and sets the value of the box.
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDef} FieldDef The {@link tp.LocatorFieldDef} descriptor
     */
    Box_Leave(LocatorLink, Box, FieldDef) {
        if (this.Active && !this.Assigning && (FieldDef instanceof tp.LocatorFieldDef)) {

            // when the user alters the text and then just leaves the text box
            //if (this.IsBoxMode)
            {
                let S = this.DataValueAsTextOf(FieldDef.DataField);
                let S2 = LocatorLink.BoxGetText(this, Box);
                //if (tp.IsBlank(S2)) this.ClearDataValue(); else
                if (S !== S2)
                    LocatorLink.BoxSetText(this, Box, S);
            }
        }
    }
    /**
     * Event handler. Does nothing.
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDef} FieldDef The {@link tp.LocatorFieldDef} descriptor
     * @param {KeyboardEvent} e The {@link KeyboardEvent} event object
     */
    Box_KeyDown(LocatorLink, Box, FieldDef, e) {
        if (this.Active && (FieldDef instanceof tp.LocatorFieldDef)) {
            if (e.keyCode === tp.Keys.Enter) {
                let S = LocatorLink.BoxGetText(this, Box);
                if (tp.IsBlankString(S)) this.ClearDataValue();

                else {
                    this.OnBoxKeyEvent(LocatorLin, Box, FieldDef);
                }
            }
        }
    }
    /**
     * Event handler.
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDef} FieldDef The {@link tp.LocatorFieldDef} descriptor
     * @param {KeyboardEvent} e The {@link KeyboardEvent} event object
     */
    Box_KeyPress(LocatorLink, Box, FieldDef, e) {
        if (this.Active && (FieldDef instanceof tp.LocatorFieldDef) && FieldDef.Searchable) {

            let KeyCode = 'charCode' in e ? e.charCode : e.keyCode;
            let c = String.fromCharCode(KeyCode);

            if (c === '*') {
                this.OnBoxKeyEvent(LocatorLin, Box, FieldDef);
            }
        }

    }

    OnBoxKeyEvent(LocatorLink, Box, FieldDef) {
        let WhereUser = '';
        let Text = LocatorLink.BoxGetText(this, Box);

        if (!tp.IsBlankString(Text)) {

            WhereUser = !tp.IsBlankString(FieldDef.TableName) ? `${FieldDef.TableName}.${FieldDef.Name}` : FieldDef.Name;

            if (FieldDef.DataType === tp.DataType.String)
                WhereUser += tp.Format(" like '{0}%' ", Text);
            else if (tp.DataType.IsDateTime(FieldDef.DataType))
                WhereUser += tp.Format(" = '{0}'", Text);
            else
                WhereUser += tp.Format(" = {0}", Text);
        }

        this.SelectListTableAsync(WhereUser)
            .then((RowCount) => {
                if (RowCount > 0) {
                    if (RowCount === 1) {
                        this.fListRow = this.ListTable.Rows[0];
                        let v = this.fListRow.Get(this.Descriptor.ListKeyField);
                        this.DataSource.Set(this.DataField, v); // this triggers the whole sequence of setting data-fields
                    } else {
                        this.DisplayListTable(Box);
                    }
                } else {
                    LocatorLink.BoxSetText(this, Box, '');
                }
            });
    }

    /**
   Returns the value of the FieldName field of the CurrentRow
   @private
   @param {string} FieldName The field name
   @returns {any} Returns the value of the FieldName field of the CurrentRow
   */
    DataValueOf(FieldName) {
        if (!tp.IsEmpty(this.CurrentRow) && this.CurrentRow.Table.ContainsColumn(FieldName))
            return this.CurrentRow.Get(FieldName);
        return null;
    }
    /**
    Returns the value of the FieldName field of the CurrentRow as string
    @private
    @param {string} FieldName The field name
    @returns {string} Returns the value of the FieldName field of the CurrentRow as string
    */
    DataValueAsTextOf(FieldName) {
        if (!tp.IsEmpty(this.CurrentRow) && this.CurrentRow.Table.ContainsColumn(FieldName)) {
            let Column = this.CurrentRow.Table.FindColumn(FieldName);
            if (!tp.IsEmpty(Column)) {
                let v = this.CurrentRow.Get(Column);
                return Column.Format(v, this.IsGridMode);
            }
        }

        return '';
    }

    /* IDataSourceListener implementation */
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    */
    DataSourceRowCreated(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    */
    async DataSourceRowAdded(Table, Row) {

        if (!this.fInDataSourceEvent && this.Active && this.DataSource.Position >= 0) {
            this.fInDataSourceEvent = true;
            try {
                await this.SetKeyValue(this.DataValue);
            }
            finally {
                this.fInDataSourceEvent = false;
            }

            this.OnDataValueChanged();
        }
    }
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    async DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        if (!this.Assigning && this.Active && Row === this.CurrentRow && Column === this.DataColumn) {

            await this.SetKeyValue(this.DataValue);

            // grid may have no idea that the row has changed after the above assignment
            if (this.IsGridMode) {
                this.Control.Render();
            }

            this.OnDataValueChanged();
        }
    }
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    */
    DataSourceRowRemoved(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {number} Position The new position
    */
    async DataSourcePositionChanged(Table, Row, Position) {
        if (this.IsBoxMode && !this.Initialized) {
            this.Initialize();
        }

        if (!this.fInDataSourceEvent && this.Active && Position >= 0) {
            //let S = this.IsGridMode ? "Grid" : "LocatorBox";
            this.fInDataSourceEvent = true;
            try {
                await this.SetKeyValue(this.DataValue);
            }
            finally {
                this.fInDataSourceEvent = false;
            }

            this.OnDataValueChanged();
        }
    }

    /**
    Notification
    */
    DataSourceSorted() {
    }
    /**
    Notification
    */
    DataSourceFiltered() {
    }
    /**
    Notification
    */
    DataSourceUpdated() {
    }

};
//#endregion

//#region tp.LocatorBox
/**
A multi-TextBox Locator control. <br />
It is used in data entry UIs representing the value of a single DataColumn and displaying multiple boxes. <br />
@implements {tp.ILocatorLink}
 */
tp.LocatorBox = class extends tp.Control {

    /**
    Constructor
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* properties */
    /**
    Gets or sets the value this control represents
    @type {any}
    */
    get Value() {
        return this.fValue;
    }
    set Value(v) {
        this.fValue = v;
        this.WriteDataValue();
    }

    /**
    Returns the locator
    @type {tp.Locator}
    */
    get Locator() {
        if (tp.IsEmpty(this.fLocator)) {
            this.fLocator = new tp.Locator();
            this.fLocator.Control = this;
        }
        return this.fLocator;
    }
    /**
    Gets or sets the locator descriptor name
    @type {string}
    */
    get LocatorName() {
        return this.Locator.Descriptor.Name;
    }
    set LocatorName(v) {
        if (v !== this.LocatorName) {
            if (!tp.IsBlank(v)) {
                tp.Locators.GetDefAsync(v)
                    .then((Des) => {
                        this.LocatorDescriptor = Des;
                    });
            }
        }
    }
    /**
    Gets or sets the locator descriptor.
    @type {tp.LocatorDef}
    */
    get LocatorDescriptor() {
        return this.Locator.Descriptor;
    }
    set LocatorDescriptor(v) {
        if (v !== this.LocatorDescriptor && v instanceof tp.LocatorDef) {
            this.Locator.Descriptor = v;
            this.Bind();
            if (this.IsDataBound === true) {
                this.OnBindCompleted();
            }
        }
    }

    /**
    Returns true if this instance is bound to a DataSource
    @type {boolean}
    */
    get IsDataBound() {

        var Result = false;

        if (!tp.IsEmpty(this.DataSource)) {
            if (tp.ControlBindMode.Simple === this.DataBindMode) {
                Result = !tp.IsBlank(this.DataField)
                    && !tp.IsBlank(this.DataValueProperty)
                    && !tp.IsEmpty(this.DataColumn)
                    && this.IsValidLocator;
            }
        }

        return Result;
    }
    /**
    Returns true if the locator is valid
    @type {boolean}
    */
    get IsValidLocator() {
        return this.Locator.Descriptor.Name !== tp.NO_NAME
            && this.Locator.Descriptor.Fields.length > 0;
    }

    /**
    Shows or hides the list button
    @type {boolean}
    */
    get ButtonListVisible() {
        return this.btnList instanceof HTMLElement ? this.btnList.style.display === '' : false;
    }
    set ButtonListVisible(v) {
        if (this.btnList instanceof HTMLElement)
            this.btnList.style.display = v === true ? '' : 'none';
    }
    /**
    Shows or hides the zoom button
    @type {boolean}
    */
    get ButtonZoomVisible() {
        return this.btnZoom instanceof HTMLElement ? this.btnZoom.style.display === '' : false;
    }
    set ButtonZoomVisible(v) {
        if (this.btnZoom instanceof HTMLElement)
            this.btnZoom.style.display = v === true ? '' : 'none';
    }

    /* private */

    /** Private
     * @private
     * */
    ClearBoxes() {
        for (let i = 0, ln = this.fBoxes.length; i < ln; i++) {
            tp.Remove(this.fBoxes[i]);
        }

        if (!tp.IsEmpty(this.btnList)) {
            tp.Remove(this.btnList);
            tp.Remove(this.btnZoom);
        }

        this.fBoxes.length = 0;

        this.fWidths.Clear();
    }
    /** Private
     * @private
     * */
    RecreateBoxes() {

        this.fCreating = true;
        try {

            this.ClearBoxes();

            let ReadOnly;
            let Box;        // HTMLInputElement;
            let FieldDes;   // tp.LocatorFieldDef;

            if (this.IsValidLocator) {

                for (let i = 0, ln = this.Locator.Descriptor.Fields.length; i < ln; i++) {
                    FieldDes = this.Locator.Descriptor.Fields[i];

                    if (FieldDes.Visible) {
                        tp.LocatorBox.BoxCount++;

                        Box = this.Handle.ownerDocument.createElement('input');
                        Box.type = 'text';
                        this.Handle.appendChild(Box);
                        this.fBoxes.push(Box);
                        tp.SetObject(Box, FieldDes);

                        Box.tabIndex = 0;
                        ReadOnly = this.Locator.Descriptor.ReadOnly || (FieldDes.Searchable === false);
                        tp.ReadOnly(Box, ReadOnly);
                        Box.title = tp.Format("{0} - ({1}.{2} -> {3})) - [Locator: {4}]", FieldDes.Title, FieldDes.TableName, FieldDes.Name, this.DataField, this.Locator.Descriptor.Name);

                        this.fWidths.Set(Box, FieldDes.Width > 0 ? FieldDes.Width : tp.LocatorFieldDef.BoxDefaultWidth);

                        //tp.On(Box, tp.Events.Focus, this.FuncBind(this.Box_Enter));
                        //tp.On(Box, tp.Events.LostFocus, this.FuncBind(this.Box_Leave));
                        //tp.On(Box, tp.Events.KeyDown, this.FuncBind(this.Box_KeyDown));
                        //tp.On(Box, tp.Events.KeyPress, this.FuncBind(this.Box_KeyPress));

                        tp.On(Box, tp.Events.Focus, (e) => { return this.Box_Enter(e); });
                        tp.On(Box, tp.Events.LostFocus, (e) => { return this.Box_Leave(e); });
                        tp.On(Box, tp.Events.KeyDown, (e) => { return this.Box_KeyDown(e); });
                        tp.On(Box, tp.Events.KeyPress, (e) => { return this.Box_KeyPress(e); });

                        this.Locator.Controls.Set(FieldDes, Box);
                    }
                }

            } else {
                for (let i = 0, ln = 3; i < ln; i++) {
                    Box = this.Handle.ownerDocument.createElement('input');
                    Box.type = 'text';
                    this.Handle.appendChild(Box);
                    tp.ReadOnly(Box, true);
                    this.fBoxes.push(Box);

                    this.fWidths.Set(Box, tp.LocatorFieldDef.BoxDefaultWidth);
                }
            }


            if (tp.IsEmpty(this.btnList)) {
                this.btnList = this.Handle.ownerDocument.createElement('div');
                this.btnList.className = tp.Classes.Btn;
                this.btnList.tabIndex = 0;
                this.btnList.innerHTML = '&dtrif;';

                this.btnZoom = this.Handle.ownerDocument.createElement('div');
                this.btnZoom.className = tp.Classes.Btn;
                this.btnZoom.tabIndex = 0;
                this.btnZoom.innerHTML = tp.IcoChars.Find;

                //this.btnList.addEventListener('click', this.FuncBind(this.AnyButton_Click));
                //this.btnZoom.addEventListener('click', this.FuncBind(this.AnyButton_Click));

                this.btnList.addEventListener('click', (e) => { return this.AnyButton_Click(e); });
                this.btnZoom.addEventListener('click', (e) => { return this.AnyButton_Click(e); });
            }

            this.Handle.appendChild(this.btnList);
            this.Handle.appendChild(this.btnZoom);

            // order
            let Counter = 1;
            for (let i = 0, ln = this.fBoxes.length; i < ln; i++) {
                Box = this.fBoxes[i];
                Box.style.order = Counter.toString();
                Counter++;
            }

            this.btnList.style.order = Counter.toString();
            Counter++;
            this.btnZoom.style.order = Counter.toString();

        } finally {
            this.fCreating = false;
        }

        this.ApplyReadOnlyAndEnabled();
        this.Layout();
    }
    /** Private
     * @private
     * */
    ApplyReadOnlyAndEnabled() {
        let ReadOnly;

        let DescriptorReadOnly = this.IsValidLocator ? this.Locator.Descriptor.ReadOnly : true;
        let Enabled = this.ReadOnly === false && this.Enabled === true && DescriptorReadOnly === false;

        tp.Enabled(this.btnList, Enabled);
        tp.Enabled(this.btnZoom, Enabled);

        let FieldDef;   // LocatorFieldDef;
        let Box;        // HTMLInputElement;
        for (let i = 0, ln = this.fBoxes.length; i < ln; i++) {
            Box = this.fBoxes[i];

            FieldDef = tp.GetObject(Box);
            if (!tp.IsEmpty(FieldDef)) {
                ReadOnly = this.ReadOnly === true || !Enabled || FieldDef.Searchable === false;
            } else {
                ReadOnly = this.ReadOnly === true || !Enabled;
            }

            tp.ReadOnly(Box, ReadOnly);
        }
    }
    /** Private
     * @private
     * */
    Layout() {
        if (!this.fLayouting && !this.fCreating) {

            this.fLayouting = true;
            try {

                let ButtonWidth = 0;
                let R; // tp.Rect

                /* button bounds */
                if (!tp.IsEmpty(this.ParentHandle)) {

                    if (this.ButtonZoomVisible) {
                        R = tp.BoundingRect(this.btnZoom);
                        ButtonWidth += R.Width;
                    }

                    if (this.ButtonListVisible) {
                        R = tp.BoundingRect(this.btnList);
                        ButtonWidth += R.Width;
                    }

                    if (ButtonWidth > 0)
                        ButtonWidth += 3;

                    let W = tp.BoundingRect(this.Handle).Width - ButtonWidth;
                    let Box; // HTMLInputElement;
                    let BoxWidth;
                    if (W > 0) {

                        for (let i = 0, ln = this.fBoxes.length; i < ln; i++) {
                            Box = this.fBoxes[i];

                            if (W <= 0) {
                                tp.Visible(Box, false);
                            } else {
                                if (i === this.fBoxes.length - 1) {
                                    BoxWidth = W;
                                    W = 0;
                                } else {
                                    BoxWidth = this.fWidths.Get(Box);

                                    if (W - BoxWidth >= 0) {
                                        W -= BoxWidth;
                                    } else {
                                        BoxWidth = W;
                                        W = 0;
                                    }
                                }


                                Box.style.width = tp.px(BoxWidth);
                                tp.Visible(Box, true);
                            }
                        }
                    } else {
                        for (let i = 0, ln = this.fBoxes.length; i < ln; i++) {
                            Box = this.fBoxes[i];
                            tp.Visible(Box, false);
                        }
                    }


                }

            } finally {
                this.fLayouting = false;
            }
        }
    }
    /** Event handler
     * @private
     * @param {Event} e The {@link Event} object.
     * */
    AnyButton_Click(e) {
        if (!this.ReadOnly && this.Enabled && this.IsValidLocator) {
            if (e.target === this.btnZoom) {
                // TODO: Zoom
            } else if (e.target === this.btnList) {
                this.Locator.ShowList(this.Handle);
            }
        }
    }
    /** Event handler
     * @private
     * @param {Event} e The {@link Event} object.
     * */
    Box_Enter(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDef = tp.GetObject(textBox); // tp.LocatorFieldDef            
            this.Locator.Box_Enter(this, textBox, FieldDef);
        }
    }
    /** Event handler
    * @private
    * @param {Event} e The {@link Event} object.
    * */
    Box_Leave(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDef = tp.GetObject(textBox); // tp.LocatorFieldDef
            this.Locator.Box_Leave(this, textBox, FieldDef);
        }
    }
    /** Event handler
    * @private
    * @param {KeyboardEvent} e The {@link KeyboardEvent} object.
    * */
    Box_KeyDown(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDef = tp.GetObject(textBox); // tp.LocatorFieldDef
            this.Locator.Box_KeyDown(this, textBox, FieldDef, e);
        }
    }
    /** Event handler
    * @private
    * @param {KeyboardEvent} e The {@link KeyboardEvent} object.
    * */
    Box_KeyPress(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDef = tp.GetObject(textBox); // tp.LocatorFieldDef
            this.Locator.Box_KeyPress(this, textBox, FieldDef, e);
        }
    }

    /* ILocatorLink implementation */
    /**
    Sets the Text to Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    @param {string} Text The text to set
    */
    BoxSetText(Locator, Box, Text) {
        Box.value = Text;
    }
    /**
    Returns the Text of the Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    @returns {string} Returns the Text of the Box
    */
    BoxGetText(Locator, Box) {
        return Box.value;
    }
    /**
    Selects all text in the Box
    @param {tp.Locator} Locator The {@link tp.Locator}
    @param {HTMLInputElement} Box The {@link HTMLInputElement} text box
    */
    BoxSelectAll(Locator, Box) {
        Box.select();
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.LocatorBox';
        this.fDefaultCssClasses = [tp.Classes.LocatorBox];

        // data-bind
        this.fDataBindMode = tp.ControlBindMode.Simple;
        this.fDataValueProperty = 'Value';
    }
    /**
    Initializes fields and properties just before applying the create params.
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
    }
    /**
    Notification <br />
    Initialization steps:
    <ul>
       <li>Handle creation</li>
       <li>Field initialization</li>
       <li>Option processing</li>
       <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnHandleCreated() {

        this.fEnabled = true;
        this.fValue = null;
        this.fBoxes = [];
        this.fWidths = new tp.Dictionary(); // Key = HTMLInputElement, Value = number
        this.fCreating = false;
        this.fLayouting = false;

        this.IsElementResizeListener = true;
        this.RecreateBoxes();

        this.ButtonZoomVisible = false;
        this.Layout();

        super.OnHandleCreated();
    }
    /**
    Event trigger
    @param {object} ResizeInfo An object of type <code>{Width: boolean, Height: boolean}</code>
    @protected
    @override
    */
    OnResized(ResizeInfo) {
        this.Layout();
        super.OnResized(ResizeInfo);
    }
    /**
    Binds the control to its DataSource. It is called after the DataSource property is assigned. <br />
    @protected
    @override
    */
    Bind() {
        if (this.IsDataBound) {
            this.RecreateBoxes();
            this.Locator.DataField = this.DataField;
            this.Locator.Initialize();
            this.ReadDataValue();
        }
    }
    /**
    Reads the value from data-source and assigns the control's data value property
    */
    ReadDataValue() {
        if (this.ReadingDataValue === true || this.WritingDataValue === true)
            return;

        if (this.IsDataBound && this.DataSource.Position >= 0) {
            this.ReadingDataValue = true;
            try {
                this.ReadingDataValue = true;
                try {
                    let v = this.DataSource.Get(this.DataField);
                    this[this.DataValueProperty] = v;
                } finally {
                    this.ReadingDataValue = false;
                }
            } finally {
                this.ReadingDataValue = false;
            }

        }
    }

    /**
    Event trigger
    @protected
    @override
    */
    OnParentChanged() {
        if (!tp.IsEmpty(this.ParentHandle))
            this.Layout();
        super.OnParentChanged();
    }
    /**
    Event trigger
    @protected
    @override
    */
    OnEnabledChanged() {
        this.ApplyReadOnlyAndEnabled();
        super.OnEnabledChanged();
    }
};


/** Field
 * @static
 * @private
 * @type {number}
 */
tp.LocatorBox.BoxCount = 0;

/** Field
 * @private
 * @type {any}
 */
tp.LocatorBox.prototype.fValue = null;
/** Field
 * @private
 * @type {tp.Locator}
 */
tp.LocatorBox.prototype.fLocator = null;
/** Field
 * @private
 * @type {tp.LocatorDef}
 */
tp.LocatorBox.prototype.fLocatorDescriptor = null;
/** Field
 * @private
 * @type {HTMLInputElement[]}
 */
tp.LocatorBox.prototype.fBoxes = [];
/** Field. A {tp.Dictionary} with Key = HTMLInputElement, Value = number.
 * @private
 * @type {tp.Dictionary}
 */
tp.LocatorBox.prototype.fWidths = null;
/** Field
 * @private
 * @type {HTMLElement}
 */
tp.LocatorBox.prototype.btnList = null;
/** Field
 * @private
 * @type {HTMLElement}
 */
tp.LocatorBox.prototype.btnZoom = null;
/** Field
 * @private
 * @type {boolean}
 */
tp.LocatorBox.prototype.fCreating = false;
/** Field
 * @private
 * @type {boolean}
 */
tp.LocatorBox.prototype.fLayouting = false;

//#endregion


//---------------------------------------------------------------------------------------
// tp.SelectSql Ui and filtering
//---------------------------------------------------------------------------------------


//#region tp.SelectSqlListUi
/** Handles the combo-box with the available {@link tp.SelectSql} items for the user to choose one, define filters and execute it. <br />
 * Displays a panel list where each {@link tp.SelectSql} item has its own panel displaying its filter controls. <br />
 * A {@link tp.SelectSql} contains a list of {@link tp.SqlFilterDef} descriptor items. <br />
 * A filter descriptor is used in creating a filter item. <br />
 * A control link associates a Ui control and a filter item.
* */
tp.SelectSqlListUi = class extends tp.tpElement {

    /**
    Constructor <br />
    The passed-in element is DIV where to build the Filters panel Ui. <br />
    The CreateParams passed-in MUST contain a property named SelectSqlList,
    which is a list of {@link tp.SelectSql} items to display. The first item must be named 'Main' is it is non-editable and non-deletable.
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }


    /** The currently selected {@link tp.SqlFilterPanel} panel object.
     * @type {tp.SqlFilterPanel}
     */
    get SelectedFilterPanel() {
        let elOption = this.cboSelectSqlList.SelectedItem;
        return elOption.PanelInfo.Panel;
    }
    /** The currently selected {@link tp.SelectSql}
     * @type {tp.SelectSql}
     */
    get SelectedSelectSql() {
        let elOption = this.cboSelectSqlList.SelectedItem;
        return elOption.PanelInfo.SelectSql;
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SelectSqlListUi';
        this.fDefaultCssClasses = [tp.Classes.SelectSqlListUi];
    }


    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing 
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() {
        let InnerHTML =
            `<div class="top-zone">
    <div class="select-list-container">
    </div>
    <div class="ToolBar ${tp.Classes.SelectSqlListToolBar}">
        <a class="ButtonEx" data-setup="{ Command: 'Execute', Text: 'Execute', ToolTip: 'Execute' , IcoClasses: 'fa fa-bolt',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'ClearFilter', Text: 'Clear', ToolTip: 'Clear Filter' , IcoClasses: 'fa fa-trash-o',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'RowLimit', Text: 'Row Limit', ToolTip: 'Row Limit' , IcoClasses: 'fa fa-compress',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'ShowSql', Text: 'Show Sql', ToolTip: 'Show Sql' , IcoClasses: 'fa fa-file-text-o',  NoText: true, Ico: 'Left'}"></a>
    </div>
</div>
<div class="tp-PanelList">
</div>`;

        tp.Html(this.Handle, InnerHTML);

        let el, CP;


        // combo-box with SelectSql items
        el = tp.Select(this.Handle, '.select-list-container');
        CP = {
            Parent: el,
            Width: 240
        };

        this.cboSelectSqlList = new tp.HtmlComboBox(null, CP);

        // SelectSqlList comes from CreateParams.
        this.SelectSqlList.forEach(item => {
            let elOption = this.cboSelectSqlList.Add(item.Name, '');

            elOption.PanelInfo = {
                SelectSql: item,
                Panel: null
            }
        });

        this.cboSelectSqlList.On('SelectedIndexChanged', this.cboSelectSqlList_SelectedIndexChanged, this);

        // ToolBar
        el = tp.Select(this.Handle, '.ToolBar');
        this.ToolBar = new tp.ToolBar(el);
        this.ToolBar.On('ButtonClick', this.AnyClick, this);

        this.ToggleUseRowLimit();

        // PanelList
        el = tp.Select(this.Handle, '.tp-PanelList');
        this.PanelList = new tp.PanelList(el);


        this.cboSelectSqlList.OnSelectedIndexChanged();

    }


    /* Event triggers */

    /**
    Event handler. If a Command exists in the clicked element then the Args.Command is assigned.
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    async AnyClick(Args) {
        if (Args.Handled !== true) {
            /** @type {tp.SelectSql} */
            let SelectSql;
            let SqlText;

            var Command = tp.GetCommand(Args);
            if (!tp.IsBlank(Command)) {
                switch (Command) {

                    case 'Execute':
                        SelectSql = this.GenerateSql();
                        SqlText = SelectSql.Text;


                        Args.Handled = true;
                        this.OnExecute();
                        break;

                    case 'ShowSql':
                        SelectSql = this.GenerateSql();
                        SqlText = SelectSql.Text;
                        await tp.InfoBoxAsync(SqlText);
                        Args.Handled = true;
                        break;

                    case 'ClearFilter':
                        this.SelectedFilterPanel.ClearControls();
                        Args.Handled = true;
                        break;

                    case 'RowLimit':
                        this.ToggleUseRowLimit();
                        Args.Handled = true;
                        break;

                }
            }
        }
    }
    /**
    Event handler. 
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    cboSelectSqlList_SelectedIndexChanged(Args) {
        Args.Handled = true;
        let elOption = Args.Sender.SelectedItem;

        // create panels on-demand only
        let PanelInfo = elOption.PanelInfo;

        if (!tp.IsValid(PanelInfo.Panel)) {
            let el = this.PanelList.AddChild();
            let CP = {
                SelectSql: PanelInfo.SelectSql,
                FilterDefs: PanelInfo.SelectSql.Filters
            }
            PanelInfo.Panel = new tp.SqlFilterPanel(el, CP);
        }
 
        this.PanelList.SelectedPanel = PanelInfo.Panel.Handle;
    }

    /** 
     *  Toggles the 'UseRowLimit' flag 
     *  */
    ToggleUseRowLimit() {
        let Btn = this.ToolBar.FindItemByCommand('RowLimit');
        tp.ToggleClass(Btn.Handle, tp.Classes.Active);
        this.UseRowLimit = !this.UseRowLimit;
    }

    /** Returns a {@link tp.SelectSql} instance along with user defined WHERE, if any.
     * @returns {tp.SelectSql} Returns a {@link tp.SelectSql} instance along with user defined WHERE, if any.
     * */
    GenerateSql() {
        let Result = this.SelectedSelectSql;
        let Conditions = this.SelectedFilterPanel.GenerateSql();
        Result.Where = Conditions.Where;
        Result.Having = Conditions.Having;
        return Result;
    }

    /**
    Event trigger
    */
    OnExecute() { this.Trigger('Execute', {}); }
};

/** The DIV where to build the Filters panel Ui.
 * @type {HTMLElement}
 */
tp.SelectSqlListUi.prototype.elPanel = null;
/** A list of {@link tp.SelectSql} items to display. The first item must be named 'Main' is it is non-editable and non-deletable.
 * NOTE: Comes from CreateParams.
 * @type {tp.SelectSql[]}
 */
tp.SelectSqlListUi.prototype.SelectSqlList = [];
/** ToolBar
 * @type {tp.ToolBar}
 */
tp.SelectSqlListUi.prototype.ToolBar = null;
/** ComboBox displaying the select statements
 * @type {tp.HtmlComboBox}
 */
tp.SelectSqlListUi.prototype.cboSelectSqlList = null;
/** A panel list. Each panel corresponds to single {@link tp.SelectSql} instance.
 * @type {tp.PanelList}
 */
tp.SelectSqlListUi.prototype.PanelList = null;
 
/** When true a row limit is applied to the current Sql statement.
 * @type {boolean}
 */
tp.SelectSqlListUi.prototype.UseRowLimit = false;
//#endregion

//#region tp.SqlFilterConditions
/** A helper class for constructing WHERE and HAVING Sql clauses */
tp.SqlFilterConditions = class {

    /** Constructor */
    constructor() {
        this.Where = '';
        this.Having = '';
    }

    /** The WHERE clause
     @type {string}
     */
    Where = '';
    /** The HAVING clause
     @type {string}
     */
    Having = '';
    /** The informational text. Infotext is a user friendly version of the generated Where.
     * @type {string}
     */
    InfoText = '';

    /** Adds a condition text to this instance Where.
     * @param {string} Text The condition to be added to Where.
     */
    AddToWhereSql(Text) {
        if (tp.IsEmpty(this.Where) || !tp.IsString(this.Where))
            this.Where = '';

        if (tp.IsString(Text) && !tp.IsBlank(Text)) {
            this.Where = tp.IsBlank(this.Where) ? Text : `${this.Where} and ${Text}`;
        }
    }
    /** Adds a condition text to this instance Having.
     * @param {string} Text The condition to be added to Having.
     */
    AddToHavingSql(Text) {
        if (tp.IsEmpty(this.Having) || !tp.IsString(this.Having))
            this.Having = '';

        if (tp.IsString(Text) && !tp.IsBlank(Text)) {
            this.Having = tp.IsBlank(this.Having) ? Text : `${this.Having} and ${Text}`;
        }
    }
};
//#endregion

//#region tp.SqlFilterPanel
/** The Ui, actually a DIV, built upon a list of {@link tp.SqlFilterDef} filters. <br />
 * This DIV displays a control for each filter item defined in the passed in {@link tp.SqlFilterDef} filters. <br />
 * The CreateParams passed in the constructor MUST contain an array property named FilterDefs of type {@link tp.SqlFilterDef}.
 * NOTE: A {@link tp.SqlFilterDef} describes a single entry (search field condition) of a WHERE clause of a SELECT sql statement.
 * */
tp.SqlFilterPanel = class extends tp.tpElement {
    /**
    Constructor <br />
    The passed-in HTML element is a DIV where to build the controls of the filter list. <br />
    The CreateParams passed-in MUST contain an array property named FilterDefs of type {@link tp.SqlFilterDef}.
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SqlFilterPanel';
        this.fDefaultCssClasses = [tp.Classes.SqlFilterPanel];
    }

    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing 
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    */
    OnInitializationCompleted() {
        this.CreateControls();
    }

    /**
     * Creates filter controls. <br />
     * It actually creates a {@link tp.SqlFilterControlLink} for each filter, and the link creates the needed controls.
     * */
    CreateControls() {
        if (!tp.IsValid(this.FilterValueList)) {
            this.ControlLinks = [];
            this.FilterValueList = new tp.SqlFilterValueList(this);

            if (tp.IsArray(this.FilterValueList.Items)) {
                this.FilterValueList.Items.forEach(item => {
                    let Link = new tp.SqlFilterControlLink(this, item);
                    this.ControlLinks.push(Link);
                    Link.CreateControls();
                });
            }
        }
    }
    /**
     * Clears filter controls.
     * */
    ClearControls() {
        if (tp.IsArray(this.ControlLinks) && this.ControlLinks.length > 0) {
            this.ControlLinks.forEach(item => {
                item.ClearControls();
            });
        }

        this.FilterValueList.ClearValues();
    }

    /** Returns a {@link tp.SqlFilterConditions} instance with user defined WHERE, if any.
     * @returns {tp.SqlFilterConditions} Returns a {@link tp.SqlFilterConditions} instance with user defined WHERE, if any.
     * */
    GenerateSql() {
        return this.FilterValueList.GenerateSql();
    }

};

/** A list of {@link tp.SqlFilterDef} items. A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
 * @type {tp.SqlFilterDef[]}
 */
tp.SqlFilterPanel.prototype.FilterDefs = [];
/** A {@link tp.SqlFilterValueList} 
 * @type {tp.SqlFilterValueList}
 */
tp.SqlFilterPanel.prototype.FilterValueList = null;
/** A list {@link tp.SqlFilterControlLink} control links.
 * @type {tp.SqlFilterControlLink[]}
 */
tp.SqlFilterPanel.prototype.ControlLinks = [];
//#endregion

//#region tp.SqlFilterValueList

/**
 * A list of {@link tp.SqlFilterValue} items. <br />
 * A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
 * */
tp.SqlFilterValueList = class {
    /**
     * Constructor
     * @param {tp.SqlFilterPanel} FilterPanel  A DIV that displays the filter controls.Provides a list of {@link tp.SqlFilterDef} items. A filter item is created based upon each {@link tp.SqlFilterDef} descriptor.
     */
    constructor(FilterPanel) {
        this.Items = [];
        this.FilterPanel = FilterPanel;

        if (tp.IsArray(this.FilterDefs)) {
            this.FilterDefs.forEach(item => {
                let Filter = new tp.SqlFilterValue(item);
                this.Items.push(Filter);
            });
        }
    }

    /** A DIV that displays the filter controls. Provides a list of {@link tp.SqlFilterDef} items.
     * @type {tp.SqlFilterPanel}
     */
    FilterPanel = null;

    /** A list of {@link tp.SqlFilterDef} items. A filter item is created based upon each {@link tp.SqlFilterDef} descriptor.
     * @type {tp.SqlFilterDef[]}
     */
    get FilterDefs() {
        return this.FilterPanel.FilterDefs;
    }
    /** A list of {@link tp.SqlFilterValue} items.
     * @type {tp.SqlFilterValue[]}
     */
    Items = [];


    /** 
     * Clears all the values items.
     * */
    ClearValues() {
        this.Items.forEach(item => { item.Clear(); });
    }
    /** Returns a {@link tp.SqlFilterConditions} instance with user defined WHERE, if any.
    * @returns {tp.SqlFilterConditions} Returns a {@link tp.SqlFilterConditions} with user defined WHERE, if any.
    * */
    GenerateSql() {
        this.Items.forEach(item => {
            item.ControlLink.InputFromControls();
        });

        let Result = new tp.SqlFilterConditions();

        this.DoGenerateSql(Result, true);
        this.DoGenerateSql(Result, false);

        return Result;
    }

    /**
     * Private method. Prepares a clause (UserWhere or Having).
     * @param {tp.SqlFilterConditions} Ref An object containing Where, Having and InfoText properties.
     * @param {boolean} IsWhere A boolean value indicating what property of the 'Ref' value to use as clause.
     */
    DoGenerateSql(Ref, IsWhere) {

        let sClause = IsWhere === true ? Ref.Where : Ref.Having;

        /** @type {tp.SqlFilterValue[]} */
        let List = [];

        // get the right items
        this.Items.forEach(item => {
            if (IsWhere === true) {
                if (item.FilterDef.PutInHaving !== true)
                    List.push(item);
            }
            else {
                if (item.FilterDef.PutInHaving == true)
                    List.push(item);
            }
        });

        let sInfoText, S, PrefixConcat;
        List.forEach(item => {
            sInfoText = '';
            S = item.GenerateSql();

            PrefixConcat = sClause.length > 0
                || (sClause.length === 0 && List.indexOf(item) === 0 && IsWhere === true && !tp.IsBlank(Ref.Where));


            if (S.length > 0) {

                switch (item.WhereOperator) {
                    case tp.WhereOperator.And:
                        if (PrefixConcat) {
                            sClause = sClause.length == 0 ? ` and ${S} ` : `${sClause} ${tp.LB}  and ${S} `;
                            sInfoText += `and ${S}`;
                        }
                        else {
                            sClause = `      ${S}  `;
                            sInfoText += S;
                        }
                        break;

                    case tp.WhereOperator.Not:
                        if (PrefixConcat) {
                            sClause = sClause.length == 0 ? ` and not ${S} ` : `${sClause} ${tp.LB}  and not ${S} `;
                            sInfoText += `and not ${S}`;
                        }
                        else {
                            sClause = `      not ${S}  `;
                            sInfoText += `not ${S}`;
                        }
                        break;

                    case tp.WhereOperator.Or:
                        if (PrefixConcat) {
                            sClause = sClause.length == 0 ? ` or ${S} ` : `${sClause} ${tp.LB}  or ${S} `;
                            sInfoText += `or ${S}`;
                        }
                        else {
                            sClause = `      ${S}  `;
                            sInfoText += S;
                        }
                        break;
                }

                if (IsWhere === true)
                    Ref.Where = sClause;
                else
                    Ref.Having = sClause;

                if (sInfoText.length > 0)
                    Ref.InfoText += `${item.FilterDef.Title} : ${sInfoText}  ${tp.LB}`;

            }


        });

    }
};
//#endregion

//#region tp.SqlFilterValue
/** A Sql Filter value item. <br />
 * Represents a single data column that participates in a WHERE clause. <br />
 * It is actually an Sql generator, regarding that data column.  <br />
 * The data column can be of a string, integer, float, datetime or boolean-integer type.
 * An instance of this class uses a {@link tp.SqlFilterControlLink} instance to get control values.
 * */
tp.SqlFilterValue = class {
    /**
     * Constructor
     * @param {tp.SqlFilterDef} FilterDef The filter descriptor this filter represents.
     */
    constructor(FilterDef) {
        this.FilterDef = FilterDef;
        this.SelectedOptionsList = [];
    }


    /** The filter descriptor this filter represents.
     * @type {tp.SqlFilterDef}
     */
    FilterDef = null;

    /** The value of the filter
     * @type {string}
     */
    Value = '';
    /** The "greater or equal" value as text.
     * @type {string}
     */
    Greater = '';
    /** The "less or equal" value as text.
     * @type {string}
     */
    Less = '';
    /** The boolean value used. One of the {@link tp.TriState} constants. Default means no value at all.
     * @type {number}
     */
    BoolValue = tp.TriState.Default;
    /** The date range. One of the {@link tp.DateRange} constants.
     * @type {number}
     */
    DateRange = tp.DateRange.Custom;
    /** The list of the selected options. 
     * It is used when the "data type" is "EnumQuery" or "EnumConst", when a list or a grid is presented to the user in order to make multiple choises.
     * @type {string[]}
     */
    SelectedOptionsList = [];
    /** The where operator used (and, not, or). One of the {@link tp.WhereOperator} constants.
     * @type {}
     */
    WhereOperator = tp.WhereOperator.And;

    /** Links this filter and a filter control.
     * @type {tp.SqlFilterControlLink}
     */
    ControlLink = null;

    /** 
     *  Resets the values of the properties of this instance.
     *  */
    Clear() {
        this.Value = '';
        this.Greater = '';
        this.Less = '';
        this.BoolValue = tp.TriState.Default;
        this.DateRange = tp.DateRange.Custom;
        this.SelectedOptionsList = [];
        this.WhereOperator = tp.WhereOperator.And;
    }
    /** Generates and returns the sql text for the data column this instance represents.
     * @returns {string} Returns the sql text for the data column this instance represents.
     * */
    GenerateSql() {
        if (tp.IsEmpty(this.FilterDef))
            return '';

        let Result = '';
        let DataType = this.FilterDef.DataType;
        //let QuotedDateTimes = true;

        let FullName = this.FilterDef.FieldPath;
        //let FromName = FullName + tp.FromField;
        //let ToName = FullName + tp.ToField;

        if (this.FilterDef.PutInHaving === true && !tp.IsBlank(this.FilterDef.AggregateFunc))
            FullName = ` ${this.FilterDef.AggregateFunc}(${FullName}) `;


        // WHERE clause generation
        let S = '';
        if (this.FilterDef.Mode == tp.SqlFilterMode.Simple) {

            // no range
            if (this.FilterDef.UseRange !== true && (DataType === tp.DataType.String || tp.DataType.IsNumber(DataType))) {

                if (DataType === tp.DataType.String && !tp.IsBlank(this.Value)) {
                    S = tp.Sql.NormalizeMaskForce(this.Value);
                    S = S.trim();
                    Result = ` (${FullName} ${S}) `;
                }
                else if (tp.DataType.IsNumber(DataType) && !tp.IsBlank(this.Value)) {
                    S = ' = ' + this.Value.trim();
                    Result = ` (${FullName}${S}) `;
                }
            }
            // range
            else if (tp.DataType.IsDateTime(DataType) || (this.FilterDef.UseRange === true && (DataType === tp.DataType.String || tp.DataType.IsNumber(DataType)))) {

                let sG = '';
                let sL = '';

                // range, for all types except Date
                if (!tp.IsBlank(this.Greater)) {
                    switch (DataType) {
                        case tp.DataType.Integer: sG = this.Greater; break;
                        case tp.DataType.Float:
                        case tp.DataType.Decimal: sG = this.Greater.replace(',', '.'); break;
                        case tp.DataType.String: sG = `'${this.Greater}'`; break;
                    }
                }

                if (!tp.IsBlank(this.Less)) {
                    switch (DataType) {
                        case tp.DataType.Integer: sL = this.Less; break;
                        case tp.DataType.Float:
                        case tp.DataType.Decimal: sL = this.Less.replace(',', '.'); break;
                        case tp.DataType.String: sL = `'${this.Less}'`; break;
                    }
                }

                // range for Dates (Date is always a range)
                if (tp.DataType.IsDateTime(DataType)) {

                    let FromDate = null;
                    let ToDate = null;

                    if (this.DateRange !== tp.DateRange.Custom) {
                        let o = tp.DateRanges.ToDates(this.DateRange, tp.Today());
                        if (o.Result === true) {
                            FromDate = o.FromDate;
                            ToDate = o.ToDate;
                        }
                    }
                    else {

                        if (!tp.IsBlank(this.Greater)) {
                            let o = tp.TryParseDateTime(this.Greater);
                            if (o.Result === true) {
                                FromDate = o.Value;
                            }
                        }

                        if (!tp.IsBlank(this.Less)) {
                            let o = tp.TryParseDateTime(this.Less);
                            if (o.Result === true) {
                                ToDate = o.Value;
                            }
                        }
                    }


                    if (tp.IsDate(FromDate)) {
                        FromDate = tp.StartOfDay(FromDate);
                        sG = tp.FormatDateTime(FromDate, 'yyyy-MM-dd HH:mm:ss'); // ISO date
                        sG = `'${sG}'`;
                    }

                    if (tp.IsDate(ToDate)) {
                        ToDate = tp.EndOfDay(ToDate);
                        sL = tp.FormatDateTime(ToDate, 'yyyy-MM-dd HH:mm:ss'); // ISO date
                        sL = `'${sL}'`;
                    }

                }

                if (!tp.IsBlank(sG))
                    sG = `(${FullName} >= ${sG})`

                if (!tp.IsBlank(sL))
                    sL = `(${FullName} <= ${sL})`;

                if (!tp.IsBlank(sG) && !tp.IsBlank(sL))
                    Result = `(${sG} and ${sL})`;
                else if (!tp.IsBlank(sG))
                    Result = sG;
                else if (!tp.IsBlank(sL))
                    Result = sL;

            }
            // boolean
            else if (DataType === tp.DataType.Boolean) {
                if (this.BoolValue !== tp.TriState.Default) {
                    S = this.BoolValue == tp.TriState.True ? '= 1' : '<> 1';        // assumes that boolean values are actually integer values  
                    Result = ` (${FullName} ${S}) `;
                }
            }
        }
        else if (tp.Bf.In(this.FilterDef.Mode, tp.SqlFilterMode.EnumConst | tp.SqlFilterMode.EnumQuery) && (DataType === tp.DataType.String || DataType === tp.DataType.Integer)) {

            S = '';

            if (tp.IsArray(this.SelectedOptionsList) && this.SelectedOptionsList.length > 0) {
                if (this.SelectedOptionsList.length === 1) {
                    S = DataType === tp.DataType.String ? `'${this.SelectedOptionsList[0]}'` : this.SelectedOptionsList[0];
                    if (!tp.IsBlank(S))
                        Result = ` (${FullName} = ${S}) `;
                }
                else {
                    this.SelectedOptionsList.forEach(item => {
                        S += DataType === tp.DataType.String ? `'${item}', ` : `${item}, `;
                    });

                    S = tp.RemoveLastComma(S);
                    if (!tp.IsBlank(S))
                        Result = ` (${FullName} in (${S})) `;
                }
            }
        }

        return Result;
    }

};
//#endregion

//#region tp.SqlFilterControlLink

/** Represents a link between a {@link tp.SqlFilterValue} and a control on a Ui.
 * */
tp.SqlFilterControlLink = class {
    /**
     * Constructor
     * @param {tp.SqlFilterPanel} FilterPanel  FilterPanel  A DIV that displays the filter controls.Provides a list of {@link tp.SqlFilterDef} items. A filter item is created based upon each {@link tp.SqlFilterDef} descriptor.
     * @param {tp.SqlFilterValue} FilterValue A {@link tp.SqlFilterValue} filter item, representing a single data column that participates in a WHERE clause.
     */
    constructor(FilterPanel, FilterValue) {
        this.FilterPanel = FilterPanel;
        this.FilterValue = FilterValue;
        this.FilterValue.ControlLink = this;
    }

    /** FilterPanel  A DIV that displays the filter controls.Provides a list of {@link tp.SqlFilterDef} items. A filter item is created based upon each {@link tp.SqlFilterDef} descriptor.
     * @type {tp.SqlFilterPanel}
     */
    FilterPanel = null;
    /** The {@link tp.SqlFilterValue} filter item, representing a single data column that participates in a WHERE clause. 
     * This is where values of a control link are placed.
     * @type {tp.SqlFilterValue}
     */
    FilterValue = null;
    /** The filter descriptor this link is associated to.
     * @type {tp.SqlFilterDef}
     */
    get FilterDef() {
        return this.FilterValue.FilterDef;
    }


    /** The tripous script class type used in creating the filter controls (edtBox, edtFrom and edtTo) of this filter
     * @type {object}
     */
    ControlClassType = null;

    /** The DOM element of the whole filter row
     * @type {HTMLElement}
     * */
    elFilterRow = null;
    /** The DOM element containing the filter control
     * @type {HTMLElement}
     * */
    elControlContainer = null;

    /** Container of the 'from' and 'to' containers
     * @type {HTMLDivElement}
     */
    elRangeContainer = null;
    /** Container of the 'from' control
     * @type {HTMLDivElement}
     */
    elFromControlContainer = null;
    /** Container of the 'to' control
     * @type {HTMLDivElement}
     */
    elToControlContainer = null;

    elDateRangeSelectorContainer = null;

    /** Label displaying the title of the filter, i.e. the field name.  
     * @type {HTMLDivElement}
     */
    lblTitle = null;
    /** Label displaying 'From'. Use when filter range is in use.
     * @type {HTMLDivElement}
     */
    lblFrom = null;
    /** Label displaying 'To'. Use when filter range is in use.
     * @type {HTMLDivElement}
     */
    lblTo = null;

    lblDateRange = null;

    /** The main input control of this filter. Used when UseRange of the filter def is false. 
     * @type {tp.tpElement}
     */
    edtBox = null;
    /** The 'from' input control of this filter. Used when UseRange of the filter def is true.
     * @type {tp.tpElement}
     */
    edtFrom = null;
    /** The 'to' input control of this filter. Used when UseRange of the filter def is true.
     * @type {tp.tpElement}
     */
    edtTo = null;
    /** A combo-box displaying  
     * @type {tp.HtmlComboBox}
     */
    cboDateRange = null;

    /** The data-table of an enum filter
     * @type {tp.DataTable}
     */
    tblEnum = null;
    /** A {@link tp.Grid} used with filters of type enum
     * @type {tp.Grid}
     */
    gridEnum = null;
    /** Internal. Used when this is an single-choise enum filter, in order to get notified when a row is selected and act accordingly.
     * @type {tp.DataSourceListener}
     */
    gridEnumDatasourceListener = null;
    /** Internal flag. Used when this is an single-choise enum filter, in order to prevent multiple choises
     * @type {boolean}
     */
    SettingSingleChoise = false;

    /**
     * Creates any needed control.
     * */
    CreateControls() {

        this.CreateFilterRow();

        switch (this.FilterDef.Mode) {

            case tp.SqlFilterMode.Simple:
                switch (this.FilterDef.DataType) {
                    case tp.DataType.String:
                        this.ControlClassType = tp.Ui.Types.TextBox
                        break;
                    case tp.DataType.Integer:
                    case tp.DataType.Float:
                    case tp.DataType.Decimal:
                        this.ControlClassType = tp.Ui.Types.NumberBox
                        break;
                    case tp.DataType.Date:
                    case tp.DataType.DateTime:
                        this.ControlClassType = tp.Ui.Types.DateBox;
                        break;
                    case tp.DataType.Boolean:
                        this.ControlClassType = tp.Ui.Types.HtmlComboBox;
                        break;
                }
                this.CreateControl_Simple();
                break;

            case tp.SqlFilterMode.EnumQuery:
            case tp.SqlFilterMode.EnumConst:
                this.CreateControl_Enum();
                break;
        }
    }
    /**
     * Creates the filter row, actually a DIV.
     * */
    CreateFilterRow() {

        /*
        <div class="tp-SqlFilterRow">
            <div class="tp-Text"></div>
            <div class="tp-SqlFilter-DateRangeSelector">
                <div class="tp-Text"></div>
                @* DateRange combo-box here *@
            </div>
            <div class="tp-SqlFilter-Ctrl">
                @* one or two controls here *@
            </div>
        </div>
         */
        let HtmlText =
`<div class="${tp.Classes.SqlFilterRow}">
    <div class="${tp.Classes.Text}">${this.FilterDef.Title}</div>    
</div>`;

        this.elFilterRow = tp.Append(this.FilterPanel.Handle, HtmlText);
        this.lblTitle = tp.Select(this.elFilterRow, '.' + tp.Classes.Text);

        // date-range selector
        if (tp.DataType.IsDateTime(this.FilterDef.DataType)) {
            let DateRangeSelectorHtmlText =
                `<div class="${tp.Classes.SqlFilterDateRangeSelector}">
</div>`;

            this.elDateRangeSelectorContainer = tp.Append(this.elFilterRow, DateRangeSelectorHtmlText);
        }


        // control container
        let ControlContainerHtmlText =
            `<div class="${tp.Classes.SqlFilterCtrl}"></div>`;

        this.elControlContainer = tp.Append(this.elFilterRow, ControlContainerHtmlText);
    }
    /**
     * Creates controls when this represents a simple filter.
     * */
    CreateControl_Simple() {
        let i, ln;

        let UseRange = this.FilterDef.UseRange === true || tp.DataType.IsDateTime(this.FilterDef.DataType); // this.FilterDef.DataType === tp.DataType.DateTime || this.FilterDef.DataType === tp.DataType.Date;

        let IsFloatingPoint = this.FilterDef.DataType === tp.DataType.Float || this.FilterDef.DataType === tp.DataType.Decimal;

        if (!UseRange) {
            this.edtBox = new this.ControlClassType(null, { Parent: this.elControlContainer });

            // Box is a a combo-box
            if (this.FilterDef.DataType === tp.DataType.Boolean) {
                this.edtBox.Add('', tp.TriState.Default);
                this.edtBox.Add('False', tp.TriState.False);
                this.edtBox.Add('True', tp.TriState.True);
            }

            if (IsFloatingPoint)
                this.edtBox.Decimals = 2;
        }
        else {

            if (tp.DataType.IsDateTime(this.FilterDef.DataType)) {

                this.lblDateRange = tp.Append(this.elDateRangeSelectorContainer, `<div class="${tp.Classes.Text}">Range</div>`);
                this.cboDateRange = new tp.HtmlComboBox(null, { Parent: this.elDateRangeSelectorContainer });

                for (i = 0, ln = tp.DateRanges.WhereRanges.length; i < ln; i++) {
                    this.cboDateRange.Add(tp.DateRanges.WhereRangesTexts[i], tp.DateRanges.WhereRanges[i]);
                }

                this.cboDateRange.SelectedIndex = 0;

                this.cboDateRange.On('SelectedIndexChanged', this.cboDateRange_SelectedIndexChanged, this);
            }

            this.elFromControlContainer = tp.Append(this.elControlContainer, `<div class="${tp.Classes.From}"></div>`);
            this.lblFrom = tp.Append(this.elFromControlContainer, `<div>From</div>`);
            this.edtFrom = new this.ControlClassType(null, { Parent: this.elFromControlContainer });
            if (IsFloatingPoint)
                this.edtFrom.Decimals = 2;

            this.elToControlContainer = tp.Append(this.elControlContainer, `<div class="${tp.Classes.To}"></div>`);
            this.lblTo = tp.Append(this.elToControlContainer, `<div>To</div>`);
            this.edtTo = new this.ControlClassType(null, { Parent: this.elToControlContainer });
            if (IsFloatingPoint)
                this.edtTo.Decimals = 2;
        }
    }
    /**
     * Creates controls when this represents an enum filter (EnumQuery or EnumConst).
     * */
    async CreateControl_Enum() {

        let TableColumn, GridColumn;
        if (this.FilterDef.Mode === tp.SqlFilterMode.EnumQuery) {
            this.tblEnum = await tp.Db.SelectAsync(this.FilterDef.Enum.Sql);
        }
        else {
            this.FilterDef.Enum.ResultField = this.FilterDef.Enum.ResultField || 'Result';

            this.tblEnum = new tp.DataTable('tblEnum');
            this.tblEnum.AddColumn(this.FilterDef.Enum.ResultField, this.FilterDef.Enum.DataType, 140);

            this.FilterDef.Enum.OptionList.forEach(item => {
                this.tblEnum.AddRow([item]);
            });
        }

        if (!this.tblEnum.ContainsColumn('Include')) {
            TableColumn = this.tblEnum.AddColumn('Include', tp.DataType.Boolean, 0, 0);
            TableColumn.Title = '+/-';
        }

        if (this.FilterDef.Enum.IsMultiChoise === true && this.FilterDef.Enum.IncludeAll === true) {
            this.tblEnum.Rows.forEach(Row => {
                Row.Set('Include', true);
            });
        }

        this.gridEnum = new tp.Grid(null, { Parent: this.elControlContainer });
        this.gridEnum.ReadOnly = false;
        this.gridEnum.ToolBarVisible = false;
        this.gridEnum.GroupsVisible = false;
        this.gridEnum.FilterVisible = false;
        this.gridEnum.FooterVisible = false;
        this.gridEnum.ButtonInsertVisible = false;
        this.gridEnum.ButtonDeleteVisible = false;

        this.gridEnum.DataSource = this.tblEnum;

        GridColumn = this.gridEnum.GetColumn('Include');
        GridColumn.Width = 36;
        GridColumn.Text = '+/-';
        GridColumn.ToolTip = 'Include';

        this.gridEnum.SetColumnListWritable(['Include']);

        // when not multi-choise, ensure only a single row is selected
        if (this.FilterDef.Enum.IsMultiChoise !== true) {
            this.gridEnumDatasourceListener = new tp.DataSourceListener();
            this.gridEnum.DataSource.AddDataListener(this.gridEnumDatasourceListener);
            this.gridEnumDatasourceListener.DataSourceRowModified = (Table, Row, Column, OldValue, NewValue) => { this.gridEnum_DataSourceRowModified(Table, Row, Column, OldValue, NewValue); };
        }
    }


    /**
    Notification
    @param {tp.DataTable} Table The table
    @param {tp.DataRow} Row The row
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    gridEnum_DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        let Flag = false;
        if (tp.IsSameText('Include', Column.Name)) {

            // when not multi-choise, ensure only a single row is selected
            if (this.SettingSingleChoise === false) {
                this.SettingSingleChoise = true;
                try {
                    let Index = Row.Table.IndexOfColumn(Column);
                    let v = Row.Get(Index);
                    let v2;
                    if (v === true) {
                        this.tblEnum.Rows.forEach((Row2) => {
                            if (Row !== Row2) {
                                v2 = Row2.Get(Index);
                                if (v2 === true) {
                                    Row2.Set(Index, false);
                                    Flag = true;
                                }
                            }
                        });
                    }
                } finally {
                    this.SettingSingleChoise = false;
                }
            }

        }

        if (Flag)
            this.gridEnum.RepaintRows();
    }
    /**
     * Event handler. Handles the SelectedIndexChanged event of the Associate.
     * @protected
     * @param {tp.EventArgs} Args A tp.EventArgs object.
     */
    cboDateRange_SelectedIndexChanged(Args) {
        let v = tp.StrToInt(this.cboDateRange.SelectedValue);
        let IsCustom = v === tp.DateRange.Custom;

        if (IsCustom) {
            this.edtFrom.Text = '';
            this.edtTo.Text = '';
        } else {
            let RangeResult = tp.DateRanges.ToDates(v);
            if (RangeResult.Result === true) {
                this.edtFrom.Date = RangeResult.FromDate;
                this.edtTo.Date = RangeResult.ToDate;
            }
        }

        this.edtFrom.Enabled = IsCustom;
        this.edtTo.Enabled = IsCustom;

        this.edtFrom.ReadOnly = !IsCustom;
        this.edtTo.ReadOnly = !IsCustom;
    }

    /**
     * Reads filter controls and assigns values to the filter value of this instance.
     * */
    InputFromControls() {

        // -------------------------------------------------------------------------------------
        let GetControlValue = (Control) => {
            let Result = null;

            if (this.FilterDef.Mode === tp.SqlFilterMode.Simple) {

                if (this.FilterDef.DataType !== tp.DataType.Boolean) {
                    Result = Control.Text;
                    Result = tp.IsString(Result) ? Result.trim() : '';
                    if (Result !== '') {
                        if (tp.Bf.In(this.FilterDef.DataType, tp.DataType.Float | tp.DataType.Decimal)) {
                            Result = Result.replace(',', '.');
                        }
                    }
                }
                else {
                    Result = Control.SelectedValue; // edtBox is a tp.HtmlComboBox
                }
            }
            else {
                Result = [];

                let ResultField = this.FilterDef.Enum.ResultField;
                if (tp.IsString(ResultField) && !tp.IsBlank(ResultField) && this.tblEnum.ContainsColumn(ResultField)) {
                    let v;
                    this.tblEnum.Rows.forEach(Row => {
                        if (Row.Get('Include', false) === true) {
                            v = Row.Get(ResultField, null);
                            if (tp.IsValid(v))
                                Result.push(!tp.IsString(v) ? v.toString() : v);
                        }
                    });
                }

            }

            return Result;
        };
        // -------------------------------------------------------------------------------------


        switch (this.FilterDef.Mode) {

            case tp.SqlFilterMode.Simple:
                switch (this.FilterDef.DataType) {
                    case tp.DataType.String:
                    case tp.DataType.Integer:
                    case tp.DataType.Float:
                    case tp.DataType.Decimal:
                        if (this.FilterDef.UseRange !== true) {
                            this.FilterValue.Value = GetControlValue(this.edtBox);
                        }
                        else {
                            this.FilterValue.Greater = GetControlValue(this.edtFrom);
                            this.FilterValue.Less = GetControlValue(this.edtTo);
                        }
                        break;

                    case tp.DataType.Date:
                    case tp.DataType.DateTime:
                        this.FilterValue.Greater = GetControlValue(this.edtFrom);
                        this.FilterValue.Less = GetControlValue(this.edtTo);
                        break;

                    case tp.DataType.Boolean:
                        let S = GetControlValue(this.edtBox);
                        if (!tp.IsBlank(S)) {
                            this.FilterValue.BoolValue = tp.StrToInt(S);
                        }
                        break;
                }
                break;

            case tp.SqlFilterMode.EnumQuery:
            case tp.SqlFilterMode.EnumConst:
                this.FilterValue.SelectedOptionsList = GetControlValue(null);
                break;
        }
    }
    /**
    * Clears any controls used with this link.
    * */
    ClearControls() {
        if (tp.IsValid(this.edtBox)) {
            if (this.FilterDef.DataType === tp.DataType.Boolean) {
                this.edtBox.SelectedIndex = 0;
            }

            else {
                this.edtBox.Text = '';
            }

        }

        if (tp.DataType.IsDateTime(this.FilterDef.DataType)) {
            this.cboDateRange.SelectedIndex = 0;
        }

        if (tp.IsValid(this.edtFrom))
            this.edtFrom.Text = '';

        if (tp.IsValid(this.edtTo))
            this.edtTo.Text = '';

        if (tp.IsValid(this.tblEnum)) {
            let Flag = this.FilterDef.Enum.IsMultiChoise === true && this.FilterDef.Enum.IncludeAll === true;
            this.tblEnum.Rows.forEach(Row => {
                Row.Set('Include', Flag);
            });

            this.gridEnum.RepaintRows();
        }
    }

};



//#endregion


//---------------------------------------------------------------------------------------
// view
//---------------------------------------------------------------------------------------

//#region tp.DataViewMode

/**
Standard modes/commands for data views.
*/
tp.DataViewMode = {
    None: 0,
    Home: 1,

    List: 2,
    Filters: 4,
    Reports: 8,

    First: 0x10,
    Prior: 0x20,
    Next: 0x40,
    Last: 0x80,

    Insert: 0x100,
    Edit: 0x200,
    Delete: 0x400,

    Save: 0x800,
    Cancel: 0x1000,

    Close: 0x2000
};
Object.freeze(tp.DataViewMode);

tp.DataViewCommandNames = [];
(function () {
    for (var PropName in tp.DataViewMode) {
        if (tp.IsInteger(tp.DataViewMode[PropName])) {
            tp.DataViewCommandNames.push(PropName);
        }
    }
})();
//#endregion

//#region tp.View




/**
A view represents a control container that automatically can create its controls from the provided markup. <br />
Example markup
<pre>
    <div class="tp-View" data-setup="{ TypeName: 'View', .... }">
        ...
    </div>
</pre>
@implements {tp.IBroadcasterListener}
*/
tp.View = class extends tp.tpElement {

    /**
    Constructor <br />
    Example markup
    <pre>
        <div class="tp-View" data-setup="{ TypeName: 'View', .... }">
            ...
        </div>
    </pre>
    @param {string|HTMLElement} [ElementOrSelector] - Optional.
    @param {Object} [CreateParams] - Optional.
    */
    constructor(ElementOrSelector, CreateParams) {
        super(ElementOrSelector, CreateParams);
    }
 
    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.View';
        this.fDefaultCssClasses = [tp.Classes.View];
    }
    /**
    Initializes fields and properties just before applying the create params.     
    @protected
    @override
    */
    InitializeFields() {
        super.InitializeFields();
        this.ViewName = tp.NextName('View');
        this.DataSources = [];
    }
    /**
    Notification 
    Initialization steps:
    - Handle creation
    - Field initialization
    - Option processing
    - Completed notification
    @protected
    @override
    */
    OnHandleCreated() {
        super.OnHandleCreated();
        this.IsScreenResizeListener = true;
    }
    /**
    Notification. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation, AFTER field initialization
    and AFTER options (CreateParams) processing <br />
    Initialization steps:
    <ul>
        <li>Handle creation</li>
        <li>Field initialization</li>
        <li>Option processing</li>
        <li>Completed notification</li>
    </ul>
    @protected
    @override
    */
    OnInitializationCompleted() {
        super.OnInitializationCompleted();

        tp.Broadcaster.Add(this);
        this.HookEvent(tp.Events.Click);
    }
    /**
    * This should be called OUTSIDE of any constructor, after the constructor finishes. <br />
    * 
    * WARNING
    * 
    * In javascript fields contained in a derived class declaration, are initialized after the base constructor finishes.
    * So, if the base constructor calls a virtual method where the derived class initializes a field,
    * when the constructor finishes, the field is assigned again whatever value the class declaration dictates.
    *
    * A = class {
    *     constructor() {
    *         this.SetFields();
    *     }
    *
    *     SetFields() { }
    * }
    *
    * B = class extends A {
    *     constructor() {
    *         super();
    *         this.Display();
    *     }
    *
    *     Field2 = 123;	// this is assigned again after the SetFields() call, when the constructor completes.
    *
    *     SetFields() {
    *         super.SetFields();
    *         this.Field2 = 987;
    *     }
    *     Display() {
    *         alert(this.Field2);
    *     }
    * }
    *
    * The result is that the field retains the value it had in the class declaration, the null value.
    *
    * @protected
    * @override
    * */
    OnAfterConstruction() {
        this.AutocreateControls();
        this.InitializeView();
    }

    /**
    Destroys the handle (element) of this instance by removing it from DOM and releases any other resources.
    @protected
    @override
    */
    DoDispose() {
        tp.Broadcaster.Remove(this);
        super.DoDispose();
    }
    /**
    Handles any DOM event
    @protected
    @override
    @param {Event} e The {@link Event} object    
    */
    OnAnyDOMEvent(e) {
        var Type = tp.Events.ToTripous(e.type);

        switch (Type) {
            case tp.Events.Click:
                this.OnAnyClick(e);
                break;
        }
    }

    /* overridables */
    /** If the AutocreateControls flag is true then it automatically creates controls from the markup.
     * Else it just calls the CreateControls() method, so inheritor classes create any needed control.
     * @protected
     * */
    AutocreateControls() {
        if (this.CreateParams.AutocreateControls === true) {
            let List = this.GetControls();
            if (List.length === 0)
                tp.CreateContainerControls(this.Handle);
        }
        else {
            this.CreateControls();
        }
    }
    /**
    Creates the controls of the view based on the provided markup.
    @protected
    */
    CreateControls() {
    }

    /** This is called after the construction is completed, and outside of construction sequence. <br />
     * @protected
    */
    InitializeView() { 
    }
    /** Closes and disposes the view
    * */
    CloseView() {
        this.Dispose();
    }


    /**
    Event trigger
    @protected
    @param {Event} e The {@link Event} object
    */
    OnAnyClick(e) {
        let Args = new tp.EventArgs('Click', this, e);
        Args.Command = tp.GetCommand(Args);
        this.AnyClick(Args);
        this.Trigger('Click', Args);
    }
    /**
    Just for the inheritors. If a Command exists in the clicked element then the Args.Command is assigned.
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    AnyClick(Args) {
    }
    /** 
    Called by tp.Broadcaster to notify a listener about an event.
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    @returns {any} Returns a value or null.
    */
    BroadcasterFunc(Args) {
        return null;
    }
 
    /* public */
    /**
    Returns a string representation of this instance.
    @returns {string} Returns a string representation of this instance.
    */
    toString() {
        return this.ViewName;
    }

    /* controls */
    /**
    Returns the controls. <br />
    Returns an array with all {@link tp.tpElement} objects existing on direct or nested child DOM elements, of the handle of this instance.
    @returns {tp.tpElement[]}  Returns an array with all {@link tp.tpElement} objects existing on direct or nested child DOM elements, of the handle of this instance.
    */
    GetControls() {
        return tp.GetScriptObjects(this.Handle);
    }
    /**
    Returns a control belonging to this instance and bound to a specified field, if any, else null
    @protected
    @param {string | tp.DataColumn} v - The field name or the {@link tp.Control} Column
    @returns {tp.Control} Returns the {@link tp.Control} control if found, or null.
    */
    FindControlByDataField(v) {
        /** @type {tp.Control} */
        let Control;  
        let List = this.GetControls();
        let FieldName = (v instanceof tp.DataColumn) ? v.Name : v;

        for (var i = 0, ln = List.length; i < ln; i++) {
            if (List[i] instanceof tp.Control) {
                Control = List[i];
                if (tp.IsSameText(FieldName, Control.DataField))
                    return Control;
            }
        }
        return null;
    }
    /**
    Sets or un-sets the read-only flag to this view controls
    @protected
    @param {boolean} Flag A flag
    */
    ApplyReadOnlyEditToControls(Flag) {
        /** @type {tp.Control} */
        let c;
        let List = this.GetControls();

        List.forEach((item) => {
            if (item instanceof tp.Control) {
                c = item;
                if ('DataColumn' in c && c.DataColumn instanceof tp.DataColumn && c.DataColumn.IsReadOnlyEdit === true) {
                    c.ReadOnly = Flag === true;
                }
            }
        }); 
    }


    /* data-binding */
    /**
    Finds and returns a {@link tp.DataSource} data-source by name, if any, else null. <br />
    @protected
    @param {string} SourceName The data-source by name
    @returns {tp.DataSource} Returns a {@link tp.DataSource} data-source or null
    */
    GetDataSource(SourceName) {
        let Result = null; 

        if (tp.IsArray(this.DataSources)) {
            Result = this.DataSources.find((item) => {
                return tp.IsSameText(SourceName, item.Name);
            });
        }

        return Result;        
    }
    /**
    Calls the Update() method of all data-sources of this view
    @protected
    */
    UpdateDataSources() {
        if (tp.IsArray(this.DataSources)) {
            this.DataSources.forEach((item) => { item.Update(); }); 
        } 
    }

    /**
     * Binds controls of the Edit part. <br />
     * NOTE:  Do NOT call this method directly. <br />
     * This method is called automatically the first time an an Insert() or Edit() is requested.
     * @param {tp.tpElement[]} ComponentList The list of controls to bind
     * @protected
     */
    BindControls(ComponentList) {
        /** @type {tp.Control} */
        let c;
        ComponentList.forEach((item) => {
            if (item instanceof tp.Control) {
                c = item;
                if (this.CanBindControl(c))
                    this.BindControl(c);
            }
        }); 
    }
    /**
    Returns true if a specified control can be bound to data
    @protected
    @param {tp.Control} Control A {@link tp.Control}
    @returns {boolean} Returns true if a specified control can be bound to data
    */
    CanBindControl(Control) {
        if (Control instanceof tp.Control && Control.DataBindMode !== tp.ControlBindMode.None /* && Control.IsDataBound !== true */) {
            switch (Control.DataBindMode) {
                case tp.ControlBindMode.Simple:
                    return tp.IsString(Control.DataField) && !tp.IsBlank(Control.DataField);
                case tp.ControlBindMode.List:
                    return tp.IsString(Control.DataField) && !tp.IsBlank(Control.DataField);
                case tp.ControlBindMode.Grid:
                    return tp.IsString(Control.TableName) && !tp.IsBlank(Control.TableName) && !tp.HasClass(Control.Handle, tp.Classes.Grid);
            }
        }

        return false;
    }
    /**
    Binds a specified control to data
    @protected
    @param {tp.Control} Control A {@link tp.Control}
    */
    BindControl(Control) {

        let DataSource = this.GetDataSource(Control.TableName);

        if (tp.IsValid(DataSource)) {
            Control.DataSource = DataSource;

            if ((Control.DataBindMode === tp.ControlBindMode.List) && tp.IsEmpty(Control['ListSource']) && !tp.IsEmpty(Control.DataColumn)) {
                 if (tp.IsString(Control['ListSourceName']) && !tp.IsBlank(Control['ListSourceName'])) {
                    let ListSource = this.GetDataSource(Control['ListSourceName']);
                    if (tp.IsValid(ListSource))
                        Control['ListSource'] = ListSource;
                }
            }

            this.SetupControl(Control);
        }

    }
    /**
    Sets up a control after data-binding
    @protected
    @param {tp.Control} Control A {@link tp.Control}
    */
    SetupControl(Control) {

        if ((Control.DataBindMode === tp.ControlBindMode.Simple) || (Control.DataBindMode === tp.ControlBindMode.List)) {
            /** @type {tp.DataColumn} */
            let Column = Control.DataColumn;
            if (!tp.IsEmpty(Column)) {
                if ('ReadOnly' in Control) {
                    Control.ReadOnly = Column.ReadOnly || Column.IsReadOnly || Column.IsReadOnlyUI;
                }
                if ('MaxLength' in Control) {
                    if (Column.MaxLength !== -1) {
                        Control['MaxLength'] = Column.MaxLength;
                    }
                }
                if ((Column.IsRequired === true) && ('Required' in Control)) {
                    Control.Required = false;
                    Control.Required = true;
                }

                if (Control instanceof tp.CheckBox) {
                    Control.Text = Column.Title;
                } else if (tp.IsElement(Control.elText)) {
                    tp.val(Control.elText, Column.Title);
                }

                if (!Column.IsVisible) {
                    /** @type {HTMLElement} */
                    let CtrlRow = tp.Ui.GetCtrlRow(Control.Handle);
                    if (tp.IsHTMLElement(CtrlRow))
                        tp.Visible(CtrlRow, false);
                }
            }
        } else if (tp.IsValid(tp.Grid) && Control.DataBindMode === tp.ControlBindMode.Grid) {
            /** @type {tp.Grid} */
            let Grid = Control;
            /** @type {tp.DataTable} */
            let Table = Grid.DataSource.Table;

            let ReadOnlyColumns = [];
            let VisibleColumns = [];

            Table.Columns.forEach((Column) => {
                if (Column.IsVisible)
                    VisibleColumns.push(Column.Name);
                if (Column.IsReadOnly || Column.IsReadOnlyUI)
                    ReadOnlyColumns.push(Column.Name);
            }); 

            Grid.SetColumnListReadOnly(ReadOnlyColumns);
            Grid.SetColumnListVisible(VisibleColumns);
        }
    }

    

    /* static */
    /**
    Creates and returns a view. The class type of the view is controlled by the provided markup or the passed in CreateParams parameter.
    Example markup
    @example
    <pre>
        <div class="tp-View" data-setup="{ TypeName: 'View', .... }"> ... </div>
        or
        <div class="tp-View" data-setup="{ ClassType: tp.MyDataViewClass, .... }"> ... </div>
    </pre>
    @param {string | HTMLElement} ElementOrSelector - The html element of the view
    @param {object} [CreateParams] Optional. The create params object
    @returns {tp.View} Returns the newly created {@link tp.View} view.
    */
    static CreateView(ElementOrSelector, CreateParams) {

        // <div class="tp-View" data-setup="{ TypeName: 'View', .... }"></div>
        // or
        // <div class="tp-View" data-setup="{ ClassType: tp.MyDataViewClass, .... }"> </div>

        let TypeName = 'View';
        let Type = tp.View = null;
        let CP = null;
        let el = tp.Select(ElementOrSelector);

        if (el) {

            if (!tp.IsEmpty(CreateParams)) {
                CP = CreateParams;
            } else {
                CP = tp.Data(el, 'setup');
                if (!tp.IsBlank(CP))
                    CP = eval("(" + CP + ")");
            }

            if (!tp.IsEmpty(CP) && 'TypeName' in CP && !tp.IsBlank(CP['TypeName'])) {
                TypeName = CP['TypeName'];

                if (TypeName in tp.ViewTypes)
                    Type = tp.ViewTypes[TypeName];

            } else if (!tp.IsEmpty(CP) && 'ClassType' in CP && !tp.IsEmpty(CP['ClassType'])) {
                Type = CP.ClassType;
            }

            if (Type) {
                if (tp.IsString(Type)) {
                    let Code = `new ${Type}(el, CP)`;
                    return eval(Code);
                }
                else {
                    return new Type(el, CP);
                }
            }

        }

        return null;
    }
};
/** 
Gets or sets the name of the view 
@type {string}
*/
tp.View.prototype.ViewName = '[no view name]';
/**
The ajax packet sent by server app when this view is created, if any.
@type {object}
*/
tp.View.prototype.Packet = {};
/** Field
 @protected
 @type {tp.DataSource[]}
 */
tp.View.prototype.DataSources = [];



tp.ViewTypes = {
    View: tp.View
};



//#endregion

//---------------------------------------------------------------------------------------
// data dialog-boxes
//---------------------------------------------------------------------------------------

//#region tp.DataSetDialog
/**
A window for displaying a dataset.
*/
tp.DataSetDialog = class extends tp.tpWindow {

    /**
    Constructor
    @param {tp.WindowArgs} Args - Setup options
    */
    constructor(Args) {
        super(Args);
    }



    /* overrides */
    /**
    Override
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.DataSetDialog';
    }
    /**
    Override
    @protected
    @override
    */
    CreateControls() {
        super.CreateControls();

        var S, i, ln, j, jln, Column, SourceColumn, Row, SourceRow;

        this.DataSet = this.Args.DataSet;

        // text
        S = this.Args.Text;
        if (tp.IsBlank(S))
            S = this.DataSet.Name;
        if (tp.IsBlank(S))
            S = 'DataSet box';

        this.Text = S;


        this.ContentWrapper.StyleProp('display', 'flex');
        this.ContentWrapper.StyleProp('flex-direction', 'column');

        // tool-bar 
        this.ToolBar = new tp.ControlToolBar();
        this.ToolBar.Parent = this.ContentWrapper;
        this.ToolBar.AddClass(tp.Classes.ToolBar);
        //this.ToolBar.StyleProp('display', 'flex');
        //this.ToolBar.StyleProp('align-items', 'center');

        // tool-bar controls
        this.cboTables = new tp.ComboBox();
        this.ToolBar.AddItem(this.cboTables);
        this.cboTables.Width = 160;
        this.cboTables.ListOnly = true;
        this.cboTables.AddRange(this.DataSet.Tables);
        this.cboTables.SelectedIndex = 0;
        this.cboTables.On('SelectedIndexChanged', this.ComboTables_SelectedIndexChanged, this);

        // tool-bar buttons
        this.btnShowIdColumns = this.ToolBar.AddButton('ShowIdColumns', 'Show/Hide Id Columns', 'Show/Hide Id Columns');  
        this.ToolBar.On('ButtonClick', this.AnyClick, this);

        // footer buttons
        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK, false);

        // grid
        this.Grid = new tp.Grid();
        this.Grid.Parent = this.ContentWrapper;
        this.Args.Grid = this.Grid;

        this.Grid.StyleProp('flex-grow', '1');
        this.Grid.Width = '100%';

        this.Grid.ToolBarVisible = false;
        this.Grid.GroupsVisible = false;
        this.Grid.FilterVisible = false;
        this.Grid.FooterVisible = false;

        this.ChangeTable();
    }

    /* protected */
    /**
    Override
    @protected
    @override
    */
    ChangeTable() {
        var Table = this.cboTables.SelectedItem; // tp.DataTable
        if (tp.IsEmpty(Table))
            return;

        if (tp.IsEmpty(Table.BindingSource))
            Table.BindingSource = new tp.DataSource(Table);

        this.Grid.DataSource = Table.BindingSource;

        if (Table.Rows.length <= 1500)
            this.Grid.BestFitColumns();

        this.Grid.Focus();
    }

    /* event handlers */
    ComboTables_SelectedIndexChanged(Args) {
        this.ChangeTable();
    }
    /**
     * Event handler for buttons added with CreateFooterButton(). <br />
     * It does nothing here though. Inheritors should override this method and provide the functionality for their added buttons.
     * @param {tp.EventArgs} Args Event arguments
     */
    AnyClick(Args) {
        var Cmd = tp.GetCommand(Args);
        switch (Cmd) {
            case 'ShowIdColumns':
                this.Grid.ShowHideIdGridColumns();
                break;
            default:
                super.AnyClick(Args);
                break;
        }
    }
};

/* fields */
/** Field
 * @field
 * @type {tp.ControlToolBar}
 */
tp.DataSetDialog.prototype.ToolBar;
/** Field
 * @field
 * @type {tp.ComboBox}
 */
tp.DataSetDialog.prototype.cboTables;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.DataSetDialog.prototype.btnShowIdColumns;
/** Field
 * @field
 * @type {tp.DataSet}
 */
tp.DataSetDialog.prototype.DataSet;
/** Field
 * @field
 * @type {tp.Grid}
 */
tp.DataSetDialog.prototype.Grid;
//#endregion

//#region tp.MultiRowPickDialog
/**
A row pick list dialog with a grid that allows the user to check/select multiple rows.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
*/
tp.MultiRowPickDialog = class extends tp.tpWindow {

    /**
    Constructor
    @param {tp.WindowArgs} Args - Setup options
    */
    constructor(Args) {
        super(Args);
    }



    /* overrides */
    /**
    Override
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.MultiRowPickDialog';
    }
    /**
    Override
    @protected
    @override
    */
    CreateControls() {
        super.CreateControls();

        var i, ln, j, jln,
            Column,         // tp.DataColumn,
            SourceColumn,   // tp.DataColumn,
            Row,            // tp.DataRow,
            SourceRow;      // tp.DataRow;

        this.tblSource = this.Args.tblSource;
        this.SelectedRows = this.Args.SelectedRows || [];
        this.VisibleColumns = this.Args.VisibleColumns || [];

        this.ContentWrapper.StyleProp('display', 'flex');
        this.ContentWrapper.StyleProp('flex-direction', 'column');

        // tool-bar 
        this.ToolBar = new tp.ControlToolBar();
        this.ToolBar.Parent = this.ContentWrapper;
        this.ToolBar.AddClass(tp.Classes.ToolBar);

        // Command, Text, ToolTip, IcoClasses, CssClasses, ToRight
        this.btnIncludeAll = this.ToolBar.AddButton('IncludeAll', tp.IcoChars.Insert, 'Include All'); 
        this.btnExcludeAll = this.ToolBar.AddButton('ExcludeAll', tp.IcoChars.Delete, 'Exclude All'); 
        this.btnShowIdColumns = this.ToolBar.AddButton('ShowIdColumns', 'Show/Hide Id Columns', 'Show/Hide Id Columns');
        this.ToolBar.On('ButtonClick', this.AnyClick, this);

        // footer buttons
        this.btnOK = this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK, false);
        this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel, false);

        // table
        this.Table = new tp.DataTable('PickRows');
        Column = this.Table.AddColumn('Include', tp.DataType.Boolean);
        Column.Title = "+/-";

        for (i = 0, ln = this.tblSource.Columns.length; i < ln; i++) {
            SourceColumn = this.tblSource.Columns[i];
            Column = SourceColumn.Clone();
            this.Table.AddColumn(Column);
        }

        // copy the rows from tblSource to Table
        for (i = 0, ln = this.tblSource.Rows.length; i < ln; i++) {
            SourceRow = this.tblSource.Rows[i];
            if (SourceRow.State !== tp.DataRowState.Deleted) {
                Row = this.Table.AddEmptyRow();
                Row.Set('Include', this.SelectedRows.indexOf(SourceRow) !== -1);        // check (include) initially SelectedRows
                Row['SourceRow'] = SourceRow;

                for (j = 0, jln = this.tblSource.Columns.length; j < jln; j++) {
                    SourceColumn = this.tblSource.Columns[j];
                    Row.Set(SourceColumn.Name, SourceRow.Get(SourceColumn.Name));
                }
            }
        }


        this.btnOK.Enabled = this.Table.Rows.length > 0;
        this.btnIncludeAll.Enabled = this.btnOK.Enabled;
        this.btnExcludeAll.Enabled = this.btnOK.Enabled;

        // grid
        this.Grid = new tp.Grid();
        this.Grid.Parent = this.ContentWrapper;
        this.Args.Grid = this.Grid;

        this.Grid.StyleProp('flex-grow', '1');
        this.Grid.Width = '100%';

        this.Grid.ToolBarVisible = false;
        this.Grid.GroupsVisible = true;
        this.Grid.FilterVisible = true;
        this.Grid.FooterVisible = true;

        this.Grid.DataSource = new tp.DataSource(this.Table);

        let GridColumn; // tp.GridColumn;
        for (i = 0, ln = this.Grid.ColumnCount; i < ln; i++) {
            GridColumn = this.Grid.ColumnByIndex(i);
            if (!tp.IsSameText('Include', GridColumn.Name)) {
                GridColumn.ReadOnly = true;
                GridColumn.Visible = this.VisibleColumns.length === 0 ? true : tp.ListContainsText(this.VisibleColumns, GridColumn.Name);
            }
        }

        if (this.Table.Rows.length <= 1500)
            this.Grid.BestFitColumns();

        this.Grid.Focus();

        this.Grid.On(tp.Events.Click, this.GridClick, this);

    }
    /**
    Override
    @protected
    @param {boolean} Flag Controls the checking of the rows to include.
    */
    IncludeAll(Flag) {
        var i, ln,
            Row;    // tp.DataRow;

        this.Table.Batch = true;
        try {
            for (i = 0, ln = this.Table.Rows.length; i < ln; i++) {
                Row = this.Table.Rows[i];
                Row.Set(0, Flag);
            }
        } finally {
            this.Table.Batch = false;
        }

    }

    /* event handlers */
    /** Event handler
     * @protected
     * @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
     */
    GridClick(Args) {
        var Row = this.Grid.FocusedRow;
        if (!tp.IsEmpty(Row)) {
            var Flag = Row.Get('Include', false);
            Flag = !Flag;
            Row.Set('Include', Flag);
        }
    }
    /**
    Event handler for buttons added with CreateFooterButton(). 
    * @protected
    * @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    AnyClick(Args) {
        var i, ln,
            Row, // tp.DataRow,
            Cmd = tp.GetCommand(Args);

        switch (Cmd) {
            case 'OK':
                // gather selected rows
                this.SelectedRows.length = 0;
                for (i = 0, ln = this.Table.Rows.length; i < ln; i++) {
                    Row = this.Table.Rows[i];
                    if (Row.Get(0, false) === true) {
                        this.SelectedRows.push(Row['SourceRow']);
                    }
                }

                this.Args.SelectedRows = this.SelectedRows;

                this.DialogResult = tp.DialogResult.OK;

                break;
            case 'IncludeAll':
                this.IncludeAll(true);
                break;
            case 'ExcludeAll':
                this.IncludeAll(false);
                break;
            case 'ShowIdColumns':
                this.Grid.ShowHideIdGridColumns();
                break;
            default:
                super.AnyClick(Args);
                break;
        }
    }
};


/* fields */

/** Field
 * @field
 * @type {tp.ControlToolBar}
 */
tp.MultiRowPickDialog.prototype.ToolBar;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.MultiRowPickDialog.prototype.btnIncludeAll;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.MultiRowPickDialog.prototype.btnExcludeAll;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.MultiRowPickDialog.prototype.btnShowIdColumns;
/** Field
 * @field
 * @type {tp.tpElement}
 */
tp.MultiRowPickDialog.prototype.btnOK;
/** Field
 * @field
 * @type {tp.Grid}
 */
tp.MultiRowPickDialog.prototype.Grid;
/** Field
 * @field
 * @type {string[]}
 */
tp.MultiRowPickDialog.prototype.VisibleColumns;
/** Field
 * @field
 * @type {tp.DataRow[]}
 */
tp.MultiRowPickDialog.prototype.SelectedRows;
/** Field
 * @field
 * @type {tp.DataTable}
 */
tp.MultiRowPickDialog.prototype.Table;
/** Field
 * @field
 * @type {tp.DataTable}
 */
tp.MultiRowPickDialog.prototype.tblSource;
//#endregion

//#region tp.SingleRowPickDialog
/**
A row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
*/
tp.SingleRowPickDialog = class extends tp.tpWindow {

    /**
    Constructor
    @param {tp.WindowArgs} Args - Setup options
    */
    constructor(Args) {
        super(Args);
    }



    /* overrides */
    /**
    Override
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SingleRowPickDialog';
    }
    /**
    Override
    @protected
    @override
    */
    CreateControls() {
        super.CreateControls(); 

        this.Table = this.Args.Table;
        this.VisibleColumns = this.Args.VisibleColumns || [];
        this.SelectSql = this.Args.SelectSql;
        this.SqlText = this.Args.SqlText;
        this.KeyValue = this.Args.KeyValue;
        this.KeyFieldName = this.Args.KeyFieldName;

        this.ContentWrapper.StyleProp('display', 'flex');
        this.ContentWrapper.StyleProp('flex-direction', 'column');

        // tool-bar 
        this.ToolBar = new tp.ControlToolBar();
        this.ToolBar.Parent = this.ContentWrapper;
        this.ToolBar.AddClass(tp.Classes.ToolBar);

        // Command, Text, ToolTip, IcoClasses, CssClasses, ToRight
        this.btnShowIdColumns = this.ToolBar.AddButton('ShowIdColumns', 'Show/Hide Id Columns', 'Show/Hide Id Columns');
        this.ToolBar.On('ButtonClick', this.AnyClick, this);

        // footer buttons
        this.btnOK = this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK, false);
        this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel, false);

        this.Grid = new tp.Grid();
        this.Grid.Parent = this.ContentWrapper;
        this.Args.Grid = this.Grid;

        this.Grid.StyleProp('flex-grow', '1');
        this.Grid.Width = '100%';

        this.Grid.ReadOnly = true;
        this.Grid.ToolBarVisible = false;
        this.Grid.GroupsVisible = true;
        this.Grid.FilterVisible = true;
        this.Grid.FooterVisible = false;

        this.Grid.On(tp.Events.DoubleClick, this.DoubleClick, this);

/*
         if (!tp.IsEmpty(this.Table)) {
            this.SetGridDataSource(this.Table);
        } else if (!tp.IsBlank(this.SqlText)) {
            tp.ShowSpinner(true);
            tp.Db.Select(this.SqlText, '')
                .then((Table) => {
                    this.SetGridDataSource(Table);
                    tp.ShowSpinner(false);
                });
        } else if (!tp.IsEmpty(this.SelectSql)) {
            tp.ShowSpinner(true);
            tp.Db.Select(this.SelectSql.Text, this.SelectSql.ConnectionName)
                .then((Table) => {
                    this.SetGridDataSource(Table);
                    tp.ShowSpinner(false);
                });
        }
 */
        this.LoadDataAsync();

    }

    async LoadDataAsync() {
        let Table = null;
        if (!tp.IsEmpty(this.Table)) {
            this.SetGridDataSource(this.Table);
        } else if (!tp.IsBlank(this.SqlText)) {

            tp.ShowSpinner(true);
            try {
                Table = await tp.Db.SelectAsync(this.SqlText, '');
                this.SetGridDataSource(Table);
            }
            finally {
                tp.ForceHideSpinner();
            } 
                
        } else if (!tp.IsEmpty(this.SelectSql)) {

            tp.ShowSpinner(true);
            try {
                Table = await tp.Db.SelectAsync(this.SelectSql.Text, this.SelectSql.ConnectionName);
                this.SetGridDataSource(Table);
            } finally {
                tp.ForceHideSpinner();
            }           
                
        }
    }

    /* protected */
    /**
     * Sets a {@link tp.DataTable} as the datasource
     * @protected
     * @param {tp.DataTable} Table A {@link tp.DataTable} table to set as datasource
     */
    SetGridDataSource(Table) {

        this.btnOK.Enabled = Table.Rows.length > 0;

        // table setup
        if (!tp.IsEmpty(this.SelectSql)) {
            this.SelectSql.SetupTable(Table);
        }

        this.Grid.DataSource = new tp.DataSource(Table);

        // grid columns setup
        if (!this.SelectSql && this.VisibleColumns.length > 0) {
            this.Grid.SetColumnListVisible(this.VisibleColumns);
        }

        if (Table.Rows.length <= 1500)
            this.Grid.BestFitColumns();

        this.Grid.Focus();

        // set the initially selected row
        if (!tp.IsBlank(this.KeyFieldName)) {
            var Row = Table.FindRow(this.KeyFieldName, this.KeyValue);
            if (!tp.IsEmpty(Row)) {
                this.Grid.FocusedRow = Row;
            }
        }

    }
    /** Sets the selected row
     * @protected
     * */
    SetSelectedRow() {
        var Row = this.Grid.FocusedRow;
        if (!tp.IsEmpty(Row)) {
            this.SelectedRow = Row;
            this.Args.SelectedRow = Row;
            this.Table = Row.Table;
            this.DialogResult = tp.DialogResult.OK;
        }
    }

    /* event handlers */
    /** Event handler
     * @protected
     * @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
     */
    DoubleClick(Args) {
        this.SetSelectedRow();
    }
    /** Event handler for buttons added with CreateFooterButton().
    * @protected
    * @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    AnyClick(Args) {
 
        let Cmd = tp.GetCommand(Args);

        switch (Cmd) {
            case 'OK':
                this.SetSelectedRow(); // get selected row
                break;

            case 'ShowIdColumns':
                this.Grid.ShowHideIdGridColumns();
                break;
            default:
                super.AnyClick(Args);
                break;
        }
    }

};

/** Field
 * @field
 * @type {tp.ControlToolBar}
 */ 
tp.SingleRowPickDialog.prototype.ToolBar = null;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SingleRowPickDialog.prototype.btnShowIdColumns = false;
/** Field
 * @field
 * @type {tp.tpElement}
 */
tp.SingleRowPickDialog.prototype.btnOK = null;
/** Field
 * @field
 * @type {tp.Grid}
 */
tp.SingleRowPickDialog.prototype.Grid = null;
/** Field
 * @field
 * @type {string[]}
 */
tp.SingleRowPickDialog.prototype.VisibleColumns = [];
/** Field
 * @field
 * @type {tp.DataRow}
 */
tp.SingleRowPickDialog.prototype.SelectedRow = null;
/** Field
 * @field
 * @type {tp.DataTable}
 */
tp.SingleRowPickDialog.prototype.Table = null;
/** Field
 * @field
 * @type {tp.SelectSql}
 */
tp.SingleRowPickDialog.prototype.SelectSql = null;
/** Field
 * @field
 * @type {string}
 */ 
tp.SingleRowPickDialog.prototype.SqlText = '';
/** Field
 * @field
 * @type {any}
 */
tp.SingleRowPickDialog.prototype.KeyValue = null;
/** Field
 * @field
 * @type {string}
 */
tp.SingleRowPickDialog.prototype.KeyFieldName = '';

//#endregion

//#region tp.SqlFilterDialog

/** A dialog box that displays and executes {@link tp.SqlFilterDef} filters. */
tp.SqlFilterDialog = class extends tp.tpWindow {
    /**
    Constructor
    @param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object. Should contain a FilterDefs array property, of type {@link tp.SqlFilterDef},
    and an async call-back function which is passed the WHERE clause this dialog generates, executes a SELECT, and returns a {@link tp.DataTable}.
    */
    constructor(Args) {
        super(Args);
    }

    /* overrides */
    /**
    Override
    @protected
    @override
    */
    InitClass() {
        super.InitClass();

        this.tpClass = 'tp.SqlFilterDialog';
    }
    /**
    Override
    @protected
    @override
    */
    CreateControls() {
        super.CreateControls();

        this.FilterDefs = this.Args.FilterDefs;
        this.SelectFunc = this.Args.SelectFunc;

        if (!tp.IsArray(this.FilterDefs) || this.FilterDefs.length === 0)
            tp.Throw('FilterDefs not defined');

        if (!tp.IsFunction(this.SelectFunc))
            tp.Throw('SelectFunc not defined');

        this.ContentWrapper.StyleProp('display', 'flex');
        this.ContentWrapper.StyleProp('flex-direction', 'column');
        this.ContentWrapper.StyleProp('gap', '2px');

        // tool-bar 
        this.ToolBar = new tp.ToolBar();
        this.ToolBar.Parent = this.ContentWrapper;
        this.ToolBar.AddClass(tp.Classes.ToolBar);

        // Command, Text, ToolTip, IcoClasses, CssClasses, ToRight
        this.btnExecute = this.ToolBar.AddButton('Execute', 'Execute', 'Execute', 'fa fa-bolt');
        this.btnClearFilter = this.ToolBar.AddButton('ClearFilter', 'Clear', 'Clear Filter', 'fa fa-trash-o');
        this.btnShowSql = this.ToolBar.AddButton('ShowSql', 'Show Sql', 'Show Sql', 'fa fa-file-text-o');
        this.btnShowIdColumns = this.ToolBar.AddButton('ShowIdColumns', 'Show/Hide Id Columns', 'Show/Hide Id Columns', 'fa fa-th-list');

        this.ToolBar.On('ButtonClick', this.AnyClick, this);

        this.ToolBar.SetNoText(true);

        // footer buttons
        this.btnOK = this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK, false);
        this.CreateFooterButton('Cancel', 'Cancel', tp.DialogResult.Cancel, false);

        // tab control
        this.Pager = new tp.TabControl();
        this.Pager.Parent = this.ContentWrapper;
        this.Pager.Height = '100%';
        this.tabFilter = this.Pager.AddPage('Filter');
        this.tabGrid = this.Pager.AddPage('Data');

        this.Pager.SelectedIndex = 0;

        // filter panel
        let el = this.tabFilter.AddDivElement();
        this.FilterPanel = new tp.SqlFilterPanel(el, { FilterDefs: this.FilterDefs });
        this.FilterPanel.Height = '100%';
        this.FilterPanel.Width = '100%';
        this.FilterPanel.StyleProp('overflow', 'auto');
        this.FilterPanel.StyleProp('padding', '2px');

        // grid
        this.Grid = new tp.Grid();
        this.Grid.Parent = this.tabGrid;
        this.Args.Grid = this.Grid;

        this.Grid.Height = '100%';
        this.Grid.Width = '100%';

        this.Grid.ReadOnly = true;
        this.Grid.ToolBarVisible = false;
        this.Grid.GroupsVisible = false;
        this.Grid.FilterVisible = false;
        this.Grid.FooterVisible = false;

        this.Grid.AllowUserToAddRows = false;
        this.Grid.AllowUserToDeleteRows = false;

        this.Grid.AutoGenerateColumns = true;

        this.Grid.On(tp.Events.DoubleClick, this.DoubleClick, this);

        this.SetTable(this.Args.Table);
    }


    /**
     * Sets a specified {@link tp.DataTable} as the grid's datasource.
     * @param {tp.DataTable} Table
     */
    SetTable(Table) {
        this.Grid.DataSource = null;
        if (Table instanceof tp.DataTable) {
            this.Grid.DataSource = Table;

            if (Table.Rows.length <= 1500)
                this.Grid.BestFitColumns();
        }
    }
    /** Sets the selected row
     * @protected
     * */
    SetSelectedRow() {
        var Row = this.Grid.FocusedRow;
        if (!tp.IsEmpty(Row)) {
            this.Args.SelectedRow = Row;
            this.Args.Table = Row.Table;
            this.DialogResult = tp.DialogResult.OK;
        }
    }

    /* event handlers */
    /** Event handler
     * @protected
     * @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
     */
    DoubleClick(Args) {
        this.SetSelectedRow();
    }

    /** Generates and returns the WHERE text, if any, else empty string.
     * @returns {string} Generates and returns the WHERE text, if any, else empty string.
     * */
    GenerateSql() {
        let Conditions = this.FilterPanel.GenerateSql();
        return Conditions.Where || '';
    }
    /** Executes the filter defined by the user and displays the result.
     * The actual SELECT statement is executed by a specified call-back function.
     * */
    async Execute() {
        let Where = this.GenerateSql() || '';
        let Table = await this.SelectFunc(Where);
        this.SetTable(Table);

        this.Pager.SelectedPage = this.tabGrid;
        this.Grid.ShowIdGridColumns(false);
    }

    /* Event triggers */
    /**
    Event handler. If a Command exists in the clicked element then the Args.Command is assigned.
    @protected
    @param {tp.EventArgs} Args The {@link tp.EventArgs} arguments
    */
    async AnyClick(Args) {
        if (Args.Handled !== true) {
            let SqlText;

            var Command = tp.GetCommand(Args);
            if (!tp.IsBlank(Command)) { 

                switch (Command) {
                    case 'OK':
                        Args.Handled = true;
                        this.SetSelectedRow(); // get selected row
                        break;

                    case 'Execute':
                        Args.Handled = true;
                        await this.Execute();
                        break;

                    case 'ShowSql':
                        Args.Handled = true;
                        SqlText = this.GenerateSql();
                        await tp.InfoBoxAsync(SqlText);
                        break;

                    case 'ClearFilter':
                        Args.Handled = true;
                        this.FilterPanel.ClearControls();
                        this.Pager.SelectedPage = this.tabFilter;
                        break;

                    case 'ShowIdColumns':
                        Args.Handled = true;
                        this.Grid.ShowHideIdGridColumns();
                        this.Pager.SelectedPage = this.tabGrid;
                        break;

                    default:
                        super.AnyClick(Args);
                        break;
                }

            }
        }
    }
};


/** A list of {@link tp.SqlFilterDef} items. A filter item is created based upon a {@link tp.SqlFilterDef} descriptor.
 * @type {tp.SqlFilterDef[]}
 */
tp.SqlFilterDialog.prototype.FilterDefs = [];

/** An async call-back function which is passed the WHERE clause this dialog generates, executes a SELECT, and returns a {@link tp.DataTable} 
 * @type {function}
 */
tp.SqlFilterDialog.prototype.SelectFunc = null;
/** The {@link tp.SqlFilterPanel} DIV panel upon which filter controls are rendered.
 * @type {tp.SqlFilterPanel}
 * */
tp.SqlFilterDialog.prototype.FilterPanel = null;

/** Field
 * @field
 * @type {tp.ControlToolBar}
 */
tp.SqlFilterDialog.prototype.ToolBar = null;

/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnExecute = false;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnClearFilter = false;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnShowSql = false;
/** Field
 * @field
 * @type {tp.ControlToolButton}
 */
tp.SqlFilterDialog.prototype.btnShowIdColumns = false;
/** Field
 * @field
 * @type {tp.tpElement}
 */
tp.SqlFilterDialog.prototype.btnOK = null;
/** Field
 * @field
 * @type {tp.TabControl}
 */
tp.SqlFilterDialog.prototype.Pager = null;
/** Field
 * @field
 * @type {tp.TabPage}
 */
tp.SqlFilterDialog.prototype.tabFilter = null;
/** Field
 * @field
 * @type {tp.TabPage}
 */
tp.SqlFilterDialog.prototype.tabGrid = null;

/** Field
 * @field
 * @type {tp.Grid}
 */
tp.SqlFilterDialog.prototype.Grid = null;
/** Field
 * @field
 * @type {tp.DataTable}
 */
tp.SqlFilterDialog.prototype.Table = null;



//#endregion


//---------------------------------------------------------------------------------------
// data dialog-box functions
//---------------------------------------------------------------------------------------


//#region TableBox
/**
Displays a modal window with a grid and returns the window. <br />
On return the Window.Args contains a Grid property that references the grid.
When the close callback function (CloseFunc) is called, the Window.Args contain SelectedRow property that references the Grid.FocusedRow, if any, or null.
@param {tp.DataTable} Table The {@link tp.DataTable} data table to display in the grid
@param {string} [Text] Optional. The caption title of the window
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.TableBox = function (Table, Text, CloseFunc, Creator) {
    Table = Table || new tp.DataTable('');

    if (tp.IsBlank(Text))
        Text = tp.Format('{0} ({1})', Table.Name || 'Table', Table.RowCount);

    var Grid = new tp.Grid(null, null);
    var Window = tp.ContentWindow.Show(true, Text, Grid.Handle, (Args) => {
        Args.Grid = Grid;
        Args.SelectedRow = Grid.FocusedRow;
        if (tp.IsFunction(CloseFunc))
            tp.Call(CloseFunc, Creator, Args);
    }, Creator);

    Grid.Handle.style.position = 'absolute';
    Grid.Width = '100%';
    Grid.Height = '100%';
    Grid.ReadOnly = true;
    Grid.ToolBarVisible = false;
    Grid.GroupsVisible = false;
    Grid.FilterVisible = false;
    Grid.FooterVisible = false;

    Window.Args.Grid = Grid;

    var DataSource = new tp.DataSource(Table);
    Grid.DataSource = DataSource;
    if (Table.RowCount < 800) {
        Grid.BestFitColumns();
    }

    Grid.Focus();

    return Window;
};
/**
Displays a modal window with a grid and returns a promise. <br />
The Window.Args contains a Grid property that references the grid and a SelectedRow property that references the Grid.FocusedRow, if any, or null. 
@param {tp.DataTable} Table The {@link tp.DataTable} data table to display in the grid
@param {string} [Text] Optional. The caption title of the window
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args. The Window.Args contains a Grid property that references the grid and a SelectedRow property that references the Grid.FocusedRow, if any, or null.
*/
tp.TableBoxAsync = function (Table, Text) {
    return new Promise((Resolve, Reject) => {
        tp.TableBox(Table, Text, (Args) => {
            Resolve(Args);
        });
    });
};
//#endregion

//#region RowBox
/**
Displays a modal window with a grid showing extended information about a specified data-row, and returns the window. <br />
Mainly for debug purposes.
@param {tp.DataRow} Row The {@link tp.DataRow} data row to display in the grid
@param {string} [Text] Optional. The caption title of the window
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.RowBox = function(Row, Text, CloseFunc, Creator) {
    Text = Text || 'Row box';

    var Column, // tp.DataColumn,
        Row2,   // tp.DataRow,
        v;

    var Table = new tp.DataTable('Row');
    Table.AddColumn('Ordinal', tp.DataType.Integer);
    Table.AddColumn('Type');
    Table.AddColumn('Column');
    Table.AddColumn('Caption');
    Table.AddColumn('Value');

    for (var i = 0, ln = Row.Table.Columns.length; i < ln; i++) {
        Column = Row.Table.Columns[i];

        Row2 = Table.AddEmptyRow();
        Row2.Set('Ordinal', i);
        Row2.Set('Type', tp.DataType.TypeName(Column.DataType));
        Row2.Set('Column', Column.Name);
        Row2.Set('Caption', Column.Title);
        if (!Row.IsNull(Column)) {
            v = Row.Get(Column);
            Row2.Set('Value', v);
        }
    }

    return tp.TableBox(Table, Text, CloseFunc, Creator);
};
/**
Displays a modal window with a grid showing extended information about a specified data-row and returns a promise. <br />
Mainly for debug purposes.
@param {tp.DataRow} Row The {@link tp.DataRow} data row to display in the grid
@param {string} [Text] Optional. The caption title of the window
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.RowBoxAsync = function (Row, Text) {
    return new Promise((Resolve, Reject) => {
        tp.RowBox(Row, Text, (Args) => {
            Resolve(Args);
        });
    });
};
//#endregion

//#region DataSetBox
/**
Displays a modal window with a grid displaying the tables of a dataset, and returns the window. <br /> 
@param {tp.DataSet} DataSet The {@link tp.DataSet} dataset to display in the grid
@param {string} [Text] Optional. The caption title of the window
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.DataSetBox = function(DataSet, Text, CloseFunc, Creator) {
    DataSet = DataSet || new tp.DataSet('');

    if (tp.IsBlank(Text))
        Text = tp.Format('{0} ({1})', DataSet.Name || 'DataSet', DataSet.Tables.length);

    var Args = new tp.WindowArgs();
    Args.Creator = Creator;
    Args.CloseFunc = CloseFunc;
    Args.Text = Text;
    //Args.Width = 800;
    //Args.Height = 600;

    Args.AsModal = true;
    Args.DataSet = DataSet;

    var Window = new tp.DataSetDialog(Args);
    Window.ShowModal();
    return Window;

};
/**
Displays a modal window with a grid displaying the tables of a dataset, returns a promise. <br />
@param {tp.DataSet} DataSet The {@link tp.DataSet} dataset to display in the grid
@param {string} [Text] Optional. The caption title of the window
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.DataSetBoxAsync = function (DataSet, Text) {
    return new Promise((Resolve, Reject) => {
        tp.DataSetBox(DataSet, Text, (Args) => {
            Resolve(Args);
        });
    });
};
//#endregion

//#region PickRowsBox

/**
Displays a row pick list dialog with a grid that allows the user to check/select multiple rows. Returns the window.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
@param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object. Must have (mandatory) a <code>tblSource: tp.DataTable</code> property. Optionally it may contain a <code>SelectedRows: tp.DataRow[]</code> and a <code>VisibleColumns: string[]</code> property.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowsBox1 = function (Args) {
    return tp.PickRowsBox(Args);
};
/**
Displays a row pick list dialog with a grid that allows the user to check/select multiple rows. Returns the window.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
@param {tp.DataTable} tblTarget The {@link tp.DataTable} table where selected rows are going to be inserted some how.
@param {tp.DataTable} tblSource The {@link tp.DataTable} table where selected rows come from.
@param {string[]} VisibleColumns A list of column names that should be visible. If null/empty, then all columns are visible.
@param {string} TargetKeyName A tblTarget Key field name used to initially find what source rows are already selected in target
@param {string} SourceKeyName A tblSource Key field name used to match with TargetKeyName
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowsBox2 = function (tblTarget, tblSource, VisibleColumns, TargetKeyName, SourceKeyName, CloseFunc, Creator) {
    return tp.PickRowsBox(tblTarget, tblSource, VisibleColumns, TargetKeyName, SourceKeyName, CloseFunc, Creator);
};
/**
Displays a row pick list dialog with a grid that allows the user to check/select multiple rows. Returns the window.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
@param {tp.DataTable} tblSource The {@link tp.DataTable} table where selected rows come from.
@param {string[]} VisibleColumns A list of column names that should be visible. If null/empty, then all columns are visible.
@param {tp.DataRow[]} SelectedRows A {@link tp.DataRow} array containing the tblSource rows that should be initially selected
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowsBox3 = function(tblSource, VisibleColumns, SelectedRows,  CloseFunc, Creator) {
    return tp.PickRowsBox(tblSource, VisibleColumns, SelectedRows, CloseFunc, Creator);
};

tp.PickRowsBox = function() {
    var i, ln,
        v,
        TargetKeyIndex,     // number,
        SourceKeyIndex,     // number,
        SourceRow,          // tp.DataRow,
        Row                 // tp.DataRow
        ;

    let Args,               // tp.WindowArgs,
        tblTarget,          // tp.DataTable,
        tblSource,          // tp.DataTable,
        VisibleColumns,     // string[],
        SelectedRows,       // tp.DataRow[],
        TargetKeyName,      // string,
        SourceKeyName,      // string,
        CloseFunc,          // (Args: tp.WindowArgs) => void,
        Creator             // Object
        ;

    let Result = null;      // tp.tpWindow
    let Case = 0;

    if (arguments.length === 1 && arguments[0] instanceof tp.WindowArgs) {
        Case = 1;

        Args = arguments[0];
    } else if (arguments.length > 2) {
        if (arguments[1] instanceof tp.DataTable) {
            Case = 2;

            tblTarget = arguments[0];
            tblSource = arguments[1];
            VisibleColumns = arguments[2];
            TargetKeyName = arguments[3];
            SourceKeyName = arguments[4];
            CloseFunc = arguments.length > 5 ? arguments[5] : null;
            Creator = arguments.length > 6 ? arguments[6] : null;
        } else {
            Case = 3;

            tblSource = arguments[0];
            VisibleColumns = arguments[1];
            SelectedRows = arguments[2];
            CloseFunc = arguments.length > 3 ? arguments[3] : null;
            Creator = arguments.length > 4 ? arguments[4] : null;
        }

    }


    switch (Case) {

        case 1:
            Result = new tp.MultiRowPickDialog(Args);
            Result.ShowModal();
            break;

        case 2:
            TargetKeyIndex = tblTarget.IndexOfColumn(TargetKeyName);
            SourceKeyIndex = tblSource.IndexOfColumn(SourceKeyName);

            for (i = 0, ln = tblSource.Rows.length; i < ln; i++) {
                SourceRow = tblSource.Rows[i];
                v = SourceRow.Get(SourceKeyIndex);
                Row = tblTarget.FindRow(TargetKeyIndex, v);
                if (!tp.IsEmpty(Row)) {
                    SelectedRows.push(SourceRow);
                }
            }

            Result = tp.PickRowsBox(tblSource, VisibleColumns, SelectedRows, CloseFunc, Creator);
            break;

        case 3:
            Args = new tp.WindowArgs();
            Args.Creator = Creator;
            Args.CloseFunc = CloseFunc;
            Args.Text = 'Multi-row Selection';

            Args.tblSource = tblSource;
            Args.VisibleColumns = VisibleColumns;
            Args.SelectedRows = SelectedRows;

            Result = tp.PickRowsBox(Args);

            break;
    }

    return Result;
};

/**
Displays a row pick list dialog with a grid that allows the user to check/select multiple rows. Returns a promise.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
@param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object.  Must have (mandatory) a <code>tblSource: tp.DataTable</code> property. Optionally it may contain a <code>SelectedRows: tp.DataRow[]</code> and a <code>VisibleColumns: string[]</code> property.
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowsBoxAsync1 = function (Args) {
    return tp.PickRowsBoxAsync(Args);
};
/**
Displays a row pick list dialog with a grid that allows the user to check/select multiple rows. Returns a promise.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
@param {tp.DataTable} tblTarget The {@link tp.DataTable} table where selected rows are going to be inserted some how.
@param {tp.DataTable} tblSource The {@link tp.DataTable} table where selected rows come from.
@param {string[]} VisibleColumns A list of column names that should be visible. If null/empty, then all columns are visible.
@param {string} TargetKeyName A tblTarget Key field name used to initially find what source rows are already selected in target
@param {string} SourceKeyName A tblSource Key field name used to match with TargetKeyName
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowsBoxAsync2 = function (tblTarget, tblSource, VisibleColumns, TargetKeyName, SourceKeyName) {
    return tp.PickRowsBoxAsync(tblTarget, tblSource, VisibleColumns, TargetKeyName, SourceKeyName);
};
/**
Displays a row pick list dialog with a grid that allows the user to check/select multiple rows. Returns a promise.
If the user clicks OK on the dialog, then the dialog SelectedRows array property contains the selected tp.DataRow rows.
@param {tp.DataTable} tblSource The {@link tp.DataTable} table where selected rows come from.
@param {string[]} VisibleColumns A list of column names that should be visible. If null/empty, then all columns are visible.
@param {tp.DataRow[]} SelectedRows A {@link tp.DataRow} array containing the tblSource rows that should be initially selected
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowsBoxAsync3 = function (tblSource, VisibleColumns, SelectedRows) {
    return tp.PickRowsBoxAsync(tblSource, VisibleColumns, SelectedRows);
};

tp.PickRowsBoxAsync = function() {
    let Args,           // tp.WindowArgs,
        tblTarget,      // tp.DataTable,
        tblSource,      // tp.DataTable,
        VisibleColumns, // string[],
        SelectedRows,   // tp.DataRow[],
        TargetKeyName,  // string,
        SourceKeyName   // string
        ;


    let Result = Promise.resolve(new tp.WindowArgs());

    let Case = 0;

    if (arguments.length === 1 && arguments[0] instanceof tp.WindowArgs) {
        Case = 1;

        Args = arguments[0];
    } else if (arguments.length > 2) {
        if (arguments[1] instanceof tp.DataTable) {
            Case = 2;

            tblTarget = arguments[0];
            tblSource = arguments[1];
            VisibleColumns = arguments[2];
            TargetKeyName = arguments[3];
            SourceKeyName = arguments[4];
        } else {
            Case = 3;

            tblSource = arguments[0];
            VisibleColumns = arguments[1];
            SelectedRows = arguments[2];
        }

    }

    switch (Case) {

        case 1:
            Result = new Promise((Resolve, Reject) => {
                Args.CloseFunc = (Args) => {
                    Resolve(Args);
                };
                tp.PickRowsBox(Args);
            });
            break;

        case 2:
            Result = new Promise((Resolve, Reject) => {
                tp.PickRowsBox(tblTarget, tblSource, VisibleColumns, TargetKeyName, SourceKeyName, (Args) => {
                    Resolve(Args);
                });
            });

            break;

        case 3:
            Result = new Promise((Resolve, Reject) => {
                tp.PickRowsBox(tblSource, VisibleColumns, SelectedRows, (Args) => {
                    Resolve(Args);
                });
            });

            break;
    }

    return Result;

};
//#endregion

//#region PickRowBox

/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object. Must have (mandatory), at least, defined either a  <code>Table: tp.DataTable</code> or a <code>SqlText: string </code> or a <code>SelectSql: tp.SelectSql </code> property.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowBox1 = function (Args) {
    return tp.PickRowBox(Args);
};
/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.DataTable} Table The table to display. If null, then SqlText is mandatory.
@param {string[]} VisibleColumns  A list of column names that should be visible. If null/empty, then all columns are visible.
@param {any} [KeyValue] Optional. If KeyValue and KeyFieldName are present, then the form positions the display grid to the KeyValue row.
@param {string} [KeyFieldName] Optional. If KeyValue and KeyFieldName are present, then the dialog positions the display grid to the KeyValue row.
@param {string} SqlText Optional. The SELECT statement to execute. If null/empty, then Table is mandatory
@param {string} [Text] Optional. The caption title of the window
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowBox2 = function (Table, VisibleColumns, KeyValue, KeyFieldName, SqlText, Text, CloseFunc, Creator) {
    return tp.PickRowBox(Table, VisibleColumns, KeyValue, KeyFieldName, SqlText, Text, CloseFunc, Creator);
};
/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.SelectSql} SelectSql - The statement to be executed
@param {string} [Text] Optional. The caption title of the window
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowBox3 = function (SelectSql, Text, CloseFunc, Creator) {
    return tp.PickRowBox(SelectSql, Text, CloseFunc, Creator);
};
/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {string} SqlText The SELECT statement to execute.
@param {string[]} VisibleColumns  A list of column names that should be visible. If null/empty, then all columns are visible.
@param {string} [Text] Optional. The caption title of the window
@param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
@param {object} [Creator]  Optional. The context (this) for the callback function.
@returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
*/
tp.PickRowBox4 = function (SqlText, VisibleColumns, Text, CloseFunc, Creator) {
    return tp.PickRowBox(SqlText, VisibleColumns, Text, CloseFunc, Creator);
};

tp.PickRowBox = function () {
    let Args,               // tp.WindowArgs,
        Table,              // tp.DataTable,
        VisibleColumns,     // string[],
        KeyValue,           // any,
        KeyFieldName,       // string,
        SqlText,            // string,
        SelectSql,          // tp.SelectSql,
        Text,               // string,
        CloseFunc,          // (Args: tp.WindowArgs) => void,
        Creator             // Object
        ;

    let Result = null;      // tp.tpWindow
    let Case = 0;


    if (arguments.length === 1 && arguments[0] instanceof tp.WindowArgs) {
        Case = 1;

        Args = arguments[0];
    } else if (arguments.length > 0) {
        if (arguments[0] instanceof tp.DataTable) {
            Case = 2;

            Table = arguments[0];
            VisibleColumns = arguments[1];
            KeyValue = arguments.length > 2 ? arguments[2] : null;
            KeyFieldName = arguments.length > 3 ? arguments[3] : null;
            SqlText = arguments.length > 4 ? arguments[4] : null;
            Text = arguments.length > 5 ? arguments[5] : null;
            CloseFunc = arguments.length > 6 ? arguments[6] : null;
            Creator = arguments.length > 7 ? arguments[7] : null;
        } else if (arguments[0] instanceof tp.SelectSql) {
            Case = 3;

            SelectSql = arguments[0];
            Text = arguments.length > 1 ? arguments[1] : null;
            CloseFunc = arguments.length > 2 ? arguments[2] : null;
            Creator = arguments.length > 3 ? arguments[3] : null;
        } else if (tp.IsString(arguments[0])) {
            Case = 4;

            SqlText = arguments[0];
            VisibleColumns = arguments[1];
            Text = arguments.length > 2 ? arguments[2] : null;
            CloseFunc = arguments.length > 3 ? arguments[3] : null;
            Creator = arguments.length > 4 ? arguments[4] : null;
        }
    }


    switch (Case) {

        case 1:
            Result = new tp.SingleRowPickDialog(Args);
            Result.ShowModal();
            break;

        case 2:
            Args = new tp.WindowArgs();
            Args.Creator = Creator;
            Args.CloseFunc = CloseFunc;
            Args.Text = Text || 'Single-row Selection';

            Args.Table = Table;
            Args.VisibleColumns = VisibleColumns;
            Args.KeyValue = KeyValue;
            Args.KeyFieldName = KeyFieldName;
            Args.SqlText = SqlText;

            Result = new tp.SingleRowPickDialog(Args);
            Result.ShowModal();
            break;

        case 3:
            Args = new tp.WindowArgs();
            Args.Creator = Creator;
            Args.CloseFunc = CloseFunc;
            Args.Text = Text || 'Single-row Selection';

            Args.SelectSql = SelectSql;

            Result = new tp.SingleRowPickDialog(Args);
            Result.ShowModal();
            break;

        case 4:
            Args = new tp.WindowArgs();
            Args.Creator = Creator;
            Args.CloseFunc = CloseFunc;
            Args.Text = Text || 'Single-row Selection';

            Args.VisibleColumns = VisibleColumns;
            Args.SqlText = SqlText;

            Result = new tp.SingleRowPickDialog(Args);
            Result.ShowModal();

            break;
    }

    return Result;
};

/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.WindowArgs} Args - A {@link tp.WindowArgs} arguments object. Must have (mandatory), at least, defined either a  <code>Table: tp.DataTable</code> or a <code>SqlText: string </code> or a <code>SelectSql: tp.SelectSql </code> property.
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowBoxAsync1 = function (Args) {
    return tp.PickRowBoxAsync(Args);
};
/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.DataTable} Table The table to display. If null, then SqlText is mandatory.
@param {string[]} VisibleColumns A list of column names that should be visible. If null/empty, then all columns are visible.
@param {any} [KeyValue] If KeyValue and KeyFieldName are present, then the form position the display grid to the KeyValue row.
@param {string} [KeyFieldName] If KeyValue and KeyFieldName are present, then the form position the display grid to the KeyValue row.
@param {string} [SqlText] Optional. The SELECT statement to execute. If null/empty, then Table is mandatory
@param {string} [Text] Optional. The caption title of the window
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowBoxAsync2 = function (Table, VisibleColumns, KeyValue, KeyFieldName, SqlText, Text) {
    return tp.PickRowBoxAsync(Table, VisibleColumns, KeyValue, KeyFieldName, SqlText, Text);
};

/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {tp.SelectSql} SelectSql The statement to be executed
@param {string} [Text] Optional. The caption title of the window
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowBoxAsync3 = function (SelectSql, Text) {
    return tp.PickRowBoxAsync(SelectSql, Text);
};
/**
Displays a row pick dialog with a grid that allows the user to check/select a single row.
If the user clicks OK on the dialog, then the dialog SelectedRow tp.DataRow property contains the selected tp.DataRow row.
@param {string} SqlText The SELECT statement to execute.
@param {string[]} VisibleColumns - A list of column names that should be visible. If null/empty, then all columns are visible.
@param {string} [Text] Optional. The caption title of the window
@returns {tp.WindowArgs} Returns a promise with the modal window {@link tp.WindowArgs} Args.
*/
tp.PickRowBoxAsync4 = function(SqlText, VisibleColumns, Text) {
    return tp.PickRowBoxAsync(SqlText, VisibleColumns, Text);
};

tp.PickRowBoxAsync = function () {
    let Args,               // tp.WindowArgs,
        Table,              // tp.DataTable,
        VisibleColumns,     // string[],
        KeyValue,           // any,
        KeyFieldName,       // string,
        SqlText,            // string,
        SelectSql,          // tp.SelectSql,
        Text                // string
        ;

    let Result = Promise.resolve(new tp.WindowArgs());
    let Case = 0;


    if (arguments.length === 1 && arguments[0] instanceof tp.WindowArgs) {
        Case = 1;

        Args = arguments[0];
    } else if (arguments.length > 0) {
        if (arguments[0] instanceof tp.DataTable) {
            Case = 2;

            Table = arguments[0];
            VisibleColumns = arguments[1];
            KeyValue = arguments.length > 2 ? arguments[2] : null;
            KeyFieldName = arguments.length > 3 ? arguments[3] : null;
            SqlText = arguments.length > 4 ? arguments[4] : null;
            Text = arguments.length > 5 ? arguments[5] : null;
        } else if (arguments[0] instanceof tp.SelectSql) {
            Case = 3;

            SelectSql = arguments[0];
            Text = arguments.length > 1 ? arguments[1] : null;
        } else if (tp.IsString(arguments[0])) {
            Case = 4;

            SqlText = arguments[0];
            VisibleColumns = arguments[1];
            Text = arguments.length > 2 ? arguments[2] : null;
        }
    }


    switch (Case) {

        case 1:
            Result = new Promise((Resolve, Reject) => {
                Args.CloseFunc = (Args) => {
                    Resolve(Args);
                };
                tp.PickRowBox(Args);
            });

            break;

        case 2:
            Result = new Promise((Resolve, Reject) => {
                tp.PickRowBox(Table, VisibleColumns, KeyValue, KeyFieldName, SqlText, Text, (Args) => {
                    Resolve(Args);
                });
            });

            break;

        case 3:
            Result = new Promise((Resolve, Reject) => {
                tp.PickRowBox(SelectSql, Text, (Args) => {
                    Resolve(Args);
                });
            });
            break;

        case 4:
            Result = new Promise((Resolve, Reject) => {
                tp.PickRowBox(SqlText, VisibleColumns, Text, (Args) => {
                    Resolve(Args);
                });
            });

            break;
    }

    return Result;
};
//#endregion

//#region tp.SqlFilterBox
 
/**
 * Displays an WHERE filter dialog. The user defines the filter, executes the SELECT, and selects a single row from a grid.
 * If the user clicks OK on the dialog, then the returned Args contain a SelectedRow and a Table property.
 * @param {tp.SqlFilterDef[]} FilterDefs Required. The filters to display in the dialog
 * @param {function} SelectFunc Required. An async call-back function which is passed the WHERE clause the dialog generates, executes a SELECT, and returns a {@link tp.DataTable}.
 * @param {tp.DataTable} Table Optional. A {@link tp.DataTable} to display initially.
 * @param {Function} [CloseFunc] Optional. A function as <code>void (Args: tp.WindowArgs)</code>. Called when the window closes
 * @param {object} [Creator]  Optional. The context (this) for the callback function.
 * @returns {tp.tpWindow} Returns the {@link tp.tpWindow} window.
 */
tp.SqlFilterBox = function (FilterDefs, SelectFunc, Table, CloseFunc, Creator) {
    let Args = new tp.WindowArgs();
    Args.Text = 'Filters';
    Args.Creator = Creator;
    Args.CloseFunc = CloseFunc;
    Args.FilterDefs = FilterDefs;
    Args.SelectFunc = SelectFunc;
    Args.Table = Table;

    let Result = new tp.SqlFilterDialog(Args);
    Result.ShowModal();
    return Result;
};
/**
 * Displays an WHERE filter dialog. The user defines the filter, executes the SELECT, and selects a single row from a grid.
 * If the user clicks OK on the dialog, then the returned Args contain a SelectedRow and a Table property.
 * @param {tp.SqlFilterDef[]} FilterDefs Required. The filters to display in the dialog
 * @param {function} SelectFunc Required. An async call-back function which is passed the WHERE clause the dialog generates, executes a SELECT, and returns a {@link tp.DataTable}.
 * @param {tp.DataTable} Table Optional. A {@link tp.DataTable} to display initially.
 * @returns {tp.WindowArgs} Returns the {@link tp.WindowArgs} of the dialog. If the user clicks OK, then the returned object contains the SelectedRow and the Table properties. 
 */
tp.SqlFilterBoxAsync = function (FilterDefs, SelectFunc, Table) {

    let Result = new Promise((Resolve, Reject) => {
        tp.SqlFilterBox(FilterDefs, SelectFunc, Table, (Args) => {
            Resolve(Args);
        }); 
    });

    return Result;

};
//#endregion

//---------------------------------------------------------------------------------------
// tp.Ui
//---------------------------------------------------------------------------------------


//#region  tp.Ui

/**
Static helper class for Tripous ui
*/
tp.Ui = class {

    /**
    Constructor
    */
    constructor() {
        throw 'Can not create an instance of a full static class';
    }


    /* private */
    /**
    Creates and returns a tp.tpElement instance based on a registered type-name.
    @private
    @param {string} TypeName - The type-name. One of the tp.Ui.Types.
    @param {string | Node} ElementOrSelector - The element up-on to create the {@link tp.tpElement} instance
    @returns {tp.tpElement} Returns a {@link tp.tpElement} instance based on a registered type-name.
    */
    static CreateElement(TypeName, ElementOrSelector) {
        let el = null;
        let Type = this.Types[TypeName];
        if (tp.IsEmpty(Type)) {
            tp.Throw('Control type name not registered in tp.Ui.Types: ' + TypeName);
        }

        if (!tp.IsEmpty(ElementOrSelector)) {
            el = tp.Select(ElementOrSelector);
            if (tp.IsHTMLElement(el) && tp.HasScriptObject(el)) {
                return tp.GetScriptObject(el);
            }
        }

        var Result = new Type(el, null);            // tp.tpElement (or descendant) constructor
        tp.RemoveClass(Result.Handle, TypeName);
        return Result;
    }
    /**
    Creates and returns a tp.tpElement. <br />
    Used in creating tp.tpElement instances that are NOT registered with tp.Ui.Types.  <br />
    The DOM element must have a markup like the following:
    <pre>
        <div class="tp-Class" data-setup="{ClassType: tp.MyDataView, .... }"></div>
    </pre>
    The ClassType property defines a class that is NOT registered with tp.Ui.Types.
    @private
    @param {HTMLElement} el The DOM element upon to create the {@link tp.tpElement}
    @returns {tp.tpElement} The newly created {@link tp.tpElement}
    */
    static CreateByClass(el) {
        var Type, o, Result = null;

        if (tp.IsHTMLElement(el)) {
            if (tp.HasScriptObject(el)) {
                Result = tp.GetScriptObject(el);
            } else {
                o = tp.GetDataSetupObject(el);
                if (tp.IsObject(o) && 'ClassType' in o) {
                    Type = o.ClassType;
                    Result = new Type(el, null);                // tp.tpElement (or descendant) constructor
                    tp.RemoveClass(Result.Handle, 'tp-Class');
                } 
            }
        }

        return Result;
    }
    /**
    Creates all the DOM elements needed for a control-row and the {@link tp.Control} and returns the {@link tp.Control}. <br />
    Example markup
    <pre>
        <div class="tp-CtrlRow" data-setup="{Text: 'Code', Control: { TypeName: 'TextBox', DataField: 'Code' } }"></div>
        <div class="tp-CtrlRow" data-setup="{Text: 'Test', Control: { TypeName: 'ComboBox', DataField: '', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0} }"></div>
    </pre>
    Produced markup
    <pre>
        <div class="tp-CtrlRow tp-Row" id="control_row_Name-2001">
           <div class="tp-CText">
	        <label for="Name">Όνομα</label>
	        <span class="tp-RequiredMark">*</span>
           </div>
           <div class="tp-Ctrl">
	        <input class="tp-TextBox" id="Name" name="Name" type="text" value="">
           </div>
        </div>
    </pre>
    @private
    @param {HTMLElement}  elRow - The DOM element upon to create the control-row.
    @returns {tp.tpElement} Returns the {@link tp.Control} of a control-row
    */
    static CreateCtrlRow(elRow) {

        let Type,
            TypeName,
            DataField,

            Setup,              // data-setup attribute
            CP,
            Prefix,
            divText,            // HTMLElement
            //elText,            // HTMLElement
            //elRequiredMark,     // HTMLSpanElement
            divCtrl,            // HTMLElement
            Result = null;      // tp.tpElement

        //tp.AddClass(elRow, tp.Classes.Row);

        if (elRow.children.length === 0) {
            Setup = tp.GetDataSetupObject(elRow);
            Setup.Text = tp.IsString(Setup.Text) ? Setup.Text.trim() : '';
            Setup.Control = tp.IsObject(Setup.Control) ? Setup.Control : {};

            // get the constructor
            TypeName = Setup.Control.TypeName;
            Type = this.Types[TypeName];
            if (tp.IsEmpty(Type)) {
                tp.Throw('Control type name not registered in tp.Ui.Types: ' + TypeName);
            }

            // prepare formatting
            DataField = tp.IsString(Setup.Control.DataField) ? Setup.Control.DataField.trim() : '';            

            // Ids
            Prefix = !tp.IsBlank(DataField) ? `${tp.Prefix}CtrlRow-${DataField}-` : `${tp.Prefix}CtrlRow-`;

            if (tp.IsString(elRow.id) && tp.IsBlank(elRow.id))
                elRow.id = tp.SafeId(Prefix);

            if (tp.IsString(Setup.Control.Id) && tp.IsBlank(Setup.Control.Id))
                Setup.Control.Id = tp.SafeId(`${tp.Prefix}${TypeName}-`);

            // Markup similar to following goes to the last div
            // <input class="tp-TextBox" id="Name" name="Name" type="text" value="">
            let InnerHTML =
`<div class="${tp.Classes.CText}">
  <label for="${Setup.Control.Id}">${Setup.Text}</label>
  <span class="${tp.Classes.RequiredMark}" style='display: none;'>*</span>
</div>
<div class="${tp.Classes.Ctrl}"></div>`;

            tp.Html(elRow, InnerHTML);

            divCtrl = tp.Select(elRow, '.' + tp.Classes.Ctrl);
            divText = tp.Select(elRow, '.' + tp.Classes.CText);
 

            CP = Setup.Control;
            CP.Parent = divCtrl;
            CP.elText = tp.Select(divText, 'label');
            CP.elRequiredMark = tp.Select(divText, '.' + tp.Classes.RequiredMark);

            // call the constructor
            Result = new Type(null, CP);

            // further adjustments
            //Result.Id = CP.Id;
        }

        return Result;
    }
    /**
    Creates all the DOM elements needed for a check-box control-row and the {@link tp.Control} and returns the {@link tp.Control}. <br />
    Example markup
    <pre>
        <div class="tp-CheckBoxRow" data-setup="{Text: 'Code', Control: { TypeName: 'CheckBox', DataField: 'IsMarried' } }
    </pre>
    Produced markup
    <pre>
        <div class="tp-CheckBoxRow tp-Row" id="control_row_IsCool-2001">
          <label class="tp-CheckBox">
            <input checked="checked" type="checkbox" >
            <span class="tp-RequiredMark">*</span>
            <span class="tp-Text">Yes, this is by design</span>
          </label>
        </div>
    </pre>
    @private
    @param  {HTMLElement} elRow - The DOM element upon to create the control-row.
    @returns {tp.tpElement} Returns the {@link tp.Control} of a check-box control-row
    */
    static CreateCheckBoxRow(elRow) { 

        let Type = tp.CheckBox,
            TypeName = 'CheckBox',
            DataField,
            Setup,              // data-setup attribute
            CP,
            Prefix, 
            Result = null;      // tp.tpElement  

        //tp.AddClass(elRow, tp.Classes.Row);

        if (elRow.children.length === 0) {
            Setup = tp.GetDataSetupObject(elRow);
            Setup.Text = tp.IsString(Setup.Text) ? Setup.Text.trim() : '';
            Setup.Control = tp.IsObject(Setup.Control) ? Setup.Control : {};

            // get the constructor
            TypeName = Setup.Control.TypeName;
            Type = this.Types[TypeName];
            if (tp.IsEmpty(Type)) {
                Type = tp.CheckBox;
            }

            // prepare formatting
            DataField = tp.IsString(Setup.Control.DataField) ? Setup.Control.DataField.trim() : '';

            // Ids
            Prefix = !tp.IsBlank(DataField) ? `${tp.Prefix}CheckBoxRow-${DataField}-` : `${tp.Prefix}CheckBoxRow-`;

            if (tp.IsString(elRow.id) && tp.IsBlank(elRow.id))
                elRow.id = tp.SafeId(Prefix);

            if (tp.IsString(Setup.Control.Id) && tp.IsBlank(Setup.Control.Id))
                Setup.Control.Id = tp.SafeId(`${tp.Prefix}${TypeName}-`);

            // <label><input type="checkbox" /><span class='tp-Text'> This is the text of the checkbox</span></label>
            let InnerHTML =
` <label class="${tp.Classes.CheckBox}">
    <input type="checkbox" />
    <span class="${tp.Classes.RequiredMark}" style='display: none;'>*</span>
    <span class="${tp.Classes.Text}">${Setup.Text}</span>
  </label>`;

            tp.Html(elRow, InnerHTML);
 
            CP = Setup.Control;
            CP.Parent = elRow;
            CP.elText = tp.Select(elRow, '.' + tp.Classes.Text);
            CP.elRequiredMark = tp.Select(elRow, '.' + tp.Classes.RequiredMark);

            // call the constructor
            let el = tp.Select(elRow, `label.${tp.Classes.CheckBox}`);
            Result = new Type(el, CP);

            // further adjustments
            //Result.Id = CP.Id;
 
        }         

        return Result;
    }

    /* public */
    /**
    Returns a selector containing all the type names registered with tp.Ui.Types <br />
    e.g. ".TextBox, .DateBox etc."  
    @returns {string} Returns a selector containing all the type names registered with tp.Ui.Types
    */
    static GetSelector() {
        var A = [];
        for (var TypeName in tp.Ui.Types) {
            A.push('.' + TypeName);
        }

        return A.join(', ');
    }
    /**
    Returns an array with all tp.Element objects constructed/existing up on child DOM elements of a specified parent element or the entire document. <br />
    When a Tripous script object is created upon a DOM element, that element is marked with the tp-Object css class and a new property, named tpObject
    is created and attached to the DOM element object, pointing to the Tripous object.
    @param {string | Node} ParentElementOrSelector - String or Element. Defaults to document. The container of controls. If null/undefined/empty the document is used
    @returns {tp.tpElement[]} Returns an array with tp.Element objects constructed up on elements of a parent element
    */
    static GetContainerControls(ParentElementOrSelector) {
        return tp.GetScriptObjects(ParentElementOrSelector);
    }

    /* control creation */
    /**
     Creates all controls whose DOM elements are children of a certain parent, automatically. <br />
     Child DOM elements must have defined a proper css class, that is a css class registered with tp.Ui.Types, e.g. 'TextBox'      
     @param {string | Node} [ParentElementSelector] - Optional. A selector for the parent. If null then the document.body is used.
     @param {string[]} [ExcludedTypes] - Optional.  An array with type names to exclude.
     @returns {tp.tpElement[]} Returns a list with all created controls for the parent.
     */
    static CreateControls(ParentElementSelector = null, ExcludedTypes = null) {
        var Result = [];
        ParentElementSelector = ParentElementSelector || tp.Doc.body;
        ExcludedTypes = ExcludedTypes || [];

        var List;
        for (let TypeName in this.Types) {
            if (ExcludedTypes.indexOf(TypeName) === -1) {
                List = this.CreateControlsOfType(ParentElementSelector, TypeName);
                if (List.length > 0)
                    Result = Result.concat(List);
            }
        }

        // fixup
        this.FixupControls(Result);

        return Result;
    }
    /**
     Creates controls of a specified control type, whose DOM elements are children of a certain parent, automatically. <br /> 
     Child DOM elements must have defined a proper css class, that is a css class registered with tp.Ui.Types, e.g. 'TextBox'              
     @param {string | Node} ParentElementSelector - A selector for the parent. If null then the document.body is used.
     @param {string} TypeName - The name of the control type. Must be registered with tp.Ui.Types
     @returns {tp.tpElement[]} Returns a list with all created controls for the parent.
     */
    static CreateControlsOfType(ParentElementSelector, TypeName) {
        var Result = [];
        ParentElementSelector = ParentElementSelector || tp.Doc.body;

        var elParent = tp.Select(ParentElementSelector);
        if (tp.IsHTMLElement(elParent)) {
            var Selector, i, ln, el, o, List;

            Selector = '.' + TypeName;
            List = tp.SelectAll(elParent, Selector);

            for (i = 0, ln = List.length; i < ln; i++) {
                el = List[i];
                if (tp.IsHTMLElement(el)) {
                    o = this.Create(TypeName, el);
                    if (o)
                        Result.push(o);
                }

            }
        }

        return Result;
    }
    /**
     Creates a control based on a type name. If the passed element is already a tp.Element.Handle then no new control is created. 
     
     @param {string} TypeName - The name of the control type. Must be registered with tp.Ui.Types
     @param {HTMLElement} el - A HTMLElement upon to create the control
     @returns {tp.tpElement} Returns the control it creates.
     */
    static Create(TypeName, el) {
        var Result = null; // tp.tpElement

        if (tp.IsElement(el) && tp.HasObject(el)) {
            if (tp.GetObject(el) instanceof tp.tpElement)
                Result = tp.GetObject(el);
        } else if (TypeName === 'tp-Class') {
            Result = this.CreateByClass(el);
        } else if (TypeName === 'tp-CtrlRow') {
            Result = this.CreateCtrlRow(el);
        } else if (TypeName === 'tp-CheckBoxRow') {
            Result = this.CreateCheckBoxRow(el);
        } else {
            Result = this.CreateElement(TypeName, el);  // TypeName, ElementOrSelector
        }

        if (Result) {
            if (tp.IsFunction(Result.OnAfterConstruction))
                Result.OnAfterConstruction();
        }

        return Result;
    }


    /**
    Fixes-up a list of just-created controls. That is resolves references etc.      
    @param {tp.tpElement[]} List An array of tp.Element instances
    */
    static FixupControls(List) {
        // not yet
    }

    /**
     * Returns the {@link HTMLElement} DIV marked with a control row css class (Control row or CheckBox row) that contains a specified element
     * @param {HTMLElement} el The element to find the parent control row of.
     */
    static GetCtrlRow(el) {
        let elResult = tp.Closest(el, '.' + tp.Classes.CtrlRow);
        if (!tp.IsHTMLElement(elResult))
            elResult = tp.Closest(el, '.' + tp.Classes.CheckBoxRow);
        return elResult;
    }
};



// Firefox v.69 does NOT support static keyword in fields

/**
A dictionary of registered ui types.
The tp.Ui class can automatically create instances of controls that are registered in this dictionary.
A container class, such as tp.Page or tp.View, uses the tp.Ui in order to create its controls when the proper markup is provided.
@type {object}
*/
tp.Ui.Types = {
    /**
    Used in creating tp.Element instances that are NOT registered with tp.Ui.Types. <br />
    Example markup
    <pre>
        <div class="tp-Class" data-setup="{ClassType: tp.MyDataView, .... }"></div>
    </pre>
    */
    'tp-Class': 'tp-Class',
    'tp-CtrlRow': 'tp-CtrlRow',
    'tp-CheckBoxRow': 'tp-CheckBoxRow',

    Element: tp.tpElement,

    IFrame: tp.IFrame,
    Splitter: tp.Splitter,
    GroupBox: tp.GroupBox,
    Accordion: tp.Accordion,
    TabControl: tp.TabControl,
    PanelList: tp.PanelList,
    ImageSlider: tp.ImageSlider,

    Menu: tp.Menu,
    ContextMenu: tp.ContextMenu,
    SiteMenu: tp.SiteMenu,
    ToolBar: tp.ToolBar,

    Button: tp.Button,
    ButtonEx: tp.ButtonEx,

    Label: tp.Label,
    TextBox: tp.TextBox,
    Memo: tp.Memo,
    CheckBox: tp.CheckBox,
    NumberBox: tp.NumberBox,

    ComboBox: tp.ComboBox,
    ListBox: tp.ListBox,
    CheckComboBox: tp.CheckComboBox,
    CheckListBox: tp.CheckListBox,
    HtmlComboBox: tp.HtmlComboBox,
    HtmlListBox: tp.HtmlListBox,
    HtmlNumberBox: tp.HtmlNumberBox,
    HtmlDateBox: tp.HtmlDateBox,

    Calendar: tp.Calendar,
    DateBox: tp.DateBox,
    ImageBox: tp.ImageBox,

    LocatorBox: tp.LocatorBox,

    RadioGroup: tp.RadioGroup,
    ValueSlider: tp.ValueSlider,    
    ProgressBar: tp.ProgressBar,
    TreeView: tp.TreeView,   

};

/**
 * Creates all controls whose DOM elements are children of a certain parent, automatically. <br />
   Child DOM elements must have defined a proper css class, that is a css class registered with tp.Ui.Types, e.g. 'TextBox'
 * @param {string | Node} [ParentElementSelector] - Optional. A selector for the parent. If null then the document.body is used.
 * @param {string[]} [ExcludedTypes] - Optional.  An array with type names to exclude.
 * @returns {tp.tpElement[]} Returns a list with all created controls for the parent.
 */
tp.CreateContainerControls = function (ParentElementSelector = null, ExcludedTypes = null) {
    return tp.Ui.CreateControls(ParentElementSelector, ExcludedTypes);
};

//#endregion