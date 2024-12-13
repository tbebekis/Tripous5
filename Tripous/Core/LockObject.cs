namespace Tripous
{


    /// <summary>
    /// The Monitor class can be used effectively with multiple application domains 
    /// if the lock object derives from MarshalByRefObject.
    /// </summary>
    public class LockObject : MarshalByRefObject
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public LockObject()
        {
        }
    }
}