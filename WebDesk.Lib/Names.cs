using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;

namespace WebLib
{

    /// <summary>
    /// Generates unique names
    /// </summary>
    static public class Names
    {
        static object syncLock = new LockObject();

        static Dictionary<string, int> Dic = new Dictionary<string, int>();

        /// <summary>
        /// Generates and returns a unique name
        /// </summary>
        static public string Next(string Prefix)
        {
            lock (syncLock)
            {
                if (string.IsNullOrWhiteSpace(Prefix))
                    Sys.Throw("Cannot generate a unique name. No prefix is defined.");

                int Index;

                Index = Dic.ContainsKey(Prefix) ? Dic[Prefix] : 2000; // tp javascript Id generations starts from 0
                Index++;
                Dic[Prefix] = Index;

                return Prefix.EndsWith("-") ? $"{Prefix}{Index}" : $"{Prefix}-{Index}";
            }

        }
    }
}
