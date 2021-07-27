

# Tripous Base Library

Tripous is the **Base Library** for all Tripous [.Net](https://en.wikipedia.org/wiki/.NET_Framework) projects. Provides classes ranging from string handling to data access to desktop forms handling.

## Solution Content
 - **Tripous**. A NetStandard library. The Tripous namespace contains base and utility classes.
 - **Tripous.Data**.  A NetStandard library. The Tripous.Data namespace contains classes that store and retrieve data from a database. The `SqlStore` class can be used to store and retrieve data from databases such as **MsSql**, **SQLite**, **MySql**, **PostgreSQL**, **Firebird** and **Oracle**, using the various `SqlProvider` derived classes.
 - **Tripous.Data.Dapper**.  A NetStandard library. Extends the Tripous.Data namespace. It contains classes that store and retrieve Data Entities to a database using [Dapper](https://en.wikipedia.org/wiki/Dapper_ORM) ORM.
 - **Tripous.Forms.** A .Net Framework Windows Forms library. The Tripous.Forms namespace contains utility classes, base Form classes, and many dialog boxes and facilitates building Desktop Windows Applications.  
 - **Tripous.Web**. A .Net Core library. The Tripous.Web namespace contains base and utility classes.


## Database Connection Settings File
An application built using the Tripous Base Library must provide a `json` file listing the available database connections. By default this file is named `SqlConnections.json` and may be placed in the main executable's folder. 

The application loads the `SqlConnections.json` file using the `Tripous.Data.SqlConnectionInfoList` class and then assigns the `Connections` property of the  `Tripous.Data.Db` static class. The static `Tripous.Data.Db` class is the pivotal class of the data sub-system. 

There must be an entry in the `SqlConnections.json` file, under the name `DEFAULT`, which is considered the **default** database connection. If a connection named `DEFAULT` is not found then the first entry is considered the **default** database connection. 

Before running the system for the first time the user has to update that `SqlConnections.json` file with the correct database connection settings.

Here is an example.

```
{
    "SqlConnections": [
        {
            "Name": "DEFAULT",
            "Provider": "MsSql",
            "ConnectionString": "data source=localhost; initial catalog=DB_NAME; Integrated Security=SSPI;",
            "AutoCreateGenerators": false,
            "CommandTimeoutSeconds": 0
        }
    ]
}
```
 

<table>
<thead>
	<tr>
		<th>Property</th>
		<th>Type</th>
		<th>Description</th>
	</tr>
</thead>
<tbody>
	<tr>
		<td>Name</td>
		<td>string</td>
		<td>The name of the connection. The default connection should be named DEFAULT.</td>
	</tr>
		<tr>
		<td>Provider</td>
		<td>string</td>
		<td>The name of the provider. Valid values: MsSql, Oracle, Firebird, SQLite, MySql and PostgreSQL.</td>
	</tr>
		<tr>
		<td>ConnectionString</td>
		<td>string</td>
		<td>The connection string</td>
	</tr>
		<tr>
		<td>AutoCreateGenerators</td>
		<td>boolean</td>
		<td>Whether to create table generators/sequences automatically. For databases that support generators/sequences such as Oracle and Firebird. Optional. Defaults to false.</td>
	</tr>
		<tr>
		<td>CommandTimeoutSeconds</td>
		<td>integer</td>
		<td>The time in seconds to wait for a SELECT/INSERT/UPDATE/DELETE/CREATE TABLE ect. command to execute. Zero means the default timeout. Optional. Defaults to 0.</td>
	</tr>
</tbody>
</table>






