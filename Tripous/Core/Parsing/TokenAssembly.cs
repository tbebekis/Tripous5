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
    /**
 * A TokenAssembly is an Assembly whose elements are Tokens.
 * Tokens are, roughly, the chunks of text that a <code>
 * Tokenizer</code> returns.
 * 
 *
 * 
 *
 */

    public class TokenAssembly : Assembly
    {
        /**
         * the "string" of tokens this assembly will consume
         */
        protected TokenString FTokenString;

        /**
         * Constructs a TokenAssembly on a TokenString constructed 
         * from the given string.
         *
         * @param   string   the string to consume
         *
         * @return   a TokenAssembly that will consume a tokenized 
         *           version of the supplied string
         */
        public TokenAssembly(string s) : this(new TokenString(s))
        {
        }
        /**
         * Constructs a TokenAssembly on a TokenString constructed 
         * from the given Tokenizer.
         *
         * @param   Tokenizer   the tokenizer to consume tokens 
         *                      from
         *
         * @return   a TokenAssembly that will consume a tokenized 
         *           version of the supplied Tokenizer
         */
        public TokenAssembly(Tokenizer t) : this(new TokenString(t))
        {
        }
        /**
         * Constructs a TokenAssembly from the given TokenString.
         *
         * @param   FTokenString   the FTokenString to consume
         *
         * @return   a TokenAssembly that will consume the supplied 
         *           TokenString
         */
        public TokenAssembly(TokenString TokenString)
        {
            this.FTokenString = TokenString;
        }
        /**
         * Returns a textual representation of the amount of this 
         * tokenAssembly that has been Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that has been Consumed
         */
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
        /**
         * Returns the default string to show between elements 
         * Consumed or remaining.
         *
         * @return   the default string to show between elements 
         *           Consumed or remaining
         */
        public override string DefaultDelimiter()
        {
            return "/";
        }
        /**
         * Returns the number of elements in this assembly.
         *
         * @return   the number of elements in this assembly
         */
        public override int Length()
        {
            return FTokenString.Length();
        }
        /**
         * Returns the next token.
         *
         * @return   the next token from the associated token string.
         *
         * @exception  ArrayIndexOutOfBoundsException  if there are no 
         *             more tokens in this tokenizer's string.
         */
        public override string NextElement()
        {
            return FTokenString.TokenAt(FIndex++).ToString();
        }
        /**
         * Shows the next object in the assembly, without removing it
         *
         * @return   the next object
         *
         */
        public override object Peek()
        {
            if (FIndex < Length())
                return FTokenString.TokenAt(FIndex);
            else return null;
        }
        /**
         * Returns a textual representation of the amount of this 
         * tokenAssembly that remains to be Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that remains to be Consumed
         */
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
        /**
function TZTokenAssembly.Clone: IZInterface;
begin
    Result := CloneProperties(TZTokenAssembly.Create(FTokenString.Clone as IZTokenString));
end;

        */
        public override object Clone()
        {
            TokenString TS = (TokenString)FTokenString.Clone();
            Assembly A = new TokenAssembly(TS);
            return CloneProperties(A);
        }

    }
}
