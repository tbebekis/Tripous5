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

using Tripous.Tokenizing;

namespace Tripous.Parsing
{
    /**
 * A CharacterAssembly is an Assembly whose elements are 
 * characters.
 * 
 *
 * 
 *
 * 
 * @see Assembly
 */

    public class CharacterAssembly : Assembly
    {
        /**
         * the string to consume
         */
        protected string FBuffer;

        /**
         * Constructs a CharacterAssembly from the given string.
         *
         * @param   string   the string to consume
         *
         * @return   a CharacterAssembly that will consume the 
         *           supplied string
         */
        public CharacterAssembly(string Buffer)
        {
            this.FBuffer = Buffer;
        }
        /**
         * Returns a textual representation of the amount of this 
         * characterAssembly that has been Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that has been Consumed
         */
        public override string Consumed(string delimiter)
        {
            if (delimiter.Equals(""))
                return FBuffer.Substring(0, ElementsConsumed());

            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < ElementsConsumed(); i++)
            {
                if (i > 0)
                    buf.Append(delimiter);

                buf.Append(FBuffer[i]);
            }
            return buf.ToString();
        }
        /**
         * Returns the default string to show between elements 
         * Consumed or remaining.
         *
         * @return   the default string to show between elements 
         *           Consumed or remaining
         */
        public override string DefaultDelimiter()
        {
            return "";
        }
        /**
         * Returns the number of elements in this assembly.
         *
         * @return   the number of elements in this assembly
         */
        public override int Length()
        {
            return FBuffer.Length;
        }
        /**
         * Returns the next character.
         *
         * @return   the next character from the associated token 
         *           string
         *
         * @exception  ArrayIndexOutOfBoundsException  if there are 
         *             no more characters in this assembly's string
         */
        public override string NextElement()
        {
            return FBuffer[FIndex++].ToString(); //      string.charAt(index++)
        }
        /**
         * Shows the next object in the assembly, without removing it
         *
         * @return   the next object
         */
        public override object Peek()
        {
            if (FIndex < Length())
                return FBuffer[FIndex];   //new Character(string.charAt(FIndex));
            else return null;
        }
        /**
         * Returns a textual representation of the amount of this 
         * characterAssembly that remains to be Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that remains to be Consumed
         */
        public override string Remainder(string delimiter)
        {
            if (delimiter.Equals(""))
                return FBuffer.Substring(ElementsConsumed());

            StringBuilder buf = new StringBuilder();
            for (int i = ElementsConsumed(); i < FBuffer.Length; i++)
            {

                if (i > ElementsConsumed())
                    buf.Append(delimiter);

                buf.Append(FBuffer[i]);
            }
            return buf.ToString();
        }
        /**
        */
        public override object Clone()
        {
            return CloneProperties(new CharacterAssembly(FBuffer));
        }


    }
}
