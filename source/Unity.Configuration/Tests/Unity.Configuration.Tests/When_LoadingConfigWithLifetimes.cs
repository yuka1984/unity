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
    /// Summary description for When_LoadingConfigWithLifetimes
    /// </summary>
     
    public class When_LoadingConfigWithLifetimes : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigWithLifetimes()
            : base("Lifetimes")
        {
            MainSetup();
        }

        [Fact]
        public void Then_ILoggerHasSingletonLifetime()
        {
            var registration = section.Containers.Default.Registrations.Where(
                r => r.TypeName == "ILogger" && r.Name == string.Empty).First();

            Assert.Equal("singleton", registration.Lifetime.TypeName);
        }

        [Fact]
        public void Then_TypeConverterInformationIsProperlyDeserialized()
        {
            var lifetime = section.Containers.Default.Registrations
                .Where(r => r.TypeName == "ILogger" && r.Name == "reverseSession")
                .First()
                .Lifetime;

            Assert.Equal("session", lifetime.TypeName);
            Assert.Equal("backwards", lifetime.Value);
            Assert.Equal("reversed", lifetime.TypeConverterTypeName);
        }
    }
}
