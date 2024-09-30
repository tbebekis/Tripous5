/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.IO;
using System.Dynamic;
using System.Globalization;
 
using System.ComponentModel;
using System.Data;

using System.Threading.Tasks;
using System.Runtime.Serialization;
 

using System.Reflection;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;


namespace Tripous
{
 

    /// <summary>
    /// Helper json static class
    /// <para>SEE: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview </para>
    /// </summary>
    static public class Json
    {

        /// <summary>
        /// Returns true if ClassType implements InterfaceType.  
        /// </summary>
        static public bool ImplementsInterface(Type ClassType, Type InterfaceType)
        {
            return (ClassType != null) && (Array.IndexOf(ClassType.GetInterfaces(), InterfaceType) != -1);
        }
        /// <summary>
        /// Returns trur if Value exists in List, case insensitively.
        /// </summary>
        static public bool ContainsText(IList<string> List, string Value)
        {
            if (List != null)
            {
                for (int i = 0; i < List.Count; i++)
                    if (string.Compare(List[i], Value, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return true;
            }
            return false;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static Json()
        {
     
        }

        /* public */
        /// <summary>
        /// Creates and returns an instance of default settings for the serializer
        /// </summary>
         static public JsonSerializerSettings CreateDefaultSettings(bool Formatted = true, bool IgnoreNullProperties = false)
        {
            JsonSerializerSettings Result = new JsonSerializerSettings();
            Result.Formatting = Formatted ? Formatting.Indented : Formatting.None;
            Result.NullValueHandling = IgnoreNullProperties ? NullValueHandling.Ignore : NullValueHandling.Include;
            Result.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return Result;
        }
        
        /// <summary>
        /// Deserializes (creates) an object of a specified type by deserializing a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public T Deserialize<T>(string JsonText, JsonSerializerSettings Settings = null)
        { 
            return !string.IsNullOrWhiteSpace(JsonText) ? JsonConvert.DeserializeObject<T>(JsonText, Settings ?? DefaultSettings) : default(T);
        }
        /// <summary>
        /// Deserializes (creates) an object of a specified type by deserializing a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public object Deserialize(string JsonText, Type ReturnType, JsonSerializerSettings Settings = null)
        {
            return !string.IsNullOrWhiteSpace(JsonText)? JsonConvert.DeserializeObject(JsonText, ReturnType, Settings ?? DefaultSettings): null;
        }
        /// <summary>
        /// Loads an object's properties from a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public void PopulateObject(object Instance, string JsonText, JsonSerializerSettings Settings = null)
        {
            if (Instance != null && !string.IsNullOrWhiteSpace(JsonText))
            {
                JsonConvert.PopulateObject(JsonText, Instance, Settings ?? DefaultSettings);
            }
        }
        
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public string Serialize(object Instance, JsonSerializerSettings Settings = null)
        {
            return Instance != null? JsonConvert.SerializeObject(Instance, Settings ?? DefaultSettings): string.Empty;
        }
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer.
        /// </summary>
        static public string Serialize(object Instance, bool Formatted)
        {
            return Serialize(Instance, CreateDefaultSettings(Formatted));
        }

        /// <summary>
        /// Returns a specified json text as formatted for readability.
        /// </summary>
        static public string Format(string JsonText)
        {
            if (string.IsNullOrWhiteSpace(JsonText))
            {
                var Settings = CreateDefaultSettings(true);
                object Instance = JsonConvert.DeserializeObject(JsonText);
                return Serialize(Instance, Settings);
            }

            return JsonText;
        }


        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer.
        /// </summary>
        static public string ToJson(object Instance, bool Formatted)
        {
            return Serialize(Instance, CreateDefaultSettings(Formatted));
        }
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public string ToJson(object Instance, JsonSerializerSettings Settings = null)
        {
            return Serialize(Instance, Settings);
        }
        /// <summary>
        /// Converts a specified Instance to json text.
        /// <para> ExcludeProperties, if not null, may contain property names to be ignored</para>
        /// </summary>
        static public string ToJson(object Instance, string[] ExcludeProperties)
        {
            string Result = string.Empty;
            if (Instance != null)
            {
                JsonSerializerSettings Settings = DefaultSettings;

                if (ExcludeProperties != null && ExcludeProperties.Length > 0)
                {
                    Settings.ContractResolver = new JsonNetContractResolver(ExcludeProperties);
                }

                Result = Serialize(Instance, Settings);
            }

            return Result;
        }

        /// <summary>
        /// Converts an object to JObject
        /// </summary>
        static public JObject ObjectToJObject(object Instance)
        {
            string JsonText = ToJson(Instance);
            return JObject.Parse(JsonText);
        } 


        /// <summary>
        /// Deserializes (creates) an object of a specified type by deserializing a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public object FromJson(Type ClassType, string JsonText, JsonSerializerSettings Settings = null)
        {
            return Deserialize(JsonText, ClassType, Settings);
        }
        /// <summary>
        /// Loads an object's properties from a specified json text.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// </summary>
        static public void FromJson(object Instance, string JsonText, JsonSerializerSettings Settings = null)
        {
            PopulateObject(Instance, JsonText, Settings);
        }
        /// <summary>
        /// Converts json text to an object of a specified type
        /// </summary>
        static public T FromJson<T>(string JsonText, JsonSerializerSettings Settings = null)
        {
            return Deserialize<T>(JsonText, Settings);
        }
 
        /// <summary>
        /// Converts a json text to a Dictionary instance.
        /// </summary>
        static public Dictionary<string, string> ToDictionary(string JsonText)
        {
            return Deserialize<Dictionary<string, string>>(JsonText);
        }
        /// <summary>
        /// Converts json text to a dynamic object
        /// </summary>
        static public dynamic ToDynamic(string JsonText)
        {
            return !string.IsNullOrWhiteSpace(JsonText) ? JsonConvert.DeserializeObject(JsonText) as dynamic : null;
        }

        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer and then to stream.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public MemoryStream ToJsonStream(object Instance, JsonSerializerSettings Settings = null)
        {
            MemoryStream MS = new MemoryStream();
            ToJsonStream(Instance, MS, Settings);
            return MS;
        }
        /// <summary>
        /// Converts Instance to a json string using the NewtonSoft json serializer and then to stream.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public void ToJsonStream(object Instance, Stream Stream, JsonSerializerSettings Settings = null)
        {
            string JsonText = ToJson(Instance, Settings);
            JsonTextToStream(JsonText, Stream);
        }

        /// <summary>
        /// Reads the json text from a stream and then deserializes (creates) an object of a specified type.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public object FromJsonStream(Type ClassType, Stream Stream, JsonSerializerSettings Settings = null)
        {
            string JsonText = StreamToJsonText(Stream);
            return FromJson(ClassType, JsonText, Settings);
        }
        /// <summary>
        /// Loads an object's properties from a specified stream, after reading the json text from the stream.
        /// <para>If no settings specified then it uses the default JsonSerializerSettings</para> 
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public void FromJsonStream(object Instance, Stream Stream, JsonSerializerSettings Settings = null)
        {
            string JsonText = StreamToJsonText(Stream);
            FromJson(Instance, JsonText, Settings);
        }

        /// <summary>
        /// Converts a specified json text to a stream.
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public MemoryStream JsonTextToStream(string JsonText)
        {
            MemoryStream MS = new MemoryStream();
            JsonTextToStream(JsonText, MS);
            return MS;
        }
        /// <summary>
        /// Converts a specified json text to a stream.
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public void JsonTextToStream(string JsonText, Stream Stream)
        {
            byte[] Buffer = Encoding.UTF8.GetBytes(JsonText);
            Stream.Write(Buffer, 0, Buffer.Length);
        }

        /// <summary>
        /// Reads a stream as json text.
        /// <para>NOTE: UTF8 encoding is used.</para>
        /// </summary>
        static public string StreamToJsonText(Stream Stream)
        {
            string JsonText = string.Empty;
            if (Stream != null && Stream.Length > 0)
            {
                using (StreamReader reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    JsonText = reader.ReadToEnd();
                }
            }

            return JsonText;
        }

        /// <summary>
        /// Returns the text of the input stream of a request (HttpContext.Request.Body) as a Dictionary. To be used when POST-ing json data.
        /// </summary>
        static public Dictionary<string, dynamic> GetRequestDic(Stream RequestBodyStream)
        {
            if (RequestBodyStream != null && RequestBodyStream.CanSeek)
            {
                string Text = StreamToJsonText(RequestBodyStream);
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    return Deserialize<Dictionary<string, dynamic>>(Text); //Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Text);
                }
            }

            return new Dictionary<string, dynamic>();
        }
 

        /// <summary>
        /// Saves an instance as json text in a specified file.
        /// </summary>
        static public void SaveToFile(object Instance, string FilePath, string Encoding = "utf-8")
        {
            string Folder = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrWhiteSpace(Folder))
                Directory.CreateDirectory(Folder);
            string JsonText = ToJson(Instance);
            File.WriteAllText(FilePath, JsonText, System.Text.Encoding.GetEncoding(Encoding));
        }

        /// <summary>
        /// Loads the properties of an instance by reading the json text of a specified file.
        /// </summary>
        static public void LoadFromFile(object Instance, string FilePath, string Encoding = "utf-8")
        {
            if (File.Exists(FilePath))
            {
                string JsonText = File.ReadAllText(FilePath, System.Text.Encoding.GetEncoding(Encoding));
                FromJson(Instance, JsonText);
            }
        }
        /// <summary>
        ///  Creates and returns an object of ClassType using the json text of a specified file
        /// </summary>
        static public object LoadFromFile(Type ClassType, string FilePath, string Encoding = "utf-8")
        {
            if (File.Exists(FilePath))
            {
                string JsonText = File.ReadAllText(FilePath, System.Text.Encoding.GetEncoding(Encoding));
                return FromJson(ClassType, JsonText);
            }

            return null;
        }
 
        /// <summary>
        /// Returns the Value as a value of T. If is null returns Default.
        /// </summary>
        static public T AsValue<T>(object Value, T Default)
        {

            try
            {
                if ((Value == null) || (DBNull.Value == Value))
                    return Default;

                if (Default != null && Default.GetType().IsValueType)
                {
                    Type DefaultType = Default.GetType();

                    if (Value.GetType() == DefaultType)
                        return (T)Value;

                    if (ImplementsInterface(DefaultType, typeof(IConvertible)))
                        return (T)System.Convert.ChangeType(Value, DefaultType, CultureInfo.InvariantCulture);
                }

                return (T)Value;

            }
            catch
            {
                return Default;
            }
        }

        /// <summary>
        /// Default settings
        /// </summary>
        static public JsonSerializerSettings DefaultSettings { get; } = CreateDefaultSettings(true);
    }



    /// <summary>
    /// Used in excluding properties when serializing.
    /// <para>E.g. JsonConvert.SerializeObject(Instance, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new JsonNetContractResolver(ExcludeProperties) }) </para>
    /// </summary>
    public class JsonNetContractResolver : DefaultContractResolver
    {

        string[] ExcludeProperties = new string[0];


        /// <summary>
        /// Override.  Creates properties for the given Newtonsoft.Json.Serialization.JsonContract.
        /// </summary>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(p => !Json.ContainsText(ExcludeProperties, p.PropertyName)).ToList();
            return properties;
        }



        /// <summary>
        /// Constructor
        /// </summary>
        public JsonNetContractResolver()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonNetContractResolver(string[] ExcludeProperties)
        {
            this.ExcludeProperties = ExcludeProperties;
        }


    }
}
