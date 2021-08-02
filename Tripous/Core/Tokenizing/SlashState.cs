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
 * This state will either delegate to a comment-handling 
 * state, or return a token with just a slash in it.
 * 
 *
 *
 *
 */
    public class SlashState : TokenizerState
    {
        /// <summary>
        /// 
        /// </summary>
        protected SlashStarState slashStarState = new SlashStarState();
        /// <summary>
        /// 
        /// </summary>
        protected SlashSlashState slashSlashState = new SlashSlashState();
        /**
         * Either delegate to a comment-handling state, or return a 
         * token with just a slash in it.
         *
         * @return   either just a slash token, or the results of 
         *           delegating to a comment-handling state
         */
        public override Token NextToken(System.IO.Stream r, int theSlash, Tokenizer t)
        {

            int c = r.ReadByte();
            if (c == '*')
            {
                return slashStarState.NextToken(r, '*', t);
            }
            if (c == '/')
            {
                return slashSlashState.NextToken(r, '/', t);
            }
            if (c >= 0)
            {
                r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
            }
            return new Token(Token.TT_SYMBOL, "/", 0);
        }
    }
}
