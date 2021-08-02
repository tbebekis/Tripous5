/*
	Original Java code by Steven J. Metsker
	from the book 
	
	Building Parsers With Java
	# Publisher : Addison-Wesley Professional; Bk&CD-Rom edition (March 26, 2001)
	# ISBN      : 0201719622
	

	Adaptation to C#, modifications and additions
	by Theo Bebekis

*/


using System;
using System.Text;
using System.Collections;

namespace bt.Parsers
{


   /**
    * Objects of this class represent a type of token, such
    * as "number" or "word".
    * 
    *
    *
    *
    */
   public class TokenKind 
   {
      /// <summary>
      /// 
      /// </summary>
      protected string FName;
      /**
       * Creates a token type of the given name.
       */
      public TokenKind(string Name) 
      {
         this.FName = Name;
      }
      /// <summary>
      /// 
      /// </summary>
      public string Name { get {return FName;} }
      
               
   }





   /**
    * A token represents a logical chunk of a string. For 
    * example, a typical tokenizer would break the string 
    * <code>"1.23 &lt;= 12.3"</code> into three tokens: the number 
    * 1.23, a less-than-or-equal symbol, and the number 12.3. A 
    * token is a receptacle, and relies on a tokenizer to decide 
    * precisely how to divide a string into tokens. 
    * 
    *
    *
    *
    */ 
   public class Token: ICloneable 
   {
      /// <summary>
      /// 
      /// </summary>
      protected TokenKind FKind;
      /// <summary>
      /// 
      /// </summary>
      protected string FStringValue;
      /// <summary>
      /// 
      /// </summary>
      protected double FNumericValue;

      /** 
       * A constant indicating that the end of the stream has 
       * been ReadByte. 
       */
      public static TokenKind TT_EOF = new TokenKind("eof");
      /**
       * A constant indicating that there are no more tokens
       */
      public static Token EOF = new Token(TT_EOF, "", 0);

      /** 
       * A constant indicating that a token is a number, 
       * like 3.14
       */
      public static TokenKind TT_NUMBER = new TokenKind("number");

      /** 
       * A constant indicating a token is a word, like "cat"
       */
      public static TokenKind TT_WORD =  new TokenKind("word");

      /**
       * A constant indicating that a token is a symbol 
       * like "&lt;=".
       */
      public static TokenKind TT_SYMBOL = new TokenKind("symbol");

      /**
       * A constant indicating that a token is a quoted string, 
       * like "Launch Mi".
       */
      public static TokenKind TT_QUOTED =  new TokenKind("quoted");
      /// <summary>
      /// 
      /// </summary>
      public static TokenKind TT_WHITESPACE = new TokenKind("whitespace");
      /// <summary>
      /// 
      /// </summary>
      public static TokenKind TT_NEWLINE = new TokenKind("newline");
      
      /**
      */
      private Token()
      {
      }
      
      /**
       * Constructs a token from the given char.
       *
       * @param   char   the char
       *
       * @return   a token constructed from the given char
       */
      public Token(char C) : this(TT_SYMBOL, new string(new char[] {C}), 0)
      {           
      }
      /**
       * Constructs a token from the given number.
       *
       * @param   double   the number
       *
       * @return   a token constructed from the given number
       */
      public Token(double N)  : this(TT_NUMBER, "", N)
      {          
      }
      /**
       * Constructs a token from the given string.
       *
       * @param   string   the string
       *
       * @return   a token constructed from the given string
       */
      public Token (string S) : this(TT_WORD, S, 0)
      {
      }
      /**
       * Constructs a token of the indicated type and associated 
       * string or numeric values.
       *
       * @param   TokenKind   the type of the token, typically one 
       *                      of the constants this class defines
       *
       * @param   string  the string value of the token, typically 
       *                  null except for WORD and QUOTED tokens
       *
       * @param   double   the numeric value of the token, typically
       *                   0 except for NUMBER tokens
       *
       * @return   a token
       */
      public Token (TokenKind Kind, string S, double N) 
      {
         this.FKind = Kind;
         this.FStringValue = S;
         this.FNumericValue = N;
      }
      /**
       * Returns true if the supplied object is an equivalent token.
       *
       * @param   object   the object to compare
       *
       * @return   true, if the supplied object is of the same type 
       *           and value
       */
      new public bool Equals(Object o) 
      {
         if (!(o is Token))
            return false;
            
         Token t = (Token) o;
	
         if (FKind != t.FKind) 
         {
            return false;
         }
         if (FKind == TT_NUMBER) 
         {
            return FNumericValue == t.FNumericValue;
         }
         if (FStringValue == null || t.FStringValue == null) 
         {
            return false;
         }
         return FStringValue.Equals(t.FStringValue);
      }
      /**
       * Returns true if the supplied object is an equivalent token,
       * given mellowness about case in strings and characters.
       *
       * @param   object   the object to compare
       *
       * @return   true, if the supplied object is of the same type 
       *           and value. This method disregards case when 
       *           comparing the string value of tokens.
       */
      public bool EqualsIgnoreCase(Object o) 
      {
         if (!(o is Token))
            return false;
            
         Token t = (Token) o;
	
         if (FKind != t.FKind) 
         {
            return false;
         }
         if (FKind == TT_NUMBER) 
         {
            return FNumericValue == t.FNumericValue;
         }
         if (FStringValue == null || t.FStringValue == null) 
         {
            return false;
         }
         return  string.Compare(FStringValue, t.FStringValue, true) == 0; // FStringValue.EqualsIgnoreCase(t.FStringValue);
      }
      /**
       * Returns true if this token is a number.
       *
       * @return   true, if this token is a number
       */
      public bool IsNumber() 
      {
         return FKind == TT_NUMBER;
      }
      /**
       * Returns true if this token is a quoted string.
       *
       * @return   true, if this token is a quoted string
       */
      public bool IsQuotedString() 
      {
         return FKind == TT_QUOTED;
      }
      /**
       * Returns true if this token is a symbol.
       *
       * @return   true, if this token is a symbol
       */
      public bool IsSymbol() 
      {
         return FKind == TT_SYMBOL;
      }
      /**
       * Returns true if this token is a word.
       *
       * @return   true, if this token is a word.
       */
      public bool IsWord() 
      {
         return FKind == TT_WORD;
      }
      
