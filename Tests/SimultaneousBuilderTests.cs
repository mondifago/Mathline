using Core.Models;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class SimultaneousBuilderTests
    {
        private static EquationTerms Terms(string input) =>
            TermCollector.Collect(Tokenizer.Tokenize(input));

        private static SimultaneousEquation Build(string first, string second) =>
            SimultaneousBuilder.Build(Terms(first), Terms(second));

        [Fact]
        public void BuildsStandardForm()
        {
            var eq = Build("2x + y = 7", "x - y = 2");

            Assert.Equal(2, eq.A1);
            Assert.Equal(1, eq.B1);
            Assert.Equal(7, eq.C1);
            Assert.Equal(1, eq.A2);
            Assert.Equal(-1, eq.B2);
            Assert.Equal(2, eq.C2);
        }

        [Fact]
        public void NormalizesConstantOnLeft()
        {
            // 2x + y - 3 = 4  →  2x + y = 7
            var eq = Build("2x + y - 3 = 4", "x - y = 2");
            Assert.Equal(2, eq.A1);
            Assert.Equal(1, eq.B1);
            Assert.Equal(7, eq.C1);
        }

        [Fact]
        public void NormalizesVariableOnRight()
        {
            // 2x = 7 - y  →  2x + y = 7
            var eq = Build("2x = 7 - y", "x - y = 2");
            Assert.Equal(2, eq.A1);
            Assert.Equal(1, eq.B1);
            Assert.Equal(7, eq.C1);
        }

        [Fact]
        public void MissingVariableIsZeroCoefficient()
        {
            // 3x = 6 has no y term
            var eq = Build("3x = 6", "x + y = 4");
            Assert.Equal(3, eq.A1);
            Assert.Equal(0, eq.B1);
            Assert.Equal(6, eq.C1);
            Assert.Equal(1, eq.A2);
            Assert.Equal(1, eq.B2);
            Assert.Equal(4, eq.C2);
        }
    }
}
