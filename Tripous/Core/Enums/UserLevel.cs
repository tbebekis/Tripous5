using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous 
{

    /// <summary>
    /// The level of a user, i.e. Guest, Admin, User, etc.
    /// </summary>
    [Flags, TypeStoreItem]
    public enum UserLevel
    {
        /// <summary>
        /// Guest
        /// </summary>
        Guest = 0,
        /// <summary>
        /// Admin
        /// </summary>
        Admin = 1,
        /// <summary>
        /// User
        /// </summary>
        User = 2,

        /// <summary>
        /// Service
        /// </summary>
        Service = 0x100,
        
    }
}
