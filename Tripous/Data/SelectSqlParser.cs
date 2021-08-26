/*--------------------------------------------------------------------------------------        
                            Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Text;
using System.Globalization;

namespace Tripous.Data
{


    /// <summary>
    /// A simple SELECT Sql parser. It parses a SELECT statement into its constituents parts.
    /// The SELECT statement can contain nested sub-SELECTs or column names in double quotes or angle brackets.
    /// </summary>
    public class SelectSqlParser
    {
        enum Token
        {
            None = 0,
            Select = 1,
            From = 2,
            Where = 3,
            GroupBy = 4,
            Having = 5,
            OrderBy = 6,
        }

        readonly string[] tokens = { "select", "from", "where", "group by", "having", "order by" };

        string text;

        string fSelect;
        string fFrom;
        string fWhere;
        string fGroupBy;
        string fHaving;
        string fOrderBy;

        Token curToken = Token.None;
        int curPos = 0;
        int lastPos = -1;


        /// <summary>
        /// Skips characters by increasing the current position (curPos)
        /// until the SkipChar is found.
        /// </summary>
        int SkipToChar(char SkipChar)
        {
            char C;
            curPos++;

            while (curPos <= text.Length - 1)
            {
                C = text[curPos];
                if (C == SkipChar)
                {
                    curPos++;
                    break;
                }

                curPos++;
            }

            return curPos;
        }
        /// <summary>
        /// Returns true if sToken is at the current position (curPos) in text
        /// </summary>
        bool FindTokenAtCurrentPos(string sToken)
        {
            if (curPos + sToken.Length <= text.Length - 1)
                try
                {
                    return string.Compare(text, curPos, sToken, 0, sToken.Length, true, CultureInfo.InvariantCulture) == 0;
                }
                catch
                {
                }

            return false;

        }
        /// <summary>
        /// Performs a "token change". Makes the NewToken the curToken,
        /// copies a part of the text string setting the last clause string
        /// and adjusts curPos, lastPos and curToken.
        /// </summary>
        void TokenChange(Token NewToken, string sNewToken)
        {
            switch (curToken)
            {
                case Token.Select:
                    Select = text.Substring(lastPos, curPos - lastPos);
                    break;

                case Token.From:
                    From = text.Substring(lastPos, curPos - lastPos);
                    break;

                case Token.Where:
                    Where = text.Substring(lastPos, curPos - lastPos);
                    break;

                case Token.GroupBy:
                    GroupBy = text.Substring(lastPos, curPos - lastPos);
                    break;

                case Token.Having:
                    Having = text.Substring(lastPos, curPos - lastPos);
                    break;

                case Token.OrderBy:
                    OrderBy = text.Substring(lastPos, curPos - lastPos);
                    break;

            }
            curPos += sNewToken.Length;
            lastPos = curPos;
            curToken = NewToken;

        }
        /// <summary>
        /// The actual parsing procedure. Returns true only when finds
        /// one of the token strings (SELECT, FROM, WHERE etc).
        /// </summary>
        bool NextTokenEnd()
        {
            bool Result = false;
            int ParenCount = 0;
            char C;


            while (curPos <= text.Length - 1)
            {
                C = text[curPos];

                if (C == '"')
                    curPos = SkipToChar('"');
                else if (C == '[')
                    curPos = SkipToChar(']');
                else if (C == '(')
                {
                    curPos++;
                    ParenCount++;
                }
                else if (C == ')')
                {
                    curPos++;
                    ParenCount--;
                    if (ParenCount < 0)
                        throw new ApplicationException("SelectSqlParser: Wrong parentheses");
                }
                else if (!char.IsWhiteSpace(text[curPos]) && ((curPos - 1) >= 0) && char.IsWhiteSpace(text[curPos - 1]))
                {
                    if (ParenCount == 0)
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            if (FindTokenAtCurrentPos(tokens[i]))
                            {
                                TokenChange((Token)i + 1, tokens[i]);
                                return true;
                            }
                        }
                    }

                    curPos++;
                }
                else
                {
                    curPos++;
                }
            }


            return Result;
        }
        /// <summary>
        /// Parses the passed in text
        /// </summary>
        void Parse()
        {
            //text = text.Replace("  ", " ");

            curToken = Token.None;
            curPos = 0;
            lastPos = -1;

            text = " " + text;

            while (curToken <= Token.OrderBy)
                if (!NextTokenEnd())
                    break;

            TokenChange(Token.None, "");
        }

        /* construction */
        /// <summary>
        /// Constructor. Text can not be null or empty.
        /// </summary>
        public SelectSqlParser(string Text)
        {
            this.Parse(Text);
        }

        /* static */
        /// <summary>
        /// Parses Text into the constituent parts of a SELECT S statement.
        /// <para>Example call: SelectSqlParser.Parse("select * from CUSTOMER").ToString();</para>
        /// </summary>
        static public SelectSqlParser Execute(string Text)
        {
            return new SelectSqlParser(Text);
        }

        /* public */
        /// <summary>
        /// Parses Text into the constituent parts of a SELECT S statement. 
        /// </summary>
        public void Parse(string Text)
        {
            Clear();

            if (!string.IsNullOrWhiteSpace(Text))
            {
                text = Text;
                Parse();
            }
        }
        /// <summary>
        /// Clears the resulting clauses
        /// </summary>
        public void Clear()
        {
            Select = string.Empty;
            From = string.Empty;
            Where = string.Empty;
            GroupBy = string.Empty;
            Having = string.Empty;
            OrderBy = string.Empty;
        }
        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();

            SB.AppendLine(string.Format("select : {0}", Select));
            SB.AppendLine(string.Format("from : {0}", From));
            SB.AppendLine(string.Format("where : {0}", Where));
            SB.AppendLine(string.Format("group by : {0}", GroupBy));
            SB.AppendLine(string.Format("having : {0}", Having));
            SB.AppendLine(string.Format("order by : {0}", OrderBy));

            return SB.ToString();
        }

        /* properties */
        /// <summary>
        /// Returns the SELECT clause of the parsed statement
        /// </summary>
        public string Select
        {
            get { return !string.IsNullOrWhiteSpace(fSelect) ? fSelect : string.Empty; }
            set { fSelect = value; }
        }
        /// <summary>
        /// Returns the FROM clause of the parsed statement
        /// </summary>
        public string From
        {
            get { return !string.IsNullOrWhiteSpace(fFrom) ? fFrom : string.Empty; }
            set { fFrom = value; }
        }
        /// <summary>
        /// Returns the WHERE clause of the parsed statement
        /// </summary>
        public string Where
        {
            get { return !string.IsNullOrWhiteSpace(fWhere) ? fWhere : string.Empty; }
            set { fWhere = value; }
        }
        /// <summary>
        /// Returns the GROUP BY clause of the parsed statement
        /// </summary>
        public string GroupBy
        {
            get { return !string.IsNullOrWhiteSpace(fGroupBy) ? fGroupBy : string.Empty; }
            set { fGroupBy = value; }
        }
        /// <summary>
        /// Returns the HAVING clause of the parsed statement
        /// </summary>
        public string Having
        {
            get { return !string.IsNullOrWhiteSpace(fHaving) ? fHaving : string.Empty; }
            set { fHaving = value; }
        }
        /// <summary>
        /// Returns the ORDER BY clause of the parsed statement
        /// </summary>
        public string OrderBy
        {
            get { return !string.IsNullOrWhiteSpace(fOrderBy) ? fOrderBy : string.Empty; }
            set { fOrderBy = value; }
        }

    }
}
