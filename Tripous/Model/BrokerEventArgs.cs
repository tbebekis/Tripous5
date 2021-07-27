/*--------------------------------------------------------------------------------------        
                           Copyright © 2014 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
 

namespace Tripous.Model
{

    /// <summary>
    /// Broker event args
    /// </summary>
    public class BrokerEventArgs : EventArgs
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerEventArgs(Broker Broker)
        {
            this.Broker = Broker;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerEventArgs(Broker Broker, ExecTime Stage, DataMode State, DataMode OldState, object RowId = null, bool Reselect = false)
        {
            this.Broker = Broker;
            this.Stage = Stage;
            this.State = State;
            this.OldState = OldState;            
            this.RowId = RowId;
            this.Reselect = Reselect;
        }

        /* properties */
        /// <summary>
        /// The broker
        /// </summary>
        public Broker Broker { get; private set; }
        /// <summary>
        /// The current state of the broker
        /// </summary>
        public DataMode State { get; private set; }
        /// <summary>
        /// The old state of the broker
        /// </summary>
        public DataMode OldState { get; private set; }
        /// <summary>
        /// The stage of the State, could be None, Before or After.
        /// </summary>
        public ExecTime Stage { get; private set; }
        /// <summary>
        /// The Id of the row the broker is acting upon. Valid on Edit and Delete state.
        /// </summary>
        public object RowId { get; private set; }
        /// <summary>
        /// Valid on Commit state. When true, then the broker reselects the table tree.
        /// </summary>
        public bool Reselect { get; private set; }
    }
}
