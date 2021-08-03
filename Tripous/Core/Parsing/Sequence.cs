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
 * A <code>Sequence</code> object is a collection of 
 * parsers, all of which must in turn Match against an 
 * assembly for this parser to successfully Match.
 * 
 *
 * 
 *
 */

    public class Sequence : CollectionParser
    {

        /**
         * Constructs a nameless sequence.
         */
        public Sequence()
        {
        }
        /**
         * Constructs a sequence with the given name.
         *
         * @param    name    a name to be known by
         */
        public Sequence(string name) : base(name)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parser.
         */
        public Sequence(Parser p) : base(p)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public Sequence(Parser p1, Parser p2) : base(p1, p2)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public Sequence(Parser p1, Parser p2, Parser p3) : base(p1, p2, p3)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public Sequence(Parser p1, Parser p2, Parser p3, Parser p4) : base(p1, p2, p3, p4)
        {
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
            pv.VisitSequence(this, visited);
        }
        /**
         * Given a set of assemblies, this method matches this
         * sequence against all of them, and returns a new set 
         * of the assemblies that result from the matches.
         *
         * @return   a ArrayList of assemblies that result from 
         *           matching against a beginning set of assemblies
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            ArrayList Out = In;
            for (int i = 0; i < FSubParsers.Count; i++)
            {
                Parser P = (Parser)FSubParsers[i];
                Out = P.MatchAndAssemble(Out);
                if (Out.Count == 0)
                    return Out;
            }

            return Out;
        }
        /**
         * Create a random expansion for each parser in this 
         * sequence and return a collection of all these expansions.
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            ArrayList v = new ArrayList();
            for (int i = 0; i < FSubParsers.Count; i++)
            {
                Parser P = (Parser)FSubParsers[i];
                ArrayList w = P.RandomExpansion(maxDepth, depth++);

                for (int j = 0; j < w.Count; j++)
                    v.Add(w[j]);
            }
            return v;
        }
        /**
         * Returns the string to show between the parsers this
         * parser is a sequence of. This is an empty string,
         * since convention indicates sequence quietly. For
         * example, note that in the regular expression 
         * <code>(a|b)c</code>, the lack of a delimiter between
         * the expression in parentheses and the 'c' indicates a 
         * sequence of these expressions.
         */
        protected override string ToStringSeparator()
        {
            return "";
        }
    }
}
