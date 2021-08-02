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
* An <code>Alternation</code> object is a collection of 
* parsers, any one of which can successfully Match against
* an assembly.
* 
*
*
*
*/

    public class Alternation : CollectionParser
    {

        /**
        * Constructs a nameless alternation.
        */
        public Alternation()
        {
        }
        /**
        * Constructs an alternation with the given name.
        *
        * @param    name    a name to be known by
        */
        public Alternation(string name) : base(name)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parser.
        */
        public Alternation(Parser p) : base(p)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parsers.
        */
        public Alternation(Parser p1, Parser p2) : base(p1, p2)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parsers.
        */
        public Alternation(Parser p1, Parser p2, Parser p3) : base(p1, p2, p3)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parsers.
        */
        public Alternation(Parser p1, Parser p2, Parser p3, Parser p4) : base(p1, p2, p3, p4)
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
            pv.VisitAlternation(this, visited);
        }
        /**
        * Given a set of assemblies, this method matches this 
        * alternation against all of them, and returns a new set
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
            ArrayList Out = new ArrayList();

            for (int i = 0; i < FSubParsers.Count; i++)
            {
                Parser p = (Parser)FSubParsers[i];
                Add(Out, p.MatchAndAssemble(In));
            }
            return Out;
        }
        /**
        * Create a random collection of elements that correspond to
        * this alternation.
        */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            if (depth >= maxDepth)
                return RandomSettle(maxDepth, depth);

            //double n = (double) FSubParsers.Count;
            Random Random = new Random();

            int i = Random.Next(FSubParsers.Count);
            Parser j = (Parser)FSubParsers[i];
            return j.RandomExpansion(maxDepth, depth++);

        }
        /**
        * This method is similar to RandomExpansion, but it will
        * pick a terminal if one is available.
        */
        protected ArrayList RandomSettle(int maxDepth, int depth)
        {

            // which alternatives are terminals?
            Parser P = null;

            ArrayList terms = new ArrayList();

            for (int i = 0; i < FSubParsers.Count; i++)
            {
                P = (Parser)FSubParsers[i];
                if (P is Terminal)
                    terms.Add(P);
            }


            /*
            Enumeration e = FSubParsers.elements();
            while (e.HasMoreElements()) 
            {
                Parser j = (Parser) e.nextElement();
                if (j is Terminal) 
                    terms.addElement(j);
            }
            */

            // pick one of the terminals or, if there are no
            // terminals, pick any subparser

            ArrayList which = terms;
            if (terms.Count == 0)
            {
                which = FSubParsers;
            }

            Random Random = new Random();

            int j = Random.Next(which.Count);
            P = (Parser)which[j];
            return P.RandomExpansion(maxDepth, depth++);




            /*
              double n = (double) which.size();
              int i = (int) (n * Math.random());
              Parser p = (Parser) which[i];
              return p.RandomExpansion(maxDepth, depth++);
              */
        }
        /**
        * Returns the string to show between the parsers this
        * parser is an alternation of.
        */
        protected override string ToStringSeparator()
        {
            return "|";
        }

    }
}