      /**
       * Return a textual description of this object.
       * 
       * @return a textual description of this object
       */
      public override string ToString () 
      {
         if (FKind == TT_EOF) 
         {
            return "EOF";
         }
         return Value.ToString();
      }      
      /**
       * Returns the numeric value of this token.
       *
       * @return    the numeric value of this token
       */
      public double AsNumeric { get { return FNumericValue; } }
      /**
       * Returns the string value of this token.
       *
       * @return    the string value of this token
       */
      public string AsString  { get {return FStringValue;} }
      /**
       * Returns the type of this token.
       *
       * @return   the type of this token, typically one of the
       *           constants this class defines
       */
      public TokenKind Kind { get{ return FKind;}}

      /**
       * Returns an object that represents the value of this token.
       *
       * @return  an object that represents the value of this token
       */
      public object Value 
      {
         get 
         {
            if (FKind == TT_NUMBER)    return FNumericValue;
            if (FKind == TT_EOF)       return EOF;
            if (FStringValue != null)  return FStringValue;

            return FKind;
         }
      }
   
      /**
      
      */
      public object Clone()
      {
         Token Res = new Token();
         Res.FKind = FKind;
         Res.FStringValue = FStringValue;
         Res.FNumericValue = FNumericValue;
         return Res;  
      }
   }



   
   /**
    * A tokenizerState returns a token, given a reader, an 
    * initial character ReadByte from the reader, and a tokenizer 
    * that is conducting an overall tokenization of the reader.
    * The tokenizer will typically have a character state table 
    * that decides which state to use, depending on an initial 
    * character. If a single character is insufficient, a state
    * such as <code>SlashState</code> will ReadByte a second 
    * character, and may delegate to another state, such as 
    * <code>SlashStarState</code>. This prospect of delegation is 
    * the reason that the <code>nextToken()</code> method has a 
    * tokenizer argument. 
    * 
    *
    *
    *
    */
   public abstract class TokenizerState 
   {
      /**
       * Return a token that represents a logical piece of a reader.
       * 
       * @return  a token that represents a logical piece of the 
       *          reader
       *
       * @param   System.IO.Stream   a reader to ReadByte from
       *
       * @param   c   the character that a tokenizer used to 
       *              determine to use this state
       *
       * @param   Tokenizer   the tokenizer conducting the overall
       *                      tokenization of the reader
       *
       * @exception   IOException   if there is any problem reading
       */
      public abstract Token NextToken(System.IO.Stream r, int c, Tokenizer t);  
   }
   
   



   /**
   * A whitespace state ignores whitespace (such as blanks
   * and tabs), and returns the tokenizer's next token. By 
   * default, all characters from 0 to 32 are whitespace.
   * 
   *
   *
   *
   */
   public class WhitespaceState : TokenizerState 
   {  
      /// <summary>
      /// 
      /// </summary>
      protected bool [] whitespaceChar = new bool[256];
      /**
      * Constructs a whitespace state with a default idea of what
      * characters are, in fact, whitespace.
      *
      * @return   a state for ignoring whitespace
      */
      public WhitespaceState() 
      {
         SetWhitespaceChars(0, ' ', true);
         SetWhitespaceChars('\r', '\r', false);
         SetWhitespaceChars('\n', '\n', false);
      }
      /**
      * Ignore whitespace (such as blanks and tabs), and return 
      * the tokenizer's next token.
      *
      * @return the tokenizer's next token
      */
      public override Token NextToken(System.IO.Stream r, int aWhitespaceChar, Tokenizer t)
      {
         int i = 0;	
         int c;
         do 
         {
            c = r.ReadByte();
            i++;
         } 
         while 
            (
            c >= 0 && 
            c < whitespaceChar.Length && 
            whitespaceChar[c]
            );
         
         if (c >= 0) 
         {
            r.Seek(-1, System.IO.SeekOrigin.Current);   //r.unread(c);
         }
	      
         //return t.NextToken();
         return new Token(Token.TT_WHITESPACE, new string(' ', i), i);
      }
      /**
      * Establish the given characters as whitespace to ignore.
      *
      * @param   first   char
      *
      * @param   second   char
      *
      * @param   bool   true, if this state should ignore
      *                    characters in the given range
      */
      public void SetWhitespaceChars(int from, int to, bool b) 
      {
         for (int i = from; i <= to; i++) 
         {
            if (i >= 0 && i < whitespaceChar.Length) 
               whitespaceChar[i] = b;     
         }
      }
      
   }
   
   
   
   
 
    
      
