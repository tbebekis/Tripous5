/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
 

namespace Tripous.Data.Metadata
{
    // Oracle Schema Collections     http://msdn.microsoft.com/en-us/library/cc716723.aspx          
 
    /// <summary>
    /// Represents schema information for a database
    /// </summary>
    public class Metastore : NamedItem, IMetaNodeListParent, IMetaNodeList, IMetaNode
    {
        /* private */
        bool isLoading;
       

        DataTable tblCollections;
        DataTable tblRestrictions;


        /* maps */
        MetaTablesMap tablesMap;
        MetaFieldsMap fieldsMap;
        MetaViewsMap viewsMap;

        DataTable tblIndexes;

        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();

            Types.Clear();
            Tables.Clear();
            Views.Clear();

            tablesMap = null;
            fieldsMap = null;
            viewsMap = null;

            tblCollections = null;
            tblRestrictions = null;
            tblIndexes = null;

            IsLoaded = false; 
        }
        /// <summary>
        /// Gets the Name
        /// </summary>
        protected override string GetName()
        {
            return ConnectionInfo.Name;
        }
        /// <summary>
        /// Sets the Name
        /// </summary>
        protected override void SetName(string value)
        { 
        } 
 
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Metastore()
        {
            Types = new MetaTypes();
            Types.Owner = this;

            Tables = new MetaTables();
            Tables.Owner = this;

            Views = new MetaViews();
            Views.Owner = this;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public Metastore(SqlConnectionInfo ConnectionInfo)
            : this()
        {
            this.ConnectionInfo = ConnectionInfo;
            this.Provider = ConnectionInfo.GetSqlProvider();
        }

        /* methods */
        /// <summary>
        /// Loads the metadata information if it is not already loaded
        /// </summary>
        public void Load()
        {
            if (!IsLoaded && !isLoading)
            {
                isLoading = true;
                try
                {
                    tblCollections = null;
                    tblRestrictions = null;

                    if (Provider != null)
                    {
                        tblCollections = GetCollection(DbMetaDataCollectionNames.MetaDataCollections, null);
                        tblRestrictions = GetCollection(DbMetaDataCollectionNames.Restrictions, null);
                    }

                }
                finally
                {
                    isLoading = false;
                }
 
                IsLoaded = true;


                Types.Load();
            }
        }
        /// <summary>
        /// Forces the loading of metadata information even if it is already loaded
        /// </summary>
        public void ReLoad()
        {
            this.Clear();
            Load();
        }

 
        /* Collections */
        /// <summary>
        /// Returns true if the specified collection is supported by the provider GetSchema()
        /// </summary>
        public bool IsCollectionSupported(string Collection)
        {
            if (!IsLoaded && !isLoading)
                Load();

            if (Sys.IsSameText(Collection, DbMetaDataCollectionNames.MetaDataCollections))
                return true;

            if (tblCollections == null)
                return false;

            DataRow ResultRow = null;

            return tblCollections.Locate("CollectionName", new object[] { Collection }, LocateOptions.CaseInsensitive, out ResultRow);
        }
        /// <summary>
        /// Returns a DataTable with schema information regarding the specified Collection
        /// </summary>
        public DataTable GetCollection(string Collection, string[] RestrictionValues)
        {
            try
            {
                if (IsCollectionSupported(Collection))
                    return Provider.GetSchema(ConnectionInfo.ConnectionString, Collection, RestrictionValues);
            }
            catch
            {
            }

            return new DataTable(Collection);
        }
        /// <summary>
        /// Returns the tblRestrictions.DefaultView properly filtered and sorted, regarding the specified RestrictionType
        /// </summary>
        public DataView GetRestrictionsView(string CollectionName)
        {
            if (tblRestrictions == null)
                return null;

            DataView Result = tblRestrictions.DefaultView;

            try
            {
                Result.RowFilter = string.Format("CollectionName = '{0}'", CollectionName);
            }
            catch
            {
            }


            try
            {
                Result.Sort = "RestrictionNumber";
            }
            catch
            {
            }
                

            return Result;
        }
        /// <summary>
        /// Gets the schema restrictions for a collection (i.e Tables or Columns) regarding an ObjectName (i.e. the name of a Table).
        /// <para>ProviderSpecificRestrictionNames is an array with provider specific restriction names
        /// applicable to the specified CollectionName</para>
        /// <para>NOTE: this code is taken from the excellent http://dbschemareader.codeplex.com/</para>
        /// </summary>
        public string[] GetRestrictions(string DbOwner, string CollectionName, string ObjectName, params string[] ProviderSpecificRestrictionNames)
        {
            //there are no restrictions
            if (string.IsNullOrEmpty(DbOwner) && string.IsNullOrEmpty(ObjectName))
                return null;

            //get the restrictions collection
            DataView dv = GetRestrictionsView(CollectionName);
            bool hasParameterName = dv.Table.Columns.Contains("ParameterName");
            string paramName = string.Empty;

            string[] restrictions = new string[dv.Count];
            bool usedRestriction = false;

            //loop through the collection looking for the restriction
            for (int i = 0; i < dv.Count; i++)
            {
                string name = (string)dv[i].Row["RestrictionName"];

                //Oracle has an alternative column name
                if (hasParameterName)
                    paramName = (string)dv[i].Row["ParameterName"];

                bool found = false;

                //if set for owner restriction, apply it here
                if (!string.IsNullOrEmpty(DbOwner))
                {
                    if (name.Equals("OWNER", StringComparison.OrdinalIgnoreCase) ||
                        paramName.Equals("OWNER", StringComparison.OrdinalIgnoreCase) ||
                        name.Equals("TABLE_SCHEMA", StringComparison.OrdinalIgnoreCase) ||
                        name.Equals("PROCEDURE_SCHEMA", StringComparison.OrdinalIgnoreCase) ||
                        //Devart MySql
                        name.Equals("DATABASENAME", StringComparison.OrdinalIgnoreCase) ||
                        //Postgresql uses "Schema"
                        name.Equals("Schema", StringComparison.OrdinalIgnoreCase))
                    {
                        restrictions[i] = DbOwner;
                        usedRestriction = true;
                        continue;
                    }

                }
                //other restrictions: different possible names
                foreach (string rname in ProviderSpecificRestrictionNames)
                {
                    if (name.Equals(rname, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        restrictions[i] = ObjectName;
                        usedRestriction = true;
                        break;
                    }
                }

                if (!found) 
                    restrictions[i] = null;
            }

            if (!usedRestriction) 
                restrictions = null;

            return restrictions;
        }
        /// <summary>
        /// Returns a DataTabe with table information regarding this database
        /// </summary>
        public DataTable GetTables()
        {
            string CollectionName = "Tables";
            return GetCollection(CollectionName, null);
        }
        /// <summary>
        /// Returns a DataTabe with information regarding a table and a table-related collection, i.e. Columns, Indexes etc
        /// </summary>
        public DataTable GetTableCollection(string TableName, string Collection)
        {
            string[] Restrictions = GetRestrictions(string.Empty, Collection, TableName, "TABLE", "TABLE_NAME", "TABLENAME");
            return GetCollection(Collection, Restrictions);
        }
        /// <summary>
        /// Returns a DataTabe with view information regarding this database
        /// </summary>
        public DataTable GetViews()
        {
            string CollectionName = "Views";
            return GetCollection(CollectionName, null);
        }


        /* maps */
        /// <summary>
        /// Returns the Tables map
        /// </summary>
        public MetaTablesMap GetTablesMap(DataTable Table)
        {
            if (tablesMap == null)
                tablesMap = new MetaTablesMap(Table);
            return tablesMap;
        }
        /// <summary>
        /// Returns the fields map
        /// </summary>
        public MetaFieldsMap GetFieldsMap(DataTable Table)
        {
            if (fieldsMap == null)
                fieldsMap = new MetaFieldsMap(Table);
            return fieldsMap;
        }
        /// <summary>
        /// Returns the Views map
        /// </summary>
        public MetaViewsMap GetViewsMap(DataTable Table)
        {
            if (viewsMap == null)
                viewsMap = new MetaViewsMap(Table);
            return viewsMap;
        }


        /// <summary>
        /// Returns a string list with the table names in the database.
        /// </summary>
        public List<string> GetTableNames()
        {
            List<string> List = new List<string>();

            MetaTables Items = this.Tables;
            Items.Load();
            foreach (MetaTable Item in Items)
                List.Add(Item.Name);

            return List;
        }
        /// <summary>
        /// Returns a string list with the field names of the TableName
        /// </summary>
        public List<string> GetFieldNames(string TableName)
        {
            List<string> List = new List<string>();

            MetaTable Table = Tables.Find(TableName);

            if (Table != null)
            {
                MetaFields Items = Table.Fields;
                Items.Load();
                foreach (MetaField Item in Items)
                    List.Add(Item.Name);
            }

            return List;
        }
        /// <summary>
        /// Returns a string list with the index names in the database
        /// </summary>
        public List<string> GetIndexNames()
        {
            List<string> List = new List<string>();

            if (tblIndexes == null)
                tblIndexes = GetCollection("Indexes", null);

            string FieldName = "CONSTRAINT_NAME";
            if (!tblIndexes.Columns.Contains(FieldName)) 
                FieldName = "INDEX_NAME";
            if (!tblIndexes.Columns.Contains(FieldName))
                FieldName = "NAME";
            if (!tblIndexes.Columns.Contains(FieldName)) 
                FieldName = "indexname";

            if (tblIndexes.Columns.Contains(FieldName))
            {
                for (int i = 0; i < tblIndexes.Rows.Count; i++)
                    List.Add(tblIndexes.Rows[i][FieldName].ToString());

            }
 
            return List;
        }


        /* properties */
        /// <summary>
        /// True if the metadata is alreade loaded
        /// </summary>
        public bool IsLoaded { get; private set; }
        /// <summary>
        /// The connection info
        /// </summary>
        public SqlConnectionInfo ConnectionInfo { get; private set; }
        /// <summary>
        /// Returns the <see cref="SqlProvider"/>
        /// </summary>
        public SqlProvider Provider { get; private set; }
        /// <summary>
        /// Gets the text this instance provides for display purposes
        /// </summary>
        public string DisplayText { get { return Name; } }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Database; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Returns true if the Restrictions metadata collection is supported by the provider.
        /// </summary>
        public bool RestrictionsSupported
        {
            get
            {
                if (!IsLoaded)
                    Load();
                return (tblRestrictions != null) || (tblRestrictions.Rows.Count <= 0);
            }
        }
        /// <summary>
        /// Gets the meta Types collection, that is information regarding the data Types the database uses
        /// </summary>
        public MetaTypes Types { get; private set; }
        /// <summary>
        /// Gets the list of Tables
        /// </summary>
        public MetaTables Tables { get; private set; }
        /// <summary>
        /// Gets the list of Views
        /// </summary>
        public MetaViews Views { get; private set; }
        /// <summary>
        /// Gets the lists
        /// </summary>
        public IMetaNodeList[] Lists { get { return new IMetaNodeList[] { Tables, Views, Types }; } }
        /// <summary>
        /// Gets the node list
        /// </summary>
        public IMetaNode[] Nodes { get { return new IMetaNode[] { }; } }
 

    }
}
