// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
    /// <summary>
    /// Summary description for When_ResolvingTypes
    /// </summary>
     
    public class When_ResolvingTypes
    {
        private TypeResolverImpl typeResolver;

        public When_ResolvingTypes()
        {
            var aliases = new Dictionary<string, string>
                {
                    { "dict", typeof(Dictionary<,>).AssemblyQualifiedName },
                    { "ILogger", "Microsoft.Practices.Unity.TestSupport.ILogger, Unity.TestSupport" },
                    { "MockLogger", "Microsoft.Practices.Unity.TestSupport.MockLogger, Unity.TestSupport" }
                };

            var namespaces = new[] { "System", "System.Collections.Generic", "Microsoft.Practices.Unity.TestSupport" };
            var assemblies = new[] { "System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "Unity.TestSupport", "an invalid assembly name", "invalid, invalid" };

            typeResolver = new TypeResolverImpl(aliases, namespaces, assemblies);
        }

        [Fact]
        public void Then_DefaultAliasesResolve()
        {
            var expected = new Dictionary<string, Type>
                {
                    { "sbyte", typeof(sbyte) },
                    { "short", typeof(short) },
                    { "int", typeof(int) },
                    { "integer", typeof(int) },
                    { "long", typeof(long) },
                    { "byte", typeof(byte) },
                    { "ushort", typeof(ushort) },
                    { "uint", typeof(uint) },
                    { "ulong", typeof(ulong) },
                    { "float", typeof(float) },
                    { "single", typeof(float) },
                    { "double", typeof(double) },
                    { "decimal", typeof(decimal) },
                    { "char", typeof(char) },
                    { "bool", typeof(bool) },
                    { "object", typeof(object) },
                    { "string", typeof(string) },
                    { "datetime", typeof(DateTime) },
                    { "DateTime", typeof(DateTime) },
                    { "date", typeof(DateTime) },
                    { "singleton", typeof(ContainerControlledLifetimeManager) },
                    { "ContainerControlledLifetimeManager", typeof(ContainerControlledLifetimeManager) },
                    { "transient", typeof(TransientLifetimeManager) },
                    { "TransientLifetimeManager", typeof(TransientLifetimeManager) },
                    { "perthread", typeof(PerThreadLifetimeManager) },
                    { "PerThreadLifetimeManager", typeof(PerThreadLifetimeManager) },
                    { "external", typeof(ExternallyControlledLifetimeManager) },
                    { "ExternallyControlledLifetimeManager", typeof(ExternallyControlledLifetimeManager) },
                    { "hierarchical", typeof(HierarchicalLifetimeManager) },
                    { "HierarchicalLifetimeManager", typeof(HierarchicalLifetimeManager) },
                    { "resolve", typeof(PerResolveLifetimeManager) },
                    { "perresolve", typeof(PerResolveLifetimeManager) },
                    { "PerResolveLifetimeManager", typeof(PerResolveLifetimeManager) }
                };

            foreach (var kv in expected)
            {
                Assert.Same(kv.Value, typeResolver.ResolveType(kv.Key, true));
            }
        }

        [Fact]
        public void Then_ILoggerResolves()
        {
            Assert.Same(typeResolver.ResolveType("ILogger", true), typeof(ILogger));
        }

        [Fact]
        public void Then_GuidIsFoundThroughSearch()
        {
            Assert.Same(typeResolver.ResolveType("Guid", true), typeof(Guid));
        }

        [Fact]
        public void Then_UriIsFoundThroughSearch()
        {
            Assert.Same(typeResolver.ResolveType("Uri", true), typeof(Uri));
        }

        [Fact]
        public void Then_OpenGenericIsResolvedThroughSearch()
        {
            Assert.Same(typeResolver.ResolveType("Dictionary`2", true), typeof(Dictionary<,>));
        }

        [Fact]
        public void Then_OpenGenericShorthandIsResolvedThroughSearch()
        {
            Assert.Same(typeResolver.ResolveType("Dictionary[,]", true), typeof(Dictionary<,>));
        }

        [Fact]
        public void Then_ShorthandForOpenGenericWithOneParameterWorks()
        {
            Assert.Same(typeResolver.ResolveType("List[]", true), typeof(List<>));
        }

        [Fact]
        public void Then_ShorthandGenericIsResolved()
        {
            Assert.Same(typeResolver.ResolveType("List[int]", true), typeof(List<int>));
        }

        [Fact]
        public void Then_ShorthandWithMultipleParametersIsResolved()
        {
            Assert.Same(typeResolver.ResolveType("Func[int, string]", true), typeof(Func<int, string>));
        }

        [Fact]
        public void Then_ShorthandWithLeadingAliasIsResolved()
        {
            Assert.Same(typeResolver.ResolveType("dict[string, datetime]", true),
                typeof(Dictionary<string, DateTime>));
        }

        [Fact]
        public void Then_TypeThatCannotBeFoundReturnsNull()
        {
            Assert.Null(typeResolver.ResolveType("Namespace.Type, Assembly", false));
        }
    }
}
