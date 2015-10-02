// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingConfigWithTypeMappings
    /// </summary>
     
    public class When_LoadingConfigWithTypeMappings : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigWithTypeMappings()
            : base("BasicTypeMapping")
        {
            MainSetup();
        }

        private ContainerElement container;

        protected override void Arrange()
        {
            base.Arrange();
            this.container = this.section.Containers.Default;
        }

        [Fact]
        public void Then_RegistrationsArePresentInContainer()
        {
            Assert.Equal(2, this.container.Registrations.Count);
        }

        [Fact]
        public void Then_TypesAreAsGivenInFile()
        {
            this.AssertRegistrationsAreSame(r => r.TypeName, "ILogger", "ILogger");
        }

        [Fact]
        public void Then_MappedNamesAreAsGivenInFile()
        {
            this.AssertRegistrationsAreSame(r => r.Name, String.Empty, "special");
        }

        [Fact]
        public void Then_MappedToTypesAreAsGivenInFile()
        {
            this.AssertRegistrationsAreSame(r => r.MapToName, "MockLogger", "SpecialLogger");
        }

        private void AssertRegistrationsAreSame(Func<RegisterElement, string> selector, params string[] expectedStrings)
        {
            CollectionAssertExtensions.AreEqual(expectedStrings, this.container.Registrations.Select(selector).ToList());
        }
    }
}
