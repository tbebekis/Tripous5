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
using System.IO;

namespace Tripous.Tokenizing
{

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
    public class Tokenizer
    {
 
        /// <summary>
        ///  The number of characters that might be in a symbol;
        /// </summary>
        protected static int DEFAULT_SYMBOL_MAX = 4;
        /// <summary>
        /// The state lookup table
        /// </summary>
        protected TokenizerState[] characterState = new TokenizerState[256];

        StreamReader Reader2;
        StringReader Reader3;
        BufferedStream Reader4;
        StringReader Reader5;
        


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
            SetCharacterState('.', '.', NumberState);
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
            s = s + ' ';  // bug when a numeric is the last token   // \n    


            byte[] Bytes = System.Text.Encoding.Default.GetBytes(s);

            Reader = new System.IO.MemoryStream(Bytes);

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
        /// Returns the next token.
        /// </summary>
        public Token NextToken()
        {
            int c = Reader.ReadByte();

            /* There was a defect here, that resulted from the fact 
             * that unreading a -1 results in the next ReadByte having a 
             * value of (int)(char)-1, which is 65535. This may be
             * a defect in System.IO.Stream. */

            if (c >= 0 && c < characterState.Length)
            {
                //TokenizerState State = characterState[c];
                return characterState[c].NextToken(Reader, c, this);
            }
            return Token.EOF;
        }

        /* properties */
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
        public System.IO.Stream Reader { get; protected set; }
 

    }
}
