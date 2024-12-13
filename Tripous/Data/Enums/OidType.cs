namespace Tripous.Data
{
    /// <summary>
    /// Indicates the data type of an OID.
    /// <para>An OID (Object Identifier) must uniquely identify a data table row and must has no business meaning at all.</para>
    /// </summary>
    public enum OidType
    {
        /// <summary>
        /// The OID is an Integer. The <see cref="OidMode"/> indicates the kind (before, after, custom)
        /// </summary>
        Integer,
        /// <summary>
        /// The OID is a Guid string.
        /// </summary>
        Guid,
    }
}
