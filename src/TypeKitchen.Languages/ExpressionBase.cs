﻿using System.Text;

namespace TypeKitchen.Languages
{
    public abstract class ExpressionBase : IExpression
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb);
            return sb.ToString();
        }

        public abstract void Print(StringBuilder sb);
    }
}