﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.ComponentModel;


namespace Tripous
{
    /// <summary>
    /// A Descriptor is a class that describes other business classes.
    /// </summary>
    public class Descriptor : NamedItem 
    {
        /// <summary>
        /// Field
        /// </summary>
        protected string fAlias;
        /// <summary>
        /// Field
        /// </summary>
        protected string fTitle;
        /// <summary>
        /// Field
        /// </summary>
        protected string fTitleKey;

        /// <summary>
        /// Returns the Alias.
        /// </summary>
        protected virtual string GetAlias()
        {
            return string.IsNullOrEmpty(fAlias) ? Name : fAlias;
        }
        /// <summary>
        /// Returns an error message
        /// </summary>
        private string E_NotFullyDefined { get { return Res.GS("E_DescriptorItemNotFullyDefined", "A descriptor item is not fully defined: {0}"); } }



        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public Descriptor()
        {
        }


        /* static */
        /// <summary>
        /// A static helper method for finding a Descriptor by its Alias.
        /// </summary>
        static public Descriptor FindByAlias(string Alias, IList List)
        {
            foreach (Descriptor Item in List)
                if (string.Compare(Alias, Item.Alias, true) == 0)
                    return Item;

            return null;
        }

        /* public */
        /// <summary>
        /// Throws an exception if this item has no name defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            if (string.IsNullOrEmpty(Name))
                Sys.Error(E_NotFullyDefined, this.GetType().Name + ".Name");
        }
        /// <summary>
        /// Throws an exception. It should be used from inside the CheckDescriptor() method,
        /// passing as PropName the name of a not-defined property
        /// </summary>
        public void NotFullyDefinedError(string PropName)
        {

            string Text = this.Collection is INamedItem ? (this.Collection as INamedItem).Name + "." + Name : Name;


            if (!string.IsNullOrEmpty(PropName))
                Text += " (" + PropName + ")";

            Text += Environment.NewLine + "Item Type: " + this.GetType().FullName;


            Sys.Error(E_NotFullyDefined, Text);
        }

        /* properties */
        /// <summary>
        /// Gets or sets tha Alias of this descriptor.
        /// <para>For a field Descriptor it may be as CUSTOMER__NAME, while for a CUSTOMER table it may be CUST or C.</para>
        /// </summary>
        [DefaultValue(""), Localizable(false)]
        public virtual string Alias
        {
            get { return GetAlias(); }
            set
            {
                fAlias = value;
                OnPropertyChanged("Alias");
            }
        }
        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        [DefaultValue(""), Localizable(true)]
        public virtual string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(fTitle))
                    return fTitle;

                if (!string.IsNullOrWhiteSpace(TitleKey))
                    return Res.GS(TitleKey, TitleKey);

                return Name;
            }
            set
            {
                fTitle = value;
                OnPropertyChanged("Title");
            }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        [DefaultValue(""), Localizable(false)]
        public virtual string TitleKey
        {
            get { return string.IsNullOrWhiteSpace(fTitleKey) ? Name : fTitleKey; }
            set
            {
                fTitleKey = value;
                OnPropertyChanged("TitleKey");
            }

        }

        /// <summary>
        /// Gets or sets the descriptor mode. See DescriptorMode
        /// </summary>             
        [Newtonsoft.Json.JsonIgnore]
        public virtual DescriptorMode DescriptorMode { get; set; }
        /// <summary>
        /// A descriptor registered by Tripous system code
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual bool IsSystem { get { return DescriptorMode == DescriptorMode.System; } }
        /// <summary>
        /// A descriptor registered by application code
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual bool IsApp { get { return DescriptorMode == DescriptorMode.Application; } }
        /// <summary>
        /// A descriptor created by user customization, based on an
        /// existing application descriptor. The new descriptor
        /// replaces the one it is based on.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual bool IsReplica { get { return DescriptorMode == DescriptorMode.Replica; } }
        /// <summary>
        /// A tottaly new descriptor created by user customization
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual bool IsCustom { get { return DescriptorMode == DescriptorMode.Custom; } }

        /// <summary>
        /// When true the descriptor is in design mode in a a design form or something
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool DesignMode { get; set; }

    }



}