   /**
   * A NumberState object returns a number from a reader. This 
   * state's idea of a number allows an optional, initial 
   * minus sign, followed by one or more digits. A decimal 
   * point and another string of digits may follow these 
   * digits. 
   * 
   *
   *
   *
   */
   public class NumberState : TokenizerState 
   {
      /// <summary>
      /// 
      /// </summary>
      protected int c;
      /// <summary>
      /// 
      /// </summary>
      protected double Fvalue;
      /// <summary>
      /// 
      /// </summary>
      protected bool absorbedLeadingMinus;
      /// <summary>
      /// 
      /// </summary>
      protected bool absorbedDot;
      /// <summary>
      /// 
      /// </summary>
      protected bool gotAdigit;
      /// <summary>
      /// Convert a stream of digits into a number, making this  number a fraction if the bool parameter is true.
      /// </summary>
      protected double AbsorbDigits(System.IO.Stream r, bool fraction)
      {
      		
         int divideBy = 1;
         double v = 0;
         while ('0' <= c && c <= '9') 
         {
            gotAdigit = true;
            v = v * 10 + (c - '0');
            c = r.ReadByte();
            if (fraction) 
            {
               divideBy *= 10;
            }
         }
         if (fraction) 
         {
            v = v / divideBy;
         }
         return v;
      }
      /**
      * Return a number token from a reader.
      *
      * @return a number token from a reader
      */
      public override Token NextToken(System.IO.Stream r, int cin, Tokenizer t)
      {      	 
         Reset(cin);	
         ParseLeft(r);
         ParseRight(r);
         r.Seek(-1, System.IO.SeekOrigin.Current); // r.unread(c);
         return Value(r, t);
      }
      /// <summary>
      /// Parse up to a decimal point.
      /// </summary>
      protected void ParseLeft(System.IO.Stream r)
      {
      	 
         if (c == '-') 
         {
            c = r.ReadByte();
            absorbedLeadingMinus = true;
         }
         Fvalue = AbsorbDigits(r, false);	 
      }
      /**
      * Parse from a decimal point to the end of the number.
      */
      protected void ParseRight(System.IO.Stream r)
      {
      	 
         if (c == '.')
         {
            c = r.ReadByte();
            absorbedDot = true;
            Fvalue += AbsorbDigits(r, true);
         }
      }
      /**
      * Prepare to assemble a new number.
      */
      protected void Reset(int cin) 
      {
         c = cin;
         Fvalue = 0;
         absorbedLeadingMinus = false;
         absorbedDot = false;
         gotAdigit = false;
      }
      /**
      * Put together the pieces of a number.
      */
      protected Token Value(System.IO.Stream r, Tokenizer t)
      {
      		
         if (!gotAdigit) 
         {
            if (absorbedLeadingMinus && absorbedDot) 
            {
               r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread('.');
               return t.SymbolState.NextToken(r, '-', t);
            }
            if (absorbedLeadingMinus) 
            {
               return t.SymbolState.NextToken(r, '-', t);
            }
            if (absorbedDot) 
            {
               return t.SymbolState.NextToken(r, '.', t);
            }
         }
         if (absorbedLeadingMinus) 
         {
            Fvalue = -Fvalue;
         }
         //return new Token(Token.TT_NUMBER, "", Fvalue);
         return new Token(Token.TT_NUMBER, Fvalue.ToString(), Fvalue);
      }
   } 
    
 

