using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebLib
{

    /// <summary>
    /// Constructs a list of claims
    /// </summary>
    public interface IClaimListProvider
    {
        /// <summary>
        /// Creates and returns a claim list regarding this instance
        /// </summary>
        List<Claim> GetClaimList(string AuthenticationScheme);
    }
}
