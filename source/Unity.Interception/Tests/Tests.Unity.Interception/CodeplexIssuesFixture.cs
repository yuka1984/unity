// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
    /// <summary>
    /// Summary description for CodeplexIssuesFixture
    /// </summary>
     
    public class CodeplexIssuesFixture
    {
        public interface IRepository { }
        public class TestRepository : IRepository { }
        public class TestService
        {
            public TestService(IRepository repository)
            {
            }
        }

        [Fact]
        public void DependenciesAndInterceptionMixProperly()
        {
            var container = new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterType<IRepository, TestRepository>()
                .RegisterType<TestService>(
                    new Interceptor<VirtualMethodInterceptor>());

            var svc1 = container.Resolve<TestService>();
            var svc2 = container.Resolve<TestService>();

            Assert.NotSame(svc1, svc2);
            Assert.NotNull(svc1 as IInterceptingProxy);
            Assert.NotNull(svc2 as IInterceptingProxy);
        }
    }
}
