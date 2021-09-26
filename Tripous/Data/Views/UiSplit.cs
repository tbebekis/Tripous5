using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
    /// <summary>
    /// Columns per screen size
    /// </summary>
    public class UiSplit
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public UiSplit()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UiSplit(int All)
            : this(All, All, All, All)
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UiSplit(int LargeAndMedium, int AllOthers)
            : this(LargeAndMedium, LargeAndMedium, AllOthers, AllOthers)
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UiSplit(int Large, int Medium, int Small, int XSmall)
        {
            this.Large = Large;
            this.Medium = Medium;
            this.Small = Small;
            this.XSmall = XSmall;
        }

        /// <summary>
        /// How many columns to have in a certain screen size.
        /// </summary>
        public int XSmall { get; set; } = 1;
        /// <summary>
        /// How many columns to have in a certain screen size.
        /// </summary>
        public int Small { get; set; } = 2;
        /// <summary>
        /// How many columns to have in a certain screen size.
        /// </summary>
        public int Medium { get; set; } = 2;
        /// <summary>
        /// How many columns to have in a certain screen size.
        /// </summary>
        public int Large { get; set; } = 3;
    }
}
