// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Unity.TestSupport;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Unity.Tests
{
    public partial class AssemblyScanningFixture
    {
        [Fact]
        public void GetsTypesFromLoadedAssembliesExcludingSystemAndUnityByDefault()
        {
            // ensure type is loaded
            var ignore = new Unity.Tests.TestNetAssembly.PublicClass1();

            var typesByAssembly = AllClasses.FromLoadedAssemblies().GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.False(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.False(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly));
        }

        [Fact]
        public void GetsTypesFromLoadedAssembliesIncludingSystemIfOverridden()
        {
            // ensure type is loaded
            var ignore = new Unity.Tests.TestNetAssembly.PublicClass1();

            var typesByAssembly = AllClasses.FromLoadedAssemblies(includeSystemAssemblies: true).GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.True(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.False(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly));
        }

        [Fact]
        public void GetsTypesFromLoadedAssembliesIncludingUnityIfOverridden()
        {
            // ensure type is loaded
            var ignore = new Unity.Tests.TestNetAssembly.PublicClass1();

            var typesByAssembly = AllClasses.FromLoadedAssemblies(includeUnityAssemblies: true).GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.False(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly));
        }

        [Fact]
        public void GetsTypesFromLoadedAssembliesIncludingUnityAndSystemIfOverridden()
        {
            // ensure type is loaded
            var ignore = new Unity.Tests.TestNetAssembly.PublicClass1();

            var typesByAssembly = AllClasses.FromLoadedAssemblies(includeSystemAssemblies: true, includeUnityAssemblies: true).GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.True(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly));
        }

        [Fact]
        public void GettingTypesFromLoadedAssembliesWithErrorsThrows()
        {
            // ensure type is loaded
            var ignore = new Unity.Tests.TestNetAssembly.PublicClass1();

            try
            {
                var types = AllClasses.FromLoadedAssemblies(skipOnError: false).ToArray();
                Assert.True(false, string.Format("should have thrown"));
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }

        [Fact]
        //[Priority(-1)]
        public void GetsTypesFromAssembliesLoadedFromBaseFolderExcludingSystemAndUnityByDefault()
        {
            if (!File.Exists("NotReferencedTestNetAssembly.dll"))
            {
                Assert.True(false, "The assembly was not found. Run this test without deployment");
            }

            var typesByAssembly = AllClasses.FromAssembliesInBasePath().GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.False(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.False(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.Any(g => g.Key.GetName().Name == "NotReferencedTestNetAssembly"), "No types for NotReferencedTestNetAssembly");
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly), "No types for TestNetAssembly");
        }

        [Fact]
        //[Priority(-1)]
        public void GetsTypesFromAssembliesLoadedFromBaseFolderIncludingSystemIfOverridden()
        {
            if (!File.Exists("NotReferencedTestNetAssembly.dll"))
            {
                Assert.True(false, "The assembly was not found. Run this test without deployment");
            }

            var typesByAssembly = AllClasses.FromAssembliesInBasePath(includeSystemAssemblies: true).GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.True(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.False(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.Any(g => g.Key.GetName().Name == "NotReferencedTestNetAssembly"), "No types for NotReferencedTestNetAssembly");
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly), "No types for TestNetAssembly");
        }

        [Fact]
        //[Priority(-1)]
        public void GetsTypesFromAssembliesLoadedFromBaseFolderIncludingUnityIfOverridden()
        {
            if (!File.Exists("NotReferencedTestNetAssembly.dll"))
            {
                Assert.True(false, "The assembly was not found. Run this test without deployment");
            }

            var typesByAssembly = AllClasses.FromAssembliesInBasePath(includeUnityAssemblies: true).GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.False(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.Any(g => g.Key.GetName().Name == "NotReferencedTestNetAssembly"), "No types for NotReferencedTestNetAssembly");
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly), "No types for TestNetAssembly");
        }

        [Fact]
        //[Priority(-1)]
        public void GetsTypesFromAssembliesLoadedFromBaseFolderIncludingSystemAndUnityIfOverridden()
        {
            if (!File.Exists("NotReferencedTestNetAssembly.dll"))
            {
                Assert.True(false, "The assembly was not found. Run this test without deployment");
            }

            var typesByAssembly = AllClasses.FromAssembliesInBasePath(includeSystemAssemblies: true, includeUnityAssemblies: true).GroupBy(t => t.Assembly).ToDictionary(g => g.Key);

            Assert.True(typesByAssembly.ContainsKey(typeof(Uri).Assembly));
            Assert.True(typesByAssembly.ContainsKey(typeof(IUnityContainer).Assembly));
            Assert.True(typesByAssembly.Any(g => g.Key.GetName().Name == "NotReferencedTestNetAssembly"), "No types for NotReferencedTestNetAssembly");
            Assert.True(typesByAssembly.ContainsKey(typeof(Unity.Tests.TestNetAssembly.PublicClass1).Assembly), "No types for TestNetAssembly");
        }

        [Fact]
        public void GettingTypesFromAssembliesLoadedFromBaseFolderWithErrorsThrows()
        {
            try
            {
                var types = AllClasses.FromAssembliesInBasePath(skipOnError: false).ToArray();
            }
            catch (Exception e)
            {
                if (e is AssertFailedException)
                {
                    throw;
                }
            }
        }
    }
}
