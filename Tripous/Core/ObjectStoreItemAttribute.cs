namespace Tripous
{

    /// <summary>
    /// Marks either a class or a constructor or a static method, indicating that the
    /// <see cref="ObjectStore"/> must create an entry based on the marked code element.
    /// <para></para>
    /// <para>When this attribute marks a class, the ObjectStore it first creates an instance of that class
    /// calling the default constructor.</para> 
    /// <para> Then, if the class implements the <see cref="IInvoker"/> it adds that instance as an IInvoker to its internal list. </para>
    /// <para>Else, if the class implements the <see cref="ICodeName"/> it adds that instance as an ICodeName to its internal list.</para>
    /// <para>Else it just creates a <see cref="ObjectStore.CodeNameInstance"/> passing that instance, and adds that CodeNameInstance to its internal list.</para>
    /// <para></para>
    /// <para>When this attribute marks a constructor or a static method of a class, 
    /// the ObjectStore creates an <see cref="ObjectStore.InvokerMethod"/>, which represents the constructor or the method,
    /// and adds that InvokerMethod to its internal list</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor)]
    public class ObjectStoreItemAttribute : Attribute
    {
        private string code;

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public ObjectStoreItemAttribute()
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ObjectStoreItemAttribute(string Code)
        {
            if (string.IsNullOrWhiteSpace(Code))
                throw new ArgumentNullException("Code");

            code = Code;
        }

        /* properties */
        /// <summary>
        /// The unique "code name" of the method or constructor. 
        /// </summary>
        public string Code { get { return !string.IsNullOrWhiteSpace(code) ? code : string.Empty; } }
    }

}
