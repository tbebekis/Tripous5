namespace Tripous
{
    /// <summary>
    /// Represents an <see cref="ISupportChange"/> object which can delay a call to its Change()
    /// method until <see cref="Changing"/> becomes true.
    /// <para>The Changing property should be called in pairs.</para>
    /// <example>
    /// Here is an example.
    /// <code>
    /// public void AddRange(T[] A)
    /// {
    ///     if (A != null)
    ///     {
    ///         Changing = true;
    ///         try
    ///         {
    ///             foreach (T Item in A)
    ///                 Add(Item);
    ///         }
    ///         finally
    ///         {
    ///             Changing = false;
    ///         }
    ///     }   
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public interface ISupportChanging : ISupportChange
    {
        /// <summary>
        /// Gets or sets a boolean value indicating whether the object is in a changing operation.
        /// </summary>
        bool Changing { get; set; }
    }
}
