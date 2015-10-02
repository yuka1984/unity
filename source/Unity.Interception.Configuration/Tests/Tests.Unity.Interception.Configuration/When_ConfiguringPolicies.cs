// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringPolicies
    /// </summary>
     
    public class When_ConfiguringPolicies : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringPolicies()
            : base("Policies")
        {
            MainSetup();
        }

        private IUnityContainer GetConfiguredContainer(string containerName)
        {
            IUnityContainer container = new UnityContainer();
            section.Configure(container, containerName);
            return container;
        }

        [Fact]
        public void Then_CanConfigureAnEmptyPolicy()
        {
            IUnityContainer container = this.GetConfiguredContainer("oneEmptyPolicy");

            var policies = new List<InjectionPolicy>(container.ResolveAll<InjectionPolicy>());

            Assert.Equal(2, policies.Count);
            Assert.IsType<AttributeDrivenPolicy>(policies[0]);
            Assert.IsType<RuleDrivenPolicy>(policies[1]);
            Assert.Equal("policyOne", policies[1].Name);
        }

        [Fact]
        public void Then_MatchingRuleInPolicyIsConfigured()
        {
            IUnityContainer container = this.GetConfiguredContainer("policyWithGivenRulesAndHandlersTypes");

            GlobalCountCallHandler.Calls.Clear();

            container.RegisterType<Wrappable>("wrappable",
                new Interceptor<TransparentProxyInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            var wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["default"]);
        }

        [Fact]
        public void Then_RulesAndHandlersCanBeConfiguredExternalToPolicy()
        {
            IUnityContainer container
                = this.GetConfiguredContainer("policyWithExternallyConfiguredRulesAndHandlers");

            GlobalCountCallHandler.Calls.Clear();

            container.RegisterType<Wrappable>("wrappable",
                new Interceptor<TransparentProxyInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            var wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);
        }

        [Fact]
        public void Then_RulesAndHandlersCanHaveInjectionConfiguredInPolicyElement()
        {
            IUnityContainer container
                = this.GetConfiguredContainer("policyWithInjectedRulesAndHandlers");

            GlobalCountCallHandler.Calls.Clear();

            container
                .RegisterType<Wrappable>("wrappable",
                    new Interceptor<TransparentProxyInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());

            var wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);
        }

        [Fact]
        public void CanSetUpAPolicyWithLifetimeManagedInjectedRulesAndHandlers()
        {
            IUnityContainer container
                = this.GetConfiguredContainer("policyWithLifetimeManagedInjectedRulesAndHandlers");

            GlobalCountCallHandler.Calls.Clear();

            container
                .RegisterType<Wrappable>("wrappable",
                    new Interceptor<TransparentProxyInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);

            var matchingRuleRegistrations = container.Registrations.Where(r => r.RegisteredType == typeof(IMatchingRule));
            var callHandlerRegistrations = container.Registrations.Where(r => r.RegisteredType == typeof(ICallHandler));

            Assert.Equal(2, matchingRuleRegistrations.Count());
            Assert.Equal(
                1,
                matchingRuleRegistrations.Where(r => r.LifetimeManagerType == typeof(ContainerControlledLifetimeManager)).Count());
            Assert.Equal(
                1,
                matchingRuleRegistrations.Where(r => r.LifetimeManagerType == typeof(TransientLifetimeManager)).Count());

            Assert.Equal(2, callHandlerRegistrations.Count());
            Assert.Equal(
                1,
                callHandlerRegistrations.Where(r => r.LifetimeManagerType == typeof(ContainerControlledLifetimeManager)).Count());
            Assert.Equal(
                1,
                callHandlerRegistrations.Where(r => r.LifetimeManagerType == typeof(TransientLifetimeManager)).Count());
        }

        [Fact]
        public void Then_RulesAndHandlersInDifferentPoliciesCanHaveTheSameName()
        {
            IUnityContainer container
                = this.GetConfiguredContainer("policyWithDuplicateRuleAndHandlerNames");

            GlobalCountCallHandler.Calls.Clear();

            container
                .RegisterType<Wrappable>("wrappable",
                    new Interceptor<TransparentProxyInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());

            var wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();
            wrappable1.Method3();

            Assert.Equal(1, GlobalCountCallHandler.Calls["Method3Handler"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["Method2Handler"]);
        }
    }
}
