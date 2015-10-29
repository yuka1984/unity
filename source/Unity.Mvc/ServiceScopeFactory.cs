using Microsoft.Framework.DependencyInjection;
using Microsoft.Practices.Unity;

namespace Unity.Mvc
{
    public class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IUnityContainer container;

        public ServiceScopeFactory(IUnityContainer container)
        {
            this.container = container;
        }

        IServiceScope IServiceScopeFactory.CreateScope()
        {
            return container.Resolve<IServiceScope>();
        }
    }
}