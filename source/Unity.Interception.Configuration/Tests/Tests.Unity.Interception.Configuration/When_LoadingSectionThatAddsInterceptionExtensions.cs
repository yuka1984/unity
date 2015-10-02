// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests
{
     
    public class When_LoadingSectionThatAddsInterceptionExtensions : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingSectionThatAddsInterceptionExtensions()
            : base("SectionExtensionBasics")
        {
            MainSetup();
        }

        [Fact]
        public void Then_SectionExtensionIsPresent()
        {
            Assert.IsType<InterceptionConfigurationExtension>(section.SectionExtensions[0].ExtensionObject);
        }

        [Fact]
        public void Then_InterceptionElementHasBeenAdded()
        {
            Assert.NotNull(ExtensionElementMap.GetContainerConfiguringElementType("interception"));
        }
    }
}
