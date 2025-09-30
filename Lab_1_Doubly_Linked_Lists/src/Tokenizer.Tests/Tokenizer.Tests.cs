using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Tokenizer; // assumes TokenType, Token, TokenConstants, TokenizerImpl live in this namespace

public class TokenizerImplTests
{
    private static List<Token> Tok(string src)
    {
        var tz = new TokenizerImpl();
        return tz.Tokenize(src);
    }

    private static (TokenType, string)[] Simplify(IEnumerable<Token> tokens)
        => tokens.Select(t => (t._tkntype, t._value)).ToArray();

    // ---------- BASIC SCAN: WHITESPACE & EMPTY ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t \n  \r\n")]
    public void EmptyOrWhitespace_ProducesNoTokens(string src)
    {
        var tokens = Tok(src);
        Assert.Empty(tokens);
    }

    // ---------- VARIABLE vs KEYWORD ----------
    [Theory]
    [InlineData("x", TokenType.VARIABLE, "x")]
    [InlineData("abc", TokenType.VARIABLE, "abc")]
    [InlineData("return", TokenType.RETURN, "return")]
    [InlineData("returnx", TokenType.VARIABLE, "returnx")] // not keyword
    public void Identifiers_And_Keyword_AreRecognized(string src, TokenType expectedType, string expectedVal)
    {
        var tokens = Tok(src);
        var one = Assert.Single(tokens);
        Assert.Equal(expectedType, one._tkntype);
        Assert.Equal(expectedVal, one._value);
    }

    [Theory]
    [InlineData("X")]        // uppercase not allowed
    [InlineData("fooBar")]   // uppercase part not allowed
    [InlineData("a1")]       // digits inside not allowed
    [InlineData("_a")]       // underscore not allowed
    [InlineData("a_b")]      // underscore not allowed
    public void Invalid_Identifiers_ShouldThrow(string src)
    {
        Assert.Throws<ArgumentException>(() => Tok(src));
    }

    // ---------- INTEGERS ----------
    [Theory]
    [InlineData("0", "0")]
    [InlineData("5", "5")]
    [InlineData("42", "42")]
    [InlineData("007", "007")] // leading zeros ok as a lexical integer
    public void Integers_AreRecognized(string src, string val)
    {
        var tokens = Tok(src);
        var one = Assert.Single(tokens);
        Assert.Equal(TokenType.INTEGER, one._tkntype);
        Assert.Equal(val, one._value);
    }

    // ---------- FLOATS ----------
    [Theory]
    [InlineData("0.0", "0.0")]
    [InlineData("1.234", "1.234")]
    [InlineData("10.5", "10.5")]
    public void Floats_AreRecognized(string src, string val)
    {
        var tokens = Tok(src);
        var one = Assert.Single(tokens);
        Assert.Equal(TokenType.FLOAT, one._tkntype);
        Assert.Equal(val, one._value);
    }

    [Theory]
    [InlineData(".5")]   // no leading digits
    [InlineData("10.")]  // no trailing digits
    [InlineData("1..2")] // double dot
    [InlineData("1.2.3")]
    public void Malformed_Floats_ShouldThrow(string src)
    {
        Assert.Throws<ArgumentException>(() => Tok(src));
    }

    // ---------- NEGATIVE NUMBERS (as MINUS + NUMBER) ----------
    [Theory]
    [InlineData("-1")]
    [InlineData("-0.5")]
    [InlineData("x-1")]
    public void NegativeNumbers_AppearAsMinusThenPositiveNumber(string src)
    {
        var tokens = Tok(src);
        var simplified = Simplify(tokens);

        if (src == "-1")
        {
            Assert.Equal(new[] {
                (TokenType.OPERATOR, "-"),
                (TokenType.INTEGER, "1")
            }, simplified);
        }
        else if (src == "-0.5")
        {
            Assert.Equal(new[] {
                (TokenType.OPERATOR, "-"),
                (TokenType.FLOAT, "0.5")
            }, simplified);
        }
        else if (src == "x-1")
        {
            Assert.Equal(new[] {
                (TokenType.VARIABLE, "x"),
                (TokenType.OPERATOR, "-"),
                (TokenType.INTEGER, "1")
            }, simplified);
        }
    }

