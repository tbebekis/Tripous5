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
* A whitespace state ignores whitespace (such as blanks
* and tabs), and returns the tokenizer's next token. By 
* default, all characters from 0 to 32 are whitespace.
* 
*
*
*
*/
    public class WhitespaceState : TokenizerState
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool[] whitespaceChar = new bool[256];
        /**
        * Constructs a whitespace state with a default idea of what
        * characters are, in fact, whitespace.
        *
        * @return   a state for ignoring whitespace
        */
        public WhitespaceState()
        {
            SetWhitespaceChars(0, ' ', true);
            SetWhitespaceChars('\r', '\r', false);
            SetWhitespaceChars('\n', '\n', false);
        }
        /**
        * Ignore whitespace (such as blanks and tabs), and return 
        * the tokenizer's next token.
        *
        * @return the tokenizer's next token
        */
        public override Token NextToken(System.IO.Stream r, int aWhitespaceChar, Tokenizer t)
        {
            int i = 0;
            int c;
            do
            {
                c = r.ReadByte();
                i++;
            }
            while
               (
               c >= 0 &&
               c < whitespaceChar.Length &&
               whitespaceChar[c]
               );

            if (c >= 0)
            {
                r.Seek(-1, System.IO.SeekOrigin.Current);   //r.unread(c);
            }

            //return t.NextToken();
            return new Token(Token.TT_WHITESPACE, new string(' ', i), i);
        }
        /**
        * Establish the given characters as whitespace to ignore.
        *
        * @param   first   char
        *
        * @param   second   char
        *
        * @param   bool   true, if this state should ignore
        *                    characters in the given range
        */
        public void SetWhitespaceChars(int from, int to, bool b)
        {
            for (int i = from; i <= to; i++)
            {
                if (i >= 0 && i < whitespaceChar.Length)
                    whitespaceChar[i] = b;
            }
        }

    }
}
