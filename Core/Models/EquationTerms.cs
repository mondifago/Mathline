using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public record EquationTerms(IReadOnlyDictionary<string, double> Left, IReadOnlyDictionary<string, double> Right);
}
