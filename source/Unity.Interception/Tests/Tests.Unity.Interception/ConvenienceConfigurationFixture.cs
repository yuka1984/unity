// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.Unity.InterceptionExtension.Tests.ObjectsUnderTest;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
     
    public class ConvenienceConfigurationFixture
    {
        [Fact]
        public void CanSetUpAnEmptyRule()
        {
            // there is no visible effect for this, but it should still be resolved.
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            // empty
            container
                .Configure<Interception>()
                    .AddPolicy("policy1");

            List<InjectionPolicy> policies
                = new List<InjectionPolicy>(container.ResolveAll<InjectionPolicy>());

            Assert.Equal(2, policies.Count);
            Assert.IsType<AttributeDrivenPolicy>(policies[0]);
            Assert.IsType<RuleDrivenPolicy>(policies[1]);
            Assert.Equal("policy1", policies[1].Name);
        }

        [Fact]
        public void SettingUpRuleWithNullNameThrows()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            try
            {
                container.Configure<Interception>().AddPolicy(null);
                Assert.True(false, string.Format("should have thrown"));
            }
            catch (ArgumentException)
            {
            }
        }

        [Fact]
        public void SettingUpRuleWithEmptyNameThrows()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            try
            {
                container.Configure<Interception>().AddPolicy(string.Empty);
                Assert.True(false, string.Format("should have thrown"));
            }
            catch (ArgumentException)
            {
            }
        }

        [Fact]
        public void CanSetUpSeveralEmptyRules()
        {
            // there is no visible effect for this, but it should still be resolved.
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            // empty
            container
                .Configure<Interception>()
                    .AddPolicy("policy1").Interception
                    .AddPolicy("policy2");

            List<InjectionPolicy> policies
                = new List<InjectionPolicy>(container.ResolveAll<InjectionPolicy>());

            Assert.Equal(3, policies.Count);
            Assert.IsType<AttributeDrivenPolicy>(policies[0]);
            Assert.IsType<RuleDrivenPolicy>(policies[1]);
            Assert.Equal("policy1", policies[1].Name);
            Assert.IsType<RuleDrivenPolicy>(policies[2]);
            Assert.Equal("policy2", policies[2].Name);
        }

        [Fact]
        public void CanSetUpAPolicyWithGivenRulesAndHandlers()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            IMatchingRule rule1 = new AlwaysMatchingRule();
            ICallHandler handler1 = new CallCountHandler();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule(rule1)
                        .AddCallHandler(handler1);

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, ((CallCountHandler)handler1).CallCount);
        }

        [Fact]
        public void CanSetUpAPolicyWithGivenRulesAndHandlersTypes()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule(typeof(AlwaysMatchingRule))
                        .AddCallHandler(typeof(GlobalCountCallHandler));

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["default"]);
        }

        [Fact]
        public void CanSetUpAPolicyWithGivenRulesAndHandlersTypesWithGenerics()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule<AlwaysMatchingRule>()
                        .AddCallHandler<GlobalCountCallHandler>();

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["default"]);
        }

        [Fact]
        public void CanSetUpAPolicyWithInjectedRulesAndHandlers()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule<AlwaysMatchingRule>()
                        .AddCallHandler<GlobalCountCallHandler>(
                            new InjectionConstructor("handler1"))
                        .AddCallHandler<GlobalCountCallHandler>(
                            new InjectionConstructor("handler2"),
                            new InjectionProperty("Order", 10));

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);
        }

        [Fact]
        public void CanSetUpAPolicyWithNonGenericInjectedRulesAndHandlers()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule(typeof(AlwaysMatchingRule))
                        .AddCallHandler(
                            typeof(GlobalCountCallHandler),
                            new InjectionConstructor("handler1"))
                        .AddCallHandler(
                            typeof(GlobalCountCallHandler),
                            new InjectionConstructor("handler2"),
                            new InjectionProperty("Order", 10));

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);
        }

        [Fact]
        public void CanSetUpAPolicyWithExternallyConfiguredRulesAndHandlers()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule("rule1")
                        .AddCallHandler("handler1")
                        .AddCallHandler("handler2").Interception.Container
                .RegisterType<IMatchingRule, AlwaysMatchingRule>("rule1")
                .RegisterType<ICallHandler, GlobalCountCallHandler>(
                    "handler1",
                    new InjectionConstructor("handler1"))
                .RegisterType<ICallHandler, GlobalCountCallHandler>(
                    "handler2",
                    new InjectionConstructor("handler2"),
                    new InjectionProperty("Order", 10));

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);
        }

        [Fact]
        public void CanSetUpAPolicyWithNamedInjectedRulesAndHandlers()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule<AlwaysMatchingRule>("rule1")
                        .AddCallHandler<GlobalCountCallHandler>(
                            "handler1",
                            new InjectionConstructor("handler1"))
                        .AddCallHandler<GlobalCountCallHandler>(
                            "handler2",
                            new InjectionConstructor("handler2"),
                            new InjectionProperty("Order", 10));

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            GlobalCountCallHandler handler1 = (GlobalCountCallHandler)container.Resolve<ICallHandler>("handler1");
            GlobalCountCallHandler handler2 = (GlobalCountCallHandler)container.Resolve<ICallHandler>("handler2");

            Assert.Equal(1, GlobalCountCallHandler.Calls["handler1"]);
            Assert.Equal(1, GlobalCountCallHandler.Calls["handler2"]);
            Assert.Equal(0, handler1.Order);
            Assert.Equal(10, handler2.Order);
        }

        [Fact]
        public void CanSetUpAPolicyWithLifetimeManagedNamedInjectedRulesAndHandlers()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container
                .Configure<Interception>()
                    .AddPolicy("policy1")
                        .AddMatchingRule<AlwaysMatchingRule>(
                            "rule1",
                            new ContainerControlledLifetimeManager())
                        .AddCallHandler<CallCountHandler>(
                            "handler1",
                            (LifetimeManager)null)
                        .AddCallHandler<CallCountHandler>(
                            "handler2",
                            new ContainerControlledLifetimeManager(),
                            new InjectionProperty("Order", 10));

            GlobalCountCallHandler.Calls.Clear();

            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>("wrappable", new VirtualMethodInterceptor());

            Wrappable wrappable1 = container.Resolve<Wrappable>("wrappable");
            wrappable1.Method2();

            CallCountHandler handler1 = (CallCountHandler)container.Resolve<ICallHandler>("handler1");
            CallCountHandler handler2 = (CallCountHandler)container.Resolve<ICallHandler>("handler2");

            Assert.Equal(0, handler1.CallCount);     // not lifetime maanaged
            Assert.Equal(1, handler2.CallCount);     // lifetime managed
        }

        [Fact]
        public void SettingUpAPolicyWithANullRuleElementThrows()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            try
            {
                container
                    .Configure<Interception>()
                        .AddPolicy("policy1")
                            .AddMatchingRule(typeof(AlwaysMatchingRule))
                            .AddMatchingRule((string)null)
                            .AddCallHandler(new CallCountHandler());
                Assert.True(false, string.Format("Should have thrown"));
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
