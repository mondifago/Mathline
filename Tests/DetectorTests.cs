using Core.Models;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class DetectorTests
    {
        private static EquationType Detect(string input) =>
            Detector.Detect(TermCollector.Collect(Tokenizer.Tokenize(input)));

        [Theory]
        [InlineData("3x + 5 = 14")]
        [InlineData("2x = -6")]
        [InlineData("x - 3 = 0")]
        public void DetectsLinear(string input)
        {
            Assert.Equal(EquationType.Linear, Detect(input));
        }

        [Theory]
        [InlineData("x^2 - 5x + 6 = 0")]
        [InlineData("3x^2 = 12")]
        [InlineData("x^2 = 9")]
        public void DetectsQuadratic(string input)
        {
            Assert.Equal(EquationType.Quadratic, Detect(input));
        }

        [Fact]
        public void CancelledSquaredTermIsLinear()
        {
            Assert.Equal(EquationType.Linear, Detect("x^2 + 3x = x^2 + 5"));
        }

        [Fact]
        public void ZeroSquaredCoefficientIsLinear()
        {
            Assert.Equal(EquationType.Linear, Detect("0x^2 + 3x = 5"));
        }

        [Theory]
        [InlineData("5 = 3")]        // no variable
        [InlineData("x + y = 5")]    // second variable
        [InlineData("x^3 = 8")]      // unsupported degree
        public void RejectsUnsupported(string input)
        {
            Assert.Throws<FormatException>(() => Detect(input));
        }

        [Fact]
        public void ValidSystemPasses()
        {
            var first = TermCollector.Collect(Tokenizer.Tokenize("2x + y = 7"));
            var second = TermCollector.Collect(Tokenizer.Tokenize("x - y = 2"));
            Detector.ValidateLinearSystem(first, second);   // no throw = pass
        }

        [Theory]
        [InlineData("x^2 + y = 7", "x - y = 2")]   // nonlinear in a system
        [InlineData("x + z = 7", "x - y = 2")]     // third variable
        [InlineData("2x = 6", "3x = 9")]           // only one variable across the system
        public void RejectsInvalidSystem(string first, string second)
        {
            var f = TermCollector.Collect(Tokenizer.Tokenize(first));
            var s = TermCollector.Collect(Tokenizer.Tokenize(second));
            Assert.Throws<FormatException>(() => Detector.ValidateLinearSystem(f, s));
        }
    }
}
