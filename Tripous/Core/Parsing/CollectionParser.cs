/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/


namespace Tripous.Parsing
{

    /// <summary>
    /// This class abstracts the behavior common to parsers that consist of a series of other parsers.
    /// </summary>
    public abstract class CollectionParser : Parser
    {
        /// <summary>
        /// the parsers this parser is a collection of
        /// </summary>
        protected ArrayList fSubParsers = new ArrayList();

        /// <summary>
        /// Helps to textually describe this CollectionParser.Returns the string to place between parsers in the collection
        /// </summary>
        protected abstract string ToStringSeparator();


        /* construction */
        /// <summary>
        /// Constructor. Supports subclass constructors with no arguments.
        /// </summary>
        public CollectionParser()
        {
        }
        /// <summary>
        /// Constructor. Supports subclass constructors with a name argument
        /// </summary>
        public CollectionParser(string name) : base(name)
        {
        }
        /// <summary>
        /// A convenient way to construct a CollectionParser with the given parser.
        /// </summary>
        public CollectionParser(Parser p)
        {
            fSubParsers.Add(p);
        }
        /// <summary>
        /// A convenient way to construct a CollectionParser with the given parsers.
        /// </summary>
        public CollectionParser(Parser[] Parsers)
        {
            fSubParsers.AddRange(Parsers);
        }


        /* public */
        /// <summary>
        /// Adds a parser to the collection. Returns this instance.
        /// </summary>
        public CollectionParser Add(Parser e)
        {
            fSubParsers.Add(e);
            return this;
        }
        /// <summary>
        /// Return this parser's FSubParsers.
        /// </summary>
        public ArrayList GetSubParsers()
        {
            return fSubParsers;
        }

        /// <summary>
        /// Returns a textual description of this parser.
        /// <para>Used in avoiding to produce the textual representation of this instance twice.</para>
        /// </summary>
        /// <param name="visited">A list of parsers already printed </param>
        /// <returns>Returns a textual version of this parser, avoiding recursion</returns>
        public override string UnvisitedString(List<Parser> visited)
        {
            StringBuilder buf = new StringBuilder("<");
            bool needSeparator = false;

            for (int i = 0; i < fSubParsers.Count; i++)
            {
                if (needSeparator)
                    buf.Append(ToStringSeparator());

                Parser next = (Parser)fSubParsers[i];
                buf.Append(next.ToString(visited));
                needSeparator = true;
            }


            /*
            Enumeration e = FSubParsers.elements();
            while (e.HasMoreElements()) 
            {
               if (needSeparator) 
               {
                  buf.Append(ToStringSeparator());
               }
               Parser next = (Parser) e.nextElement();
               buf.Append(next.ToString(visited));
               needSeparator = true;
            }
            */

            buf.Append(">");
            return buf.ToString();
        }
    }
}
