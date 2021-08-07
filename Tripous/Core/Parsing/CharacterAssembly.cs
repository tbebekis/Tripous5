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

 

namespace Tripous.Parsing
{
 
    /// <summary>
    /// A CharacterAssembly is an Assembly whose elements are  characters.
    /// </summary>
    public class CharacterAssembly : Assembly
    { 
        /* construction */
        /// <summary>
        /// Constructs a CharacterAssembly from the given string.
        /// </summary>
        /// <param name="Buffer">the string to consume</param>
        public CharacterAssembly(string Buffer)
        {
            this.Buffer = Buffer;
        }

        /* public */
        /// <summary>
        /// Returns a textual representation of the amount of this  characterAssembly that has been Consumed.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed  elements</param>
        /// <returns>Returns a textual representation of the amount of this  characterAssembly that has been Consumed.</returns>
        public override string Consumed(string delimiter)
        {
            if (delimiter.Equals(""))
                return Buffer.Substring(0, ElementsConsumed);

            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < ElementsConsumed; i++)
            {
                if (i > 0)
                    buf.Append(delimiter);

                buf.Append(Buffer[i]);
            }
            return buf.ToString();
        }
        /// <summary>
        /// Returns a textual representation of the amount of this  characterAssembly that remains to be Consumed.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed  elements</param>
        /// <returns>Returns a textual representation of the amount of this  characterAssembly that remains to be Consumed.</returns>
        public override string Remainder(string delimiter)
        {
            if (delimiter.Equals(""))
                return Buffer.Substring(ElementsConsumed);

            StringBuilder buf = new StringBuilder();
            for (int i = ElementsConsumed; i < Buffer.Length; i++)
            {
                if (i > ElementsConsumed)
                    buf.Append(delimiter);

                buf.Append(Buffer[i]);
            }
            return buf.ToString();
        }

        /// <summary>
        /// Returns the next character from the associated token  string
        /// </summary>
        public override string NextElement()
        {
            return Buffer[Index++].ToString(); //      string.charAt(index++)
        }
        /// <summary>
        /// Returns the next object in the assembly, without removing it
        /// </summary>
        public override object Peek()
        {
            if (Index < Length)
                return Buffer[Index];   //new Character(string.charAt(FIndex));
            else return null;
        }

        /* properties */
        /// <summary>
        /// Returns the default string to show between elements  Consumed or remaining.
        /// </summary>
        public override string DefaultDelimiter => ""; 
        /// <summary>
        /// Returns the number of elements in this assembly.
        /// </summary>
        public override int Length => Buffer.Length;
        /// <summary>
        /// The string to consume. It is passed in the constructor.
        /// </summary>
        public virtual string Buffer { get; protected set; }

    }
}
