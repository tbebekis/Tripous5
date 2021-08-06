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
    /// A slash + start <see cref="TokenizerState"/>
    /// <para>A slashStar state ignores everything up to a closing star and slash, and then returns the tokenizer's next token.</para>
    /// </summary>
    public class SlashStarState : TokenizerState
    {
 
        /// <summary>
        ///  Ignore everything up to a closing star and slash, and  then return the tokenizer's next token.
        /// </summary>
        /// <returns>Returns the tokenizer's next token</returns>
        public override Token NextToken(ICharReader r, int theStar, Tokenizer t)
        {

            int c = 0;
            int lastc = 0;
            while (c >= 0)
            {
                if ((lastc == '*') && (c == '/'))
                {
                    break;
                }
                lastc = c;
                c = r.Read();
            }
            return t.NextToken();
        }
    }
}
