// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit;

[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1403:FileMayOnlyContainASingleNamespace", Justification = "Test needs multiple namespaces so keep the namespaces and test together")]

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
     
    public class TypeMatchingRuleFixture
    {
        [Fact]
        public void ShouldMatchExactClassName()
        {
            TestMatch("MyType1", typeof(MyType1), true);
        }

        [Fact]
        public void ShouldNotMatchWithDifferentClassName()
        {
            TestMatch("MyType1", typeof(MyType2), false);
        }

        [Fact]
        public void ShouldMatchInDifferentNamespaces()
        {
            TestMatch("MyType1", typeof(ANestedNamespace.MyType1), true);
        }

        [Fact]
        public void ShouldMatchCaseInsensitive()
        {
            TestMatch("mytype2", typeof(MyType2), true, true);
        }

        [Fact]
        public void ShouldMatchWithFullTypeName()
        {
            TestMatch(
                typeof(MyType1).FullName,
                typeof(MyType1), false, true);
        }

        [Fact]
        public void ShouldNotMatchFullNameWithDifferentNamespace()
        {
            TestMatch(typeof(MyType1).FullName, typeof(ANestedNamespace.MyType1), false);
        }

        [Fact]
        public void ShouldMatchOneOfMultipleMatchOptions()
        {
            IMatchingRule rule = new TypeMatchingRule(new MatchingInfo[]
                                                          {
                                                              new MatchingInfo(typeof(MyType1).FullName),
                                                              new MatchingInfo("MYTYPE2", true)
                                                          });
            Assert.True(rule.Matches(typeof(MyType1).GetMethod("TargetMethod")));
            Assert.False(rule.Matches(typeof(ANestedNamespace.MyType1).GetMethod("TargetMethod")));
            Assert.True(rule.Matches(typeof(MyType2).GetMethod("TargetMethod")));
        }

        [Fact]
        public void ShouldNotMatchTypeIfItImplementsMatchingInterface()
        {
            TestMatch(typeof(IInterfaceOne).FullName, typeof(ANestedNamespace.MyType1), false);
            TestMatch(typeof(IInterfaceOne).FullName, typeof(MyType1), false);
        }

        public void TestMatch(string typeName,
                              Type typeToMatch,
                              bool ignoreCase,
                              bool shouldMatch)
        {
            TypeMatchingRule rule = new TypeMatchingRule(typeName, ignoreCase);
            MethodInfo methodToMatch = typeToMatch.GetMethod("TargetMethod");
            Assert.Equal(shouldMatch, rule.Matches(methodToMatch));
        }

        public void TestMatch(string typeName,
                              Type typeToMatch,
                              bool shouldMatch)
        {
            TestMatch(typeName, typeToMatch, false, shouldMatch);
        }
    }

    internal class MyType1
    {
        public void TargetMethod() { }
    }

    namespace ANestedNamespace
    {
        internal class MyType1 : IInterfaceOne
        {
            public void TargetMethod() { }
        }
    }

    internal class MyType2 : IAnotherInterface
    {
        public void ADifferentTargetMethod() { }

        public void TargetMethod() { }
    }

    public interface IInterfaceOne
    {
        void TargetMethod();
    }

    internal interface IAnotherInterface
    {
        void ADifferentTargetMethod();
    }
}
