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
    /// A Terminal parser is a parser that can match an assembly without the aid of other parsers. 
    /// <para>
    /// Terminal parsers get their name from the fact 
    /// that they terminate the recursion inherent in the idea that parsers are compositions of parsers.
    /// </para>
    /// <para>
    /// A Terminal parser is a parser that is not a composition of other parsers. 
    /// Terminals are "terminal" because they do not pass matching work on to other parsers. 
    /// </para>
    /// <para>
    /// The criterion that terminals use to check a Match is something other than another parser. 
    /// </para>
    /// <para>
    /// Terminals are also the only parsers that advance an assembly. 
    /// </para>
    /// <para>
    /// IMPORTANT: Each time a terminal parser asks an assembly for its nextElement(), 
    /// the terminal parser must anticipate receiving either a character or a token.
    /// The terminal parser decides whether the character or token it receives is a match.
    /// </para>
    /// </summary>
    public class TerminalParser : Parser
    {
        /// <summary>
        /// whether or not this terminal parser should Push itself upon an assembly's stack after a successful Match
        /// </summary>
        protected bool FDiscard = false;

        /// <summary>
        /// Returns an assembly equivalent to the supplied assembly, 
        /// except that this terminal will have been removed from the
        /// front of the assembly. As with any parser, if the 
        /// Match succeeds, this terminal's assembler will work on 
        /// the assembly. If the Match fails, this method returns null.
        /// </summary>
        /// <param name="In">the assembly to Match against</param>
        /// <returns>Returns a copy of the incoming assembly, advanced by this  terminal</returns>
        protected Assembly MatchOneAssembly(Assembly In)
        {
            if (!In.HasMoreElements)
                return null;

            if (Qualifies(In.Peek()))
            {
                Assembly Out = (Assembly)In.Clone();
                object o = Out.NextElement();
                if (!FDiscard)
                {
                    Out.Push(o);
                }
                return Out;
            }
            return null;
        }

        /* construction */
        /// <summary>
        /// Constructs an unnamed terminal.
        /// </summary>
        public TerminalParser()
        {
        }
        /// <summary>
        /// Constructs a terminal with the given name.
        /// </summary>
        public TerminalParser(string name) : base(name)
        {
        }

        /* public */
        /// <summary>
        /// Given a collection of assemblies, this method matches 
        /// this terminal against all of them, and returns a new 
        /// collection of the assemblies that result from the  matches.
        /// </summary>
        /// <param name="In">a ArrayList of assemblies that result from  matching against a beginning set of assemblies</param>
        /// <returns>Returns a list of assemblies to Match against</returns>
        public override List<Assembly> Match(List<Assembly> In)
        {
            List<Assembly> ResultList = new List<Assembly>();
            for (int i = 0; i < In.Count; i++)
            {
                Assembly a = (Assembly)In[i];
                Assembly b = MatchOneAssembly(a);
                if (b != null)
                    ResultList.Add(b);
            }
            return ResultList;
        }

        /// <summary>
        /// Accept a "visitor" and a collection of previously visited parsers.
        /// </summary>
        /// <param name="pv">the visitor to Accept</param>
        /// <param name="visited">Returns a collection of previously visited parsers</param>
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitTerminal(this, visited);
        }
        /// <summary>
        /// A convenience method that sets discarding to be true. Returns this instance.
        /// </summary>
        public TerminalParser Discard()
        {
            return SetDiscard(true);
        }


        /// <summary>
        /// The mechanics of matching are the same for many terminals,
        /// except for the check that the next element on the assembly
        /// Qualifies as the type of terminal this terminal looks for.
        /// This method performs that check.
        /// </summary>
        /// <param name="o">an element from a assembly</param>
        /// <returns>Returns true, if the object is the kind of terminal this  parser seeks</returns>
        public virtual bool Qualifies(object o)
        {
            return true;
        }
        /// <summary>
        /// By default, create a collection with this terminal's  string representation of itself. 
        /// (Most subclasses override this.)
        /// </summary>
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            ArrayList v = new ArrayList();
            v.Add(this.ToString());
            return v;
        }
        /// <summary>
        /// By default, terminals Push themselves upon a assembly's 
        /// stack, after a successful Match. This routine will turn
        /// off (or turn back on) that behavior. Returns this instance
        /// </summary>
        /// <param name="Discard">true, if this terminal should Push itself on a assembly's stack</param>
        /// <returns>Returns this instance</returns>
        public TerminalParser SetDiscard(bool Discard)
        {
            this.FDiscard = Discard;
            return this;
        }
        /// <summary>
        /// Returns a textual description of this parser.
        /// </summary>
        public override string UnvisitedString(List<Parser> visited)
        {
            return "any";
        }
    }
}
