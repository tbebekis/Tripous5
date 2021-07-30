using System;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using System.IdentityModel.Tokens.Jwt;

using Tripous;
using Tripous.Web;

namespace WebDesk
{

    /// <summary>
    /// Request context for JWT clients
    /// </summary>
    internal class JwtRequestContext : RequestContext, IJwtRequestContext
    {
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
                Language Result = null;

                // read the token from HTTP headers
                JwtSecurityToken Token = JwtAuthHelper.ReadTokenFromRequestHeader(HttpContext);

                if (Token != null && WSys.ContainsClaim(Token.Claims, Requestor.SCultureClaimType))
                {
                    string CultureCode = WSys.GetClaimValue(Token.Claims, Requestor.SCultureClaimType);
                    Language[] Languages = DataStore.GetLanguages();
                    Result = Languages.FindByCultureCode(CultureCode);
                } 

                if (Result == null)
                    Result = DataStore.EnLanguage;

                return Result;
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
