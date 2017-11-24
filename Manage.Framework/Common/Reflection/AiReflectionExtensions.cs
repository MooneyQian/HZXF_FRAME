using System;
using System.Reflection;

namespace Manage.Framework
{
    internal static class AiReflectionExtensions
    {
        public static object AiGetValue(this MemberInfo member, object instance)
        {
            MemberTypes memberType = member.MemberType;
            object value;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    throw new InvalidOperationException();
                }
                value = ((PropertyInfo)member).GetValue(instance, null);
            }
            else
            {
                value = ((FieldInfo)member).GetValue(instance);
            }
            return value;
        }

        public static void AiSetValue(this MemberInfo member, object instance, object value)
        {
            MemberTypes memberType = member.MemberType;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    throw new InvalidOperationException();
                }
                PropertyInfo propertyInfo = (PropertyInfo)member;
                propertyInfo.SetValue(instance, value, null);
            }
            else
            {
                FieldInfo fieldInfo = (FieldInfo)member;
                fieldInfo.SetValue(instance, value);
            }
        }
    }
}
