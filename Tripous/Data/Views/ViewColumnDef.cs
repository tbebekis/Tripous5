using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{


    /// <summary>
    /// Represents a column in any ui container.
    /// </summary>
    public class ViewColumnDef  
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewColumnDef()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewColumnDef(IEnumerable<SqlBrokerFieldDef> Fields)
        {
            foreach (var Field in Fields)
                Controls.Add(new ViewControlDef(Field));
        }

        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string SourceName { get; set; }



        /// <summary>
        /// A list of control rows.  
        /// </summary>
        public List<ViewControlDef> Controls { get; } = new List<ViewControlDef>();
    }


 
}
