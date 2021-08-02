//
// Java Code by Steven J. Metsker, from the book
// Building Parsers With Java
// 
// Adaptation to C#, modifications and additions
// by Theo Bebekis
//



namespace bt.Parsers
{

    using System;
    using System.Text;
    using System.Collections;


    /**
     * An assembly maintains a stream of language elements along 
     * with FStack and FTarget objects.
     *
     * Parsers use assemblers to record progress at 
     * recognizing language elements from assembly's string. 
     * 
     *
     * 
     *
     * 
     */
    public abstract class Assembly : ICloneable
    {

        /**
         * a place to keep track of consumption progress
         */
        protected Stack FStack = new Stack();

        /** Another place to record progress; this is just an object. 
         * If a parser were recognizing an HTML page, for 
         * example, it might create a Page object early, and store it 
         * as an assembly's "FTarget". As its recognition of the HTML 
         * progresses, it could use the FStack to build intermediate 
         * results, like a heading, and then apply them to the FTarget 
         * object.
         */
        protected ICloneable FTarget;

        /**
         * which element is next
         */
        protected int FIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        protected virtual Assembly CloneProperties(Assembly A)
        {
            try
            {
                //deep clone the FStack
                object[] StackElements = FStack.ToArray();
                Array.Reverse(StackElements);

                for (int i = 0; i < StackElements.Length; i++)
                    if (StackElements[i] is ICloneable)
                        A.FStack.Push(((ICloneable)StackElements[i]).Clone());
                    else A.FStack.Push(StackElements[i]);

                if (FTarget != null)
                    A.FTarget = (ICloneable)FTarget.Clone();

                return A;
            }
            catch
            {
                throw new Exception("CloneProperties() failed");
            }
        }


        /**
         * Return a copy of this object.
         *
         * @return a copy of this object
         */
        public abstract object Clone();

        /**
         * Returns the elements of the assembly that have been 
         * Consumed, separated by the specified delimiter.
         *
         * @param   string   the mark to show between Consumed
         *                   elements
         *
         * @return   the elements of the assembly that have been 
         *           Consumed
         */
        public abstract string Consumed(string delimiter);
        /**
         * Returns the default string to show between elements.
         *
         * @return   the default string to show between elements
         */
        public abstract string DefaultDelimiter();
        /**
         * Returns the number of elements that have been Consumed.
         *
         * @return   the number of elements that have been Consumed
         */
        public int ElementsConsumed()
        {
            return FIndex;
        }
        /**
         * Returns the number of elements that have not been Consumed.
         *
         * @return   the number of elements that have not been 
         *           Consumed
         */
        public int ElementsRemaining()
        {
            return Length() - ElementsConsumed();
        }
        /**
         * Removes this assembly's FStack.
         *
         * @return   this assembly's FStack
         */
        public Stack GetStack()
        {
            return FStack;
        }
        /**
         * Returns the object identified as this assembly's "FTarget". 
         * Clients can set and retrieve a FTarget, which can be a 
         * convenient supplement as a place to work, in addition to 
         * the assembly's FStack. For example, a parser for an 
         * HTML file might use a web page object as its "FTarget". As 
         * the parser recognizes markup commands like , it 
         * could apply its findings to the FTarget.
         * 
         * @return   the FTarget of this assembly
         */
        public object GetTarget()
        {
            return FTarget;
        }
        /**
         * Returns true if this assembly has unconsumed elements.
         *
         * @return   true, if this assembly has unconsumed elements
         */
        public bool HasMoreElements()
        {
            return ElementsConsumed() < Length();
        }
        /**
         * Returns the number of elements in this assembly.
         *
         * @return   the number of elements in this assembly
         */
        public abstract int Length();
        /**
         * Shows the next object in the assembly, without removing it
         *
         * @return   the next object
         *
         */
        public abstract object Peek();
        /**
         * Removes the object at the top of this assembly's FStack and
         * returns it.
         *
         * @return   the object at the top of this assembly's FStack
         *
         * @exception   EmptyStackException   if this FStack is empty
         */
        public object Pop()
        {
            return FStack.Pop();
        }
        /**
         * Pushes an object onto the top of this assembly's FStack. 
         *
         * @param   object   the object to be pushed
         */
        public void Push(object o)
        {
            FStack.Push(o);
        }
        /**
         * Returns the elements of the assembly that remain to be 
         * Consumed, separated by the specified delimiter.
         *
         * @param   string   the mark to show between unconsumed 
         *                   elements
         *
         * @return   the elements of the assembly that remain to be 
         *           Consumed
         */
        public abstract string Remainder(string delimiter);
        /**
         * Sets the FTarget for this assembly. Targets must implement 
         * <code>Clone()</code> as a public method.
         * 
         * @param   FTarget   a publicly cloneable object
         */
        public void SetTarget(ICloneable FTarget)
        {
            this.FTarget = FTarget;
        }
        /**
         * Returns true if this assembly's FStack is empty.
         *
         * @return   true, if this assembly's FStack is empty
         */
        public bool StackIsEmpty()
        {
            return FStack.Count == 0;
        }
        /**
         * Returns a textual description of this assembly.
         *
         * @return   a textual description of this assembly
         */
        public override string ToString()
        {
            string delimiter = DefaultDelimiter();
            return FStack +
               Consumed(delimiter) + "^" + Remainder(delimiter);
        }
        /**
         * Put back n objects
         *
         */
        public void UnGet(int n)
        {
            FIndex -= n;
            if (FIndex < 0)
            {
                FIndex = 0;
            }
        }
        /** λείπει από του Metsker */
        public abstract string NextElement();
    }


    /**
     * A CharacterAssembly is an Assembly whose elements are 
     * characters.
     * 
     *
     * 
     *
     * 
     * @see Assembly
     */

    public class CharacterAssembly : Assembly
    {
        /**
         * the string to consume
         */
        protected string FBuffer;

