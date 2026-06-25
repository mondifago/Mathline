using Core.Coordination;
using Core.Models;
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

        [Fact]
        public void RoutesSimultaneous()
        {
            var outcome = Coordinator.Solve("2x + y = 7\nx - y = 2");
            Assert.Equal(EquationType.Simultaneous, outcome.Type);
            Assert.Equal("Elimination", outcome.Method);
        }

        [Fact]
        public void SimultaneousProducesSolutionStep()
        {
            var outcome = Coordinator.Solve("2x + y = 7\nx - y = 2");
            Assert.Contains("x = 3, y = 1", outcome.Steps.Select(s => s.Expression));
        }

        [Fact]
        public void ThreeEquationsRejected()
        {
            Assert.Throws<FormatException>(() => Coordinator.Solve("x = 1\ny = 2\nz = 3"));
        }

        [Fact]
        public void EmptyInputRejected()
        {
            Assert.Throws<FormatException>(() => Coordinator.Solve("   "));
        }
    }
}