   /**
    * A quoteState returns a quoted string token from a reader. 
    * This state will collect characters until it sees a match
    * to the character that the tokenizer used to switch to 
    * this state. For example, if a tokenizer uses a double-
    * quote character to enter this state, then <code>
    * NextToken()</code> will search for another double-quote 
    * until it finds one or finds the end of the reader.
    * 
    *
    *
    *
    */
   public class QuoteState : TokenizerState 
   {
      /// <summary>
      /// 
      /// </summary>
      protected byte[] Bytes = new byte[16];
      /**
       * Fatten up charbuf as necessary.
       */
      protected void CheckBufLength(int i) 
      {
         if (i >= Bytes.Length) 
         {
            byte[] nb = new byte[Bytes.Length * 2];
            System.Array.Copy(Bytes, 0, nb, 0, Bytes.Length);
            Bytes = nb;
         }
      }
      /**
       * Return a quoted string token from a reader. This method 
       * will collect characters until it sees a match to the
       * character that the tokenizer used to switch to this 
       * state.
       *
       * @return a quoted string token from a reader
       */
      public override Token NextToken(System.IO.Stream r, int cin, Tokenizer t)
      {
		
         int i = 0;
         Bytes[i++] = (byte) cin;
         int c;
         do 
         {
            c = r.ReadByte();
            if (c < 0) 
            {
               c = cin;
            }
            CheckBufLength(i);
            Bytes[i++] = (byte) c;
         } while (c != cin);
	                  
         char[] Chars = new char[Encoding.Default.GetCharCount(Bytes, 0, i)];  
         Encoding.Default.GetChars(Bytes, 0, i, Chars, 0);
         
         string sval = new string(Chars);   //string sval = string.copyValueOf(charbuf, 0, i);
         return new Token(Token.TT_QUOTED, sval, 0);
      }
   } 
   
   
   
   
   /**
    * A slashSlash state ignores everything up to an end-of-line
    * and returns the tokenizer's next token.
    * 
    *
    *
    *
    */
   public class SlashSlashState : TokenizerState 
   {
      /**
       * Ignore everything up to an end-of-line and return the 
       * tokenizer's next token.
       *
       * @return the tokenizer's next token
       */
      public override Token NextToken( System.IO.Stream r, int theSlash, Tokenizer t)
      {
		
         int c;
         //while ((c = r.ReadByte()) != '\n' && c != '\r' && c >= 0) 
         while ((c = r.ReadByte()) >= 0) 
         {
            if ("\n\r".IndexOf((char)c) != -1)
            {
               r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
               break;
            }
         }
         return t.NextToken();
      }
   }   
   
   
   
   
   /**
    * A slashStar state ignores everything up to a closing star
    * and slash, and then returns the tokenizer's next token.
    * 
    *
    *
    *
    */
   public class SlashStarState : TokenizerState 
   {
      /**
       * Ignore everything up to a closing star and slash, and 
       * then return the tokenizer's next token.
       *
       * @return the tokenizer's next token
       */
      public override Token NextToken(System.IO.Stream r, int theStar, Tokenizer t)
      {
		
         int c = 0;
         int lastc = 0;
         while (c >= 0) 
         {
            if ((lastc == '*') && (c == '/')) 
            {
               break;
            }
            lastc = c;
            c = r.ReadByte();
         }
         return t.NextToken();
      }
   }   
     
     
     

   /**
    * This state will either delegate to a comment-handling 
    * state, or return a token with just a slash in it.
    * 
    *
    *
    *
    */
   public class SlashState : TokenizerState 
   {
      /// <summary>
      /// 
      /// </summary>
      protected SlashStarState slashStarState =      new SlashStarState();
      /// <summary>
      /// 
      /// </summary>
      protected SlashSlashState slashSlashState =    new SlashSlashState();
      /**
       * Either delegate to a comment-handling state, or return a 
       * token with just a slash in it.
       *
       * @return   either just a slash token, or the results of 
       *           delegating to a comment-handling state
       */
      public override Token NextToken( System.IO.Stream r, int theSlash, Tokenizer t)
      {
		
         int c = r.ReadByte();
         if (c == '*') 
         {
            return slashStarState.NextToken(r, '*', t);
         }
         if (c == '/') 
         {
            return slashSlashState.NextToken(r, '/', t);
         }
         if (c >= 0) 
         {
            r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
         }
         return new Token(Token.TT_SYMBOL, "/", 0);
      }
   }     
     
     
   /// <summary>
   /// 
   /// </summary>  
   public class NewLineState: TokenizerState
   {
      /// <summary>
      /// 
      /// </summary>
      public override Token NextToken(System.IO.Stream r, int c, Tokenizer t)
      {
         c = r.ReadByte();
         if (c == '\r')
         {
            c = r.ReadByte();
            if (c != '\n')
               r.Seek(-1, System.IO.SeekOrigin.Current);   //r.unread(c);
         }
           
         return new Token(Token.TT_NEWLINE, " ", 0);  
      }
   
   }  
    
    
     
