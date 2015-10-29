using System;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Practices.Unity;

namespace WebApplication6.Unity
{
    internal class UnityServiceScope : IServiceScope
    {
        private readonly IUnityContainer container;
        private readonly IServiceProvider provider;

        public UnityServiceScope(IUnityContainer container)
        {
            this.container = container;
            provider = container.Resolve<IServiceProvider>();
        }

        public IServiceProvider ServiceProvider
        {
            get { return provider; }
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}