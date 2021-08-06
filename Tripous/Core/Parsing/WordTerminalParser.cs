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
    /// A Word matches a word from a token assembly.
    /// </summary>
    public class WordTerminalParser : TerminalParser
    { 
        /// <summary>
        /// Returns true if an assembly's next element is a word.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if an assembly's next element is a word.</returns>
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsWord();
        }
        /// <summary>
        /// Create a set with one random word (with 3 to 7 characters).
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            int n = Random.Next(5) + 3;

            char[] letters = new char[n];
            for (int i = 0; i < n; i++)
            {
                int c = Random.Next(26) + 'a';
                letters[i] = (char)c;
            }

            ArrayList v = new ArrayList();
            v.Add(new string(letters));
            return v;
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// </summary>
        /// <param name="visited">a list of parsers already printed in  this description</param>
        /// <returns>Returns a textual description of this parser.</returns>
        public override string UnvisitedString(ArrayList visited)
        {
            return "Word";
        }
    }
}
