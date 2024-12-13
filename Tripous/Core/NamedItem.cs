namespace Tripous
{
    /// <summary>
    /// Represents an object with a Name property. A NamedItem can be an element
    /// in a NamedItems&lt;NamedItem&gt;. In that case the Name must be unique in the 
    /// collection of items.
    /// </summary>
    public class NamedItem : Assignable, INamedItem
    {
        /// <summary>
        /// Field
        /// </summary>
        protected string fName;

        /// <summary>
        /// Called when Name changes
        /// </summary>
        protected virtual void DoNameChanged()
        {
        }

        /// <summary>
        /// Gets the Name
        /// </summary>
        protected virtual string GetName()
        {
            return string.IsNullOrEmpty(fName) ? string.Empty : fName;
        }
        /// <summary>
        /// Sets the Name
        /// </summary>
        protected virtual void SetName(string value)
        {
            if (value != fName)
            {
                if (value == null)
                    throw new ArgumentNullException("NewName");

                if (fCollection is IUniqueNamesList)
                    (fCollection as IUniqueNamesList).CheckUniqueName(null, value);

                fName = value;

                DoNameChanged();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public NamedItem()
        {
        }

        /// <summary>
        /// Returns a string representation of this item.
        /// </summary>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            return base.ToString();
        }

        /// <summary>
        /// Gets or sets the Name property. The Name must be unique when the item is in a collection.
        /// </summary>
        [DefaultValue(""), Localizable(false)]
        public virtual string Name
        {
            get { return GetName(); }
            set
            {
                SetName(value);
                OnPropertyChanged("Name");
            }
        }

    }



}

