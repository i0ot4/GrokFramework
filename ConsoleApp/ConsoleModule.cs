using Grok;
using Grok.Modularity;

namespace ConsoleApp
{
    public class CoreModule : GrokModule
    {
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("✅ CoreModule initialized");
        }
    }


    [DependsOn(typeof(CoreModule))]
    public class ConsoleModule : GrokModule
    {
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("✅ ApplicationModule initialized (after CoreModule)");
        }
    }
}
