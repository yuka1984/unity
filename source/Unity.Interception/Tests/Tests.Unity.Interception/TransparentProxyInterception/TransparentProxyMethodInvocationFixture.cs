// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
    /// <summary>
    /// Tests for the TransparentProxyMethodInvocation class, which wraps
    /// a IMethodCallMessage in an IMethodInvocation implementation.
    /// </summary>
     
    public class TransparentProxyMethodInvocationFixture
    {
        #region Test Methods

        [Fact]
        public void ShouldBeCreatable()
        {
            MethodBase methodInfo = GetTargetMethodInfo("FirstTarget");
            InvocationTarget target = new InvocationTarget();
            IMethodInvocation invocation = GetInvocation(methodInfo, target);
        }

        [Fact]
        public void ShouldMapInputsCorrectly()
        {
            MethodBase methodInfo = GetTargetMethodInfo("FirstTarget");
            InvocationTarget target = new InvocationTarget();
            IMethodInvocation invocation = GetInvocation(methodInfo, target);

            Assert.Equal(2, invocation.Inputs.Count);
            Assert.Equal(1, invocation.Inputs[0]);
            Assert.Equal("two", invocation.Inputs[1]);
            Assert.Equal("two", invocation.Inputs["two"]);
            Assert.Equal(1, invocation.Inputs["one"]);
            Assert.Equal(methodInfo, invocation.MethodBase);
            Assert.Same(target, invocation.Target);
        }

        [Fact]
        public void ShouldBeAbleToAddToContext()
        {
            MethodBase methodInfo = GetTargetMethodInfo("FirstTarget");
            InvocationTarget target = new InvocationTarget();
            IMethodInvocation invocation = GetInvocation(methodInfo, target);

            invocation.InvocationContext["firstItem"] = 1;
            invocation.InvocationContext["secondItem"] = "hooray!";

            Assert.Equal(2, invocation.InvocationContext.Count);
            Assert.Equal(1, invocation.InvocationContext["firstItem"]);
            Assert.Equal("hooray!", invocation.InvocationContext["secondItem"]);
        }

        [Fact]
        public void ShouldBeAbleToChangeInputs()
        {
            MethodBase methodInfo = GetTargetMethodInfo("FirstTarget");
            InvocationTarget target = new InvocationTarget();
            IMethodInvocation invocation = GetInvocation(methodInfo, target);

            Assert.Equal(1, invocation.Inputs["one"]);
            invocation.Inputs["one"] = 42;
            Assert.Equal(42, invocation.Inputs["one"]);
        }

        #endregion

        #region Helper factories

        private MethodBase GetTargetMethodInfo(string methodName)
        {
            return (MethodBase)(typeof(InvocationTarget).GetMember(methodName)[0]);
        }

        private IMethodInvocation GetInvocation(MethodBase methodInfo,
                                        InvocationTarget target)
        {
            IMethodCallMessage remotingMessage = new FakeMethodCallMessage(methodInfo, new object[] { 1, "two" });

            return new TransparentProxyMethodInvocation(remotingMessage, target);
        }

        #endregion
    }

    public class InvocationTarget : MarshalByRefObject
    {
        public string FirstTarget(int one,
                                  string two)
        {
            return "Boo!";
        }
    }
}
