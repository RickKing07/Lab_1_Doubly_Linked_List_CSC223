//using TokenConstants?
public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        //split input, loop through split results
        inputList = input.split(" ");

        //!! for skipping indicies, use index ref/out var to manipulate it from within methods
        List<Token> TokenList = [];
        int index = 0;
        while (index < inputList.Length())
        {
            currentChar = input[index];
            if (char.IsDigit(currentChar))
            {
                TokenList.Add(LiteralH(currentChar));
            }
            if (char.IsLetter(currentChar))
            {
                TokenList.Add(PrimitiveH(currentChar));
            }
            if (!char.IsWhiteSpace(currentChar))
            {
                if (currentChar == null) //big equals chain here, not sure thats good code
                {
                    TokenList.Add(OperatorsH(currentChar));
                }
                //could just be else i think
                else if (currentChar == "{" || currentChar == "}" || currentChar == "(" || currentChar == ")")
                {
                    TokenList.Add(StructureH(currentChar));
                }
            }
        }

        index++;
    }
    //Determine Token type
    //call appropriate helper method that returns a token
    //add token to big list
    //return bit list<Token>


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