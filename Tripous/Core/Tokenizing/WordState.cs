/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Tokenizing
{


    /// <summary>
    /// <para>A word <see cref="TokenizerState"/> </para>
    /// <para>
    /// The decision that a character begins a word is the Tokenizer class's decision. The decision that a
    /// character belongs inside a word is the WordState class's decision. 
    /// </para>
    /// <para> 
    /// If you want to customize the characters that can make a word, 
    /// you may need to customize the Tokenizer object, 
    /// and you may need to customize the WordState object that the Tokenizer uses.
    /// </para>
    /// <para>
    /// To change how words may begin, update the Tokenizer object with a call to setCharacterState().
    /// To change the characters that may appear inside a word, retrieve the Tokenizer object's WordState
    /// object with a call to wordState(), and update the WordState object with a call to setWordChars().
    /// </para>
    /// <para>
    /// A wordState returns a word from a reader. Like other 
    /// states, a tokenizer transfers the job of reading to this 
    /// state, depending on an initial character. Thus, the 
    /// tokenizer decides which characters may begin a word, and 
    /// this state determines which characters may appear as a 
    /// second or later character in a word. These are typically 
    /// different sets of characters; in particular, it is typical 
    /// for digits to appear as parts of a word, but not as the 
    /// initial character of a word. 
    /// </para>
    /// <para>
    /// By default, the following characters may appear in a word.
    /// The method <code>SetWordChars()</code> allows customizing
    /// this.
    /// </para>
    /// <para>
    /// <code>
    /// 
    ///     From    To
    ///      'a', 'z'
    ///      'A', 'Z'
    ///      '0', '9'
    /// 
    /// </code>
    /// </para>
    /// <para>as well as: minus sign, underscore, and apostrophe.</para>
    /// </summary>
    public class WordState : TokenizerState
    {
        /// <summary>
        /// 
        /// </summary>
        protected char[] CharBuf = new char[16];
        /// <summary>
        /// 
        /// </summary>
        protected bool[] fWordChar = new bool[256];

        /// <summary>
        /// Fatten up charbuf as necessary.
        /// </summary>
        protected void CheckBufLength(int i)
        {
            if (i >= CharBuf.Length)
            {
                char[] nb = new char[CharBuf.Length * 2];
                System.Array.Copy(CharBuf, 0, nb, 0, CharBuf.Length);
                CharBuf = nb;
            }
        }
        /// <summary>
        /// Just a test of the wordChar array.
        /// </summary>
        protected bool WordChar(int c)
        {
            if (c >= 0 && c < fWordChar.Length)
            {
                return fWordChar[c];
            }
            return false;
        }

        /* construction */
        /// <summary>
        /// Constructs a word state with a default idea of what 
        /// characters are admissible inside a word (as described in 
        /// the class comment). 
        /// </summary>
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
 
        /* public */
        /// <summary>
        /// Return a word token from a reader.
        /// </summary>
        public override Token NextToken(ITokenizer t, int c)
        {
            int LineIndex = t.CurrentLineIndex;
            int CharIndex = t.CurrentCharIndex;

            int i = 0;
            do
            {
                CheckBufLength(i);
                CharBuf[i++] = Convert.ToChar(c);
                c = t.Read();
            } while (WordChar(c));

            if (c >= 0)
            {
                t.Unread(c);
            }
 
            string sval = new string(CharBuf, 0, i);    
            return t.CreateToken(Token.TT_WORD, sval, 0, LineIndex, CharIndex);
        }
        /// <summary>
        /// Establish characters in the given range as valid 
        /// characters for part of a word after the first character. 
        /// Note that the tokenizer must determine which characters
        /// are valid as the beginning character of a word.
        /// </summary>
        /// <param name="from">char</param>
        /// <param name="to">char</param>
        /// <param name="b">true, if this state should allow characters in the given range as part of a word</param>
        public void SetWordChars(int from, int to, bool b)
        {
            for (int i = from; i <= to; i++)
            {
                if (i >= 0 && i < fWordChar.Length)
                {
                    fWordChar[i] = b;
                }
            }
        }
    }
}
