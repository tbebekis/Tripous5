tp.Urls.LocatorGetDef = '/LocatorGetDef';

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
                Name: Name,
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
        return tp.IsString(this.fName) && !tp.IsBlank(this.fName) ? this.fName : 'no-descriptor-name';
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
    Finds a {@link tp.LocatorFieldDef}  field descriptor by list field alias and returns the field or null if not found
    @param {string} ListFieldAlias - The alias of the ListField.
    @returns {tp.LocatorFieldDef} Finds a {@link tp.LocatorFieldDef}  field descriptor by list field alias and returns the field or null if not found
    */
    Find(ListFieldAlias) {
        return this.Fields.find((item) => { return tp.IsSameText(ListFieldAlias, item.ListFieldAlias); });
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

    /** When not empty/null then it denotes a field in the dest table where to put the value of this field.
     * @type {string}
     */
    DestField = '';
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
        return tp.IsBlank(this.fTitleKey) ? this.ListFieldAlias : this.fTitleKey;
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