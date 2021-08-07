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
    /// A number <see cref="TokenizerState"/>
    /// <para>
    /// A NumberState object returns a number from a reader. 
    /// This state's idea of a number allows an optional, initial minus sign, followed by one or more digits. 
    /// A decimal point and another string of digits may follow these digits. 
    /// </para>
    /// </summary> 
    public class NumberState : TokenizerState
    {
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int ZeroValue = (int)'0';
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int NineValue = (int)'9';
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int MinusValue = (int)'-';
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int DotValue = (int)'.';

        /// <summary>
        /// 
        /// </summary>
        protected int c;
        /// <summary>
        /// 
        /// </summary>
        protected double fValue;
        /// <summary>
        /// 
        /// </summary>
        protected bool absorbedLeadingMinus;
        /// <summary>
        /// 
        /// </summary>
        protected bool absorbedDot;
        /// <summary>
        /// 
        /// </summary>
        protected bool gotAdigit;

        /// <summary>
        /// Convert a stream of digits into a number, making this  number a fraction if the bool parameter is true.
        /// </summary>
        protected double AbsorbDigits(ICharReader r, bool fraction)
        {

            int divideBy = 1;
            double v = 0;
            while (ZeroValue <= c && c <= NineValue)
            {
                gotAdigit = true;
                v = v * 10 + (c - ZeroValue);
                c = r.Read();
                if (fraction)
                {
                    divideBy *= 10;
                }
            }
            if (fraction)
            {
                v = v / divideBy;
            }
            return v;
        }

        /*
        protected double absorbDigits(
            PushbackReader r, boolean fraction) throws IOException {

            int divideBy = 1;
            double v = 0;
            while ('0' <= c && c <= '9') {
                gotAdigit = true;
                v = v * 10 + (c - '0');
                c = r.read();
                if (fraction) {
                    divideBy *= 10;
                }
            }
            if (fraction) {
                v = v / divideBy;
            }
            return v;
        } 
         */

        /// <summary>
        /// Parse up to a decimal point.
        /// </summary>
        protected void ParseLeft(ICharReader r)
        {
            if (c == MinusValue)
            {
                c = r.Read();
                absorbedLeadingMinus = true;
            }
            fValue = AbsorbDigits(r, false);
        }

        /*
        protected void parseLeft(PushbackReader r)
            throws IOException {

            if (c == '-') {
                c = r.read();
                absorbedLeadingMinus = true;
            }
            value = absorbDigits(r, false);	 
        } 
         */

        /// <summary>
        /// Parse from a decimal point to the end of the number.
        /// </summary>
        protected void ParseRight(ICharReader r)
        {
            if (c == DotValue)
            {
                c = r.Read();
                absorbedDot = true;
                fValue += AbsorbDigits(r, true);
            }
        }
        /*
        protected void parseRight(PushbackReader r)
            throws IOException {

            if (c == '.') {
                c = r.read();
                absorbedDot = true;
                value += absorbDigits(r, true);
            }
        } 
         */
        /// <summary>
        /// Prepare to assemble a new number.
        /// </summary>
        protected void Reset(int cin)
        {
            c = cin;
            fValue = 0;
            absorbedLeadingMinus = false;
            absorbedDot = false;
            gotAdigit = false;
        }
        /*
        protected void reset(int cin) {
            c = cin;
            value = 0;
            absorbedLeadingMinus = false;
            absorbedDot = false;
            gotAdigit = false;
        } 
         */
        /// <summary>
        /// Put together the pieces of a number.
        /// </summary>
        protected Token Value(ICharReader r, Tokenizer t)
        {
            if (!gotAdigit)
            {
                if (absorbedLeadingMinus && absorbedDot)
                {
                    r.Unread(DotValue);
                    return t.SymbolState.NextToken(r, MinusValue, t);
                }
                if (absorbedLeadingMinus)
                {
                    return t.SymbolState.NextToken(r, MinusValue, t);
                }
                if (absorbedDot)
                {
                    return t.SymbolState.NextToken(r, DotValue, t);
                }
            }
            if (absorbedLeadingMinus)
            {
                fValue = -fValue;
            }
            return new Token(Token.TT_NUMBER, "", fValue);
            //return new Token(Token.TT_NUMBER, fValue.ToString(), fValue);
        }
        /*
        protected Token value(PushbackReader r, Tokenizer t)  throws IOException {

            if (!gotAdigit) {
                if (absorbedLeadingMinus && absorbedDot) {
                    r.unread('.');
                    return t.symbolState().nextToken(r, '-', t);
                    }
                if (absorbedLeadingMinus) {
                    return t.symbolState().nextToken(r, '-', t);
                }
                if (absorbedDot) {
                    return t.symbolState().nextToken(r, '.', t);
                }
            }
            if (absorbedLeadingMinus) {
                value = -value;
            }
            return new Token(Token.TT_NUMBER, "", value);
        } 
         */
        /// <summary>
        /// Return a number token from a reader.
        /// </summary>
        /// <param name="r">a reader to ReadByte from</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <param name="t">the tokenizer conducting the overall tokenization of the reader</param>
        /// <returns> a token that represents a logical piece of the  reader</returns>
        public override Token NextToken(ICharReader r, int c, Tokenizer t)
        {
            Reset(c);
            ParseLeft(r);
            ParseRight(r);
            //r.Unread(c);  // why this unread here?
            return Value(r, t);
        }
        /*
        public Token nextToken(PushbackReader r, int cin, Tokenizer t) throws IOException {
            reset(cin);	
            parseLeft(r);
            parseRight(r);
            r.unread(c);
            return value(r, t);
        } 
         */
    }
}
