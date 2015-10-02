// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerForSpecificConstructors
    /// </summary>
     
    public class When_ConfiguringContainerForSpecificConstructors : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerForSpecificConstructors()
            : base("VariousConstructors")
        {
            MainSetup();
        }

        private IUnityContainer container;

        protected override void Arrange()
        {
            base.Arrange();
            this.container = new UnityContainer();
        }

        [Fact]
        public void Then_CanResolveMockDatabaseAndItCallsDefaultConstructor()
        {
            section.Configure(this.container, "defaultConstructor");
            var result = this.container.Resolve<MockDatabase>();
            Assert.True(result.DefaultConstructorCalled);
        }

        [Fact]
        public void Then_ConstructorsThatDoNotMatchThrowAnException()
        {
            AssertExtensions.AssertException<InvalidOperationException>(() =>
                {
                    section.Configure(container, "invalidConstructor");
                });
        }

        // Disable obsolete warning for this one test
#pragma warning disable 618
        [Fact]
        public void Then_OldConfigureAPIStillWorks()
        {
            this.section.Containers["defaultConstructor"].Configure(this.container);
            var result = this.container.Resolve<MockDatabase>();
            Assert.True(result.DefaultConstructorCalled);
        }
#pragma warning restore 618
    }
}
