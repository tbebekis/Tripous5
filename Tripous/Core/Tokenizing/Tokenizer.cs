/*--------------------------------------------------------------------------------------        
	Original Java code by Steven J. Metsker from the book: Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622	

	Adaptation to C#, modifications and additions
	by teo.bebekis@gmail.com                                
--------------------------------------------------------------------------------------*/


namespace Tripous.Tokenizing
{
    /// <summary>
    /// A buffered character reader
    /// </summary>
    public interface ITokenizer
    {
        /// <summary>
        /// The character read, or -1 if the end of the stream has been reached
        /// </summary>
        int Read();
        /// <summary>
        /// Pushes back a single character by placint it to the current position of the buffer. 
        /// After this method returns, the next character to be read will have the value of the specified character.
        /// </summary>
        /// <param name="C">The int value representing a character to be pushed back</param>
        void Unread(int C);
        /// <summary>
        /// Pushes back a single character by placing it to the current position of the buffer. 
        /// <para>NOTE: The push-back is performed only if the specified character is greater than or equal to zero. </para>
        /// <para>After this method returns, the next character to be read will have the value of the specified character.</para>
        /// <para>NOTE: Returns true only if a push-back is happened.</para>
        /// </summary>
        bool UnreadSafe(int C);

        /// <summary>
        /// Returns the next token.
        /// </summary>
        Token NextToken();
        /// <summary>
        /// Constructs a token of the indicated type and associated string or numeric values.
        /// </summary>
        /// <param name="Kind">the type of the token, typically one  of the constants this class defines</param>
        /// <param name="StringValue">the string value of the token, typically null except for WORD and QUOTED tokens</param>
        /// <param name="NumericValue">the numeric value of the token, typically 0 except for NUMBER tokens</param>
        /// <param name="LineIndex">The line index in the source text, of the token</param>
        /// <param name="CharIndex">The starting character index in the current line in the source text, of the token</param>
        /// <returns>Returns a token</returns>
        Token CreateToken(TokenKind Kind, string StringValue, double NumericValue, int LineIndex, int CharIndex);

        /* properties */
        /// <summary>
        /// The length of the internal buffer
        /// </summary>
        int Length { get; }
        /// <summary>
        /// The current position in the internal buffer. Essentially the next position to be read.  
        /// </summary>
        int Position { get; }
        /// <summary>
        /// Returns the text of the internal buffer.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// The current line index in the source text
        /// </summary>
        int CurrentLineIndex { get; }
        /// <summary>
        /// The current character index in the current line in the source text
        /// </summary>
        int CurrentCharIndex { get; }
 
        /// <summary>
        /// Returns the state this tokenizer uses to build numbers.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        NumberState NumberState { get; }
        /// <summary>
        /// Returns the state this tokenizer uses to build quoted  strings.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        QuoteState QuoteState { get; }
        /// <summary>
        /// Returns the state this tokenizer uses to recognize (and ignore) comments.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        SlashState SlashState { get; }
        /// <summary>
        /// Return the state this tokenizer uses to recognize  symbols.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        SymbolState SymbolState { get; }
        /// <summary>
        /// Returns the state this tokenizer uses to recognize (and ignore) whitespace.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        WhitespaceState WhitespaceState { get; }
        /// <summary>
        /// Returns the state this tokenizer uses to build words.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        WordState WordState { get; }
        /// <summary>
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        NewLineState NewLineState { get; }
    }


