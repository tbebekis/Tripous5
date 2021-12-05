
//#region tp.LocatorDescriptor
/**
Describes a {@link tp.Locator}. <br />  

A locator represents (returns) a single value, but it can handle and display multiple values
in order to help the end user in identifying and locating that single value.  <br />

For example, a TRADES data table has a CUSTOMER_ID column, representing that single value, but the user interface
has to display information from the CUSTOMERS table, specifically, the ID, CODE and NAME columns.

The TRADES table is the target data table and the CUSTOMER_ID is the DataField field name.
The CUSTOMERS table is the ListTableName and the ID is the ListKeyField field name.

The fields, ID, CODE and NAME, may be described by individual LocatorFieldDescriptor field items.  <br />

A locator can be used either as a single-row control, as the LocatorBox does, or as a group of
related columns in a Grid.  <br />
NOTE: A locator of a LocatorBox type, may or may not define the LocatorFieldDescriptor.DataField
field names. Usually in a case like that, the data table contains just the key field, the LocatorBox.DataField.  
A locator of a grid-type must define the names of those fields always and the data table must contain DataColumn columns
on those fields.
 */
tp.LocatorDescriptor = class extends tp.tpObject {

    /**
    Constructor.
    @param {string} [Name] Optional. The name of the locator descriptor.
    @param {string} [ListTableName] Optional.
    @param {string} [ListKeyField] Optional.
    */
    constructor(Name, ListTableName, ListKeyField) {
        super();

        this.Name = Name || tp.NO_NAME;

        this.ConnectionName = tp.SysConfig.DefaultConnection;
        this.ListTableName = ListTableName || Name;
        this.ListKeyField = ListKeyField || 'Id';
        this.ZoomCommand = '';
        this.ReadOnly = false;
        this.SelectSql = new tp.SelectSql();
        this.Fields = [];
        this.OrderBy = '';
    }

    /* private */

    /** Field
    * @private
    * @type {string}
    */
    fName;
    /** Field
    * @private
    * @type {string}
    */
    fTitle;
    /** Field
    * @private
    * @type {string}
    */
    fConnectionName;
    /** Field
    * @private
    * @type {string}
    */
    fListKeyField;
    /** Field
    * @private
    * @type {string}
    */
    fZoomCommand;

    /* properties */
    /**
    Gets or sets the locator descriptor name
    @type {string}
    */
    get Name() {
        return !tp.IsBlank(this.fName) ? this.fName : 'no-descriptor-name';
    }
    set Name(v) {
        this.fName = v;
    }
    /**
    Gets or sets the title of the locator descriptor.
    @type {string}
    */
    get Title() {
        return !tp.IsBlank(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        this.fTitle = v;
    }
    /**
    Gets or sets the connection name  
    @type {string}
    */
    get ConnectionName() {
        return !tp.IsBlank(this.fConnectionName) ? this.fConnectionName : tp.SysConfig.DefaultConnection;
    }
    set ConnectionName(v) {
        this.fConnectionName = v;
    }
    /**
    Gets or sets the name of the list table
    @type {string}
    */
    ListTableName;
    /**
    Gets or sets the key field of the list table. The value of this field goes to the DataField
    */
    get ListKeyField() {
        return !tp.IsBlank(this.fListKeyField) ? this.fListKeyField : 'Id';
    }
    set ListKeyField(v) {
        this.fListKeyField = v;
    }
    /**
    Gets or sets the zoom command name. A user inteface (form) can use this name to call the command.
    @type {string}
    */
    get ZoomCommand() {
        return tp.IsBlank(this.fZoomCommand) ? (tp.IsBlank(this.ListTableName) ? '' : tp.SysConfig.DefaultConnection + "." + this.ListTableName) : this.fZoomCommand;
    }
    set ZoomCommand(v) {
        this.fZoomCommand = v;
    }
    /**
    Indicates whether the locator is readonly
    @type {boolean}
    */
    ReadOnly;
    /**
    If the value of this property is set then the locator does not generates the SELECT automatically.
    @type {tp.SelectSql}
    */
    SelectSql;
    /**
    Gets the list of descriptor fields.
    @type {tp.LocatorFieldDescriptor[]}
    */
    Fields;
    /**
    The order by field when the SELECT Sql is constructed by the Locator. In a description with Id and Name fields could be the ListTableName.Name
    @type {string}
    */
    OrderBy;


    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.LocatorDescriptor';
    }

    /* public */
    /**
    Adds a {@link tp.LocatorFieldDescriptor} field to the locator field list. Returns the newly added field.
    @param {string} DataType - The data type of the field. One of the tp.DataType constants
    @param {string} DataField - The field name of the field in the target data-source
    @param {string} ListField - The field name of the field in the list table.
    @param {string} ListFieldAlias - The alias of the ListField.
    @param {string} ListTableName - The name of the list table
    @param {string} [Title] Optional. The title of the field
    @param {boolean} [Searchable = true] Optional. Defaults to true. When true the field can be part in a where clause in a select statement.
    @param {boolean} [DataVisible = true] Optional. Defaults to true. Indicates whether a TextBox for this field is visible in a LocatorBox
    @param {boolean} [ListVisible = true] Optional. Defaults to true. Indicates whether the field is visible when the list table is displayed
    @returns {tp.LocatorFieldDescriptor} Returns the newly added {@link tp.LocatorFieldDescriptor}  field.
    */
    Add(DataType, DataField, ListField, ListFieldAlias, ListTableName, Title, Searchable, DataVisible, ListVisible) {
        var Result = new tp.LocatorFieldDescriptor();
        Result.Descriptor = this;

        Result.DataType = DataType;
        Result.DataField = DataField;
        Result.ListField = ListField;
        Result.ListFieldAlias = ListFieldAlias;
        Result.ListTableName = ListTableName;
        Result.Title = Title || DataField;
        Result.Searchable = Searchable;
        Result.DataVisible = DataVisible;
        Result.ListVisible = ListVisible;

        this.Fields.push(Result);
        return Result;
    }
    /**
    Finds a {@link tp.LocatorFieldDescriptor}  field descriptor by list field alias and returns the field or null if not found
    @param {string} ListFieldAlias - The alias of the ListField.
    @returns {tp.LocatorFieldDescriptor} Finds a {@link tp.LocatorFieldDescriptor}  field descriptor by list field alias and returns the field or null if not found
    */
    Find(ListFieldAlias) {
        return tp.FirstOrDefault(this.Fields, (item) => { return tp.IsSameText(ListFieldAlias, item.ListFieldAlias); });
    }
    /**
    Finds a {@link tp.LocatorFieldDescriptor} field descriptor by data field and returns the field or null if not found
    @param {string} DataField - The field name of the field in the target data-source
    @returns {tp.LocatorFieldDescriptor} Finds a {@link tp.LocatorFieldDescriptor} field descriptor by data field and returns the field or null if not found
    */
    FindByDataField(DataField) {
        return tp.FirstOrDefault(this.Fields, (item) => { return tp.IsSameText(DataField, item.DataField); });
    }
    /**
    Assigns the properties of this instance from a specified source object.
    @param {object} Source The source to copy property values from.
    */
    Assign(Source) {
        this.Name = Source.Name;

        this.ConnectionName = Source.ConnectionName;
        this.ListTableName = Source.ListTableName;
        this.ListKeyField = Source.ListKeyField;
        this.ZoomCommand = Source.ZoomCommand;
        this.ReadOnly = Source.ReadOnly;
        this.SelectSql = new tp.SelectSql();
        this.SelectSql.Assign(Source.SelectSql);
        this.OrderBy = Source.OrderBy;

        this.Fields = [];

        var i, ln, Field, SF;
        for (i = 0, ln = Source.Fields.length; i < ln; i++) {
            SF = Source.Fields[i];
            Field = this.Add(SF.DataType, SF.DataField, SF.ListField, SF.ListFieldAlias, SF.ListTableName, SF.Title, SF.Searchable, SF.DataVisible, SF.ListVisible);
            Field.TitleKey = SF.TitleKey;
            Field.IsIntegerBoolean = SF.IsIntegerBoolean;
            Field.Width = SF.Width;
        }
    }


};



