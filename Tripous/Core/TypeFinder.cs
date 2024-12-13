namespace Tripous
{

    /// <summary>
    /// Helper class for loading types and methods from assemblies
    /// </summary>
    static public class TypeFinder
    {
        /// <summary>
        /// Returns a type specified either by simply a type name, or a namespace qualified type name or an AssemblyQualifiedName type name.
        /// <para>Optionally, an Assmbly may be specified using either the long form of its name or a file path, to load and search for the type. </para>
        /// <para>Returns null if nothing is found. </para>
        /// </summary>
        static public Type GetTypeByName(string TypeName, string AssemblyNameOrPath = "")
        {
 
            Type Result = Type.GetType(TypeName, false, true);
           
            if (Result == null)
            {
                Assembly[] Items = AppDomain.CurrentDomain.GetAssemblies();
                 
                foreach (Assembly Item in Items)
                {
                    Result = Item.GetType(TypeName, false, true);

                    if (Result != null)
                        break;
                }
            }

 
            if (Result == null && !string.IsNullOrWhiteSpace(AssemblyNameOrPath))
            {
                Assembly A = LoadAssembly(AssemblyNameOrPath);
                if (A != null)
                {
                    Result = A.GetType(TypeName, false, true);
                }
            }

            if (Result == null)
            {
                string[] Parts = TypeName.Split('.');
                string ClassName = Parts.Last();

                Result = Type.GetType(ClassName, false, true);
            }

            return Result;
        }

        /// <summary>
        /// Returns true if the specified Assembly is registerable
        /// </summary>
        static public bool CanSearchAssembly(Assembly Assembly)
        {             
            if (Assembly.IsDynamic)
                return false;

            string Name = Assembly.FullName.ToUpperInvariant();

            if (Name.ContainsText("Microsoft") 
                || Name.ContainsText("netstandard")
                || Name.ContainsText("Newtonsoft")
                )
                return false;

            foreach (string S in SysConfig.ObjectStoreExcludedAssemblies)
            {
                if (Name.StartsWith(S, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Returns an array of the current domain assemblies. 
        /// <para>The result is sorted. Tripous assemblies come first of all.</para>
        /// <para>If SearchableOnly is true, then only those assemblies that pass the CanSearchAssembly() check are returned.</para>
        /// </summary>
        static public Assembly[] GetDomainAssemblies(bool SearchableOnly = true)
        {
            List<Assembly> List = new List<Assembly>();

            List.AddRange(AppDomain.CurrentDomain.GetAssemblies());

            /* get searcable assemblies only? */
            if (SearchableOnly)
            {
                Assembly[] Items = List.ToArray();

                foreach (Assembly A in Items)
                {
                    if (!CanSearchAssembly(A))
                        List.Remove(A);
                }
            }


            /* nested comparison function */
            Comparison<Assembly> Func = delegate (Assembly A, Assembly B)
            {
                string FileNameA = Path.GetFileName(A.GetFullPath());
                string FileNameB = Path.GetFileName(B.GetFullPath());

                if (FileNameA.IsSameText(FileNameB))
                    return 0;

                if (FileNameA.StartsWith("Tripous.dll", StringComparison.InvariantCultureIgnoreCase))
                    return -1;
                if (FileNameB.StartsWith("Tripous.dll", StringComparison.InvariantCultureIgnoreCase))
                    return 1; 

                if (FileNameA.StartsWith("Tripous", StringComparison.InvariantCultureIgnoreCase))
                    return -1;
                if (FileNameB.StartsWith("Tripous", StringComparison.InvariantCultureIgnoreCase))
                    return 1;

                return 1;
            };


            List.Sort(Func);


            return List.ToArray();
        }


        /// <summary>
        /// Loads and returns and Assembly specified by either the long form of its name or a file path.  
        /// <para>Returns null if nothing is found. </para>
        /// </summary>
        static public Assembly LoadAssembly(string NameOrPath)
        {
            Assembly Result = null;
            try
            {
                Result = Assembly.Load(NameOrPath);
            }
            catch
            {
            }             

            if (Result == null && File.Exists(NameOrPath))
            {
                try
                {
                    Result = Assembly.LoadFrom(NameOrPath);
                }
                catch 
                {
                }
            }

            return Result;
        }

        /// <summary>
        /// Loads plugin assemblies from Folder into this AppDomain and returns the list of loaded assemblies.
        /// <para>Prefix could be null or empty or any prefix.</para>
        /// <para>If Folder is null or empty then the folder of this assembly is used.</para>
        /// <para>FilterFunc should be either null, or a nested function (anonymous method)
        /// or belong to a type marked as Serializable or inheriting from MarshalByRefObject.</para>
        /// <para>FilterFunc is executed in a secondary AppDomain and it is passed each assembly for examination.
        /// It must return true if that assembly is to be included in the returned list.</para>
        /// <para>Example: Assembly[] AssemblyList = TypeFinder.LoadAssemblies("EM_", "", (a) => { return true; });</para>
        /// </summary>
        static public Assembly[] LoadAssemblies(string Prefix = "EM_", string Folder = "") 
        {
            List<Assembly> Result = new List<Assembly>();

            if (string.IsNullOrWhiteSpace(Folder))
            {
                Folder = typeof(TypeFinder).Assembly.ManifestModule.FullyQualifiedName;
                Folder = Path.GetDirectoryName(Folder);
            }

            string SearchPattern = string.IsNullOrWhiteSpace(Prefix) ? "*.dll" : string.Format("{0}*.dll", Prefix);
            string[] FileNames = Directory.GetFiles(Folder, SearchPattern);


            List<string> List = new List<string>(FileNames);  

            Assembly A = null;
            foreach (string FilePath in List)
            {
                try
                {
                    A = Assembly.LoadFrom(FilePath);
                    Result.Add(A);
                }
                catch
                {
                }
            }

            return Result.ToArray();
        }

        /* class types derived from a base class */
        /// <summary>
        /// Returns a list of class types derived from BaseClass.
        /// <para>Searches all searchable domain assemblies.</para>
        /// </summary>
        static public List<Type> FindDerivedClasses(Type BaseClass)
        {
            List<Assembly> AssemblyList = new List<Assembly>(GetDomainAssemblies(true));
            return FindDerivedClasses(BaseClass, AssemblyList);
        }
        /// <summary>
        /// Returns a list of class types derived from BaseClass
        /// </summary>
        static public List<Type> FindDerivedClasses(Type BaseClass, List<Assembly> AssemblyList)
        {
            List<Type> Result = new List<Type>();
            foreach (Assembly Assembly in AssemblyList)
                FindDerivedClasses(BaseClass, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a list of class types derived from BaseClass
        /// </summary>
        static public List<Type> FindDerivedClasses(Type BaseClass, Assembly Assembly)
        {
            List<Type> Result = new List<Type>();
            FindDerivedClasses(BaseClass, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a list of class types derived from BaseClass
        /// </summary>
        static public void FindDerivedClasses(Type BaseClass, Assembly Assembly, List<Type> Result)
        {
            if (BaseClass.IsClass)
            {
                try
                {
                    Type[] Types = Assembly.GetTypesSafe();

                    foreach (Type T in Types)
                    {
                        try
                        {
                            if (T.IsClass && T.IsSubclassOf(BaseClass))
                                Result.Add(T);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }
        }


        /* class types implementing a certain interface type */
        /// <summary>
        /// Returns a list of class types implenting the InterfaceType interface.
        /// <para>Searches all searchable domain assemblies.</para>
        /// </summary>
        static public List<Type> FindImplementorClasses(Type InterfaceType)
        {
            List<Assembly> AssemblyList = new List<Assembly>(GetDomainAssemblies(true));
            return FindImplementorClasses(InterfaceType, AssemblyList);
        }
        /// <summary>
        /// Returns a list of class types implenting the InterfaceType interface
        /// </summary>
        static public List<Type> FindImplementorClasses(Type InterfaceType, List<Assembly> AssemblyList)
        {
            List<Type> Result = new List<Type>();
            foreach (Assembly Assembly in AssemblyList)
                FindImplementorClasses(InterfaceType, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a list of class types implenting the InterfaceType interface
        /// </summary>
        static public List<Type> FindImplementorClasses(Type InterfaceType, Assembly Assembly)
        {
            List<Type> Result = new List<Type>();
            FindImplementorClasses(InterfaceType, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a list of class types implenting the InterfaceType interface
        /// </summary>
        static public void FindImplementorClasses(Type InterfaceType, Assembly Assembly, List<Type> Result)
        {
            if (InterfaceType.IsInterface)
            {
                try
                {
                    Type[] Types = Assembly.GetTypesSafe();

                    foreach (Type T in Types)
                    {
                        try
                        {
                            if (T.IsClass && T.ImplementsInterface(InterfaceType))
                                Result.Add(T);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }
        }


        /* class types marked with a certain attribute */
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type, of class types 
        /// marked with the AttributeType attribute.
        /// <para>Searches all searchable domain assemblies.</para>
        /// </summary>
        static public Dictionary<object, Type> FindClassesMarkedWith(Type AttributeType)
        {
            List<Assembly> AssemblyList = new List<Assembly>(GetDomainAssemblies(true));
            return FindClassesMarkedWith(AttributeType, AssemblyList);
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type, of class types 
        /// marked with the AttributeType attribute.
        /// </summary>
        static public Dictionary<object, Type> FindClassesMarkedWith(Type AttributeType, List<Assembly> AssemblyList)
        {
            Dictionary<object, Type> Result = new Dictionary<object, Type>();
            foreach (Assembly Assembly in AssemblyList)
                FindClassesMarkedWith(AttributeType, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type, of class types 
        /// marked with the AttributeType attribute.
        /// </summary>
        static public Dictionary<object, Type> FindClassesMarkedWith(Type AttributeType, Assembly Assembly)
        {
            Dictionary<object, Type> Result = new Dictionary<object, Type>();
            FindClassesMarkedWith(AttributeType, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type, of class types 
        /// marked with the AttributeType attribute.
        /// </summary>
        static public void FindClassesMarkedWith(Type AttributeType, Assembly Assembly, Dictionary<object, Type> Result)
        {
            if (AttributeType.IsClass && AttributeType.InheritsFrom(typeof(Attribute)))
            {
                try
                {
                    Type[] Types = Assembly.GetTypesSafe();
                    Attribute Attr;

                    foreach (Type T in Types)
                    {
                        try
                        {
                            if (T.IsClass && T.IsDefined(AttributeType, false))
                            {
                                Attr = Attribute.GetCustomAttribute(T, AttributeType, false);
                                Result[Attr] = T;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }
        }


        /* static methods and/or constructors marked with a certain attribute */
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a MethodBase 
        /// (actually a static method or a constructor), of MethodBase methods marked with the AttributeType attribute.
        /// <para>Searches all searchable domain assemblies.</para>
        /// </summary>
        static public Dictionary<object, MethodBase> FindMethodsMarkedWith(Type AttributeType, bool SelectStaticMethods = true, bool SelectConstructors = true)
        {
            List<Assembly> AssemblyList = new List<Assembly>(GetDomainAssemblies(true));
            return FindMethodsMarkedWith(AttributeType, AssemblyList, SelectStaticMethods, SelectConstructors);
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a MethodBase 
        /// (actually a static method or a constructor), of MethodBase methods marked with the AttributeType attribute.
        /// </summary>
        static public Dictionary<object, MethodBase> FindMethodsMarkedWith(Type AttributeType, List<Assembly> AssemblyList, bool SelectStaticMethods = true, bool SelectConstructors = true)
        {
            Dictionary<object, MethodBase> Result = new Dictionary<object, MethodBase>();
            foreach (Assembly Assembly in AssemblyList)
                FindMethodsMarkedWith(AttributeType, Assembly, Result, SelectStaticMethods, SelectConstructors);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a MethodBase 
        /// (actually a static method or a constructor), of MethodBase methods marked with the AttributeType attribute.
        /// </summary>
        static public Dictionary<object, MethodBase> FindMethodsMarkedWith(Type AttributeType, Assembly Assembly, bool SelectStaticMethods = true, bool SelectConstructors = true)
        {
            Dictionary<object, MethodBase> Result = new Dictionary<object, MethodBase>();
            FindMethodsMarkedWith(AttributeType, Assembly, Result, SelectStaticMethods, SelectConstructors);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a MethodBase 
        /// (actually a static method or a constructor), of MethodBase methods marked with the AttributeType attribute.
        /// </summary>
        static public void FindMethodsMarkedWith(Type AttributeType, Assembly Assembly, Dictionary<object, MethodBase> Result, bool SelectStaticMethods = true, bool SelectConstructors = true)
        {
            if (AttributeType.IsClass && AttributeType.InheritsFrom(typeof(Attribute)))
            {
                if (!SelectStaticMethods && !SelectConstructors)
                    return;

                Attribute Attr;
                Action<MethodBase[]> FindMethods = delegate (MethodBase[] Methods)
                {
                    if ((Methods != null) && (Methods.Length > 0))
                    {
                        foreach (MethodBase Method in Methods)
                        {
                            try
                            {
                                Attr = Attribute.GetCustomAttribute(Method, AttributeType, false);
                                if (Attr != null)
                                {
                                    Result[Attr] = Method;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                };



                try
                {
                    Type[] Types = Assembly.GetTypesSafe();
                    MethodInfo[] Methods;
                    ConstructorInfo[] Constructors;

                    foreach (Type T in Types)
                    {
                        try
                        {
                            if (SelectStaticMethods)
                            {
                                Methods = T.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                                FindMethods(Methods);
                            }

                            if (SelectConstructors)
                            {
                                Constructors = T.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                                FindMethods(Constructors);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }

        }


        /* class types 1) marked with a certain attribute and 2) derived from a base class  */
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types marked with AttributeType and derived from BaseClass.
        /// <para>Searches all searchable domain assemblies.</para>
        /// </summary>
        static public Dictionary<object, Type> FindDerivedClasses(Type AttributeType, Type BaseClass)
        {
            List<Assembly> AssemblyList = new List<Assembly>(GetDomainAssemblies(true));
            return FindDerivedClasses(AttributeType, BaseClass, AssemblyList);
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types marked with AttributeType and derived from BaseClass.
        /// </summary>
        static public Dictionary<object, Type> FindDerivedClasses(Type AttributeType, Type BaseClass, List<Assembly> AssemblyList)
        {
            Dictionary<object, Type> Result = new Dictionary<object, Type>();
            foreach (Assembly Assembly in AssemblyList)
                FindDerivedClasses(AttributeType, BaseClass, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types marked with AttributeType and derived from BaseClass.
        /// </summary>
        static public Dictionary<object, Type> FindDerivedClasses(Type AttributeType, Type BaseClass, Assembly Assembly)
        {
            Dictionary<object, Type> Result = new Dictionary<object, Type>();
            FindDerivedClasses(AttributeType, BaseClass, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types marked with AttributeType and derived from BaseClass.
        /// </summary>
        static public void FindDerivedClasses(Type AttributeType, Type BaseClass, Assembly Assembly, Dictionary<object, Type> Result)
        {

            if (AttributeType.IsClass && AttributeType.InheritsFrom(typeof(Attribute)))
            {
                try
                {
                    Type[] Types = Assembly.GetTypesSafe();
                    Attribute Attr;

                    foreach (Type T in Types)
                    {
                        try
                        {
                            if (T.IsClass && T.IsDefined(AttributeType, false) && T.InheritsFrom(BaseClass))
                            {
                                Attr = Attribute.GetCustomAttribute(T, AttributeType, false);
                                Result[Attr] = T;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }

        }


        /* class types 1) marked with a certain attribute, 2) implementing a certain interface type and 3) possibly derived from a base class  */
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types 1) marked with AttributeType 2) implementing a InterfaceType interface 
        /// and 3) if BaseClass is NOT null, derived from that BaseClass.
        /// <para>Searches all searchable domain assemblies.</para>
        /// </summary>
        static public Dictionary<object, Type> FindClasses(Type AttributeType, Type InterfaceType, Type BaseClass = null)
        {
            List<Assembly> AssemblyList = new List<Assembly>(GetDomainAssemblies(true));
            return FindClasses(AttributeType, InterfaceType, BaseClass, AssemblyList);
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types 1) marked with AttributeType 2) implementing a InterfaceType interface 
        /// and 3) if BaseClass is NOT null, derived from that BaseClass.
        /// </summary>
        static public Dictionary<object, Type> FindClasses(Type AttributeType, Type InterfaceType, Type BaseClass, List<Assembly> AssemblyList)
        {
            Dictionary<object, Type> Result = new Dictionary<object, Type>();
            foreach (Assembly Assembly in AssemblyList)
                FindClasses(AttributeType, InterfaceType, BaseClass, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types 1) marked with AttributeType 2) implementing a InterfaceType interface 
        /// and 3) if BaseClass is NOT null, derived from that BaseClass.
        /// </summary>
        static public Dictionary<object, Type> FindClasses(Type AttributeType, Type InterfaceType, Type BaseClass, Assembly Assembly)
        {
            Dictionary<object, Type> Result = new Dictionary<object, Type>();
            FindClasses(AttributeType, InterfaceType, BaseClass, Assembly, Result);
            return Result;
        }
        /// <summary>
        /// Returns a dictionary where Key is an Attribute instance and Value a class type,
        /// of class types 1) marked with AttributeType 2) implementing a InterfaceType interface 
        /// and 3) if BaseClass is NOT null, derived from that BaseClass.
        /// </summary>
        static public void FindClasses(Type AttributeType, Type InterfaceType, Type BaseClass, Assembly Assembly, Dictionary<object, Type> Result)
        {
            if (AttributeType.IsClass && AttributeType.InheritsFrom(typeof(Attribute)) && InterfaceType.IsInterface)
            {
                try
                {
                    Type[] Types = Assembly.GetTypesSafe();
                    Attribute Attr;

                    foreach (Type T in Types)
                    {
                        try
                        {
                            if (T.IsClass
                                && T.IsDefined(AttributeType, false)
                                && T.ImplementsInterface(InterfaceType)
                                && ((BaseClass == null) || T.InheritsFrom(BaseClass)))
                            {
                                Attr = Attribute.GetCustomAttribute(T, AttributeType, false);
                                Result[Attr] = T;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }


        }

    }
}
