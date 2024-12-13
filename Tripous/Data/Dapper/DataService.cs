namespace Tripous.Data
{
 
    /// <summary>
    /// A data service executes CRUD operations to the database.
    /// <para>NOTE: A data service has no state at all.</para>
    /// </summary>
    public class DataService<T> where T : DataEntity
    {
        SqlStore fStore;

        /* protected */
        /// <summary>
        /// The type of the entity this service persists in the database.
        /// </summary>
        protected Type EntityType = typeof(T);
        /// <summary>
        /// The descriptor of the entity
        /// </summary>
        protected EntityDescriptor Descriptor = EntityDescriptors.Find(typeof(T));
 
        /// <summary>
        /// Throws an exception if a specified entity has compound primary key
        /// </summary>
        protected void CheckForCompoundKey(EntityDescriptor Descriptor)
        {
            if (Descriptor.DetailLists.Count > 0 && Descriptor.PrimaryKeyList.Count > 1)
                Sys.Throw($"{EntityType.Name} Entity has a compound primary key");
        }

        /* overridables */
        /// <summary>
        /// Called from GetById() to select relational lists, aka details.
        /// </summary>
        protected virtual async Task SelectDetails(DbConnection Con, DataEntity MasterEntity, EntityDescriptor Descriptor)
        {

            if (Descriptor.DetailLists.Count > 0)
            {
                CheckForCompoundKey(Descriptor);

                object MasterId = Descriptor.GetPrimaryKeyValue(MasterEntity);

                EntityDescriptor DetailDescriptor;
                DynamicParameters Params;
                string SqlText;

                // select and add details to master
                foreach (var DetailRelation in Descriptor.DetailLists)
                {
                    DetailDescriptor = DetailRelation.DetailDescriptor;

                    CheckForCompoundKey(DetailDescriptor);

                    Params = new DynamicParameters();
                    Params.Add(DetailRelation.DetailKeyFieldDescriptor.FieldName, MasterId);

                    SqlText = DetailRelation.SelectDetailsSql;
                    var ResultList = await Con.QueryAsync(DetailRelation.DetailEntityType, SqlText, Params);

                    foreach (var Item in ResultList)
                        Trim(Item as DataEntity, DetailDescriptor);

                    DetailRelation.AddDetailEntities(MasterEntity, ResultList);

                    // now each detail becomes a master
                    foreach (DataEntity DetailEntity in ResultList)
                    {
                        //DetailDescriptor = Tables.Get(DetailEntity.GetType());
                        await SelectDetails(Con, DetailEntity, DetailDescriptor);
                    }
                }
            }

        }
        /// <summary>
        /// Selects relational properties
        /// </summary>
        protected virtual async Task SelectRelationals(DbConnection Con, DataEntity MasterEntity, EntityDescriptor Descriptor)
        {            
            if (Descriptor.Relationals.Count > 0)
            {
                string SqlText;

                foreach (var Relational in Descriptor.Relationals)
                {
                    CheckForCompoundKey(Relational.ForeignTableDescriptor);

                    var Params = new DynamicParameters();

                    var KeyField = Relational.KeyField;
                    if (KeyField != null)
                    {
                        var KeyValue = KeyField.GetValue(MasterEntity);

                        if (KeyValue != null)
                        {
                            Params.Add(Relational.ForeignTableDescriptor.PrimaryKeyList[0].FieldName, KeyValue);

                            SqlText = Relational.ForeignTableDescriptor.SelectRowSql;
                            var Result = await Con.QuerySingleOrDefaultAsync(Relational.RelationalProperty.PropertyType, SqlText, Params);

                            Trim(Result as DataEntity, Relational.ForeignTableDescriptor);

                            Relational.RelationalProperty.SetValue(MasterEntity, Result);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called from GetByIds() to select details.
        /// </summary>
        protected virtual async Task SelectDetailsWithCompoundKey(DbConnection Con, T Entity, object[] Ids)
        {
            await Task.FromResult(0); // prevent a compiler warning
        }
        /// <summary>
        /// Selects relational properties
        /// </summary>
        protected virtual async Task SelectRelationalsWithCompoundKey(DbConnection Con, T Entity, object[] Ids)
        {
            await Task.FromResult(0); // prevent a compiler warning
        }

        /// <summary>
        /// Inserts an entity to the database.
        /// </summary>
        protected virtual async Task InsertEntity(DbConnection Con, DataEntity Entity, EntityDescriptor Descriptor, object MasterId = null, DetailListDescriptor DetailRelation = null)
        {
            CheckForCompoundKey(Descriptor);

            if (MasterId != null && DetailRelation != null)
            {
                PropDescriptor DetailKeyFieldDescriptor = DetailRelation.DetailKeyFieldDescriptor;
                DetailKeyFieldDescriptor.SetValue(Entity, MasterId);
            }

            if (Generators.Contains(Descriptor.EntityType))
                Generators.Execute(Con, Entity);

            StringBuilder Errors = Entity.BeforeSaveCheck(true);
            if (Errors.Length > 0)
                Sys.Throw(Errors.ToString());

            var Params = Descriptor.CreateParams(Entity);

            if (Descriptor.Autoincrement && Descriptor.Provider.ServerType == SqlServerType.MsSql)
            {
                PropDescriptor FieldDes = Descriptor.PrimaryKeyList[0];

                StringBuilder SB = new StringBuilder(Descriptor.InsertRowSql);
                SB.AppendLine(";");
                SB.AppendLine(string.Format("SELECT CAST(SCOPE_IDENTITY() AS INT) as {0}", FieldDes.FieldName));

                object NewEntity = await Con.QuerySingleAsync(Entity.GetType(), SB.ToString(), Params);
                object Id = FieldDes.Property.GetValue(NewEntity);
                FieldDes.Property.SetValue(Entity, Id);
            }
            else
            {
                await Con.ExecuteAsync(Descriptor.InsertRowSql, Params);
            }

            await InsertDetails(Con, Entity, Descriptor);
        }
        /// <summary>
        /// Called from Insert() to insert details
        /// </summary>
        protected virtual async Task InsertDetails(DbConnection Con, DataEntity MasterEntity, EntityDescriptor MasterDescriptor)
        {
            if (MasterDescriptor.DetailLists.Count > 0)
            {
                object MasterId = MasterDescriptor.GetPrimaryKeyValue(MasterEntity);

                IEnumerable Details;
                foreach (var DetailRelation in MasterDescriptor.DetailLists)
                {
                    Details = DetailRelation.GetListAsEnumerable(MasterEntity);

                    // set the MasterId to the detail and INSERT it
                    foreach (DataEntity DetailEntity in Details)
                    {
                        await InsertEntity(Con, DetailEntity, DetailRelation.DetailDescriptor, MasterId, DetailRelation);                        
                    }
                }
            }
        }

        /// <summary>
        /// Updates an entity to the database
        /// </summary>
        protected virtual async Task UpdateEntity(DbConnection Con, DataEntity Entity, DataEntity OriginalEntity, EntityDescriptor Descriptor)
        {
            CheckForCompoundKey(Descriptor);
 
            StringBuilder Errors = Entity.BeforeSaveCheck(false);
            if (Errors.Length > 0)
                Sys.Throw(Errors.ToString());

            await UpdateDetails(Con, Entity, OriginalEntity, Descriptor);

            var Params = Descriptor.CreateParams(Entity);

            int AffectedRows = await Con.ExecuteAsync(Descriptor.UpdateRowSql, Params);
            if (AffectedRows <= 0)
                Sys.Throw($"{Descriptor.EntityName} not updated. Reason: not found in database"); 
        }
        /// <summary>
        /// Called from Update() to update details.
        /// <para>This step could be tricky as some details may be updated, some may be inserted and some may be deleted.</para>
        /// </summary>
        protected virtual async Task UpdateDetails(DbConnection Con, DataEntity MasterEntity, DataEntity OriginalMasterEntity, EntityDescriptor MasterDescriptor)
        {

            if (MasterDescriptor.DetailLists.Count > 0)
            {
                object MasterId = MasterDescriptor.GetPrimaryKeyValue(MasterEntity);

                DataClassifier Classifier;
                foreach (var DetailRelation in MasterDescriptor.DetailLists)
                {
                    Classifier = DataClassifier.Classify(MasterEntity, OriginalMasterEntity, MasterDescriptor, DetailRelation);

                    // deletions
                    foreach (var Deleted in Classifier.Deletes)
                    {
                        await DeleteEntity(Con, Deleted, DetailRelation.DetailDescriptor);
                    }

                    // updates
                    foreach (var UpdatedPair in Classifier.Updates)
                    {
                        await UpdateEntity(Con, UpdatedPair.Entity, UpdatedPair.OriginalEntity, DetailRelation.DetailDescriptor);
                    }

                    // insertions                   
                    foreach (var Inserted in Classifier.Inserts)
                    {
                        await InsertEntity(Con, Inserted, DetailRelation.DetailDescriptor, MasterId, DetailRelation);
                    }
                }
            }

            await Task.FromResult(0); // prevent a compiler warning
        }

        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        protected virtual async Task DeleteEntity(DbConnection Con, DataEntity Entity, EntityDescriptor Descriptor)
        {
            await DeleteDetails(Con, Entity, Descriptor);
            var Params = Descriptor.CreateParams(Entity);
            await Con.ExecuteAsync(Descriptor.DeleteRowSql, Params);
        }
        /// <summary>
        /// Deletes details entities
        /// </summary>
        protected virtual async Task DeleteDetails(DbConnection Con, DataEntity MasterEntity, EntityDescriptor MasterDescriptor)
        {
            if (MasterDescriptor.DetailLists.Count > 0)
            {
                object MasterId = MasterDescriptor.GetPrimaryKeyValue(MasterEntity);

                IEnumerable Details;
                EntityDescriptor DetailDescriptor;
                PropDescriptor DetailKeyFieldDescriptor;
                DynamicParameters Params;

                foreach (var DetailRelation in MasterDescriptor.DetailLists)
                {
                    Details = DetailRelation.GetListAsEnumerable(MasterEntity);
                    DetailDescriptor = DetailRelation.DetailDescriptor;
                    DetailKeyFieldDescriptor = DetailRelation.DetailKeyFieldDescriptor;

                    // set the MasterId to the detail and INSERT it
                    foreach (DataEntity DetailEntity in Details)
                    {
                        await DeleteDetails(Con, DetailEntity, DetailRelation.DetailDescriptor);
                    }

                    Params = new DynamicParameters();
                    Params.Add(DetailRelation.DetailKeyFieldDescriptor.FieldName, MasterId);
                    await Con.ExecuteAsync(DetailRelation.DeleteDetailsSql, Params);
                }               
            }
        }



        /// <summary>
        /// Trims field values of char database type.
        /// </summary>
        protected virtual void Trim(DataEntity Entity, EntityDescriptor Descriptor)
        {
            if (Entity != null)
            {
                string v;
                foreach (var Field in Descriptor.Fields)
                {
                    if (Field.TrimRequired)
                    {
                        v = Field.GetValue(Entity) as string;
                        if (!string.IsNullOrWhiteSpace(v) && !v.StartsWith(" "))
                        {
                            v = v.TrimEnd();
                            Field.SetValue(Entity, v);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Trims field values of char database type.
        /// </summary>
        protected virtual void Trim(T Entity)
        {
            Trim(Entity, Descriptor);
        }

        /* overridables - before-after */

        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void BeforeNewEntity()
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void AfterNewEntity(T Entity)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void BeforeGetAll()
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void AfterGetAll(List<T> List)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void BeforeGetByMasterId(object MasterId)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void AfterGetByMasterId(List<T> List)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void BeforeGetByFilterAsync(WhereSql Filter)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void AfterGetByFilterAsync(List<T> List)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void BeforeGetById(object Id)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void AfterGetById(T Entity)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void BeforeGetByIds(params object[] Ids)
        {
        }
        /// <summary>
        /// For inheritors
        /// </summary>
        protected virtual void AfterGetByIds(T Entity)
        {
        }

        /// <summary>
        /// For inheritors
        /// <para>CAUTION: This method calls any entity event listeners, therefore should always be called. </para>
        /// </summary>
        protected virtual void BeforeInsert(T Entity)
        {
            EntityEvents.Send(Entity, EntityEvents.SBeforeInsert);
        }
        /// <summary>
        /// For inheritors
        /// <para>CAUTION: This method calls any entity event listeners, therefore should always be called. </para>
        /// </summary>
        protected virtual void AfterInsert(T Entity)
        {
            EntityEvents.Send(Entity, EntityEvents.SAfterInsert);
        }
        /// <summary>
        /// For inheritors
        /// <para>CAUTION: This method calls any entity event listeners, therefore should always be called. </para>
        /// </summary>
        protected virtual void BeforeUpdate(T Entity)
        {
            EntityEvents.Send(Entity, EntityEvents.SBeforeUpdate);
        }
        /// <summary>
        /// For inheritors
        /// <para>CAUTION: This method calls any entity event listeners, therefore should always be called. </para>
        /// </summary>
        protected virtual void AfterUpdate(T Entity)
        {
            EntityEvents.Send(Entity, EntityEvents.SAfterUpdate);
        }
        /// <summary>
        /// For inheritors
        /// <para>CAUTION: This method calls any entity event listeners, therefore should always be called. </para>
        /// </summary>
        protected virtual void BeforeDelete(T Entity)
        {
            EntityEvents.Send(Entity, EntityEvents.SBeforeDelete);
        }
        /// <summary>
        /// For inheritors
        /// <para>CAUTION: This method calls any entity event listeners, therefore should always be called. </para>
        /// </summary>
        protected virtual void AfterDelete(T Entity)
        {
            EntityEvents.Send(Entity, EntityEvents.SAfterDelete);
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataService()
        {
            if (Descriptor == null)
                Sys.Throw("No Descriptor for Entity: {0}", EntityType.Name);
        }



        // https://stackoverflow.com/questions/21406973/wrapping-synchronous-code-into-asynchronous-call
        // 

        /* public */
        /// <summary>
        /// Creates and returns a new entity
        /// </summary>
        public virtual T NewEntity()
        {
            try
            {
                BeforeNewEntity();
                T Result = EntityType.Create() as T;
                AfterNewEntity(Result);
                return Result;
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }
        }

        /// <summary>
        /// Returns all the entities from the database table.
        /// <para>CAUTION: Not all Entities support this call.</para>
        /// </summary>
        public virtual async Task<List<T>> GetAllAsync()
        {
            List<T> Result = null;

            try
            {
                if (!Bf.In(CRUDMode.GetAll, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: GetAll");

                BeforeGetAll();

                using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                {
                    var Res = await Con.QueryAsync<T>(Descriptor.SelectSql);
                    Result = Res.ToList();

                    foreach (var Entity in Result)
                        Trim(Entity);
                }

                AfterGetAll(Result);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

            return Result;
        }
        /// <summary>
        /// Returns all the entities from the database table that are details of another Entity specified by an id.
        /// <para>CAUTION: Not all Entities support this call.</para>
        /// </summary>
        public virtual async Task<List<T>> GetByMasterIdAsync(object MasterId)
        {
            List<T> Result = null;

            try
            {
                if (!Bf.In(CRUDMode.GetByMasterId, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: GetByMasterId");

                BeforeGetByMasterId(MasterId);

                var Params = new DynamicParameters();
                Params.Add(Descriptor.DetailKeyField.FieldName, MasterId);

                using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                {
                    var Res = await Con.QueryAsync<T>(Descriptor.SelectByMasterIdSql, Params);
                    Result = Res.ToList();

                    foreach (var Entity in Result)
                        Trim(Entity);
                }

                AfterGetByMasterId(Result);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

            return Result;
        }
        /// <summary>
        /// Returns all entities from the database table, based on a specified entity filter.
        /// </summary>
        public virtual async Task<List<T>> GetByFilterAsync(WhereSql Filter)
        {
            List<T> Result = null;

            try
            {
                if (!Bf.In(CRUDMode.GetByFilter, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: GetByFilter");

                Descriptor.ProcessEntityFilter(Filter);

                BeforeGetByFilterAsync(Filter);

                string FilterText = Filter.SqlText;
                if (string.IsNullOrWhiteSpace(FilterText))
                    throw new ApplicationException($"{EntityType.Name}. Can not get by filter. Filter is empty.");

                using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                {
                    StringBuilder SB = new StringBuilder();
                    SB.AppendLine(Descriptor.SelectSql);
                    SB.AppendLine(" where ");
                    SB.AppendLine(FilterText);

                    var Res = await Con.QueryAsync<T>(SB.ToString());
                    Result = Res.ToList();

                    foreach (var Entity in Result)
                        Trim(Entity);
                }

                AfterGetByFilterAsync(Result);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

            return Result;
        }

        /// <summary>
        /// Selects and returns a single entity from the database, based on a specified key (which may be a compound one). Returns null if no entity found.
        /// <para>CAUTION: Not all Entities support this call.</para>
        /// </summary>
        public virtual async Task<T> GetByIdAsync(object Id)
        {
            T Result = null;

            try
            {
                if (!Bf.In(CRUDMode.GetById, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: GetById");

                if (Descriptor.PrimaryKeyList.Count > 1)
                    Sys.Throw($"{EntityType.Name} Entity has a compound primary key");

                BeforeGetById(Id);

                var Params = new DynamicParameters();
                Params.Add(Descriptor.PrimaryKeyList[0].FieldName, Id);

                using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                {
                    Result = await Con.QueryFirstOrDefaultAsync<T>(Descriptor.SelectRowSql, Params);

                    if (Result == null)
                    {
                        Sys.Throw($"{EntityType.Name} Entity not found by Id: {Id}");
                    }

                    Trim(Result);
                    await SelectDetails(Con, Result, Descriptor);
                    await SelectRelationals(Con, Result, Descriptor);
                }

                AfterGetById(Result);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

            return Result;
        }
        /// <summary>
        /// Selects and returns a single entity from the database, based on a specified key (which may be a compound one). Returns null if no entity found.
        /// <para>CAUTION: Not all Entities support this call.</para>
        /// </summary>
        public virtual async Task<T> GetByIdsAsync(params object[] Ids)
        {
            T Result = null;

            try
            {
                if (!Bf.In(CRUDMode.GetById, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: GetById");

                BeforeGetByIds(Ids);

                var Params = new DynamicParameters();
                for (int i = 0; i < Ids.Length; i++)
                {
                    Params.Add(Descriptor.PrimaryKeyList[i].FieldName, Ids[i]);
                }

                using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                {
                    Result = await Con.QueryFirstOrDefaultAsync<T>(Descriptor.SelectRowSql, Params);

                    if (Result == null)
                    {
                        StringBuilder SB = new StringBuilder();
                        SB.Append($"{EntityType.Name} Entity not found by Id(s): ");

                        for (int i = 0; i < Ids.Length; i++)
                        {
                            if (i > 0)
                                SB.Append(", " + Ids[i].ToString());
                            else
                                SB.Append(Ids[i].ToString());
                        }

                        Sys.Throw(SB.ToString());
                    }
                    else
                    {
                        Trim(Result);
                        await SelectDetailsWithCompoundKey(Con, Result, Ids);
                        await SelectRelationalsWithCompoundKey(Con, Result, Ids);
                    }
                }

                AfterGetByIds(Result);

            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

            return Result;
        }

        /// <summary>
        /// Inserts an entity to the database.
        /// </summary>
        public virtual async Task InsertAsync(T Entity)
        {
            try
            {
                if (!Bf.In(CRUDMode.Insert, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: Insert");

                BeforeInsert(Entity);              

                // Transactions: https://stackoverflow.com/questions/2884863/under-what-circumstances-is-an-sqlconnection-automatically-enlisted-in-an-ambient
                using (var TansScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {                    
                    using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                    {
                        await InsertEntity(Con, Entity, Descriptor);
                    }                     

                    TansScope.Complete();
                }

                AfterInsert(Entity);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

        }
        /// <summary>
        /// Updates an entity to the database
        /// </summary>
        public virtual async Task UpdateAsync(T Entity)
        {  
            try
            {
                if (!Bf.In(CRUDMode.Update, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: Update");

                BeforeUpdate(Entity);

                // get the original entity, as it is right now in the database
                object Id = Descriptor.PrimaryKeyList[0].Property.GetValue(Entity);

                T OriginalEntity = null;

                // OriginalEntity is needed only when we have details
                if (Descriptor.DetailLists.Count > 0)
                    OriginalEntity = await GetByIdAsync(Id);

                // Transactions: https://stackoverflow.com/questions/2884863/under-what-circumstances-is-an-sqlconnection-automatically-enlisted-in-an-ambient                
                using (var TansScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                    {
                        await UpdateEntity(Con, Entity, OriginalEntity, Descriptor);
                    }

                    TansScope.Complete();
                }

                AfterUpdate(Entity);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

        }
        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        public virtual async Task DeleteAsync(T Entity)
        {
            try
            {
                if (!Bf.In(CRUDMode.Delete, Descriptor.Mode))
                    throw new NotSupportedException($"{EntityType.Name}. CRUD mode not supported: Delete");

                BeforeDelete(Entity);                

                // Transactions: https://stackoverflow.com/questions/2884863/under-what-circumstances-is-an-sqlconnection-automatically-enlisted-in-an-ambient                
                using (var TansScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var Con = Db.OpenConnection(Descriptor.ConnectionName))
                    {
                        await DeleteEntity(Con, Entity, Descriptor);
                    }

                    TansScope.Complete();
                }

                AfterDelete(Entity);
            }
            catch (Exception ex)
            {
                Sys.LogError(ex, EntityType.Name);
                throw;
            }

        }
        /// <summary>
        /// Deletes an entity from the database, based on a specified key
        /// </summary>
        public virtual async Task DeleteByIdAsync(object Id)
        {
            T Entity = await GetByIdAsync(Id);
            if (Entity != null)
            {
                await DeleteAsync(Entity);
            }
        }
        /// <summary>
        /// Deletes an entity from the database, based on a specified key (which may be a compound one).
        /// <para>NOTE: Nothing happens if the entity is not found.</para>
        /// </summary>
        public virtual async Task DeleteByIdsAsync(params object[] Ids)
        {
            T Entity = await GetByIdsAsync(Ids);
            if (Entity != null)
            {
                await DeleteAsync(Entity);
            }
        }


        /* properties */
        /// <summary>
        /// The data store of this service
        /// </summary>
        public virtual SqlStore Store
        {
            get
            {
                if (fStore == null)
                    fStore = SqlStores.CreateSqlStore(Descriptor.ConnectionName);
                return fStore;
            }
        }
    }
}