//#endregion

//#region tp.LocatorFieldDescriptor
/**
Describes the "field" (text box or grid column) of a Locator. A field such that associates a column in the data table (the target) to a column in the list table (the source).
*/
tp.LocatorFieldDescriptor = class extends tp.tpObject {

    /**
    Constructor
    */
    constructor() {
        super();

        this.DataType = tp.DataType.String;
        this.DataField = '';
        this.ListField = '';
        this.ListFieldAlias = '';
        this.ListTableName = '';
        this.TitleKey = '';
        this.Title = '';

        this.DataVisible = true;
        this.ListVisible = true;
        this.Searchable = true;
        this.IsIntegerBoolean = false;

        this.Width = 70;
    }



    /* fields */
    /** Field
    * @private
    * @type {string}
    */
    fListFieldAlias;
    /** Field
    * @private
    * @type {string}
    */
    fListTableName;
    /** Field
    * @private
    * @type {string}
    */
    fTitleKey;

    /** Field
    * @type {tp.LocatorDescriptor}
    */
    Descriptor = null;

    /* properties */
    /**
    Gets or sets the data type of the field. One of the tp.DataType constants
    @type {string}
    */
    DataType;
    /**
    Gets or sets the the name of the field in the data table. It can not be empty for grid-type locators.
    @type {string}
    */
    DataField;
    /**
    Gets or sets the field name of the field in the list table.
    @type {string}
    */
    ListField;
    /**
    Gets or sets the alias of the ListField.
    @type {string}
    */
    get ListFieldAlias() {
        return tp.IsBlank(this.fListFieldAlias) ? this.ListField : this.fListFieldAlias;
    }
    set ListFieldAlias(v) {
        this.fListFieldAlias = v;
    }
    /**
    Gets or sets the  name of the list table
    @type {string}
    */
    get ListTableName() {
        return tp.IsBlank(this.fListTableName) ? (tp.IsEmpty(this.Descriptor) ? '' : this.Descriptor.ListTableName) : this.fListTableName;
    }
    set ListTableName(v) {
        this.fListTableName = v;
    }
    /**
    Gets or sets a resource Key used in returning a localized version of Title.
    @type {string}
    */
    get TitleKey() {
        return tp.IsBlank(this.fTitleKey) ? this.ListFieldAlias : this.fTitleKey;
    }
    set TitleKey(v) {
        this.fTitleKey = v;
    }
    /**
    Gets or sets tha Title of this descriptor, used for display purposes.
    @type {string}
    */
    Title;
    /**
    Indicates whether a TextBox for this field is visible in a LocatorBox
    @type {boolean}
    */
    DataVisible;
    /**
    Indicates whether the field is visible when the list table is displayed
    @type {boolean}
    */
    ListVisible;
    /**
    When true the field can be part in a where clause in a select statement.
    @type {boolean}
    */
    Searchable;
    /**
    Used to notify criterial links to treat the field as an integer boolea fieldn (1 = true, 0 = false)
    @type {boolean}
    */
    IsIntegerBoolean;
    /**
    Controls the width of the text box in a LocatorBox.
    @type {number}
    */
    Width;

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    @override
    */
    InitClass() {
        super.InitClass();
        this.tpClass = 'tp.LocatorFieldDescriptor';
    }

    /**
    Returns a string representation of this instance.
    @returns {string} Returns a string representation of this instance.
    */
    toString() {
        return `${this.ListTableName}.${this.ListFieldAlias}`;
    }
};

