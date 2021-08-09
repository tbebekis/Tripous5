using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous.Tokenizing;

namespace Tripous.Parsing
{
    /// <summary>
    /// A parser that matches against a new line sequence (CRLF, LFCR, CR, LF)
    /// </summary>
    public class NewLineParser : TerminalParser
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public NewLineParser()
        {
        }

        /* public */
        /// <summary>
        /// Returns true if the FLiteral this object Equals an assembly's next element.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if the FLiteral this object Equals an assembly's next element.</returns>
        public override bool Qualifies(object o)
        {
            Token T = o as Token;
            return T != null && T.Kind == Token.TT_NEWLINE;
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return "CRLF";
        }
    }
}
