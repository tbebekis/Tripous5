

# Tripous Base Library

[![Nuget](https://img.shields.io/nuget/v/Tripous5)](https://www.nuget.org/packages/Tripous5/)

Tripous is a `net7.0` **Base Library** providing classes ranging from string handling to data access to desktop forms handling.

## The Content
 - **Tripous**. A library. The Tripous namespace contains base and utility classes.
 - **Tripous.Data**.  A namespace. 
     - The `Tripous.Data` namespace contains classes that store and retrieve data from a database. 
     - The `Tripous.Data.SqlStore` class can be used to store and retrieve data from databases such as **MsSql**, **SQLite**, **MySql**, **PostgreSQL**, **Firebird** and **Oracle**, using the various `SqlProvider` derived classes.
	 - The `Tripous.Data.DataService` class can be used to store and retrieve `Tripous.Data.DataEntity` class derived objects to a database, using [Dapper](https://en.wikipedia.org/wiki/Dapper_ORM) ORM.
	 - The `Tripous.Data.SqlBroker` class is another way to store and retrieve data in a database. A broker represents a tree of tables of a data module, lets say `Sales` and `SalesLines` tables. The top table is always a single-row table. It may have detail tables and sub-detail tables and so on. The broker loads, saves and deletes the whole table tree with a single call.
 - **Tripous.Forms.** [![Nuget](https://img.shields.io/nuget/v/Tripous5.Forms)](https://www.nuget.org/packages/Tripous5.Forms/) A `net7.0-windows` Windows Forms library. The `Tripous.Forms` namespace contains utility classes, base Form classes, and many dialog boxes and facilitates building Desktop Windows Applications.  
 - **WebDesk**. A `net7.0` ASP.NET MVC Web Application. A, not-yet-finished, web application to be used as a template in creating **desktop-like** web applications. Contains a full list of javascript controls ranging from a simple `tp.Button` or `tp.TextBox` to a complex `tp.TreeView` and `tpGrid` with groups, summaries and virtual scrolling.
    - Use `admin` as user and `webdesk` as password to log-in to the WebDesk application.
	- Log-out and click on **Demos** to see the demos of the javascript controls.


## Database Connection Settings File
The developer writes database-neutral code. The developer may use any of the supported databases, such as **MsSql**, **SQLite**, **MySql**, **PostgreSQL**, **Firebird** and **Oracle**, by just setting `"Provider": "PROVIDER NAME"` in a settings file. 

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






