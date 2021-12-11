
/* eslint no-constant-condition: ["error", { "checkLoops": false }] */

//#region tp.Sql
/**
 * A static helper class for working with sql statements
 * @class
 * @static
 * @hideconstructor
 * */
tp.Sql = class {

    constructor() {
        throw 'Can not create an instance of a full static class';
    }

    /**
    Returns true if a specified string value contains any of the mask characters (%, ?, *)
    @param {string} v - The string value to check
    @returns {boolean} Returns true if a specified string value contains any of the mask characters (%, ?, *)
    */
    static IsMasked (v) {
        if (tp.IsBlank(v))
            return false;

        if (v.indexOf('*') !== -1)
            return true;

        if (v.indexOf('?') !== -1)
            return true;

        if (v.indexOf('%') !== -1)
            return true;

        return false;
    }

    /**
     * Normalizes a string value for use in a LIKE clause. It returns a string as <code>LIKE 'VALUE%'</code>
     * The specified value may or may not contain mask characters (%, ?, *) <br />
     * @param {string} Value The string value to operate on.
     */
    static NormalizeMaskForce(Value) {
        if (tp.IsString(Value)) {
            Value = Value.trim();

            if (Value.length > 0) {
                let SB = new tp.StringBuilder();
                SB.Append(Value);
                SB.Replace('*', '%');
                SB.Replace('?', '%');
                Value = SB.ToString();

                if (Value.indexOf('%') === -1)
                    Value = Value + '%';

                return ` like '${Value}' `;
            }
        }

        return ` like '__not__existed__' `;  
    }


/*
        /// <summary>
        /// Normalizes Value for use in a LIKE clause. It returns a string as <code>LIKE 'VALUE%'</code>
        /// <para>Value may or may not contain mask characters (%, ?, *)</para>
        /// </summary>
        static public string NormalizeMaskForce(string Value)
        {
            if (Value != null)
            {
                Value = Value.Trim();

                if (Value.Length > 0)
                {
                    StringBuilder SB = new StringBuilder(Value);
                    SB.Replace('*', '%');
                    SB.Replace('?', '%');
                    Value = SB.ToString();

                    if (Value.IndexOf('%') != -1)
                        return string.Format(" like '{0}' ", Value);
                    else
                        return string.Format(" like '{0}' ", Value + @"%");

                }
            }

            return " like '__not__existed__' ";
        }
 */
};
//#endregion  

//#region tp.Sql.Token
tp.Sql.Token = {
    None: 0,
    Select: 1,
    From: 2,
    Where: 3,
    GroupBy: 4,
    Having: 5,
    OrderBy: 6
};
tp.Sql.Tokens = ["select", "from", "where", "group by", "having", "order by"];
//#endregion  

//#region tp.DataType
/**
A static enum-like class to be used for database types
@class
*/
tp.DataType = {

    /**
     * Unknown, none.
     * @type {string}
     */
    get Unknown() { return "Unknown"; },          
    /**
     *"string", "nvarchar", "nchar", "varchar", "char"
     * @type {string}
     */
    get String() { return "String"; },         
    /**
     * "integer", "int", "larginteger", "largint", "smallint", "autoinc", "autoincrement", "identity", "counter"
     * @type {string}
     */
    get Integer() { return "Integer"; },          
    /**
     * "float", "double", "extended", "real", "BCD", "FBCD", "currency", "money"
     * @type {string}
     */
    get Float() { return "Float"; },         
    /**
     * decimal(18, 4)
     * @type {string}
     */
    get Decimal() { return "Decimal"; },           
    /**
     * Date (date)
     * @type {string}
     */
    get Date() { return "Date"; },          
    /**
     * "moment", "datetime", "timestamp"
     * @type {string}
     */
    get DateTime() { return "DateTime"; },       
    /**
     * Boolean (integer always, 1 = true, else false)
     * @type {string}
     */
    get Boolean() { return "Boolean"; },         
    /**
     * "blob", "graphic", "image", "bin", "binary"
     * @type {string}
     */
    get Blob() { return "Blob"; },            
    /**
     * "memo", "text", "clob"
     * @type {string}
     */
    get Memo() { return "Memo"; },            
 
    

/*
    get None() { return "N"; }, // = "N";          // none
    get String() { return "S"; }, //= "S";         // {"string", "nvarchar", "nchar", "varchar", "char"}
    get Integer() { return "I"; }, // = "I";       // {"integer", "int", "larginteger", "largint", "smallint", "autoinc", "autoincrement", "identity", "counter"}
    get Boolean() { return "L"; }, // = "L";       // {"boolean", "bit", "logical"}
    get Float() { return "F"; }, // = "F";         // {"float", "double", "extended", "real", "BCD", "FBCD", "currency", "money"}
    get Date() { return "D"; }, // = "D";          // {"date"}
    get Time() { return "T"; }, // = "T";          // {"time"}
    get DateTime() { return "M"; }, // = "M";      // {"moment", "datetime", "timestamp"}
    get Memo() { return "X"; }, // = "X";          // {"memo", "text", "clob"}
    get Graphic() { return "G"; }, // = "G";       // {"graphic", "image"}
    get Blob() { return "B"; }, // = "B";          // {"blob", "bin", "binary"}
 */

    /** Converts a DataType value to json string
    @param {string} v - One of the DataType string constants
    @returns {string} Returns a json equivalent
    */
    TypeToJson(v) {
        if (v === tp.DataType.String || v === tp.DataType.Memo)
            return "string";
        if (v === tp.DataType.Integer || v === tp.DataType.Float || v === tp.DataType.Decimal)
            return "number";
        if (v === tp.DataType.DateTime || v === tp.DataType.Date)
            return "date";
        if (v === tp.DataType.Boolean)
            return "boolean";

        return "string";
    },
    /** Returns the name (property name) of a DataType value
    @param {string} v - One of the DataType string constants
    @returns {string} Returns the name (property name) of the DataType value
    */
    TypeName(v) {
        for (var Prop in tp.DataType) {
            if (tp.DataType[Prop] === v)
                return Prop;
        }
        return 'Unknown';
    },
    /**
    True if the specified data type is one of the constants of this class
    @param {string} v - One of the DataType string constants
    @returns {boolean} Returns true if the specified data type is one of the constants of this class
    */
    IsValid(v)  {
        return v === tp.DataType.String
            || v === tp.DataType.Integer
            || v === tp.DataType.Float
            || v === tp.DataType.Decimal
            || v === tp.DataType.Date
            || v === tp.DataType.DateTime
            || v === tp.DataType.Boolean
            || v === tp.DataType.Blob
            || v === tp.DataType.Memo
            ;

    },
    /**
    Returns a bit-field number with the valid aggregate types of a specified data type
    @param {string} v - One of the DataType string constants
    @returns {number} Returns a bit-field number with the valid aggregate types of a specified data type
    */
    ValidAggregates(v) {

        switch (v) {

            case tp.DataType.Integer:
            case tp.DataType.Float:
            case tp.DataType.Decimal:
                return tp.AggregateType.Count
                    | tp.AggregateType.Avg
                    | tp.AggregateType.Sum
                    | tp.AggregateType.Max
                    | tp.AggregateType.Min
                    ;

            case tp.DataType.Date:
            case tp.DataType.DateTime:
                return tp.AggregateType.Count
                    | tp.AggregateType.Max
                    | tp.AggregateType.Min
                    ;
        }

        return tp.AggregateType.Count;
    },
    /**
    Returns the default alignmet (left or right) of a specified data type
    @param {string} v - One of the DataType string constants
    @returns {number} Returns the default alignmet (left or right) of a specified data type
    */
    DefaultAlignment(v) {
        switch (v) {
            case tp.DataType.String: return tp.Alignment.Left;
            case tp.DataType.Integer:
            case tp.DataType.Float:
            case tp.DataType.Decimal:
            case tp.DataType.Date: 
            case tp.DataType.DateTime: return tp.Alignment.Right;
            default: return tp.Alignment.Mid;
        }
    },
    /**
    Returns true if a specified data type is sortable
    @param {string} v - One of the DataType string constants
    @returns {boolean} Returns true if a specified data type is sortable
    */
    IsSortableType(v) {
        return v === tp.DataType.String
            || v === tp.DataType.Integer
            || v === tp.DataType.Boolean
            || v === tp.DataType.Float
            || v === tp.DataType.Decimal
            || v === tp.DataType.Date
            || v === tp.DataType.DateTime
            ;
    },

    /**
    Returns true if a specified data type is string
    @param {string} v - One of the DataType string constants
    @returns {boolean} Returns true if a specified data type is string
    */
    IsString(v) {
        return v === this.String;
    },
    /**
    Returns true if a specified data type is a number (Integer, Float or Decimal)
    @param {string} v - One of the DataType string constants
    @returns {boolean} Returns true if a specified data type is a number (Integer, Float or Decimal)
    */
    IsNumber(v) {
        return v === this.Integer || v === this.Float || v === this.Decimal;
    },
    /**
    Returns true if a specified data type is date-time, date or time.
    @param {string} v - One of the DataType string constants
    @returns {boolean} Returns true if a specified data type is date-time, date.
    */
    IsDateTime(v) {
        return v === this.DateTime || v === this.Date;
    }
     
};
Object.freeze(tp.DataType);
//#endregion  



//#region tp.DataRowState
/**
A static enum-like class. Indicates the state of a DataRow
@class
@enum {number}
*/
tp.DataRowState = {
    Detached: 1,
    Unchanged: 2,
    Added: 4,
    Deleted: 8,
    Modified: 16
};
Object.freeze(tp.DataRowState);
//#endregion  

//#region tp.AggregateType
/** A static enum-like class. Indicates the type of an aggregate function
 @class
 @enum {number}
 */
tp.AggregateType = {
    None: 0,
    Count: 1,
    Avg: 2,
    Sum: 4,
    Max: 8,
    Min: 16
};
Object.freeze(tp.AggregateType);

/** An array with valid aggregate function names 
 @constant
 @type {string[]}
 */
tp.ValidAggregateFunctions = ["", "count", "avg", "sum", "max", "min"];
//#endregion  

//#region  tp.DateRange
/** An enum-like static class
 * @class
 * @enum {numer}
 * */
tp.DateRange = {
    Custom: 0,
    Today: 1,
    Yesterday: 2,
    Tomorrow: 3,
    LastWeek: 4,
    LastTwoWeeks: 5,
    LastMonth: 6,
    LastTwoMonths: 7,
    LastThreeMonths: 8,
    LastSemester: 9,
    LastYear: 10,
    LastTwoYears: 11,
    NextWeek: 12,
    NextTwoWeeks: 13,
    NextMonth: 14,
    NextTwoMonths: 15,
    NextThreeMonths: 16,
    NextSemester: 17,
    NextYear: 18,
    NextTwoYears: 19
};
Object.freeze(tp.DateRange);
//#endregion 

//#region tp.DateRanges
/**
A static helper class for working with date ranges
*/
tp.DateRanges = {

    /**
     @constant
     */
    PrefixFrom: "FROM_DATE_RANGE_",
    /**
     @constant
     */
    PrefixTo: "TO_DATE_RANGE_",

    WhereRanges: [
        tp.DateRange.Custom,
        tp.DateRange.Today,
        tp.DateRange.Yesterday,
        tp.DateRange.LastWeek,
        tp.DateRange.LastTwoWeeks,
        tp.DateRange.LastMonth,
        tp.DateRange.LastTwoMonths,
        tp.DateRange.LastThreeMonths,
        tp.DateRange.LastSemester,
        tp.DateRange.LastYear,
        tp.DateRange.LastTwoYears
    ],
    WhereRangesTexts: [
        'Custom',         
        'Today',          
        'Yesterday',      
        'LastWeek',       
        'LastTwoWeeks',   
        'LastMonth',      
        'LastTwoMonths',  
        'LastThreeMonths',
        'LastSemester',   
        'LastYear',       
        'LastTwoYears'
    ],

    /**
    Returns true if a specified {@link tp.DateRange} value denotes a period in the past.
    @param {Number} Range - One of the {@link tp.DateRange} constants
    @returns {boolean} Returns true if a specified {@link tp.DateRange} value denotes a period in the past.
    */
    IsPast(Range) {

        switch (Range) {
            case tp.DateRange.Today:
            case tp.DateRange.Yesterday:
            case tp.DateRange.LastWeek:
            case tp.DateRange.LastTwoWeeks:
            case tp.DateRange.LastMonth:
            case tp.DateRange.LastTwoMonths:
            case tp.DateRange.LastThreeMonths:
            case tp.DateRange.LastSemester:
            case tp.DateRange.LastYear:
            case tp.DateRange.LastTwoYears:
                return true;
        }

        return false;
    },
    /**
     * Converts a {@link tp.DateRange} value to two DateTime values. <br />
     * Returns an object with 3 properties FromDate, ToDate and Result. Result indicates the success or the failure.
     * @param {number} Range One of the {@link tp.DateRange} constants.
     * @param {Date} [Today=null] Optional. The date to be used as the 'today' date.
     * @returns {object} Returns an object with 3 properties FromDate, ToDate and Result. Result indicates the success or the failure.
     */
    ToDates(Range, Today = null) {
        if (!tp.IsValid(Today))
            Today = tp.Today();

        let Result = true;
        let FromDate = new Date(tp.ClearTime(Today));
        let ToDate = new Date(tp.ClearTime(Today));

        if (tp.IsInteger(Range)) {
            switch (Range) {
                case tp.DateRange.Today: break;
                case tp.DateRange.Yesterday: { FromDate = tp.AddDays(FromDate, -1); ToDate = tp.AddDays(ToDate, -1); } break;
                case tp.DateRange.Tomorrow: { FromDate = tp.AddDays(FromDate, 1); ToDate = tp.AddDays(ToDate, 1); } break;

                case tp.DateRange.LastWeek: FromDate = tp.AddDays(FromDate, -7); break;
                case tp.DateRange.LastTwoWeeks: FromDate = tp.AddDays(FromDate, -14); break;
                case tp.DateRange.LastMonth: FromDate = tp.AddDays(FromDate, -30); break;
                case tp.DateRange.LastTwoMonths: FromDate = tp.AddDays(FromDate, -60); break;
                case tp.DateRange.LastThreeMonths: FromDate = tp.AddDays(FromDate, -90); break;
                case tp.DateRange.LastSemester: FromDate = tp.AddDays(FromDate, -180); break;
                case tp.DateRange.LastYear: FromDate = tp.AddDays(FromDate, -365); break;
                case tp.DateRange.LastTwoYears: FromDate = tp.AddDays(FromDate, -730); break;

                case tp.DateRange.NextWeek: ToDate = tp.AddDays(ToDate, 7); break;
                case tp.DateRange.NextTwoWeeks: ToDate = tp.AddDays(ToDate, 14); break;
                case tp.DateRange.NextMonth: ToDate = tp.AddDays(ToDate, 30); break;
                case tp.DateRange.NextTwoMonths: ToDate = tp.AddDays(ToDate, 60); break;
                case tp.DateRange.NextThreeMonths: ToDate = tp.AddDays(ToDate, 90); break;
                case tp.DateRange.NextSemester: ToDate = tp.AddDays(ToDate, 180); break;
                case tp.DateRange.NextYear: ToDate = tp.AddDays(ToDate, 365); break;
                case tp.DateRange.NextTwoYears: ToDate = tp.AddDays(ToDate, 730); break;

                default: Result = false; break;
            }
        } else {
            Result = false;
        }


        return {
            FromDate: FromDate,
            ToDate: ToDate,
            Result: Result
        };
    }

 
 
};
//#endregion  

//#region  tp.WhereOperator
tp.WhereOperator = {
    And: 1,
    Not: 2,
    Or: 4
};
Object.freeze(tp.WhereOperator);
//#endregion 

//#region tp.ColumnDisplayType
/**
A static enum-like class. The display type of a column. Used with grids.
@class
@enum {number}
*/
tp.ColumnDisplayType = {
    Default: 0,
    DateTime: 1,
    Date: 2,
    Time: 3,
    CheckBox: 4,
    Memo: 5,
    Image: 6
};
Object.freeze(tp.ColumnDisplayType);
//#endregion


//#region tp.SelectSqlColumn
/**
Settings for a grid column
*/
tp.SelectSqlColumn = class {
    /**
    Constructor
    @param {string} [Name] Column name
    */
    constructor() {       
    }

    /**
    Assigns a source item to this instance
    @param {tp.SelectSqlColumn} Source The source to copy values from
    */
    Assign(Source) {
        this.Name = Source.Name;
        this.DisplayType = Source.DisplayType;
        this.Title = Source.Title;

        this.Visible = Source.Visible;
        this.Width = Source.Width;
        this.ReadOnly = Source.ReadOnly;
        this.DisplayIndex = Source.DisplayIndex;
        this.GroupIndex = Source.GroupIndex;
        this.Decimals = Source.Decimals;
        this.FormatString = Source.FormatString;
        this.Aggregate = Source.Aggregate;
        this.AggregateFormat = Source.AggregateFormat;        
    }
};
/** Name
 @type {string}
 * */
tp.SelectSqlColumn.prototype.Name = '';
/** The display type of a column. Used with grids.
 @type {tp.ColumnDisplayType.Default}
 * */
tp.SelectSqlColumn.prototype.DisplayType = tp.ColumnDisplayType.Default;
/** Title
 @type {string}
 * */
tp.SelectSqlColumn.prototype.Title = '';

/** Controls the visibility of the column
 @type {boolean}
 */
tp.SelectSqlColumn.prototype.Visible = true;
/** Width of the column
 @type {number}
 */
tp.SelectSqlColumn.prototype.Width = 90;
/** When true the column is read-only
 @type {boolean}
 */
tp.SelectSqlColumn.prototype.ReadOnly = true;
/** Display index
 @type {number}
 */
tp.SelectSqlColumn.prototype.DisplayIndex = 0;
/** Group index. -1 means not defined
 @type {number}
 */
tp.SelectSqlColumn.prototype.GroupIndex = -1;    
/** Decimal places to use when displaying float numbers. -1 means not defined
 @type {number}
 */
tp.SelectSqlColumn.prototype.Decimals = -1;      
/** Format string of the value
 @type {string}
 */
