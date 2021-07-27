/*--------------------------------------------------------------------------------------        
                            Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;




namespace Tripous.Model
{
    /// <summary>
    /// A list of <see cref="BrokerDescriptor"/> items.
    /// </summary>
    public class BrokerDescriptors : ModelItems<BrokerDescriptor>
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string SSysDataType = "Brokers";

        /// <summary>
        /// Constructor.
        /// </summary>
        public BrokerDescriptors()
        {
            UseSafeAdd = true;
        }

        /// <summary>
        /// Removes any descriptor marked as custom or replica
        /// </summary>
        public void RemoveCustom()
        {

            Lock();
            try
            {
                List<BrokerDescriptor> List = new List<BrokerDescriptor>();
                foreach (var Item in this)
                {
                    if (Item.IsCustom || Item.IsReplica)
                        List.Add(Item);
                }

                foreach (var Item in List)
                    this.Remove(Item);
            }
            finally
            {
                UnLock();
            }

        }

        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        public BrokerDescriptor Add(string ConnectionName, string Name, string MainTableName, string TitleKey, string BrokerClassName, string CodeProducerName)
        {
            BrokerDescriptor Result = base.Add(Name);

            Result.ConnectionName = ConnectionName;
            Result.MainTableName = MainTableName;
            Result.TitleKey = TitleKey;
            Result.TypeClassName = BrokerClassName;
            Result.CodeProducerName = CodeProducerName;

            return Result;
        }
        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        public BrokerDescriptor Add(string Name, string MainTableName, string BrokerClassName, string CodeProducerName)
        {
            return Add(SysConfig.DefaultConnection, Name, MainTableName, Name, BrokerClassName, CodeProducerName);
        }
        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        public BrokerDescriptor Add(string Name, string MainTableName, string BrokerClassName)
        {
            return Add(SysConfig.DefaultConnection, Name, MainTableName, Name, BrokerClassName, "");
        }
        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        public BrokerDescriptor Add(string Name, string BrokerClassName)
        {
            return Add(SysConfig.DefaultConnection, Name, Name, Name, BrokerClassName, "");
        }
        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        public override BrokerDescriptor Add(string Name)
        {
            return Add(SysConfig.DefaultConnection, Name, Name, Name, "Tripous.Model.SqlBroker", "");
        }
        /// <summary>
        /// Adds a SqlBrokerList along with its Fields.
        /// <para>NOTE: The type of the Id Field is SysConfig.OidDataType</para>
        /// </summary>
        public BrokerDescriptor AddLookUp(string Name, string NameTitleKey = "")
        {
            BrokerDescriptor Result = Add(SysConfig.DefaultConnection, Name, Name, Name, "Tripous.Model.SqlBrokerList", string.Empty);

            if (string.IsNullOrWhiteSpace(NameTitleKey))
                NameTitleKey = "Name";

            TableDescriptor Table = Result.Tables.Add(Result.MainTableName, Result.Title);
            Table.Fields.Add("Id", SysConfig.OidDataType, SysConfig.OidSize, "Id", FieldFlags.None);
            Table.Fields.Add("Name", SimpleType.String, 96, NameTitleKey, FieldFlags.Visible | FieldFlags.Searchable | FieldFlags.Localizable | FieldFlags.Required);

            return Result;
        }
        /// <summary>
        /// Adds a SqlBrokerList along with its Fields.
        /// <para>NOTE: The type of the Id Field is SimpleType.Integer</para>
        /// </summary>
        public BrokerDescriptor AddEnum(string Name, string NameTitleKey = "")
        {
            BrokerDescriptor Result = Add(SysConfig.DefaultConnection, Name, Name, Name, "Tripous.Model.SqlBrokerList", string.Empty);
            Result.GuidOids = false;

            if (string.IsNullOrWhiteSpace(NameTitleKey))
                NameTitleKey = "Name";

            TableDescriptor Table = Result.Tables.Add(Result.MainTableName, Result.Title);
            Table.Fields.Add("Id", SimpleType.Integer, 0, "Id", FieldFlags.None);
            Table.Fields.Add("Name", SimpleType.String, 96, NameTitleKey, FieldFlags.Visible | FieldFlags.Searchable | FieldFlags.Localizable | FieldFlags.Required);

            return Result;
        }

    }

}
