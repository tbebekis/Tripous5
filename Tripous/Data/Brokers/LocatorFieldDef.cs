using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Tripous.Data;

namespace Tripous.Data
{

    /// <summary>
    /// Describes a Locator field.
    /// <para>A field such that may associate a column in the data table (the dest) to a column in the list table (the source).</para>
    /// </summary>
    public class LocatorFieldDef
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorFieldDef()
        {
        }

        /* properties */
        /// <summary>
        /// The field name in the source table
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// When not empty/null then it denotes a field in the dest table where to put the value of this field.
        /// </summary>
        public string DestFieldName { get; set; }
        /// <summary>
        /// The data-type of the field
        /// </summary>
        public DataFieldType DataType { get; set; } = DataFieldType.String;
        /// <summary>
        /// When true the field value is displayed in a UI locator control.
        /// <para>NOTE: There can be only one visible field.</para>
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// When true the field can be part in a where clause in a SELECT statement.
        /// </summary>
        public bool Searchable { get; set; } = true;
        /// <summary>
        /// Used to notify criterial links to treat the field as an integer boolea field (1 = true, 0 = false)
        /// </summary>
        public bool IsIntegerBoolean { get; set; }

        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        [JsonIgnore]
        public string Title => !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name;
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
    }
}