        /**
         * Constructs a CharacterAssembly from the given string.
         *
         * @param   string   the string to consume
         *
         * @return   a CharacterAssembly that will consume the 
         *           supplied string
         */
        public CharacterAssembly(string Buffer)
        {
            this.FBuffer = Buffer;
        }
        /**
         * Returns a textual representation of the amount of this 
         * characterAssembly that has been Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that has been Consumed
         */
        public override string Consumed(string delimiter)
        {
            if (delimiter.Equals(""))
                return FBuffer.Substring(0, ElementsConsumed());

            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < ElementsConsumed(); i++)
            {
                if (i > 0)
                    buf.Append(delimiter);

                buf.Append(FBuffer[i]);
            }
            return buf.ToString();
        }
        /**
         * Returns the default string to show between elements 
         * Consumed or remaining.
         *
         * @return   the default string to show between elements 
         *           Consumed or remaining
         */
        public override string DefaultDelimiter()
        {
            return "";
        }
        /**
         * Returns the number of elements in this assembly.
         *
         * @return   the number of elements in this assembly
         */
        public override int Length()
        {
            return FBuffer.Length;
        }
        /**
         * Returns the next character.
         *
         * @return   the next character from the associated token 
         *           string
         *
         * @exception  ArrayIndexOutOfBoundsException  if there are 
         *             no more characters in this assembly's string
         */
        public override string NextElement()
        {
            return FBuffer[FIndex++].ToString(); //      string.charAt(index++)
        }
        /**
         * Shows the next object in the assembly, without removing it
         *
         * @return   the next object
         */
        public override object Peek()
        {
            if (FIndex < Length())
                return FBuffer[FIndex];   //new Character(string.charAt(FIndex));
            else return null;
        }
        /**
         * Returns a textual representation of the amount of this 
         * characterAssembly that remains to be Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that remains to be Consumed
         */
        public override string Remainder(string delimiter)
        {
            if (delimiter.Equals(""))
                return FBuffer.Substring(ElementsConsumed());

            StringBuilder buf = new StringBuilder();
            for (int i = ElementsConsumed(); i < FBuffer.Length; i++)
            {

                if (i > ElementsConsumed())
                    buf.Append(delimiter);

                buf.Append(FBuffer[i]);
            }
            return buf.ToString();
        }
        /**
        */
        public override object Clone()
        {
            return CloneProperties(new CharacterAssembly(FBuffer));
        }


    }








    /**
     * A TokenAssembly is an Assembly whose elements are Tokens.
     * Tokens are, roughly, the chunks of text that a <code>
     * Tokenizer</code> returns.
     * 
     *
     * 
     *
     */

    public class TokenAssembly : Assembly
    {
        /**
         * the "string" of tokens this assembly will consume
         */
        protected TokenString FTokenString;

        /**
         * Constructs a TokenAssembly on a TokenString constructed 
         * from the given string.
         *
         * @param   string   the string to consume
         *
         * @return   a TokenAssembly that will consume a tokenized 
         *           version of the supplied string
         */
        public TokenAssembly(string s) : this(new TokenString(s))
        {
        }
        /**
         * Constructs a TokenAssembly on a TokenString constructed 
         * from the given Tokenizer.
         *
         * @param   Tokenizer   the tokenizer to consume tokens 
         *                      from
         *
         * @return   a TokenAssembly that will consume a tokenized 
         *           version of the supplied Tokenizer
         */
        public TokenAssembly(Tokenizer t) : this(new TokenString(t))
        {
        }
        /**
         * Constructs a TokenAssembly from the given TokenString.
         *
         * @param   FTokenString   the FTokenString to consume
         *
         * @return   a TokenAssembly that will consume the supplied 
         *           TokenString
         */
        public TokenAssembly(TokenString TokenString)
        {
            this.FTokenString = TokenString;
        }
        /**
         * Returns a textual representation of the amount of this 
         * tokenAssembly that has been Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that has been Consumed
         */
        public override string Consumed(string delimiter)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < ElementsConsumed(); i++)
            {
                if (i > 0)
                    buf.Append(delimiter);
                buf.Append(FTokenString.TokenAt(i));
            }
            return buf.ToString();
        }
        /**
         * Returns the default string to show between elements 
         * Consumed or remaining.
         *
         * @return   the default string to show between elements 
         *           Consumed or remaining
         */
        public override string DefaultDelimiter()
        {
            return "/";
        }
        /**
         * Returns the number of elements in this assembly.
         *
         * @return   the number of elements in this assembly
         */
        public override int Length()
        {
            return FTokenString.Length();
        }
        /**
         * Returns the next token.
         *
         * @return   the next token from the associated token string.
         *
         * @exception  ArrayIndexOutOfBoundsException  if there are no 
         *             more tokens in this tokenizer's string.
         */
        public override string NextElement()
        {
            return FTokenString.TokenAt(FIndex++).ToString();
        }
        /**
         * Shows the next object in the assembly, without removing it
         *
         * @return   the next object
         *
         */
        public override object Peek()
        {
            if (FIndex < Length())
                return FTokenString.TokenAt(FIndex);
            else return null;
        }
        /**
         * Returns a textual representation of the amount of this 
         * tokenAssembly that remains to be Consumed.
         *
         * @param   delimiter   the mark to show between Consumed 
         *                      elements
         *
         * @return   a textual description of the amount of this 
         *           assembly that remains to be Consumed
         */
        public override string Remainder(string delimiter)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = ElementsConsumed(); i < FTokenString.Length(); i++)
            {
                if (i > ElementsConsumed())
                    buf.Append(delimiter);

                buf.Append(FTokenString.TokenAt(i));
            }
            return buf.ToString();
        }
        /**
function TZTokenAssembly.Clone: IZInterface;
begin
    Result := CloneProperties(TZTokenAssembly.Create(FTokenString.Clone as IZTokenString));
end;

        */
        public override object Clone()
        {
            TokenString TS = (TokenString)FTokenString.Clone();
            Assembly A = new TokenAssembly(TS);
            return CloneProperties(A);
        }

    }






    /**
     * Parsers that have an Assembler ask it to work on an
     * assembly after a successful match.
     *  
     * By default, terminals Push their matches on a assembly's
     * stack after a successful match. 
     *  
     * Parsers recognize text, and assemblers provide any 
     * sort of work that should occur after this recognition. 
     * This work usually has to do with the state of the assembly,
     * which is why assemblies have a stack and a target. 
     * Essentially, parsers trade advancement on a assembly 
     * for work on the assembly's stack or target.
     * 
     *
     * 
     *
     */
    public abstract class Assembler
    {
        /**
         * Returns a vector of the elements on an assembly's stack 
         * that appear before a specified fence.
         *  
         * Sometimes a parser will recognize a list from within 
         * a pair of parentheses or brackets. The parser can mark 
         * the beginning of the list with a fence, and then retrieve 
         * all the items that come after the fence with this method.
         *
         * @param   assembly   a assembly whose stack should contain 
         * some number of items above a fence marker
         *
         * @param   object   the fence, a marker of where to stop 
         *                   popping the stack
         * 
         * @return   ArrayList   the elements above the specified fence
         * 
         */
        public static ArrayList ElementsAbove(Assembly A, object Fence)
        {
            ArrayList Items = new ArrayList();

            while (!A.StackIsEmpty())
            {
                object Top = A.Pop();
                if (Top.Equals(Fence))
                {
                    break;
                }
                Items.Add(Top);//       items.addElement(top);
            }
            return Items;
        }
        /**
         * This is the one method all subclasses must implement. It 
         * specifies what to do when a parser successfully 
         * matches against a assembly.
         *
         * @param   Assembly   the assembly to work on
         */
        public abstract void WorkOn(Assembly a);
    }








    /**
    * A <code>Parser</code> is an object that recognizes the
    * elements of a language.
    *  
    * Each <code>Parser</code> object is either a <code>
    * Terminal</code> or a composition of other parsers.
    * The <code>Terminal</code> class is a subclass of <code>
    * Parser</code>, and is itself a hierarchy of 
    * parsers that recognize specific patterns of text. For 
    * example, a <code>Word</code> recognizes any word, and a 
    * <code>Literal</code> matches a specific string. 
    *  
    * In addition to <code>Terminal</code>, other subclasses of 
    * <code>Parser</code> provide composite parsers, 
    * describing sequences, alternations, and repetitions of 
    * other parsers. For example, the following <code>
    * Parser</code> objects culminate in a <code>good
    * </code> parser that recognizes a description of good 
    * coffee.
    *
    *
    *     Alternation adjective = new Alternation();
    *     adjective.Add(new Literal("steaming"));
    *     adjective.Add(new Literal("hot"));
    *     Sequence good = new Sequence();
    *     good.Add(new Repetition(adjective));
    *     good.Add(new Literal("coffee"));
    *     string s = "hot hot steaming hot coffee";
    *     Assembly a = new TokenAssembly(s);
    *     System.out.println(good.BestMatch(a));
    * 
    *
    * This prints out:
    *
    * 
    *     [hot, hot, steaming, hot, coffee]
    *     hot/hot/steaming/hot/coffee^
    * 
    *
    * The parser does not Match directly against a string, 
    * it matches against an Assembly.  The 
    * resulting assembly shows its stack, with four words on it, 
    * along with its sequence of tokens, and the index at the 
    * end of these. In practice, parsers will do some work 
    * on an assembly, based on the text they recognize. 
    * 
    *
    *
    * 
    */

    public abstract class Parser
    {
        /**
        * a FName to identify this parser
        */
        protected string FName;
        /**
        * an object that will work on an assembly whenever this 
        * parser successfully matches against the assembly
        */
        protected Assembler FAssembler;


        /**
        * Constructs a nameless parser.
        */
        public Parser()
        {
        }
        /**
        * Constructs a parser with the given FName.
        *
        * @param   string   A FName to be known by. For parsers
        *                   that are deep composites, a simple FName
        *                   identifying its purpose is useful.
        */
        public Parser(string Name)
        {
            this.FName = Name;
        }
        /**
        * Accepts a "visitor" which will perform some operation on
        * a parser structure. The book, "Design Patterns", explains
        * the visitor pattern.
        *
        * @param   ParserVisitor   the visitor to Accept
        */
        public void Accept(ParserVisitor pv)
        {
            Accept(pv, new ArrayList());
        }
        /**
        * Accepts a "visitor" along with a collection of previously
        * visited parsers.
        *
        * @param   ParserVisitor   the visitor to Accept
        *
        * @param   ArrayList   a collection of previously visited 
        *                   parsers.
        */
        public abstract void Accept(ParserVisitor pv, ArrayList visited);
        /**
        * Adds the elements of one vector to another.
        *
        * @param   v1   the vector to Add to
        *
        * @param   v2   the vector with elements to Add
        */
        public static void Add(ArrayList v1, ArrayList v2)
        {
            v1.AddRange(v2);
        }
        /**
        * Returns the most-matched assembly in a collection.
        *
        * @return   the most-matched assembly in a collection.
        *
        * @param   ArrayList   the collection to look through
        *
        */
        public Assembly Best(ArrayList v)
        {
            Assembly best = null;
            for (int i = 0; i < v.Count; i++)
            {
                Assembly A = (Assembly)v[i];

                if (!A.HasMoreElements())
                    return A;

                if (best == null)
                    best = A;
                else if (A.ElementsConsumed() > best.ElementsConsumed())
                    best = A;

            }

            return best;
        }
        /**
        * Returns an assembly with the greatest possible number of 
        * elements Consumed by matches of this parser.
        *
        * @return   an assembly with the greatest possible number of 
        *           elements Consumed by this parser
        *
        * @param   Assembly   an assembly to Match against
        *
        */
        public Assembly BestMatch(Assembly a)
        {
            ArrayList In = new ArrayList();
            In.Add(a);
            ArrayList Out = MatchAndAssemble(In);
            return Best(Out);
        }
        /**
        * Returns either null, or a completely matched version of 
        * the supplied assembly.
        *
        * @return   either null, or a completely matched version of the
        *           supplied assembly
        *
        * @param   Assembly   an assembly to Match against
        *
        */
        public Assembly CompleteMatch(Assembly a)
        {
            Assembly best = BestMatch(a);
            if (best != null && !best.HasMoreElements())
            {
                return best;
            }
            return null;
        }
        /**
        * Create a copy of a vector, cloning each element of
        * the vector.
        *
        * @param   in   the vector to copy
        *
        * @return   a copy of the input vector, cloning each 
        *           element of the vector
        */
        public static ArrayList ElementClone(ArrayList v)
        {
            ArrayList copy = new ArrayList();
            for (int i = 0; i < v.Count; i++)
            {
                Assembly A = (Assembly)v[i];
                copy.Add(A.Clone());
            }
            return copy;
        }
        /**
        * Returns the FName of this parser.
        *
        * @return   the FName of this parser
        *
        */
        public string GetName()
        {
            return FName;
        }
        /**
        * Given a set (well, a <code>ArrayList</code>, really) of 
        * assemblies, this method matches this parser against 
        * all of them, and returns a new set (also really a 
        * <code>ArrayList</code>) of the assemblies that result from 
        * the matches.
        *  
        * For example, consider matching the regular expression 
        * <code>a*</code> against the string <code>"aaab"</code>. 
        * The initial set of states is <code>{^aaab}</code>, where 
        * the ^ indicates how far along the assembly is. When 
        * <code>a*</code> matches against this initial state, it 
        * creates a new set <code>{^aaab, a^aab, aa^ab, 
        * aaa^b}</code>. 
        * 
        * @return   a ArrayList of assemblies that result from 
        *           matching against a beginning set of assemblies
        *
        * @param   ArrayList   a vector of assemblies to Match against
        *
        */
        public abstract ArrayList Match(ArrayList In);
        /**
        * Match this parser against an input state, and then
        * apply this parser's FAssembler against the resulting
        * state.
        *
        * @return   a ArrayList of assemblies that result from matching
        *           against a beginning set of assemblies
        *
        * @param   ArrayList   a vector of assemblies to Match against
        *
        */
        public ArrayList MatchAndAssemble(ArrayList In)
        {
            ArrayList Out = Match(In);
            if (FAssembler != null)
            {

                for (int i = 0; i < Out.Count; i++)
                    FAssembler.WorkOn((Assembly)Out[i]);

                /*
                 Enumeration e = Out.elements();
                 while (e.HasMoreElements()) 
                 {
                     FAssembler.WorkOn((Assembly) e.nextElement());
                 }
                 */
            }
            return Out;
        }
        /**
        * Create a random expansion for this parser, where a
        * concatenation of the returned collection will be a
        * language element.
        */
        public abstract ArrayList RandomExpansion(int maxDepth, int depth);
        /**
        * Return a random element of this parser's language.
        *
        * @return  a random element of this parser's language
        */
        public string RandomInput(int maxDepth, string separator)
        {
            StringBuilder buf = new StringBuilder();
            ArrayList E = RandomExpansion(maxDepth, 0);
            bool first = true;

            for (int i = 0; i < E.Count; i++)
            {
                if (!first)
                    buf.Append(separator);

                buf.Append(E[i]);
                first = false;
            }

            return buf.ToString();

        }
        /**
        * Sets the object that will work on an assembly whenever 
        * this parser successfully matches against the 
        * assembly.
        *
        * @param   Assembler   the FAssembler to apply
        *
        * @return   Parser   this
        */
        public Parser SetAssembler(Assembler Assembler)
        {
            this.FAssembler = Assembler;
            return this;
        }
        /**
        * Returns a textual description of this parser.
        *
        * @return   string   a textual description of this 
        *                    parser, taking care to avoid 
        *                    infinite recursion
        */
        public override string ToString()
        {
            return ToString(new ArrayList());
        }
        /**
        * Returns a textual description of this parser.
        * Parsers can be recursive, so when building a 
        * descriptive string, it is important to avoid infinite 
        * recursion by keeping track of the objects already 
        * described. This method keeps an object from printing 
        * twice, and uses <code>UnvisitedString</code> which 
        * subclasses must implement.
        * 
        * @param   ArrayList    a list of objects already printed 
        *
        * @return   a textual version of this parser,
        *           avoiding recursion
        */
        public string ToString(ArrayList visited)
        {
            if (FName != null)
                return FName;
            else if (visited.IndexOf(this) != -1)
                return "...";
            else
            {
                visited.Add(this);
                return UnvisitedString(visited);
            }
        }
        /**
        * Returns a textual description of this string.
        */
        public abstract string UnvisitedString(ArrayList visited);
    }



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





    /**
    * An <code>Alternation</code> object is a collection of 
    * parsers, any one of which can successfully Match against
    * an assembly.
    * 
    *
    *
    *
    */

    public class Alternation : CollectionParser
    {

        /**
        * Constructs a nameless alternation.
        */
        public Alternation()
        {
        }
        /**
        * Constructs an alternation with the given name.
        *
        * @param    name    a name to be known by
        */
        public Alternation(string name) : base(name)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parser.
        */
        public Alternation(Parser p) : base(p)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parsers.
        */
        public Alternation(Parser p1, Parser p2) : base(p1, p2)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parsers.
        */
        public Alternation(Parser p1, Parser p2, Parser p3) : base(p1, p2, p3)
        {
        }
        /**
        * A convenient way to construct a CollectionParser with the
        * given parsers.
        */
        public Alternation(Parser p1, Parser p2, Parser p3, Parser p4) : base(p1, p2, p3, p4)
        {
        }
        /**
        * Accept a "visitor" and a collection of previously visited
        * parsers.
        *
        * @param   ParserVisitor   the visitor to Accept
        *
        * @param   ArrayList   a collection of previously visited parsers
        */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitAlternation(this, visited);
        }
        /**
        * Given a set of assemblies, this method matches this 
        * alternation against all of them, and returns a new set
        * of the assemblies that result from the matches.
        *
        * @return   a ArrayList of assemblies that result from 
        *           matching against a beginning set of assemblies
        *
        * @param   ArrayList   a vector of assemblies to Match against
        *
        */
        public override ArrayList Match(ArrayList In)
        {
            ArrayList Out = new ArrayList();

            for (int i = 0; i < FSubParsers.Count; i++)
            {
                Parser p = (Parser)FSubParsers[i];
                Add(Out, p.MatchAndAssemble(In));
            }
            return Out;
        }
        /**
        * Create a random collection of elements that correspond to
        * this alternation.
        */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            if (depth >= maxDepth)
                return RandomSettle(maxDepth, depth);

            //double n = (double) FSubParsers.Count;
            Random Random = new Random();

            int i = Random.Next(FSubParsers.Count);
            Parser j = (Parser)FSubParsers[i];
            return j.RandomExpansion(maxDepth, depth++);

        }
        /**
        * This method is similar to RandomExpansion, but it will
        * pick a terminal if one is available.
        */
        protected ArrayList RandomSettle(int maxDepth, int depth)
        {

            // which alternatives are terminals?
            Parser P = null;

            ArrayList terms = new ArrayList();

            for (int i = 0; i < FSubParsers.Count; i++)
            {
                P = (Parser)FSubParsers[i];
                if (P is Terminal)
                    terms.Add(P);
            }


            /*
            Enumeration e = FSubParsers.elements();
            while (e.HasMoreElements()) 
            {
                Parser j = (Parser) e.nextElement();
                if (j is Terminal) 
                    terms.addElement(j);
            }
            */

            // pick one of the terminals or, if there are no
            // terminals, pick any subparser

            ArrayList which = terms;
            if (terms.Count == 0)
            {
                which = FSubParsers;
            }

            Random Random = new Random();

            int j = Random.Next(which.Count);
            P = (Parser)which[j];
            return P.RandomExpansion(maxDepth, depth++);




            /*
              double n = (double) which.size();
              int i = (int) (n * Math.random());
              Parser p = (Parser) which[i];
              return p.RandomExpansion(maxDepth, depth++);
              */
        }
        /**
        * Returns the string to show between the parsers this
        * parser is an alternation of.
        */
        protected override string ToStringSeparator()
        {
            return "|";
        }

    }



    /**
     * A <code>Sequence</code> object is a collection of 
     * parsers, all of which must in turn Match against an 
     * assembly for this parser to successfully Match.
     * 
     *
     * 
     *
     */

    public class Sequence : CollectionParser
    {

        /**
         * Constructs a nameless sequence.
         */
        public Sequence()
        {
        }
        /**
         * Constructs a sequence with the given name.
         *
         * @param    name    a name to be known by
         */
        public Sequence(string name) : base(name)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parser.
         */
        public Sequence(Parser p) : base(p)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public Sequence(Parser p1, Parser p2) : base(p1, p2)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public Sequence(Parser p1, Parser p2, Parser p3) : base(p1, p2, p3)
        {
        }
        /**
         * A convenient way to construct a CollectionParser with the
         * given parsers.
         */
        public Sequence(Parser p1, Parser p2, Parser p3, Parser p4) : base(p1, p2, p3, p4)
        {
        }
        /**
         * Accept a "visitor" and a collection of previously visited
         * parsers.
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitSequence(this, visited);
        }
        /**
         * Given a set of assemblies, this method matches this
         * sequence against all of them, and returns a new set 
         * of the assemblies that result from the matches.
         *
         * @return   a ArrayList of assemblies that result from 
         *           matching against a beginning set of assemblies
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            ArrayList Out = In;
            for (int i = 0; i < FSubParsers.Count; i++)
            {
                Parser P = (Parser)FSubParsers[i];
                Out = P.MatchAndAssemble(Out);
                if (Out.Count == 0)
                    return Out;
            }

            return Out;
        }
        /**
         * Create a random expansion for each parser in this 
         * sequence and return a collection of all these expansions.
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            ArrayList v = new ArrayList();
            for (int i = 0; i < FSubParsers.Count; i++)
            {
                Parser P = (Parser)FSubParsers[i];
                ArrayList w = P.RandomExpansion(maxDepth, depth++);

                for (int j = 0; j < w.Count; j++)
                    v.Add(w[j]);
            }
            return v;
        }
        /**
         * Returns the string to show between the parsers this
         * parser is a sequence of. This is an empty string,
         * since convention indicates sequence quietly. For
         * example, note that in the regular expression 
         * <code>(a|b)c</code>, the lack of a delimiter between
         * the expression in parentheses and the 'c' indicates a 
         * sequence of these expressions.
         */
        protected override string ToStringSeparator()
        {
            return "";
        }
    }




    /**
     * An <code>Empty</code> parser matches any assembly once, 
     * and applies its assembler that one time.
     *  
     * Language elements often contain empty parts. For example, 
     * a language may at some point allow a list of parameters
     * in parentheses, and may allow an empty list. An empty
     * parser makes it easy to Match, within the 
     * parenthesis, either a list of parameters or "empty".
     * 
     *
     * 
     *
     * 
     */
    public class Empty : Parser
    {
        /**
         * Accept a "visitor" and a collection of previously visited
         * parsers.
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitEmpty(this, visited);
        }
        /**
         * Given a set of assemblies, this method returns the set as
         * a successful Match.
         * 
         * @return   the input set of states
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            return ElementClone(In);
        }
        /**
        * There really is no way to expand an empty parser, so
        * return an empty vector.
        */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            return new ArrayList();
        }
        /**
        * Returns a textual description of this parser.
        */
        public override string UnvisitedString(ArrayList visited)
        {
            return " empty ";
        }
    }




    /**
     * A <code>Repetition</code> matches its underlying parser 
     * repeatedly against a assembly.
     * 
     *
     * 
     *
     */

    public class Repetition : Parser
    {
        /**
         * the parser this parser is a repetition of
         */
        protected Parser FSubParser;

        /**
         * the width of a random expansion
         */
        protected static int EXPWIDTH = 4;

        /**
         * an assembler to apply at the beginning of a Match
         */
        protected Assembler FPreAssembler;
        /**
         * Constructs a repetition of the given parser. 
         * 
         * @param   parser   the parser to repeat
         *
         * @return   a repetiton that will Match the given 
         *           parser repeatedly in successive matches
         */
        public Repetition(Parser p) : this(p, null)
        {
        }
        /**
         * Constructs a repetition of the given parser with the
         * given name.
         * 
         * @param   Parser   the parser to repeat
         *
         * @param   string   a name to be known by
         *
         * @return   a repetiton that will Match the given 
         *           parser repeatedly in successive matches
         */
        public Repetition(Parser SubParser, string name) : base(name)
        {
            this.FSubParser = SubParser;
        }
        /**
         * Accept a "visitor" and a collection of previously visited
         * parsers.
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitRepetition(this, visited);
        }
        /**
         * Return this parser's FSubParser.
         *
         * @return   Parser   this parser's FSubParser
         */
        public Parser GetSubParser()
        {
            return FSubParser;
        }
        /**
         * Given a set of assemblies, this method applies a preassembler
         * to all of them, matches its FSubParser repeatedly against each
         * of them, applies its post-assembler against each, and returns
         * a new set of the assemblies that result from the matches.
         *  
         * For example, matching the regular expression <code>a*
         * </code> against <code>{^aaab}</code> results in <code>
         * {^aaab, a^aab, aa^ab, aaa^b}</code>.
         *
         * @return   a ArrayList of assemblies that result from 
         *           matching against a beginning set of assemblies
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            if (FPreAssembler != null)
            {
                for (int i = 0; i < In.Count; i++)
                    FPreAssembler.WorkOn((Assembly)In[i]);
            }
            ArrayList Out = ElementClone(In);
            ArrayList s = In; // a working state
            while (s.Count > 0)            // !s.IsEmpty()
            {
                s = FSubParser.MatchAndAssemble(s);
                Add(Out, s);
            }
            return Out;
        }
        /**
         * Create a collection of random elements that correspond to
         * this repetition.
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            ArrayList v = new ArrayList();
            if (depth >= maxDepth)
            {
                return v;
            }

            Random Random = new Random();

            int n = Random.Next(EXPWIDTH);
            for (int j = 0; j < n; j++)
            {
                ArrayList w = FSubParser.RandomExpansion(maxDepth, depth++);
                for (int i = 0; i < w.Count; i++)
                    v.Add(w[i]);
            }
            return v;
        }
        /**
         * Sets the object that will work on every assembly before 
         * matching against it.
         *
         * @param   Assembler   the assembler to apply
         *
         * @return   Parser   this
         */
        public Parser SetPreAssembler(Assembler FPreAssembler)
        {
            this.FPreAssembler = FPreAssembler;
            return this;
        }
        /**
         * Returns a textual description of this parser.
         */
        public override string UnvisitedString(ArrayList visited)
        {
            return FSubParser.ToString(visited) + "*";
        }
    }




    /**
     * A <code>Terminal</code> is a parser that is not a 
     * composition of other parsers. Terminals are "terminal" 
     * because they do not pass matching work on to other 
     * parsers. The criterion that terminals use to check a 
     * Match is something other than another parser. Terminals 
     * are also the only parsers that advance an assembly. 
     *
     *
     * 
     * @version 1.0
     */

    public class Terminal : Parser
    {
        /**
         * whether or not this terminal should Push itself upon an
         * assembly's stack after a successful Match
         */
        protected bool FDiscard = false;

        /**
         * Constructs an unnamed terminal.
         */
        public Terminal()
        {
        }
        /**
         * Constructs a terminal with the given name.
         *
         * @param    string    A name to be known by.
         */
        public Terminal(string name) : base(name)
        {
        }
        /**
         * Accept a "visitor" and a collection of previously visited
         * parsers.
         *
         * @param   ParserVisitor   the visitor to Accept
         *
         * @param   ArrayList   a collection of previously visited parsers
         */
        public override void Accept(ParserVisitor pv, ArrayList visited)
        {
            pv.VisitTerminal(this, visited);
        }
        /**
         * A convenience method that sets discarding to be true.
         *
         * @return   this
         */
        public Terminal Discard()
        {
            return SetDiscard(true);
        }
        /**
         * Given a collection of assemblies, this method matches 
         * this terminal against all of them, and returns a new 
         * collection of the assemblies that result from the 
         * matches.
         *
         * @return   a ArrayList of assemblies that result from 
         *           matching against a beginning set of assemblies
         *
         * @param   ArrayList   a vector of assemblies to Match against
         *
         */
        public override ArrayList Match(ArrayList In)
        {
            ArrayList Out = new ArrayList();
            for (int i = 0; i < In.Count; i++)
            {
                Assembly a = (Assembly)In[i];
                Assembly b = MatchOneAssembly(a);
                if (b != null)
                    Out.Add(b);
            }
            return Out;
        }
        /**
         * Returns an assembly equivalent to the supplied assembly, 
         * except that this terminal will have been removed from the
         * front of the assembly. As with any parser, if the 
         * Match succeeds, this terminal's assembler will work on 
         * the assembly. If the Match fails, this method returns
         * null.
         *
         * @param   Assembly  the assembly to Match against
         *
         * @return a copy of the incoming assembly, advanced by this 
         *         terminal
         */
        protected Assembly MatchOneAssembly(Assembly In)
        {
            if (!In.HasMoreElements())
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
        /**
         * The mechanics of matching are the same for many terminals,
         * except for the check that the next element on the assembly
         * Qualifies as the type of terminal this terminal looks for.
         * This method performs that check.
         *
         * @param   object   an element from a assembly
         *
         * @return   true, if the object is the kind of terminal this 
         *           parser seeks
         */
        public virtual bool Qualifies(object o)
        {
            return true;
        }
        /**
         * By default, create a collection with this terminal's 
         * string representation of itself. (Most subclasses 
         * override this.)
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            ArrayList v = new ArrayList();
            v.Add(this.ToString());
            return v;
        }
        /**
         * By default, terminals Push themselves upon a assembly's 
         * stack, after a successful Match. This routine will turn
         * off (or turn back on) that behavior.
         * 
         * @param   bool   true, if this terminal should Push
         *                    itself on a assembly's stack
         *
         * @return   this
         */
        public Terminal SetDiscard(bool Discard)
        {
            this.FDiscard = Discard;
            return this;
        }
        /**
         * Returns a textual description of this parser.
         */
        public override string UnvisitedString(ArrayList visited)
        {
            return "any";
        }
    }




    /**
     * A Char matches a character from a character assembly.
     * 
     *
     * 
     *
     */
    public class CharTerm : Terminal
    {

        /**
         * Returns true every time, since this class assumes it is 
         * working against a CharacterAssembly.
         *
         * @param   object   ignored
         *
         * @return   true, every time, since this class assumes it is 
         *           working against a CharacterAssembly
         */
        public override bool Qualifies(object o)
        {
            return true;
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
            return "C";
        }
    }




    /**
     * A SpecificChar matches a specified character from a character 
     * assembly.
     * 
     *
     * 
     *
     */
    public class SpecificChar : Terminal
    {

        /**
         * the character to Match
         */
        protected char FChar;
        /**
         * Constructs a SpecificChar to Match the specified char.
         *
         * @param   char  the character to Match
         *
         * @return   a SpecificChar to Match a Character constructed
         *           from the specified char.
         */
        public SpecificChar(char c)
        {
            this.FChar = c;
        }

        /**
         * Returns true if an assembly's next element is equal to the 
         * character this object was constructed with.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is equal to 
         *           the character this object was constructed with
         */
        public override bool Qualifies(object o)
        {
            char c = (char)o;
            return c.Equals(FChar);
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
            return FChar.ToString();
        }

    }



    /**
     * A Digit matches a digit from a character assembly.
     * 
     *
     * 
     *
     */
    public class Digit : Terminal
    {

        /**
         * Returns true if an assembly's next element is a digit.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is a digit
         */
        public override bool Qualifies(object o)
        {
            return char.IsDigit((char)o);
        }
        /**
         * Create a set with one random digit.
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            char c = (char)(Random.Next(10) + '0');
            ArrayList v = new ArrayList();
            v.Add(c.ToString());      //   new string(new char[]{c})
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
            return "D";
        }
    }



    /**
     * A Letter matches any letter from a character assembly.
     * 
     *
     * 
     *
     */
    public class Letter : Terminal
    {

        /**
         * Returns true if an assembly's next element is a letter.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is a letter
         */
        public override bool Qualifies(object o)
        {
            return char.IsLetter((char)o);
        }
        /**
         * Create a set with one random letter.
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();

            char c = (char)(Random.Next(26) + 'a');
            ArrayList v = new ArrayList();
            v.Add(c.ToString());      //   new string(new char[]{c})
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
            return "L";
        }
    }




    /**
     * A Symbol matches a specific sequence, such as greater, or equal
     *  that a tokenizer
     * returns as a FSymbol. 
     * 
     *
     * 
     *
     */

    public class Symbol : Terminal
    {
        /**
         * the literal to Match
         */
        protected Token FSymbol;
        /**
         * Constructs a FSymbol that will Match the specified char.
         *
         * @param   char   the character to Match. The char must be 
         *                 one that the tokenizer will return as a 
         *                 FSymbol token. This typically includes most 
         *                 characters except letters and digits. 
         *
         * @return   a FSymbol that will Match the specified char
         */
        public Symbol(char c) : this(c.ToString())
        {
        }
        /**
         * Constructs a FSymbol that will Match the specified sequence
         * of characters.
         *
         * @param   string   the characters to Match. The characters
         *                   must be a sequence that the tokenizer will 
         *                   return as a FSymbol token, such as greater.
         *                   
         *
         * @return   a Symbol that will Match the specified sequence
         *           of characters
         */
        public Symbol(string s)
        {
            FSymbol = new Token(Token.TT_SYMBOL, s, 0);
        }
        /**
         * Returns true if the FSymbol this object represents Equals an
         * assembly's next element.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if the specified FSymbol Equals the next 
         *           token from an assembly
         */
        public override bool Qualifies(object o)
        {
            return FSymbol.Equals((Token)o);
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
            return FSymbol.ToString();
        }
    }



    /**
     * A Num matches a number from a token assembly.
     * 
     *
     * 
     *
     */

    public class Num : Terminal
    {

        /**
         * Returns true if an assembly's next element is a number.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if an assembly's next element is a number as
         *           recognized the tokenizer
         */
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsNumber();
        }
        /**
         * Create a set with one random number (between 0 and 100).
         * 
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
 
            Random Random = new Random();
            double d = Convert.ToDouble(Random.Next(100));

            d = Math.Floor(d);

            ArrayList v = new ArrayList();
            v.Add(d.ToString());
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
            return "Num";
        }
    }




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





    /**
     * A QuotedString matches a quoted string, like "this one" 
     * from a token assembly.
     * 
     *
     * 
     *
     */
    public class QuotedString : Terminal
    {
        /**
         * Returns true if an assembly's next element is a quoted 
         * string.
         *
         * @param   object   an element from a assembly
         *
         * @return   true, if a assembly's next element is a quoted 
         *           string, like "chubby cherubim".
         */
        public override bool Qualifies(object o)
        {
            Token t = (Token)o;
            return t.IsQuotedString();
        }
        /**
         * Create a set with one random quoted string (with 2 to
         * 6 characters).
         */
        public override ArrayList RandomExpansion(int maxDepth, int depth)
        {
            Random Random = new Random();
            int n = Random.Next(5);

            char[] letters = new char[n + 2];
            letters[0] = '"';
            letters[n + 1] = '"';

            for (int i = 0; i < n; i++)
            {
                int c = Random.Next(26) + 'a';
                letters[i + 1] = (char)c;
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
            return "QuotedString";
        }
    }




    /**
     * A Literal matches a specific string from an assembly.
     * 
     *
     *
     *
     */

    public class Literal : Terminal
    {
        /**
         * the FLiteral to Match
         */
        protected Token FLiteral;

        /**
         * Constructs a FLiteral that will Match the specified string.
         *
         * @param   string   the string to Match as a token
         *
         * @return   a FLiteral that will Match the specified string
         */
        public Literal(string s)
        {
            FLiteral = new Token(s);
        }
        /**
         * Returns true if the FLiteral this object Equals an
         * assembly's next element.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if the specified FLiteral Equals the next 
         *           token from an assembly
         */
        public override bool Qualifies(object o)
        {
            return FLiteral.Equals((Token)o);
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
            return FLiteral.ToString();
        }
    }



    /**
     * A CaselessLiteral matches a specified string from an
     * assembly, disregarding case.
     * 
     *
     * 
     *
     */
    public class CaselessLiteral : Literal
    {
        /**
         * Constructs a literal that will Match the specified string,
         * given mellowness about case.
         *
         * @param   string   the string to Match as a token
         *
         * @return   a literal that will Match the specified string,
         *           disregarding case
         */
        public CaselessLiteral(string literal) : base(literal)
        {
        }
        /**
         * Returns true if the literal this object Equals an
         * assembly's next element, disregarding case.
         *
         * @param   object   an element from an assembly
         *
         * @return   true, if the specified literal Equals the next
         *           token from an assembly, disregarding case
         */
        public override bool Qualifies(object o)
        {
            return FLiteral.EqualsIgnoreCase((Token)o);
        }
    }















    /**
     * This class provides a "visitor" hierarchy in support of
     * the Visitor pattern -- see the book, "Design Patterns" for
     * an explanation of this pattern.
     * 
     *
     * 
     *
     */
    public abstract class ParserVisitor
    {
        /**
         * Visit an alternation.
         *
         * @param   Alternation   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitAlternation(Alternation a, ArrayList visited);
        /**
         * Visit an empty parser.
         *
         * @param   Empty   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitEmpty(Empty e, ArrayList visited);
        /**
         * Visit a repetition.
         *
         * @param   Repetition   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitRepetition(Repetition r, ArrayList visited);
        /**
         * Visit a sequence.
         *
         * @param   Sequence   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitSequence(Sequence s, ArrayList visited);
        /**
         * Visit a terminal.
         *
         * @param   Terminal   the parser to visit
         *
         * @param   ArrayList   a collection of previously visited parsers
         *
         */
        public abstract void VisitTerminal(Terminal t, ArrayList visited);
    }









    /**
     * This class generates random language elements for a
     * parser and tests that the parser can Accept them.
     * 
     *
     * 
     *
     */
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
        /**
         * Constructs a tester for the given parser.
         */
        public ParserTester(Parser p)
        {
            this.FParser = p;
        }
        /**
         * Subclasses must override this, to produce an assembly
         * from the given (random) string.
         */
        protected abstract Assembly CreateAssembly(string s);
        /**
         * Generate a random language element, and return true if
         * the parser cannot unambiguously parse it.
         */
        protected bool CanGenerateProblem(int depth)
        {
            string s = FParser.RandomInput(depth, Separator());
            LogTestString(s);
            Assembly a = CreateAssembly(s);
            a.SetTarget(FreshTarget());
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
        /**
         * Return a subset of the supplied vector of assemblies,
         * filtering for assemblies that have been completely
         * matched.
         *
         * @param   ArrayList   a collection of partially or completely
         *                   matched assemblies
         *
         * @return   a collection of completely matched assemblies
         */
        public static ArrayList CompleteMatches(ArrayList In)
        {
            ArrayList Out = new ArrayList();
            for (int i = 0; i < In.Count; i++)
            {
                Assembly a = (Assembly)In[i];
                if (!a.HasMoreElements())
                    Out.Add(a);

            }
            return Out;
        }
        /**
         * Give subclasses a chance to provide fresh target at
         * the beginning of a parse.
         */
        protected ICloneable FreshTarget()
        {
            return null;
        }
        /**
         * This method is broken out to allow subclasses to create
         * less verbose tester, or to direct logging to somewhere
         * other than System.out.
         */
        protected string LogDepthChange(int depth)
        {
            //System.out.println("Testing depth " + depth + "...");
            return string.Format("Testing depth {0} ...", depth.ToString());
        }
        /**
         * This method is broken out to allow subclasses to create
         * less verbose tester, or to direct logging to somewhere
         * other than System.out.
         */
        protected string LogPassed()
        {
            return "No problems found.";//System.out.println("No problems found.");
        }
        /**
         * This method is broken out to allow subclasses to create
         * less verbose tester, or to direct logging to somewhere
         * other than System.out.
         */
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
        /**
         * This method is broken out to allow subclasses to create
         * less verbose tester, or to direct logging to somewhere
         * other than System.out.
         */
        protected string LogTestString(string s)
        {
            if (logTestStrings)
                return "    Testing string " + s;
            else return " ";

        }
        /**
         * By default, place a blank between randomly generated
         * "words" of a language.
         */
        protected virtual string Separator()
        {
            return " ";
        }
        /**
         * Set the bool which determines if this class displays
         * every test string.
         *
         * @param   bool   true, if the user wants to see
         *                    every test string
         */
        public void SetLogTestStrings(bool logTestStrings)
        {
            this.logTestStrings = logTestStrings;
        }
        /**
         * Create a series of random language elements, and test
         * that the parser can unambiguously parse each one.
         */
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