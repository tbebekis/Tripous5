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
    /// A SpecificChar matches a specified character from a character  assembly.
    /// </summary>
    public class SpecificCharTerminalParser : TerminalParser
    {

        /// <summary>
        /// the character to Match
        /// </summary>
        protected char FChar;

        /* construction */
        /// <summary>
        /// Constructs a SpecificChar to Match a Character constructed rom the specified char.
        /// </summary>
        /// <param name="c">the character to Match</param>
        public SpecificCharTerminalParser(char c)
        {
            this.FChar = c;
        }

 
        /// <summary>
        /// Returns true if an assembly's next element is equal to the character this object was constructed with.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if an assembly's next element is equal to the character this object was constructed with.</returns>
        public override bool Qualifies(object o)
        {
            char c = (char)o;
            return c.Equals(FChar);
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return FChar.ToString();
        }

    }
}
