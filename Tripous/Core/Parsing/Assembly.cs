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
    /**
 * An assembly maintains a stream of language elements along 
 * with FStack and FTarget objects.
 *
 * Parsers use assemblers to record progress at 
 * recognizing language elements from assembly's string. 
 * 
 *
 * 
 *
 * 
 */
    public abstract class Assembly : ICloneable
    {

        /**
         * a place to keep track of consumption progress
         */
        protected Stack FStack = new Stack();

        /** Another place to record progress; this is just an object. 
         * If a parser were recognizing an HTML page, for 
         * example, it might create a Page object early, and store it 
         * as an assembly's "FTarget". As its recognition of the HTML 
         * progresses, it could use the FStack to build intermediate 
         * results, like a heading, and then apply them to the FTarget 
         * object.
         */
        protected ICloneable FTarget;

        /**
         * which element is next
         */
        protected int FIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        protected virtual Assembly CloneProperties(Assembly A)
        {
            try
            {
                //deep clone the FStack
                object[] StackElements = FStack.ToArray();
                Array.Reverse(StackElements);

                for (int i = 0; i < StackElements.Length; i++)
                    if (StackElements[i] is ICloneable)
                        A.FStack.Push(((ICloneable)StackElements[i]).Clone());
                    else A.FStack.Push(StackElements[i]);

                if (FTarget != null)
                    A.FTarget = (ICloneable)FTarget.Clone();

                return A;
            }
            catch
            {
                throw new Exception("CloneProperties() failed");
            }
        }


        /**
         * Return a copy of this object.
         *
         * @return a copy of this object
         */
        public abstract object Clone();

        /**
         * Returns the elements of the assembly that have been 
         * Consumed, separated by the specified delimiter.
         *
         * @param   string   the mark to show between Consumed
         *                   elements
         *
         * @return   the elements of the assembly that have been 
         *           Consumed
         */
        public abstract string Consumed(string delimiter);
        /**
         * Returns the default string to show between elements.
         *
         * @return   the default string to show between elements
         */
        public abstract string DefaultDelimiter();
        /**
         * Returns the number of elements that have been Consumed.
         *
         * @return   the number of elements that have been Consumed
         */
        public int ElementsConsumed()
        {
            return FIndex;
        }
        /**
         * Returns the number of elements that have not been Consumed.
         *
         * @return   the number of elements that have not been 
         *           Consumed
         */
        public int ElementsRemaining()
        {
            return Length() - ElementsConsumed();
        }
        /**
         * Removes this assembly's FStack.
         *
         * @return   this assembly's FStack
         */
        public Stack GetStack()
        {
            return FStack;
        }
        /**
         * Returns the object identified as this assembly's "FTarget". 
         * Clients can set and retrieve a FTarget, which can be a 
         * convenient supplement as a place to work, in addition to 
         * the assembly's FStack. For example, a parser for an 
         * HTML file might use a web page object as its "FTarget". As 
         * the parser recognizes markup commands like , it 
         * could apply its findings to the FTarget.
         * 
         * @return   the FTarget of this assembly
         */
        public object GetTarget()
        {
            return FTarget;
        }
        /**
         * Returns true if this assembly has unconsumed elements.
         *
         * @return   true, if this assembly has unconsumed elements
         */
        public bool HasMoreElements()
        {
            return ElementsConsumed() < Length();
        }
        /**
         * Returns the number of elements in this assembly.
         *
         * @return   the number of elements in this assembly
         */
        public abstract int Length();
        /**
         * Shows the next object in the assembly, without removing it
         *
         * @return   the next object
         *
         */
        public abstract object Peek();
        /**
         * Removes the object at the top of this assembly's FStack and
         * returns it.
         *
         * @return   the object at the top of this assembly's FStack
         *
         * @exception   EmptyStackException   if this FStack is empty
         */
        public object Pop()
        {
            return FStack.Pop();
        }
        /**
         * Pushes an object onto the top of this assembly's FStack. 
         *
         * @param   object   the object to be pushed
         */
        public void Push(object o)
        {
            FStack.Push(o);
        }
        /**
         * Returns the elements of the assembly that remain to be 
         * Consumed, separated by the specified delimiter.
         *
         * @param   string   the mark to show between unconsumed 
         *                   elements
         *
         * @return   the elements of the assembly that remain to be 
         *           Consumed
         */
        public abstract string Remainder(string delimiter);
        /**
         * Sets the FTarget for this assembly. Targets must implement 
         * <code>Clone()</code> as a public method.
         * 
         * @param   FTarget   a publicly cloneable object
         */
        public void SetTarget(ICloneable FTarget)
        {
            this.FTarget = FTarget;
        }
        /**
         * Returns true if this assembly's FStack is empty.
         *
         * @return   true, if this assembly's FStack is empty
         */
        public bool StackIsEmpty()
        {
            return FStack.Count == 0;
        }
        /**
         * Returns a textual description of this assembly.
         *
         * @return   a textual description of this assembly
         */
        public override string ToString()
        {
            string delimiter = DefaultDelimiter();
            return FStack +
               Consumed(delimiter) + "^" + Remainder(delimiter);
        }
        /**
         * Put back n objects
         *
         */
        public void UnGet(int n)
        {
            FIndex -= n;
            if (FIndex < 0)
            {
                FIndex = 0;
            }
        }
        /** ëåßðåé áðü ôïõ Metsker */
        public abstract string NextElement();
    }
}
