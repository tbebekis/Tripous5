namespace Tripous
{

    /// <summary>
    /// Type code abstraction
    /// </summary>
    [Flags]
    [TypeStoreItem]
    [JsonConverter(typeof(SimpleTypeJsonConverter))]
    public enum SimpleType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,                   // N
        /// <summary>
        /// 
        /// </summary>
        String = 1,                 // S        {"string", "nvarchar", "nchar", "varchar", "char"}
        /// <summary>
        /// 
        /// </summary>
        Integer = 2,                // I        {"integer", "int", "larginteger", "largint", "smallint", "autoinc", "autoincrement", "identity", "counter"}
        /// <summary>
        /// 
        /// </summary>
        Boolean = 4,                // L        {"boolean", "bit", "logical"}
        /// <summary>
        /// 
        /// </summary>
        Float = 8,                  // F        {"float", "double", "extended", "real", "BCD", "FBCD"}
        /// <summary>
        /// 
        /// </summary>
        Currency = 0x10,            // C        {"currency", "money"}
        /// <summary>
        /// 
        /// </summary>
        Date = 0x20,                // D        {"date"}
        /// <summary>
        /// 
        /// </summary>
        Time = 0x40,                // T        {"time"}
        /// <summary>
        /// 
        /// </summary>
        DateTime = 0x80,            // M        {"datetime", "timestamp"}
        /// <summary>
        /// 
        /// </summary>
        Object = 0x100,             // O        {"object", "ref", "reference", "byref"}
        /// <summary>
        /// 
        /// </summary>
        Enum = 0x200,               // E
        /// <summary>
        /// 
        /// </summary>
        Memo = 0x400,               // X        {"memo", "text", "clob"}
        /// <summary>
        /// 
        /// </summary>
        Graphic = 0x800,            // G        {"graphic", "image"}
        /// <summary>
        /// 
        /// </summary>
        Blob = 0x1000,              // B        {"blob", "bin", "binary"}
        /// <summary>
        /// 
        /// </summary>
        Interface = 0x2000,         // U        {"interface", "IUnknown", "IDispatch", "IInterface"}
        /// <summary>
        /// 
        /// </summary>
        WideString = 0x4000,        // W        {"widestring", "widechar",    "bstr", "olestr", "olestring"}
    }







}
