/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Tripous.Data
{
 

    /// <summary>
    /// Describes a criterion item.
    /// </summary>
    public class SqlFilterDef 
    {
        /// <summary>
        /// Field
        /// </summary>
        static public readonly List<string> ValidAggregateFunctions = new List<string>(new string[] { "", "count", "avg", "sum", "max", "min" });
        string fAggregateFunc;
        SqlFilterEnum fSqlFilterEnum;


        /// <summary>
        /// It is used when the data type of the criterion is either <see cref="SqlFilterMode.EnumConst"/> or <see cref="SqlFilterMode.EnumQuery"/>
        /// </summary>
        public class SqlFilterEnum 
        {
            List<string> fOptionList;

            /* construction */
            /// <summary>
            /// Constructor.
            /// </summary>
            public SqlFilterEnum()
            {
            }

            /* properties */
            /// <summary>
            /// Gets or sets the SQL SELECT statement of a <see cref="SqlFilterMode.EnumQuery"/> criterion.
            /// </summary>
            public string Sql { get; set; }
            /// <summary>
            /// Ges or sets the result field  
            /// </summary>
            public string ResultField { get; set; } = "Id";
            /// <summary>
            /// For EnumConst and EnumQuery only items. When true the user interface presents
            /// a multi choise control, otherwise a combo box is presented.
            /// </summary>
            public bool IsMultiChoise { get; set; }
            /// <summary>
            /// Gets the list of constant options. Used only when the filter is a <see cref="SqlFilterMode.EnumConst"/>  filter. 
            /// </summary>
            public List<string> OptionList
            {
                get
                {
                    if (fOptionList == null)
                        fOptionList = new List<string>();
                    return fOptionList;
                }
                set { fOptionList = value; }
            }
            /// <summary>
            /// When true, constant options are displayed initially to the user as checked.
            /// </summary>
            public bool IncludeAll { get; set; }
            /// <summary>
            /// A list where each line is FIELD_NAME=Title
            /// </summary>
            public string DisplayLabels { get; set; }
        }

 
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlFilterDef()
        {
        }

        /* public */
        /// <summary>
        /// Throws exception if this instance is not a valid one.
        /// </summary>
        public void CheckDescriptor()
        {

            string Format = Res.GS("E_EmptyField", "Field \"{0}\" can not be null or empty");
  
            if (Mode == SqlFilterMode.EnumConst)
            {
                if (Enum.OptionList == null || Enum.OptionList.Count == 0)
                    Sys.Throw(Format, Res.GS("SqlFilter_Enum_ConstantOptionsList", "EnumConst Sql Filter. List of constants not defined."));

                if (!Bf.In(this.DataType, DataFieldType.String | DataFieldType.Integer))
                    Sys.Throw("EnumConst Sql Filter. Invalid data type. Only string and integer is allowed.");
            }
            else if (Mode == SqlFilterMode.EnumQuery)
            {
                if (string.IsNullOrWhiteSpace(Enum.ResultField))
                    Sys.Throw(Format, Res.GS("SqlFilter_Enum_ResultFieldName", "EnumQuery Sql Filter. Result Field Name not defined."));

                if (!Bf.In(this.DataType, DataFieldType.String | DataFieldType.Integer))
                    Sys.Throw("EnumQuery Sql Filter. Invalid data type. Only string and integer is allowed.");

                if (string.IsNullOrWhiteSpace(Enum.Sql))
                    Sys.Throw(Format, Res.GS("SqlFilter_Enum_Sql", "EnumQuery Sql Filter. SELECT Sql is not defined."));

                Format = Res.GS("E_InvalidDisplayLabels", "Invalid field titles in line {0} ");
                if (Enum.DisplayLabels != null && Enum.DisplayLabels.Length > 0)
                {
                    string[] Lines = Enum.DisplayLabels.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < Lines.Length; i++)
                        if (!Lines[i].Contains('='))
                            Sys.Throw(Format, i + 1);
                }

            }
        }


        /// <summary>
        /// Sets the value of a property of this instance. Returns this instance.
        /// </summary>
        public SqlFilterDef SetUseRange(bool Value = true)
        {
            this.UseRange = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property of this instance. Returns this instance.
        /// </summary>
        public SqlFilterDef SetPutInHaving(bool Value = true)
        {
            this.PutInHaving = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property of this instance. Returns this instance.
        /// </summary>
        public SqlFilterDef SetAggregateFunc(string Value = "count")
        {
            this.AggregateFunc = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property of this instance. Returns this instance.
        /// </summary>
        public SqlFilterDef SetInitialValue(string Value)
        {
            this.InitialValue = Value;
            return this;
        }

        /* properties */
        /// <summary>
        /// The full path to the field, i.e. TableAlias.FieldName
        /// </summary>
        public string FieldPath { get; set; }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Sys.None; }
            set { }
        }

        /// <summary>
        /// Gets or sets the "data type" of the filter.
        /// </summary>
        public DataFieldType DataType { get; set; } = DataFieldType.String;
        /// <summary>
        /// Indicates how the user enters of selects the filter value
        /// </summary>
        public SqlFilterMode Mode { get; set; } = SqlFilterMode.Simple;
 
        /// <summary>
        /// Valid ONLY when Mode is Simple and DataType String, Float or Integer.
        /// <para>Date and Time are ALWAYS used as a range from-to.</para>
        /// <para>Defaults to false</para>
        /// </summary>
        public bool UseRange { get; set; }
        /// <summary>
        /// Gets or sets the name of the locator descriptor, used when this is of a Locator DataType
        /// </summary>
        public string Locator { get; set; }
        /// <summary>
        /// When true then the produced CommandText goes to the HAVING clause, instead of the WHERE clause
        /// </summary>
        public bool PutInHaving { get; set; }
        /// <summary>
        /// Gets or sets the aggregation function (sum, count, avg, min, max) to use, when <see cref="PutInHaving"/> is true.
        /// It could be an empty string.
        /// </summary>
        public string AggregateFunc
        {
            get { return ValidAggregateFunctions.IndexOf(fAggregateFunc) != -1 ? fAggregateFunc : string.Empty; }
            set { fAggregateFunc = value; }
        }

        /// <summary>
        /// Gets the Enum.
        /// <para>Valid ONLY when DataType is String or Integer</para>
        /// </summary>
        public SqlFilterEnum Enum
        {
            get
            {
                if (fSqlFilterEnum == null)
                    fSqlFilterEnum = new SqlFilterEnum();
                return fSqlFilterEnum;
            }
            set { fSqlFilterEnum = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string InitialValue { get; set; }




    }
}