   /**
    * A <code>SymbolNode</code> object is a member of a tree that 
    * contains all possible prefixes of allowable symbols. Multi-
    * character symbols appear in a <code>SymbolNode</code> tree 
    * with one node for each character. 
    * 
    * For example, the symbol <code>=:~</code> will appear in a 
    * tree as three nodes. The first node contains an equals sign, 
    * and has a child; that child contains a colon and has a 
    * child; this third child contains a tilde, and has no 
    * children of its own. If the colon node had another child 
    * for a dollar sign character, then the tree would contain 
    * the symbol <code>=:$</code>.
    * 
    * A tree of <code>SymbolNode</code> objects collaborate to 
    * ReadByte a (potentially multi-character) symbol from an input 
    * stream. A root node with no character of its own finds an 
    * initial node that represents the first character in the 
    * input. This node looks to see if the next character in the 
    * stream matches one of its children. If so, the node 
    * delegates its reading task to its child. This approach 
    * walks down the tree, pulling symbols from the input that 
    * match the path down the tree.
    * 
    * When a node does not have a child that matches the next 
    * character, we will have ReadByte the longest possible symbol 
    * prefix. This prefix may or may not be a valid symbol. 
    * Consider a tree that has had <code>=:~</code> added and has 
    * not had <code>=:</code> added. In this tree, of the three 
    * nodes that contain <code>=:~</code>, only the first and 
    * third contain complete symbols. If, say, the input contains 
    * <code>=:a</code>, the colon node will not have a child that 
    * matches the 'a' and so it will stop reading. The colon node 
    * has to "unread": it must push back its character, and ask 
    * its parent to unread. Unreading continues until it reaches 
    * an ancestor that represents a valid symbol.
    * 
    *
    * 
    * @version 1.0
    */
   public class SymbolNode 
   {
      /// <summary>
      /// 
      /// </summary>
      protected char FValue;
      /// <summary>
      /// 
      /// </summary>
      protected System.Collections.ArrayList children = new System.Collections.ArrayList(); // of Node
      /// <summary>
      /// 
      /// </summary>
      protected bool valid = false;
      /// <summary>
      /// 
      /// </summary>
      protected SymbolNode FParent;
      /**
       * Constructs a SymbolNode with the given parent, representing 
       * the given character.
       *
       * @param   SymbolNode   this node's parent
       *
       * @param   char   this node's character
       */
      public SymbolNode(SymbolNode parent, char Value) 
      {
         this.FParent = parent;
         this.FValue = Value;
      }
      /**
       * Add a line of descendants that represent the characters
       * in the given string.
       */
      public void AddDescendantLine(string s)    // was protected
      {
         if (s.Length > 0) 
         {
            char c = s[0]; //s.charAt(0);
            SymbolNode n = EnsureChildWithChar(c);
            n.AddDescendantLine(s.Substring(1));
         }
      }
      /**
       * Show the symbol this node represents.
       *
       * @return the symbol this node represents
       */
      public virtual string Ancestry() 
      {
         if (FParent == null)
            return FValue.ToString();
            
         return FParent.Ancestry() + FValue.ToString();
      }
      /// <summary>
      ///  Find the descendant that takes as many characters as  possible from the input.
      /// </summary>
      public SymbolNode DeepestRead(System.IO.Stream r)  // was protected
      {

         char c = (char) r.ReadByte();
         SymbolNode n = FindChildWithChar(c);
         if (n == null) 
         {
            r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
            return this;
         }
         return n.DeepestRead(r);
      }
      /// <summary>
      /// Find or create a child for the given character. 
      /// </summary>
      protected SymbolNode EnsureChildWithChar(char c) 
      {
         SymbolNode n = FindChildWithChar(c);
         if (n == null) 
         {
            n = new SymbolNode(this, c);
            children.Add(n); //children.addElement(n);
         }
         return n;
      }
      /**
       * Find a child with the given character.
       */
      protected virtual SymbolNode FindChildWithChar(char c) 
      {
         for (int i = 0; i < children.Count; i++)
         {
            SymbolNode n = (SymbolNode)children[i];
            if (n.FValue == c) 
            {
               return n;
            }                 
         }
         return null;
         /*
         Enumeration e = children.elements();
         while (e.hasMoreElements()) 
         {
            SymbolNode n = (SymbolNode) e.nextElement();
            if (n.myChar == c) 
            {
               return n;
            }
         }
         return null;
         */
      }
      /**
       * Find a descendant which is down the path the given string
       * indicates. 
       */
      protected SymbolNode FindDescendant(string s) 
      {
         char c = s[0];// s.charAt(0);
         SymbolNode n = FindChildWithChar(c);
         if (s.Length == 1) 
         {
            return n;
         }
         return n.FindDescendant(s.Substring(1));
      }
      /**
       * Mark this node as valid, which means its Ancestry is a
       * complete symbol, not just a prefix.
       */
      public void SetValid(bool b)    // was protected
      {
         valid = b;
      }
      /**
       * Give a string representation of this node.
       *
       * @return a string representation of this node
       */
      public override string ToString() 
      {
         return "" + FValue + '(' + valid + ')';
      }
      /**
       * Unwind to a valid node; this node is "valid" if its
       * Ancestry represents a complete symbol. If this node is
       * not valid, put back the character and ask the parent to
       * unwind. 
       */
      public SymbolNode unreadToValid(System.IO.Stream r)  // was protected
      {
		
         if (valid) 
         {
            return this;
         }
         r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(myChar);
         return FParent.unreadToValid(r);
      }
   }
   
   
   
   

