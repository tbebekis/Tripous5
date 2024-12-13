namespace WebLib
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

}
