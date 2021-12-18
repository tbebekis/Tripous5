using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a panel. It may contain a grid or rows with controls.
    /// <para><see cref = "Grid" /> and <see cref = "Columns" /> are checked in that order. If any is not empty the rest are ignored.</para>
    /// <para>Contains a single Grid when the <see cref="Grid"/> is not empty. </para>
    /// <para>Contains a list of columns when the <see cref="Columns"/> is not empty. Columns are control containers. </para>
    /// </summary>
    public class ViewRowDef
    {


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewRowDef()
        {
        }

 

        /* properties */ 
        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }

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
