using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Tripous
{
    /// <summary>
    /// Provides the basic functionality for objects capable of performing 
    /// assignment and cloning. 
    ///
    /// Derived classes may provide overriden versions of the 
    /// DoClear(), DoAssign() methods.
    /// </summary>
    public class Assignable : MarshalByRefObject, IAssignable, IOwned, ISupportChange, ICollectionItem, INotifyPropertyChanged
    {
        /// <summary>
        /// The collection of this instance, used when this instance belongs to a collection
        /// </summary>
        protected IList fCollection;

        /* overridable methods */
        /// <summary>
        /// Initializes all members of this instance
        /// </summary>
        protected virtual void DoClear()
        {
        }
        /// <summary>
        /// If Source is not null then it assigns Source's properties to this instance properties.
        /// </summary>
        protected virtual void DoAssign(object Source)
        {
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Triggers the Changed event
        /// </summary>
        protected virtual void OnChanged(object Item)
        {
            if (Owner is ISupportChange)
                (Owner as ISupportChange).Change(Item);

            if ((Collection != Owner) && (Collection is ISupportChange))
                (Collection as ISupportChange).Change(Item);

            if (Changed != null)
                Changed(this, new ChangedEventArgs(Item));
        }
        /// <summary>
        /// Informs any subscriber for a change in a property
        /// </summary>
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        /* constructors */
        /// <summary>
        /// Constructor.
        /// </summary>
        public Assignable()
        {
        }
         

        /* public */
        /// <summary>
        /// Initializes all members of this instance
        /// </summary>
        public void Clear()
        {
            DoClear();
            Change(this);
        }
        /// <summary>
        /// Clears property values of this instance and then, if Source is not null,
        /// assigns Source's properties to this instance properties.
        /// <para>Example call</para>
        /// <code>Destination.Assign(Source);</code>
        /// </summary>
        public void Assign(object Source)
        {
            DoClear();

            if (Source != null)
            {
                DoAssign(Source);
            }

            Change(this);

        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public virtual object Clone()
        {
            Assignable Result = (Assignable)this.GetType().Create(new Type[] { }, new object[] { });
            Result.Assign(this);
            return Result;
        }
        /// <summary>
        /// Notifies about a change in a property or item.
        /// <para>Item could be this instance or an internal list element.</para>
        /// </summary>
        public void Change(object Item)
        {
            OnChanged(Item);
        }

        /* properties */
        /// <summary>
        /// The "owner" of this instance.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false), ReadOnly(true)]
        public virtual object Owner { get; set; }

        /// <summary>
        /// Gets or sets the Collection this object belongs to. It can be null.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false), ReadOnly(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList Collection
        {
            get { return fCollection; }
            set
            {
                if (fCollection != value)
                {
                    if ((fCollection != null) && (fCollection.Contains(this)))
                        fCollection.Remove(this);

                    fCollection = value;

                    if ((fCollection != null) && (!fCollection.Contains(this)))
                        fCollection.Add(this);
                }
            }
        }
        /// <summary>
        /// Returns the Owner of the Collection  this object belongs to. It can be null.
        /// <para>The Collection may be an IOwned and may have an Owner defined.</para>
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false), ReadOnly(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CollectionOwner
        {
            get
            {
                if (Collection is IOwned)
                    return (Collection as IOwned).Owner;
                return null;
            }
        }

        /* events */
        /// <summary>
        /// Occurs when properties of this instance change values.
        /// </summary>
        public event EventHandler<ChangedEventArgs> Changed;
        /// <summary>
        /// Enforced by the INotifyPropertyChanged implementation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