tp.SelectSqlColumn.prototype.FormatString = '';
/** The aggregate function to use
 @type {tp.AggregateType}
 */
tp.SelectSqlColumn.prototype.Aggregate = tp.AggregateType.None;
/** The aggregate format to use
 @type {number}
 */
tp.SelectSqlColumn.prototype.AggregateFormat = '';
//#endregion  

 



//#region tp.SelectSqlParser
/**
A SELECT sql statement parser capable to parse simple SELECT statements.
CAUTION: Do NOT use it with nested SELECT statements or other complex SELECT statements.
*/
tp.SelectSqlParser = class {

    /**
    Constructor
    @param {string} [Text] The SELECT Sql statement to parse
    */
    constructor(Text) {
        this.Parse(Text);
    }



    /** 
     Gets the SELECT Sql statement
     @type {string}
     */
    get Text() {
        return this.fText;
    }
    /**
     Gets the SELECT clause of the Sql statement
     @type {string}
     */
    get Select() {
        return this.fSelect;
    }
    /**
     Gets the FROM clause of the Sql statement
     @type {string}
     */
    get From() {
        return this.fFrom;
    }
    /**
    Gets the WHERE clause of the Sql statement
    @type {string}
    */
    get Where() {
        return this.fWhere;
    }
    /**
    Gets the GROUP BY clause of the Sql statement
    @type {string}
    */
    get GroupBy() {
        return this.fGroupBy;
    }
    /**
    Gets the HAVING clause of the Sql statement
    @type {string}
    */
    get Having() {
        return this.fHaving;
    }
    /**
    Gets the ORDER BY clause of the Sql statement
    @type {string}
    */
    get OrderBy() {
        return this.fOrderBy;
    }

    /**
    Parses a specified SELECT sql statement string and returns the parser.
    @param {string} Text - The sql statement to parse
    @return {tp.SelectSqlParser} Returns this.
    */
    Parse(Text) {
        this.fText = Text || '';
        this.fSelect = '';
        this.fFrom = '';
        this.fWhere = '';
        this.fGroupBy = '';
        this.fHaving = '';
        this.fOrderBy = '';

        this.curToken = tp.Sql.Token.None;
        this.curPos = 0;
        this.lastPos = -1;

        this.fText = ' ' + this.fText;

        while (this.curToken <= tp.Sql.Token.OrderBy)
            if (!this.NextTokenEnd())
                break;

        this.TokenChange(tp.Sql.Token.None, '');

        return this;
    }

    /** Skips characters by increasing the current position (curPos) until the SkipChar is found. Returns the current position at Text.
     * @private
     * @param {char} SkipChar The character to find while skiping
     * @returns {number}  Returns the current position at Text.
     */
    SkipToChar(SkipChar) {
        let C;
        this.curPos++;

        while (this.curPos <= this.fText.length - 1) {
            C = this.fText.charAt(this.curPos);
            if (C === SkipChar) {
                this.curPos++;
                break;
            }

            this.curPos++;
        }

        return this.curPos;

    }
    /**
     * Returns true if sToken is at the current position (curPos) in text
     * @private
     * @param {string} sToken A token
     * @returns {boolean} Returns true if sToken is at the current position (curPos) in text
     */
    FindTokenAtCurrentPos(sToken) {
        if (this.curPos + sToken.length <= this.fText.length - 1) {
            try {
                return this.fText.toLowerCase().indexOf(sToken, this.curPos) === this.curPos;
            } catch (e) {
                //
            }
        }

        return false;
    }
    /**
     * Performs a "token change". Makes the NewToken the curToken, copies a part of the text string setting the last clause string and adjusts curPos, lastPos and curToken.
     * @private
     * @param {tp.Sql.Token} NewToken One of the tp.Sql.Token constants
     * @param {string} sNewToken The token string
     */
    TokenChange(NewToken, sNewToken) {
        let LastIndex = this.lastPos + (this.curPos - this.lastPos);
        let S = this.fText.substring(this.lastPos, LastIndex);

        switch (this.curToken) {
            case tp.Sql.Token.Select:
                this.fSelect = S;
                break;

            case tp.Sql.Token.From:
                this.fFrom = S;
                break;

            case tp.Sql.Token.Where:
                this.fWhere = S;
                break;

            case tp.Sql.Token.GroupBy:
                this.fGroupBy = S;
                break;

            case tp.Sql.Token.Having:
                this.fHaving = S;
                break;

            case tp.Sql.Token.OrderBy:
                this.fOrderBy = S;
                break;

        }
        this.curPos += sNewToken.length;
        this.lastPos = this.curPos;
        this.curToken = NewToken;
    }
    /**
     * The actual parsing procedure. Returns true only when finds one of the token strings (SELECT, FROM, WHERE etc).
     * @private
     * @return {boolean} Returns true only when finds one of the token strings (SELECT, FROM, WHERE etc).
     * */
    NextTokenEnd() {
 

        let Result = false;
        let ParenCount = 0;
        let C;


        while (this.curPos <= this.fText.length - 1) {
            C = this.fText.charAt(this.curPos);

            if (C === '"')
                this.curPos = this.SkipToChar('"');
            else if (C === '[')
                this.curPos = this.SkipToChar(']');
            else if (C === '(') {
                this.curPos++;
                ParenCount++;
            }
            else if (C === ')') {
                this.curPos++;
                ParenCount--;
                if (ParenCount < 0)
                    throw "SelectSqlParser: Wrong parentheses";
            }
            else if (!tp.IsWhitespaceChar(C) && ((this.curPos - 1) >= 0) && tp.IsWhitespaceChar(this.fText.charAt(this.curPos - 1))) {
                if (ParenCount === 0) {
                    for (let i = 0; i < tp.Sql.Tokens.length; i++) {
                        if (this.FindTokenAtCurrentPos(tp.Sql.Tokens[i])) {
                            this.TokenChange(i + 1, tp.Sql.Tokens[i]);
                            return true;
                        }
                    }
                }

                this.curPos++;
            }
            else {
                this.curPos++;
            }
        }


        return Result;
    }


    /**
    Parses a specified SELECT sql statement string and returns the parser.
    @param {string} Text - The sql statement to parse
    @returns {tp.SelectSqlParser} Returns a tp.SelectSqlParser
    */
    static Parse(Text) {
        return new tp.SelectSqlParser(Text);
    }

};
/* private */
tp.SelectSqlParser.prototype.fText = '';
tp.SelectSqlParser.prototype.fSelect = '';
tp.SelectSqlParser.prototype.fFrom = '';
tp.SelectSqlParser.prototype.fWhere = '';
tp.SelectSqlParser.prototype.fGroupBy = '';
tp.SelectSqlParser.prototype.fHaving = '';
tp.SelectSqlParser.prototype.fOrderBy = '';
tp.SelectSqlParser.prototype.curToken = tp.Sql.Token.None;
tp.SelectSqlParser.prototype.curPos = 0;
tp.SelectSqlParser.prototype.lastPos = -1;
//#endregion  

//#region tp.ParseSelectSql
/**
Parses a simple SELECT sql statement and returns the parser with the results.
CAUTION: Do NOT use it with nested SELECT statements or other complex SELECT statements.
@param {string} Text - The sql statement to parse
@returns {tp.SelectSqlParser} Returns a tp.SelectSqlParser
*/
tp.ParseSelectSql = function (Text) {
    return tp.SelectSqlParser.Parse(Text);
};
//#endregion  

//#region tp.SqlFilterMode
/** A static enum-like class. Indicates the type of an Sql filter
 * @class
 * @enum {number}
 * */
tp.SqlFilterMode = {
    None: 0,
    Simple: 1,
    EnumQuery: 2,
    EnumConst: 4,
    Locator: 8
};
Object.freeze(tp.SqlFilterMode);
//#endregion  

//#region tp.SqlFilterEnum
/**
Represents the enum settings of a Sql filter descriptor
*/
tp.SqlFilterEnum = class {

    /**
    Constructor
    */
    constructor() {
    }
 
    /**
    Assigns source to this instance
    @param {tp.SqlFilterEnum} Source The source tp.SqlFilterEnum
    */
    Assign(Source) {
        for (var key in Source) {
            if (typeof Source[key] !== 'function')
                this[key] = Source[key];
        }
    }
};

/** SQL SELECT statement of an EnumQuery filter.
 * @type {string}
 */
tp.SqlFilterEnum.prototype.Sql = '';                    
/** The result field
 * @type {string}
 */
tp.SqlFilterEnum.prototype.ResultField = 'Id';           
/** For EnumConst and EnumQuery only items. When true the user may select multiple items.
 * @type {boolean}
 */
tp.SqlFilterEnum.prototype.IsMultiChoise = false;         
/** list of constant options. Used only when the is an EnumConst filter.
 * @type {string[]}
 */
tp.SqlFilterEnum.prototype.OptionList = [];
/** When true, constant options are displayed initially to the user as checked.
 * @type {boolean}
 */
tp.SqlFilterEnum.prototype.IncludeAll = true;            
/**  A list where each line is FIELD_NAME=Title
 * @type {string}
 */
tp.SqlFilterEnum.prototype.DisplayLabels = '';
//#endregion  

//#region tp.SqlFilterDef
/**
Describes a single entry of a WHERE clause of a SELECT sql statement.
*/
tp.SqlFilterDef = class   {
    /**
    Constructor
    @param {string} [FieldPath] The field path, i.e. TableName.FieldName or just FieldName
    */
    constructor(FieldPath) {
        this.Enum = new tp.SqlFilterEnum();
        this.FieldPath = FieldPath || '';
    }

    /**
     * Creates and returns a {@link tp.SqlFilterDef} descriptor.
     * @param {string} FieldPath The full path to the field, i.e. TableAlias.FieldName, or just FieldName
     * @param {string} Title The Title of this instance, used for display purposes
     * @param {string} DataType Datatype. One of the values of the properties of the {@link tp.DataType}
     */
    static Create(FieldPath, Title, DataType = tp.DataType.String) {
        let Result = new tp.SqlFilterDef(FieldPath);
        Result.Title = Title || FieldPath;
        Result.DataType = DataType;
        return Result;
    }

    /**
    Assigns a source item to this instance
    @param {tp.SqlFilterDef} Source The source to copy from
    */
    Assign(Source) {

        this.FieldPath = Source.FieldPath;
        this.Title = Source.Title;
 

        this.DataType = Source.DataType;
        this.Mode = Source.Mode;
        
        this.UseRange = Source.UseRange;
        this.Locator = Source.Locator;
        this.PutInHaving = Source.PutInHaving;
        this.AggregateFunc = Source.AggregateFunc;

        this.InitialValue = Source.InitialValue;

        if (tp.IsValid(Source.Enum)) {
            this.Enum.Assign(Source.Enum);
        }
    }

    ValidateAggregateFunc() {
        this.AggregateFunc = tp.ValidAggregateFunctions.indexOf(this.AggregateFunc) === -1? '': this.AggregateFunc;
    }
    
};


 

 
/**
The full path to the field, i.e. TableAlias.FieldName, or just FieldName
@type {string}
*/
tp.SqlFilterDef.prototype.FieldPath = '';
/**
The Title of this instance, used for display purposes
@type {string}
*/
tp.SqlFilterDef.prototype.Title = '';
 

/**
* Datatype. One of the values of the properties of the {@link tp.DataType}
* @type {string}
*/
tp.SqlFilterDef.prototype.DataType = tp.DataType.String;
/**
Indicates how the user enters of selects the filter value. One of the {@link tp.SqlFilterMode} constants.
@type {number}
*/
tp.SqlFilterDef.prototype.Mode = tp.SqlFilterMode.Simple;

/**
If true then range is used. Valid ONLY when Mode is Simple and DataType String, Integer, Float or Decimal. <br />
Date and Time are ALWAYS used as a range from-to. <br />
Defaults to false. <br />
@type {boolean}
*/
tp.SqlFilterDef.prototype.UseRange = false;
/**
The locator name, when mode Locator
@type {string}
*/
tp.SqlFilterDef.prototype.Locator = '';
/**
If true then the result string of this criterion goes to the HAVING clause of a SELECT statement.
@type {boolean}
*/
tp.SqlFilterDef.prototype.PutInHaving = false;
/**
the aggregation function (sum, count, avg, min, max) to use, PutInHaving is true. It could be an empty string.
@type {string}
*/
tp.SqlFilterDef.prototype.AggregateFunc = '';
/**
Returns the filter enum settings, that is special settings object when Mode is EnumQuery or EnumConst
@type {tp.SqlFilterEnum}
*/
tp.SqlFilterDef.prototype.Enum = new tp.SqlFilterEnum();

/**
 
@type {string}
*/
tp.SqlFilterDef.prototype.InitialValue = '';

//#endregion  


