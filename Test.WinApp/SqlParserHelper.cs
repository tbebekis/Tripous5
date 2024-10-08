﻿ 



namespace Test.WinApp
{
    // https://dotnetfiddle.net/U6BfmE
    // https://dotnetfiddle.net/Hz0TvA
    // https://www.codeproject.com/Articles/32524/SQL-Parser


    // https://scholarworks.lib.csusb.edu/cgi/viewcontent.cgi?article=3536&context=etd-project - page 56


    static public partial class SqlParserHelper
    {

        static public void Tokenize(string SqlText)        
        {

            StringBuilder SB = new StringBuilder(); 

            Tokenizer Tokenizer = new Tokenizer();
            Tokenizer.SetString(SqlText);

            Token T = null;
            string S;

            while (true)
            {
                T = Tokenizer.NextToken();

                if (T.Kind == Token.TT_EOF)
                {
                    SB.AppendLine("EOF");
                    break;
                }
                else if (T.Kind == Token.TT_NEWLINE)
                {
                    SB.AppendLine("New Line");
                }
                else if (T.Kind == Token.TT_SYMBOL)
                { 
                    SB.AppendLine($"Symbol: {T.StringValue}" );
                }
                else if (T.Kind == Token.TT_WORD)
                {
                    SB.AppendLine($"Word: {T.StringValue}");
                }
                else if (T != null)
                {
                    S = T.StringValue;
                    if (!string.IsNullOrWhiteSpace(S))
                        SB.AppendLine($"Unknown Token: {S}");
                }
            }


            LogBox.Clear();
            LogBox.AppendLine(SB.ToString());
        }
    }
}
