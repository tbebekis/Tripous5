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
 * A TokenStringSource enumerates over a specified reader, 
 * returning TokenStrings delimited by a specified FDelimiter.
 * For example, 
 * <blockquote><pre>
 *   
 *    string s = "I came; I saw; I left in peace;";
 *
 *    TokenStringSource tss =
 *        new TokenStringSource(new Tokenizer(s), ";");
 *
 *    while (tss.HasMoreTokenStrings()) {
 *        System.out.println(tss.NextTokenString());
 *    }	
 * 
 * </pre></blockquote>
 * 
 * prints out:
 * 
 * <blockquote><pre>    
 *     I came
 *     I saw
 *     I left in peace
 * </pre></blockquote>
 * 
 *
 * 
 * @version 1.0
 */

    public class TokenStringSource
    {
        /// <summary>
        /// 
        /// </summary>
        protected Tokenizer FTokenizer;
        /// <summary>
        /// 
        /// </summary>
        protected string FDelimiter;
        /// <summary>
        /// 
        /// </summary>
        protected TokenString FCachedTokenString = null;
        /**
         * Constructs a TokenStringSource that will read TokenStrings
         * using the specified FTokenizer, delimited by the specified 
         * FDelimiter.
         *
         * @param   FTokenizer   a FTokenizer to read tokens from
         *
         * @param   FDelimiter   the character that fences off where one 
         *                      TokenString ends and the next begins
         *
         * @returns   a TokenStringSource that will read TokenStrings
         *            from the specified FTokenizer, delimited by the 
         *            specified FDelimiter
         */
        public TokenStringSource(Tokenizer Tokenizer, string Delimiter)
        {
            this.FTokenizer = Tokenizer;
            this.FDelimiter = Delimiter;
        }
        /**
         * The design of <code>NextTokenString</code> is that is 
         * always returns a cached value. This method will (at least 
         * attempt to) load the cache if the cache is empty.
         */
        protected void EnsureCacheIsLoaded()
        {
            if (FCachedTokenString == null)
                LoadCache();
        }
        /**
         * Returns true if the source has more TokenStrings.
         *
         * @return   true, if the source has more TokenStrings that 
         *           have not yet been popped with <code>
         *           NextTokenString</code>.
         */
        public bool HasMoreTokenStrings()
        {
            EnsureCacheIsLoaded();
            return FCachedTokenString != null;
        }
        /**
         * Loads the next TokenString into the cache, or sets the 
         * cache to null if the source is out of tokens.
         */
        protected void LoadCache()
        {
            ArrayList tokenVector = NextVector();
            if (tokenVector.Count == 0)
                FCachedTokenString = null;
            else
            {
                Token[] tokens = new Token[tokenVector.Count];
                tokenVector.CopyTo(tokens);
                FCachedTokenString = new TokenString(tokens);
            }
        }
        /**
         * Shows the example in the class comment.
         *
         * @param args ignored

        public static void main(string args[]) {
      
           string s = "I came; I saw; I left in peace;";
      
           TokenStringSource tss =
              new TokenStringSource(new Tokenizer(s), ";");
          
           while (tss.HasMoreTokenStrings()) {
              System.out.println(tss.NextTokenString());
           }	
        }  
        */
        /**
         * Returns the next TokenString from the source.
         *
         * @return   the next TokenString from the source
         */
        public TokenString NextTokenString()
        {
            EnsureCacheIsLoaded();
            TokenString returnTokenString = FCachedTokenString;
            FCachedTokenString = null;
            return returnTokenString;
        }
        /**
         * Returns a ArrayList of the tokens in the source up to either 
         * the FDelimiter or the end of the source.
         *
         * @return   a ArrayList of the tokens in the source up to either
         *           the FDelimiter or the end of the source.
         */
        protected ArrayList NextVector()
        {
            ArrayList v = new ArrayList();
            try
            {
                while (true)
                {
                    Token tok = FTokenizer.NextToken();
                    if (tok.Kind == Token.TT_EOF || tok.AsString.Equals(FDelimiter))
                        break;

                    v.Add(tok);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("Problem tokenizing string: " + e.Message));
            }
            return v;
        }
    }
}
