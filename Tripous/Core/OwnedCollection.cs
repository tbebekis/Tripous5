/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Tripous
{
    /// <summary>
    /// A generic collection. It can provide notifications when an item is inserted or deleted and when
    /// the list is changed.
    /// </summary>
    public class OwnedCollection<T> : Collection<T>, IAssignable, IOwned, ISupportChange, ISupportChanging where T : class, new()
    {
        /// <summary>
        /// Field
        /// </summary>
        private int changing;
        /// <summary>
        /// Field
        /// </summary>
        protected object fOwner;

        /* overrides */
        /// <summary>
        /// Removes all elements from the list
        /// </summary>
        protected override void ClearItems()
        {
            Lock();
            try
            {
                Changing = true;
                try
                {
                    T[] Items = new T[Count];
                    CopyTo(Items, 0);

                    base.ClearItems();

                    foreach (T Item in Items)
                    {
                        if (Item is ICollectionItem)
                            (Item as ICollectionItem).Collection = null;

                        DisposeItem(Item);
                    }
                }
                finally
                {
                    Changing = false;
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Override
        /// </summary>
        protected override void SetItem(int Index, T Item)
        {

            Lock();
            try
            {
                T OldItem = this[Index];

                base.SetItem(Index, Item);

                if ((Item != null) && !Contains(Item))
                {
                    if ((Item is ICollectionItem) && ((Item as ICollectionItem).Collection != this))
                        (Item as ICollectionItem).Collection = this;
                }

                if (OldItem is ICollectionItem)
                    (OldItem as ICollectionItem).Collection = null;

                DisposeItem(OldItem);
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Inserts the specified item in the list at the specified index.
        /// </summary>
        protected override void InsertItem(int Index, T Item)
        {

            Lock();
            try
            {
                if (!Contains(Item))
                {
                    try
                    {
                        if (Item == null)
                            throw new ArgumentNullException("Item");

                        if (Contains(Item))
                            throw new ApplicationException("Item already in list");

                        CheckInsert(Count, Item);

                        OnInsertBefore(Index, Item);

                        base.InsertItem(Index, Item);

                        if ((Item is ICollectionItem) && ((Item as ICollectionItem).Collection != this))
                            (Item as ICollectionItem).Collection = this;

                        OnInsertAfter(Index, Item);

                        Change(Item as object);
                    }
                    catch //(Exception ex)
                    {
                        //CancelNew(Index);
                        //if (!isBound)
                        throw;
                        //else
                        //    Sys.ErrorBox(ex);
                    }
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Removes the item at the specified Index.
        /// </summary>
        protected override void RemoveItem(int Index)
        {

            Lock();
            try
            {
                if ((Index >= 0) && (Index <= Count - 1))
                {
                    T Item = this[Index];

                    OnRemoveBefore(Index, Item);
                    base.RemoveItem(Index);

                    if (Item is ICollectionItem)
                        (Item as ICollectionItem).Collection = null;

                    OnRemoveAfter(Index, Item);

                    Change(Item as object);

                    DisposeItem(Item);
                }
            }
            finally
            {
                UnLock();
            }

        }

        /* overrridables */
        /// <summary>
        /// For thread synchronization
        /// </summary>
        protected virtual void Lock()
        {
        }
        /// <summary>
        /// For thread synchronization
        /// </summary>
        protected virtual void UnLock()
        {
        }
        /// <summary>
        /// Clears property values of this instance and then, if Source is not null,
        /// assigns Source's properties to this instance properties.
        /// <para>Example call</para>
        /// <code>Destination.Assign(Source);</code>
        /// </summary>
        protected virtual void DoAssign(object Source)
        {

            Lock();
            try
            {
                if (Source is OwnedCollection<T>)
                {
                    Sys.AssignObject(Source, this); 
                }
            }
            finally
            {
                UnLock();
            }
        }
        /// <summary>
        /// Triggers the Changed event
        /// </summary>
        protected virtual void OnChanged(object Item)
        {
            Lock();
            try
            {
                if (changing == 0)
                {
                    if (fOwner is ISupportChange)
                        (fOwner as ISupportChange).Change(Item);

                    if (Changed != null)
                        Changed(this, new ChangedEventArgs(Item));
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Returns true if a specified item can be disposed
        /// </summary>
        protected virtual bool CanDispose(T Item)
        {
            if (Sys.HasProperty(Item, "IsDisposed"))
            {
                object V = Sys.GetProperty(Item, "IsDisposed");
                if (V != null)
                    return !Convert.ToBoolean(V);
            }

            return Item is IDisposable;
        }
        /// <summary>
        /// Disposes Item if is an IDisposable
        /// </summary>
        protected virtual void DisposeItem(T Item)
        {
            if (CanDispose(Item))
            {
                (Item as IDisposable).Dispose();
            }
        }
        /// <summary>
        /// It is called by the <see cref="Collection&lt;T&gt;.Insert"/> method to check the validitiy of the Item before inserting.
        /// </summary>
        protected virtual void CheckInsert(int Index, T Item)
        {
        }
        /// <summary>
        /// Calls the respective event
        /// </summary>
        protected virtual void OnInsertBefore(int Index, T Item)
        {
            if (BeforeInsert != null)
                BeforeInsert(this, new ChangedEventArgs<T>(Item, Index));
        }
        /// <summary>
        /// Calls the respective event
        /// </summary>
        protected virtual void OnInsertAfter(int Index, T Item)
        {
            if (AfterInsert != null)
                AfterInsert(this, new ChangedEventArgs<T>(Item, Index));
        }
        /// <summary>
        /// Calls the respective event
        /// </summary>
        protected virtual void OnRemoveBefore(int Index, T Item)
        {
            if (BeforeRemove != null)
                BeforeRemove(this, new ChangedEventArgs<T>(Item, Index));
        }
        /// <summary>
        /// Calls the respective event
        /// </summary>
        protected virtual void OnRemoveAfter(int Index, T Item)
        {
            if (AfterRemove != null)
                AfterRemove(this, new ChangedEventArgs<T>(Item, Index));
        }
        /// <summary>
        /// Calls the respective event
        /// </summary>
        protected virtual void OnExtractBefore(int Index, T Item)
        {
            if (BeforeExtract != null)
                BeforeExtract(this, new ChangedEventArgs<T>(Item, Index));
        }
        /// <summary>
        /// Calls the respective event
        /// </summary>
        protected virtual void OnExtractAfter(int Index, T Item)
        {
            if (AfterExtract != null)
                AfterExtract(this, new ChangedEventArgs<T>(Item, Index));
        }

        /* constructors */
        /// <summary>
        /// Constructor.
        /// </summary>
        public OwnedCollection()
        {
        }

 
        /// <summary>
        /// Helper design time method 
        /// </summary>
        public void DesignTimeClearItems(System.ComponentModel.Design.IDesignerHost DesignerHost)
        {

            Lock();
            try
            {
                Changing = true;
                try
                {
                    /* extract and destroy */
                    IList<T> List = ExtractAll();

                    foreach (T OldItem in List)
                    {
                        if (OldItem is IComponent)
                            DesignerHost.DestroyComponent(OldItem as IComponent);
                    }

                }
                finally
                {
                    Changing = false;
                }
            }
            finally
            {
                UnLock();
            }


        }
        /// <summary>
        ///  Helper design time method 
        /// </summary>
        public void DesignTimeAssign(object SourceCollection, System.ComponentModel.Design.IDesignerHost DesignerHost)
        {
            Lock();
            try
            {
                Changing = true;
                try
                {
                    /* extract and destroy */
                    DesignTimeClearItems(DesignerHost);

                    if (SourceCollection is OwnedCollection<T>)
                    {
                        /* create the new items */
                        T Item;
                        foreach (T SourceItem in (SourceCollection as OwnedCollection<T>))
                        {
                            if (SourceItem is IComponent)
                                Item = DesignerHost.CreateComponent(typeof(T)) as T;
                            else
                                Item = typeof(T).Create() as T;

                            if (Item is IAssignable)
                                (Item as IAssignable).Assign(SourceItem);
                            else
                                Sys.AssignObject(SourceItem, Item); // Assignable.AssignInstance(SourceItem as object, Item as object);

                            Add(Item);
                        }
                    }

                }
                finally
                {
                    Changing = false;
                }
            }
            finally
            {
                UnLock();
            }


        }
 

        /* public methods */
        /// <summary>
        /// Clears property values of this instance and then, if Source is not null,
        /// assigns Source's properties to this instance properties.
        /// <para>Example call</para>
        /// <code>Destination.Assign(Source);</code>
        /// </summary>
        public void Assign(object Source)
        {

            Lock();
            try
            {
                Changing = true;
                try
                {
                    Clear();
                    if (Source is OwnedCollection<T>)
                    {
                        DoAssign(Source);
                    }
                }
                finally
                {
                    Changing = false;
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            OwnedCollection<T> Result = (OwnedCollection<T>)this.GetType().Create();
            Result.Assign(this);
            return Result;
        }
        /// <summary>
        /// Adds an array of items to the list.
        /// </summary>
        public void AddRange(T[] A)
        {
            Lock();
            try
            {
                if (A != null)
                {
                    Changing = true;
                    try
                    {
                        foreach (T Item in A)
                            Add(Item);
                    }
                    finally
                    {
                        Changing = false;
                    }
                }
            }
            finally
            {
                UnLock();
            }


        }
        /// <summary>
        /// Notifies about a change in a property or item.
        /// <para>Item could be this instance or an internal list element.</para>
        /// </summary>
        public void Change(object Item)
        {
            Lock();
            try
            {
                if (changing == 0)
                {
                    OnChanged(Item);
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Extracts item at Index. The item no longer belongs to this list.
        /// </summary>
        public T Extract(int Index)
        {
            return Extract(this[Index]);
        }
        /// <summary>
        /// Extracts Item from list. 
        /// </summary>
        public T Extract(T Item)
        {

            Lock();
            try
            {
                if (Contains(Item))
                {
                    int Index = IndexOf(Item);
                    OnExtractBefore(Index, Item);

                    base.SetItem(Index, null as T);
                    base.RemoveItem(Index);
                    if (Item is ICollectionItem)
                        (Item as ICollectionItem).Collection = null;
                    OnExtractAfter(Index, Item);
                    Change(null);
                    return Item;
                }

                return null;
            }
            finally
            {
                UnLock();
            }


        }
        /// <summary>
        /// Extracts all items from list.
        /// </summary>
        public List<T> ExtractAll()
        {

            Lock();
            try
            {
                List<T> Result = new List<T>();
                foreach (T Item in this)
                    Result.Add(Item);

                Changing = true;
                try
                {
                    foreach (T Item in Result)
                        Extract(Item);
                }
                finally
                {
                    Changing = false;
                }


                return Result;
            }
            finally
            {
                UnLock();
            }




        }
        /// <summary>
        /// Returns an array with all the items. Suitable for passing the array to list and combo boxes.
        /// </summary>
        public T[] AsArray()
        {
            return ToArray();
        }
        /// <summary>
        /// Returns an array with all the items. Suitable for passing the array to list and combo boxes.
        /// </summary>
        public T[] ToArray()
        {
            Lock();
            try
            {
                T[] Result = new T[Count];
                for (int i = 0; i < Count; i++)
                    Result[i] = this[i];
                return Result;
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Exchanges source to source positions.
        /// </summary>
        public void Exchange(int SourceIndex, int DestIndex)
        {
            Lock();
            try
            {
                if ((SourceIndex >= 0) && (SourceIndex <= Count - 1) && (DestIndex >= 0) && (DestIndex <= Count - 1))
                {
                    T Source = this[SourceIndex];
                    T Dest = this[DestIndex];

                    base.SetItem(SourceIndex, Dest);
                    base.SetItem(DestIndex, Source);
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Exchanges source to source positions.
        /// </summary>
        public void Exchange(T Source, T Dest)
        {
            Exchange(IndexOf(Source), IndexOf(Dest));
        }
        

        /// <summary>
        /// Saves this instance to an xml file
        /// </summary>
        public virtual void SaveToFile(string FileName)
        {
            Lock();
            try
            {
                Xml.SaveToFile(this, FileName);
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Loads this instance from an xml file.
        /// </summary>
        public virtual void LoadFromFile(string FileName)
        {
            Lock();
            try
            {
                Xml.LoadFromFile(this, FileName);
            }
            finally
            {
                UnLock();
            }

        }


        /* properties */
        /// <summary>
        /// The "owner" of this instance.
        /// </summary> 
        [Browsable(false)]
        [ReadOnly(true)]
        public object Owner
        {
            get
            {
                Lock();
                try
                {
                    return fOwner;
                }
                finally
                {
                    UnLock();
                }

            }
            set
            {
                Lock();
                try
                {
                    fOwner = value;
                }
                finally
                {
                    UnLock();
                }

            }
        }
        /// <summary>
        /// Gets or sets a boolean value indicating whether the object is in a changing operation.
        /// </summary>
        [Browsable(false)]
        [ReadOnly(true)]
        public bool Changing
        {
            get
            {
                Lock();
                try
                {
                    return changing > 0;
                }
                finally
                {
                    UnLock();
                }

            }
            set
            {

                Lock();
                try
                {
                    if (value)
                        changing++;
                    else
                        changing--;

                    Change(null);
                }
                finally
                {
                    UnLock();
                }

            }
        }


        /* events */
        /// <summary>
        /// Occurs before insert
        /// </summary>
        public event EventHandler<ChangedEventArgs<T>> BeforeInsert;
        /// <summary>
        /// Occurs after insert
        /// </summary>
        public event EventHandler<ChangedEventArgs<T>> AfterInsert;
        /// <summary>
        /// Occurs before remove
        /// </summary>
        public event EventHandler<ChangedEventArgs<T>> BeforeRemove;
        /// <summary>
        /// Occurs after remove
        /// </summary>
        public event EventHandler<ChangedEventArgs<T>> AfterRemove;
        /// <summary>
        /// Occurs before extract
        /// </summary>
        public event EventHandler<ChangedEventArgs<T>> BeforeExtract;
        /// <summary>
        /// Occurs after extract
        /// </summary>
        public event EventHandler<ChangedEventArgs<T>> AfterExtract;
        /// <summary>
        /// Occurs when properties of this instance change values.
        /// </summary>
        public event EventHandler<ChangedEventArgs> Changed;
    }






}
