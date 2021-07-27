/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;

namespace Tripous
{
    /// <summary>
    /// Represents an object with a unique Name (Code property)  
    /// </summary>
    public class CodeName : ICodeName
    {
        /// <summary>
        /// Field
        /// </summary>
        protected string fCode;

        /// <summary>
        /// constructor
        /// </summary>
        public CodeName()
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        public CodeName(string Code)
        {
            fCode = Code;
        }

        /// <summary>
        /// Gets the unique Code value
        /// </summary>
        public virtual string Code { get { return fCode; } }

    }



}
