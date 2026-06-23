using Core.Models;
using Core.Solving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class SimultaneousSolvingTests
    {
        private static SimultaneousEquationSolution Solve(
            double a1, double b1, double c1, double a2, double b2, double c2) =>
            SolvingLogic.SolveSimultaneousEquation(
                new SimultaneousEquation { A1 = a1, B1 = b1, C1 = c1, A2 = a2, B2 = b2, C2 = c2 }).Solution;

        [Fact]
        public void UniqueSolution()
        {
            var s = Solve(2, 1, 7, 1, -1, 2);     // 2x + y = 7 ; x - y = 2
            Assert.Equal(SimultaneousSolutionType.Unique, s.SolutionType);
            Assert.Equal(3, s.X, 10);
            Assert.Equal(1, s.Y, 10);
        }

        [Fact]
        public void AnotherUniqueSolution()
        {
            var s = Solve(1, 1, 10, 1, -1, 2);    // x + y = 10 ; x - y = 2
            Assert.Equal(6, s.X, 10);
            Assert.Equal(4, s.Y, 10);
        }

        [Fact]
        public void EliminatesYWhenXIsMissing()
        {
            var s = Solve(0, 1, 3, 1, 1, 5);      // y = 3 ; x + y = 5
            Assert.Equal(SimultaneousSolutionType.Unique, s.SolutionType);
            Assert.Equal(2, s.X, 10);
            Assert.Equal(3, s.Y, 10);
        }

        [Fact]
        public void DecoupledSystem()
        {
            var s = Solve(3, 0, 6, 0, 2, 4);      // 3x = 6 ; 2y = 4
            Assert.Equal(2, s.X, 10);
            Assert.Equal(2, s.Y, 10);
        }

        [Fact]
        public void NoSolution()
        {
            var s = Solve(1, 1, 2, 2, 2, 5);      // x + y = 2 ; 2x + 2y = 5
            Assert.Equal(SimultaneousSolutionType.NoSolution, s.SolutionType);
        }

        [Fact]
        public void InfinitelyManySolutions()
        {
            var s = Solve(1, 1, 2, 2, 2, 4);      // x + y = 2 ; 2x + 2y = 4
            Assert.Equal(SimultaneousSolutionType.InfinitelyMany, s.SolutionType);
        }
    }
}
