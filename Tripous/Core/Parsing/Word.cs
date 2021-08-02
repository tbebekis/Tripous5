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
 * A Word matches a word from a token assembly.
 * 
 *
 * 
 *
 */
    public class Word : Terminal
    {

        /**
         * Returns true if an assembly's next element is a word.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is a word
         */
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsWord();
        }
        /**
         * Create a set with one random word (with 3 to 7 characters).
         * 
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            int n = Random.Next(5) + 3;

            char[] letters = new char[n];
            for (int i = 0; i < n; i++)
            {

                int c = Random.Next(26) + 'a';
                letters[i] = (char)c;
            }

            ArrayList v = new ArrayList();
            v.Add(new string(letters));
            return v;
        }
        /**
         * Returns a textual description of this parser.
         *
         * @param   vector   a list of parsers already printed in 
         *                   this description
         * 
         * @return   string   a textual description of this parser
         *
         * @see Parser#ToString()
         */
        public override string UnvisitedString(ArrayList visited)
        {
            return "Word";
        }
    }
}
