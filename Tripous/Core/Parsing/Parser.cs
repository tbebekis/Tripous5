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
* A <code>Parser</code> is an object that recognizes the
* elements of a language.
*  
* Each <code>Parser</code> object is either a <code>
* Terminal</code> or a composition of other parsers.
* The <code>Terminal</code> class is a subclass of <code>
* Parser</code>, and is itself a hierarchy of 
* parsers that recognize specific patterns of text. For 
* example, a <code>Word</code> recognizes any word, and a 
* <code>Literal</code> matches a specific string. 
*  
* In addition to <code>Terminal</code>, other subclasses of 
* <code>Parser</code> provide composite parsers, 
* describing sequences, alternations, and repetitions of 
* other parsers. For example, the following <code>
* Parser</code> objects culminate in a <code>good
* </code> parser that recognizes a description of good 
* coffee.
*
*
*     Alternation adjective = new Alternation();
*     adjective.Add(new Literal("steaming"));
*     adjective.Add(new Literal("hot"));
*     Sequence good = new Sequence();
*     good.Add(new Repetition(adjective));
*     good.Add(new Literal("coffee"));
*     string s = "hot hot steaming hot coffee";
*     Assembly a = new TokenAssembly(s);
*     System.out.println(good.BestMatch(a));
* 
*
* This prints out:
*
* 
*     [hot, hot, steaming, hot, coffee]
*     hot/hot/steaming/hot/coffee^
* 
*
* The parser does not Match directly against a string, 
* it matches against an Assembly.  The 
* resulting assembly shows its stack, with four words on it, 
* along with its sequence of tokens, and the index at the 
* end of these. In practice, parsers will do some work 
* on an assembly, based on the text they recognize. 
* 
*
*
* 
*/

    public abstract class Parser
    {
        /**
        * a FName to identify this parser
        */
        protected string FName;
        /**
        * an object that will work on an assembly whenever this 
        * parser successfully matches against the assembly
        */
        protected Assembler FAssembler;


        /**
        * Constructs a nameless parser.
        */
        public Parser()
        {
        }
        /**
        * Constructs a parser with the given FName.
        *
        * @param   string   A FName to be known by. For parsers
        *                   that are deep composites, a simple FName
        *                   identifying its purpose is useful.
        */
        public Parser(string Name)
        {
            this.FName = Name;
        }
        /**
        * Accepts a "visitor" which will perform some operation on
        * a parser structure. The book, "Design Patterns", explains
        * the visitor pattern.
        *
        * @param   ParserVisitor   the visitor to Accept
        */
        public void Accept(ParserVisitor pv)
        {
            Accept(pv, new ArrayList());
        }
        /**
        * Accepts a "visitor" along with a collection of previously
        * visited parsers.
        *
        * @param   ParserVisitor   the visitor to Accept
        *
        * @param   ArrayList   a collection of previously visited 
        *                   parsers.
        */
        public abstract void Accept(ParserVisitor pv, ArrayList visited);
        /**
        * Adds the elements of one vector to another.
        *
        * @param   v1   the vector to Add to
        *
        * @param   v2   the vector with elements to Add
        */
        public static void Add(ArrayList v1, ArrayList v2)
        {
            v1.AddRange(v2);
        }
        /**
        * Returns the most-matched assembly in a collection.
        *
        * @return   the most-matched assembly in a collection.
        *
        * @param   ArrayList   the collection to look through
        *
        */
        public Assembly Best(ArrayList v)
        {
            Assembly best = null;
            for (int i = 0; i < v.Count; i++)
            {
                Assembly A = (Assembly)v[i];

                if (!A.HasMoreElements())
                    return A;

                if (best == null)
                    best = A;
                else if (A.ElementsConsumed() > best.ElementsConsumed())
                    best = A;

            }

            return best;
        }
        /**
        * Returns an assembly with the greatest possible number of 
        * elements Consumed by matches of this parser.
        *
        * @return   an assembly with the greatest possible number of 
        *           elements Consumed by this parser
        *
        * @param   Assembly   an assembly to Match against
        *
        */
        public Assembly BestMatch(Assembly a)
        {
            ArrayList In = new ArrayList();
            In.Add(a);
            ArrayList Out = MatchAndAssemble(In);
            return Best(Out);
        }
        /**
        * Returns either null, or a completely matched version of 
        * the supplied assembly.
        *
        * @return   either null, or a completely matched version of the
        *           supplied assembly
        *
        * @param   Assembly   an assembly to Match against
        *
        */
        public Assembly CompleteMatch(Assembly a)
        {
            Assembly best = BestMatch(a);
            if (best != null && !best.HasMoreElements())
            {
                return best;
            }
            return null;
        }
        /**
        * Create a copy of a vector, cloning each element of
        * the vector.
        *
        * @param   in   the vector to copy
        *
        * @return   a copy of the input vector, cloning each 
        *           element of the vector
        */
        public static ArrayList ElementClone(ArrayList v)
        {
            ArrayList copy = new ArrayList();
            for (int i = 0; i < v.Count; i++)
            {
                Assembly A = (Assembly)v[i];
                copy.Add(A.Clone());
            }
            return copy;
        }
        /**
        * Returns the FName of this parser.
        *
        * @return   the FName of this parser
        *
        */
        public string GetName()
        {
            return FName;
        }
        /**
        * Given a set (well, a <code>ArrayList</code>, really) of 
        * assemblies, this method matches this parser against 
        * all of them, and returns a new set (also really a 
        * <code>ArrayList</code>) of the assemblies that result from 
        * the matches.
        *  
        * For example, consider matching the regular expression 
        * <code>a*</code> against the string <code>"aaab"</code>. 
        * The initial set of states is <code>{^aaab}</code>, where 
        * the ^ indicates how far along the assembly is. When 
        * <code>a*</code> matches against this initial state, it 
        * creates a new set <code>{^aaab, a^aab, aa^ab, 
        * aaa^b}</code>. 
        * 
        * @return   a ArrayList of assemblies that result from 
        *           matching against a beginning set of assemblies
        *
        * @param   ArrayList   a vector of assemblies to Match against
        *
        */
        public abstract ArrayList Match(ArrayList In);
        /**
        * Match this parser against an input state, and then
        * apply this parser's FAssembler against the resulting
        * state.
        *
        * @return   a ArrayList of assemblies that result from matching
        *           against a beginning set of assemblies
        *
        * @param   ArrayList   a vector of assemblies to Match against
        *
        */
        public ArrayList MatchAndAssemble(ArrayList In)
        {
            ArrayList Out = Match(In);
            if (FAssembler != null)
            {

                for (int i = 0; i < Out.Count; i++)
                    FAssembler.WorkOn((Assembly)Out[i]);

                /*
                 Enumeration e = Out.elements();
                 while (e.HasMoreElements()) 
                 {
                     FAssembler.WorkOn((Assembly) e.nextElement());
                 }
                 */
            }
            return Out;
        }
        /**
        * Create a random expansion for this parser, where a
        * concatenation of the returned collection will be a
        * language element.
        */
        public abstract ArrayList RandomExpansion(int maxDepth, int depth);
        /**
        * Return a random element of this parser's language.
        *
        * @return  a random element of this parser's language
        */
        public string RandomInput(int maxDepth, string separator)
        {
            StringBuilder buf = new StringBuilder();
            ArrayList E = RandomExpansion(maxDepth, 0);
            bool first = true;

            for (int i = 0; i < E.Count; i++)
            {
                if (!first)
                    buf.Append(separator);

                buf.Append(E[i]);
                first = false;
            }

            return buf.ToString();

        }
        /**
        * Sets the object that will work on an assembly whenever 
        * this parser successfully matches against the 
        * assembly.
        *
        * @param   Assembler   the FAssembler to apply
        *
        * @return   Parser   this
        */
        public Parser SetAssembler(Assembler Assembler)
        {
            this.FAssembler = Assembler;
            return this;
        }
        /**
        * Returns a textual description of this parser.
        *
        * @return   string   a textual description of this 
        *                    parser, taking care to avoid 
        *                    infinite recursion
        */
        public override string ToString()
        {
            return ToString(new ArrayList());
        }
        /**
        * Returns a textual description of this parser.
        * Parsers can be recursive, so when building a 
        * descriptive string, it is important to avoid infinite 
        * recursion by keeping track of the objects already 
        * described. This method keeps an object from printing 
        * twice, and uses <code>UnvisitedString</code> which 
        * subclasses must implement.
        * 
        * @param   ArrayList    a list of objects already printed 
        *
        * @return   a textual version of this parser,
        *           avoiding recursion
        */
        public string ToString(ArrayList visited)
        {
            if (FName != null)
                return FName;
            else if (visited.IndexOf(this) != -1)
                return "...";
            else
            {
                visited.Add(this);
                return UnvisitedString(visited);
            }
        }
        /**
        * Returns a textual description of this string.
        */
        public abstract string UnvisitedString(ArrayList visited);
    }
}
