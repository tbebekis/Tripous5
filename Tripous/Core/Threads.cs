/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tripous
{

    /// <summary>
    /// A static helper class that provides the means of executing a client method using a background thread (thread pool thread).
    /// <para>When such a thread starts executing, increases an internal counter. When
    /// finishes executing decreases the counter. The public property Working reflects
    /// the status of that counter. It returns false when there is not a thread executing.
    /// </para>
    /// <para>IMPORTANT: Any code that needs to run in the background it MUST use this helper class
    /// for executing its threaded methods.</para>
    /// </summary>
    static public class Threads
    {
        static object syncLock = new LockObject();
        static int working;
        static bool terminated;

        /* public */
        /// <summary>
        /// Executes the specified clent procedure in the context of a background thread passing it a specified parameter. 
        /// </summary>
        static public void Run(ParameterizedThreadStart Proc, object Param)
        {
            if (!Terminated)
            {
                Task.Run(() => {

                    Working = true;
                    try
                    {
                        Proc(Param);
                    }
                    finally
                    {
                        Working = false;
                    }
                });
            }
        }
        /// <summary>
        /// Executes the specified clent procedure in the context of a background thread passing it a specified parameter. 
        /// </summary>
        static public void Run<T>(Action<T> Proc, T Param)
        {
            if (!Terminated)
            {
                Task.Run(() => {

                    Working = true;
                    try
                    {
                        Proc(Param);
                    }
                    finally
                    {
                        Working = false;
                    }
                });
            }
        }
        /// <summary>
        /// Executes the specified clent procedure in the context of a background thread. 
        /// </summary>
        static public void Run(Action Proc)
        {
            if (!Terminated)
            {
                Task.Run(() => {

                    Working = true;
                    try
                    {
                        Proc();
                    }
                    finally
                    {
                        Working = false;
                    }
                });
            }
        }
        
 


        /// <summary>
        /// After a call to this method, the Start() method no longer accepts client calls.
        /// </summary>
        static public void Terminate()
        {
            Terminated = true;
        }

        /* properties */
        /// <summary>
        /// Returns true while one or more threads of this class are still running.
        /// </summary>
        static public bool Working
        {
            get
            {
                lock (syncLock)
                {
                    return working > 0;
                }
            }
            private set
            {
                lock (syncLock)
                {
                    if (value)
                        working++;
                    else
                    {
                        working--;
                        if (working < 0)
                            working = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Returns true if the Terminate() has called. 
        /// <para>NOTE: After a call to Terminate(), the Start() method no longer accepts client calls.</para>
        /// </summary>
        static public bool Terminated
        {
            get { lock (syncLock) return terminated; }
            private set { lock (syncLock) terminated = value; }
        }
    }
}
