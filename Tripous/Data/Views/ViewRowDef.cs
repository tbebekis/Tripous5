using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a panel. It may contain a grid or rows with controls.
    /// <para>May contain: A single Grid or Columns.</para>
    /// <para><see cref = "Grid" /> and <see cref = "Columns" /> are checked in that order. If any is not empty the rest are ignored.</para>
    /// <para>Contains a single Grid when the <see cref="Grid"/> is not empty. </para>
    /// <para>Contains a list of columns when the <see cref="Columns"/> is not empty. Columns are control containers. </para>
    /// </summary>
    public class ViewRowDef
    {
        /// <summary>
        /// Constant. Default column and width classes
        /// </summary>
        public const string DefaultColumnCssClasses = "tp-Col l-75 m-70 s-70 xs-100 ";

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewRowDef()
        {
        }

        /// <summary>
        /// Adds and returns a <see cref="ViewColumnDef"/>
        /// </summary>
        public ViewColumnDef AddColumn(string TableName = "")
        {
            ViewColumnDef Result = new ViewColumnDef();
            Result.TableName = TableName;
            Columns.Add(Result);
            return Result;
        }
        /// <summary>
        /// Sets the <see cref="Grid"/> control.
        /// </summary>
        public ViewControlDef SetGrid(string TableName = "")
        {
            this.Grid = new ViewControlDef() {
                TypeName = ViewControlDef.Grid,
                TableName = TableName,
            };

            this.Grid["Width"] = "100%";

            return this.Grid;
        }

        /* properties */
        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Column and width classes. Not used when columns is empty.
        /// </summary>
        public string ColumnCssClasses { get; set; } = DefaultColumnCssClasses;

        /// <summary>
        /// The whole row is occupied by a grid control.
        /// </summary>
        public ViewControlDef Grid { get; set; }
        /// <summary>
        /// A list of columns. Could be empty.
        /// </summary>
        public List<ViewColumnDef> Columns { get; set; } = new List<ViewColumnDef>();
    }


 
}
