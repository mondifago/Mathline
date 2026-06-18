using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class SolveResult<TSolution>
    {
        public TSolution Solution { get; set; }
        public List<SolutionStep> Steps { get; set; } = new();
        public string Method { get; set; }
    }
}
