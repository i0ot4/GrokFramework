using Grok.Modularity.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Grok
{
    public abstract class GrokModule
    {
        protected IModuleContext Context { get; private set; }

        internal void SetContext(IModuleContext context)
        {
            Context = context;
        }

        public virtual void PreInitialize() { }

        public virtual void Initialize()
        {
            if (Context != null)
            {
                Context.Services.AddByConvention(this.GetType().Assembly);
            }
        }

        public virtual void PostInitialize() { }


    }

}
