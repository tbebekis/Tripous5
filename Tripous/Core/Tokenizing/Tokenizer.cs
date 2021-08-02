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
 * A tokenizer divides a string into tokens. This class is 
 * highly customizable with regard to exactly how this division 
 * occurs, but it also has defaults that are suitable for many 
 * languages. This class assumes that the character values ReadByte 
 * from the string lie in the range 0-255. For example, the 
 * Unicode value of a capital A is 65, so
 * <code> System.out.println((char)65); </code> prints out a 
 * capital A.
 * 
 * The behavior of a tokenizer depends on its character state 
 * table. This table is an array of 256 <code>TokenizerState
 * </code>  states. The state table decides which state to 
 * enter upon reading a character from the input 
 * string. 
 *
 * For example, by default, upon reading an 'A', a tokenizer 
 * will enter a "word" state. This means the tokenizer will 
 * ask a <code>WordState</code> object to consume the 'A', 
 * along with the characters after the 'A' that form a word. 
 * The state's responsibility is to consume characters and 
 * return a complete token.
 * 
 * The default table sets a SymbolState for every character 
 * from 0 to 255, and then overrides this with:
 *
 *     From    To     State
 *       0     ' '    WhitespaceState
 *      'a'    'z'    WordState
 *      'A'    'Z'    WordState
 *     160     255    WordState
 *      '0'    '9'    NumberState
 *      '-'    '-'    NumberState
 *      '.'    '.'    NumberState
 *      '"'    '"'    QuoteState
 *     '\''   '\''    QuoteState
 *      '/'    '/'    SlashState
 * 
 * In addition to allowing modification of the state table, 
 * this class makes each of the states above available. Some 
 * of these states are customizable. For example, WordState 
 * allows customization of what characters can be part of a 
 * word, after the first character. 
 *
 *
 *
 *
 */
    public class Tokenizer
    {

        /// <summary>
        /// The reader to ReadByte characters from
        /// </summary>
        protected System.IO.Stream FReader;
        /// <summary>
        ///  The number of characters that might be in a symbol;
        /// </summary>
        protected static int DEFAULT_SYMBOL_MAX = 4;
        /// <summary>
        /// The state lookup table
        /// </summary>
        protected TokenizerState[] characterState = new TokenizerState[256];
        /// <summary>
        ///  The default states that actually consume text and produce a token
        /// </summary>
        protected NumberState FNumberState = new NumberState();
        /// <summary>
        /// 
        /// </summary>
        protected QuoteState FQuoteState = new QuoteState();
        /// <summary>
        /// 
        /// </summary>
        protected SlashState FSlashState = new SlashState();
        /// <summary>
        /// 
        /// </summary>
        protected SymbolState FSymbolState = new SymbolState();
        /// <summary>
        /// 
        /// </summary>
        protected WhitespaceState FWhitespaceState = new WhitespaceState();
        /// <summary>
        /// 
        /// </summary>
        protected WordState FWordState = new WordState();
        /// <summary>
        /// 
        /// </summary>
        protected NewLineState FNewLineState = new NewLineState();

        #region properties
        /**
         * Return the state this tokenizer uses to build numbers.
         *
         * @return  the state this tokenizer uses to build numbers
         */
        public NumberState NumberState { get { return FNumberState; } }
        /**
         * Return the state this tokenizer uses to build quoted 
         * strings.
         *
         * @return  the state this tokenizer uses to build quoted 
         *          strings
         */
        public QuoteState QuoteState { get { return FQuoteState; } }
        /**
         * Return the state this tokenizer uses to recognize
         * (and ignore) comments.
         *
         * @return  the state this tokenizer uses to recognize
         *          (and ignore) comments
         *
         */
        public SlashState SlashState { get { return FSlashState; } }
        /**
         * Return the state this tokenizer uses to recognize 
         * symbols.
         *
         * @return  the state this tokenizer uses to recognize 
         *          symbols
         */
        public SymbolState SymbolState { get { return FSymbolState; } }
        /**
         * Return the state this tokenizer uses to recognize (and
         * ignore) whitespace.
         *
         * @return  the state this tokenizer uses to recognize
         *          whitespace
         */
        public WhitespaceState WhitespaceState { get { return FWhitespaceState; } }
        /**
         * Return the state this tokenizer uses to build words.
         *
         * @return  the state this tokenizer uses to build words
         */
        public WordState WordState { get { return FWordState; } }
        /// <summary>
        /// A stream, the tokenizer reads from and writes to
        /// </summary>
        public System.IO.Stream Reader
        {
            get { return FReader; }
            set { FReader = value; }
        }
        #endregion properties      

        #region constructors
        /**
         * Constructs a tokenizer with a default state table (as 
         * described in the class comment). 
         *
         * @return   a tokenizer
         */
        public Tokenizer()
        {

            SetCharacterState(0, 255, SymbolState); // the default

            SetCharacterState(0, ' ', FWhitespaceState);
            SetCharacterState('a', 'z', FWordState);
            SetCharacterState('A', 'Z', FWordState);
            SetCharacterState(0xc0, 0xff, FWordState);
            SetCharacterState('0', '9', FNumberState);
            SetCharacterState('-', '-', FNumberState);
            SetCharacterState('.', '.', FNumberState);
            SetCharacterState('"', '"', FQuoteState);
            SetCharacterState('\'', '\'', FQuoteState);
            SetCharacterState('/', '/', FSlashState);
            SetCharacterState('\r', '\r', FNewLineState);
            SetCharacterState('\n', '\n', FNewLineState);
        }
        /**
         * Constructs a tokenizer to ReadByte from the supplied string.
         *
         * @param   string   the string to ReadByte from
         */
        public Tokenizer(string s) : this()
        {
            SetString(s);
        }

        #endregion constructors  

        #region public  
        /**
         * Set the string to ReadByte from.
         * 
         * @param   string   the string to ReadByte from
         */
        public void SetString(string s)
        {
            SetString(s, DEFAULT_SYMBOL_MAX);
        }
        /**
         * Set the string to ReadByte from.
         * 
         * @param   string   the string to ReadByte from
         *
         * @param   int   the maximum Length of a symbol, which
         *                establishes the size of pushback buffer
         *                we need
         */
        public void SetString(string s, int symbolMax)
        {
            s = s + ' ';  // bug when a numeric is the last token   // \n    


            byte[] Bytes = System.Text.Encoding.Default.GetBytes(s);

            FReader = new System.IO.MemoryStream(Bytes);

        }
        /**
         * Change the state the tokenizer will enter upon reading 
         * any character between "from" and "to".
         *
         * @param   from   the "from" character
         *
         * @param   to   the "to" character
         *
         * @param   TokenizerState   the state to enter upon reading a
         *                           character between "from" and "to"
         */
        public void SetCharacterState(int from, int to, TokenizerState state)
        {
            for (int i = from; i <= to; i++)
                if (i >= 0 && i < characterState.Length)
                    characterState[i] = state;
        }
        /**
         * Return the next token.
         *
         * @return the next token.
         *
         * @exception   IOException   if there is any problem reading
         */
        public Token NextToken()
        {
            int c = FReader.ReadByte();

            /* There was a defect here, that resulted from the fact 
             * that unreading a -1 results in the next ReadByte having a 
             * value of (int)(char)-1, which is 65535. This may be
             * a defect in System.IO.Stream. */

            if (c >= 0 && c < characterState.Length)
            {
                return characterState[c].NextToken(FReader, c, this);
            }
            return Token.EOF;
        }

        #endregion public

    }
}
