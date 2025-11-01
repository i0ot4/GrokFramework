using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.Loader;

namespace Grok.Reflection
{
    internal static class AssemblyHelper
    {
        public static Type[] GetLoadableTypes(Assembly assembly, ILogger logger)
        {
            Type[] types;

            try
            {
                types = assembly
                    .GetTypes()
                    .Where(type => type != null && type.IsClass && !type.IsAbstract && !type.IsGenericType)
                    .ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types
                    .Where(type => type != null && type.IsClass && !type.IsAbstract && !type.IsGenericType)
                    .Select(x => x!)
                    .ToArray();

                logger.LogError($"[GorkModuleLoader] Failed to fully load assembly: {assembly.FullName}");
                logger.LogError($"Successfully loaded {types.Length} types, but some failed to load.");

                if (ex.LoaderExceptions != null && ex.LoaderExceptions.Length > 0)
                {
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        if (loaderException == null) continue;

                        logger.LogError($"[LoaderException] {loaderException.GetType().Name}: {loaderException.Message}");

                        if (loaderException is FileNotFoundException fnf)
                        {
                            logger.LogWarning($"Missing Assembly: {fnf.FileName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"[GalaxyModuleLoader] Unexpected error while loading assembly {assembly.FullName}");
                logger.LogError(ex.ToString());
                types = Array.Empty<Type>();
            }

            return types;
        }

        public static List<Assembly> LoadAssemblies(string folderPath, SearchOption searchOption)
        {
            return GetAssemblyFiles(folderPath, searchOption)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToList();
        }

        public static IEnumerable<string> GetAssemblyFiles(string folderPath, SearchOption searchOption)
        {
            return Directory
                .EnumerateFiles(folderPath, "*.*", searchOption)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));
        }

        public static IReadOnlyList<Type> GetAllTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }
    }
}
