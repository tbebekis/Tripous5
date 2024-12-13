/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Parsing
{
    using Tripous.Tokenizing;

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
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return "Word";
        }
    }
}
