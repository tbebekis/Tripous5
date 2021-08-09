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
using System.Collections.Generic;


namespace Tripous.Parsing
{

    /// <summary>
    /// A parser that matches a character from a character assembly.
    /// </summary>
    public class CharTerminalParser : TerminalParser
    {

        /// <summary>
        /// The mechanics of matching are the same for many terminals,
        /// except for the check that the next element on the assembly
        /// Qualifies as the type of terminal this terminal looks for.
        /// This method performs that check.
        /// </summary>
        /// <param name="o">an element from a assembly</param>
        /// <returns>Returns true, if the object is the kind of terminal this  parser seeks</returns>
        public override bool Qualifies(object o)
        {
            return true;
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return "C";
        }
    }
}
