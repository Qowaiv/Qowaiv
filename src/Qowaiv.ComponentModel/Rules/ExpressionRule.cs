using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Qowaiv.ComponentModel.Rules
{
    public static class ExpressionRule
    {
        public static string GetMemberName(LambdaExpression expression)
        {
            Guard.NotNull(expression, nameof(expression));

            switch (expression.Body)
            {
                case MemberExpression m:
                    return m.Member.Name;
                case UnaryExpression u when u.Operand is MemberExpression m:
                    return m.Member.Name;

                default:
                    throw new NotSupportedException();
            }

        }
    }
}
