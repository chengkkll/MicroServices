using System;
using System.Text;
using System.Linq.Expressions;

namespace TianCheng.Common
{
    /// <summary>
    /// 表达式解析辅助类
    /// </summary>
    public class AnalyseExpressionHelper : ExpressionVisitor
    {
        private readonly StringBuilder express = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        public string Result { get { return express.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public void AnalyseExpression<T>(Expression<Func<T, bool>> expression)
        {
            Visit(expression.Body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public void AnalyseExpression<T>(Expression<Func<T, T>> expression)
        {
            Visit(expression.Body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.OrElse)
                express.Append("(");
            Visit(node.Left);
            express.Append($" {node.NodeType.TransferOperand()} ");
            Visit(node.Right);
            if (node.NodeType == ExpressionType.OrElse)
                express.Append(")");
            return node;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type.IsValueType && node.Type != typeof(DateTime))
            {
                express.Append(node.Value);
            }
            else
            {
                express.Append($"'{node.Value}'");
            }
            return node;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            express.Append(node.Member.Name);
            return node;
        }
    }
}
