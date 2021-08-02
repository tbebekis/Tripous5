/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

using System;
using System.Text;
using System.Collections;

namespace Tripous.Tokenizing
{
    /**
 * A token represents a logical chunk of a string. For 
 * example, a typical tokenizer would break the string 
 * <code>"1.23 &lt;= 12.3"</code> into three tokens: the number 
 * 1.23, a less-than-or-equal symbol, and the number 12.3. A 
 * token is a receptacle, and relies on a tokenizer to decide 
 * precisely how to divide a string into tokens. 
 * 
 *
 *
 *
 */
    public class Token : ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        protected TokenKind FKind;
        /// <summary>
        /// 
        /// </summary>
        protected string FStringValue;
        /// <summary>
        /// 
        /// </summary>
        protected double FNumericValue;

        /** 
         * A constant indicating that the end of the stream has 
         * been ReadByte. 
         */
        public static TokenKind TT_EOF = new TokenKind("eof");
        /**
         * A constant indicating that there are no more tokens
         */
        public static Token EOF = new Token(TT_EOF, "", 0);

        /** 
         * A constant indicating that a token is a number, 
         * like 3.14
         */
        public static TokenKind TT_NUMBER = new TokenKind("number");

        /** 
         * A constant indicating a token is a word, like "cat"
         */
        public static TokenKind TT_WORD = new TokenKind("word");

        /**
         * A constant indicating that a token is a symbol 
         * like "&lt;=".
         */
        public static TokenKind TT_SYMBOL = new TokenKind("symbol");

        /**
         * A constant indicating that a token is a quoted string, 
         * like "Launch Mi".
         */
        public static TokenKind TT_QUOTED = new TokenKind("quoted");
        /// <summary>
        /// 
        /// </summary>
        public static TokenKind TT_WHITESPACE = new TokenKind("whitespace");
        /// <summary>
        /// 
        /// </summary>
        public static TokenKind TT_NEWLINE = new TokenKind("newline");

        /**
        */
        private Token()
        {
        }

        /**
         * Constructs a token from the given char.
         *
         * @param   char   the char
         *
         * @return   a token constructed from the given char
         */
        public Token(char C) : this(TT_SYMBOL, new string(new char[] { C }), 0)
        {
        }
        /**
         * Constructs a token from the given number.
         *
         * @param   double   the number
         *
         * @return   a token constructed from the given number
         */
        public Token(double N) : this(TT_NUMBER, "", N)
        {
        }
        /**
         * Constructs a token from the given string.
         *
         * @param   string   the string
         *
         * @return   a token constructed from the given string
         */
        public Token(string S) : this(TT_WORD, S, 0)
        {
        }
        /**
         * Constructs a token of the indicated type and associated 
         * string or numeric values.
         *
         * @param   TokenKind   the type of the token, typically one 
         *                      of the constants this class defines
         *
         * @param   string  the string value of the token, typically 
         *                  null except for WORD and QUOTED tokens
         *
         * @param   double   the numeric value of the token, typically
         *                   0 except for NUMBER tokens
         *
         * @return   a token
         */
        public Token(TokenKind Kind, string S, double N)
        {
            this.FKind = Kind;
            this.FStringValue = S;
            this.FNumericValue = N;
        }
        /**
         * Returns true if the supplied object is an equivalent token.
         *
         * @param   object   the object to compare
         *
         * @return   true, if the supplied object is of the same type 
         *           and value
         */
        new public bool Equals(Object o)
        {
            if (!(o is Token))
                return false;

            Token t = (Token)o;

            if (FKind != t.FKind)
            {
                return false;
            }
            if (FKind == TT_NUMBER)
            {
                return FNumericValue == t.FNumericValue;
            }
            if (FStringValue == null || t.FStringValue == null)
            {
                return false;
            }
            return FStringValue.Equals(t.FStringValue);
        }
        /**
         * Returns true if the supplied object is an equivalent token,
         * given mellowness about case in strings and characters.
         *
         * @param   object   the object to compare
         *
         * @return   true, if the supplied object is of the same type 
         *           and value. This method disregards case when 
         *           comparing the string value of tokens.
         */
        public bool EqualsIgnoreCase(Object o)
        {
            if (!(o is Token))
                return false;

            Token t = (Token)o;

            if (FKind != t.FKind)
            {
                return false;
            }
            if (FKind == TT_NUMBER)
            {
                return FNumericValue == t.FNumericValue;
            }
            if (FStringValue == null || t.FStringValue == null)
            {
                return false;
            }
            return string.Compare(FStringValue, t.FStringValue, true) == 0; // FStringValue.EqualsIgnoreCase(t.FStringValue);
        }
        /**
         * Returns true if this token is a number.
         *
         * @return   true, if this token is a number
         */
        public bool IsNumber()
        {
            return FKind == TT_NUMBER;
        }
        /**
         * Returns true if this token is a quoted string.
         *
         * @return   true, if this token is a quoted string
         */
        public bool IsQuotedString()
        {
            return FKind == TT_QUOTED;
        }
        /**
         * Returns true if this token is a symbol.
         *
         * @return   true, if this token is a symbol
         */
        public bool IsSymbol()
        {
            return FKind == TT_SYMBOL;
        }
        /**
         * Returns true if this token is a word.
         *
         * @return   true, if this token is a word.
         */
        public bool IsWord()
        {
            return FKind == TT_WORD;
        }

        /**
         * Return a textual description of this object.
         * 
         * @return a textual description of this object
         */
        public override string ToString()
        {
            if (FKind == TT_EOF)
            {
                return "EOF";
            }
            return Value.ToString();
        }
        /**
         * Returns the numeric value of this token.
         *
         * @return    the numeric value of this token
         */
        public double AsNumeric { get { return FNumericValue; } }
        /**
         * Returns the string value of this token.
         *
         * @return    the string value of this token
         */
        public string AsString { get { return FStringValue; } }
        /**
         * Returns the type of this token.
         *
         * @return   the type of this token, typically one of the
         *           constants this class defines
         */
        public TokenKind Kind { get { return FKind; } }

        /**
         * Returns an object that represents the value of this token.
         *
         * @return  an object that represents the value of this token
         */
        public object Value
        {
            get
            {
                if (FKind == TT_NUMBER) return FNumericValue;
                if (FKind == TT_EOF) return EOF;
                if (FStringValue != null) return FStringValue;

                return FKind;
            }
        }

        /**
        
        */
        public object Clone()
        {
            Token Res = new Token();
            Res.FKind = FKind;
            Res.FStringValue = FStringValue;
            Res.FNumericValue = FNumericValue;
            return Res;
        }
    }
}
