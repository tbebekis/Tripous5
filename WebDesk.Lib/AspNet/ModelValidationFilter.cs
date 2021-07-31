using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

 

namespace WebDesk.AspNet
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
