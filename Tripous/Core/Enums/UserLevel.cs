using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous 
{

    /// <summary>
    /// The level of the current user.
    /// </summary>
    [Flags, TypeStoreItem]
    public enum UserLevel
    {
        /// <summary>
        /// Service
        /// </summary>
        Service = 1,
        /// <summary>
        /// Admin
        /// </summary>
        Admin = 2,
        /// <summary>
        /// User
        /// </summary>
        User = 4,
        /// <summary>
        /// Guest
        /// </summary>
        Guest = 8,
        
    }
}
