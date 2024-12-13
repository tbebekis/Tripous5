namespace Tripous.Data
{


    /// <summary>
    /// A list of <see cref="SqlFilterDef"/> items.
    /// <para>A list of criterion fields is used in producing the "user where" clause of a SELECT statement.</para>
    /// </summary>
    public class SqlFilterDefs : List<SqlFilterDef>  //  OwnedCollection<SqlFilter>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlFilterDefs()
        {
        }

        /* public */
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilterDef Add(string TableName, string FieldName, string TitleKey, DataFieldType DataType, SqlFilterMode Mode = SqlFilterMode.Simple)
        {
            return Add(Sys.FieldPath(TableName, FieldName), TitleKey, DataType, Mode);
        }
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilterDef Add(string FieldPath, string TitleKey, DataFieldType DataType, SqlFilterMode Mode = SqlFilterMode.Simple)
        {
            SqlFilterDef Result = new SqlFilterDef();
            Result.FieldPath = FieldPath;
            Result.TitleKey = TitleKey;
            Result.Mode = Mode;
            Result.DataType = DataType;
            base.Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilterDef Add(string TableName, string FieldName, string TitleKey, DataFieldType DataType)
        {
            return Add(Sys.FieldPath(TableName, FieldName), TitleKey, DataType);
        }
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilterDef AddLocator(string FieldPath, string TitleKey, string Locator, DataFieldType DataType)
        {
            SqlFilterDef Result = Add(FieldPath, TitleKey, DataType, SqlFilterMode.Locator);
            Result.Locator = Locator;
            return Result;
        }
 
        /// <summary>
        /// Sets the IsMultiChoise flag to all criterions according to specified Value.
        /// </summary>
        public void SetMultiChoise(bool Value)
        {
            foreach (var Item in this)
            {
                if (Bf.Member(Item.Mode, SqlFilterMode.EnumQuery | SqlFilterMode.EnumConst))
                    Item.EnumIsMultiChoise = Value;
            }
        }

        /// <summary>
        /// Throws exception if any of the items of this instance is not valid
        /// </summary>
        public void CheckDescriptors()
        {
            foreach (var Item in this)
                Item.CheckDescriptor();
        }
    }
}
