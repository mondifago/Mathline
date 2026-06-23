using Core.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class SplitterTests
    {
        [Fact]
        public void SingleEquationReturnsOneItem()
        {
            var result = Splitter.Split("3x + 5 = 14");
            Assert.Single(result);
            Assert.Equal("3x + 5 = 14", result[0]);
        }

        [Fact]
        public void TwoLinesReturnTwoEquations()
        {
            var result = Splitter.Split("2x + y = 7\nx - y = 2");
            Assert.Equal(2, result.Count);
            Assert.Equal("2x + y = 7", result[0]);
            Assert.Equal("x - y = 2", result[1]);
        }

        [Fact]
        public void HandlesWindowsLineEndings()
        {
            var result = Splitter.Split("2x + y = 7\r\nx - y = 2");
            Assert.Equal(2, result.Count);
            Assert.Equal("2x + y = 7", result[0]);   // '\r' trimmed
            Assert.Equal("x - y = 2", result[1]);
        }

        [Fact]
        public void DropsBlankLines()
        {
            var result = Splitter.Split("2x + y = 7\n\nx - y = 2\n");
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TrimsSurroundingWhitespace()
        {
            var result = Splitter.Split("  3x + 5 = 14  ");
            Assert.Single(result);
            Assert.Equal("3x + 5 = 14", result[0]);
        }

        [Fact]
        public void EmptyInputReturnsEmptyList()
        {
            Assert.Empty(Splitter.Split("   "));
        }
    }
}
