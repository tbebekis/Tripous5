/*--------------------------------------------------------------------------------------        
                            Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using Tripous.Data;


namespace Tripous.Model
{

    /// <summary>
    /// A base class used to inherit table Descriptor classes.
    /// </summary>
    public class TableDescriptorBase : Descriptor
    {
        private string primaryKeyField;
        private string masterKeyField;
        private JoinTableDescriptors joinTables = new JoinTableDescriptors();

        /// <summary>
        /// Helper method. Finds a <see cref="JoinTableDescriptor"/> by its Alias in the JoinTables.
        /// <para>It searches the whole tree of the join tables.</para>
        /// </summary>
        protected JoinTableDescriptor FindJoinTable(string JoinAlias, JoinTableDescriptors JoinTables)
        {
            JoinTableDescriptor Result = null;

            foreach (JoinTableDescriptor JoinTable in JoinTables)
            {
                if (string.Compare(JoinAlias, JoinTable.Alias, true) == 0)
                    Result = JoinTable;
                else
                    Result = FindJoinTable(JoinAlias, JoinTable.JoinTables);

                if (Result != null)
                    return Result;

            }

            return null;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public TableDescriptorBase()
        {
            joinTables.Owner = this;
        }



        /// <summary>
        /// Finds a <see cref="JoinTableDescriptor"/> by its Alias in the JoinTables.
        /// <para>It searches the whole tree of the join tables.</para>
        /// </summary>
        public JoinTableDescriptor FindJoinTable(string JoinAlias)
        {
            return FindJoinTable(JoinAlias, this.JoinTables);
        }

        /// <summary>
        /// Gets or sets the name of the primary key field of this table.
        /// </summary>
        public string PrimaryKeyField
        {
            get { return string.IsNullOrEmpty(primaryKeyField) ? "Id" : primaryKeyField; }
            set { primaryKeyField = value; }
        }
        /// <summary>
        /// Gets or sets the field name of a field belonging to a master table.
        /// <para>Used when this table is a detail table in a master-detail relation.</para>
        /// </summary>
        public string MasterKeyField
        {
            get { return string.IsNullOrEmpty(masterKeyField) ? "Id" : masterKeyField; }
            set { masterKeyField = value; }
        }
        /// <summary>
        /// Returns the list of join tables.
        /// </summary>
        public JoinTableDescriptors JoinTables { get { return joinTables; } }

        /// <summary>
        /// Returns this.Owner as BrokerDescriptor
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public BrokerDescriptor BrokerDescriptor
        {
            get
            {
                if (this.CollectionOwner is BrokerDescriptor)
                    return this.CollectionOwner as BrokerDescriptor;

                if (this.CollectionOwner is TableDescriptorBase)
                    return (this.CollectionOwner as TableDescriptorBase).BrokerDescriptor;

                return null;
            }
        }
        /// <summary>
        /// Returns the database connection info this table belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public SqlConnectionInfo ConnectionInfo
        {
            get
            {
                if (BrokerDescriptor != null)
                    return BrokerDescriptor.ConnectionInfo;
                return null;
            }
        }
    }
}
