using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Manage.Framework
{
    internal static class AiTypeHelper
    {
        public static Type FindIEnumerable(Type seqType)
        {
            Type result;
            if (seqType == null || seqType == typeof(string))
            {
                result = null;
            }
            else if (seqType.IsArray)
            {
                result = typeof(IEnumerable<>).MakeGenericType(new Type[]
				{
					seqType.GetElementType()
				});
            }
            else
            {
                if (seqType.IsGenericType)
                {
                    Type[] array = seqType.GetGenericArguments();
                    for (int i = 0; i < array.Length; i++)
                    {
                        Type type = array[i];
                        Type type2 = typeof(IEnumerable<>).MakeGenericType(new Type[]
						{
							type
						});
                        if (type2.IsAssignableFrom(seqType))
                        {
                            result = type2;
                            return result;
                        }
                    }
                }
                Type[] interfaces = seqType.GetInterfaces();
                if (interfaces != null && interfaces.Length > 0)
                {
                    Type[] array = interfaces;
                    for (int i = 0; i < array.Length; i++)
                    {
                        Type seqType2 = array[i];
                        Type type2 = AiTypeHelper.FindIEnumerable(seqType2);
                        if (type2 != null)
                        {
                            result = type2;
                            return result;
                        }
                    }
                }
                if (seqType.BaseType != null && seqType.BaseType != typeof(object))
                {
                    result = AiTypeHelper.FindIEnumerable(seqType.BaseType);
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public static Type GetSequenceType(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(new Type[]
			{
				elementType
			});
        }

        public static Type GetElementType(Type seqType)
        {
            Type type = AiTypeHelper.FindIEnumerable(seqType);
            Type result;
            if (type == null)
            {
                result = seqType;
            }
            else
            {
                result = type.GetGenericArguments()[0];
            }
            return result;
        }

        public static bool IsNullableType(Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullAssignable(Type type)
        {
            return !type.IsValueType || AiTypeHelper.IsNullableType(type);
        }

        public static Type GetNonNullableType(Type type)
        {
            Type result;
            if (AiTypeHelper.IsNullableType(type))
            {
                result = type.GetGenericArguments()[0];
            }
            else
            {
                result = type;
            }
            return result;
        }

        public static Type GetNullAssignableType(Type type)
        {
            Type result;
            if (!AiTypeHelper.IsNullAssignable(type))
            {
                result = typeof(Nullable<>).MakeGenericType(new Type[]
				{
					type
				});
            }
            else
            {
                result = type;
            }
            return result;
        }

        public static ConstantExpression GetNullConstant(Type type)
        {
            return Expression.Constant(null, AiTypeHelper.GetNullAssignableType(type));
        }

        public static Type GetMemberType(MemberInfo mi)
        {
            FieldInfo fieldInfo = mi as FieldInfo;
            Type result;
            if (fieldInfo != null)
            {
                result = fieldInfo.FieldType;
            }
            else
            {
                PropertyInfo propertyInfo = mi as PropertyInfo;
                if (propertyInfo != null)
                {
                    result = propertyInfo.PropertyType;
                }
                else
                {
                    EventInfo eventInfo = mi as EventInfo;
                    if (eventInfo != null)
                    {
                        result = eventInfo.EventHandlerType;
                    }
                    else
                    {
                        MethodInfo methodInfo = mi as MethodInfo;
                        if (methodInfo != null)
                        {
                            result = methodInfo.ReturnType;
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
            }
            return result;
        }

        public static object GetDefault(Type type)
        {
            object result;
            if (type.IsValueType && !AiTypeHelper.IsNullableType(type))
            {
                result = Activator.CreateInstance(type);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static bool IsReadOnly(MemberInfo member)
        {
            MemberTypes memberType = member.MemberType;
            bool result;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    result = true;
                }
                else
                {
                    PropertyInfo propertyInfo = (PropertyInfo)member;
                    result = (!propertyInfo.CanWrite || propertyInfo.GetSetMethod() == null);
                }
            }
            else
            {
                result = ((((FieldInfo)member).Attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope);
            }
            return result;
        }

        public static bool IsInteger(Type type)
        {
            Type nonNullableType = AiTypeHelper.GetNonNullableType(type);
            bool result;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
    }
}
