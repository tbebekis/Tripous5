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

    /// <summary>
    /// A tokenizerState returns a token, given a reader, an  initial character ReadByte from the reader, 
    /// and a tokenizer that is conducting an overall tokenization of the reader.
    /// The tokenizer will typically have a character state table 
    /// that decides which state to use, depending on an initial character.
    /// If a single character is insufficient, a state such as <code>SlashState</code> 
    /// will ReadByte a second character, and may delegate to another state, 
    /// such as  <code>SlashStarState</code>.
    /// This prospect of delegation is  the reason that the <code>nextToken()</code> method has a tokenizer argument.
    /// </summary>
    public abstract class TokenizerState
    {
        /// <summary>
        /// Return a token that represents a logical piece of a reader.
        /// </summary>
        /// <param name="t">the tokenizer and reader, conducting the overall tokenization</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <returns> Returns a token that represents a logical piece of the  reader</returns>
        public abstract Token NextToken(ITokenizer t, int c);
    }

}
