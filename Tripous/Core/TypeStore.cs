using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous
{

    /// <summary>
    /// A "registry"-like class. It keeps an internal list of Type items associated with a set of type names. 
    /// <para>Types are registered to this "registry" either by calling the Add method or just decorating
    /// the Type with the <see cref="TypeStoreItemAttribute"/> attribute. </para>
    /// <para>NOTE: The TypeStore can create an instance of a registered Type by just passing one of the
    /// registered type names. See the <see cref="Create"/> method for more information.</para>
    /// </summary>
    static public class TypeStore
    {
        static Dictionary<Type, List<string>> Dic = new Dictionary<Type, List<string>>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static TypeStore()
        {
            TypeStore.Add(typeof(string), "S;String;W");
            TypeStore.Add(typeof(int), "I;Int;Integer;Int32");
            TypeStore.Add(typeof(bool), "B;Boolean;Bool");
            TypeStore.Add(typeof(double), "F;Double;Float");
            TypeStore.Add(typeof(decimal), "C;Decimal;Currency");
            TypeStore.Add(typeof(DateTime), "M;DateTime;Date;Time;D;T");
            TypeStore.Add(typeof(char), "Char;Character");
        }

        /* public */
        /// <summary>
        /// Adds a type under a set of type names separated by semicolons.
        /// <para>If not included in names, the type is also registered under Type.Name, Type.FullName and Type.AssemblyQualifiedName</para>
        /// </summary>
        static public void Add(Type T, string Names)
        {
            string[] NameList = string.IsNullOrWhiteSpace(Names)? new string[0] : Names.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Add(T, NameList);
        }
        /// <summary>
        /// Adds a type under a set of type names.
        /// <para>If not included in names, the type is also registered under Type.Name, Type.FullName and Type.AssemblyQualifiedName</para>
        /// </summary>
        static public void Add(Type T, string[] Names)
        {
            if (!Dic.ContainsKey(T))
            {
                Dic[T] = new List<string>();
            }

            List<string> NameList = new List<string>(Names);
            if (!NameList.ContainsText(T.Name))
                NameList.Add(T.Name);

            if (!NameList.ContainsText(T.FullName))
                NameList.Add(T.FullName);

            if (!NameList.ContainsText(T.AssemblyQualifiedName))
                NameList.Add(T.AssemblyQualifiedName);

            Type T2;
            foreach (string Name in NameList)
            {
                T2 = Find(Name);
                if (T2 != null && T2 != T)
                    throw new ApplicationException(string.Format("Name {0} already registered to type: {1}", Name, T2.FullName));
            }

            Dic[T].AddRange(NameList);

        }
 
        /// <summary>
        /// Finds and returns a Type by name, if any, else null.
        /// </summary>
        static public Type Find(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return null;

            /* try to find an exact match of the Name in the internal dictionary */
            foreach (var Entry in Dic)
            {
                if (Entry.Value.ContainsText(Name))
                    return Entry.Key;
            } 

            return null;
        }

        /// <summary>
        /// Returns true if Type is registered
        /// </summary>
        static public bool Contains(Type T)
        {
            return Dic.ContainsKey(T);
        }
        /// <summary>
        /// Returns true if Name type name is registered
        /// </summary>
        static public bool Contains(string Name)
        {
            return Find(Name) != null;
        }

        /// <summary>
        /// Creates and returns an object of the Type specified by Name.
        /// <para>Name could be a name registered to the TypeStore or a valid <see cref="Type.AssemblyQualifiedName"/> of a Type </para>
        /// </summary>
        static public object Create(string Name, object[] Args = null)
        {
            Type Type = Find(Name);

            if (Type != null)
                return Args == null? Type.Create(): Type.Create(Args);

            /* it could be a Type.AssemblyQualifiedName of a non-registered Type */
            try
            {
                Type = Type.GetType(Name, true, true);
                return Args == null ? Type.Create() : Type.Create(Args);
            }
            catch
            {
            }


            return null;
        }

        /// <summary>
        /// Returns a string array with the Names registered with Type.
        /// </summary>
        static public string[] GetNamesOf(Type T)
        {
            return Dic.ContainsKey(T)? Dic[T].ToArray(): new string[0];
        }
    }
}
