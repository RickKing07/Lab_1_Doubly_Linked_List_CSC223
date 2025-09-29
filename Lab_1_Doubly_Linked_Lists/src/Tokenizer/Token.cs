using System;

namespace Tokenizer
{
    //enum and class def

    public enum TokenType
    {
        RETURN,
        VARIABLE,
        INT,
        FLOAT,
        ASSIGMENT,
        OPERATOR,
        LEFT_PAEREN,
        RIGHT_PAREN,
        LEFT_CURLY,
        RIGHT_CURLY
    }
}
public static class TokenConstants
{
    //Plus, LEFT_PAEREN, LEFT_CURLY, ASSIGMENT, DECIMAL_POINT contstatns defined

    //TokenType ASSIGMENT PLUS = "+"; //check if this is correct
}
public class Token(char x) //better name -_-
{
    //aggregating structure that gets tokentype and value of a token
    public TokenType Type { get; set; }
    public char Value { get; set; } //might could be readonly
                                    //standard class constructors like to string, equals, and more
}




