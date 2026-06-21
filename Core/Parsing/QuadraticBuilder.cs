using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class QuadraticBuilder
    {
        public static QuadraticEquation Build(EquationTerms terms)
        {
            double a = terms.Left.GetValueOrDefault("x^2") - terms.Right.GetValueOrDefault("x^2");
            double b = terms.Left.GetValueOrDefault("x") - terms.Right.GetValueOrDefault("x");
            double c = terms.Left.GetValueOrDefault("1") - terms.Right.GetValueOrDefault("1");

            return new QuadraticEquation { A = a, B = b, C = c };
        }
    }
}
