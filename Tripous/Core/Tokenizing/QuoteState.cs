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
        protected byte[] Bytes = new byte[16];
        /// <summary>
        /// Fatten up charbuf as necessary.
        /// </summary>
        protected void CheckBufLength(int i)
        {
            if (i >= Bytes.Length)
            {
                byte[] nb = new byte[Bytes.Length * 2];
                System.Array.Copy(Bytes, 0, nb, 0, Bytes.Length);
                Bytes = nb;
            }
        }

        /// <summary>
        /// Return a quoted string token from a reader. This method 
        /// will collect characters until it sees a match to the
        /// character that the tokenizer used to switch to this 
        /// state.
        /// </summary>
        /// <returns>Returns a quoted string token from a reader</returns>
        public override Token NextToken(System.IO.Stream r, int cin, Tokenizer t)
        {

            int i = 0;
            Bytes[i++] = (byte)cin;
            int c;
            do
            {
                c = r.ReadByte();
                if (c < 0)
                {
                    c = cin;
                }
                CheckBufLength(i);
                Bytes[i++] = (byte)c;
            } while (c != cin);

            char[] Chars = new char[Encoding.Default.GetCharCount(Bytes, 0, i)];
            Encoding.Default.GetChars(Bytes, 0, i, Chars, 0);

            string sval = new string(Chars);   //string sval = string.copyValueOf(charbuf, 0, i);
            return new Token(Token.TT_QUOTED, sval, 0);
        }
    }
}
