// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingConfigurationWithArrayInjection
    /// </summary>
     
    public class When_LoadingConfigurationWithArrayInjection : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigurationWithArrayInjection()
            : base("ArrayInjection")
        {
            MainSetup();
        }

        [Fact]
        public void Then_ArrayPropertyHasArrayElementAsValue()
        {
            var prop = this.GetArrayPropertyElement("specificElements");

            Assert.IsType<ArrayElement>(prop.Value);
        }

        [Fact]
        public void Then_ArrayPropertyHasTwoValuesThatWillBeInjected()
        {
            var prop = this.GetArrayPropertyElement("specificElements");
            var arrayValue = (ArrayElement)prop.Value;

            Assert.Equal(2, arrayValue.Values.Count);
        }

        [Fact]
        public void Then_ArrayPropertyValuesAreAllDependencies()
        {
            var prop = this.GetArrayPropertyElement("specificElements");
            var arrayValue = (ArrayElement)prop.Value;

            Assert.True(arrayValue.Values.All(v => v is DependencyElement));
        }

        [Fact]
        public void Then_ArrayPropertyValuesHaveExpectedNames()
        {
            var prop = this.GetArrayPropertyElement("specificElements");
            var arrayValue = (ArrayElement)prop.Value;

            CollectionAssertExtensions.AreEqual(new[] { "main", "special" },
                arrayValue.Values.Cast<DependencyElement>().Select(e => e.Name).ToList());
        }

        private PropertyElement GetArrayPropertyElement(string registrationName)
        {
            var registration = section.Containers.Default.Registrations
                .Where(r => r.TypeName == "ArrayDependencyObject" && r.Name == registrationName)
                .First();

            return registration.InjectionMembers.OfType<PropertyElement>()
                .Where(pe => pe.Name == "Loggers")
                .First();
        }
    }
}
