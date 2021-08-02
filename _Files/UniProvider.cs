





namespace bt.UniProvider
{
   using System;
   using System.Reflection;
   using System.Data;  
   using System.Data.Common;
   using System.Collections;
   using bt.Parsers;



   /// <summary>
   /// Represents an open connection to a data source.
   /// </summary>
   public interface IUniConnection : IDbConnection
   {
      /// <summary>
      /// Begins a database transaction.
      /// </summary>
      /// <returns>A <see cref='UniTransaction'/> object.</returns>
      new UniTransaction BeginTransaction();
      /// <summary>
      /// Begins a database transaction with the specified IsolationLevel.
      /// </summary>
      /// <param name="il">One of the IsolationLevel values.</param>
      /// <returns>A <see cref='UniTransaction'/> object.</returns>
      new UniTransaction BeginTransaction(System.Data.IsolationLevel il);
      /// <summary>
      /// Creates and returns a <see cref='UniCommand'/> object associated with the connection.
      /// </summary>
      /// <returns>A <see cref='UniCommand'/> object.</returns>
      new UniCommand CreateCommand();
   }     
   

   
   /// <summary>
   /// Represents a transaction to be performed at a data source.
   /// </summary>
   public interface IUniTransaction : IDbTransaction 
   {
      /// <summary>
      /// Specifies the <see cref='UniConnection'/> object to associate with the transaction.
      /// </summary>
      new UniConnection Connection { get; }
   }
   
   
   
   /// <summary>
   /// Represents an SQL statement that is executed while connected to a data source.
   /// </summary>
   public interface IUniCommand : IDbCommand
   {
      /// <summary>
      /// Gets or sets the <see cref='UniConnection'/> used by this instance of the <see cref='UniCommand'/>.
      /// </summary>
      new UniConnection Connection { get; set; }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <returns>A <see cref='UniParameter'/> object.</returns>
      new UniParameter CreateParameter();  
      /// <summary>
      /// Executes an SQL statement against the connection and returns the number of rows affected.
      /// </summary>
      /// <returns>The number os rows affected.</returns>
      new UniDataReader ExecuteReader();  
      /// <summary>
      /// Executes an SQL statement against the connection and returns the number of rows affected.
      /// </summary>
      /// <param name="behavior">One of the <see cref='CommandBehavior'/> values.</param>
      /// <returns>The number os rows affected.</returns>
      new UniDataReader ExecuteReader(System.Data.CommandBehavior behavior);  
      /// <summary>
      /// Gets the <see cref='UniParameterCollection'/>.
      /// </summary>
      new UniParameterCollection Parameters { get; }

      /// <summary>
      /// Gets or sets the <see cref='UniTransaction'/> within which the <see cref='UniCommand'/> executes.
      /// </summary>
      new UniTransaction Transaction { get; set; }
   }   
   

   
   /// <summary>
   /// Provides a means of reading one or more forward-only streams of result sets obtained 
   /// by executing a command at a data source.
   /// </summary>
   public interface IUniDataReader : IDataReader
   {
      /// <summary>
      /// Gets an <see cref='UniDataReader'/> to be used when a field points to more remote structured data.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>An <see cref='UniDataReader'/> to be used when the field points to more remote structured data.</returns>
      new UniDataReader GetData(int i);
   }
   

   
   /// <summary>
   /// Represents a set of command-related properties that are used to fill the <see cref='DataSet'/>
   /// and update a data source.
   /// </summary>
   public interface IUniDataAdapter : IDbDataAdapter 
   {
      /// <summary>
      /// Gets or sets an SQL statement for deleting records from the data set.
      /// </summary>
      new UniCommand DeleteCommand { get; set; }

      /// <summary>
      /// Gets or sets an SQL statement used to insert new records into the data source.
      /// </summary>
      new UniCommand InsertCommand { get; set; }

      /// <summary>
      /// Gets or sets an SQL statement used to select records in the data source.
      /// </summary>
      new UniCommand SelectCommand { get; set; }

      /// <summary>
      /// Gets or sets an SQL statement used to update records in the data source.
      /// </summary>
      new UniCommand UpdateCommand { get; set; }
   }      
   
   
   
   
   
   
   /// <summary>
   /// Represents an open connection to a database server.
   /// </summary>
   public class UniConnection : IUniConnection
   {
   
   #region private
      private IDbConnection FNativeInterface;
      private UniProvider FProvider;  
      
      /// <summary>
      /// Throws an exception in regard to <paramref name="InvalidState"/> value
      /// </summary>
      private void CheckConnected(bool InvalidState)
      {
         if (InvalidState)
         {
           if (IsConnected)
             throw(new Exception("UniConnection is connected. Illegal operation"));               
         }
         else
         {
            if (!IsConnected)
              throw(new Exception("UniConnection is NOT connected. Illegal operation"));  
         }
      
      }        
      
      /// <summary>
      /// Sets the string used to open a database server.
      /// </summary>
      /// <param name="connectionString">The connection string used to open the database server.</param>              
      private void SetConnectionString(string connectionString) 
      {
      
         string ProviderName = "";
         string ConStr  = "";
         
         ExtractProviderName(connectionString, ref ProviderName, ref ConStr);
         
         if (FProvider == null)
           if (ProviderName != "")           
             FProvider = ProviderStore.Find(ProviderName);
         
         if (FProvider == null)
            throw(new Exception("Provider not defined"));

         CheckConnected(true);
         
         
         //object [] Args = new object [] {ConStr};
			
			FNativeInterface = (IDbConnection)FProvider.Assembly.CreateInstance(FProvider.ConnectionClassName);
			FNativeInterface.ConnectionString = ConStr;
			
         //FNativeInterface = (IDbConnection)FProvider.Assembly.CreateInstance(FProvider.ConnectionClassName, true, 0, null, Args, null, null);


      }
   #endregion private
		
   #region constructors
      /// <summary>
      /// Initializes a new instance of the <see cref='UniConnection'/> class.
      /// </summary>
      public UniConnection() 
      {
      }
      
