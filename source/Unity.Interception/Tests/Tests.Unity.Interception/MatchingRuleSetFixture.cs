// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
    /// <summary>
    /// Tests for the MatchingRuleSet class
    /// </summary>
     
    public class MatchingRuleSetFixture
    {
        [Fact]
        public void ShouldNotMatchWithNoContainedRules()
        {
            MatchingRuleSet ruleSet = new MatchingRuleSet();

            MethodBase member = GetType().GetMethod("ShouldNotMatchWithNoContainedRules");
            Assert.False(ruleSet.Matches(member));
        }

        [Fact]
        public void ShouldMatchWithMatchingTypeRule()
        {
            MatchingRuleSet ruleSet = new MatchingRuleSet();
            ruleSet.Add(new TypeMatchingRule(typeof(MatchingRuleSetFixture)));
            MethodBase member = GetType().GetMethod("ShouldMatchWithMatchingTypeRule");
            Assert.True(ruleSet.Matches(member));
        }

        [Fact]
        public void ShouldNotMatchWhenOneRuleDoesntMatch()
        {
            MethodBase member = GetType().GetMethod("ShouldNotMatchWhenOneRuleDoesntMatch");
            MatchingRuleSet ruleSet = new MatchingRuleSet();

            ruleSet.Add(new TypeMatchingRule(typeof(MatchingRuleSetFixture)));
            Assert.True(ruleSet.Matches(member));

            ruleSet.Add(new MemberNameMatchingRule("ThisMethodDoesntExist"));
            Assert.False(ruleSet.Matches(member));
        }
    }
}
