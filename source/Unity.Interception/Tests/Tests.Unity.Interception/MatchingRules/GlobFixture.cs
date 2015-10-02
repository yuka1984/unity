// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests.MatchingRules
{
    /// <summary>
    /// Test figure for the Glob class, which implements matching using
    /// file system style patterns for simple pattern matches rather than
    /// full-bore regexes.
    /// </summary>
     
    public class GlobFixture
    {
        [Fact]
        public void ShouldMatchExactlyWhenNoWildcards()
        {
            Glob glob = new Glob("MyClass");
            Assert.True(glob.IsMatch("MyClass"));
            Assert.False(glob.IsMatch("MyClass2"));
            Assert.False(glob.IsMatch("ReallyMyClass"));
        }

        [Fact]
        public void ShouldMatchWithTrailingWildcard()
        {
            Glob glob = new Glob("MyClass*");
            Assert.True(glob.IsMatch("MyClass"));
            Assert.True(glob.IsMatch("MyClassAndMore2"));
            Assert.False(glob.IsMatch("ReallyMyClass"));
        }

        [Fact]
        public void ShouldMatchWithLeadingWildcard()
        {
            Glob glob = new Glob("*Class");
            Assert.True(glob.IsMatch("MyClass"));
            Assert.True(glob.IsMatch("My.other.Class"));
            Assert.False(glob.IsMatch("MyClassAndMore2"));
        }

        [Fact]
        public void ShouldBeCaseSensitiveByDefault()
        {
            Glob glob = new Glob("MyClass");
            Assert.False(glob.IsMatch("myclass"));
        }

        [Fact]
        public void ShouldMatchWithCaseInsensitiveFlag()
        {
            Glob glob = new Glob("MyClass", false);

            Assert.True(glob.IsMatch("myclass"));
        }

        [Fact]
        public void DotsShouldBeLiterals()
        {
            Glob glob = new Glob("*.cs");

            Assert.True(glob.IsMatch("someFile.cs"));
            Assert.False(glob.IsMatch("notmatchedcs"));
        }

        [Fact]
        public void BracketsShouldMatchSingleCharacters()
        {
            Glob glob = new Glob("Test[0-9][0-9]");
            Assert.True(glob.IsMatch("Test01"));
            Assert.True(glob.IsMatch("Test54"));
            Assert.False(glob.IsMatch("Test200"));
        }

        [Fact]
        public void QuestionMarksShouldMatchSingleCharacters()
        {
            Glob glob = new Glob("one??two");

            Assert.True(glob.IsMatch("one00two"));
            Assert.True(glob.IsMatch("oneWEtwo"));
            Assert.False(glob.IsMatch("oneTooManytwo"));
            Assert.True(glob.IsMatch("one??two"));
        }

        [Fact]
        public void DollarSignsAndCaretsAreLiterals()
        {
            Glob glob = new Glob("abc$def^*");
            Assert.True(glob.IsMatch("abc$def^"));
            Assert.True(glob.IsMatch("abc$def^Stuff"));
            Assert.False(glob.IsMatch("abc$"));
        }

        [Fact]
        public void ShouldMatchNothingWithEmptyPattern()
        {
            Glob glob = new Glob(string.Empty);
            Assert.False(glob.IsMatch("a"));
        }
    }
}
