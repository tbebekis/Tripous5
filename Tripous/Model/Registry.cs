/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Model
{

    /// <summary>
    /// Represents the registry of the tripous business model. 
    /// </summary>
    static public class Registry
    {
        /// <summary>
        /// A list of registered descriptors
        /// </summary>
        static public SqlBrowserDescriptors Browsers { get; set; } = new SqlBrowserDescriptors();
        /// <summary>
        /// A list of registered descriptors
        /// </summary>
        static public BrokerDescriptors Brokers { get; set; } = new BrokerDescriptors();
        /// <summary>
        /// Returns the CodeProducers
        /// </summary>
        static public CodeDescriptors CodeProducers { get; set; } = new CodeDescriptors();
        /// <summary>
        /// Returns the Locators
        /// </summary>
        static public LocatorDescriptors Locators { get; set; } = new LocatorDescriptors();
 
    }
}
