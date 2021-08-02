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
 * A wordState returns a word from a reader. Like other 
 * states, a tokenizer transfers the job of reading to this 
 * state, depending on an initial character. Thus, the 
 * tokenizer decides which characters may begin a word, and 
 * this state determines which characters may appear as a 
 * second or later character in a word. These are typically 
 * different sets of characters; in particular, it is typical 
 * for digits to appear as parts of a word, but not as the 
 * initial character of a word. 
 *
 * By default, the following characters may appear in a word.
 * The method <code>SetWordChars()</code> allows customizing
 * this.
 * 
 *     From    To
 *      'a', 'z'
 *      'A', 'Z'
 *      '0', '9'
 *
 *     as well as: minus sign, underscore, and apostrophe.
 * 
 *
 *
 *
 *
 */
    public class WordState : TokenizerState
    {
        /// <summary>
        /// 
        /// </summary>
        protected byte[] Bytes = new byte[16];
        /// <summary>
        /// 
        /// </summary>
        protected bool[] FwordChar = new bool[256];
        /**
         * Constructs a word state with a default idea of what 
         * characters are admissible inside a word (as described in 
         * the class comment). 
         *
         * @return   a state for recognizing a word
         */
        public WordState()
        {
            SetWordChars('a', 'z', true);
            SetWordChars('A', 'Z', true);
            SetWordChars('0', '9', true);
            SetWordChars('-', '-', true);
            SetWordChars('_', '_', true);
            SetWordChars('\'', '\'', true);
            SetWordChars(0xc0, 0xff, true);
        }
        /**
         * Fatten up charbuf as necessary.
         */
        protected void CheckBufLength(int i)
        {
            if (i >= Bytes.Length)
            {
                byte[] nb = new byte[Bytes.Length * 2];
                System.Array.Copy(Bytes, 0, nb, 0, Bytes.Length);
                Bytes = nb;
            }
        }
        /**
         * Return a word token from a reader.
         *
         * @return a word token from a reader
         */
        public override Token NextToken(System.IO.Stream r, int c, Tokenizer t)
        {

            int i = 0;
            do
            {
                CheckBufLength(i);
                Bytes[i++] = (byte)c;
                c = r.ReadByte();
            } while (wordChar(c));

            if (c >= 0)
            {
                r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
            }

            char[] Chars = new char[Encoding.Default.GetCharCount(Bytes, 0, i)];
            Encoding.Default.GetChars(Bytes, 0, i, Chars, 0);

            string sval = new string(Chars);   //string sval = string.copyValueOf(charbuf, 0, i);

            return new Token(Token.TT_WORD, sval, 0);
        }
        /**
         * Establish characters in the given range as valid 
         * characters for part of a word after the first character. 
         * Note that the tokenizer must determine which characters
         * are valid as the beginning character of a word.
         *
         * @param   from   char
         *
         * @param   to   char
         *
         * @param   bool   true, if this state should allow
         *                    characters in the given range as part
         *                    of a word
         */
        public void SetWordChars(int from, int to, bool b)
        {
            for (int i = from; i <= to; i++)
            {
                if (i >= 0 && i < FwordChar.Length)
                {
                    FwordChar[i] = b;
                }
            }
        }
        /**
         * Just a test of the wordChar array.
         */
        protected bool wordChar(int c)
        {
            if (c >= 0 && c < FwordChar.Length)
            {
                return FwordChar[c];
            }
            return false;
        }
    }
}
