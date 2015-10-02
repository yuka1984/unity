// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingConfigUsingOldContainersSyntax
    /// </summary>
     
    public class When_LoadingConfigUsingOldContainersSyntax : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigUsingOldContainersSyntax()
            : base("OldContainersSyntax")
        {
            MainSetup();
        }

        [Fact]
        public void Then_SectionContainsExpectedNumberOfContainers()
        {
            Assert.Equal(2, section.Containers.Count);
        }

        public void Then_ContainersArePresentInFileOrder()
        {
            CollectionAssertExtensions.AreEqual(new[] { String.Empty, "two" },
                section.Containers.Select(c => c.Name).ToList());
        }
    }
}
