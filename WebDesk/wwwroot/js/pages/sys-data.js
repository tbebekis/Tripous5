﻿ 

/**
 * Represents a page.
 * @param {HTMLElement} elPage The DOM element.
 */
app.SysDataPage = class extends app.Desk.Page {
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
        

        this.Grid = new tp.Grid(el);

        this.Grid.DataSource = this.Table;
 
        this.Grid.ReadOnly = false;
        this.Grid.ToolBarVisible = true;
        this.Grid.ButtonInsertVisible = true;
        this.Grid.ButtonDeleteVisible = true;
    }
 
    async AnyToolBarButtonClick(Args) {
        var Command = Args.Command;
        switch (Command) {
            case 'Close':
                this.Close();
                break;
            default:
                tp.InfoNote('Command: ' + Command);
                break;
        }        
    }
};




