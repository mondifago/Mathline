using Core.Models;
using Core.Solving;

namespace Tests
{
    public class LinearSolvingTests
    {
        [Theory]
        [InlineData(2, 6, 18, 6)]    // 2x + 6 = 18  -> x = 6
        [InlineData(2, -4, 10, 7)]   // 2x - 4 = 10  -> x = 7
        [InlineData(2, -4, 0, 2)]    // 2x - 4 = 0   -> x = 2
        [InlineData(2, 0, 5, 2.5)]   // 2x = 5       -> x = 2.5
        public void SolvesForX(double a, double b, double c, double expectedX)
        {
            var equation = new LinearEquation { A = a, B = b, C = c };

            var result = SolvingLogic.SolveLinearEquation(equation);

            Assert.Equal(expectedX, result.Solution.X, 10);
        }

        [Fact]
        public void ZeroCoefficient_Throws()
        {
            var equation = new LinearEquation { A = 0, B = 6, C = 18 };

            Assert.Throws<ArgumentException>(() => SolvingLogic.SolveLinearEquation(equation));
        }

        [Fact]
        public void ShowsEveryStep_NoneHidden()
        {
            var equation = new LinearEquation { A = 2, B = 6, C = 18 };

            var result = SolvingLogic.SolveLinearEquation(equation);

            var expressions = result.Steps.Select(s => s.Expression).ToList();

            Assert.Equal(new[]
            {
                "2x + 6 = 18",
                "2x = 18 - 6",
                "2x = 12",
                "x = 12/2",
                "x = 6",
            }, expressions);
        }
    }
}
