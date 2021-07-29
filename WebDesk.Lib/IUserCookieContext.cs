using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace WebDesk
{
    /// <summary>
    /// A requestor user cookie context
    /// </summary>
    public interface IUserCookieContext : IRequestContext
    {
        /// <summary>
        /// Sign-in. Authenticates a specified, already validated, Visitor
        /// </summary>
        Task SignInAsync(Requestor V, bool IsPersistent, bool IsImpersonation);
        /// <summary>
        /// Sign-out.
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        /// True when the user is authenticated.
        /// <para>NOTE: We check both 1) the <see cref="ClaimsPrincipal"/> of the <see cref="HttpContext"/> (the User property)
        /// and 2) that the <see cref="ClaimTypes.NameIdentifier"/> claim equals to <see cref="Requestor.Id"/></para>
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
