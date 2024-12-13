/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

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
        static public Token EOF = new Token() { Kind = TT_EOF };

        /* construction */
        /// <summary>
        /// Private constructor
        /// </summary>
        private Token()
        {
        }

        /// <summary>
        /// Constructs a token of the indicated type and associated string or numeric values.
        /// </summary>
        /// <param name="Kind">the type of the token, typically one  of the constants this class defines</param>
        /// <param name="StringValue">the string value of the token, typically null except for WORD and QUOTED tokens</param>
        /// <param name="NumericValue">the numeric value of the token, typically 0 except for NUMBER tokens</param>
        static public Token Create(TokenKind Kind, string StringValue, double NumericValue)
        {
            Token Result = new Token();
            Result.Kind = Kind;
            Result.StringValue = StringValue;
            Result.NumericValue = NumericValue;
            return Result;
        }


        /* public */
        /// <summary>
        /// Return a textual description of this object.
        /// </summary>
        public override string ToString()
        {
            return Kind == TT_EOF ? "EOF" : Value.ToString(); //$"{Kind.ToString().PadRight(12)}{Value}"; 
        }
        /// <summary>
        /// Clones this instance and returns the clone.
        /// </summary>
        public object Clone()
        {
            Token Result = new Token();
            Result.Kind = Kind;
            Result.StringValue = StringValue;
            Result.NumericValue = NumericValue;
            return Result;
        }

        /// <summary>
        /// Returns true if the supplied object is an equivalent token.
        /// </summary>
        /// <param name="o">the object to compare</param>
        /// <returns>Returns true, if the supplied object is of the same type and value</returns>
        new public bool Equals(object o)
        {
            if (!(o is Token))
                return false;

            Token t = (Token)o;

            if (Kind != t.Kind)
            {
                return false;
            }
            if (Kind == TT_NUMBER)
            {
                return NumericValue == t.NumericValue;
            }
            if (StringValue == null || t.StringValue == null)
            {
                return false;
            }
            return StringValue.Equals(t.StringValue);
        }
        /// <summary>
        /// Returns true, if the supplied object is of the same type and value. 
        /// This method disregards case when  comparing the string value of tokens.
        /// </summary>
        public bool EqualsIgnoreCase(object o)
        {
            if (!(o is Token))
                return false;

            Token t = (Token)o;

            if (Kind != t.Kind)
            {
                return false;
            }
            if (Kind == TT_NUMBER)
            {
                return NumericValue == t.NumericValue;
            }
            if (StringValue == null || t.StringValue == null)
            {
                return false;
            }
            return string.Compare(StringValue, t.StringValue, true) == 0;  
        }

        /// <summary>
        /// Returns true if this token is a number.
        /// </summary>
        public bool IsNumber()
        {
            return Kind == TT_NUMBER;
        }
        /// <summary>
        /// Returns true if this token is a quoted string.
        /// </summary>
        public bool IsQuotedString()
        {
            return Kind == TT_QUOTED;
        }
        /// <summary>
        /// Returns true if this token is a symbol.
        /// </summary>
        public bool IsSymbol()
        {
            return Kind == TT_SYMBOL;
        }
        /// <summary>
        /// Returns true if this token is a word.
        /// </summary>
        public bool IsWord()
        {
            return Kind == TT_WORD;
        }

        /* properties */
        /// <summary>
        /// Returns the type of this token., typically one of the constants this class defines
        /// </summary>
        public TokenKind Kind { get; protected set; }
        /// <summary>
        /// Returns an object that represents the value of this token.
        /// </summary>
        public object Value
        {
            get
            {
                if (Kind == TT_NUMBER) return NumericValue;
                if (Kind == TT_EOF) return EOF;
                if (StringValue != null) return StringValue;

                return Kind;
            }
        }
        /// <summary>
        /// Returns the numeric value of this token.
        /// </summary>
        public double NumericValue { get; protected set; } = 0;
        /// <summary>
        /// Returns the string value of this token.
        /// </summary>
        public string StringValue { get; protected set; } = "";

        /// <summary>
        /// Returns a string representation of this token for display purposes.
        /// </summary>
        public string DisplayText
        {
            get
            {
                if (Kind == TT_EOF)
                    return "EOF";

                string sKind = Kind.ToString(); //.PadRight(10);
                string sPos = $"{LineIndex},{CharIndex}";

                return $"{sPos.PadRight(10)} {sKind.PadRight(10)} {Value}";
            }
        }

        /// <summary>
        /// Returns the line index of the source text, this token is found
        /// </summary>
        public int LineIndex { get; set; }
        /// <summary>
        /// Returns the character index, in the line this token is found, of the first character of this token.
        /// </summary>
        public int CharIndex { get; set; }
    }
}
