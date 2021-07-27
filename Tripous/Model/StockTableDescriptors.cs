/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;




namespace Tripous.Model
{
    /// <summary>
    /// A list of <see cref="StockTableDescriptor"/> items.
    /// </summary>
    public class StockTableDescriptors : ModelItems<StockTableDescriptor> // NamedItems
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public StockTableDescriptors()
        {
            UseSafeAdd = true;
        }


        /// <summary>
        /// Displays an edit dialog for this instance. 
        /// <para>Returns true if the user presses the OK button in the dialog</para>
        /// </summary>
        public bool ShowEditDialog()
        {
            StockTableDescriptors Instance = this.Clone() as StockTableDescriptors;

            if ((bool)ObjectStore.CallDef("StockTableDescriptors.Edit.Dialog", false, Instance))
            {
                this.Assign(Instance);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a stock table Descriptor
        /// </summary>
        public override StockTableDescriptor Add(string Name)
        {
            return Add(Name, "select * from " + Name, "");
        }
        /// <summary>
        /// Adds a stock table Descriptor
        /// </summary>
        public StockTableDescriptor Add(string Name, string Sql)
        {
            return Add(Name, Sql, "");
        }
        /// <summary>
        /// Adds a stock table Descriptor
        /// </summary>
        public StockTableDescriptor Add(string Name, string Sql, string ZoomCommand)
        {
            StockTableDescriptor Result = base.Add(Name);
            Result.Sql = Sql;
            Result.ZoomCommand = ZoomCommand;
            return Result;
        }
    }
}
