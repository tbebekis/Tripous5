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
 * This class provides a "visitor" hierarchy in support of
 * the Visitor pattern -- see the book, "Design Patterns" for
 * an explanation of this pattern.
 * 
 *
 * 
 *
 */
    public abstract class ParserVisitor
    {
        /**
         * Visit an alternation.
         *
         * @param   Alternation   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitAlternation(Alternation a, ArrayList visited);
        /**
         * Visit an empty parser.
         *
         * @param   Empty   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitEmpty(Empty e, ArrayList visited);
        /**
         * Visit a repetition.
         *
         * @param   Repetition   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitRepetition(Repetition r, ArrayList visited);
        /**
         * Visit a sequence.
         *
         * @param   Sequence   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitSequence(Sequence s, ArrayList visited);
        /**
         * Visit a terminal.
         *
         * @param   Terminal   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitTerminal(Terminal t, ArrayList visited);
    }
}
