/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Drawing;
using System.Globalization;

namespace Tripous
{
    /// <summary>
    /// Represents an object that provides resources such as string and images.
    /// <para>The class ResourceProvider provides a base implementation.</para>
    /// <para>Each Assembly may contain a ResourceProvider class
    /// decorated with the ResourceProviderAttribute. It can contain more than one though.</para>
    /// <para>An external module may implement this interface.</para>
    /// <para>The ObjectStore loads automatically those classes marked with ResourceProviderAttribute
    /// and implementing this interface.</para>
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// Returns a resource string for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        string GetString(string Key, CultureInfo Culture = null);
        /// <summary>
        /// Returns a resource object for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        object GetObject(string Key, CultureInfo Culture = null);
        /// <summary>
        /// Returns a binary resource for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        byte[] GetBinary(string Key, CultureInfo Culture = null);
        /// <summary>
        /// Returns a resource Image for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        Image GetImage(string Key, CultureInfo Culture = null);

 


        /* properties */
        /// <summary>
        /// The name of this provider
        /// </summary>
        string Name { get; }
 
    }
}
