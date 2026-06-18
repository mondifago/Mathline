using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Parsing
{
    public static class TermCollector
    {
        public static EquationTerms Collect(IReadOnlyList<Token> tokens)
        {
            int equalsIndex = -1;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Kind != TokenKind.Equals) continue;
                if (equalsIndex != -1)
                    throw new FormatException("An equation must contain exactly one '='.");
                equalsIndex = i;
            }

            if (equalsIndex == -1)
                throw new FormatException("An equation must contain an '='.");

            var leftTokens = tokens.Take(equalsIndex).ToList();
            var rightTokens = tokens.Skip(equalsIndex + 1).ToList();

            if (leftTokens.Count == 0 || rightTokens.Count == 0)
                throw new FormatException("Both sides of '=' must have an expression.");

            return new EquationTerms(CollectSide(leftTokens), CollectSide(rightTokens));
        }

        private static Dictionary<string, double> CollectSide(IReadOnlyList<Token> tokens)
        {
            var bag = new Dictionary<string, double>();
            int i = 0;
            bool first = true;

            while (i < tokens.Count)
            {
                double sign = 1.0;
                int signs = 0;
                while (i < tokens.Count &&
                       (tokens[i].Kind == TokenKind.Plus || tokens[i].Kind == TokenKind.Minus))
                {
                    if (tokens[i].Kind == TokenKind.Minus) sign = -sign;
                    i++;
                    signs++;
                }

                if (!first && signs == 0)
                    throw new FormatException(
                        $"Missing operator before '{tokens[i].Text}' at position {tokens[i].Position}.");
                first = false;

                if (i >= tokens.Count)
                    throw new FormatException("An expression ends with an operator.");

                double coefficient;
                string monomial;

                if (tokens[i].Kind == TokenKind.Number)
                {
                    coefficient = ParseNumber(tokens[i]);
                    i++;
                    if (i < tokens.Count && tokens[i].Kind == TokenKind.Variable)
                    {
                        monomial = tokens[i].Text;
                        i++;
                    }
                    else
                    {
                        monomial = "1";
                    }
                }
                else if (tokens[i].Kind == TokenKind.Variable)
                {
                    coefficient = 1.0;
                    monomial = tokens[i].Text;
                    i++;
                }
                else
                {
                    throw new FormatException(
                        $"Expected a number or variable at position {tokens[i].Position}, found '{tokens[i].Text}'.");
                }

                bag[monomial] = bag.GetValueOrDefault(monomial) + sign * coefficient;
            }

            return bag;
        }

        private static double ParseNumber(Token token)
        {
            if (double.TryParse(token.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;

            throw new FormatException($"Invalid number '{token.Text}' at position {token.Position}.");
        }
    }
}
