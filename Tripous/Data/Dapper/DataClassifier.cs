using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;

using Dapper;

namespace Tripous.Data
{

    /// <summary>
    /// Classifies detail entities of a List type property (say Customer.Addresses) of a master entity, into Inserts, Deletes and Updates.
    /// </summary>
    public class DataClassifier
    {
        DataEntity MasterEntity;
        DataEntity OriginalMasterEntity;
        DetailListDescriptor RelationListDescriptor;
        EntityDescriptor MasterDescriptor;
        EntityDescriptor DetailDescriptor;
        List<DataEntity> EntityList;
        List<DataEntity> OriginalList;

        PropertyInfo KeyProperty;

        /* construction */
        DataClassifier(DataEntity MasterEntity, DataEntity OriginalMasterEntity, EntityDescriptor MasterDescriptor, DetailListDescriptor RelationListDescriptor)
        {
            this.MasterEntity = MasterEntity;
            this.OriginalMasterEntity = OriginalMasterEntity;
            this.RelationListDescriptor = RelationListDescriptor;
            this.MasterDescriptor = MasterDescriptor;
            this.DetailDescriptor = RelationListDescriptor.DetailDescriptor;
            this.KeyProperty = DetailDescriptor.PrimaryKeyList[0].Property;

            this.EntityList = RelationListDescriptor.GetListAsEnumerable(MasterEntity).Cast<DataEntity>().ToList();
            this.OriginalList = RelationListDescriptor.GetListAsEnumerable(OriginalMasterEntity).Cast<DataEntity>().ToList();
        }

        /* private */
        DataEntity EntityListFind(DataEntity OriginalEntity)
        {
            object Id = KeyProperty.GetValue(OriginalEntity);

            foreach (var Entity in EntityList)
            {
                if (Id == KeyProperty.GetValue(Entity))
                    return Entity;
            }

            return null;
        }
        DataEntity OriginalListFind(DataEntity Entity)
        {
            object Id = KeyProperty.GetValue(Entity);

            foreach (var OriginalEntity in OriginalList)
            {
                if (Id == KeyProperty.GetValue(OriginalEntity))
                    return OriginalEntity;
            }


            return null;
        }
        void Classify()
        {
            foreach (var OriginalEntity in OriginalList)
            {
                if (EntityListFind(OriginalEntity) == null)
                    Deletes.Add(OriginalEntity);
            }

            foreach (var Entity in EntityList)
            {
                var OriginalEntity = OriginalListFind(Entity);

                if (OriginalEntity != null)
                {
                    Updates.Add(new UpdateEntityPair(Entity, OriginalEntity));
                }
                else
                {
                    Inserts.Add(Entity);
                }
            }
        }

        /// <summary>
        /// Classifies detail entities of a List type property (say Customer.Addresses) of a master entity, into Inserts, Deletes and Updates.
        /// <para>Returns an instance with three corresponding lists with classified entities.</para>
        /// </summary>
        static public DataClassifier Classify(DataEntity MasterEntity, DataEntity OriginalMasterEntity, EntityDescriptor MasterDescriptor, DetailListDescriptor RelationalListDescriptor)
        {
            DataClassifier Result = new DataClassifier(MasterEntity, OriginalMasterEntity, MasterDescriptor, RelationalListDescriptor);
            Result.Classify();
            return Result;
        }

        /* properties */
        /// <summary>
        /// Entities classified for insertion
        /// </summary>
        public List<DataEntity> Inserts { get; } = new List<DataEntity>();
        /// <summary>
        /// Entities classified for update
        /// </summary>
        public List<UpdateEntityPair> Updates { get; } = new List<UpdateEntityPair>();
        /// <summary>
        /// Entities classified for deletion
        /// </summary>
        public List<DataEntity> Deletes { get; } = new List<DataEntity>();
    }

    /// <summary>
    /// A pair of Entities. This class is used when issuing UPDATE statements.
    /// <para>The OriginalEntity is the entity as is in the database, while the Entity is the one altered and must be saved back to the database.</para>
    /// </summary>
    public class UpdateEntityPair
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateEntityPair(DataEntity Entity, DataEntity OriginalEntity)
        {
            this.Entity = Entity;
            this.OriginalEntity = OriginalEntity;
        }

        /// <summary>
        /// The entity that is altered and must be saved back to the database
        /// </summary>
        public DataEntity Entity { get; }
        /// <summary>
        /// The entity entity as is in the database
        /// </summary>
        public DataEntity OriginalEntity { get; }
    }
}
