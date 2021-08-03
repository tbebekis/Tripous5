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
    /// A TokenStringSource enumerates over a specified reader, 
    /// returning TokenStrings delimited by a specified FDelimiter.
    /// For example, 
    /// <code>
    ///    
    ///     string s = "I came; I saw; I left in peace;";
    /// 
    ///     TokenStringSource tss =
    ///         new TokenStringSource(new Tokenizer(s), ";");
    /// 
    ///     while (tss.HasMoreTokenStrings()) {
    ///         System.out.println(tss.NextTokenString());
    ///     }	
    ///  
    /// </code>
    /// <para>
    /// prints out:
    /// </para>
    /// <para>
    /// <code>
    ///     I came
    ///     I saw
    ///     I left in peace
    /// </code>
    /// </para>
    /// </summary>
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
 

        /// <summary>
        /// Constructs a TokenStringSource that will read TokenStrings
        /// using the specified FTokenizer, delimited by the specified 
        /// FDelimiter.
        /// </summary>
        /// <param name="Tokenizer">a FTokenizer to read tokens from</param>
        /// <param name="Delimiter">the character that fences off where one  TokenString ends and the next begins</param>
        public TokenStringSource(Tokenizer Tokenizer, string Delimiter)
        {
            this.FTokenizer = Tokenizer;
            this.FDelimiter = Delimiter;
        }
 
        /// <summary>
        /// The design of <code>NextTokenString</code> is that is 
        /// always returns a cached value. This method will (at least 
        /// attempt to) load the cache if the cache is empty.
        /// </summary>
        protected void EnsureCacheIsLoaded()
        {
            if (FCachedTokenString == null)
                LoadCache();
        }

        /// <summary>
        /// Returns true , if the source has more TokenStrings that have not yet been popped with <code> NextTokenString</code>.
        /// </summary>
        public bool HasMoreTokenStrings()
        {
            EnsureCacheIsLoaded();
            return FCachedTokenString != null;
        }
        /// <summary>
        /// Loads the next TokenString into the cache, or sets the  cache to null if the source is out of tokens.
        /// </summary>
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
 
        /// <summary>
        /// Returns the next TokenString from the source.
        /// </summary>
         public TokenString NextTokenString()
        {
            EnsureCacheIsLoaded();
            TokenString returnTokenString = FCachedTokenString;
            FCachedTokenString = null;
            return returnTokenString;
        }
 
        /// <summary>
        /// Returns a ArrayList of the tokens in the source up to either  the FDelimiter or the end of the source.
        /// </summary>
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
