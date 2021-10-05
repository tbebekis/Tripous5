 

/** Represents a view. Displays a list of items of a certain DataType. */
app.SysDataViewList = class extends app.DeskView {
    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [Params=null] Optional. A javascript object with initialization parameters.
     */
    constructor(elPage, CreateParams = null) {
        super(elPage, CreateParams); 
    }

    /**
     * @type {tp.ToolBar}
     */
    ToolBar = null;
    /**
     * @type {tp.Grid}
     */
    Grid = null;
    /**
     * @type {tp.DataTable}
     */
    Table = null;

    /** Creates page controls */
    CreateControls() {
        super.CreateControls();
        let el = tp.Select(this.Handle, '.tp-ToolBar');
        this.ToolBar = new tp.ToolBar(el);

        this.ToolBar.On('ButtonClick', this.AnyToolBarButtonClick, this);

        this.Table = new tp.DataTable();
        this.Table.Assign(this.CreateParams.Packet.Table);

        el = tp.Select(this.Handle, '.tp-Grid');
        
        let CP = {
            DataSource: this.Table,
            ReadOnly: true,
            ToolBarVisible: false,
            GroupsVisible: false,
            FilterVisible: false,
            FooterVisible: false,
            Columns: [ 
                { Name: 'DataName' },
                { Name: 'TitleKey' },
                { Name: 'Owner' }
            ]
        };

        this.Grid = new tp.Grid(el, CP);
    }

    async ShowModal(IsInsert) {

        // get the packet
        let DataType = this.Setup.DataType;
        let Cmd = new app.Command();
        Cmd.Name = IsInsert === true ? `Ui.SysData.Insert.${DataType}` : `Ui.SysData.Edit.${DataType}`;
        if (IsInsert !== true) {
            let Row = this.Grid.DataSource.Current;
            Cmd.Params.Id = Row.GetByName('Id');
        }

        let Packet = await this.AjaxExecuteDialog(Cmd);

        // create the page element
        let elPage = app.GetContentElement(Packet.HtmlText.trim());
 
        let DeskInfo = {};
        DeskInfo.Name = Packet.ViewName; 
        DeskInfo.elPage = elPage;
        app.DeskInfo(elPage, DeskInfo);

        // create the page instance
        let CreateParams = {
            Name: Packet.ViewName,
            Packet: Packet,
        };

        let Page = await app.Desk.Instance.CreateViewObject(elPage, CreateParams);

        // display the dialog
        let WindowArgs = await tp.ContentWindow.ShowModalAsync(Packet.ViewName, elPage);

        let DialogResult = WindowArgs.Window.DialogResult;
        let S = tp.EnumNameOf(tp.DialogResult, DialogResult);
        tp.InfoNote('DialogResult = ' + S);
 
        return DialogResult;
    }
    async AnyToolBarButtonClick(Args) {
        let Command = Args.Command;
        switch (Command) {
            case 'Close':
                this.Close();
                break;
            case 'Insert':
                await this.ShowModal(true);
                break;
            default:
                tp.InfoNote('Command: ' + Command);
                break;
        }        
    }
};

/** Represents a page. Edit/Insert view of the Table DataType. */
app.SysDataViewEditTable = class extends app.DeskView {
    /**
     * Constructs the page
     * @param {HTMLElement} elPage The page element.
     * @param {object} [Params=null] Optional. A javascript object with initialization parameters.
     */
    constructor(elPage, CreateParams = null) {
        super(elPage, CreateParams);
    }
};





