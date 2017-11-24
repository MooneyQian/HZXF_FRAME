using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Mamage.Framework;

namespace Manage.Framework
{
    /// <summary>
    /// lamdal表达式转sql语句
    /// </summary>
    public class ExpressionToSql
    {
        public static bool In<T>(T obj, T[] array)
        {
            return true;
        }

        public static bool NotIn<T>(T obj, T[] array)
        {
            return true;
        }

        public static bool Like(string str, string likeStr)
        {
            return true;
        }

        public static bool NotLike(string str, string likeStr)
        {
            return true;
        }

        public string Where<T>(Expression<Func<T, bool>> func) where T : class, new()
        {
            string result;
            if (func.Body is BinaryExpression)
            {
                BinaryExpression binaryExpression = (BinaryExpression)func.Body;
                result = this.BinarExpressionProvider(binaryExpression.Left, binaryExpression.Right, binaryExpression.NodeType);
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        private string BinarExpressionProvider(Expression left, Expression right, ExpressionType type)
        {
            string text = "(";
            left = PartialEvaluator.Eval(left);
            text += this.ExpressionRouter(left);
            text += this.ExpressionTypeCast(type);
            right = PartialEvaluator.Eval(right);
            string text2 = this.ExpressionRouter(right);
            if (text2 == "null")
            {
                if (text.EndsWith(" ="))
                {
                    text = text.Substring(0, text.Length - 2) + " is null";
                }
                else if (text.EndsWith("<>"))
                {
                    text = text.Substring(0, text.Length - 2) + " is not null";
                }
            }
            else
            {
                text += text2;
            }
            return text + ")";
        }

        private string ExpressionRouter(Expression exp)
        {
            string empty = string.Empty;
            string result;
            if (exp is BinaryExpression)
            {
                BinaryExpression binaryExpression = (BinaryExpression)exp;
                result = this.BinarExpressionProvider(binaryExpression.Left, binaryExpression.Right, binaryExpression.NodeType);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)exp;
                result = memberExpression.Member.Name;
            }
            else if (exp is NewArrayExpression)
            {
                NewArrayExpression newArrayExpression = (NewArrayExpression)exp;
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Expression current in newArrayExpression.Expressions)
                {
                    stringBuilder.Append(this.ExpressionRouter(current));
                    stringBuilder.Append(",");
                }
                result = stringBuilder.ToString(0, stringBuilder.Length - 1);
            }
            else
            {
                if (exp is MethodCallExpression)
                {
                    MethodCallExpression methodCallExpression = (MethodCallExpression)exp;
                    if (methodCallExpression.Method.Name == "Like")
                    {
                        result = string.Format("({0} like {1})", this.ExpressionRouter(methodCallExpression.Arguments[0]), this.ExpressionRouter(methodCallExpression.Arguments[1]));
                        return result;
                    }
                    if (methodCallExpression.Method.Name == "NotLike")
                    {
                        result = string.Format("({0} Not like {1})", this.ExpressionRouter(methodCallExpression.Arguments[0]), this.ExpressionRouter(methodCallExpression.Arguments[1]));
                        return result;
                    }
                    if (methodCallExpression.Method.Name == "In")
                    {
                        result = string.Format("{0} In ({1})", this.ExpressionRouter(methodCallExpression.Arguments[0]), this.ExpressionRouter(methodCallExpression.Arguments[1]));
                        return result;
                    }
                    if (methodCallExpression.Method.Name == "NotIn")
                    {
                        result = string.Format("{0} Not In ({1})", this.ExpressionRouter(methodCallExpression.Arguments[0]), this.ExpressionRouter(methodCallExpression.Arguments[1]));
                        return result;
                    }
                }
                else if (exp is ConstantExpression)
                {
                    ConstantExpression constantExpression = (ConstantExpression)exp;
                    if (constantExpression.Value == null)
                    {
                        result = "null";
                        return result;
                    }
                    if (constantExpression.Value is ValueType)
                    {
                        result = constantExpression.Value.ToString();
                        return result;
                    }
                    if (constantExpression.Value is string || constantExpression.Value is DateTime || constantExpression.Value is char)
                    {
                        result = string.Format("'{0}'", constantExpression.Value.ToString());
                        return result;
                    }
                    if (constantExpression.Value is string[] || constantExpression.Value is char[])
                    {
                        result = string.Format("'{0}'", string.Join("','", (object[])constantExpression.Value));
                        return result;
                    }
                    if (constantExpression.Value is Array)
                    {
                        result = string.Format("{0}", string.Join(",", (object[])constantExpression.Value));
                        return result;
                    }
                }
                else if (exp is UnaryExpression)
                {
                    UnaryExpression unaryExpression = (UnaryExpression)exp;
                    result = this.ExpressionRouter(unaryExpression.Operand);
                    return result;
                }
                result = null;
            }
            return result;
        }

        private string ExpressionTypeCast(ExpressionType type)
        {
            string result;
            if (type <= ExpressionType.LessThanOrEqual)
            {
                switch (type)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                        result = "+";
                        return result;
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        result = " AND ";
                        return result;
                    default:
                        switch (type)
                        {
                            case ExpressionType.Divide:
                                result = "/";
                                return result;
                            case ExpressionType.Equal:
                                result = " =";
                                return result;
                            case ExpressionType.GreaterThan:
                                result = " >";
                                return result;
                            case ExpressionType.GreaterThanOrEqual:
                                result = ">=";
                                return result;
                            case ExpressionType.LessThan:
                                result = "<";
                                return result;
                            case ExpressionType.LessThanOrEqual:
                                result = "<=";
                                return result;
                        }
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        result = "*";
                        return result;
                    default:
                        switch (type)
                        {
                            case ExpressionType.NotEqual:
                                result = "<>";
                                return result;
                            case ExpressionType.Or:
                            case ExpressionType.OrElse:
                                result = " Or ";
                                return result;
                            case ExpressionType.Subtract:
                            case ExpressionType.SubtractChecked:
                                result = "-";
                                return result;
                        }
                        break;
                }
            }
            result = null;
            return result;
        }
    }
}
