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
    /// Summary description for When_LoadingSectionWithAliases
    /// </summary>
     
    public class When_LoadingSectionWithAliases : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingSectionWithAliases()
            : base("TwoContainersAndAliases")
        {
            MainSetup();
        }

        [Fact]
        public void Then_AliasesAreAvailableInTheSection()
        {
            Assert.NotNull(section.TypeAliases);
        }

        [Fact]
        public void Then_ExpectedNumberOfAliasesArePresent()
        {
            Assert.Equal(2, section.TypeAliases.Count);
        }

        [Fact]
        public void Then_IntIsMappedToSystemInt32()
        {
            Assert.Equal("System.Int32, mscorlib", section.TypeAliases["int"]);
        }

        [Fact]
        public void Then_StringIsMappedToSystemString()
        {
            Assert.Equal("System.String, mscorlib", section.TypeAliases["string"]);
        }

        [Fact]
        public void Then_EnumerationReturnsAliasesInOrderAsGivenInFile()
        {
            CollectionAssertExtensions.AreEqual(new[] { "int", "string" },
                section.TypeAliases.Select(alias => alias.Alias).ToList());
        }

        [Fact]
        public void Then_ContainersInTheFileAreAlsoLoaded()
        {
            Assert.Equal(2, section.Containers.Count);
        }
    }
}
