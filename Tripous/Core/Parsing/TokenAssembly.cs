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

using Tripous.Tokenizing;

namespace Tripous.Parsing
{
    /// <summary>
    /// A TokenAssembly is an Assembly whose elements are Tokens.
    /// Tokens are, roughly, the chunks of text that a <code>
    /// Tokenizer</code> returns.
    /// </summary>
    public class TokenAssembly : Assembly
    {
        /// <summary>
        ///  the "string" of tokens this assembly will consume
        /// </summary>
        protected TokenString FTokenString;

        /* construction */
        /// <summary>
        /// Constructs a TokenAssembly on a TokenString constructed from the given string.
        /// </summary>
        /// <param name="s">the string to consume</param>
        public TokenAssembly(string s) 
            : this(new TokenString(s))
        {
        }
        /// <summary>
        /// Constructs a TokenAssembly on a TokenString constructed from the given Tokenizer.
        /// </summary>
        /// <param name="t">the tokenizer to consume tokens  from</param>
        public TokenAssembly(Tokenizer t) 
            : this(new TokenString(t))
        {
        }
        /// <summary>
        /// Constructs a TokenAssembly from the given TokenString.
        /// </summary>
        /// <param name="TokenString">the FTokenString to consume</param>
        public TokenAssembly(TokenString TokenString)
        {
            this.FTokenString = TokenString;
        }


        /* public */
        /// <summary>
        /// Creates and returns a copy of this instance.
        /// </summary>
        public override object Clone()
        {
            TokenString TS = (TokenString)FTokenString.Clone();
            Assembly A = new TokenAssembly(TS);
            return CloneProperties(A);
        }

        /// <summary>
        ///  Returns a textual representation of the amount of this  tokenAssembly that has been Consumed.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed  elements</param>
        /// <returns> Returns a textual representation of the amount of this  tokenAssembly that has been Consumed.</returns>
        public override string Consumed(string delimiter)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < ElementsConsumed(); i++)
            {
                if (i > 0)
                    buf.Append(delimiter);
                buf.Append(FTokenString.TokenAt(i));
            }
            return buf.ToString();
        }
        /// <summary>
        /// Returns a textual representation of the amount of this  tokenAssembly that remains to be Consumed.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed  elements</param>
        /// <returns>Returns a textual representation of the amount of this  tokenAssembly that remains to be Consumed.</returns>
        public override string Remainder(string delimiter)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = ElementsConsumed(); i < FTokenString.Length(); i++)
            {
                if (i > ElementsConsumed())
                    buf.Append(delimiter);

                buf.Append(FTokenString.TokenAt(i));
            }
            return buf.ToString();
        }

        /// <summary>
        /// Returns the default string to show between elements  Consumed or remaining.
        /// </summary>
        public override string DefaultDelimiter()
        {
            return "/";
        }
        
        /// <summary>
        /// Returns the number of elements in this assembly.
        /// </summary>
        public override int Length()
        {
            return FTokenString.Length();
        }
        /// <summary>
        /// Returns the next token from the associated token string.
        /// </summary>
        public override string NextElement()
        {
            return FTokenString.TokenAt(FIndex++).ToString();
        }
        /// <summary>
        /// Returns the next object in the assembly, without removing it
        /// </summary>
        public override object Peek()
        {
            if (FIndex < Length())
                return FTokenString.TokenAt(FIndex);
            else return null;
        }



    }
}
