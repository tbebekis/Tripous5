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
 * A Num matches a number from a token assembly.
 * 
 *
 * 
 *
 */

    public class Num : Terminal
    {

        /**
         * Returns true if an assembly's next element is a number.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is a number as
         *           recognized the tokenizer
         */
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsNumber();
        }
        /**
         * Create a set with one random number (between 0 and 100).
         * 
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {

            Random Random = new Random();
            double d = Convert.ToDouble(Random.Next(100));

            d = Math.Floor(d);

            ArrayList v = new ArrayList();
            v.Add(d.ToString());
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
            return "Num";
        }
    }
}
