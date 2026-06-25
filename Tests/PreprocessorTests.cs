using Core.Coordination;
using Core.Models;
using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class PreprocessorTests
    {
        [Fact]
        public void ConvertsSquaredSymbol() =>
            Assert.Equal("x^2 - 5x + 6 = 0", Preprocessor.Canonicalize("x² - 5x + 6 = 0"));

        [Fact]
        public void ConvertsMultiDigitSuperscript() =>
            Assert.Equal("x^10 = 1", Preprocessor.Canonicalize("x¹⁰ = 1"));

        [Fact]
        public void ConvertsUnicodeMinusAndTimes() =>
            Assert.Equal("3*x - 5 = 1", Preprocessor.Canonicalize("3×x − 5 = 1"));

        [Fact]
        public void LeavesPlainAsciiUnchanged() =>
            Assert.Equal("x^2 - 5x + 6 = 0", Preprocessor.Canonicalize("x^2 - 5x + 6 = 0"));

        [Fact]
        public void FullPipelineAcceptsPastedSuperscript()
        {
            var outcome = Coordinator.Solve("x² - 5x + 6 = 0");
            Assert.Equal(EquationType.Quadratic, outcome.Type);
        }
    }
}
