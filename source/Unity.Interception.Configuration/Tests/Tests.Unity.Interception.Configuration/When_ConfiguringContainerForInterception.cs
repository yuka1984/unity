// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Summary description for When_ConfiguringContainerForInterception
    /// </summary>
     
    public class When_ConfiguringContainerForInterception : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerForInterception()
            : base("InterceptionInjectionMembers")
        {
            MainSetup();
        }

        protected override void Arrange()
        {
            GlobalCountInterceptionBehavior.Calls.Clear();
            base.Arrange();
        }

        protected override void Teardown()
        {
            GlobalCountInterceptionBehavior.Calls.Clear();
            base.Teardown();
        }

        private IUnityContainer ConfiguredContainer(string containerName)
        {
            return new UnityContainer().LoadConfiguration(this.section, containerName);
        }

        [Fact]
        public void Then_CanConfigureInterceptorThroughConfigurationFile()
        {
            var container = this.ConfiguredContainer("configuringInterceptorThroughConfigurationFile");

            var callCount = new CallCountInterceptionBehavior();
            container.RegisterType<Interceptable>(new InterceptionBehavior(callCount));

            var instance = container.Resolve<Interceptable>();
            instance.DoSomething();

            Assert.Equal(1, callCount.CallCount);
        }

        [Fact]
        public void Then_CanConfigureAdditionalInterfaceThroughConfigurationFile()
        {
            IUnityContainer container = this.ConfiguredContainer("configuringAdditionalInterfaceThroughConfigurationFile");

            var callCount = new CallCountInterceptionBehavior();
            container.RegisterType<Interceptable>(
                new InterceptionBehavior(callCount),
                new Interceptor(new VirtualMethodInterceptor()));

            var instance = container.Resolve<Interceptable>();
            instance.DoSomething();

            Assert.Equal(1, callCount.CallCount);
            Assert.True(instance is IServiceProvider);
        }

        [Fact]
        public void Then_CanConfigureResolvedInterceptionBehavior()
        {
            IUnityContainer container =
                this.ConfiguredContainer("configuringInterceptionBehaviorWithTypeThroughConfigurationFile");

            container.RegisterType<Interceptable>(new Interceptor(new VirtualMethodInterceptor()));

            var instance = container.Resolve<Interceptable>();
            instance.DoSomething();

            Assert.Equal(1, GlobalCountInterceptionBehavior.Calls.Values.First());
        }

        [Fact]
        public void Then_CanConfigureNamedResolvedBehavior()
        {
            IUnityContainer container = this.ConfiguredContainer("canConfigureNamedBehavior")
                .RegisterType<Interceptable>(new Interceptor(new VirtualMethodInterceptor()));

            var instance = container.Resolve<Interceptable>();
            instance.DoSomething();

            Assert.Equal(1, GlobalCountInterceptionBehavior.Calls["fixed"]);
        }

        [Fact]
        public void Then_CanConfigureBehaviorWithNameOnly()
        {
            var callCount = new CallCountInterceptionBehavior();

            IUnityContainer container = this.ConfiguredContainer("canConfigureBehaviorWithNameOnly")
                .RegisterType<Interceptable>(new Interceptor(new VirtualMethodInterceptor()))
                .RegisterInstance<IInterceptionBehavior>("call count", callCount);

            var instance = container.Resolve<Interceptable>();
            instance.DoSomething();

            Assert.Equal(1, callCount.CallCount);
        }

        [Fact]
        public void Then_CanConfigureDefaultInterceptor()
        {
            IUnityContainer container = this.ConfiguredContainer("configuringDefaultInterceptor")
                .Configure<Interception>()
                    .AddPolicy("all")
                        .AddMatchingRule<AlwaysMatchingRule>()
                        .AddCallHandler<GlobalCountCallHandler>()
                .Container;

            var instance1 = container.Resolve<Wrappable>();
            var instance2 = container.Resolve<Wrappable>("two");

            Assert.True(RemotingServices.IsTransparentProxy(instance1));
            Assert.True(RemotingServices.IsTransparentProxy(instance2));
        }

        [Fact]
        public void Then_CanAddInterfaceThroughConfiguredBehavior()
        {
            IUnityContainer container = this.ConfiguredContainer("addingInterfacesImplicitlyThroughBehavior");

            var instance = container.Resolve<Interceptable>();
            Assert.NotNull(instance as IAdditionalInterface);
        }

        [Fact]
        public void Then_CanAddInterfaceThroughExplicitConfiguration()
        {
            IUnityContainer container = this.ConfiguredContainer("addingInterfacesExplicitlyWithBehavior");

            var instance = container.Resolve<Interceptable>();
            Assert.NotNull(instance as IAdditionalInterface);
        }

        [Fact]
        public void Then_MultipleBehaviorsCanBeConfigured()
        {
            var container = this.ConfiguredContainer("multipleBehaviorsOnOneRegistration");
            var instance = container.Resolve<Interceptable>();

            instance.DoSomething();

            var countHandler = (CallCountInterceptionBehavior)container.Resolve<IInterceptionBehavior>("fixed");

            Assert.Equal(1, countHandler.CallCount);

            Assert.NotNull(instance as IAdditionalInterface);
        }
    }
}
