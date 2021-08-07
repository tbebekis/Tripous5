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
    /// A TokenString is like a string, but it is a series of 
    /// Tokens rather than a series of chars. Once a TokenString is 
    /// created, it is "immutable", meaning it cannot change. This
    /// lets you freely copy TokenStrings without worrying about 
    /// their state. 
    /// </summary>
    public class TokenString : ICloneable
    { 
        /* construction */
        /// <summary>
        /// Constructs a tokenString from the supplied FTokens.
        /// </summary>
        /// <param name="Tokens">the FTokens to use</param>
        public TokenString(Token[] Tokens)
        {
            this.Tokens = Tokens;
        }
        /// <summary>
        /// Constructs a tokenString from the supplied string. 
        /// </summary>
        /// <param name="s">the string to tokenize</param>
        public TokenString(string s) 
            : this(new Tokenizer(s))
        {
        }
        /// <summary>
        /// Constructs a tokenString from the supplied reader and  tokenizer. 
        /// </summary>
        /// <param name="t">the tokenizer that will produces the FTokens</param>
        public TokenString(Tokenizer t)
        {
            ArrayList v = new ArrayList();
            try
            {
                while (true)
                {
                    Token tok = t.NextToken();
                    if (tok.Kind == Token.TT_EOF)
                        break;
                    v.Add(tok);
                };
            }
            catch (Exception e)
            {
                throw (new Exception("Problem tokenizing string: " + e.Message));

            }
            Tokens = new Token[v.Count];
            v.CopyTo(Tokens);
        }
 
        /* public */
        /// <summary>
        /// Returns a string representation of this tokenString. 
        /// </summary>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < Tokens.Length; i++)
            {
                if (i > 0)
                    buf.Append(" ");

                buf.Append(Tokens[i]);
            }
            return buf.ToString();
        }
        /// <summary>
        /// Clones this instance and returns the clone.
        /// </summary>
        public object Clone()
        {
            Token[] Result = new Token[this.Tokens.Length];
            for (int i = 0; i < this.Tokens.Length; i++)
                Result[i] = this.Tokens[i].Clone() as Token;

            return new TokenString(Result);
        }

        /// <summary>
        /// Returns the token at a specified index.
        /// </summary>
        public Token TokenAt(int i)
        {
            return Tokens[i];
        }

        /* properties */
        /// <summary>
        /// Indexer. Returns the token at a specified index.
        /// </summary>
        public Token this[int Index] => Tokens[Index];
        /// <summary>
        /// Returns the number of tokens in this token string.
        /// </summary>
        public int Length => Tokens.Length;
        /// <summary>
        ///  The tokens in this token string.
        /// </summary>
        public Token[] Tokens { get; private set; }
    }
}
