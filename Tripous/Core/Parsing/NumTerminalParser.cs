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

using Tripous.Tokenizing;

namespace Tripous.Parsing
{
    /// <summary>
    /// A parser that matches a number from a token assembly.
    /// </summary>
    public class NumTerminalParser : TerminalParser
    {

        /// <summary>
        /// Returns true if an assembly's next element is a number.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true, if an assembly's next element is a number as recognized the tokenizer</returns>
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsNumber();
        }
        /// <summary>
        /// Create a set with one random number (between 0 and 100).
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();
            double d = Convert.ToDouble(Random.Next(100));

            d = Math.Floor(d);

            ArrayList v = new ArrayList();
            v.Add(d.ToString());
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
            return "Num";
        }
    }
}
