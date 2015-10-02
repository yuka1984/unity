// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for SingleSectionSingleContainerFixture
    /// </summary>
     
    public class When_LoadingConfigWithSingleContainer : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigWithSingleContainer()
            : base("SingleSectionSingleContainer")
        {
            MainSetup();
        }

        [Fact]
        public void Then_SectionIsNotNull()
        {
            Assert.NotNull(this.section);
        }

        [Fact]
        public void Then_ContainersPropertyIsSet()
        {
            Assert.NotNull(section.Containers);
        }

        [Fact]
        public void Then_ThereIsOneContainerInSection()
        {
            Assert.Equal(1, section.Containers.Count);
        }
    }
}