    /// <summary>
    /// <para> A tokenizer divides a string into tokens.</para> 
    /// 
    /// <para> This class is  highly customizable with regard to exactly how this division occurs, 
    /// but it also has defaults that are suitable for many  languages. </para>
    /// <para>NOTE: By default the following characters a treated as symbols
    ///  <code>! # $ % &amp; ( ) * + , : ; &lt; = &gt; ? @ ` [ \ ] ^ _ { | } ~</code>
    /// </para>
    /// <para>In addition, the default value of Tokenizer treats the following multicharacter sequences as symbols:
    /// <code>!= &lt;= &gt;= :- </code>
    ///</para>
    /// <para> This class assumes that the character values ReadByte 
    /// from the string lie in the range 0-255. </para>
    /// 
    /// <para>For example, the Unicode value of a capital A is 65, so
    /// <code> System.out.println((char)65); </code> 
    /// prints out a capital A. </para>
    /// <para>
    /// The behavior of a tokenizer depends on its character state table. 
    /// This table is an array of 256 <see cref="TokenizerState"/> states. 
    /// The state table decides which state to enter upon reading a character from the input string. 
    /// </para>
    /// <para>
    /// For example, by default, upon reading an 'A', a tokenizer  will enter a "word" state. 
    /// This means the tokenizer will ask a <see cref="WordState"/> object to consume the 'A', 
    /// along with the characters after the 'A' that form a word. 
    /// The state's responsibility is to consume characters and return a complete token.
    /// </para>
    /// <para>
    /// The default table sets a SymbolState for every character 
    /// from 0 to 255, and then overrides this with:
    /// </para>
    /// <code>
    ///      From    To     State
    ///        0     ' '    WhitespaceState
    ///       'a'    'z'    WordState
    ///       'A'    'Z'    WordState
    ///      160     255    WordState
    ///       '0'    '9'    NumberState
    ///       '-'    '-'    NumberState
    ///       '.'    '.'    NumberState
    ///       '"'    '"'    QuoteState
    ///      '\''   '\''    QuoteState
    ///       '/'    '/'    SlashState
    /// </code>
    /// <para>
    /// In addition to allowing modification of the state table, 
    /// this class makes each of the states above available. Some 
    /// of these states are customizable. For example, WordState 
    /// allows customization of what characters can be part of a 
    /// word, after the first character. 
    /// </para>
    /// </summary>
    public class Tokenizer: ITokenizer
    {
 
        /// <summary>
        ///  The number of characters that might be in a symbol;
        /// </summary>
        protected static int DEFAULT_SYMBOL_MAX = 4;
        /// <summary>
        /// The state lookup table
        /// </summary>
        protected TokenizerState[] characterState = new TokenizerState[256];
 
        /* construction */
        /// <summary>
        /// Constructs a tokenizer with a default state table (as described in the class comment). 
        /// </summary>
        public Tokenizer()
        {
            SetCharacterState(0, 255, SymbolState); // the default

            SetCharacterState(0, ' ', WhitespaceState);
            SetCharacterState('a', 'z', WordState);
            SetCharacterState('A', 'Z', WordState);
            SetCharacterState(0xc0, 0xff, WordState);
            SetCharacterState('0', '9', NumberState);
            SetCharacterState('-', '-', NumberState);
            //SetCharacterState('.', '.', NumberState); NO
            SetCharacterState('"', '"', QuoteState);
            SetCharacterState('\'', '\'', QuoteState);
            SetCharacterState('/', '/', SlashState);
            SetCharacterState('\r', '\r', NewLineState);
            SetCharacterState('\n', '\n', NewLineState);
        }
        /// <summary>
        /// Constructs a tokenizer to ReadByte from the supplied string.
        /// </summary>
        public Tokenizer(string s) 
            : this()
        {
            SetString(s);
        }

        /* public */
        /// <summary>
        /// Helper method for displaying the content of a tokenized and parsed elements.
        /// <example>
        /// <code>     
        /// TokenAssembly A = new TokenAssembly("aa bb cc");
        /// 
        /// Parser SubParser = new WordTerminalParser(); 
        /// Parser Parser = new RepetitionParser(SubParser); 
        /// ArrayList List = new ArrayList(); 
        /// List.Add(A); 
        ///  
        /// List = Parser.Match(List); 
        ///  
        /// string S = List != null? Tokenizer.ToString(List): "[no match]";
        /// </code>
        /// </example>
        /// </summary>
        static public string ToString(IList Source)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append("[");
            if (Source == null)
            {
                SB.Append("null");
            }
            else
            {
                if (Source != null && Source.Count > 0)
                {
                    for (int i = 0; i < Source.Count; i++)
                    {
                        SB.Append(Source[i] == null ? "null" : Source[i].ToString());
                        if (i < Source.Count - 1)
                        {
                            SB.Append(", ");
                        }
                    }
                }
            }
            SB.Append("]");
            return SB.ToString();
        }

        /// <summary>
        /// Set the string to ReadByte from.
        /// </summary>
        public void SetString(string s)
        {
            SetString(s, DEFAULT_SYMBOL_MAX);
        }
        /// <summary>
        /// Set the string to ReadByte from.
        /// </summary>
        /// <param name="s"> the string to ReadByte from</param>
        /// <param name="symbolMax">the maximum Length of a symbol, which establishes the size of pushback buffer we need</param>
        public void SetString(string s, int symbolMax)
        {
            Reader = new CharReader(s); 
        }
        /// <summary>
        /// Change the state the tokenizer will enter upon reading any character between "from" and "to".
        /// </summary>
        /// <param name="from">the "from" character</param>
        /// <param name="to">the "to" character</param>
        /// <param name="state">the state to enter upon reading a character between "from" and "to"</param>
        public void SetCharacterState(int from, int to, TokenizerState state)
        {
            for (int i = from; i <= to; i++)
                if (i >= 0 && i < characterState.Length)
                    characterState[i] = state;
        }
 
