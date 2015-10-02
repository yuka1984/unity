// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingSectionWithProperties
    /// </summary>
     
    public class When_LoadingSectionWithProperties : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingSectionWithProperties()
            : base("InjectingProperties")
        {
            MainSetup();
        }

        [Fact]
        public void Then_RegistrationHasOnePropertyElement()
        {
            var registration = (from reg in section.Containers.Default.Registrations
                                where reg.TypeName == "ObjectWithTwoProperties" && reg.Name == "singleProperty"
                                select reg).First();

            Assert.Equal(1, registration.InjectionMembers.Count);
            Assert.IsType<PropertyElement>(registration.InjectionMembers[0]);
        }

        [Fact]
        public void Then_RegistrationHasTwoPropertyElements()
        {
            var registration = (from reg in section.Containers.Default.Registrations
                                where reg.TypeName == "ObjectWithTwoProperties" && reg.Name == "twoProperties"
                                select reg).First();

            Assert.Equal(2, registration.InjectionMembers.Count);
            Assert.True(registration.InjectionMembers.All(im => im is PropertyElement));
        }

        [Fact]
        public void Then_PropertyNamesAreProperlyDeserialized()
        {
            var registration = (from reg in section.Containers.Default.Registrations
                                where reg.TypeName == "ObjectWithTwoProperties" && reg.Name == "twoProperties"
                                select reg).First();

            CollectionAssertExtensions.AreEqual(new string[] { "Obj1", "Obj2" },
                registration.InjectionMembers.OfType<PropertyElement>().Select(pe => pe.Name).ToList());
        }
    }
}
