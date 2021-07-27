/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Tripous.Model
{
    /// <summary>
    /// Describes a query, that is a SQL SELECT. It actually is a table Descriptor.
    /// </summary>
    public class QueryDescriptor : Descriptor
    {
        private string sql = "";

        private string zoomCommand = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        public QueryDescriptor()
        {
        }

        /// <summary>
        /// Override
        /// </summary>
        public override void CheckDescriptor()
        {
            base.CheckDescriptor();


            if (string.IsNullOrEmpty(Sql))
                NotFullyDefinedError("Sql");

        }

        /* properties */
        /// <summary>
        /// Gets or sets the CommandText script.
        /// </summary>
        public string Sql
        {
            get { return string.IsNullOrEmpty(sql) ? "" : sql; }
            set { sql = value; }
        }
        /// <summary>
        /// The DisplayLabels list, where each line has the format FIELD_NAME=Title
        /// determines the visibility of the fields in the drop-down grids:
        /// if it is empty then all fields are visible
        /// else only the included fields are visible
        /// </summary>
        public string DisplayLabels { get; set; }

        /// <summary>
        /// Gets or sets the zoom command path. 
        /// <para>A zoom command is used by locators and other drill-down controls.</para>
        /// <para>It is something similar to PROCESSOR.COMMAND. For example MAIN_PROCESSOR.CUSTOMER</para>
        /// </summary>
        public string ZoomCommand
        {
            get { return string.IsNullOrEmpty(zoomCommand) ? "" : zoomCommand; }
            set { zoomCommand = value; }
        }

    }
}