//#region tp.SelectSql
/**
Represents a SELECT statement
*/
tp.SelectSql = class {
    /**
    Constructor
    @param {string} [StatementText] The SELECT Sql statement text
    */
    constructor(StatementText) {
        this.Name = tp.Names.Next('SelectSql');
        this.Columns = [];
        this.Filters = [];
        this.Text = StatementText || '';        
    }

 
    /**
    Gets or sets the SELECT statement text
    @type {string}
    */
    get Text() {
        return tp.IsBlank(this.Select) ? '' : this.GetSqlText();
    }
    set Text(v) {
        this.Parse(v);
    }
    /**
    True when the statement is empty
    @type {boolean}
    */
    get IsEmpty() {
        return tp.IsBlank(this.Text);
    }
    /**
    True when the DisplayLabels is not empty
    @type {boolean}
    */
    get HasDisplayLabels() {
        return !tp.IsBlank(this.DisplayLabels);
    }
    /**
    True when has ColumnSettings
    
    */
    get HasColumnSettings() {
        return !tp.IsEmpty(this.fColumnSettings) && (this.fColumnSettings.length > 0);
    }
    /**
    True when has SqlFilters
     @type {boolean}
    */
    get HasSqlFilters() {
        return !tp.IsEmpty(this.fSqlFilters) && (this.fSqlFilters.length > 0);
    }

    /* protected */
    /** Adds a carriage return (Line Break) to the end of a string
     * @protected
     * @param {string} S The string to operate on
     * @returns {string} Returns the new string
     */
    CR(S) {
        if (!tp.IsBlank(S))
            S = tp.TrimEnd(S) + tp.LB;

        return S;
    }
    /** Concatenates Clause + Delimiter + Plus
     * @protected
     * @param {string} Clause Clause
     * @param {string} Delimiter Delimiter
     * @param {string} Plus Plus text
     * @returns {string} Returns the new string
     */
    AddTo(Clause, Delimiter, Plus) {
        Clause = Clause || '';

        if (tp.Trim(Clause) === '')
            return this.SPACES + tp.Trim(Plus);

        Clause = tp.TrimEnd(Clause);
        Delimiter = tp.Trim(Delimiter);
        Plus = tp.Trim(Plus);

        if (tp.EndsWith(Clause, Delimiter, true) || tp.StartsWith(Plus, Delimiter, true))
            return this.CR(Clause) + this.SPACES + tp.Trim(Plus);
        else
            return this.CR(Clause) + this.SPACES + Delimiter + SPACE + Plus;
    }
    /**
     * Concatenates Keyword + Clause
     * @protected
     * @param {string} Clause Clause
     * @param {string} Keyword Keyword
     * @returns {string} Returns the new string
     */
    NormalizeClause(Clause, Keyword) {
        Clause = Clause || '';
        Keyword = Keyword || '';

        Clause = tp.Trim(Clause);
        Keyword = tp.Trim(Keyword);

        if (!tp.IsBlank(Clause) && !tp.IsBlank(Keyword)) {
            if (!tp.StartsWith(Clause, Keyword, true))
                return this.CR(Keyword) + this.SPACES + Clause;
        }

        return Clause;
    }
    /** Constructs and returns a DateTime param pair (date range params) suitable for thw WHERE part in a SelectSql
     * @protected
     * @param {tp.DateRange} Range One of the {@link tp.DateRange} constants
     * @param {string} FieldName A field name
     * @returns {string} Returns a DateTime param pair (date range params) suitable for thw WHERE part in a SelectSql
     */
     DateRangeConstructWhereParams(Range, FieldName) {

        var sFrom = ":" + tp.DateRanges.PrefixFrom + tp.EnumNameOf(tp.DateRange, Range);
        var sTo = ":" + tp.DateRanges.PrefixTo + tp.EnumNameOf(tp.DateRange, Range);

        var S = tp.Format(" (({0} >= {1}) and ({0} <= {2})) ", FieldName, sFrom, sTo);

        return S;
    }

    /* public */
    /**
    Assigns this instance from a source object
    @param {tp.SelectSql} Source The source object to copy from
    */
    Assign(Source) {
        this.Name = Source.Name;
        this.Title = Source.Title;
        this.TitleKey = Source.TitleKey;

        this.CompanyAware = Source.CompanyAware;
        this.ConnectionName = Source.ConnectionName;

        this.DateRange = Source.DateRange;
        this.DateRangeColumn = Source.DateRangeColumn;

        this.Columns = [];
        if (!tp.IsEmpty(Source.Columns) && (Source.Columns.length > 0)) {
            Source.Columns.forEach(item => {
                let Item = new tp.SelectSqlColumn();
                this.Columns.push(Item);
                Item.Assign(item);
            });
        }

        this.Filters = [];
        if (!tp.IsEmpty(Source.Filters) && (Source.Filters.length > 0)) {
            Source.Filters.forEach(item => {
                let Item = new tp.SqlFilterDef();
                this.Filters.push(Item);
                Item.Assign(item);
            });
        }

        this.Text = Source.Text;

    }
    /**
    Clones this instance
    @returns {tp.SelectSql} Returns the clone object
    */
    Clone() {
        let Result = new tp.SelectSql();
        Result.Assign(this);
        return Result;
    }

    /**
    Returns the SELECT statement
    @returns {string}  Returns the SELECT statement
    */
    GetSqlText() {
        // select
        var sSelect = this.NormalizeClause(this.Select, "select") + this.SPACES;

        // from
        var sFrom = this.NormalizeClause(this.From, "from") + this.SPACES;

        // where
        var sWhere = tp.IsBlank(this.Where) ? '' : tp.Trim(this.Where);

        if (this.CompanyAware) {
            var sCompany = tp.SysConfig.CompanyFieldName + tp.Format(" = {0}{1}", tp.SysConfig.VariablesPrefix, tp.SysConfig.CompanyFieldName);

            if (!tp.ContainsText(sWhere, tp.Trim(sCompany)))
                sWhere = sWhere.length === 0 ? sCompany : this.AddTo(sWhere, "and", sCompany);
        }

        if (!tp.IsBlank(this.DateRangeColumn) && tp.DateRanges.IsPast(this.DateRange)) {
            var Range = this.DateRangeConstructWhereParams(this.DateRange, this.DateRangeColumn);

            if (!tp.ContainsText(sWhere, tp.Trim(Range)))
                sWhere = sWhere.length === 0 ? Range : this.AddTo(sWhere, "and", Range);
        }

        if (tp.Trim(this.WhereUser).length > 0) {
            if (!tp.ContainsText(sWhere, tp.Trim(this.WhereUser)))
                sWhere = sWhere.length === 0 ? this.WhereUser : this.AddTo(sWhere, "and", this.WhereUser);
        }


        sWhere = this.NormalizeClause(sWhere, "where");

        // group by
        var sGroupBy = this.NormalizeClause(this.GroupBy, "group by");

        // having
        var sHaving = this.NormalizeClause(this.Having, "having");

        // order by
        var sOrderBy = this.NormalizeClause(this.OrderBy, "order by");

        var Result = tp.TrimEnd(sSelect) + tp.LB + tp.TrimEnd(sFrom);
        if (tp.Trim(sWhere).length > 0) Result += tp.LB + tp.TrimEnd(sWhere) + this.SPACES;  // tp.LB +
        if (tp.Trim(sGroupBy).length > 0) Result += tp.LB + tp.TrimEnd(sGroupBy) + this.SPACES;
        if (tp.Trim(sHaving).length > 0) Result += tp.LB + tp.TrimEnd(sHaving) + this.SPACES;
        if (tp.Trim(sOrderBy).length > 0) Result += tp.LB + tp.TrimEnd(sOrderBy) + this.SPACES;

        return Result;
    }
    /**
    Parses StatementText and assigns its clause properties.
    @param {string} StatementText The SELECT Sql statement text
    */
    Parse(StatementText) {
        this.Select = '';
        this.From = '';
        this.Where = '';
        this.WhereUser = '';
        this.GroupBy = '';
        this.Having = '';
        this.OrderBy = '';

        if (!tp.IsBlank(StatementText)) {
            var Parser = tp.ParseSelectSql(StatementText);
            this.Select = Parser.Select;
            this.From = Parser.From;
            this.Where = Parser.Where;
            this.GroupBy = Parser.GroupBy;
            this.Having = Parser.Having;
            this.OrderBy = Parser.OrderBy;
        }
    }
    /**
    Creates a select * from TableName statement and then calls Parse()
    @param {string} TableName The table name to use 
    */
    ParseFromTableName(TableName) {
        this.Parse("select * from " + TableName);
    }

    /**
    Concatenates WHERE + and + Plus
    @param {string} Plus The text to add to WHERE clause
    @param {string} [Delimiter="and"] The delimiter to use when adding the text. Defaults to 'and'
    */
    AddToWhere(Plus, Delimiter) {

        Delimiter = Delimiter || 'and';
        this.Where = this.AddTo(this.Where, Delimiter, Plus);
    }
    /**
    Concatenates WHERE + or + Plus
    @param {string} Plus The text to add to WHERE clause
    */
    OrToWhere(Plus) {
        this.Where = this.AddTo(this.Where, "or", Plus);
    }
    /**
    Concatenates GROUP BY + , + Plus
    @param {string} Plus The text to add to GROUP BY clause
    */
    AddToGroupBy(Plus) {
        this.GroupBy = this.AddTo(this.GroupBy, ",", Plus);
    }
    /**
    Concatenates HAVING + and + Plus
    @param {string} Plus The text to add to HAVING clause
    */
    AddToHaving(Plus) {
        this.Having = this.AddTo(this.Having, "and", Plus);
    }
    /**
    Concatenates ORDER BY + , + Plus
    @param {string} Plus The text to add to ORDER BY clause
    */
    AddToOrderBy(Plus) {
        this.OrderBy = this.AddTo(this.OrderBy, ",", Plus);
    }
    /**
     Returns the SELECT and FROM only clauses concatenated.
     @returns {string} Returns the SELECT and FROM only clauses concatenated.
    */
    SelectFromToString() {
        return this.NormalizeClause(this.Select, "select") + tp.LB + this.NormalizeClause(this.From, "from");
    }

    /**
    Tries to get and return the main table name from the statement
    @returns {string} Returns the main table name from the statement or empty string.
    */
    GetMainTableName() {
        var S = tp.Trim(this.From);

        if (!tp.IsBlank(S)) {
            while (tp.ContainsText(S, "  "))
                S = tp.ReplaceAll(S, "  ", " ");

            var Parts = tp.Split(S, ' ');
            if ((Parts !== null) && (Parts.length > 0))
                return Parts[0];
        }

        return '';
    }
    /**
    Sets-up the column types, the captions and the visibility of Table.Columns.
    @param {tp.DataTable} Table The table to operate on 
    */
    SetupTable(Table) {
        let i, ln, SqlColumn;
        if (tp.IsArray(this.Columns) && this.Columns.length > 0) {

            Table.Columns.forEach((TableColumn) => {
                SqlColumn = this.Columns.find((item) => { return tp.IsSameText(item.Name, TableColumn.Name); });
                if (!tp.IsValid(SqlColumn)) {
                    TableColumn.Visible = false;
                }
                else {
                    TableColumn.Visible = true;
                    TableColumn.Title = SqlColumn.Title;
                    TableColumn.DisplayType = SqlColumn.DisplayType; 
                }

            });
        }
    }

 
};
/** Spaces 
 @constant
 @type {string}
 */
tp.SelectSql.prototype.SPACES = '  ';

/* properties */
/**
Name
@type {string}
*/
tp.SelectSql.prototype.Name = '';
/**
Title
@type {string}
*/
tp.SelectSql.prototype.Title = '';
/**
TitleKey. Used when inserting a new instance or altering an existend.
@type {string}
*/
tp.SelectSql.prototype.TitleKey = '';


/**
When true then adds a company related part in the WHERE statement, i.e. CompanyFieldName = :CompanyFieldName
@type {boolean}
*/
tp.SelectSql.prototype.CompanyAware = false;
/**
The connection name 
@type {string}
*/
tp.SelectSql.prototype.ConnectionName = tp.SysConfig.DefaultConnection;

/**
It works in conjuction with DateRangeColumn property in order to produce a fixed part in the WHERE clause of this select statement.
@type {tp.DateRange}
*/
tp.SelectSql.prototype.DateRange = tp.DateRange.LastWeek;
/**
A fully qualified (i.e. TABLE_NAME.FIELD_NAME) column of type date or datetime
@type {string}
*/
tp.SelectSql.prototype.DateRangeColumn = '';


/** The list of column descriptors of columns to display. If null or empty, then all columns are displayed. Else only the columns defined in this list are displayed.
 * @private
 * @type {tp.SelectSqlColumn[]} */
tp.SelectSql.prototype.Columns = [];
/** The filter descriptors used to generate the "user where" clause. User's where is appended to the WHERE clause.
 * @private
 * @type {tp.SqlFilterDef[]} */
tp.SelectSql.prototype.Filters = [];


/**
Ges or sets a statement part
@type {string}
*/
tp.SelectSql.prototype.Select = '';
/**
Ges or sets a statement part
@type {string}
*/
tp.SelectSql.prototype.From = '';
/**
Ges or sets a statement part
@type {string}
*/
tp.SelectSql.prototype.Where = '';
/**
Gets or sets the user defined part of the WHERE clause
@type {string}
*/
tp.SelectSql.prototype.WhereUser = '';
/**
Ges or sets a statement part
@type {string}
*/
tp.SelectSql.prototype.GroupBy = '';
/**
Ges or sets a statement part
@type {string}
*/
tp.SelectSql.prototype.Having = '';
/**
Ges or sets a statement part
@type {string}
*/
tp.SelectSql.prototype.OrderBy = '';



 
/**
The DataTable that results after the select execution
@type {tp.DataTable}
*/
tp.SelectSql.prototype.Table = null;


 
//#endregion  

//#region tp.SqlTextItem
/**
Represents a named sql statement
*/
tp.SqlTextItem = class {

    /**
     * Constructor     
     * @param {string} SqlText The statement text
     * @param {string}[ConnectionName='DEFAULT'] Optional. Defaults to DEFAULT.The name of the connection (database)
     * @param {string} Name Optional. A name for this statement
     */
    constructor(SqlText, ConnectionName = null, Name = null) {
        this.Name = Name || tp.NextName('SqlTextItem');
        this.SqlText = SqlText;
        this.ConnectionName = ConnectionName || tp.SysConfig.DefaultConnection;
    }

    /**
     * Creates a new {@link tp.DataRow} based on this instance property values and adds the row to a specified {@link tp.DataTable}
     * @param {tp.DataTable} Table The {@link tp.DataTable} table to add the information
     */
    AddTo(Table) {
        var Row = Table.NewRow();
        Row.SetByName('Name', this.Name);
        Row.SetByName('ConnectionName', this.ConnectionName);
        Row.SetByName('SqlText', this.SqlText);
        Table.AddRow(Row);
    }
};

/**
The name of the statement
@type {string}
*/
tp.SqlTextItem.prototype.Name = '';
/**
The statement text
@type {string}
*/
tp.SqlTextItem.prototype.SqlText = '';
/**
The name of the connection (database)
@type {string}
*/
tp.SqlTextItem.prototype.ConnectionName = '';
//#endregion  



//#region tp.DataEventArgs
/**
EventArgs derived class for the data events
*/
tp.DataEventArgs = class extends tp.EventArgs {
    /**
    Constructor
    @param {tp.DataColumn} [Column] - Optional. The {@link tp.DataColumn} data column
    @param {tp.DataRow} [Row] - Optional. The {@link tp.DataRow} data row
    @param {any} [OldValue] - Optional.
    @param {any} [NewValue] - Optional.
    */
    constructor(Column, Row, OldValue, NewValue) {
        super('', null);

        this.Column = Column;
        this.Row = Row;
        this.OldValue = OldValue;
        this.NewValue = NewValue;
    }

};

/**
The data column, if applicable, else null.
@type {tp.DataColumn}
*/
tp.DataEventArgs.prototype.Column = null;
/**
The data row, if applicable, else null.
@type {tp.DataRow}
*/
tp.DataEventArgs.prototype.Row = null;
/**
The column old value, if applicable, else null.
@type {any}
*/
tp.DataEventArgs.prototype.OldValue = null;
/**
The column new value, if applicable, else null.
@type {any}
*/
tp.DataEventArgs.prototype.NewValue = null;
//#endregion  

//#region tp.DataTableEventArgs
/**
EventArgs derived class for the datatable events
*/
tp.DataTableEventArgs = class extends tp.DataEventArgs {
    /**
    Constructor
    @param {tp.DataColumn} [Column] - Optional. The {@link tp.DataColumn} data column
    @param {tp.DataRow} [Row] - Optional. The {@link tp.DataRow} data row
    @param {any} [OldValue] - Optional.
    @param {any} [NewValue] - Optional.
    */
    constructor(Column, Row, OldValue, NewValue) {
        super(Column, Row, OldValue, NewValue);
    }

    /**
    The sender {@link tp.DataTable} data table
    @type {tp.DataTable}
    */
    get Table() { return this.Sender; }
};
//#endregion  

//#region tp.DataSourceEventArgs
/**
EventArgs derived class for the datatable events
*/
tp.DataSourceEventArgs = class extends tp.DataEventArgs {
    /**
    Constructor
    @param {tp.DataColumn} [Column] - Optional. The {@link tp.DataColumn} data column
    @param {tp.DataRow} [Row] - Optional. The {@link tp.DataRow} data row
    @param {any} [OldValue] - Optional.
    @param {any} [NewValue] - Optional..
    */
    constructor(Column, Row, OldValue, NewValue) {
        super(null, null, null, null);

        if (arguments.length === 1 && arguments[0] instanceof tp.DataTableEventArgs) {
            this.Column = arguments[0].Column;
            this.Row = arguments[0].Row;
            this.OldValue = arguments[0].OldValue;
            this.NewValue = arguments[0].NewValue;
        } else if (arguments.length > 0) {
            this.Column = arguments.length > 0 ? arguments[0] : null;
            this.Row = arguments.length > 1 ? arguments[1] : null;
            this.OldValue = arguments.length > 2 ? arguments[2] : null;
            this.NewValue = arguments.length > 3 ? arguments[3] : null;
        }
    }



    /**
    The sender {@link tp.DataSource} datasource
    @type {tp.DataSource}
    */
    get DataSource() { return this.Sender; }
    /**
    Returns the {@link tp.DataTable} data-table bound to datasource
    @type {tp.DataTable}
    */
    get Table() {
        return this.DataSource.Table;
    }
    /**
    Returns the position of datasource
    @type {number}
    */
    get Position() {
        return this.DataSource.Position;
    }
};

/**
 * Creates and returns a {@link tp.DataSourceEventArgs} based on a specified {@link tp.DataTableEventArgs} source instance.
 * @param {tp.DataTableEventArgs} Source The source to base the new instance on.
 * @returns {tp.DataSourceEventArgs} Returns a {@link tp.DataSourceEventArgs}
 */
tp.DataSourceEventArgs.Create = function (Source) {
    return new tp.DataSourceEventArgs(Source);
};
//#endregion  

//#region tp.FieldFlags

/** Enum-like object. Used in flagging table fields.
 @class
 @enum {number}
 */
tp.FieldFlags = {
    None: 0,
    Hidden: 1,
    ReadOnly: 2,
    ReadOnlyUI: 4,
    ReadOnlyEdit: 8,
    Required: 0x10,
    Boolean: 0x20,
    Memo: 0x40,
    HtmlMemo: 0x80,
    Image: 0x100,
    ImagePath: 0x200,
    Searchable: 0x400,
    Extra: 0x800,
    ForeignKey: 0x1000,
    NoInsertUpdate: 0x2000,
    Localizable: 0x4000,
};
 
Object.freeze(tp.FieldFlags);
//#endregion

//#region tp.DataMode
/** Enum-like object. Indicates the state of a broker or data-view
 @class
 @enum {number}
 */
tp.DataMode = {
    None: 0,
    Browse: 1,

    Insert: 2,
    Edit: 4,
    Delete: 8,

    Commit: 0x10,
    Cancel: 0x20 
};
Object.freeze(tp.DataMode);
//#endregion


//---------------------------------------------------------------------------------------
// tp.Db
//---------------------------------------------------------------------------------------

tp.Urls.SqlSelect = '/SqlSelect';
tp.Urls.SqlSelectAll = '/SqlSelectAll';
 

//#region tp.Db
/** Helper static class 
 @class
 @static
 */
