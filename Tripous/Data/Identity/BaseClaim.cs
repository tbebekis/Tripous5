using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security.Claims;

namespace Tripous.Identity
{
 
    /// <summary>
    /// Base claim entity
    /// </summary>
    public class BaseClaim : IdentityEntity
    {        
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseClaim()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseClaim(DataRow Row)
        {
            Assign(Row);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseClaim(Claim Source)
        {
            Assign(Source);
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return Type;
        }
        /// <summary>
        /// Converts this instance to a <see cref="Claim"/>
        /// </summary>
        public Claim ToClaim()
        {
            return new Claim(Type, Value);
        }
        /// <summary>
        /// Assigns the properties of this instance from a  <see cref="Claim"/>
        /// </summary>
        public void Assign(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        /* property */
        /// <summary>
        /// The type of a claim.
        /// <para>NOTE: For a list of "well-known" claim types see the <see cref="ClaimTypes"/> static class </para>
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The value of a claim.
        /// </summary>
        public string Value { get; set; }
    }
}
