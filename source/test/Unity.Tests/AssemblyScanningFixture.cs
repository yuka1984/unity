// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.TestSupport;
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

namespace Microsoft.Practices.Unity.Tests
{
    public partial class AssemblyScanningFixture
    {
        [Fact]
        public void DoesNotRegisterTypeWithNoLifetimeOrInjectionMembers()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new[] { typeof(MockLogger) }, getName: t => "name");

            Assert.False(container.Registrations.Any(r => r.MappedToType == typeof(MockLogger)));
        }

        [Fact]
        public void RegisteringNullAssembliesListThrows()
        {
            AssertExtensions.AssertException<ArgumentNullException>(() =>
            {
                AllClasses.FromAssemblies(null, true);
            });
        }

        [Fact]
        public void RegisteringNullAssembliesArrayThrows()
        {
            AssertExtensions.AssertException<ArgumentNullException>(() =>
            {
                AllClasses.FromAssemblies(true, (Assembly[])null);
            });
        }

        [Fact]
        public void RegisteringNullAssemblyThrows()
        {
            AssertExtensions.AssertException<ArgumentException>(() =>
            {
                AllClasses.FromAssemblies(true, typeof(object).GetTypeInfo().Assembly, (Assembly)null);
            });
        }

        [Fact]
        public void RegistersTypeWithLifetime()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new[] { typeof(MockLogger) }, getName: t => "name", getLifetimeManager: t => new ContainerControlledLifetimeManager());

            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger)).ToArray();

            Assert.Equal(1, registrations.Length);
            Assert.Same(typeof(MockLogger), registrations[0].MappedToType);
            Assert.Equal("name", registrations[0].Name);
            Assert.IsType<ContainerControlledLifetimeManager>(registrations[0].LifetimeManager);
        }

        [Fact]
        public void RegistersTypeWithInjectionMembers()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new[] { typeof(MockLogger) }, getName: t => "name", getInjectionMembers: t => new InjectionMember[] { new InjectionConstructor() });

            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger)).ToArray();

            Assert.Equal(1, registrations.Length);
            Assert.Same(typeof(MockLogger), registrations[0].RegisteredType);
            Assert.Same(typeof(MockLogger), registrations[0].MappedToType);
            Assert.Equal("name", registrations[0].Name);
            Assert.Null(registrations[0].LifetimeManager);
        }

        [Fact]
        public void RegistersMappingOnlyWithNoLifetimeOrInjectionMembers()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new[] { typeof(MockLogger) }, getName: t => "name", getFromTypes: t => t.GetTypeInfo().ImplementedInterfaces);

            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger)).ToArray();

            Assert.Equal(1, registrations.Length);
            Assert.Same(typeof(ILogger), registrations[0].RegisteredType);
            Assert.Same(typeof(MockLogger), registrations[0].MappedToType);
            Assert.Equal("name", registrations[0].Name);
            Assert.Null(registrations[0].LifetimeManager);
        }

        [Fact]
        public void RegistersMappingAndImplementationTypeWithLifetime()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new[] { typeof(MockLogger) }, getName: t => "name", getFromTypes: t => t.GetTypeInfo().ImplementedInterfaces, getLifetimeManager: t => new ContainerControlledLifetimeManager());

            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger)).ToArray();

            Assert.Equal(2, registrations.Length);

            var mappingRegistration = registrations.Single(r => r.RegisteredType == typeof(ILogger));
            var implementationRegistration = registrations.Single(r => r.RegisteredType == typeof(MockLogger));

            Assert.Same(typeof(ILogger), mappingRegistration.RegisteredType);
            Assert.Same(typeof(MockLogger), mappingRegistration.MappedToType);
            Assert.Equal("name", mappingRegistration.Name);
            Assert.IsType<ContainerControlledLifetimeManager>(mappingRegistration.LifetimeManager);

            Assert.Same(typeof(MockLogger), implementationRegistration.RegisteredType);
            Assert.Same(typeof(MockLogger), implementationRegistration.MappedToType);
            Assert.Equal("name", implementationRegistration.Name);
            Assert.IsType<ContainerControlledLifetimeManager>(implementationRegistration.LifetimeManager);
        }

        [Fact]
        public void GetsTypesFromAssembliesWithErrorsIfSkippingErrors()
        {
            var types = AllClasses.FromAssemblies(true, typeof(Microsoft.Practices.Unity.Tests.TestNetAssembly.PublicClass1).GetTypeInfo().Assembly).ToArray();

            Assert.Equal(2, types.Length);
            types.AssertContainsInAnyOrder(
                typeof(Microsoft.Practices.Unity.Tests.TestNetAssembly.PublicClass1),
                typeof(Microsoft.Practices.Unity.Tests.TestNetAssembly.DisposableClass));
        }

        [Fact]
        public void GettingTypesFromAssembliesWithErrorsThrowsIfNotIfNotSkippingErrors()
        {
            try
            {
                AllClasses.FromAssemblies(false, typeof(Microsoft.Practices.Unity.Tests.TestNetAssembly.PublicClass1).GetTypeInfo().Assembly).ToArray();
                Assert.True(false, string.Format("should have failed"));
            }
            catch (Exception e)
            {
                if (e is TestSupport.AssertFailedException)
                {
                    throw;
                }
            }
        }
    }
}
