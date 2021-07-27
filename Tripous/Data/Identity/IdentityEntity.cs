using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Globalization;

using Tripous.Data;

namespace Tripous.Identity
{

    /// <summary>
    /// The base entity class for all identity classes in this namespace
    /// </summary>
    public class IdentityEntity
    {
        /* private */
        string fTableName;

        /* protected */
        /// <summary>
        /// Constant
        /// </summary>
        protected readonly Type BOOL = typeof(bool);

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        protected virtual void SetPropertyValue(PropertyInfo Prop, object Value)
        {
            if (Prop.PropertyType == BOOL)
            {
                Prop.SetValue(this, Convert.ToBoolean(Value));
            }
            else
            {
                if (Sys.IsNull(Value))
                {
                    if (Sys.IsNullable(Prop.PropertyType))
                    {
                        Prop.SetValue(this, null);
                    }
                    else
                    {
                        Prop.SetValue(this, Prop.PropertyType.IsValueType ? Activator.CreateInstance(Prop.PropertyType) : null);
                    }
                }
                else
                {
                    Prop.SetValue(this, Value);
                }
            }
        }
        /// <summary>
        /// Sets the value of a parameter that is going to be used with Sql statemement execution.
        /// <para>In the specified dictionary there should be an entry for each property of this instance.</para>
        /// </summary>
        protected virtual void SetDictionaryParam(PropertyInfo Prop, Dictionary<string, object> Params, object Value)
        {
            if (Prop.PropertyType == BOOL)
            {
                Value = (bool)Value ? 1 : 0;
            }

            Params[Prop.Name] = Value;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public IdentityEntity()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public IdentityEntity(DataRow Row)
        {
            Assign(Row);
        }

        /* public */
        /// <summary>
        /// Assigns the properties of this instance using a <see cref="DataRow"/> as source.
        /// </summary>
        public virtual void Assign(DataRow Row)
        { 
            Type T = this.GetType();
            PropertyInfo[] Props = T.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object Value;
            foreach (PropertyInfo Prop in Props)
            {
                if (Prop.CanWrite && Row.TryGetValue(Prop.Name, out Value))
                {
                    SetPropertyValue(Prop, Value);
                }
            }
        }
        /// <summary>
        /// Creates and returns a dictionary where there is an entry for each property of this instance.
        /// </summary>
        public virtual Dictionary<string, object> CreateSqlCommandParams()
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();
 
            Type T = this.GetType();
            PropertyInfo[] Props = T.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            object Value;
            foreach (PropertyInfo Prop in Props)
            {
                if (Prop.CanWrite)
                {
                    Value = Prop.GetValue(this, null);

                    SetDictionaryParam(Prop, Result, Value);
                }
            }

            return Result;
        }
        /// <summary>
        /// It is called just before this instance is save to the database
        /// </summary>
        public virtual void BeforeSave(bool IsInsert)
        {

        }

        /* properties */
        /// <summary>
        /// Id. A GUID without brackets.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The name of the database table this instance is associated with
        /// </summary>
        public string TableName 
        { 
            get 
            { 
                if (string.IsNullOrWhiteSpace(fTableName))
                    fTableName = IdDb.GetTableName(this.GetType());
                return fTableName;
            } 
        }
 
    }
}
