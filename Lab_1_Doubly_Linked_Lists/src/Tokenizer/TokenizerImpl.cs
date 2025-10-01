using Tokenizer;
using Xunit.Abstractions;
using Xunit.Sdk;

public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        //!! for skipping indicies, use index ref/out var to manipulate it from within methods
        List<Token> TokenList = [];
        int index = 0;
        while (index < input.Length)
        {
            char currentChar = input[index];

            if (char.IsWhiteSpace(currentChar)) { continue; }

            else if (char.IsDigit(currentChar)) { TokenList.Add(LiteralH(input, ref index)); }
            else if (char.IsLetter(currentChar)) { TokenList.Add(PrimitiveH(input, ref index)); }

            else if (new[] { TokenConstants.LEFT_CURLY, TokenConstants.RIGHT_CURLY, TokenConstants.LEFT_PAREN, TokenConstants.RIGHT_PAREN } //
            .Contains(currentChar.ToString())) { TokenList.Add(StructureH(currentChar.ToString())); }

            else { TokenList.Add(OperatorsH(input, ref index)); }
            index++;
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

        if (input[index].ToString() == TokenConstants.DECIMAL_POINT)
        {
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
        //determine type (Operator or ASSIGMENT)
        //Feed it to token init with char && type
        // //return Token
        // if (input == TokenConstants.PLUS) { return new Token(input, TokenType.OPERATOR); } style changed with .Contains(input)
        if (new[] {
                TokenConstants.PLUS,
                TokenConstants.MINUS,
                TokenConstants.TIMES,
                TokenConstants.MOD,
                TokenConstants.EXP
            }.Contains(input))
        { return new Token(input, TokenType.OPERATOR); }

        else if (input == TokenConstants.INT_DIV)
        {
            if (input[index + 1] == '/')
            {
                string item = TokenConstants.FLOAT_DIV;
                index += 2;
                return new Token(item, TokenType.OPERATOR);
            }
            return new Token(input, TokenType.OPERATOR);
        }

        else if (input == ":")
        {
            string item = TokenConstants.ASSIGMENT;
            index = index + 2;
            return new Token(item, TokenType.ASSIGNMENT);
        }

        else { throw new ArgumentException("Unexpected String"); }
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