tp.Db = class {

    /** Constant
     * @type {string}
     */
    static get NULL() { return "___null___"; }
    /** Constant
     * @type {string}
     */
    static get NEW() { return "___NEW___"; }
    /** Constant
     * @type {string}
     */
    static get INVALID_ID() { return '27C15428-7892-4F7D-B28F-9BA059C94BA4'; }
 
    /* binding */
    /**
    Returns a specified value formatted as text
    @param {any} v - The value to convert to string.
    @param {tp.DataType} DataType - The datatype of the specified value. One of the {@link tp.DataType} constants.
    @param {tp.ColumnDisplayType} DisplayType - The display type. One of the {@link tp.ColumnDisplayType} constants.
    @param {boolean} ForList - If true then the value is formatted for grids and lists
    @param {number} [Decimals=2] - Optional. Defaults to 2. The number of decimal places into string (when float value).
    @param {boolean} [LocalDate=false] - Optional. Defaults to false. When true a local date string is returned, else an ISO date string (when date-time).
    @param {boolean} [DisplaySeconds=false] - Optional. Defaults to false. When true, then seconds are included in the returned string (when date-time).
    @returns {string} Returns the specified value as a string.
    */
    static Format(v, DataType, DisplayType, ForList, Decimals, LocalDate, DisplaySeconds) {
        if (tp.IsEmpty(v)) {
            return '';
        } else {
            switch (DataType) {
                case tp.DataType.Unknown: return '';
                case tp.DataType.String: return v.toString();
                case tp.DataType.Integer: return DisplayType === tp.ColumnDisplayType.CheckBox ? (v === 1 ? 'x' : '') : v.toString();
                case tp.DataType.Boolean: return (v === true) || (v === 1) ? 'x' : '';
                case tp.DataType.Float: return tp.FormatNumber2(v, Decimals);
                case tp.DataType.Decimal: return tp.FormatNumber2(v, Decimals);
                case tp.DataType.Date: return tp.ToDateString(v, LocalDate === true? '': 'ISO');
                //case tp.DataType.Time: return tp.ToTimeString(v, DisplaySeconds);
                case tp.DataType.DateTime:
                    if (DisplayType === tp.ColumnDisplayType.Time)
                        return tp.ToTimeString(v, DisplaySeconds);

                    if (DisplayType === tp.ColumnDisplayType.Date)
                        return tp.ToDateString(v, LocalDate === true ? '' : 'ISO');

                    return tp.ToDateTimeString(v, LocalDate === true ? '' : 'ISO');
                    break;
                case tp.DataType.Memo: return Boolean(ForList) === true ? '[memo]' : v; // '[memo]';
                case tp.DataType.Blob: return Boolean(ForList) === true ? '[blob]' : v; //  '[blob]';
            }
        }

        return '';
    }
    /**
    Converts a specified string into a primitive value (or a date-time)
    @param {string} S - The string to convert.
    @param {tp.DataType} DataType - The datatype of the specified value. One of the {@link tp.DataType} constants.
    @returns {any} Returns a primitive value (or a date-time)
    */
    static Parse(S, DataType) {
        if (!tp.IsBlank(S)) {
            var Info;
            switch (DataType) {

                case tp.DataType.String:
                    return S;

                case tp.DataType.Integer:
                    Info = tp.TryStrToInt(S);
                    if (!Info.Result) {
                        tp.Throw('Not an integer: ' + S);
                    }
                    return Info.Value;

                case tp.DataType.Boolean:
                    if (tp.IsSameText(S, 'false') || S === '0')
                        return false;

                    if (tp.IsSameText(S, 'true') || S === 'x' || S === '1')
                        return true;

                    return false;

                case tp.DataType.Float:
                case tp.DataType.Decimal:
                    Info = tp.TryStrToFloat(S);
                    if (!Info.Result) {
                        tp.Throw('Not a float number: ' + S);
                    }
                    return Info.Value;

                case tp.DataType.Date:
                    Info = tp.TryParseDateTime(S);
                    if (!Info.Result) {
                        tp.Throw('Not a date: ' + S);
                    }
                    return tp.ClearTime(Info.Value);
 
                case tp.DataType.DateTime:
                    Info = tp.TryParseDateTime(S);
                    if (!Info.Result) {
                        tp.Throw('Not a date-time: ' + S);
                    }
                    return Info.Value;

                case tp.DataType.Memo:
                    return S;

            }
        }

        return null;
    }

    /* select */
    /**
    Executes a SELECT statement and "returns" a {@link tp.DataTable}. <br />
    NOTE: This method returns a Promise, that is, it is a thenable/chainable method.
    @example
        tp.Db.Select('select * from Customer', 'DEFAULT')
            .then((Table) => {
                // use the Table here
            });
    @param {string} SqlText - The SELECT statement to execute
    @param {string} [ConnectionName='DEFAULT'] - Optional. Defaults to DEFAULT. The connection name.
    @returns {tp.DataTable} Returns a {@link Promise} promise with a {@link tp.DataTable} data table
    */
    static async SelectAsync(SqlText, ConnectionName) {

        let Url = tp.Urls.SqlSelect;
        let Model = new tp.SqlTextItem(SqlText, ConnectionName); 

        let Args = tp.Ajax.ModelArgs(Url, Model);
        Args = await tp.Ajax.Async(Args);
 
        var Table = new tp.DataTable();
        Table.Assign(Args.ResponseData.Packet);

        return Table;
    }
    /**
    Executes a list of SELECT statements and "returns" a {@link tp.DataSet}. <br />
    NOTE: This method returns a Promise, that is, it is a thenable/chainable method.
    @param {tp.SqlTextItem[]} SqlTextItemList - An array of tp.SqlTextItem elements
    @returns {tp.DataSet} Returns a {@link Promise}  with a {@link tp.DataSet} dataset.
    */
    static async SelectAllAsync(SqlTextItemList) {

        let Url = tp.Urls.SqlSelectAll; 

        let Table = new tp.DataTable('SqlTextList');

        Table.AddColumn('Name', tp.DataType.String, 96);
        Table.AddColumn('ConnectionName', tp.DataType.String, 96);
        Table.AddColumn('SqlText', tp.DataType.String, 1024 * 32);

        for (let i = 0, ln = SqlTextItemList.length; i < ln; i++) {
            SqlTextItemList[i].AddTo(Table);
        }

        let Model = Table.toJSON();

        let Args = tp.Ajax.ModelArgs(Url, Model);   
        Args = await tp.Ajax.Async(Args);

        var DataSet = new tp.DataSet();
        DataSet.Assign(Args.ResponseData.Packet);

        return DataSet;
    }


    /* aggregates */
    /**
    Static. Calculates and returns the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    @param {tp.DataRow[]} RowList A list of {@link tp.DataRow} items.
    @param {number} ColumnIndex The index of a column
    @returns {number} Returns  the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    */
    static Sum(RowList, ColumnIndex) {
        var i, ln, Row, v, Result = 0;

        for (i = 0, ln = RowList.length; i < ln; i++) {
            Row = RowList[i];
            v = Row.Get(ColumnIndex);
            if (!tp.IsEmpty(v)) {
                Result += v;
            }
        }

        return Result;
    }
    /**
    Static. Calculates and returns the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    @param {tp.DataRow[]} RowList A list of {@link tp.DataRow} items.
    @param {number} ColumnIndex The index of a column
    @returns {number}  Returns  the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    */
    static Avg(RowList, ColumnIndex) {
        var Result = tp.Db.Sum(RowList, ColumnIndex);
        if (RowList.length > 0) {
            Result = Math.ceil(Result / RowList.length);
        }

        return Result;
    }
    /**
    Static. Calculates and returns the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    @param {tp.DataRow[]} RowList A list of {@link tp.DataRow} items.
    @param {number} ColumnIndex The index of a column
    @returns {any} Returns  the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    */
    static Min(RowList, ColumnIndex) {
        var i, ln, Row, v, Result = null;

        for (i = 0, ln = RowList.length; i < ln; i++) {
            Row = RowList[i];
            v = Row.Get(ColumnIndex);
            if (!tp.IsEmpty(v)) {
                if (tp.IsEmpty(Result)) {
                    Result = v;
                } else {
                    if (v < Result) {
                        Result = v;
                    }
                }
            }
        }

        return Result;
    }
    /**
    Static. Calculates and returns the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    @param {tp.DataRow[]} RowList A list of {@link tp.DataRow} items.
    @param {number} ColumnIndex The index of a column
    @returns {any} Returns  the value of an aggregate function against a specified list of data-rows and an index of a grid column.
    */
    static Max(RowList, ColumnIndex) {
        var i, ln, Row, v, Result = null;

        for (i = 0, ln = RowList.length; i < ln; i++) {
            Row = RowList[i];
            v = Row.Get(ColumnIndex);
            if (!tp.IsEmpty(v)) {
                if (tp.IsEmpty(Result)) {
                    Result = v;
                } else {
                    if (v > Result) {
                        Result = v;
                    }
                }
            }
        }

        return Result;
    }


    /* miscs */
    /**
    Returns the max value of a specified numeric field.
    @param {tp.DataTable} Table - The data table
    @param {string} [FieldName='DisplayOrder'] - Optional. Defaults to 'DisplayOrder'. The numeric field name.
    @returns {number} Returns the max value of a specified numeric field.
    */
    static GetMaxDisplayOrder(Table, FieldName) {
        var i, ln, v, Result = 0;
        let Row;

        FieldName = FieldName || 'DisplayOrder';
        var Index = Table.IndexOfColumn(FieldName);
        if (Index !== -1) {
            for (i = 0, ln = Table.Rows.length; i < ln; i++) {
                Row = Table.Rows[i];
                v = Row.GetByIndex(Index);
                Result = Math.max(Result, tp.IsNumber(v) ? v : 0);
            }
            Result++;
        }

        return Result;
    }
    /**
    Returns true if a specified column name ends with the string Id
    @param {string} ColumnName The column name
    @returns {boolean} Returns true if a specified column name ends with the string Id
    */
    static IsIdColumn(ColumnName) {
        return !tp.IsBlank(ColumnName) && tp.EndsWith(ColumnName, 'Id', true);
    }
    /**
    Deletes data rows that exist in a source array but not in a target array. Decides what to delete by comparing two provided key fields.
    @param {tp.DataRow[]} TargetList - The target array where deletions are going to happen.
    @param {tp.DataRow[]} SourceList - The source array.
    @param {string} [TargetKey='Id'] - Optional. Defaults to 'Id'. The key field name in the target array
    @param {string} [SourceKey='Id'] - Optional. Defaults to 'Id'.The key field name in the source array
    */
    static DeleteNonExistedRows(TargetList, SourceList, TargetKey, SourceKey) {
        TargetKey = TargetKey || 'Id';
        SourceKey = SourceKey || 'Id';

        let i, ln, v, TargetIndex, SourceIndex;
        let tblTarget;
        let tblSource;
        let List = []; // tp.DataRow[]

        if ((TargetList.length > 0) && (SourceList.length > 0)) {
            tblTarget = TargetList[0].Table;
            tblSource = SourceList[0].Table;

            TargetIndex = tblTarget.IndexOfColumn(TargetKey);
            SourceIndex = tblSource.IndexOfColumn(SourceKey);

            for (i = 0, ln = TargetList.length; i < ln; i++) {
                v = TargetList[i].GetByIndex(TargetIndex);

                v = tp.FirstOrDefault(SourceList, (SourceRow) => {
                    return v === SourceRow.GetByIndex(SourceIndex);
                });

                if (tp.IsEmpty(v)) {
                    List.push(TargetList[i]);
                }
            }
        }


        if (List.length > 0) {
            for (i = 0, ln = List.length; i < ln; i++)
                List[i].Remove();
        }
    }
 
};
 

//#endregion  

//---------------------------------------------------------------------------------------
// DataSet, DataTable, DataRow, DataColumn
//---------------------------------------------------------------------------------------

//#region tp.DataSet
/**
A dataset mainly is a list of data tables
*/
tp.DataSet = class extends tp.tpObject {
    /**
    Constructor
    @param {string} [Name] - Optional. The name of the dataset
    */
    constructor(Name) {
        super();
        this.Tables = [];
        this.Name = Name || tp.NextName("DataSet");
    }



    /* public */
    /**
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() {
        return this.Name;
    }
    /**
    If an object being stringified has a property named toJSON whose value is a function,
    then the toJSON method customizes JSON stringification behavior: instead of the object
    being serialized, the value returned by the toJSON method when called will be serialized.
    @see {@link http://www.ecma-international.org/ecma-262/5.1/#sec-15.12.3|specification}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify|mdn}
    @returns {string} Returns this instance serialized.
     */
    toJSON() {
        var Result = {};

        Result.Name = this.Name;
        Result.Tables = [];
        for (var i = 0, ln = this.Tables.length; i < ln; i++) {
            Result.Tables.push(this.Tables[i].toJSON());
        }

        return Result;
    }
    /**
    Assigns a source object to this instance
    @param {tp.DataSet} Source - A dataset or a plain javascript object
    */
    Assign(Source) {
        this.Name = Source.Name || this.Name;
        this.AssignSchema(Source);
        this.AssignRows(Source.Tables, false);
    }
    /**
    Assigns ONLY the table schema (table column information) of a source dataset or object to this instance
    @param {tp.DataSet} Source - A dataset or a plain javascript object
    */
    AssignSchema(Source) {
        if (Source instanceof tp.DataTable || tp.IsPlainObject(Source)) {
            if (tp.IsArrayLike(Source.Tables)) {
                let Table;
                this.Tables = [];
                for (let i = 0, ln = Source.Tables.length; i < ln; i++) {
                    Table = new tp.DataTable(Source.Tables[i].Name);
                    this.AddTable(Table);
                    Table.AssignSchema(Source.Tables[i]);
                }
            }
        }
    }
    /**
    Assigns data rows from each table in provided array of tables to a table with the same name, if found, in this instance.
    @param {tp.DataTable[]} SourceTables - An array of source data tables
    @param {boolean} UpdateExisted - If true then rows found in the target data table are updated with the source data row data.
    */
    AssignRows(SourceTables, UpdateExisted) {
        if (tp.IsArrayLike(SourceTables)) {
            let Table;
            for (let i = 0, ln = SourceTables.length; i < ln; i++) {
                Table = this.FindTable(SourceTables[i].Name);
                if (!tp.IsEmpty(Table)) {
                    Table.AssignRows(SourceTables[i].Rows, UpdateExisted);
                }
            }
        }
    }
    /**
    Sets all rows to Unchaged state and empties the Deleted cache in all of the tables
    */
    AcceptChanges() {
        for (var i = 0, ln = this.Tables.length; i < ln; i++) {
            this.Tables[i].AcceptChanges();
        }
    }
    /**
    Finds and returns a data table by name, if any, else null.
    @param {string} TableName The name of the table to find.
    @returns {tp.DataTable} A data table or null.
    */
    FindTable(TableName) {
        return tp.FirstOrDefault(this.Tables, function (item) {
            return tp.IsSameText(TableName, item.Name);
        });
    }
    /**
    Adds a data table to this instance.
    @param {tp.DataTable|string} TableOrName - If it is a string then a new table is created with that name.
    @returns {tp.DataTable} The newly added data table.
    */
    AddTable(TableOrName) {
        let Table = null;
        if (typeof TableOrName === 'string') {
            if (!this.FindTable(TableOrName)) {
                Table = new tp.DataTable(TableOrName);
            }
        } else if (TableOrName instanceof tp.DataTable) {
            if (this.Tables.indexOf(TableOrName) < 0) {
                Table = TableOrName;
            }
        }

        if (Table) {
            Table.DataSet = this;
            this.Tables.push(Table);
        }

        return Table;

    }
    /**
    Removes a data table from this instance
    @param {tp.DataTable|string} TableOrName The table to remove.
    */
    RemoveTable(TableOrName) {
        let Table = null;
        if (typeof TableOrName === 'string') {
            Table = this.FindTable(TableOrName);
            if (Table) {
                Table.DataSet = null;
                tp.ListRemove(this.Tables, Table);
            }
        } else if (TableOrName instanceof tp.DataTable) {
            Table = TableOrName;
            if (tp.ListContains(this.Tables, Table)) {
                Table.DataSet = null;
                tp.ListRemove(this.Tables, Table);
            }
        }
    }
};
/* properties */
/**
The list of the data tables
@type {tp.DataTable[]}
*/
tp.DataSet.prototype.Tables = [];
/**
Dataset name
@type {string}
*/
tp.DataSet.prototype.Name = "";

//#endregion  

