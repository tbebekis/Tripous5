namespace Tripous
{
    /// <summary>
    /// "/" – Slash
    /// "." – Dots or full stops
    /// "-" – Hyphens or dashes
    /// </summary>
    public enum DateSeparator
    {
        /// <summary>
        /// "/" – Slash
        /// </summary>
        Slash,
        /// <summary>
        /// "." – Dots or full stops
        /// </summary>
        Dot,
        /// <summary>
        /// "-" – Hyphens or dashes
        /// </summary>
        Hyphen,
    }

    /// <summary>
    /// Represents a date separator
    /// </summary>
    public class DateSeparatorItem
    {
        static List<DateSeparatorItem> Items;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DateSeparatorItem(DateSeparator Value)
        {
            this.Value = Value;
            this.Text = DateFormat.GetSeparator(Value);
        }

        /// <summary>
        /// Returns the list of valid <see cref="DateSeparatorItem"/> items.
        /// </summary>
        static public DateSeparatorItem[] GetItems()
        {
            if (Items == null)
            {
                Items = new List<DateSeparatorItem>();
                Items.Add(new DateSeparatorItem(DateSeparator.Hyphen));
                Items.Add(new DateSeparatorItem(DateSeparator.Slash));
                Items.Add(new DateSeparatorItem(DateSeparator.Dot));
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
        public DateSeparator Value { get; }
        /// <summary>
        /// The display text
        /// </summary>
        public string Text { get; }
    }
}
