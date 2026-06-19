using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Solving
{
    public class SolvingLogic
    {
        #region Linear Equations
        public static SolveResult<LinearEquationSolution> SolveLinearEquation(LinearEquation equation)
        {
            if (equation.A == 0)
                throw new ArgumentException(
                    "Not a linear equation: the coefficient of x is zero.", nameof(equation));

            double a = equation.A;
            double b = equation.B;
            double c = equation.C;

            double rightHandSide = c - b;   // ax + b = c  ->  ax = c - b
            double x = rightHandSide / a;   // x = (c - b) / a

            return new SolveResult<LinearEquationSolution>
            {
                Solution = new LinearEquationSolution { X = x },
                Steps = BuildLinearSteps(a, b, c, rightHandSide, x),
                Method = "Rearrangement"
            };
        }

        private static List<SolutionStep> BuildLinearSteps(double a, double b, double c, double rightHandSide, double x)
        {
            var steps = new List<SolutionStep>();

            steps.Add(new("Start with the equation", $"{LeftSide(a, b)} = {c}"));

            if (b != 0)
            {
                string moveDesc = b > 0
                    ? $"Subtract {b} from both sides"
                    : $"Add {Math.Abs(b)} to both sides";

                steps.Add(new(moveDesc, $"{Term(a)} = {MovedConstant(c, b)}"));
                steps.Add(new("Work out the right-hand side", $"{Term(a)} = {rightHandSide}"));
            }

            if (a != 1)
            {
                steps.Add(new($"Divide both sides by {a}", $"x = {rightHandSide}/{a}"));
                steps.Add(new("Work out x", $"x = {x}"));
            }

            return steps;
        }

        private static string Term(double a) => a == 1 ? "x" : a == -1 ? "-x" : $"{a}x";

        private static string LeftSide(double a, double b) => b == 0 ? Term(a) : $"{Term(a)} {(b > 0 ? "+" : "-")} {Math.Abs(b)}";

        private static string MovedConstant(double c, double b) => $"{c} {(b > 0 ? "-" : "+")} {Math.Abs(b)}";

        #endregion

        #region Quadratic Equations
        public static SolveResult<QuadraticEquationSolution> SolveQuadraticEquation(QuadraticEquation equation)
        {
            if (equation.A == 0)
                throw new ArgumentException(
                    "Not a quadratic equation: the coefficient of x² is zero.", nameof(equation));

            double a = equation.A;
            double b = equation.B;
            double c = equation.C;

            double discriminant = b * b - 4 * a * c;

            QuadraticRootType rootType =
                discriminant > 0 ? QuadraticRootType.TwoDistinct :
                discriminant == 0 ? QuadraticRootType.Repeated :
                                    QuadraticRootType.NoRealRoots;

            double sqrt = rootType == QuadraticRootType.NoRealRoots ? double.NaN : Math.Sqrt(discriminant);

            double x1, x2;

            if (rootType == QuadraticRootType.NoRealRoots)
            {
                x1 = double.NaN;
                x2 = double.NaN;
            }
            else
            {
                x1 = (-b + sqrt) / (2 * a);
                x2 = (-b - sqrt) / (2 * a);
            }

            var solution = new QuadraticEquationSolution
            {
                Discriminant = discriminant,
                SquareRootOfDiscriminant = sqrt,
                X1 = x1,
                X2 = x2,
                RootType = rootType
            };

            return new SolveResult<QuadraticEquationSolution>
            {
                Solution = solution,
                Steps = BuildQuadraticFormulaSteps(a, b, c, discriminant, sqrt, x1, x2, rootType),
                Method = "Quadratic formula"
            };
        }

        private static List<SolutionStep> BuildQuadraticFormulaSteps(double a, double b, double c, double discriminant, double sqrt, double x1, double x2, QuadraticRootType rootType)
        {
            var steps = new List<SolutionStep>
            {
                new("Start with the equation", $"{FormatQuadratic(a, b, c)} = 0"),
                new("Identify the coefficients", $"a = {a}, b = {b}, c = {c}"),
                new("Apply the quadratic formula", "x = (-b ± √(b² - 4ac)) / 2a"),
                new("Work out the discriminant", $"b² - 4ac = {discriminant}")
            };

            switch (rootType)
            {
                case QuadraticRootType.TwoDistinct:
                    steps.Add(new("The discriminant is positive, so there are two real roots", $"√{discriminant} = {sqrt}"));
                    steps.Add(new("Work out the first root", $"x = (-({b}) + {sqrt}) / (2 × {a}) = {x1}"));
                    steps.Add(new("Work out the second root", $"x = (-({b}) - {sqrt}) / (2 × {a}) = {x2}"));
                    break;

                case QuadraticRootType.Repeated:
                    steps.Add(new("The discriminant is zero, so there is one repeated root", $"x = -({b}) / (2 × {a}) = {x1}"));
                    break;

                case QuadraticRootType.NoRealRoots:
                    steps.Add(new("The discriminant is negative, so there are no real roots", "no real solutions"));
                    break;
            }

            return steps;
        }

        private static string FormatQuadratic(double a, double b, double c)
        {
            string result = a == 1 ? "x²" : a == -1 ? "-x²" : $"{a}x²";

            if (b != 0)
                result += $" {(b > 0 ? "+" : "-")} {(Math.Abs(b) == 1 ? "" : Math.Abs(b).ToString())}x";

            if (c != 0)
                result += $" {(c > 0 ? "+" : "-")} {Math.Abs(c)}";

            return result;
        }

        #endregion
    }
}
