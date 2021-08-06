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
    /// <para>A white space <see cref="TokenizerState"/></para>
    /// <para>
    /// A whitespace state ignores whitespace (such as blanks
    /// and tabs), and returns the tokenizer's next token. By 
    /// default, all characters from 0 to 32 are whitespace.
    /// </para>
    /// </summary>
    public class WhitespaceState : TokenizerState
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool[] whitespaceChar = new bool[256];
 
        /// <summary>
        /// Constructs a whitespace state with a default idea of what characters are, in fact, whitespace.
        /// </summary>
        public WhitespaceState()
        {
            SetWhitespaceChars(0, ' ', true);
            SetWhitespaceChars('\r', '\r', false);
            SetWhitespaceChars('\n', '\n', false);
        }
 
        /// <summary>
        /// Ignores whitespace (such as blanks and tabs), and returns  the tokenizer's next token.
        /// </summary>
        public override Token NextToken(ICharReader r, int aWhitespaceChar, Tokenizer t)
        {
            int i = 0;
            int c;
            do
            {
                c = r.Read();
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
                r.Unread(c);
            }

            return t.NextToken();
            //return new Token(Token.TT_WHITESPACE, new string(' ', i), i);
        }
 
        /// <summary>
        /// Establish the given characters as whitespace to ignore.
        /// </summary>
        /// <param name="from">char</param>
        /// <param name="to">char</param>
        /// <param name="b">true, if this state should ignore characters in the given range</param>
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
