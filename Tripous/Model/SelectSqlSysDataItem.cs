/*--------------------------------------------------------------------------------------        
                            Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Tripous.Data;


namespace Tripous.Model
{

    /// <summary>
    /// A SysDataItem for the SelectSql
    /// </summary>
    public class SelectSqlSysDataItem : SysDataItem
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string DataTypePrefix = "SelectSql.";
        /// <summary>
        /// Constant
        /// </summary>
        public const string DataTypeFormat = "SelectSql.{0}.{1}";       // i.e. SelectSql.Broker.BrokerName
 

        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();
            Descriptor.Clear();
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectSqlSysDataItem()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectSqlSysDataItem(string DataType, SelectSql Source)
        {
            LoadFrom(DataType, Source);
        }

        /* static */
        /// <summary>
        /// Constructs and returns a DataType
        /// </summary>
        static public string FormatDataType(string OwnerType, string OwnerName)
        {
            return string.Format(DataTypeFormat, OwnerType, OwnerName);
        }
        /// <summary>
        /// Returns an array of SelectSql  items, configured by the user at runtime
        /// </summary>
        static public List<SelectSql> GetDesignedSelectSqlList(string DataType)
        {
            List<SelectSql> List = new List<SelectSql>();

            // system tables may not present, because not all applications need them
            // so guard the statement and swallow any exception
            try
            {
                DataTable Table = SysData.Select(DataType, false); //DataTable Table = Censor.Apply(DataType, false);

                SelectSqlSysDataItem SDI = new SelectSqlSysDataItem();
                SelectSql SS;
                foreach (DataRow Row in Table.Rows)
                {
                    SDI.LoadFromRow(Row);
                    SS = SDI.Descriptor.Clone() as SelectSql;
                    List.Add(SS);
                }
            }
            catch  
            { 
            }

            return List;
        }


        /// <summary>
        /// Loads this instance from the specified arguments
        /// </summary>
        public void LoadFrom(string DataType, SelectSql Source)
        {
            this.DataType = DataType;
            this.DataName = Source.Name;
            this.Title = Source.Title;
            this.StoreName = Source.ConnectionName;

            Descriptor = Source.Clone() as SelectSql;
        }
        /// <summary>
        /// Updates the critical properties from its Descriptor
        /// </summary>
        public void UpdateFromDescriptor()
        {
            this.DataName = Descriptor.Name;
            this.Title = Descriptor.Title;
            this.StoreName = Descriptor.ConnectionName;
        }

        /* properties */
        /// <summary>
        /// Gets the broker descriptor
        /// </summary>
        public SelectSql Descriptor { get; private set; }
    }
}
