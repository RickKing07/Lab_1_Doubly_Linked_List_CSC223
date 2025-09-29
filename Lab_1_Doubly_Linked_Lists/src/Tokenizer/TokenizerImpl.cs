//using TokenConstants?
public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        List<Token> TokenList = [];
        int index = 0;
        while (index < input.Length())
        {
            if (char.IsDigit(input[index]))
            {
                TokenList.Add(PrimitiveH(input[index]));
            }

        }
        //Determine Token type
        //call appropriate helper method that returns a token
        //add token to big list
        //return bit list<Token>
    }

    public Token PrimitiveH(char letter)
    {
        //determine type (VARIABLE or RETURN)
        //Feed it to token init with char && type
        //return Token
    }

    public Token LiteralH(char letter)
    {
        //determine type (INTEGER or FLOAT)
        //Feed it to token init with char && type
        //return Token
    }

    public Token OperatorsH(char letter)
    {
        //determine type (Operator or ASSIGMENT)
        //Feed it to token init with char && type
        //return Token
    }

    public Token StructureH(char letter)
    {
        //determine type (LEFT_PAREN or RIGHT_CURLY ect.)
        //Feed it to token init with char && type
        //return Token
    }
}