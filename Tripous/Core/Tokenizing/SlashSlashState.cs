﻿/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Tokenizing
{
 
    /// <summary>
    /// A double slash <see cref="TokenizerState"/>
    /// <para>A slashSlash state ignores everything up to an end-of-line and returns the tokenizer's next token.</para>
    /// </summary>
    public class SlashSlashState : TokenizerState
    {

        /// <summary>
        /// Ignore everything up to an end-of-line and return the  tokenizer's next token.
        /// </summary>
        /// <returns>Returns the tokenizer's next token</returns>
        public override Token NextToken(ITokenizer t, int theSlash) 
        {
            /* was..............
             
                int c;
                while ((c = t.read()) != '\n' && c != '\r' && c >= 0) {
                }
                return t.nextToken(); 
             */

            int c;
 
            while ((c = t.Read()) >= 0)
            {
                if ("\n\r".IndexOf((char)c) != -1)
                {
                    t.Unread(c);
                    break;
                }
            }
            return t.NextToken();
        }
    }
}
