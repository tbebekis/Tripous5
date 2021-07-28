using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{

    /// <summary>
    /// Represents the user or api client of a request
    /// </summary>
    public class Requestor
    {
        /// <summary>
        /// Required. Database Id
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Required. Email or User name or something
        /// </summary> 
        public string UserId { get; set; }
        /// <summary>
        /// Optional. The requestor name
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        /// Optional. The requestor email
        /// </summary> 
        public string Email { get; set; }
        /// <summary>
        /// True when requestor is blocked by admins
        /// </summary>
        public bool IsBlocked { get; set; }
    }
}
