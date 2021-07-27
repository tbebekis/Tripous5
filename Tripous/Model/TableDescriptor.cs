/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;


namespace Tripous.Model
{
    /// <summary>
    /// Describes a table
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    ///   ----------------------------------------------------------------------------
    ///   TableName              : 
    ///   Alias                  : 
    ///   PrimaryKeyField        :
    ///   DetailKeyField         :  when is detail, the field to match to MasterKeyField in master-detail relationships
    ///   MasterTableName        :  It is the Name property, NOT the TableName. Required when this is a detail table
    ///   MasterKeyField         :
    ///   DisplayLabel           :
    ///   Flags                  :  operations are done automatically (they don't demand an explicit method call)
    ///   Fields                 :
    ///   ----------------------------------------------------------------------------
    ///                          |  MASTER           |      DETAIL
    ///   ----------------------------------------------------------------------------
    ///   TableName              |  CUSTOMERS               ORDERS
    ///   PrimaryKeyField        |  ID                      ID
    ///   DetailKeyField         |                          CUST_ID
    ///   MasterTableName        |                          CUSTOMERS
    ///   MasterKeyField         |                          ID
    ///   ----------------------------------------------------------------------------
    ///   TableName              |  ORDERS                  ORDER_LINES
    ///   PrimaryKeyField        |  ID                      ID
    ///   DetailKeyField         |                          ORDER_ID
    ///   MasterTableName        |                          ORDERS
    ///   MasterKeyField         |                          ID
    ///   ---------------------------------------------------------------------------
    /// </code>
    /// </example>
    /// </remarks>
    public class TableDescriptor : TableDescriptorBase
    {
        private string detailKeyField;
        private string masterTableName;
        //private string columnNames;
        private FieldDescriptors fields = new FieldDescriptors();
        private StockTableDescriptors stockTables = new StockTableDescriptors();

        /// <summary>
        /// Finds a field by Name or Alias by searching the whole tree of JoinTables tables.
        /// Returns null if a field not found.
        /// </summary>
        private FieldDescriptorBase FindAnyField(string NameOrAlias, JoinTableDescriptors JoinTables)
        {
            FieldDescriptorBase Result = null;

            foreach (JoinTableDescriptor JoinTable in JoinTables)
            {
                Result = JoinTable.Fields.Find(NameOrAlias);
                if (Result != null)
                    return Result;

                Result = JoinTable.Fields.FindByAlias(NameOrAlias);
                if (Result != null)
                    return Result;

                Result = FindAnyField(NameOrAlias, JoinTable.JoinTables);
            }

            return Result;
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public TableDescriptor()
        {
            fields.Owner = this;
            stockTables.Owner = this;
        }

        /* public */
        /// <summary>
        /// Returns an array of all fields of this table and its joined tables
        /// </summary>
        public List<FieldDescriptorBase> GetFlatFieldList()
        {
            List<FieldDescriptorBase> List = new List<FieldDescriptorBase>();

            Action<JoinTableDescriptor> AddFields = null;

            AddFields = delegate (JoinTableDescriptor Table)
            {
                foreach (FieldDescriptorBase Field in Table.Fields)
                    List.Add(Field);

                foreach (JoinTableDescriptor JoinTable in Table.JoinTables)
                    AddFields(JoinTable);
            };


            foreach (FieldDescriptorBase Field in this.Fields)
                List.Add(Field);

            foreach (JoinTableDescriptor JoinTable in this.JoinTables)
                AddFields(JoinTable);

            return List;
        }
        /// <summary>
        /// Returns a list of fields that are localizable
        /// </summary>
        public List<FieldDescriptorBase> GetLocalizableFields()
        {
            List<FieldDescriptorBase> FlatFieldList = GetFlatFieldList();
            List<FieldDescriptorBase> List = new List<FieldDescriptorBase>();

            foreach (FieldDescriptorBase Field in FlatFieldList)
            {
                if (Field.IsLocalizable)
                    List.Add(Field);
            }

            return List;
        }


        /// <summary>
        /// Searces the whole JoinTables joined tree for a table by a Name or Alias and returns
        /// a JoinTableDescriptor, if any, else null.
        /// </summary>
        public JoinTableDescriptor FindAnyJoinTable(string Name)
        {
            return JoinTables.FindAny(Name);
        }
        /// <summary>
        /// Finds and returns, if exists, a TFieldDescriptorBase that has NameOrAlias
        /// Name or Alias. It searches this TableDes and its joined tables in the full tree.
        /// </summary>
        public FieldDescriptorBase FindAnyField(string NameOrAlias)
        {
            FieldDescriptorBase Result = Fields.Find(NameOrAlias);
            if (Result != null)
                return Result;

            Result = Fields.FindByAlias(NameOrAlias);
            if (Result != null)
                return Result;

            return FindAnyField(NameOrAlias, this.JoinTables);

        }
        /// <summary>
        /// Finds a field title by searching the whole tree of fields.
        /// </summary>
        public string FindAnyFieldTitle(string NameOrAlias)
        {
            FieldDescriptorBase Field = FindAnyField(NameOrAlias);
            return (Field == null) ? NameOrAlias : Field.Title;
        }


        /* properties */
        /// <summary>
        /// Gets or sets the detail key field. A field that belongs to this table and mathes the <see cref="MasterTableName"/> primary key field.
        /// <para>It is used when this table is a detail table in a master-detail relation.</para>
        /// </summary>
        public string DetailKeyField
        {
            get { return string.IsNullOrEmpty(detailKeyField) ? string.Empty : detailKeyField; }
            set { detailKeyField = value; }
        }
        /// <summary>
        /// Gets or sets the name of the master table.
        /// <para>It is used when this table is a detail table in a master-detail relation.</para>
        /// </summary>
        public string MasterTableName
        {
            get { return string.IsNullOrEmpty(masterTableName) ? string.Empty : masterTableName; }
            set { masterTableName = value; }
        }
 
        /// <summary>
        /// Gets the fields of this table
        /// </summary>
        public FieldDescriptors Fields { get { return fields; } }
        /// <summary>
        /// The main table of a Broker (Item) is selected as 
        /// <para>  <c>select * from TABLE_NAME where ID = :ID</c></para>
        /// <para>
        /// If the table contains foreign keys, for instance CUSTOMER_ID etc,
        /// then those foreign tables are NOT joined. The programmer who
        /// designs the form just creates a Locator where needed.
        /// </para>
        /// <para>
        /// But there is always the need to have data from those
        /// foreign tables in many situations, ie reports.
        /// </para>
        /// <para>
        /// StockTables are used for that. They are selected each time
        /// after the select of the main broker table (Item)          
        /// </para>
        /// </summary>
        public StockTableDescriptors StockTables { get { return stockTables; } }
    }
}
