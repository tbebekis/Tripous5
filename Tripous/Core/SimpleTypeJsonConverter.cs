namespace Tripous
{
    /// <summary>
    ///  A custom json converter for the SimpleType enum
    /// </summary>
    internal class SimpleTypeJsonConverter : JsonConverter
    {
        /// <summary>
        ///  Writes the JSON representation of the object.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SimpleType v = (SimpleType)value;
            string S = v.ToChar().ToString();
            writer.WriteValue(S);
        }
        /// <summary>
        /// Reads the JSON representation of the object. Returns the object value.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string)
            {
                string S = reader.Value as string;
                return Simple.SimpleTypeOf(S[0]);
            }

            return reader.Value;

        }
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SimpleType);
        }

        /* properties */
        /// <summary>
        /// Gets a value indicating whether this instance can read JSON.
        /// </summary>
        public override bool CanRead { get { return true; } }
        /// <summary>
        /// Gets a value indicating whether this instance can write JSON.
        /// </summary>
        public override bool CanWrite { get { return true; } }
    }
}
