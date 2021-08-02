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
 * A Symbol matches a specific sequence, such as greater, or equal
 *  that a tokenizer
 * returns as a FSymbol. 
 * 
 *
 * 
 *
 */

    public class Symbol : Terminal
    {
        /**
         * the literal to Match
         */
        protected Token FSymbol;
        /**
         * Constructs a FSymbol that will Match the specified char.
         *
         * @param   char   the character to Match. The char must be 
         *                 one that the tokenizer will return as a 
         *                 FSymbol token. This typically includes most 
         *                 characters except letters and digits. 
         *
         * @return   a FSymbol that will Match the specified char
         */
        public Symbol(char c) : this(c.ToString())
        {
        }
        /**
         * Constructs a FSymbol that will Match the specified sequence
         * of characters.
         *
         * @param   string   the characters to Match. The characters
         *                   must be a sequence that the tokenizer will 
         *                   return as a FSymbol token, such as greater.
         *                   
         *
         * @return   a Symbol that will Match the specified sequence
         *           of characters
         */
        public Symbol(string s)
        {
            FSymbol = new Token(Token.TT_SYMBOL, s, 0);
        }
        /**
         * Returns true if the FSymbol this object represents Equals an
         * assembly's next element.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if the specified FSymbol Equals the next 
         *           token from an assembly
         */
        public override bool Qualifies(object o)
        {
            return FSymbol.Equals((Token)o);
        }
        /**
         * Returns a textual description of this parser.
         *
         * @param   vector   a list of parsers already printed in 
         *                   this description
         * 
         * @return   string   a textual description of this parser
         *
         * @see Parser#ToString()
         */
        public override string UnvisitedString(ArrayList visited)
        {
            return FSymbol.ToString();
        }
    }
}
