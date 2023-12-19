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
    public class SelectSqlColumn 
    {
        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectSqlColumn()
        {
            GroupIndex = -1;
            Visible = true;
            Width = 90;
        }

        /* public */
        /// <summary>
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                Sys.Throw(Res.GS("E_SelectSqlColumn_NameIsEmpty", "SelectSqlColumn Name is empty"));
        }
        /// <summary>
        /// Returns a format string. It uses the FormatString property first,
        /// and then the Decimals property.
        /// </summary>
        public string GetDisplayFormat()
        {
            if (!string.IsNullOrWhiteSpace(FormatString))
                return FormatString;

            if (Decimals != 0)
                return Sys.FormatStringFor(Decimals);

            return string.Empty;
        }


        /// <summary>
        /// Sets a property value and returns this instance.
        /// </summary>
        public SelectSqlColumn SetVisible(bool Value)
        {
            this.Visible = Value;
            return this;
        }
        /// <summary>
        /// Sets a property value and returns this instance.
        /// </summary>
        public SelectSqlColumn SetWidth(int Value)
        {
            this.Width = Value;
            return this;
        }
        /// <summary>
        /// Sets a property value and returns this instance.
        /// </summary>
        public SelectSqlColumn SetReadOnly(bool Value)
        {
            this.ReadOnly = Value;
            return this;
        }
        /// <summary>
        /// Sets a property value and returns this instance.
        /// </summary>
        public SelectSqlColumn SetGroupIndex(int Value)
        {
            this.GroupIndex = Value;
            return this;
        }
        /// <summary>
        /// Sets a property value and returns this instance.
        /// </summary>
        public SelectSqlColumn SetDecimals(int Value)
        {
            this.Decimals = Value;
            return this;
        }
        /// <summary>
        /// Sets a property value and returns this instance.
        /// </summary>
        public SelectSqlColumn SetAggregate(AggregateFunctionType Value)
        {
            this.Aggregate = Value;
            return this;
        }

        /* properties */
        /// <summary>
        /// The name of the field this column represents.
        /// </summary> 
        public string Name { get; set; } = "";
        /// <summary>
        /// The display type of a column. Used with grids.
        /// </summary>
        public ColumnDisplayType DisplayType { get; set; } = ColumnDisplayType.Default;

        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name; }
            set { }
        }

        /// <summary>
        /// Gets or sets the visibility of the Column.
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// Gets or sets the Width of the column
        /// </summary>
        public int Width { get; set; } = 90;
        /// <summary>
        /// Gets or sets the read only attribute of the column.
        /// </summary>
        public bool ReadOnly { get; set; } = true;
        /// <summary>
        /// Gets or sets the DisplayIndex of the column
        /// </summary>
        public int DisplayIndex { get; set; } = 0;
        /// <summary>
        /// Gets or sets the group index of the column. -1 means not defined
        /// </summary> 
        public int GroupIndex { get; set; } = -1;   // -1 means not defined
        /// <summary>
        /// Gets or sets the decimals. -1 means not defined
        /// </summary>
        public int Decimals { get; set; } = -1;     // -1 means not defined
        /// <summary>
        /// Gets or sets the format string
        /// </summary>
        public string FormatString { get; set; } = "";

        /// <summary>
        /// Gets or sets the aggregate function type
        /// </summary>
        public AggregateFunctionType Aggregate { get; set; } = AggregateFunctionType.None;
        /// <summary>
        /// Gets or sets the aggregate format string
        /// </summary>
        public string AggregateFormat { get; set; } = "";
    }

}
