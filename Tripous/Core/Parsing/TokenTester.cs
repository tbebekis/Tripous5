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
    public class TokenTester : ParserTester
    {
        /**
         * 
         */
        public TokenTester(Parser p) : base(p)
        {
        }
        /**
         * assembly method comment.
         */
        protected override Assembly CreateAssembly(string s)
        {
            return new TokenAssembly(s);
        }
    }
}
