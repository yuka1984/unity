// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class CustomAttributeMatchingRuleFixture
    {
        [Fact]
        public void CustomAttributeRuleMatchesWhenAttributeFuond()
        {
            MethodInfo method = typeof(TestObject).GetMethod("MethodWithAttribute");
            CustomAttributeMatchingRule rule = new CustomAttributeMatchingRule(typeof(RandomAttribute), false);
            Assert.True(rule.Matches(method));
        }

        [Fact]
        public void CustomAttributeRuleDeniesMatchWhenNoAttributeFound()
        {
            MethodInfo method = typeof(TestObject).GetMethod("MethodWithoutAttribute");
            CustomAttributeMatchingRule rule = new CustomAttributeMatchingRule(typeof(RandomAttribute), false);
            Assert.False(rule.Matches(method));
        }

        [Fact]
        public void CustomAttributeRuleSearchesInheritanceChainWhenInheritedIsTrue()
        {
            MethodInfo method = typeof(DerivedObject).GetMethod("MethodWithAttribute");
            CustomAttributeMatchingRule rule = new CustomAttributeMatchingRule(typeof(RandomAttribute), true);
            Assert.True(rule.Matches(method));
        }

        [Fact]
        public void CustomAttributeRuleDoesNotSearchInheritanceChainWhenInheritedIsFalse()
        {
            MethodInfo method = typeof(DerivedObject).GetMethod("MethodWithAttribute");
            CustomAttributeMatchingRule rule = new CustomAttributeMatchingRule(typeof(RandomAttribute), false);
            Assert.False(rule.Matches(method));
        }

        private class DerivedObject : TestObject
        {
            public override void MethodWithAttribute() { }
        }

        private class TestObject
        {
            [RandomAttribute]
            public virtual void MethodWithAttribute() { }

            public void MethodWithoutAttribute() { }
        }

        private class RandomAttribute : Attribute { }
    }
}
