using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{


    /// <summary>
    /// Represents a column in any ui container.
    /// <para>May contain: Controls.</para>
    /// </summary>
    public class ViewColumnDef  
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewColumnDef()
        {
        }

        /// <summary>
        /// Adds and returns a <see cref="ViewControlDef"/>
        /// </summary>
        public ViewControlDef AddControl(string TypeName, string TitleKey, string DataField = "", string TableName = "")
        {
            ViewControlDef Result = new ViewControlDef()
            {
                TypeName = TypeName,
                TitleKey = TitleKey,
                DataField = DataField,
                TableName = TableName,
            };

            Controls.Add(Result);
            return Result;
        }


        /* properties */
        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }
 
        /// <summary>
        /// A list of control rows.  
        /// </summary>
        public List<ViewControlDef> Controls { get; } = new List<ViewControlDef>();
    }


 
}
