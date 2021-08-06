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
    /// A slash <see cref="TokenizerState"/>
    /// <para>
    /// This state will either delegate to a comment-handling 
    /// state, or return a token with just a slash in it.
    /// </para>
    /// </summary>
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

 
        /// <summary>
        /// Returns either delegate to a comment-handling state, or return a  oken with just a slash in it.
        /// </summary>
        /// <returns>either just a slash token, or the results of  delegating to a comment-handling state</returns>
        public override Token NextToken(ICharReader r, int theSlash, Tokenizer t)
        {

            int c = r.Read();
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
                r.Unread(c);
            }
            return new Token(Token.TT_SYMBOL, "/", 0);
        }
    }
}
