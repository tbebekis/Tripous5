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
 * A Literal matches a specific string from an assembly.
 * 
 *
 *
 *
 */

    public class Literal : Terminal
    {
        /**
         * the FLiteral to Match
         */
        protected Token FLiteral;

        /**
         * Constructs a FLiteral that will Match the specified string.
         *
         * @param   string   the string to Match as a token
         *
         * @return   a FLiteral that will Match the specified string
         */
        public Literal(string s)
        {
            FLiteral = new Token(s);
        }
        /**
         * Returns true if the FLiteral this object Equals an
         * assembly's next element.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if the specified FLiteral Equals the next 
         *           token from an assembly
         */
        public override bool Qualifies(object o)
        {
            return FLiteral.Equals((Token)o);
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
            return FLiteral.ToString();
        }
    }
}
