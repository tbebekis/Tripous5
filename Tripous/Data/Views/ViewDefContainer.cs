using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a container of view def component items suchs as PanelList, TabControl and Accordion.
    /// </summary>
    public class ViewDefContainer<T> : ViewDefComponent where T : ViewDefComponent, new()
    {
        /// <summary>
        /// Creates and returns a component item.
        /// </summary>
        protected virtual T CreateItem(string TitleKey, string Name = "")
        {
            T Result = new T()
            {
                TitleKey = TitleKey,
                Name = Name,
            };

            return Result;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDefContainer()
        {
        }

        /// <summary>
        /// Returns a <see cref="ViewPanelListPanelDef"/> found under a specified name, if any, else null.
        /// </summary>
        public T FindTabByName(string ComponentName)
        {
            return Items.FirstOrDefault(item => Sys.IsSameText(item.Name, ComponentName));
        }
        /// <summary>
        /// Returns true if a <see cref="ViewPanelListPanelDef"/> found under a specified name.
        /// </summary>
        public bool Contains(string ComponentName)
        {
            return FindTabByName(ComponentName) != null;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewPanelListPanelDef"/>
        /// </summary>
        public T Add(string TitleKey, string Name = "")
        {
            T Result = CreateItem(TitleKey, Name);
            Items.Add(Result);
            return Result;
        }

        /* properties */
        /// <summary>
        /// A list of tabs. Could be empty. When not empty then this describes a TabControl (Pager) with child tab pages
        /// </summary>
        public List<T> Items { get; } = new List<T>();

    }

}
