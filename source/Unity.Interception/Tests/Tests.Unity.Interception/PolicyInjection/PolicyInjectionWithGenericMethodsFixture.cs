// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.TestSupport;
using System;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.PolicyInjection
{
    /// <summary>
    /// Summary description for PolicyInjectionWithGenericMethodsFixture
    /// </summary>
     
    public partial class PolicyInjectionWithGenericMethodsFixture: IDisposable
    {
        [Fact]
        public void InterfaceInterceptorCanInterceptNonGenericMethod()
        {
            CanInterceptNonGenericMethod<InterfaceInterceptor>();
        }

        [Fact]
        public void InterfaceInterceptorCanInterceptGenericMethod()
        {
            CanInterceptGenericMethod<InterfaceInterceptor>();
        }

        [Fact]
        public void VirtualMethodCanInterceptNonGenericMethod()
        {
            CanInterceptNonGenericMethod<VirtualMethodInterceptor>();
        }

        [Fact]
        public void VirtualMethodCanInterceptGenericMethod()
        {
            CanInterceptGenericMethod<VirtualMethodInterceptor>();
        }

        private static IUnityContainer CreateContainer<TInterceptor>()
            where TInterceptor : IInterceptor
        {
            return new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterType<IInterfaceWithGenericMethod, MyClass>(
                    new Interceptor<TInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());
        }

        private static void CanInterceptNonGenericMethod<TInterceptor>()
            where TInterceptor : IInterceptor
        {
            var container = CreateContainer<TInterceptor>();

            var instance = container.Resolve<IInterfaceWithGenericMethod>();

            instance.DoSomethingElse("boo");

            Assert.Equal(1, GlobalCountCallHandler.Calls["NonGeneric"]);
        }

        private static void CanInterceptGenericMethod<TInterceptor>()
            where TInterceptor : IInterceptor
        {
            var container = CreateContainer<TInterceptor>();

            var instance = container.Resolve<IInterfaceWithGenericMethod>();

            instance.DoSomething<string>();
            instance.DoSomething<int>();

            Assert.Equal(2, GlobalCountCallHandler.Calls["Generic"]);
        }

        public void Dispose()
        {
            GlobalCountCallHandler.Calls.Clear();
        }
    }

    public interface IInterfaceWithGenericMethod
    {
        [GlobalCountCallHandler(HandlerName = "Generic")]
        T DoSomething<T>();

        [GlobalCountCallHandler(HandlerName = "NonGeneric")]
        void DoSomethingElse(string param);
    }

    public class MyClass : IInterfaceWithGenericMethod
    {
        public virtual T DoSomething<T>()
        {
            return default(T);
        }

        public virtual void DoSomethingElse(string param)
        {
        }
    }
}
