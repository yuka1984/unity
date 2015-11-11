// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.TestSupport;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif __IOS__
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using TestInitializeAttribute = NUnit.Framework.TestFixtureSetUpAttribute;
#else
using Xunit;
#endif

namespace Unity.Tests
{
     
    public class RegistrationByConventionHelpersFixture
    {
        [Fact]
        public void GetsNoTypes()
        {
            WithMappings.None(typeof(TypeWithoutInterfaces)).AssertHasNoItems();
            WithMappings.None(typeof(DisposableType)).AssertHasNoItems();
            WithMappings.None(typeof(TestObject)).AssertHasNoItems();
            WithMappings.None(typeof(AnotherTestObject)).AssertHasNoItems();
        }

        [Fact]
        public void GetsAllInterfaces()
        {
            WithMappings.FromAllInterfaces(typeof(TypeWithoutInterfaces)).AssertHasNoItems();
            WithMappings.FromAllInterfaces(typeof(DisposableType)).AssertHasNoItems();
            WithMappings.FromAllInterfaces(typeof(TestObject)).AssertContainsInAnyOrder(typeof(IAnotherInterface), typeof(ITestObject), typeof(IComparable));
            WithMappings.FromAllInterfaces(typeof(AnotherTestObject)).AssertContainsInAnyOrder(typeof(IAnotherInterface), typeof(ITestObject), typeof(IComparable));

            // Generics
            WithMappings.FromAllInterfaces(typeof(GenericTestObject<,>)).AssertContainsInAnyOrder(typeof(IGenericTestObject<,>));
            WithMappings.FromAllInterfaces(typeof(GenericTestObjectAlt<,>)).AssertHasNoItems();
            WithMappings.FromAllInterfaces(typeof(GenericTestObject<>)).AssertContainsInAnyOrder(typeof(IComparable<>));
            WithMappings.FromAllInterfaces(typeof(GenericTestObject)).AssertContainsInAnyOrder(typeof(IGenericTestObject<string, int>), typeof(IComparable<int>), typeof(IEnumerable<IList<string>>), typeof(IEnumerable));
        }

        [Fact]
        public void GetsMatchingInterface()
        {
            WithMappings.FromMatchingInterface(typeof(TypeWithoutInterfaces)).AssertHasNoItems();
            WithMappings.FromMatchingInterface(typeof(DisposableType)).AssertHasNoItems();
            WithMappings.FromMatchingInterface(typeof(TestObject)).AssertContainsInAnyOrder(typeof(ITestObject));
            WithMappings.FromMatchingInterface(typeof(AnotherTestObject)).AssertHasNoItems();

            // Generics
            WithMappings.FromMatchingInterface(typeof(GenericTestObject<,>)).AssertContainsExactly(typeof(IGenericTestObject<,>));
            WithMappings.FromMatchingInterface(typeof(GenericTestObjectAlt<,>)).AssertHasNoItems();
            WithMappings.FromMatchingInterface(typeof(GenericTestObject<>)).AssertHasNoItems();
            WithMappings.FromMatchingInterface(typeof(GenericTestObject)).AssertHasNoItems();
        }

        [Fact]
        public void GetsAllInterfacesInSameAssembly()
        {
            WithMappings.FromAllInterfacesInSameAssembly(typeof(TypeWithoutInterfaces)).AssertHasNoItems();
            WithMappings.FromAllInterfacesInSameAssembly(typeof(DisposableType)).AssertHasNoItems();
            WithMappings.FromAllInterfacesInSameAssembly(typeof(TestObject)).AssertContainsInAnyOrder(typeof(ITestObject), typeof(IAnotherInterface));
            WithMappings.FromAllInterfacesInSameAssembly(typeof(AnotherTestObject)).AssertContainsInAnyOrder(typeof(ITestObject), typeof(IAnotherInterface));

            // Generics
            WithMappings.FromAllInterfacesInSameAssembly(typeof(GenericTestObject<,>)).AssertContainsExactly(typeof(IGenericTestObject<,>));
            WithMappings.FromAllInterfacesInSameAssembly(typeof(GenericTestObjectAlt<,>)).AssertHasNoItems();
            WithMappings.FromAllInterfacesInSameAssembly(typeof(GenericTestObject<>)).AssertHasNoItems();
            WithMappings.FromAllInterfacesInSameAssembly(typeof(GenericTestObject)).AssertContainsExactly(typeof(IGenericTestObject<string, int>));
        }

        [Fact]
        public void GetsNames()
        {
            Assert.Equal("MockLogger", WithName.TypeName(typeof(MockLogger)));
            Assert.Equal("List`1", WithName.TypeName(typeof(List<>)));
            Assert.Null(WithName.Default(typeof(MockLogger)));
            Assert.Null(WithName.Default(typeof(List<>)));
        }

        [Fact]
        public void GetsLifetimeManagers()
        {
            Assert.IsType< ContainerControlledLifetimeManager>(WithLifetime.ContainerControlled(typeof(MockLogger)));
            Assert.IsType< ExternallyControlledLifetimeManager>(WithLifetime.ExternallyControlled(typeof(MockLogger)));
            Assert.IsType< HierarchicalLifetimeManager>(WithLifetime.Hierarchical(typeof(MockLogger)));
            Assert.Null(WithLifetime.None(typeof(MockLogger)));
            Assert.IsType<PerResolveLifetimeManager>(WithLifetime.PerResolve(typeof(MockLogger)));
            Assert.IsType<TransientLifetimeManager>(WithLifetime.Transient(typeof(MockLogger)));
            Assert.IsType<CustomLifetimeManager>(WithLifetime.Custom<CustomLifetimeManager>(typeof(MockLogger)));

#if !NETFX_CORE
            Assert.IsType< PerThreadLifetimeManager>(WithLifetime.PerThread(typeof(MockLogger)));
#endif
        }

        public class CustomLifetimeManager : LifetimeManager
        {
            public override object GetValue()
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object newValue)
            {
                throw new NotImplementedException();
            }

            public override void RemoveValue()
            {
                throw new NotImplementedException();
            }
        }

        public class TypeWithoutInterfaces { }

        public class DisposableType : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        public class TestObject : IAnotherInterface, ITestObject, IDisposable, IComparable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }
        }

        public class AnotherTestObject : IAnotherInterface, ITestObject, IDisposable, IComparable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }
        }

        public interface ITestObject { }

        public interface IAnotherInterface { }

        public interface IGenericTestObject<T, U> { }

        public class GenericTestObject<T, U> : IGenericTestObject<T, U>, IComparable<T>, IEnumerable<IList<T>>
        {
            public int CompareTo(T other)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<IList<T>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        public class GenericTestObjectAlt<T, U> : IGenericTestObject<U, T>
        {
        }

        public class GenericTestObject<T> : IGenericTestObject<T, int>, IComparable, IEnumerable<IList<int>>, IComparable<T>
        {
            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }

            public int CompareTo(T other)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<IList<int>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        public class GenericTestObject : IGenericTestObject<string, int>, IComparable<int>, IEnumerable<IList<string>>
        {
            public int CompareTo(int other)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<IList<string>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
