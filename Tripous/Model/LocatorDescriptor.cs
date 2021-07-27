/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel;
using System.Collections; 
using System.Drawing;
using System.ComponentModel.Design;

using Tripous.Data;

namespace Tripous.Model
{


    /// <summary>
    /// Describes a Locator.
    /// <para>A locator represents (returns) a single value, but it can handle and display multiple values
    /// in order to help the end user in identifying and locating that single value.</para>
    /// <para>For example, a TRADES data table has a CUSTOMER_ID column, representing that single value, but the user interface
    /// has to display information from the CUSTOMERS table, specifically, the ID, CODE and NAME columns.</para>
    /// <para>The TRADES table is the target data table and the CUSTOMER_ID is the DataField field name.</para>
    /// <para>The CUSTOMERS table is the <see cref="ListTableName"/> and the ID is the <see cref="ListKeyField"/> field name.</para>
    /// <para>The fields, ID, CODE and NAME, may be described by individual <see cref="LocatorFieldDescriptor"/> field items.</para>
    /// <para>A locator can be used either as a single-row control, as the LocatorBox does, or as a group of
    /// related columns in a Grid.</para>
    /// <para>NOTE: A locator of a LocatorBox type, may or may not define the <see cref="LocatorFieldDescriptor.DataField"/> 
    /// field names. Usually in a case like that, the data table contains just the key field, the LocatorBox.DataField.  </para>
    /// <para>A locator of a grid-type must define the names of those fields always and the data table must contain DataColumn columns
    /// on those fields.</para>
    /// </summary>
    public class LocatorDescriptor : Descriptor 
    {
        string fConnectionName = SysConfig.DefaultConnection;
        string fListTableName;

        string fListKeyField;
        string fZoomCommand;
        bool fReadOnly;
        string fOrderBy;

        SelectSql selectSql = new SelectSql();
        LocatorFieldDescriptors fields = new LocatorFieldDescriptors();
  
        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override string GetName()
        {
            return !string.IsNullOrWhiteSpace(base.GetName()) ? base.GetName() : ListTableName;  
        }
 

        /* constructor */
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocatorDescriptor()
        {
            fields.Owner = this;
        }
        
        /* public */
        /// <summary>
        /// Override
        /// </summary>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name)? Name: base.ToString();
        }


        /* properties */
        /// <summary>
        /// Gets or sets the name of the database connection 
        /// </summary>
        [Localizable(false), Description("Gets or sets the name of the database connection")]
        public string ConnectionName
        {
            get { return string.IsNullOrEmpty(fConnectionName) ? SysConfig.DefaultConnection : fConnectionName; }
            set 
            { 
                fConnectionName = value;
                OnPropertyChanged("ConnectionName");
            }
        }
        /// <summary>
        /// Gets or sets the name of the list table
        /// </summary>       
        [DefaultValue(""), Localizable(false), Description("Gets or sets the name of the list table")]
        public string ListTableName
        {
            get { return string.IsNullOrEmpty(fListTableName) ? string.Empty : fListTableName; }
            set 
            { 
                fListTableName = value;
                OnPropertyChanged("ListTableName");
            }
        }
        /// <summary>
        /// Gets or sets the key field of the list table. The value of this field goes to the DataField
        /// </summary>      
        [DefaultValue("Id"), Localizable(false), Description("Gets or sets the key field of the list table. The value of this field goes to the DataField.")]
        public string ListKeyField
        {
            get { return string.IsNullOrEmpty(fListKeyField) ? "Id" : fListKeyField; }
            set 
            { 
                fListKeyField = value;
                OnPropertyChanged("ListKeyField");
            }
        }
        /// <summary>
        /// Gets or sets the zoom command name. A user inteface (form) can use this name to call the command.
        /// </summary>
        [DefaultValue(""), Localizable(false), Description("Gets or sets the zoom command name. A user inteface (form) can use this name to call the command.")]
        public string ZoomCommand
        {
            get { return string.IsNullOrEmpty(fZoomCommand) ? (string.IsNullOrEmpty(ListTableName) ? string.Empty : SysConfig.DefaultConnection + "." + ListTableName) : fZoomCommand; }
            set 
            { 
                fZoomCommand = value;
                OnPropertyChanged("ZoomCommand");
            }
        }
        /// <summary>
        /// Indicates whether the locator is readonly
        /// </summary>
        [DefaultValue(false), Description("Indicates whether the locator is readonly")]
        public bool ReadOnly
        {
            get { return fReadOnly; }
            set
            {
                fReadOnly = value;
                OnPropertyChanged("ZoomCommand");
            }
        }
        /// <summary>
        /// If the value of this property is set then the locator does not generates the SELECT automatically.
        /// </summary>     
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("If the value of this property is set then the locator does not generates the SELECT automatically.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SelectSql SelectSql { get { return selectSql; } }
        /// <summary>
        /// Gets the list of descriptor fields.
        /// </summary>    
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets the list of descriptor fields.")]
        public LocatorFieldDescriptors Fields { get { return fields; } }
        /// <summary>
        /// The order by field when the SELECT Sql is constructed by the Locator.
        /// <para>In a description with Id and Name fields could be the ListTableName.Name</para>
        /// </summary>
        [DefaultValue(""), Localizable(false), Description("The order by field when the SELECT Sql is constructed by the Locator.")]
        public string OrderBy
        {
            get { return string.IsNullOrEmpty(fOrderBy) ? string.Empty : fOrderBy; }
            set
            {
                fOrderBy = value;
                OnPropertyChanged("OrderBy");
            }
        }


    }




}
