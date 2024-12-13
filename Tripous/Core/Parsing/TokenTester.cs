/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/

namespace Tripous.Parsing
{
    using Tripous.Tokenizing;

    /// <summary>
    /// Token parser tester
    /// </summary>
    public class TokenTester : ParserTester
    {
        /// <summary>
        /// Subclasses must override this, to produce an assembly from the given (random) string.
        /// </summary>
        protected override Assembly CreateAssembly(string s)
        {
            return new TokenAssembly(s);
        }

        /// <summary>
        /// Constructs a tester for the given parser.
        /// </summary>
        public TokenTester(Parser p) :
            base(p)
        {
        }

    }
}
