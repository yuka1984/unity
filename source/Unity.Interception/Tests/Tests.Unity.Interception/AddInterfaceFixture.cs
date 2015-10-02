// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension.Tests.ObjectsUnderTest;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
    /// <summary>
    /// Summary description for AddInterfaceFixture
    /// </summary>
     
    public class AddInterfaceFixture
    {
        [Fact]
        public void CanProxyWithBehaviorThatAddsInterface()
        {
            var target = new MockDal();
            var proxied = Intercept.ThroughProxy<IDal>(target,
                new InterfaceInterceptor(),
                new[] { new AdditionalInterfaceBehavior() });

            Assert.NotNull(proxied);
        }

        [Fact]
        public void BehaviorAddsInterface()
        {
            var target = new MockDal();
            var proxied = Intercept.ThroughProxy<IDal>(target,
                new InterfaceInterceptor(),
                new[] { new AdditionalInterfaceBehavior() });

            Assert.NotNull(proxied as IAdditionalInterface);
        }

        [Fact]
        public void CanInvokeMethodAddedByBehavior()
        {
            var proxied = Intercept.NewInstance<MockDal>(
                new VirtualMethodInterceptor(),
                new[] { new AdditionalInterfaceBehavior() });

            Assert.Equal(10, ((IAdditionalInterface)proxied).DoNothing());
        }

        [Fact]
        public void CanManuallyAddAdditionalInterface()
        {
            var target = new MockDal();
            var proxied = Intercept.ThroughProxyWithAdditionalInterfaces<IDal>(target,
                new InterfaceInterceptor(),
                new[] { new AdditionalInterfaceBehavior(false) },
                new[] { typeof(IAdditionalInterface) });

            Assert.NotNull(proxied as IAdditionalInterface);
        }

        [Fact]
        public void CanInvokeMethodOnManuallyAddedInterface()
        {
            var target = new MockDal();
            var proxied = Intercept.ThroughProxyWithAdditionalInterfaces<IDal>(target,
                new InterfaceInterceptor(),
                new[] { new AdditionalInterfaceBehavior(false) },
                new[] { typeof(IAdditionalInterface) });

            Assert.Equal(10, ((IAdditionalInterface)proxied).DoNothing());
        }
    }
}
