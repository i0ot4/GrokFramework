using Grok.System;
using System.Reflection;

namespace Grok.Modularity
{
    public static class GrokModuleHelper
    {
        public static Assembly[] GetAllAssemblies(Type moduleType)
        {
            GrokModule.CheckGrokModuleType(moduleType);
            var assemblies = new List<Assembly>();


            var moduleAssembly = moduleType.Assembly;
            assemblies.Add(moduleAssembly);
            // You can add more logic here to include referenced assemblies if needed.
            return assemblies.ToArray();
        }

        public static List<Type> FindAllModuleTypes(Type startupModuleType)
        {
            var moduleTypes = new List<Type>();
            //logger?.Log(LogLevel.Debug, "Loaded ABP modules:");
            Console.WriteLine($"LogLevel.Debug, Loaded ABP modules:");
            AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType);
            return moduleTypes;
        }

        public static void AddModuleAndDependenciesRecursively(List<Type> moduleTypes, Type moduleType, int depth = 0)
        {
            GrokModule.CheckGrokModuleType(moduleType);
            moduleTypes.AddIfNotContains(moduleType);

            Console.WriteLine($"LogLevel.Debug, {new string(' ', depth * 2)}- {moduleType.FullName}");
            //logger?.Log(LogLevel.Debug, $"{new string(' ', depth * 2)}- {moduleType.FullName}");
            foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
            {
                AddModuleAndDependenciesRecursively(moduleTypes, moduleType, depth + 1);
            }
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            GrokModule.CheckGrokModuleType(moduleType);
            var dependencies = new List<Type>();

            var dependencyDescriptors = moduleType.Assembly
                .GetCustomAttributes()
                .OfType<IDependedTypesProvider>();

            foreach (var descriptor in dependencyDescriptors)
            {
                foreach (var dependedModuleType in descriptor.GetDependedTypes())
                {
                    dependencies.AddIfNotContains(dependedModuleType);
                }
            }


            return dependencies;

        }

    }
}
