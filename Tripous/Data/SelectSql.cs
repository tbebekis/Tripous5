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
        string fTitleKey;

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
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            if (string.IsNullOrWhiteSpace(this.Name) || string.IsNullOrWhiteSpace(this.Select) || string.IsNullOrWhiteSpace(this.From))
                Sys.Throw(Res.GS("E_SelectSql_NotFullyDefined", "SelectSql Name or SQL statement is empty"));

            if (Sys.IsNullOrWhiteSpace(this.Name) || Sys.IsNullOrWhiteSpace(this.Select) || Sys.IsNullOrWhiteSpace(this.From))
                Sys.Throw(Res.GS("E_SelectSql_NotFullyDefined", "SelectSql Name or SQL statement is empty"));

            this.Columns.ForEach((item) => { item.CheckDescriptor(); });
            this.Filters.ForEach((item) => { item.CheckDescriptor(); }); 
                
        }

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

            //CheckBoxColumns = string.Empty;
            //MemoColumns = string.Empty;
            //DateTimeColumns = string.Empty;
            //DateColumns = string.Empty;
            //TimeColumns = string.Empty;
            //ImageColumns = string.Empty; 

            DateRangeColumn = string.Empty;
            DateRange = Tripous.DateRange.Custom;

            //DisplayLabels = string.Empty;
            Columns.Clear();
            Filters.Clear();

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

                //CheckBoxColumns = Src.CheckBoxColumns;
                //MemoColumns = Src.MemoColumns;
                //DateTimeColumns = Src.DateTimeColumns;
                //DateColumns = Src.DateColumns;
                //TimeColumns = Src.TimeColumns;
                //ImageColumns = Src.ImageColumns;

                DateRangeColumn = Src.DateRangeColumn;
                DateRange = Src.DateRange;

                //DisplayLabels = Src.DisplayLabels;
                Columns = Src.Columns;
                Filters = Src.Filters;
 

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
        /// Sets-up the column types, the captions and the visibility of Table.Columns.
        /// </summary>
        public void SetupTable(DataTable Table)
        {
            // table name
            if (string.IsNullOrWhiteSpace(Table.TableName) || Table.TableName.StartsWithText("Table_"))
            {
                Table.TableName = (!string.IsNullOrWhiteSpace(this.Name) && !DefaultName.IsSameText(this.Name)) ?
                                    this.Name :
                                    MemTable.NextTableName();
            }


            if (Columns != null && Columns.Count > 0)
            {
                SelectSqlColumn SqlColumn;
                foreach (DataColumn TableColumn in Table.Columns)
                {
                    SqlColumn = Columns.Find(item => Sys.IsSameText(item.Name, TableColumn.ColumnName));
                    if (SqlColumn == null)
                    {
                        TableColumn.IsVisible(false);
                    }
                    else
                    {
                        TableColumn.IsVisible(SqlColumn.Visible);
                        TableColumn.Caption = SqlColumn.Title;

                        TableColumn.IsDateTime(SqlColumn.DisplayType == ColumnDisplayType.DateTime);
                        TableColumn.IsDate(SqlColumn.DisplayType == ColumnDisplayType.Date);
                        TableColumn.IsTime(SqlColumn.DisplayType == ColumnDisplayType.Time);
                        TableColumn.IsCheckBox(SqlColumn.DisplayType == ColumnDisplayType.CheckBox);
                        TableColumn.IsMemo(SqlColumn.DisplayType == ColumnDisplayType.Memo);
                        TableColumn.IsImage(SqlColumn.DisplayType == ColumnDisplayType.Image);
                    }
                }
            } 

        }

        /* properties */
        /// <summary>
        /// A name for this SELECT statement. Used when this goes into SELECT statement lists.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name; }
            set { }
        }

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
        /// A fully qualified (i.e. TABLE_NAME.FIELD_NAME) column of type date or datetime.
        /// <para>It works in conjuction with DateRange property in order to produce
        /// a fixed part in the WHERE clause of this select statement.</para>
        /// </summary>
        public string DateRangeColumn { get; set; } = "";
        /// <summary>
        /// A DateRange.
        /// <para>It works in conjuction with DateRangeColumn property in order to produce
        /// a fixed part in the WHERE clause of this select statement.</para>
        /// </summary>
        public DateRange DateRange { get; set; } = DateRange.LastWeek;

        /// <summary>
        /// The list of column descriptors of columns to display. If null or empty, then all columns are displayed. Else only the columns defined in this list are displayed.
        /// </summary>
        public List<SelectSqlColumn> Columns { get; set; } = new List<SelectSqlColumn>();
        /// <summary>
        /// The filter descriptors used to generate the "user where" clause. User's where is appended to the WHERE clause.
        /// </summary>
        public SqlFilterDefs Filters { get; set; } = new SqlFilterDefs();

        /// <summary>
        /// Adds a column in the list of columns and returns the newly added column.
        /// </summary>
        public SelectSqlColumn AddColumn(string Name, string TitleKey = "", ColumnDisplayType Type = ColumnDisplayType.Default)
        {
            SelectSqlColumn Result = Columns.Find(item => Sys.IsSameText(item.Name, Name));
            if (Result == null)
            {
                Result = new SelectSqlColumn()
                {
                    Name = Name,
                    TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : Name,
                    DisplayType = Type
                };

                Columns.Add(Result);
            }

            return Result;
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
