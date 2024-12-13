namespace Tripous 
{

    /// <summary>
    /// Represents an object that processes an <see cref="AjaxRequest"/> and returns an <see cref="AjaxResponse"/>
    /// </summary>
    public interface IAjaxRequestHandler
    {

        /// <summary>
        /// Processes an <see cref="AjaxRequest"/> and if it handles the request returns an <see cref="AjaxResponse"/>. Else returns null.
        /// </summary>
        AjaxResponse Process(AjaxRequest Request, IViewToStringConverter ViewToStringConverter);
    }
}
