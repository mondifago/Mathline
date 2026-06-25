using Core.Models;
using Core.Parsing;
using Core.Solving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Coordination
{
    public record SolveOutcome(EquationType Type, string Method, IReadOnlyList<SolutionStep> Steps);

    public static class Coordinator
    {
        public static SolveOutcome Solve(string input)
        {
            var equations = Splitter.Split(input);

            return equations.Count switch
            {
                0 => throw new FormatException("Enter an equation to solve."),
                1 => SolveSingle(equations[0]),
                2 => SolveSystem(equations[0], equations[1]),
                _ => throw new FormatException("Only one equation, or a system of two, can be solved at the moment.")
            };
        }

        private static SolveOutcome SolveSingle(string equation)
        {
            var terms = Collect(equation);

            return Detector.Detect(terms) switch
            {
                EquationType.Linear =>
                    ToOutcome(EquationType.Linear, SolvingLogic.SolveLinearEquation(LinearBuilder.Build(terms))),
                EquationType.Quadratic =>
                    ToOutcome(EquationType.Quadratic, SolvingLogic.SolveQuadraticEquation(QuadraticBuilder.Build(terms))),
                var other => throw new InvalidOperationException($"Unhandled equation type: {other}")
            };
        }

        private static SolveOutcome SolveSystem(string first, string second)
        {
            var firstTerms = Collect(first);
            var secondTerms = Collect(second);

            Detector.ValidateLinearSystem(firstTerms, secondTerms);

            var equation = SimultaneousBuilder.Build(firstTerms, secondTerms);
            return ToOutcome(EquationType.Simultaneous, SolvingLogic.SolveSimultaneousEquation(equation));
        }

        private static EquationTerms Collect(string equation) =>
            TermCollector.Collect(Tokenizer.Tokenize(Preprocessor.Canonicalize(equation)));

        private static SolveOutcome ToOutcome<T>(EquationType type, SolveResult<T> result) =>
            new(type, result.Method, result.Steps);
    }
}
