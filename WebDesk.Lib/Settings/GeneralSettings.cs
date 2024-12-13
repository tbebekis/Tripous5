namespace WebLib
{
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
            get => !string.IsNullOrWhiteSpace(fCultureCode) ? fCultureCode : Lib.DefaultCultureCode;
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
}
