// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
     
    public class InterceptFixture
    {
        [Fact]
        public void CanInterceptTargetWithInstanceInterceptor()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior((mi, next) => { invoked = true; return mi.CreateMethodReturn(100); });

            IInterface proxy = (IInterface)
                Intercept.ThroughProxyWithAdditionalInterfaces(
                    typeof(IInterface),
                    new BaseClass(10),
                    new InterfaceInterceptor(),
                    new[] { interceptionBehavior },
                    Type.EmptyTypes);

            int value = proxy.DoSomething();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        [Fact]
        public void GeneratedProxyImplementsUserProvidedAdditionalInterfaces()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior((mi, next) => { invoked = true; return mi.CreateMethodReturn(100); });

            IInterface proxy = (IInterface)
                Intercept.ThroughProxyWithAdditionalInterfaces(
                    typeof(IInterface),
                    new BaseClass(10),
                    new InterfaceInterceptor(),
                    new[] { interceptionBehavior },
                    new[] { typeof(ISomeInterface) });

            int value = ((ISomeInterface)proxy).DoSomethingElse();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        [Fact]
        public void GeneratedProxyImplementsInterceptionBehaviorProvidedAdditionalInterfaces()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior(
                    (mi, next) => { invoked = true; return mi.CreateMethodReturn(100); },
                    () => new[] { typeof(ISomeInterface) });

            IInterface proxy = (IInterface)
                Intercept.ThroughProxyWithAdditionalInterfaces(
                    typeof(IInterface),
                    new BaseClass(10),
                    new InterfaceInterceptor(),
                    new[] { interceptionBehavior },
                    Type.EmptyTypes);

            int value = ((ISomeInterface)proxy).DoSomethingElse();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        [Fact]
        public void CanInterceptTargetWithInstanceInterceptorUsingGenericVersion()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior((mi, next) => { invoked = true; return mi.CreateMethodReturn(100); });

            IInterface proxy =
                Intercept.ThroughProxyWithAdditionalInterfaces<IInterface>(
                    new BaseClass(10),
                    new InterfaceInterceptor(),
                    new[] { interceptionBehavior },
                    Type.EmptyTypes);

            int value = proxy.DoSomething();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        [Fact]
        public void InterceptingANullTypeThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
               null,
               new BaseClass(),
               new InterfaceInterceptor(),
               new IInterceptionBehavior[0],
               Type.EmptyTypes));
        }

        [Fact]
        public void InterceptingANullTargetThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
                typeof(IInterface),
                null,
                new InterfaceInterceptor(),
                new IInterceptionBehavior[0],
                Type.EmptyTypes));
        }

        [Fact]
        public void InterceptingWithANullInterceptorThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
                typeof(IInterface),
                new BaseClass(),
                null,
                new IInterceptionBehavior[0],
                Type.EmptyTypes));
        }

        [Fact]
        public void InterceptingTypesNotCompatibleWithTheInterceptorThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
                typeof(IInterface),
                new BaseClass(),
                new RejectingInterceptor(),
                new IInterceptionBehavior[0],
                Type.EmptyTypes));
        }

        [Fact]
        public void InterceptingWithANullSetOfInterceptionBehaviorsThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
                typeof(IInterface),
                new BaseClass(),
                new InterfaceInterceptor(),
                null,
                Type.EmptyTypes));
        }

        [Fact]
        public void InterceptingWithANullSetOfAdditionalInterfacesThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
                typeof(IInterface),
                new BaseClass(),
                new InterfaceInterceptor(),
                new IInterceptionBehavior[0],
                null));
        }

        [Fact]
        public void InterceptingWithASetOfAdditionalInterfacesIncludingNonInterfaceTypeThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.ThroughProxyWithAdditionalInterfaces(
                typeof(IInterface),
                new BaseClass(),
                new InterfaceInterceptor(),
                new IInterceptionBehavior[0],
                new[] { typeof(object) }));
        }

        [Fact]
        public void CanInterceptNewInstanceWithTypeInterceptor()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior((mi, next) => { invoked = true; return mi.CreateMethodReturn(100); });

            BaseClass instance = (BaseClass)
                Intercept.NewInstanceWithAdditionalInterfaces(
                    typeof(BaseClass),
                    new VirtualMethodInterceptor(),
                    new[] { interceptionBehavior },
                    Type.EmptyTypes,
                    10);

            int value = instance.DoSomething();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        [Fact]
        public void CanInterceptNewInstanceWithTypeInterceptorUsingGenericVersion()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior((mi, next) => { invoked = true; return mi.CreateMethodReturn(100); });

            BaseClass instance =
                Intercept.NewInstanceWithAdditionalInterfaces<BaseClass>(
                    new VirtualMethodInterceptor(),
                    new[] { interceptionBehavior },
                    Type.EmptyTypes,
                    10);

            int value = instance.DoSomething();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        [Fact]
        public void InterceptingNewInstanceOfANullTypeThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.NewInstance(
                null,
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[0]));
        }

        [Fact]
        public void InterceptingNewInstanceWithANullInterceptorThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                null,
                new IInterceptionBehavior[0]));
        }

        [Fact]
        public void InterceptingNewInstanceOfTypeNotCompatibleWithTheInterceptorThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new RejectingInterceptor(),
                new IInterceptionBehavior[0]));
        }

        [Fact]
        public void InterceptingNewInstanceWithANullSetOfInterceptionBehaviorsThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                null));
        }

        [Fact]
        public void InterceptingNewInstanceWithANullSetOfAdditionalInterfacesThrows()
        {
            Assert.Throws<ArgumentNullException>(() => Intercept.NewInstanceWithAdditionalInterfaces(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[0],
                null));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfAdditionalInterfacesWithNullElementsThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstanceWithAdditionalInterfaces(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[0],
                new Type[] { null }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfAdditionalInterfacesWithNonInterfaceElementsThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstanceWithAdditionalInterfaces(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[0],
                new Type[] { typeof(object) }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfAdditionalInterfacesWithGenericInterfaceElementsThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstanceWithAdditionalInterfaces(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[0],
                new Type[] { typeof(IComparable<>) }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfBehaviorsWithNullElementsThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[] { null }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfBehaviorsWithElementReturningNullRequiredInterfacesThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[] 
                {
                    new DelegateInterceptionBehavior(null, () => null)
                }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfBehaviorsWithElementReturningRequiredInterfacesWithNullElementThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[] 
                {
                    new DelegateInterceptionBehavior(null, () => new Type[] { null })
                }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfBehaviorsWithElementReturningRequiredInterfacesWithNonInterfaceElementThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[] 
                {
                    new DelegateInterceptionBehavior(null, () => new Type[] { typeof(object) })
                }));
        }

        [Fact]
        public void InterceptingNewInstanceWithASetOfBehaviorsWithElementReturningRequiredInterfacesWithGenericInterfaceElementThrows()
        {
            Assert.Throws<ArgumentException>(() => Intercept.NewInstance(
                typeof(BaseClass),
                new VirtualMethodInterceptor(),
                new IInterceptionBehavior[] 
                {
                    new DelegateInterceptionBehavior(null, () => new Type[] { typeof(IEnumerable<>) })
                }));
        }

        [Fact]
        public void CanInterceptAbstractClassWithVirtualMethodInterceptor()
        {
            bool invoked = false;
            IInterceptionBehavior interceptionBehavior =
                new DelegateInterceptionBehavior((mi, next) => { invoked = true; return mi.CreateMethodReturn(100); });

            AbstractClass instance =
                Intercept.NewInstance<AbstractClass>(
                    new VirtualMethodInterceptor(),
                    new[] { interceptionBehavior });

            int value = instance.DoSomething();

            Assert.Equal(100, value);
            Assert.True(invoked);
        }

        public class BaseClass : IInterface
        {
            private readonly int value;

            public BaseClass()
                : this(0)
            { }

            public BaseClass(int value)
            {
                this.value = value;
            }

            public virtual int DoSomething()
            {
                return value;
            }
        }

        public interface IInterface
        {
            int DoSomething();
        }

        public interface ISomeInterface
        {
            int DoSomethingElse();
        }

        public class RejectingInterceptor : IInstanceInterceptor, ITypeInterceptor
        {
            public bool CanIntercept(Type t)
            {
                return false;
            }

            public IEnumerable<MethodImplementationInfo> GetInterceptableMethods(Type interceptedType, Type implementationType)
            {
                throw new NotImplementedException();
            }

            public IInterceptingProxy CreateProxy(Type t, object target, params Type[] additionalInterfaces)
            {
                throw new NotImplementedException();
            }

            public Type CreateProxyType(Type t, params Type[] additionalInterfaces)
            {
                throw new NotImplementedException();
            }
        }

        public abstract class AbstractClass
        {
            public abstract int DoSomething();
        }
    }
}