//#region tp.DataTable
/**
Represents a data table
*/
tp.DataTable = class extends tp.tpObject {

    /**
    Constructor
    @param {string} [Name] - Optional. The name of the data table
    */
    constructor(Name) {
        super();

        this.DataSet = null;
        this.Columns = [];
        this.Rows = [];
        this.Deleted = [];
        this.Details = [];
        this.StockTables = [];
        this.PrimaryKeyField = "Id";
        this.MasterKeyField = "Id";
        this.DetailKeyField = "";
        this.MasterTableName = "";
        this.AutoGenerateGuidKeys = true;

        this.Name = Name || tp.NextName("DataTable");
    }


    /**
    A data-source created on this table. Used when a list of tables/datasources used with the same grid
    @type {tp.DataSource}
    */
    get BindingSource() {
        if (tp.IsEmpty(this.fBindingSource))
            this.fBindingSource = new tp.DataSource(this);
        return this.fBindingSource;
    }
    set BindingSource(v) {
        if (v instanceof tp.DataSource && v.Table === this) {
            this.fBindingSource = v;
        } else {
            this.fBindingSource = null;
        }
    }

    /**
    Gets or sets the name of the data table. Must be unique among the tables in the same dataset.
    @type {string}
    */
    get Name() {
        return this.fName;
    }
    set Name(v) {
        if (v !== this.fName) {
            this.Trigger("NameChanging");
            this.fName = v;
            this.Trigger("NameChanged");
        }
    }
    /**
    Returns the master data table
    @type {tp.DataTable}
    */
    get MasterTable() {
        return (this.DataSet) ? this.DataSet.FindTable(this.MasterTableName) : null;
    }
    /**
    Returns the number of columns
    @type {number}
    */
    get ColumnCount() {
        return this.Columns.length;
    }
    /**
    Returns the number of rows
    @type {number}
    */
    get RowCount() {
        return this.Rows.length;
    }
    /**
    Gets or sets the name of the primary key field. Defaults to Id.
    @default Id
    @type {string}
    */
    get PrimaryKeyField() {
        return !tp.IsBlank(this.fPrimaryKeyField) ? this.fPrimaryKeyField : 'Id';
    }
    set PrimaryKeyField(v) {
        if (v !== this.fPrimaryKeyField) {
            this.fPrimaryKeyField = v;
            this.fPrimaryKeyIndex = -1;
        }
    }
    /**
    Returns the index of the primary key column
    @type {number}
    */
    get PrimaryKeyIndex() {
        if (this.fPrimaryKeyIndex < 0) {
            this.fPrimaryKeyIndex = this.IndexOfColumn(this.PrimaryKeyField);
        }
        return this.fPrimaryKeyIndex;
    }
    /**
    Returns the data column of the primary key field
    @type {tp.DataColumn}
    */
    get PrimaryKeyColumn() {
        return this.Columns[this.PrimaryKeyIndex];
    }
    /**
    Gets or sets a boolean value indicating whether this instance is currently performing a batch operation.
    @type {boolean}
    */
    get Batch() {
        return this.fBatchCounter > 0;
    }
    set Batch(v) {
        this.fBatchCounter = v === true ? this.fBatchCounter + 1 : this.fBatchCounter - 1;
        if (this.fBatchCounter === 0) {
            this.OnBatchModified();
        }
        if (this.fBatchCounter < 0)
            this.fBatchCounter = 0;
    }


    /** 
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() {
        return this.Name;
    }
    /**
    If an object being stringified has a property named toJSON whose value is a function,
    then the toJSON method customizes JSON stringification behavior: instead of the object
    being serialized, the value returned by the toJSON method when called will be serialized.
    @see {@link http://www.ecma-international.org/ecma-262/5.1/#sec-15.12.3|specification}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify|mdn}
    @returns {string} Returns this instance serialized.
     */
    toJSON() {
        var Result = {};

        Result.Name = this.Name;
        Result.PrimaryKeyField = this.PrimaryKeyField;
        Result.MasterKeyField = this.MasterKeyField;
        Result.DetailKeyField = this.DetailKeyField;
        Result.MasterTableName = this.MasterTableName;
        Result.AutoGenerateGuidKeys = this.AutoGenerateGuidKeys;

        var i, ln;
        Result.Details = this.Details;
        Result.StockTables = this.StockTables;

        Result.Columns = [];
        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Result.Columns.push(this.Columns[i].toJSON());
        }

        Result.Rows = [];
        for (i = 0, ln = this.Rows.length; i < ln; i++) {
            Result.Rows.push(this.Rows[i].toJSON());
        }

        Result.Deleted = [];
        for (i = 0, ln = this.Deleted.length; i < ln; i++) {
            Result.Deleted.push(this.Deleted[i].toJSON());
        }

        return Result;
    }
    /**
    Assigns a source object to this instance
    @param {tp.DataTable} Source The source to copy from
    */
    Assign(Source) {
        this.AssignSchema(Source);
        this.AssignRows(Source.Rows, false);      
    }
    /**
    Assigns just the schema (column information) of a source object to this instance
    @param {tp.DataTable} Source The source to copy from
    */
    AssignSchema(Source) {
        if (Source instanceof tp.DataTable || tp.IsPlainObject(Source)) {
            this.Details = Source.Details || this.Details;
            this.StockTables = Source.StockTables || this.StockTables;
            this.Columns = [];

            var i, ln, k, kln, Column;

            this.Deleted.length = 0;

            this.Name = Source.Name;
            this.PrimaryKeyField = Source.PrimaryKeyField;
            this.MasterKeyField = Source.MasterKeyField;
            this.DetailKeyField = Source.DetailKeyField;
            this.MasterTableName = Source.MasterTableName;
            this.AutoGenerateGuidKeys = Source.AutoGenerateGuidKeys;

            for (i = 0, ln = Source.Columns.length; i < ln; i++) {
                Column = new tp.DataColumn();
                Column.Assign(Source.Columns[i]);
                this.AddColumn(Column);
            }
        }
    }
    /**
    Assigns rows from a source array of rows to this instance.
    @param {tp.DataRow[]} SourceRows - An array of source data rows
    @param {boolean} UpdateExisted - If true then rows found in this data table are updated with the source data row data.
    */
    AssignRows(SourceRows, UpdateExisted) {

        if (!tp.IsArrayLike(SourceRows))
            return;

        let self = this;
        let PrimaryKeyIsString = this.PrimaryKeyColumn.DataType === tp.DataType.String;
        let PrimaryColumnIndex = this.IndexOfColumn(this.PrimaryKeyColumn);

        // nested - Gets a string or number and returns a tp.DataRow
        function RowByPrimaryKey(v) {
            let i, ln, v2, Row = null;
            if (PrimaryKeyIsString === true) {
                let s = v.toString();
                for (i = 0, ln = self.Rows.length; i < ln; i++) {
                    Row = self.Rows[i];
                    v2 = Row.Data[PrimaryColumnIndex];
                    if (tp.IsSameText(s, v2))
                        return Row;
                }
            } else {
                let n = Number(v);
                for (i = 0, ln = self.Rows.length; i < ln; i++) {
                    Row = self.Rows[i];
                    v2 = Row.Data[PrimaryColumnIndex];
                    if (v2 === v)
                        return Row;
                }
            }

            return null;
        }


        var v,
            SourceRow,  // tp.DataRow,
            Row;        // tp.DataRow;
        var i, ln, k, kln,
            Column,     // tp.DataColumn,
            Dates = [];       // indexes of date columns

        this.Deleted = [];

        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Column = this.Columns[i];

            if ((Column.DataType === tp.DataType.Date) || (Column.DataType === tp.DataType.DateTime)) {
                Dates.push(i);
            }
        }

 
        for (i = 0, ln = SourceRows.length; i < ln; i++) {
            SourceRow = SourceRows[i];
            Row = null;

            if (UpdateExisted === true) {
                Row = RowByPrimaryKey(SourceRow.Data[PrimaryColumnIndex]);
                if (!tp.IsEmpty(Row))
                    Row.Data = SourceRow.Data.slice();
            }

            if (tp.IsEmpty(Row)) {
                Row = new tp.DataRow(this, SourceRow.Data.slice());
                this.Rows.push(Row);
            }

            for (k = 0, kln = Dates.length; k < kln; k++) {
                v = Row.Data[Dates[k]];
                if (tp.IsString(v) && !tp.IsBlank(v)) {
                    v = new Date(v);
                    Row.Data[Dates[k]] = v;
                }
            }

            Row.State = !tp.IsEmpty(SourceRow.State) ? SourceRow.State : tp.DataRowState.Unchanged;

        }

    }

    /**
    Creates and retuns a new tp.DataTable with just the structure of this table. No data at all.
    @returns {tp.DataTable} Returns the data table clone.
    */
    Clone()  {
        var Result = this.CreateInstance();
        Result.AssignSchema(this);
        return Result;
    }
    /**
    Creates and retuns a new tp.DataTable with both the structure and data of this table
    @returns {tp.DataTable}  Returns the data table copy.
    */
    Copy() {
        var Result = this.CreateInstance();
        Result.Assign(this);
        return Result;
    }

    /**
    Returns the index of a column or -1 if a column not found. 
    @param {string | number | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {number} Returns the index of column or -1
    */
    IndexOfColumn(Column) {
        if (tp.IsNumber(Column))
            return Column;

        var i, ln;

        if (tp.IsString(Column)) {
            for (i = 0, ln = this.Columns.length; i < ln; i++) {
                if (tp.IsSameText(this.Columns[i].Name, Column)) {
                    return i;
                }
            }
        } else if (Column instanceof tp.DataColumn) {
            for (i = 0, ln = this.Columns.length; i < ln; i++) {
                if (this.Columns[i] === Column) {
                    return i;
                }
            }
        }

        return -1;
    }
    /**
    Returns true if a column exists in Columns.
    @param {string | number | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns  {boolean} Returns true if the column exists.
    */
    ContainsColumn(Column) {
        return this.IndexOfColumn(Column) !== -1;
    }
    /**
    Finds and returns a data column
    @param {string | number | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {tp.DataColumn} Returns the data column or null
    */
    FindColumn(Column)  {
        var Index = this.IndexOfColumn(Column);
        return Index === -1 ? null : this.Columns[Index];
    }

    /**
    Adds (or inserts at a specified index) a new column to table's Columns. <br />
    NOTE: A new column can be added even if the table is NOT empty and already contains rows.
    @param {string | tp.DataColumn} NameOrColumn - A column name or a data column
    @param {string} [DataType] - Optional. Defaults to tp.DataType.String. The data type of the new column
    @param {number} [MaxLength] - Optional. Defautls to 96 if applicable. The max length for a string column.
    @param {number} [ColumnIndex] - Optional. The index of the new column in the array of columns. If is < 0 the column is added at the end of the columns.
    @returns {tp.DataColumn} Returns the newly added data column
    */
    AddColumn(NameOrColumn, DataType, MaxLength, ColumnIndex = -1) {
        let Column = this.FindColumn(NameOrColumn);
        if (Column) {
            return Column;
        }

        if (tp.IsString(NameOrColumn)) {
            Column = new tp.DataColumn(NameOrColumn, DataType, MaxLength);
        } else if (NameOrColumn instanceof tp.DataColumn) {
            Column = NameOrColumn;
        }

        if (Column) {
            Column.Table = this;

            if (tp.IsNumber(ColumnIndex)) {
                if (ColumnIndex < 0) {
                    this.Columns.push(Column);
                }
                else {
                    if (tp.InRange(this.Columns, ColumnIndex))
                        tp.ListInsert(this.Columns, ColumnIndex, Column);
                    else
                        tp.Throw(`Cannot insert table column. Invalid column index: ${ColumnIndex}`);
                }
            }
            

            // Update Rows
            if (this.Rows.length > 0) {
                let Index = this.Columns.indexOf(Column);
                for (var i = 0, ln = this.Rows.length; i < ln; i++) {
                    this.Rows[i].Data.splice(Index, 0, null);
                }
            }

            this.OnColumnAdded(Column);
        }
        return Column;
    }
    /**
    Removes a column from table's Columns. <br />
    NOTE: A column can be removed even if the table is NOT empty and already contains rows.
    @param { string | number | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    */
    RemoveColumn(Column) {
        let Col = this.FindColumn(Column);
        if (Col) {
            let Index = this.Columns.indexOf(Col);

            tp.ListRemove(this.Columns, Col);

            // Update Rows
            if (this.Rows.length > 0) {
                for (var i = 0, ln = this.Rows.length; i < ln; i++) {
                    this.Rows[i].Data.splice(Index, 1);
                }
            }

            this.OnColumnRemoved(Col);
        }
    }

    /**
    Sets a column's visibility according to a specified flag
    @param {string | number | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {boolean} Flag - True to make a visible column.
    */
    SetColumnVisible(Column, Flag) {
        var Col = this.FindColumn(Column);
        if (Col)
            Col.Visible = Flag;
    }
    /**
    Sets column visibility according to an array of column names. Each column in the array becomes visible, where the rest become invisible.
    @param {string[]} ColumnNames A list of column names
    */
    SetColumnListVisible(ColumnNames) {
        var Column, i, ln;

        for (i = 0, ln = this.Columns.length; i < ln; i++) {
            Column = this.Columns[i];
            Column.Visible = tp.ListContainsText(ColumnNames, Column.Name);
        }
    }

    /**
    Sets all rows to Unchaged state and empties the Deleted cache.
    */
    AcceptChanges() {
        for (var i = 0, ln = this.Rows.length; i < ln; i++) {
            this.Rows[i].AcceptChanges();
        }
        this.Deleted.length = 0;
    }

    /**
    Removes all rows from the table and empties the deleted cache.
    */
    Clear() {
        this.ClearRows();
        this.Deleted.length = 0;
        super.Clear();
    }
    /**
    Removes all rows from the table. All deleted rows go to deleted cache.
    */
    ClearRows() {
        var i, ln, Table;

        if (!tp.IsEmpty(this.DataSet)) {

            // details
            for (i = 0, ln = this.Details.length; i < ln; i++) {
                Table = this.DataSet.FindTable(this.Details[i]);
                if (!tp.IsEmpty(Table))
                    Table.ClearRows();
            }

            // stocks
            for (i = 0, ln = this.StockTables.length; i < ln; i++) {
                Table = this.DataSet.FindTable(this.StockTables[i]);
                if (!tp.IsEmpty(Table))
                    Table.ClearRows();
            }
        }


        if (this.Rows.length > 0) {
            this.OnRowsClearing();

            let BatchFlag = this.Rows.length > 1;
            if (BatchFlag)
                this.Batch = true;
            try {
                var List = this.RowsToList();
                for (i = 0, ln = List.length; i < ln; i++) {
                    List[i].State = tp.DataRowState.Deleted;
                }
                this.Deleted = this.Deleted.concat(List);
                this.Rows.length = 0;
            } finally {
                if (BatchFlag)
                    this.Batch = false;
            }

            this.OnRowsCleared();
        }
    }

    /**
    Creates and returns a new row. The new row is NOT added to the rows. The new row can be added with an explicit call to AddRow(). <br />
    NOTE: If AutoGenerateGuidKeys is true and a PrimaryKeyField is defined, then the value of that field is set to a new GUID string.
    @param {any[]} [Data] - Optional. The data array
    @returns {tp.DataRow} Returns a data row
    */
    NewRow(Data) {
        var Row = new tp.DataRow(this, Data);
        return Row;
    }
    /**
    Adds a row to the table Rows and returns the newly added data row
    @param {...Data} Data - Rest parameter. Data can be 1. unspecified 2. a tp.DataRow 3. a javascript array of values, or 4. just arguments separated by commas.
    @returns {tp.DataRow} Returns the newly added data row
    */
    AddRow(...Data) {

        let Row = null, i, ln;

        if (Data.length === 0) {
            Row = this.NewRow();
        } else {
            if (Data[0] instanceof tp.DataRow) {
                Row = Data[0];
            } else if (tp.IsArray(Data[0])) {
                Row = this.NewRow(Data[0]);
            } else {
                Row = this.NewRow(Data);
            }
        }

        if (Row && (Row.Table === this) && (Row.State === tp.DataRowState.Detached)) {
            if (!this.Batch)
                this.OnRowAdding(Row);
            this.Rows.push(Row);
            Row.State = tp.DataRowState.Added;
            if (!this.Batch)
                this.OnRowAdded(Row);
            return Row;
        }

        return null;
    }
    /**
    Creates and adds a new empty row to Rows. Returns the newly added row
    @returns {tp.DataRow} Returns the newly added data row
    */
    AddEmptyRow() {
        var Result = this.NewRow();
        this.AddRow(Result);
        return Result;
    }
    /**
    Removes a row from Rows. <br />
    Actually puts the row to the Deleted cache until AcceptChanges() is called.
    @param {number | tp.DataRow} IndexOrRow - The index of the data row or the data row to be removed
    */
    RemoveRow(IndexOrRow) {
        var Index = -1;
        var Row = null;

        if (IndexOrRow instanceof tp.DataRow) {
            Row = IndexOrRow;
            Index = this.Rows.indexOf(Row);
            if (Index < 0)
                return;
        } else {
            Index = IndexOrRow;
            if ((Index < 0) || (Index > this.Rows.length - 1)) {
                return;
            }
            Row = this.Rows[Index];
        }

        if (Index !== -1) {
            if (!this.Batch)
                this.OnRowRemoving(Row);
            this.Rows.splice(Index, 1);
            this.Deleted.push(Row);
            Row.State = tp.DataRowState.Deleted;
            if (!this.Batch)
                this.OnRowRemoved(Row);
        }
    }
    /**
    Creates and returns a copy of a data row.
    @param {tp.DataRow} SourceRow The source row
    @param {string[]} [ExcludeFieldList] - Optional. Array of field names to exclude from the copy.
    @returns {tp.DataRow} Returns the copy of the row.
    */
    CopyRow(SourceRow, ExcludeFieldList) {
        var Result = this.AddRow();
        Result.CopyFromRow(SourceRow, ExcludeFieldList);
        return Result;
    }

    /**
    Finds and returns a data row where a specified value equals to a specified column value, if any, else null
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} v - The value to look up for.
    @returns {tp.DataRow} Returns the row if found or null.
    */
    FindRow(Column, v)  {
        var v2, Index = this.IndexOfColumn(Column);
        for (var i = 0, ln = this.Rows.length; i < ln; i++) {
            v2 = this.Rows[i].Data[Index];
            if (v === v2) {
                return this.Rows[i];
            }
        }

        return null;
    }
    /**
    Finds and returns a data row where a specified value equals to a specified column value, if any, else null
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} v - The value to look up for.
    @returns {tp.DataRow} Returns the row if found or null.
    */
    Locate(Column, v) {
        return this.FindRow(Column, v);
    }
    /**
    Performs a look-up and returns a value on success, else null.
    @param {number | string | tp.DataColumn} KeyColumn - Specifies the key column. Could be a number (index), a string (field name) or a data column.
    @param {any} KeyValue - The value of the key column to match.
    @param {number | string | tp.DataColumn} ResultColumn - Specifies the result column. Could be a number (index), a string (field name) or a data column.
    @returns {any} Returns a value on success, else null.
    */
    LookUp(KeyColumn, KeyValue, ResultColumn) {
        var Row = this.FindRow(KeyColumn, KeyValue);
        if (!tp.IsEmpty(Row))
            return Row.Get(ResultColumn, null);
        return null;
    }
    /**
    Selects rows from the table that satisfy a condition where a specified column has a specified value. Returns an array of rows.
    @param {number | string | tp.DataColumn} Column The column to test for the specified value.
    @param {any} v The value to test
    @returns {tp.DataRow[]} Returns an array of rows. The array could be empty if none of the rows satisfies the condition.
    */
    SelectRows(Column, v)  {
        let Result = [];

        let ColumnIndex = this.IndexOfColumn(Column);
        if (ColumnIndex >= 0) {
            let v2,
                Row;
            for (let i = 0, ln = this.Rows.length; i < ln; i++) {
                Row = this.Rows[i];
                v2 = Row.GetByIndex(ColumnIndex);
                if (v === v2)
                    Result.push(Row);
            }
        }


        return Result;
    }


    /**
    Returns the rows as a javascript array of objects. <br />
    Each object in a rows array is a key/value pair, where key is the column name. <br />
    i.e. <code>[{ColName: value, ColName: value, ...}, {ColName: value, ColName: value, ...}, ... ]</code> <br />
    NOTE: A field named __Row is added to any returned object. That field points to the row itself.
    @returns {Object[]} An array of plain javascript objects.
    */
    RowsToObjectArray()  {
        var Result = [];
        for (var i = 0, ln = this.Rows.length; i < ln; i++) {
            Result.push(this.Rows[i].ToObject());
        }
        return Result;
    }
    /**
    Returns the data rows as an array
    @returns {tp.DataRow[]} An array of data rows
    */
    RowsToList() {
        return this.Rows.slice();
    }
    /**
    Updates column titles with localized titles
    */
    UpdateColumnTitles() {
        for (var i = 0, ln = this.Columns.length; i < ln; i++) {
            this.Columns[i].UpdateTitle();
        }
    }

    // event triggers
    /**
    Event trigger
    */
    OnBatchModified() { this.Trigger("BatchModified", new tp.DataTableEventArgs()); }
    /**
    Event trigger
    */
    OnRowsClearing() { this.Trigger("RowsClearing", new tp.DataTableEventArgs()); }
    /**
    Event trigger
    */
    OnRowsCleared() { this.Trigger("RowsCleared", new tp.DataTableEventArgs()); }
    /**
    Event trigger
    @param {tp.DataColumn} Column The column
    */
    OnColumnAdded(Column) { this.Trigger('ColumnAdded', new tp.DataTableEventArgs(Column)); }
    /**
    Event trigger
    @param {tp.DataColumn} Column The column
    */
    OnColumnRemoved(Column) { this.Trigger('ColumnRemoved', new tp.DataTableEventArgs(Column)); }
    /**
    Event trigger
    @param {tp.DataRow} Row The row
    */
    OnRowCreated(Row) { if (!this.Batch) this.Trigger('RowCreated', new tp.DataTableEventArgs(null, Row)); }
    /**
    Event trigger
    @param {tp.DataRow} Row The row
    */
    OnRowAdding(Row) { if (!this.Batch) this.Trigger('RowAdding', new tp.DataTableEventArgs(null, Row)); }
    /**
    Event trigger
    @param {tp.DataRow} Row The row
    */
    OnRowAdded(Row) { if (!this.Batch) this.Trigger('RowAdded', new tp.DataTableEventArgs(null, Row)); }
    /**
    Event trigger
    @param {tp.DataRow} Row The row
    */
    OnRowRemoving(Row) { if (!this.Batch) this.Trigger('RowRemoving', new tp.DataTableEventArgs(null, Row)); }
    /**
    Event trigger
    @param {tp.DataRow} Row The row
    */
    OnRowRemoved(Row) { if (!this.Batch) this.Trigger('RowRemoved', new tp.DataTableEventArgs(null, Row)); }
    /**
    Event trigger
    @param {tp.DataRow} Row The row 
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    OnRowModifying(Row, Column, OldValue, NewValue) {
        if (!this.Batch && (Row.State !== tp.DataRowState.Detached)) {
            let Args = new tp.DataTableEventArgs(Column, Row, OldValue, NewValue);
            this.Trigger("RowModifying", Args);
        }
    }
    /**
    Event trigger
    @param {tp.DataRow} Row The row
    @param {tp.DataColumn} Column The column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    OnRowModified(Row, Column, OldValue, NewValue) {
        if (!this.Batch && (Row.State !== tp.DataRowState.Detached)) {
            let Args = new tp.DataTableEventArgs(Column, Row, OldValue, NewValue);
            this.Trigger("RowModified", Args);
        }
    }

};

tp.DataTable.prototype.fName = '';
tp.DataTable.prototype.fBatchCounter = 0;
tp.DataTable.prototype.fPrimaryKeyField = '';
tp.DataTable.prototype.fPrimaryKeyIndex = -1;
tp.DataTable.prototype.fBindingSource = null; // tp.DataSource;

/* properties */

