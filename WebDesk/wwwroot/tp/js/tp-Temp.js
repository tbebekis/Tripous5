
//#region DatabaseFieldEditDialog

tp.DatabaseFieldEditDialog = class extends tp.Window {
    /**
     * Constructor
     * @param {tp.WindowArgs} Args The window args
     */
    constructor(Args) {
        super(Args);
    }

    /** The parent element of controls
    * @type {HTMLElement}
    */
    ContentRow = null;
    /** The datasource controls are bind to.
    * @type {tp.DataSource}
    */
    DataSource = null;

    /* overrides */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.DatabaseFieldEditDialog';
    }
    ProcessInitInfo() {
        super.ProcessInitInfo();

        this.tblField = this.Args.Instance;
        this.IsInsertMode = this.Args.IsInsertMode;
    }
    /**
     * Creates all controls of this window.
     * */
    CreateControls() {
        super.CreateControls();

        this.CreateFooterButton('OK', 'OK', tp.DialogResult.OK);
        this.CreateFooterButton('Cancel', _L('Cancel'), tp.DialogResult.Cancel);

        let ContentHtmlText;
        let HtmlText;
        let HtmlRowList = [];

        let ColumnNames = [];       // Visible Controls
        let EditableColumns = []    // Editable Controls

        ColumnNames = ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'ForeignKeyConstraintName', 'Unique', 'UniqueConstraintName'];

        // inserting or editing?
        EditableColumns = this.IsInsertMode === true ?
            ['Name', 'TitleKey', 'DataType', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique'] :
            ['Name', 'TitleKey', 'Length', 'Required', 'DefaultExpression', 'ForeignKey', 'Unique'];
 

        this.DataSource = new tp.DataSource(this.tblField);

        // Editable Controls
        this.tblField.Columns.forEach((column) => {
            if (column.Name === 'Name')
                column.MaxLength = 30;

            column.ReadOnly = EditableColumns.indexOf(column.Name) < 0;
        });

        // Visible Controls
        // prepare HTML text for each column in tblFields
        ColumnNames.forEach((ColumnName) => {
            let Column = this.tblField.FindColumn(ColumnName);
            let IsCheckBox = Column.DataType === tp.DataType.Boolean;

            let Text = Column.Title;
            let Ctrl = {
                TypeName: Column.Name === 'DataType' ? 'ComboBox' : tp.DataTypeToUiType(Column.DataType),
                TableName: this.tblField.Name,
                DataField: Column.Name
            };

            if (ColumnName === 'DataType') {
                Ctrl.ListOnly = true;
                Ctrl.ListValueField = 'Id';
                Ctrl.ListDisplayField = 'Name';
                Ctrl.ListSourceName = 'DataType';
            }

            // <div class="tp-CtrlRow" data-setup="{Text: 'Id', Control: { TypeName: 'TextBox', Id: 'Code', DataField: 'Code', ReadOnly: true } }"></div>
            HtmlText = tp.CtrlRow.GetHtml(IsCheckBox, Text, Ctrl);
            HtmlRowList.push(HtmlText);
        });


        // join html text for all control rows
        HtmlText = HtmlRowList.join('\n');

        // content
        let RowHtmlText = `
<div class="Row" data-setup='{Breakpoints: [450, 768, 1050, 1480]}'>
    <div class="Col" data-setup='{ControlWidthPercents: [100, 60, 60, 60, 60]}'>
        ${HtmlText}
    </div>
</div>
`;
        this.ContentRow = tp.HtmlToElement(RowHtmlText);  
        this.ContentWrapper.Handle.appendChild(this.ContentRow);

        tp.Ui.CreateContainerControls(this.ContentRow.parentElement);
 
    }


    /**
     * To be used by modal dialogs in passing values from the object being edited to controls.
     * This method is called after the window becomes visible.
     */
    ItemToControls() {

        tp.StyleProp(this.ContentWrapper.Handle, 'padding', '5px');

        // force tp-Cols to adjust
        this.BroadcastSizeModeChanged();

        // bind dialog controls
        tp.BindAllDataControls(this.ContentRow, (DataSourceName) => {
            if (DataSourceName === 'Field')
                return this.DataSource;

            if (DataSourceName === 'DataType') {
                let Result = new tp.DataSource(tp.EnumToLookUpTable(tp.DataType, [tp.DataType.Unknown]));
                return Result;
            }

            return null;
        });
    }
    /**
     * To be used by modal dialogs in passing values from controls to the object being edited, in case of a valid DialogResult.
     * This method is called just before setting the DialogResult property.
     * NOTE: Throwing an exception from inside this method cancels the setting of the DialogResult property.
     */
    ControlsToItem() {

        /** @type {tp.DataRow} */
        let Row = this.tblField.Rows[0];
 
        let Errors = [];

        // Name
        let v = Row.Get('Name', '');
        if (tp.IsBlank(v)) {
            Errors.push('\nName is required'); 
        }
 
        if (tp.IsString(v) && !tp.IsValidIdentifier(v, '$')) {
            Errors.push('\nName should start with _ or letter \nand cannot contain spaces, special characters and punctuation.');
        }

        // Length
        v = Row.Get('DataType', '');
        if (v === tp.DataType.String) {
            v = Row.Get('Length', 0);
            if (v <= 0) {
                Errors.push('\nInvalid Length');
            }
        }

        if (Errors.length > 0) {
            let Message = Errors.join('\n');
            Message += '\n'
            tp.Throw(Message);
        }


        v = Row.Get('TitleKey', '');
        if (tp.IsBlank(v)) {
            Row.Set('TitleKey', Row.Get('Name', ''));
        }
    }

}

/** When true then dialog is in insert mode, else in edit mode
 * @type {boolean}
 * */
tp.DatabaseFieldEditDialog.prototype.IsInsertMode = false;
/** The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
 * @type {tp.DataTable}   
 */
tp.DatabaseFieldEditDialog.prototype.tblField = null;



/**
Displays a modal dialog box for editing a {@link tp.SqlBrokerFieldDef} object
@static
@param {tp.DataTable} tblField The {@link tp.DataTable} that is going to be edited. Actually the first and only row it contains.
@param {boolean} IsInsertMode When true then dialog is in insert mode, else in edit mode
@returns {tp.SqlBrokerFieldDefEditDialog} Returns the {@link tp.Window}  dialog box
*/
tp.DatabaseFieldEditDialog.ShowModalAsync = function (tblField, IsInsertMode) {

    let BodyWidth = tp.Doc.body.offsetWidth
    let w = BodyWidth <= 580 ? BodyWidth - 6 : 580;

    let WindowArgs = new tp.WindowArgs({ Text: 'Edit Database Field', Width: w, Height: 'auto', IsInsertMode: IsInsertMode });
 
    return tp.Window.ShowModalForAsync(tblField, tp.DatabaseFieldEditDialog, WindowArgs);
};

//#endregion

