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

namespace Tripous.Tokenizing
{
    /**
 * The idea of a symbol is a character that stands on its 
 * own, such as an ampersand or a parenthesis. For example, 
 * when tokenizing the expression   
 *         (isReady) AND (isWilling),
     *  a typical tokenizer would return 7 
 * tokens, including one for each parenthesis and one for 
 * the ampersand. Thus a series of symbols such as 
 *    ) AND (  
 * becomes three tokens, while a series 
 * of letters such as 
 *   isReady
 * becomes a single word token.
 * 
 * Multi-character symbols are an exception to the rule 
 * that a symbol is a standalone character.  For example, a 
 * tokenizer may want less-than-or-equals to tokenize as a 
 * single token. This class provides a method for 
 * establishing which multi-character symbols an object of 
 * this class should treat as single symbols. This allows, 
 * for example, 
 *   "cat &lt;= dog"
 * to tokenize as 
 * three tokens, rather than splitting the less-than and 
 * equals symbols into separate tokens.
 
 * By default, this state recognizes the following multi-
 * character symbols: <code>!=, :-, &lt;=, &lt;=</code> 
 *
 *
 *
 *
 */
    public class SymbolState : TokenizerState
    {
        SymbolRootNode FSymbols = new SymbolRootNode();
        /**
         * Constructs a symbol state with a default idea of what 
         * multi-character symbols to accept (as described in the 
         * class comment).
         *
         * @return   a state for recognizing a symbol
         */
        public SymbolState()
        {
            Add("!=");
            Add(":-");
            Add("<=");
            Add(">=");
        }
        /**
         * Add a multi-character symbol.
         *
         * @param   string   the symbol to Add, such as "=:="
         */
        public void Add(string s)
        {
            FSymbols.Add(s);
        }
        /**
         * Return a symbol token from a reader.
         *
         * @return a symbol token from a reader
         */
        public override Token NextToken(System.IO.Stream r, int first, Tokenizer t)
        {
            string s = FSymbols.NextSymbol(r, first);
            return new Token(Token.TT_SYMBOL, s, 0);
        }
    }
}
