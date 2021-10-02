using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

 

namespace WebLib.AspNet
{
    /// <summary>
    /// Extension for the Required attribute
    /// </summary>
    public class RequiredExAttribute : RequiredAttribute
    {
        /// <summary>
        /// Applies formatting to an error message based on the data field where the error occurred.
        /// </summary>
        public override string FormatErrorMessage(string name)
        {
            string Format = WSys.Localize("RequiredField");
            return string.Format(Format, name);
        }
    }

 
}
