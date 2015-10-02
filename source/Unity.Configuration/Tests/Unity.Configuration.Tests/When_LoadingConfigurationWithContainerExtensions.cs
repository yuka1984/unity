// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingConfigurationWithContainerExtensions
    /// </summary>
     
    public class When_LoadingConfigurationWithContainerExtensions : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigurationWithContainerExtensions()
            : base("ContainerExtensions")
        {
            MainSetup();
        }

        private ContainerElement defaultContainer;
        private ContainerElement newSchemaContainer;

        protected override void Act()
        {
            base.Act();
            this.defaultContainer = this.section.Containers.Default;
            this.newSchemaContainer = this.section.Containers["newSchema"];
        }

        [Fact]
        public void Then_ContainerElementContainsOneExtension()
        {
            Assert.Equal(1, this.defaultContainer.Extensions.Count);
        }

        [Fact]
        public void Then_ExtensionElementHasExpectedType()
        {
            Assert.Equal("MockContainerExtension",
               this.defaultContainer.Extensions[0].TypeName);
        }

        [Fact]
        public void Then_NewSchemaContainerContainsOneExtension()
        {
            Assert.Equal(1, this.newSchemaContainer.Extensions.Count);
        }

        [Fact]
        public void Then_NewSchemaContainerExtensionElementHasExpectedType()
        {
            Assert.Equal("MockContainerExtension",
                this.newSchemaContainer.Extensions[0].TypeName);
        }
    }
}
