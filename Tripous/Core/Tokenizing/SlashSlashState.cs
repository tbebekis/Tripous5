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
 * A slashSlash state ignores everything up to an end-of-line
 * and returns the tokenizer's next token.
 * 
 *
 *
 *
 */
    public class SlashSlashState : TokenizerState
    {
        /**
         * Ignore everything up to an end-of-line and return the 
         * tokenizer's next token.
         *
         * @return the tokenizer's next token
         */
        public override Token NextToken(System.IO.Stream r, int theSlash, Tokenizer t)
        {

            int c;
            //while ((c = r.ReadByte()) != '\n' && c != '\r' && c >= 0) 
            while ((c = r.ReadByte()) >= 0)
            {
                if ("\n\r".IndexOf((char)c) != -1)
                {
                    r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
                    break;
                }
            }
            return t.NextToken();
        }
    }
}
