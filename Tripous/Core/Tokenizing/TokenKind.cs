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
    /// <summary>
    /// Objects of this class represent a type of token, such as "number" or "word".
    /// </summary>
    public class TokenKind
    { 

        /// <summary>
        /// Creates a token type of the given name.
        /// </summary>
        public TokenKind(string Name)
        {
            this.Name = Name;
        }


        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name)? Name: base.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; protected set; }


    }
}
