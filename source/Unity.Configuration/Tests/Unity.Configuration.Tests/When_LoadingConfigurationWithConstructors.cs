// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_LoadingConfigurationWithConstructors
    /// </summary>
     
    public class When_LoadingConfigurationWithConstructors : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingConfigurationWithConstructors()
            : base("RegistrationWithConstructors")
        {
            MainSetup();
        }

        private ContainerElement container;
        private RegisterElement firstRegistration;
        private RegisterElement secondRegistration;
        private RegisterElement thirdRegistration;

        protected override void Arrange()
        {
            base.Arrange();
            this.container = this.section.Containers.Default;
            this.firstRegistration = this.container.Registrations[0];
            this.secondRegistration = this.container.Registrations[1];
            this.thirdRegistration = this.container.Registrations[2];
        }

        [Fact]
        public void Then_FirstRegistrationHasOneInjectionMember()
        {
            Assert.Equal(1, this.firstRegistration.InjectionMembers.Count);
        }

        [Fact]
        public void Then_FirstRegistrationHasConstructorMember()
        {
            Assert.IsType<ConstructorElement>(this.firstRegistration.InjectionMembers[0]);
        }

        [Fact]
        public void Then_FirstRegistrationConstructorHasExpectedParameters()
        {
            var constructorElement = (ConstructorElement)this.firstRegistration.InjectionMembers[0];

            constructorElement.Parameters.Select(p => p.Name).AssertContainsExactly("one", "two", "three");
        }

        [Fact]
        public void Then_SecondRegistrationHasNoInjectionMembers()
        {
            Assert.Equal(0, this.secondRegistration.InjectionMembers.Count);
        }

        [Fact]
        public void Then_ThirdRegistrationHasZeroArgConstructor()
        {
            Assert.Equal(0,
                ((ConstructorElement)this.thirdRegistration.InjectionMembers[0]).Parameters.Count);
        }
    }
}
