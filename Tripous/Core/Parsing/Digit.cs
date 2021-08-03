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
 * A Digit matches a digit from a character assembly.
 * 
 *
 * 
 *
 */
    public class Digit : Terminal
    {

        /**
         * Returns true if an assembly's next element is a digit.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is a digit
         */
        public override bool Qualifies(object o)
        {
            return char.IsDigit((char)o);
        }
        /**
         * Create a set with one random digit.
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            char c = (char)(Random.Next(10) + '0');
            ArrayList v = new ArrayList();
            v.Add(c.ToString());      //   new string(new char[]{c})
            return v;
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
            return "D";
        }
    }
}
