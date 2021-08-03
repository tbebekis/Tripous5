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
    /// A Symbol matches a specific sequence, such as greater, or equal that a tokenizer
    /// </summary>
    public class SymbolTerminalParser : TerminalParser
    {
        /// <summary>
        /// the literal to Match
        /// </summary>
        protected Token FSymbol;
 
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
            FSymbol = new Token(Token.TT_SYMBOL, s, 0);
        }

        /* public */
        /// <summary>
        /// Returns true if the FSymbol this object represents Equals an ssembly's next element.
        /// </summary>
        /// <param name="o">an element from an assembly</param>
        /// <returns>Returns true if the FSymbol this object represents Equals an ssembly's next element.</returns>
        public override bool Qualifies(object o)
        {
            return FSymbol.Equals((Token)o);
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// </summary>
        /// <param name="visited">a list of parsers already printed in  this description</param>
        public override string UnvisitedString(ArrayList visited)
        {
            return FSymbol.ToString();
        }
    }
}
