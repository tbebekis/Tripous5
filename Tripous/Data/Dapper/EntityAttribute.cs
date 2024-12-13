namespace Tripous.Data
{
    /// <summary>
    /// Marks table/entity classes. Used in mapping an entity to a database table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// A mapping to CRUDMode values.
        /// </summary>
        static public Dictionary<char, CRUDMode> ModeDic = new Dictionary<char, CRUDMode>() {
            { 'N', CRUDMode.None },
            { 'I', CRUDMode.Insert },
            { 'U', CRUDMode.Update },
            { 'D', CRUDMode.Delete },
            { 'G', CRUDMode.GetById },
            { 'M', CRUDMode.GetByMasterId },
            { 'A', CRUDMode.GetAll },
            { 'F', CRUDMode.GetByFilter },
        };


        /// <summary>
        /// Converts a string Mode (e.g. "IUDG") to a CRUDMode value.
        /// </summary>
        static CRUDMode StrToMode(string Mode)
        {
            CRUDMode Result = CRUDMode.None;

            if (!string.IsNullOrWhiteSpace(Mode))
            {
                char C;
                foreach (char c in Mode)
                {
                    C = char.ToUpper(c);
                    if (ModeDic.ContainsKey(C))
                    {
                        Result |= ModeDic[C];
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityAttribute()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConnectionName">The name of the connection in config file that represents the database this table belongs to.</param>
        /// <param name="TableName">The name of the table in the database.</param>
        /// <param name="PrimaryKeys">A semicolon delimited list of field names that comprise of the primary key. When multiple, then the order is sigificant and must be kept everywhere (Service, Controller, etc).</param>
        /// <param name="Mode">A bit-field indicating the allowable CRUD operations in a database table</param>
        /// <param name="Autoincrement">True when the table provides a single field primary key that is an auto-increment integer.</param>
        /// <param name="PacketType">The type of the Packet. A Packet represents an entity and used as a Model in conveying information to client applications.</param>
        public EntityAttribute(string TableName, string PrimaryKeys, CRUDMode Mode, Type PacketType = null, bool Autoincrement = false, string ConnectionName = "")
        {
            this.ConnectionName = !string.IsNullOrWhiteSpace(ConnectionName)? ConnectionName: SysConfig.DefaultConnection;
            this.TableName = TableName;
            this.PrimaryKeys = PrimaryKeys;
            this.Mode = Mode;
            this.PacketType = PacketType;
            this.Autoincrement = Autoincrement;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConnectionName">The name of the connection in config file that represents the database this table belongs to.</param>
        /// <param name="TableName">The name of the table in the database.</param>
        /// <param name="PrimaryKeys">A semicolon delimited list of field names that comprise of the primary key. When multiple, then the order is sigificant and must be kept everywhere (Service, Controller, etc).</param>
        /// <param name="Mode">A string with characters that map to CRUDMode value. Valid characters: NIUDGMA </param>
        /// <param name="Autoincrement">True when the table provides a single field primary key that is an auto-increment integer.</param>
        /// <param name="PacketType">The type of the Packet. A Packet represents an entity and used as a Model in conveying information to client applications.</param>
        public EntityAttribute(string TableName, string PrimaryKeys, string Mode, Type PacketType = null, bool Autoincrement = false, string ConnectionName = "")
        {
            this.ConnectionName = !string.IsNullOrWhiteSpace(ConnectionName) ? ConnectionName : SysConfig.DefaultConnection;
            this.TableName = TableName;
            this.PrimaryKeys = PrimaryKeys;
             
            this.PacketType = PacketType;
            this.Autoincrement = Autoincrement;
            this.Mode = StrToMode(Mode);            
        }


        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(TableName) ? TableName : base.ToString();
        }


        /// <summary>
        /// The name of the connection in config file that represents the database this table belongs to.
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// The name of the table in the database.
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// A semicolon delimited list of field names that comprise of the primary key. 
        /// <para>CAUTION: When multiple, then the order is sigificant and must be kept everywhere (Service, Controller, etc).</para>
        /// </summary>
        public string PrimaryKeys { get; set; }
        /// <summary>
        /// True when the table provides a single field primary key that is an auto-increment integer.
        /// </summary>
        public bool Autoincrement { get; set; }
        /// <summary>
        /// A bit-field indicating the allowable CRUD operations in a database table
        /// </summary>
        public CRUDMode Mode { get; set; }
        /// <summary>
        /// The Packet (model) type to configure a mapping.
        /// </summary>
        public Type PacketType { get; set; }
    }

}
