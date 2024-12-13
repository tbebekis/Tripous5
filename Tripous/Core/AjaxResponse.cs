namespace Tripous
{

    /// <summary>
    /// The response following an <see cref="AjaxRequest"/>
    /// </summary>
    public class AjaxResponse
    {
        Dictionary<string, object> Properties = new Dictionary<string, object>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxResponse()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxResponse(string OperationName)
        {
            this.OperationName = OperationName;
        }

        /* public */
        /// <summary>
        /// Returns the packet object.
        /// <para>The packet object goes to <see cref="HttpActionResult.Packet"/> property and it is the actual information that is returned to the caller.</para>
        /// </summary>
        public object GetPacketObject()
        {
            JObject Result = new JObject();

            if (!string.IsNullOrWhiteSpace(OperationName))
                Result["OperationName"] = OperationName;

            if (Properties != null && Properties.Count > 0)
            {
                foreach (var Entry in Properties)
                    Result[Entry.Key] = JToken.FromObject(Entry.Value);
            }

            return Result;
        }
        /// <summary>
        /// Returns true if the internal properties dictionary contains a specified key.
        /// </summary>
        public bool ContainsKey(string Key)
        {
            return Properties.ContainsKey(Key);
        }

        /* properties */
        /// <summary>
        /// Optional. The name of the request/response operation, if any, else null.
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Indexer. Get or sets the value of an entry in internal properties dictionary.
        /// <para>If the specified key not exists, returns null.</para>
        /// </summary>
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key) ? Properties[Key] : null; }
            set { Properties[Key] = value; }
        }
    }
}
