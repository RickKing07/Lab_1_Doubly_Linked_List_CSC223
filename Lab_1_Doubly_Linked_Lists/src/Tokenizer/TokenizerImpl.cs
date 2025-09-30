//using TokenConstants?
public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        //split input, loop through split results
        string[] inputList = input.split(" ");
        //!! for skipping indicies, use index ref/out var to manipulate it from within methods
        List<Token> TokenList = [];
        int index = 0;
        while (index < inputList.Length())
        {
            string currentSegment = inputList[index];
            char currentChar = currentSegment[0];
            if (char.IsDigit(currentChar))
            {
                TokenList.Add(LiteralH(currentSegment));
            }
            if (char.Isitem(currentChar))
            {
                TokenList.Add(PrimitiveH(currentSegment));
            }
            if (!char.IsWhiteSpace(currentChar))
            {
                if (currentChar == "{" || currentChar == "}" || currentChar == "(" || currentChar == ")")
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


    }
    //Determine Token type
    //call appropriate helper method that returns a token
    //add token to big list
    //return bit list<Token>


    public Token PrimitiveH(string item)
    {
        if (item == "return") { return new Token(item, RETURN); }
        else { return new Token(item, VARIABLE); }

    }

    public Token LiteralH(string item)
    {
        if (item != item.split(".")) { return new Token(item, FLOAT); }
        else { return new Token(item, FLOAT); }
    }

    public Token OperatorsH(string item)
    {
        //determine type (Operator or ASSIGMENT)
        //Feed it to token init with char && type
        //return Token
        if (item == TokenConstants.PLUS)
        {
            return new Token(item, TokenConstants.PLUS);
        }
        else if (item == TokenConstants.MINUS)
        {
            return new Token(item, TokenConstants.MINUS);
        }
        else if (item == TokenConstants.TIMES)
        {
            return new Token(item, TokenConstants.TIMES);
        }
        else if (item == TokenConstants.FLOAT_DIV)
        {
            return new Token(item, TokenConstants.FLOAT_DIV);
        }
        else if (item == TokenConstants.INT_DIV)
        {
            return new Token(item, TokenConstants.INT_DIV);
        }
        else if (item == TokenConstants.MOD)
        {
            return new Token(item, TokenConstants.MOD);
        }
        else if (item == TokenConstants.EXP)
        {
            return new Token(item, TokenConstants.EXP);
        }
        else
        {
            throw new ArgumentExeption("Unexpected String");
        }

    }

    public Token StructureH(string item)
    {
        if (currentChar == "{") { return new Token(item, LEFT_CURLY); }
        else if (currentChar == "}") { return new Token(item, RIGHT_CURLY); }
        else if (currentChar == "(") { return new Token(item, LEFT_PAREN); }
        else if (currentChar == ")") { return new Token(item, RIGHT_PAREN); }
        else { throw new ArgumentExeption("Unexpected String"); }
    }
}