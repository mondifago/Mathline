using Core.Models;
using Core.Parsing;
using Core.Solving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public record SolveOutcome(EquationType Type, string Method, IReadOnlyList<SolutionStep> Steps);

    public static class Coordinator
    {
        public static SolveOutcome Solve(string input)
        {
            var terms = TermCollector.Collect(Tokenizer.Tokenize(input));

            return Detector.Detect(terms) switch
            {
                EquationType.Linear =>
                    ToOutcome(EquationType.Linear,
                        SolvingLogic.SolveLinearEquation(LinearBuilder.Build(terms))),

                EquationType.Quadratic =>
                    ToOutcome(EquationType.Quadratic,
                        SolvingLogic.SolveQuadraticEquation(QuadraticBuilder.Build(terms))),

                var other => throw new InvalidOperationException($"Unhandled equation type: {other}")
            };
        }

        private static SolveOutcome ToOutcome<T>(EquationType type, SolveResult<T> result) =>
            new(type, result.Method, result.Steps);
    }
}
