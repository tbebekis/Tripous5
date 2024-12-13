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
    /// A Symbol matches a specific sequence, such as greater, or equal that a tokenizer
    /// </summary>
    public class SymbolTerminalParser : TerminalParser
    {
 
 
        /* construction */
        /// <summary>
        /// Constructs a Symbol that will Match the specified char.
        /// </summary>
        /// <param name="c">the character to Match. The char must be one that the tokenizer will return as a  Symbol token. This typically includes most  characters except letters and digits. </param>
        public SymbolTerminalParser(char c) 
            : this(c.ToString())
        {
        }
        /// <summary>
        /// Constructs a FSymbol that will Match the specified sequence of characters.
        /// </summary>
        /// <param name="s">the characters to Match. The characters must be a sequence that the tokenizer will return as a Symbol token, such as greater.</param>
        public SymbolTerminalParser(string s)
        {
            Symbol = s;
        }

        /* public */
        /// <summary>
        /// Returns true if the Symbol this object represents Equals an assembly's next element.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if the Symbol this object represents Equals an ssembly's next element.</returns>
        public override bool Qualifies(object o)
        {
            Token T = o as Token;
            return T != null && T.Kind == Token.TT_SYMBOL && Symbol.Equals(T.StringValue);
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            return Symbol;
        }

        /* properties */
        /// <summary>
        /// The symbol to match.
        /// </summary>
        public string Symbol { get; private set; }
    }
}
