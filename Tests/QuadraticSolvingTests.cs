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
    }
}
