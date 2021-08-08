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


    /// <summary>
    /// A token represents a logical chunk of a string. 
    /// <para> For example, a typical tokenizer would break the string  <code>"1.23 &lt;= 12.3"</code> 
    /// into three tokens: the number 1.23, a less-than-or-equal symbol, and the number 12.3. 
    /// </para>
    /// <para>A token is a receptacle, and relies on a tokenizer to decide 
    /// precisely how to divide a string into tokens. </para>
    /// </summary>
    public class Token : ICloneable
    {
        /// <summary>
        /// The kind of a token
        /// </summary>
        protected TokenKind FKind;
        /// <summary>
        /// Holds the value, when the value is a string
        /// </summary>
        protected string FStringValue;
        /// <summary>
        /// Holds the value, when the value is a number
        /// </summary>
        protected double FNumericValue;

        /* static public fields */
        /// <summary>
        /// A constant indicating that the end of the stream has  been ReadByte. 
        /// </summary>
        static public TokenKind TT_EOF = new TokenKind("eof");
        /// <summary>
        /// A constant indicating that a token is a number, like 3.14
        /// </summary>
        static public TokenKind TT_NUMBER = new TokenKind("number");
        /// <summary>
        /// A constant indicating a token is a word, like "cat"
        /// </summary>
        static public TokenKind TT_WORD = new TokenKind("word");
        /// <summary>
        /// A constant indicating that a token is a symbol like "&lt;=".
        /// </summary>
        static public TokenKind TT_SYMBOL = new TokenKind("symbol");
        /// <summary>
        /// A constant indicating that a token is a quoted string,  like "Launch Mi".
        /// </summary>
        static public TokenKind TT_QUOTED = new TokenKind("quoted");
        /// <summary>
        /// Indicates a whitespace token
        /// </summary>
        static public TokenKind TT_WHITESPACE = new TokenKind("whitespace");
        /// <summary>
        /// Indicates a new line token
        /// </summary>
        static public TokenKind TT_NEWLINE = new TokenKind("newline");

        /// <summary>
        /// A constant indicating that there are no more tokens
        /// </summary>
        static public Token EOF = new Token(TT_EOF, "", 0);

        /* construction */
        /// <summary>
        /// Private constructor
        /// </summary>
        private Token()
        {
        }
        /// <summary>
        /// Constructs a token from the given char.
        /// </summary>
        public Token(char C) 
            : this(TT_SYMBOL, new string(new char[] { C }), 0)
        {
        }
        /// <summary>
        /// Constructs a token from the given number.
        /// </summary>
        public Token(double N) 
            : this(TT_NUMBER, "", N)
        {
        }
        /// <summary>
        /// Constructs a token from the given string.
        /// </summary>
        public Token(string S) : this(TT_WORD, S, 0)
        {
        }
        /// <summary>
        /// Constructs a token of the indicated type and associated string or numeric values.
        /// </summary>
        /// <param name="Kind">the type of the token, typically one  of the constants this class defines</param>
        /// <param name="S">the string value of the token, typically null except for WORD and QUOTED tokens</param>
        /// <param name="N">the numeric value of the token, typically 0 except for NUMBER tokens</param>
        public Token(TokenKind Kind, string S, double N)
        {
            this.FKind = Kind;
            this.FStringValue = S;
            this.FNumericValue = N;
        }


        /* public */
        /// <summary>
        /// Return a textual description of this object.
        /// </summary>
        public override string ToString()
        {
            return FKind == TT_EOF ? "EOF" : Value.ToString(); //$"{FKind.ToString().PadRight(12)}{Value}"; 
        }
        /// <summary>
        /// Clones this instance and returns the clone.
        /// </summary>
        public object Clone()
        {
            Token Res = new Token();
            Res.FKind = FKind;
            Res.FStringValue = FStringValue;
            Res.FNumericValue = FNumericValue;
            return Res;
        }

        /// <summary>
        /// Returns true if the supplied object is an equivalent token.
        /// </summary>
        /// <param name="o">the object to compare</param>
        /// <returns>Returns true, if the supplied object is of the same type and value</returns>
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
        /// <summary>
        /// Returns true, if the supplied object is of the same type and value. 
        /// This method disregards case when  comparing the string value of tokens.
        /// </summary>
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

        /// <summary>
        /// Returns true if this token is a number.
        /// </summary>
        public bool IsNumber()
        {
            return FKind == TT_NUMBER;
        }
        /// <summary>
        /// Returns true if this token is a quoted string.
        /// </summary>
        public bool IsQuotedString()
        {
            return FKind == TT_QUOTED;
        }
        /// <summary>
        /// Returns true if this token is a symbol.
        /// </summary>
        public bool IsSymbol()
        {
            return FKind == TT_SYMBOL;
        }
        /// <summary>
        /// Returns true if this token is a word.
        /// </summary>
        public bool IsWord()
        {
            return FKind == TT_WORD;
        }

        /* properties */
        /// <summary>
        /// Returns the type of this token., typically one of the constants this class defines
        /// </summary>
        public TokenKind Kind { get { return FKind; } }
        /// <summary>
        /// Returns an object that represents the value of this token.
        /// </summary>
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
        /// <summary>
        /// Returns the numeric value of this token.
        /// </summary>
        public double AsNumeric { get { return FNumericValue; } }
        /// <summary>
        /// Returns the string value of this token.
        /// </summary>
        public string AsString { get { return FStringValue; } }

        /// <summary>
        /// Returns a string representation of this token for display purposes.
        /// </summary>
        public string DisplayText => FKind == TT_EOF ? "EOF" : $"{FKind.ToString().PadRight(12)}{Value}"; 


        public int LineIndex { get; set; }
        public int CharIndex { get; set; }
    }
}
