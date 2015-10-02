// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class ParameterTypeMatchingRuleFixture
    {
        private MethodInfo targetMethodString;
        private MethodInfo targetMethodInt;
        private MethodInfo returnsAString;
        private MethodInfo targetMethodIntString;
        private MethodInfo targetMethodStringInt;
        private MethodInfo targetWithOutParams;

        public ParameterTypeMatchingRuleFixture()
        {
            Type targetType = typeof(ParameterTypeMatchingRuleTarget);
            targetMethodString = targetType.GetMethod("TargetMethodString");
            targetMethodInt = targetType.GetMethod("TargetMethodInt");
            returnsAString = targetType.GetMethod("ReturnsAString");
            targetMethodIntString = targetType.GetMethod("TargetMethodIntString");
            targetMethodStringInt = targetType.GetMethod("TargetMethodStringInt");
            targetWithOutParams = targetType.GetMethod("TargetWithOutParams");
        }

        [Fact]
        public void ShouldMatchOnSingleInputParameter()
        {
            ParameterTypeMatchingRule rule = new ParameterTypeMatchingRule(
                new ParameterTypeMatchingInfo[]
                    {
                        new ParameterTypeMatchingInfo("System.String", false, ParameterKind.Input)
                    });

            Assert.True(rule.Matches(targetMethodString));
            Assert.False(rule.Matches(targetMethodInt));
        }

        [Fact]
        public void ShouldMatchOnReturnType()
        {
            ParameterTypeMatchingRule rule = new ParameterTypeMatchingRule(
                new ParameterTypeMatchingInfo[]
                    {
                        new ParameterTypeMatchingInfo("System.String", false, ParameterKind.ReturnValue)
                    });
            Assert.False(rule.Matches(targetMethodString));
            Assert.True(rule.Matches(returnsAString));
        }

        [Fact]
        public void ShouldMatchOnOneOfManyParameters()
        {
            ParameterTypeMatchingRule rule = new ParameterTypeMatchingRule(
                new ParameterTypeMatchingInfo[]
                    {
                        new ParameterTypeMatchingInfo("System.Int32", false, ParameterKind.Input)
                    });

            Assert.True(rule.Matches(targetMethodInt));
            Assert.True(rule.Matches(targetMethodIntString));
            Assert.True(rule.Matches(targetMethodStringInt));
            Assert.False(rule.Matches(returnsAString));
        }

        [Fact]
        public void ShouldMatchOnOutParams()
        {
            ParameterTypeMatchingRule rule = new ParameterTypeMatchingRule(
                new ParameterTypeMatchingInfo[]
                    {
                        new ParameterTypeMatchingInfo("System.Int32", false, ParameterKind.Output)
                    });
            Assert.True(rule.Matches(targetWithOutParams));
            Assert.False(rule.Matches(targetMethodInt));
        }

        [Fact]
        public void ShouldMatchInOrOut()
        {
            ParameterTypeMatchingRule rule = new ParameterTypeMatchingRule(
                new ParameterTypeMatchingInfo[]
                    {
                        new ParameterTypeMatchingInfo("System.Int32", false, ParameterKind.InputOrOutput)
                    });
            Assert.True(rule.Matches(targetWithOutParams));
            Assert.True(rule.Matches(targetMethodInt));
        }

        [Fact]
        public void ShouldMatchOr()
        {
            ParameterTypeMatchingRule rule = new ParameterTypeMatchingRule(
                new ParameterTypeMatchingInfo[]
                    {
                        new ParameterTypeMatchingInfo("System.Int32", false, ParameterKind.InputOrOutput),
                        new ParameterTypeMatchingInfo("String", false, ParameterKind.InputOrOutput),
                    });

            Assert.True(rule.Matches(targetMethodString));
            Assert.True(rule.Matches(targetMethodInt));
            Assert.True(rule.Matches(targetMethodIntString));
            Assert.True(rule.Matches(targetWithOutParams));
            Assert.False(rule.Matches(returnsAString));
        }
    }

    public class ParameterTypeMatchingRuleTarget
    {
        public string ReturnsAString()
        {
            return String.Empty;
        }

        public void TargetMethodInt(int intParam) { }

        public void TargetMethodIntString(int intParam,
                                          string stringParam) { }

        public void TargetMethodString(string param1) { }

        public void TargetMethodStringInt(string stringParam,
                                          int intParam) { }

        public string TargetWithOutParams(double doubleParam,
                                          out int outIntParam)
        {
            outIntParam = 42;
            return String.Empty;
        }
    }
}
