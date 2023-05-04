using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Resources;
using System.Globalization;

namespace Tripous.Core
{

    /// <summary>
    /// Represents an object that provides resources such as string and images.
    /// </summary>
    internal class LanguageResourceProvider : IResourceProvider
    {

        Language GetLanguage(CultureInfo Culture = null)
        {
            if (Culture == null)
                Culture = System.Threading.Thread.CurrentThread.CurrentCulture;

            Language Result = Languages.GetByCultureCode(Culture.Name);
            return Result;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LanguageResourceProvider()
        {
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
            Language Language = GetLanguage(Culture);
            return Language.Resources.Find(Key, DefaultToKey: true);
        }
        /// <summary>
        /// Returns a resource object for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        public virtual object GetObject(string Key, CultureInfo Culture = null)
        {
            return null;
        }
        /// <summary>
        /// Returns a binary resource for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// </summary>
        public virtual byte[] GetBinary(string Key, CultureInfo Culture = null)
        {
            return null;
        }
        /// <summary>
        /// Returns a resource Image for the Key, if any, else null.
        /// <para>NOTE: Culture must be a specific culture (e.g. en-US, el-GR, etc.)</para>
        /// <para>NOTE: If not culture is specified the default culture is used.</para>
        /// <para>NOTE: If in Windows, cast thre return object to the System.Drawing.Image class.</para>
        /// </summary>
        public virtual object GetImage(string Key, CultureInfo Culture = null)
        {
            return null;
        }
 
        /* properties */
        /// <summary>
        /// The name of this provider
        /// </summary>
        public virtual string Name { get; } = typeof(LanguageResourceProvider).Name;
    }
}
