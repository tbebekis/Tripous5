namespace Tripous
{
    /// <summary>
    /// Represents a method which can be called with Params. The method could
    /// be a factory method or whatever.
    /// <para>The "name" of the method is the CodeName.Code property, which is passed to the constructor</para>
    /// </summary>
    public class Invoker : CodeName, IInvoker
    {
        /// <summary>
        /// constructor
        /// </summary>
        public Invoker()
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        public Invoker(string Code)
            : base(Code)
        {
        }

        /// <summary>
        /// The method the Invoker represents. It returns an object which could be null though.
        /// </summary>
        public virtual object Call(params object[] Args)
        {
            return null;
        }
    }
}
