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
    /// <para>
    /// Parsers that have an Assembler ask it to work on an
    /// assembly after a successful match.
    /// </para>
    /// <para>
    /// By default, terminals Push their matches on a assembly's
    /// stack after a successful match. 
    /// </para>
    /// <para>
    /// Parsers recognize text, and assemblers provide any 
    /// sort of work that should occur after this recognition. 
    /// This work usually has to do with the state of the assembly,
    /// which is why assemblies have a stack and a target. 
    /// Essentially, parsers trade advancement on a assembly 
    /// for work on the assembly's stack or target.
    /// </para>
    /// </summary>
    public abstract class Assembler
    {
        /// <summary>
        /// <para>
        /// Returns a vector of the elements on an assembly's stack 
        /// that appear before a specified fence.
        /// </para>
        /// <para>
        /// Sometimes a parser will recognize a list from within 
        /// a pair of parentheses or brackets. The parser can mark 
        /// the beginning of the list with a fence, and then retrieve 
        /// all the items that come after the fence with this method.
        /// </para>
        /// </summary>
        /// <param name="A">a assembly whose stack should contain  some number of items above a fence marker</param>
        /// <param name="Fence">the fence, a marker of where to stop  popping the stack</param>
        /// <returns>Returns a list of the elements above the specified fence</returns>
        public static ArrayList ElementsAbove(Assembly A, object Fence)
        {
            ArrayList Items = new ArrayList();

            while (!A.StackIsEmpty())
            {
                object Top = A.Pop();
                if (Top.Equals(Fence))
                {
                    break;
                }
                Items.Add(Top);//       items.addElement(top);
            }
            return Items;
        }
        /// <summary>
        /// This is the one method all subclasses must implement. It 
        /// specifies what to do when a parser successfully 
        /// matches against a assembly.
        /// </summary>
        /// <param name="a"></param>
        public abstract void WorkOn(Assembly a);
    }
}
