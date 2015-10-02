// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfigurationContainerForMethodInjection
    /// </summary>
     
    public class When_ConfigurationContainerForMethodInjection : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfigurationContainerForMethodInjection()
            : base("MethodInjection", String.Empty)
        {
        }

        [Fact]
        public void Then_SingleInjectionMethodIsCalledWithExpectedValues()
        {
            var result = Container.Resolve<ObjectWithInjectionMethod>("singleMethod");

            Assert.Equal("northwind", result.ConnectionString);
            Assert.IsType<MockLogger>(result.Logger);
        }

        [Fact]
        public void Then_MultipleInjectionMethodsCalledWithExpectedValues()
        {
            var result = Container.Resolve<ObjectWithInjectionMethod>("twoMethods");

            Assert.Equal("northwind", result.ConnectionString);
            Assert.IsType<MockLogger>(result.Logger);
            Assert.NotNull(result.MoreData);
        }

        [Fact]
        public void Then_CorrectFirstOverloadIsCalled()
        {
            var result = Container.Resolve<ObjectWithOverloads>("callFirstOverload");

            Assert.Equal(1, result.FirstOverloadCalls);
            Assert.Equal(0, result.SecondOverloadCalls);
        }

        [Fact]
        public void Then_CorrectSecondOverloadIsCalled()
        {
            var result = Container.Resolve<ObjectWithOverloads>("callSecondOverload");

            Assert.Equal(0, result.FirstOverloadCalls);
            Assert.Equal(1, result.SecondOverloadCalls);
        }

        [Fact]
        public void Then_BothOverloadsAreCalled()
        {
            var result = Container.Resolve<ObjectWithOverloads>("callBothOverloads");

            Assert.Equal(1, result.FirstOverloadCalls);
            Assert.Equal(1, result.SecondOverloadCalls);
        }

        [Fact]
        public void Then_FirstOverloadIsCalledTwice()
        {
            var result = Container.Resolve<ObjectWithOverloads>("callFirstOverloadTwice");

            Assert.Equal(2, result.FirstOverloadCalls);
            Assert.Equal(0, result.SecondOverloadCalls);
        }
    }
}
