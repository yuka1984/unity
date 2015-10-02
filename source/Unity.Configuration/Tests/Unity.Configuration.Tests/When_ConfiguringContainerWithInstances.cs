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
    /// Summary description for When_ConfiguringContainerWithInstances
    /// </summary>
     
    public class When_ConfiguringContainerWithInstances : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerWithInstances()
            : base("RegisteringInstances")
        {
            MainSetup();
        }

        private IUnityContainer container;

        protected override void Arrange()
        {
            base.Arrange();
            this.container = new UnityContainer();
        }

        protected override void Act()
        {
            base.Act();
            this.section.Configure(this.container);
        }

        [Fact]
        public void Then_DefaultStringInstanceIsRegistered()
        {
            Assert.Equal("AdventureWorks", this.container.Resolve<string>());
        }

        [Fact]
        public void Then_DefaultIntInstanceIsRegistered()
        {
            Assert.Equal(42, this.container.Resolve<int>());
        }

        [Fact]
        public void Then_NamedIntIsRegistered()
        {
            Assert.Equal(23, this.container.Resolve<int>("forward"));
        }

        [Fact]
        public void Then_InstanceUsingTypeConverterIsCreatedProperly()
        {
            Assert.Equal(-23, this.container.Resolve<int>("negated"));
        }
    }
}
