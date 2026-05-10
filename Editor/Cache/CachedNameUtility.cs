using System;
using System.Linq;
using System.Collections.Generic;
using Assembly = System.Reflection.Assembly;

namespace Thimble.Editor
{
    public static class CachedNameUtility
    {
        private static readonly HashSet<string> internalAssemblyPrefixes = new()
        {
            "Unity.",
            "UnityEditor.",
            "UnityEngine.",
            "JetBrains.",
            "System.",
            "Microsoft.",
            "Mono.",
            "ICSharpCode.",
            "Newtonsoft."
        };

        private static readonly HashSet<string> internalAssemblyNames = new()
        {
            "Bee.BeeDriver",
            "ExCSS.Unity",
            "Mono.Security",
            "mscorlib",
            "netstandard",
            "Newtonsoft.Json",
            "nunit.framework",
            "ReportGeneratorMerged",
            "Unrelated",
            "SyntaxTree.VisualStudio.Unity.Bridge",
            "SyntaxTree.VisualStudio.Unity.Messaging"
        };

        public static IEnumerable<Assembly> GetUserCreatedAssemblies(this AppDomain appDomain)
        {
            // Iterate through all assemblies in the AppDomain
            foreach (var assembly in appDomain.GetAssemblies())
            {
                // Skip dynamic assemblies
                if (assembly.IsDynamic) continue;

                // Get the assembly name
                string assemblyName = assembly.GetName().Name;

                // Skip editor assemblies
                if (assemblyName.Contains("Editor")) continue;

                // Skip internal/system assemblies by prefix
                if (internalAssemblyPrefixes.Any(prefix => assemblyName.Contains(prefix))) continue;

                // Skip internal/system assemblies
                if (internalAssemblyNames.Contains(assemblyName)) continue;

                // Yield return user-created assembly
                yield return assembly;
            }
        }

        public static Dictionary<string, List<CachedYarnObjectInfo>> GetStaticCachedNames()
        {
            // Initialize the cache dictionary to store YarnName as the key and a list of CachedYarnObjectInfo as the value
            Dictionary<string, List<CachedYarnObjectInfo>> cache = new();

            // Get the current AppDomain
            var appDomain = AppDomain.CurrentDomain.GetUserCreatedAssemblies();

            // Iterate through user-created assemblies and collect cached names
            foreach (var assembly in appDomain)
            {
                // Get all types in the assembly
                var types = assembly.GetTypes();

                // Iterate through types and find those that implement ICachedYarnObject
                foreach (var type in types)
                {
                    // Check if the type implements ICachedYarnObject and is not an interface or abstract class
                    if (typeof(ICachedYarnObject).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        // Create an instance of the type
                        var instance = Activator.CreateInstance(type) as ICachedYarnObject;

                        // Add the instance's YarnName, ObjectName, and ConditionName to the cache
                        if (instance != null)
                        {
                            // If the cache doesn't already contain the YarnName, add it
                            if (!cache.ContainsKey(instance.YarnName)) cache[instance.YarnName] = new List<CachedYarnObjectInfo>();

                            // Add the ObjectName and ConditionName to the cache for the YarnName
                            cache[instance.YarnName].Add(CachedYarnObjectInfo.FromReference(instance));
                        }
                    }
                }
            }

            // Return the cache
            return cache;
        }
    }
}
