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

 

namespace Tripous.Parsing
{


    /// <summary>
    /// <para>
    /// A parser is an object that recognizes the elements of a language.
    /// A parser recognizes a string (wrapped in an Assembly object) and assemble a result.
    /// </para>
    /// <para>
    /// Each parser object is either a <see cref="TerminalParser"/> or a composition of other parsers.
    /// The <see cref="TerminalParser"/> class is a subclass of <see cref="Parser"/>, 
    /// and is itself a hierarchy of parsers that recognize specific patterns of text. 
    /// For example, a <see cref="WordTerminalParser"/> recognizes any word, 
    /// and a <see cref="LiteralTerminalParser"/> matches a specific string. 
    /// </para>
    /// <para>
    /// In addition to <see cref="TerminalParser"/>, other subclasses of <see cref="Parser"/> provide composite parsers, 
    /// describing sequences, alternations, and repetitions of other parsers. 
    /// For example, the following <see cref="Parser"/> objects culminate in a <code>GoodParser</code> parser 
    /// that recognizes a description of good coffee.
    /// </para>
    /// <para>
    /// <code>
    /// 
    ///      Alternation adjective = new Alternation();
    ///      adjective.Add(new Literal("steaming"));
    ///      adjective.Add(new Literal("hot"));
    ///      Sequence good = new Sequence();
    ///      good.Add(new Repetition(adjective));
    ///      good.Add(new Literal("coffee"));
    ///      string s = "hot hot steaming hot coffee";
    ///      Assembly a = new TokenAssembly(s);
    ///      System.out.println(good.BestMatch(a));
    ///     
    /// </code>
    /// </para>    
    /// <para>
    /// This prints out:
    /// </para>
    /// <para>
    /// <code>
    /// 
    ///      [hot, hot, steaming, hot, coffee]
    ///      hot/hot/steaming/hot/coffee^
    /// 
    /// </code>
    /// </para>
    /// <para>
    /// The parser does not match directly against a string, it matches against an <see cref="Assembly"/>.  
    /// The resulting assembly shows its stack, with four words on it, along with its sequence of tokens, and the index at the end of these. 
    /// In practice, parsers will do some work on an assembly, based on the text they recognize. 
    /// </para>
    /// </summary>
    public abstract class Parser
    {
 
        /// <summary>
        /// an object that will work on an assembly whenever this  parser successfully matches against the assembly
        /// </summary>
        protected Assembler FAssembler;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Parser()
        {
        }
        /// <summary>
        /// Constructs a parser with the given name.  For parsers that are deep composites, a simple FName identifying its purpose is useful.
        /// </summary>
        public Parser(string Name)
        {
            this.Name = Name;
        }

        /* public */


