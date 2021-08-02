using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tripous.Data
{

    /// <summary>
    /// The data-type of a data field
    /// </summary>
    [Flags]
    [TypeStoreItem]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataFieldType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// String (nvarchar, varchar)
        /// </summary>
        String = 1,
        /// <summary>
        /// Integer
        /// </summary>
        Integer = 2,
        /// <summary>
        /// Float (float, double precision, etc)
        /// </summary>
        Float = 4,
        /// <summary>
        /// Decimal (decimal(18, 4))
        /// </summary>
        Decimal = 8,
        /// <summary>
        /// Date (date)
        /// </summary>
        Date = 0x10,
        /// <summary>
        /// DateTime (datetime, timestamp, etc)
        /// </summary>
        DateTime = 0x20,
        /// <summary>
        /// Boolean (integer always, 1 = true, else false)
        /// </summary>
        Boolean = 0x40,
        /// <summary>
        /// Blob
        /// </summary>
        Blob = 0x80,
        /// <summary>
        /// Text Blob
        /// </summary>
        TextBlob = 0x100,
    }

}
