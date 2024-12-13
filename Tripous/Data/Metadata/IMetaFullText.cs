namespace Tripous.Data.Metadata
{
    /// <summary>
    /// Represents a metadata item having enough information to display,
    /// that may be displayed in a separate Form.
    /// </summary>
    public interface IMetaFullText
    {
        /// <summary>
        /// Gets the full text
        /// </summary>
        string FullText { get; }
    }
}
