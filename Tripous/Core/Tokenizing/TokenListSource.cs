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
using System.Collections.Generic;

namespace Tripous.Tokenizing
{


    /// <summary>
    /// A <see cref="TokenListSource"/> enumerates over a specified <see cref="Tokenizing.Tokenizer"/>, 
    /// returning <see cref="TokenList"/>s delimited by a specified Delimiter.
    /// For example, 
    /// <code>
    ///    
    ///     string s = "I came; I saw; I left in peace;";
    /// 
    ///     TokenListSource tss = new TokenListSource(new Tokenizer(s), ";");
    /// 
    ///     while (tss.HasMoreTokenLists()) {
    ///         System.out.println(tss.NextTokenList());
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
    /// <para>NOTE: TokenStringSource in the original code.</para>
    /// </summary>
    public class TokenListSource
    {

        /// <summary>
        /// 
        /// </summary>
        protected TokenList fChachedTokenList = null;

        /// <summary>
        /// The design of <see cref="NextTokenList"/>() is that is always returns a cached value. 
        /// This method will (at least attempt to) load the cache if the cache is empty.
        /// </summary>
        protected void EnsureCacheIsLoaded()
        {
            if (fChachedTokenList == null)
                LoadCache();
        }
        /// <summary>
        /// Loads the next <see cref="TokenList"/> into the cache, or sets the cache to null if the source is out of tokens.
        /// </summary>
        protected void LoadCache()
        {
            Token[] tokens = NextTokenVector();
            fChachedTokenList = tokens.Length != 0 ? new TokenList(tokens) : null;
        }
        /// <summary>
        /// Returns a ArrayList of the tokens in the source up to either the <see cref="Delimiter"/> or the end of the source.
        /// </summary>
        protected Token[] NextTokenVector()
        {
            Token tok = null;
            List<Token> ResultList = new List<Token>();
            try
            {
                while (true)
                {
                    tok = Tokenizer.NextToken();
                    if (tok.Kind == Token.TT_EOF || tok.StringValue.Equals(Delimiter))
                        break;

                    ResultList.Add(tok);
                }
            }
            catch (Exception e)
            {
                string Message = tok != null ? $"Position: {tok.LineIndex}, {tok.CharIndex}. " : string.Empty;
                Message = $"{Message}Problem tokenizing string: {e.Message}";
                throw new ApplicationException(Message);
            }

            return ResultList.ToArray();
        }

        /* construction */
        /// <summary>
        /// Constructs a TokenStringSource that will read TokenStrings
        /// using the specified FTokenizer, delimited by the specified 
        /// FDelimiter.
        /// </summary>
        /// <param name="Tokenizer">The <see cref="Tokenizer"/> to read tokens from</param>
        /// <param name="Delimiter">The character that fences off where one <see cref="TokenList"/> ends and the next begins</param>
        public TokenListSource(Tokenizer Tokenizer, string Delimiter)
        {
            this.Tokenizer = Tokenizer;
            this.Delimiter = Delimiter;
        }
 
        /* public */
        /// <summary>
        /// Returns true, if the source has more TokenStrings that have not yet been popped with <code> NextTokenString</code>.
        /// </summary>
        public bool HasMoreTokenLists()
        {
            EnsureCacheIsLoaded();
            return fChachedTokenList != null;
        }
        /// <summary>
        /// Returns the next <see cref="TokenList"/> from the source.
        /// </summary>
        public TokenList NextTokenList()
        {
            EnsureCacheIsLoaded();
            TokenList Result = fChachedTokenList;
            fChachedTokenList = null;
            return Result;
        }
 
        /* properties */
        /// <summary>
        /// The <see cref="Tokenizer"/> to read tokens from
        /// </summary>
        public Tokenizer Tokenizer { get; private set; }
        /// <summary>
        /// The character that fences off where one <see cref="TokenList"/> ends and the next begins
        /// </summary>
        public string Delimiter { get; private set; }
    }
}
