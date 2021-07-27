﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Reflection;
using System.IO;

namespace Tripous
{

    /// <summary>
    /// Extensions
    /// </summary>
    static public class AssemblyExtensions
    {
        /// <summary>
        /// Gets the full path of the Assembly
        /// </summary>
        static public string GetFullPath(this Assembly A)
        {
			return A == null? string.Empty: A.Location;
            
			/* 
			if (A == null)
                return string.Empty;

			string sFile = @"file:///";
            string Result = A.GetName().CodeBase;
            if (Result.StartsWith(sFile))
                Result = Result.Substring(sFile.Length);

            return Path.GetFullPath(Result);
			 */

        }
        /// <summary>
        /// Gets the directory where the Assembly resides.
        /// <para>The returned string includes a trailing path separator.</para>
        /// </summary>
        static public string GetFolder(this Assembly A)
        {
            if (A == null)
                return string.Empty;

            string Result = Path.GetDirectoryName(GetFullPath(A: A));

            if (!Result.EndsWith(Path.DirectorySeparatorChar.ToString()))
                Result = Result + Path.DirectorySeparatorChar;
            return Result;
        }
        /// <summary>
        /// Gets the filename of the Assembly without path and extension
        /// </summary>
        static public string GetFileName(this Assembly A)
        {
            if (A == null)
                return string.Empty;

            return Path.GetFileNameWithoutExtension(GetFullPath(A));
        }
        /// <summary>
        /// Returns an array of Type of A types, in a safe manner
        /// </summary>
        static public Type[] GetTypesSafe(this Assembly A)
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
    }
}
