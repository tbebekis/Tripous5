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
 * A TokenString is like a string, but it is a series of 
 * Tokens rather than a series of chars. Once a TokenString is 
 * created, it is "immutable", meaning it cannot change. This
 * lets you freely copy TokenStrings without worrying about 
 * their state. 
 * 
 *
 * 
 *
 */

    public class TokenString : ICloneable
    {
        /**
         * the FTokens in this tokenString
         */
        protected Token[] FTokens;
        /**
         * Constructs a tokenString from the supplied FTokens.
         *
         * @param   FTokens   the FTokens to use
         *
         * @return    a tokenString constructed from the supplied 
         *            FTokens
         */
        public TokenString(Token[] Tokens)
        {
            this.FTokens = Tokens;
        }
        /**
         * Constructs a tokenString from the supplied string. 
         *
         * @param   string   the string to tokenize
         *
         * @return    a tokenString constructed from FTokens read from 
         *            the supplied string
         */
        public TokenString(string s) : this(new Tokenizer(s))
        {
        }
        /**
         * Constructs a tokenString from the supplied reader and 
         * tokenizer. 
         * 
         * @param   Tokenizer   the tokenizer that will produces the 
         *                      FTokens
         *
         * @return    a tokenString constructed from the tokenizer's 
         *            FTokens
         */
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
        /**
         * Returns the number of FTokens in this tokenString.
         *
         * @return   the number of FTokens in this tokenString
         */
        public int Length()
        {
            return FTokens.Length;
        }
        /**
         * Returns the token at the specified index.
         *
         * @param    index   the index of the desired token
         * 
         * @return   token   the token at the specified index
         */
        public Token TokenAt(int i)
        {
            return FTokens[i];
        }
        /**
         * Returns a string representation of this tokenString. 
         *
         * @return   a string representation of this tokenString
         */
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < FTokens.Length; i++)
            {
                if (i > 0)
                {
                    buf.Append(" ");
                }
                buf.Append(FTokens[i]);
            }
            return buf.ToString();
        }
        /**
        */
        public object Clone()
        {
            Token[] Tokens = new Token[FTokens.Length];
            for (int i = 0; i < FTokens.Length; i++)
                Tokens[i] = (Token)FTokens[i].Clone();

            return new TokenString(Tokens);
        }
    }
}
