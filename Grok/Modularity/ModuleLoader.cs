using Grok.DependencyInjection;
using Grok.System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Grok.Modularity
{
    public class ModuleLoader : IModuleLoader, ISingletonDependency
    {
        private static List<Type> SortModulesByDependencies(List<Type> moduleTypes)
        {
            var sorted = new List<Type>();
            var visited = new HashSet<Type>();

            void Visit(Type module)
            {
                if (visited.Contains(module))
                    return;

                visited.Add(module);

                var dependsAttr = module.GetCustomAttribute<DependsOnAttribute>();
                if (dependsAttr != null)
                {
                    foreach (var dep in dependsAttr.DependedTypes)
                    {
                        if (!moduleTypes.Contains(dep))
                            throw new Exception($"Module {module.Name} depends on {dep.Name}, which is not loaded.");

                        Visit(dep);
                    }
                }

                sorted.Add(module);
            }

            foreach (var module in moduleTypes)
                Visit(module);

            return sorted;
        }

        public IGrokModuleDescriptor[] LoadModules(
            [NotNull] IServiceCollection services,
            [NotNull] Type startupModuleType)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(startupModuleType, nameof(startupModuleType));

            var modules = GetDescriptors(services, startupModuleType);
            modules = SortByDependency(modules, startupModuleType);

            return modules.ToArray();
        }

        private List<IGrokModuleDescriptor> GetDescriptors(IServiceCollection services, Type startupModuleType)
        {
            var modules = new List<GrokModuleDescriptor>();

            FillModules(modules, services, startupModuleType);

            SetDependencies(modules);

            return modules.Cast<IGrokModuleDescriptor>().ToList();
        }

        protected virtual void FillModules(List<GrokModuleDescriptor> modules, IServiceCollection services, Type startupModuleType)
        {
            foreach (var module in GrokModuleHelper.FindAllModuleTypes(startupModuleType))
            {
                modules.Add(CreateModuleDescriptor(services, module));
            }

        }

        protected virtual GrokModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType)
        {
            var instance = (GrokModule)Activator.CreateInstance(moduleType)!;
            services.AddSingleton(moduleType, instance);

            return new GrokModuleDescriptor(moduleType, instance);
        }

        protected virtual void SetDependencies(List<GrokModuleDescriptor> modules)
        {
            foreach (var module in modules)
            {
                SetDependencies(modules, module);
            }
        }

        protected virtual void SetDependencies(List<GrokModuleDescriptor> modules, GrokModuleDescriptor module)
        {
            foreach (var dependedModuleType in GrokModuleHelper.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = modules.FirstOrDefault(d => d.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new Exception("Could not find a depended module " +
                        dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.AddDependency(dependedModule);
            }
        }

        protected virtual List<IGrokModuleDescriptor> SortByDependency(List<IGrokModuleDescriptor> modules, Type startupModuleType)
        {
            var sortModules = modules.SortByDependencies(m => m.Dependencies);
            sortModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
            return sortModules;
        }

    }
}
