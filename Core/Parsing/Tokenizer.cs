using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Parsing
{
    public static class Tokenizer
    {
        public static List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int i = 0;

            while (i < input.Length)
            {
                char c = input[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                if (char.IsDigit(c) || c == '.')
                {
                    int start = i;
                    while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                        i++;
                    tokens.Add(new Token(TokenKind.Number, input[start..i], start));
                    continue;
                }

                if (char.IsLetter(c))
                {
                    tokens.Add(new Token(TokenKind.Variable, c.ToString(), i));
                    i++;
                    continue;
                }

                TokenKind kind = c switch
                {
                    '+' => TokenKind.Plus,
                    '-' => TokenKind.Minus,
                    '*' => TokenKind.Star,
                    '^' => TokenKind.Caret,
                    '=' => TokenKind.Equals,
                    _ => throw BuildError(c, i)
                };

                tokens.Add(new Token(kind, c.ToString(), i));
                i++;
            }

            return tokens;
        }

        private static FormatException BuildError(char c, int position)
        {
            string reason = c is '(' or ')' or '/'
                ? $"'{c}' is not supported yet"
                : $"unexpected character '{c}'";
            return new FormatException($"{reason} at position {position}.");
        }
    }
}
