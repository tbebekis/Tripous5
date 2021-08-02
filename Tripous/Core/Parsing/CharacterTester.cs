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
    /// <summary>
    /// 
    /// </summary>
    public class CharacterTester : ParserTester
    {
        /**
         * 
         */
        public CharacterTester(Parser p) : base(p)
        {
        }
        /**
         * assembly method comment.
         */
        protected override Assembly CreateAssembly(string s)
        {
            return new CharacterAssembly(s);
        }
        /**
         * 
         * @return java.lang.string
         */
        protected override string Separator()
        {
            return "";
        }
    }
}
