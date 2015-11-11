// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
     
    public partial class RegistrationByConventionFixture
    {
        [Fact]
        public void DoesNotRegisterTypeWithNoLifetimeOrInjectionMembers()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new[] { typeof(MockLogger) }, getName: t => "name");

            Assert.False(container.Registrations.Any(r => r.MappedToType == typeof(MockLogger)));
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
        public void RegistersMappingAndImplementationTypeWithLifetimeAndMixedInjectionMembers()
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
        public void RegistersUsingTheHelperMethods()
        {
            var container = new UnityContainer();
            container.RegisterTypes(AllClasses.FromAssemblies(typeof(MockLogger).GetTypeInfo().Assembly).Where(t => t == typeof(MockLogger)), WithMappings.FromAllInterfaces, WithName.Default, WithLifetime.ContainerControlled);
            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger)).ToArray();

            Assert.Equal(2, registrations.Length);

            var mappingRegistration = registrations.Single(r => r.RegisteredType == typeof(ILogger));
            var implementationRegistration = registrations.Single(r => r.RegisteredType == typeof(MockLogger));

            Assert.Same(typeof(ILogger), mappingRegistration.RegisteredType);
            Assert.Same(typeof(MockLogger), mappingRegistration.MappedToType);
            Assert.Equal(null, mappingRegistration.Name);
            Assert.IsType<ContainerControlledLifetimeManager>(mappingRegistration.LifetimeManager);

            Assert.Same(typeof(MockLogger), implementationRegistration.RegisteredType);
            Assert.Same(typeof(MockLogger), implementationRegistration.MappedToType);
            Assert.Equal(null, implementationRegistration.Name);
            Assert.IsType<ContainerControlledLifetimeManager>(implementationRegistration.LifetimeManager);
        }

#if !NETFX_CORE

        [Fact]
        public void RegistersAllTypesWithHelperMethods()
        {
            var container = new UnityContainer();
            container.RegisterTypes(AllClasses.FromLoadedAssemblies(), WithMappings.FromAllInterfaces, WithName.TypeName, WithLifetime.ContainerControlled, overwriteExistingMappings: true);
            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger)).ToArray();

            Assert.Equal(2, registrations.Length);

            var mappingRegistration = registrations.Single(r => r.RegisteredType == typeof(ILogger));
            var implementationRegistration = registrations.Single(r => r.RegisteredType == typeof(MockLogger));

            Assert.Same(typeof(ILogger), mappingRegistration.RegisteredType);
            Assert.Same(typeof(MockLogger), mappingRegistration.MappedToType);
            Assert.Equal("MockLogger", mappingRegistration.Name);
            Assert.IsType<ContainerControlledLifetimeManager>(mappingRegistration.LifetimeManager);

            Assert.Same(typeof(MockLogger), implementationRegistration.RegisteredType);
            Assert.Same(typeof(MockLogger), implementationRegistration.MappedToType);
            Assert.Equal("MockLogger", implementationRegistration.Name);
            Assert.IsType<ContainerControlledLifetimeManager>(implementationRegistration.LifetimeManager);
        }
#endif

        public void CanResolveTypeRegisteredWithAllInterfaces()
        {
            var container = new UnityContainer();
            container.RegisterTypes(AllClasses.FromAssemblies(typeof(MockLogger).GetTypeInfo().Assembly).Where(t => t == typeof(MockLogger)), WithMappings.FromAllInterfaces, WithName.Default, WithLifetime.ContainerControlled);

            var logger1 = container.Resolve<ILogger>();
            var logger2 = container.Resolve<ILogger>();

            Assert.IsType<MockLogger>(logger1);
            Assert.Same(logger1, logger2);
        }

        public void CanResolveGenericTypeMappedWithMatchingInterface()
        {
            var container = new UnityContainer();
            container.RegisterTypes(AllClasses.FromAssemblies(typeof(IList<>).GetTypeInfo().Assembly), WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.None);

            var list = container.Resolve<IList<int>>();

            Assert.IsType<List<int>>(list);
        }

        [Fact]
        public void RegistersTypeAccordingToConvention()
        {
            var container = new UnityContainer();
            container.RegisterTypes(new TestConventionWithAllInterfaces());

            var registrations = container.Registrations.Where(r => r.MappedToType == typeof(MockLogger) || r.MappedToType == typeof(SpecialLogger)).ToArray();

            Assert.Equal(4, registrations.Length);
        }

        [Fact]
        public void OverridingExistingMappingWithDifferentMappingThrowsByDefault()
        {
            var container = new UnityContainer();
            container.RegisterType<object, string>();

            AssertExtensions.AssertException<DuplicateTypeMappingException>(
                () => container.RegisterTypes(new[] { typeof(int) }, t => new[] { typeof(object) }));
        }

        [Fact]
        public void OverridingNewMappingWithDifferentMappingThrowsByDefault()
        {
            var container = new UnityContainer();

            AssertExtensions.AssertException<DuplicateTypeMappingException>(
                () => container.RegisterTypes(new[] { typeof(string), typeof(int) }, t => new[] { typeof(object) }));
        }

        [Fact]
        public void OverridingExistingMappingWithSameMappingDoesNotThrow()
        {
            var container = new UnityContainer();
            container.RegisterInstance("a string");
            container.RegisterType<object, string>();

            container.RegisterTypes(new[] { typeof(string) }, t => new[] { typeof(object) });

            Assert.Equal("a string", container.Resolve<object>());
        }

        [Fact]
        public void CanOverrideExistingMappingWithMappingForDifferentName()
        {
            var container = new UnityContainer();
            container.RegisterType<object, string>("string");
            container.RegisterInstance("string", "a string");
            container.RegisterInstance("int", 42);

            container.RegisterTypes(new[] { typeof(int) }, t => new[] { typeof(object) }, t => "int");

            Assert.Equal("a string", container.Resolve<object>("string"));
            Assert.Equal(42, container.Resolve<object>("int"));
        }

        [Fact]
        public void OverridingExistingMappingWithDifferentMappingReplacesMappingIfAllowed()
        {
            var container = new UnityContainer();
            container.RegisterType<object, string>();
            container.RegisterInstance("a string");
            container.RegisterInstance(42);

            container.RegisterTypes(new[] { typeof(int) }, t => new[] { typeof(object) }, overwriteExistingMappings: true);

            Assert.Equal(42, container.Resolve<object>());
        }

        [Fact]
        public void OverridingNewMappingWithDifferentMappingReplacesMappingIfAllowed()
        {
            var container = new UnityContainer();
            container.RegisterInstance("a string");
            container.RegisterInstance(42);

            container.RegisterTypes(new[] { typeof(string), typeof(int) }, t => new[] { typeof(object) }, overwriteExistingMappings: true);

            Assert.Equal(42, container.Resolve<object>());
        }

        public class TestConventionWithAllInterfaces : RegistrationConvention
        {
            public override System.Collections.Generic.IEnumerable<System.Type> GetTypes()
            {
                yield return typeof(MockLogger);
                yield return typeof(SpecialLogger);
            }

            public override System.Func<System.Type, System.Collections.Generic.IEnumerable<System.Type>> GetFromTypes()
            {
                return t => t.GetTypeInfo().ImplementedInterfaces;
            }

            public override System.Func<System.Type, string> GetName()
            {
                return t => t.Name;
            }

            public override System.Func<System.Type, LifetimeManager> GetLifetimeManager()
            {
                return t => new ContainerControlledLifetimeManager();
            }

            public override System.Func<System.Type, System.Collections.Generic.IEnumerable<InjectionMember>> GetInjectionMembers()
            {
                return null;
            }
        }
    }
}
