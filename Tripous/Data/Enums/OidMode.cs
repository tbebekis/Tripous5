/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/


namespace Tripous.Data
{
    /// <summary>
    /// Indicates when a numeric ID, which uniquelly identifies a database row, is created: Before or After the
    /// insertion of the new row. For example, MS Sql identity columns are considered After. Firebird Generators
    /// and Oracle Sequencers are considered Before.
    /// <para>OidMode has effect only when <see cref="OidType"/> is set to Integer.</para>
    /// <para>An OID (Object Identifier) must uniquely identify a data table row and must has no business meaning at all.</para>
    /// </summary>
    public enum OidMode
    {
        /// <summary>
        /// Not known.
        /// </summary>
        Unknown,
        /// <summary>
        /// Numeric ID values are generated before the insert of the new row, possibly by
        /// a generator or sequencer mechanism
        /// </summary>
        Before,
        /// <summary>
        /// Numeric ID values are generated along with the insert of the new row,  
        /// by an auto-increment mechanism
        /// </summary>
        Along,

    }

}
