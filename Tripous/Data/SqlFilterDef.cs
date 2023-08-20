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
 
        string fTitleKey;
 
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
            if (string.IsNullOrWhiteSpace(FieldPath))
                Sys.Throw(Res.GS("E_SqlFilterDef_NoFieldPath", "SqlFilterDef must have a FieldPath"));

            string FirstPart = Res.GS("E_SqlFilterDef_Invalid", "Invalid SqlFilterDef: \"{0}\"."); 
            FirstPart = string.Format(FirstPart, FieldPath) + " ";

            if (Bf.In(this.Mode, SqlFilterMode.EnumConst | SqlFilterMode.EnumQuery))
                if (!Bf.In(this.DataType, DataFieldType.String | DataFieldType.Integer))
                    Sys.Throw(FirstPart + Res.GS("SqlFilterDef_InvalidDataType", "Invalid data type. Only string and integer is allowed."));

            if (Mode == SqlFilterMode.EnumConst)
            {
                if (EnumOptionList == null || EnumOptionList.Count == 0)
                    Sys.Throw(FirstPart + Res.GS("SqlFilterDef_EnumConst_NoOptionsList", "Enum Constant: Option List not defined."));
            }
            else if (Mode == SqlFilterMode.EnumQuery)
            {
                if (string.IsNullOrWhiteSpace(EnumResultField))
                    Sys.Throw(FirstPart + Res.GS("SqlFilterDef_EnumQuery_NoResultField", "Enum Query. Result Field Name not defined."));
 

                if (string.IsNullOrWhiteSpace(EnumSql))
                    Sys.Throw(FirstPart + Res.GS("SqlFilterDef_EnumQuery_NoSql", "Enum Query. SELECT Sql is not defined."));

                string Format = Res.GS("E_InvalidDisplayLabels", "Invalid field titles in line {0} ");
                if (EnumDisplayLabels != null && EnumDisplayLabels.Length > 0)
                {
                    string[] Lines = EnumDisplayLabels.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < Lines.Length; i++)
                        if (!Lines[i].Contains('='))
                            Sys.Throw(FirstPart + Format, i + 1);
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
        /// The full path to the field, i.e. TableAlias.FieldName, or just FieldName
        /// </summary>
        public string FieldPath { get; set; }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : FieldPath; }
            set { fTitleKey = value; }
        }
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

 

        /* enum properties - Valid ONLY when Mode is Enum and DataType is String or Integer */

        /// <summary>
        /// Gets or sets the SQL SELECT statement of a <see cref="SqlFilterMode.EnumQuery"/> criterion.
        /// </summary>
        public string EnumSql { get; set; }
        /// <summary>
        /// Ges or sets the result field  
        /// </summary>
        public string EnumResultField { get; set; } = "Id";
        /// <summary>
        /// For EnumConst and EnumQuery only items. When true the user interface presents
        /// a multi choise control, otherwise a combo box is presented.
        /// </summary>
        public bool EnumIsMultiChoise { get; set; }
        /// <summary>
        /// Gets the list of constant options. Used only when the filter is a <see cref="SqlFilterMode.EnumConst"/>  filter. 
        /// </summary>
        public List<string> EnumOptionList { get; set; } = new List<string>();
 
        /// <summary>
        /// When true, constant options are displayed initially to the user as checked.
        /// </summary>
        public bool EnumIncludeAll { get; set; }
        /// <summary>
        /// A list where each line is FIELD_NAME=Title
        /// </summary>
        public string EnumDisplayLabels { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string InitialValue { get; set; }




    }
}
