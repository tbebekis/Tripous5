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
    /// A <see cref="RepetitionParser"/> matches its underlying parser repeatedly against a assembly.
    /// </summary>
    public class RepetitionParser : Parser
    {
        /// <summary>
        /// the parser this parser is a repetition of
        /// </summary>
        protected Parser FSubParser;
        /// <summary>
        /// the width of a random expansion
        /// </summary>
        protected static int EXPWIDTH = 4;
        /// <summary>
        /// an assembler to apply at the beginning of a Match
        /// </summary>
        protected Assembler FPreAssembler;

        /* construction */
        /// <summary>
        /// Constructs a repetiton that will Match the given  parser repeatedly in successive matches
        /// </summary>
        /// <param name="SubParser">the parser to repeat</param>
        public RepetitionParser(Parser SubParser) 
            : this(SubParser, null)
        {
        } 
        /// <summary>
        /// Constructs a repetiton that will Match the given  parser repeatedly in successive matches
        /// </summary>
        /// <param name="SubParser">the parser to repeat</param>
        /// <param name="name">a name to be known by</param>
        public RepetitionParser(Parser SubParser, string name) : base(name)
        {
            this.FSubParser = SubParser;
        }
       
        /// <summary>
        /// Accept a "visitor" and a collection of previously visited parsers.
        /// </summary>
        /// <param name="pv">the visitor to Accept</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitRepetition(this, visited);
        }
        /// <summary>
        /// Return this parser's SubParser.
        /// </summary>
        public Parser GetSubParser()
        {
            return FSubParser;
        }
 
        /// <summary>
        /// <para>
        /// Given a set of assemblies, this method applies a preassembler
        /// to all of them, matches its FSubParser repeatedly against each
        /// of them, applies its post-assembler against each, and returns
        /// a new set of the assemblies that result from the matches.
        /// </para>
        /// <para>
        /// For example, matching the regular expression <code>a*
        /// </code> against <code>{^aaab}</code> results in <code>
        ///  {^aaab, a^aab, aa^ab, aaa^b}</code>.
        /// </para>
        /// </summary>
        /// <param name="In">a vector of assemblies to Match against</param>
        /// <returns>Returns a ArrayList of assemblies that result from  matching against a beginning set of assemblies</returns>
        public override ArrayList Match(ArrayList In)
        {
            if (FPreAssembler != null)
            {
                foreach (Assembly Element in In)
                {
                    FPreAssembler.WorkOn(Element);
                }
            }

            ArrayList Out = ElementClone(In);
            ArrayList s = In;               // a working state
            while (s.Count > 0)            // !s.IsEmpty()
            {
                s = FSubParser.MatchAndAssemble(s);
                Add(Out, s);
            }
            return Out;
        }
        /*
        public Vector match(Vector in) {
            if (preAssembler != null) {
                Enumeration e = in.elements();
                while (e.hasMoreElements()) {
                    preAssembler.workOn((Assembly) e.nextElement());
                }
            }
            Vector out = elementClone(in);
            Vector s = in; // a working state
            while (!s.isEmpty()) {
                s = subparser.matchAndAssemble(s);
                add(out, s);
            }
            return out;
        } 
         */
        /// <summary>
        /// Create a collection of random elements that correspond to this repetition.
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            ArrayList v = new ArrayList();
            if (depth >= maxDepth)
            {
                return v;
            }

            Random Random = new Random();

            int n = Random.Next(EXPWIDTH);
            for (int j = 0; j < n; j++)
            {
                ArrayList w = FSubParser.RandomExpansion(maxDepth, depth++);
                for (int i = 0; i < w.Count; i++)
                    v.Add(w[i]);
            }
            return v;
        } 
        /// <summary>
        /// Sets the object that will work on every assembly before  matching against it. Returns this instance.
        /// </summary>
        /// <param name="FPreAssembler">the assembler to apply</param>
        /// <returns>Returns this instance.</returns>
        public Parser SetPreAssembler(Assembler FPreAssembler)
        {
            this.FPreAssembler = FPreAssembler;
            return this;
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// </summary>
        public override string UnvisitedString(ArrayList visited)
        {
            return FSubParser.ToString(visited) + "*";
        }
    }
}
