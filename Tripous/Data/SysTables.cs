/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Helper config class with the system table names
    /// </summary>
    static public class SysTables
    {
        const string SIni = "Ini";
        const string SLog = "Log";
        const string SData = "Data";
        const string SCompany = "Company";
        const string SLang = "Lang";
        const string SStrRes = "StrRes";
        const string SSmtpProvider = "SmtpProvider";

        /// <summary>
        /// Default prefix for system table names
        /// </summary>
        public const string DefaultPrefix = "SYS_";

        static Dictionary<string, string> Names = new Dictionary<string, string>();
 
        static string RemovePrefix(string Name)
        {
            if (Name.StartsWith(Prefix))
                Name = Name.Remove(0, Prefix.Length);
            return Name;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static SysTables()
        {
            Ini = "INI";
            Company = "COMPANY";
            Lang = "LANG";
            Log = "LOG";
            Data = "DATA";            
            StrRes = "STR_RES";
            SmtpProvider = "SMTP_PROVIDER";
        }

        /* system table names */ 
        /// <summary>
        /// Gets the name of SYS_INI system table
        /// <para>Defaults to SYS_INI</para>
        /// </summary>
        static public string Ini
        {
            get { return Prefix + Names[SIni]; }
            set { Names[SIni] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_COMPANY system table
        /// <para>Defaults to SYS_COMPANY</para>
        /// </summary>
        static public string Company
        {
            get { return Prefix + Names[SCompany]; }
            set { Names[SCompany] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_LANG system table
        /// <para>Defaults to SYS_LANG</para>
        /// </summary>
        static public string Lang
        {
            get { return Prefix + Names[SLang]; }
            set { Names[SLang] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_LOG system table
        /// <para>Defaults to SYS_LOG</para>
        /// </summary>
        static public string Log
        {
            get { return Prefix + Names[SLog]; }
            set { Names[SLog] = RemovePrefix(value); }
        } 
        /// <summary>
        /// Gets the name of SYS_DATA system table
        /// <para>Defaults to SYS_DATA</para>
        /// </summary>
        static public string Data
        {
            get { return Prefix + Names[SData]; }
            set { Names[SData] = RemovePrefix(value); }
        } 
        /// <summary>
        /// Gets the name of SYS_STR_RES system table
        /// <para>Defaults to SYS_STR_RES</para>
        /// </summary>
        static public string StrRes
        {
            get { return Prefix + Names[SStrRes]; }
            set { Names[SStrRes] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_SMTP_PROVIDER system table
        /// <para>Defaults to SYS_SMTP_PROVIDER</para>
        /// </summary>
        static public string SmtpProvider
        {
            get { return Prefix + Names[SSmtpProvider]; }
            set { Names[SSmtpProvider] = RemovePrefix(value); }
        }

        /* miscs */
        /// <summary>
        /// Gets or sets the string used as a prefix
        /// when constructing system table nick-names
        /// </summary>
        static public string Prefix { get; set; } = DefaultPrefix;
        /// <summary>
        /// Returns a dictionary with system table names where Keys are the property names of this class.
        /// </summary>
        static public Dictionary<string, string> GetDictionary()
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            foreach (var Entry in Names)
            {
                Result[Entry.Key] = Entry.Value;
            }

            return Result;
        }
    }
}
