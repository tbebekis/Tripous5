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
 * An <code>Empty</code> parser matches any assembly once, 
 * and applies its assembler that one time.
 *  
 * Language elements often contain empty parts. For example, 
 * a language may at some point allow a list of parameters
 * in parentheses, and may allow an empty list. An empty
 * parser makes it easy to Match, within the 
 * parenthesis, either a list of parameters or "empty".
 * 
 *
 * 
 *
 * 
 */
    public class Empty : Parser
    {
        /**
         * Accept a "visitor" and a collection of previously visited
         * parsers.
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitEmpty(this, visited);
        }
        /**
         * Given a set of assemblies, this method returns the set as
         * a successful Match.
         * 
         * @return   the input set of states
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            return ElementClone(In);
        }
        /**
        * There really is no way to expand an empty parser, so
        * return an empty vector.
        */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            return new ArrayList();
        }
        /**
        * Returns a textual description of this parser.
        */
        public override string UnvisitedString(ArrayList visited)
        {
            return " empty ";
        }
    }
}
