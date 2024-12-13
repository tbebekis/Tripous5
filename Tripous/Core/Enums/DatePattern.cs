namespace Tripous
{
    /// <summary>
    /// Indicates the pattern of a date format string.
    /// </summary>
    public enum DatePattern
    {
        /// <summary>
        /// MM-dd-yyyy. Middle-endian (month, day, year), e.g. 04/22/96  
        /// </summary>
        MDY,
        /// <summary>
        /// dd-MM-yyyy. Little-endian (day, month, year), e.g. 22.04.96 or 22/04/96
        /// </summary>
        DMY,
        /// <summary>
        /// yyyy-MM-dd. Big-endian (year, month, day), e.g. 1996-04-22
        /// </summary>
        YMD
    }



    /// <summary>
    /// Represents a date pattern
    /// </summary>
    public class DatePatternItem
    {
        static List<DatePatternItem> Items;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DatePatternItem(DatePattern Value)
        {
            this.Value = Value;
            switch (Value)
            {
                case DatePattern.MDY:
                    this.Text = "Month, Day, Year";
                    break;
                case DatePattern.DMY:
                    this.Text = "Day, Month, Year";
                    break;
                default: // YMD
                    this.Text = "Year, Month, Day";
                    break;
            }
        }


        /// <summary>
        /// Returns the list of valid <see cref="DatePatternItem"/> items.
        /// </summary>
        static public DatePatternItem[] GetItems()
        {
            if (Items == null)
            {
                Items = new List<DatePatternItem>();
                Items.Add(new DatePatternItem(DatePattern.MDY));
                Items.Add(new DatePatternItem(DatePattern.DMY));
                Items.Add(new DatePatternItem(DatePattern.YMD));
            }

            return Items.ToArray();
        }

        /* overrides */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Text;
        }

        /* properties */
        /// <summary>
        /// The value
        /// </summary>
        public DatePattern Value { get; }
        /// <summary>
        /// The display text
        /// </summary>
        public string Text { get; }
    }
}
