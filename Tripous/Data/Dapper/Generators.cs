using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;

namespace Tripous.Data
{



    /// <summary>
    /// Helper for registering and executing generator functions.
    /// <para>NOTE: A generator function is responsible in creating and returning primary key values for databases tables.</para>
    /// </summary>
    static public class Generators
    {
        /* private */
        static readonly Type  AttributeType = typeof(GeneratorAttribute);        
        static Dictionary<Type, MethodInfo> Dic = new Dictionary<Type, MethodInfo>();

        /* public */
        /// <summary>
        /// Loads all static function marked with the generator attribute of a specified assembly
        /// </summary>
        static public void Load(Assembly A)
        {
            // select static methods only
            Dictionary<object, MethodBase> MethodsDic = TypeFinder.FindMethodsMarkedWith(AttributeType, A, true, false);

            Type EntityType;
            GeneratorAttribute Attribute;
            foreach (var Entry in MethodsDic)
            {
                Attribute = Entry.Key as GeneratorAttribute;
                EntityType = Attribute.EntityType;

                if (!Contains(EntityType) && Entry.Value is MethodInfo)
                {
                    Dic[EntityType] = Entry.Value as MethodInfo; 
                }
            }
        }
        /// <summary>
        /// Returns true if a generator function is already registered for a specified entity
        /// </summary>
        static public bool Contains(Type EntityType)
        {
            return Dic.ContainsKey(EntityType);
        }

        /// <summary>
        /// Executes a generator function and assigns the result(s) to the specified Entity primary key field(s).
        /// </summary>
        static public void Execute(DbConnection Con, DataEntity Entity)
        {
            if (Entity == null)
                Sys.Error("Can not execute a generator on a null Entity");

            Type EntityType = Entity.GetType();

            if (!Dic.ContainsKey(EntityType))
                Sys.Error("No generator is registered for this type: {0}", EntityType.FullName);

            MethodInfo Proc = Dic[EntityType];

            Proc.Invoke(null, new object[] { Con, Entity });
        }
 
    }


    
}
