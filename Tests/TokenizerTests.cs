using Core.Models;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class TokenizerTests
    {
        [Fact]
        public void TokenizesLinearEquation()
        {
            var tokens = Tokenizer.Tokenize("3x + 5 = 14");

            var kinds = tokens.Select(t => t.Kind).ToArray();

            Assert.Equal(new[]
            {
                TokenKind.Number,
                TokenKind.Variable,
                TokenKind.Plus,
                TokenKind.Number,
                TokenKind.Equals,
                TokenKind.Number
            }, kinds);

            Assert.Equal("3", tokens[0].Text);
            Assert.Equal("x", tokens[1].Text);
            Assert.Equal("14", tokens[5].Text);
        }

        [Fact]
        public void SkipsWhitespace()
        {
            var spaced = Tokenizer.Tokenize("3x + 5 = 14").Select(t => t.Kind);
            var tight = Tokenizer.Tokenize("3x+5=14").Select(t => t.Kind);

            Assert.Equal(spaced, tight);
        }

        [Fact]
        public void ReadsDecimalAsSingleNumber()
        {
            var tokens = Tokenizer.Tokenize("2.5x = 5");

            Assert.Equal(TokenKind.Number, tokens[0].Kind);
            Assert.Equal("2.5", tokens[0].Text);
        }

        [Fact]
        public void RecordsPosition()
        {
            var tokens = Tokenizer.Tokenize("3x + 5 = 14");

            // '=' sits at index 7
            var equals = tokens.Single(t => t.Kind == TokenKind.Equals);
            Assert.Equal(7, equals.Position);
        }

        [Theory]
        [InlineData("3x / 2 = 1")]
        [InlineData("(3x) = 1")]
        public void RejectsUnsupportedCharacters(string input)
        {
            Assert.Throws<FormatException>(() => Tokenizer.Tokenize(input));
        }
    }
}
