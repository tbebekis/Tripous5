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
    /**
 * This class is a special case of a <code>SymbolNode</code>. A
 * <code>SymbolRootNode</code> object has no symbol of its
 * own, but has children that represent all possible symbols.
 * 
 *
 * 
 * @version 1.0
 */
    public class SymbolRootNode : SymbolNode
    {
        /// <summary>
        /// 
        /// </summary>
        new protected SymbolNode[] children = new SymbolNode[256];
        /**
         * Create and initialize a root node.
         */
        public SymbolRootNode() : base(null, (char)0)
        {

            Init();
        }
        /**
         * Add the given string as a symbol.
         *
         * @param   string   the character sequence to Add
         */
        public void Add(string s)
        {
            char c = s[0];
            SymbolNode n = EnsureChildWithChar(c);
            n.AddDescendantLine(s.Substring(1));
            FindDescendant(s).SetValid(true);
        }
        /**
         * A root node has no parent and no character of its own, so 
         * its Ancestry is "".
         *
         * @return an empty string
         */
        public override string Ancestry()
        {
            return "";
        }
        /**
         * A root node maintains its children in an array instead of
         * a Vector, to be faster.
         */
        protected override SymbolNode FindChildWithChar(char c)
        {
            return children[c];
        }
        /**
         * Set all possible symbols to be valid children. This means
         * that the decision of which characters are valid one-
         * character symbols lies outside this tree. If a tokenizer
         * asks this tree to produce a symbol, this tree assumes that
         * the first available character is a valid symbol.
         */
        protected void Init()
        {
            int len = children.Length;
            for (int i = 0; i < len; i++)
            {
                children[i] = new SymbolNode(this, (char)i);
                children[i].SetValid(true);
            }
        }
        /**
         * Return a symbol string from a reader.
         *
         * @param   System.IO.Stream   a reader to ReadByte from
         *
         * @param   int   the first character of this symbol, already
         *                ReadByte from the reader
         *
         * @return a symbol string from a reader
         */
        public string NextSymbol(System.IO.Stream r, int first)
        {

            SymbolNode n1 = FindChildWithChar((char)first);
            SymbolNode n2 = n1.DeepestRead(r);
            SymbolNode n3 = n2.unreadToValid(r);
            return n3.Ancestry();
        }
    }
}
