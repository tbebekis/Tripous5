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
    /// <para>Used by the <see cref="SymbolState"/></para>
    /// <para>
    /// A <see cref="SymbolNode"/> object is a member of a tree 
    /// that contains all possible prefixes of allowable symbols. 
    /// Multi-character symbols appear in a <see cref="SymbolNode"/> tree 
    /// with one node for each character. 
    /// </para>
    /// <para>
    /// For example, the symbol <code>=:~</code> will appear in a 
    /// tree as three nodes. The first node contains an equals sign,
    /// and has a child; that child contains a colon and has a 
    /// child; this third child contains a tilde, and has no 
    /// children of its own. If the colon node had another child 
    /// for a dollar sign character, then the tree would contain 
    /// the symbol <code>=:$</code>.
    /// </para>
    /// <para>
    /// A tree of <code>SymbolNode</code> objects collaborate to 
    /// ReadByte a (potentially multi-character) symbol from an input 
    /// stream. A root node with no character of its own finds an 
    /// initial node that represents the first character in the 
    /// input. This node looks to see if the next character in the 
    /// stream matches one of its children. If so, the node 
    /// delegates its reading task to its child. This approach 
    /// walks down the tree, pulling symbols from the input that 
    /// match the path down the tree.
    /// </para>
    /// <para>
    /// When a node does not have a child that matches the next 
    /// character, we will have ReadByte the longest possible symbol 
    /// prefix. This prefix may or may not be a valid symbol. 
    /// Consider a tree that has had <code>=:~</code> added and has 
    /// not had <code>=:</code> added. In this tree, of the three 
    /// nodes that contain <code>=:~</code>, only the first and 
    /// third contain complete symbols. If, say, the input contains 
    /// <code>=:a</code>, the colon node will not have a child that 
    /// matches the 'a' and so it will stop reading. The colon node 
    /// has to "unread": it must push back its character, and ask 
    /// its parent to unread. Unreading continues until it reaches 
    /// an ancestor that represents a valid symbol.
    /// </para>
    /// </summary>
    public class SymbolNode
    {
        /// <summary>
        /// 
        /// </summary>
        protected char FValue;
        /// <summary>
        /// 
        /// </summary>
        protected ArrayList children = new ArrayList(); // of Node
        /// <summary>
        /// 
        /// </summary>
        protected bool valid = false;
        /// <summary>
        /// 
        /// </summary>
        protected SymbolNode FParent;

        /// <summary>
        /// Find or create a child for the given character. 
        /// </summary>
        protected SymbolNode EnsureChildWithChar(char c)
        {
            SymbolNode n = FindChildWithChar(c);
            if (n == null)
            {
                n = new SymbolNode(this, c);
                children.Add(n); //children.addElement(n);
            }
            return n;
        }
        /// <summary>
        /// Find a child with the given character.
        /// </summary>
        protected virtual SymbolNode FindChildWithChar(char c)
        {
            for (int i = 0; i < children.Count; i++)
            {
                SymbolNode n = (SymbolNode)children[i];
                if (n.FValue == c)
                {
                    return n;
                }
            }
            return null;
            /*
            Enumeration e = children.elements();
            while (e.hasMoreElements()) 
            {
               SymbolNode n = (SymbolNode) e.nextElement();
               if (n.myChar == c) 
               {
                  return n;
               }
            }
            return null;
            */
        }
        /// <summary>
        /// Find a descendant which is down the path the given string indicates. 
        /// </summary>
        protected SymbolNode FindDescendant(string s)
        {
            char c = s[0];// s.charAt(0);
            SymbolNode n = FindChildWithChar(c);
            if (s.Length == 1)
            {
                return n;
            }
            return n.FindDescendant(s.Substring(1));
        }
 
        /* construction */
        /// <summary>
        ///  Constructs a SymbolNode with the given parent, representing  the given character.
        /// </summary>
        /// <param name="parent">this node's parent</param>
        /// <param name="Value">this node's character</param>
        public SymbolNode(SymbolNode parent, char Value)
        {
            this.FParent = parent;
            this.FValue = Value;
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this node.
        /// </summary>
        public override string ToString()
        {
            return "" + FValue + '(' + valid + ')';
        }


        /// <summary>
        /// Add a line of descendants that represent the characters in the given string.
        /// </summary>
        public void AddDescendantLine(string s)    // was protected
        {
            if (s.Length > 0)
            {
                char c = s[0]; //s.charAt(0);
                SymbolNode n = EnsureChildWithChar(c);
                n.AddDescendantLine(s.Substring(1));
            }
        }
 
        /// <summary>
        /// Returns the symbol this node represents
        /// </summary>
        public virtual string Ancestry()
        {
            if (FParent == null)
                return FValue.ToString();

            return FParent.Ancestry() + FValue.ToString();
        }
        /// <summary>
        ///  Find the descendant that takes as many characters as  possible from the input.
        /// </summary>
        public SymbolNode DeepestRead(System.IO.Stream r)  // was protected
        {

            char c = (char)r.ReadByte();
            SymbolNode n = FindChildWithChar(c);
            if (n == null)
            {
                r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
                return this;
            }
            return n.DeepestRead(r);
        }
        /// <summary>
        /// Mark this node as valid, which means its Ancestry is a complete symbol, not just a prefix.
        /// </summary>
        public void SetValid(bool b)    // was protected
        {
            valid = b;
        }
        /// <summary>
        /// Unwind to a valid node; this node is "valid" if its Ancestry represents a complete symbol. 
        /// If this node is not valid, put back the character and ask the parent to unwind. 
        /// </summary>
        public SymbolNode unreadToValid(System.IO.Stream r)  // was protected
        {

            if (valid)
            {
                return this;
            }
            r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(myChar);
            return FParent.unreadToValid(r);
        }
    }
}
