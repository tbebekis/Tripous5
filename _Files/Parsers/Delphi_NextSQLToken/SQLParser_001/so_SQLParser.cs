using System;
using System.Text;

namespace bt.Data.SQLParser
{



    public enum TokenKind
    {
        Unknown,
        TableName,
        FieldName,
        Ascending,
        Descending,
        Select,
        Insert,
        Update,
        Delete,
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
        Set,
        Like,
        And,
        Or,
        Number,
        AllFields,
        Comment,
        Distinct,
        In,
        Between,
        LParen,
        RParen,
        LineFeed

    }


    /// <summary>
    /// Summary description for so_SQLParser.
    /// </summary>
    public class Tokenizer
    {
        private string SQL = "";
        private int P = 0;
        private string Token = "";
        private TokenKind CurSection = TokenKind.Unknown;
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

            while ((P < L) && (!EndQuoteFound))
            {
                if (SB[P] == QuoteChar)
                {
                    if ((P == L) || (S[P] != QuoteChar))  // end of line or of quote
                    {
                        Result.Append(S.Substring(Start, P - Start));
                        EndQuoteFound = true;
                    }
                    else  // quoted quotechar
                    {
                        P++; //skip the "quoting char"
                        Result.Append(S.Substring(Start, P - Start));
                        Start = P;
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
        public static TokenKind NextToken(string SQL, ref int P, ref string Token, TokenKind CurSection)
        {
            TokenKind Res = TokenKind.Unknown;

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
        private TokenKind GetSQLToken(ref string Str)
        {
            TokenKind Res = TokenKind.Unknown;
            int L = 0;
            string S = "";

            if (Str.Length == 0)
                Res = TokenKind.End;
            else if ((Str == "*") && (CurSection == TokenKind.Select))
                Res = TokenKind.AllFields;
            else if (DotStart)
                Res = TokenKind.FieldName;
            else if ((string.Compare("DISTINT", Str, true) == 0) && (CurSection == TokenKind.Select))
                Res = TokenKind.Distinct;
            else if ((string.Compare("ASC", Str, true) == 0) || (string.Compare("ASCENDING", Str, true) == 0))
                Res = TokenKind.Ascending;
            else if ((string.Compare("DESC", Str, true) == 0) || (string.Compare("DESCENDING", Str, true) == 0))
                Res = TokenKind.Descending;
            else if (string.Compare("SELECT", Str, true) == 0)
                Res = TokenKind.Select;
            else if (string.Compare("INSERT", Str, true) == 0)
                Res = TokenKind.Insert;
            else if (string.Compare("UPDATE", Str, true) == 0)
                Res = TokenKind.Update;
            else if (string.Compare("DELETE", Str, true) == 0)
                Res = TokenKind.Delete;
            else if (string.Compare("AND", Str, true) == 0)
                Res = TokenKind.And;
            else if (string.Compare("OR", Str, true) == 0)
                Res = TokenKind.Or;
            else if (string.Compare("LIKE", Str, true) == 0)
                Res = TokenKind.Like;
            else if (string.Compare("IS", Str, true) == 0)
            {
                if (NextTokenIs("NULL", ref Str))
                    Res = TokenKind.IsNull;
                else
                {
                    L = P;
                    S = Str;
                    if (NextTokenIs("NOT", ref Str) && NextTokenIs("NULL", ref Str))
                        Res = TokenKind.IsNotNull;
                    else
                    {
                        P = L;
                        Str = S;
                        Res = TokenKind.Value;
                    }
                }
            }
            else if (string.Compare("SET", Str, true) == 0)
                Res = TokenKind.Set;
            else if (string.Compare("FROM", Str, true) == 0)
                Res = TokenKind.From;
            else if (string.Compare("WHERE", Str, true) == 0)
                Res = TokenKind.Where;
            else if ((string.Compare("GROUP", Str, true) == 0) && NextTokenIs("BY", ref Str))
                Res = TokenKind.GroupBy;
            else if (string.Compare("HAVING", Str, true) == 0)
                Res = TokenKind.Having;
            else if (string.Compare("UNION", Str, true) == 0)
                Res = TokenKind.Union;
            else if (string.Compare("PLAN", Str, true) == 0)
                Res = TokenKind.Plan;
            else if ((string.Compare("FOR", Str, true) == 0) && NextTokenIs("UPDATE", ref Str))
                Res = TokenKind.ForUpdate;
            else if ((string.Compare("ORDER", Str, true) == 0) && NextTokenIs("BY", ref Str))
                Res = TokenKind.OrderBy;
            else if (string.Compare("NULL", Str, true) == 0)
                Res = TokenKind.Value;
            else if (string.Compare("IN", Str, true) == 0)
                Res = TokenKind.In;
            else if (string.Compare("BETWEEN", Str, true) == 0)
                Res = TokenKind.Between;
            else if (CurSection == TokenKind.From)
                Res = TokenKind.TableName;
            else Res = TokenKind.FieldName;


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
        private TokenKind NextToken()
        {

            TokenKind Res = TokenKind.Unknown;
            char Literal;
            int Mark;
            int EndPos;
            char C;

            TokenStart = -1;
            DotStart = false;
            EndPos = SQL.Length;

            while (P < EndPos)
            {
                C = SQL[P];
                switch (C)
                {
                    case '\"':
                    case '\'':
                    case '`':
                        StartToken();
                        Literal = SQL[P];
                        Mark = P;

                        //repeat Inc(p) until (p > EndPos) or (SQL[p] = Literal);
                        P++;
                        while (P < EndPos)
                        {
                            if (SQL[P] == Literal)
                                break;
                            else P++;
                        }
                        //while ((P < EndPos) && (SQL[P] != Literal))
                        //  P++;

                        if (P > EndPos)
                        {
                            P = Mark;
                            P++;
                        }
                        else
                        {
                            P++;

                            //string SS = SQL.Substring(TokenStart, P - TokenStart);
                            //Token = DequotedStr(SS, Literal, ref Temp);
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            if (DotStart)
                                Res = TokenKind.FieldName;
                            else if ((P < EndPos) && (SQL[P] == '.'))
                                Res = TokenKind.TableName;
                            else Res = TokenKind.Value;

                            return Res;
                        }
                        break;  //---------------------- 
                    case '/':
                        StartToken();
                        P++;
                        if ((P < EndPos) && ((SQL[P] == '/') || (SQL[P] == '*')))
                        {
                            if (SQL[P] == '*')
                            {
                                do
                                {
                                    P++;

                                    if (SQL[P] == '*')
                                        if (P < EndPos - 1)
                                            if (SQL[P + 1] == '/')
                                            {
                                                P++;
                                                P++;
                                                break;
                                            }

                                }
                                while (P < EndPos);
                            }
                            else
                            {
                                while ((P < EndPos) && !((SQL[P] == '\r') || (SQL[P] == '\n')))
                                    P++;
                            }

                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = TokenKind.Comment;
                            return Res;
                        }
                        break;  //---------------------- 
                    case ' ':
                    case '\r':
                    case '\n':
                    case ',':
                        //case '(':
                        if (TokenStart >= 0)
                        {
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = GetSQLToken(ref Token);
                            return Res;
                        }
                        else
                        {
                            //while ((P < EndPos) && (" \r\n").IndexOf(SQL[P]) != -1)
                            while ((P < EndPos) && (", \r\n").IndexOf(SQL[P]) != -1)
                                P++;
                        }
                        break;   //----------------------                
                    case '(':
                        P++;
                        Token = "(";
                        Res = TokenKind.LParen;
                        return Res;
                    //break;   //----------------------                   
                    case ')':
                        P++;
                        Token = ")";
                        Res = TokenKind.RParen;
                        return Res;
                    //break;  //----------------------                
                    case '.':
                        if (TokenStart > 0)
                        {
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = TokenKind.TableName;
                            return Res;
                        }
                        else
                        {
                            DotStart = true;
                            P++;
                        }
                        break;  //---------------------- 
                    case '=':
                    case '<':
                    case '>':
                        if (TokenStart < 1)
                        {
                            TokenStart = P;
                            while ((P < EndPos) && ("=<>").IndexOf(SQL[P]) != -1)
                                P++;

                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = TokenKind.Predicate;
                            return Res;
                        }
                        P++;
                        break;  //---------------------- 
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
                            while ((P < EndPos) && (char.IsDigit(SQL[P]) || (SQL[P] == '.')))
                                P++;
                            Token = SQL.Substring(TokenStart, P - TokenStart);
                            Res = TokenKind.Number;
                            return Res;
                        }
                        else
                            P++;
                        break;  //---------------------- 
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
                Res = TokenKind.End;
                Token = "";
            }

            return Res;
        }
    }



    public class Token
    {
        private TokenKind FKind = TokenKind.Unknown;
        private string FText = TokenKind.Unknown.ToString();

        /// <summary>
        /// 
        /// </summary>
        public Token()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Kind"></param>
        /// <param name="Value"></param>
        public Token(TokenKind Kind, string Text)
        {
            this.FKind = Kind;
            this.FText = Text;
        }
        /// <summary>
        /// 
        /// </summary>
        public TokenKind Kind { get { return FKind; } }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return FText; } }

    }








}
