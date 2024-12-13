/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Parsing
{
    /// <summary>
    /// A Digit matches a digit from a character assembly.
    /// </summary>
    public class DigitTerminalParser : TerminalParser
    {

 
        /// <summary>
        /// Returns true if an assembly's next element is a digit.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if an assembly's next element is a digit.</returns>
        public override bool Qualifies(object o)
        {
            return char.IsDigit((char)o);
        }
        /// <summary>
        /// Create a set with one random digit.
        /// <para>NOTE: Parameteres are ignored in this method</para>
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            char c = (char)(Random.Next(10) + '0');
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
            return "D";
        }
    }
}
