/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Tripous.Data;

namespace Tripous.Data
{

    /// <summary>
    /// JsonDataColumn
    /// </summary>
    public class JsonDataColumn
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataColumn()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataColumn(DataColumn Source)
        {
            this.Name = Source.ColumnName;
            this.Title = Source.Caption;
            this.TitleKey = Source.TitleKey();
            this.DataType = Source.DataTypeToJson();
            this.Expression = string.IsNullOrWhiteSpace(Source.Expression) ? null : Source.Expression;
            this.DefaultValue = Sys.IsNull(Source.DefaultValue) ? null : Source.DefaultValue.ToString();
            this.MaxLength = Source.MaxLength;
            this.ReadOnly = Source.ReadOnly;
            this.Visible = Source.IsVisibleSet() ? Source.IsVisible() : true;
            this.Required = !Source.AllowDBNull;
            this.Unique = Source.Unique;
 
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataColumn(DataColumn Source, SqlBrokerFieldDef Descriptor)
            : this(Source)
        {
            if (Descriptor != null)
            {
                this.Title = Descriptor.Title;
                this.TitleKey = Descriptor.TitleKey;
                this.DataType = Descriptor.DataType.ToString();
                this.Expression = Descriptor.Expression;
                this.DefaultValue = Descriptor.DefaultValue;
                this.MaxLength = Descriptor.MaxLength;
                this.ReadOnly = Descriptor.IsReadOnly;
                this.Visible = !Descriptor.IsHidden;
                this.Required = Descriptor.IsRequired;

                this.Decimals = Descriptor.Decimals;
                this.Flags = Descriptor.Flags;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataColumn(DataColumn Source, int Decimals, FieldFlags Flags)
            : this(Source)
        {
            this.Decimals = Decimals;
            this.Flags = Flags;
        }
        
        /* public */
        /// <summary>
        /// Converts this to a DataColumn
        /// </summary>
        public DataColumn ToColumn()
        {
            DataColumn Column = new DataColumn();
            Column.ColumnName = this.Name;
            Column.Caption = this.Title;
            var S = this.DataType;
            Column.JsonToDataType(S);

            return Column;
        }

        /* properties */
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// TitleKey
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// DataType (see DataColumn extension method DataTypeToJson() )
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Expression
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// DefaultValue
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// MaxLength
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// Decimals
        /// </summary>
        public int Decimals { get; set; }
        /// <summary>
        /// ReadOnly
        /// </summary>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// Visible
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// Unique
        /// </summary>
        public bool Unique { get; set; }
        /// <summary>
        /// FieldFlags
        /// </summary>
        public FieldFlags Flags { get; set; } 
    }
}
