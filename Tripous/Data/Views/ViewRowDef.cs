using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a panel. It may contain a grid or rows with controls.
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
        /// The whole row is occupied by a grid controls.
        /// </summary>
        public ViewControlDef Grid { get; set; }
        /// <summary>
        /// A list of columns. Could be empty.
        /// </summary>
        public List<ViewColumnDef> Columns { get; set; } = new List<ViewColumnDef>();
    }
}
