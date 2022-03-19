/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
//using System.Drawing;
using System.Globalization;

namespace Tripous
{
    /// <summary>
    /// Represents an object that provides resources such as string and images.
    /// <para>This is the base implementation.</para>
    /// </summary>
    public class ResourceProviderWithResourceManager: IResourceProvider
    {
        ResourceManager Manager;
 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceProviderWithResourceManager(ResourceManager Manager, string Name)
        {
            if (Manager == null)
                throw new ArgumentNullException("Manager");
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException("Name");

            this.Manager = Manager;
            this.Name = Name;
        }

        /* overrides */
        /// <summary>
        /// Returns a string that describes this object
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /* public */
        /// <summary>
        /// Returns a resource string for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        public virtual string GetString(string Key, CultureInfo Culture = null)
        {
            return Culture == null? Manager.GetString(Key): Manager.GetString(Key, Culture);
        }
        /// <summary>
        /// Returns a resource object for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        public virtual object GetObject(string Key, CultureInfo Culture = null)
        {
            return Culture == null ? Manager.GetObject(Key) : Manager.GetObject(Key, Culture);
        }
        /// <summary>
        /// Returns a binary resource for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        public virtual byte[] GetBinary(string Key, CultureInfo Culture = null)
        {
            return GetObject(Key, Culture) as byte[];
        }
        /// <summary>
        /// Returns a resource Image for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        public virtual Image GetImage(string Key, CultureInfo Culture = null)
        {
            return GetObject(Key, Culture) as Image;
        }


 

        /* properties */
        /// <summary>
        /// The name of this provider
        /// </summary>
        public virtual string Name { get; protected set; }
 
    }
}
