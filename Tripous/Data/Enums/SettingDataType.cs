namespace Tripous.Data
{
    /// <summary>
    /// The data-type of a <see cref="Setting"/>
    /// </summary>
    [Flags]
    [TypeStoreItem]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SettingDataType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// String (nvarchar, varchar)
        /// </summary>
        String = 1,
        /// <summary>
        /// Integer
        /// </summary>
        Integer = 2,
        /// <summary>
        /// Float (float, double precision, etc)
        /// </summary>
        Float = 4,
        /// <summary>
        /// Decimal (decimal(18, 4))
        /// </summary>
        Decimal = 8,
        /// <summary>
        /// Date (date)
        /// </summary>
        Date = 0x10,
        /// <summary>
        /// DateTime (datetime, timestamp, etc)
        /// </summary>
        DateTime = 0x20,
        /// <summary>
        /// Boolean (integer always, 1 = true, else false)
        /// </summary>
        Boolean = 0x40,
        /// <summary>
        /// Single select. The Setting provides the list along with its values.
        /// </summary>
        SingleSelect = 0x80,
        /// <summary>
        /// Multi select. The Setting provides the list along with its values.
        /// </summary>
        MultiSelect = 0x100,

    }
}
