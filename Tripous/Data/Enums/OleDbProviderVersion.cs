﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Indicates the OleDb Provider version
    /// </summary>
    [Flags]
    public enum OleDbProviderVersion
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Provider=Microsoft.Jet.OLEDB.4.0;
        /// </summary>
        JetOleDb4 = 1,
        /// <summary>
        /// Provider=Microsoft.ACE.OLEDB.12.0
        /// </summary>
        AceOleDb12 = 2,
    }
}