namespace Tripous
{
    /// <summary>
    /// Indicates the type of the descriptor.
    /// That is how this descriptor came into existence.
    /// </summary>
    public enum DescriptorMode
    {
        /// <summary>
        /// A descriptor registered by application code
        /// </summary>
        Application,
        /// <summary>
        /// A descriptor created by user customization, based on an
        /// existing application descriptor. The new descriptor
        /// replaces the one it is based on.
        /// </summary>
        Replica,
        /// <summary>
        /// A totally new descriptor created by user customization
        /// </summary>
        Custom,
        /// <summary>
        /// A descriptor registered by Tripous system code
        /// </summary>
        System,
    }

}
