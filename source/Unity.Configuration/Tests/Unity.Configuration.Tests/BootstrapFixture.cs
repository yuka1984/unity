// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Configuration;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Basic bootstrapping to confirm that our "load from resources" config
    /// file helper works.
    /// </summary>
     
    public class BootstrapFixture
    {
        [Fact]
        public void CanReadBootstrapConfig()
        {
            var loader = new ConfigFileLoader<ConfigFileLocator>("Bootstrap");
            var section = loader.GetSection<AppSettingsSection>("appSettings");
            Assert.NotNull(section);
            Assert.Equal("value", section.Settings["Test"].Value);
        }
    }
}
