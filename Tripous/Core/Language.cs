using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;

 

using Newtonsoft.Json;

namespace Tripous
{

    /// <summary>
    /// Language information.  Represents a language this application supports, i.e. provides localized resources for.
    /// </summary>
    public class Language
    { 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Language()
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Id">The id, if any, else null or empty string</param>
        /// <param name="Name">The language name</param>
        /// <param name="Code">The two letter code of the language, e.g en, el, it, fr, etc.</param>
        /// <param name="CultureCode">The culture code associated to this language, e.g.  e.g en-US, el-GR, etc.</param>
        /// <param name="FlagImage">The image file name, i.e. flag_greece.png</param>
        public Language(string Id, string Name, string Code, string CultureCode, string FlagImage = "")
        {
            this.Id = Id;
            this.Name = Name;
            this.Code = Code;
            this.CultureCode = CultureCode;            
            this.FlagImage = FlagImage;
        }
        /// <summary>
        /// Constructor. The specified <see cref="DataRow"/> must have the schema of the SysTables.Lang table.
        /// </summary>
        public Language(DataRow Row)
            : this(Row.AsString("Id"),
                  Row.AsString("Name"),
                  Row.AsString("SeoCode"),
                  Row.AsString("CultureCode"),
                  Row.AsString("FlagImage"))
        {
        }
 
        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return $"{CultureCode} - {Name}";
        }
        /// <summary>
        /// Returns <see cref="CultureInfo"/> instance associated to this language
        /// </summary>
        public CultureInfo GetCulture() { return new CultureInfo(CultureCode); }

        /* properties */
        /// <summary>
        /// The id, if any, else null or empty string
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The language name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The two letter code of the language, e.g en, el, it, fr, etc.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The culture code associated to this language, e.g.  e.g en-US, el-GR, etc.
        /// </summary>
        public string CultureCode { get; set; }
        /// <summary>
        /// The image file name, i.e. flag_greece.png
        /// </summary>
        public string FlagImage { get; set; }

        /// <summary>
        /// User defined information
        /// </summary>
        [JsonIgnore]
        public object Tag { get; set; }

        /// <summary>
        /// String resources
        /// </summary>
        [JsonIgnore]
        public LanguageResourceStringList Resources { get; } = new LanguageResourceStringList();

    }





}
