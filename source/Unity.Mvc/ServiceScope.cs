using System;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Practices.Unity;

namespace Unity.Mvc
{
    public class ServiceScope : IServiceScope
    {
        private readonly IUnityContainer container;
        private readonly IServiceProvider serviceProvider;

        public ServiceScope(IUnityContainer container)
        {
            this.container = container.CreateChildContainer();
            serviceProvider = this.container.Resolve<IServiceProvider>();
        }

        IServiceProvider IServiceScope.ServiceProvider
        {
            get
            {
                return serviceProvider;
            }
        }

        void IDisposable.Dispose()
        {
            container.Dispose();
        }
    }
}