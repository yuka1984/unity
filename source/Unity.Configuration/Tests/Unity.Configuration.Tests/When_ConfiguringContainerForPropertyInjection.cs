// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerForPropertyInjection
    /// </summary>
     
    public class When_ConfiguringContainerForPropertyInjection : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerForPropertyInjection()
            : base("InjectingProperties", String.Empty)
        {
        }

        [Fact]
        public void Then_InjectedPropertyIsResolvedAccordingToConfiguration()
        {
            var expected = Container.Resolve<object>("special");
            var result = Container.Resolve<ObjectWithTwoProperties>("singleProperty");

            Assert.Same(expected, result.Obj1);
        }

        [Fact]
        public void Then_InjectedPropertyIsResolvedAccordingToConfigurationUsingAttributes()
        {
            var expected = Container.Resolve<object>("special");
            var result = Container.Resolve<ObjectWithTwoProperties>("twoProperties");

            Assert.Same(expected, result.Obj1);
        }

        [Fact]
        public void Then_InjectedPropertyIsProperType()
        {
            var result = Container.Resolve<ObjectWithTwoProperties>("injectingDifferentType");

            Assert.IsType<SpecialLogger>(result.Obj1);
        }

        [Fact]
        public void Then_MultiplePropertiesGetInjected()
        {
            var expected = Container.Resolve<object>("special");
            var result = Container.Resolve<ObjectWithTwoProperties>("injectingDifferentType");

            Assert.IsType<SpecialLogger>(result.Obj1);
            Assert.Same(expected, result.Obj2);
        }
    }
}
