﻿namespace WebLib.AspNet
{
    /// <summary>
    /// Marks a model property as disable. Results in a disabled html attribute
    /// <para>NOTE: Used by <see cref="TagHelperControlRow"/> custom tag helper.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PlaceholderAttribute: Attribute, IModelAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlaceholderAttribute(string Key)
        {
            this.Key = Key;
        }

        /// <summary>
        /// The resource string key
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// The localized placeholder text
        /// </summary>
        public string Text { get { return Key; } } // TODO: return localizable string, see NopLabelTagHelper
        /// <summary>
        /// The class name of the attribute.
        /// <para>NOTE: <see cref="IModelAttribute"/> implementation. </para>
        /// </summary>
        public string ClassName { get { return this.GetType().Name; } }
    }
}
