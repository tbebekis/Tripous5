using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// A listener is a subscriber to DomainEntity related events
    /// </summary>
    public interface IEntityListener
    {

        /// <summary>
        /// Event handler of DomainEntity event listener. 
        /// </summary>
        /// <param name="Entity">The entity caused the event</param>
        /// <param name="Event">A string representing the event, e.g. BeforeInsert</param>
        /// <param name="Info">A special information provided by the event sender. Could be null.</param>
        void HandleEntityEvent(DataEntity Entity, string Event, object Info);
    }
}
