using Grok.DependencyInjection;
using Grok.Modularity.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Grok.Modularity
{
    public class ModuleLoader
    {
        public static List<GrokModule> LoadModules(Assembly assembly, IServiceCollection services)
        {
            var moduleTypes = assembly
                .GetTypes()
                .Where(t => typeof(GrokModule).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();

            var modules = moduleTypes
                .Select(t => (GrokModule)Activator.CreateInstance(t)!)
                .ToList();

            var sortedModules = SortModulesByDependencies(moduleTypes)
                .Select(t => modules.First(m => m.GetType() == t))
                .ToList();

            var _context = new ModuleContext(services);

            foreach (var module in modules)
            {
                module.SetContext(_context);

                var registrar = new DefaultConventionalRegistrar();
                var types = module.GetType().Assembly.GetTypes();

                registrar.AddTypes(services, types);

                module.PreInitialize();
            }

            foreach (var module in modules) module.Initialize();
            foreach (var module in modules) module.PostInitialize();

            return sortedModules;

        }

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
                    foreach (var dep in dependsAttr.DependsOnTypes)
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

        //public static List<GrokModule> LoadModules(Assembly entryAssembly, IServiceCollection services)
        //{
        //    // 1. اكتشاف جميع أنواع الـ Modules
        //    var moduleTypes = entryAssembly.GetTypes()
        //        .Where(t => typeof(GrokModule).IsAssignableFrom(t) && !t.IsAbstract)
        //        .ToList();

        //    var modules = moduleTypes
        //        .Select(t => (GrokModule)Activator.CreateInstance(t)!)
        //        .ToList();

        //    // 2. ترتيب Modules حسب الـ DependsOn
        //    var sortedModules = SortModulesByDependencies(moduleTypes)
        //        .Select(t => modules.First(m => m.GetType() == t))
        //        .ToList();

        //    // 3. إنشاء ModuleContext
        //    var _context = new ModuleContext(services);

        //    // 4. PreInitialize
        //    foreach (var module in sortedModules)
        //    {
        //        module.SetContext(_context);
        //        module.PreInitialize();
        //    }

        //    var _registrar = new DefaultConventionalRegistrar();
        //    // 5. Conventional registration لكل Assembly
        //    foreach (var module in sortedModules)
        //    {
        //        _registrar.AddAssembly(services, module.GetType().Assembly);
        //    }

        //    // 6. Initialize + PostInitialize
        //    foreach (var module in sortedModules) module.Initialize();
        //    foreach (var module in sortedModules) module.PostInitialize();

        //    return sortedModules;
        //}

        //private static List<Type> SortModulesByDependencies(List<Type> moduleTypes)
        //{
        //    var sorted = new List<Type>();
        //    var visited = new HashSet<Type>();

        //    void Visit(Type module)
        //    {
        //        if (visited.Contains(module)) return;

        //        visited.Add(module);

        //        var dependsAttr = module.GetCustomAttribute<DependsOnAttribute>();
        //        if (dependsAttr != null)
        //        {
        //            foreach (var dep in dependsAttr.DependsOnTypes)
        //            {
        //                if (!moduleTypes.Contains(dep))
        //                    throw new Exception($"Module {module.Name} depends on {dep.Name}, which is not loaded.");

        //                Visit(dep);
        //            }
        //        }

        //        sorted.Add(module);
        //    }

        //    foreach (var module in moduleTypes)
        //        Visit(module);

        //    return sorted;
        //}

    }
}
