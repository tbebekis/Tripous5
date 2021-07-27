using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Model
{

    /// <summary>
    /// Represents a class that contains a Broker property
    /// </summary>
    public interface IBrokerContainer
    {
        /// <summary>
        /// The broker
        /// </summary>
        Broker Broker { get; }
    }
}
