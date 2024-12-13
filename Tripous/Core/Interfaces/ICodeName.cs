namespace Tripous
{
    /// <summary>
    /// Represents an object with a unique Name  (Code property)
    /// </summary>
    public interface ICodeName
    {
        /// <summary>
        /// The unique "code name" of the method or constructor. 
        /// </summary>
        string Code { get; }
    }

}
