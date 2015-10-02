// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects.MyGenericTypes;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerForGenericAndNonGenericTypes
    /// </summary>
     
    public class When_ConfiguringContainerForGenericAndNonGenericTypes : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerForGenericAndNonGenericTypes()
            : base("Generics", "container1")
        {
        }

        [Fact]
        public void Then_CanResolveConfiguredGenericType()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>();

            Assert.Equal(8, result.Items.Length);
            Assert.IsType<MyPrintService<IItem>>(result.Printer);
        }

        [Fact]
        public void Then_CanResolveConfiguredGenericTypeWithSpecificElements()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>("OnlyThree");
            Assert.Equal(3, result.Items.Length);
        }

        [Fact]
        public void Then_CanConfigureGenericArrayInjectionViaAPI()
        {
            Container.RegisterType(typeof(ItemsCollection<>), "More",
                new InjectionConstructor("MyGenericCollection", new ResolvedParameter(typeof(IGenericService<>))),
                new InjectionProperty("Items",
                    new GenericResolvedArrayParameter("T",
                        new GenericParameter("T", "Xray"),
                        new GenericParameter("T", "Common"),
                        new GenericParameter("T", "Tractor"))));

            var result = Container.Resolve<ItemsCollection<IItem>>("More");
            Assert.Equal(3, result.Items.Length);
        }

        [Fact]
        public void Then_CanResolveConfiguredResolvableOptionalGenericType()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>("optional resolvable");

            Assert.Equal(1, result.Items.Length);
            Assert.NotNull(result.Items[0]);
            Assert.Equal("Charlie Miniature", result.Items[0].ItemName);
        }

        [Fact]
        public void Then_CanResolveConfiguredNonResolvableOptionalGenericType()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>("optional non resolvable");

            Assert.Equal(1, result.Items.Length);
            Assert.Null(result.Items[0]);
        }

        [Fact]
        public void Then_CanResolveConfiguredGenericTypeWithArrayInjectedInConstructor()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>("ThroughConstructor");

            Assert.Equal(8, result.Items.Length);
            Assert.IsType<MyPrintService<IItem>>(result.Printer);
        }

        [Fact]
        public void Then_CanResolveConfiguredGenericTypeWithArrayInjectedInConstructorWithSpecificElements()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>("ThroughConstructorWithSpecificElements");

            Assert.Equal(3, result.Items.Length);
        }

        // [Fact]
        // nested arrays with generics not supported by container
        public void Then_CanResolveConfiguredGenericTypeWithArrayOfArraysInjectedInConstructorWithSpecificElements()
        {
            var result = Container.Resolve<ItemsCollection<IItem>>("ArrayOfArraysThroughConstructorWithSpecificElements");

            Assert.Equal(3, result.Items.Length);
        }
    }

     
    public class When_ConfiguringContainerWithDependencyElementForGenericPropertyArrayWithTypeSet : ContainerConfiguringFixture<ConfigFileLocator>
    {
        private InvalidOperationException exception;

        public When_ConfiguringContainerWithDependencyElementForGenericPropertyArrayWithTypeSet()
            : base("Generics", "dependency with type")
        { }

        protected override void Act()
        {
            try
            {
                base.Act();
            }
            catch (InvalidOperationException ex)
            {
                this.exception = ex;
            }
        }

        [Fact]
        public void ThenContainerSetupThrows()
        {
            Assert.NotNull(this.exception);
        }
    }

     
    public class When_ConfiguringContainerWithParameterWithValueElement : ContainerConfiguringFixture<ConfigFileLocator>
    {
        private InvalidOperationException exception;

        public When_ConfiguringContainerWithParameterWithValueElement()
            : base("Generics", "property with value")
        { }

        protected override void Act()
        {
            try
            {
                base.Act();
            }
            catch (InvalidOperationException ex)
            {
                this.exception = ex;
            }
        }

        [Fact]
        public void ThenContainerSetupThrows()
        {
            Assert.NotNull(this.exception);
        }
    }

     
    public class When_ConfiguringContainerWithGenericArrayPropertyWithValueElement : ContainerConfiguringFixture<ConfigFileLocator>
    {
        private InvalidOperationException exception;
        
        public When_ConfiguringContainerWithGenericArrayPropertyWithValueElement()
            : base("Generics", "generic array property with value")
        { }

        protected override void Act()
        {
            try
            {
                base.Act();
            }
            catch (InvalidOperationException ex)
            {
                this.exception = ex;
            }
        }

        [Fact]
        public void ThenContainerSetupThrows()
        {
            Assert.NotNull(this.exception);
        }
    }

     
    public class When_ConfiguringContainerWithChainedGenericParameterWithValueElement : ContainerConfiguringFixture<ConfigFileLocator>
    {
        private InvalidOperationException exception;
        
        public When_ConfiguringContainerWithChainedGenericParameterWithValueElement()
            : base("Generics", "chained generic parameter with value")
        { }

        protected override void Act()
        {
            try
            {
                base.Act();
            }
            catch (InvalidOperationException ex)
            {
                this.exception = ex;
            }
        }

        [Fact]
        public void ThenContainerSetupThrows()
        {
            Assert.NotNull(this.exception);
        }
    }

     
    public class When_ConfiguringContainerWithDependencyElementForArrayWithTypeSet : ContainerConfiguringFixture<ConfigFileLocator>
    {
        private InvalidOperationException exception;

        public When_ConfiguringContainerWithDependencyElementForArrayWithTypeSet()
            : base("Generics", "generic array property with dependency with type")
        { }

        protected override void Act()
        {
            try
            {
                base.Act();
            }
            catch (InvalidOperationException ex)
            {
                this.exception = ex;
            }
        }

        [Fact]
        public void ThenContainerSetupThrows()
        {
            Assert.NotNull(this.exception);
        }
    }

     
    public class When_ConfiguringContainerWithArrayElementForChainedGenericParameter : ContainerConfiguringFixture<ConfigFileLocator>
    {
        private InvalidOperationException exception;
        
        public When_ConfiguringContainerWithArrayElementForChainedGenericParameter()
            : base("Generics", "chained generic parameter with array")
        { }

        protected override void Act()
        {
            try
            {
                base.Act();
            }
            catch (InvalidOperationException ex)
            {
                this.exception = ex;
            }
        }

        [Fact]
        public void ThenContainerSetupThrows()
        {
            Assert.NotNull(this.exception);
        }
    }
}
