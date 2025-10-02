using Tokenizer;
using Xunit.Abstractions;
using Xunit.Sdk;

public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        List<Token> TokenList = [];
        int index = 0;

        while (index < input.Length)
        {
            char currentChar = input[index];

            if (char.IsWhiteSpace(currentChar)){ index++; }
            else if (char.IsDigit(currentChar)){ TokenList.Add(LiteralH(input, ref index)); }
            else if (char.IsLetter(currentChar)) { TokenList.Add(PrimitiveH(input, ref index)); }
            else if (new[] {
                TokenConstants.LEFT_CURLY,
                TokenConstants.RIGHT_CURLY,
                TokenConstants.LEFT_PAREN,
                TokenConstants.RIGHT_PAREN
            }.Contains(currentChar.ToString()))
            {
                TokenList.Add(StructureH(currentChar.ToString()));
                index++;
            }
            else{ TokenList.Add(OperatorsH(input, ref index)); }
        }
        return TokenList;
    }



    //Determine Token type
        //call appropriate helper method that returns a token
        //add token to big list
        //return bit list<Token>


    private Token PrimitiveH(string input, ref int index)
    {
        string item = "";
        while (index < input.Length && char.IsLetter(input[index]))
        {
            item = item + input[index];
            index++;
        }
        if (item == TokenConstants.RETURN)
        {
            return new Token(item, TokenType.RETURN);
        }
        else return new Token(item, TokenType.VARIABLE);

        throw new ArgumentException("Unexpected input");
    }



    private Token LiteralH(string input, ref int index)
    {
        string item = "";
        while ((index < input.Length) && char.IsDigit(input[index]))
        {
            item = item + input[index];
            index++;
        }

        if (index < input.Length && input[index].ToString() == TokenConstants.DECIMAL_POINT)
        {
            item += TokenConstants.DECIMAL_POINT;
            index++;

            while ((index < input.Length) && char.IsDigit(input[index]))
            {
                item = item + input[index];
                index++;
            }
            return new Token(item, TokenType.FLOAT);
        }
        return new Token(item, TokenType.INTEGER);

        throw new ArgumentException("Unexpected String");

    }

    private Token OperatorsH(string input, ref int index)
    {
        // handle "**" else print indiudally in the test
        if (input[index] == '*' &&
            index + 1 < input.Length && input[index + 1] == '*')
        {
            index += 2;
            return new Token(TokenConstants.EXP, TokenType.OPERATOR);
        }

        // handle "//" else individually in test
        if (input[index] == '/' &&
            index + 1 < input.Length && input[index + 1] == '/')
        {
            index += 2;
            return new Token(TokenConstants.FLOAT_DIV, TokenType.OPERATOR);
        }

        // handle ":="
        if (input[index] == ':' &&
            index + 1 < input.Length && input[index + 1] == '=')
        {
            index += 2;
            return new Token(TokenConstants.ASSIGMENT, TokenType.ASSIGNMENT);
        }

        // single-char operators
        string element = input[index].ToString();
        if (new[] {
                TokenConstants.PLUS,
                TokenConstants.MINUS,
                TokenConstants.TIMES,
                TokenConstants.MOD,
                TokenConstants.INT_DIV
            }.Contains(element))
        {
            index++;
            return new Token(element, TokenType.OPERATOR);
        }

        throw new ArgumentException("Unexpected String");
    }


    private Token StructureH(string letter)
    {
        if (letter == TokenConstants.LEFT_CURLY) { return new Token(letter, TokenType.LEFT_CURLY); }
        else if (letter == TokenConstants.RIGHT_CURLY) { return new Token(letter, TokenType.RIGHT_CURLY); }
        else if (letter == TokenConstants.LEFT_PAREN) { return new Token(letter, TokenType.LEFT_PAREN); }
        else if (letter == TokenConstants.RIGHT_PAREN) { return new Token(letter, TokenType.RIGHT_PAREN); }
        else { throw new ArgumentException("Unexpected String"); }
    }
}