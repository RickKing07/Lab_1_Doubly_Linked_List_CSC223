using Tokenizer;
public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        //split input, loop through split results
        string[] inputList = input.Split(" ");
        //!! for skipping indicies, use index ref/out var to manipulate it from within methods
        List<Token> TokenList = [];
        int index = 0;
        while (index < inputList.Length)
        {
            string currentSegment = inputList[index];
            char currentChar = currentSegment[0];
            if (char.IsDigit(currentChar))
            {
                TokenList.Add(LiteralH(currentSegment));
            }
            if (char.IsLetter(currentChar))
            {
                TokenList.Add(PrimitiveH(currentSegment));
            }
            if (!char.IsWhiteSpace(currentChar))
            {
                if (currentSegment == "{" || currentSegment == "}" || currentSegment == "(" || currentSegment == ")")
                {
                    TokenList.Add(StructureH(currentSegment));
                }

                else
                {
                    TokenList.Add(OperatorsH(currentSegment));
                }
            }
            index++;
        }
        return TokenList;

    }
    //Determine Token type
    //call appropriate helper method that returns a token
    //add token to big list
    //return bit list<Token>


    public Token PrimitiveH(string item)
    {
        if (item == "return") { return new Token(item, TokenType.RETURN); }
        else { return new Token(item, TokenType.VARIABLE); }

    }

    public Token LiteralH(string item)
    {
        if (item != item.Split(".")[0]) { return new Token(item, TokenType.FLOAT); }
        else { return new Token(item, TokenType.FLOAT); }
    }

    public Token OperatorsH(string item)
    {
        //determine type (Operator or ASSIGMENT)
        //Feed it to token init with char && type
        //return Token
        if (item == TokenConstants.PLUS)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else if (item == TokenConstants.MINUS)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else if (item == TokenConstants.TIMES)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else if (item == TokenConstants.FLOAT_DIV)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else if (item == TokenConstants.INT_DIV)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else if (item == TokenConstants.MOD)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else if (item == TokenConstants.EXP)
        {
            return new Token(item, TokenType.OPERATOR);
        }
        else
        {
            throw new ArgumentException("Unexpected String");
        }

    }

    public Token StructureH(string item)
    {
        if (item == "{") { return new Token(item, TokenType.LEFT_CURLY); }
        else if (item == "}") { return new Token(item, TokenType.RIGHT_CURLY); }
        else if (item == "(") { return new Token(item, TokenType.LEFT_PAREN); }
        else if (item == ")") { return new Token(item, TokenType.RIGHT_PAREN); }
        else { throw new ArgumentException("Unexpected String"); }
    }
}