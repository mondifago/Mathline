using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class TermCollectorTests
    {
        private static EquationTerms Collect(string input) =>
            TermCollector.Collect(Tokenizer.Tokenize(input));

        [Fact]
        public void CollectsSimpleLinear()
        {
            var terms = Collect("3x + 5 = 14");

            Assert.Equal(3, terms.Left.GetValueOrDefault("x"));
            Assert.Equal(5, terms.Left.GetValueOrDefault("1"));
            Assert.Equal(14, terms.Right.GetValueOrDefault("1"));
        }

        [Fact]
        public void HandlesLeadingMinusAndSubtraction()
        {
            var terms = Collect("-4x + 1 = 9");

            Assert.Equal(-4, terms.Left.GetValueOrDefault("x"));
            Assert.Equal(1, terms.Left.GetValueOrDefault("1"));
            Assert.Equal(9, terms.Right.GetValueOrDefault("1"));
        }

        [Fact]
        public void BareVariableHasCoefficientOne()
        {
            var terms = Collect("x - 3 = 0");

            Assert.Equal(1, terms.Left.GetValueOrDefault("x"));
            Assert.Equal(-3, terms.Left.GetValueOrDefault("1"));
        }

        [Fact]
        public void CombinesLikeTerms()
        {
            var terms = Collect("2x + 3x - 1 = 5 + 5");

            Assert.Equal(5, terms.Left.GetValueOrDefault("x"));
            Assert.Equal(-1, terms.Left.GetValueOrDefault("1"));
            Assert.Equal(10, terms.Right.GetValueOrDefault("1"));
        }

        [Fact]
        public void KeepsTermsOnBothSides()
        {
            var terms = Collect("3x + 5 = x + 14");

            Assert.Equal(3, terms.Left.GetValueOrDefault("x"));
            Assert.Equal(5, terms.Left.GetValueOrDefault("1"));
            Assert.Equal(1, terms.Right.GetValueOrDefault("x"));
            Assert.Equal(14, terms.Right.GetValueOrDefault("1"));
        }

        [Theory]
        [InlineData("3x + 5")]      // no '='
        [InlineData("x = 5 = 3")]   // two '='
        [InlineData("3x 5 = 1")]    // missing operator
        [InlineData("3x + = 1")]    // trailing operator
        [InlineData("= 5")]         // empty side
        public void RejectsMalformedInput(string input)
        {
            Assert.Throws<FormatException>(() => Collect(input));
        }
    }
}
