using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Model
{
    /// <summary>
    /// System predefined code producers
    /// </summary>
    static public class SysCodeProducers
    {
        /// <summary>
        /// A code producer that produces a code of the form: XX
        /// </summary>
        public const string Simple2 = "SIMPLE XX";
        /// <summary>
        /// A code producer that produces a code of the form: XXX
        /// </summary>
        public const string Simple3 = "SIMPLE XXX";
        /// <summary>
        /// A code producer that produces a code of the form: XXXX
        /// </summary>
        public const string Simple4 = "SIMPLE XXXX";
        /// <summary>
        /// A code producer that produces a code of the form: XXXXX
        /// </summary>
        public const string Simple5 = "SIMPLE XXXXX";
        /// <summary>
        /// A code producer that produces a code of the form: XXXXXX
        /// </summary>
        public const string Simple6 = "SIMPLE XXXXXX";

        /// <summary>
        /// A code producer that produces a code of the form: XX-XX
        /// </summary>
        public const string Simple4_2 = "SIMPLE XX-XX";
        /// <summary>
        /// A code producer that produces a code of the form: XXX-XXX
        /// </summary>
        public const string Simple6_3 = "SIMPLE XXX-XXX";


        /// <summary>
        /// A code producer that produces a code of the form: YY-XXX
        /// </summary>
        public const string By2Year_3 = "Sys.By2Year_3";
        /// <summary>
        /// A code producer that produces a code of the form: YY-XXXX
        /// </summary>
        public const string By2Year_4 = "Sys.By2Year_4";
        /// <summary>
        /// A code producer that produces a code of the form: YY-XXXXX
        /// </summary>
        public const string By2Year_5 = "Sys.By2Year_5";
        /// <summary>
        /// A code producer that produces a code of the form: YY-XXX-XXX
        /// </summary>
        public const string By2Year_6 = "Sys.By2Year_6";


        /// <summary>
        /// A code producer that produces a code of the form: YYYY-MM-XXX
        /// </summary>
        public const string By4YearMonth_3 = "Sys.By4YearMonth_3";
        /// <summary>
        /// A code producer that produces a code of the form: YYYY-MM-XX-XX
        /// </summary>
        public const string By4YearMonth_4 = "Sys.By4YearMonth_4";
        /// <summary>
        /// A code producer that produces a code of the form: YYYY-MM-XXXXX
        /// </summary>
        public const string By4YearMonth_5 = "Sys.By4YearMonth_5";
        /// <summary>
        /// A code producer that produces a code of the form: YYYY-MM-XXX-XXX
        /// </summary>
        public const string By4YearMonth_6 = "Sys.By4YearMonth_6";

        /// <summary>
        /// A code producer that produces a code of the form: YYYY-MM-DD-XXX-XXX
        /// <para>Used by the SYS_LOG system broker.</para>
        /// </summary>
        public const string SysLogCode = "Sys.LogCode";


        /// <summary>
        /// Registers system code producers
        /// </summary>
        static public void Register()
        {
            CodeDescriptor CodeDescriptor;

            /* all the following code producers have a pivot part mode of CodePartMode.FieldName 
               and a prefix part mode of CodePartMode.Literal.
               The pivot field name is always Code, so the MasterTableName MUST contain a field named Code 
               for any of those code producers to be used. */
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple2, "Code", "XX", "CodeProducer", "");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple3, "Code", "XXX", "CodeProducer", "");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple4, "Code", "XXXX", "CodeProducer", "");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple5, "Code", "XXXXX", "CodeProducer", "");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple6, "Code", "XXXXXX", "CodeProducer", "");

            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple4_2, "Code", "XX-XX", "CodeProducer", "");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.Simple6_3, "Code", "XXX-XXX", "CodeProducer", "");


            /* ByYear_3 - a code producer with a 2-digit year literal and a 3-digit pivot */
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By2Year_3, "Code", "-XXX", "CodeProducer", "YY");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By2Year_4, "Code", "-XXXX", "CodeProducer", "YY");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By2Year_5, "Code", "-XXXXX", "CodeProducer", "YY");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By2Year_6, "Code", "-XXX-XXX", "CodeProducer", "YY");

            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By4YearMonth_3, "Code", "-XXX", "CodeProducer", "YYYY-MM");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By4YearMonth_4, "Code", "-XX-XX", "CodeProducer", "YYYY-MM");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By4YearMonth_5, "Code", "-XXXXX", "CodeProducer", "YYYY-MM");
            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.By4YearMonth_6, "Code", "-XXX-XXX", "CodeProducer", "YYYY-MM");

            CodeDescriptor = Registry.CodeProducers.Add(SysCodeProducers.SysLogCode, "Code", "-XXX-XXX", "CodeProducer", "YYYY-MM-DD");


        }
    }
}
