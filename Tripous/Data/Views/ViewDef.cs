using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
 


    /// <summary>
    /// Top level container. Represents a desktop form or a html page
    /// </summary>
    public class ViewDef
    {
        string fSourceName;

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDef()
        {

        }


        static public ViewDef CreateViewDef(SqlBrokerDef Broker)
        {
            ViewDef View = new ViewDef();

#warning EDW

            return View;
        }


        /// <summary>
        /// A unique name among all view containers.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The caption text.
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// The data source name
        /// </summary>
        public string SourceName
        {
            get { return !string.IsNullOrWhiteSpace(fSourceName) ? fSourceName : Name; }
            set { fSourceName = value; }
        }
        /// <summary>
        /// A list of tabs. Could be empty.
        /// </summary>
        public List<ViewTabDef> Tabs { get; } = new List<ViewTabDef>();
        /// <summary>
        /// A list of groups. Could be empty.
        /// </summary>
        public List<ViewGroupDef> Groups { get; } = new List<ViewGroupDef>();
        /// <summary>
        /// A list of columns. Could be empty.
        /// </summary>
        public List<ViewColumnDef> Columns { get; } = new List<ViewColumnDef>();

        /// <summary>
        /// Columns per screen size
        /// </summary>
        public UiSplit Split { get; set; } = new UiSplit();
    } 

}
