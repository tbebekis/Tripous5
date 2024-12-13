namespace Tripous
{
    /// <summary>
    /// Represents a method which can be called with Params. The method could
    /// be a factory method or whatever.
    /// <para>The "name" of the method is the ICodeName.Code property</para>
    /// </summary>
    public interface IInvoker : ICodeName
    {
        /// <summary>
        /// The method the Invoker represents. It returns an object that could be null.
        /// </summary>
        object Call(params object[] Params);
    }
}