    // ---------- OPERATORS: single-char ----------
    [Theory]
    [InlineData("+", "+")]
    [InlineData("-", "-")]
    [InlineData("*", "*")]
    [InlineData("/", "/")] // float division
    [InlineData("%", "%")]
    [InlineData("^", "^")] // exponent
    public void SingleCharOperators_AreRecognized_AsOperator(string src, string lexeme)
    {
        var tokens = Tok(src);
        var one = Assert.Single(tokens);
        Assert.Equal(TokenType.OPERATOR, one._tkntype);
        Assert.Equal(lexeme, one._value);
    }

    // ---------- OPERATORS: multi-char ----------
    [Theory]
    [InlineData("//")]
    [InlineData(":=")]
    public void MultiCharOperators_AreRecognized_AsSingleOperatorToken(string src)
    {
        var tokens = Tok(src);
        var one = Assert.Single(tokens);
        if (src == ":=")
            Assert.Equal(TokenType.ASSIGNMENT, one._tkntype);
        else
            Assert.Equal(TokenType.OPERATOR, one._tkntype);

        Assert.Equal(src, one._value);
    }

    // ---------- DISAMBIGUATION: / vs // ----------
    [Theory]
    [InlineData("/ x")]
    [InlineData("// x")]
    [InlineData("a//b")]
    [InlineData("a/b")]
    public void SlashVariants_AreDisambiguatedCorrectly(string src)
    {
        var tokens = Tok(src);
        var simplified = Simplify(tokens);

        if (src == "/ x")
        {
            Assert.Equal(new[] {
                (TokenType.OPERATOR, "/"),
                (TokenType.VARIABLE, "x")
            }, simplified);
        }
        else if (src == "// x")
        {
            Assert.Equal(new[] {
                (TokenType.OPERATOR, "//"),
                (TokenType.VARIABLE, "x")
            }, simplified);
        }
        else if (src == "a//b")
        {
            Assert.Equal(new[] {
                (TokenType.VARIABLE, "a"),
                (TokenType.OPERATOR, "//"),
                (TokenType.VARIABLE, "b")
            }, simplified);
        }
        else if (src == "a/b")
        {
            Assert.Equal(new[] {
                (TokenType.VARIABLE, "a"),
                (TokenType.OPERATOR, "/"),
                (TokenType.VARIABLE, "b")
            }, simplified);
        }
    }

    // ---------- GROUPING ----------
    [Theory]
    [InlineData("(", TokenType.LEFT_PAREN, "(")]
    [InlineData(")", TokenType.RIGHT_PAREN, ")")]
    [InlineData("{", TokenType.LEFT_CURLY, "{")]
    [InlineData("}", TokenType.RIGHT_CURLY, "}")]
    public void GroupingTokens_AreRecognized(string src, TokenType expectedType, string expectedVal)
    {
        var tokens = Tok(src);
        var one = Assert.Single(tokens);
        Assert.Equal(expectedType, one._tkntype);
        Assert.Equal(expectedVal, one._value);
    }

    // ---------- FULL STATEMENT from the spec: x := (1 + 2) ----------
    [Fact]
    public void ExampleFromSpec_XAssignParen1Plus2Paren()
    {
        var src = "x := (1 + 2)";
        var tokens = Tok(src);

        var expected = new (TokenType, string)[]
        {
            (TokenType.VARIABLE, "x"),
            (TokenType.ASSIGNMENT, ":="),
            (TokenType.LEFT_PAREN, "("),
            (TokenType.INTEGER, "1"),
            (TokenType.OPERATOR, "+"),
            (TokenType.INTEGER, "2"),
            (TokenType.RIGHT_PAREN, ")")
        };

        Assert.Equal(expected, Simplify(tokens));
    }

