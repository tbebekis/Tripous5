/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace Tripous.Data
{




    /// <summary>
    /// Represents the settings of a column, resulting by user actions
    /// </summary>
    public class ColumnSetting //: NamedItem
    {
        string formatString;
        string aggregateFormat;


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnSetting()
        {
            GroupIndex = -1;
            Visible = true;
            Width = 90;
        }

        /* methods */
        /// <summary>
        /// Returns a format string. It uses the FormatString property first,
        /// and then the Decimals property.
        /// </summary>
        public string GetDisplayFormat()
        {
            if (!string.IsNullOrEmpty(FormatString))
                return FormatString;

            if (Decimals != 0)
                return Sys.FormatStringFor(Decimals);

            return string.Empty;
        }

        /* properties */
        /// <summary>
        /// The Name must be unique.
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        [JsonIgnore]
        public string Title => !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name;
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the Column.
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// Gets or sets the Width of the column
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the read only attribute of the column.
        /// </summary>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// Gets or sets the DisplayIndex of the column
        /// </summary>
        public int DisplayIndex { get; set; }
        /// <summary>
        /// Gets or sets the group index of the column
        /// </summary> 
        public int GroupIndex { get; set; }
        /// <summary>
        /// Gets or sets the decimals
        /// </summary>
        public int Decimals { get; set; }
        /// <summary>
        /// Gets or sets the format string
        /// </summary>
        public string FormatString
        {
            get { return !string.IsNullOrEmpty(formatString) ? formatString : string.Empty; }
            set { formatString = value; }
        }
        /// <summary>
        /// Gets or sets the aggregate function type
        /// </summary>
        public AggregateFunctionType Aggregate { get; set; }
        /// <summary>
        /// Gets or sets the aggregate format string
        /// </summary>
        public string AggregateFormat
        {
            get { return !string.IsNullOrEmpty(aggregateFormat) ? aggregateFormat : string.Empty; }
            set { aggregateFormat = value; }
        }
    }

}
