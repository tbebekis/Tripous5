using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Data;

namespace Tripous
{
    /// <summary>
    /// 
    /// </summary>
    static public class ObjectStore
    {
        #region nested types
        /// <summary>
        /// A <see cref="CodeName"/> derived class that represent another object
        /// specified by <see cref="Instance"/>
        /// </summary>
        public class CodeNameInstance : ICodeName
        {

            /// <summary>
            /// Constructor
            /// </summary>
            public CodeNameInstance(string Code, object Instance)
            {
                this.Code = Code;
                this.Instance = Instance;
            }

            /// <summary>
            /// Gets the unique Code value
            /// </summary>
            public string Code { get; private set; }
            /// <summary>
            /// Returns the instance
            /// </summary>
            public object Instance { get; private set; }
        }

        /// <summary>
        /// An Invoker class whose Call() calls a passed GenericEventHandler event or invokes a Method.
        /// <para>Method should be a static method or an instance constructor.</para>
        /// </summary>
        public class InvokerMethod : Invoker
        {
            private event GenericEventHandler callBack;
            private MethodBase method;

            /// <summary>
            /// constructor.
            /// </summary>
            public InvokerMethod(string Code, GenericEventHandler CallBack)
                : base(Code)
            {
                if (CallBack == null)
                    throw new ArgumentException("CallBack is null");

                this.callBack = CallBack;
            }
            /// <summary>
            /// constructor.
            /// </summary>
            public InvokerMethod(string Code, MethodBase Method)
                : base(Code)
            {
                if (Method == null)
                    throw new ArgumentException("Method is null");

                this.method = Method;
            }

            /// <summary>
            /// The method the Invoker represents. It returns an object which could be null.
            /// </summary>
            public override object Call(params object[] Args)
            {
                if (callBack != null)
                    return callBack(Args);

                if (method != null)
                {
                    if (method is ConstructorInfo)
                        return (method as ConstructorInfo).Invoke(Args);
                    else if ((method is MethodInfo) && (method as MethodInfo).IsStatic)
                    {
                        return (method as MethodInfo).Invoke(null, Args);
                    }
                }
                return null;
            }

        }
        #endregion




        static bool fInitialized;
        static private List<ICodeName> list = new List<ICodeName>();

        static private List<Assembly> assemblies = new List<Assembly>();
        static private List<MethodBase> initializerMethods = new List<MethodBase>();



        /// <summary>
        /// Static constructor.  
        /// </summary>
        static ObjectStore()
        { 
        }

        /// <summary>
        /// Registers any class that is marked with the GlobalObjectAttribute attribute.
        /// <para>Returns a string with errors, if any, else empty string.</para>
        /// </summary>
        static public string Initialize()
        {
            StringBuilder SB = new StringBuilder();

            if (!fInitialized)
            {
                Assembly[] Items = TypeFinder.GetDomainAssemblies(true);
                string Res = null;
                foreach (Assembly Assembly in Items)
                {
                    Res = RegisterObjectsOf(Assembly);
                    if (!string.IsNullOrWhiteSpace(Res))
                        SB.AppendLine(Res);
                }

                fInitialized = true;
            }

            return SB.ToString();
        }

