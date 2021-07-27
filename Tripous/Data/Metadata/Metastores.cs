/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Linq;


namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Represents a meta-data information entity
    /// </summary>
    public class Metastores : MetaItems<Metastore>
    {

        /* constructor */
        /// <summary>
        /// Constructor
        /// </summary>
        public Metastores()
        {
            this.UniqueNames = true;
            this.UseSafeAdd = true;
            fDisplayText = "Databases";
            fKind = MetaNodeKind.Databases;
        }


    }

}
