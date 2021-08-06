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
    /// A new line <see cref="TokenizerState"/>
    /// </summary>  
    public class NewLineState : TokenizerState
    {
        /// <summary>
        /// Return a token that represents a logical piece of a reader.
        /// </summary>
        /// <param name="r">a reader to ReadByte from</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <param name="t">the tokenizer conducting the overall tokenization of the reader</param>
        /// <returns> a token that represents a logical piece of the  reader</returns>
        public override Token NextToken(ICharReader r, int c, Tokenizer t)
        {
            c = r.Read();
            if (c == '\r')
            {
                c = r.Read();
                if (c != '\n')
                    r.Unread(c);
            }
            else
            {
                r.Unread(c);   //r.unread(c);
            }

            return new Token(Token.TT_NEWLINE, " ", 0);
        }

    }
}
