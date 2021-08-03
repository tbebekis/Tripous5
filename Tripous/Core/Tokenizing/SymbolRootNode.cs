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
    /// This class is a special case of a <code>SymbolNode</code>. 
    /// A <code>SymbolRootNode</code> object has no symbol of its own, but has children that represent all possible symbols.
    /// </summary>
    public class SymbolRootNode : SymbolNode
    {
        /// <summary>
        /// 
        /// </summary>
        new protected SymbolNode[] children = new SymbolNode[256];

        /// <summary>
        /// A root node maintains its children in an array instead of a Vector, to be faster.
        /// </summary>
        protected override SymbolNode FindChildWithChar(char c)
        {
            return children[c];
        }
        /// <summary>
        /// Set all possible symbols to be valid children. This means
        /// that the decision of which characters are valid one-
        /// character symbols lies outside this tree. If a tokenizer
        /// asks this tree to produce a symbol, this tree assumes that
        /// the first available character is a valid symbol.
        /// </summary>
        protected void Init()
        {
            int len = children.Length;
            for (int i = 0; i < len; i++)
            {
                children[i] = new SymbolNode(this, (char)i);
                children[i].SetValid(true);
            }
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SymbolRootNode() 
            : base(null, (char)0)
        {

            Init();
        }
 
        /// <summary>
        /// Add the given string as a symbol.
        /// </summary>
        /// <param name="s">the character sequence to Add</param>
        public void Add(string s)
        {
            char c = s[0];
            SymbolNode n = EnsureChildWithChar(c);
            n.AddDescendantLine(s.Substring(1));
            FindDescendant(s).SetValid(true);
        }
        /// <summary>
        /// Returns an empty string. A root node has no parent and no character of its own, so its Ancestry is "".
        /// </summary> 
        public override string Ancestry()
        {
            return "";
        }
        /// <summary>
        /// Returns a symbol string from a reader.
        /// </summary>
        /// <param name="r"> a reader to ReadByte from</param>
        /// <param name="first">the first character of this symbol, already ReadByte from the reader</param>
        /// <returns>Returns a symbol string from a reader.</returns>
        public string NextSymbol(System.IO.Stream r, int first)
        {

            SymbolNode n1 = FindChildWithChar((char)first);
            SymbolNode n2 = n1.DeepestRead(r);
            SymbolNode n3 = n2.unreadToValid(r);
            return n3.Ancestry();
        }
    }
}
