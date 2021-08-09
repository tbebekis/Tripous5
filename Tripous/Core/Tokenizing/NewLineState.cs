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
    /// A new line <see cref="TokenizerState"/>
    /// </summary>  
    public class NewLineState : TokenizerState
    {
        /// <summary>
        /// Constant. An integer value of the character \r (Carriage Return) in the ASCII table
        /// </summary>
        public const int CR = 13;   // carriage return  \r
        /// <summary>
        /// Constant. An integer value of the character \r (Line Feed) in the ASCII table
        /// </summary>
        public const int LF = 10;   // line feed        \n


        /*
                Token NextToken_Original(ITokenizer t, int c)
        {
            c = t.Read();
            if (c == '\r')
            {
                c = t.Read();
                if (c != '\n')
                    t.Unread(c);
            }
            else
            {
                t.Unread(c);   //t.unread(c);
            }

            return t.CreateToken(Token.TT_NEWLINE, " ", 0);
        } 
         */

        /// <summary>
        /// Return a token that represents a logical piece of a reader.
        /// </summary>
        /// <param name="t">the tokenizer and reader, conducting the overall tokenization</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <returns> Returns a token that represents a logical piece of the  reader</returns>
        public override Token NextToken(ITokenizer t, int c)
        {
            int LineIndex = t.CurrentLineIndex;
            int CharIndex = t.CurrentCharIndex;

            int c2 = t.Read();

            bool IsDoubleChar = (c == CR && c2 == LF) || (c == LF && c2 == CR);

            if (!IsDoubleChar)
            {
                t.UnreadSafe(c2);
            }

            return t.CreateToken(Token.TT_NEWLINE, " ", 0, LineIndex, CharIndex);
        }

    }
}
