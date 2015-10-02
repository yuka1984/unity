// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class MethodSignatureMatchingRuleFixture
    {
        private MethodBase objectToStringMethod;
        private MethodBase objectCtor;
        private MethodBase stringCopyToMethod;

        public MethodSignatureMatchingRuleFixture()
        {
            objectToStringMethod = typeof(object).GetMethod("ToString");
            objectCtor = typeof(object).GetConstructor(new Type[0]);
            stringCopyToMethod = typeof(string).GetMethod("CopyTo");
        }

        [Fact]
        public void MatchIsDeniedWhenParamterValuesCountDiffers()
        {
            List<string> oneParam = new List<string>();
            oneParam.Add("one");

            MethodSignatureMatchingRule matchingRule = new MethodSignatureMatchingRule(oneParam);
            Assert.False(matchingRule.Matches(objectToStringMethod));
        }

        [Fact]
        public void CanMatchOnParameterlessMethods()
        {
            List<string> parameterLess = new List<string>();
            MethodSignatureMatchingRule matchingRule = new MethodSignatureMatchingRule(parameterLess);
            Assert.True(matchingRule.Matches(objectToStringMethod));
        }

        [Fact]
        public void CanMatchOnMultipleParameterTypes()
        {
            List<string> parametersForCopyToMethod = new List<string>();
            parametersForCopyToMethod.Add("System.Int32");
            parametersForCopyToMethod.Add("System.Char[]");
            parametersForCopyToMethod.Add("System.Int32");
            parametersForCopyToMethod.Add("System.Int32");

            MethodSignatureMatchingRule matchingRule = new MethodSignatureMatchingRule(parametersForCopyToMethod);
            Assert.True(matchingRule.Matches(stringCopyToMethod));
        }

        [Fact]
        public void MatchIsDeniedWhenASingleParameterIsWrong()
        {
            List<string> parametersForCopyToMethod = new List<string>();
            parametersForCopyToMethod.Add("System.Int32");
            parametersForCopyToMethod.Add("System.Char[]");
            parametersForCopyToMethod.Add("System.NotAnInt32");
            parametersForCopyToMethod.Add("System.Int32");

            MethodSignatureMatchingRule matchingRule = new MethodSignatureMatchingRule(parametersForCopyToMethod);
            Assert.False(matchingRule.Matches(stringCopyToMethod));
        }
    }
}
