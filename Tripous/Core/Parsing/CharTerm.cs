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

 

namespace Tripous.Parsing
{
    /**
 * A Char matches a character from a character assembly.
 * 
 *
 * 
 *
 */
    public class CharTerm : Terminal
    {

        /**
         * Returns true every time, since this class assumes it is 
         * working against a CharacterAssembly.
         *
         * @param   object   ignored
         *
         * @return   true, every time, since this class assumes it is 
         *           working against a CharacterAssembly
         */
        public override bool Qualifies(object o)
        {
            return true;
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
            return "C";
        }
    }
}
