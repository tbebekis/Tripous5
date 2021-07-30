using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace WebDesk
{
 


    /// <summary>
    /// Visitor cookie
    /// </summary>
    public class UserCookie
    {
        string fRequestorId = "";
        string fUnAuthenticatedId = "";
        string fCultureCode = "";

        /* constructor */
        /// <summary>
        /// Constructor. 
        /// </summary>
        public UserCookie()
        {
        }
 

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            string S1 = !string.IsNullOrWhiteSpace(RequestorId) ? RequestorId : "<NO DATA>";
            string S2 = !string.IsNullOrWhiteSpace(UnAuthenticatedId) ? UnAuthenticatedId : "<NO DATA>";
            string Result = $"RequestorId: {S1} - UnAuthenticatedId: {S2}";
            return Result;
        }
 
        /* properties */
        /// <summary>
        /// The Requestor.Id, this is the Id of the current Requestor
        /// </summary>
        public string RequestorId
        {
            get { return fRequestorId; }
            set
            {
                string sValue = !string.IsNullOrWhiteSpace(value) ? value : "";
                if (sValue != fRequestorId)
                {
                    fRequestorId = sValue;

                    if (Changed != null)
                        Changed(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// This is the Id of the last un-authenticated Visitor.
        /// </summary>
        public string UnAuthenticatedId
        {
            get { return fUnAuthenticatedId; }
            set
            {
                if (value != null && value != fUnAuthenticatedId)
                {
                    fUnAuthenticatedId = value;

                    if (Changed != null)
                        Changed(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// The culture code of the requestor
        /// </summary>
        public string CultureCode
        {
            get { return !string.IsNullOrWhiteSpace(fCultureCode) ? fCultureCode : "en-US"; }
            set
            {
                if (value != null && value != fCultureCode)
                {
                    fCultureCode = value;

                    if (Changed != null)
                        Changed(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Called whenever a property of this instance changes.
        /// </summary>
        public event EventHandler Changed;
    }



}
