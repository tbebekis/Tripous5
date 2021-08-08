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
        public const int CR = 13;   // \r   carriage return
        /// <summary>
        /// Constant. An integer value of the character \r (Line Feed) in the ASCII table
        /// </summary>
        public const int LF = 10;   // \n   line feed 

        Token NextToken_Original(ICharReader r, int c, Tokenizer t)
        {
            c = r.Read();
            if (c == '\r')
            {
                c = r.Read();
                if (c != '\n')
                    r.Unread(c);
            }
            else
            {
                r.Unread(c);   //r.unread(c);
            }

            return new Token(Token.TT_NEWLINE, " ", 0);
        }
        /// <summary>
        /// Return a token that represents a logical piece of a reader.
        /// </summary>
        /// <param name="r">a reader to ReadByte from</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <param name="t">the tokenizer conducting the overall tokenization of the reader</param>
        /// <returns> a token that represents a logical piece of the  reader</returns>
        public override Token NextToken(ICharReader r, int c, Tokenizer t)
        {
            c = r.Read();
            int c2 = r.Read();

            bool IsDoubleChar = (c == CR && c2 == LF) || (c == LF && c2 == CR);

            if (!IsDoubleChar)
            {
                r.UnreadSafe(c2);
            }

            return new Token(Token.TT_NEWLINE, " ", 0);
        }

    }
}
