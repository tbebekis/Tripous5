/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Provides a unified way in accessing the DataTable columns, 
    /// the GetSchema() returns for a Collection such as Tables, Columns, etc.
    /// <para>NOTE: this code is taken from the excellent http://dbschemareader.codeplex.com/</para>
    /// </summary>
    public class MetaViewsMap
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaViewsMap(DataTable dt)
        {
            Key = "TABLE_NAME"; //yep, it's Table_Name in SqlServer.
            OwnerKey = "TABLE_SCHEMA";
            Definition = "TEXT";
            TypeKey = "TABLE_TYPE";

            //mysql
            if (!dt.Columns.Contains(Definition)) Definition = "VIEW_DEFINITION";

            //firebird
            if (!dt.Columns.Contains(OwnerKey)) OwnerKey = "VIEW_SCHEMA"; //always null
            if (!dt.Columns.Contains(Definition)) Definition = "DEFINITION";

            //oracle
            if (!dt.Columns.Contains(Key)) Key = "VIEW_NAME";
            if (!dt.Columns.Contains(OwnerKey)) OwnerKey = "OWNER";

            //Oracle does not expose ViewColumns, only the raw sql.
            HasSql = dt.Columns.Contains(Definition);

            //Devart.Data.Oracle
            if (!dt.Columns.Contains(Key)) Key = "NAME";
            if (!dt.Columns.Contains(OwnerKey)) OwnerKey = "SCHEMA";

            //Devart.Data.MySQL
            if (!dt.Columns.Contains(OwnerKey)) OwnerKey = "DATABASE";

            if (!dt.Columns.Contains(TypeKey)) TypeKey = null;
        }

        /* properties */
        /// <summary>
        /// Gets the name of the Name  column
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// Gets the name of the schema owner column
        /// </summary>
        public string OwnerKey { get; private set; }
        /// <summary>
        /// Gets the name of the Type column
        /// </summary>
        public string TypeKey { get; private set; }
        /// <summary>
        /// True if Sql source code is provided
        /// </summary>
        public bool HasSql { get; private set; }
        /// <summary>
        /// Gets the name of source code the  column
        /// </summary>
        public string Definition { get; private set; }
    }
}
