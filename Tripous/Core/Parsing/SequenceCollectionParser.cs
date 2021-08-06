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
    /// A <see cref="SequenceCollectionParser"/> object is a collection of  parsers, 
    /// all of which must in turn Match against an assembly for this parser to successfully Match.
    /// </summary>
    public class SequenceCollectionParser : CollectionParser
    {
        /// <summary>
        /// Returns the string to show between the parsers this
        /// parser is a sequence of. This is an empty string,
        /// since convention indicates sequence quietly. For
        /// example, note that in the regular expression 
        /// <code>(a|b)c</code>, the lack of a delimiter between
        /// the expression in parentheses and the 'c' indicates a 
        /// sequence of these expressions.
        /// </summary>
        protected override string ToStringSeparator()
        {
            return "";
        }

        /* construction */
        /// <summary>
        /// Constructs a nameless sequence.
        /// </summary>
        public SequenceCollectionParser()
        {
        }
        /// <summary>
        /// Constructs a sequence with the given name.
        /// </summary>
        public SequenceCollectionParser(string name) 
            : base(name)
        {
        }
        /// <summary>
        /// A convenient way to construct a CollectionParser with the given parser.
        /// </summary>
        public SequenceCollectionParser(Parser p) 
            : base(p)
        {
        }
        /// <summary>
        /// A convenient way to construct a CollectionParser with the given parsers.
        /// </summary>
        public SequenceCollectionParser(Parser[] Parsers)
            : base(Parsers)
        {
        }
 
        /* public */
        /// <summary>
        /// Accept 
        /// </summary>
        /// <param name="pv">the visitor to Accept</param>
        /// <param name="visited">a "visitor" and a collection of previously visited parsers.</param>
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitSequence(this, visited);
        }
        /// <summary>
        /// Given a set of assemblies, this method matches this
        /// sequence against all of them, and returns a new set 
        /// of the assemblies that result from the matches.
        /// </summary>
        /// <param name="In">a vector of assemblies to Match against</param>
        /// <returns>Returns a ArrayList of assemblies that result from   matching against a beginning set of assemblies</returns>
        public override ArrayList Match(ArrayList In)
        {
            ArrayList Out = In;

            foreach (Parser P in FSubParsers)
            {
                Out = P.MatchAndAssemble(Out);
                if (Out.Count == 0)
                    return Out;
            }

            return Out;
        }

        /*
        public Vector match(Vector in) {
            Vector out = in;
            Enumeration e = subparsers.elements();
            while (e.hasMoreElements()) {
                Parser p = (Parser) e.nextElement();
                out = p.matchAndAssemble(out);
                if (out.isEmpty()) {
                    return out;
                }
            }
            return out;
        } 
         */

        /// <summary>
        /// Create a random expansion for each parser in this  sequence and return a collection of all these expansions.
        /// </summary>
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

    }
}
