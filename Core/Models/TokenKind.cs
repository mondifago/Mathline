using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public enum TokenKind
    {
        Number,
        Variable,
        Plus,
        Minus,
        Star,
        Caret,
        Equals
    }

    public record Token(TokenKind Kind, string Text, int Position);
}
