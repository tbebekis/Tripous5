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

    /**
     * This class abstracts the behavior common to parsers
     * that consist of a series of other parsers.
     * 
     *
     * 
     *
     */
    public abstract class CollectionParser : Parser
    {
        /**
         * the parsers this parser is a collection of
         */
        protected ArrayList FSubParsers = new ArrayList();
        /**
         * Supports subclass constructors with no arguments.
         */
        public CollectionParser()
        {
        }
        /**
         * Supports subclass constructors with a name argument
         *
         * @param   string   the name of this parser
         */
        public CollectionParser(string name) : base(name)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parser.
         */
        public CollectionParser(Parser p)
        {
            FSubParsers.Add(p);
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public CollectionParser(Parser p1, Parser p2)
        {
            FSubParsers.Add(p1);
            FSubParsers.Add(p2);
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public CollectionParser(Parser p1, Parser p2, Parser p3)
        {
            FSubParsers.Add(p1);
            FSubParsers.Add(p2);
            FSubParsers.Add(p3);
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public CollectionParser(Parser p1, Parser p2, Parser p3, Parser p4)
        {
            //
            FSubParsers.Add(p1);
            FSubParsers.Add(p2);
            FSubParsers.Add(p3);
            FSubParsers.Add(p4);
        }
        /**
         * Adds a parser to the collection.
         *
         * @param   Parser   the parser to Add
         *
         * @return   this
         */
        public CollectionParser Add(Parser e)
        {
            FSubParsers.Add(e);
            return this;
        }
        /**
         * Return this parser's FSubParsers.
         *
         * @return   ArrayList   this parser's FSubParsers
         */
        public ArrayList GetSubParsers()
        {
            return FSubParsers;
        }
        /**
         * Helps to textually describe this CollectionParser.
         *
         * @returns   the string to place between parsers in 
         *            the collection
         */
        protected abstract string ToStringSeparator();
        /**
         * Returns a textual description of this parser.
         */
        public override string UnvisitedString(ArrayList visited)
        {
            StringBuilder buf = new StringBuilder("<");
            bool needSeparator = false;

            for (int i = 0; i < FSubParsers.Count; i++)
            {
                if (needSeparator)
                    buf.Append(ToStringSeparator());

                Parser next = (Parser)FSubParsers[i];
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
