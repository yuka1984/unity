// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingConfigWithMultipleContainers
    /// </summary>
     
    public class When_LoadingConfigWithMultipleContainers : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigWithMultipleContainers()
            : base("SingleSectionMultipleNamedContainers")
        {
            MainSetup();
        }

        [Fact]
        public void Then_ExpectedNumberOfContainersArePresent()
        {
            Assert.Equal(2, section.Containers.Count);
        }

        [Fact]
        public void Then_FirstContainerNameIsCorrect()
        {
            Assert.Equal("one", section.Containers[0].Name);
        }

        [Fact]
        public void Then_SecondContainerNameIsCorrect()
        {
            Assert.Equal("two", section.Containers[1].Name);
        }

        [Fact]
        public void Then_EnumeratingContainersHappensInOrderOfConfigFile()
        {
            CollectionAssertExtensions.AreEqual(new[] { "one", "two" },
                section.Containers.Select(c => c.Name).ToList());
        }
    }
}
