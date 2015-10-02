// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
     
    public class InterceptionConfigurationFixture
    {
        [Fact]
        public void CanSetUpInterceptorThroughInjectionMember()
        {
            CallCountHandler handler = new CallCountHandler();

            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.Configure<Interception>()
                .AddPolicy("policy")
                    .AddMatchingRule<AlwaysMatchingRule>()
                    .AddCallHandler(handler);

            container.RegisterType<IInterface, BaseClass>(
                "test",
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            IInterface instance = container.Resolve<IInterface>("test");

            instance.DoSomething("1");

            Assert.Equal(1, handler.CallCount);
        }

        [Fact]
        public void CanSetUpInterceptorThroughInjectionMemberForExistingInterceptor()
        {
            CallCountInterceptionBehavior interceptionBehavior = new CallCountInterceptionBehavior();

            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container.RegisterType<IInterface, BaseClass>(
                "test",
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior(interceptionBehavior));

            IInterface instance = container.Resolve<IInterface>("test");

            instance.DoSomething("1");

            Assert.Equal(1, interceptionBehavior.CallCount);
        }

        [Fact]
        public void CanSetUpAdditionalInterfaceThroughInjectionMemberForInstanceInterception()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            int invokeCount = 0;

            container.RegisterType<IInterface, BaseClass>(
                "test",
                new Interceptor<InterfaceInterceptor>(),
                new AdditionalInterface(typeof(IOtherInterface)),
                new InterceptionBehavior(
                    new DelegateInterceptionBehavior(
                        (mi, gn) => { invokeCount++; return mi.CreateMethodReturn(0); })));

            IInterface instance = container.Resolve<IInterface>("test");

            ((IOtherInterface)instance).DoSomethingElse("1");

            Assert.Equal(1, invokeCount);
        }

        [Fact]
        public void CanSetUpAdditionalInterfaceThroughInjectionMemberForTypeInterception()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            int invokeCount = 0;

            container.RegisterType<IInterface, BaseClass>(
                "test",
                new Interceptor<VirtualMethodInterceptor>(),
                new AdditionalInterface(typeof(IOtherInterface)),
                new InterceptionBehavior(
                    new DelegateInterceptionBehavior(
                        (mi, gn) => { invokeCount++; return mi.CreateMethodReturn(0); })));

            IInterface instance = container.Resolve<IInterface>("test");

            ((IOtherInterface)instance).DoSomethingElse("1");

            Assert.Equal(1, invokeCount);
        }

        [Fact]
        public void CanSetUpAdditionalInterfaceThroughGenericInjectionMemberForTypeInterception()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            int invokeCount = 0;

            container.RegisterType<IInterface, BaseClass>(
                "test",
                new Interceptor<VirtualMethodInterceptor>(),
                new AdditionalInterface<IOtherInterface>(),
                new InterceptionBehavior(
                    new DelegateInterceptionBehavior(
                        (mi, gn) => { invokeCount++; return mi.CreateMethodReturn(0); })));

            IInterface instance = container.Resolve<IInterface>("test");

            ((IOtherInterface)instance).DoSomethingElse("1");

            Assert.Equal(1, invokeCount);
        }

        [Fact]
        public void ConfiguringAnAdditionalInterfaceWithANonInterfaceTypeThrows()
        {
            Assert.Throws<ArgumentException>(() => new AdditionalInterface(typeof(int)));
        }

        [Fact]
        public void ConfiguringAnAdditionalInterfaceWithANullTypeThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new AdditionalInterface(null));
        }

        public class BaseClass : IInterface
        {
            public int DoSomething(string param)
            {
                return int.Parse(param);
            }

            public int Property
            {
                get;
                set;
            }
        }

        public interface IInterface
        {
            int DoSomething(string param);
            int Property { get; set; }
        }

        public interface IOtherInterface
        {
            int DoSomethingElse(string param);
        }
    }
}
