// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class ReturnTypeMatchingRuleFixture
    {
        private MethodBase objectToStringMethod;
        private MethodBase objectCtor;
        private MethodBase stringCopyToMethod;

        public ReturnTypeMatchingRuleFixture()
        {
            objectToStringMethod = typeof(object).GetMethod("ToString");
            objectCtor = typeof(object).GetConstructor(new Type[0]);
            stringCopyToMethod = typeof(string).GetMethod("CopyTo");
        }

        [Fact]
        public void MatchIsDeniedWhenReturnTypeNameDiffers()
        {
            IMatchingRule matchingRule = new ReturnTypeMatchingRule("System.wichReturnType?");
            Assert.False(matchingRule.Matches(objectToStringMethod));
        }

        [Fact]
        public void MatchIsAcceptedWhenReturnTypeNameIsExactMatch()
        {
            IMatchingRule matchingRule = new ReturnTypeMatchingRule("System.String");
            Assert.True(matchingRule.Matches(objectToStringMethod));
        }

        [Fact]
        public void MatchIsDeniedWhenReturnTypeIsSpecifiedButNoReturnTypeExistsOnMethodBase()
        {
            IMatchingRule matchingRule = new ReturnTypeMatchingRule("void");
            Assert.False(matchingRule.Matches(objectCtor));
        }

        [Fact]
        public void MatchIsAcceptedWhenReturnTypeIsVoidAndMethodReturnsVoid()
        {
            IMatchingRule matchingRule = new ReturnTypeMatchingRule("System.Void");
            Assert.True(matchingRule.Matches(stringCopyToMethod));
        }

        [Fact]
        public void MatchIsAcceptedForTypeNameWithoutNamespace()
        {
            IMatchingRule matchingRule = new ReturnTypeMatchingRule("string", true);
            Assert.True(matchingRule.Matches(objectToStringMethod));
        }
    }
}
