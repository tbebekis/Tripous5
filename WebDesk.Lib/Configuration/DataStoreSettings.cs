using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{

    /// <summary>
    /// Application settings
    /// </summary>
    public class DataStoreSettings
    {
        /// <summary>
        /// General settings
        /// </summary>
        public GeneralSettings General { get; set; } = new GeneralSettings();
        /// <summary>
        /// Http related settings
        /// </summary>
        public HttpSettings Http { get; set; } = new HttpSettings();
    }



    /// <summary>
    /// General settings
    /// </summary>
    public class GeneralSettings
    {
        string fCultureCode;
        string fCurrencyCode;
        string fCurrencySymbol;
        string fMoneyFormat;

        /// <summary>
        /// The default culture, i.e. el-GR
        /// <para>NOTE: This setting is assigned initially by default to any new visitor.</para>
        /// </summary>
        public string CultureCode
        {
            get => !string.IsNullOrWhiteSpace(fCultureCode) ? fCultureCode : "en-US"; // "el-GR";
            set => fCultureCode = value;
        }

        /// <summary>
        /// Default currency code, e.g. EUR, USD, etc.
        /// <para>NOTE: This setting is assigned initially by default to any new visitor.</para>
        /// </summary>
        public string CurrencyCode
        {
            get => !string.IsNullOrWhiteSpace(fCurrencyCode) ? fCurrencyCode : "EUR";
            set => fCurrencyCode = value;
        }
        /// <summary>
        /// Returns the currency symbol. Used in formatting prices
        /// </summary>
        public string CurrencySymbol
        {
            get => !string.IsNullOrWhiteSpace(fCurrencySymbol) ? fCurrencySymbol : "€";
            set => fCurrencySymbol = value;
        }
        /// <summary>
        /// Format string for formatting money values
        /// </summary>
        public string MoneyFormat
        {
            get => !string.IsNullOrWhiteSpace(fMoneyFormat) ? fMoneyFormat : $"{CurrencySymbol} 0.00";
            set => fMoneyFormat = value;
        }

        /// <summary>
        /// Default Cache retention policy, in minutes
        /// </summary>
        public int DefaultCacheTimeoutMinutes { get; set; } = 15;

        /// <summary>
        /// How many hours to keep visitor cookie valid.
        /// <para> -1 = never expires, 0 = expire immediately, nnn = expire after nnn hours</para>
        /// </summary>
        public int UserCookieExpirationHours { get; set; } = -1;

        /// <summary>
        /// Password for the super user
        /// </summary>
        public string SuperUserPassword { get; set; }
    }

    /// <summary>
    /// Http related settings
    /// </summary>
    public class HttpSettings
    {
        string fStaticFilesCacheControl;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpSettings()
        {
        }

        /* properties */
        /// <summary>
        /// Gets or sets the value of the "Cache-Control" header value for static content.
        /// <para>Leave it empty or null, for no setting at all.</para>
        /// <para>SEE: https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cache-Control </para>
        /// <para>SEE: https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response </para>
        /// </summary>
        public string StaticFilesCacheControl
        {
            get => !string.IsNullOrWhiteSpace(fStaticFilesCacheControl) ? fStaticFilesCacheControl : "no-store,no-cache";
            set => fStaticFilesCacheControl = value;
        }
    }



}
