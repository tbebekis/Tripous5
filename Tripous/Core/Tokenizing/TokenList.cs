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
    /// A <see cref="TokenList"/> is a list of tokens that can present its tokens as a string. 
    /// <para>Once a <see cref="TokenList"/> is created, it is "immutable", meaning it cannot change. </para>
    /// <para>This lets you freely copy instances of this class without worrying about their state. </para>
    /// <para>NOTE: TokenString in the original code.</para>
    /// </summary>
    public class TokenList: ICloneable
    { 
        /* construction */
        /// <summary>
        /// Constructs a tokenString from the supplied FTokens.
        /// </summary>
        /// <param name="Tokens">the FTokens to use</param>
        public TokenList(Token[] Tokens)
        {
            this.Tokens = Tokens;
        }
        /// <summary>
        /// Constructs a tokenString from the supplied string. 
        /// </summary>
        /// <param name="s">the string to tokenize</param>
        public TokenList(string s) 
            : this(new Tokenizer(s))
        {
        }
        /// <summary>
        /// Constructs a tokenString from the supplied reader and  tokenizer. 
        /// </summary>
        /// <param name="t">the tokenizer that will produces the FTokens</param>
        public TokenList(Tokenizer t)
        {
            Token tok = null;
            ArrayList v = new ArrayList();
            try
            {
                while (true)
                {
                    tok = t.NextToken();
                    if (tok.Kind == Token.TT_EOF)
                        break;
                    v.Add(tok);
                };
            }
            catch (Exception e)
            {
                string Message = tok != null ? $"Position: {tok.LineIndex}, {tok.CharIndex}. " : string.Empty;
                Message = $"{Message}Problem tokenizing string: {e.Message}";
                throw new ApplicationException(Message);
            }

            Tokens = new Token[v.Count];
            v.CopyTo(Tokens);
        }
 
        /* public */
        /// <summary>
        /// Returns a string representation of this tokenString. 
        /// </summary>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < Tokens.Length; i++)
            {
                if (i > 0)
                    buf.Append(" ");

                buf.Append(Tokens[i]);
            }
            return buf.ToString();
        }
        /// <summary>
        /// Clones this instance and returns the clone.
        /// </summary>
        public object Clone()
        {
            Token[] Result = new Token[this.Tokens.Length];
            for (int i = 0; i < this.Tokens.Length; i++)
                Result[i] = this.Tokens[i].Clone() as Token;

            return new TokenList(Result);
        }

        /// <summary>
        /// Returns the token at a specified index.
        /// </summary>
        public Token TokenAt(int i)
        {
            return Tokens[i];
        }

        /* properties */
        /// <summary>
        /// Indexer. Returns the token at a specified index.
        /// </summary>
        public Token this[int Index] => Tokens[Index];
        /// <summary>
        /// Returns the number of tokens in this token string.
        /// </summary>
        public int Length => Tokens.Length;
        /// <summary>
        ///  The tokens in this token string.
        /// </summary>
        public Token[] Tokens { get; private set; }
    }
}
