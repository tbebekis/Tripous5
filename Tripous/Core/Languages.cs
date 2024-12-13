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


        /* construction */
        /// <summary>
        /// Static constructor
        /// </summary>
        static Languages()
        {
            Add(Language.En);
            Add(Language.El);
        }

        /* public */
        /// <summary>
        /// Registers a language
        /// </summary>
        static public void Add(Language Item)
        {
            lock (syncLock)
            {
                if (!ContainsByCode(Item.Code))
                    fItems.Add(Item);
            }
        }
        /// <summary>
        /// Clears the existing languages and loads a specified list of new languages.
        /// <para>It preserves the <see cref="DefaultLanguage"/>.</para>
        /// </summary>
        static public void SetLanguages(IEnumerable<Language> NewItems)
        {
            lock (syncLock)
            {
                string DefaultLanguageCultureCode = DefaultLanguage.CultureCode;

                fItems.Clear();
                fItems.AddRange(NewItems);

                SetDefaultLanguage(DefaultLanguageCultureCode);
            }
        }

        /// <summary>
        /// Returns true if a language, specified by a culture code (en-US, el-GR) is registered.
        /// </summary>
        static public bool ContainsByCultureCode(string CultureCode)
        {
            return FindByCultureCode(CultureCode) != null;
        }
        /// <summary>
        /// Returns true if a language, specified by a two letter code (en, el, it, fr, etc) is registered.
        /// </summary>
        static public bool ContainsByCode(string TwoLetterLanguageCode)
        {
            return FindByCode(TwoLetterLanguageCode) != null;
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
        /// Returns a language, specified by the two letter code of the language, e.g en, el, it, fr, etc, if any, else null.
        /// </summary>
        static public Language FindByCode(string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                TwoLetterLanguageCode = TwoLetterLanguageCode.ToLowerInvariant();
                return fItems.FirstOrDefault(item => item.Code.ToLowerInvariant() == TwoLetterLanguageCode);
            }
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
                {
                    Item = DefaultLanguage;
                    Logging.Logger.Warn($"Language not registered: {CultureCode}");
                }
                //    throw new ApplicationException($"Language not registered: {CultureCode}");
                return Item;
            }
        }
        /// <summary>
        /// Returns a language, specified by the two letter code of the language, e.g en, el, it, fr, etc, if any, else throws an exception.
        /// </summary>
        static public Language GetByCode(string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                Language Item = FindByCode(TwoLetterLanguageCode);
                if (Item == null)
                {
                    Item = DefaultLanguage;
                    Logging.Logger.Warn($"Language not registered: {TwoLetterLanguageCode}");
                }
                //    throw new ApplicationException($"Language not registered: {TwoLetterLanguageCode}");
                return Item;
            }
        }

        /// <summary>
        /// Finds and returns a language by a Culture code, e.g. en-US
        /// </summary>
        static public Language FindByCultureCode(this IEnumerable<Language> Languages, string CultureCode)
        {
            lock (syncLock)
            {
                CultureCode = CultureCode.ToLowerInvariant();
                return Languages.FirstOrDefault(item => item.CultureCode.ToLowerInvariant() == CultureCode);
            }
        }
        /// <summary>
        /// Finds and returns a language by a Culture code, e.g. en-US
        /// </summary>
        static public Language FindByCode(this IEnumerable<Language> Languages, string TwoLetterLanguageCode)
        {
            lock (syncLock)
            {
                TwoLetterLanguageCode = TwoLetterLanguageCode.ToLowerInvariant();
                return Languages.FirstOrDefault(item => item.Code.ToLowerInvariant() == TwoLetterLanguageCode);
            }
        }

        /// <summary>
        /// Sets the default language by a specified culture code, i.e. en-US.
        /// <para>Throws an exception if the language is not already registered.</para>
        /// </summary>
        static public void SetDefaultLanguage(string CultureCode)
        {
            lock (syncLock)
            {
                fDefaultLanguage = GetByCultureCode(CultureCode);
            }                
        }

        /* properties */
        /// <summary>
        /// The default language
        /// </summary>
        static public Language DefaultLanguage
        {
            get
            {
                lock (syncLock)
                {
                    if (fDefaultLanguage == null)
                    {
                        fDefaultLanguage = fItems.FirstOrDefault(item => item.Code.IsSameText("en"));
                        return fDefaultLanguage != null ? fDefaultLanguage : new Language("", "English", "en", "en-US");
                    }

                    return fDefaultLanguage;
                }
            }
        }
        /// <summary>
        /// The number of registered languages
        /// </summary>
        static public int Count { get { lock (syncLock) return fItems.Count; } }
        /// <summary>
        /// The list of registered languages
        /// </summary>
        static public Language[] Items { get { lock (syncLock) return fItems.ToArray(); } }
    }
}
