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
using System.Linq;
using System.Collections.Generic;

namespace Tripous.Parsing
{


    /// <summary>
    /// An assembly maintains a stream of language elements (strings or characters) along with Stack and Target objects.
    /// <para>Parsers use <see cref="Assembler"/> assemblers to record progress at recognizing language elements from assembly's string.</para>
    /// <para>There are two types of assemblies: assemblies of tokens and assemblies of characters.</para>
    /// <para>To allow parsing text as strings of tokens, <see cref="Assembly"/> has two subclasses: <see cref="CharacterAssembly"/> and <see cref="TokenAssembly"/> </para>
    /// <para>Essentially, a <see cref="CharacterAssembly"/> object manipulates an array of characters, 
    /// and a <see cref="TokenAssembly"/> object manipulates an array of <see cref="Tripous.Tokenizing.Token"/> tokens.</para>
    /// <para>NOTE: The enumeration of this object consumes its elements since it calls the <see cref="NextElement"/>() method </para>
    /// </summary>
    public abstract class Assembly : ICloneable, IEnumerable<string>
    {
        /// <summary>
        /// <see cref="IEnumerator"/> implementation
        /// </summary>
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            while (HasMoreElements)  
            {
                yield return NextElement();
            }
        }
        /// <summary>
        /// <see cref="IEnumerator"/> implementation
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<string>).GetEnumerator();
        }
 
        /* public */
        /// <summary>
        /// Returns a textual description of this assembly.
        /// </summary>
        public override string ToString()
        {
            string delimiter = DefaultDelimiter;
            string Result = StackToString() +           //fStack +
                            Consumed(delimiter) + 
                            "^" + 
                            Remainder(delimiter)
                            ;
            return Result;            
        }
        /// <summary>
        /// Returns the content of the stack as a string.
        /// </summary>
        public string StackToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.Append("[");
            object[] Items = Stack.ToArray().Reverse().ToArray();
            for (int i = 0; i < Items.Length; i++)
            {
                SB.Append($"{Items[i]}");
                if (i < Items.Length - 1)
                    SB.Append(", ");
            }

            SB.Append("]");
            return SB.ToString();
        }
        /// <summary>
        /// Creates and returns a copy of this instance
        /// </summary>
        public virtual object Clone()
        {
            Assembly Result = base.MemberwiseClone() as Assembly;

            Result.Stack = Result.Stack.Clone() as Stack;

            if (Target != null)
                Result.Target = Target.Clone() as ICloneable;

            return Result;
        }

        /// <summary>
        /// Returns the elements of the assembly that have been "consumed", separated by the specified delimiter.
        /// </summary>
        /// <param name="delimiter">the mark to show between "consumed" elements</param>
        /// <returns>Returns the elements of the assembly that have been "consumed", separated by the specified delimiter.</returns>
        public abstract string Consumed(string delimiter);
        /// <summary>
        /// Returns the elements of the assembly that remain to be Consumed, separated by the specified delimiter.
        /// </summary>
        /// <param name="delimiter">the mark to show between unconsumed  elements</param>
        /// <returns>Returns the elements of the assembly that remain to be  Consumed, separated by the specified delimiter.</returns>
        public abstract string Remainder(string delimiter);

        /// <summary>
        /// Returns the next token from the associated token string and advances the internal element index (position).
        /// <para>In a sense it "consumes" the element.</para>
        /// </summary>
        public abstract string NextElement();


        /// <summary>
        /// Returns the next object in the assembly, without removing it
        /// </summary>
        public abstract object Peek();
        /// <summary>
        /// Removes the object at the top of this assembly's FStack and returns it.
        /// </summary>
        public object Pop()
        {
            return Stack.Pop();
        }
        /// <summary>
        /// Pushes an object onto the top of this assembly's FStack. 
        /// </summary>
        public void Push(object o)
        {
            Stack.Push(o);
        } 
        /// <summary>
        /// Put back n objects
        /// </summary>
        public void UnGet(int n)
        {
            Index -= n;
            if (Index < 0)
            {
                Index = 0;
            }
        }

        /* properties */
        /// <summary>
        /// The index of the next element
        /// </summary>
        public int Index { get; protected set; }
        /// <summary>
        /// Returns the number of elements in this assembly.
        /// </summary>
        public abstract int Length { get; }
        /// <summary>
        /// Returns the number of elements that have been Consumed.
        /// </summary>
        /// <returns></returns>
        public int ElementsConsumed => Index;
        /// <summary>
        /// Returns the number of elements that have not been Consumed.
        /// </summary>
        public int ElementsRemaining => Length - ElementsConsumed;
        /// <summary>
        /// Returns true if this assembly has unconsumed elements.
        /// </summary>
        public bool HasMoreElements => ElementsConsumed < Length; 
        /// <summary>
        /// Returns the default string to show between elements.
        /// <para>The DefaultDelimiter method allows the Assembly subclasses to decide how to separate their elements.</para>
        /// </summary>
        public abstract string DefaultDelimiter { get; }
        /// <summary>
        /// The "target" of this assembly.
        /// <para>
        /// Clients can set and retrieve a target, which can be a convenient supplement as a place to work, 
        /// in addition to the assembly's Stack. 
        /// </para>
        /// <para>
        /// For example, a parser for an HTML file might use a Web Page object as its "target". 
        /// As the parser recognizes markup commands, it could apply its findings to the target.
        /// The parser might create a Page object early, and store it as an assembly's "target". 
        /// As its recognition of the HTML  progresses, it could use the stack to build intermediate results, 
        /// like a heading, and then apply them to the target object.
        /// </para>
        /// <para>NOTE: Targets must be <see cref="ICloneable"/> objects.</para>
        /// </summary>
        public ICloneable Target { get; set; }
        /// <summary>
        /// A stack to keep track of consumption progress
        /// </summary>
        public Stack Stack { get; private set; } = new Stack();
        /// <summary>
        /// Returns true if stack is empty.
        /// </summary>
        public bool IsStackEmpty => Stack.Count == 0;
 
    }
}
