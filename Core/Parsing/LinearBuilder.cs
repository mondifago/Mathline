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
            double a = terms.Left.GetValueOrDefault("x") - terms.Right.GetValueOrDefault("x");
            double b = terms.Left.GetValueOrDefault("1");
            double c = terms.Right.GetValueOrDefault("1");

            return new LinearEquation { A = a, B = b, C = c };
        }
    }
}
