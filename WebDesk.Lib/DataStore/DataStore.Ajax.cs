namespace WebLib
{
 
    static public partial class DataStore
    {


        /// <summary>
        /// Executes a request coming from an ajax call to ajax controller and returns the result.
        /// </summary>
        static public HttpPacketResult AjaxExecute(IViewToStringConverter Controller, AjaxRequest Request)
        {
            AjaxResponse Response = AjaxRequest.Process(Request, Controller);

            if (Response == null)
                Sys.Throw($"Ajax Operation not supported: {Request.OperationName}");

            HttpPacketResult Result = HttpPacketResult.SetPacket(Response.GetPacketObject(), true);
            return Result;
        }

    }
}
