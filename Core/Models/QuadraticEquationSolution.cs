using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class QuadraticEquationSolution
    {
        public double Discriminant { get; set; }
        public double SquareRootOfDiscriminant { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public QuadraticRootType RootType { get; set; }
    }
}
