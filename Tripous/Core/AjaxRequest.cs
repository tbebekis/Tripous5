namespace Tripous
{
    /// <summary>
    /// An AJAX request.
    /// <para>An AJAX request could be either a Ui or a Proc request. </para>
    /// <para>A Ui request may set the an <see cref="IsSingleInstance"/> flag indicating that the Ui may exist only once. </para>
    /// <para>A Proc request may or may not return a packet. </para>
    /// <para>A requester may optionally set the <see cref="CommandId"/> and/or <see cref="CommandName"/> properties. </para>
    /// </summary>
    public class AjaxRequest
    {
        static object syncLock = new LockObject();
        static List<IAjaxRequestHandler> Handlers = new List<IAjaxRequestHandler>();


        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxRequest()
        {
        }

        /* static */
        /// <summary>
        /// Registers a handler.
        /// </summary>
        static public void Register(IAjaxRequestHandler Handler)
        {
            lock (syncLock)
                Handlers.Insert(0, Handler);
        }
        /// <summary>
        /// Unregisters a handler.
        /// </summary>
        static public void UnRegister(IAjaxRequestHandler Handler)
        {
            lock (syncLock)
                Handlers.Remove(Handler);
        }
 
        /// <summary>
        /// Iterates through its registered handlers, finds the right one and processes a specified <see cref="AjaxRequest"/>.
        /// Returns an <see cref="AjaxResponse"/> on success. Else returns null.
        /// </summary>
        static public AjaxResponse Process(AjaxRequest Request, IViewToStringConverter ViewToStringConverter)
        {
            lock (syncLock)
            {
                AjaxResponse Result = null;

                foreach (IAjaxRequestHandler Handler in Handlers)
                {
                    Result = Handler.Process(Request, ViewToStringConverter);
                    if (Result != null)
                        return Result;
                }

                return Result;
            }
        }

        /* public */
        /// <summary>
        /// Returns true when Params contains a specified key.
        /// </summary>
        public bool ParamsContainsKey(string Key)
        {
            return !string.IsNullOrWhiteSpace(Key) && Params != null && Params.ContainsKey(Key) && Params[Key] != null;
        }
        /// <summary>
        /// Returns a param under a specified key, if any, else null.
        /// </summary>
        public object GetParam(string Key)
        {
            return ParamsContainsKey(Key) ? Params[Key] : null;
        }

        /* properties */
        /// <summary>
        /// Optional. The id of the request.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Required. The name of the operation to execute
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Optional. Any parameteres coming along with the request.
        /// </summary>
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The request type. Ui or Proc.
        /// A Ui request returns HTML.
        /// A Proc request may or may not return a packet.
        /// </summary>
        public RequestType Type { get; set; }
        /// <summary>
        /// True when this is a single instance Ui request.
        /// </summary>
        public bool IsSingleInstance { get; set; }

        /// <summary>
        /// A requester may optionally set the <see cref="CommandId"/> and/or <see cref="CommandName"/> properties.
        /// </summary>
        public string CommandId { get; set; }
        /// <summary>
        /// A requester may optionally set the <see cref="CommandId"/> and/or <see cref="CommandName"/> properties.
        /// </summary>
        public string CommandName { get; set; }
    }


}
