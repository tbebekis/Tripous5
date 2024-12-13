namespace WebLib
{
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
