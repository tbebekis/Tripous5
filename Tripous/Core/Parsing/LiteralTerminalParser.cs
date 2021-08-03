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
    /// <summary>
    /// A parser that matches a specific string from an assembly.
    /// </summary>
    public class LiteralTerminalParser : TerminalParser
    {
        /// <summary>
        /// the FLiteral to Match
        /// </summary>
        protected Token FLiteral;

        /* construction */
        /// <summary>
        /// Constructs a FLiteral that will Match the specified string.
        /// </summary>
        /// <param name="s">the string to Match as a token</param>
        public LiteralTerminalParser(string s)
        {
            FLiteral = new Token(s);
        }

        /* public */
        /// <summary>
        /// Returns true if the FLiteral this object Equals an assembly's next element.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if the FLiteral this object Equals an assembly's next element.</returns>
        public override bool Qualifies(object o)
        {
            return FLiteral.Equals((Token)o);
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// </summary>
        /// <param name="visited">a list of parsers already printed in  this description</param>
        /// <returns>Returns a textual description of this parser.</returns>
        public override string UnvisitedString(ArrayList visited)
        {
            return FLiteral.ToString();
        }
    }
}