/**
The owner dataset, if any, else null.
@type {tp.DataSet}
*/
tp.DataTable.prototype.DataSet = null;
/**
The columns list
@type {tp.DataColumn[]}
*/
tp.DataTable.prototype.Columns = [];               // array of tp.DataColumn objects 
/**
The rows list
@type {tp.DataRow[]}
*/
tp.DataTable.prototype.Rows = [];                     // array of tp.DataRow objects 
/**
The deleted rows list
@type {tp.DataRow[]}
*/
tp.DataTable.prototype.Deleted = [];                 // array of deleted tp.DataRow objects
/**
A list of table names of tables that are detail tables to this table
@type {string[]}
*/
tp.DataTable.prototype.Details = [];                      // table names
/**
Stock table names list. Stock tables are used by brokers.
A stock table of the main broker table is a single row table and it is SELECTed each time the broker SELECTs the main table.
The SELECT of a stock table could be something like <code>select Field0, Field1, FieldN from STOCK_TABLE_NAME where Id = :SQL_PARAM</code>
@type {string[]}
*/
tp.DataTable.prototype.StockTables = [];                  // table names
/**
The name of the primary key field of the master table of this table
@default Id
@type {string}
*/
tp.DataTable.prototype.MasterKeyField = "Id";                  // belongs to master table
/**
The name of the foreign key field of this table to match with the primary key field of the master table (MasterKeyField)
@type {string}
*/
tp.DataTable.prototype.DetailKeyField = "";                    // belongs to this, match to the master's Id (MasterKeyField)
/**
The name of the master table, this table is detail to. Both tables sould belong to the same dataset
@type {string}
*/
tp.DataTable.prototype.MasterTableName = "";
/**
When true then a GUID string value is auto-generated and set as value to the primary key field, on any new data row.
@default true
@type {boolean}
*/
tp.DataTable.prototype.AutoGenerateGuidKeys = true;
//#endregion  

//#region tp.DataColumn
/**
Represents a data column
*/
tp.DataColumn = class extends tp.tpObject {
    /**
    Constructor
    @param {string} [Name] - Optional. The column name (field name)
    @param {string} [DataType] - Optional. One of the tp.DataType string constants. Defaults to tp.DataType.String. The data type of the new column
    @param {number} [MaxLength] - Optional. Defautls to 96 if applicable. The max length for a string column.
    */
    constructor(Name, DataType, MaxLength) {
        super();

        this.DataType = tp.DataType.IsValid(DataType) ? DataType : tp.DataType.String;


        this.Table = null;             // The table this column belongs to. Do NOT touch it
        this.fName = Name;
        this.TitleKey = '';
        this.DataType = DataType;
        this.Expression = null;
        this.DefaultValue = null;
        this.MaxLength = this.DataType === tp.DataType.String ? MaxLength : 0;
        this.Decimals = this.DataType === tp.DataType.Float || this.DataType === tp.DataType.Decimal ? 2 : 0;
        this.ReadOnly = false;
        this.Visible = true;
        this.Required = false;
        this.Unique = false;
    }

    fName = '';
    fTitle = '';
    fDataType = tp.DataType.String;
    fMaxLength = -1;
    /**
    Decimals. -1 means not defined
    @default  0
    @type {number}
    */
    fDecimals = -1;

    /* properties */
    /**
    The table this column belongs to.
    @type {tp.DataTable}
    */
    Table = null;
    /**
    Resource key for title localization
    @type {string}
    */
    TitleKey = '';

    /**
    Expression
    @type {string}
    */
    Expression = null;
    /**
    Default value
    @default null
    @type {any}
    */
    DefaultValue = null;

    /**
    Gets the decimals. 
    @returns {number} Returns the decimals.
    */
    get Decimals() {
        let Result = tp.IsInteger(this.fDecimals) ? this.fDecimals: -1;

        if (this.DataType === tp.DataType.Float || this.DataType === tp.DataType.Decimal) {
            if (Result < 0) {
                let Culture = tp.Cultures.Find(tp.CultureCode);
                if (tp.IsValid(Culture)) {
                    Result = Culture.CurrencyDecimals;
                }

                if (Result < 0)
                    Result = 2;
            }
        }

        return Result;
    }
    /** Sets the decimals
     * @param {number} v An integer indicating the decimals.
     */
    set Decimals(v) {
        if (tp.IsInteger(v))
            this.fDecimals = v;
    }
    /**
    ReadOnly
    @default false
    @type {boolean}
    */
    ReadOnly = false;
    /**
    Visible
    @default true
    @type {boolean}
    */
    Visible = true;
    /**
    Required
    @default false
    @type {boolean}
    */
    Required = false;
    /**
    Unique
    @default false
    @type {boolean}
    */
    Unique = false;
    /**
    The flags bit-field
    @default tp.FieldFlags.None
    @type {tp.FieldFlags}
    */
    Flags = tp.FieldFlags.None;

    /**
    True to use local date with the Format() method
    @default true
    @type {boolean}
    */
    LocalDate = true;
    /**
    True to diplay seconds when formatting with the Format() method
    @default false
    @type {boolean}
    */
    DisplaySeconds = false;

    /**
    The display type of a column. Used with grids.
    @default tp.ColumnDisplayType.Default
    @type {tp.ColumnDisplayType}
    */
    DisplayType = tp.ColumnDisplayType.Default;

    /**
    The name (field name) of the column
    @type {string}
    */
    get Name() {
        return this.fName;
    }
    set Name(v) {
        if (v !== this.fName) {
            this.Trigger("NameChanging");
            this.fName = v;
            this.Trigger("NameChanged");
        }
    }
    /**
    The title of the column
    @type {string}
    */
    get Title() {
        return !tp.IsBlank(this.fTitle) ? this.fTitle : this.Name;
    }
    set Title(v) {
        this.fTitle = v;
    }
    /**
    Data type. One of the tp.DataType constants
    @type {string}
    */
    get DataType() {
        return this.fDataType;
    }
    set DataType(v) {
        if (tp.IsString(v) && tp.DataType.IsValid(v) && v !== this.DataType) {
            this.fDataType = v;
        }
    }
    /**
    The max length for a string column.
    @type {number}
    */
    get MaxLength() {
        if (this.DataType === tp.DataType.String) {
            return tp.IsNumber(this.fMaxLength) && this.fMaxLength > 0 ? this.fMaxLength : 96;
        }

        return -1;
    }
    set MaxLength(v) {
        if (tp.IsNumber(v))
            this.fMaxLength = v;
    }

    /* flag properties */
    /**
    Returns a boolean value indicating whether the corresponding flag is set in the Flags property.
    @type {boolean}
    */
    get IsVisible() { return !tp.Bf.In(tp.FieldFlags.Hidden, this.Flags); }
    /**
   Returns a boolean value indicating whether the corresponding flag is set in the Flags property.
   @type {boolean}
   */
    get IsReadOnly() { return tp.Bf.In(tp.FieldFlags.ReadOnly, this.Flags); }
    /**
   Returns a boolean value indicating whether the corresponding flag is set in the Flags property.
   @type {boolean}
   */
    get IsReadOnlyUI() { return tp.Bf.In(tp.FieldFlags.ReadOnlyUI, this.Flags); }
    /**
   Returns a boolean value indicating whether the corresponding flag is set in the Flags property.
   @type {boolean}
   */
    get IsReadOnlyEdit() { return tp.Bf.In(tp.FieldFlags.ReadOnlyEdit, this.Flags); }
    /**
   Returns a boolean value indicating whether the corresponding flag is set in the Flags property.
   @type {boolean}
   */
    get IsRequired() { return tp.Bf.In(tp.FieldFlags.Required, this.Flags); }
    /**
   Returns a boolean value indicating whether the corresponding flag is set in the Flags property.
   @type {boolean}
   */
    get IsBoolean() { return tp.Bf.In(tp.FieldFlags.Boolean, this.Flags); }
 


    /* methods */
    /**
    Returns a string representation of this instance
    @returns {string} Returns a string representation of this instance
    */
    toString() {
        return this.Name;
    }
    /**
    If an object being stringified has a property named toJSON whose value is a function,
    then the toJSON method customizes JSON stringification behavior: instead of the object
    being serialized, the value returned by the toJSON method when called will be serialized.
    @see {@link http://www.ecma-international.org/ecma-262/5.1/#sec-15.12.3|specification}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify|mdn}
    @return {string} Returns this instance serialized.
     */
    toJSON() {
        var Result = {};

        Result.Name = this.Name;
        Result.Title = this.Title;
        Result.TitleKey = this.TitleKey;
        Result.DataType = this.DataType;
        Result.Expression = this.Expression;
        Result.DefaultValue = this.DefaultValue;
        Result.MaxLength = this.MaxLength;
        Result.ReadOnly = this.ReadOnly;
        Result.Visible = this.Visible;
        Result.Required = this.Required;
        Result.Unique = this.Unique;
        Result.Flags = this.Flags;

        Result.Decimals = this.Decimals;

        return Result;
    }
    /**
    Assigns a source to this instance
    @param {tp.DataColumn} Source The source to copy from.
    */
    Assign(Source) {
        this.Name = Source.Name;
        this.Title = Source.Title;
        this.TitleKey = Source.TitleKey;
        this.DataType = Source.DataType;
        this.Expression = Source.Expression;
        this.DefaultValue = Source.DefaultValue;
        this.MaxLength = Source.MaxLength;
        this.ReadOnly = Source.ReadOnly;
        this.Visible = Source.Visible;
        this.Required = Source.Required;
        this.Unique = Source.Unique;
        this.Flags = Source.Flags;

        this.Decimals = Source.Decimals;
    }
    /**
    Clones this instance and returns the clone
    @return {tp.DataColumn} Returns the clone.
    */
    Clone()  {
        return super.Clone();
    }
    /**
    Updates the title of the column with a localized value.
    */
    UpdateTitle() {
        var Key = this.TitleKey || this.Name;

        var self = this;
        tp.Res.GS(Key, function (Value, UserTag) {
            self.Title = Value;
        }, null, Key, this);

    }
    /**
    Returns a specified value of this column, formatted as text
    @param {any} v A value suitable for this column
    @param {boolean} ForList - If true then the value is formatted for grids and lists
    @returns {string} Returns a specified value of this column, formatted as text
    */
    Format(v, ForList) {
        let DataType = this.DisplayType === tp.ColumnDisplayType.CheckBox ? tp.DataType.Boolean : this.DataType;
        return tp.Db.Format(v, DataType, this.DisplayType, ForList, this.Decimals, this.LocalDate, this.DisplaySeconds);
    }
    /**
    Converts a specified string into a primitive value (or a date-time) suitable for this column
    @param {string} S The string value to parse.
    @returns {any} Returns the parsed value.
    */
    Parse(S) {
        return tp.Db.Parse(S, this.DataType);
    }

 
};
//#endregion  

//#region tp.DataRow
/**
Represents a data row
*/
tp.DataRow = class {


    /**
    Constructor
    NOTE: If table AutoGenerateGuidKeys is true and a PrimaryKeyField is defined in the table, then the value of that field is set to a new GUID string.
    @param {tp.DataTable} Table - The owner table
    @param {any[]} [Data] - Optional. A javascript array of values
    */
    constructor(Table, Data) {
        this.Table = Table;
        this.Data = Data || [];
        this.Data.length = Table.Columns.length;

        if (!this.Table.Batch)
            this.Table.OnRowCreated(this);

        if (this.Table.AutoGenerateGuidKeys && this.Table.PrimaryKeyIndex >= 0 && this.Table.Columns[this.Table.PrimaryKeyIndex].DataType === tp.DataType.String) {
            var v = this.Data[this.Table.PrimaryKeyIndex];
            if (tp.IsEmpty(v) || (tp.IsString(v) && tp.IsBlank(v)))
                this.SetByIndex(this.Table.PrimaryKeyIndex, tp.Guid());
        }

        this.State = tp.DataRowState.Detached;
    }


    /**
    The table this row belongs to.
    @type {tp.DataTable}
    */
    Table = null;
    /**
    Array with row data - read-only
    @type {any[]}
    */
    Data = [];
    /**
    Row state  - read-only
    @default tp.DataRowState.Detached
    @type {tp.DataRowState}
    */
    State = tp.DataRowState.Detached;


    /* methods */
    /**
    If an object being stringified has a property named toJSON whose value is a function,
    then the toJSON method customizes JSON stringification behavior: instead of the object
    being serialized, the value returned by the toJSON method when called will be serialized.
    @see {@link http://www.ecma-international.org/ecma-262/5.1/#sec-15.12.3|specification}
    @see {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify|mdn}
    @returns {string} Returns this instance serialized.
     */
    toJSON() {
        var Result = {};
        Result.State = this.State;
        Result.Data = this.Data;
        return Result;
    }


    /**
    Returns the value of a column by column index
    @param {number} Index The index of the column
    @return {any} Returns the value of a column
    */
    GetByIndex(Index) {
        return this.Data[Index];
    }
    /**
    Sets the value of a column by column index
    @param {number} Index The index of the column
    @param {any} v - The value to set
    */
    SetByIndex(Index, v) {
        if (this.Table.Batch) {
            this.Data[Index] = v;
        } else {
            var Column = this.Table.Columns[Index];
            var OldValue = this.Data[Index];
            var NewValue = v;

            /* send notification */
            this.Table.OnRowModifying(this, Column, OldValue, NewValue);
            this.Data[Index] = NewValue;
            this.Table.OnRowModified(this, Column, OldValue, NewValue);
        }

        if (this.State === tp.DataRowState.Unchanged) {
            this.State = tp.DataRowState.Modified;
        }
    }
    /**
    Returns the value of a column by column name
    @param {string} Name The name of the column
    @return {any}  Returns the value of a column
    */
    GetByName(Name) {
        return this.GetByIndex(this.Table.IndexOfColumn(Name));
    }
    /**
    Sets the value of a column by column name
    @param {string} Name The name of the column
    @param {any} v - The value to set
    */
    SetByName(Name, v) {
        this.SetByIndex(this.Table.IndexOfColumn(Name), v);
    }

    /**
    Returns the value of a column either by column index, column name or the column itself.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} [Default] - Optional. The value to return if the column is null or undefined
    @return {any} Returns the value of a column
    */
    Get(Column, Default) {
        var Index = this.Table.IndexOfColumn(Column);
        var Result = Index >= 0 ? this.Data[Index] : null;
        Result = !tp.IsEmpty(Result) ? Result : (!tp.IsEmpty(Default) ? Default : null);
        return Result;
    }
    /**
    Sets the value of a column either by column index, column name or the column itself.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} v - The value to set
    */
    Set(Column, v) {
        var Index = this.Table.IndexOfColumn(Column);
        this.SetByIndex(Index, v);
    }

    /**
    Removes the row. Actually puts the row to the Table.Deleted cache
    */
    Remove() {
        this.Table.RemoveRow(this);
    }
    /**
    Sets the row state to Unchaged
    */
    AcceptChanges() {
        this.State = tp.DataRowState.Unchanged;
    }
    /**
    Returns true if a column is null.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {boolean} Returns Returns true if a column is null.
    */
    IsNull(Column) {
        var Index = this.Table.IndexOfColumn(Column);
        return tp.IsEmpty(this.GetByIndex(Index));
    }
    /**
    Returns true if Column is null or empty in the Row.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {boolean} Returns a boolean
    */
    IsNullOrEmpty(Column) {

        var Index = this.Table.IndexOfColumn(Column);
        var v = this.GetByIndex(Index);

        if (tp.IsEmpty(v))
            return true;

        if (this.Table.Columns[Index].DataType === tp.DataType.String && tp.IsString(v))
            return tp.IsBlank(v);

        return false;
    }

    /**
    NOT YET IMPLEMENTED 
    @param {number | string | tp.DataColumn} Param The column. 
    */
    GetChildRows(Param) {
        throw "tp.DataRow.prototype.GetChildRows - not yet";
    }
    /**
    Copies values from a specified source row.
    @param {tp.DataRow} SourceRow The source row
    @param {string[]} [ExcludeFieldList] - Optional. An array of field names to be excluded from copying
    */
    CopyFromRow(SourceRow, ExcludeFieldList) {
        ExcludeFieldList = ExcludeFieldList || [];

        for (var i = 0, ln = this.Table.Columns.length; i < ln; i++) {
            if (!tp.ListContainsText(ExcludeFieldList, this.Table.Columns[i].Name)) {
                this.Set(i, SourceRow.Get(i));
            }
        }
    }

    /**
    Returns the row  as a javascript object.
    The object is a key/value pair, where key is the column name. <br />
    i.e. <code>[{ColName: value, ColName: value, ...}, {ColName: value, ColName: value, ...}, ... ]</code> <br />
    NOTE: A field named __Row is added to any returned object. That field points to the row itself.
    @returns {Object} An plain javascript object
    */
    ToObject() {
        var o = {};
        for (var i = 0, ln = this.Table.Columns.length; i < ln; i++) {
            o[this.Table.Columns[i].Name] = this.Data[i];
        }

        o.__Row = this;
        return o;
    }

};
//#endregion  

