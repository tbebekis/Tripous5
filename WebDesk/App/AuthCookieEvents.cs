using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebDesk
{

    /// <summary>
    /// SEE: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#react-to-back-end-changes
    /// SEE: http://codereform.com/blog/post/asp-net-core-2-0-authentication-with-local-logins-responding-to-backend-changes/
    /// </summary>
    internal class AuthCookieEvents : CookieAuthenticationEvents
    {
        IUserCookieContext fCookieContext;

        public AuthCookieEvents(IUserCookieContext CookieContext)
        {
            this.fCookieContext = CookieContext;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            //await Task.CompletedTask;

            try
            {
                if (fCookieContext != null && context.Principal.Identity.IsAuthenticated)
                {

                    Claim Claim = context.Principal.FindFirst(ClaimTypes.NameIdentifier); // we have Visitor.Id stored in ClaimTypes.NameIdentifier claim
                    Requestor Requestor = fCookieContext.Requestor;

                    // it is the Id claim and must be there
                    if (Claim != null && Requestor != null )
                    {
                        // Visitor is blocked  but we still have a logged-in Visitor
                        if (Requestor.IsBlocked)
                        {
                            context.RejectPrincipal();
                            await fCookieContext.SignOutAsync();
                        }
                    }
                }
            }
            catch
            {
                // do nothing
            }
        }

    }
}
