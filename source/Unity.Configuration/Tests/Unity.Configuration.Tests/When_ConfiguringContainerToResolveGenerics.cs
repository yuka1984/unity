// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerToResolveGenerics
    /// </summary>
     
    public class When_ConfiguringContainerToResolveGenerics : ContainerConfiguringFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerToResolveGenerics()
            : base("InjectingGenerics", String.Empty)
        {
        }

        [Fact]
        public void Then_GenericParameterAsStringIsProperlySubstituted()
        {
            Container.RegisterType(typeof(GenericObjectWithConstructorDependency<>), "manual",
                new InjectionConstructor(new GenericParameter("T")));
            var manualResult = Container.Resolve<GenericObjectWithConstructorDependency<string>>("manual");

            var resultForString = Container.Resolve<GenericObjectWithConstructorDependency<string>>("basic");
            Assert.Equal(Container.Resolve<string>(), resultForString.Value);
        }

        [Fact]
        public void Then_GenericParameterAsIntIsProperlySubstituted()
        {
            var resultForInt = Container.Resolve<GenericObjectWithConstructorDependency<int>>("basic");
            Assert.Equal(Container.Resolve<int>(), resultForInt.Value);
        }
    }
}
