using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Manage.Framework;

namespace Mamage.Framework
{
    internal static class PartialEvaluator
    {
        private class SubtreeEvaluator : AiExpressionVisitor
        {
            private HashSet<Expression> candidates;

            private SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }

            internal static Expression Eval(HashSet<Expression> candidates, Expression exp)
            {
                return new PartialEvaluator.SubtreeEvaluator(candidates).Visit(exp);
            }

            protected override Expression Visit(Expression exp)
            {
                Expression result;
                if (exp == null)
                {
                    result = null;
                }
                else if (this.candidates.Contains(exp))
                {
                    result = this.Evaluate(exp);
                }
                else
                {
                    result = base.Visit(exp);
                }
                return result;
            }

            private Expression Evaluate(Expression e)
            {
                Type type = e.Type;
                if (e.NodeType == ExpressionType.Convert)
                {
                    UnaryExpression unaryExpression = (UnaryExpression)e;
                    if (AiTypeHelper.GetNonNullableType(unaryExpression.Operand.Type) == AiTypeHelper.GetNonNullableType(type))
                    {
                        e = ((UnaryExpression)e).Operand;
                    }
                }
                Expression result;
                if (e.NodeType == ExpressionType.Constant)
                {
                    ConstantExpression constantExpression = (ConstantExpression)e;
                    if (e.Type != type && AiTypeHelper.GetNonNullableType(e.Type) == AiTypeHelper.GetNonNullableType(type))
                    {
                        e = Expression.Constant(constantExpression.Value, type);
                    }
                    result = e;
                }
                else
                {
                    MemberExpression memberExpression = e as MemberExpression;
                    if (memberExpression != null)
                    {
                        ConstantExpression constantExpression = memberExpression.Expression as ConstantExpression;
                        if (constantExpression != null)
                        {
                            result = Expression.Constant(memberExpression.Member.AiGetValue(constantExpression.Value), type);
                            return result;
                        }
                    }
                    if (type.IsValueType)
                    {
                        e = Expression.Convert(e, typeof(object));
                    }
                    Expression<Func<object>> expression = Expression.Lambda<Func<object>>(e, new ParameterExpression[0]);
                    Func<object> func = expression.Compile();
                    result = Expression.Constant(func(), type);
                }
                return result;
            }
        }

        private class Nominator : AiExpressionVisitor
        {
            private Func<Expression, bool> fnCanBeEvaluated;

            private HashSet<Expression> candidates;

            private bool cannotBeEvaluated;

            private Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.candidates = new HashSet<Expression>();
                this.fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal static HashSet<Expression> Nominate(Func<Expression, bool> fnCanBeEvaluated, Expression expression)
            {
                PartialEvaluator.Nominator nominator = new PartialEvaluator.Nominator(fnCanBeEvaluated);
                nominator.Visit(expression);
                return nominator.candidates;
            }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                return base.VisitConstant(c);
            }

            protected override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    bool flag = this.cannotBeEvaluated;
                    this.cannotBeEvaluated = false;
                    base.Visit(expression);
                    if (!this.cannotBeEvaluated)
                    {
                        if (this.fnCanBeEvaluated(expression))
                        {
                            this.candidates.Add(expression);
                        }
                        else
                        {
                            this.cannotBeEvaluated = true;
                        }
                    }
                    this.cannotBeEvaluated |= flag;
                }
                return expression;
            }
        }

        public static Expression Eval(Expression expression)
        {
            return PartialEvaluator.Eval(expression, null);
        }

        public static Expression Eval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            if (fnCanBeEvaluated == null)
            {
                fnCanBeEvaluated = new Func<Expression, bool>(PartialEvaluator.CanBeEvaluatedLocally);
            }
            return PartialEvaluator.SubtreeEvaluator.Eval(PartialEvaluator.Nominator.Nominate(fnCanBeEvaluated, expression), expression);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }
    }
}
