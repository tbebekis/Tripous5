using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Tokenizing
{
    /// <summary>
    /// A buffered character reader
    /// </summary>
    public interface ICharReader
    {
        /// <summary>
        /// The character read, or -1 if the end of the stream has been reached
        /// </summary>
        int Read();

        /// <summary>
        /// Pushes back a single character by placint it to the current position of the buffer. 
        /// After this method returns, the next character to be read will have the value of the specified character.
        /// </summary>
        /// <param name="C">The int value representing a character to be pushed back</param>
        void Unread(int C);
        /// <summary>
        /// Pushes back a single character by placing it to the current position of the buffer. 
        /// <para>NOTE: The push-back is performed only if the specified character is greater than or equal to zero. </para>
        /// <para>After this method returns, the next character to be read will have the value of the specified character.</para>
        /// </summary>
        void UnreadSafe(int C);

        /* properties */
        /// <summary>
        /// The length of the internal buffer
        /// </summary>
        int Length { get; }
        /// <summary>
        /// The current position in the internal buffer. Essentially the next position to be read.  
        /// </summary>
        int Position { get; }
        /// <summary>
        /// Returns the text of the internal buffer.
        /// </summary>
        string Text { get; }
    }


    /// <summary>
    /// A buffered character reader.
    /// <para>NOTE: C# does not have a java.io.PushbackReader equivalent Steven J. Metsker uses. </para>
    /// </summary>
    public class CharReader: ICharReader
    {
        object syncLock = new LockObject();

        StringBuilder SB;
        int fPosition;

        /* construction */
        /// <summary>
        /// Constructor. Creates the character reader by placing a specified text in the internal buffer.
        /// </summary>
        /// <param name="Text">The text to place in the internal buffer.</param>
        public CharReader(string Text)
        {
            SB = new StringBuilder(Text);
        }
 
        /// <summary>
        /// The character read, or -1 if the end of the stream has been reached
        /// </summary>
        public int Read()
        {
            lock (syncLock)
            {
                if (fPosition > SB.Length - 1)
                {
                    return -1;
                }
                else
                {
                    int Result = Convert.ToInt32(SB[fPosition]);
                    fPosition++;
                    return Result;
                }
            }

        }
        /// <summary>
        /// Pushes back a single character by placint it to the current position of the buffer. 
        /// After this method returns, the next character to be read will have the value of the specified character.
        /// </summary>
        /// <param name="C">The int value representing a character to be pushed back</param>
        public void Unread(int C)
        {
            lock (syncLock)
            {
                if (fPosition == 0)
                    throw new ApplicationException("Character reader buffer overflow");
                fPosition--;
                SB[fPosition] = Convert.ToChar(C);
            }
        }
        /// <summary>
        /// Pushes back a single character by placing it to the current position of the buffer. 
        /// <para>NOTE: The push-back is performed only if the specified character is greater than or equal to zero. </para>
        /// <para>After this method returns, the next character to be read will have the value of the specified character.</para>
        /// </summary>
        public void UnreadSafe(int C)
        {
            lock (syncLock)
            {
                if (C >= 0)
                {
                    Unread(C);
                }
            }
        }
 
        /* properties */
        /// <summary>
        /// The length of the internal buffer
        /// </summary>
        public int Length => SB.Length;
        /// <summary>
        /// The current position in the internal buffer. Essentially the next position to be read.  
        /// </summary>
        public int Position => fPosition;
        /// <summary>
        /// Returns the text of the internal buffer.
        /// </summary>
        public string Text => SB.ToString();
    }
}
