using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tripous.Web
{
    /// <summary>
    /// Generates unique Ids for HTML elements.
    /// <para>WARNING: HTML element id is case-sensitive.</para>
    /// </summary>
    internal class ElementIdGenerator
    {
        Dictionary<string, int> Dic = new Dictionary<string, int>();

        /// <summary>
        /// Generates and returns a unique id for an HTML Element.
        /// </summary>
        public string GetNextId(string Prefix = "")
        {
            if (string.IsNullOrWhiteSpace(Prefix))
                Prefix = "el";

            int Index;

            lock (this.GetType())
            {
                Index = Dic.ContainsKey(Prefix) ? Dic[Prefix] : 2000; // tp javascript Id generations starts from 0
                Index++;
                Dic[Prefix] = Index;
            }

            return  Prefix.EndsWith("-")? $"{Prefix}{Index}" : $"{Prefix}-{Index}";
        }
    }
}
