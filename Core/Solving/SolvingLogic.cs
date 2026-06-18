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
    }
}
