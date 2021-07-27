using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// A list of SelectSql items
    /// </summary>
    public class SelectSqlList : NamedItems<SelectSql>
    {

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SelectSqlList()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
        }


    }
}
