﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{



    /// <summary>
    /// Represents a container such as a DIV or panel which groups controls under a specified text.
    /// <para>May contain: Tabs and Rows.</para>
    /// <para><see cref="Tabs"/> and <see cref="Rows"/> are checked in that order. If any is not empty the rest are ignored.</para>
    /// <para>Contains a single Pager (TabControl) when the <see cref="Tabs"/> are not empty. </para>
    /// <para>Contains a signle Panel (DIV) with one or more rows when the <see cref="Rows"/> is not empty. </para>
    /// </summary>
    public class ViewGroupDef
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewGroupDef()
        {

        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Title;
        }

        /// <summary>
        /// Adds and returns a <see cref="ViewTabDef"/>
        /// </summary>
        public ViewTabDef AddTab(string TitleKey, string TabId = "")
        {
            ViewTabDef Result = new ViewTabDef()
            {
                TitleKey = TitleKey,
                TabId = TabId,
            };

            Tabs.Add(Result);

            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewRowDef"/>
        /// </summary>
        public ViewRowDef AddRow(string TableName = "")
        {
            ViewRowDef Result = new ViewRowDef();
            Result.TableName = TableName;
            Rows.Add(Result);
            return Result;
        }

        /// <summary>
        /// Returns a <see cref="ViewTabDef"/> found under a specified Id, if any, else null.
        /// </summary>
        public ViewTabDef FindTabById(string Id)
        {
            return Tabs.Find(item => Sys.IsSameText(item.TabId, Id));
        }
        /// <summary>
        /// Returns true if a <see cref="ViewTabDef"/> found under a specified Id.
        /// </summary>
        public bool ContainsTab(string TabId)
        {
            return FindTabById(TabId) != null;
        }

        /* properties */
        /// <summary>
        /// A unique Id among sibling groups, e.g. Main, Details, Addresses,  etc
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Sys.None; }
            set { }
        }

        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// A list of tabs. Could be empty. When not empty then this describes a TabControl (Pager) with child tab pages
        /// </summary>
        public List<ViewTabDef> Tabs { get; } = new List<ViewTabDef>();
        /// <summary>
        /// A list of rows. Could be empty.
        /// </summary>
        public List<ViewRowDef> Rows { get; } = new List<ViewRowDef>();
    }


    /// <summary>
    /// Extensions
    /// </summary>
    static public class ViewGroupDefExtensions
    {
        /// <summary>
        /// Returns a <see cref="ViewGroupDef"/> found under a specified Id, if any, else null.
        /// </summary>
        static public ViewGroupDef FindGroupById(this IEnumerable<ViewGroupDef> Groups, string GroupId)
        {
            return Groups.FirstOrDefault(item => Sys.IsSameText(item.GroupId, GroupId));
        }
        /// <summary>
        /// Returns true if a <see cref="ViewGroupDef"/> found under a specified Id.
        /// </summary>
        static public bool Contains(this IEnumerable<ViewGroupDef> Groups, string TabId)
        {
            return Groups.FindGroupById(TabId) != null;
        }

        /// <summary>
        /// Adds and returns a <see cref="ViewGroupDef"/>
        /// </summary>
        static public ViewGroupDef Add(this List<ViewGroupDef> Groups, string TitleKey, string GroupId = "")
        {
            ViewGroupDef Result = new ViewGroupDef()
            {
                TitleKey = TitleKey,
                GroupId = GroupId,
            };

            Groups.Add(Result);

            return Result;
        }
    }

}
