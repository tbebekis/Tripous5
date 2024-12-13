namespace WebLib
{


    /// <summary>
    /// Web Api Controller.
    /// <para>CAUTION: This should be used for WebAPIs controllers only, not for AJAX controllers. </para>
    /// <para>Use the <see cref="Controller"/> as base class for AJAX controllers. Otherwise the ActionExceptionFilter will provide error results. </para>
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = Lib.JwtAuthScheme)]
    public class ControllerApi : ControllerBase
    {
        IJwtRequestContext fJwtRequestContext;
 
        /* properties */
        /// <summary>
        /// The context regarding the current HTTP request (current visitor, selected warehouse, etc.)
        /// </summary>
        protected IJwtRequestContext JwtRequestContext => fJwtRequestContext ?? (fJwtRequestContext = Lib.GetService<IJwtRequestContext>());
 
        /* public */
        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult" /> object that serializes the specified data object to JSON.
        /// </summary>
        [NonAction]
        protected JsonResult Json(object data)
        {
            return new JsonResult(data);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ControllerApi()
        {
        }

    }
}
