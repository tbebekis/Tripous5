using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDesk.AspNet
{
    /// <summary>
    /// A custom model attribute.
    /// <para>See <see cref="TagHelperModelMetadataProvider"/> for usage instructions. </para>
    /// </summary>
    public interface IModelAttribute
    {
        /// <summary>
        /// The class name of the attribute
        /// </summary>
        string ClassName { get; }
    }
}
