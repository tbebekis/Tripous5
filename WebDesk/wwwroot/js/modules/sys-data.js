 

/**
 * Represents a page.
 * @param {HTMLElement} elPage The DOM element.
 */
let SysDataPage = class extends app.Desk.Page {
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

    /** Creates page controls */
    CreateControls() {
        super.CreateControls();
        let el = tp.Select(this.Handle, '.ToolBar');
        this.ToolBar = new tp.ToolBar(el);
    }

    
};




/** The page.
 * @type {SysDataPage}
 */
let Page = null;

/**
    * Starts the page this file handles.
    * @param {HTMLElement} elPage The page element.
    * @param {object} [CreateParams=null] Optional. A javascript object with initialization parameters.
    */
export function StartPage(elPage, CreateParams = null) {
    Page = new SysDataPage(elPage, CreateParams);
};
