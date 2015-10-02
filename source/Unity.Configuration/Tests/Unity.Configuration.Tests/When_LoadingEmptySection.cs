// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingEmptySection
    /// </summary>
     
    public class When_LoadingEmptySection : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingEmptySection()
            : base("EmptySection")
        {
            MainSetup();
        }

        [Fact]
        public void Then_SectionIsPresent()
        {
            Assert.NotNull(this.section);
        }
    }
}
