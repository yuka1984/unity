// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
    public partial class InterceptionFixture
    {
        [Fact]
        public void CanConfigureRemotingInterceptionOnMBRO()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container.Configure<Interception>().SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());
        }

        [Fact]
        public void CanConfigureRemotingInterceptionOnInterface()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container.Configure<Interception>().SetInterceptorFor<Interface>(new TransparentProxyInterceptor());
        }

        [Fact]
        public void ConfiguringRemotingInterceptionOnNonMBROTypeThrows()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            try
            {
                container.Configure<Interception>()
                    .SetInterceptorFor<WrappableThroughInterface>(new TransparentProxyInterceptor());
                Assert.True(false, string.Format("Call to SetInjectorFor<T>() should have thrown"));
            }
            catch (ArgumentException)
            {
                // expected exception
            }
        }

        [Fact]
        public void CanConfigureDefaultRemotingInterceptionOnMBRO()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container.Configure<Interception>()
                .SetDefaultInterceptorFor<Wrappable>(new TransparentProxyInterceptor());
        }

        [Fact]
        public void CanConfigureDefaultRemotingInterceptionOnInterface()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            container.Configure<Interception>()
                .SetDefaultInterceptorFor<Interface>(new TransparentProxyInterceptor());
        }

        [Fact]
        public void ConfiguringDefaultRemotingInterceptionOnNonMBROTypeThrows()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            try
            {
                container.Configure<Interception>()
                    .SetDefaultInterceptorFor<WrappableThroughInterface>(new TransparentProxyInterceptor());
                Assert.True(false, string.Format("Call to SetInjectorFor<T>() should have thrown"));
            }
            catch (ArgumentException)
            {
                // expected exception
            }
        }

        [Fact]
        public void CanCreateWrappedObjectIfInterceptionPolicyIsSet()
        {
            IUnityContainer container = CreateContainer("CanCreateWrappedObject");
            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Wrappable wrappable = container.Resolve<Wrappable>();
            Assert.NotNull(wrappable);
            Assert.True(RemotingServices.IsTransparentProxy(wrappable));
        }

        [Fact]
        public void CanCreateWrappedObjectIfDefaultInterceptionPolicy()
        {
            IUnityContainer container = CreateContainer("CanCreateWrappedObject");
            container.Configure<Interception>().SetDefaultInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Wrappable wrappable = container.Resolve<Wrappable>();
            Assert.NotNull(wrappable);
            Assert.True(RemotingServices.IsTransparentProxy(wrappable));
        }

        [Fact]
        public void CanCreateNamedWrappedObjectIfDefaultInterceptionPolicy()
        {
            IUnityContainer container = CreateContainer("CanCreateWrappedObject");
            container.Configure<Interception>().SetDefaultInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Wrappable wrappable = container.Resolve<Wrappable>("someName");
            var wrappable2 = container.Resolve<Wrappable>("another");
            Assert.NotNull(wrappable);
            Assert.True(RemotingServices.IsTransparentProxy(wrappable));
            Assert.True(RemotingServices.IsTransparentProxy(wrappable2));
        }

        [Fact]
        public void CanSetDefaultInterceptionPolicyThroughRegisterType()
        {
            IUnityContainer container = CreateContainer("CanCreateWrappedObject");
            container.RegisterType<Wrappable>(
                new DefaultInterceptor(new TransparentProxyInterceptor()),
                new DefaultInterceptionBehavior<PolicyInjectionBehavior>());

            var wrappable = container.Resolve<Wrappable>("someName");
            var wrappable2 = container.Resolve<Wrappable>("another");
            Assert.True(RemotingServices.IsTransparentProxy(wrappable));
            Assert.True(RemotingServices.IsTransparentProxy(wrappable2));
        }

        [Fact]
        public void WillNotCreateWrappedObjectIfNoInterceptionPolicyIsSpecified()
        {
            IUnityContainer container = CreateContainer("CanCreateWrappedObject");

            Wrappable wrappable = container.Resolve<Wrappable>();
            Assert.NotNull(wrappable);
            Assert.False(RemotingServices.IsTransparentProxy(wrappable));
        }

        [Fact]
        public void CanInterceptExistingWrappedObject()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanCreateWrappedObject")
                .RegisterType<Wrappable>(
                    new Interceptor<TransparentProxyInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());

            Wrappable wrappable = container.BuildUp<Wrappable>(new Wrappable());
            wrappable.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanCreateWrappedObject"]);
        }

        [Fact]
        public void CanInterceptCallsToDerivedOfMBRO()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanInterceptCallsToDerivedOfMBRO")
                .RegisterType<DerivedWrappable>(
                    new Interceptor<TransparentProxyInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());

            DerivedWrappable wrappable = container.Resolve<DerivedWrappable>();
            wrappable.Method2();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanInterceptCallsToDerivedOfMBRO"]);
        }

        [Fact]
        public void CanInterceptCallsToMBROOverInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanInterceptCallsToMBROOverInterface");
            container
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Wrappable wrappable = container.Resolve<Wrappable>();
            ((Interface)wrappable).Method();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanInterceptCallsToMBROOverInterface"]);
        }

        [Fact]
        public void CanInterceptCallsToMappedMBROOverInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanInterceptCallsToMappedMBROOverInterface");
            container
                .RegisterType<Interface, Wrappable>()
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Interface wrappable = container.Resolve<Interface>();
            wrappable.Method();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanInterceptCallsToMappedMBROOverInterface"]);
        }

        [Fact]
        public void CanInterceptCallsToMappedMBROOverInterfaceCastedToType()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanInterceptCallsToMappedMBROOverInterfaceCastedToType");
            container
                .RegisterType<Interface, Wrappable>()
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Interface wrappable = container.Resolve<Interface>();
            ((Wrappable)wrappable).Method();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanInterceptCallsToMappedMBROOverInterfaceCastedToType"]);
        }

        [Fact]
        public void CanInterceptCallsToLifetimeManagedMappedMBROOverInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanInterceptCallsToMappedMBROOverInterface");
            container
                .RegisterType<Interface, Wrappable>(new ContainerControlledLifetimeManager())
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Interface wrappable = container.Resolve<Interface>();
            wrappable.Method();
            Wrappable wrappable2 = container.Resolve<Wrappable>();
            wrappable2.Method();

            Assert.Equal(2, GlobalCountCallHandler.Calls["CanInterceptCallsToMappedMBROOverInterface"]);
        }

        [Fact]
        public void CanInterceptCallsToMappedMBROOverInterfaceWithDefaultConfiguredForType()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container
                = CreateContainer("CanInterceptCallsToMappedMBROOverInterfaceWithDefaultConfiguredForType");
            container
                .RegisterType<Interface, Wrappable>()
                .Configure<Interception>()
                    .SetDefaultInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Interface wrappable = container.Resolve<Interface>();
            wrappable.Method();

            Assert.Equal(
                1,
                GlobalCountCallHandler.Calls["CanInterceptCallsToMappedMBROOverInterfaceWithDefaultConfiguredForType"]);
        }

        [Fact]
        public void CanInterceptCallsToMappedMBROOverInterfaceWithDefaultConfiguredForInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container
                = CreateContainer("CanInterceptCallsToMappedMBROOverInterfaceWithDefaultConfiguredForInterface");
            container
                .RegisterType<Interface, Wrappable>()
                .Configure<Interception>()
                    .SetDefaultInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Interface wrappable = container.Resolve<Interface>();
            wrappable.Method();

            Assert.Equal(
                1,
                GlobalCountCallHandler.Calls["CanInterceptCallsToMappedMBROOverInterfaceWithDefaultConfiguredForInterface"]);
        }

        [Fact]
        public void CanInterceptCallsToMappedMBROOverDifferentInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanInterceptCallsToMappedMBROOverDifferentInterface");
            container
                .RegisterType<Interface, Wrappable>()
                .Configure<Interception>()
                    .SetInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            Interface wrappable = container.Resolve<Interface>();
            ((InterfaceA)wrappable).MethodA();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanInterceptCallsToMappedMBROOverDifferentInterface"]);
        }

        [Fact]
        public void CanConfigureInterceptionOnInterfaceToWrapMBRO()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanConfigureInterceptionOnInterfaceToWrapMBRO");
            container.RegisterType<Interface, Wrappable>();
            container
                .Configure<Interception>()
                    .SetInterceptorFor<Interface>(new TransparentProxyInterceptor());

            Interface wrapped = container.Resolve<Interface>();
            wrapped.Method3();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanConfigureInterceptionOnInterfaceToWrapMBRO"]);
        }

        [Fact]
        public void CanConfigureInterceptionOnInterfaceToWrapNonMBRO()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanConfigureInterceptionOnInterfaceToWrapNonMBRO");
            container.RegisterType<Interface, WrappableThroughInterface>();
            container
                .Configure<Interception>()
                    .SetInterceptorFor<Interface>(new TransparentProxyInterceptor());

            Interface wrapped = container.Resolve<Interface>();
            wrapped.Method3();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanConfigureInterceptionOnInterfaceToWrapNonMBRO"]);
        }

        [Fact]
        public void CanInterceptCallToMapppedNonMBROThroughDifferentInterface()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = CreateContainer("CanConfigureInterceptionOnInterfaceToWrapNonMBRO");
            container.RegisterType<Interface, WrappableThroughInterface>();
            container
                .Configure<Interception>()
                    .SetInterceptorFor<Interface>(new TransparentProxyInterceptor());

            Interface wrapped = container.Resolve<Interface>();
            ((InterfaceA)wrapped).MethodA();

            Assert.Equal(1, GlobalCountCallHandler.Calls["CanConfigureInterceptionOnInterfaceToWrapNonMBRO"]);
        }

        [Fact]
        public void CanInterceptMBROWithDependencyOnOtherMBRO()
        {
            GlobalCountCallHandler.Calls.Clear();

            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<Interception>();

            container
                .RegisterInstance<IMatchingRule>(
                    "parentRule",
                    new TypeMatchingRule(typeof(WrappableWithProperty)))
                .RegisterInstance<IMatchingRule>(
                    "childRule",
                    new TypeMatchingRule(typeof(Wrappable)))
                .RegisterInstance<ICallHandler>(
                    "parentCallHandler",
                    new GlobalCountCallHandler("parent"))
                .RegisterInstance<ICallHandler>(
                    "childCallHandler",
                    new GlobalCountCallHandler("child"))
                .RegisterType<InjectionPolicy, RuleDrivenPolicy>("parentPolicy",
                    new InjectionConstructor(
                        new ResolvedArrayParameter<IMatchingRule>(
                            new ResolvedParameter<IMatchingRule>("parentRule")),
                        new string[] { "parentCallHandler" }))
                .RegisterType<InjectionPolicy, RuleDrivenPolicy>("childPolicy",
                    new InjectionConstructor(
                        new ResolvedArrayParameter<IMatchingRule>(
                            new ResolvedParameter<IMatchingRule>("childRule")),
                        new string[] { "childCallHandler" }))
                .RegisterType<WrappableWithProperty>(new InjectionProperty("Wrappable"))
                .Configure<Interception>()
                    .SetDefaultInterceptorFor<WrappableWithProperty>(new TransparentProxyInterceptor())
                    .SetDefaultInterceptorFor<Wrappable>(new TransparentProxyInterceptor());

            WrappableWithProperty instance = container.Resolve<WrappableWithProperty>();

            instance.Method();
            Assert.Equal(1, GlobalCountCallHandler.Calls["parent"]); // method

            instance.Wrappable.Method();
            Assert.Equal(2, GlobalCountCallHandler.Calls["parent"]); // method and getter
            Assert.Equal(1, GlobalCountCallHandler.Calls["child"]);
        }

        public partial class BaseInterceptable : MarshalByRefObject
        {
        }
    }
}
