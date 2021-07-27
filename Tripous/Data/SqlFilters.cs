/*--------------------------------------------------------------------------------------        
                            Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Data
{


    /// <summary>
    /// A list of <see cref="SqlFilter"/> items.
    /// <para>A list of criterion fields is used in producing the "user where" clause of a SELECT statement.</para>
    /// </summary>
    public class SqlFilters : OwnedCollection<SqlFilter>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlFilters()
        {
        }

        /* public */
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilter Add(string TableName, string FieldName, string TitleKey, SimpleType DataType, SqlFilterMode Mode = SqlFilterMode.Simple)
        {
            return Add(Sys.FieldPath(TableName, FieldName), TitleKey, DataType, Mode);
        }
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilter Add(string FieldPath, string TitleKey, SimpleType DataType, SqlFilterMode Mode = SqlFilterMode.Simple)
        {
            SqlFilter Result = new SqlFilter();
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
        public SqlFilter Add(string TableName, string FieldName, string TitleKey, SimpleType DataType)
        {
            return Add(Sys.FieldPath(TableName, FieldName), TitleKey, DataType);
        }
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilter Add(string TableName, string FieldName, string TitleKey, Type DataType)
        {
            return Add(Sys.FieldPath(TableName, FieldName), TitleKey, Simple.SimpleTypeOf(DataType));
        }
        /// <summary>
        /// Adds a new criterion descriptor to the list
        /// </summary>
        public SqlFilter AddLocator(string FieldPath, string TitleKey, string Locator, SimpleType DataType)
        {
            SqlFilter Result = Add(FieldPath, TitleKey, DataType, SqlFilterMode.Locator);
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
                    Item.Enum.IsMultiChoise = Value;
            }
        }
    }
}
