using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class SimultaneousBuilder
    {
        public static SimultaneousEquation Build(EquationTerms first, EquationTerms second)
        {
            return new SimultaneousEquation
            {
                A1 = NetVariable(first, "x"),
                B1 = NetVariable(first, "y"),
                C1 = NetConstant(first),
                A2 = NetVariable(second, "x"),
                B2 = NetVariable(second, "y"),
                C2 = NetConstant(second)
            };
        }

        private static double NetVariable(EquationTerms terms, string variable) =>
            terms.Left.GetValueOrDefault(variable) - terms.Right.GetValueOrDefault(variable);

        private static double NetConstant(EquationTerms terms) =>
            terms.Right.GetValueOrDefault("1") - terms.Left.GetValueOrDefault("1");
    }
}
