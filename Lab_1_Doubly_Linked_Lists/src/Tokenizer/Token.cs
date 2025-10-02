using System;
namespace Tokenizer
{
    //enum and class def

    public enum TokenType
    {
        RETURN,
        VARIABLE,
        INTEGER,
        FLOAT,
        ASSIGNMENT,
        OPERATOR,
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_CURLY,
        RIGHT_CURLY
    }

    public static class TokenConstants
    {
        //Plus, LEFT_PAEREN, LEFT_CURLY, ASSIGMENT, DECIMAL_POINT contstatns defined

        public const string PLUS = "+";
        public const string MINUS = "-";
        public const string TIMES = "*";
        public const string FLOAT_DIV = "//";
        public const string INT_DIV = "/";
        public const string MOD = "%";
        public const string EXP = "**";
        public const string LEFT_PAREN = "(";
        public const string LEFT_CURLY = "{";
        public const string ASSIGMENT = ":=";
        public const string DECIMAL_POINT = ".";
        public const string RIGHT_PAREN = ")";
        public const string RIGHT_CURLY = "}";
        public const string RETURN = "return";

    }
    public class Token
    {
        public string _value { get; set; }
        public TokenType _tkntype { get; set; }

        public Token(string tkn, TokenType type)
        {
            _value = tkn;
            _tkntype = type;
        }

        public void ToString(Token tkn)
        {
            Console.WriteLine($"This token has the value {_value} and the type of {_tkntype}"); //print tostring, prob dosnt work rn
        }

        public bool Equals(Token tkn1, Token tkn2)
        {
            return tkn1._value == tkn2._value;
        }


    }
}