   /**
    * This class is a special case of a <code>SymbolNode</code>. A
    * <code>SymbolRootNode</code> object has no symbol of its
    * own, but has children that represent all possible symbols.
    * 
    *
    * 
    * @version 1.0
    */
   public class SymbolRootNode : SymbolNode 
   {
      /// <summary>
      /// 
      /// </summary>
      new protected SymbolNode[] children = new SymbolNode[256];
      /**
       * Create and initialize a root node.
       */
      public SymbolRootNode() : base(null, (char) 0)
      {
         
         Init();
      }
      /**
       * Add the given string as a symbol.
       *
       * @param   string   the character sequence to Add
       */
      public void Add(string s) 
      {
         char c = s[0];
         SymbolNode n = EnsureChildWithChar(c);
         n.AddDescendantLine(s.Substring(1));
         FindDescendant(s).SetValid(true);
      }
      /**
       * A root node has no parent and no character of its own, so 
       * its Ancestry is "".
       *
       * @return an empty string
       */
      public override string Ancestry() 
      {
         return "";
      }
      /**
       * A root node maintains its children in an array instead of
       * a Vector, to be faster.
       */
      protected override SymbolNode FindChildWithChar(char c) 
      {
         return children[c];
      }
      /**
       * Set all possible symbols to be valid children. This means
       * that the decision of which characters are valid one-
       * character symbols lies outside this tree. If a tokenizer
       * asks this tree to produce a symbol, this tree assumes that
       * the first available character is a valid symbol.
       */
      protected void Init() 
      {
         int len = children.Length;
         for (int i = 0; i < len; i++) 
         {
            children[i] = new SymbolNode(this, (char)i);
            children[i].SetValid(true);
         }
      }
      /**
       * Return a symbol string from a reader.
       *
       * @param   System.IO.Stream   a reader to ReadByte from
       *
       * @param   int   the first character of this symbol, already
       *                ReadByte from the reader
       *
       * @return a symbol string from a reader
       */
      public string NextSymbol(System.IO.Stream r, int first)
      {
		
         SymbolNode n1 = FindChildWithChar((char) first);
         SymbolNode n2 = n1.DeepestRead(r);
         SymbolNode n3 = n2.unreadToValid(r);
         return n3.Ancestry();
      }
   }   
   
   
      
     
     
   /**
    * The idea of a symbol is a character that stands on its 
    * own, such as an ampersand or a parenthesis. For example, 
    * when tokenizing the expression   
    *         (isReady) AND (isWilling),
        *  a typical tokenizer would return 7 
    * tokens, including one for each parenthesis and one for 
    * the ampersand. Thus a series of symbols such as 
    *    ) AND (  
    * becomes three tokens, while a series 
    * of letters such as 
    *   isReady
    * becomes a single word token.
    * 
    * Multi-character symbols are an exception to the rule 
    * that a symbol is a standalone character.  For example, a 
    * tokenizer may want less-than-or-equals to tokenize as a 
    * single token. This class provides a method for 
    * establishing which multi-character symbols an object of 
    * this class should treat as single symbols. This allows, 
    * for example, 
    *   "cat &lt;= dog"
    * to tokenize as 
    * three tokens, rather than splitting the less-than and 
    * equals symbols into separate tokens.
    
    * By default, this state recognizes the following multi-
    * character symbols: <code>!=, :-, &lt;=, &lt;=</code> 
    *
    *
    *
    *
    */
   public class SymbolState : TokenizerState 
   {
      SymbolRootNode FSymbols = new SymbolRootNode();
      /**
       * Constructs a symbol state with a default idea of what 
       * multi-character symbols to accept (as described in the 
       * class comment).
       *
       * @return   a state for recognizing a symbol
       */
      public SymbolState() 
      {
         Add("!=");
         Add(":-");
         Add("<=");
         Add(">=");
      }
      /**
       * Add a multi-character symbol.
       *
       * @param   string   the symbol to Add, such as "=:="
       */
      public void Add(string s) 
      {
         FSymbols.Add(s);
      }
      /**
       * Return a symbol token from a reader.
       *
       * @return a symbol token from a reader
       */
      public override Token NextToken( System.IO.Stream r, int first, Tokenizer t)
      {  
         string s = FSymbols.NextSymbol(r, first); 
         return new Token(Token.TT_SYMBOL, s, 0);
      }
   }     
     
     
    
    
  
     
     
     
     
