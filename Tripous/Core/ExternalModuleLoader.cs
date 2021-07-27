using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Tripous
{


    /// <summary>
    /// Loads dynamically loadable (plugins) external modules (assemblies) using the <see cref="AssemblyLoadContext"/> class.
    /// <para>The loading process is done in two steps. </para>
    /// <para>1. Loads assemblies into an un-loadable context and decides whether to load the assembly in step 2, based on contained creatable types.</para>
    /// <para>2. Loads assemblies into the default context, creates instances based on creatable types, and returns a list of instances.</para>
    /// </summary>
    public class ExternalModuleLoader<TExternalModule> where TExternalModule : class
    {

        /// <summary>
        /// Loads assemblies into an un-loadable context and decides whether to load the assembly in step 2, based on contained creatable types.
        /// Unloads all assemblies in the end.
        /// </summary>
        /// <returns>A list of assembly filepaths of assemblies to load into the default context.</returns>
        protected virtual string[] GetAssemblyPaths()
        {
            List<string> ResultList = new List<string>();

            string SearchPattern = string.IsNullOrWhiteSpace(Prefix) ? "*.dll" : string.Format("{0}*.dll", Prefix);
            string[] AssemblyPaths = Directory.GetFiles(RootFolder, SearchPattern, SearchOption.AllDirectories);

            //List<string> FileNamesList = new List<string>();

            bool Unloadable = true;
            AssemblyLoadContext LoadContext = new AssemblyLoadContext("ExternalModuleFinder", Unloadable);

            Assembly A;
            //string FileName;
            foreach (string AssemblyPath in AssemblyPaths)
            {
                if (ExcludeAssemblyFunc(AssemblyPath))
                    continue;

                //FileName = Path.GetFileName(AssemblyPath).ToLower();
                //if (FileNamesList.Contains(FileName))
                //    continue;

                //FileNamesList.Add(FileName);

                A = LoadContext.LoadFromAssemblyPath(AssemblyPath);
                if (GetCreatableTypesFunc(A).Length > 0)
                {
                    ResultList.Add(AssemblyPath);
                }
            }

            LoadContext.Unload();

            return ResultList.ToArray();
        }

        /// <summary>
        /// Returns true if an assembly of a specified path should be excluded.
        /// </summary>
        protected virtual bool DefaultExcludeAssembly(string AssemblyPath)
        {
            return AssemblyPath.Contains(@"\ref\");
        }
        /// <summary>
        /// Returns a list of types contained in a specified assembly that should be used in creating plugin instances.
        /// </summary>
        protected virtual Type[] DefaultGetCreatableTypes(Assembly A)
        {
            Type[] GetTypesSafe()
            {
                try
                {
                    if (A != null)
                        return A.GetTypes();
                }
                catch
                {
                }

                return new Type[0];
            }

            return GetTypesSafe()
            .Where(type => !type.IsAbstract && typeof(TExternalModule).IsAssignableFrom(type))
            .ToArray()
            ;
        }
        /// <summary>
        /// Creates and returns a plugin instance based on a specified type
        /// </summary>
        protected virtual TExternalModule DefaultCreateInstance(Type T)
        {
            return Activator.CreateInstance(T) as TExternalModule;
        }


        /// <summary>
        /// Loads assemblies into the default context, creates plugin instances based on creatable types, and returns a list of plugin instances.
        /// </summary>
        /// <returns>Returns a list of created instances.</returns>
        public TExternalModule[] Execute()
        {
            if (string.IsNullOrWhiteSpace(RootFolder))
                throw new ApplicationException("Can not load external modules. No root folder defined");

            if (ExcludeAssemblyFunc == null)
                ExcludeAssemblyFunc = DefaultExcludeAssembly;

            if (GetCreatableTypesFunc == null)
                GetCreatableTypesFunc = DefaultGetCreatableTypes;

            if (CreateInstanceFunc == null)
                CreateInstanceFunc = DefaultCreateInstance;


            List<TExternalModule> ResultList = new List<TExternalModule>();

            string[] AssemblyPaths = GetAssemblyPaths();

            AssemblyLoadContext LoadContext = AssemblyLoadContext.Default;

            Assembly A;
            Type[] Types;
            TExternalModule Instance;
            foreach (string AssemblyPath in AssemblyPaths)
            {
                A = LoadContext.LoadFromAssemblyPath(AssemblyPath);
                Types = GetCreatableTypesFunc(A);

                foreach (Type T in Types)
                {
                    Instance = CreateInstanceFunc(T);
                    if (Instance != null)
                        ResultList.Add(Instance);
                }
            }

            return ResultList.ToArray();
        }
        
        
        /* properties */
        /// <summary>
        /// The folder to start looking for external modules (assemblies), e.g. Debug\bin\Modules
        /// </summary>
        public string RootFolder { get; set; }
        /// <summary>
        /// A filename prefix denoting an external module, e.g. ewm_ for ewm_XXXXX
        /// </summary>
        public string Prefix { get; set; } = "";
        /// <summary>
        /// A function that returns true if an assembly of a specified path should be excluded.
        /// </summary>
        public Func<string, bool> ExcludeAssemblyFunc { get; set; } 
        /// <summary>
        /// A function that returns the creatable types contained in a specified assembly.
        /// </summary>
        public Func<Assembly, Type[]> GetCreatableTypesFunc { get; set; } 
        /// <summary>
        /// A function that creates and returns a plugin instance based on a specified type
        /// </summary>
        public Func<Type, TExternalModule> CreateInstanceFunc { get; set; }  

    }
}
