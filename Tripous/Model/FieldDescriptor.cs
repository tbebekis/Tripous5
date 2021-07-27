/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;


namespace Tripous.Model
{

    /// <summary>
    /// Describes a field
    /// </summary>
    public class FieldDescriptor : FieldDescriptorBase
    {
        private string defaultValue = Sys.NULL;
        private string expression;
        //private string keyField;
        private string lookUpTableName;         // COUNTRY
        private string lookUpTableAlias;        // COUNTRY
        //private string lookUpKeyField;          // CUSTOMER.COUNTRY_ID
        private string lookUpResultField;       // COUNTRY.ID   -ValueMember
        private string lookUpDisplayFields;     // COUNTRY.NAME -DisplayMember-s, to create look up combo boxes

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldDescriptor()
        {
        }

        /// <summary>
        /// Override
        /// </summary>
        public override void CheckDescriptor()
        {
            base.CheckDescriptor();
        }

        /* methods */
        /// <summary>
        /// Gets a comma text field list for constructing SQL statements
        /// </summary>
        public string GetLookUpFieldList()
        {
            string[] Lines = LookUpDisplayFields.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> List = new List<string>(Lines);
            List.Insert(0, LookUpResultField);
            return List.CommaText();
        }
        /// <summary>
        /// Returns the first field name from the LookUpDisplayFields
        /// </summary>
        public string GetFirstLookUpDisplayField()
        {
            string[] Lines = LookUpDisplayFields.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if ((Lines != null) && (Lines.Length > 0))
                return Lines[0];

            return string.Empty;
        }

        /* properties */
        /// <summary>
        /// Gets or sets the default value of the field.
        /// </summary>
        public string DefaultValue
        {
            get { return string.IsNullOrEmpty(defaultValue) ? Sys.NULL : defaultValue; }
            set { defaultValue = value; }
        }
        /// <summary>
        /// Gets or sets the expression used to calculate the values in a column, or create an aggregate column
        /// </summary>
        public string Expression
        {
            get { return string.IsNullOrEmpty(expression) ? string.Empty : expression; }
            set { expression = value; }
        }
        /// <summary>
        /// Gets the table descriptor this field belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public new TableDescriptor Table { get { return base.Table as TableDescriptor; } }



        /* look-up relation */
        /*  
                /// <summary>
                /// Gets or sets the key field of the table this field belongs too, which is used in the look-up relation.
                /// That key field must match the <see cref="LookUpKeyField"/>.
                /// <para>Valid when this field is the result of a look-up operation.</para>
                /// </summary>
                public string KeyField
                {
                    get { return string.IsNullOrEmpty(keyField) ? string.Empty : keyField; }
                    set { keyField = value; }
                } 
         */
        /// <summary>
        /// Gets or sets the name of the look-up table.
        /// <para>Valid when this field is the result of a look-up operation.</para>
        /// </summary>
        /// <remarks>
        /// <para></para>
        /// <example> Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
        /// and a COUNTRY table with ID and NAME fields. To establish a look up
        /// <code>
        ///     LookUpTableName = "COUNTRY";                           
        ///     LookUpKeyFields = "COUNTRY_ID";  // CUSTOMER.COUNTRY_ID
        ///     LookUpResultField = "ID";        // COUNTRY.ID         
        ///     LookUpDisplayFields = "NAME";    // COUNTRY.NAME       
        ///  </code>
        /// </example>
        /// </remarks>
        public string LookUpTableName
        {
            get { return string.IsNullOrEmpty(lookUpTableName) ? string.Empty : lookUpTableName; }
            set { lookUpTableName = value; }
        }
        /// <summary>
        /// Gets or sets the Alias of the look-up table
        /// <para>Valid when this field is the result of a look-up operation.</para>
        /// </summary>
        /// <remarks>
        /// <para></para>
        /// <example> Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
        /// and a COUNTRY table with ID and NAME fields. To establish a look up
        /// <code>
        ///     LookUpTableName = "COUNTRY";                           
        ///     LookUpKeyFields = "COUNTRY_ID";  // CUSTOMER.COUNTRY_ID
        ///     LookUpResultField = "ID";        // COUNTRY.ID         
        ///     LookUpDisplayFields = "NAME";    // COUNTRY.NAME       
        ///  </code>
        /// </example>
        /// </remarks>
        public string LookUpTableAlias
        {
            get { return string.IsNullOrEmpty(lookUpTableAlias) ? LookUpTableName : lookUpTableAlias; }
            set { lookUpTableAlias = value; }
        }
        /*  
                /// <summary>
                /// Gets or sets the key field of the look-up table that must match to the <see cref="KeyField"/>.
                /// <para>Valid when this field is the result of a look-up operation.</para>
                /// </summary>
                /// <remarks>
                /// <para></para>
                /// <example> Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
                /// and a COUNTRY table with ID and NAME fields. To establish a look up
                /// <code>
                ///     LookUpTableName = "COUNTRY";                           
                ///     LookUpKeyFields = "COUNTRY_ID";  // CUSTOMER.COUNTRY_ID
                ///     LookUpResultField = "ID";        // COUNTRY.ID         
                ///     LookUpDisplayFields = "NAME";    // COUNTRY.NAME       
                ///  </code>
                /// </example>
                /// </remarks>
                public string LookUpKeyField
                {
                    get { return string.IsNullOrEmpty(lookUpKeyField) ? string.Empty : lookUpKeyField; }
                    set { lookUpKeyField = value; }
                } 
         */
        /// <summary>
        /// Gets or sets the name of the result field in the look-up table. The value of that result field
        /// is given to this field.
        /// <para>Valid when this field is the result of a look-up operation.</para>
        /// </summary>
        /// <remarks>
        /// <para></para>
        /// <example> Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
        /// and a COUNTRY table with ID and NAME fields. To establish a look up
        /// <code>
        ///     LookUpTableName = "COUNTRY";                           
        ///     LookUpKeyFields = "COUNTRY_ID";  // CUSTOMER.COUNTRY_ID
        ///     LookUpResultField = "ID";        // COUNTRY.ID         
        ///     LookUpDisplayFields = "NAME";    // COUNTRY.NAME 
        ///  </code>
        /// </example>
        /// </remarks>
        public string LookUpResultField
        {
            get { return string.IsNullOrEmpty(lookUpResultField) ? "Id" : lookUpResultField; }
            set { lookUpResultField = value; }
        }
        /// <summary>
        /// Gets or sets the names of the fields of the look-up table that should by displayed in a control.
        /// <para>Valid when this field is the result of a look-up operation.</para>
        /// <para>It is a semi-colon separated list.</para>
        /// </summary>
        /// <remarks>
        /// <para></para>
        /// <example> Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
        /// and a COUNTRY table with ID and NAME fields. To establish a look up
        /// <code>
        ///     LookUpTableName = "COUNTRY";                           
        ///     LookUpKeyFields = "COUNTRY_ID";  // CUSTOMER.COUNTRY_ID
        ///     LookUpResultField = "ID";        // COUNTRY.ID         
        ///     LookUpDisplayFields = "NAME";    // COUNTRY.NAME    
        /// </code>
        /// </example>
        /// </remarks>
        public string LookUpDisplayFields
        {
            get { return string.IsNullOrEmpty(lookUpDisplayFields) ? "Name" : lookUpDisplayFields; }
            set { lookUpDisplayFields = value; }
        }
    }
}
