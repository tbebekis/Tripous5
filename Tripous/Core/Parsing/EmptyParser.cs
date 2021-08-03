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
 
    /// <summary>
    /// <para>
    /// An  parser that matches any assembly once, 
    /// and applies its assembler that one time.
    /// </para>
    /// <para>
    /// Language elements often contain empty parts. For example, 
    /// a language may at some point allow a list of parameters
    /// in parentheses, and may allow an empty list. An empty
    /// parser makes it easy to Match, within the 
    /// parenthesis, either a list of parameters or "empty".
    /// </para>
    /// </summary>
    public class EmptyParser : Parser
    {
        /*
         * 
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */

        /// <summary>
        /// Accept a "visitor" and a collection of previously visited parsers.
        /// </summary>
        /// <param name="pv">the visitor to Accept</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitEmpty(this, visited);
        }
        /// <summary>
        /// Given a set of assemblies, this method returns the set as a successful Match.
        /// </summary>
        public override ArrayList Match(ArrayList In)
        {
            return ElementClone(In);
        }
        /// <summary>
        /// There really is no way to expand an empty parser, so return an empty vector.
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            return new ArrayList();
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// </summary>
        public override string UnvisitedString(ArrayList visited)
        {
            return " empty ";
        }
    }
}