   /**
    * A wordState returns a word from a reader. Like other 
    * states, a tokenizer transfers the job of reading to this 
    * state, depending on an initial character. Thus, the 
    * tokenizer decides which characters may begin a word, and 
    * this state determines which characters may appear as a 
    * second or later character in a word. These are typically 
    * different sets of characters; in particular, it is typical 
    * for digits to appear as parts of a word, but not as the 
    * initial character of a word. 
    *
    * By default, the following characters may appear in a word.
    * The method <code>SetWordChars()</code> allows customizing
    * this.
    * 
    *     From    To
    *      'a', 'z'
    *      'A', 'Z'
    *      '0', '9'
    *
    *     as well as: minus sign, underscore, and apostrophe.
    * 
    *
    *
    *
    *
    */
   public class WordState : TokenizerState 
   {
      /// <summary>
      /// 
      /// </summary>
      protected byte[] Bytes = new byte[16];
      /// <summary>
      /// 
      /// </summary>
      protected bool[] FwordChar = new bool[256];
      /**
       * Constructs a word state with a default idea of what 
       * characters are admissible inside a word (as described in 
       * the class comment). 
       *
       * @return   a state for recognizing a word
       */
      public WordState() 
      {
         SetWordChars('a', 'z', true);
         SetWordChars('A', 'Z', true);
         SetWordChars('0', '9', true);
         SetWordChars('-', '-', true);
         SetWordChars('_', '_', true);
         SetWordChars('\'', '\'', true);
         SetWordChars(0xc0, 0xff, true);
      }
      /**
       * Fatten up charbuf as necessary.
       */
      protected void CheckBufLength(int i) 
      {
         if (i >= Bytes.Length) 
         {
            byte[] nb = new byte[Bytes.Length * 2];
            System.Array.Copy(Bytes, 0, nb, 0, Bytes.Length);
            Bytes = nb;
         }
      }
      /**
       * Return a word token from a reader.
       *
       * @return a word token from a reader
       */
      public override Token NextToken(System.IO.Stream r, int c, Tokenizer t)
      {
		
         int i = 0;
         do 
         {
            CheckBufLength(i);
            Bytes[i++] = (byte) c;
            c = r.ReadByte();
         } while (wordChar(c));
	
         if (c >= 0) 
         {
            r.Seek(-1, System.IO.SeekOrigin.Current);//r.unread(c);
         }
         
         char[] Chars = new char[Encoding.Default.GetCharCount(Bytes, 0, i)];  
         Encoding.Default.GetChars(Bytes, 0, i, Chars, 0); 
         
         string sval = new string(Chars);   //string sval = string.copyValueOf(charbuf, 0, i);
         
         return new Token(Token.TT_WORD, sval, 0);
      }
      /**
       * Establish characters in the given range as valid 
       * characters for part of a word after the first character. 
       * Note that the tokenizer must determine which characters
       * are valid as the beginning character of a word.
       *
       * @param   from   char
       *
       * @param   to   char
       *
       * @param   bool   true, if this state should allow
       *                    characters in the given range as part
       *                    of a word
       */
      public void SetWordChars(int from, int to, bool b) 
      {
         for (int i = from; i <= to; i++) 
         {
            if (i >= 0 && i < FwordChar.Length) 
            {
               FwordChar[i] = b;
            }
         }
      }
      /**
       * Just a test of the wordChar array.
       */
      protected bool wordChar(int c) 
      {
         if (c >= 0 && c < FwordChar.Length) 
         {
            return FwordChar[c];
         }
         return false;
      }
   }     
   
   
     
      
     

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
      protected static   int DEFAULT_SYMBOL_MAX = 4;  	
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
      public NumberState NumberState { get { return FNumberState; }  }           
      /**
       * Return the state this tokenizer uses to build quoted 
       * strings.
       *
       * @return  the state this tokenizer uses to build quoted 
       *          strings
       */
      public QuoteState QuoteState  { get { return FQuoteState; } }
      /**
       * Return the state this tokenizer uses to recognize
       * (and ignore) comments.
       *
       * @return  the state this tokenizer uses to recognize
       *          (and ignore) comments
       *
       */
      public SlashState SlashState  { get { return FSlashState; } }
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

