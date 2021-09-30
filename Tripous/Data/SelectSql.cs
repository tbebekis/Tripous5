/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Data;

using Newtonsoft.Json;

namespace Tripous.Data
{

    /*   

    SelectSqlEditDialog
    ColumnSettingsGridControl

    select
      Id,
      Code,
      Name
    from
      Material
      ///////////////////////////////////////////////////// 
select
  Job.Title              as Job
 ,Trade.Code             as Code
 ,Trade.TradeDate        as Date
 ,Trade.Closed           as Closed
 ,Trader.Name            as Trader
from
  Trade
    left join Job on Job.Id = Trade.JobId
    left join Trader on Trader.Id = Trade.TraderId
      ///////////////////////////////////////////////////// 
select
  Trade.Id               as Id
 ,Job.Title              as Job
 ,Trade.Code             as Code
 ,Trade.TradeDate        as Date
 ,Trade.Closed           as Closed
from
  Trade
    left join Job on Job.Id = Trade.JobId
    left join Trader on Trader.Id = Trade.TraderId
     */


    /// <summary>
    /// Represents a SELECT S statement. It provides the means to parse a SELECT statement.
    /// <para>Provides a property for each clause, i.e. Select, From, etc. </para>
    /// </summary>
    public class SelectSql 
    {
        /* private */ 
        string fConnectionName;

