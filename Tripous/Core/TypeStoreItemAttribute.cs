namespace Tripous
{
    /// <summary>
    /// Marks a <see cref="System.Type"/>, such as enums, Broker and Form classes, as a code element that must be registered with the <see cref="TypeStore"/>.
    /// <para>The ObjectStore registers automatically to the <see cref="TypeStore"/> any code element which is marked with this attribute. </para>
    /// <para>A <see cref="System.Type"/> may be registered with the <see cref="TypeStore"/> using multiple names. </para>
    /// <para>After that, those names can be used in order to create an instance of the type. </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class TypeStoreItemAttribute : Attribute
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public TypeStoreItemAttribute()
        {
        }
        /// <summary>
        /// Constructor
        /// <para>TypeNames is a semi-colon delimited list of type names associated with the type this attributes marks</para>
        /// </summary>
        public TypeStoreItemAttribute(string TypeNames)
        {
            this.TypeNames = TypeNames;
        }



        /* properties */
        /// <summary>
        /// Gets a semi-colon delimited list of type names associated with the type this attributes marks.
        /// </summary>
        public string TypeNames { get; private set; }

    }




}
