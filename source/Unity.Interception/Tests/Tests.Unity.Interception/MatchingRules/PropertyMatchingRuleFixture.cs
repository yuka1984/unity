// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class PropertyMatchingRuleFixture
    {
        private MethodInfo getMyProperty;
        private MethodInfo setMyProperty;
        private MethodInfo getMyOtherProperty;
        private MethodInfo setNotAProperty;
        private MethodInfo getACompletelyDifferentProperty;
        private MethodInfo setACompletelyDifferentProperty;

        public PropertyMatchingRuleFixture()
        {
            Type propTarget = typeof(PropertyTarget);
            getMyProperty = propTarget.GetProperty("MyProperty").GetGetMethod();
            setMyProperty = propTarget.GetProperty("MyProperty").GetSetMethod();
            getMyOtherProperty = propTarget.GetProperty("MyOtherProperty").GetGetMethod();
            setNotAProperty = propTarget.GetMethod("SetNotAProperty");
            getACompletelyDifferentProperty =
                propTarget.GetProperty("ACompletelyDifferentProperty").GetGetMethod();
            setACompletelyDifferentProperty =
                propTarget.GetProperty("ACompletelyDifferentProperty").GetSetMethod();
        }

        [Fact]
        public void ShouldMatchPropertyName()
        {
            IMatchingRule rule =
                new PropertyMatchingRule("MyProperty");
            Assert.True(rule.Matches(getMyProperty));
            Assert.True(rule.Matches(setMyProperty));
        }

        [Fact]
        public void ShouldNotMatchSetWithGetOption()
        {
            IMatchingRule rule =
                new PropertyMatchingRule("MyProperty", PropertyMatchingOption.Get);
            Assert.True(rule.Matches(getMyProperty));
            Assert.False(rule.Matches(setMyProperty));
        }

        [Fact]
        public void ShouldNotMatchGetWithSetOption()
        {
            IMatchingRule rule =
                new PropertyMatchingRule("MyProperty", PropertyMatchingOption.Set);
            Assert.False(rule.Matches(getMyProperty));
            Assert.True(rule.Matches(setMyProperty));
        }

        [Fact]
        public void ShouldMatchWithWildcard()
        {
            IMatchingRule rule =
                new PropertyMatchingRule("My*");
            Assert.True(rule.Matches(getMyProperty));
            Assert.True(rule.Matches(setMyProperty));
            Assert.True(rule.Matches(getMyOtherProperty));
        }

        [Fact]
        public void ShouldNotMatchPathologiciallyNamedMethod()
        {
            IMatchingRule rule = new PropertyMatchingRule("NotAProperty");
            Assert.False(rule.Matches(setNotAProperty));
        }

        [Fact]
        public void ShouldMatchWithMultipleMatchTargets()
        {
            IMatchingRule rule = new PropertyMatchingRule(new PropertyMatchingInfo[]
                                                              {
                                                                  new PropertyMatchingInfo("MyProperty"),
                                                                  new PropertyMatchingInfo("ACompletelyDifferentProperty", PropertyMatchingOption.Set)
                                                              });
            Assert.True(rule.Matches(getMyProperty));
            Assert.True(rule.Matches(setMyProperty));
            Assert.False(rule.Matches(getMyOtherProperty));
            Assert.False(rule.Matches(getACompletelyDifferentProperty));
            Assert.True(rule.Matches(setACompletelyDifferentProperty));
        }
    }

    internal class PropertyTarget
    {
        public double ACompletelyDifferentProperty
        {
            get { return -23.4; }
            set { }
        }

        public string MyOtherProperty
        {
            get { return "abc"; }
        }

        public int MyProperty
        {
            get { return 1; }
            set { }
        }

        public void NotAProperty() { }

        public void SetNotAProperty(string value) { }
    }
}
