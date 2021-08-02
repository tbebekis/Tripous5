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

namespace Tripous.Tokenizing
{
    /**
     * Objects of this class represent a type of token, such
     * as "number" or "word".
     * 
     *
     *
     *
     */
    public class TokenKind
    {
        /// <summary>
        /// 
        /// </summary>
        protected string FName;
        /**
         * Creates a token type of the given name.
         */
        public TokenKind(string Name)
        {
            this.FName = Name;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return FName; } }


    }
}