        /// <summary>
        /// Returns textual description of this  parser, taking care to avoid infinite recursion
        /// </summary>
        public override string ToString()
        {
            return ToString(new ArrayList());
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// Parsers can be recursive, so when building a 
        /// descriptive string, it is important to avoid infinite 
        /// recursion by keeping track of the objects already 
        /// described. This method keeps an object from printing 
        /// twice, and uses <code>UnvisitedString</code> which 
        /// subclasses must implement.
        /// </summary>
        /// <param name="visited">a list of objects already printed </param>
        /// <returns>returns a textual version of this parser, avoiding recursion</returns>
        public virtual string ToString(ArrayList visited)
        {
            if (Name != null)
                return Name;
            else if (visited.IndexOf(this) != -1)
                return "...";
            else
            {
                visited.Add(this);
                return UnvisitedString(visited);
            }
        }
        /// <summary>
        /// Returns a textual description of this string.
        /// </summary>
        public abstract string UnvisitedString(ArrayList visited);
       
        /// <summary>
        /// Adds the elements of one vector to another.
        /// </summary>
        /// <param name="v1">the vector to Add to</param>
        /// <param name="v2">the vector with elements to Add</param>
        static public void Add(ArrayList v1, ArrayList v2)
        {
            foreach (var Element in v2)
                v1.Add(Element); 
        }
        /// <summary>
        /// Creates and returns a copy of a vector, cloning each element of the vector.
        /// </summary>
        /// <param name="v">the vector to copy</param>
        /// <returns>Creates and returns a copy of a vector, cloning each element of the vector.</returns>
        static public ArrayList ElementClone(ArrayList v)
        {
            ArrayList copy = new ArrayList();
            foreach (Assembly A in v)
            {
                copy.Add(A.Clone());
            }
            return copy;
        }
        
        /// <summary>
        /// Accepts a "visitor" which will perform some operation on
        /// a parser structure. The book, "Design Patterns", explains
        /// the visitor pattern.
        /// </summary>
        public void Accept(ParserVisitor pv)
        {
            Accept(pv, new ArrayList());
        }
        /// <summary>
        /// Accepts a "visitor" along with a collection of previously visited parsers.
        /// </summary>
        /// <param name="pv">the visitor to Accept</param>
        /// <param name="visited">a collection of previously visited  parsers.</param>
        public abstract void Accept(ParserVisitor pv, ArrayList visited);

        /// <summary>
        /// Returns the most-matched assembly in a collection.
        /// </summary>
        /// <param name="v">the collection to look through</param>
        /// <returns> Returns the most-matched assembly in a collection.</returns>
        public Assembly Best(ArrayList v)
        {
            Assembly best = null;

            foreach (Assembly A in v)
            {
                if (!A.HasMoreElements)
                    return A;

                if (best == null)
                    best = A;
                else if (A.ElementsConsumed > best.ElementsConsumed)
                    best = A;
            } 

            return best;
        }
        /// <summary>
        /// Returns an assembly with the greatest possible number of elements Consumed by matches of this parser.
        /// </summary>
        /// <param name="a">an assembly to Match against</param>
        /// <returns>Returns an assembly with the greatest possible number of elements Consumed by matches of this parser.</returns>
        public Assembly BestMatch(Assembly a)
        {
            ArrayList In = new ArrayList();
            In.Add(a);
            ArrayList Out = MatchAndAssemble(In);
            return Best(Out);
        }
        /// <summary>
        ///  Returns either null, or a completely matched version of the supplied assembly.
        /// </summary>
        /// <param name="a">an assembly to Match against</param>
        /// <returns> Returns either null, or a completely matched version of  the supplied assembly.</returns>
        public Assembly CompleteMatch(Assembly a)
        {
            Assembly best = BestMatch(a);
            if (best != null && !best.HasMoreElements)
            {
                return best;
            }
            return null;
        }
 

        /// <summary>
        /// <para>
        /// Given a set (well, a <code>ArrayList</code>, really) of 
        /// assemblies, this method matches this parser against 
        /// all of them, and returns a new set (also really a 
        /// <code>ArrayList</code>) of the assemblies that result from 
        /// the matches.
        /// </para>
        /// <para>
        /// For example, consider matching the regular expression 
        /// <code>a*</code> against the string <code>"aaab"</code>. 
        /// The initial set of states is <code>{^aaab}</code>, where 
        /// the ^ indicates how far along the assembly is. When 
        /// <code>a*</code> matches against this initial state, it 
        /// creates a new set <code>{^aaab, a^aab, aa^ab, aaa^b}</code>. 
        /// </para>
        /// </summary>
        /// <param name="In">a vector of assemblies to Match against</param>
        /// <returns>Returns  a ArrayList of assemblies that result from  matching against a beginning set of assemblies</returns>
        public abstract ArrayList Match(ArrayList In);
        /// <summary>
        /// Match this parser against an input state, and then
        /// apply this parser's FAssembler against the resulting state.
        /// </summary>
        /// <param name="In">a vector of assemblies to Match against</param>
        /// <returns>Returns a ArrayList of assemblies that result from matching against a beginning set of assemblies</returns>
        public ArrayList MatchAndAssemble(ArrayList In)
        {
            ArrayList Out = Match(In);
            if (FAssembler != null)
            {
                foreach (Assembly Element in Out)
                    FAssembler.WorkOn(Element);
            }
            return Out;
        }


        /// <summary>
        /// Create a random expansion for this parser, where a
        /// concatenation of the returned collection will be a
        /// language element.
        /// </summary>
        public abstract ArrayList RandomExpansion(int maxDepth, int depth);
        /// <summary>
        /// Returns a random element of this parser's language.
        /// </summary>
        public string RandomInput(int maxDepth, string separator)
        {
            StringBuilder buf = new StringBuilder();
            ArrayList E = RandomExpansion(maxDepth, 0);
            bool first = true;

            foreach (var Element in E)
            {
                if (!first)
                    buf.Append(separator);

                buf.Append(Element);
                first = false;
            }

            return buf.ToString();
        }
        /// <summary>
        /// Sets the object that will work on an assembly whenever 
        /// this parser successfully matches against the  assembly. 
        /// Returns this instance.
        /// </summary>
        /// <param name="Assembler">the FAssembler to apply</param>
        /// <returns>Returns this instance</returns>
        public Parser SetAssembler(Assembler Assembler)
        {
            this.FAssembler = Assembler;
            return this;
        }


        /* properties */
        /// <summary>
        /// A name that identifies this parser
        /// </summary>
        public string Name { get; protected set; }
    }
}
