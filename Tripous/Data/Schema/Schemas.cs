/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tripous.Data
{

    /// <summary>
    /// The registry of database schemas.
    /// <para>A Schema is a list of versions under a name, i.e. Application, MyPlugin etc.</para>
    /// </summary>
    static public class Schemas
    {
        /* construction */
        /// <summary>
        /// Static constructor
        /// </summary>
        static Schemas()
        {
            List = new NamedItems<Schema>();
            AppSchema = List.Add(Sys.APPLICATION);
        }

        /* public */
        /// <summary>
        /// Executes schemas, that is creates tables etc.
        /// </summary>
        static public void Execute()
        {
            foreach (var Item in List)
                Item.Execute();
        }

        /* properties */
        /// <summary>
        /// The list of registered schemas
        /// </summary>
        static public NamedItems<Schema> List { get; private set; }
        /// <summary>
        /// The main schema of this application. It is executed first of all.
        /// </summary>
        static public Schema AppSchema { get; private set; }
    }
}
