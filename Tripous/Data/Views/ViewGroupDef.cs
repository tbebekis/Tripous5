using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
 


    /// <summary>
    /// Represents a container such as a DIV or panel which groups controls under a specified text.
    /// </summary>
    public class ViewGroupDef
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewGroupDef()
        {

        }

        /// <summary>
        /// The caption text.
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// A list of columns. Could be empty.
        /// </summary>
        public List<ViewColumnDef> Columns { get; } = new List<ViewColumnDef>();

        /// <summary>
        /// Columns per screen size
        /// </summary>
        public UiSplit Split { get; set; } = new UiSplit();
    }
}
