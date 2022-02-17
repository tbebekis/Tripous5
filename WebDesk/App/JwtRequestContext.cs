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
        string fCultureCode;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JwtRequestContext(IHttpContextAccessor HttpContextAccessor)
            : base(HttpContextAccessor)
        { 
        }

        /// <summary>
        /// The culture (language) of the current request specified as a culture code (en-US, el-GR)
        /// </summary>
        public override string CultureCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fCultureCode))
                {
                    // read the token from HTTP headers
                    JwtSecurityToken Token = JwtAuthHelper.ReadTokenFromRequestHeader(HttpContext);

                    if (Token != null && WSys.ContainsClaim(Token.Claims, Requestor.SCultureClaimType))
                    {
                        fCultureCode = WSys.GetClaimValue(Token.Claims, Requestor.SCultureClaimType);
                    }
                }
        
                return !string.IsNullOrWhiteSpace(fCultureCode)? fCultureCode: Languages.DefaultLanguage.CultureCode;
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