        /* static public */
        /// <summary>
        /// LineBreak
        /// </summary>
        static public readonly string LB = Environment.NewLine; // Line Break
        /// <summary>
        /// Space 
        /// </summary>
        static public readonly string SPACE = " ";
        /// <summary>
        /// Spaces
        /// </summary>
        static public readonly string SPACES = "  ";
        /// <summary>
        /// Constant
        /// </summary>
        static public readonly string DefaultName = "SelectSql";

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SelectSql()
        {
            this.Name = DefaultName;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public SelectSql(string StatementText)
            : this()
        {
            this.Text = StatementText;
        }



        /* static */
        /// <summary>
        /// Adds a Carriage Return (LineBreak) after S
        /// </summary>
        static public string CR(string S)
        {
            if (S != null)
                S = S.TrimEnd() + LB;

            return S;
        }
        /// <summary>
        /// Replaces any trailing comma with space in S, ignoring trailing spaces.
        /// </summary>
        static public void ReplaceLastComma(ref string S)
        {
            if (!string.IsNullOrWhiteSpace(S))
            {
                StringBuilder SB = new StringBuilder(S);

                int i = SB.Length;
                char C;

                while (true)
                {
                    i--;
                    if (i < 0)
                        break;

                    C = SB[i];

                    if (C == ',')
                    {
                        SB[i] = ' ';
                        break;
                    }
                    else if (!Char.IsWhiteSpace(C))
                        break;

                }

                S = SB.ToString();
            }

        }
        /// <summary>
        /// Concatenates Clause + Delimiter + Plus 
        /// </summary>
        static public string AddTo(string Clause, string Delimiter, string Plus)
        {
            if (string.IsNullOrWhiteSpace(Plus))
                Plus = string.Empty;

            if (!string.IsNullOrWhiteSpace(Clause) && !string.IsNullOrWhiteSpace(Delimiter))
            {
                Clause = Clause.TrimEnd();
                Delimiter = Delimiter.Trim();
                Plus = Plus.Trim();

                if (Clause.EndsWith(Delimiter, StringComparison.InvariantCultureIgnoreCase)
                    || Plus.StartsWith(Delimiter, StringComparison.InvariantCultureIgnoreCase))
                    return CR(Clause) + SPACES + Plus.Trim();
                else
                    return CR(Clause) + SPACES + Delimiter + SPACE + Plus;
            }
 
            return SPACES + Plus.Trim();

        }
        /// <summary>
        /// Concatenates Keyword + Clause
        /// </summary>
        static public string NormalizeClause(string Clause, string Keyword)
        {
            if (!string.IsNullOrWhiteSpace(Clause) && !string.IsNullOrWhiteSpace(Keyword))
            {
                Clause = Clause.Trim();
                Keyword = Keyword.Trim();

                if (!Clause.StartsWith(Keyword, StringComparison.InvariantCultureIgnoreCase))
                    return CR(Keyword) + SPACES + Clause;
            }

            return !string.IsNullOrWhiteSpace(Clause)? Clause: string.Empty;
        }
        /// <summary>
        /// Returns true if Value contains any of the mask characters (%, ?, *)
        /// </summary>
        static public bool IsMasked(string Value)
        {
            return Sql.IsMasked(Value);
        }

        /* DateRange */
        /// <summary>
        /// Constructs a DateTime param pair (date range params) suitable for thw WHERE part in a SelectSql
        /// </summary>
        static public string DateRangeConstructWhereParams(DateRange Range, string FieldName)
        {
            string sFrom = ":" + DateRanges.PrefixFrom + Range.ToString();
            string sTo = ":" + DateRanges.PrefixTo + Range.ToString();

            string S = string.Format(" (({0} >= {1}) and ({0} <= {2})) ", FieldName, sFrom, sTo);

            return S;
        }
        /// <summary>
        /// Replaces any DateTime param pair (date range params) found in SqlText with actual values.
        /// </summary>
        static public void DateRangeReplaceWhereParams(ref string SqlText, SqlProvider Provider)
        {
            if (!SqlText.Contains(DateRanges.PrefixFrom))
                return;

            string sFrom = string.Empty;
            string sTo = string.Empty;


            //----------------------------------------------------
            Func<DateRange, string, string> Replace = delegate (DateRange Range, string S)
            {
                DateTime FromDate = Sys.Today;
                DateTime ToDate = Sys.Today;

                Range.ToDates(Sys.Today, ref FromDate, ref ToDate);

                string sFromValue = Provider.QSDateTime(FromDate.StartOfDay());
                string sToValue = Provider.QSDateTime(ToDate.EndOfDay());

                S = S.Replace(sFrom, sFromValue);
                S = S.Replace(sTo, sToValue);

                return S;
            };
            //----------------------------------------------------

            string Prefix = SqlProvider.GlobalPrefix.ToString();
            foreach (DateRange Range in DateRanges.WhereRanges)
            {
                sFrom = Prefix + DateRanges.PrefixFrom + Range.ToString();  // :FROM_DATE_RANGE_<field name>
                if (SqlText.Contains(sFrom))
                {
                    sTo = Prefix + DateRanges.PrefixTo + Range.ToString();  // :TO_DATE_RANGE_<field name>    
                    SqlText = Replace(Range, SqlText);
                    return;
                }
            }
        }

        /* instance */
        /// <summary>
        /// Clears the content of the clause properties
        /// </summary>
        public void Clear()
        {
            Select = string.Empty;
            From = string.Empty;
            Where = string.Empty;
            WhereUser = string.Empty;
            GroupBy = string.Empty;
            Having = string.Empty;
            OrderBy = string.Empty;

            CompanyAware = false;
            ConnectionName = string.Empty;

            CheckBoxColumns = string.Empty;
            MemoColumns = string.Empty;
            DateTimeColumns = string.Empty;
            DateColumns = string.Empty;
            TimeColumns = string.Empty;
            ImageColumns = string.Empty; 

            DateRangeColumn = string.Empty;
            DateRange = Tripous.DateRange.Custom;

            DisplayLabels = string.Empty;
            ColumnSettings.Clear();
            SqlFilters.Clear();

        }
        /// <summary>
        /// Assigns this properties from Source.
        /// <para>WARNING: We need this because an automated Assign() calls the Text property
        /// which in turn calls the SelectSqlParser which has no ability to handle fields 
        /// surrounded by double quotes or [] or something.</para>
        /// </summary>
        public void Assign(object Source)
        {
            if (Source is SelectSql)
            {
                SelectSql Src = Source as SelectSql;

                this.Name = Src.Name;

                this.TitleKey = Src.TitleKey;

                Select = Src.Select;
                From = Src.From;
                Where = Src.Where;
                WhereUser = Src.WhereUser;
                GroupBy = Src.GroupBy;
                Having = Src.Having;
                OrderBy = Src.OrderBy;

                CompanyAware = Src.CompanyAware;
                ConnectionName = Src.ConnectionName;

                CheckBoxColumns = Src.CheckBoxColumns;
                MemoColumns = Src.MemoColumns;
                DateTimeColumns = Src.DateTimeColumns;
                DateColumns = Src.DateColumns;
                TimeColumns = Src.TimeColumns;
                ImageColumns = Src.ImageColumns;

                DateRangeColumn = Src.DateRangeColumn;
                DateRange = Src.DateRange;

                DisplayLabels = Src.DisplayLabels;
                ColumnSettings = Src.ColumnSettings;
                SqlFilters = Src.SqlFilters;
 

            }
            else
            {
                Clear();
            }
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public SelectSql Clone()
        {
            SelectSql Result = new SelectSql();
            Result.Assign(this);
            return Result;
        }

        /// <summary>
        /// Concatenates Keyword + Clause for all clauses
        /// </summary>
        public string GetSqlText()
        {
            // select
            string sSelect = NormalizeClause(Select, "select") + SPACES;

            // from
            string sFrom = NormalizeClause(From, "from") + SPACES;

            // where
            string sWhere = string.IsNullOrWhiteSpace(Where) ? string.Empty : Where.Trim();

            if (CompanyAware)
            {
                string sCompany = SysConfig.CompanyFieldName + string.Format(" = {0}{1}", SysConfig.VariablesPrefix, SysConfig.CompanyFieldName);

                if (!sWhere.Contains(sCompany.Trim()))
                    sWhere = sWhere.Length == 0 ? sCompany : AddTo(sWhere, "and", sCompany);
            }

            if (!string.IsNullOrWhiteSpace(DateRangeColumn) && this.DateRange.IsPast())
            {
                string Range = SelectSql.DateRangeConstructWhereParams(this.DateRange, DateRangeColumn);

                if (!sWhere.Contains(Range.Trim()))
                    sWhere = sWhere.Length == 0 ? Range : AddTo(sWhere, "and", Range);
            }

            if (!string.IsNullOrWhiteSpace(WhereUser) && WhereUser.Trim().Length > 0)
            {
                if (!sWhere.Contains(WhereUser.Trim()))
                    sWhere = sWhere.Length == 0 ? WhereUser : AddTo(sWhere, "and", WhereUser);
            }


            sWhere = NormalizeClause(sWhere, "where");

            // group by
            string sGroupBy = NormalizeClause(GroupBy, "group by");

            // having
            string sHaving = NormalizeClause(Having, "having");

            // order by
            string sOrderBy = NormalizeClause(OrderBy, "order by");

            string Result = sSelect.TrimEnd() + LB + sFrom.TrimEnd();
            if (sWhere.Trim().Length > 0) Result += LB + sWhere.TrimEnd() + SPACES;
            if (sGroupBy.Trim().Length > 0) Result += LB + sGroupBy.TrimEnd() + SPACES;
            if (sHaving.Trim().Length > 0) Result += LB + sHaving.TrimEnd() + SPACES;
            if (sOrderBy.Trim().Length > 0) Result += LB + sOrderBy.TrimEnd() + SPACES;
            return Result;

        }

        /// <summary>
        /// Concatenates WHERE + and + Plus 
        /// </summary>
        public void AddToWhere(string Plus)
        {
            Where = AddTo(Where, "and", Plus);
        }
        /// <summary>
        /// Concatenates WHERE + or + Plus 
        /// </summary>
        public void OrToWhere(string Plus)
        {
            Where = AddTo(Where, "or", Plus);
        }
        /// <summary>
        /// Concatenates WHERE + Delimiter + Plus 
        /// </summary>
        public void AddToWhere(string Plus, string Delimiter)
        {
            Where = AddTo(Where, Delimiter, Plus);
        }
        /// <summary>
        /// Concatenates GROUP BY + , + Plus 
        /// </summary>
        public void AddToGroupBy(string Plus)
        {
            GroupBy = AddTo(GroupBy, ",", Plus);
        }
        /// <summary>
        /// Concatenates HAVING + and + Plus 
        /// </summary>
        public void AddToHaving(string Plus)
        {
            Having = AddTo(Having, "and", Plus);
        }
        /// <summary>
        /// Concatenates ORDER BY + , + Plus 
        /// </summary>
        public void AddToOrderBy(string Plus)
        {
            OrderBy = AddTo(OrderBy, ",", Plus);
        }

        /// <summary>
        /// Returns concatenated the SELECT and FROM clauses only.
        /// </summary>
        public string SelectFromToString()
        {
            return NormalizeClause(Select, "select") + LB + NormalizeClause(From, "from");
        }

        /// <summary>
        /// Parses StatementText and assigns its clause properties.
        /// </summary>
        public void Parse(string StatementText)
        {

            Select = string.Empty;
            From = string.Empty;
            Where = string.Empty;
            WhereUser = string.Empty;
            GroupBy = string.Empty;
            Having = string.Empty;
            OrderBy = string.Empty;


            if (!string.IsNullOrWhiteSpace(StatementText))
            {
                SelectSqlParser parser = SelectSqlParser.Execute(StatementText);

                this.Select = parser.Select;
                this.From = parser.From;
                this.Where = parser.Where;
                this.GroupBy = parser.GroupBy;
                this.Having = parser.Having;
                this.OrderBy = parser.OrderBy;
            }
        }
        /// <summary>
        /// Creates a select * from TableName statement and then calls Parse()
        /// </summary>
        public void ParseFromTableName(string TableName)
        {
            Parse("select * from " + TableName);
        }

        /// <summary>
        /// Tries to get the main table name from the statement
        /// </summary>
        public string GetMainTableName()
        {
            string S = this.From.Trim();

            if (!string.IsNullOrWhiteSpace(S))
            {
                while (S.Contains("  "))
                    S = S.Replace("  ", " ");

                string[] Parts = S.Split(' ');
                if ((Parts != null) && (Parts.Length > 0))
                    return Parts[0];
            }

            return string.Empty;
        }
        /// <summary>
        /// Sets a condition (pseudo-property) for each Table Column to a value (i.e. IsDateTime, IsCheckBox, etc).
        /// <para>NOTE: The condition is a pseudo-property defined in Table.ExtendedProperties.</para>
        /// </summary>
        public void SetColumnTypes(DataTable Table)
        {
            foreach (DataColumn Column in Table.Columns)
            {
                Column.IsDateTime(this.DateTimeColumns.ContainsText(Column.ColumnName) ? true : false);
                Column.IsDate(this.DateColumns.ContainsText(Column.ColumnName) ? true : false);
                Column.IsTime(this.TimeColumns.ContainsText(Column.ColumnName) ? true : false);
                Column.IsCheckBox(this.CheckBoxColumns.ContainsText(Column.ColumnName) ? true : false);
                Column.IsMemo(this.MemoColumns.ContainsText(Column.ColumnName) ? true : false);
                Column.IsImage(this.ImageColumns.ContainsText(Column.ColumnName) ? true : false);
            }
        }
        /// <summary>
        /// Sets-up the column types, the captions and the visibility of Table.Columns.
        /// <para>Returns true if has DisplayLabels or ColumnSettings in order to setup the Table.</para>
        /// </summary>
        public bool SetupTable(DataTable Table)
        {
            // table name
            if (string.IsNullOrWhiteSpace(Table.TableName) || Table.TableName.StartsWithText("Table_"))
            {
                Table.TableName = (!string.IsNullOrWhiteSpace(this.Name) && !DefaultName.IsSameText(this.Name)) ?
                                    this.Name :
                                    MemTable.NextTableName();
            }

            // column types in Table.ExtendedProperties (pseudo-properties)
            SetColumnTypes(Table);


            // column titles and visibility
            if (HasDisplayLabels)
            {
                NameValueStringList List = new NameValueStringList(DisplayLabels, true);
                foreach (DataColumn Column in Table.Columns)
                {
                    if (!List.ContainsName(Column.ColumnName))
                    {
                        Column.IsVisible(false);
                    }
                    else
                    {
                        Column.IsVisible(true);
                        Column.Caption = List.Values[Column.ColumnName];
                    }
                }
            }
            else if (HasColumnSettings)
            {
                ColumnSetting Setting;
                foreach (DataColumn Column in Table.Columns)
                {
                    Setting =   ColumnSettings.Find(item => item.Name == Column.ColumnName);
                    if (Setting == null)
                    {
                        Column.IsVisible(false);
                    }
                    else
                    {
                        Column.IsVisible(Setting.Visible);
                        Column.Caption = Setting.Title;
                    }
                }
            }


            return HasDisplayLabels || HasColumnSettings;

        }
        /// <summary>
        /// Translates (localizes) the column captions in DisplayLabels
        /// </summary>
        public void TranslateColumnCaptions()
        {
            if (HasDisplayLabels)
            {
                NameValueStringList List = new NameValueStringList(DisplayLabels, true);
                this.DisplayLabels = List.Text;
            }
        }


        /* properties */
        /// <summary>
        /// A name for this SELECT statement. Used when this goes into SELECT statement lists.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        [JsonIgnore]
        public string Title => !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name;
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// Gets the statement as a whole. 
        /// <para>Sets clause properties by parsing the passed string value.</para>
        /// </summary>
        public string Text
        {
            get { return string.IsNullOrWhiteSpace(Select) ? string.Empty : GetSqlText(); }
            set { Parse(value); }
        }
       
        /// <summary>
        /// Gets or sets the SELECT clause.
        /// </summary>
        [JsonIgnore]
        public string Select { get; set; }
        /// <summary>
        /// Gets or sets the FROM clause.
        /// </summary>
        [JsonIgnore]
        public string From { get; set; }
        /// <summary>
        /// Gets or sets the WHERE clause.
        /// </summary>
        [JsonIgnore]
        public string Where { get; set; }
        /// <summary>
        /// Gets or sets the WHERE clause, that part that is considered user where.
        /// </summary>
        [JsonIgnore]
        public string WhereUser { get; set; }
        /// <summary>
        /// Gets or sets the GROUP BY clause.
        /// </summary>
        [JsonIgnore]
        public string GroupBy { get; set; }
        /// <summary>
        /// Gets or sets the HAVING clause.
        /// </summary>
        [JsonIgnore]
        public string Having { get; set; }
        /// <summary>
        /// Gets or sets the ORDER BY clause.
        /// </summary>
        [JsonIgnore]
        public string OrderBy { get; set; }

        /// <summary>
        /// Returns true if the statement is empty
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty { get { return string.IsNullOrWhiteSpace(Text); } }
        /// <summary>
        /// Gets or sets a value indicating whether this object
        /// <para>uses the CompanyFieldName when preparing the statement as a whole</para>
        /// </summary>
        public bool CompanyAware { get; set; }
 
        /// <summary>
        /// Gets or sets the database connection name
        /// </summary>
        public string ConnectionName
        {
            get { return string.IsNullOrWhiteSpace(fConnectionName) ? SysConfig.DefaultConnection : fConnectionName; }
            set { fConnectionName = value; }
        }

        /// <summary>
        /// Gets or sets the names of the check box columns (integer columns as boolean columns)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string CheckBoxColumns { get; set; }
        /// <summary>
        /// Gets or sets the names of the DateTime columns  
        /// as a list of strings separated by the character ;
        /// </summary>
        public string DateTimeColumns { get; set; }
        /// <summary>
        /// Gets or sets the names of the Date columns (date ONLY, not date and time)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string DateColumns { get; set; }
        /// <summary>
        /// Gets or sets the names of the Time columns (Time ONLY, not date and time)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string TimeColumns { get; set; }
        /// <summary>
        /// Gets or sets the names of the Image columns 
        /// as a list of strings separated by the character ;
        /// </summary>
        public string ImageColumns { get; set; }
        /// <summary>
        /// Gets or sets the names of the memo columns (text blob columns)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string MemoColumns { get; set; }

        /// <summary>
        /// A fully qualified (i.e. TABLE_NAME.FIELD_NAME) column of type date or datetime.
        /// <para>It works in conjuction with DateRange property in order to produce
        /// a fixed part in the WHERE clause of this select statement.</para>
        /// </summary>
        public string DateRangeColumn { get; set; }
        /// <summary>
        /// A DateRange.
        /// <para>It works in conjuction with DateRangeColumn property in order to produce
        /// a fixed part in the WHERE clause of this select statement.</para>
        /// </summary>
        public DateRange DateRange { get; set; }

        /* properties */
        /// <summary>
        /// A dictionary where Keys = FieldNames and Values = User Friendly Titles.
        /// </summary>
        public string DisplayLabels { get; set; }
        /// <summary>
        /// A collection of column settings elements
        /// </summary>
        public List<ColumnSetting> ColumnSettings { get; set; } = new List<ColumnSetting>();
        /// <summary>
        /// The criterion field descriptors used to generate the "user where" clause of the SelectSql
        /// </summary>
        public SqlFilterDefs SqlFilters { get; set; } = new SqlFilterDefs();



        /// <summary>
        /// True if DisplayLabels is not null or empty
        /// </summary>
        [JsonIgnore]
        public bool HasDisplayLabels { get { return !string.IsNullOrWhiteSpace(this.DisplayLabels); } }
        /// <summary>
        /// True if ColumnSettings is NOT null and contains items
        /// </summary>
        [JsonIgnore]
        public bool HasColumnSettings { get { return (ColumnSettings != null) && (ColumnSettings.Count > 0); } }
        /// <summary>
        /// True if CriterionDescriptors is NOT null and contains items
        /// </summary>
        [JsonIgnore]
        public bool HasSqlFilters { get { return (SqlFilters != null) && (SqlFilters.Count > 0); } }


        /// <summary>
        /// The DataTable that results after the select execution
        /// </summary>
        [JsonIgnore]
        public MemTable Table { get; set; }
        /// <summary>
        /// A user defined value.
        /// </summary>
        [JsonIgnore]
        public object Tag { get; set; }
    }
}
