using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Solving
{
    public class SolvingLogic
    {
        public static SolveResult<LinearEquationSolution> SolveLinearEquation(LinearEquation equation)
        {
            if (equation.A == 0)
                throw new ArgumentException(
                    "Not a linear equation: the coefficient of x is zero.", nameof(equation));

            double a = equation.A;
            double b = equation.B;
            double c = equation.C;

            double rhs = c - b;   // ax + b = c  ->  ax = c - b
            double x = rhs / a;   // x = (c - b) / a

            return new SolveResult<LinearEquationSolution>
            {
                Solution = new LinearEquationSolution { X = x },
                Steps = BuildLinearSteps(a, rhs, x),
                Method = "Rearrangement"
            };
        }

        private static List<SolutionStep> BuildLinearSteps(double a, double rhs, double x)
        {
            return new List<SolutionStep>
            {
                new("Move the constant to the right-hand side", $"{a}x = {rhs}"),
                new("Divide both sides by the coefficient of x", $"x = {rhs}/{a}"),
                new("Simplify", $"x = {x}")
            };
        }
    }
}
