using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using Tripous;

namespace WebDesk.AspNet
{
    /// <summary>
    /// Provides access to session variables (entries)
    /// </summary>
    static public class Session
    {
        /* private */
        /// <summary>
        /// Returns a value stored in session, found under a specified key or a default value if not found.
        /// </summary>
        static T Get<T>(this ISession Instance, string Key)
        {
            Key = Key.ToLowerInvariant();
            string JsonText = Instance.GetString(Key);
            if (JsonText == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(JsonText);
        }
        /// <summary>
        /// Returns a value stored in session, found under a specified key or a default value if not found.
        /// </summary>
        static T Get<T>(this ISession Instance, string Key, T Default)
        {
            Key = Key.ToLowerInvariant();
            string JsonText = Instance.GetString(Key);
            if (JsonText == null)
                return Default;

            return JsonConvert.DeserializeObject<T>(JsonText);
        }
        /// <summary>
        /// Stores a value in session under a specified key.
        /// </summary>
        static void Set<T>(this ISession Instance, string Key, T Value)
        {
            Key = Key.ToLowerInvariant();
            string JsonText = JsonConvert.SerializeObject(Value);
            Instance.SetString(Key, JsonText);
        }
 

        /* public */
        /// <summary>
        /// Returns a value stored in session, found under a specified key or a default value if not found.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public T Get<T>(string Key)
        {
            return WSys.HttpContext.Session.Get<T>(Key);
        }
        /// <summary>
        /// Returns a value stored in session, found under a specified key or a default value if not found.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public T Get<T>(string Key, T Default)
        {
            return WSys.HttpContext.Session.Get(Key, Default);
        }
        /// <summary>
        /// Stores a value in session under a specified key.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// <para>WARNING: Whenever an object is added by calling Set(), the object is serialized.
        /// So adding the object first and then altering the object will NOT work.
        /// The object should be added after any alteration to it is done.</para>
        /// </summary>
        static public void Set<T>(string Key, T Value)
        {
 
                WSys.HttpContext.Session.Set(Key, Value);
        }

        /// <summary>
        /// Removes and returns a value found under a specified key, if any, else returns the default value of the specified type argument.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public T Pop<T>(string Key)
        {
            T Result = Get<T>(Key);
            Remove(Key);
            return Result;
        }


        /// <summary>
        /// Returns a string stored in session, found under a specified key or null if not found.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public string GetString(string Key)
        {
            return Get<string>(Key, null);
        }
        /// <summary>
        /// Stores a string value in session under a specified key.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public void SetString(string Key, string Value)
        {
            Set(Key, Value);
        }

        /// <summary>
        /// Clears all session variables
        /// </summary>
        static public void Clear()
        {
            WSys.HttpContext.Session.Clear();
        }
        /// <summary>
        /// Removes a session variable under a specified key.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public void Remove(string Key)
        {
            Key = Key.ToLowerInvariant();
            WSys.HttpContext.Session.Remove(Key);
        }
        /// <summary>
        /// Returns true if a variable exists in session under a specified key.
        /// <para>NOTE: Key is NOT case sensitive.</para>
        /// </summary>
        static public bool ContainsKey(string Key)
        {
            Key = Key.ToLowerInvariant();
            byte[] Buffer = null;
            return WSys.HttpContext.Session.TryGetValue(Key, out Buffer);
        }

        /* properties */
        /// <summary>
        /// Provides acces to request variables.
        /// <para>This dictionary is used to store data while processing a single request. The dictionary's contents are discarded after a request is processed.</para>
        /// </summary>
        static public IDictionary<object, object> Request { get { return WSys.HttpContext.Items; } }


        /// <summary>
        /// Gets or sets the current language of the session.
        /// <para>Represents a language this application supports, i.e. provides localized resources for.</para>
        /// </summary>
        static public Language Language
        {
            get
            {
                Language Result = Get<Language>("Language", null);
                return Result != null ? Result : Languages.DefaultLanguage;
            }
            set
            {
                if (value != null)
                {
                    Set("Language", value);
                }
            }
        }

    }
}
