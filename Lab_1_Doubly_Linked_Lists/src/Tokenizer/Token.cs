using System;

namespace Tokenizer
{
    //enum and class def

    public class TokenType()
    {
        public enum PrimitiveTypes
        {
            //Keywords & Variables, should it just be VAR & Return?
            RETURN,
            IF,
            ELSE,
            WHILE,
            FOR,
            VAR,
            STRING,
            BOOL,
            VOID
        }

        public enum Literals
        {
            //Integer & floats

            INT,
            FLOAT
        }

        public enum Operators
        // Assigment, + - * / // %,  to OPERATOR or ASSIGMENT?
        {
            ASSIGMENT,
            PLUS,
            MINUS,
            TIMES,
            FLOAT_DIVISION,
            INT_DIVISION,
            MOD,
            EXPONENT
        }

        public enum StructureAndGrouping
        //Parentheses & curly braces to LEFT_PAREN, RIGHT_PAREN, LEFT_CURLY, RIGHT_CURLY
        {
            LEFT_PAEREN,
            RIGHT_PAREN,
            LEFT_CURLY,
            RIGHT_CURLY
        }
    }
    public static class TokenConstants
    {
        //Plus, LEFT_PAEREN, LEFT_CURLY, ASSIGMENT, DECIMAL_POINT contstatns defined

        TokenType.Operators PLUS = "+"; //check if this is correct
    }
    public class Token(char x) //better name -_-
    {
        //aggregating structure that gets tokentype and value of a token

        public TokenType Type { get; set; }
        public char Value { get; set; } //might could be readonly
        //standard class constructors like to string, equals, and more
    }
}



