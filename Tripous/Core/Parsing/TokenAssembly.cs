/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Parsing
{
    using Tripous.Tokenizing;

    /// <summary>
    /// A TokenAssembly is an Assembly whose elements are Tokens.
    /// Tokens are, roughly, the chunks of text that a tokenizer returns.
    /// </summary>
    public class TokenAssembly: Assembly  
    {
        /// <summary>
        ///  the "string" of tokens this assembly will consume
        /// </summary>
        protected TokenList fTokenList;

        /* construction */
        /// <summary>
        /// Constructs a <see cref="TokenAssembly"/> on a <see cref="TokenList"/> constructed from the given string.
        /// </summary>
        /// <param name="s">The string to consume</param>
        public TokenAssembly(string s) 
            : this(new TokenList(s))
        {
        }
        /// <summary>
        /// Constructs a <see cref="TokenAssembly"/> on a <see cref="TokenList"/> constructed from the given <see cref="Tokenizer"/>.
        /// </summary>
        /// <param name="t">The tokenizer to consume tokens  from</param>
        public TokenAssembly(Tokenizer t) 
            : this(new TokenList(t))
        {
        }
        /// <summary>
        /// Constructs a <see cref="TokenAssembly"/> from the given <see cref="TokenList"/>.
        /// </summary>
        /// <param name="TokenList">The <see cref="TokenList"/> to consume</param>
        public TokenAssembly(TokenList TokenList)
        {
            this.fTokenList = TokenList;
        }

        /* public */
        /// <summary>
        ///  Returns a textual representation of the amount of this  tokenAssembly that has been Consumed.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed  elements</param>
        /// <returns> Returns a textual representation of the amount of this  tokenAssembly that has been Consumed.</returns>
        public override string Consumed(string delimiter)
        {
            StringBuilder SB = new StringBuilder();
            string sToken;
            for (int i = 0; i < ElementsConsumed; i++)
            {
                if (i > 0)
                    SB.Append(delimiter);
                sToken = fTokenList.TokenAt(i).ToString();
                SB.Append(sToken);
            }
            return SB.ToString();
        }
        /// <summary>
        /// Returns a textual representation of the amount of this  tokenAssembly that remains to be Consumed.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed  elements</param>
        /// <returns>Returns a textual representation of the amount of this  tokenAssembly that remains to be Consumed.</returns>
        public override string Remainder(string delimiter)
        {
            StringBuilder SB = new StringBuilder();
            for (int i = ElementsConsumed; i < fTokenList.Length; i++)
            {
                if (i > ElementsConsumed)
                    SB.Append(delimiter);

                SB.Append(fTokenList.TokenAt(i));
            }
            return SB.ToString();
        }

        /// <summary>
        /// Returns the value of the next token from the associated token string, as a string.
        /// </summary>
        public override string NextElement()
        {
            return fTokenList.TokenAt(Index++).ToString();
        }
        /// <summary>
        /// Returns the next object in the assembly, without removing it
        /// </summary>
        public override object Peek()
        {
            if (Index < Length)
                return fTokenList.TokenAt(Index);
            else return null;
        }

        /* properties */
        /// <summary>
        /// Indexer. Returns the token at a specified index.
        /// </summary>
        public Token this[int Index] => fTokenList[Index];
        /// <summary>
        /// Returns the number of elements in this assembly.
        /// </summary>
        public override int Length => fTokenList.Length;
        /// <summary>
        ///  The tokens in this token string.
        /// </summary>
        public Token[] Tokens => fTokenList.Tokens;
        /// <summary>
        /// Returns the default string to show between elements  Consumed or remaining.
        /// </summary>
        public override string DefaultDelimiter => "/";

    }
}
