using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Tripous;
using Tripous.Web;

namespace WebDesk
{



    /// <summary>
    /// Request context for browser clients (cookies)
    /// </summary>
    internal class UserRequestContext: RequestContext, IUserRequestContext
    {
        const string SUserCookieName = "WebDesk.User";        

        Requestor fRequestor;
        UserCookie Cookie;
        bool LoadingCookie;

        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// <para>If the user is found, then it sets the cookie too.</para>
        /// </summary>
        Requestor GetRequestor(string Id)
        {
            Requestor Result = null;

            if (!string.IsNullOrWhiteSpace(Id))
            {
                Result = DataStore.GetRequestor(Id);
                if (Result != null)
                    Cookie.RequestorId = Result.Id;
            }

            return Result;
        }

        /// <summary>
        /// Reads the cookie from <see cref="HttpRequest"/> and loads the properties of this instance.
        /// </summary>
        void ReadCookie()
        {
            if (!LoadingCookie)
            {
                LoadingCookie = true;
                try
                {
                    string Base64 = HttpContext.Request.Cookies[SUserCookieName];
                    if (!string.IsNullOrWhiteSpace(Base64))
                    {
                        string JsonText = Sys.Base64ToString(Base64, Encoding.UTF8);
                        if (!string.IsNullOrWhiteSpace(JsonText))
                            Json.FromJson(this.Cookie, JsonText);
                    }
                }
                catch
                {
                }
                finally
                {
                    LoadingCookie = false;
                }
            }
        }
        /// <summary>
        /// Writes cookie's properties to the <see cref="HttpResponse"/> (as base64 string).
        /// </summary>
        void WriteCookie()
        {
            if (!LoadingCookie)
            {
                string JsonText = Json.ToJson(this.Cookie);
                string Base64 = Sys.StringToBase64(JsonText, Encoding.UTF8);
                HttpContext.Response.Cookies.Delete(SUserCookieName);
                HttpContext.Response.Cookies.Append(SUserCookieName, Base64, CookieAuthHelper.GetUserCookieOptions());
            }
        }

        /// <summary>
        /// Called whenever a property of Cookie changes.
        /// </summary>
        void Cookie_Changed(object sender, EventArgs e)
        {
            WriteCookie();
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public UserRequestContext(IHttpContextAccessor HttpContextAccessor)
            : base(HttpContextAccessor)
        {
            this.Cookie = new UserCookie();

            ReadCookie();
            this.Cookie.Changed += Cookie_Changed;
        }

        /* public */
        /// <summary>
        /// Creates and returns a claim list regarding a specified visitor
        /// </summary>
        public List<Claim> GetClaimList(Requestor V)
        {
            List<Claim> ClaimList = V.GetClaimList(Lib.CookieAuthScheme);
            return ClaimList;
        }
        /// <summary>
        /// Sign-in. Authenticates a specified, already validated, Visitor
        /// </summary>
        public async Task SignInAsync(Requestor V, bool IsPersistent, bool IsImpersonation)
        {
            // await Task.CompletedTask;
            this.IsImpersonation = IsImpersonation;
 
            // create claim list
            List<Claim> ClaimList = GetClaimList(V); 

            // identity and principal
            // NOTE: setting the second parameter actually authenticates the identity (IsAuthenticated returns true)
            ClaimsIdentity Identity = new ClaimsIdentity(ClaimList, Lib.CookieAuthScheme);
            ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);

            // properties
            AuthenticationProperties AuthProperties = new AuthenticationProperties();
            AuthProperties.AllowRefresh = true;
            AuthProperties.IssuedUtc = DateTime.UtcNow;
            AuthProperties.ExpiresUtc = DateTime.UtcNow.AddDays(30); // override the ExpireTimeSpan option of CookieAuthenticationOptions set with AddCookie
            AuthProperties.IsPersistent = IsPersistent;

            // authenticate the principal under the scheme
            await HttpContext.SignInAsync(Lib.CookieAuthScheme, Principal, AuthProperties);
            this.Requestor = V;
        }
        /// <summary>
        /// Sign-out.
        /// </summary>
        public async Task SignOutAsync()
        {
            IsImpersonation = false;

            string Scheme = Lib.CookieAuthScheme;

            await HttpContext.SignOutAsync(Scheme);
            this.Requestor = null;
        }

        /* properties */ 
        /// <summary>
        /// The current Requestor (the session Requestor)
        /// <para>NOTE: Setting or unsetting the Requestor, sets or unsets the Requestor cookie too.</para>
        /// </summary>
        public override Requestor Requestor
        {
            get
            {
                // get Visitor from Business Central, if any
                if (fRequestor == null)
                    fRequestor = GetRequestor(Cookie.RequestorId);

                if (fRequestor == null)
                    fRequestor = GetRequestor(Cookie.UnAuthenticatedId);

                // no visitor at all, so create a new visitor and set the cookie
                //if (fRequestor == null)
                //    fRequestor = CreateNewVisitor();

                if (fRequestor == null && Cookie != null)
                {
                    Cookie.RequestorId = string.Empty;
                }

                return fRequestor;
            }

            set
            {
                // right after a logout
                if (value == null)
                {
                    fRequestor = GetRequestor(Cookie.UnAuthenticatedId);

                    //if (fRequestor == null)
                    //    fRequestor = CreateNewVisitor();
                }
                // right after the login
                else
                {
                    fRequestor = value;
                    Cookie.RequestorId = value.Id;

                    if (!string.IsNullOrWhiteSpace(Cookie.UnAuthenticatedId) && Cookie.UnAuthenticatedId == Cookie.RequestorId)
                        Cookie.UnAuthenticatedId = "";
                }
            }

        }
        /// <summary>
        /// The selected language (and culture) of the Requestor
        /// </summary>
        public override Language Language
        {
            get
            {
                Language Result = null;

                if (Cookie != null && DataStore.Initialized)
                {
                    string CultureCode = Cookie.CultureCode;
                    Language[] Languages = DataStore.GetLanguages();
                    Result =  Tripous.Languages.FindByCultureCode(CultureCode);
                }

                if (Result == null)
                    Result = DataStore.EnLanguage;

                return Result;

            }
            set
            {
                if (Language != value)
                {
                    // CAUTION: Language setter is called from the inherited constructor too, when Cookie is still null.
                    if (Cookie != null)
                    {
                        string CultureCode = Cookie.CultureCode;
                        if (value != null && !value.CultureCode.IsSameText(CultureCode))
                        {
                            Cookie.CultureCode = value.CultureCode;
                        }
                    }
                }
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
                    string Scheme = Lib.CookieAuthScheme;
                    Result = HttpContext.User.Identity.AuthenticationType == Scheme;
                }

                return Result;
            }
        }
        /// <summary>
        /// True when the Visitor has loged-in usin the SuperUserPassword
        /// </summary>
        public bool IsImpersonation
        {
            get
            {
                return Session.Get<bool>("IsImpersonation");
            }
            private set
            {
                Session.Set<bool>("IsImpersonation", value);
            }
        }

    }
}
