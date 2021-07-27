/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tripous.Data
{
 


    /// <summary>
    /// A collection of ColumnSetting elements
    /// </summary>
    public class ColumnSettings : NamedItems<ColumnSetting>
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnSettings()
        {
        }


        /* public */
        /// <summary>
        /// Loads the items of this instance to the specified List
        /// </summary>
        public void ToList(IList<ColumnSetting> List)
        {
            List.Clear();
            foreach (ColumnSetting CS in this)
                List.Add(CS);
        }
        /// <summary>
        /// Loads the items of the specified List to this instance
        /// </summary>
        public void FromList(IList<ColumnSetting> List)
        {
            this.Clear();
            foreach (ColumnSetting CS in List)
                this.Add(CS);
        }

        /// <summary>
        /// Constructs and returns a string suitable for DisplayLabels StringLists
        /// </summary>
        public string AsDisplayLabelsText()
        {
            StringBuilder SB = new StringBuilder();
            foreach (ColumnSetting CS in this)
            {
                if (CS.Visible)
                {
                    SB.AppendLine(string.Format("{0}={1}", CS.Name, CS.Title));
                }
            }

            return SB.ToString();

        }

        /// <summary>
        /// Returns a list of columns having GroupIndex > -1, already sorted by their GroupIndex
        /// </summary>
        public List<ColumnSetting> GetGroupColumnsSorted()
        {
            List<ColumnSetting> List = new List<ColumnSetting>();
            foreach (ColumnSetting CS in this)
                if (CS.GroupIndex >= 0)
                    List.Add(CS);

            Comparison<ColumnSetting> CompareFunc = delegate (ColumnSetting A, ColumnSetting B)
            {
                if (A.GroupIndex < B.GroupIndex)
                    return -1;
                if (A.GroupIndex > B.GroupIndex)
                    return 1;
                return 0;
            };

            List.Sort(CompareFunc);

            return List;

        }
    }

}
