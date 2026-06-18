using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class LinearBuilder
    {
        public static LinearEquation Build(EquationTerms terms)
        {
            EnsureLinear(terms.Left, "left");
            EnsureLinear(terms.Right, "right");

            double a = terms.Left.GetValueOrDefault("x") - terms.Right.GetValueOrDefault("x");
            double b = terms.Left.GetValueOrDefault("1");
            double c = terms.Right.GetValueOrDefault("1");

            return new LinearEquation { A = a, B = b, C = c };
        }

        private static void EnsureLinear(IReadOnlyDictionary<string, double> side, string which)
        {
            foreach (var monomial in side.Keys)
            {
                if (monomial != "1" && monomial != "x")
                    throw new FormatException(
                        $"'{monomial}' is not part of a linear equation in x (found on the {which} side).");
            }
        }
    }
}
