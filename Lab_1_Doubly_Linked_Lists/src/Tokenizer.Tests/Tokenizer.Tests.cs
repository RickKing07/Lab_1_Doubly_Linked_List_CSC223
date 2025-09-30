/*
 * ====================================================================
 * Tokenizer Unit Tests
 * ====================================================================
 * 
 * Test Author: Claude Sonnet 4.5 (Anthropic AI Assistant)
 * Created: September 29, 2025-
 * ====================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Tokenizer;

namespace Tokenizer.Tests
{
    public class TokenizerImplTests
    {
        private readonly TokenizerImpl tokenizer = new TokenizerImpl();

        #region Null and Empty Input Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n\r")]
        public void Tokenize_NullOrEmptyInput_ReturnsEmptyList(string input)
        {
            var result = tokenizer.Tokenize(input);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region Variable Tests

        [Theory]
        [InlineData("x", "x", TokenType.VARIABLE)]
        [InlineData("abc", "abc", TokenType.VARIABLE)]
        [InlineData("variable", "variable", TokenType.VARIABLE)]
        [InlineData("a", "a", TokenType.VARIABLE)]
        [InlineData("xyz", "xyz", TokenType.VARIABLE)]
        public void Tokenize_SingleVariable_ReturnsCorrectToken(string input, string expectedValue, TokenType expectedType)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Single(result);
            Assert.Equal(expectedValue, result[0].Value);
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("x y", 2)]
        [InlineData("abc def ghi", 3)]
        [InlineData("a b c d e", 5)]
        public void Tokenize_MultipleVariables_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
            Assert.All(result, token => Assert.Equal(TokenType.VARIABLE, token.Type));
        }

        [Theory]
        [InlineData("x\ty\nz", 3)]
        [InlineData("a  b    c", 3)]
        public void Tokenize_VariablesWithWhitespace_IgnoresWhitespace(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
        }

        #endregion

        #region Return Keyword Tests

        [Theory]
        [InlineData("return", "return", TokenType.RETURN)]
        public void Tokenize_ReturnKeyword_ReturnsCorrectToken(string input, string expectedValue, TokenType expectedType)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Single(result);
            Assert.Equal(expectedValue, result[0].Value);
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("return x")]
        [InlineData("return 5")]
        [InlineData("x return")]
        public void Tokenize_ReturnWithOtherTokens_ReturnsMultipleTokens(string input)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Type == TokenType.RETURN && t.Value == "return");
        }

        #endregion

        #region Integer Tests

        [Theory]
        [InlineData("0", "0", TokenType.INTEGER)]
        [InlineData("1", "1", TokenType.INTEGER)]
        [InlineData("42", "42", TokenType.INTEGER)]
        [InlineData("999", "999", TokenType.INTEGER)]
        [InlineData("1234567890", "1234567890", TokenType.INTEGER)]
        public void Tokenize_PositiveInteger_ReturnsCorrectToken(string input, string expectedValue, TokenType expectedType)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Single(result);
            Assert.Equal(expectedValue, result[0].Value);
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("1 2 3", 3)]
        [InlineData("10 20 30 40", 4)]
        public void Tokenize_MultipleIntegers_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
            Assert.All(result, token => Assert.Equal(TokenType.INTEGER, token.Type));
        }

        #endregion

        #region Float Tests

        [Theory]
        [InlineData("0.0", "0.0", TokenType.FLOAT)]
        [InlineData("1.5", "1.5", TokenType.FLOAT)]
        [InlineData("3.14159", "3.14159", TokenType.FLOAT)]
        [InlineData("99.99", "99.99", TokenType.FLOAT)]
        [InlineData("0.1", "0.1", TokenType.FLOAT)]
        [InlineData("123.456", "123.456", TokenType.FLOAT)]
        public void Tokenize_PositiveFloat_ReturnsCorrectToken(string input, string expectedValue, TokenType expectedType)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Single(result);
            Assert.Equal(expectedValue, result[0].Value);
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("1.0 2.5 3.7", 3)]
        [InlineData("0.1 0.2 0.3 0.4", 4)]
        public void Tokenize_MultipleFloats_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
            Assert.All(result, token => Assert.Equal(TokenType.FLOAT, token.Type));
        }

        [Theory]
        [InlineData("1.")]
        [InlineData(".5")]
        [InlineData("1..5")]
        [InlineData("1.2.3")]
        public void Tokenize_InvalidFloat_ThrowsException(string input)
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(input));
        }

        #endregion

        #region Operator Tests - Addition

        [Theory]
        [InlineData("+", "+", TokenType.OPERATOR)]
        [InlineData("1 + 2", "+")]
        [InlineData("x+y", "+")]
        public void Tokenize_PlusOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        #endregion

        #region Operator Tests - Subtraction

        [Theory]
        [InlineData("-", "-", TokenType.OPERATOR)]
        [InlineData("5 - 3", "-")]
        [InlineData("x-y", "-")]
        public void Tokenize_MinusOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        #endregion

        #region Operator Tests - Multiplication

        [Theory]
        [InlineData("*", "*", TokenType.OPERATOR)]
        [InlineData("2 * 3", "*")]
        [InlineData("x*y", "*")]
        public void Tokenize_TimesOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        #endregion

        #region Operator Tests - Division

        [Theory]
        [InlineData("/", "/", TokenType.OPERATOR)]
        [InlineData("10 / 2", "/")]
        [InlineData("x/y", "/")]
        public void Tokenize_IntDivisionOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        [Theory]
        [InlineData("//", "//", TokenType.OPERATOR)]
        [InlineData("10 // 3", "//")]
        [InlineData("x//y", "//")]
        public void Tokenize_FloatDivisionOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        #endregion

        #region Operator Tests - Modulus

        [Theory]
        [InlineData("%", "%", TokenType.OPERATOR)]
        [InlineData("10 % 3", "%")]
        [InlineData("x%y", "%")]
        public void Tokenize_ModulusOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        #endregion

        #region Operator Tests - Exponentiation

        [Theory]
        [InlineData("**", "**", TokenType.OPERATOR)]
        [InlineData("2 ** 3", "**")]
        [InlineData("x**y", "**")]
        public void Tokenize_ExponentiationOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.OPERATOR);
        }

        #endregion

        #region Assignment Operator Tests

        [Theory]
        [InlineData(":=", ":=", TokenType.ASSIGNMENT)]
        [InlineData("x := 5", ":=")]
        [InlineData("y:=10", ":=")]
        public void Tokenize_AssignmentOperator_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.ASSIGNMENT);
        }

        [Theory]
        [InlineData(":")]
        [InlineData(": =")]
        public void Tokenize_InvalidAssignment_ThrowsException(string input)
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(input));
        }

        #endregion

        #region Parentheses Tests

        [Theory]
        [InlineData("(", "(", TokenType.LEFT_PAREN)]
        [InlineData("(x)", "(")]
        [InlineData("( 1 + 2 )", "(")]
        public void Tokenize_LeftParenthesis_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.LEFT_PAREN);
        }

        [Theory]
        [InlineData(")", ")", TokenType.RIGHT_PAREN)]
        [InlineData("(x)", ")")]
        [InlineData("( 1 + 2 )", ")")]
        public void Tokenize_RightParenthesis_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.RIGHT_PAREN);
        }

        [Theory]
        [InlineData("()", 2)]
        [InlineData("((()))", 6)]
        [InlineData("(1 + (2 * 3))", 9)]
        public void Tokenize_NestedParentheses_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
        }

        #endregion

        #region Curly Braces Tests

        [Theory]
        [InlineData("{", "{", TokenType.LEFT_CURLY)]
        [InlineData("{ x }", "{")]
        public void Tokenize_LeftCurlyBrace_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.LEFT_CURLY);
        }

        [Theory]
        [InlineData("}", "}", TokenType.RIGHT_CURLY)]
        [InlineData("{ x }", "}")]
        public void Tokenize_RightCurlyBrace_ReturnsCorrectToken(string input, string expectedValue)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Contains(result, t => t.Value == expectedValue && t.Type == TokenType.RIGHT_CURLY);
        }

        [Theory]
        [InlineData("{}", 2)]
        [InlineData("{ { } }", 4)]
        public void Tokenize_NestedCurlyBraces_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
        }

        #endregion

        #region Complex Expression Tests

        [Fact]
        public void Tokenize_SimpleAssignment_ReturnsCorrectSequence()
        {
            var result = tokenizer.Tokenize("x := 5");
            Assert.Equal(3, result.Count);
            Assert.Equal("x", result[0].Value);
            Assert.Equal(TokenType.VARIABLE, result[0].Type);
            Assert.Equal(":=", result[1].Value);
            Assert.Equal(TokenType.ASSIGNMENT, result[1].Type);
            Assert.Equal("5", result[2].Value);
            Assert.Equal(TokenType.INTEGER, result[2].Type);
        }

        [Fact]
        public void Tokenize_ExampleFromAssignment_ReturnsCorrectSequence()
        {
            var result = tokenizer.Tokenize("x := (1 + 2)");
            Assert.Equal(7, result.Count);
            Assert.Equal("x", result[0].Value);
            Assert.Equal(TokenType.VARIABLE, result[0].Type);
            Assert.Equal(":=", result[1].Value);
            Assert.Equal(TokenType.ASSIGNMENT, result[1].Type);
            Assert.Equal("(", result[2].Value);
            Assert.Equal(TokenType.LEFT_PAREN, result[2].Type);
            Assert.Equal("1", result[3].Value);
            Assert.Equal(TokenType.INTEGER, result[3].Type);
            Assert.Equal("+", result[4].Value);
            Assert.Equal(TokenType.OPERATOR, result[4].Type);
            Assert.Equal("2", result[5].Value);
            Assert.Equal(TokenType.INTEGER, result[5].Type);
            Assert.Equal(")", result[6].Value);
            Assert.Equal(TokenType.RIGHT_PAREN, result[6].Type);
        }

        [Theory]
        [InlineData("x := 1 + 2 * 3", 7)]
        [InlineData("result := (a + b) / (c - d)", 13)]
        [InlineData("x := 2 ** 3", 5)]
        public void Tokenize_ComplexExpressions_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public void Tokenize_FloatExpression_ReturnsCorrectSequence()
        {
            var result = tokenizer.Tokenize("x := 3.14 + 2.5");
            Assert.Equal(5, result.Count);
            Assert.Equal(TokenType.VARIABLE, result[0].Type);
            Assert.Equal(TokenType.ASSIGNMENT, result[1].Type);
            Assert.Equal(TokenType.FLOAT, result[2].Type);
            Assert.Equal("3.14", result[2].Value);
            Assert.Equal(TokenType.OPERATOR, result[3].Type);
            Assert.Equal(TokenType.FLOAT, result[4].Type);
            Assert.Equal("2.5", result[4].Value);
        }

        [Fact]
        public void Tokenize_MixedIntegerAndFloat_ReturnsCorrectTypes()
        {
            var result = tokenizer.Tokenize("x := 10 + 3.5");
            Assert.Equal(5, result.Count);
            Assert.Equal(TokenType.INTEGER, result[2].Type);
            Assert.Equal("10", result[2].Value);
            Assert.Equal(TokenType.FLOAT, result[4].Type);
            Assert.Equal("3.5", result[4].Value);
        }

        [Theory]
        [InlineData("{ x := 5 }", 5)]
        [InlineData("{ return x }", 4)]
        public void Tokenize_BlockWithStatements_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
        }

        #endregion

        #region All Operators in One Expression

        [Fact]
        public void Tokenize_AllOperators_ReturnsCorrectTokens()
        {
            var result = tokenizer.Tokenize("a + b - c * d / e // f % g ** h");
            var operators = result.Where(t => t.Type == TokenType.OPERATOR).Select(t => t.Value).ToList();
            Assert.Contains("+", operators);
            Assert.Contains("-", operators);
            Assert.Contains("*", operators);
            Assert.Contains("/", operators);
            Assert.Contains("//", operators);
            Assert.Contains("%", operators);
            Assert.Contains("**", operators);
        }

        #endregion

        #region Edge Cases and Error Handling

        [Theory]
        [InlineData("@")]
        [InlineData("#")]
        [InlineData("$")]
        [InlineData("&")]
        [InlineData("!")]
        public void Tokenize_InvalidCharacters_ThrowsException(string input)
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(input));
        }

        [Theory]
        [InlineData("X")]
        [InlineData("ABC")]
        [InlineData("Variable")]
        public void Tokenize_UppercaseLetters_ThrowsException(string input)
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(input));
        }

        [Theory]
        [InlineData("x1")]
        [InlineData("var2")]
        [InlineData("abc123")]
        public void Tokenize_VariablesWithNumbers_ThrowsException(string input)
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(input));
        }

        [Fact]
        public void Tokenize_OnlyWhitespace_ReturnsEmptyList()
        {
            var result = tokenizer.Tokenize("     \t\n\r   ");
            Assert.Empty(result);
        }

        #endregion

        #region Division Operator Disambiguation

        [Fact]
        public void Tokenize_SingleSlash_ReturnsIntDivision()
        {
            var result = tokenizer.Tokenize("10 / 2");
            var divToken = result.FirstOrDefault(t => t.Type == TokenType.OPERATOR && t.Value.Contains("/"));
            Assert.NotNull(divToken);
            Assert.Equal("/", divToken.Value);
        }

        [Fact]
        public void Tokenize_DoubleSlash_ReturnsFloatDivision()
        {
            var result = tokenizer.Tokenize("10 // 2");
            var divToken = result.FirstOrDefault(t => t.Type == TokenType.OPERATOR && t.Value == "//");
            Assert.NotNull(divToken);
            Assert.Equal("//", divToken.Value);
        }

        [Fact]
        public void Tokenize_MixedDivisionOperators_ReturnsCorrectTokens()
        {
            var result = tokenizer.Tokenize("a / b // c");
            var divTokens = result.Where(t => t.Type == TokenType.OPERATOR && t.Value.Contains("/")).ToList();
            Assert.Equal(2, divTokens.Count);
            Assert.Equal("/", divTokens[0].Value);
            Assert.Equal("//", divTokens[1].Value);
        }

        #endregion

        #region Exponentiation Operator Disambiguation

        [Fact]
        public void Tokenize_SingleAsterisk_ReturnsMultiplication()
        {
            var result = tokenizer.Tokenize("2 * 3");
            var opToken = result.FirstOrDefault(t => t.Type == TokenType.OPERATOR && t.Value.Contains("*"));
            Assert.NotNull(opToken);
            Assert.Equal("*", opToken.Value);
        }

        [Fact]
        public void Tokenize_DoubleAsterisk_ReturnsExponentiation()
        {
            var result = tokenizer.Tokenize("2 ** 3");
            var opToken = result.FirstOrDefault(t => t.Type == TokenType.OPERATOR && t.Value == "**");
            Assert.NotNull(opToken);
            Assert.Equal("**", opToken.Value);
        }

        [Fact]
        public void Tokenize_MixedAsteriskOperators_ReturnsCorrectTokens()
        {
            var result = tokenizer.Tokenize("a * b ** c");
            var opTokens = result.Where(t => t.Type == TokenType.OPERATOR && t.Value.Contains("*")).ToList();
            Assert.Equal(2, opTokens.Count);
            Assert.Equal("*", opTokens[0].Value);
            Assert.Equal("**", opTokens[1].Value);
        }

        #endregion

        #region Comprehensive Multi-Token Tests

        [Theory]
        [InlineData("return x + y", 4)]
        [InlineData("return (a * b)", 6)]
        [InlineData("return result", 2)]
        public void Tokenize_ReturnStatements_ReturnsCorrectCount(string input, int expectedCount)
        {
            var result = tokenizer.Tokenize(input);
            Assert.Equal(expectedCount, result.Count);
            Assert.Contains(result, t => t.Type == TokenType.RETURN);
        }

        [Fact]
        public void Tokenize_ComplexNestedExpression_ReturnsCorrectSequence()
        {
            var result = tokenizer.Tokenize("{ x := ((a + b) * (c - d)) // e }");
            Assert.Equal(19, result.Count);
            Assert.Equal(TokenType.LEFT_CURLY, result[0].Type);
            Assert.Equal(TokenType.RIGHT_CURLY, result[18].Type);
        }

        #endregion
    }
}