        /// <summary>
        /// Registers objects found in Assembly. Those objects are created by using classes marked with
        /// the  <see cref="ObjectStoreItemAttribute"/>  and implementing the ICodeName or the IInvoker or they have a contructor or a static mathod
        /// marked with the <see cref="ObjectStoreItemAttribute"/> attribute.
        /// <para>WARNING: In Compact Framework this method should be manually called.</para>
        /// <para>Returns a string with errors, if any, else empty string.</para>
        /// </summary>
        static public string RegisterObjectsOf(Assembly Assembly)
        {
            StringBuilder SB = new StringBuilder();

            if (!assemblies.Contains(Assembly))
            {
                ObjectStoreItemAttribute ObjectStoreItemAttr;
                InitializerAttribute InitializerAttr;
                TypeStoreItemAttribute TypeStoreItemAttr;

                object Instance;
                Type[] Types = Assembly.GetTypesSafe();

                foreach (Type Type in Types)
                {

                    /* ================================================================================================ */
                    /* type is decorated with ObjectStoreItemAttribute */
                    try
                    {
                        if (Type.IsDefined(typeof(ObjectStoreItemAttribute), true))
                        {
                            ObjectStoreItemAttr = Attribute.GetCustomAttribute(Type, typeof(ObjectStoreItemAttribute), true) as ObjectStoreItemAttribute;

                            Instance = Type.Create();
                            if (Instance is IInvoker)
                                Add(Instance as IInvoker);
                            else if (Instance is ICodeName)
                                Add(Instance as ICodeName);
                            else
                                Add(ObjectStoreItemAttr.Code, Instance);
                        }
                    }
                    catch (Exception Ex)
                    {
                        SB.AppendLine(string.Format("{0}: {1}", Ex.GetType().Name, Ex.Message));
                    }



                    /* ================================================================================================ */
                    /* type is decorated with TypeItemAttribute */
                    try
                    {
                        if (Type.IsDefined(typeof(TypeStoreItemAttribute), true))
                        {
                            TypeStoreItemAttr = Attribute.GetCustomAttribute(Type, typeof(TypeStoreItemAttribute), true) as TypeStoreItemAttribute;
                            TypeStore.Add(Type, TypeStoreItemAttr.TypeNames);
                        }
                    }
                    catch (Exception Ex)
                    {
                        SB.AppendLine(string.Format("{0}: {1}", Ex.GetType().Name, Ex.Message));
                    }



                    /* ================================================================================================ */
                    /* type implementing IResourceProvider */
                    try
                    {
                        if (Type.ImplementsInterface(typeof(Tripous.IResourceProvider)) && !Type.IsAbstract && Type.HasDefaultConstructor())
                        {
                            Tripous.IResourceProvider provider = Type.Create() as Tripous.IResourceProvider;
                            if (provider != null)
                                Res.Add(provider);
                        }
                    }
                    catch (Exception Ex)
                    {
                        SB.AppendLine(string.Format("{0}: {1}", Ex.GetType().Name, Ex.Message));
                    }



                    List<MethodBase> List = new List<MethodBase>();


                    /* ================================================================================================ */
                    /* static method is decorated with InitializerAttribute */
                    if (!Platform.IsCompact || (Platform.IsCompact && (!Type.IsGenericType || (Type.IsGenericType && !Type.ContainsGenericParameters))))
                    {
                        MethodInfo[] Methods = Type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                        if (Methods != null)
                            List.AddRange(Methods);

                        foreach (MethodBase Method in List)
                        {
                            try
                            {
                                InitializerAttr = Attribute.GetCustomAttribute(Method, typeof(InitializerAttribute)) as InitializerAttribute;
                                if (InitializerAttr != null)
                                {
                                    if (SysConfig.ObjectStoreAutoInvokeInitializers)
                                    {
                                        Method.Invoke(null, null);
                                    }
                                    else
                                    {
                                        initializerMethods.Add(Method);
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                SB.AppendLine(string.Format("{0}: {1}", Ex.GetType().Name, Ex.Message));
                            }
                        }
                    }


                    /* ================================================================================================ */
                    /* constructor or static method is decorated with ObjectStoreItemAttribute */
                    if (!Platform.IsCompact || (Platform.IsCompact && (!Type.IsGenericType || (Type.IsGenericType && !Type.ContainsGenericParameters))))
                    {
                        ConstructorInfo[] Constructors = Type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); //
                        if (Constructors != null)
                            List.AddRange(Constructors);

                        foreach (MethodBase Method in List)
                        {
                            try
                            {
                                ObjectStoreItemAttr = Attribute.GetCustomAttribute(Method, typeof(ObjectStoreItemAttribute)) as ObjectStoreItemAttribute;
                                if (ObjectStoreItemAttr != null)
                                    Add(ObjectStoreItemAttr.Code, Method);
                            }
                            catch (Exception Ex)
                            {
                                SB.AppendLine(string.Format("{0}: {1}", Ex.GetType().Name, Ex.Message));
                            }
                        }
                    }


                }

                assemblies.Add(Assembly);
            }

            return SB.ToString();
        }
        /// <summary>
        /// Executes those static methods marked by the InitializerAttribute
        /// <para>Returns a string with errors, if any, else empty string.</para>
        /// </summary>
        static public string InvokeInitializers()
        {
            StringBuilder SB = new StringBuilder();
            foreach (MethodBase Method in initializerMethods)
            {
                try
                {
                    Method.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    SB.AppendLine(ex.Message);
                }
            }

            initializerMethods.Clear();

            return SB.ToString();
        }


        /// <summary>
        /// Adds Instance to the collection. 
        /// <para>NOTE: If Instance.Code already exists, the new instance replaces the old one.</para>
        /// </summary>
        static public void Add(ICodeName Instance)
        {
            if (string.IsNullOrEmpty(Instance.Code))
                throw new ArgumentException("Illegal ICodeName Code");

            int Index = list.FindIndex(item => item.Code.IsSameText(Instance.Code));

            if (Index == -1)
                list.Add(Instance);
            else
                list[Index] = Instance;
        }
        /// <summary>
        /// Adds Instance to the collection. If Instance.Code already exists, it throws an exception.
        /// <para>NOTE: If Instance.Code already exists, the new instance replaces the old one.</para>
        /// </summary>
        static public void Add(IInvoker Instance)
        {
            Add(Instance as ICodeName);
        }
        /// <summary>
        /// Creates an Invoker with Code and CreateCallBack. The CreateCallBack 
        /// is the method which is going to be called by the Invoker. 
        /// <para>NOTE: If Code already exists, the new instance replaces the old one.</para>
        /// <para>The newly created Invoker is added to the collection.</para>
        /// </summary>
        /// <remarks>
        /// <example>
        /// <code>
        /// static public class TestObjectStore
        /// {
        ///     static public void Execute()
        ///     {
        ///         if (!ObjectStore.Contains("Test.CallBack"))
        ///             ObjectStore.AddInvoker("Test.CallBack", new GenericEventHandler(CreateObject));
        /// 
        ///         IInvoker Invoker;
        ///         if (ObjectStore.Find("Test.CallBack", out Invoker))
        ///             Invoker.Call("Hi there", DateTime.Today);
        /// 
        ///         ObjectStore.Call("Test.Callback", 1, 3, false);
        ///         ObjectStore.Call("FactoryTest", 321, 334, false);
        ///     }
        /// 
        ///     static public object CreateObject(params object[] Args)
        ///     {
        ///         string S = "";
        /// 
        ///         foreach (object o in Args)
        ///             S += o.ToString() + Environment.NewLine;
        /// 
        ///         MessageBox.Show(S);
        ///         return null;
        ///     }
        /// }
        /// 
        /// /* this Factory is auto-registered by the ObjectStore */
        /// [ObjectStore]
        /// public class FactoryTest : Invoker
        /// {
        ///     public FactoryTest()
        ///         : base("FactoryTest")
        ///     {
        ///     }
        /// 
        ///     public override object Call(params object[] Args)
        ///     {
        ///         string S = "FactoryTest" + Environment.NewLine;
        /// 
        ///         foreach (object o in Args)
        ///             S += o.ToString() + Environment.NewLine;
        /// 
        ///         MessageBox.Show(S);
        ///         return null;   
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        static public void Add(string Code, GenericEventHandler CreateCallBack)
        {
            Add(new InvokerMethod(Code, CreateCallBack));
        }
        /// <summary>
        /// Creates an Invoker with Code and Method. 
        /// <para>NOTE: If Instance.Code already exists, the new instance replaces the old one.</para>
        /// <para>The Method is the method which is going to be called by the Invoker.</para>
        /// <para>The newly created Invoker is added to the collection.</para>
        /// <para>WARNING: Method should be a static method or an instance constructor.</para>
        /// <para>A static method or an instance contructor may be decorated with the <see cref="ObjectStoreItemAttribute"/>
        /// which indicates to the ObjectStore that the method should be used in creating an Invoker automatically. </para>
        /// <para>See the <see cref="RegisterObjectsOf"/> method code of this class of how that attribute is used.</para>
        /// </summary>
        static public void Add(string Code, MethodBase Method)
        {
            Add(new InvokerMethod(Code, Method));
        }
        /// <summary>
        /// Creates and adds a <see cref="CodeNameInstance"/> based on passed arguments.
        /// <para>NOTE: If Instance.Code already exists, the new instance replaces the old one.</para>
        /// <para>An object may register itself multiple times, each time with a different Code, by using this method.</para>
        /// </summary>
        static public void Add(string Code, object Instance)
        {
            Add(new CodeNameInstance(Code, Instance));
        }

        /// <summary>
        /// Removes an entry by Code.
        /// </summary>
        static public void Remove(string Code)
        {
            int Index = list.FindIndex(item => item.Code.IsSameText(Code));
            if (Index != -1)
                list.RemoveAt(Index);
        }

        /// <summary>
        /// Returns an ICodeName with Code, if any, else null.
        /// </summary>
        static public ICodeName Find(string Code)
        {
            int Index = list.FindIndex(item => item.Code.IsSameText(Code));
            if (Index != -1)
                return list[Index];
            return null;
        }
        /// <summary>
        /// Returns true if the Code identifies a <see cref="CodeNameInstance"/>. 
        /// On success the Instance contains the <see cref="CodeNameInstance.Instance"/>
        /// </summary>
        static public bool Find<T>(string Code, out T Instance) where T : class
        {
            bool Result = false;
            Instance = default(T);

            object Item = Find(Code);

            if (Item is CodeNameInstance)
                Item = (Item as CodeNameInstance).Instance;

            if (Item != null)
            {
                Type ItemType = Item.GetType();
                Type OutType = typeof(T);

                if (OutType.IsInterface)
                    Result = ItemType.ImplementsInterface(OutType);
                else
                    Result = Item.InheritsFrom(OutType);

                if (Result)
                    Instance = Item as T;
            }

            return Result;
        }
        /// <summary>
        /// Returns true if an ICodeName with Code exists. On success the Instance is loaded.
        /// </summary>
        static public bool Find(string Code, out ICodeName Instance)
        {
            Instance = Find(Code);
            return Instance != null;
        }
        /// <summary>
        /// Returns true if an IInvoker with Code exists. On success the Instance is loaded.
        /// </summary>
        static public bool Find(string Code, out IInvoker Instance)
        {
            Instance = Find(Code) as IInvoker;
            return Instance != null;
        }

        /// <summary>
        /// Returns an array of ICodeName elements where each ICodeName element's Code
        /// contains the specified PartOfCode.
        /// </summary>
        static public ICodeName[] FindLike(string PartOfCode)
        {
            List<ICodeName> Result = new List<ICodeName>();

            ICodeName Item;

            for (int i = 0; i < list.Count; i++)
            {
                Item = list[i];
                if ((Item != null) && Item.Code.ContainsText(PartOfCode))
                {
                    Result.Add(Item);
                }
            }

            return Result.ToArray();
        }
        /// <summary>
        /// Returns an array of IInvoker elements where each IInvoker element's Code
        /// contains the specified PartOfCode.
        /// </summary>
        static public IInvoker[] FindInvokersLike(string PartOfCode)
        {
            ICodeName[] CodeNames = FindLike(PartOfCode);
            return CodeNames.OfType<IInvoker>().ToArray();
            /*  
                        var q = from CN in CodeNames
                                where CN is IInvoker
                                select CN as IInvoker;

                        return q.ToArray(); 
             */
            /*  

                        List<IInvoker> Result = new List<IInvoker>();

                        ICodeName Item;

                        for (int i = 0; i < list.Count; i++)
                        {
                            Item = list[i];
                            if ((Item != null) 
                                && Item.Code.ContainsText(PartOfCode)
                                && (Item is IInvoker))
                            {
                                Result.Add(Item as IInvoker);
                            }
                        }

                        return Result.ToArray(); 
             */
        }

        /// <summary>
        /// Returns true if an object with Code exists in collection.
        /// </summary>
        static public bool Contains(string Code)
        {
            return Find(Code) != null;
        }

        /// <summary>
        /// If Code represents a registered Invoker, then it calls the Call() of
        /// that Invoker, passing Args. On success returns the created object.
        /// If the Invoker does not exist, then it returns Default.
        /// </summary>
        static public T CallDef<T>(string Code, T Default, params object[] Args)
        {
            IInvoker Invoker = null;
            if (Find(Code, out Invoker))
                return (T)Invoker.Call(Args);
            return Default;
        }
        /// <summary>
        /// If Code represents a registered Invoker, then it calls the Call() of
        /// that Invoker, passing Args. On success returns the created object. 
        /// </summary>
        static public object Call(string Code, params object[] Args)
        {
            return CallDef(Code, (object)null, Args);
        }


        /// <summary>
        /// Returns a DataTable with the content of this store
        /// </summary>
        static public DataTable GetContentTable()
        {

            DataTable Table = new DataTable("ObjectStore");
            Table.Columns.Add("Code", typeof(System.String));
            Table.Columns.Add("Type", typeof(System.String));

            for (int i = 0; i < list.Count; i++)
            {
                Table.Rows.Add(list[i].Code, list[i].GetType().FullName);
            }

            return Table;
        }
        /// <summary>
        /// Returns a dictionary with the content of this store
        /// </summary>
        static public IDictionary<string, object> GetContentDictionary()
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();
            for (int i = 0; i < list.Count; i++)
                Result.Add(list[i].Code, list[i]);
            return Result;
        }

        /* properties */
        /// <summary>
        /// Returns a string array with all the registered names
        /// </summary>
        static public string[] GetCodes()
        {
            List<string> Result = new List<string>();
            foreach (var Item in list)
                Result.Add(Item.Code);
            Result.Sort();

            return Result.ToArray();
        }
        /// <summary>
        /// Returns the list of registered assemblies
        /// </summary>
        static public Assembly[] GetRegisteredAssemblies()
        {
            return assemblies.ToArray();
        }

    }
}
