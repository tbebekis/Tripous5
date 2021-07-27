/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Tripous
{


    /// <summary>
    /// A classic Node
    /// </summary>
    public class Node : IAssignable, IList, ICollectionItem
    {
        /// <summary>
        /// Field
        /// </summary>
        protected List<Node> list = new List<Node>();
        /// <summary>
        /// Field
        /// </summary>
        protected Node fParent = null;
        /// <summary>
        /// Field
        /// </summary>
        protected IList fCollection;


        #region Explicit interface implementations
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        void ICollection.CopyTo(Array Array, int Index)
        {
            (list as IList).CopyTo(Array, Index);
        }

        int IList.Add(object Value)
        {
            if (Value is Node)
                Add(Value as Node);
            return list.Count;
        }

        bool IList.Contains(object Value)
        {
            if (Value is Node)
                return Contains(Value as Node);
            return false;
        }

        int IList.IndexOf(object Value)
        {
            if (Value is Node)
                return IndexOf(Value as Node);
            return -1;
        }

        void IList.Insert(int Index, object Value)
        {
            if (Value is Node)
                Insert(Index, Value as Node);
        }

        void IList.Remove(object Value)
        {
            if (Value is Node)
                Remove(Value as Node);
        }

        object IList.this[int Index]
        {
            get
            {
                return list[Index] as object;
            }
            set
            {
                if (value is Node)
                    list[Index] = value as Node;
            }
        }

        #endregion

        /// <summary>
        /// Called when list changes (inserts, additions, clear).
        /// </summary>
        protected virtual void OnChanged()
        {
        }
        /// <summary>
        /// Called before inserting an item
        /// </summary>
        protected virtual void OnBeforeInsert(int Index, Node Node)
        {
        }
        /// <summary>
        /// Called after inserting an item
        /// </summary>
        protected virtual void OnAfterInsert(Node Node)
        {
        }
        /// <summary>
        /// Called before removing an item
        /// </summary>
        protected virtual void OnBeforeRemove(Node Node)
        {
        }
        /// <summary>
        /// Called after removing an item
        /// </summary>
        protected virtual void OnAfterRemove(Node Node)
        {
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public Node()
        {
        }

        /* public */
        /// <summary>
        /// Initializes all members of this instance
        /// </summary>
        public virtual void Clear()
        {
            if (list.Count > 0)
            {
                list.Clear();
                OnChanged();
            }
        }
        /// <summary>
        /// Assigns Source properties to this instance
        /// </summary>
        /// <param name="Source"></param>
        public virtual void Assign(object Source)
        {
            Clear();

            if (Source is Node)
            {
                foreach (Node Node in (Source as Node))
                    Add((Node)Node.Clone());
            }

            if (Assigned != null)
                Assigned(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public virtual object Clone()
        {
            Node Result = (Node)this.GetType().Create(new Type[] { }, new object[] { });
            Result.Assign(this);
            return Result;
        }

        /// <summary>
        /// Adds a Node
        /// </summary>
        public Node Add()
        {
            Node Result = (Node)this.GetType().Create();
            Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds Node and returns the current number of Nodes in the list
        /// </summary>
        public virtual int Add(Node Node)
        {
            Insert(list.Count, Node);
            return list.Count;
        }
        /// <summary>
        /// Inserts a Node at Index
        /// </summary>
        public Node Insert(int Index)
        {
            Node Result = (Node)this.GetType().Create();
            Insert(Index, Result);
            return Result;
        }
        /// <summary>
        /// Inserts Node at Index
        /// </summary>
        public virtual void Insert(int Index, Node Node)
        {
            if (list.Contains(Node))
                Sys.Error("Can not insert Node. Node already contained in list.");

            OnBeforeInsert(Index, Node);
            list.Insert(Index, Node);
            Node.fParent = this;
            OnAfterInsert(Node);
            OnChanged();
        }
        /// <summary>
        /// Removes the node at the Index
        /// </summary>
        public void RemoveAt(int Index)
        {
            list.RemoveAt(Index);
        }
        /// <summary>
        /// Removes the Node
        /// </summary>
        public void Remove(Node Node)
        {
            list.Remove(Node);
        }
        /// <summary>
        /// Returns true if the Node is in the list
        /// </summary>
        public bool Contains(Node Node)
        {
            return list.Contains(Node);
        }
        /// <summary>
        /// Returns true if the Node contained in the tree that
        /// has this instance as parent.
        /// </summary>
        public bool TreeContains(Node Node)
        {
            if (list.Contains(Node))
                return true;

            foreach (Node child in list)
                if (child.TreeContains(Node))
                    return true;

            return false;
        }
        /// <summary>
        /// Returns the index of the Node in the list.
        /// </summary>
        public int IndexOf(Node Node)
        {
            return list.IndexOf(Node);
        }

        /// <summary>
        /// Returns the first child, if any, else null.
        /// </summary>
        public Node FirstChild()
        {
            if (list.Count > 0)
                return list[0];
            return null;
        }
        /// <summary>
        /// Returns the next child after Node, if any, else null.
        /// </summary>
        public Node NextChild(Node Node)
        {
            int Index = list.IndexOf(Node);

            if ((Index != -1) && ((Index + 1 >= 0) && (Index + 1 <= list.Count - 1)))
                return list[Index + 1];
            return null;
        }
        /// <summary>
        /// Returns the previous child before Node, if any, else null.
        /// </summary>
        public Node PrevChild(Node Node)
        {
            int Index = list.IndexOf(Node);

            if ((Index != -1) && ((Index - 1 >= 0) && (Index - 1 <= list.Count - 1)))
                return list[Index - 1];
            return null;
        }
        /// <summary>
        /// Returns the last child, if any, else null.
        /// </summary>
        public Node LastChild()
        {
            if (list.Count > 0)
                return list[list.Count - 1];
            return null;
        }
        /// <summary>
        /// Returns the next sibling Node. If Parent is null, null is returned.
        /// </summary>
        /// <returns></returns>
        public Node NextSibling()
        {
            if (fParent != null)
                return fParent.NextChild(this);
            return null;
        }
        /// <summary>
        /// Returns the previous sibling Node. If Parent is null, null is returned.
        /// </summary>
        public Node PrevSibling()
        {
            if (fParent != null)
                return fParent.PrevChild(this);
            return null;
        }

        /* properties */
        /// <summary>
        /// Gets a value indicating whether access to this collection is synchronized (thread safe).
        /// </summary>
        public virtual bool IsSynchronized
        {
            get { return false; }
        }
        /// <summary>
        /// Gets an object that can be used to synchronize access to this collection
        /// </summary>
        [JsonIgnore]
        public virtual object SyncRoot
        {
            get { return (list as ICollection).SyncRoot; }
        }
        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }
        /// <summary>
        /// Always false. The collection is not fixed.
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }
        /// <summary>
        /// Always false. The collection is not read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Indexer
        /// </summary>
        [JsonIgnore]
        public Node this[int Index] { get { return list[Index]; } }
        /// <summary>
        /// Returns the root of the tree this Node may belong to.
        /// </summary>
        [JsonIgnore]
        public Node Root
        {
            get
            {
                Node Current = this;
                Node CurrentParent = fParent;

                while (CurrentParent != null)
                {
                    Current = CurrentParent;
                    CurrentParent = Current.fParent;
                }

                return Current;
            }
        }
        /// <summary>
        /// Returns true if this Node is the root in a tree.
        /// </summary>
        public bool IsRoot { get { return (Root == this) || (Root == null); } }
        /// <summary>
        /// Returns the parent of this Node, if any, else null.
        /// </summary>
        [JsonIgnore]
        public Node Parent { get { return fParent; } }
        /// <summary>
        /// Returns true if this Node has child nodes.
        /// </summary>
        public bool HasChildNodes { get { return list.Count > 0; } }
        /// <summary>
        /// Returns the level of this Node. A root node has level 0, its children have level 1 and so on.
        /// </summary>
        public int Level { get { return fParent == null ? 0 : fParent.Level + 1; } }
        /// <summary>
        /// Returns the index of this Node in the list of its parent, if any, else -1
        /// </summary>
        public int Index { get { return fParent == null ? -1 : fParent.list.IndexOf(this); } }
        /// <summary>
        /// Returns the total number of Nodes of this node and its child nodes.
        /// </summary>
        public int TotalCount
        {
            get
            {
                int Result = list.Count;
                foreach (Node Child in list)
                    Result += Child.TotalCount;
                return Result;
            }
        }
        /*
        /// <summary>
        /// Gets or sets the "owner" of this instance.
        /// </summary>
        [Persistent(PersistAs.Ignore)]
        public object Owner
        {
            get { return owner == null ? Collection : owner; }
            set { owner = value; }
        }*/
        /// <summary>
        /// Gets or sets the collection this item belongs to.
        /// </summary>
        [JsonIgnore]
        public IList Collection
        {
            get { return fCollection == null ? Parent : fCollection; }
            set { fCollection = value; }
        }

        /// <summary>
        /// Occurs after a successful call to Assign()
        /// </summary>
        public event EventHandler Assigned;
    }
}
