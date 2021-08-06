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
    /// An assembly maintains a stream of language elements along with Stack and Target objects.
    /// <para>Parsers use assemblers to record progress at recognizing language elements from assembly's string.</para>
    /// <para>There are two types of assemblies: assemblies of tokens and assemblies of characters.</para>
    /// <para>To allow parsing text as strings of tokens, <see cref="Assembly"/> has two subclasses: <see cref="CharacterAssembly"/> and <see cref="TokenAssembly"/> </para>
    /// <para>Essentially, a <see cref="CharacterAssembly"/> object manipulates an array of characters, 
    /// and a <see cref="TokenAssembly"/> object manipulates an array of <see cref="Tripous.Tokenizing.Token"/> tokens.</para>
    /// </summary>
    public abstract class Assembly : ICloneable
    {

        /// <summary>
        /// a place to keep track of consumption progress
        /// </summary>
        protected Stack fStack = new Stack();
        /// <summary>
        /// Another place to record progress; this is just an object. 
        /// If a parser were recognizing an HTML page, for 
        /// example, it might create a Page object early, and store it 
        /// as an assembly's "FTarget". As its recognition of the HTML 
        /// progresses, it could use the FStack to build intermediate 
        /// results, like a heading, and then apply them to the FTarget 
        /// object.
        /// </summary>
        protected ICloneable FTarget;
        /// <summary>
        /// which element is next
        /// </summary>
        protected int FIndex = 0;

        /// <summary>
        /// Returns the content of the stack as a string.
        /// </summary>
        protected string StackToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.Append("[");
            object[] Items =  fStack.ToArray();
            for (int i = 0; i < Items.Length; i++)
            {
                SB.Append($"{Items[i]}");
                if (i < Items.Length - 1)
                    SB.Append(",");
            }
 
            SB.Append("]");
            return SB.ToString();
        }
 

        /* public */
        /// <summary>
        /// Returns a textual description of this assembly.
        /// </summary>
        public override string ToString()
        {
            string delimiter = DefaultDelimiter();
            string Result = StackToString() +           //fStack +
                            Consumed(delimiter) + 
                            "^" + 
                            Remainder(delimiter)
                            ;
            return Result;            
        }
        /// <summary>
        /// Returns the elements of the assembly that have been Consumed, separated by the specified delimiter.
        /// </summary>
        /// <param name="delimiter">the mark to show between Consumed elements</param>
        /// <returns>Returns the elements of the assembly that have been Consumed, separated by the specified delimiter.</returns>
        public abstract string Consumed(string delimiter);
        /// <summary>
        /// Returns the elements of the assembly that remain to be Consumed, separated by the specified delimiter.
        /// </summary>
        /// <param name="delimiter">the mark to show between unconsumed  elements</param>
        /// <returns>Returns the elements of the assembly that remain to be  Consumed, separated by the specified delimiter.</returns>
        public abstract string Remainder(string delimiter);

        /// <summary>
        /// Returns the number of elements in this assembly.
        /// </summary>
        public abstract int Length();

        /// <summary>
        /// Creates and returns a copy of this instance
        /// </summary>
        public virtual object Clone()
        {
            Assembly Result = base.MemberwiseClone() as Assembly;

            Result.fStack = Result.fStack.Clone() as Stack;

            if (FTarget != null)
                Result.FTarget = FTarget.Clone() as ICloneable;

            return Result;
        }
 
        /// <summary>
        /// Returns the default string to show between elements.
        /// <para>The DefaultDelimiter() method allows the Assembly subclasses to decide how to separate their elements.</para>
        /// </summary>
        public abstract string DefaultDelimiter();
 
        /// <summary>
        /// Returns true if this assembly has unconsumed elements.
        /// </summary>
        public bool HasMoreElements()
        {
            return ElementsConsumed() < Length();
        }
        /// <summary>
        /// Returns the next token from the associated token string.
        /// </summary>
        public abstract string NextElement();

        /// <summary>
        /// Returns the number of elements that have been Consumed.
        /// </summary>
        /// <returns></returns>
        public int ElementsConsumed()
        {
            return FIndex;
        }
        /// <summary>
        /// Returns the number of elements that have not been Consumed.
        /// </summary>
        public int ElementsRemaining()
        {
            return Length() - ElementsConsumed();
        }
 
        /// <summary>
        /// Returns the object identified as this assembly's "FTarget". 
        /// Clients can set and retrieve a FTarget, which can be a 
        /// convenient supplement as a place to work, in addition to 
        /// the assembly's FStack. For example, a parser for an 
        /// HTML file might use a web page object as its "FTarget". As 
        /// the parser recognizes markup commands like , it 
        /// could apply its findings to the FTarget.
        /// </summary>
        /// <returns>Returns the FTarget of this assembly</returns>
        public object GetTarget()
        {
            return FTarget;
        }
        /// <summary>
        /// Sets the FTarget for this assembly. Targets must implement 
        /// <code>Clone()</code> as a public method.
        /// </summary>
        /// <param name="FTarget">a publicly cloneable object</param>
        public void SetTarget(ICloneable FTarget)
        {
            this.FTarget = FTarget;
        }

        /// <summary>
        /// Removes this assembly's Stack.
        /// </summary>
        public Stack GetStack()
        {
            return fStack;
        }
        /// <summary>
        /// Returns true if this assembly's FStack is empty.
        /// </summary>
        public bool StackIsEmpty()
        {
            return fStack.Count == 0;
        }

        /// <summary>
        /// Returns the next object in the assembly, without removing it
        /// </summary>
        public abstract object Peek();
        /// <summary>
        /// Removes the object at the top of this assembly's FStack and returns it.
        /// </summary>
        public object Pop()
        {
            return fStack.Pop();
        }
        /// <summary>
        /// Pushes an object onto the top of this assembly's FStack. 
        /// </summary>
        public void Push(object o)
        {
            fStack.Push(o);
        }
 
        /// <summary>
        /// Put back n objects
        /// </summary>
        public void UnGet(int n)
        {
            FIndex -= n;
            if (FIndex < 0)
            {
                FIndex = 0;
            }
        }

    }
}
