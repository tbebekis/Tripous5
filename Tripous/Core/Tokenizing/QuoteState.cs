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
    /// A quote <see cref="TokenizerState"/>
    /// <para>
    ///  A quoteState returns a quoted string token from a reader. 
    ///  This state will collect characters until it sees a match
    ///  to the character that the tokenizer used to switch to 
    ///  this state. For example, if a tokenizer uses a double-
    ///  quote character to enter this state, then <code>
    ///  NextToken()</code> will search for another double-quote 
    ///  until it finds one or finds the end of the reader.
    /// </para>
    /// </summary>
    public class QuoteState : TokenizerState
    {
        /// <summary>
        /// 
        /// </summary>
        protected char[] CharBuf = new char[16];
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
        /// Return a quoted string token from a reader. This method 
        /// will collect characters until it sees a match to the
        /// character that the tokenizer used to switch to this 
        /// state.
        /// </summary>
        /// <returns>Returns a quoted string token from a reader</returns>
        public override Token NextToken(ITokenizer t, int cin)
        {
            int LineIndex = t.CurrentLineIndex;
            int CharIndex = t.CurrentCharIndex;

            int i = 0;
            CharBuf[i++] = Convert.ToChar(cin);
            int c;
            do
            {
                c = t.Read();
                if (c < 0)
                {
                    c = cin;
                }
                CheckBufLength(i);
                CharBuf[i++] = Convert.ToChar(c);
            } while (c != cin);

            string sval = new string(CharBuf, 0, i);   //   //string sval = string.copyValueOf(charbuf, 0, i);
            return t.CreateToken(Token.TT_QUOTED, sval, 0, LineIndex, CharIndex);
        }

        /*
        public Token nextToken(
            PushbackReader r, int cin, Tokenizer t)
            throws IOException {

            int i = 0;
            charbuf[i++] = (char) cin;
            int c;
            do {
                c = t.read();
                if (c < 0) {
                    c = cin;
                }
                checkBufLength(i);
                charbuf[i++] = (char) c;
            } while (c != cin);

            String sval = String.copyValueOf(charbuf, 0, i);
            return new Token(Token.TT_QUOTED, sval, 0);
        } 
         */
    }
}
