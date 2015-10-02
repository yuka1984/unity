// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerWithBasicTypeMappings
    /// </summary>
     
    public class When_ConfiguringContainerWithBasicTypeMappings : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerWithBasicTypeMappings()
            : base("BasicTypeMapping")
        {
            MainSetup();
        }

        private IUnityContainer container;

        protected override void Arrange()
        {
            base.Arrange();
            this.container = new UnityContainer();
        }

        protected override void Act()
        {
            this.section.Configure(this.container);
        }

        [Fact]
        public void Then_ContainerHasTwoMappingsForILogger()
        {
            Assert.Equal(2,
               this.container.Registrations.Where(r => r.RegisteredType == typeof(ILogger)).Count());
        }

        [Fact]
        public void Then_DefaultILoggerIsMappedToMockLogger()
        {
            Assert.Equal(typeof(MockLogger),
               this.container.Registrations
                    .Where(r => r.RegisteredType == typeof(ILogger) && r.Name == null)
                    .Select(r => r.MappedToType)
                    .First());
        }

        [Fact]
        public void Then_SpecialILoggerIsMappedToSpecialLogger()
        {
            Assert.Equal(typeof(SpecialLogger),
               this.container.Registrations
                    .Where(r => r.RegisteredType == typeof(ILogger) && r.Name == "special")
                    .Select(r => r.MappedToType)
                    .First());
        }

        [Fact]
        public void Then_AllRegistrationsHaveTransientLifetime()
        {
            Assert.True(this.container.Registrations
                .Where(r => r.RegisteredType == typeof(ILogger))
                .All(r => r.LifetimeManagerType == typeof(TransientLifetimeManager)));
        }
    }
}
