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
    /// A QuotedString matches a quoted string, like "this one"  from a token assembly.
    /// </summary>
    public class QuotedStringTerminalParser : TerminalParser
    {
 
        /// <summary>
        ///  Returns true if an assembly's next element is a quoted  string, like "chubby cherubim".
        /// </summary>
        /// <param name="o">an element from a assembly</param>
        /// <returns>Returns true if an assembly's next element is a quoted  string, like "chubby cherubim".</returns>
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsQuotedString();
        }
        /// <summary>
        /// Create a set with one random quoted string (with 2 to 6 characters).
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();
            int n = Random.Next(5);

            char[] letters = new char[n + 2];
            letters[0] = '"';
            letters[n + 1] = '"';

            for (int i = 0; i < n; i++)
            {
                int c = Random.Next(26) + 'a';
                letters[i + 1] = (char)c;
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
            return "QuotedString";
        }
    }
}
