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
    public class MetaTablesMap
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaTablesMap(DataTable Table)
        {
 

            //sql server
            TableName = "TABLE_NAME";
            OwnerKey = "TABLE_SCHEMA";
            TypeKey = "TABLE_TYPE";

            //oracle
            if (!Table.Columns.Contains(OwnerKey)) 
                OwnerKey = "OWNER";

            if (!Table.Columns.Contains(TypeKey)) 
                TypeKey = "TYPE";

            //Devart.Data.Oracle - TABLE_NAME is NAME
            if (!Table.Columns.Contains(TableName)) 
                TableName = "NAME";

            if (!Table.Columns.Contains(OwnerKey)) 
                OwnerKey = "SCHEMA";

            //Devart.Data.PostgreSql
            if (!Table.Columns.Contains(TypeKey)) 
                TypeKey = "tabletype";


            //Devart.Data.MySQL
            if (!Table.Columns.Contains(OwnerKey)) 
                OwnerKey = "DATABASE";

            IsDb2 = Table.Columns.Contains("REMARKS");

            //Intersystems Cache
            if (!Table.Columns.Contains(OwnerKey)) 
                OwnerKey = "TABLE_SCHEM";

            //no schema
            if (!Table.Columns.Contains(OwnerKey)) 
                OwnerKey = null;
        }

        /* properties */
        /// <summary>
        /// Gets the name of the TableName column
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// Gets the name of the DbOwner column
        /// </summary>
        public string OwnerKey { get; private set; }
        /// <summary>
        /// Gets the name of the TableType column
        /// </summary>
        public string TypeKey { get; private set; }
        /// <summary>
        /// True if it is a Db2 database
        /// </summary>
        public bool IsDb2 { get; private set; }
    }


}
