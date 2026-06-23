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

            bool canFactor = IsPerfectSquare(discriminant);

            return new SolveResult<QuadraticEquationSolution>
            {
                Solution = solution,
                Steps = canFactor
                    ? BuildFactoringSteps(a, b, c, x1, x2, rootType)
                    : BuildQuadraticFormulaSteps(a, b, c, discriminant, sqrt, x1, x2, rootType),
                Method = canFactor ? "Factorisation" : "Quadratic formula"
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

        private static bool IsPerfectSquare(double discriminant)
        {
            if (discriminant < 0) return false;          // negative D → no real roots → can't factor
            double root = Math.Sqrt(discriminant);
            return root == Math.Floor(root);             // exact for integer coefficients (the school case)
        }

        private static List<SolutionStep> BuildFactoringSteps(
    double a, double b, double c, double x1, double x2, QuadraticRootType rootType)
        {
            var steps = new List<SolutionStep>
        {
            new("Start with the equation", $"{FormatQuadratic(a, b, c)} = 0")
        };

            // Divide to monic form — a SHOWN step, never hidden (mirrors linear's "divide both sides").
            if (a != 1)
                steps.Add(new($"Divide both sides by {a}", $"{FormatQuadratic(1, b / a, c / a)} = 0"));

            if (rootType == QuadraticRootType.Repeated)
                steps.Add(new("Factor the left-hand side", $"{Factor(x1)}² = 0"));
            else
                steps.Add(new("Factor the left-hand side", $"{Factor(x1)}{Factor(x2)} = 0"));

            return steps;
        }

        private static string Factor(double root) =>
            root == 0 ? "x"
            : root > 0 ? $"(x - {root})"
            : $"(x + {Math.Abs(root)})";
        #endregion

        #region Simultaneous Equations
        public static SolveResult<SimultaneousEquationSolution> SolveSimultaneousEquation(SimultaneousEquation equation)
        {
            double a1 = equation.A1, b1 = equation.B1, c1 = equation.C1;
            double a2 = equation.A2, b2 = equation.B2, c2 = equation.C2;

            double determinant = a1 * b2 - a2 * b1;

            var steps = new List<SolutionStep>
    {
        new("Equation 1", $"{FormatTwoVar(a1, b1)} = {c1}"),
        new("Equation 2", $"{FormatTwoVar(a2, b2)} = {c2}")
    };

            if (determinant == 0)
            {
                double dx = c1 * b2 - c2 * b1;
                double dy = a1 * c2 - a2 * c1;

                SimultaneousSolutionType degenerate;
                if (dx == 0 && dy == 0)
                {
                    degenerate = SimultaneousSolutionType.InfinitelyMany;
                    steps.Add(new("The two equations describe the same line", "infinitely many solutions"));
                }
                else
                {
                    degenerate = SimultaneousSolutionType.NoSolution;
                    steps.Add(new("The two lines are parallel and never meet", "no solution"));
                }

                return new SolveResult<SimultaneousEquationSolution>
                {
                    Solution = new SimultaneousEquationSolution { SolutionType = degenerate },
                    Steps = steps,
                    Method = "Elimination"
                };
            }

            double x, y;

            if (a1 != 0 && a2 != 0)                 
            {
                y = Eliminate(a1, b1, c1, a2, b2, c2, "x", steps);
                x = BackSubstitute(b1, a1, c1, "x", "y", y, 1, steps);
            }
            else if (b1 != 0 && b2 != 0)               
            {
                x = Eliminate(a1, b1, c1, a2, b2, c2, "y", steps);
                y = BackSubstitute(a1, b1, c1, "y", "x", x, 1, steps);
            }
            else                                       
            {
                x = a1 != 0 ? SolveSingleVariable(a1, c1, "x", 1, steps)
                            : SolveSingleVariable(a2, c2, "x", 2, steps);
                y = b1 != 0 ? SolveSingleVariable(b1, c1, "y", 1, steps)
                            : SolveSingleVariable(b2, c2, "y", 2, steps);
            }

            steps.Add(new("Solution", $"x = {x}, y = {y}"));

            return new SolveResult<SimultaneousEquationSolution>
            {
                Solution = new SimultaneousEquationSolution { X = x, Y = y, SolutionType = SimultaneousSolutionType.Unique },
                Steps = steps,
                Method = "Elimination"
            };
        }

        private static double Eliminate(
            double a1, double b1, double c1,
            double a2, double b2, double c2,
            string eliminate, List<SolutionStep> steps)
        {
            double p1 = eliminate == "x" ? a1 : b1;
            double p2 = eliminate == "x" ? a2 : b2;
            double s1 = p2, s2 = p1;                      

            double na1 = a1 * s1, nb1 = b1 * s1, nc1 = c1 * s1;
            double na2 = a2 * s2, nb2 = b2 * s2, nc2 = c2 * s2;

            if (s1 != 1)
                steps.Add(new($"Multiply equation 1 by {s1}", $"{FormatTwoVar(na1, nb1)} = {nc1}"));
            if (s2 != 1)
                steps.Add(new($"Multiply equation 2 by {s2}", $"{FormatTwoVar(na2, nb2)} = {nc2}"));

            string keep = eliminate == "x" ? "y" : "x";
            double keptCoeff = eliminate == "x" ? nb1 - nb2 : na1 - na2;
            double rhs = nc1 - nc2;

            steps.Add(new($"Subtract the equations to eliminate {eliminate}", $"{TermXY(keptCoeff, keep)} = {rhs}"));

            double value = rhs / keptCoeff;
            steps.Add(new($"Solve for {keep}", $"{keep} = {value}"));
            return value;
        }

        private static double BackSubstitute(
            double keepCoeff, double solveCoeff, double rhs,
            string solveFor, string knownName, double knownValue,
            int eqNumber, List<SolutionStep> steps)
        {
            double knownTerm = keepCoeff * knownValue;

            steps.Add(new($"Substitute {knownName} = {knownValue} into equation {eqNumber}",
                $"{TermXY(solveCoeff, solveFor)}{Signed(knownTerm)} = {rhs}"));

            double remaining = rhs - knownTerm;
            if (knownTerm != 0)
            {
                steps.Add(new("Move the constant to the right-hand side",
                    $"{TermXY(solveCoeff, solveFor)} = {rhs}{Signed(-knownTerm)}"));
                steps.Add(new("Work out the right-hand side",
                    $"{TermXY(solveCoeff, solveFor)} = {remaining}"));
            }

            double value = remaining / solveCoeff;
            if (solveCoeff != 1)
            {
                steps.Add(new($"Divide both sides by {solveCoeff}", $"{solveFor} = {remaining}/{solveCoeff}"));
                steps.Add(new($"Work out {solveFor}", $"{solveFor} = {value}"));
            }
            else
            {
                steps.Add(new($"Solve for {solveFor}", $"{solveFor} = {value}"));
            }
            return value;
        }

        private static double SolveSingleVariable(double coef, double rhs, string name, int eqNumber, List<SolutionStep> steps)
        {
            steps.Add(new($"Equation {eqNumber} contains only {name}", $"{TermXY(coef, name)} = {rhs}"));
            double value = rhs / coef;
            if (coef != 1)
                steps.Add(new($"Divide both sides by {coef}", $"{name} = {rhs}/{coef}"));
            steps.Add(new($"Work out {name}", $"{name} = {value}"));
            return value;
        }

        private static string FormatTwoVar(double a, double b)
        {
            string left = a != 0 ? TermXY(a, "x") : "";
            if (b == 0) return left == "" ? "0" : left;
            if (left == "") return TermXY(b, "y");

            double mag = Math.Abs(b);
            string yTerm = mag == 1 ? "y" : $"{mag}y";
            return left + (b > 0 ? " + " : " - ") + yTerm;
        }

        private static string TermXY(double coef, string name) =>
            coef == 1 ? name : coef == -1 ? $"-{name}" : $"{coef}{name}";

        private static string Signed(double v) =>
            v == 0 ? "" : v > 0 ? $" + {v}" : $" - {Math.Abs(v)}";



        #endregion
    }
}
