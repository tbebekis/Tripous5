﻿/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

using Tripous.Tokenizing;

namespace Tripous.Parsing
{
 
    /// <summary>
    /// A CaselessLiteral matches a specified string from an assembly, disregarding case.
    /// </summary>
    public class CaselessLiteralTerminalParser : LiteralTerminalParser
    {
        /* construction */
        /// <summary>
        /// Constructs a literal that will Match the specified string, given mellowness about case.
        /// </summary>
        /// <param name="literal">the string to Match as a token</param>
        public CaselessLiteralTerminalParser(string literal) 
            : base(literal)
        {
        }

        /* public */
        /// <summary>
        /// Returns true if the literal this object Equals an assembly's next element, disregarding case.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if the literal this object Equals an assembly's next element, disregarding case.</returns>
        public override bool Qualifies(object o)
        {
            Token T = o as Token;
            return T != null && string.Compare(LiteralValue, T.StringValue, true) == 0;  
        }
    }
}
