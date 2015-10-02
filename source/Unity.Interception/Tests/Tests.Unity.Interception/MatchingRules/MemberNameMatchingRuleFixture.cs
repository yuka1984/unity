// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class MemberNameMatchingRuleFixture
    {
        private MethodInfo methodOne;
        private MethodInfo methodTwo;
        private MethodInfo save;
        private MethodInfo reset;
        private MethodInfo closeAndReset;

        public MemberNameMatchingRuleFixture()
        {
            methodOne = typeof(TestTarget).GetMethod("MethodOne");
            methodTwo = typeof(TestTarget).GetMethod("MethodTwo");
            save = typeof(TestTarget).GetMethod("Save");
            reset = typeof(TestTarget).GetMethod("Reset");
            closeAndReset = typeof(TestTarget).GetMethod("CloseAndReset");
        }

        [Fact]
        public void ShouldMatchExactName()
        {
            MemberNameMatchingRule rule = new MemberNameMatchingRule("Save");
            Assert.True(rule.Matches(save));
        }

        [Fact]
        public void ShouldMatchWithWildcard()
        {
            MemberNameMatchingRule rule = new MemberNameMatchingRule("*Reset");
            foreach (MethodInfo method in new MethodInfo[] { reset, closeAndReset })
            {
                Assert.True(rule.Matches(method),
                              string.Format("Match failed for method {0}", method.Name));
            }
        }

        [Fact]
        public void ShouldMatchMultipleMethods()
        {
            IMatchingRule rule = new MemberNameMatchingRule(
                new string[] { "MethodTwo", "Save" });
            Assert.False(rule.Matches(methodOne));
            Assert.True(rule.Matches(methodTwo));
            Assert.True(rule.Matches(save));
        }

        [Fact]
        public void ShouldMatchMultipleWildcards()
        {
            IMatchingRule rule = new MemberNameMatchingRule(
                new string[] { "Method*", "*Reset" });
            foreach (MethodInfo method in new MethodInfo[] { methodOne, methodTwo, reset, closeAndReset })
            {
                Assert.True(rule.Matches(method),
                              string.Format("Match failed for method {0}", method.Name));
            }
        }
    }

    public class TestTarget
    {
        public void CloseAndReset() { }

        public void MethodOne() { }

        public void MethodTwo() { }

        public void Reset() { }

        public void Save() { }
    }
}
