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
    /* 
* 
* 
* 
* 
* 
* 
*
*
*
*/


    /// <summary>
    /// A number <see cref="TokenizerState"/>
    /// <para>
    /// A NumberState object returns a number from a reader. 
    /// This state's idea of a number allows an optional, initial 
    /// minus sign, followed by one or more digits. A decimal 
    /// point and another string of digits may follow these 
    /// digits. 
    /// </para>
    /// </summary> 
    public class NumberState : TokenizerState
    {
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
        protected double AbsorbDigits(System.IO.Stream r, bool fraction)
        {

            int divideBy = 1;
            double v = 0;
            while ('0' <= c && c <= '9')
            {
                gotAdigit = true;
                v = v * 10 + (c - '0');
                c = r.ReadByte();
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
        /// <summary>
        /// Parse up to a decimal point.
        /// </summary>
        protected void ParseLeft(System.IO.Stream r)
        {

            if (c == '-')
            {
                c = r.ReadByte();
                absorbedLeadingMinus = true;
            }
            fValue = AbsorbDigits(r, false);
        }
        /// <summary>
        /// Parse from a decimal point to the end of the number.
        /// </summary>
        protected void ParseRight(System.IO.Stream r)
        {

            if (c == '.')
            {
                c = r.ReadByte();
                absorbedDot = true;
                fValue += AbsorbDigits(r, true);
            }
        }
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
        /// <summary>
        /// Put together the pieces of a number.
        /// </summary>
        protected Token Value(System.IO.Stream r, Tokenizer t)
        {

            if (!gotAdigit)
            {
                if (absorbedLeadingMinus && absorbedDot)
                {
                    r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread('.');
                    return t.SymbolState.NextToken(r, '-', t);
                }
                if (absorbedLeadingMinus)
                {
                    return t.SymbolState.NextToken(r, '-', t);
                }
                if (absorbedDot)
                {
                    return t.SymbolState.NextToken(r, '.', t);
                }
            }
            if (absorbedLeadingMinus)
            {
                fValue = -fValue;
            }
            //return new Token(Token.TT_NUMBER, "", Fvalue);
            return new Token(Token.TT_NUMBER, fValue.ToString(), fValue);
        }


        /// <summary>
        /// Return a number token from a reader.
        /// </summary>
        /// <param name="r">a reader to ReadByte from</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <param name="t">the tokenizer conducting the overall tokenization of the reader</param>
        /// <returns> a token that represents a logical piece of the  reader</returns>
        public override Token NextToken(System.IO.Stream r, int c, Tokenizer t)
        {
            Reset(c);
            ParseLeft(r);
            ParseRight(r);
            r.Seek(-1, System.IO.SeekOrigin.Current); // r.unread(c);
            return Value(r, t);
        }
    }
}
