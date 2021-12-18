using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;

namespace WebLib.Models
{

    /// <summary>
    /// A model for a row
    /// </summary>
    public class ViewRowModel
    {
        /// <summary>
        /// Constant. Default column and width classes
        /// </summary>
        public const string DefaultColumnCssClasses = "tp-Col l-75 m-70 s-70 xs-100 ";

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewRowModel(ViewDef ViewDef, ViewRowDef RowDef)
        {
            this.ViewDef = ViewDef;
            this.RowDef = RowDef;

            this.ColumnCssClasses = ViewDef.ColumnSplit.GetColumnCssClasses();
        }


        /* properties */
        /// <summary>
        /// Column and width classes. Not used when columns is empty.
        /// </summary>
        public string ColumnCssClasses { get; set; }

        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The definition of the row.
        /// </summary>
        public ViewRowDef RowDef { get; }
    }


}
