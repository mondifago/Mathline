using Core.Models;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class QuadraticBuilderTests
    {
        private static QuadraticEquation Build(string input) =>
            QuadraticBuilder.Build(TermCollector.Collect(Tokenizer.Tokenize(input)));

        [Theory]
        [InlineData("x^2 - 5x + 6 = 0", 1, -5, 6)]
        [InlineData("3x^2 = 12", 3, 0, -12)]
        [InlineData("x^2 = 9", 1, 0, -9)]
        [InlineData("-x^2 + 4 = 0", -1, 0, 4)]
        public void BuildsCoefficients(string input, double a, double b, double c)
        {
            var eq = Build(input);

            Assert.Equal(a, eq.A);
            Assert.Equal(b, eq.B);
            Assert.Equal(c, eq.C);
        }

        [Fact]
        public void NetsAcrossSides()
        {
            // 2x² + 3x = x² + 5  →  x² + 3x - 5 = 0
            var eq = Build("2x^2 + 3x = x^2 + 5");

            Assert.Equal(1, eq.A);
            Assert.Equal(3, eq.B);
            Assert.Equal(-5, eq.C);
        }
    }
}
