﻿namespace WebLib.AspNet
{
    /// <summary>
    /// Generates unique Ids for HTML elements.
    /// <para>WARNING: HTML element id is case-sensitive.</para>
    /// </summary>
    static internal class ElementIdGenerator
    {
        static object syncLock = new LockObject();

        static Dictionary<string, int> Dic = new Dictionary<string, int>();

        /// <summary>
        /// Generates and returns a unique id for an HTML Element.
        /// </summary>
        static public string Next(string Prefix = "")
        {
            lock(syncLock)
            {
                if (string.IsNullOrWhiteSpace(Prefix))
                    Prefix = "el";

                int Index;

                Index = Dic.ContainsKey(Prefix) ? Dic[Prefix] : 2000; // tp javascript Id generations starts from 0
                Index++;
                Dic[Prefix] = Index;

                return Prefix.EndsWith("-") ? $"{Prefix}{Index}" : $"{Prefix}-{Index}";
            }

        }
    }
}