         SetCharacterState(   0,   ' ', FWhitespaceState);
         SetCharacterState( 'a',   'z', FWordState);
         SetCharacterState( 'A',   'Z', FWordState);
         SetCharacterState(0xc0,  0xff, FWordState);
         SetCharacterState( '0',   '9', FNumberState);
         SetCharacterState( '-',   '-', FNumberState);
         SetCharacterState( '.',   '.', FNumberState);
         SetCharacterState( '"',   '"', FQuoteState);
         SetCharacterState( '\'', '\'', FQuoteState);
         SetCharacterState( '/',   '/', FSlashState);
         SetCharacterState( '\r',   '\r', FNewLineState);
         SetCharacterState( '\n',   '\n', FNewLineState);
      }
      /**
       * Constructs a tokenizer to ReadByte from the supplied string.
       *
       * @param   string   the string to ReadByte from
       */
      public Tokenizer(string s): this () 
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
     
     
     
     



   /**
    * A TokenString is like a string, but it is a series of 
    * Tokens rather than a series of chars. Once a TokenString is 
    * created, it is "immutable", meaning it cannot change. This
    * lets you freely copy TokenStrings without worrying about 
    * their state. 
    * 
    *
    * 
    *
    */

   public class TokenString: ICloneable 
   {
      /**
       * the FTokens in this tokenString
       */
      protected Token[] FTokens;
      /**
       * Constructs a tokenString from the supplied FTokens.
       *
       * @param   FTokens   the FTokens to use
       *
       * @return    a tokenString constructed from the supplied 
       *            FTokens
       */
      public TokenString(Token[] Tokens) 
      {
         this.FTokens = Tokens;
      }
      /**
       * Constructs a tokenString from the supplied string. 
       *
       * @param   string   the string to tokenize
       *
       * @return    a tokenString constructed from FTokens read from 
       *            the supplied string
       */
      public TokenString(string s): this(new Tokenizer(s)) 
      {          
      }
      /**
       * Constructs a tokenString from the supplied reader and 
       * tokenizer. 
       * 
       * @param   Tokenizer   the tokenizer that will produces the 
       *                      FTokens
       *
       * @return    a tokenString constructed from the tokenizer's 
       *            FTokens
       */
      public TokenString(Tokenizer t) 
      {
         ArrayList v = new ArrayList();
         try 
         {
            while (true) 
            {
               Token tok = t.NextToken();
               if (tok.Kind == Token.TT_EOF)
                  break;

               v.Add(tok);
            };
         } 
         catch (Exception e) 
         {
            throw(new Exception("Problem tokenizing string: " + e.Message));

         }
         FTokens = new Token[v.Count];
         v.CopyTo(FTokens);
      }
      /**
       * Returns the number of FTokens in this tokenString.
       *
       * @return   the number of FTokens in this tokenString
       */
      public int Length() 
      {
         return FTokens.Length;
      }
      /**
       * Returns the token at the specified index.
       *
       * @param    index   the index of the desired token
       * 
       * @return   token   the token at the specified index
       */
      public Token TokenAt(int i)
      {
         return FTokens[i];
      }
      /**
       * Returns a string representation of this tokenString. 
       *
       * @return   a string representation of this tokenString
       */
      public override string ToString() 
      {
         StringBuilder buf = new StringBuilder();
         for (int i = 0; i < FTokens.Length; i++) 
         {
            if (i > 0) 
            {
               buf.Append(" ");
            }	
            buf.Append(FTokens[i]);
         }
         return buf.ToString();
      }
      /**
      */
      public object Clone()
      {
         Token[] Tokens = new Token[FTokens.Length];
         for (int i = 0; i < FTokens.Length; i++)
            Tokens[i] = (Token)FTokens[i].Clone();
             
         return new TokenString(Tokens);
      }
   }

  
  
  

   /**
    * A TokenStringSource enumerates over a specified reader, 
    * returning TokenStrings delimited by a specified FDelimiter.
    * For example, 
    * <blockquote><pre>
    *   
    *    string s = "I came; I saw; I left in peace;";
    *
    *    TokenStringSource tss =
    *        new TokenStringSource(new Tokenizer(s), ";");
    *
    *    while (tss.HasMoreTokenStrings()) {
    *        System.out.println(tss.NextTokenString());
    *    }	
    * 
    * </pre></blockquote>
    * 
    * prints out:
    * 
    * <blockquote><pre>    
    *     I came
    *     I saw
    *     I left in peace
    * </pre></blockquote>
    * 
    *
    * 
    * @version 1.0
    */

   public class TokenStringSource 
   {
      /// <summary>
      /// 
      /// </summary>
      protected Tokenizer FTokenizer;
      /// <summary>
      /// 
      /// </summary>
      protected string FDelimiter;
      /// <summary>
      /// 
      /// </summary>
      protected TokenString FCachedTokenString = null;
      /**
       * Constructs a TokenStringSource that will read TokenStrings
       * using the specified FTokenizer, delimited by the specified 
       * FDelimiter.
       *
       * @param   FTokenizer   a FTokenizer to read tokens from
       *
       * @param   FDelimiter   the character that fences off where one 
       *                      TokenString ends and the next begins
       *
       * @returns   a TokenStringSource that will read TokenStrings
       *            from the specified FTokenizer, delimited by the 
       *            specified FDelimiter
       */
      public TokenStringSource (Tokenizer Tokenizer, string Delimiter)
      {
         this.FTokenizer = Tokenizer;
         this.FDelimiter = Delimiter;
      }
      /**
       * The design of <code>NextTokenString</code> is that is 
       * always returns a cached value. This method will (at least 
       * attempt to) load the cache if the cache is empty.
       */
      protected void EnsureCacheIsLoaded() 
      {
         if (FCachedTokenString == null)
            LoadCache();
      }
      /**
       * Returns true if the source has more TokenStrings.
       *
       * @return   true, if the source has more TokenStrings that 
       *           have not yet been popped with <code>
       *           NextTokenString</code>.
       */
      public bool HasMoreTokenStrings()
      {
         EnsureCacheIsLoaded();
         return FCachedTokenString != null;
      }
      /**
       * Loads the next TokenString into the cache, or sets the 
       * cache to null if the source is out of tokens.
       */
      protected void LoadCache() 
      {
         ArrayList tokenVector = NextVector();
         if (tokenVector.Count == 0) 
            FCachedTokenString = null;
         else 
         {
            Token[] tokens = new Token[tokenVector.Count];
            tokenVector.CopyTo(tokens);
            FCachedTokenString = new TokenString(tokens);
         }	
      }
      /**
       * Shows the example in the class comment.
       *
       * @param args ignored

      public static void main(string args[]) {
	
         string s = "I came; I saw; I left in peace;";
	
         TokenStringSource tss =
            new TokenStringSource(new Tokenizer(s), ";");
		
         while (tss.HasMoreTokenStrings()) {
            System.out.println(tss.NextTokenString());
         }	
      }  
      */
      /**
       * Returns the next TokenString from the source.
       *
       * @return   the next TokenString from the source
       */
      public TokenString NextTokenString() 
      {
         EnsureCacheIsLoaded();
         TokenString returnTokenString = FCachedTokenString;
         FCachedTokenString = null;
         return returnTokenString;
      }
      /**
       * Returns a ArrayList of the tokens in the source up to either 
       * the FDelimiter or the end of the source.
       *
       * @return   a ArrayList of the tokens in the source up to either
       *           the FDelimiter or the end of the source.
       */
      protected ArrayList NextVector() 
      {
         ArrayList v = new ArrayList();
         try 
         {
            while (true) 
            {
               Token tok = FTokenizer.NextToken();
               if (tok.Kind == Token.TT_EOF || tok.AsString.Equals(FDelimiter))
                  break;

               v.Add(tok);
            }	
         }	
         catch (Exception e)
         {
            throw(new Exception("Problem tokenizing string: " + e.Message));
         }
         return v;
      }
   }  
  
  
}