        /// <summary>
        /// The character read, or -1 if the end of the stream has been reached
        /// </summary>
        public int Read()
        {
            CurrentCharIndex++;
            return Reader.Read();
        }
        /// <summary>
        /// Pushes back a single character by placint it to the current position of the buffer. 
        /// After this method returns, the next character to be read will have the value of the specified character.
        /// </summary>
        /// <param name="C">The int value representing a character to be pushed back</param>
        public void Unread(int C)
        {
            CurrentCharIndex--;
            Reader.Unread(C);
        }
        /// <summary>
        /// Pushes back a single character by placing it to the current position of the buffer. 
        /// <para>NOTE: The push-back is performed only if the specified character is greater than or equal to zero. </para>
        /// <para>After this method returns, the next character to be read will have the value of the specified character.</para>
        /// <para>NOTE: Returns true only if a push-back is happened.</para>
        /// </summary>
        public bool UnreadSafe(int C)
        {
            bool Result = Reader.UnreadSafe(C);
            if (Result)
                CurrentCharIndex--;
            return Result;
        }

        /// <summary>
        /// Constructs a token of the indicated type and associated string or numeric values.
        /// </summary>
        /// <param name="Kind">the type of the token, typically one  of the constants this class defines</param>
        /// <param name="StringValue">the string value of the token, typically null except for WORD and QUOTED tokens</param>
        /// <param name="NumericValue">the numeric value of the token, typically 0 except for NUMBER tokens</param>
        /// <param name="LineIndex">The line index in the source text, of the token</param>
        /// <param name="CharIndex">The starting character index in the current line in the source text, of the token</param>
        /// <returns>Returns a token</returns>
        public Token CreateToken(TokenKind Kind, string StringValue, double NumericValue, int LineIndex, int CharIndex)
        {
            Token Result = Token.Create(Kind, StringValue, NumericValue);

            Result.LineIndex = LineIndex;
            Result.CharIndex = CharIndex;

            if (Kind == Token.TT_NEWLINE)
            {
                CurrentLineIndex++;
                CurrentCharIndex = -1;
            }

            return Result;
        }
        /// <summary>
        /// Returns the next token.
        /// </summary>
        public Token NextToken()
        {
            int c = Read();

            if (c >= 0 && c < characterState.Length)
            {
                TokenizerState State = characterState[c];
                return State.NextToken(this as ITokenizer, c);
            }
            return Token.EOF;
        }

        /* properties */
        /// <summary>
        /// The length of the internal buffer
        /// </summary>
        public int Length => Reader.Length;
        /// <summary>
        /// The current position in the internal buffer. Essentially the next position to be read.  
        /// </summary>
        public int Position => Reader.Position;
        /// <summary>
        /// Returns the text of the internal buffer.
        /// </summary>
        public string Text => Reader.Text;

        /// <summary>
        /// The current line index in the source text
        /// </summary>
        public int CurrentLineIndex { get; protected set; } = 0;
        /// <summary>
        /// The current character index in the current line in the source text
        /// </summary>
        public int CurrentCharIndex { get; protected set; } = -1;

        /// <summary>
        /// Returns the state this tokenizer uses to build numbers.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public NumberState NumberState { get; protected set; } = new NumberState();
        /// <summary>
        /// Returns the state this tokenizer uses to build quoted  strings.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public QuoteState QuoteState { get; protected set; } = new QuoteState();
        /// <summary>
        /// Returns the state this tokenizer uses to recognize (and ignore) comments.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public SlashState SlashState { get; protected set; } = new SlashState();
        /// <summary>
        /// Return the state this tokenizer uses to recognize  symbols.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public SymbolState SymbolState { get; protected set; } = new SymbolState();
        /// <summary>
        /// Returns the state this tokenizer uses to recognize (and ignore) whitespace.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public WhitespaceState WhitespaceState { get; protected set; } = new WhitespaceState();
        /// <summary>
        /// Returns the state this tokenizer uses to build words.
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public WordState WordState { get; protected set; } = new WordState();
        /// <summary>
        /// <para>The default states that actually consume text and produce a token</para>
        /// </summary>
        public NewLineState NewLineState { get; protected set; } = new NewLineState();
        /// <summary>
        /// A stream, the tokenizer reads from and writes to
        /// </summary>
        protected CharReader Reader { get; set; }
    }
}
