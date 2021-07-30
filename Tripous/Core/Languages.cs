using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Tripous
{
    /// <summary>
    /// The languages this application supports, i.e. provides localized resources for.
    /// </summary>
    static public class Languages
    { 

        /* private */
        static object syncLock = new LockObject();
        static List<Language> fItems = new List<Language>();
        static Language fDefaultLanguage;

        /* public */
        /// <summary>
        /// Returns true if a language, specified by a two letter code (en, el, it, fr, etc) is registered.
        /// </summary>
        static public bool Contains(string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                return Find(TwoLetterLanguageCode) != null;
            }
        }
        /// <summary>
        /// Returns a language, specified by a two letter code (en, el, it, fr, etc), if registered, else null.
        /// </summary>
        static public Language Find(string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                Language Item = fItems.FirstOrDefault(item => item.Code.IsSameText(TwoLetterLanguageCode));
                return Item;
            }
        }
        /// <summary>
        /// Returns a language, specified by a two letter code (en, el, it, fr, etc), if registered, else returns the default language (en).
        /// </summary>
        static public Language FindOrDefault(string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                Language Item = Find(TwoLetterLanguageCode);
                if (Item == null)
                    Item = DefaultLanguage;
                return Item;
            }
        }
        /// <summary>
        /// Returns a language, specified by a two letter code (en, el, it, fr, etc), if registered, else throws an exception.
        /// </summary>
        static public Language Get(string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                Language Item = Find(TwoLetterLanguageCode);
                if (Item == null)
                    throw new ApplicationException($"Language not registered: {TwoLetterLanguageCode}");
                return Item;
            }
        }

        /// <summary>
        /// Returns a language, specified by a culture code (en-US, el-GR), if registered, else null.
        /// </summary>
        static public Language FindByCultureCode(string CultureCode)
        {
            lock (syncLock)
            {
                CultureCode = CultureCode.ToLowerInvariant();
                return fItems.FirstOrDefault(item => item.CultureCode.ToLowerInvariant() == CultureCode);
            }            
        }
        /// <summary>
        /// Finds and returns a language by a Culture code, e.g. en-US
        /// </summary>
        static public Language FindByCultureCode(this IEnumerable<Language> Languages, string CultureCode)
        {
            CultureCode = CultureCode.ToLowerInvariant();
            return Languages.FirstOrDefault(item => item.CultureCode.ToLowerInvariant() == CultureCode);
        }
        /// <summary>
        /// Returns a language, specified by a culture code (en-US, el-GR), if registered, else throws an exception.
        /// </summary>
        static public Language GetByCultureCode(string CultureCode)
        {
            lock (syncLock)
            {
                Language Item = FindByCultureCode(CultureCode);
                if (Item == null)
                    throw new ApplicationException($"Language not registered: {CultureCode}");
                return Item;
            }
        }

        /// <summary>
        /// Registers a language
        /// </summary>
        static public void Add(Language Item)
        {
            lock (syncLock)
            {
                if (!Contains(Item.Code))
                    fItems.Add(Item);
            }
        }
        /// <summary>
        /// Sets the default language by a specified culture code, i.e. en-US.
        /// <para>Throws an exception if the language is not already registered.</para>
        /// </summary>
        static public void SetDefaultLanguage(string CultureCode)
        {
            fDefaultLanguage = GetByCultureCode(CultureCode);
        }

        /* properties */
        /// <summary>
        /// The default language
        /// </summary>
        static public Language DefaultLanguage
        {
            get
            {
                if (fDefaultLanguage == null)
                {
                    fDefaultLanguage = fItems.FirstOrDefault(item => item.Code.IsSameText("en"));
                    return fDefaultLanguage != null ? fDefaultLanguage : new Language("", "English", "en", "en-US");
                }
                return fDefaultLanguage;
            }
        }
        /// <summary>
        /// The number of registered languages
        /// </summary>
        static public int Count { get { lock (syncLock) return fItems.Count; } }
        /// <summary>
        /// The list of registered languages
        /// </summary>
        static public Language[] Items { get {   return fItems.ToArray(); } }
    }
}