//---------------------------------------------------------------------------------------
// sorting and filtering
//---------------------------------------------------------------------------------------



//#endregion

//#region tp.DataSourceInfoItem
/**
   Information object for sorting or filtering a data table. It represents the information about a single column. <br />
   Used as the base class of {@link tp.DataTableSortItem} and {@link tp.DataTableFilterItem} classes. <br />
*/
tp.DataSourceInfoItem = class {
    /**
    Constructor
    */
    constructor() {
    }

    /**
    The index of the data item (column), this item represents, in a data row
    @type {number}
    */
    Prop = null;

    /**
    The data type of the column this item represents. One of the {@link tp.DataType} constants.
    @type {tp.DataType}
    */
    DataType = null;

    /** LookUpTable 
     @type {tp.DataTable}
     */
    LookUpTable = null;
    /** LookUpValue 
     * @type {any}
     */
    LookUpValue = null;
    /** ListValueField 
     @type {string}
     */
    ListValueField = null;
    /** ListDisplayField
     @type {string}     
     */
    ListDisplayField = null;
};
//#endregion  

//#region tp.DataTableSortItem
/**
Information object for sorting a data table. It represents the information about a single column.
*/
tp.DataTableSortItem = class extends tp.DataSourceInfoItem {

    /**
    Constructor
    */
    constructor() {
        super();
    }

    /* properties */
    /**
    When true the column is sorted in reverse order
    @default false
    @type {boolean}
    */
    Reverse = false;
    /**
    A callback that is passed an object being sorted and returns the value of one of the properties of that object.<br />
    <code>any GetValueFunc(Row: tp.DataRow, Info: tp.DataTableSortItem)</code>
    @param {tp.DataRow} Row - The object that is currently sorted
    @param {tp.DataTableSortItem} Info - Information about the property
    @returns {any} Returns the value of the property
    */
    GetValueFunc = null; //: (Row: tp.DataRow, Info: tp.DataTableSortItem) => any;

};
//#endregion  

//#region tp.DataTableFilterItem
/**
Information object for filtering a data table. It represents the information about a single column.

*/
tp.DataTableFilterItem = class extends tp.DataSourceInfoItem {

    /**
    Constructor
    */
    constructor() {
        super();
    }

    /* properties */
    /**
    The filter value
    @type {any}
    */
    Value = null;
    /**
    The filter operator. One of the {@link tp.FilterOp} constants
    @type {tp.FilterOp}
    */
    Operator = null;
    /**
    A callback that is passed an object being filtered and returns a boolean value indicating whether the row passes the filter condition. <br />
    <code>boolean FilterFunc(Row: tp.DataRow, Info: tp.DataTableFilterItem)</code>
    @param {tp.DataRow} Row - The object that is currently sorted
    @param {tp.DataTableFilterItem} Info - Information about the property
    @returns {boolean} Returns boolean value indicating whether the row passes the filter condition.
    */
    FilterFunc = null //: (Row: tp.DataRow, Info: tp.DataTableFilterItem) => boolean;

};
//#endregion  

//#region tp.DataSourceInfoList
/**
A base class for an information list class used with sorting and filtering a data table. <br />
This class is a collection of {@link tp.DataSourceInfoItem} items.
*/
tp.DataSourceInfoList = class {

    /**
    Constructor
    @param {tp.DataTable} Table - The table to operate on
    */
    constructor(Table) {
        this.Table = Table;
        this.List = [];
        this.fGetFieldValueFuncBind = this.GetFieldValueFunc.bind(this);
    }

    /* protected */

    /** fGetFieldValueFuncBind 
     @field
     @protected
     @type {any}
     */
    fGetFieldValueFuncBind = null;

    /* properties */
    /** Table 
     @type {tp.DataTable}
     */
    Table = null;
    /** 
    A list of {@link tp.DataSourceInfoItem} items (either {@link tp.DataTableSortItem} or {@link tp.DataTableFilterItem} instances).
    @type {tp.DataSourceInfoItem[]}
     */
    List = [];
    /**
    Returns the length of the information list.
    @returns {number} Returns the length of the information list.
    */
    get Count() {
        return this.List ? this.List.length : 0;
    }


    /* protected */
    /**
    Used with the sorting and filtering  algorithm.
    A callback that is passed an object being sorted or filtered and returns the value of one of its properties <br />
    @protected
    @param {tp.DataRow} Row - The object that is currently sorted
    @param {tp.DataSourceInfoItem} Info - Information about the property
    @returns {any} Returns the value of the property
    */
    GetFieldValueFunc(Row, Info) {
        return Row.Data[Info.Prop];
    }
    /**
    Returns the index of a data column in the data table of this instance
    @protected
    @param { number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {number} Returns the index of a data column in the table of this instance
    */
    IndexOfColumn(Column)  {
        return this.Table.IndexOfColumn(Column);
    }

    /* methods */
    /**
    Empties the information list
    */
    Clear() {
        this.List.length = 0;
    }
    /**
    Maps the index of a data column in the data table of this instance to the index of an information object in the information list, and returns that index
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {number} Returns the index of an information object in the information list
    */
    IndexOf(Column) {
        var DataIndex = this.IndexOfColumn(Column);
        for (var i = 0, ln = this.List.length; i < ln; i++) {
            if (this.List[i].Prop === DataIndex) {
                return i;
            }
        }

        return -1;
    }
    /**
    Returns true if the information list contains an information object associated to a specified data column of the table of this instance.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {boolean} A boolean value
    */
    Contains(Column) {
        return this.IndexOf(Column) !== -1;
    }
    /**
    Finds and returns an information object associated to a specified data column of the table of this instance.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @returns {tp.DataSourceInfoItem} An information object
    */
    Find(Column) {
        var Index = this.IndexOf(Column);
        return Index !== -1 ? this.List[Index] : null;
    }

    /**
    Removes an information object associated to a specified data column of the table of this instance
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    */
    Remove(Column) {
        var Index = this.IndexOf(Column);
        if (Index !== -1) {
            tp.ListRemoveAt(this.List, Index);
        }
    }

};
//#endregion  

//#region tp.SortInfoList
/**
A list of information objects for sorting a data table <br />
This class is a collection of {@link tp.DataTableSortItem} items.
*/
tp.SortInfoList = class extends tp.DataSourceInfoList {

    /**
    Constructor
    @param {tp.DataTable} Table - The table to operate on
    */
    constructor(Table) {
        super(Table);
    }

    /* overrides */
    /**
    Used with the sorting and filtering  algorithm. <br />
    A callback that is passed an object being sorted or filtered and returns the value of one of its properties
    @protected
    @override
    @param {tp.DataRow} Row - The object that is currently sorted
    @param {tp.DataTableSortItem} Info - Information about the property
    @returns {any} Returns the value of the property
    */
    GetFieldValueFunc(Row, Info) {
        let Result = Row.Data[Info.Prop];

        // look-up
        if (Info.LookUpTable) {
            var LookUpRow = Info.LookUpTable.FindRow(Info.ListValueField, Result);
            if (!tp.IsEmpty(LookUpRow)) {
                Result = LookUpRow.Get(Info.ListDisplayField);
            }
        }

        return Result;
    }

    /* methods */
    /**
    Adds an information item in this list.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {boolean} Reverse - When true the column is sorted in reverse order
    @param {Function} [GetValueFunc] - Optional. A callback that is passed an object being sorted and returns the value of one of its properties.<br /> 
    The function signature is <code>any GetValueFunc(Row: tp.DataRow, Info: tp.DataTableSortItem)</code>
    @returns {tp.DataTableSortItem} Returns the newly added item
    */
    Add(Column, Reverse, GetValueFunc) {
        return this.Insert(this.List.length, Column, Reverse, GetValueFunc);
    }
    /**
    Insert an information item in this list at a specified index.
    @param {number} Index The index where to insert the new item.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {boolean} Reverse - When true the column is sorted in reverse order
    @param {Function} [GetValueFunc] - Optional. A callback that is passed an object being sorted and returns the value of one of its properties.<br />
    The function signature is <code>any GetValueFunc(Row: tp.DataRow, Info: tp.DataTableSortItem)</code>
    @returns {tp.DataTableSortItem} Returns the newly added item
    */
    Insert(Index, Column, Reverse, GetValueFunc) {
        var Item = this.Find(Column);
        if (!Item) {
            Item = new tp.DataTableSortItem();

            Item.Prop = this.IndexOfColumn(Column);
            Item.DataType = this.Table.Columns[Item.Prop].DataType;
            Item.Reverse = Reverse === true;
            Item.GetValueFunc = GetValueFunc || this.fGetFieldValueFuncBind;

            tp.ListInsert(this.List, Index, Item);
        }
        return Item;
    }

};
//#endregion  

//#region tp.FilterInfoList
/**
A list of information objects for filtering a data table<br />
This class is a collection of {@link tp.DataTableFilterItem} items.
*/
tp.FilterInfoList = class extends tp.DataSourceInfoList {
    /**
    Constructor
    @param {tp.DataTable} Table - The table to operate on
    @param {boolean} [OrLogic=false] - Defaults to false. True applies OR logic, else AND logic.  
    */
    constructor(Table, OrLogic) {
        super(Table);
        this.OrLogic = OrLogic === true;
        this.fFilterFuncBind = this.FilterFunc.bind(this);
    }

    /* protected */

    /**
     fFilterFuncBind
     @protected
     @type {any}
    */
    fFilterFuncBind = null;

    /* properties */
    /**
    True applies OR logic, else AND logic
    @default false
    @type {boolean}
    */
    OrLogic = false;

    /**
    A callback that is passed an object being filtered and returns a boolean value indicating whether the row passes the filter condition.
    @private
    @param {tp.DataRow} Row - The object that is currently sorted
    @param {tp.DataTableFilterItem} Info - Information about the property
    @returns {boolean} Returns boolean value indicating whether the row passes the filter condition.
    */
    FilterFunc(Row, Info) {
        var Value = this.GetFieldValueFunc(Row, Info);

        if (!tp.IsEmpty(Value)) {

            if (Info.DataType === tp.DataType.Date) {
                Value = tp.ClearTime(Value);
            }
            //else if (Info.DataType === tp.DataType.Time)  
            //    Value = tp.ClearDate(Value);
         

            // look-up
            if (Info.LookUpTable) {
                var LookUpRow = Info.LookUpTable.FindRow(Info.ListValueField, Value);
                if (!tp.IsEmpty(LookUpRow)) {
                    Value = LookUpRow.Get(Info.ListDisplayField);
                }
            }
        }

        return tp.FilterOp.Compare(Info.Operator, Value, Info.Value);
    }


    /* methods */
    /**
    Adds an information item in this list.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {tp.FilterOp} Operator - The filter operator. One of the {@link tp.FilterOp} constants
    @param {any} Value - The filter value
    @param {Function} [FilterFunc] - Optional. A callback that is passed an object being filtered and returns a boolean value indicating whether the row passes the filter condition.<br />
    The function signature is <code>boolean FilterFunc(Row: tp.DataRow, Info: tp.DataTableFilterItem)</code>
    @returns {tp.DataTableFilterItem} Returns the newly added item
    */
    Add(Column, Operator, Value, FilterFunc) {
        return this.Insert(this.List.length, Column, Operator, Value, FilterFunc);
    }
    /**
    Insert an information item in this list at a specified index.
    @param {number} Index The index where to insert the new item.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {tp.FilterOp} Operator - The filter operator. One of the {@link tp.FilterOp} constants
    @param {any} Value - The filter value
    @param {Function} [FilterFunc] - Optional. A callback that is passed an object being filtered and returns a boolean value indicating whether the row passes the filter condition.<br />
    The function signature is <code>boolean FilterFunc(Row: tp.DataRow, Info: tp.DataTableFilterItem)</code>
    @returns {tp.DataTableFilterItem} Returns the newly added item
    */
    Insert(Index, Column, Operator, Value, FilterFunc) {
        var Item = this.Find(Column);
        if (!Item) {
            Item = new tp.DataTableFilterItem();

            Item.Prop = this.IndexOfColumn(Column);
            Item.DataType = this.Table.Columns[Item.Prop].DataType;
            Item.Operator = Operator || tp.FilterOp.Equal;
            Item.FilterFunc = FilterFunc || this.fFilterFuncBind;
            Item.Value = Value;

            tp.ListInsert(this.List, Index, Item);
        }
        return Item;
    }
    /**
    Finds or adds an item. Returns the item.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {tp.FilterOp} Operator - The filter operator. One of the {@link tp.FilterOp} constants
    @param {any} Value - The filter value
    @returns {tp.DataTableFilterItem} Returns the newly added item
    */
    FindOrAdd(Column, Operator, Value) {
        var Item = this.Find(Column);
        if (!Item) {
            Item = this.Add(Column, Operator, Value, null);
        } else {
            Item.Operator = Operator;
            Item.Value = Value;
        }

        return Item;
    }


};
//#endregion  

 
//---------------------------------------------------------------------------------------
// DataSource
//---------------------------------------------------------------------------------------

