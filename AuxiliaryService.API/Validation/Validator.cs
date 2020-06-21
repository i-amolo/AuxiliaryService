using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Validation
{
    public static class Validator
    {
        private static string GetName(Expression<Func<object>> expr) 
        {
            if (expr.Body is MemberExpression)
            {
                return ((MemberExpression)expr.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expr.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }

        public static void NotNull(Expression<Func<object>> expr, string userMessage = null) 
        {
            var value = expr.Compile()();

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                var name = GetName(expr);
                throw new Exception($"Argument {name.ToUpper()} can not be null or empty." + (userMessage != null ?  $" User message: {userMessage}" : string.Empty));
            }
        }




    }
}
