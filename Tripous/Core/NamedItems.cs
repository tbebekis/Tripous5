/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Tripous
{

    /// <summary>
    /// Represents a list of <see cref="INamedItem"/> objects.
    /// </summary>
    public class NamedItems<T> : OwnedCollection<T>, INamedItems<T> where T : class, INamedItem, new()
    {
        private bool uniqueNames = true;
        private bool useSafeAdd;

        void IUniqueNamesList.CheckUniqueName(object Item, string Name)
        {
            if (UniqueNames && Item is INamedItem)
            {
                this.CheckUniqueName(Item as INamedItem, Name);
            }
        }

        /// <summary>
        /// Normalizes a specified name.
        /// <para>Some inheritors may need to replace or delete invalid characters.</para>
        /// </summary>
        protected virtual string NormalizeName(string Name)
        {
            return Name;
        }

        /// <summary>
        /// It is called by the <see cref="System.Collections.ObjectModel.Collection&lt;T&gt;.Insert"/> method to check the validitiy of the Item before inserting.
        /// </summary>
        protected override void CheckInsert(int Index, T Item)
        {
            base.CheckInsert(Index, Item);

            if (UniqueNames)
                CheckUniqueName(Item, string.Empty);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NamedItems()
        {
        }


        /// <summary>
        /// Creates an Item with Name and adds it to the list.
        /// </summary>
        public virtual T Add(string Name)
        {
            T Result;
            if (UniqueNames && UseSafeAdd)
            {
                Result = Find(Name);
                if (Result != null)
                    return Result;
            }

            CheckUniqueName(null, Name);
            Result = (T)typeof(T).Create();
            (Result as INamedItem).Name = Name;
            base.Add(Result);
            return Result;
        }
        /// <summary>
        /// Removes an Item with Name from list, if any
        /// </summary>
        public virtual void Remove(string Name)
        {
            Remove(Find(Name));
        }
        /// <summary>
        /// Returns the index of an Item with Name in the list.
        /// </summary>
        public virtual int IndexOf(string Name)
        {
            return IndexOf(Find(Name));
        }
        /// <summary>
        /// Returns true if an Item with Name exists in list.
        /// </summary>
        public virtual bool Contains(string Name)
        {
            return Find(Name) != null;
        }
        /// <summary>
        /// Finds an Item by Name, if any and if T has a Name property. Else returns null.
        /// </summary>
        public virtual T Find(string Name)
        {
            Name = NormalizeName(Name);

            foreach (T Item in this)
                if (string.Compare(Name, (Item as INamedItem).Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return Item;

            return null;
        }
        /// <summary>
        /// Throws an exception of Name is not unique in the list.
        /// </summary>
        public virtual void CheckUniqueName(INamedItem Item, string Name)
        {
            if (UniqueNames)
            {
                INamedItem Item2;
                string ErrorText = Res.GS("E_ItemWithNameAlreadyInList", "An item with this name already exists in list: {0} ");

                /* is a new insert, or after an add */
                if (Item != null)
                {
                    Item2 = this.Find(Item.Name);
                    if ((Item2 != null) && (Item2 != Item))
                        Sys.Error(ErrorText, Item.Name);
                }
                /* is a name change of an existing item, or before an add */
                else
                {
                    if (Contains(Name))
                        Sys.Error(ErrorText, Name);
                }
            }
        }

 





        /// <summary>
        /// String indexer.
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        public virtual T this[string Name]
        {
            get
            {
                T Result = Find(Name);
                if (Result == null)
                    Sys.Error("{0} not found: {1}", typeof(T).Name, Name);
                return Result;
            }
        }
        /// <summary>
        /// When the item type T has a Name property, indicates whether items in the list should have or not unique names.
        /// <para>Defaults to true</para>
        /// </summary>
        public virtual bool UniqueNames
        {
            get { return uniqueNames; }
            set { uniqueNames = value; }
        }
        /// <summary>
        /// When true, then when adding using the Add(string Name), no new item is added, if there is already an item with Name .
        /// <para>Defaults to false.</para>
        /// </summary>
        public virtual bool UseSafeAdd
        {
            get { return useSafeAdd; }
            set { useSafeAdd = value; }
        }

        bool IUniqueNamesList.UniqueNames { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        T INamedItems<T>.this[string Name] => throw new NotImplementedException();
    }


}
