// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.Practices.Unity.TestSupport.Configuration;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_SerializingPolicies
    /// </summary>
     
    public class When_SerializingPolicies : SerializationFixture
    {
        [Fact]
        public void Then_EmptyPoliciesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("EmptyPolicies.config", c =>
            {
                c.ConfiguringElements.Add(new InterceptionElement());
            });

            var policies = loadedConfig.Containers.Default.ConfiguringElements[0] as InterceptionElement;
            Assert.NotNull(policies);
            Assert.Equal(0, policies.Policies.Count);
        }

        [Fact]
        public void Then_PoliciesWithNoContentsAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("EmptyPolicies.config", c =>
            {
                var interceptionElement = new InterceptionElement();
                interceptionElement.Policies.Add(new PolicyElement()
                {
                    Name = "Policy1"
                });
                interceptionElement.Policies.Add(new PolicyElement()
                {
                    Name = "Policy2"
                });
                c.ConfiguringElements.Add(interceptionElement);
            });

            var policies = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];

            policies.Policies.Select(p => p.Name)
                .AssertContainsExactly("Policy1", "Policy2");
        }

        [Fact]
        public void Then_MatchingRuleNamesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("MatchingRules.config", CreateConfigWithMatchingRules);

            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];
            interception.Policies[0].MatchingRules.Select(mr => mr.Name)
                .AssertContainsExactly("NameOnly", "NameAndType", "RuleWithLifetime", "RuleWithElements");
        }

        [Fact]
        public void Then_MatchingRuleTypeNamesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("MatchingRules.config", CreateConfigWithMatchingRules);
            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];
            interception.Policies[0].MatchingRules.Select(mr => mr.TypeName)
                .AssertContainsExactly(String.Empty, "AlwaysMatchingRule", "AlwaysMatchingRule", "AlwaysMatchingRule");
        }

        [Fact]
        public void Then_MatchingRuleLifetimesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("MatchingRules.config", CreateConfigWithMatchingRules);
            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];

            var lifetime = interception.Policies[0].MatchingRules
                .Where(mr => mr.Name == "RuleWithLifetime")
                .Select(mr => mr.Lifetime)
                .First();

            Assert.Equal("singleton", lifetime.TypeName);
        }

        [Fact]
        public void Then_MatchingRuleInjectionMembersAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("MatchingRules.config", CreateConfigWithMatchingRules);
            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];

            var injectionMembers = interception.Policies[0].MatchingRules
                .Where(mr => mr.Name == "RuleWithElements")
                .First().Injection;

            injectionMembers.Select(m => m.GetType())
                .AssertContainsExactly(typeof(ConstructorElement), typeof(PropertyElement));

            Assert.Equal("MyProp", injectionMembers.OfType<PropertyElement>().First().Name);
        }

        [Fact]
        public void Then_CallHandlerNamesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("CallHandlers.config", CreateConfigWithCallHandlers);

            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];
            interception.Policies[0].CallHandlers.Select(ch => ch.Name)
                .AssertContainsExactly("NamedRule", "NameAndType", "HandlerWithLifetime", "HandlerWithElements");
        }

        [Fact]
        public void Then_CallHandlerTypeNamesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("CallHandlers.config", CreateConfigWithCallHandlers);
            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];

            interception.Policies[0].CallHandlers.Select(ch => ch.TypeName)
                .AssertContainsExactly(String.Empty, "DoMoreRule", "DoSomethingRule", "CallCountHandler");
        }

        [Fact]
        public void Then_CallHandlerLifetimesAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("CallHandlers.config", CreateConfigWithCallHandlers);
            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];

            var lifetime = interception.Policies[0].CallHandlers
                .Where(ch => ch.Name == "HandlerWithLifetime")
                .Select(ch => ch.Lifetime)
                .First();

            Assert.Equal("singleton", lifetime.TypeName);
        }

        [Fact]
        public void Then_CallHandlerInjectionMembersAreSerialized()
        {
            var loadedConfig = SerializeAndLoadConfig("CallHandlers.config", CreateConfigWithCallHandlers);
            var interception = (InterceptionElement)loadedConfig.Containers.Default.ConfiguringElements[0];

            var injectionMembers = interception.Policies[0].CallHandlers
                .Where(ch => ch.Name == "HandlerWithElements")
                .First().Injection;

            injectionMembers.Select(m => m.GetType())
                .AssertContainsExactly(typeof(ConstructorElement), typeof(PropertyElement));

            Assert.Equal("MyProp", injectionMembers.OfType<PropertyElement>().First().Name);
        }

        private static void CreateConfigWithMatchingRules(ContainerElement c)
        {
            var interceptionElement = new InterceptionElement();
            var policy = new PolicyElement() { Name = "PolicyOne" };
            policy.MatchingRules.Add(new MatchingRuleElement()
            {
                Name = "NameOnly"
            });

            policy.MatchingRules.Add(new MatchingRuleElement()
            {
                Name = "NameAndType",
                TypeName = "AlwaysMatchingRule"
            });

            policy.MatchingRules.Add(new MatchingRuleElement
            {
                Name = "RuleWithLifetime",
                TypeName = "AlwaysMatchingRule",
                Lifetime = new LifetimeElement()
                {
                    TypeName = "singleton"
                }
            });

            var ruleWithMembers = new MatchingRuleElement
            {
                Name = "RuleWithElements",
                TypeName = "AlwaysMatchingRule"
            };
            ruleWithMembers.Injection.Add(new ConstructorElement());
            ruleWithMembers.Injection.Add(new PropertyElement() { Name = "MyProp" });

            policy.MatchingRules.Add(ruleWithMembers);

            interceptionElement.Policies.Add(policy);

            c.ConfiguringElements.Add(interceptionElement);
        }

        private static void CreateConfigWithCallHandlers(ContainerElement c)
        {
            var interceptionElement = new InterceptionElement();
            var policy = new PolicyElement() { Name = "PolicyOne" };
            policy.MatchingRules.Add(new MatchingRuleElement() { Name = "All", TypeName = "AlwaysMatchingRule" });

            policy.CallHandlers.Add(new CallHandlerElement()
            {
                Name = "NamedRule"
            });

            policy.CallHandlers.Add(new CallHandlerElement()
            {
                Name = "NameAndType",
                TypeName = "DoMoreRule"
            });

            policy.CallHandlers.Add(new CallHandlerElement()
            {
                Name = "HandlerWithLifetime",
                TypeName = "DoSomethingRule",
                Lifetime = new LifetimeElement()
                {
                    TypeName = "singleton"
                }
            });

            var handlerWithMembers = new CallHandlerElement()
            {
                Name = "HandlerWithElements",
                TypeName = "CallCountHandler"
            };
            handlerWithMembers.Injection.Add(new ConstructorElement());
            handlerWithMembers.Injection.Add(new PropertyElement() { Name = "MyProp" });

            policy.CallHandlers.Add(handlerWithMembers);

            interceptionElement.Policies.Add(policy);

            c.ConfiguringElements.Add(interceptionElement);
        }
    }
}
