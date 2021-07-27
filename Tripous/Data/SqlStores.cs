﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2019 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;

namespace Tripous.Data
{
    /// <summary>
    /// Helper
    /// </summary>
    static public class SqlStores
    {

        /* create sql stores */
        /// <summary>
        /// Returns the <see cref="SqlStore"/> for the Default database connection
        /// </summary>
        static public SqlStore CreateDefaultSqlStore()
        {
            return CreateSqlStore(Db.GetDefaultConnectionInfo());
        }
        /// <summary>
        /// Creates and returns a <see cref="SqlStore"/>
        /// </summary>
        static public SqlStore CreateSqlStore(SqlConnectionInfo ConnectionInfo)
        {
            SqlProvider Provider = ConnectionInfo.GetSqlProvider();
            SqlStore Result = Activator.CreateInstance(Provider.SqlStoreClass, new object[] { ConnectionInfo }) as SqlStore;
            return Result;
        }
        /// <summary>
        /// Creates and returns a <see cref="SqlStore"/>
        /// </summary>
        static public SqlStore CreateSqlStore(string ConnectionName)
        {
            return CreateSqlStore(Db.GetConnectionInfo(ConnectionName));
        }

    }
}
