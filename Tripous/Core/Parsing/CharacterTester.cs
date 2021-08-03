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
    /// This class generates random language elements for a parser and tests that the parser can Accept them.
    /// </summary>
    public class CharacterTester : ParserTester
    {
        /// <summary>
        /// Subclasses must override this, to produce an assembly from the given (random) string.
        /// </summary>
        protected override Assembly CreateAssembly(string s)
        {
            return new CharacterAssembly(s);
        }
        /// <summary>
        ///  By default, place a blank between randomly generated "words" of a language.
        /// </summary>
        protected override string Separator()
        {
            return "";
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public CharacterTester(Parser p) :
            base(p)
        {
        }

    }
}
