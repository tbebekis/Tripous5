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
