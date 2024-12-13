namespace Tripous.Data
{
  
    /// <summary>
    /// Indicates how the user enters of selects the criterion value
    /// </summary>
    [Flags]
    [TypeStoreItem]
    public enum SqlFilterMode
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that the user enters the criterion value into an input control.
        /// </summary>
        Simple = 1,
        /// <summary>
        /// Indicates that the user selects the criterion value from a list of values returned by a SELECT statement
        /// </summary>
        EnumQuery = 2,
        /// <summary>
        /// Indicates that the user selects the criterion value from a constant list of values given at design time
        /// </summary>
        EnumConst = 4,
        /// <summary>
        /// Indicates that the user selects the criterion value by using a LocatorBox Ui to issue a lookup select
        /// </summary>
        Locator = 8,
    }
}
