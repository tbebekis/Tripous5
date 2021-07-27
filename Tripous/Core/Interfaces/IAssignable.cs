/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;

namespace Tripous
{
    /// <summary>
    /// See the <see cref="Assignable"/> class summary.
    /// </summary>
    public interface IAssignable : ICloneable
    {
        /// <summary>
        /// Initializes all members of this instance
        /// </summary>
        void Clear();
        /// <summary>
        /// Derived classes should clear the content of this instance and assign 
        /// Source's members to their members.
        /// <para>Example call: <c>Destination.Assign(Source);</c></para>
        /// </summary>
        void Assign(object Source);
    }
}
