using Microsoft.Framework.DependencyInjection;
using Microsoft.Practices.Unity;

namespace WebApplication6.Unity
{
    internal class UnityServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IUnityContainer container;

        public UnityServiceScopeFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public IServiceScope CreateScope()
        {
            return new UnityServiceScope(CreateChildContainer());
        }

        private IUnityContainer CreateChildContainer()
        {
            var child = container.CreateChildContainer();
            child.AddExtension(new EnumerableResolutionExtension());
            return child;
        }
    }
}