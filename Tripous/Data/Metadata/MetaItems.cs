/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Linq;


namespace Tripous.Data.Metadata
{
    /// <summary>
    /// Represents a collection of items in the metadata sub-system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MetaItems<T> : DbItems<T>, IMetaNodeList, IMetaNode where T : class, INamedItem, new()
    {
        /* protected */
        /// <summary>
        /// Field
        /// </summary>
        protected bool fIsLoading;
        /// <summary>
        /// Field
        /// </summary>
        protected bool fIsLoaded;
        /// <summary>
        /// Field
        /// </summary>
        protected object fTag;
        /// <summary>
        /// Field
        /// </summary>
        protected string fDisplayText;
        /// <summary>
        /// Field
        /// </summary>
        protected MetaNodeKind fKind;


        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            IsLoaded = false;
        }

        /* overridables */
        /// <summary>
        /// Loads the metadata information 
        /// </summary>
        protected virtual void DoLoad()
        {
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaItems()
        {
        }


        /* methods */
        /// <summary>
        /// Loads the metadata information if it is not already loaded
        /// </summary>
        public void Load()
        {
            Lock();
            try
            {
                if (!IsLoaded && !fIsLoading)
                {
                    fIsLoading = true;
                    try
                    {

                        DoLoad();

                    }
                    finally
                    {
                        fIsLoading = false;
                    }

                    IsLoaded = true;
                }
            }
            finally
            {
                UnLock();
            }

        }
        /// <summary>
        /// Forces the loading of metadata information even if it is already loaded
        /// </summary>
        public void ReLoad()
        {
            this.Clear();
            Load();
        }

        /* properties */
        /// <summary>
        /// Gets the text this instance provides for display purposes
        /// </summary>
        public virtual string DisplayText { get { return fDisplayText; } }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public virtual MetaNodeKind Kind { get { return fKind; } }
        /// <summary>
        /// True while is loading
        /// </summary>
        public bool IsLoading
        {
            get
            {
                Lock();
                try
                {
                    return fIsLoading;
                }
                finally
                {
                    UnLock();
                }
            }
            protected set
            {
                Lock();
                try
                {
                    fIsLoading = value;
                }
                finally
                {
                    UnLock();
                }
            }

        }
        /// <summary>
        /// True if the metadata is alreade loaded
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                Lock();
                try
                {
                    return fIsLoaded;
                }
                finally
                {
                    UnLock();
                }
            }
            protected set
            {
                Lock();
                try
                {
                    fIsLoaded = value;
                }
                finally
                {
                    UnLock();
                }
            }

        }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag
        {
            get
            {
                Lock();
                try
                {
                    return fTag;
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
                    fTag = value;
                }
                finally
                {
                    UnLock();
                }
            }

        }
        /// <summary>
        /// Gets the node list
        /// </summary>
        public IMetaNode[] Nodes
        {
            get
            {
                Lock();
                try
                {
                    List<IMetaNode> List = new List<IMetaNode>();
                    foreach (IMetaNode Item in this)
                        List.Add(Item);
                    return List.ToArray();
                }
                finally
                {
                    UnLock();
                }

            }
        }
    }


}
