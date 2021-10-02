using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLib
{

    /// <summary>
    /// Represents the sole ajax controller
    /// </summary>
    public interface IAjaxController
    {
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string ViewToString(string ViewName, object Model, IDictionary<string, object> PlusViewData = null);
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string ViewToString(string ViewName, IDictionary<string, object> PlusViewData = null);

    }
}
