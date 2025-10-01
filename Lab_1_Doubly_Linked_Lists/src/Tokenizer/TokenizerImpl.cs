using Tokenizer;
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
            if (char.IsDigit(currentChar)) { TokenList.Add(LiteralH(input, index)); }
            if (char.IsLetter(currentChar)) { TokenList.Add(PrimitiveH(input, index)); }
            if (currentChar == "{" || currentChar == "}" || currentChar == "(" || currentChar == ")") { TokenList.Add(StructureH(currentChar)); }
            else { TokenList.Add(OperatorsH(input, index)); }
            index++;
        }

        return TokenList;
    }


    //Determine Token type
    //call appropriate helper method that returns a token
    //add token to big list
    //return bit list<Token>


    public Token PrimitiveH(string input, ref int index)
    {
        string item = "";
        while (char.IsLetter(input[index]))
        {
            item = item + input[index];
            index++;
        }
        if (item == "return") { return new Token(item, TokenType.RETURN); }
        else return new Token(item, TokenType.VARIABLE);
    }



    public Token LiteralH(string input, ref int index)
    {
        string item = "";
        while (char.IsDigit(input[index]))
        {
            item = item + input[index];
            index++;
        }
        if (input[index] == ".")
        {
            while (char.IsDigit(input[index]))
            {
                item = item + input[index];
                index++;
            }
            return new Token(item, TokenType.FLOAT);
        }
        else return new Token(item, TokenType.INTEGER);

    }

    public Token OperatorsH(string input, ref int index)
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

    public Token StructureH(char letter)
    {
        if (item == "{") { return new Token(item, TokenType.LEFT_CURLY); }
        else if (item == "}") { return new Token(item, TokenType.RIGHT_CURLY); }
        else if (item == "(") { return new Token(item, TokenType.LEFT_PAREN); }
        else if (item == ")") { return new Token(item, TokenType.RIGHT_PAREN); }
        else { throw new ArgumentException("Unexpected String"); }
    }
}