/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Model
{

    /// <summary>
    /// A list of <see cref="QueryDescriptor"/> items.
    /// </summary>
    public class QueryDescriptors : ModelItems<QueryDescriptor> // NamedItems
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public QueryDescriptors()
        {
            UseSafeAdd = true;
        }

        /* methods */ 

        /// <summary>
        /// Adds a query Descriptor
        /// </summary>
        public override QueryDescriptor Add(string Name)
        {
            /* in order to avoid infinite call and stack overflow */
            QueryDescriptor Result = base.Add(Name);
            Result.Sql = "select * from " + Name;
            return Result;
        }
        /// <summary>
        /// Adds a query Descriptor
        /// </summary>
        public QueryDescriptor Add(string Name, string Sql)
        {
            return Add(Name, Sql, "");
        }
        /// <summary>
        /// Adds a query Descriptor
        /// </summary>
        public QueryDescriptor Add(string Name, string Sql, string ZoomCommand)
        {
            QueryDescriptor Result = base.Add(Name);
            Result.Sql = Sql;
            Result.ZoomCommand = ZoomCommand;
            return Result;
        }
    }
}
