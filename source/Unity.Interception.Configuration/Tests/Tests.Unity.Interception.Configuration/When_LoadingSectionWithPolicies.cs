// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests.ConfigFiles;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests
{
     
    public class When_LoadingSectionWithPolicies : SectionLoadingFixture<ConfigFileLocator>
    {
        public When_LoadingSectionWithPolicies()
            : base("Policies")
        {
            MainSetup();
        }

        private InterceptionElement GetInterceptionElement(string containerName)
        {
            return (InterceptionElement)section.Containers[containerName].ConfiguringElements[0];
        }

        [Fact]
        public void Then_ElementWithEmptyPoliciesLoads()
        {
            Assert.Equal(0, this.GetInterceptionElement("emptyPolicies").Policies.Count);
        }

        [Fact]
        public void Then_PoliciesAreLoadedFromExplicitCollection()
        {
            this.GetInterceptionElement("explicitPolicyCollection").Policies.Select(p => p.Name)
                .AssertContainsExactly("policyOne", "policyTwo");
        }

        [Fact]
        public void Then_PoliciesAreLoadedFromImplicitCollection()
        {
            this.GetInterceptionElement("implicitPolicyCollection").Policies.Select(p => p.Name)
                .AssertContainsExactly("policyA", "policyB");
        }

        [Fact]
        public void Then_PoliciesLoadNameOnlyMatchingRules()
        {
            var interceptionElement = this.GetInterceptionElement("policyWithNamedMatchingRules");
            var policyOne = interceptionElement.Policies["policyOne"];

            policyOne.MatchingRules.Select(mr => mr.Name).AssertContainsExactly("ruleOne", "ruleTwo");
        }

        [Fact]
        public void Then_CanDefinePolicyWithMatchingRuleAndCallHandler()
        {
            var interceptionElement = this.GetInterceptionElement("policyWithGivenRulesAndHandlersTypes");
            var policyOne = interceptionElement.Policies["policyOne"];

            policyOne.MatchingRules.Select(mr => mr.Name).AssertContainsExactly("rule1");
            policyOne.MatchingRules.Select(mr => mr.TypeName).AssertContainsExactly("AlwaysMatchingRule");

            policyOne.CallHandlers.Select(ch => ch.Name).AssertContainsExactly("handler1");
            policyOne.CallHandlers.Select(ch => ch.TypeName).AssertContainsExactly("GlobalCountCallHandler");
        }

        [Fact]
        public void Then_CanLoadPolicyWithMultipleHandlers()
        {
            var interceptionElement = this.GetInterceptionElement("policyWithExternallyConfiguredRulesAndHandlers");
            var policyOne = interceptionElement.Policies["policyOne"];

            policyOne.MatchingRules.Select(mr => mr.Name).AssertContainsExactly("rule1");
            policyOne.MatchingRules.Select(mr => mr.TypeName).AssertContainsExactly(String.Empty);

            policyOne.CallHandlers.Select(ch => ch.Name).AssertContainsExactly("handler1", "handler2");
            policyOne.CallHandlers.Select(ch => ch.TypeName).AssertContainsExactly(String.Empty, String.Empty);
        }
    }
}
