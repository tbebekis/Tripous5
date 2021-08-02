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

using Tripous.Tokenizing;

namespace Tripous.Parsing
{
    /**
 * A <code>Repetition</code> matches its underlying parser 
 * repeatedly against a assembly.
 * 
 *
 * 
 *
 */

    public class Repetition : Parser
    {
        /**
         * the parser this parser is a repetition of
         */
        protected Parser FSubParser;

        /**
         * the width of a random expansion
         */
        protected static int EXPWIDTH = 4;

        /**
         * an assembler to apply at the beginning of a Match
         */
        protected Assembler FPreAssembler;
        /**
         * Constructs a repetition of the given parser. 
         * 
         * @param   parser   the parser to repeat
         *
         * @return   a repetiton that will Match the given 
         *           parser repeatedly in successive matches
         */
        public Repetition(Parser p) : this(p, null)
        {
        }
        /**
         * Constructs a repetition of the given parser with the
         * given name.
         * 
         * @param   Parser   the parser to repeat
         *
         * @param   string   a name to be known by
         *
         * @return   a repetiton that will Match the given 
         *           parser repeatedly in successive matches
         */
        public Repetition(Parser SubParser, string name) : base(name)
        {
            this.FSubParser = SubParser;
        }
        /**
         * Accept a "visitor" and a collection of previously visited
         * parsers.
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitRepetition(this, visited);
        }
        /**
         * Return this parser's FSubParser.
         *
         * @return   Parser   this parser's FSubParser
         */
        public Parser GetSubParser()
        {
            return FSubParser;
        }
        /**
         * Given a set of assemblies, this method applies a preassembler
         * to all of them, matches its FSubParser repeatedly against each
         * of them, applies its post-assembler against each, and returns
         * a new set of the assemblies that result from the matches.
         *  
         * For example, matching the regular expression <code>a*
         * </code> against <code>{^aaab}</code> results in <code>
         * {^aaab, a^aab, aa^ab, aaa^b}</code>.
         *
         * @return   a ArrayList of assemblies that result from 
         *           matching against a beginning set of assemblies
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            if (FPreAssembler != null)
            {
                for (int i = 0; i < In.Count; i++)
                    FPreAssembler.WorkOn((Assembly)In[i]);
            }
            ArrayList Out = ElementClone(In);
            ArrayList s = In; // a working state
            while (s.Count > 0)            // !s.IsEmpty()
            {
                s = FSubParser.MatchAndAssemble(s);
                Add(Out, s);
            }
            return Out;
        }
        /**
         * Create a collection of random elements that correspond to
         * this repetition.
         */
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
        /**
         * Sets the object that will work on every assembly before 
         * matching against it.
         *
         * @param   Assembler   the assembler to apply
         *
         * @return   Parser   this
         */
        public Parser SetPreAssembler(Assembler FPreAssembler)
        {
            this.FPreAssembler = FPreAssembler;
            return this;
        }
        /**
         * Returns a textual description of this parser.
         */
        public override string UnvisitedString(ArrayList visited)
        {
            return FSubParser.ToString(visited) + "*";
        }
    }
}
