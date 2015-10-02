// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
     
    public class When_LoadingConfigWithSectionExtensions : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigWithSectionExtensions()
            : base("SectionExtensions")
        {
            MainSetup();
        }

        [Fact]
        public void Then_ExpectedNumberOfSectionExtensionArePresent()
        {
            Assert.Equal(2, section.SectionExtensions.Count);
        }

        [Fact]
        public void Then_FirstSectionExtensionIsPresent()
        {
            Assert.Equal("TestSectionExtension", section.SectionExtensions[0].TypeName);
            Assert.Equal(String.Empty, section.SectionExtensions[0].Prefix);
        }

        [Fact]
        public void Then_SecondSectionExtensionIsPresent()
        {
            Assert.Equal("TestSectionExtension", section.SectionExtensions[1].TypeName);
            Assert.Equal("ext2", section.SectionExtensions[1].Prefix);
        }

        [Fact]
        public void Then_TestSectionExtensionWasInvokedOnce()
        {
            Assert.Equal(1, TestSectionExtension.NumberOfCalls);
        }

        [Fact]
        public void Then_ContainerConfiguringExtensionElementsWereAdded()
        {
            Assert.Equal(typeof(ContainerConfigElementOne),
                ExtensionElementMap.GetContainerConfiguringElementType("configOne"));
            Assert.Equal(typeof(ContainerConfigElementTwo),
                ExtensionElementMap.GetContainerConfiguringElementType("configTwo"));
        }

        [Fact]
        public void Then_PrefixedContainerConfiguringExtensionsWereAdded()
        {
            Assert.Equal(typeof(ContainerConfigElementOne),
                ExtensionElementMap.GetContainerConfiguringElementType("ext2.configOne"));
            Assert.Equal(typeof(ContainerConfigElementTwo),
                ExtensionElementMap.GetContainerConfiguringElementType("ext2.configTwo"));
        }

        [Fact]
        public void Then_ValueElementWasAdded()
        {
            Assert.Equal(typeof(SeventeenValueElement),
                ExtensionElementMap.GetParameterValueElementType("seventeen"));
        }

        [Fact]
        public void Then_UnprefixedAliasWasAdded()
        {
            string typeName = section.TypeAliases["scalarObject"];
            Assert.NotNull(typeName);
            Assert.Equal(typeof(ObjectTakingScalars).AssemblyQualifiedName, typeName);
        }

        [Fact]
        public void Then_PrefixedAliasWasAdded()
        {
            string typeName = section.TypeAliases["ext2.scalarObject"];
            Assert.NotNull(typeName);
            Assert.Equal(typeof(ObjectTakingScalars).AssemblyQualifiedName, typeName);
        }
    }
}
