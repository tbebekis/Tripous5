using System;
using System.Text;

namespace bt.Data.SQLParser
{



    public enum SQLToken
    {
        Unknown,
        TableName,
        FieldName,
        Ascending,
        Descending,
        Select,
        From,
        Where,
        GroupBy,
        Having,
        Union,
        Plan,
        OrderBy,
        ForUpdate,
        End,
        Predicate,
        Value,
        IsNull,
        IsNotNull,
        Like,
        And,
        Or,
        Number,
        AllFields,
        Comment,
        Distinct

    }


    /// <summary>
    /// Summary description for so_SQLParser.
    /// </summary>
    public class Tokenizer
    {
        private string SQL = "";
        private int P = 0;
        private string Token = "";
        private SQLToken CurSection = SQLToken.Unknown;
        private bool DotStart = false;
        private int TokenStart = 0;

        private Tokenizer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="S"></param>
        /// <param name="QuoteChar"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static string DequotedStr(string S, char QuoteChar, ref int P)
        {
            StringBuilder SB = new StringBuilder(S);

            if ((SB.Length == 0) || (SB[P] != QuoteChar))
                return "";

            int L = SB.Length;
            bool EndQuoteFound = false;
            StringBuilder Result = new StringBuilder();
            P++;                         // skip the first quotechar;
            int Start = P;

            while ((P <= L) && (!EndQuoteFound))
            {
                if (SB[P] == QuoteChar)
                {
                    if ((P == L) || ((P < L - 1) && (S[P + 1] != QuoteChar)))  // end of line or of quote
                    {
                        Result.Append(S.Substring(Start, P - Start));
                        EndQuoteFound = true;
                    }
                    else  // quoted quotechar
                    {
                        P++; //skip the "quoting char"
                        Result.Append(S.Substring(Start, P - Start));
                        Start = P + 1;
                    }
                }
                P++;
            }

            return Result.ToString();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="P"></param>
        /// <param name="Token"></param>
        /// <param name="CurSection"></param>
        /// <returns></returns>
        public static SQLToken NextToken(string SQL, ref int P, ref string Token, SQLToken CurSection)
        {
            SQLToken Res = SQLToken.Unknown;

            Tokenizer T = new Tokenizer();
            T.SQL = SQL;
            T.P = P;
            T.Token = Token;
            T.CurSection = CurSection;

            Res = T.NextToken();
            Token = T.Token;
            P = T.P;

            return Res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Str"></param>
        /// <returns></returns>
        private bool NextTokenIs(string Value, ref string Str)
        {
            bool Res = false;
            int Tmp = P;
            string S = "";

            Tokenizer.NextToken(SQL, ref Tmp, ref S, CurSection);
            Res = string.Compare(Value, S, true) == 0;

            if (Res)
            {
                Str = Str + " " + S;
                P = Tmp;
            }

            return Res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        private SQLToken GetSQLToken(ref string Str)
        {
            SQLToken Res = SQLToken.Unknown;
            int L = 0;
            string S = "";

            if (Str.Length == 0)
                Res = SQLToken.End;
            else if ((Str == "*") && (CurSection == SQLToken.Select))
                Res = SQLToken.AllFields;
            else if (DotStart)
                Res = SQLToken.FieldName;
            else if ((string.Compare("DISTINT", Str, true) == 0) && (CurSection == SQLToken.Select))
                Res = SQLToken.Distinct;
            else if ((string.Compare("ASC", Str, true) == 0) || (string.Compare("ASCENDING", Str, true) == 0))
                Res = SQLToken.Ascending;
            else if ((string.Compare("DESC", Str, true) == 0) || (string.Compare("DESCENDING", Str, true) == 0))
                Res = SQLToken.Descending;
            else if (string.Compare("SELECT", Str, true) == 0)
                Res = SQLToken.Select;
            else if (string.Compare("AND", Str, true) == 0)
                Res = SQLToken.And;
            else if (string.Compare("OR", Str, true) == 0)
                Res = SQLToken.Or;
            else if (string.Compare("LIKE", Str, true) == 0)
                Res = SQLToken.Like;
            else if (string.Compare("IS", Str, true) == 0)
            {
                if (NextTokenIs("NULL", ref Str))
                    Res = SQLToken.IsNull;
                else
                {
                    L = P;
                    S = Str;
                    if (NextTokenIs("NOT", ref Str) && NextTokenIs("NULL", ref Str))
                        Res = SQLToken.IsNotNull;
                    else
                    {
                        P = L;
                        Str = S;
                        Res = SQLToken.Value;
                    }
                }
            }
            else if (string.Compare("FROM", Str, true) == 0)
                Res = SQLToken.From;
            else if (string.Compare("WHERE", Str, true) == 0)
                Res = SQLToken.Where;
            else if ((string.Compare("GROUP", Str, true) == 0) && NextTokenIs("BY", ref Str))
                Res = SQLToken.GroupBy;
            else if (string.Compare("HAVING", Str, true) == 0)
                Res = SQLToken.Having;
            else if (string.Compare("UNION", Str, true) == 0)
                Res = SQLToken.Union;
            else if (string.Compare("PLAN", Str, true) == 0)
                Res = SQLToken.Plan;
            else if ((string.Compare("FOR", Str, true) == 0) && NextTokenIs("UPDATE", ref Str))
                Res = SQLToken.ForUpdate;
            else if ((string.Compare("ORDER", Str, true) == 0) && NextTokenIs("BY", ref Str))
                Res = SQLToken.OrderBy;
            else if (string.Compare("NULL", Str, true) == 0)
                Res = SQLToken.Value;
            else if (CurSection == SQLToken.From)
                Res = SQLToken.TableName;
            else Res = SQLToken.FieldName;


            return Res;

        }
        /// <summary>
        /// 
        /// </summary>
        private void StartToken()
        {
            if (TokenStart < 0)   //    =
                TokenStart = P;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SQLToken NextToken()
        {

            SQLToken Res = SQLToken.Unknown;
            char Literal;
            int Mark;
            int EndPos;

            TokenStart = -1;
            DotStart = false;
            EndPos = SQL.Length;

            while (P < EndPos)
            {
                switch (SQL[P])
                {
                    case '\"':
                    case '\'':
                    case '`':
                        StartToken();
                        Literal = SQL[P];
                        Mark = P;

                        do
                        {
                            P++;
                            if (P > EndPos)
                                break;
                        }
                        while (SQL[P] != Literal);

                        if (P > EndPos)
                        {
                            P = Mark;
                            P++;
                        }
                        else
                        {
                            P++;
                            int Temp = 0;
                            Token = DequotedStr(SQL.Substring(TokenStart, P - TokenStart), Literal, ref Temp);
                            if (DotStart)
                                Res = SQLToken.FieldName;
                            else if ((P <= EndPos) && (SQL[P] == '.'))
                                Res = SQLToken.TableName;
                            else Res = SQLToken.Value;

                            return Res;
                        }
                        break;
                    case '/':
                        StartToken();
                        P++;
                        if ((P <= EndPos) && ((SQL[P] == '/') || (SQL[P] == '*')))
                        {
                            if (SQL[P] == '*')
                            {
                                do
                                    P++;
                                while ((P <= EndPos) ||
                                      ((SQL[P] != '*') && (P >= EndPos) && (SQL[P + 1] != '/')));
                            }
                            else
                            {
                                while ((P <= EndPos) && !((SQL[P] == '\r') || (SQL[P] == '\n')))
                                    P++;
                            }

                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = SQLToken.Comment;
                            return Res;
                        }
                        break;
                    case ' ':
                    case '\r':
                    case '\n':
                    case ',':
                    case '(':
                        if (TokenStart >= 0)
                        {
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = GetSQLToken(ref Token);
                            return Res;
                        }
                        else
                        {
                            while ((P <= EndPos) && (" \r\n,(").IndexOf(SQL[P]) != -1)
                                P++;
                        }
                        break;

                    case '.':
                        if (TokenStart > 0)
                        {
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = SQLToken.TableName;
                            return Res;
                        }
                        else
                        {
                            DotStart = true;
                            P++;
                        }
                        break;
                    case '=':
                    case '<':
                    case '>':
                        if (TokenStart < 1)
                        {
                            TokenStart = P;
                            while ((P <= EndPos) && ("=<>").IndexOf(SQL[P]) != -1)
                                P++;

                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = SQLToken.Predicate;
                            return Res;
                        }
                        P++;
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        if (TokenStart < 1)
                        {
                            TokenStart = P;
                            while ((P <= EndPos) && (char.IsDigit(SQL[P]) || (SQL[P] == '.')))
                                P++;
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = SQLToken.Number;
                            return Res;
                        }
                        else
                            P++;
                        break;
                    default:
                        StartToken();
                        P++;
                        break;
                }
            }

            //{ we reached the end of the SQL string (p = EndPos + 1)}
            if (TokenStart >= 0)
            {
                Token = SQL.Substring(TokenStart, P - TokenStart);
                Res = GetSQLToken(ref Token);
            }
            else
            {
                Res = SQLToken.End;
                Token = "";
            }

            return Res;
        }
    }
}
