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
 * A SpecificChar matches a specified character from a character 
 * assembly.
 * 
 *
 * 
 *
 */
    public class SpecificChar : Terminal
    {

        /**
         * the character to Match
         */
        protected char FChar;
        /**
         * Constructs a SpecificChar to Match the specified char.
         *
         * @param   char  the character to Match
         *
         * @return   a SpecificChar to Match a Character constructed
         *           from the specified char.
         */
        public SpecificChar(char c)
        {
            this.FChar = c;
        }

        /**
         * Returns true if an assembly's next element is equal to the 
         * character this object was constructed with.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is equal to 
         *           the character this object was constructed with
         */
        public override bool Qualifies(object o)
        {
            char c = (char)o;
            return c.Equals(FChar);
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
            return FChar.ToString();
        }

    }
}
