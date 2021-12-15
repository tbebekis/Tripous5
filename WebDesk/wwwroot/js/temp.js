
//#region tp.DeskMainMenu

tp.DeskMainMenu = class extends tp.ItemBar {

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

    /**
    Initializes the 'static' and 'read-only' class fields
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.DeskMainMenu';
        this.fDefaultCssClasses.push(tp.Classes.DeskMainMenu);
    }
    /**
    Event trigger. Called by CreateHandle() after all creation and initialization processing is done, that is AFTER handle creation and AFTER option processing
    @protected
    @override
    */
    OnInitializationCompleted() {
        this.ResponsiveMode = tp.ItemBarRenderMode.NextPrev;
        super.OnInitializationCompleted(); 
    }
    /** Creates child controls of this instance. Called in construction sequence.
    * @private
    * */
    async CreateControls() {
        super.CreateControls();

        let Url = tp.Urls.DeskGetMainMenu;
 
        let Args = await tp.Ajax.GetAsync(Url); 

        var o = JSON.parse(Args.ResponseText);
        if (o.IsSuccess === false)
            tp.Throw(o.ErrorText);

        let Packet = JSON.parse(o.Packet);

 

        let BarItemList = [];
        let BarItem;
        if (tp.IsArray(Packet)) {
            Packet.forEach((cmd) => {
                BarItem = new tp.DeskMainMenuBarItem(cmd);
                BarItemList.push(BarItem.Handle);
            });
        }

        this.AddRange(BarItemList);

    }
};


//#endregion


//#region tp.DeskMainMenuBarItem
tp.DeskMainMenuBarItem = class {
    constructor(Cmd) {

        this.Cmd = Cmd;
        let Text = Cmd.Title ? Cmd.Title : Cmd.Name;

        let BarItemHtmlText = `
<div class="${tp.Classes.BarItem}">
    <div class="${tp.Classes.Text}">${Text}</div>
    <div class="${tp.Classes.List}"> 
    </div>
</div>
`;
        this.Handle = tp.HtmlToElement(BarItemHtmlText);
        let List = tp.ChildHTMLElements(this.Handle);
        this.elText = List[0];
        this.elList = List[1];

        tp.DropDownHandler(this.elText, this.elList, tp.Classes.Visible);

        if (tp.IsArray(Cmd.Items) && Cmd.Items.length > 0) {
            Cmd.Items.forEach((childCmd) => {
                let el = tp.Doc.createElement('div');
                let Text = childCmd.Title ? childCmd.Title : childCmd.Name;
                let E = new tp.tpElement(el, { Parent: this.elList, Text: Text });
                E.Cmd = childCmd;
            });
        }
    }

    Handle = null;
    elText = null;
    elList = null;
    Cmd = null;

    get Text() {
        return elText.innerHTML;
    }
    set Text(v) {
        elText.innerHTML = v;
    }


};
//#endregion