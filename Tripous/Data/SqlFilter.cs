/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Tripous.Data
{
 

    /// <summary>
    /// Describes a criterion item.
    /// </summary>
    public class SqlFilter : Descriptor
    {
        /// <summary>
        /// Field
        /// </summary>
        static public readonly List<string> ValidAggregateFunctions = new List<string>(new string[] { "", "count", "avg", "sum", "max", "min" });
 
        SqlFilterMode fMode = SqlFilterMode.Simple;
        SimpleType fDataType = SimpleType.String;
        bool fUseRange = false;

        string fLocator;
        string fAggregateFunc;
        bool fPutInHaving = false;

        /// <summary>
        /// It is used when the data type of the criterion is either <see cref="SqlFilterMode.EnumConst"/> or <see cref="SqlFilterMode.EnumQuery"/>
        /// </summary>
        public class SqlFilterEnum : Assignable
        {
            string fSql;
            string fResultField = "Id";
            bool fIsMultiChoise;
            string fConstantOptionsList;
            bool fIncludeAll = true;
            string fDisplayLabels;

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
            public string Sql
            {
                get { return fSql; }
                set
                {
                    fSql = value;
                    OnPropertyChanged("Sql");
                }
            }
            /// <summary>
            /// Ges or sets the result field  
            /// </summary>
            public string ResultField
            {
                get { return fResultField; }
                set
                {
                    fResultField = value;
                    OnPropertyChanged("ResultField");
                }
            }
            /// <summary>
            /// For EnumConst and EnumQuery only items. When true the user interface presents
            /// a multi choise control, otherwise a combo box is presented.
            /// </summary>
            public bool IsMultiChoise
            {
                get { return fIsMultiChoise; }
                set
                {
                    fIsMultiChoise = value;
                    OnPropertyChanged("IsMultiChoise");
                }
            }
            /// <summary>
            /// Gets the list of constant options. Used only when the is a <see cref="SqlFilterMode.EnumConst"/>  criterion. 
            /// </summary>
            public string ConstantOptionsList
            {
                get { return fConstantOptionsList; }
                set
                {
                    fConstantOptionsList = value;
                    OnPropertyChanged("ConstantOptionsList");
                }
            }
            /// <summary>
            /// When true, constant options are displayed initially to the user as checked.
            /// </summary>
            public bool IncludeAll
            {
                get { return fIncludeAll; }
                set
                {
                    fIncludeAll = value;
                    OnPropertyChanged("IncludeAll");
                }
            }
            /// <summary>
            /// A list where each line is FIELD_NAME=Title
            /// </summary>
            public string DisplayLabels
            {
                get { return fDisplayLabels; }
                set
                {
                    fDisplayLabels = value;
                    OnPropertyChanged("DisplayLabels");
                }
            }

        }

 
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlFilter()
        {
        }


        /* public */
        /// <summary>
        /// Override
        /// </summary>
        public override void CheckDescriptor()
        {
            base.CheckDescriptor();


            string Format = Res.GS("E_EmptyField", "Field \"{0}\" can not be null or empty");

            /*  
                        if (string.IsNullOrEmpty(TableName))
                            Sys.Error(Format, Res.GS("Criterion_TableName", "Criterion Table Name"));

                        if (string.IsNullOrEmpty(Name))
                            Sys.Error(Format, Res.GS("Criterion_FieldName", "Criterion Field Name")); 
             */



            if (Mode == SqlFilterMode.EnumConst)
            {
                if (string.IsNullOrWhiteSpace(Enum.ConstantOptionsList))
                    Sys.Throw(Format, Res.GS("Criterion_Enum_ConstantOptionsList", "List of constants"));
            }
            else if (Mode == SqlFilterMode.EnumQuery)
            {
                if (string.IsNullOrEmpty(Enum.ResultField))
                    Sys.Throw(Format, Res.GS("Criterion_Enum_ResultFieldName", "Criterion Enum Result Field Name"));

                if (string.IsNullOrEmpty(Enum.Sql))
                    Sys.Throw(Format, Res.GS("Criterion_Enum_Sql", "Criterion Enum Sql"));

                Format = Res.GS("E_InvalidDisplayLabels", "Invalid field titles in line {0} ");
                string[] Lines = Enum.DisplayLabels.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < Lines.Length; i++)
                    if (!Lines[i].Contains('='))
                        Sys.Throw(Format, i + 1);
            }
        }


        /* properties */
        /// <summary>
        /// Indicates how the user enters of selects the criterion value
        /// </summary>
        public SqlFilterMode Mode
        {
            get { return fMode; }
            set
            {
                fMode = value;
                OnPropertyChanged("Mode");
            }
        }
        /// <summary>
        /// Gets or sets the "data type" of the criterion.
        /// </summary>
        public SimpleType DataType
        {
            get { return fDataType; }
            set
            {
                fDataType = value;
                OnPropertyChanged("DataType");
            }
        }
        /// <summary>
        /// The full path to the field, i.e. TableAlias.FieldName
        /// </summary>
        public string FieldPath
        {
            get { return this.Name; }
            set
            {
                Name = value;
                OnPropertyChanged("FieldPath");
            }
        }
        /// <summary>
        /// Valid ONLY when Mode is Simple and DataType String, Float or Integer.
        /// <para>Date and Time are ALWAYS used as a range from-to.</para>
        /// <para>Defaults to false</para>
        /// </summary>
        public bool UseRange
        {
            get { return fUseRange; }
            set
            {
                fUseRange = value;
                OnPropertyChanged("UseRange");
            }
        }
        /// <summary>
        /// Gets or sets the name of the locator descriptor, used when this is of a Locator DataType
        /// </summary>
        public string Locator
        {
            get { return fLocator; }
            set
            {
                fLocator = value;
                OnPropertyChanged("Locator");
            }
        }
        /// <summary>
        /// Gets or sets the aggregation function (sum, count, avg, min, max) to use, when <see cref="PutInHaving"/> is true.
        /// It could be an empty string.
        /// </summary>
        public string AggregateFunc
        {
            get { return ValidAggregateFunctions.IndexOf(fAggregateFunc) != -1 ? fAggregateFunc : string.Empty; }
            set
            {
                fAggregateFunc = value;
                OnPropertyChanged("AggregateFunc");
            }
        }
        /// <summary>
        /// When true then the produced CommandText goes to the HAVING clause, instead of the WHERE clause
        /// </summary>
        public bool PutInHaving
        {
            get { return fPutInHaving; }
            set
            {
                fPutInHaving = value;
                OnPropertyChanged("PutInHaving");
            }
        }
        /// <summary>
        /// Gets the Enum.
        /// <para>Valid ONLY when DataType is String or Integer</para>
        /// </summary>
        public SqlFilterEnum Enum { get; } = new SqlFilterEnum();

        /// <summary>
        /// 
        /// </summary>
        public string InitialValue { get; set; }




    }
}
