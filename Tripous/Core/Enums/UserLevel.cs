using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous 
{

    /// <summary>
    /// The level of a user, i.e. Guest, Admin, User, etc.
    /// <para>CAUTION: Do NOT change the numbers. This type is used in systems, such as BC, where Options or Enums are zero-based.</para>
    /// </summary>
    [Flags, TypeStoreItem]
    public enum UserLevel
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// A guest user
        /// </summary>
        Guest = 1,
        /// <summary>
        /// A normal user
        /// </summary>
        User = 2,
        /// <summary>
        /// A system administrator
        /// </summary>
        Admin = 4,
        /// <summary>
        /// A client application making API calls
        /// </summary>
        ClientApp = 8,
        /// <summary>
        /// A service application
        /// </summary>
        Service = 0x100,
        
    }
}
