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
        string fTitle;

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDef()
        {

        }


        /// <summary>
        /// Creates a default view based on a broker descriptor.
        /// </summary>
        static public ViewDef CreateViewDef(SqlBrokerDef Broker)
        {
            ViewDef View = new ViewDef() {
                Title = Broker.Title,
                SourceName = Broker.MainTableName
            };

            // filters (search) tab
            ViewTabDef FilterTab = new ViewTabDef() { TitleKey = "Filters" };
            View.Tabs.Add(FilterTab);

            // list (browse) tab
            ViewTabDef ListTab = new ViewTabDef() { TitleKey = "List" };
            View.Tabs.Add(ListTab);

            // data tab
            ViewTabDef DataTab = new ViewTabDef() { TitleKey = "Data" };
            View.Tabs.Add(DataTab);



            return View;
        }


        /// <summary>
        /// A unique name among all view containers.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(fTitle) ? fTitle : (!string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name); }
            set { fTitle = value; }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
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
        /// Width percent of text in rows.
        /// </summary>
        public int TextSplit { get; set; } = 35;

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
