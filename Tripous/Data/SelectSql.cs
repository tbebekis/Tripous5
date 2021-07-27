﻿/*--------------------------------------------------------------------------------------        
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
    public class SelectSql : NamedItem
    {
        /* private */
        string fSelect;
        string fFrom;
        string fWhere;
        string fWhereUser;
        string fGroupBy;
        string fHaving;
        string fOrderBy;

        bool fCompanyAware;
        string fConnectionName;

        string fCheckBoxColumns;
        string fMemoColumns;
        string fDateTimeColumns;
        string fDateColumns;
        string fTimeColumns;
        string fImageColumns;

        string fDateRangeColumn;
        DateRange fDateRange = DateRange.Custom;

        string fZoomCommand;

        string fDisplayLabels;
        ColumnSettings fColumnSettings;
        SqlFilters fSqlFilters;

        /// <summary>
        /// Field
        /// </summary>
        protected string fTitle;
        /// <summary>
        /// Field
        /// </summary>
        protected string fTitleKey;


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

        /* overrides */
        /// <summary>
        /// Clears the content of the clause properties
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();

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

            ZoomCommand = string.Empty;
            AlwaysVisibleView = false;
            ModalView = false;

            DateRangeColumn = string.Empty;
            DateRange = Tripous.DateRange.Custom;

            DisplayLabels = string.Empty;
            fColumnSettings = null;
            fSqlFilters = null;

            IsAccessible = false;

        }
        /// <summary>
        /// Assigns this properties from Source.
        /// <para>WARNING: We need this because an automated Assign() calls the Text property
        /// which in turn calls the SelectSqlParser which has no ability to handle fields 
        /// surrounded by double quotes or [] or something.</para>
        /// </summary>
        protected override void DoAssign(object Source)
        {
            if (Source is SelectSql)
            {
                SelectSql Src = Source as SelectSql;

                this.Name = Src.Name;
                this.fTitle = Src.fTitle;
                this.fTitleKey = Src.fTitleKey;

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

                ZoomCommand = Src.ZoomCommand;
                AlwaysVisibleView = Src.AlwaysVisibleView;
                ModalView = Src.ModalView;

                DateRangeColumn = Src.DateRangeColumn;
                DateRange = Src.DateRange;

                DisplayLabels = Src.DisplayLabels;

                if (Src.fColumnSettings == null)
                    this.fColumnSettings = null;
                else
                    this.ColumnSettings.Assign(Src.ColumnSettings);

                if (Src.fSqlFilters == null)
                    this.fSqlFilters = null;
                else
                    this.SqlFilters.Assign(Src.SqlFilters);

            }
            else
            {
                DoClear();
            }
        }


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
            if (string.IsNullOrWhiteSpace(S))
                return;

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
        /// <summary>
        /// Concatenates Clause + Delimiter + Plus 
        /// </summary>
        static public string AddTo(string Clause, string Delimiter, string Plus)
        {
            if (Clause.Trim() == string.Empty)
                return SPACES + Plus.Trim();

            Clause = Clause.TrimEnd();
            Delimiter = Delimiter.Trim();
            Plus = Plus.Trim();

            if (Clause.EndsWith(Delimiter, StringComparison.InvariantCultureIgnoreCase)
                || Plus.StartsWith(Delimiter, StringComparison.InvariantCultureIgnoreCase))
                return CR(Clause) + SPACES + Plus.Trim();
            else
                return CR(Clause) + SPACES + Delimiter + SPACE + Plus;
        }
        /// <summary>
        /// Concatenates Keyword + Clause
        /// </summary>
        static public string NormalizeClause(string Clause, string Keyword)
        {
            Clause = Clause.Trim();
            Keyword = Keyword.Trim();

            if (!string.IsNullOrEmpty(Clause) && !string.IsNullOrEmpty(Keyword))
            {
                if (!Clause.StartsWith(Keyword, StringComparison.InvariantCultureIgnoreCase))
                    return CR(Keyword) + SPACES + Clause;
            }

            return Clause;
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
        /// Concatenates Keyword + Clause for all clauses
        /// </summary>
        public string GetSqlText()
        {
            // select
            string sSelect = NormalizeClause(Select, "select") + SPACES;

            // from
            string sFrom = NormalizeClause(From, "from") + SPACES;

            // where
            string sWhere = string.IsNullOrEmpty(Where) ? string.Empty : Where.Trim();

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

            if (WhereUser.Trim().Length > 0)
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


            if (!string.IsNullOrEmpty(StatementText))
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
                    Setting = ColumnSettings.Find(Column.ColumnName);
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
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        public string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(fTitle))
                    return fTitle;

                if (!string.IsNullOrWhiteSpace(TitleKey))
                    return Res.GS(TitleKey, TitleKey);

                return Name;
            }
            set
            {
                fTitle = value;
                OnPropertyChanged("Title");
            }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return string.IsNullOrWhiteSpace(fTitleKey) ? Name : fTitleKey; }
            set
            {
                fTitleKey = value;
                OnPropertyChanged("TitleKey");
            }

        }

        /// <summary>
        /// Gets the statement as a whole. 
        /// <para>Sets clause properties by parsing the passed string value.</para>
        /// </summary>
        public string Text
        {
            get { return string.IsNullOrWhiteSpace(fSelect) ? string.Empty : GetSqlText(); }
            set
            {
                Parse(value);
                OnPropertyChanged("Text");
            }
        }
        /// <summary>
        /// Gets or sets the SELECT clause.
        /// </summary>
        [JsonIgnore]
        public string Select
        {
            get { return string.IsNullOrEmpty(fSelect) ? string.Empty : fSelect; }
            set { fSelect = value; }
        }
        /// <summary>
        /// Gets or sets the FROM clause.
        /// </summary>
        [JsonIgnore]
        public string From
        {
            get { return string.IsNullOrEmpty(fFrom) ? string.Empty : fFrom; }
            set { fFrom = value; }
        }
        /// <summary>
        /// Gets or sets the WHERE clause.
        /// </summary>
        [JsonIgnore]
        public string Where
        {
            get { return string.IsNullOrEmpty(fWhere) ? string.Empty : fWhere; }
            set { fWhere = value; }
        }
        /// <summary>
        /// Gets or sets the WHERE clause, that part that is considered user where.
        /// </summary>
        [JsonIgnore]
        public string WhereUser
        {
            get { return string.IsNullOrEmpty(fWhereUser) ? string.Empty : fWhereUser; }
            set { fWhereUser = value; }
        }
        /// <summary>
        /// Gets or sets the GROUP BY clause.
        /// </summary>
        [JsonIgnore]
        public string GroupBy
        {
            get { return string.IsNullOrEmpty(fGroupBy) ? string.Empty : fGroupBy; }
            set { fGroupBy = value; }
        }
        /// <summary>
        /// Gets or sets the HAVING clause.
        /// </summary>
        [JsonIgnore]
        public string Having
        {
            get { return string.IsNullOrEmpty(fHaving) ? string.Empty : fHaving; }
            set { fHaving = value; }
        }
        /// <summary>
        /// Gets or sets the ORDER BY clause.
        /// </summary>
        [JsonIgnore]
        public string OrderBy
        {
            get { return string.IsNullOrEmpty(fOrderBy) ? string.Empty : fOrderBy; }
            set { fOrderBy = value; }
        }

        /// <summary>
        /// Returns true if the statement is empty
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty { get { return string.IsNullOrWhiteSpace(Text); } }
        /// <summary>
        /// Gets or sets a value indicating whether this object
        /// <para>uses the CompanyFieldName when preparing the statement as a whole</para>
        /// </summary>
        public bool CompanyAware
        {
            get { return fCompanyAware; }
            set
            {
                fCompanyAware = value;
                OnPropertyChanged("CompanyAware");
            }
        }
        /// <summary>
        /// Gets or sets the database connection name
        /// </summary>
        public string ConnectionName
        {
            get { return string.IsNullOrWhiteSpace(fConnectionName) ? SysConfig.DefaultConnection : fConnectionName; }
            set
            {
                fConnectionName = value;
                OnPropertyChanged("ConnectionName");
            }
        }

        /// <summary>
        /// Gets or sets the names of the check box columns (integer columns as boolean columns)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string CheckBoxColumns
        {
            get { return string.IsNullOrWhiteSpace(fCheckBoxColumns) ? string.Empty : fCheckBoxColumns; }
            set
            {
                fCheckBoxColumns = value;
                OnPropertyChanged("CheckBoxColumns");
            }
        }
        /// <summary>
        /// Gets or sets the names of the DateTime columns  
        /// as a list of strings separated by the character ;
        /// </summary>
        public string DateTimeColumns
        {
            get { return string.IsNullOrWhiteSpace(fDateTimeColumns) ? string.Empty : fDateTimeColumns; }
            set
            {
                fDateTimeColumns = value;
                OnPropertyChanged("DateTimeColumns");
            }
        }
        /// <summary>
        /// Gets or sets the names of the Date columns (date ONLY, not date and time)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string DateColumns
        {
            get { return string.IsNullOrWhiteSpace(fDateColumns) ? string.Empty : fDateColumns; }
            set
            {
                fDateColumns = value;
                OnPropertyChanged("DateColumns");
            }
        }
        /// <summary>
        /// Gets or sets the names of the Time columns (Time ONLY, not date and time)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string TimeColumns
        {
            get { return string.IsNullOrWhiteSpace(fTimeColumns) ? string.Empty : fTimeColumns; }
            set
            {
                fTimeColumns = value;
                OnPropertyChanged("TimeColumns");
            }
        }
        /// <summary>
        /// Gets or sets the names of the Image columns 
        /// as a list of strings separated by the character ;
        /// </summary>
        public string ImageColumns
        {
            get { return string.IsNullOrWhiteSpace(fImageColumns) ? string.Empty : fImageColumns; }
            set
            {
                fImageColumns = value;
                OnPropertyChanged("ImageColumns");
            }
        }
        /// <summary>
        /// Gets or sets the names of the memo columns (text blob columns)
        /// as a list of strings separated by the character ;
        /// </summary>
        public string MemoColumns
        {
            get { return string.IsNullOrWhiteSpace(fMemoColumns) ? string.Empty : fMemoColumns; }
            set
            {
                fMemoColumns = value;
                OnPropertyChanged("MemoColumns");
            }
        }

        /// <summary>
        /// A fully qualified (i.e. TABLE_NAME.FIELD_NAME) column of type date or datetime.
        /// <para>It works in conjuction with DateRange property in order to produce
        /// a fixed part in the WHERE clause of this select statement.</para>
        /// </summary>
        public string DateRangeColumn
        {
            get { return string.IsNullOrWhiteSpace(fDateRangeColumn) ? string.Empty : fDateRangeColumn; }
            set
            {
                fDateRangeColumn = value;
                OnPropertyChanged("DateRangeColumn");
            }
        }
        /// <summary>
        /// A DateRange.
        /// <para>It works in conjuction with DateRangeColumn property in order to produce
        /// a fixed part in the WHERE clause of this select statement.</para>
        /// </summary>
        public DateRange DateRange
        {
            get { return fDateRange; }
            set
            {
                fDateRange = value;
                OnPropertyChanged("DateRange");
            }
        }

        /* properties */
        /// <summary>
        /// When true then the statement ui is always visible,
        /// else it is visible only on demand.
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool AlwaysVisibleView { get; set; }
        /// <summary>
        /// When true then the statement ui is shown in a modal form.
        /// <para>NOTE: Not applicable when AlwaysVisible is true.</para>
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool ModalView { get; set; }
        /// <summary>
        /// Gets or sets the zoom command name. A user inteface (form) can use this name to call the command.
        /// </summary>
        public string ZoomCommand
        {
            get { return string.IsNullOrEmpty(fZoomCommand) ? string.Empty : fZoomCommand; }
            set
            {
                fZoomCommand = value;
                OnPropertyChanged("ZoomCommand");
            }
        }



        /// <summary>
        /// A dictionary where Keys = FieldNames and Values = User Friendly Titles.
        /// </summary>
        public string DisplayLabels
        {
            get { return string.IsNullOrWhiteSpace(fDisplayLabels) ? string.Empty : fDisplayLabels; }
            set
            {
                fDisplayLabels = value;
                OnPropertyChanged("DisplayLabels");
            }
        }
        /// <summary>
        /// A collection of column settings elements
        /// </summary>
        public ColumnSettings ColumnSettings
        {
            get
            {
                if (fColumnSettings == null)
                    fColumnSettings = new ColumnSettings();
                return fColumnSettings;
            }
        }
        /// <summary>
        /// The criterion field descriptors used to generate the "user where" clause of the SelectSql
        /// </summary>
        public SqlFilters SqlFilters
        {
            get
            {
                if (fSqlFilters == null)
                    fSqlFilters = new SqlFilters();
                return fSqlFilters;
            }
        }


        /// <summary>
        /// True if DisplayLabels is not null or empty
        /// </summary>
        [JsonIgnore]
        public bool HasDisplayLabels { get { return !string.IsNullOrWhiteSpace(this.DisplayLabels); } }
        /// <summary>
        /// True if ColumnSettings is NOT null and contains items
        /// </summary>
        [JsonIgnore]
        public bool HasColumnSettings { get { return (fColumnSettings != null) && (fColumnSettings.Count > 0); } }
        /// <summary>
        /// True if CriterionDescriptors is NOT null and contains items
        /// </summary>
        [JsonIgnore]
        public bool HasSqlFilters { get { return (fSqlFilters != null) && (fSqlFilters.Count > 0); } }

        /// <summary>
        /// When true then the current user has the right to execute that select
        /// </summary>
        [JsonIgnore]
        public bool IsAccessible { get; set; }
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
