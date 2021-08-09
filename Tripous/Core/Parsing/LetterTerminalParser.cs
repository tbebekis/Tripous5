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
    /// A parser that matches any letter from a character assembly.
    /// </summary>
    public class LetterTerminalParser : TerminalParser
    {

        /// <summary>
        /// Returns true if an assembly's next element is a letter.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if an assembly's next element is a letter.</returns>
        public override bool Qualifies(object o)
        {
            return char.IsLetter((char)o);
        }
        /// <summary>
        /// Create a set with one random letter.
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            char c = (char)(Random.Next(26) + 'a');
            ArrayList v = new ArrayList();
            v.Add(c.ToString());      //   new string(new char[]{c})
            return v;
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return "L";
        }
    }
}
