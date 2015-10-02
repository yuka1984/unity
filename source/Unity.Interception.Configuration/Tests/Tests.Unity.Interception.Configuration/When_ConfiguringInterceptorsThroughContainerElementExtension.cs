// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Runtime.Remoting;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringInterceptorsThroughContainerElementExtension
    /// </summary>
     
    public class When_ConfiguringInterceptorsThroughContainerElementExtension : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringInterceptorsThroughContainerElementExtension()
            : base("InterceptorsThroughContainerElementExtension")
        {
            MainSetup();
        }

        private IUnityContainer GetContainer(string containerName)
        {
            return new UnityContainer()
                .LoadConfiguration(section, containerName)
                .Configure<Interception>()
                .AddPolicy("policy")
                .AddMatchingRule<AlwaysMatchingRule>()
                .AddCallHandler<CallCountHandler>()
                .Container;
        }

        [Fact]
        public void Then_CanConfigureDefaultInterceptorForType()
        {
            IUnityContainer container = this.GetContainer("configuringDefaultInterceptorForType");

            var anonymous = container.Resolve<Wrappable>();
            var named = container.Resolve<Wrappable>("name");

            Assert.True(RemotingServices.IsTransparentProxy(anonymous));
            Assert.True(RemotingServices.IsTransparentProxy(named));
        }

        [Fact]
        public void Then_CanConfigureVirtualMethodInterceptor()
        {
            IUnityContainer container = this.GetContainer("configuringDefaultInterceptorForTypeWithVirtualMethodInterceptor");

            var anonymous = container.Resolve<WrappableWithVirtualMethods>();
            var named = container.Resolve<WrappableWithVirtualMethods>("name");

            Assert.Same(typeof(WrappableWithVirtualMethods), anonymous.GetType().BaseType);
            Assert.Same(typeof(WrappableWithVirtualMethods), named.GetType().BaseType);
        }

        [Fact]
        public void Then_CanConfigureInterceptorForType()
        {
            IUnityContainer container = this.GetContainer("configuringInterceptorForType");

            var anonymous = container.Resolve<Wrappable>();
            var named = container.Resolve<Wrappable>("name");

            Assert.True(RemotingServices.IsTransparentProxy(anonymous));
            Assert.False(RemotingServices.IsTransparentProxy(named));
        }

        [Fact]
        public void Then_CanConfigureInterceptorForTypeAndName()
        {
            IUnityContainer container = this.GetContainer("configuringInterceptorForTypeAndName");

            var anonymous = container.Resolve<Wrappable>();
            var named = container.Resolve<Wrappable>("name");

            Assert.False(RemotingServices.IsTransparentProxy(anonymous));
            Assert.True(RemotingServices.IsTransparentProxy(named));
        }

        [Fact]
        public void Then_CanConfigureSeveralInterceptors()
        {
            IUnityContainer container = this.GetContainer("configuringSeveralInterceptors");

            var anonymous = container.Resolve<Wrappable>();
            var named = container.Resolve<Wrappable>("name");

            Assert.True(RemotingServices.IsTransparentProxy(anonymous));
            Assert.True(RemotingServices.IsTransparentProxy(named));
        }

        [Fact]
        public void Then_CanMixDefaultAndNonDefaultInterceptors()
        {
            IUnityContainer container = this.GetContainer("mixingDefaultAndNonDefaultInterceptors");

            var anonymousWrappable = container.Resolve<Wrappable>();
            var namedWrappable = container.Resolve<Wrappable>("name");
            var anonymousWrappableWithProperty
                = container.Resolve<WrappableWithProperty>();
            var namedWrappableWithProperty
                = container.Resolve<WrappableWithProperty>("name");

            Assert.True(RemotingServices.IsTransparentProxy(anonymousWrappable));
            Assert.True(RemotingServices.IsTransparentProxy(namedWrappable));
            Assert.True(RemotingServices.IsTransparentProxy(anonymousWrappableWithProperty));
            Assert.False(RemotingServices.IsTransparentProxy(namedWrappableWithProperty));
        }

        [Fact]
        public void Then_CanMixTransparentProxyAndVirtualMethodInterceptors()
        {
            IUnityContainer container = this.GetContainer("mixingTransparentProxyAndVirtualMethodInterceptors");

            var anonymousWrappable = container.Resolve<Wrappable>();
            var namedWrappable = container.Resolve<Wrappable>("name");
            var anonymousWrappableWrappableWithVirtualMethods
                = container.Resolve<WrappableWithVirtualMethods>();
            var namedWrappableWrappableWithVirtualMethods
                = container.Resolve<WrappableWithVirtualMethods>("name");

            Assert.True(RemotingServices.IsTransparentProxy(anonymousWrappable));
            Assert.True(RemotingServices.IsTransparentProxy(namedWrappable));
            Assert.Same(
                typeof(WrappableWithVirtualMethods),
                anonymousWrappableWrappableWithVirtualMethods.GetType());
            Assert.Same(
                typeof(WrappableWithVirtualMethods),
                namedWrappableWrappableWithVirtualMethods.GetType().BaseType);
        }

        [Fact]
        public void Then_CanSpecifyInterceptorUsingTypeConverter()
        {
            this.GetContainer("specifyingInterceptorWithTypeConverter");

            Assert.Equal("source value", MyTransparentProxyInterceptorTypeConverter.SourceValue);
        }
    }
}
