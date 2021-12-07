tp.Urls.LocatorGetDef = '/LocatorGetDef';
tp.Urls.LocatorSqlSelect = '/LocatorSqlSelect';

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

            let Args = await tp.Ajax.GetAsync(Url, Data);  //= async function (Url, Data = null

            var o = JSON.parse(Args.ResponseText);
            if (o.IsSuccess === false)
                tp.Throw(o.ErrorText);

            let Packet = JSON.parse(o.Packet);
            Result = new tp.LocatorDef();
            Result.Assign(Packet);
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

        let Args = await tp.Ajax.GetAsync(Url, Data);  //= async function (Url, Data = null

        var o = JSON.parse(Args.ResponseText);
        if (o.IsSuccess === false)
            tp.Throw(o.ErrorText);

        var Table = new tp.DataTable();
        Table.Assign(Args.ResponseData.Packet);

        return Table;
    }
};

/** A list of locator definitions already downloaded from server.
 * @static
 * @type {tp.LocatorDef[]} 
 * */
tp.Locators.Descriptors = [];



tp.LocatorDef = class {

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

tp.LocatorFieldDef = class {

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
    DataField = '';

    /**
    Gets or set the list DataTable, that is the source table where data values come from.
    @type {tp.DataTable}
    */
    ListTable = null;

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
    async SetKeyValueAsync(v) {
        if (this.Active) {

            this.fKeyValue = v;

            if (tp.IsEmpty(v)) {
                this.fListRow = null;
                this.SetDataFieldValues(null);
            }
            else {
                let Flag = await this.LocateKeyAsync();

                if (Flag === true)
                    this.SetDataFieldValues(this.fListRow);
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
 
    /** Ensures that a {@link tp.DropDownBox} is created.
     * @private
     * */
    EnsureDropDownBox() {
        if (tp.IsEmpty(this.fDropDownBox)) {
            this.fDropDownBox = new tp.DropDownBox();
            this.fDropDownBox.Owner = this;

            // this listbox is displayed by the drop-down box
            this.fListBox = new tp.ListBox();
            this.fListBox.ParentHandle = this.fDropDownBox.Handle;
            this.fListBox.Position = 'absolute';
            this.fListBox.Width = '100%';
            this.fListBox.Height = '100%';
            this.fListBox.Handle.style.border = 'none';

            this.fListBox.DataField = this.DataField;
            this.fListBox.DataSource = this.DataSource;

            this.fListBox.On(tp.Events.Click, this.ListBox_Click, this);
        }
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
    async ResetListData() {
        this.ListTable = null;
        let Result = await this.SelectListTableAsync("");
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
    async DisplayListTableAsync(Associate) {
        if (!tp.IsEmpty(this.ListTable)) {
            if (this.ListTable.Rows.length <= tp.SysConfig.LocatorShowDropDownRowCountLimit) {

                // EDW
                this.EnsureDropDownBox();

                if (this.fDropDownBox.IsOpen)
                    this.fDropDownBox.Close();

                await this.LocateKeyAsync();

                this.Assigning = true;  // to avoid calling DataSourceRowModified() with no reason
                try {
                    this.fListBox.ListSource = this.ListTable;

                    this.fDropDownBox.Associate = Associate;
                    this.fDropDownBox.Open();
                } finally {
                    this.Assigning = false;
                }

            } else {
                // TODO: PopUpForm()
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

        switch (Stage) {

            case tp.DropDownBoxStage.Opening:
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
    async ShowListAsync(Associate) {
        if (!tp.IsValid(Associate))
            tp.Throw("No Associate");

        let RowCount = await this.SelectListTableAsync('');

        if (RowCount > 0) {
            await this.DisplayListTableAsync(Associate);
        } else {
            // nothing here
        }
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
    async Box_KeyDown(LocatorLink, Box, FieldDef, e) {
        if (this.Active && (FieldDef instanceof tp.LocatorFieldDef)) {
            if (e.keyCode === tp.Keys.Enter) {
                let S = LocatorLink.BoxGetText(this, Box);
                if (tp.IsBlankString(S)) this.ClearDataValue();

                else {
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

                    let RowCount = await this.SelectListTableAsync(WhereUser);
                    if (RowCount > 0) {
                        if (RowCount === 1) {
                            this.fListRow = this.ListTable.Rows[0];
                            let v = this.fListRow.Get(this.Descriptor.ListKeyField);
                            this.DataSource.Set(this.DataField, v); // this triggers the whole sequence of setting data-fields
                        } else {
                            await this.DisplayListTableAsync(Box);
                        }
                    } else {
                        LocatorLink.BoxSetText(this, Box, '');
                    }
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
    async Box_KeyPress(LocatorLink, Box, FieldDef, e) {
        if (this.Active && (FieldDef instanceof tp.LocatorFieldDef) && FieldDef.Searchable) {

            let KeyCode = 'charCode' in e ? e.charCode : e.keyCode;
            let c = String.fromCharCode(KeyCode);

            if (c === '*') {

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

                let RowCount = await this.SelectListTableAsync(WhereUser);
                if (RowCount > 0) {
                    if (RowCount === 1) {
                        this.fListRow = this.ListTable.Rows[0];
                        let v = this.fListRow.Get(this.Descriptor.ListKeyField);
                        this.DataSource.Set(this.DataField, v); // this triggers the whole sequence of setting data-fields
                    } else {
                        await this.DisplayListTableAsync(Box);
                    }
                } else {
                    LocatorLink.BoxSetText(this, Box, '');
                }
            }
        }

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
                await this.SetKeyValueAsync(this.DataValue);
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

            await this.SetKeyValueAsync(this.DataValue);

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
                await this.SetKeyValueAsync(this.DataValue);
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

//#region tp.LocatorBox
/**
A multi-TextBox Locator control.
It is used in data entry forms representing the value of a single DataColumn and displaying
multiple boxes.
A Locator represents (returns) a single value, but it can handle and display multiple values
in order to help the end user in identifying and locating that single value.
For example, a TRADES data table has a CUSTOMER_ID column, representing that single value, but the user interface
has to display information from the CUSTOMERS table, specifically, the ID, CODE and NAME columns.
The TRADES table is the target data table and the CUSTOMER_ID is the DataField field name.
The CUSTOMERS table is the ListTableName and the ID is the ListKeyField field name.
The fields, ID, CODE and NAME, may be described by individual  field items.
A Locator can be used either as a single-row control, as the LocatorBox does, or as a group of
related columns in a Grid.
NOTE: A Locator of a LocatorBox, may or may not define the LocatorFieldDescriptor.DataField
field names. Usually in a case like that, the data table contains just the key field, the LocatorBox.DataField.  
A Locator of a grid-type must define the names of those fields always and the data table must contain DataColumn columns
on those fields.
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
    async AnyButton_Click(e) {
        if (!this.ReadOnly && this.Enabled && this.IsValidLocator) {
            if (e.target === this.btnZoom) {
                // TODO: Zoom
            } else if (e.target === this.btnList) {
               await this.Locator.ShowListAsync(this.Handle);
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
        this.fDefaultCssClasses = tp.Classes.LocatorBox;

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
    @protected
    @override
    */
    OnResized() {
        this.Layout();
        super.OnResized();
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