/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using Tripous.Data;


namespace Tripous.Model
{


    /// <summary>
    /// A SysDataItem for the BrokerDescriptor
    /// </summary>
    public class BrokerSysDataItem : SysDataItem
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string SSysDataType = "Broker.SqlBroker";

    

        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();
            Descriptor.Clear();
        }
        /// <summary>
        /// Returns the data type 
        /// </summary>
        protected override string GetDataType()
        {
            return SSysDataType;
        }
        /// <summary>
        /// Sets the data type 
        /// </summary>
        protected override void SetDataType(string Value)
        {
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerSysDataItem()
        {
            Descriptor = new BrokerDescriptor();
            Descriptor.DesignMode = true;
        }

        /* public */
        /// <summary>
        /// Loads this SysDataItem instance from the specified descriptor
        /// </summary>
        public void LoadFromDescriptor(BrokerDescriptor Source)
        {
            if (Source != null)
            {
                this.DataName = Source.Name;
                this.Title = Source.Title;
                this.StoreName = Source.ConnectionName;

                this.Descriptor.Assign(Source);
            }
        }

        /* static */
        /// <summary>
        /// Loads and returns a BrokerDescriptor from the SysData table, if DataName
        /// exists in that table, else null.
        /// </summary>
        static public BrokerDescriptor FindDescriptor(string DataName)
        {
            BrokerDescriptor Result = null;

            if (SysData.Exists(SSysDataType, DataName))
            {
                BrokerSysDataItem sysDataItem = new BrokerSysDataItem();
                sysDataItem.DataName = DataName;
                sysDataItem.Load();

                Result = sysDataItem.Descriptor.Clone() as BrokerDescriptor;
            }


            return Result;
        }

        /* properties */
        /// <summary>
        /// Gets the broker descriptor
        /// </summary>
        public BrokerDescriptor Descriptor { get; private set; } 
    }


}