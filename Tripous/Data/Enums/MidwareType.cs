/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

namespace Tripous.Data
{

    /// <summary>
    /// Indicates the midware technology used when connecting to a database
    /// </summary>
    public enum MidwareType
    {
        /// <summary>
        /// No midware is used. The Provider goes directly to the database
        /// </summary>
        Direct,
        /// <summary>
        /// The .Net OleDb Provider is used
        /// </summary>
        OleDb,
        /// <summary>
        /// The .Net Odbc Provider is used
        /// </summary>
        Odbc
    }
}
