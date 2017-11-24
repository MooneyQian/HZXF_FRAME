using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Manage.Framework
{
    internal abstract class AiExpressionVisitor
    {
        protected virtual Expression Visit(Expression exp)
        {
            Expression result;
            if (exp != null)
            {
                switch (exp.NodeType)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.Coalesce:
                    case ExpressionType.Divide:
                    case ExpressionType.Equal:
                    case ExpressionType.ExclusiveOr:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LeftShift:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.Modulo:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.RightShift:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                        result = this.VisitBinary((BinaryExpression)exp);
                        return result;
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.Quote:
                    case ExpressionType.TypeAs:
                        result = this.VisitUnary((UnaryExpression)exp);
                        return result;
                    case ExpressionType.Call:
                        result = this.VisitMethodCall((MethodCallExpression)exp);
                        return result;
                    case ExpressionType.Conditional:
                        result = this.VisitConditional((ConditionalExpression)exp);
                        return result;
                    case ExpressionType.Constant:
                        result = this.VisitConstant((ConstantExpression)exp);
                        return result;
                    case ExpressionType.Invoke:
                        result = this.VisitInvocation((InvocationExpression)exp);
                        return result;
                    case ExpressionType.Lambda:
                        result = this.VisitLambda((LambdaExpression)exp);
                        return result;
                    case ExpressionType.ListInit:
                        result = this.VisitListInit((ListInitExpression)exp);
                        return result;
                    case ExpressionType.MemberAccess:
                        result = this.VisitMemberAccess((MemberExpression)exp);
                        return result;
                    case ExpressionType.MemberInit:
                        result = this.VisitMemberInit((MemberInitExpression)exp);
                        return result;
                    case ExpressionType.New:
                        result = this.VisitNew((NewExpression)exp);
                        return result;
                    case ExpressionType.NewArrayInit:
                    case ExpressionType.NewArrayBounds:
                        result = this.VisitNewArray((NewArrayExpression)exp);
                        return result;
                    case ExpressionType.Parameter:
                        result = this.VisitParameter((ParameterExpression)exp);
                        return result;
                    case ExpressionType.TypeIs:
                        result = this.VisitTypeIs((TypeBinaryExpression)exp);
                        return result;
                }
                throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
            result = exp;
            return result;
        }

        protected virtual Expression VisitUnknown(Expression expression)
        {
            throw new Exception(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
        }

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            MemberBinding result;
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    result = this.VisitMemberAssignment((MemberAssignment)binding);
                    break;
                case MemberBindingType.MemberBinding:
                    result = this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                    break;
                case MemberBindingType.ListBinding:
                    result = this.VisitMemberListBinding((MemberListBinding)binding);
                    break;
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
            return result;
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> readOnlyCollection = this.VisitExpressionList(initializer.Arguments);
            ElementInit result;
            if (readOnlyCollection != initializer.Arguments)
            {
                result = Expression.ElementInit(initializer.AddMethod, readOnlyCollection);
            }
            else
            {
                result = initializer;
            }
            return result;
        }

        protected virtual Expression VisitUnary(UnaryExpression u)
        {
            Expression expression = this.Visit(u.Operand);
            Expression result;
            if (expression != u.Operand)
            {
                result = Expression.MakeUnary(u.NodeType, expression, u.Type, u.Method);
            }
            else
            {
                result = u;
            }
            return result;
        }

        protected virtual Expression VisitBinary(BinaryExpression b)
        {
            Expression expression = this.Visit(b.Left);
            Expression expression2 = this.Visit(b.Right);
            Expression expression3 = this.Visit(b.Conversion);
            Expression result;
            if (expression != b.Left || expression2 != b.Right || expression3 != b.Conversion)
            {
                if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                {
                    result = Expression.Coalesce(expression, expression2, expression3 as LambdaExpression);
                }
                else
                {
                    result = Expression.MakeBinary(b.NodeType, expression, expression2, b.IsLiftedToNull, b.Method);
                }
            }
            else
            {
                result = b;
            }
            return result;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
        {
            Expression expression = this.Visit(b.Expression);
            Expression result;
            if (expression != b.Expression)
            {
                result = Expression.TypeIs(expression, b.TypeOperand);
            }
            else
            {
                result = b;
            }
            return result;
        }

        protected virtual Expression VisitConstant(ConstantExpression c)
        {
            return c;
        }

        protected virtual Expression VisitConditional(ConditionalExpression c)
        {
            Expression expression = this.Visit(c.Test);
            Expression expression2 = this.Visit(c.IfTrue);
            Expression expression3 = this.Visit(c.IfFalse);
            Expression result;
            if (expression != c.Test || expression2 != c.IfTrue || expression3 != c.IfFalse)
            {
                result = Expression.Condition(expression, expression2, expression3);
            }
            else
            {
                result = c;
            }
            return result;
        }

        protected virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression m)
        {
            Expression expression = this.Visit(m.Expression);
            Expression result;
            if (expression != m.Expression)
            {
                result = Expression.MakeMemberAccess(expression, m.Member);
            }
            else
            {
                result = m;
            }
            return result;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression m)
        {
            Expression expression = this.Visit(m.Object);
            IEnumerable<Expression> enumerable = this.VisitExpressionList(m.Arguments);
            Expression result;
            if (expression != m.Object || enumerable != m.Arguments)
            {
                result = Expression.Call(expression, m.Method, enumerable);
            }
            else
            {
                result = m;
            }
            return result;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            int i = 0;
            int count = original.Count;
            while (i < count)
            {
                Expression expression = this.Visit(original[i]);
                if (list != null)
                {
                    list.Add(expression);
                }
                else if (expression != original[i])
                {
                    list = new List<Expression>(count);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(expression);
                }
                i++;
            }
            ReadOnlyCollection<Expression> result;
            if (list != null)
            {
                result = list.AsReadOnly();
            }
            else
            {
                result = original;
            }
            return result;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Expression expression = this.Visit(assignment.Expression);
            MemberAssignment result;
            if (expression != assignment.Expression)
            {
                result = Expression.Bind(assignment.Member, expression);
            }
            else
            {
                result = assignment;
            }
            return result;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            IEnumerable<MemberBinding> enumerable = this.VisitBindingList(binding.Bindings);
            MemberMemberBinding result;
            if (enumerable != binding.Bindings)
            {
                result = Expression.MemberBind(binding.Member, enumerable);
            }
            else
            {
                result = binding;
            }
            return result;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            IEnumerable<ElementInit> enumerable = this.VisitElementInitializerList(binding.Initializers);
            MemberListBinding result;
            if (enumerable != binding.Initializers)
            {
                result = Expression.ListBind(binding.Member, enumerable);
            }
            else
            {
                result = binding;
            }
            return result;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            int i = 0;
            int count = original.Count;
            while (i < count)
            {
                MemberBinding memberBinding = this.VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(memberBinding);
                }
                else if (memberBinding != original[i])
                {
                    list = new List<MemberBinding>(count);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(memberBinding);
                }
                i++;
            }
            IEnumerable<MemberBinding> result;
            if (list != null)
            {
                result = list;
            }
            else
            {
                result = original;
            }
            return result;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            int i = 0;
            int count = original.Count;
            while (i < count)
            {
                ElementInit elementInit = this.VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(elementInit);
                }
                else if (elementInit != original[i])
                {
                    list = new List<ElementInit>(count);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(elementInit);
                }
                i++;
            }
            IEnumerable<ElementInit> result;
            if (list != null)
            {
                result = list;
            }
            else
            {
                result = original;
            }
            return result;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            Expression expression = this.Visit(lambda.Body);
            Expression result;
            if (expression != lambda.Body)
            {
                result = Expression.Lambda(lambda.Type, expression, lambda.Parameters);
            }
            else
            {
                result = lambda;
            }
            return result;
        }

        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> enumerable = this.VisitExpressionList(nex.Arguments);
            NewExpression result;
            if (enumerable != nex.Arguments)
            {
                if (nex.Members != null)
                {
                    result = Expression.New(nex.Constructor, enumerable, nex.Members);
                }
                else
                {
                    result = Expression.New(nex.Constructor, enumerable);
                }
            }
            else
            {
                result = nex;
            }
            return result;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression newExpression = this.VisitNew(init.NewExpression);
            IEnumerable<MemberBinding> enumerable = this.VisitBindingList(init.Bindings);
            Expression result;
            if (newExpression != init.NewExpression || enumerable != init.Bindings)
            {
                result = Expression.MemberInit(newExpression, enumerable);
            }
            else
            {
                result = init;
            }
            return result;
        }

        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            NewExpression newExpression = this.VisitNew(init.NewExpression);
            IEnumerable<ElementInit> enumerable = this.VisitElementInitializerList(init.Initializers);
            Expression result;
            if (newExpression != init.NewExpression || enumerable != init.Initializers)
            {
                result = Expression.ListInit(newExpression, enumerable);
            }
            else
            {
                result = init;
            }
            return result;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> enumerable = this.VisitExpressionList(na.Expressions);
            Expression result;
            if (enumerable != na.Expressions)
            {
                if (na.NodeType == ExpressionType.NewArrayInit)
                {
                    result = Expression.NewArrayInit(na.Type.GetElementType(), enumerable);
                }
                else
                {
                    result = Expression.NewArrayBounds(na.Type.GetElementType(), enumerable);
                }
            }
            else
            {
                result = na;
            }
            return result;
        }

        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> enumerable = this.VisitExpressionList(iv.Arguments);
            Expression expression = this.Visit(iv.Expression);
            Expression result;
            if (enumerable != iv.Arguments || expression != iv.Expression)
            {
                result = Expression.Invoke(expression, enumerable);
            }
            else
            {
                result = iv;
            }
            return result;
        }
    }
}
