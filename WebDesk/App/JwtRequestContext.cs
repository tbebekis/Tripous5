using System;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using System.IdentityModel.Tokens.Jwt;

using Tripous;

using WebLib;
using WebLib.AspNet;

namespace WebDesk
{

    /// <summary>
    /// Request context for JWT clients.
    /// <para>NOTE: This is a Scoped Service (i.e. one instance per HTTP Request) </para>
    /// </summary>
    internal class JwtRequestContext : RequestContext, IJwtRequestContext
    {
        Language fLanguage;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JwtRequestContext(IHttpContextAccessor HttpContextAccessor)
            : base(HttpContextAccessor)
        { 
        }

        /// <summary>
        /// The language of the current request
        /// </summary>
        public override Language Language
        {
            get
            {
                if (fLanguage == null)
                {
                    // read the token from HTTP headers
                    JwtSecurityToken Token = JwtAuthHelper.ReadTokenFromRequestHeader(HttpContext);

                    if (Token != null && WSys.ContainsClaim(Token.Claims, Requestor.SCultureClaimType))
                    {
                        string CultureCode = WSys.GetClaimValue(Token.Claims, Requestor.SCultureClaimType);
                        fLanguage = Languages.GetByCultureCode(CultureCode);
                    }
                }

                return fLanguage != null? fLanguage: DataStore.EnLanguage;
            }
            set
            {
            }
        }

        /// <summary>
        /// True when the user is authenticated with the cookie authentication scheme.
        /// </summary>
        public override bool IsAuthenticated
        {
            get
            {
                bool Result = HttpContext.User.Identity.IsAuthenticated;
                if (Result)
                {
                    string Scheme = Lib.JwtAuthScheme;
                    Result = HttpContext.User.Identity.AuthenticationType == Scheme;
                }

                return Result;
            }
        }
    }
}
