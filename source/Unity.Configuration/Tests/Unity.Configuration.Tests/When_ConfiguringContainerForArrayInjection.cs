// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerForArrayInjection
    /// </summary>
     
    public class When_ConfiguringContainerForArrayInjection : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerForArrayInjection()
            : base("ArrayInjection", String.Empty)
        {
        }

        [Fact]
        public void Then_DefaultResolutionReturnsAllRegisteredLoggers()
        {
            var result = Container.Resolve<ArrayDependencyObject>("defaultInjection");

            result.Loggers.Select(l => l.GetType()).AssertContainsInAnyOrder(
                typeof(SpecialLogger), typeof(MockLogger), typeof(MockLogger));
        }

        [Fact]
        public void Then_SpecificElementsAreInjected()
        {
            var result = Container.Resolve<ArrayDependencyObject>("specificElements");

            result.Loggers.Select(l => l.GetType()).AssertContainsInAnyOrder(
                typeof(SpecialLogger), typeof(MockLogger));
        }

        [Fact]
        public void Then_CanMixResolutionAndValuesInAnArray()
        {
            var result = Container.Resolve<ArrayDependencyObject>("mixingResolvesAndValues");

            result.Strings.AssertContainsExactly("first", "Not the second", "third");
        }

        [Fact]
        public void Then_CanConfigureZeroLengthArrayForInjection()
        {
            var result = Container.Resolve<ArrayDependencyObject>("zeroLengthArray");

            Assert.NotNull(result.Strings);
            Assert.Equal(0, result.Strings.Length);
        }

        [Fact]
        public void Then_GenericArrayPropertiesAreInjected()
        {
            var result = Container.Resolve<GenericArrayPropertyDependency<string>>("defaultResolution");

            result.Stuff.AssertContainsInAnyOrder("first", "second", "third");
        }

        [Fact]
        public void Then_CanConfigureZeroLengthGenericArrayToBeInjected()
        {
            var result = Container.Resolve<GenericArrayPropertyDependency<string>>("explicitZeroLengthArray");

            Assert.Equal(0, result.Stuff.Count());
        }
    }
}
