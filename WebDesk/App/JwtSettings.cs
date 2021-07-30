using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDesk
{
    /// <summary>
    /// Security settings
    /// </summary>
    public class JwtSettings
    {

        /// <summary>
        /// True to enable security
        /// </summary>
        public bool Enabled { get; set; } = true;


        /// <summary>
        /// A string used in signing the Jwt token.
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Optional. A case-sensitive string or URI value representing the entity that generates the tokens.
        /// <para>SEE: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.1 </para>
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Optional. A string array or a signle string or URI value identifying the recipients that the Jwt Token is intended for.
        /// <para>SEE: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.3 </para>
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// The number of minutes a Jwt is valid.
        /// </summary>
        public int LifeTimeMinutes { get; set; } = 15;

    }
}
