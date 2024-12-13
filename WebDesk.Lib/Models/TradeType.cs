namespace WebLib.Models
{
    /// <summary>
    /// Indicates the type of a trade
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Sales Order
        /// </summary>
        SalesOrder = 1,
        /// <summary>
        /// Sales Invoice
        /// </summary>
        SalesInvoice = 2,
    }
}
