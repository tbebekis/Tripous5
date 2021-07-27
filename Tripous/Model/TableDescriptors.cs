/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using Tripous.Data;

namespace Tripous.Model
{
    /// <summary>
    /// A list of TableDescriptor items.
    /// </summary>
    public class TableDescriptors : ModelItems<TableDescriptor> // NamedItems
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string ITEM = "_ITEM_";
        /// <summary>
        /// Constant
        /// </summary>
        public const string LINES = "_LINES_";
        /// <summary>
        /// Constant
        /// </summary>
        public const string SUBLINES = "_SUBLINES_";


        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public TableDescriptors()
        {
            UseSafeAdd = true;
        }

        /* methods */
        /// <summary>
        /// Adds a table to the list
        /// </summary>
        public TableDescriptor Add(string Name, string TitleKey)
        {
            TableDescriptor Result = base.Add(Name);
            Result.TitleKey = TitleKey;
            return Result;
        }
        /// <summary>
        /// Finds a table by its Alias, if any, else null.
        /// </summary>
        public TableDescriptor FindByAlias(string Alias)
        {
            return Descriptor.FindByAlias(Alias, this) as TableDescriptor;
        }
        /// <summary>
        /// Finds and returns a table descriptor by Name, if any, else null.
        /// </summary>
        public override TableDescriptor Find(string Name)
        {
            TableDescriptor Result = base.Find(Name);

            if ((Result == null) && (this.Owner is BrokerDescriptor))
            {
                BrokerDescriptor BrokerDes = this.Owner as BrokerDescriptor;
                if (string.IsNullOrEmpty(Name) || Sys.IsSameText(Name, "Item") || Sys.IsSameText(Name, ITEM))
                    return this[BrokerDes.MainTableName];
                else if (Sys.IsSameText(Name, "Lines") || Sys.IsSameText(Name, LINES))
                    return this[BrokerDes.LinesTableName];
                else if (Sys.IsSameText(Name, "SubLines") || Sys.IsSameText(Name, SUBLINES))
                    return this[BrokerDes.SubLinesTableName];
            }

            return Result;
        }
        /// <summary>
        /// Displays an edit dialog for this instance. 
        /// <para>Returns true if the user presses the OK button in the dialog</para>
        /// </summary>
        public bool ShowEditDialog()
        {
            TableDescriptors Instance = this.Clone() as TableDescriptors;

            if ((bool)ObjectStore.CallDef("TableDescriptors.Edit.Dialog", false, Instance))
            {
                this.Assign(Instance);
                return true;
            }

            return false;
        }

        /* properties */
        /// <summary>
        /// Returns this.Owner as BrokerDescriptor
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public BrokerDescriptor BrokerDescriptor { get { return this.Owner as BrokerDescriptor; } }
        /// <summary>
        /// Returns the database connection info  
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