//#region tp.IDataSourceListener
/**
Represents a listener of datasource notifications
@interface
*/
tp.IDataSourceListener = class {
 
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DataSourceRowCreated(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DataSourceRowAdded(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @param {tp.DataColumn} Column The {@link tp.DataColumn} column
    @param {any} OldValue The old value
    @param {any} NewValue The new value
    */
    DataSourceRowModified(Table, Row, Column, OldValue, NewValue) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    */
    DataSourceRowRemoved(Table, Row) {
    }
    /**
    Notification
    @param {tp.DataTable} Table The {@link tp.DataTable} table
    @param {tp.DataRow} Row The {@link tp.DataRow} row
    @param {number} Position The new position
    */
    DataSourcePositionChanged(Table, Row, Position) {
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

//#region tp.DataSourceListener
/**
A base implementation of a {@link tp.IDataSourceListener} listener of datasource notifications
@implements {tp.IDataSourceListener}
*/
tp.DataSourceListener = class extends tp.tpObject {


    /**
     Constructor
     @param {Object} Owner - The owner object of this listener
     */
    constructor(Owner) {
        super();
        this.Owner = Owner;
    }

    /**
    The owner object of this listener
    @type {Object}
    */
    Owner = null;

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

//#region tp.DataSource
/**
The datasource is part of the data-binding mechanism. It sits between a data table and UI controls bound to that data table data.
The datasource gets data (data rows) from a data table and provides data to controls.
Also the datasource implements the notion of the "current position" by informing controls about position changes.
It is also capable of sorting and filtering the data it provides.
A control has to implement the {@link tp.IDataSourceListener} if it is to get notifications from the datasource
*/
tp.DataSource = class extends tp.tpObject {

    /**
    Constructor
    @param {tp.DataTable} Table - The actual source of data
    */
    constructor(Table) {
        super();

        this.fPosition = -1;
        this.fForcePosition = false;
        this.Table = Table;
        this.fRows = Table.Rows.slice(0);
        this.fListeners = [];
        this.fSuspendBindingCounter = 0;
        this.fDetails = [];

        this.fSortInfoList = new tp.SortInfoList(Table);
        this.fFilterInfoList = new tp.FilterInfoList(Table, false);

        this.Table.On('BatchModified', this.Table_BatchModified, this);
        this.Table.On('RowCreated', this.Table_RowCreated, this);
        this.Table.On('RowAdded', this.Table_RowAdded, this);
        this.Table.On('RowModified', this.Table_RowModified, this);
        this.Table.On('RowRemoved', this.Table_RowRemoved, this);

        if (this.fRows.length > 0)
            this.fPosition = 0;
    }

    /* private */

    /**
     fPosition, -1 means no rows yet
     @private
     @default -1
     @type {number}
     */
    fPosition = -1;
    /** fForcePosition
     @private
     @default false
     @type {boolean}
     */
    fForcePosition = false;
    /**
     fPropagating, is true while informing listeners
     @private
     @default false
     @type {boolean}
     */
    fPropagating = false;
    /**
     fListeners, an array of listeners
     @private
     @type {object[]}
     */
    fListeners = [];
    /**
     fRows, an array of tp.DataRow
     @private
     @type {tp.DataRow[]}
     */
    fRows = [];
    /** fSuspendBindingCounter
     @private
     @type {number}
     */
    fSuspendBindingCounter = 0;
    /** fSortInfoList
     @private
     @type {tp.SortInfoList}
     */
    fSortInfoList = null;
    /** fFilterInfoList
     @private
     @type {tp.FilterInfoList}
     */
    fFilterInfoList = null;


    /**
     fMasterSource, the master datasource, if any.
     @private
     @type {tp.DataSource}
     */
    fMasterSource = null;
    /** fDetails, a list of detail datasources, if any.
     @private
     @type {tp.DataSource[]}
     */
    fDetails = [];
    /** fMasterKeyField
     @private
     @type {string}
     */
    fMasterKeyField = null;
    /**
     fDetailKeyField
     @private
     @type {string}
     */
    fDetailKeyField = null;


    /* properties */
    /**
    The actual source of data
    @type {tp.DataTable}
    */
    Table = null;

    /**
    Returns the name of this instance
    @type {string}
    */
    get Name() {
        return this.Table.Name;
    }
    /**
    Gets or sets the current data row. Setting this property changes the position.
    @type {tp.DataRow}
    */
    get Current() {
        return (this.fPosition >= 0) || (this.fPosition <= (this.fRows.length - 1)) ? this.fRows[this.fPosition] : null;
    }
    set Current(v) {
        var Index = this.fRows.indexOf(v);
        if (Index !== -1) {
            this.Position = Index;
        }
    }
    /**
    Gets or sets the position
    @type {number}
    */
    get Position() {
        return this.fPosition;
    }
    set Position(v) {
        let Flag = v >= 0 && v <= this.fRows.length && (v !== this.fPosition || this.fForcePosition);

        this.fForcePosition = false;

        if (Flag) {
            this.fPosition = v;

            // inform listeners
            if (!this.BindingSuspended && !this.fPropagating) {
                this.fPropagating = true;
                try {
                    var i, ln,
                        Table = this.Table, Row = this.Current;

                    for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                        this.fListeners[i].DataSourcePositionChanged(Table, Row, v);
                    }

                    for (i = 0, ln = this.fDetails.length; i < ln; i++) {
                        this.fDetails[i].MasterSource_PositionChanged();
                    }

                    this.OnPositionChanged();
                } finally {
                    this.fPropagating = false;
                }
            }
        }
    }
    /**
    Returns the number of rows. It takes into account any active filter.
    @type {number}
    */
    get Count() {
        return this.fRows.length;
    }
    /**
    Returns the list of data rows. It takes into account any active filter.
    @type {tp.DataRow[]}
    */
    get Rows() {
        return this.fRows;
    }
    /**
    Gets or sets a boolean value indicating whether listener notification is active or not.
    @type {boolean}
    */
    get BindingSuspended() {
        return this.fSuspendBindingCounter > 0;
    }
    set BindingSuspended(v) {
        this.fSuspendBindingCounter = v === true ? this.fSuspendBindingCounter + 1 : this.fSuspendBindingCounter - 1;
        if (this.fSuspendBindingCounter < 0)
            this.fSuspendBindingCounter = 0;

        if (this.BindingSuspended === false) {
            this.fForcePosition = true;
            this.Position = this.Position;
        }
    }
    /**
    Returns true if position is in the first row
    @type {boolean}
    */
    get IsFirst() {
        return this.Count === 0 || this.Position <= 0;
    }
    /**
    Returns true if position is in the last row
    @type {boolean}
    */
    get IsLast() {
        return this.Count === 0 || this.Position === this.Count - 1;
    }

    /**
    Gets the last applied sort information
    @type {tp.SortInfoList}
    */
    get SortInfoList() {
        return this.fSortInfoList;
    }
    /**
    Gets the last applied filter information
    @type {tp.FilterInfoList}
    */
    get FilterInfoList() {
        return this.fFilterInfoList;
    }

    /** Gets or sets the master datasource, if any.
     @type {tp.DataSource}
     */
    get MasterSource() {
        return this.fMasterSource;
    }
    set MasterSource(v) {
        if (this.fMasterSource instanceof tp.DataSource) {
            tp.ListRemove(this.fMasterSource.fDetails, this);
        }

        this.fMasterSource = v;

        if (this.fMasterSource instanceof tp.DataSource) {
            this.fMasterSource.fDetails.push(this);
        }

        this.MasterSource_PositionChanged();
    }
    /** Gets or sets the master key field. Defaults to Id. The MasterKeyField belongs to the MasterSource and maps to the DetailKeyField. Used when there is a master-detail relationship.
     @default Id
     @type {string}
     */
    get MasterKeyField() {
        return !tp.IsBlank(this.fMasterKeyField) ? this.fMasterKeyField : 'Id';
    }
    set MasterKeyField(v) {
        this.fMasterKeyField = v;
        this.MasterSource_PositionChanged();
    }
    /** Gets or sets the detail key field. The DetailKeyField belongs to this datasource and maps to the MasterKeyField of the MasterSource. Used when there is a master-detail relationship.
     @type {string}
     */
    get DetailKeyField() {
        return this.fDetailKeyField;
    }
    set DetailKeyField(v) {
        this.fDetailKeyField = v;
        this.MasterSource_PositionChanged();
    }


    /** Returns a list of rows that considered the set of working rows.
     @protected
     @returns {tp.DataRow[]} Returns a list of rows that considered the set of working rows.
     */
    GetWorkingRows() {

        if (this.fMasterSource instanceof tp.DataSource) {
            let MasterKeyIndex = this.fMasterSource.Table.IndexOfColumn(this.MasterKeyField);
            let DetailKeyIndex = this.Table.IndexOfColumn(this.DetailKeyField);

            if (MasterKeyIndex >= 0 && DetailKeyIndex >= 0) {
                let v2, v = this.fMasterSource.Get(MasterKeyIndex);
                if (!tp.IsEmpty(v)) {
                    let Result = [];
                    let List = this.Table.Rows.slice(0);
                    for (let i = 0, ln = List.length; i < ln; i++) {
                        v2 = List[i].Get(DetailKeyIndex);
                        if (v === v2) {
                            Result.push(List[i]);
                        }
                    }
                    return Result;
                }
            }
        }

        return this.Table.Rows.slice(0);
    }
    /**
     Event handler. Called when the MasterSource changes position.
     @protected
     */
    MasterSource_PositionChanged() {
        this.Update();
        this.Filter();
        this.Sort();
    }


    /* private - event handlers */

    /** Event handler.
     @private
     @param {tp.DataTableEventArgs} Args The passed arguments.
     */
    Table_BatchModified(Args) {
        //this.fRows = this.GetWorkingRows();
        this.Update();
    }
    /** Event handler.
     @private
     @param {tp.DataTableEventArgs} Args The passed arguments.
     */
    Table_RowCreated(Args) {
        // inform listeners
        if (!this.BindingSuspended && !this.fPropagating) {
            this.fPropagating = true;
            try {
                var i, ln,
                    Table = Args.Table, Row = Args.Row;

                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceRowCreated(Table, Row);
                }

                this.OnRowCreated(Args);
            } finally {
                this.fPropagating = false;
            }
        }
    }
    /** Event handler.
     @private
     @param {tp.DataTableEventArgs} Args The passed arguments.
     */
    Table_RowAdded(Args) {
        this.fRows.push(Args.Row);

        // inform listeners
        if (!this.BindingSuspended && !this.fPropagating) {
            this.fPropagating = true;
            try {
                var i, ln,
                    Table = Args.Table, Row = Args.Row;

                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceRowAdded(Table, Row);
                }

                this.OnRowAdded(Args);
            } finally {
                this.fPropagating = false;
            }
        }

        if (this.fPosition === -1) {
            this.Position = 0;
        }
    }
    /** Event handler.
     @private
     @param {tp.DataTableEventArgs} Args The passed arguments.
     */
    Table_RowModified(Args) {
        /*
        Args = {
        Table: this.Table,
        Row: this,
        Column: this.Table.Columns[Index],
        OldValue: this.Data[Index],
        NewValue: v,
        }
        */

        // inform listeners
        if (!this.BindingSuspended && !this.fPropagating) {
            this.fPropagating = true;
            try {
                var i, ln,
                    Table = Args.Table, Row = Args.Row,
                    Column = Args.Column, OldValue = Args.OldValue, NewValue = Args.NewValue;

                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceRowModified(Table, Row, Column, OldValue, NewValue);
                }

                this.OnRowModified(Args);

            } finally {
                this.fPropagating = false;
            }
        }
    }
    /** Event handler.
     @private
     @param {tp.DataTableEventArgs} Args The passed arguments.
     */
    Table_RowRemoved(Args) {

        var NewPosition = -2;

        if (Args.Row === this.Current) {
            if ((this.fPosition === 0) || (this.fPosition === (this.fRows.length - 1))) {
                NewPosition = this.fRows.length === 1 ? -1 : this.fPosition - 1;
            }
        }

        tp.ListRemove(this.fRows, Args.Row);

        // inform listeners
        if (!this.BindingSuspended && !this.fPropagating) {
            this.fPropagating = true;
            try {
                var i, ln,
                    Table = Args.Table, Row = Args.Row;

                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceRowRemoved(Table, Row);
                }

                this.OnRowRemoved(Args);
            } finally {
                this.fPropagating = false;
            }
        }



        if (NewPosition !== -2) {
            this.Position = NewPosition;
        }
    }

    /* overrides */
    /**
    Initializes the 'static' and 'read-only' class fields, such as tpClass etc.
    @protected
    */
    InitClass() {
        this.tpClass = 'tp.DataSource';
    }

    /* methods */
    /**
    Registers a listener.
    @param {tp.IDataSourceListener} Listener An object that is or implements the {@link tp.IDataSourceListener} interface.
    */
    AddDataListener(Listener) {
        if (!tp.ListContains(this.fListeners, Listener)) {
            this.fListeners.push(Listener);
        }
    }
    /**
    Removes a listener
    @param {tp.IDataSourceListener} Listener An object that is or implements the {@link tp.IDataSourceListener} interface.
    */
    RemoveDataListener(Listener) {
        tp.ListRemove(this.fListeners, Listener);
    }

    /**
    Sets position to the first data row. It respects any active filter.
    */
    First() {
        if (this.CanFirst()) {
            this.Position = 0;
        }
    }
    /**
    Decreases the position by 1. It respects any active filter.
    */
    Prior() {
        if (this.CanPrior()) {
            this.Position = this.fPosition - 1;
        }
    }
    /**
    Increases the position by 1. It respects any active filter.
    */
    Next() {
        if (this.CanNext()) {
            this.Position = this.fPosition + 1;
        }
    }
    /**
    Sets the position to the last data row. It respects any active filter.
    */
    Last() {
        if (this.CanLast()) {
            this.Position = this.fRows.length - 1;
        }
    }

    /**
    Moves position to a specified position. It respects any active filter.
    @param {number} NewPosition The new position
    */
    Move(NewPosition) {
        if (this.CanMoveTo(NewPosition)) {
            this.Position = NewPosition;
        }
    }
    /**
    Returns true if can move to a specified position.
    @param {number} NewPosition The new position
    @returns {boolean} Returns true if can move to a specified position.
    */
    CanMoveTo(NewPosition) {
        return (NewPosition >= 0) && (NewPosition <= this.fRows.length - 1);
    }

    /**
    Returns true if can move to a new position.
    @returns {boolean} Returns true if can move to a new position.
    */
    CanFirst() {
        return !this.IsFirst && this.CanMoveTo(0);
    }
    /**
    Returns true if can move to a new position.
    @returns {boolean}  Returns true if can move to a new position.
    */
    CanNext() {
        return !this.IsLast && this.CanMoveTo(this.fPosition + 1);
    }
    /**
    Returns true if can move to a new position.
    @returns {boolean} Returns true if can move to a new position.
    */
    CanPrior() {
        return !this.IsFirst && this.CanMoveTo(this.fPosition - 1);
    }
    /**
    Returns true if can move to a new position.
    @returns {boolean} Returns true if can move to a new position.
    */
    CanLast() {
        return !this.IsLast && this.CanMoveTo(this.fRows.length - 1);
    }


    /**
    Removes all rows from the table. All deleted rows go to deleted cache.
    */
    ClearRows() {
        this.Table.ClearRows();
    }

    /**
    Creates and adds a new empty row to Rows. Returns the newly added row
    @returns {tp.DataRow} Returns the newly added data row
    */
    AddEmptyRow() {
        return this.Table.AddEmptyRow();
    }
    /**
    Adds a row to the table Rows and returns the newly added data row
    @param {...Data} Data - Rest parameter. Data can be 1. unspecified 2. a tp.DataRow 3. a javascript array of values, or 4. just arguments separated by commas.
    @returns { tp.DataRow} Returns the newly added data row
    */
    AddNew(...Data) {
        return this.Table.AddRow(Data);
    }
    /**
    Creates and returns a new row. The new row is NOT added to the rows. The new row can be added with an explicit call to AddRow(). <br />
    NOTE: If AutoGenerateGuidKeys is true and a PrimaryKeyField is defined, then the value of that field is set to a new GUID string.
    @param {any[]} [Data] - Optional. The data array
    @returns {tp.DataRow} Returns a data row
    */
    NewRow(Data) {
        return this.Table.NewRow(Data);
    }


    /**
    Returns a value of a column, of a specified row, either by column index, column name or the column itself.
    @param {tp.DataRow} Row The row to operate on.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} [Default] - Optional. The value to return if the column is null or undefined
    @return {any} Returns the value of a column
    */
    GetValue(Row, Column, Default) {
        return tp.IsEmpty(Row) ? Default : Row.Get(Column, Default);
    }
    /**
    Sets the value of a column,  of a specified row, either by column index, column name or the column itself.
    @param {tp.DataRow} Row The row to operate on.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} v - The value to set
    */
    SetValue(Row, Column, v) {
        if (!tp.IsEmpty(Row)) {
            Row.Set(Column, v);
        }
    }
    /**
    Returns a value of a column, of the current row, either by column index, column name or the column itself.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} [Default] - Optional. The value to return if the column is null or undefined
    @return {any} Returns the value of a column
    */
    Get(Column, Default) {
        return this.GetValue(this.Current, Column, Default);
    }
    /**
    Sets the value of a column,  of the current row, either by column index, column name or the column itself.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} v - The value to set
    */
    Set(Column, v) {
        this.SetValue(this.Current, Column, v);
    }

    /**
    Sorts the datasource rows by multiple fields based on the sort information list of this instance.
    */
    Sort() {
        var i, ln;

        if (!tp.IsEmpty(this.SortInfoList) && this.SortInfoList.Count > 0) {
            tp.ListSort(this.fRows, this.SortInfoList.List);

            if (!this.BindingSuspended) {
                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceSorted();
                }

                this.OnSorted();
            }


        }

    }
    /**
    Filters the datasource rows by multiple fields based on the filter information list of this instance.
    */
    Filter() {
        var i, ln;

        if (!tp.IsEmpty(this.FilterInfoList)) {

            var Rows = this.GetWorkingRows();

            if (this.FilterInfoList.Count > 0) {
                this.fRows = tp.ListFilter(Rows, this.FilterInfoList.List, this.FilterInfoList.OrLogic);
            } else {
                this.fRows = Rows;
            }

            // if it was sorted, restore the sorting
            if (!tp.IsEmpty(this.SortInfoList) && this.SortInfoList.Count > 0) {
                tp.ListSort(this.fRows, this.SortInfoList.List);
            }

            if (this.fRows.length === 0) {
                this.fPosition = -1;
            } else if (!tp.InRange(this.fRows, this.Position)) {
                this.Position = 0;
            }

            if (!this.BindingSuspended) {
                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceFiltered();
                }

                this.OnFiltered();
            }
        }

    }
    /**
    Cancels any previously applied filter.
    */
    CancelFilter() {
        let i, ln;

        this.fRows = this.GetWorkingRows();

        // if it was sorted, restore the sorting
        if (!tp.IsEmpty(this.SortInfoList) && this.SortInfoList.Count > 0) {
            tp.ListSort(this.fRows, this.SortInfoList.List);
        }

        if (this.fRows.length === 0) {
            this.fPosition = -1;
        } else if (!tp.InRange(this.fRows, this.Position)) {
            this.Position = 0;
        }

        if (!this.BindingSuspended) {
            for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                this.fListeners[i].DataSourceFiltered();
            }
        }
    }
    /**
    Should be called after a full update of the datasource rows (e.g. after a commit)
    */
    Update() {

        this.fRows = this.GetWorkingRows();

        var i, ln, Pos = this.Position;
        this.fPosition = -1;

        this.Position = 0;

        // inform listeners
        if (!this.BindingSuspended && !this.fPropagating) {
            this.fPropagating = true;
            try {
                for (i = 0, ln = this.fListeners.length; i < ln; i++) {
                    this.fListeners[i].DataSourceUpdated();
                }

                this.OnUpdated();
            } finally {
                this.fPropagating = false;
            }
        }

        if (Pos >= 0)
            this.Position = Pos;
    }

    /**
    Sorts this datasource on a column.
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {boolean} [Reverse] Optional. A flag. Controls the sort order.
    */
    SortOn(Column, Reverse) {
        let ColumnIndex = this.Table.IndexOfColumn(Column);
        if (ColumnIndex >= 0) {
            Reverse = Reverse === true;
            this.SortInfoList.Clear();
            let SortItem = this.SortInfoList.Add(ColumnIndex, Reverse);
            this.Sort();
        }
    }
    /**
    Filters this datasource on a column
    @param {number | string | tp.DataColumn} Column - Could be a number (index), a string (field name) or a data column
    @param {any} v The filter value. Rows with a column containing that value pass the filter test.
    */
    FilterOn(Column, v) {
        let ColumnIndex = this.Table.IndexOfColumn(Column);
        if (ColumnIndex >= 0) {
            this.FilterInfoList.Clear();
            let FilterItem = this.FilterInfoList.FindOrAdd(ColumnIndex, tp.FilterOp.EQ, v);
            this.Filter();
        }
    }


    /* Event triggers */
    /**
    Event trigger
    @param {tp.DataTableEventArgs} Source The source arguments of the event.
    */
    OnRowCreated(Source) {
        let Args = new tp.DataSourceEventArgs(Source);
        this.Trigger('RowCreated', Args);
    }
    /**
    Event trigger
    @param {tp.DataTableEventArgs} Source The source arguments of the event.
    */
    OnRowAdded(Source) {
        let Args = new tp.DataSourceEventArgs(Source);
        this.Trigger('RowAdded', Args);
    }
    /**
    Event trigger
    @param {tp.DataTableEventArgs} Source The source arguments of the event.
    */
    OnRowModified(Source) {
        let Args = new tp.DataSourceEventArgs(Source);
        this.Trigger('RowModified', Args);
    }
    /**
    Event trigger
    @param {tp.DataTableEventArgs} Source The source arguments of the event.
    */
    OnRowRemoved(Source) {
        let Args = new tp.DataSourceEventArgs(Source);
        this.Trigger('RowRemoved', Args);
    }
    /**
    Event trigger
    */
    OnPositionChanged() {
        let Args = new tp.DataSourceEventArgs(null, null, null, null);
        this.Trigger('PositionChanged', Args);
    }
    /**
    Event trigger
    */
    OnSorted() {
        let Args = new tp.DataSourceEventArgs(null, null, null, null);
        this.Trigger('Sorted', Args);
    }
    /**
    Event trigger
    */
    OnFiltered() {
        let Args = new tp.DataSourceEventArgs(null, null, null, null);
        this.Trigger('Filtered', Args);
    }
    /**
    Event trigger
    */
    OnUpdated() {
        let Args = new tp.DataSourceEventArgs(null, null, null, null);
        this.Trigger('Updated', Args);
    }
};
//#endregion  
  