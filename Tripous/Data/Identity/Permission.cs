using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Tripous.Identity
{

    /// <summary>
    /// Permission
    /// </summary>
    public class Permission : IdentityEntity
    {
        /* construction */
        /// <summary>
        /// Construction
        /// </summary>
        public Permission()
        {
        }
        /// <summary>
        /// Construction
        /// </summary>
        public Permission(DataRow Row)
            : base(Row)
        {
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /* properties */
        /// <summary>
        /// The name of a permission
        /// </summary>
        public string Name { get; set; }
    }
}
