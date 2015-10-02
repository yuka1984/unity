// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.VirtualMethodInterception
{
     
    public class InterceptingInterfaceMethodsFixture
    {
        [Fact]
        public void ImplicitlyImplementedMethodsAreInterceptedIfVirtual()
        {
            CallCountHandler handler = new CallCountHandler();
            Interesting instance = WireupHelper.GetInterceptedInstance<Interesting>("DoSomethingInteresting", handler);

            instance.DoSomethingInteresting();

            Assert.True(instance.SomethingWasCalled);
            Assert.Equal(1, handler.CallCount);
        }
    }

    public interface IOne
    {
        void DoSomethingInteresting();
    }

    public class Interesting : IOne
    {
        public bool SomethingWasCalled;

        public virtual void DoSomethingInteresting()
        {
            SomethingWasCalled = true;
        }
    }
}
