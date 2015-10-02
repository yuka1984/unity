// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Reflection;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public partial class AssemblyMatchingRuleFixture
    {
        private MethodBase objectToStringMethod;
        
        public AssemblyMatchingRuleFixture()
        {
            objectToStringMethod = typeof(object).GetMethod("ToString");
        }

        [Fact]
        public void CanMatchAssemblyNameByNameOnly()
        {
            AssemblyMatchingRule matchingRule = new AssemblyMatchingRule("mscorlib");
            Assert.True(matchingRule.Matches(objectToStringMethod));
        }

        [Fact]
        public void CanExplicitlyDenyMatchOnVersion()
        {
            AssemblyMatchingRule matchingRule = new AssemblyMatchingRule("mscorlib, Version=1.2.3.4, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Assert.False(matchingRule.Matches(objectToStringMethod));
        }
    }
}
