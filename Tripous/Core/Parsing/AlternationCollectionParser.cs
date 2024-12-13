/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Parsing
{


    /// <summary>
    /// An <see cref="AlternationCollectionParser"/> object is a collection of  parsers, 
    /// any one of which can successfully Match against an assembly.
    /// </summary>
    public class AlternationCollectionParser : CollectionParser
    {

        /// <summary>
        ///  This method is similar to RandomExpansion, but it will pick a terminal if one is available.
        /// </summary>
        protected ArrayList RandomSettle(int maxDepth, int depth)
        {

            // which alternatives are terminals?
            Parser P = null;

            ArrayList terms = new ArrayList();

            for (int i = 0; i < fSubParsers.Count; i++)
            {
                P = (Parser)fSubParsers[i];
                if (P is TerminalParser)
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
                which = fSubParsers;
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
        /// <summary>
        /// Returns the string to show between the parsers this parser is an alternation of.
        /// </summary>
        protected override string ToStringSeparator()
        {
            return "|";
        }

        /* construction */
        /// <summary>
        /// Constructs a nameless alternation.
        /// </summary>
        public AlternationCollectionParser()
        {
        }
        /// <summary>
        /// Constructs an alternation with the given name.
        /// </summary>
        public AlternationCollectionParser(string name) 
            : base(name)
        {
        }
        /// <summary>
        /// A convenient way to construct a CollectionParser with a parser.
        /// </summary>
        public AlternationCollectionParser(Parser p) 
            : base(p)
        {
        }
        /// <summary>
        /// A convenient way to construct a CollectionParser with the given parsers.
        /// </summary>
        public AlternationCollectionParser(Parser[] Parsers)
            : base(Parsers)
        {
        }
 
        /* public */
        /// <summary>
        /// Accept a "visitor" and a collection of previously visited parsers.
        /// </summary>
        /// <param name="pv">the visitor to Accept</param>
        /// <param name="visited"> a collection of previously visited parsers</param>
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitAlternation(this, visited);
        }
        /// <summary>
        /// Given a set of assemblies, this method matches this 
        /// alternation against all of them, and returns a new set
        /// of the assemblies that result from the matches.
        /// </summary>
        /// <param name="In"> a ArrayList of assemblies that result from matching against a beginning set of assemblies</param>
        /// <returns>Returns a list of assemblies to Match against</returns>
        public override List<Assembly> Match(List<Assembly> In)
        {
            List<Assembly> ResultList = new List<Assembly>();

            List<Assembly> Matched;
            for (int i = 0; i < fSubParsers.Count; i++)
            {
                Parser p = (Parser)fSubParsers[i];
                Matched = p.MatchAndAssemble(In);
                ResultList.AddRange(Matched);
            }
            return ResultList;
        }
        /// <summary>
        /// Create a random collection of elements that correspond to this alternation.
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            if (depth >= maxDepth)
                return RandomSettle(maxDepth, depth);

            //double n = (double) FSubParsers.Count;
            Random Random = new Random();

            int i = Random.Next(fSubParsers.Count);
            Parser j = (Parser)fSubParsers[i];
            return j.RandomExpansion(maxDepth, depth++);

        }

    }
}
