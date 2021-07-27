/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;



namespace Tripous.Model
{

    /// <summary>
    /// Describes a join table. A table that participates in a left join.
    /// <example>
    /// <code>
    ///     TableName              | COUNTRY (this table)
    ///     Alias                  |
    ///     PrimaryKeyField        | ID
    ///     MasterKeyField         | COUNTRY_ID (in the left table, i.e. CUSTOMER.COUNTRY_ID)
    /// </code>    
    /// </example>
    /// </summary>
    public class JoinTableDescriptor : TableDescriptorBase
    {
        private string zoomCommand = "";
 

        /*
        private JoinTableDescriptor CollectionOwner
        {
            get
            {
                if ((Collection is NamedItems<NamedItem>) && ((Collection as NamedItems<NamedItem>).Owner is JoinTableDescriptor))
                    return (Collection as NamedItems<NamedItem>).Owner as JoinTableDescriptor;

                return null;
            }
        }
        */

        /// <summary>
        /// Constructor.
        /// </summary>
        public JoinTableDescriptor()
        {
            Fields = new JoinFieldDescriptors();
            Fields.Owner = this;
        }

        /* properties */
        /// <summary>
        /// Gets or sets the zoom  command path.
        /// <para>A zoom command is used by locators and other drill-down controls.</para>
        /// <para>It is something similar to PROCESSOR.COMMAND. For example MAIN_PROCESSOR.CUSTOMER</para>
        /// </summary>
        public string ZoomCommand
        {
            get { return string.IsNullOrEmpty(zoomCommand) ? SysConfig.DefaultConnection + "." + Name : zoomCommand; }
            set { zoomCommand = value; }
        }
        /// <summary>
        /// Returns an alias for the MasterKeyField.
        /// <para>We need this in order to get the right FieldName as the DataField of a Locator, in a sub-join case,
        /// because the table name is included in produced field names (aliases) of the join tables (except ta main one).
        /// </para>
        /// <example> For example, in the next join the field CUSTOMER_ADDRESS.CITY_ID would give a CUSTOMER_ADDRESS__CITY_ID alias.
        /// <code>        
        /// --------------------------------------------------------------------------------------------------------
        /// TableName            | Field Names                     | MasterKeyField       | MasterKeyFieldAlias
        /// --------------------------------------------------------------------------------------------------------
        /// CUSTOMER             | ID, CUSTOMER_ADDRESS_ID ... etc |                      |
        ///   CUSTOMER_ADDRESS   | ID, CITY_ID             ... etc | CUSTOMER_ADDRESS_ID  | CUSTOMER_ADDRESS_ID
        ///     CITY             | ID, CODE, NAME                  | CITY_ID              | CUSTOMER_ADDRESS__CITY_ID
        /// 
        ///  so  >>>  Locator.DataField = JoinTableDes.MasterKeyFieldAlias
        /// </code>
        /// </example>
        /// </summary>
        public string MasterKeyFieldAlias
        {
            get
            {
                string Result = MasterKeyField;

                if (CollectionOwner is JoinTableDescriptor)
                    Result = Sys.FieldAlias((CollectionOwner as JoinTableDescriptor).Alias, Result);
                return Result;
            }
        }
        /// <summary>
        /// Gets the Fields of this table.
        /// </summary>
        public JoinFieldDescriptors Fields { get; private set; }
    }
}