      /// <summary>
      /// Initializes a new instance of the <see cref='UniConnection'/> class.
      /// </summary>
      /// <remarks> The <paramref name="connectionString"/> should contain a "UniProviderName=XXX" entry-value pair.
      ///</remarks>
      /// <param name="connectionString">The connection string used to open the database server.</param>
      public UniConnection(string connectionString)
      {
         SetConnectionString(connectionString);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref='UniConnection'/> class.
      /// </summary>
      /// <param name="Provider">The Provider to be used.</param>
      public UniConnection(UniProvider Provider)
      {
         FProvider = Provider;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref='UniConnection'/> class.
      /// </summary>
      /// <param name="Provider">The Provider to be used.</param>
      /// <param name="connectionString">The connection string used to open the database server.</param>
      public UniConnection(UniProvider Provider, string connectionString)
      {
         FProvider = Provider;
         SetConnectionString(connectionString);
      }       

      /// <summary>
      /// Releases the resources used by the Component.
      /// </summary>
      public void Dispose()
      {
      }  
   #endregion constructors

   #region public 
      /// <summary>
      /// Begins a database transaction with the specified IsolationLevel value.
      /// </summary>
      /// <param name="il">One of the <see cref='IsolationLevel'/> values.</param>
      /// <returns>A interface to <see cref='IDbTransaction'/>.</returns>
      IDbTransaction System.Data.IDbConnection.BeginTransaction(System.Data.IsolationLevel il)
      {
         return UniTransaction.Create(this, il);
      }

      /// <summary>
      /// Begins a database transaction.
      /// </summary>
      /// <param name="il">One of the <see cref='IsolationLevel'/> values</param>
      /// <returns>A <see cref='UniTransaction'/> object.</returns>
      public UniTransaction BeginTransaction(System.Data.IsolationLevel il)
      {
         return (UniTransaction)((IDbConnection)this).BeginTransaction(il);
      }

      /// <summary>
      /// Begins a database transaction.
      /// </summary>
      /// <returns>A interface to <see cref='IDbTransaction'/>.</returns>
      IDbTransaction System.Data.IDbConnection.BeginTransaction()
      {
         return ((IDbConnection)this).BeginTransaction(IsolationLevel.Unspecified);
      }

      /// <summary>
      /// Begins a database transaction.
      /// </summary>
      /// <returns>A <see cref='UniTransaction'/> object.</returns>
      public UniTransaction BeginTransaction()
      {
         return (UniTransaction)((IDbConnection)this).BeginTransaction();
      }

      /// <summary>
      /// Creates and returns a interfaco to <see cref='IDbCommand'/> associated with the <see cref='UniConnection'/>.
      /// </summary>
      /// <returns>A interface to <see cref='IDbCommand'/>.</returns>
      IDbCommand System.Data.IDbConnection.CreateCommand()
      {            
         return new UniCommand(this);
      }

      /// <summary>
      /// Creates and returns a <see cref='UniCommand'/> object associated with the <see cref='UniConnection'/>.
      /// </summary>
      /// <returns>A <see cref='UniCommand'/> object.</returns>
      public UniCommand CreateCommand()
      {
         return (UniCommand)((IDbConnection)this).CreateCommand();
      }      
		/// <summary>
		/// Creates and returns a <see cref='UniCommand'/> object associated with the <see cref='UniConnection'/>.
		/// </summary>
		/// <returns>A <see cref='UniCommand'/> object.</returns>
		public UniCommand CreateCommand(string CommandText)
		{
			UniCommand Command = CreateCommand();
			Command.CommandText = CommandText;
			return Command;
		}

      /// <summary>
      /// Changes the current database for an open <see cref='UniConnection'/>.
      /// </summary>
      /// <param name="databaseName">The name of the database to use in place of the current database.</param>
      public void ChangeDatabase(string databaseName)
      {
         FNativeInterface.ChangeDatabase(databaseName);
      }

      /// <summary>
      /// Opens a database connection with the property settings specified by the ConnectionString.
      /// </summary>
      public void Open()
      {
         if (FNativeInterface.State != ConnectionState.Open) 
            FNativeInterface.Open();
      }

      /// <summary>
      /// Closes the connection to the database. This is the preferred method of closing any open connection.
      /// </summary>
      public void Close()
      {
         FNativeInterface.Close();
      }

      /// <summary>
      /// Extracts the <paramref name="ProviderName"/> and the <paramref name="ConnectionString"/> 
      /// from the <paramref name="Input"/> parameter.
      /// </summary>
      /// <remarks>The  <paramref name="Input"/> parameter is a ConnectionString that may contains a 
      /// "UniProviderName=XXX" entry-value pair. This static method parses the Input parameter
      /// and returns the contained information. The ProviderName is deleted from the ConnectionString if found </remarks>
      public static void ExtractProviderName(string Input, ref string ProviderName, ref string ConnectionString)
      {
         ProviderName = "";
         ConnectionString = "";
		   
         string [] Split = Input.Split(';');
		   
         foreach (string SpliItem in Split) 
         {
            string S = SpliItem.Trim();
            string[] EntryValuePair = S.Split('=');
            
            if ((string.Compare(EntryValuePair[0], "UniProviderName", true) == 0)) 
               ProviderName = EntryValuePair[1].Trim();
            else ConnectionString = ConnectionString + S + ";"; 

         }

         ConnectionString = ConnectionString.TrimEnd(';');		 
      }
   #endregion public
       
   #region properties

      /// <summary>
      /// Indicates whether this UniConnection is connected.
      /// </summary>
      public bool IsConnected 
      { 
         get 
         { 
            if (FNativeInterface == null)
               return false;
            else return FNativeInterface.State == ConnectionState.Open ; } 
         }

      /// <summary>
      /// Gets the <see cref='UniProvider'/> to access a database server.
      /// </summary>
      public UniProvider Provider  
      { 
         get { return FProvider; } 
         set 
         {
            CheckConnected(true);
            FProvider = value;
         }         
      }

      /// <summary>
      /// Gets the current state of the connection.
      /// </summary>
      public System.Data.ConnectionState State  { get { return FNativeInterface.State; } }

      /// <summary>
      /// Gets or sets the string used to open a database server.
      /// </summary>
      public string ConnectionString
      {
         get { throw new Exception("Connection string cannot be viewed due to security reasons");  }
         set {  SetConnectionString(value); }
      }

      /// <summary>
      /// Gets the name of the current database or the database to be used after a connection is opened.
      /// </summary>
      public string Database { get { return FNativeInterface.Database; } }
              
      /// <summary>
      /// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
      /// </summary>
      public int ConnectionTimeout { get { return FNativeInterface.ConnectionTimeout; } }
      
      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper.
      /// </summary>
      public IDbConnection NativeInterface { get { return FNativeInterface; } }
      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      /*
                    if (Con.NativeObject is System.Data.SqlClient.SqlConnection)
              MessageBox.Show( ((System.Data.SqlClient.SqlConnection)Con.NativeObject).WorkstationId  );
      */
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties

   }
   
      
   
   /// <summary>
   /// Represents a SQL transaction to be made in a database server.
   /// </summary>
   public class UniTransaction : IUniTransaction
   {

   #region private  
      private IDbTransaction FNativeInterface;
      private UniConnection FConnection;    
   #endregion private
     
   #region constructors
      /// <summary>
      /// creates an instance of this class and starts the transaction.
      /// </summary>
      internal static UniTransaction Create(UniConnection Connection)
      {
         return UniTransaction.Create(Connection, IsolationLevel.Unspecified);            
      }
      /// <summary>
      /// creates an instance of this class and starts the transaction.
      /// </summary>
      internal static UniTransaction Create(UniConnection Connection, System.Data.IsolationLevel il)
      {
         UniTransaction Res = new UniTransaction();
         Res.FConnection   = Connection;
         Res.FNativeInterface  = Connection.NativeInterface.BeginTransaction(il);
         return Res;            
      }
      /// <summary>
      /// Releases the resources used by the <see cref='UniDataReader'/>.
      /// </summary>
      public void Dispose()
      {
      }

   #endregion constructors
   
   #region public

      /// <summary>
      /// Rolls back a transaction from a pending state.
      /// </summary>
      public void Rollback()
      {
         FNativeInterface.Rollback();
      }

      /// <summary>
      /// Commits the database transaction.
      /// </summary>
      public void Commit()
      {
         FNativeInterface.Commit();
      }

   #endregion public
   
   #region properties

      /// <summary>
      /// Gets the <see cref='UniConnection'/> object associated with the transaction, or a null reference (Nothing in Visual Basic) if the transaction is no longer valid.
      /// </summary>
      IDbConnection System.Data.IDbTransaction.Connection { get { return FConnection; } }


      /// <summary>
      /// Gets the <see cref='UniConnection'/> object associated with the transaction, or a null reference (Nothing in Visual Basic) if the transaction is no longer valid.
      /// </summary>
      public UniConnection Connection { get { return FConnection; } }


      /// <summary>
      /// Specifies the <see cref='IsolationLevel'/> for this transaction.
      /// </summary>
      public System.Data.IsolationLevel IsolationLevel { get { return FNativeInterface.IsolationLevel; } }
      
      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper.
      /// </summary>
      public IDbTransaction NativeInterface { get { return FNativeInterface; } }
      
      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties
   
   }   
   
  
   
   /// <summary>
   /// Represents a SQL statement or stored procedure to execute in a database server.
   /// </summary>
   public class UniCommand : IUniCommand
   {

   #region private  
      private UniParameterCollection FParameters;
      private IDbCommand FNativeInterface;
      private UniConnection FConnection;
      private UniTransaction FTransaction; 
      private string FCommandText;   
      private bool FPrepared = false;
   #endregion

   #region constructors
      /// <summary>
      /// Initializes a new instance of the <see cref='UniCommand'/> class.
      /// </summary>
      private UniCommand()
      {
      }
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Connection"></param>
      public UniCommand(UniConnection Connection): this(Connection, "")
      {          
      }
      /// <summary>
      /// Initializes a new instance of the <see cref='UniCommand'/> class.
      /// </summary>
      /// <param name="cmdText">An SQL statement or stored procedure name to execute.</param>
      /// <param name="Connection">The <see cref='UniConnection'/> used by this instance of the <see cref='UniCommand'/>.</param>
      public UniCommand(UniConnection Connection, string cmdText): this(Connection, cmdText, null)
      {
      }         
      /// <summary>
      /// Initializes a new instance of the <see cref='UniCommand'/> class with the text of the query,
      /// a <see cref='UniConnection'/> and a <see cref='UniTransaction'/>.
      /// </summary>
      /// <param name="cmdText">An SQL statement or stored procedure name to execute.</param>
      /// <param name="Connection">The <see cref='UniConnection'/> used by this instance of the <see cref='UniCommand'/>.</param>
      /// <param name="Transaction">The <see cref='UniTransaction'/> within which the <see cref='UniCommand'/> executes.</param>
      public UniCommand(UniConnection Connection,	string cmdText, UniTransaction Transaction): this()
      {
         
         this.Connection = Connection;      
         FParameters = UniParameterCollection.Create(this);     
         this.CommandText = cmdText;
         this.Transaction = Transaction;
         
      }  

      /// <summary>
      /// Releases the resources used by the <see cref='UniCommand'/>.
      /// </summary>
      public void Dispose()
      {
      }

   #endregion constructors

   #region public  
      /// <summary>
      /// Attempts to cancel the execution of a <see cref='UniCommand'/>.
      /// </summary>
      public void Cancel()
      {
         FNativeInterface.Cancel();
      }
      
      /// <summary>
      /// Executes an SQL statement against the connection and returns the number of rows affected.
      /// </summary>
      /// <returns>The number os rows affected.</returns>
      public int ExecuteNonQuery()
      {
         Prepare();
         return  FNativeInterface.ExecuteNonQuery();
      }
      
      /// <summary>
      /// Overload the implemented interface <see cref='IDataReader'/>.
      /// </summary>
      /// <returns>A <see cref='UniDataReader'/> object.</returns>
      IDataReader System.Data.IDbCommand.ExecuteReader()
      {
         return ((IDbCommand)this).ExecuteReader(CommandBehavior.Default);
      }    
        
      /// <summary>
      /// Overload the implemented interface <see cref='IDataReader'/>.
      /// </summary>
      /// <returns>A <see cref='UniDataReader'/> object.</returns>
      IDataReader System.Data.IDbCommand.ExecuteReader(System.Data.CommandBehavior behavior)
      {
         Prepare();
         return UniDataReader.Create(this, behavior);
      }

      /// <summary>
      /// Sends the CommandText to the connection and builds a <see cref='UniDataReader'/>.
      /// </summary>
      /// <returns>A UniDataReader object.</returns>
      public UniDataReader ExecuteReader()
      {
         return (UniDataReader)((IDbCommand)this).ExecuteReader();
      }     
      
      /// <summary>
      /// Sends the CommandText to the Connection and builds a <see cref='UniDataReader'/> using one of the <see cref='CommandBehavior'/> values.
      /// </summary>
      /// <param name="behavior">One of the <see cref='CommandBehavior'/> values.</param>
      /// <returns>A <see cref='UniDataReader'/> object.</returns>
      public UniDataReader ExecuteReader(System.Data.CommandBehavior behavior)
      {
         return (UniDataReader)((IDbCommand)this).ExecuteReader(behavior);
      }

      /// <summary>
      /// Executes the query, and returns the first column of the first row in the result set returned by the query.
      /// Extra columns or rows are ignored.
      /// </summary>
      /// <returns>The first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</returns>
      public object ExecuteScalar()
      {
         Prepare();
         return FNativeInterface.ExecuteScalar();
      }           

      /// <summary>
      /// Creates a prepared version of the command on an instance of database server.
      /// </summary>
      public void Prepare()
      {
         if (!FPrepared)
           FNativeInterface.Prepare();
      }

   #endregion public

   #region additional execute methods
      /// <summary>
      /// Executes a SQL statement defined in CommandText property of <see cref='UniCommand'/> and returns
      /// a <see cref='DataSet'/> object.
      /// </summary>
      /// <returns>A <see cref='DataSet'/> object.</returns>
      public DataSet ExecuteDataSet()
      {
         UniDataAdapter adp = new UniDataAdapter(this);
         DataSet result = new DataSet();
         adp.Fill(result);

         return result;
      } 
      /// <summary>
      /// Executes a SQL statement defined in CommandText property of <see cref='UniCommand'/> and returns
      /// a <see cref='DataTable'/> object.
      /// </summary>
      /// <returns>A <see cref='DataTable'/> object.</returns>
      public DataTable ExecuteDataTable()
      {
         DataSet ds = ExecuteDataSet();
			
         return ds.Tables[0];
      }

  #endregion additional execute methods

   #region CreateParameter
      /// <summary>
      /// Overload the implemented interface <see cref='IDbDataParameter'/>.
      /// </summary>
      IDbDataParameter System.Data.IDbCommand.CreateParameter()
      {
         UniParameter Res = UniParameter.Create(this);
         Parameters.Add(Res);
         return Res;
      }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <returns>A <see cref='UniParameter'/> object.</returns>
      public UniParameter CreateParameter()
      {
         UniParameter param = (UniParameter)((IDbCommand)this).CreateParameter();
         //param.paramPrefix = this.Provider.ParamPrefix
         //connection.Provider.providerParamPrefix;
         return param;
      }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <param name="parameterName">The name of the <see cref='UniParameter'/>.</param>
      /// <param name="value">The value of the <see cref='UniParameter'/>.</param>
      /// <returns>A <see cref='UniParameter'/> object.</returns>
      public UniParameter CreateParameter(string parameterName, object value)
      {
         UniParameter param = CreateParameter();
         param.ParameterName = parameterName;
         param.Value = value;
         return param;
      }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <param name="parameterName">The name of the <see cref='UniParameter'/>.</param>
      /// <param name="dbType">The data type of the <see cref='UniParameter'/>.</param>
      /// <returns>A <see cref='UniParameter'/> object.</returns>
      public UniParameter CreateParameter(string parameterName, DbType dbType)
      {
         UniParameter param = CreateParameter();
         param.ParameterName = parameterName;
         param.DbType = dbType;
         return param;
      }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <param name="parameterName">The name of the <see cref='UniParameter'/>.</param>
      /// <param name="dbType">The data type of the <see cref='UniParameter'/>.</param>
      /// <param name="size">The maximum size, in bytes, of the data within the column.</param>
      /// <returns>A <see cref='UniParameter'/> object.</returns>
      public UniParameter CreateParameter(string parameterName, DbType dbType, int size)
      {
         UniParameter param = CreateParameter(parameterName, dbType);
         param.Size = size;
         return param;
      }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <param name="parameterName">The name of the <see cref='UniParameter'/>.</param>
      /// <param name="dbType">The data type of the <see cref='UniParameter'/>.</param>
      /// <param name="size">The maximum size, in bytes, of the data within the column.</param>
      /// <param name="sourceColumn">The name of the source column that is mapped to the <see cref='DataSet'/> and used for loading or returning the Value.</param>
      /// <returns>A UniParameter object.</returns>
      public UniParameter CreateParameter(string parameterName, DbType dbType, int size, string sourceColumn)
      {
         UniParameter param = CreateParameter(parameterName, dbType, size);
         param.SourceColumn = sourceColumn;
         return param;
      }

      /// <summary>
      /// Creates a new instance of a <see cref='UniParameter'/> object.
      /// </summary>
      /// <param name="parameterName">The name of the <see cref='UniParameter'/>.</param>
      /// <param name="dbType">The data type of the <see cref='UniParameter'/>.</param>
      /// <param name="size">The maximum size, in bytes, of the data within the column.</param>
      /// <param name="direction">Indicating whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.</param>
      /// <param name="precision">The maximum number of digits used to represent the Value property.</param>
      /// <param name="scale">The number of decimal places to which Value is resolved.</param>
      /// <param name="sourceColumn">The name of the source column that is mapped to the <see cref='DataSet'/> and used for loading or returning the Value.</param>
      /// <param name="sourceVersion">The <see cref='DataRowVersion'/> to use when loading Value.</param>
      /// <param name="value">The value of the <see cref='UniParameter'/>.</param>
      /// <returns>A <see cref='UniParameter'/> object.</returns>
      public UniParameter CreateParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
      {
         UniParameter param = CreateParameter(parameterName, dbType, size, sourceColumn);
         param.Direction = direction;
         param.Precision = precision;
         param.Scale = scale;
         param.SourceVersion = sourceVersion;
         param.Value = value;
         return param;
      }
      
   #endregion CreateParameter
   
   #region special
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ParameterName"></param>
      /// <returns></returns>
      public UniParameter ParamByName(string ParameterName) 
      { 
         return Parameters.ByName(ParameterName); 
      }       
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Index"></param>
      /// <returns></returns>
      public UniParameter ParamByIndex(int Index)
      {
         return Parameters.ByIndex(Index);    
      }   
      /// <summary>
      /// 
      /// </summary>
      public int ParamCount { get {return Parameters.Count;}}
  
   #endregion special

   #region properties  
      /// <summary>
      /// Overload the implemented interface <see cref='IDbConnection'/>.
      /// </summary>
      IDbConnection System.Data.IDbCommand.Connection
      {
         get
         {
            return FConnection;
         }
         set
         {
            FConnection = (UniConnection)value;
         }
      }

      /// <summary>
      /// Gets or sets the <see cref='UniConnection'/> used by this instance of the <see cref='UniCommand'/>.
      /// </summary>
      public UniConnection Connection
      {
         get
         {
            return FConnection;
         }
         set
         {
            FConnection = value;
            FNativeInterface = FConnection.NativeInterface.CreateCommand();
         }
      }

      /// <summary>
      /// Gets or sets the SQL statement or stored procedure to execute at the data source.
      /// </summary>
      public string CommandText
      {
         get
         {
            return FCommandText;
         }
         set
         {
            if ((string.Compare(value, FCommandText, true) != 0))
            {
               FPrepared = false;
               FCommandText = value;
               //FNativeInterface.CommandText = FConnection.Provider.NormalizeCommandText(FCommandText);
               FNativeInterface.CommandText = FConnection.Provider.NormalizeCommandText2(this);
            }     
            
         }
      }

      /// <summary>
      /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
      /// </summary>
      public int CommandTimeout
      {
         get
         {
            return FNativeInterface.CommandTimeout;
         }
         set
         {
            FNativeInterface.CommandTimeout=value;
         }
      }

      /// <summary>
      /// Gets or sets a value indicating how the CommandText property is to be interpreted.
      /// </summary>
      public System.Data.CommandType CommandType
      {
         get
         {
            return FNativeInterface.CommandType;
         }
         set
         {
            FNativeInterface.CommandType = value;
         }
      }

      /// <summary>
      /// Overload the implemented interface <see cref='IDataParameterCollection'/>.
      /// </summary>
      IDataParameterCollection System.Data.IDbCommand.Parameters
      {
         get
         {
            return FParameters;
         }
      }

      /// <summary>
      /// Gets the <see cref='UniParameterCollection'/>.
      /// </summary>
      public UniParameterCollection Parameters
      {
         get
         {
            return FParameters;
         }
      }

      /// <summary>
      /// Gets the <see cref='UniProvider'/> used by this instance of the <see cref='UniCommand'/>.
      /// </summary>
      public UniProvider Provider
      {
         get
         {
            return FConnection.Provider;
         }
      }

      /// <summary>
      /// Overload the implemented interface <see cref='IDbTransaction'/>.
      /// </summary>
      IDbTransaction System.Data.IDbCommand.Transaction
      {
         get
         {
            return FTransaction;
         }
         set
         {
            FTransaction = (UniTransaction)value;
         }
      }

      /// <summary>
      /// Gets or sets the <see cref='UniTransaction'/> within which the <see cref='UniCommand'/> executes.
      /// </summary>
      public UniTransaction Transaction
      {
         get
         {
            return FTransaction;
         }
         set
         {
            FTransaction = value;
            if (value == null)
               FNativeInterface.Transaction = null;
            else FNativeInterface.Transaction = FTransaction.NativeInterface;
         }
      }

      /// <summary>
      /// Gets or sets how command results are applied to the DataRow when used by the Update method of the <see cref='UniDataAdapter'/>.
      /// </summary>
      public System.Data.UpdateRowSource UpdatedRowSource
      {
         get
         {
            return FNativeInterface.UpdatedRowSource;
         }
         set
         {
            FNativeInterface.UpdatedRowSource = value;
         }
      }
      

      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper
      /// </summary>
      public IDbCommand NativeInterface { get { return FNativeInterface; } }

      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties
   
   }
   
         
   
   /// <summary>
   /// Represents a collection of parameters relevant to a <see cref='UniCommand'/> as well as their respective mappings to columns in a <see cref='DataSet'/>.
   /// </summary>
   public class UniParameterCollection : IDataParameterCollection
   {
   
   #region private  
      /// <summary>
      /// Represents this instance of the <see cref='UniParameterCollection'/> object created.
      /// </summary>
      private IDataParameterCollection FNativeInterface;   
      private UniCommand FCommand;   
      private ArrayList FParams = new ArrayList();
      private void CheckValue(object value)
      {
          if ((!(value is UniParameter)) || (value == null))
            throw(new Exception("Parameter object is not a UniParameter or is a null value"));     
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Param"></param>
      private void CheckExists(UniParameter Param)
      {
         if (Contains(Param)) 
            throw(new Exception("Param object already in list. ParameterName: " + Param.ParameterName));  
         CheckExists(Param.ParameterName); 
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ParameterName"></param>
      private void CheckExists(string ParameterName)
      {
         if (Exists(ParameterName))
            throw(new Exception("Param object already in list. ParameterName: " + ParameterName));   
      }
   #endregion

   #region constructors
      private UniParameterCollection()
      {
        // just to hide the default constructor
      }
      
      internal static UniParameterCollection Create(UniCommand Command)
      {
         UniParameterCollection Res = new UniParameterCollection();
         Res.FCommand = Command;
         Res.FNativeInterface = Command.NativeInterface.Parameters;
         return Res;
      }
   #endregion constructors

   #region IList  
      /// <summary>
      /// Adds a UniParameter to the <see cref='UniParameterCollection'/>.
      /// </summary>
      /// <param name="value">The <see cref='UniParameter'/> to add to the collection.</param>
      /// <returns>The position into which the new element was inserted.</returns>
      int IList.Add(object value)
      {
         int Res = -1;
         CheckValue(value);    
             
         UniParameter Param = (UniParameter)value; 
         CheckExists(Param);
        
         Res = FNativeInterface.Add(Param.NativeInterface);
         FParams.Add(Param);     
                   
         return Res;          
      }
      /// <summary>
      /// Removes all items from the collection.
      /// </summary>
      void IList.Clear()
      {
         FNativeInterface.Clear();
         FParams.Clear();
      }           
      /// <summary>
      /// Determines whether the collection contains a specific value.
      /// </summary>
      /// <param name="value">The name of the object to find.</param>
      /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
      bool IList.Contains(object value)
      {
         return FParams.Contains(value);
      }            
      /// <summary>
      /// Gets the location of a <see cref='UniParameter'/> in the collection.
      /// </summary>
      /// <param name="value">The name of the parameter to find.</param>
      /// <returns>The zero-based location of the <see cref='IDataParameter'/> within the collection.</returns>
      int IList.IndexOf(object value)
      {
         return FParams.IndexOf(value);
      }              
      /// <summary>
      /// Inserts a <see cref='UniParameter'/> into the collection at the specified index.
      /// </summary>
      /// <param name="index">The zero-based index where the <see cref='UniParameter'/> is to be inserted within the collection.</param>
      /// <param name="value">The <see cref='UniParameter'/> to add to the collection.</param>
      void IList.Insert(int index, object value)
      {
         CheckValue(value);       
         FNativeInterface.Insert(index,((UniParameter)value).NativeInterface);
         FParams.Insert(index, value);
      }
      /// <summary>
      /// Removes the specified <see cref='UniParameter'/> from the collection.
      /// </summary>
      /// <param name="value">The <see cref='UniParameter'/> to remove from the collection.</param>
      void IList.Remove(object value)
      {
         CheckValue(value);
         FNativeInterface.Remove(((UniParameter)value).NativeInterface);
         FParams.Remove(value);
      }
      /// <summary>
      /// Removes the specified <see cref='UniParameter'/> from the collection.
      /// </summary>
      /// <param name="index">The zero-based index of the <see cref='UniParameter'/>.</param>
      void IList.RemoveAt(int index)
      {
         FNativeInterface.RemoveAt(index);
         FParams.RemoveAt(index);
      }  
      /// <summary>
      /// Gets a value indicating whether the collection has a fixed size.
      /// </summary>
      bool IList.IsFixedSize
      {
         get
         {
            return FNativeInterface.IsFixedSize;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the <see cref='UniParameterCollection'/> is read-only.
      /// </summary>
      bool IList.IsReadOnly
      {
         get
         {
            return FNativeInterface.IsReadOnly;
         }
      }
 
      /// <summary>
      /// This property is the indexer for the <see cref='UniParameterCollection'/> class.
      /// </summary>
      object IList.this[int index]
      {
         get
         {
            //return FNativeInterface[index];
            return FParams[index];
         }
         set
         {
            CheckValue(value);       
            FParams[index] = value;
            //FNativeInterface[index] = value;
         }
      } 

   #endregion IList 
   
   #region IEnumerable  
      /// <summary>
      /// Returns an enumerator that can iterate through a collection.
      /// </summary>
      /// <returns>An <see cref='System.Collections.IEnumerator'/> that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return FNativeInterface.GetEnumerator();
      }  
   #endregion IEnumerable 
   
   #region ICollection  
      /// <summary>
      /// Gets the number of <see cref='UniParameter'/> objects in the collection.
      /// </summary>
      int ICollection.Count
      {
         get
         {
            return FNativeInterface.Count;
         }
      }
      /// <summary>
      /// Returns a value that indicates whether access to the collection is synchronized (thread safe).
      /// </summary>
      bool ICollection.IsSynchronized
      {
         get
         {
            return FNativeInterface.IsSynchronized;
         }
      }

      /// <summary>
      /// Returns the object that can be used to synchronize access to the collection.
      /// </summary>
      object ICollection.SyncRoot
      {
         get
         {
            return FNativeInterface.SyncRoot;
         }
      }

      /// <summary>
      /// Copies <see cref='UniParameter'/> objects from the <see cref='UniParameterCollection'/> to the specified array.
      /// </summary>
      /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref='UniParameterCollection'/>. The Array must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in array at which copying begins.</param>
      void ICollection.CopyTo(Array array, int index)
      {
         FNativeInterface.CopyTo(array,index);
      }

   #endregion ICollection 

   #region IDataParameterCollection  
      /// <summary>
      /// Gets the <see cref='UniParameter'/> with the specified name.
      /// </summary>
      /// <param parameterName="name">The name of the parameter to retrieve.</param>
      object IDataParameterCollection.this[string ParameterName]
      {
         get
         {
            return ByName(ParameterName);
         }
         set
         {
            CheckValue(value);               
            FParams[IndexOf(ParameterName)] = value;
         }
      }

      /// <summary>
      /// Gets a value indicating whether a <see cref='UniParameter'/> exists in the collection.
      /// </summary>
      /// <param name="ParameterName">The name of the <see cref='UniParameter'/> to find.</param>
      /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
      bool IDataParameterCollection.Contains(string ParameterName)
      {           
         return Exists(ParameterName);
      }
      /// <summary>
      /// Gets the location of a <see cref='UniParameter'/> in the collection.
      /// </summary>
      /// <param name="ParameterName">The name of the <see cref='UniParameter'/> to find.</param>
      /// <returns>The zero-based location of the <see cref='UniParameter'/> within the collection.</returns>
      int IDataParameterCollection.IndexOf(string ParameterName)
      {
         for (int i = 0; i < FParams.Count; i++)
            if (string.Compare(((UniParameter)FParams[i]).ParameterName, ParameterName, true) == 0)
               return i;
                 
         return -1;
      }
      /// <summary>
      /// Removes the specified <see cref='UniParameter'/> from the collection.
      /// </summary>
      /// <param name="ParameterName">The name of the <see cref='UniParameter'/> to remove.</param>
      void IDataParameterCollection.RemoveAt(string ParameterName)
      {
         if (Exists(ParameterName))
            Remove(Find(ParameterName));
      }
   #endregion IDataParameterCollection 

   #region public 
      /// <summary>
      /// Adds the Parameter to this collection.
      /// </summary>
      public UniParameter Add(UniParameter Parameter)
      {
         ((IList)this).Add((object)Parameter);
         return Parameter;
      }
      
      /// <summary>
      /// Creates and adds a Parameter to this collection.
      /// </summary>
      public UniParameter Add(string ParameterName, DbType dbType)
      {
         CheckExists(ParameterName);
         return Add(new UniParameter(FCommand, ParameterName, dbType));
      }
      /// <summary>
      /// Creates and adds a Parameter to this collection.
      /// </summary>
      public UniParameter Add(string ParameterName, DbType dbType, int Size)
      {
         CheckExists(ParameterName);
         return Add(new UniParameter(FCommand, ParameterName, dbType, Size));
      }
      
      /// <summary>
      /// Creates and adds a Parameter to this collection.
      /// </summary>
      public UniParameter Add(string ParameterName, DbType dbType, int Size, string SourceColumn)
      {
         CheckExists(ParameterName);
         return Add(new UniParameter(FCommand, ParameterName, dbType, Size, SourceColumn));
      }
      
      /// <summary>
      /// Creates and adds a Parameter to this collection.
      /// </summary>
      public UniParameter Add(string ParameterName, object Value)
      {
         CheckExists(ParameterName);
         return Add(new UniParameter(FCommand, ParameterName, Value));
      }
     
      /// <summary>
      /// Removes all items from the collection.
      /// </summary>
      public void Clear()
      {
         ((IList)this).Clear();
      }  
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Parameter"></param>
      /// <returns></returns>
      public bool Contains(UniParameter Parameter)
      {
         return ((IList)this).Contains(Parameter);
      }
      /// <summary>
      /// Gets a value indicating whether a <see cref='UniParameter'/> exists in the collection.
      /// </summary>
      /// <param name="ParameterName">The name of the <see cref='UniParameter'/> to find.</param>
      /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
      public bool Contains(string ParameterName)
      {           
         return ((IDataParameterCollection)this).Contains(ParameterName);
      }  
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Parameter"></param>
      /// <returns></returns>
      public int IndexOf(UniParameter Parameter)
      {
         return ((IList)this).IndexOf(Parameter);
      }
      /// <summary>
      /// Gets the location of a <see cref='UniParameter'/> in the collection.
      /// </summary>
      /// <param name="ParameterName">The name of the <see cref='UniParameter'/> to find.</param>
      /// <returns>The zero-based location of the <see cref='UniParameter'/> within the collection.</returns>
      public int IndexOf(string ParameterName)
      {
         return ((IDataParameterCollection)this).IndexOf(ParameterName);
      } 
      /// <summary>
      /// Inserts a <see cref='UniParameter'/> into the collection at the specified index.
      /// </summary>
      /// <param name="index">The zero-based index where the <see cref='UniParameter'/> is to be inserted within the collection.</param>
      /// <param name="Value">The <see cref='UniParameter'/> to add to the collection.</param>
      public void Insert(int index, UniParameter Value)
      {
         ((IList)this).Insert(index, Value);
      }
      /// <summary>
      /// Removes the specified <see cref='UniParameter'/> from the collection.
      /// </summary>
      /// <param name="Value">The <see cref='UniParameter'/> to remove from the collection.</param>
      public void Remove(UniParameter Value)
      {
         ((IList)this).Remove(Value);
      }    		      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Index"></param>
      public void RemoveAt(int Index)
      {
         ((IList)this).RemoveAt(Index); 
      }       
      /// <summary>
      /// Removes the specified <see cref='UniParameter'/> from the collection.
      /// </summary>
      /// <param name="ParameterName">The name of the <see cref='UniParameter'/> to remove.</param>
      public void RemoveAt(string ParameterName)
      {
         ((IDataParameterCollection)this).RemoveAt(ParameterName);
      }      
   #endregion public
   
   #region public special
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Index"></param>
      /// <returns></returns>
      public UniParameter ByIndex(int Index)
      {
         return (UniParameter)((IList)this)[Index];
      }   
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ParameterName"></param>
      /// <returns></returns>
      public UniParameter Find(string ParameterName)
      {
         UniParameter Param = null;
         for (int i = 0; i < FParams.Count; i++)
            if (string.Compare(((UniParameter)FParams[i]).ParameterName, ParameterName, true) == 0)
            {
               Param = (UniParameter)FParams[i];
               break;
            }            
         
         return Param;       
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ParameterName"></param>
      /// <returns></returns>
      public bool Exists(string ParameterName)
      {
         return (Find(ParameterName) != null);
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ParameterName"></param>
      /// <returns></returns>
      public UniParameter ByName(string ParameterName)
      {
         return Find(ParameterName);
      }
   #endregion public special
                   
   #region properties 
      /// <summary>
      /// Gets the number of <see cref='UniParameter'/> objects in the collection.
      /// </summary>
      public int Count { get { return ((ICollection)this).Count; } } 
      /// <summary>
      /// Gets a value indicating whether the collection has a fixed size.
      /// </summary>
      public bool IsFixedSize  { get {return ((IList)this).IsFixedSize; }}
      /// <summary>
      /// Gets a value indicating whether the <see cref='UniParameterCollection'/> is read-only.
      /// </summary>
      public bool IsReadOnly { get { return ((IList)this).IsReadOnly; }}
      /// <summary>
      /// Returns a value that indicates whether access to the collection is synchronized (thread safe).
      /// </summary>
      public bool IsSynchronized { get {return ((ICollection)this).IsSynchronized; }}  
      /// <summary>
      /// Returns the object that can be used to synchronize access to the collection.
      /// </summary>
      public object SyncRoot  { get {return ((ICollection)this).SyncRoot; }}
      /// <summary>
      /// Gets the <see cref='UniParameter'/> with the specified name.
      /// </summary>
      /// <param parameterName="name">The name of the parameter to retrieve.</param>
      public UniParameter this[string ParameterName]
      {
         get  { return (UniParameter)((IDataParameterCollection)this)[ParameterName];  }
         set  { ((IDataParameterCollection)this)[ParameterName] = value; }  
      }  
      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper
      /// </summary>
      public IDataParameterCollection NativeInterface { get { return FNativeInterface; } }
      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties

   }
   

   
   /// <summary>
   /// Represents a parameter to a <see cref='UniCommand'/>, and optionally, its mapping to <see cref='DataSet'/> columns.
   /// </summary>
   public class UniParameter : IDbDataParameter
   {
   
   #region private 
      private UniCommand FCommand;
      private IDbDataParameter FNativeInterface;
      private string FParameterName = "";
      
      /// <summary>
      /// Sets the name of the <see cref='UniParameter'/>.
      /// </summary>
      /// <param name="Name">The name of the parameter to map.</param>
      private void SetNativeParameterName(string Name)
      {
         FNativeInterface.ParameterName = FCommand.Provider.NormalizeParameterName(Name);
      }      
   #endregion private
          
   #region constructors 
      /// <summary>
      /// Creates a Parameter for the Command.
      /// </summary>
      public static UniParameter Create(UniCommand Command)
      {
         UniParameter Res = new UniParameter();
         Res.FCommand = Command;
         Res.FNativeInterface = Command.NativeInterface.CreateParameter();
         return Res;      
      }
      
      private UniParameter() 
      {
         // just to hide the default constructor
      }
      
      /// <summary>
      /// Constructor.
      /// </summary>
      public UniParameter(UniCommand Command): this ()
      {
         FCommand = Command;
         this.FNativeInterface = Command.NativeInterface.CreateParameter();
      }
      /// <summary>
      /// Constructor.
      /// </summary>
      public UniParameter(UniCommand Command, string ParameterName, object Value) : this(Command)          
      {
         this.ParameterName = ParameterName;
         this.Value = Value;
      }
      
      /// <summary>
      /// Constructor.
      /// </summary>
      public UniParameter(UniCommand Command, string ParameterName, DbType dbType) : this(Command)           
      {            
         this.ParameterName = ParameterName;
         this.DbType = dbType;
      }
      
      /// <summary>
      /// Constructor.
      /// </summary>
      public UniParameter(UniCommand Command, string ParameterName, DbType dbType, int Size) : this(Command, ParameterName, dbType)
      {
         this.Size = Size;
      }
      
      /// <summary>
      /// Constructor.
      /// </summary>
      public UniParameter(UniCommand Command, string ParameterName, DbType dbType, int Size, string SourceColumn) : this(Command, ParameterName, dbType, Size)   
      {      
         this.SourceColumn = SourceColumn;
      }     
      
   #endregion constructors
         
   #region properties 
      /*
      /// <summary>
      /// Gets or sets the <see cref='AdpType'/> of the parameter.
      /// </summary>
      public AdpType AdpType
      {
         get
         {
            return (Advanced.Data.Provider.AdpType) Enum.Parse(typeof(Advanced.Data.Provider.AdpType), Enum.GetName(typeof(System.Data.DbType),FParameter.DbType));
         }
         set
         {
            FParameter.DbType = (System.Data.DbType) Enum.Parse(typeof(System.Data.DbType), Enum.GetName(typeof(Advanced.Data.Provider.AdpType),value));
            Type t = FParameter.GetType();
            string a = t.FullName;
         }
      }
      */

      /// <summary>
      /// Gets or sets a value indicating whether the parameter is input-only, output-only, bidirectional, 
      /// or a stored procedure return value parameter.
      /// </summary>
      public System.Data.ParameterDirection Direction
      {
         get
         {
            return FNativeInterface.Direction;
         }
         set
         {
            FNativeInterface.Direction = value;
         }
      }

      /// <summary>
      /// Gets or sets the <see cref='DbType'/> of the parameter.
      /// </summary>
      public System.Data.DbType DbType
      {
         get
         {
            return FNativeInterface.DbType;
         }
         set
         {
            FNativeInterface.DbType = value;
         }
      }

      /// <summary>
      /// Gets or sets the maximum number of digits used to represent the Value property.
      /// </summary>
      public byte Precision
      {
         get
         {
            return FNativeInterface.Precision;
         }
         set
         {
            FNativeInterface.Precision = value;
         }
      }

      /// <summary>
      /// Gets or sets the number of decimal places to which Value is resolved.
      /// </summary>
      public byte Scale
      {
         get
         {
            return FNativeInterface.Scale;
         }
         set
         {
            FNativeInterface.Scale = value;
         }
      }

      /// <summary>
      /// Gets or sets the maximum size, in bytes, of the data within the column.
      /// </summary>
      public int Size
      {
         get
         {
            return FNativeInterface.Size;
         }
         set
         {
            FNativeInterface.Size = value;
         }
      }

      /// <summary>
      /// Gets or sets the value of the parameter.
      /// </summary>
      public object Value
      {
         get
         {
            return FNativeInterface.Value;
         }
         set
         {
            FNativeInterface.Value = value;
         }
      }

      /// <summary>
      /// Gets or sets a value indicating whether the parameter accepts null values.
      /// </summary>
      public bool IsNullable
      {
         get
         {
            return FNativeInterface.IsNullable;
         }
      }

      /// <summary>
      /// Gets or sets the <see cref='DataRowVersion'/> to use when loading Value.
      /// </summary>
      public System.Data.DataRowVersion SourceVersion
      {
         get
         {
            return FNativeInterface.SourceVersion;
         }
         set
         {
            FNativeInterface.SourceVersion = value;
         }
      }

      /// <summary>
      /// Gets or sets the name of the <see cref='UniParameter'/>.
      /// </summary>
      public string ParameterName
      {
         get
         {
            return FParameterName;
            //return FNativeInterface.ParameterName;
         }
         set
         {
            SetNativeParameterName(value);
            FParameterName = value;
         }
      }

      /// <summary>
      /// Gets or sets the name of the source column that is mapped to the DataSet and used for loading 
      /// or returning the Value.
      /// </summary>
      public string SourceColumn
      {
         get
         {
            return FNativeInterface.SourceColumn;
         }
         set
         {
            FNativeInterface.SourceColumn = value;
         }
      }
      
      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper
      /// </summary> 
      public IDbDataParameter NativeInterface { get { return FNativeInterface; } }
   
      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties
   
   }
   

   
   /// <summary>
   /// Provides a means of reading a forward-only stream of rows from a database server.
   /// </summary>
   public class UniDataReader : IUniDataReader
   {
   
   #region private
      private IDataReader FNativeInterface; 
   #endregion

   #region constructors   
      /// <summary>
      /// Static constructor.
      /// </summary>
      internal static UniDataReader Create(UniCommand Command, System.Data.CommandBehavior behavior)
      {
         UniDataReader Res = new UniDataReader();
         Res.FNativeInterface = Command.NativeInterface.ExecuteReader(behavior);     
         return Res;
      } 
      /// <summary>
      /// Releases the resources used by the <see cref='UniCommand'/>.
      /// </summary>
      public void Dispose()
      {
      }

   #endregion
           
   #region public     
      /// <summary>
      /// Closes the <see cref='UniDataReader'/> object.
      /// </summary>
      public void Close()
      {
         FNativeInterface.Close();
      }

      /// <summary>
      /// Gets a value indicating whether the column contains non-existent or missing values.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <returns>true if the specified column value is equivalent to DBNull; otherwise, false.</returns>
      public bool IsDBNull(int i)
      {
         return FNativeInterface.IsDBNull(i);
      }

      /// <summary>
      /// Advances the data reader to the next result, when reading the results of batch SQL statements.
      /// </summary>
      /// <returns>true if there are more result sets; otherwise, false.</returns>
      public bool NextResult()
      {
         return FNativeInterface.NextResult();
      }

      /// <summary>
      /// Advances the <see cref='UniDataReader'/> to the next record.
      /// </summary>
      /// <returns>true if there are more rows; otherwise, false.</returns>
      public bool Read()
      {
         return FNativeInterface.Read();
      }

      /// <summary>
      /// Gets the value of the specified column as a Boolean.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <returns>The value of the column.</returns>
      public bool GetBoolean(int i)
      {
         return FNativeInterface.GetBoolean(i);
      }

      /// <summary>
      /// Gets the value of the specified column as a byte.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <returns>The value of the specified column as a byte.</returns>
      public byte GetByte(int i)
      {
         return FNativeInterface.GetByte(i);
      }

      /// <summary>
      /// Reads a stream of bytes from the specified column offset into the buffer an array starting at the given buffer offset.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <param name="fieldOffset">The index within the field from which to begin the read operation.</param>
      /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
      /// <param name="bufferoffset">The index for buffer to begin the read operation.</param>
      /// <param name="length">The number of bytes to read.</param>
      /// <returns>The actual number of bytes read.</returns>
      public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
      {
         return FNativeInterface.GetBytes(i,fieldOffset,buffer,bufferoffset,length);
      }

      /// <summary>
      /// Gets the value of the specified column as a single character.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <returns>The value of the specified column.</returns>
      public char GetChar(int i)
      {
         return FNativeInterface.GetChar(i);
      }

      /// <summary>
      /// Reads a stream of characters from the specified column offset into the buffer as an array starting at the given buffer offset.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <param name="fieldoffset">The index within the row from which to begin the read operation.</param>
      /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
      /// <param name="bufferoffset">The index for buffer to begin the read operation.</param>
      /// <param name="length">The number of bytes to read.</param>
      /// <returns>The actual number of characters read.</returns>
      public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
      {
         return FNativeInterface.GetChars(i,fieldoffset,buffer,bufferoffset,length);
      }

      /// <summary>
      /// Gets an <see cref='IDataReader'/> to be used when the field points to more remote structured data.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>An <see cref='IDataReader'/> to be used when the field points to more remote structured data.</returns>
      IDataReader System.Data.IDataRecord.GetData(int i)
      {
         UniDataReader reader=new UniDataReader();
         reader.FNativeInterface=FNativeInterface.GetData(i);
         return reader;
      }

      /// <summary>
      /// Gets an <see cref='UniDataReader'/> to be used when the field points to more remote structured data.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>An <see cref='UniDataReader'/> to be used when the field points to more remote structured data.</returns>
      public UniDataReader GetData(int i)
      {
         return (UniDataReader)((IDataReader)this).GetData(i);
      }

      /// <summary>
      /// Gets the data type information for the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The data type information for the specified field.</returns>
      public string GetDataTypeName(int i)
      {
         return FNativeInterface.GetDataTypeName(i);
      }

      /// <summary>
      /// Gets the date and time data value of the spcified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The date and time data value of the spcified field.</returns>
      public DateTime GetDateTime(int i)
      {
         return FNativeInterface.GetDateTime(i);
      }

      /// <summary>
      /// Gets the fixed-position numeric value of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The fixed-position numeric value of the specified field.</returns>
      public decimal GetDecimal(int i)
      {
         return FNativeInterface.GetDecimal(i);
      }

      /// <summary>
      /// Gets the double-precision floating point number of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The double-precision floating point number of the specified field.</returns>
      public double GetDouble(int i)
      {
         return FNativeInterface.GetDouble(i);
      }

      /// <summary>
      /// Gets the Type information corresponding to the type of Object that would be returned from GetValue.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The Type information corresponding to the type of Object that would be returned from GetValue.</returns>
      public Type GetFieldType(int i)
      {
         return FNativeInterface.GetFieldType(i);
      }

      /// <summary>
      /// Gets the single-precision floating point number of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The single-precision floating point number of the specified field.</returns>
      public float GetFloat(int i)
      {
         return FNativeInterface.GetFloat(i);
      }

      /// <summary>
      /// Returns the guid value of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The guid value of the specified field.</returns>
      public Guid GetGuid(int i)
      {
         return FNativeInterface.GetGuid(i);
      }

      /// <summary>
      /// Gets the 16-bit signed integer value of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The 16-bit signed integer value of the specified field.</returns>
      public short GetInt16(int i)
      {
         return FNativeInterface.GetInt16(i);
      }

      /// <summary>
      /// Gets the value of the specified column as a 32-bit signed integer.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      /// <returns>TThe 32-bit signed integer value of the specified field.</returns>
      public int GetInt32(int i)
      {
         return FNativeInterface.GetInt32(i);
      }

      /// <summary>
      /// Gets the 64-bit signed integer value of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The 64-bit signed integer value of the specified field.</returns>
      public long GetInt64(int i)
      {
         return FNativeInterface.GetInt64(i);
      }

      /// <summary>
      /// Gets the name for the field to find.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
      public string GetName(int i)
      {
         return FNativeInterface.GetName(i);
      }

      /// <summary>
      /// Return the index of the named field.
      /// </summary>
      /// <param name="name">The name of the field to find.</param>
      /// <returns>The index of the named field.</returns>
      public int GetOrdinal(string name)
      {
         return FNativeInterface.GetOrdinal(name);
      }

      /// <summary>
      /// Returns a <see cref='DataTable'/> that describes the column metadata of the <see cref='UniDataReader'/>.
      /// </summary>
      /// <returns>A DataTable that describes the column metadata.</returns>
      public DataTable GetSchemaTable()
      {
         return FNativeInterface.GetSchemaTable();
      }

      /// <summary>
      /// Gets the string value of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The string value of the specified field.</returns>
      public string GetString(int i)
      {
         return FNativeInterface.GetString(i);
      }

      /// <summary>
      /// Return the value of the specified field.
      /// </summary>
      /// <param name="i">The index of the field to find.</param>
      /// <returns>The Object which will contain the field value upon return.</returns>
      public object GetValue(int i)
      {
         return FNativeInterface.GetValue(i);
      }

      /// <summary>
      /// Gets all the attribute fields in the collection for the current record.
      /// </summary>
      /// <param name="values">An array of Object to copy the attribute fields into.</param>
      /// <returns>The number of instances of Object in the array.</returns>
      public int GetValues(object[] values)
      {
         return FNativeInterface.GetValues(values);
      }

   #endregion public

   #region properties    
      /// <summary>
      /// Gets a value indicating the depth of nesting for the current row.
      /// </summary>
      public int Depth
      {
         get
         {
            return FNativeInterface.Depth;
         }
      }

      /// <summary>
      /// Gets the number of columns in the current row.
      /// </summary>
      public int FieldCount
      {
         get
         {
            return FNativeInterface.FieldCount;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the data reader is closed.
      /// </summary>
      public bool IsClosed
      {
         get
         {
            return FNativeInterface.IsClosed;
         }
      }

      /// <summary>
      /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
      /// </summary>
      public int RecordsAffected
      {
         get
         {
            return FNativeInterface.RecordsAffected;
         }
      }

      /// <summary>
      /// Gets the value of the specified column in its native format given the column name.
      /// </summary>
      /// <param name="name">The column name.</param>
      public object this[string name]
      {
         get
         {
            return FNativeInterface[name];
         }
      }

      /// <summary>
      /// Gets the column located at the specified index.
      /// </summary>
      /// <param name="i">The zero-based column ordinal.</param>
      object System.Data.IDataRecord.this[int i]
      {
         get
         {
            return FNativeInterface[i];
         }
      }

      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper
      /// </summary>
      public IDataReader NativeInterface { get { return FNativeInterface; } }
   
      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties   
   
   }
   

   
   /// <summary>
   /// Represents a set of data commands and a database connection that are used to fill the <see cref='DataSet'/> and update a database server.
   /// </summary>
   public class UniDataAdapter : IUniDataAdapter
   {
   
   #region private
		
      private IDbDataAdapter FNativeInterface;  
      private UniConnection FConnection;
      private UniCommand FSelectCmd;
      private UniCommand FDeleteCmd;
      private UniCommand FUpdateCmd;
      private UniCommand FInsertCmd;
      
      private void SetAdapter(UniCommand Command)
      {
         if (Command != null && Command.Connection != null && FNativeInterface == null) 
         {
            UniProvider Provider = Command.Provider;         
            FNativeInterface = (IDbDataAdapter) Provider.Assembly.CreateInstance(Provider.DataAdapterClassName);             
         }    
      }
   #endregion private

   #region constructors

      /// <summary>
      /// Initializes a new instance of the <see cref='UniDataAdapter'/> class.
      /// </summary>
      private UniDataAdapter()
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Connection"></param>
      public UniDataAdapter(UniConnection Connection)
      {   
         this.FConnection = Connection;               
      }  
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Connection"></param>
      /// <param name="SelectCommand"></param>
      public UniDataAdapter(UniConnection Connection, UniCommand SelectCommand)
      {       
         this.FConnection = Connection;  
         this.SelectCommand = SelectCommand;           
      } 
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Connection"></param>
      /// <param name="SelectCommandText"></param>
      public UniDataAdapter(UniConnection Connection, string SelectCommandText)
      { 
         this.FConnection = Connection;  
         this.SelectCommand = new UniCommand(Connection, SelectCommandText);                 
      } 
      
      /// <summary>
      /// Initializes a new instance of the <see cref='UniDataAdapter'/> class with the specified <see cref='UniCommand'/> as the SelectCommand property.
      /// </summary>
      /// <param name="SelectCommand">A <see cref='UniCommand'/> that is a SQL SELECT statement or stored procedure and is set as the SelectCommand property of the <see cref='UniDataAdapter'/>.</param>
      public UniDataAdapter(UniCommand SelectCommand)
      {
         this.FConnection = SelectCommand.Connection;  
         this.SelectCommand = SelectCommand;
      }  
                                       


   #endregion constructors

   #region public

      /// <summary>
      /// Adds or refreshes rows in the DataSet to match those in the data source using the <see cref='DataSet'/> name, and creates a <see cref='DataTable'/> named "Table".
      /// </summary>
      /// <param name="dataSet">A <see cref='DataSet'/> to fill with records and, if necessary, schema.</param>
      /// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
      public int Fill(DataSet dataSet)
      {
         if (FNativeInterface==null) SetAdapter(FSelectCmd);
         if (FNativeInterface==null) 
            throw new Exception("DataAdapter cannot be determined.Check select command and select command's connection");
         return FNativeInterface.Fill(dataSet);
      }

      /// <summary>
      /// Adds or refreshes rows in the <see cref='DataSet'/> to match those in the data source using the <see cref='DataSet'/> and <see cref='DataTable'/> names.
      /// </summary>
      /// <param name="dataSet">A <see cref='DataSet'/> to fill with records and, if necessary, schema.</param>
      /// <param name="tableName">The name of the source table to use for table mapping.</param>
      /// <returns>The number of rows successfully added to or refreshed in the <see cref='DataSet'/>. This does not include rows affected by statements that do not return rows.</returns>
      public int Fill(DataSet dataSet, string tableName)
      {
         return ((DbDataAdapter) FNativeInterface).Fill(dataSet, tableName);
      }

      /// <summary>
      /// Gets the parameters set by the user when executing an SQL SELECT statement.
      /// </summary>
      /// <returns>An array of IDataParameter objects that contains the parameters set by the user.</returns>
      public IDataParameter[] GetFillParameters()
      {
         return FNativeInterface.GetFillParameters();
      }

      /// <summary>
      /// Adds a <see cref='DataTable'/> named "Table" to the specified <see cref='DataSet'/> and configures the schema to match that in the data source based on the specified <see cref='SchemaType'/>.
      /// </summary>
      /// <param name="dataSet">A <see cref='DataSet'/> to insert the schema in.</param>
      /// <param name="schemaType">One of the <see cref='SchemaType'/> values that specify how to insert the schema.</param>
      /// <returns>A reference to a collection of <see cref='DataTable'/> objects that were added to the <see cref='DataSet'/>.</returns>
      public DataTable[] FillSchema(DataSet dataSet, System.Data.SchemaType schemaType)
      {
         return FNativeInterface.FillSchema(dataSet,schemaType);
      }

      /// <summary>
      /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified <see cref='DataSet'/>.
      /// </summary>
      /// <param name="dataRow">A DataRow object used to update the data source.</param>
      /// <returns>The number of rows successfully updated from the <see cref='DataSet'/>.</returns>
      public int Update(DataRow dataRow)
      {
         DataRow[] rows = {dataRow};
         return ((DbDataAdapter) FNativeInterface).Update(rows);
      }

      /// <summary>
      /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified <see cref='DataSet'/>.
      /// </summary>
      /// <param name="dataRows">An array of DataRow objects used to update the data source.</param>
      /// <returns>The number of rows successfully updated from the <see cref='DataSet'/>.</returns>
      public int Update(DataRow[] dataRows)
      {
         return ((DbDataAdapter) FNativeInterface).Update(dataRows);
      }

      /// <summary>
      /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified <see cref='DataSet'/>.
      /// </summary>
      /// <param name="dataSet">The <see cref='DataSet'/> used to update the data source.</param>
      /// <returns>The number of rows successfully updated from the <see cref='DataSet'/>.</returns>
      public int Update(DataSet dataSet)
      {
         return FNativeInterface.Update(dataSet);
      }

      /// <summary>
      /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified <see cref='DataSet'/>.
      /// </summary>
      /// <param name="dataSet">A <see cref='DataSet'/> to fill with records and, if necessary, schema.</param>
      /// <param name="tableName">The name of the source table to use for table mapping.</param>
      /// <returns>The number of rows successfully updated from the <see cref='DataSet'/>.</returns>
      public int Update(DataSet dataSet, string tableName)
      {
         return ((DbDataAdapter) FNativeInterface).Update(dataSet, tableName);
      }

   #endregion public
       
   #region properties
      /// <summary>
      /// 
      /// </summary>
      public UniConnection Connection { get { return FConnection; } }
      /// <summary>
      /// Gets or sets a SQL statement or stored procedure used to update records in the data source.
      /// </summary>
      IDbCommand System.Data.IDbDataAdapter.UpdateCommand
      {
         get
         {
            return FUpdateCmd;
         }
         set
         {
            FUpdateCmd = (UniCommand)value;
            SetAdapter(FUpdateCmd);
            FNativeInterface.UpdateCommand = FUpdateCmd.NativeInterface;					
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure used to update records in the data source.
      /// </summary>
      public UniCommand UpdateCommand
      {
         get
         {
            return (UniCommand)((IDbDataAdapter)this).UpdateCommand;
         }
         set
         {
            ((IDbDataAdapter)this).UpdateCommand=value;
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure used to select records in the data source.
      /// </summary>
      IDbCommand System.Data.IDbDataAdapter.SelectCommand
      {
         get
         {
            return FSelectCmd;
         }
         set
         {
            FSelectCmd = (UniCommand)value;
            SetAdapter(FSelectCmd);
            FNativeInterface.SelectCommand = FSelectCmd.NativeInterface;					
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure used to select records in the data source.
      /// </summary>
      public UniCommand SelectCommand
      {
         get
         {
            return (UniCommand)((IDbDataAdapter)this).SelectCommand;
         }
         set
         {
            ((IDbDataAdapter)this).SelectCommand=value;
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure to delete records from the data set.
      /// </summary>
      IDbCommand System.Data.IDbDataAdapter.DeleteCommand
      {
         get
         {
            return FDeleteCmd;
         }
         set
         {
            FDeleteCmd = (UniCommand)value;
            SetAdapter(FDeleteCmd);
            FNativeInterface.DeleteCommand = FDeleteCmd.NativeInterface;					
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure to delete records from the data set.
      /// </summary>
      public UniCommand DeleteCommand
      {
         get
         {
            return (UniCommand)((IDbDataAdapter)this).DeleteCommand;
         }
         set
         {
            ((IDbDataAdapter)this).DeleteCommand=value;
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure to insert new records into the data source.
      /// </summary>
      IDbCommand System.Data.IDbDataAdapter.InsertCommand
      {
         get
         {
            return FInsertCmd;
         }
         set
         {
            FInsertCmd = (UniCommand)value;
            SetAdapter(FInsertCmd);
            FNativeInterface.InsertCommand = FInsertCmd.NativeInterface;					
         }
      }

      /// <summary>
      /// Gets or sets a SQL statement or stored procedure to insert new records into the data source.
      /// </summary>
      public UniCommand InsertCommand
      {
         get
         {
            return (UniCommand)((IDbDataAdapter)this).InsertCommand;
         }
         set
         {
            ((IDbDataAdapter)this).InsertCommand=value;
         }
      }

      /// <summary>
      /// Gets a collection that provides the master mapping between a source table and a DataTable.
      /// </summary>
      public ITableMappingCollection TableMappings
      {
         get
         {
            return FNativeInterface.TableMappings;
         }
      }

      /// <summary>
      /// Determines the action to take when existing <see cref='DataSet'/> schema does not match incoming data.
      /// </summary>
      public System.Data.MissingSchemaAction MissingSchemaAction
      {
         get
         {
            return FNativeInterface.MissingSchemaAction;
         }
         set
         {
            FNativeInterface.MissingSchemaAction=value;
         }
      }

      /// <summary>
      /// Determines the action to take when incoming data does not have a matching table or column.
      /// </summary>
      public System.Data.MissingMappingAction MissingMappingAction
      {
         get
         {
            return FNativeInterface.MissingMappingAction;
         }
         set
         {
            FNativeInterface.MissingMappingAction=value;
         }
      }

      /// <summary>
      /// Returns the .Net native interface for which this class is a wrapper
      /// </summary> 
      public IDbDataAdapter NativeInterface { get {return FNativeInterface; } }
   
      /// <summary>
      /// Returns the .Net native object for which this class is a wrapper.
      /// </summary>
      public object NativeObject { get { return FNativeInterface; } }
   #endregion properties
    
   } 
   
   
   
   
   
   
   
   

   
   
   
   
   
   
   
   


       
   /// <summary>
   /// A static class that represents a store of available Providers.
   /// </summary>
   public sealed class ProviderStore
   {
      private static ArrayList FList = new ArrayList(); 
      private static char FParamPrefix = ':';            
       
       
      /// <summary>
      /// The static constructor of this class
      /// </summary>
      static ProviderStore()
      {
      }     
      
      
      
      /// <summary>
      /// Searches for an instance of a Provider by ProviderName.
      /// </summary>
      /// <param name="ProviderName">The  name of a Provider.</param>
      /// <returns>The Provider if a match is find, else null</returns>
      static public  UniProvider Find(string ProviderName)
      {
         UniProvider Temp = null;
         for (int i = 0; i < FList.Count; i++)
         {
            Temp = (UniProvider)FList[i];
            if (Temp.NameIs(ProviderName))
               return Temp;
         }
                  
         return null;
      }    
      /// <summary>
      /// Returns UniProvider with Name. Exception if not exists.
      /// </summary>
      static public UniProvider ByName(string ProviderName)
      {
         UniProvider Res = Find(ProviderName);
         if (Res == null)
            throw new ApplicationException(string.Format("UniProvider not found: {0}", ProviderName));
         return Res;
      }       
                
      /// <summary>
      /// Returns UniProvider at Index. Exception if not exists.
      /// </summary>
      static public UniProvider ByIndex(int Index)
      {
         return (UniProvider)FList[Index];
      }
      
      /// <summary>
      /// Determines whether a Provider is in the Providers list. 
      /// </summary>
      static public bool Contains(string ProviderName)
      {
         return Find(ProviderName) != null;
      }               

      /// <summary>
      /// Adds the Provider to the internal list.
      /// </summary>
      static public void Add(UniProvider Provider)
      {
         if (!Contains(Provider.GetType().ToString()))
            FList.Add(Provider);
      }
      
      /// <summary>
      /// Adds the ProviderClasses array to the available Providers.
      /// Actually it creates a Provider instance for each Provider class added.
      /// </summary>
      static public void AddProviders(Type[] ProviderClasses)
      {
         for (int i = 0; i < ProviderClasses.Length; i++)
            try
            {
               Activator.CreateInstance(ProviderClasses[i]);
            }
            catch
            {
            }
      }       
      /// <summary>
      /// Adds the default Providers to the internal list.
      /// </summary>
      static public void AddProviders()
      {
         Type[] ProviderTypes = new Type[] {  typeof(ProviderMSSQL)
                                            , typeof(ProviderFirebird)
                                            , typeof(ProviderOLEDB)
                                            //, typeof(ProviderODBC)
                                            //, typeof(ProviderOracleClient)
                                            //, typeof(ProviderOracle)
                                            };
         ProviderStore.AddProviders(ProviderTypes);
      }
      
                                
      
      /// <summary>
      /// Gets the number of Engines.
      /// </summary>
      static public  int Count { get {return FList.Count;} }      
      /// <summary>
      /// Gets or sets the ParamPrefix to be used globally for this application.
      /// UniProviders provide methods for replacing their native ParamPrefix with this prefix.
      /// </summary>
      static public  char ParamPrefix
      {
         get { return FParamPrefix; }
         set { FParamPrefix = value; }
      } 
      
   }
   
   
   /// <summary>
   /// Represents a specific database connection technology.
   /// </summary>
   public abstract class UniProvider
   {
      
      static private string FNewLine = "\r\n";
      /// <summary>
      /// The Name of this UniProvider, ie MSSQL.
      /// </summary>
      protected string FName = "";
      /// <summary>
      /// The Description of this UniProvider.
      /// </summary>
      protected string FDescription = "";
      /// <summary>
      /// The ParamPrefix the native .Net Provider uses.
      /// </summary>
      protected char   FParamPrefix = ':';           
      /// <summary>
      /// The Assembly where the .Net Provider resides.
      /// </summary>
      protected System.Reflection.Assembly FAssembly;   
      /// <summary>
      /// The full namespace of the Assembly.
      /// </summary>            
      protected string FAssemblyName;
      /// <summary>
      /// The fully qualified ClassName of the Connection class for the .Net native Provider.
      /// </summary>
      protected string FConnectionClassName;
      /// <summary>
      /// The fully qualified ClassName of the Adapter class for the .Net native Provider.
      /// </summary>
      protected string FDataAdapterClassName;
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>
      protected abstract void Initialize();     
      /// <summary>
      /// Normalizes Param to use the native prefix for the .Net Provider.
      /// </summary>
      protected virtual string NormalizeParamPrefix(string Param)
      {
         return FParamPrefix + Param;
      }    
  
  



      /// <summary>
      /// constructor.
      /// </summary>
      public UniProvider()
      {
         Initialize();
         FAssembly = System.Reflection.Assembly.LoadWithPartialName(FAssemblyName); 
         //FAssembly = System.Reflection.Assembly.LoadFrom(FAssemblyName + ".dll"); 
         ProviderStore.Add(this);          
      }
      
      
      
      /// <summary>
      /// Returns true if AName equals case-insensitively to Name.
      /// </summary>
      public bool NameIs(string AName)
      {
         return (string.Compare(FName, AName, true) == 0);
      }
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="ConnectionString"></param>
      /// <returns></returns>
      public UniConnection CreateConnection(string ConnectionString)
      {                                         		
         return new UniConnection(this, ConnectionString);  			
      } 

      /// <summary>
      /// Normalizes CommandText to use the ParamPrefix of the native .Net Provider this UniProvider represents.
      /// </summary>
      public string NormalizeCommandText(string CommandText)
      {

         System.Text.StringBuilder SQL = new System.Text.StringBuilder();  
         bool IsVariable = false;
                                  
         Tokenizer Tokenizer = new Tokenizer();
         Tokenizer.SetString(CommandText);
        
         Token T = null;
         

         
            while (true)
            {
               T = Tokenizer.NextToken();

               if (T.Kind == Token.TT_EOF)
                  return SQL.ToString();
               else if (T.Kind == Token.TT_NEWLINE)
                  SQL.Append(Environment.NewLine);                  
               else if (T.Kind == Token.TT_SYMBOL)
               {
                  IsVariable = (T.AsString.IndexOf(ProviderStore.ParamPrefix) == 0);
                  if (!IsVariable)
                     SQL.Append(T.AsString);
               }
               else if (T.Kind == Token.TT_WORD)
               {
                  if (IsVariable)
                  {
                     SQL.Append(NormalizeParamPrefix(T.AsString));
                     IsVariable = false;
                  }
                  else SQL.Append(T.AsString);               
               }               
               else 
                  SQL.Append(T.AsString);
           

                              
            }
         

      }
      
      /// <summary>
      /// Normalizes the text of the Command to use the ParamPrefix of the native .Net Provider this UniProvider represents.
      /// </summary>
      public string NormalizeCommandText2(UniCommand Command)
      {

         System.Text.StringBuilder SQL = new System.Text.StringBuilder();  
         bool IsVariable = false;
                                  
         Tokenizer Tokenizer = new Tokenizer();
         Tokenizer.SetString(Command.CommandText);
         
         Command.Parameters.Clear();
        
         Token T = null;
         

         
         while (true)
         {
            T = Tokenizer.NextToken();

            if (T.Kind == Token.TT_EOF)
               return SQL.ToString();
            else if (T.Kind == Token.TT_NEWLINE)
               SQL.Append(NewLine);            
            else if (T.Kind == Token.TT_SYMBOL)
            {
               IsVariable = (T.AsString.IndexOf(ProviderStore.ParamPrefix) == 0);
               if (!IsVariable)
                  SQL.Append(T.AsString);                 
            }
            else if (T.Kind == Token.TT_WORD)
            {
               if (IsVariable)
               {
                  SQL.Append(NormalizeParamPrefix(T.AsString));
                  IsVariable = false;
                  
                  UniParameter Param = UniParameter.Create(Command);
                  Command.Parameters.Add(Param);
                  Param.ParameterName = T.AsString;                   
                  
               }
               else SQL.Append(T.AsString);               
            }               
            else 
               SQL.Append(T.AsString);
           

                              
         }
         

      }
      /// <summary>
      /// Normalizes the ParameterName to be what the .Net native Provider expects.
      /// </summary>
      public virtual string NormalizeParameterName(string ParameterName)
      {          
        if (ParameterName.StartsWith(ProviderStore.ParamPrefix.ToString()))
            ParameterName = FParamPrefix + ParameterName.Substring(1, ParameterName.Length - 1);
         else if (!ParameterName.StartsWith(FParamPrefix.ToString()))
            ParameterName = FParamPrefix + ParameterName;
          
         return ParameterName;
      }






      /// <summary>
      /// Gets the Name of this UniProvider, ie MSSQL.
      /// </summary>         
      public string Name { get { return  FName; } }  
      /// <summary>
      /// Gets the Description of this UniProvider.
      /// </summary>         
      public string Description { get { return  FDescription; } }
      /// <summary>
      /// Gets the ParamPrefix the native .Net Provider uses.
      /// </summary>
      public char ParamPrefix { get { return  FParamPrefix; } }
      /// <summary>
      /// Gets the Assembly where the native .Net Provider resides.
      /// </summary>
      public System.Reflection.Assembly Assembly { get { return  FAssembly; } }
      /// <summary>
      /// Gets the fully qualified namespace path of the Assembly of the native .Net Provider. 
      /// </summary>
      public string AssemblyName { get { return  FAssemblyName; } }
      /// <summary>
      /// Gets the fully qualified ClassName of the Connection class for the .Net native Provider.
      /// </summary>
      public string ConnectionClassName { get { return  FConnectionClassName; } }
      /// <summary>
      /// Gets the fully qualified ClassName of the Adapter class for the .Net native Provider.
      /// </summary>
      public string DataAdapterClassName { get { return  FDataAdapterClassName; } }



		/// <summary>
		/// 
		/// </summary>		   
		public static string NewLine
		{
			get { return FNewLine; }
			set { FNewLine = value;}
		}

   }
   
   
   
   
   
   
   /// <summary>
   /// 
   /// </summary>
   public class ProviderMSSQL: UniProvider
   {    
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>
      protected override void Initialize()
      {
         FName = "MSSQL";
         FDescription  = "Microsoft SQL Server 7.0/2000";
         FParamPrefix  = '@';
         FAssemblyName = "System.Data";
         FConnectionClassName  = "System.Data.SqlClient.SqlConnection";
         FDataAdapterClassName  = "System.Data.SqlClient.SqlDataAdapter";
     
      }  
   }
   
   
   /// <summary>
   /// 
   /// </summary>
   public class ProviderFirebird: UniProvider
   {    
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>
      protected override void Initialize()
      {
         FName = "Firebird";
         FDescription  = "Firebird";
         FParamPrefix  = '@';
         FAssemblyName = "FirebirdSql.Data.Firebird";
         FConnectionClassName  = "FirebirdSql.Data.Firebird.FbConnection";
         FDataAdapterClassName  = "FirebirdSql.Data.Firebird.FbDataAdapter";
         //Con = new FbConnection("User=SYSDBA;Password=masterkey;DataSource=localhost;Database=C:\\BUGTRACK.GDB");
      }  
   }   
   
   
   /// <summary>
   /// 
   /// </summary>
   public class ProviderOLEDB: UniProvider
   {  
      /// <summary>
      /// Normalizes Param to use the native prefix for the .Net Provider.
      /// </summary>
      protected override string NormalizeParamPrefix(string Param)
      {
         return FParamPrefix.ToString();
      } 
        
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>  
      protected override void Initialize()
      {
         FName = "OLEDB";
         FDescription  = "OLE DB Data Provider";
         FParamPrefix  = '?';
         FAssemblyName = "System.Data";
         FConnectionClassName  = "System.Data.OleDb.OleDbConnection";
         FDataAdapterClassName  = "System.Data.OleDb.OleDbDataAdapter";
     
      }  
      /// <summary>
      /// Normalizes the ParameterName to be what the .Net native Provider expects.
      /// </summary>
      public override string NormalizeParameterName(string ParameterName)
      {
         return ParameterName;
      }
   }
 
   
   
   
   /// <summary>
   /// 
   /// </summary>
   public class ProviderODBC: UniProvider
   {    
      /// <summary>
      /// Normalizes Param to use the native prefix for the .Net Provider.
      /// </summary>
      protected override string NormalizeParamPrefix(string Param)
      {
         return FParamPrefix.ToString();
      } 
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>
      protected override void Initialize()
      {
         FName = "ODBC";
         FDescription  = "ODBC Data Provider";
         FParamPrefix  = '?';
         FAssemblyName = "System.Data";
         FConnectionClassName  = "System.Data.Odbc.OdbcConnection";
         FDataAdapterClassName  = "System.Data.Odbc.OdbcDataAdapter";
     
      }  
      /// <summary>
      /// Normalizes the ParameterName to be what the .Net native Provider expects.
      /// </summary>
      public override string NormalizeParameterName(string ParameterName)
      {
         return ParameterName;
      }
   }
   
 
   
   
   /// <summary>
   /// 
   /// </summary>
   public class ProviderOracleClient: UniProvider
   {    
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>
      protected override void Initialize()
      {
         FName = "OracleClient";
         FDescription  = "Oracle (Microsoft version) Provider";
         FParamPrefix  = ':';
         FAssemblyName = "System.Data.OracleClient";
         FConnectionClassName  = "System.Data.OracleClient.OracleConnection";
         FDataAdapterClassName  = "System.Data.OracleClient.OracleDataAdapter";
     
      }  
   }
   
   
   
   
   /// <summary>
   /// 
   /// </summary>
   public class ProviderOracle: UniProvider
   {    
      /// <summary>
      /// Initializes a instance by assinging valid values to its properties.
      /// </summary>
      protected override void Initialize()
      {
         FName = "Oracle";
         FDescription  = "Oracle 9i Provider";
         FParamPrefix  = ':';
         FAssemblyName = "Oracle.DataAccess";
         FConnectionClassName  = "Oracle.DataAccess.Client.OracleConnection";
         FDataAdapterClassName  = "Oracle.DataAccess.Client.OracleDataAdapter";
     
      }  
   }
   
   

   

}
