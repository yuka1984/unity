// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerWithContainerExtensions
    /// </summary>
     
    public class When_ConfiguringContainerWithContainerExtensions : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerWithContainerExtensions()
            : base("ContainerExtensions")
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
            base.Act();
            this.section.Configure(this.container);
        }

        [Fact]
        public void Then_ContainerHasExtensionAdded()
        {
            Assert.NotNull(this.container.Configure<MockContainerExtension>());
        }
    }
}
