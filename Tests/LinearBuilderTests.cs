using Core.Models;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class LinearBuilderTests
    {
        private static LinearEquation Build(string input) =>
            LinearBuilder.Build(TermCollector.Collect(Tokenizer.Tokenize(input)));

        [Theory]
        [InlineData("3x + 5 = 14", 3, 5, 14)]
        [InlineData("2x = -6", 2, 0, -6)]
        [InlineData("x - 3 = 0", 1, -3, 0)]
        [InlineData("-4x + 1 = 9", -4, 1, 9)]
        public void BuildsCoefficients(string input, double a, double b, double c)
        {
            var eq = Build(input);

            Assert.Equal(a, eq.A);
            Assert.Equal(b, eq.B);
            Assert.Equal(c, eq.C);
        }

        [Fact]
        public void NetsVariablesAcrossSides()
        {
            var eq = Build("3x + 5 = x + 14");

            Assert.Equal(2, eq.A);   // 3 - 1
            Assert.Equal(5, eq.B);
            Assert.Equal(14, eq.C);
        }
    }
}
