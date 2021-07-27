/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;


namespace Tripous.Data.Metadata
{
    /// <summary>
    /// Represents schema information of a data type used by the database
    /// </summary>
    public class MetaType : NamedItem, IMetaNode, IMetaFullText
    {
        /* private */
        private string netType;
        private string metaTag;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaType()
        {
        }

        /* properties */
        /// <summary>
        /// Gets the owner metastore
        /// </summary>
        public Metastore Store { get { return CollectionOwner as Metastore; } }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Type; } }
        /// <summary>
        /// Gets the text this instance provides for display purposes
        /// </summary>
        public string DisplayText 
        { 
            get 
            {
                if (!string.IsNullOrEmpty(NetType))
                    return string.Format("{0} ({1})", Name, NetType);
                
                return Name; 
            
            } 
        
        }
        /// <summary>
        /// Gets the full text
        /// </summary>
        public string FullText
        {
            get
            {

                StringBuilder SB = new StringBuilder();
                SB.AppendLine(string.Format("{0}: {1}", Kind.ToString(), Name));
                if (!string.IsNullOrEmpty(NetType))
                    SB.AppendLine(string.Format("NetType: {0}", NetType));

                return SB.ToString();

            }
        }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the associated .Net data type, ie. System.String, System.Double etc.
        /// </summary>
        public string NetType
        {
            get { return string.IsNullOrEmpty(netType) ? string.Empty : netType; }
            set { netType = value; }
        }
        /// <summary>
        /// Gets or sets a user defined tag, used internally
        /// </summary>
        public string MetaTag
        {
            get { return string.IsNullOrEmpty(metaTag) ? string.Empty : metaTag; }
            set { metaTag = value; }
        }
        /// <summary>
        /// Gets a true value if this data type is a string one, ie varchar, nvarchar etc.
        /// </summary>
        public bool IsString
        {
            get
            {
                /*
                Type T = null;
                try
                {
                    T = Type.GetType(NetType);
                }
                catch  
                {
                }

                if (T != null)
                    return Tripous.Simple.IsString(T);

                return false;
                */

                return Name.ContainsText("varchar");
            }
        }

       
       
    }
}
