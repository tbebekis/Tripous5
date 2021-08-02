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
 * A CaselessLiteral matches a specified string from an
 * assembly, disregarding case.
 * 
 *
 * 
 *
 */
    public class CaselessLiteral : Literal
    {
        /**
         * Constructs a literal that will Match the specified string,
         * given mellowness about case.
         *
         * @param   string   the string to Match as a token
         *
         * @return   a literal that will Match the specified string,
         *           disregarding case
         */
        public CaselessLiteral(string literal) : base(literal)
        {
        }
        /**
         * Returns true if the literal this object Equals an
         * assembly's next element, disregarding case.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if the specified literal Equals the next
         *           token from an assembly, disregarding case
         */
        public override bool Qualifies(object o)
        {
            return FLiteral.EqualsIgnoreCase((Token)o);
        }
    }
}
