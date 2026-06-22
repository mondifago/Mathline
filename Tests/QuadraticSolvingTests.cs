using Core.Models;
using Core.Solving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class QuadraticSolvingTests
    {
        [Fact]
        public void TwoDistinctRealRoots()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = -5, C = 6 });

            Assert.Equal(QuadraticRootType.TwoDistinct, result.Solution.RootType);
            Assert.Equal(3, result.Solution.X1, 10);
            Assert.Equal(2, result.Solution.X2, 10);
        }

        [Fact]
        public void RepeatedRoot()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = -4, C = 4 });

            Assert.Equal(QuadraticRootType.Repeated, result.Solution.RootType);
            Assert.Equal(2, result.Solution.X1, 10);
            Assert.Equal(2, result.Solution.X2, 10);
        }

        [Fact]
        public void NoRealRoots()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = 1, C = 1 });

            Assert.Equal(QuadraticRootType.NoRealRoots, result.Solution.RootType);
            Assert.True(double.IsNaN(result.Solution.X1));
            Assert.True(double.IsNaN(result.Solution.X2));
        }

        [Fact]
        public void ZeroLeadingCoefficient_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 0, B = 2, C = 1 }));
        }

        [Fact]
        public void PerfectSquareDiscriminant_UsesFactorisation()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = -5, C = 6 });
            Assert.Equal("Factorisation", result.Method);
        }

        [Fact]
        public void NonPerfectSquareDiscriminant_UsesFormula()
        {
            // x² - 3x + 1 = 0, discriminant 5 → irrational roots
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = -3, C = 1 });
            Assert.Equal("Quadratic formula", result.Method);
        }

        [Fact]
        public void NoRealRoots_UsesFormula()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = 1, C = 1 });
            Assert.Equal("Quadratic formula", result.Method);
        }

        [Fact]
        public void Factorisation_ShowsFactoredForm()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = -5, C = 6 });
            var expressions = result.Steps.Select(s => s.Expression).ToList();
            Assert.Contains("(x - 3)(x - 2) = 0", expressions);
        }

        [Fact]
        public void Factorisation_RepeatedRoot_ShowsSquaredFactor()
        {
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 1, B = -4, C = 4 });
            Assert.Equal("Factorisation", result.Method);
            Assert.Contains("(x - 2)² = 0", result.Steps.Select(s => s.Expression));
        }

        [Fact]
        public void Factorisation_NonMonic_ShowsDivideThroughStep()
        {
            // 2x² - 10x + 12 = 0  →  ÷2  →  x² - 5x + 6 = 0  →  (x - 3)(x - 2) = 0
            var result = SolvingLogic.SolveQuadraticEquation(new QuadraticEquation { A = 2, B = -10, C = 12 });
            var expressions = result.Steps.Select(s => s.Expression).ToList();
            Assert.Equal("Factorisation", result.Method);
            Assert.Contains("x² - 5x + 6 = 0", expressions);
            Assert.Contains("(x - 3)(x - 2) = 0", expressions);
        }
    }
}
