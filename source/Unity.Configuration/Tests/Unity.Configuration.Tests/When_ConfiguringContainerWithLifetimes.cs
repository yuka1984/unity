// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Practices.Unity.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ConfiguringContainerWithLifetimes
    /// </summary>
     
    public class When_ConfiguringContainerWithLifetimes : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_ConfiguringContainerWithLifetimes()
            : base("Lifetimes")
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
        public void Then_BaseILoggerHasSingletonLifetime()
        {
            AssertRegistration<ILogger>(null).HasLifetime<ContainerControlledLifetimeManager>();
        }

        [Fact]
        public void Then_MockLoggerHasExternalLifetime()
        {
            AssertRegistration<ILogger>("mock").HasLifetime<ExternallyControlledLifetimeManager>();
        }

        [Fact]
        public void Then_SessionLoggerHasSessionLifetime()
        {
            AssertRegistration<ILogger>("session").HasLifetime<SessionLifetimeManager>();
        }

        [Fact]
        public void Then_ReverseSessionLoggerHasSessionLifetime()
        {
            AssertRegistration<ILogger>("reverseSession").HasLifetime<SessionLifetimeManager>();
        }

        [Fact]
        public void Then_ReverseSessionLoggerLifetimeWasInitializedUsingTypeConverter()
        {
            AssertRegistration<ILogger>("reverseSession").LifetimeHasSessionKey("sdrawkcab");
        }

        [Fact]
        public void Then_RegistrationWithoutExplicitLifetimeIsTransient()
        {
            AssertRegistration<ILogger>("transient").HasLifetime<TransientLifetimeManager>();
        }

        [Fact]
        public void Then_RegistrationWithEmptyLifetimeTypeIsTransient()
        {
            AssertRegistration<ILogger>("explicitTransient").HasLifetime<TransientLifetimeManager>();
        }

        private RegistrationsToAssertOn AssertRegistration<TRegisterType>(string registeredName)
        {
            return new RegistrationsToAssertOn(
                this.container.Registrations
                    .Where(r => r.RegisteredType == typeof(TRegisterType) && r.Name == registeredName));
        }
    }

    internal static partial class RegistrationsToAssertOnExtensions
    {
        public static void LifetimeHasSessionKey(this RegistrationsToAssertOn r, string sessionKey)
        {
            Assert.True(
                r.Registrations.All(reg => ((SessionLifetimeManager)reg.LifetimeManager).SessionKey == sessionKey));
        }
    }
}
