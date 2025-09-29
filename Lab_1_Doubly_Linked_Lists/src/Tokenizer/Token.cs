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
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_CURLY,
        RIGHT_CURLY
    }

    public static class TokenConstants
    {
        //Plus, LEFT_PAEREN, LEFT_CURLY, ASSIGMENT, DECIMAL_POINT contstatns defined

        const string PLUS = "+";
        const string MINUS = "-";
        const string TIMES = "*";
        const string FLOAT_DIV = "//";
        const string INT_DIV = "/";
        const string MOD = "%";
        const string EXP = "**";
        const string LEFT_PAREN = "(";
        const string LEFT_CURLY = "{";
        const string ASSIGMENT = ":=";
        const string DECIMAL_POINT = ".";
        const string RIGHT_PAREN = ")";
        const string RIGHT_CURLY = "}";

    }
    public class Token
    {
        public char _value;
        public TokenType _tkntype;

        public Token(char tkn, TokenType type)
        {
            _value = tkn;
            _tkntype = type;
        }

        public string ToString(Token tkn)
        {
            Console.WriteLine($"This token has the value {_value} and the type of {_tkntype}"); //print tostring, prob dosnt work rn
        }

        public bool Equals(Token tkn1, Token tkn2)
        {
            return tkn1._value == tkn2._value; //use equality comparer???
        }


    }
}