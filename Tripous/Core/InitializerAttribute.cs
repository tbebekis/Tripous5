namespace Tripous
{


    /// <summary>
    /// Decorates a static parameterless method.
    /// <para>All methods marked with this attribute are called by <see cref="ObjectStore"/> when
    /// the system initializes.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InitializerAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InitializerAttribute()
        {
        }
    }

}
