using Core;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class CoordinatorTests
    {
        [Fact]
        public void RoutesLinear()
        {
            var outcome = Coordinator.Solve("3x + 5 = 14");
            Assert.Equal(EquationType.Linear, outcome.Type);
            Assert.Equal("Rearrangement", outcome.Method);
        }

        [Fact]
        public void RoutesQuadratic()
        {
            var outcome = Coordinator.Solve("x^2 - 5x + 6 = 0");
            Assert.Equal(EquationType.Quadratic, outcome.Type);
        }

        [Fact]
        public void ZeroSquaredCoefficientRoutesToLinear()
        {
            var outcome = Coordinator.Solve("0x^2 + 3x = 5");
            Assert.Equal(EquationType.Linear, outcome.Type);
        }

        [Fact]
        public void PropagatesParseErrors()
        {
            Assert.Throws<FormatException>(() => Coordinator.Solve("x + y = 5"));
        }
    }
}
