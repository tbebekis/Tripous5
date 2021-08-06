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
    /// This class provides a "visitor" hierarchy in support of
    /// the Visitor pattern -- see the book, "Design Patterns" for
    /// an explanation of this pattern.
    /// </summary>
    public abstract class ParserVisitor
    {
 
        /// <summary>
        /// Visit an alternation.
        /// </summary>
        /// <param name="a">the parser to visit</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public abstract void VisitAlternation(AlternationCollectionParser a, ArrayList visited);
        /// <summary>
        /// Visit an empty parser.
        /// </summary>
        /// <param name="e">the parser to visit</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public abstract void VisitEmpty(EmptyParser e, ArrayList visited);
        /// <summary>
        /// Visit a repetition.
        /// </summary>
        /// <param name="r">the parser to visit</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public abstract void VisitRepetition(RepetitionParser r, ArrayList visited);
        /// <summary>
        /// Visit a sequence.
        /// </summary>
        /// <param name="s">the parser to visit</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public abstract void VisitSequence(SequenceCollectionParser s, ArrayList visited);
        /// <summary>
        /// Visit a terminal.
        /// </summary>
        /// <param name="t">the parser to visit</param>
        /// <param name="visited">a collection of previously visited parsers</param>
        public abstract void VisitTerminal(TerminalParser t, ArrayList visited);
    }
}
