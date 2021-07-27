/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Data;


namespace Tripous.Model
{

    /// <summary>
    /// A list of LocatorDescriptor items.
    /// </summary>
    public class LocatorDescriptors : ModelItems<LocatorDescriptor> // NamedItems
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorDescriptors()
        {
        }


        /// <summary>
        /// Adds a locator descriptor to the list.
        /// </summary>
        public LocatorDescriptor Add(string Name, string ListTableName, string ListKeyField, string TitleKey, string ZoomCommand)
        {
            LocatorDescriptor Result = Add(Name);
            Result.ListTableName = ListTableName;
            Result.ListKeyField = ListKeyField;
            Result.TitleKey = TitleKey;
            Result.ZoomCommand = ZoomCommand;
            return Result;
        }
        /// <summary>
        /// Adds a locator descriptor to the list.
        /// </summary>
        public LocatorDescriptor Add(string Name, string ListTableName, string ListKeyField, string DialogTitleKey)
        {
            return Add(Name, ListTableName, ListKeyField, DialogTitleKey, string.Empty);
        }
        /// <summary>
        /// Adds a locator descriptor to the list.
        /// </summary>
        public LocatorDescriptor Add(string Name, string ListTableName, string ListKeyField)
        {
            return Add(Name, ListTableName, ListKeyField, ListTableName);
        }
    }
}