    // ---------- MIXED: returns, arithmetic, grouping ----------
    [Theory]
    [InlineData("return 0")]
    [InlineData("sum:=a+b*c")]
    [InlineData("{x:=10//3}")]
    [InlineData("(2^3)%3")]
    [InlineData("y:=10/4.0")]
    public void MixedPrograms_AreTokenizedCorrectly(string src)
    {
        var tokens = Tok(src);
        var simplified = Simplify(tokens);

        if (src == "return 0")
        {
            Assert.Equal(new[] {
                (TokenType.RETURN, "return"),
                (TokenType.INTEGER, "0")
            }, simplified);
        }
        else if (src == "sum:=a+b*c")
        {
            Assert.Equal(new[] {
                (TokenType.VARIABLE, "sum"),
                (TokenType.ASSIGNMENT, ":="),
                (TokenType.VARIABLE, "a"),
                (TokenType.OPERATOR, "+"),
                (TokenType.VARIABLE, "b"),
                (TokenType.OPERATOR, "*"),
                (TokenType.VARIABLE, "c")
            }, simplified);
        }
        else if (src == "{x:=10//3}")
        {
            Assert.Equal(new[] {
                (TokenType.LEFT_CURLY, "{"),
                (TokenType.VARIABLE, "x"),
                (TokenType.ASSIGNMENT, ":="),
                (TokenType.INTEGER, "10"),
                (TokenType.OPERATOR, "//"),
                (TokenType.INTEGER, "3"),
                (TokenType.RIGHT_CURLY, "}")
            }, simplified);
        }
        else if (src == "(2^3)%3")
        {
            Assert.Equal(new[] {
                (TokenType.LEFT_PAREN, "("),
                (TokenType.INTEGER, "2"),
                (TokenType.OPERATOR, "^"),
                (TokenType.INTEGER, "3"),
                (TokenType.RIGHT_PAREN, ")"),
                (TokenType.OPERATOR, "%"),
                (TokenType.INTEGER, "3")
            }, simplified);
        }
        else if (src == "y:=10/4.0")
        {
            Assert.Equal(new[] {
                (TokenType.VARIABLE, "y"),
                (TokenType.ASSIGNMENT, ":="),
                (TokenType.INTEGER, "10"),
                (TokenType.OPERATOR, "/"),
                (TokenType.FLOAT, "4.0")
            }, simplified);
        }
    }

    // ---------- DECIMAL POINT constant should only appear within floats ----------
    [Theory]
    [InlineData(".")]
    [InlineData("..")]
    [InlineData("a.")]
    public void BareDecimalPointOrDangling_ShouldThrow(string src)
    {
        Assert.Throws<ArgumentException>(() => Tok(src));
    }

    // ---------- UNSUPPORTED CHARACTERS ----------
    [Theory]
    [InlineData("@")]
    [InlineData("#")]
    [InlineData("\"")]
    [InlineData("\\")]
    public void UnsupportedCharacters_ShouldThrow(string src)
    {
        Assert.Throws<ArgumentException>(() => Tok(src));
    }

    // ---------- WHITESPACE INSIDE ----------
    [Theory]
    [InlineData("  x   :=   1  ")]
    [InlineData("\treturn\t(  0  )\n")]
    public void Whitespace_IsIgnored(string src)
    {
        var tokens = Tok(src);
        var simplified = Simplify(tokens);

        if (src.Contains("x"))
        {
            Assert.Equal(new[] {
                (TokenType.VARIABLE, "x"),
                (TokenType.ASSIGNMENT, ":="),
                (TokenType.INTEGER, "1")
            }, simplified);
        }
        else
        {
            Assert.Equal(new[] {
                (TokenType.RETURN, "return"),
                (TokenType.LEFT_PAREN, "("),
                (TokenType.INTEGER, "0"),
                (TokenType.RIGHT_PAREN, ")")
            }, simplified);
        }
    }
}
