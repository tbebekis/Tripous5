using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL;
using TSQL.Statements;
using TSQL.Tokens;
using TSQL.Clauses;

namespace Test.WinApp
{
    // https://dotnetfiddle.net/U6BfmE
    // https://dotnetfiddle.net/Hz0TvA
    // https://www.codeproject.com/Articles/32524/SQL-Parser


    static public class SqlParserHelper
    {

        static public void Parse(string SqlText)
        {
            TSQLSelectStatement Statement = TSQLStatementReader.ParseStatements(SqlText)[0] as TSQLSelectStatement;
            string S = Statement.From.ToString();
        }
    }
}
