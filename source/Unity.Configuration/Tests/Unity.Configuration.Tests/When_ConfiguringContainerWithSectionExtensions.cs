// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerWithSectionExtensions
    /// </summary>
     
    public class When_ConfiguringContainerWithSectionExtensions : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerWithSectionExtensions()
            : base("SectionExtensions", String.Empty)
        {
        }

        [Fact]
        public void Then_ExtensionValueElementIsCalled()
        {
            var result = Container.Resolve<ObjectTakingScalars>();

            Assert.Equal(17, result.IntValue);
        }

        [Fact]
        public void Then_PrefixedExtensionValueElementIsCalled()
        {
            var result = Container.Resolve<ObjectTakingScalars>("prefixedValue");

            Assert.Equal(17, result.IntValue);
        }
    }
}
