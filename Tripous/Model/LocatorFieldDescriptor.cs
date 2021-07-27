/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel;
using System.Data;

namespace Tripous.Model
{
    /// <summary>
    /// Describes the "field" (text box or grid column) of a Locator.
    /// <para>A field such that associates a column in the data table (the target) to
    /// a column in the list table (the source).</para>
    /// </summary>
    [DesignTimeVisible(false)]
    public class LocatorFieldDescriptor : Assignable
    {
        /// <summary>
        /// field
        /// </summary>
        public const int BoxDefaultWidth = 70;
 
 
        private string listFieldAlias;
        private string listTableName;
        private string titleKey;


        /* private */
        private string CollectionListTableName
        {
            get
            {
                if ((Collection is LocatorFieldDescriptors) && (((Collection as LocatorFieldDescriptors).Owner is LocatorDescriptor)))
                    return ((Collection as LocatorFieldDescriptors).Owner as LocatorDescriptor).ListTableName;

                return string.Empty;
            }

        }

        /* constructor */
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocatorFieldDescriptor()
        {
            Searchable = true;
            ListVisible = true;
            DataVisible = true;
            DataType = SimpleType.String;
            Width = BoxDefaultWidth;
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return string.IsNullOrEmpty(ListTableName) ? ListFieldAlias : ListTableName + "." + ListFieldAlias;
        }

        /* properties */
        /// <summary>
        /// Gets or sets the data type of the field.
        /// </summary>    
        [DefaultValue(SimpleType.String), Description("Gets or sets the data type of the field.")]
        public SimpleType DataType { get; set; } 
        /// <summary>
        /// Gets or sets the <see cref="DataColumn.ColumnName"/> of the data table. It can not be empty for grid-type locators.
        /// </summary>
        [DefaultValue(""), Localizable(false), Description("Gets or sets the DataColumn.ColumnName of the data table. It can not be empty for grid-type locators.")]
        public string DataField { get; set; }
        /*  
                /// <summary>
                /// Gets or sets the target field
                /// </summary>
                [DefaultValue(""), Localizable(false), Description("Gets or sets the target field")]
                public string TargetField { get; set; }  
         */
        /// <summary>
        /// Gets or sets the field name of the list table in the underlying database table. 
        /// </summary>
        [DefaultValue(""), Localizable(false), Description("Gets or sets the field name of the list table  in the underlying database table..")]
        public string ListField { get; set; } 
        /// <summary>
        /// Gets or sets the <see cref="DataColumn.ColumnName"/> of the list table.
        /// </summary>
        [DefaultValue(""), Localizable(false), Description("Gets or sets the DataColumn.ColumnName of the list table.")]
        public string ListFieldAlias
        {
            get { return string.IsNullOrEmpty(listFieldAlias) ? ListField : listFieldAlias; }
            set { listFieldAlias = value; }
        }
        /// <summary>
        /// Gets or sets the name of the list table.
        /// </summary>
        [DefaultValue(""), Localizable(false), Description("Gets or sets the name of the list table.")]
        public string ListTableName
        {
            get { return string.IsNullOrEmpty(listTableName) ? (string.IsNullOrEmpty(CollectionListTableName) ? string.Empty : CollectionListTableName) : listTableName; }
            set { listTableName = value; }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        [DefaultValue(""), Description("Gets or sets a resource Key used in returning a localized version of Title.")]
        public string TitleKey
        {
            get { return string.IsNullOrWhiteSpace(titleKey) ? ListFieldAlias : titleKey; }
            set { titleKey = value; }
        }
        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        public string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TitleKey))
                    return Res.GS(TitleKey, TitleKey);

                return TitleKey;
            }
            //set { title = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the TextBox of this field is visible in a LocatorBox.
        /// </summary>
        [DefaultValue(true), Description("Gets or sets a value indicating whether the TextBox of this field is visible in a LocatorBox")]
        public bool DataVisible { get; set; } 
        /// <summary>
        /// Gets or sets a value indicating whether the column is visible when the list table is displayed.
        /// </summary>
        [DefaultValue(true), Description("Gets or sets a value indicating whether the column is visible when the list table is displayed.")]
        public bool ListVisible { get; set; } 
        /// <summary>
        /// When true the field can be part in a where clause in a select statement.
        /// </summary>
        [DefaultValue(true), Description("When true the field can be part in a where clause in a select statement.")]
        public bool Searchable { get; set; } 
        /// <summary>
        /// Used to notify criterial links to treat the field as an integer boolea field (1 = true, 0 = false)
        /// </summary>
        [DefaultValue(false), Description("Used to notify criterial links to treat the field as an integer boolea field (1 = true, 0 = false)")]
        public bool IsIntegerBoolean { get; set; } 
        /// <summary>
        /// Controls the width of the text box in a LocatorBox.
        /// </summary>
        [DefaultValue(BoxDefaultWidth), Description("Controls the width of the text box in a LocatorBox.")]
        public int Width { get; set; } 
 
        


    }
}
