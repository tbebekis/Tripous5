using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

using WebLib;

namespace WebDesk
{

    /// <summary>
    /// Cookie authentication helper
    /// </summary>
    static public class CookieAuthHelper
    {
        /// <summary>
        /// Returns the default cookie options for the user cookie.
        /// </summary>
        /// <returns></returns>
        static public CookieOptions GetUserCookieOptions()
        {
            var Settings = DataStore.GetSettings();

            CookieOptions Options = new CookieOptions();
            Options.HttpOnly = true;

            // NOTE: If the CookiePolicyOptions.CheckConsentNeeded is set to true in the ConfigureServices()
            // then the CookieOptions.IsEssential must be set to true too.
            // Otherwise the cookie is considered a non-essential one
            // and it will not being sent to the browser (no Set-Cookie header) without the user's explicit permission.
            // SEE: https://stackoverflow.com/questions/52456388/net-core-cookie-will-not-be-set
            Options.IsEssential = true;

            Options.SameSite = SameSiteMode.Strict;

            // expiration
            if (Settings.General.UserCookieExpirationHours < 0)             // never 
            {
                // SEE: https://stackoverflow.com/questions/51536506/how-to-set-never-expiring-cookie-in-asp-net-core
                Options.Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));
            }
            else if (Settings.General.UserCookieExpirationHours == 0)       // immediately
            {
                // do nothing
            }
            else                                                        // after CookieExpirationHours    
            {
                Options.Expires = DateTime.Now.AddHours(Settings.General.UserCookieExpirationHours);
            }

            return Options;
        }
        /// <summary>
        /// Sets the <see cref="CookieAuthenticationOptions"/>. To be called from Startup.
        /// </summary>
        static public void SetCookieConfigurationOptions(CookieAuthenticationOptions o)
        {
            o.EventsType = typeof(AuthCookieEvents);
            o.LoginPath = "/login";
            o.ReturnUrlParameter = "ReturnUrl";
            //o.AccessDeniedPath = "/Account/Forbidden/";
        }
    }
}
