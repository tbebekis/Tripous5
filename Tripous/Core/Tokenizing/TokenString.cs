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
        /// <summary>
        /// the FTokens in this tokenString
        /// </summary>
        protected Token[] FTokens;
 
        /// <summary>
        /// Constructs a tokenString from the supplied FTokens.
        /// </summary>
        /// <param name="Tokens">the FTokens to use</param>
        public TokenString(Token[] Tokens)
        {
            this.FTokens = Tokens;
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
            FTokens = new Token[v.Count];
            v.CopyTo(FTokens);
        }

        /// <summary>
        /// Returns the number of FTokens in this tokenString.
        /// </summary>
        public int Length()
        {
            return FTokens.Length;
        }
        /// <summary>
        /// Returns the token at the specified index.
        /// </summary>
        public Token TokenAt(int i)
        {
            return FTokens[i];
        }
        /// <summary>
        /// Returns a string representation of this tokenString. 
        /// </summary>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < FTokens.Length; i++)
            {
                //if (i > 0)
                //    buf.Append(" ");

                buf.Append(FTokens[i]);
            }
            return buf.ToString();
        }
        /// <summary>
        /// Clones this instance and returns the clone.
        /// </summary>
        public object Clone()
        {
            Token[] Tokens = new Token[FTokens.Length];
            for (int i = 0; i < FTokens.Length; i++)
                Tokens[i] = (Token)FTokens[i].Clone();

            return new TokenString(Tokens);
        }
    }
}
