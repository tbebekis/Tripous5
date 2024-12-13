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
    /// A parser that matches a specific string from an assembly.
    /// </summary>
    public class LiteralTerminalParser : TerminalParser
    {
 

        /* construction */
        /// <summary>
        /// Constructs a FLiteral that will Match the specified string.
        /// </summary>
        /// <param name="s">the string to Match as a token</param>
        public LiteralTerminalParser(string s)
        {
            LiteralValue = s;
            //FLiteral = new Token(s);
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
            return T != null && LiteralValue.Equals(T.StringValue);
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return LiteralValue;
        }

        /* properties */
        /// <summary>
        /// A string value against which this parser matches, i.e. the literal to match.
        /// </summary>
        public string LiteralValue { get; protected set; }
    }



}
