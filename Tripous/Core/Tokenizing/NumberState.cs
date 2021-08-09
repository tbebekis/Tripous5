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
        public const int Zero = (int)'0';
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int Nine = (int)'9';
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int Minus = (int)'-';
        /// <summary>
        /// Constant. A numeric value of a character in the ASCII table
        /// </summary>
        public const int Dot = (int)'.';

        /// <summary>
        /// 
        /// </summary>
        protected int c;

        /* Steve Metsker code - many problems          

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
            int ctemp;

            while (true)
            {
                if (IsDigit(c))
                {
                    gotAdigit = true;
                    v = v * 10 + (c - Zero);                    
                    if (fraction)
                    {
                        divideBy *= 10;
                    }
                }

                ctemp = t.Read();
                if (!IsDigit(ctemp))
                {  
                    t.Unread(ctemp);
                    break;
                }
                else
                {
                    c = ctemp;
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
        protected void ParseLeft(ICharReader r)
        {
            if (c == Minus)
            {
                c = t.Read();
                absorbedLeadingMinus = true;
            }
            fValue = AbsorbDigits(r, false);
        }
        /// <summary>
        /// Parse from a decimal point to the end of the number.
        /// </summary>
        protected void ParseRight(ICharReader r)
        {
            if (c == Dot)
            {
                c = t.Read();
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
        protected Token Value(ICharReader r, Tokenizer t)
        {
            if (!gotAdigit)
            {
                if (absorbedLeadingMinus && absorbedDot)
                {
                    t.Unread(Dot);
                    return t.SymbolState.NextToken(r, Minus, t);
                }
                if (absorbedLeadingMinus)
                {
                    return t.SymbolState.NextToken(r, Minus, t);
                }
                if (absorbedDot)
                {
                    return t.SymbolState.NextToken(r, Dot, t);
                }
            }
            if (absorbedLeadingMinus)
            {
                fValue = -fValue;
            }
            return new Token(Token.TT_NUMBER, "", fValue);
        }
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
            //t.Unread(c);  // why this unread here?
            return Value(r, t);
        }
        */

        static bool IsDigit(int C)
        {
            return C >= Zero && C <= Nine;
        }
        bool TryParse(string S, out double V)
        {
            bool Result = false;
            V = 0;
            try
            {
                V = double.Parse(S, System.Globalization.CultureInfo.InvariantCulture);
                Result = true;
            }
            catch  
            {                
            }

            return Result;
        }
        Token ConstructResult(ITokenizer t, bool LeadingMinusFound, bool DotFound, StringBuilder Integers, StringBuilder Decimals)
        {
            int LineIndex = t.CurrentLineIndex;
            int CharIndex = t.CurrentCharIndex;

            StringBuilder SB = new StringBuilder();

            if (LeadingMinusFound)
                SB.Append('-');

            SB.Append(Integers.ToString());

            if (DotFound)
            {
                SB.Append('.');
                SB.Append(Decimals.ToString());
            }

            string S = SB.ToString();
            double V;
            if (!TryParse(S, out V))
            {
                throw new ApplicationException($"Error parsing number. Cannot convert string to number: {SB}");
            }
            else
            {
                return t.CreateToken(Token.TT_NUMBER, "", V, LineIndex, CharIndex);
            }
        }
        /// <summary>
        /// Return a token that represents a logical piece of a reader.
        /// </summary>
        /// <param name="t">the tokenizer and reader, conducting the overall tokenization</param>
        /// <param name="c">the character that a tokenizer used to  determine to use this state</param>
        /// <returns> Returns a token that represents a logical piece of the  reader</returns>
        public override Token NextToken(ITokenizer t, int c)
        {
            bool LeadingMinusFound = false;
            bool DotFound = false; 

            StringBuilder Integers = new StringBuilder();
            StringBuilder Decimals = new StringBuilder();
            StringBuilder SB = new StringBuilder();
            char C;            

            if (c == Minus)
            {
                LeadingMinusFound = true;
                c = t.Read();
                SB.Append('-');
            }

            while (true)
            {
                if (IsDigit(c))
                {
                    C = Convert.ToChar(c);

                    if (!DotFound)
                        Integers.Append(C);
                    else
                        Decimals.Append(C);

                    SB.Append(C);
                }
                else if (c == Dot)
                {
                    if (DotFound)
                    {
                        throw new ApplicationException($"There is already a decimal separator in number: {SB}");
                    }
                    else if (LeadingMinusFound && Integers.Length == 0)
                    {
                        t.Unread(Dot);
                        t.Unread(Minus);
                        return t.SymbolState.NextToken(t, Minus);
                    }
                    else
                    {
                        int temp = t.Read();
                        if (IsDigit(temp))
                        {
                            DotFound = true;
                            SB.Append('.');
                            t.UnreadSafe(temp);
                        }
                        else
                        {
                            t.Unread(Dot);
                            t.UnreadSafe(temp);
                            return ConstructResult(t, LeadingMinusFound, DotFound, Integers, Decimals);
                        }                     
                    }                 
                }

                c = t.Read();

                if (!IsDigit(c) && c != Minus && c != Dot)
                {
                    t.UnreadSafe(c);
                    return ConstructResult(t, LeadingMinusFound, DotFound, Integers, Decimals);
                }

            }
        }

    }
}
