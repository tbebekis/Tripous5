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
    public abstract class ParserTester
    {
        /// <summary>
        /// 
        /// </summary>
        protected Parser FParser;
        /// <summary>
        /// 
        /// </summary>
        protected bool logTestStrings = true;

        /// <summary>
        /// Subclasses must override this, to produce an assembly from the given (random) string.
        /// </summary>
        protected abstract Assembly CreateAssembly(string s);
        /// <summary>
        /// Generate a random language element, and return true if the parser cannot unambiguously parse it.
        /// </summary>
        protected bool CanGenerateProblem(int depth)
        {
            string s = FParser.RandomInput(depth, Separator());
            LogTestString(s);
            Assembly a = CreateAssembly(s);
            a.Target = FreshTarget();
            ArrayList In = new ArrayList();
            In.Add(a);
            ArrayList Out = CompleteMatches(FParser.Match(In));
            if (Out.Count != 1)
            {
                LogProblemFound(s, Out.Count);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Give subclasses a chance to provide fresh target at the beginning of a parse.
        /// </summary>
        protected ICloneable FreshTarget()
        {
            return null;
        }
        /// <summary>
        /// This method is broken out to allow subclasses to create
        /// less verbose tester, or to direct logging to somewhere
        /// other than System.out.
        /// </summary>
        protected string LogDepthChange(int depth)
        {
            //System.out.println("Testing depth " + depth + "...");
            return string.Format("Testing depth {0} ...", depth.ToString());
        }
        /// <summary>
        /// This method is broken out to allow subclasses to create
        /// less verbose tester, or to direct logging to somewhere
        /// other than System.out.
        /// </summary>
        protected string LogPassed()
        {
            return "No problems found.";//System.out.println("No problems found.");
        }
        /// <summary>
        /// This method is broken out to allow subclasses to create
        /// less verbose tester, or to direct logging to somewhere
        /// other than System.out.
        /// </summary>
        protected string LogProblemFound(string s, int matchSize)
        {

            StringBuilder Buf = new StringBuilder();
            Buf.Append("Problem found for string:");
            Buf.Append(Environment.NewLine);

            Buf.Append(s);
            Buf.Append(Environment.NewLine);

            if (matchSize == 0)
                Buf.Append("Parser cannot Match this apparently valid string.");
            else Buf.Append(string.Format("The parser found {0} ways to parse this string.", matchSize.ToString()));

            return Buf.ToString();
        }
        /// <summary>
        /// This method is broken out to allow subclasses to create
        /// less verbose tester, or to direct logging to somewhere
        /// other than System.out.
        /// </summary>
        protected string LogTestString(string s)
        {
            if (logTestStrings)
                return "    Testing string " + s;
            else return " ";

        }
        /// <summary>
        ///  By default, place a blank between randomly generated "words" of a language.
        /// </summary>
        protected virtual string Separator()
        {
            return " ";
        }


        /* construction */
        /// <summary>
        /// Constructs a tester for the given parser.
        /// </summary>
        public ParserTester(Parser p)
        {
            this.FParser = p;
        }
 
        /* public */
        /// <summary>
        /// Return a subset of the supplied vector of assemblies,
        /// filtering for assemblies that have been completely matched.
        /// </summary>
        /// <param name="In">a collection of partially or completely matched assemblies</param>
        /// <returns>Returns a collection of completely matched assemblies</returns>
        static public ArrayList CompleteMatches(ArrayList In)
        {
            ArrayList Out = new ArrayList();
            for (int i = 0; i < In.Count; i++)
            {
                Assembly a = (Assembly)In[i];
                if (!a.HasMoreElements)
                    Out.Add(a);

            }
            return Out;
        }

        /// <summary>
        /// Set the bool which determines if this class displays every test string.
        /// </summary>
        /// <param name="logTestStrings">true, if the user wants to see  every test string</param>
        public void SetLogTestStrings(bool logTestStrings)
        {
            this.logTestStrings = logTestStrings;
        }
        /// <summary>
        /// Create a series of random language elements, and test
        /// that the parser can unambiguously parse each one.
        /// </summary>
        public string Test()
        {
            StringBuilder Buf = new StringBuilder();

            for (int depth = 2; depth < 8; depth++)
            {
                Buf.Append(LogDepthChange(depth));
                Buf.Append(Environment.NewLine);

                for (int k = 0; k < 100; k++)
                    if (CanGenerateProblem(depth))
                        return Buf.ToString();

            }
            Buf.Append(LogPassed());
            return Buf.ToString();
        }
    }
}
