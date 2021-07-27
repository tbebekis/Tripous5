/*--------------------------------------------------------------------------------------        
                            Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Tripous.Model
{

    /// <summary>
    /// A list of SqlBrowserDescriptor items
    /// </summary>
    public class SqlBrowserDescriptors : ModelItems<SqlBrowserDescriptor>  
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlBrowserDescriptors()
        {
            UseSafeAdd = true;
        }

        /* public */
        /// <summary>
        /// Removes any descriptor marked as custom or replica
        /// </summary>
        public void RemoveCustom()
        {
            List<SqlBrowserDescriptor> List = new List<SqlBrowserDescriptor>();
            foreach (var Item in this)
            {
                if (Item.IsCustom || Item.IsReplica)
                    List.Add(Item);
            }

            foreach (var Item in List)
                this.Remove(Item);
        }


        /// <summary>
        /// Adds a browser descriptor to the list.
        /// </summary>
        public SqlBrowserDescriptor Add(string Name, string MainTableName, string TitleKey, string TypeClassName, string BrokerName)
        {
            SqlBrowserDescriptor Result = base.Add(Name);

            Result.UiMode = UiMode.Desktop;

            Result.ConnectionName = SysConfig.DefaultConnection;
            Result.MainTableName = MainTableName;
            Result.TitleKey = TitleKey;
            Result.TypeClassName = TypeClassName;
            Result.BrokerName = BrokerName;

            return Result;
        }
 



        /// <summary>
        /// Adds a browser descriptor to the list.
        /// </summary>
        public SqlBrowserDescriptor AddWeb(string Name, string TitleKey, string TypeClassName, string BrokerName)
        {
            SqlBrowserDescriptor Result = base.Add(Name);

            Result.UiMode = UiMode.Web;
            Result.TitleKey = TitleKey;
            Result.TypeClassName = TypeClassName;
            Result.BrokerName = BrokerName; 

            return Result;
        }


    }
}
