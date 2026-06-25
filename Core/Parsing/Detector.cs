using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class Detector
    {
        private static readonly HashSet<string> SupportedMonomials = new() { "1", "x", "x^2" };

        private static readonly HashSet<string> SystemMonomials = new() { "1", "x", "y" };

        public static EquationType Detect(EquationTerms terms)
        {
            foreach (string monomial in AllMonomials(terms))
            {
                if (!SupportedMonomials.Contains(monomial) && Net(terms, monomial) != 0)
                    throw new FormatException(
                        $"'{monomial}' isn't supported — only linear and quadratic equations in x are handled.");
            }

            if (Net(terms, "x^2") != 0)
                return EquationType.Quadratic;

            if (Net(terms, "x") != 0)
                return EquationType.Linear;

            throw new FormatException("No variable found — this isn't an equation in x.");
        }

        private static double Net(EquationTerms terms, string monomial) =>
            terms.Left.GetValueOrDefault(monomial) - terms.Right.GetValueOrDefault(monomial);

        private static IEnumerable<string> AllMonomials(EquationTerms terms)
        {
            var keys = new HashSet<string>(terms.Left.Keys);
            keys.UnionWith(terms.Right.Keys);
            return keys;
        }

        public static void ValidateLinearSystem(EquationTerms first, EquationTerms second)
        {
            EnsureOnlySystemMonomials(first, 1);
            EnsureOnlySystemMonomials(second, 2);

            bool hasX = Net(first, "x") != 0 || Net(second, "x") != 0;
            bool hasY = Net(first, "y") != 0 || Net(second, "y") != 0;

            if (!hasX || !hasY)
                throw new FormatException(
                    "A simultaneous system needs two variables (x and y) across the two equations.");
        }

        private static void EnsureOnlySystemMonomials(EquationTerms terms, int eqNumber)
        {
            foreach (string monomial in AllMonomials(terms))
            {
                if (!SystemMonomials.Contains(monomial) && Net(terms, monomial) != 0)
                    throw new FormatException(
                        $"Equation {eqNumber} isn't linear in x and y — '{monomial}' isn't allowed in a simultaneous system.");
            }
        }
    }
}