tp.LocatorFieldDescriptor.BoxDefaultWidth = 70;
//#endregion


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
    fLocator;
    /** Field
     @private
     @type {tp.LocatorEventType} */
    fEventType;

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
    /**
    Gets or sets the SELECT statement. Used with the AddToSelectSqlWhere type only.
    @type {tp.SelectSql}
    */
    SelectSql;
    /**
    Gets or sets the filter to apply to ListTable.DefaultView.RowFilter
    @type {string}
    */
    ListTableFilter;
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

//#region tp.Locator
/**
The locator represents (returns) a single value. <br />

A locator acts similarly to a lookup combobox, in helping the user to select and return a single value. <br />
It can be used with large data-sets because it constructs and issues SELECT statements to the server and displays the result.  <br />
A locator is just a class, not a user interface control. There are special user interface controls, such as the {@link tp.Grid} and the {@link tp.LocatorBox},
that connect to a locator and help the user to select a value using the locator. <br />

The programmer never creates a {@link tp.Locator} instance. 
A locator instance is created automatically when a {@link tp.LocatorBox} is created and initialized and when a locator column is added to a {@link tp.Grid}. <br />
A locator always works in conjuction with a control, a {@link tp.LocatorBox} or a {@link tp.Grid}, and its Control property returns that control.
From that associated control the locator knows its DataSource, DataTable, DataColumn and DataField. <br />

The locator provides the Descriptor property or type {@link tp.LocatorDescriptor}. 
That descriptor contains the SELECT Sql statement of the locator and the list of its fields. <br />
The SELECT statement returns the ListTable of the locator. 
The ListTable is used as a source by the locator. The locator locates the ListRow and then uses that row in assigning its data fields.
@implements {tp.IDataSourceListener}
@implements {tp.IDropDownBoxListener}
*/
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
        this.fControls = new tp.Dictionary(); // Key = tp.LocatorFieldDescriptor, Value = HTMLInputElement 

        this.fListRow = null;
        this.fListKeyFieldDes = null;

        this.fInDataSourceEvent = false;
        this.fKeyValue = null;

        this.fInitialized = false;
        this.fControl = null;
    }

    /* private */

    /** Field
     * @private
     * @type {tp.Locator}
     */
    fMaster;
    /** Field
     * @private
     * @type {tp.Locator[]}
     */
    fDetailLocators;

    /** Field. The located DataRow in ListTable
     * @private
     * @type {tp.DataRow}
     */
    fListRow;
    /** Field. The field descriptor of the key field of the ListTable
     * @private
     * @type {tp.LocatorFieldDescriptor}
     */
    fListKeyFieldDes;

    /** Field
    * @private
    * @type {boolean}
    */
    fActive; // true
    /** Field
    * @private
    * @type {number}
    */
    fAssigningCount;
    /** Field
    * @private
    * @type {boolean}
    */
    fInDataSourceEvent;

    /** Field
    * @private
    * @type {any}
    */
    fKeyValue;

    /** Field
    * @private
    * @type {boolean}
    */
    fInitialized;
    /** Field
    * @private
    * @type {tp.Control}
    */
    fControl;

    /** Field. A {@link tp.Dictionary} where Key = tp.LocatorFieldDescriptor and Value = HTMLInputElement
    * @private
    * @type {tp.Dictionary}
    */
    fControls;
    /** Field
    * @private
    * @type {tp.DropDownBox}
    */
    fDropDownBox;
    /** Field. This listbox is displayed by the drop-down box
    * @private
    * @type {tp.ListBox}
    */
    fListBox;
    /** Field
    * @private
    * @type {number}
    */
    fMaxDropdownItems;
    /** Field
    * @private
    * @type {tp.LocatorDescriptor}
    */
    fDescriptor;

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
    Gets or sets the descriptor of this locator
    @type {tp.LocatorDescriptor}
    */
    get Descriptor() {
        if (tp.IsEmpty(this.fDescriptor)) {
            this.fDescriptor = new tp.LocatorDescriptor();
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
            if (this.DataField) {
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
    DataField;

    /**
    Gets or set the list DataTable, that is the source table where data values come from.
    @type {tp.DataTable}
    */
    ListTable;

    /**
    A {@link tp.Dictionary} dictionary, where Key = {@link tp.LocatorFieldDescriptor}  and Value = {@link HTMLInputElement}, 
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
    DetailKey;

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
    Returns the key value, the value the locator represents. Could be null. <br />
    NOTE: Treat setter as private and with extreme care.
    CAUTION: Setting this property, ends up setting all DataFields, the DataValue and the text of the controls. <br />
    It may end up calling the server and re-selecting the {@link tp.Locator.ListTable}.
    @type {any}
    */
    get KeyValue() {
        return !tp.IsEmpty(this.fKeyValue) ? this.fKeyValue : null;
    }
    set KeyValue(v) {
        if (this.Active) {

            this.fKeyValue = v;

            if (tp.IsEmpty(v)) {
                this.fListRow = null;
                this.SetDataValues(null);
            }
            else {
                this.LocateKeyAsync()
                    .then((Result) => {
                        if (Result === true)
                            this.SetDataValues(this.fListRow);
                    });
            }
        }
    }
    /** Gets or sets the value of the DataField <br />
     * @type {any}
     */
    get DataValue() {
        return this.Active && !tp.IsEmpty(this.CurrentRow) ? this.CurrentRow.Get(this.DataColumn) : null;
    }

    /** Sets the values of all DataFields, except of the DataField, which is the key field, and the text of the controls, based on a specified {@link tp.DataRow} that comes from the ListTable.
     * @private
     * @param {tp.DataRow} SourceRow A {@link tp.DataRow} that comes from the ListTable.
     */
    SetDataValues(SourceRow) {
        if (this.Active && !this.Assigning && !tp.IsEmpty(this.fListKeyFieldDes)) {
            this.Assigning = true;
            try {
                //let S = this.IsGridMode ? "Grid" : "LocatorBox";

                let Row = this.CurrentRow;

                if (!tp.IsEmpty(Row)) {

                    let Clearing = tp.IsEmpty(SourceRow);

                    let i, ln, v,
                        SubControl,         // HTMLInputElement
                        FieldDes,           // tp.LocatorFieldDescriptor
                        Column,             // tp.DataColumn
                        Dest,               // string
                        Source;             // string                    

                    for (i = 0, ln = this.Descriptor.Fields.length; i < ln; i++) {

                        FieldDes = this.Descriptor.Fields[i];

                        // data field
                        if (FieldDes !== this.fListKeyFieldDes && !tp.IsBlank(FieldDes.DataField) && Row.Table.ContainsColumn(FieldDes.DataField)) {
                            if (Clearing) {
                                Row.Set(FieldDes.DataField, null);
                            } else {
                                if (SourceRow.Table.ContainsColumn(FieldDes.ListFieldAlias)) {
                                    v = SourceRow.Get(FieldDes.ListFieldAlias);
                                    Row.Set(FieldDes.DataField, v);
                                }
                            }
                        }

                        // sub-controls
                        SubControl = this.Controls.ContainsKey(FieldDes) ? this.Controls.Get(FieldDes) : null;

                        if (SubControl) {
                            if (Clearing) {
                                SubControl.value = '';
                            } else {
                                Column = SourceRow.Table.FindColumn(FieldDes.ListFieldAlias);
                                if (Column) {
                                    v = SourceRow.Get(FieldDes.ListFieldAlias);
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
                this.fListRow = this.ListTable.Locate(this.fListKeyFieldDes.ListFieldAlias, this.KeyValue);
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

        if (this.Active && !tp.IsEmpty(this.KeyValue) && !tp.IsEmpty(this.fListKeyFieldDes)) {

            this.fListRow = null;

            if (this.LocateListRowInternal()) {
                return true;
            }
            else {

                // then re-select
                try {
                    let WhereUser = tp.Format("{0}.{1}", this.fListKeyFieldDes.ListTableName, this.fListKeyFieldDes.ListField);
                    let v = this.KeyValue;

                    if (this.fListKeyFieldDes.DataType === tp.DataType.String)
                        WhereUser += tp.Format(" = '{0}' ", v.toString());
                    else if (tp.DataType.IsDateTime(this.fListKeyFieldDes.DataType))
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

            }
        }

        return false;

    }

    /* engine */
    /**
    Constructs and returns the {@link tp.SelectSql} statement that is going to be executed.
    WARNING:  we assume that all fields belong to the same table, the FListSourceName.
    For more complex SELECTs the user must provide the descriptor.SelectSql manually
    @private
    @returns {tp.SelectSql} Returns the {@link tp.SelectSql} statement that is going to be executed
    */
    ConstructSelectSql() {

        let Result = this.Descriptor.SelectSql.Clone();

        if (tp.Trim(Result.Select).length <= 0) {

            /* WARNING: we assume that all fields belong to the same table, the FListSourceName. 
               For more complex SELECTs the user must provide the descriptor.SelectSql manually */
            if (!tp.IsEmpty(this.Descriptor.ListTableName)) {
                let S = '';
                let FieldDes; // LocatorFieldDescriptor;
                for (let i = 0, ln = this.Descriptor.Fields.length; i < ln; i++) {
                    FieldDes = this.Descriptor.Fields[i];
                    S = S + "  " + this.Descriptor.ListTableName + "." + FieldDes.ListField + " as " + FieldDes.ListFieldAlias + ", " + '\r\n';
                }

                S = tp.TrimEnd(S);

                if (S.length === 0)
                    S = "*";

                S = tp.RemoveLastComma(S);

                Result.Select = S;
                Result.From = this.Descriptor.ListTableName;

                if (!tp.IsBlank(this.Descriptor.OrderBy))
                    Result.AddToOrderBy(this.Descriptor.OrderBy);
            }

        }

        let Args = new tp.LocatorEventArgs(this, tp.LocatorEventType.AddToSelectSqlWhere);
        Args.SelectSql = Result;
        this.OnAnyEvent(Args);

        return Args.SelectSql;
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

            this.fListBox.DataField = this.DataField;
            this.fListBox.DataSource = this.DataSource;

            this.fListBox.On(tp.Events.Click, this.FuncBind(this.ListBox_Click));
        }
    }

    /** Sets-up the list table
     * @private
     * */
    SetupListTable() {
        if (!tp.IsEmpty(this.ListTable)) {
            this.FilterListTable();

            let FieldDes; // LocatorFieldDescriptor;
            let Column; // tp.DataColumn;
            for (let i = 0, ln = this.ListTable.Columns.length; i < ln; i++) {
                Column = this.ListTable.Columns[i];
                FieldDes = this.Descriptor.Find(Column.Name);

                Column.Visible = (FieldDes !== null) && FieldDes.ListVisible;
                if (Column.Visible === true) {
                    Column.Title = '';
                    Column.Title = FieldDes.Title;
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
        return this.SelectListTableAsync("");
    }
    /**
    Displays the result of a SELECT either as drop-down list or in a locator dialog
    @private
    @param {HTMLElement} Associate The associate control to set to the drop-down box
    */
    DisplayListTable(Associate) {
        if (!tp.IsEmpty(this.ListTable)) {
            if (this.ListTable.Rows.length <= tp.SysConfig.LocatorShowDropDownRowCountLimit) {

                this.EnsureDropDownBox();

                if (this.fDropDownBox.IsOpen)
                    this.fDropDownBox.Close();

                this.LocateKeyAsync()
                    .then(() => {
                        this.Assigning = true;  // to avoid calling DataSourceRowModified() with no reason
                        try {
                            this.fListBox.ListSource = this.ListTable;

                            this.fDropDownBox.Associate = Associate;
                            this.fDropDownBox.Open();
                        } finally {
                            this.Assigning = false;
                        }
                    });
            } else {
                // TODO: PopUpForm()
            }
        }

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
            let SS = this.ConstructSelectSql();

            if (!tp.IsBlank(WhereUser))
                SS.WhereUser = WhereUser;

            let Args = new tp.LocatorEventArgs(this, tp.LocatorEventType.SelectListTable);
            Args.SelectSql = SS;

            // this may assign the ListTable
            this.OnAnyEvent(Args);

            if (tp.IsEmpty(this.ListTable)) {
                let Table = await tp.Db.SelectAsync(SS.Text, this.Descriptor.ConnectionName);
                this.ListTable = Table;
            }

            this.SetupListTable();

            return this.ListTable.Rows.length;
        }

        return 0;
    }

    /**
    Forces a re-filtering of list table
    @private
    */
    MasterDataValueChanged() {
        this.FilterListTable();
        this.SetDataValues(null);
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
        if (!this.Initialized && !tp.IsEmpty(this.Control) && !tp.IsEmpty(this.Descriptor) && !tp.IsBlank(this.DataField)) {
            let DataSource = this.Control.DataSource;
            if (DataSource && DataSource.Table instanceof tp.DataTable) {
                this.fListKeyFieldDes = this.Descriptor.Find(this.Descriptor.ListKeyField);

                if (this.fListKeyFieldDes) {
                    let Column = DataSource.Table.FindColumn(this.DataField);
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
        this.SelectListTableAsync('')
            .then((RowCount) => {
                if (RowCount > 0) {
                    this.DisplayListTable(Associate);
                } else {
                    // nothing here
                }
            });
    }

    /* boxes */
    /**
     * Event handler
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDescriptor} FieldDes The {@link tp.LocatorFieldDescriptor} descriptor
     */
    Box_Enter(LocatorLink, Box, FieldDes) {
        if (!this.Initialized)
            this.Initialize();

        if (this.Active) {
            //this.fLastEnteredText = LocatorLink.BoxGetText(this, Box);

            let S = this.DataValueAsTextOf(FieldDes.DataField);
            LocatorLink.BoxSetText(this, Box, S);

            if (this.IsBoxMode)
                LocatorLink.BoxSelectAll(this, Box);
        }
    }
    /**
     * Event handler
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDescriptor} FieldDes The {@link tp.LocatorFieldDescriptor} descriptor
     */
    Box_Leave(LocatorLink, Box, FieldDes) {
        if (this.Active && !this.Assigning) {

            // when the user alters the text and then just leaves the text box
            if (this.IsBoxMode) {
                let S = this.DataValueAsTextOf(FieldDes.DataField);
                let S2 = LocatorLink.BoxGetText(this, Box);
                if (S !== S2) {
                    LocatorLink.BoxSetText(this, Box, S);
                }
            }
        }
    }
    /**
     * Event handler
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDescriptor} FieldDes The {@link tp.LocatorFieldDescriptor} descriptor
     * @param {KeyboardEvent} e The {@link KeyboardEvent} event object
     */
    Box_KeyDown(LocatorLink, Box, FieldDes, e) {
    }
    /**
     * Event handler
     * @param {tp.ILocatorLink} LocatorLink The {@link tp.ILocatorLink} locator link
     * @param {HTMLInputElement} Box The text box
     * @param {tp.LocatorFieldDescriptor} FieldDes The {@link tp.LocatorFieldDescriptor} descriptor
     * @param {KeyboardEvent} e The {@link KeyboardEvent} event object
     */
    Box_KeyPress(LocatorLink, Box, FieldDes, e) {
        if (this.Active && (FieldDes !== null) && FieldDes.Searchable) {

            let KeyCode = 'charCode' in e ? e.charCode : e.keyCode;
            let c = String.fromCharCode(KeyCode);

            if (c === '*') {

                let WhereUser = '';
                let Text = LocatorLink.BoxGetText(this, Box);

                if (!tp.IsBlank(Text)) {

                    WhereUser = tp.Format("{0}.{1}", FieldDes.ListTableName, FieldDes.ListField);

                    if (FieldDes.DataType === tp.DataType.String)
                        WhereUser += tp.Format(" like '{0}%' ", Text);
                    else if (tp.DataType.IsDateTime(FieldDes.DataType))
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
    DataSourceRowAdded(Table, Row) {

        if (!this.fInDataSourceEvent && this.Active && this.DataSource.Position >= 0) {
            this.fInDataSourceEvent = true;
            try {
                this.KeyValue = this.DataValue;
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
    DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
        if (!this.Assigning && this.Active && Row === this.CurrentRow && Column === this.DataColumn) {
            this.KeyValue = this.DataValue;

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
    DataSourcePositionChanged(Table, Row, Position) {
        if (this.IsBoxMode && !this.Initialized) {
            this.Initialize();
        }

        if (!this.fInDataSourceEvent && this.Active && Position >= 0) {
            //let S = this.IsGridMode ? "Grid" : "LocatorBox";
            this.fInDataSourceEvent = true;
            try {
                this.KeyValue = this.DataValue;
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
                tp.Registry.FindLocatorAsync(v)
                    .then((Des) => {
                        this.LocatorDescriptor = Des;
                    });
            }
        }
    }
    /**
    Gets or sets the locator descriptor.
    @type {tp.LocatorDescriptor}
    */
    get LocatorDescriptor() {
        return this.Locator.Descriptor;
    }
    set LocatorDescriptor(v) {
        if (v !== this.LocatorDescriptor && v instanceof tp.LocatorDescriptor) {
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
            let FieldDes;   // tp.LocatorFieldDescriptor;

            if (this.IsValidLocator) {

                for (let i = 0, ln = this.Locator.Descriptor.Fields.length; i < ln; i++) {
                    FieldDes = this.Locator.Descriptor.Fields[i];

                    if (FieldDes.DataVisible) {
                        tp.LocatorBox.BoxCount++;

                        Box = this.Handle.ownerDocument.createElement('input');
                        Box.type = 'text';
                        this.Handle.appendChild(Box);
                        this.fBoxes.push(Box);
                        tp.SetObject(Box, FieldDes);

                        Box.tabIndex = 0;
                        ReadOnly = this.Locator.Descriptor.ReadOnly || (FieldDes.Searchable === false);
                        tp.ReadOnly(Box, ReadOnly);
                        Box.title = tp.Format("{0} - ({1}.{2} -> {3})) - [Locator: {4}]", FieldDes.Title, FieldDes.ListTableName, FieldDes.ListField, this.DataField, this.Locator.Descriptor.Name);

                        this.fWidths.Set(Box, FieldDes.Width > 0 ? FieldDes.Width : tp.LocatorFieldDescriptor.BoxDefaultWidth);

                        tp.On(Box, tp.Events.Focus, this.FuncBind(this.Box_Enter));
                        tp.On(Box, tp.Events.LostFocus, this.FuncBind(this.Box_Leave));
                        tp.On(Box, tp.Events.KeyDown, this.FuncBind(this.Box_KeyDown));
                        tp.On(Box, tp.Events.KeyPress, this.FuncBind(this.Box_KeyPress));

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

                    this.fWidths.Set(Box, tp.LocatorFieldDescriptor.BoxDefaultWidth);
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

                this.btnList.addEventListener('click', this.FuncBind(this.AnyButton_Click));
                this.btnZoom.addEventListener('click', this.FuncBind(this.AnyButton_Click));
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

        let DescriptorReadOnly = this.IsValidLocator ? this.Locator.Descriptor.ReadOnly : true;// !tp.IsEmpty(this.Locator) ? this.Locator.Descriptor.ReadOnly : false;
        let Enabled = this.ReadOnly === false && this.Enabled === true && DescriptorReadOnly === false;

        tp.Enabled(this.btnList, Enabled);
        tp.Enabled(this.btnZoom, Enabled);

        let FieldDes;   // LocatorFieldDescriptor;
        let Box;        // HTMLInputElement;
        for (let i = 0, ln = this.fBoxes.length; i < ln; i++) {
            Box = this.fBoxes[i];

            FieldDes = tp.GetObject(Box);
            if (!tp.IsEmpty(FieldDes)) {
                ReadOnly = this.ReadOnly === true || !Enabled || FieldDes.Searchable === false;
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
            let FieldDes = tp.GetObject(textBox); // tp.LocatorFieldDescriptor;
            this.Locator.Box_Enter(this, textBox, FieldDes);
        }
    }
    /** Event handler
    * @private
    * @param {Event} e The {@link Event} object.
    * */
    Box_Leave(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDes = tp.GetObject(textBox); // tp.LocatorFieldDescriptor;
            this.Locator.Box_Leave(this, textBox, FieldDes);
        }
    }
    /** Event handler
    * @private
    * @param {KeyboardEvent} e The {@link KeyboardEvent} object.
    * */
    Box_KeyDown(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDes = tp.GetObject(textBox); // tp.LocatorFieldDescriptor;
            this.Locator.Box_KeyDown(this, textBox, FieldDes, e);
        }
    }
    /** Event handler
    * @private
    * @param {KeyboardEvent} e The {@link KeyboardEvent} object.
    * */
    Box_KeyPress(e) {
        let textBox = e.target;

        if (!tp.ReadOnly(textBox) && tp.Enabled(textBox) && !this.ReadOnly && this.Enabled && this.IsValidLocator) {
            let FieldDes = tp.GetObject(textBox); // tp.LocatorFieldDescriptor;
            this.Locator.Box_KeyPress(this, textBox, FieldDes, e);
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
tp.LocatorBox.prototype.fValue;
/** Field
 * @private
 * @type {tp.Locator}
 */
tp.LocatorBox.prototype.fLocator;
/** Field
 * @private
 * @type {tp.LocatorDescriptor}
 */
tp.LocatorBox.prototype.fLocatorDescriptor;
/** Field
 * @private
 * @type {HTMLInputElement[]}
 */
tp.LocatorBox.prototype.fBoxes;
/** Field. A {tp.Dictionary} with Key = HTMLInputElement, Value = number.
 * @private
 * @type {tp.Dictionary}
 */
tp.LocatorBox.prototype.fWidths;
/** Field
 * @private
 * @type {HTMLElement}
 */
tp.LocatorBox.prototype.btnList;
/** Field
 * @private
 * @type {HTMLElement}
 */
tp.LocatorBox.prototype.btnZoom;
/** Field
 * @private
 * @type {boolean}
 */
tp.LocatorBox.prototype.fCreating;
/** Field
 * @private
 * @type {boolean}
 */
tp.LocatorBox.prototype.fLayouting;

//#endregion