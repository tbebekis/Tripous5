namespace Tripous.Data
{

    /// <summary>
    /// To be used when this is a single or multi select <see cref="Setting"/> instances.
    /// </summary>
    public class SettingSelectItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingSelectItem()
        {
        }

        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return $"{Id}-{TextKey}";
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A resource string key or a display text for this item
        /// </summary>
        public string TextKey { get; set; }
        /// <summary>
        /// The display order of this instance
        /// </summary>
        public int DisplayOrder { get; set; }
    }

}
