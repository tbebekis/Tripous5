/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;




namespace Tripous.Model
{
    /// <summary>
    /// Describes a stock table
    /// </summary>
    public class StockTableDescriptor : QueryDescriptor
    {
        private StockTableDescriptors stockTables = new StockTableDescriptors();
        /// <summary>
        /// Constructor.
        /// </summary>
        public StockTableDescriptor()
        {
        }


        /// <summary>
        /// Override
        /// </summary>
        public override void CheckDescriptor()
        {
            base.CheckDescriptor();
        }

        /* properties */
        /// <summary>
        /// Gets the stock tables of this stock table.
        /// </summary>
        public StockTableDescriptors StockTables { get { return stockTables; } }
    }
}
