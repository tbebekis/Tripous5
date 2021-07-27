/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Tripous
{


    /// <summary>
    /// CommandLine class by 
    ///	GriffonRL 	
    /// article in CodeProject at
    ///	http://www.codeproject.com/csharp/command_line.asp
    ///
    /// This class parses your command line arguments, find all parameters 
    /// starting with -, -- or / and all the values linked. I assumed that
    /// a value could be separated from a parameter with a space, a : or a =.
    /// The parser also look for enclosing characters like ' or " and remove them.
    /// Of course if you have a value like 'Mike's house', only the first and
    /// last ' will be removed. To achieve its goal, the class relies heavily
    /// on the regular expressions capabilities of .NET. The first regular
    /// expression (^-{1,2}|^/|=|:) splits one argument into several parts:
    ///
    ///     * the parameter
    ///     * the value
    ///
    /// This regular expression handles cases where only a parameter is present,
    /// only a value is present or if both are present. The program performs
    /// accordingly to the number of parts found. The second regular
    /// expression (^['"]?(.*?)['"]?$) is used to detect and remove all starting
    /// and trailing ' or " characters from a value. When all your arguments are
    /// parsed, retrieving a value from a parameter is as easy as
    /// writing MyValue=params["MyParam"]. If the parameter doesn't exist
    /// or was not in the command line then you will get a null reference you
    /// can test against.
    ///
    ///
    /// Execution sample
    ///
    /// I provided the following command line as the CommandLine setting in the
    /// properties dialog of the Visual Studio .NET solution included in the ZIP file:
    /// 	-size=100 /height:'400' -param1 "Nice stuff !" --debug
    ///
    /// that command line produced the following output
    ///
    /// Param1 value: Nice stuff !
    /// Height value: 400
    /// Width not defined !
    /// Size value: 100
    /// Debug value: true
    /// CommandLine parsed. Press a key...	
    /// </summary>
    public class CommandLine
    {
        private StringDictionary Parameters;

        /// <summary>
        ///  constructor
        /// </summary>
        public CommandLine(string[] Args)
        {
            Parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
             RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
             RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                 Remover.Replace(Parts[0], "$1");

                                Parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }

                        Parameter = Parts[1];

                        // Remove possible enclosing characters (",')
                        if (!Parameters.ContainsKey(Parameter))
                        {
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            Parameters.Add(Parameter, Parts[2]);
                        }

                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!Parameters.ContainsKey(Parameter))
                    Parameters.Add(Parameter, "true");
            }
        }

        /// <summary>
        ///  Retrieve a parameter value if it exists (overriding C# indexer property)
        /// </summary>
        public string this[string Param]
        {
            get
            {
                return (Parameters[Param]);
            }
        }

    }


}
