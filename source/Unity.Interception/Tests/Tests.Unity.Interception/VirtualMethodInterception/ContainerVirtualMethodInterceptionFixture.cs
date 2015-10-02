// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.VirtualMethodInterception
{
    /// <summary>
    /// Tests for the virtual method interception mechanism as invoked
    /// through the container.
    /// </summary>
     
    public class ContainerVirtualMethodInterceptionFixture
    {
        [Fact]
        public void InterceptedClassGetsReturned()
        {
            CallCountHandler h1 = new CallCountHandler();
            CallCountHandler h2 = new CallCountHandler();

            IUnityContainer container = GetConfiguredContainer(h1, h2);
            AddPoliciesToContainer(container);
            ConfigureInterceptionWithRegisterType(container);

            Interceptee target = container.Resolve<Interceptee>();

            Assert.NotSame(typeof(Interceptee), target.GetType());
        }

        [Fact]
        public void AttachedHandlersAreCalled()
        {
            CallCountHandler h1 = new CallCountHandler();
            CallCountHandler h2 = new CallCountHandler();

            IUnityContainer container = new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterInstance<ICallHandler>("h1", h1)
                .RegisterInstance<ICallHandler>("h2", h2)
                .RegisterType<Interceptee>(
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>())
                .Configure<Interception>()
                    .AddPolicy("methodOne")
                        .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor("MethodOne"))
                        .AddCallHandler("h1")
                .Interception
                    .AddPolicy("methodTwo")
                        .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor("MethodTwo"))
                        .AddCallHandler("h2")
                .Interception.Container;

            Interceptee target = container.Resolve<Interceptee>();

            int oneCount = 0;
            int twoCount = 0;

            for (oneCount = 0; oneCount < 2; ++oneCount)
            {
                target.MethodOne();
            }

            for (twoCount = 0; twoCount < 3; ++twoCount)
            {
                target.MethodTwo("hi", twoCount);
            }

            Assert.Equal(oneCount, h1.CallCount);
            Assert.Equal(twoCount, h2.CallCount);
        }

        [Fact]
        public void RegisteringInterceptionOnOpenGenericsLetsYouResolveMultipleClosedClasses()
        {
            IUnityContainer container = new UnityContainer()
                .AddNewExtension<Interception>();

            AddPoliciesToContainer(container);

            container.Configure<Interception>()
                .SetDefaultInterceptorFor(typeof(GenericFactory<>), new VirtualMethodInterceptor());

            GenericFactory<SubjectOne> resultOne = container.Resolve<GenericFactory<SubjectOne>>();
            GenericFactory<SubjectTwo> resultTwo = container.Resolve<GenericFactory<SubjectTwo>>();

            Assert.True(resultOne is IInterceptingProxy);
            Assert.True(resultTwo is IInterceptingProxy);

            Assert.Equal("**Hi**", resultTwo.MakeAT().GetAValue("Hi"));
        }

        [Fact]
        public virtual void TestNewVirtualOverride()
        {
            IUnityContainer container = GetContainer();

            NewVirtualOverrideTestClass testClass = container.Resolve<NewVirtualOverrideTestClass>();

            Assert.True(testClass.TestMethod1(), "override");
            Assert.True(testClass.TestMethod2(), "new virtual");
            Assert.True(testClass.TestMethod3(), "always true");
            Assert.True(testClass.TestMethod4(), "abstract");

            Assert.Equal(4, container.Resolve<CallCountHandler>("TestCallHandler").CallCount);
        }

        [Fact]
        public void CanInterceptWithInterceptorSetAsDefaultForBaseClassWithMultipleImplementations()
        {
            IUnityContainer container =
                new UnityContainer()
                    .RegisterType<BaseClass, ImplementationOne>("one")
                    .RegisterType<BaseClass, ImplementationTwo>("two")
                    .AddNewExtension<Interception>()
                    .Configure<Interception>()
                        .SetDefaultInterceptorFor<BaseClass>(new VirtualMethodInterceptor())
                    .Container;

            BaseClass instanceOne = container.Resolve<BaseClass>("one");
            BaseClass instanceTwo = container.Resolve<BaseClass>("two");

            Assert.Equal("ImplementationOne", instanceOne.Method());
            Assert.Equal("ImplementationTwo", instanceTwo.Method());
        }

        [Fact]
        public void CanAddInterceptionBehaviorsWithRequiredInterfaces()
        {
            IUnityContainer container =
                new UnityContainer()
                    .AddNewExtension<Interception>()
                    .RegisterType<ClassWithVirtualProperty>(
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior(new NaiveINotifyPropertyChangedInterceptionBehavior()));

            ClassWithVirtualProperty instance = container.Resolve<ClassWithVirtualProperty>();

            string changedProperty = null;
            ((INotifyPropertyChanged)instance).PropertyChanged += (s, a) => changedProperty = a.PropertyName;

            instance.Property = 10;

            Assert.Equal("Property", changedProperty);
        }

        [Fact]
        public void ResolvingKeyForTheSecondTimeAfterAddingBehaviorWithRequiredInterfaceReflectsLastConfiguration()
        {
            IUnityContainer container =
                new UnityContainer()
                    .AddNewExtension<Interception>()
                    .RegisterType<ClassWithVirtualProperty>(new Interceptor<VirtualMethodInterceptor>());

            Assert.False(container.Resolve<ClassWithVirtualProperty>() is INotifyPropertyChanged);

            container
                .RegisterType<ClassWithVirtualProperty>(
                    new InterceptionBehavior(new NaiveINotifyPropertyChangedInterceptionBehavior()));

            Assert.True(container.Resolve<ClassWithVirtualProperty>() is INotifyPropertyChanged);
        }

        [Fact]
        public void GeneratedDerivedTypeIsCached()
        {
            IUnityContainer container =
                new UnityContainer()
                    .AddNewExtension<Interception>()
                    .RegisterType<ClassWithVirtualProperty>(new Interceptor<VirtualMethodInterceptor>());

            ClassWithVirtualProperty instanceOne = container.Resolve<ClassWithVirtualProperty>();
            ClassWithVirtualProperty instanceTwo = container.Resolve<ClassWithVirtualProperty>();

            Assert.Same(typeof(ClassWithVirtualProperty), instanceOne.GetType().BaseType);
            Assert.Same(instanceOne.GetType(), instanceTwo.GetType());
        }

        [Fact]
        public void CanInterceptClassWithSingleNonDefaultConstructor()
        {
            CallCountInterceptionBehavior callCountBehavior = new CallCountInterceptionBehavior();

            IUnityContainer container =
                new UnityContainer()
                    .AddNewExtension<Interception>()
                    .RegisterType<ClassWithSingleNonDefaultConstructor>(
                        new InjectionConstructor("some value"),
                        new Interceptor<VirtualMethodInterceptor>(),
                        new InterceptionBehavior(callCountBehavior));

            ClassWithSingleNonDefaultConstructor instance = container.Resolve<ClassWithSingleNonDefaultConstructor>();

            string value = instance.GetValue();

            Assert.Equal("some value", value);
            Assert.Equal(1, callCountBehavior.CallCount);
        }

        [Fact]
        public void CanInterceptGenericTypeWithGenericMethodsWithConstraints()
        {
            var container = new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterType(typeof(GenericTypeWithGenericMethodsAndConstraints<>),
                    new Interceptor<VirtualMethodInterceptor>());

            var result = container.Resolve<GenericTypeWithGenericMethodsAndConstraints<ClassWithVirtualProperty>>();

            Type interceptedType = result.GetType();
            Type genericInterceptedType = interceptedType.GetGenericTypeDefinition();
            Assert.False(interceptedType.ContainsGenericParameters);
            Assert.True(interceptedType.IsGenericType);
            Assert.False(interceptedType.IsGenericTypeDefinition);
        }

        protected virtual IUnityContainer GetContainer()
        {
            IUnityContainer container = new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterType<IMatchingRule, AlwaysMatchingRule>("AlwaysMatchingRule")
                .RegisterType<ICallHandler, CallCountHandler>("TestCallHandler", new ContainerControlledLifetimeManager());

            container.Configure<Interception>()
                .SetDefaultInterceptorFor<NewVirtualOverrideTestClass>(new VirtualMethodInterceptor())
                .AddPolicy("Rules")
                .AddMatchingRule("AlwaysMatchingRule")
                .AddCallHandler("TestCallHandler");

            return container;
        }

        private IUnityContainer GetConfiguredContainer(ICallHandler h1, ICallHandler h2)
        {
            IUnityContainer container = new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterInstance<ICallHandler>("h1", h1)
                .RegisterInstance<ICallHandler>("h2", h2);

            return container;
        }

        private IUnityContainer AddPoliciesToContainer(IUnityContainer container)
        {
            container.Configure<Interception>()
                .AddPolicy("MethodOne")
                .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor("MethodOne"))
                .AddCallHandler("h1")
                .Interception
                .AddPolicy("MethodTwo")
                .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor("MethodTwo"))
                .AddCallHandler("h2");
            return container;
        }

        private IUnityContainer ConfigureInterceptionWithRegisterType(IUnityContainer container)
        {
            container.RegisterType<Interceptee>(
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());
            return container;
        }
    }

    public class Interceptee
    {
        public virtual int MethodOne()
        {
            return 37;
        }

        public virtual string MethodTwo(string s, int i)
        {
            return s + ";" + i.ToString();
        }
    }

    public class GenericFactory<T> where T : class, new()
    {
        public T MakeAT()
        {
            return new T();
        }
    }

    public class SubjectOne
    {
    }

    public class SubjectTwo
    {
        public string GetAValue(string s)
        {
            return "**" + s + "**";
        }
    }

    public class NewVirtualOverrideTestClass : NewVirtualOverrideTestClassBase
    {
        public override bool TestMethod1()
        {
            return true;
        }

        public new virtual bool TestMethod2()
        {
            return true;
        }

        public override bool TestMethod4()
        {
            return true;
        }

        public new virtual void TestMethod5(int bb)
        {
        }
    }

    public abstract class NewVirtualOverrideTestClassBase
    {
        public virtual bool TestMethod1()
        {
            return false;
        }

        public virtual bool TestMethod2()
        {
            return false;
        }

        public virtual bool TestMethod3()
        {
            return true;
        }

        public abstract bool TestMethod4();

        public virtual void TestMethod5(int a)
        {
        }
    }

    public class BaseClass
    {
        public virtual string Method()
        {
            return "base";
        }
    }

    public class ImplementationOne : BaseClass
    {
        public override string Method()
        {
            return "ImplementationOne";
        }
    }

    public class ImplementationTwo : BaseClass
    {
        public override string Method()
        {
            return "ImplementationTwo";
        }
    }

    public class ClassWithVirtualProperty
    {
        public virtual int Property { get; set; }
    }

    public class ClassWithSingleNonDefaultConstructor
    {
        private string value;

        public ClassWithSingleNonDefaultConstructor(string value)
        {
            this.value = value;
        }

        public virtual string GetValue()
        {
            return value;
        }
    }

    internal class ClassWithInternalConstructor
    {
        public ClassWithInternalConstructor()
        {
        }

        public virtual int Method()
        {
            return 10;
        }
    }

    public class GenericTypeWithGenericMethodsAndConstraints<T>
        where T : class
    {
        public virtual void TestMethod1<T1>()
        { }

        public virtual void TestMethod2<T2>()
            where T2 : struct
        { }

        public virtual void TestMethod3<T3>()
            where T3 : class
        { }

        public virtual void TestMethod4<T4>()
            where T4 : class, new()
        { }

        public virtual void TestMethod5<T5>()
            where T5 : InjectionPolicy
        { }

        public virtual void TestMethod6<T6>()
            where T6 : IMatchingRule
        { }
    }
}
