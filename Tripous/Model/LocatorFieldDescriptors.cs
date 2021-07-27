/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;

namespace Tripous.Model
{

    /// <summary>
    /// A list of <see cref="LocatorFieldDescriptor"/> descriptors.
    /// </summary>
    public class LocatorFieldDescriptors : OwnedCollection<LocatorFieldDescriptor>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocatorFieldDescriptors()
        {
        }

        /// <summary>
        /// Adds a field to the list.
        /// </summary>
        public LocatorFieldDescriptor Add(SimpleType DataType, string DataField, string ListField, string ListFieldAlias, string ListTableName, string TitleKey, bool Searchable, bool DataVisible, bool ListVisible)
        {
            LocatorFieldDescriptor Result = new LocatorFieldDescriptor();
            Result.DataType = DataType;
            Result.DataField = DataField;
            Result.ListField = ListField;
            Result.ListFieldAlias = ListFieldAlias;
            Result.ListTableName = ListTableName;
            Result.TitleKey = TitleKey;
            Result.Searchable = Searchable;
            Result.DataVisible = DataVisible;
            Result.ListVisible = ListVisible;
            Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds a field to the list.
        /// </summary>
        public LocatorFieldDescriptor Add(SimpleType DataType, string DataField, string ListField, string ListTableName, string TitleKey)
        {
            bool Searchable = Bf.Member(DataType, SimpleType.String | SimpleType.Date);
            bool DataVisible = Searchable || !ListField.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase);

            return Add(DataType, DataField, ListField, ListField, ListTableName, TitleKey, Searchable, DataVisible, DataVisible);
        }
        /// <summary>
        /// Adds a field to the list.
        /// </summary>
        public LocatorFieldDescriptor Add(SimpleType DataType, string DataField, string ListField, string ListTableName)
        {
            return Add(DataType, DataField, ListField, ListTableName, ListField);
        }

        /// <summary>
        /// Finds a field descriptor by <see cref="LocatorFieldDescriptor.ListFieldAlias"/>, if any, else null.
        /// </summary>
        public LocatorFieldDescriptor Find(string ListFieldAlias)
        {
            if (!string.IsNullOrEmpty(ListFieldAlias))
                foreach (LocatorFieldDescriptor Item in this)
                    if (string.Compare(ListFieldAlias, Item.ListFieldAlias, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return Item;

            return null;
        }
        /// <summary>
        /// Finds a field descriptor by <see cref="LocatorFieldDescriptor.DataField"/>, if any, else null.
        /// </summary>
        public LocatorFieldDescriptor FindByDataField(string DataField)
        {
            if (!string.IsNullOrEmpty(DataField))
                foreach (LocatorFieldDescriptor Item in this)
                    if (string.Compare(DataField, Item.DataField, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return Item;

            return null;
        }
    }
}
