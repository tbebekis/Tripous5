using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tripous.Data
{
    /// <summary>
    /// View tool-bar flags. Indicates what buttons to display.
    /// </summary>
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ViewToolBarFlags_NOT_USED
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Home
        /// </summary>
        Home = 1,

        /// <summary>
        /// List
        /// </summary>
        List = 2,
        /// <summary>
        /// Filters
        /// </summary>
        Filters = 4,

        /// <summary>
        /// First
        /// </summary>
        First = 0x10,
        /// <summary>
        /// Prior
        /// </summary>
        Prior = 0x20,
        /// <summary>
        /// Next
        /// </summary>
        Next = 0x40,
        /// <summary>
        /// Last
        /// </summary>
        Last = 0x80,

        /// <summary>
        /// Edit
        /// </summary>
        Edit = 0x100,
        /// <summary>
        /// Insert
        /// </summary>
        Insert = 0x200,
        /// <summary>
        /// Delete
        /// </summary>
        Delete = 0x400,
        /// <summary>
        /// Save
        /// </summary>
        Save = 0x800,

        /// <summary>
        /// Cancel
        /// </summary>
        Cancel = 0x1000,
        /// <summary>
        /// Close
        /// </summary>
        Close = 0x2000,

        /// <summary>
        /// Navigation
        /// </summary>
        Navigation = First | Prior | Next | Last,
        /// <summary>
        /// AllEdits
        /// </summary>
        AllEdits = Edit | Insert | Delete | Save,
    }

    /*
    <div class="ToolBar ViewToolBar">
        <a class="ButtonEx" data-setup="{ Command: 'Home', Text: 'Home', ToolTip: 'Home' , IcoClasses: 'fa fa-home',  NoText: true, Ico: 'Left'}"></a>

        <a class="ButtonEx" data-setup="{ Command: 'List', Text: 'List', ToolTip: 'List' , IcoClasses: 'fa fa-list-alt',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Filter', Text: 'Filter', ToolTip: 'Filter' , IcoClasses: 'fa fa-search',  NoText: true, Ico: 'Left'}"></a>

        <a class="ButtonEx" data-setup="{ Command: 'First', Text: 'First', ToolTip: 'First' , IcoClasses: 'fa fa-step-backward',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Prior', Text: 'Prior', ToolTip: 'Prior' , IcoClasses: 'fa fa-caret-left',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Next', Text: 'Next', ToolTip: 'Next' , IcoClasses: 'fa fa-caret-right',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Last', Text: 'Last', ToolTip: 'Last' , IcoClasses: 'fa fa-step-forward',  NoText: true, Ico: 'Left'}"></a>

        <a class="ButtonEx" data-setup="{ Command: 'Edit', Text: 'Edit', ToolTip: 'Edit' , IcoClasses: 'fa fa-edit',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Insert', Text: 'Insert', ToolTip: 'Insert' , IcoClasses: 'fa fa-plus',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Delete', Text: 'Delete', ToolTip: 'Delete' , IcoClasses: 'fa fa-minus',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Save', Text: 'Save', ToolTip: 'Save' , IcoClasses: 'fa fa-floppy-o',  NoText: true, Ico: 'Left'}"></a>

        <a class="ButtonEx" data-setup="{ Command: 'Cancel', Text: 'Cancel', ToolTip: 'Cancel' , IcoClasses: 'fa fa-times',  NoText: true, Ico: 'Left'}"></a>
        <a class="ButtonEx" data-setup="{ Command: 'Close', Text: 'Close', ToolTip: 'Close' , IcoClasses: 'fa fa-sign-out',  NoText: true, Ico: 'Left'}"></a>
    </div> 
     */
}
