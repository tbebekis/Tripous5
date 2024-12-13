namespace WebLib.AspNet
{

    /// <summary>
    /// Global filter for throwing an exception when a model with invalid state comes to a controller action.  
    /// <para>To register</para>
    /// <para><code> services.AddControllersWithViews(o =&gt; { o.Filters.Add&lt;ModelValidationFilter&gt;(); })
    /// </code></para>
    /// </summary>    
    public class ModelValidationFilter : IActionFilter // ActionFilterAttribute // IActionFilter 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ModelValidationFilter()
        {
        }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // context.Result = new BadRequestObjectResult(context.ModelState);    // this returns JSON          

                string Text = context.ModelState.GetErrorsText();
                throw new ValidationException(Text);       
            }
        }
        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


    }


}
