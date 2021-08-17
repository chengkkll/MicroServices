using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TianCheng.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExpressionExtend
    {
        /// <summary>
        /// 操作符转换
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TransferOperand(this ExpressionType type)
        {
            string operand = string.Empty;
            switch (type)
            {
                case ExpressionType.AndAlso:
                    operand = "AND";
                    break;
                case ExpressionType.OrElse:
                    operand = "OR";
                    break;
                case ExpressionType.Equal:
                    operand = "=";
                    break;
                case ExpressionType.NotEqual:
                    operand = "<>";
                    break;
                case ExpressionType.LessThan:
                    operand = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    operand = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    operand = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    operand = ">=";
                    break;
            }
            return operand;
        }
    }
}
