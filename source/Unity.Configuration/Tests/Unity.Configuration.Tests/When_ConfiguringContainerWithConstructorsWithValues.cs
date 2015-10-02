// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerWithConstructorsWithValues
    /// </summary>
     
    public class When_ConfiguringContainerWithConstructorsWithValues : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerWithConstructorsWithValues()
            : base("VariousConstructors", "constructorWithValue")
        {
        }

        [Fact]
        public void Then_ConstructorGetsProperLiteralValuePassedFromChildElement()
        {
            var result = Container.Resolve<MockDatabase>("withExplicitValueElement");

            Assert.Equal("northwind", result.ConnectionString);
        }

        [Fact]
        public void Then_ConstructorGetsProperResolvedDependency()
        {
            var result = Container.Resolve<MockDatabase>("resolvedWithName");

            Assert.Equal("adventureWorks", result.ConnectionString);
        }

        [Fact]
        public void Then_ConstructorGetsProperResolvedDependencyViaAttribute()
        {
            var result = Container.Resolve<MockDatabase>("resolvedWithNameViaAttribute");

            Assert.Equal("contosoDB", result.ConnectionString);
        }

        [Fact]
        public void Then_ValuesAreProperlyConvertedWhenTypeIsNotString()
        {
            var result = Container.Resolve<ObjectTakingScalars>("injectInt");

            Assert.Equal(17, result.IntValue);
        }

        [Fact]
        public void Then_ConstructorGetsPropertyLiteralValueFromValueAttribute()
        {
            var result = Container.Resolve<ObjectTakingScalars>("injectIntWithValueAttribute");

            Assert.Equal(35, result.IntValue);
        }

        [Fact]
        public void Then_TypeConverterIsUsedToGenerateConstructorValue()
        {
            var result = Container.Resolve<ObjectTakingScalars>("injectIntWithTypeConverter");

            Assert.Equal(-35, result.IntValue);
        }

        [Fact]
        public void Then_TypeConverterIsUsedToGenerateConstructorValueViaAttribute()
        {
            var result = Container.Resolve<ObjectTakingScalars>("injectIntWithTypeConverterAttribute");

            Assert.Equal(-35, result.IntValue);
        }
    }
}
