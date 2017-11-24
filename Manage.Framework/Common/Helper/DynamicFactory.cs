using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Manage.Framework
{
    internal static class DynamicFactory
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, Type> s_dynamicTypes = new System.Collections.Concurrent.ConcurrentDictionary<Type, Type>();

        private static Func<Type, Type> s_dynamicTypeCreator = new Func<Type, Type>(CreateDynamicType);

        /// <summary>
        /// 将dynamic类型的对象传递到view页面
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static object ToDynamic(object entity)
        {
            var entityType = entity.GetType();
            var dynamicType = s_dynamicTypes.GetOrAdd(entityType, s_dynamicTypeCreator);

            var dynamicObject = Activator.CreateInstance(dynamicType);

            foreach (var entityProperty in entityType.GetProperties())
            {
                var value = entityProperty.GetValue(entity, null);
                dynamicType.GetField(entityProperty.Name).SetValue(dynamicObject, value);
            }

            return dynamicObject;
        }

        private static Type CreateDynamicType(Type entityType)
        {
            var asmName = new System.Reflection.AssemblyName("DynamicAssembly_" + Guid.NewGuid());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, System.Reflection.Emit.AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule("DynamicModule_" + Guid.NewGuid());

            var typeBuilder = moduleBuilder.DefineType(
                entityType.GetType() + "$DynamicType",
                System.Reflection.TypeAttributes.Public);

            typeBuilder.DefineDefaultConstructor(System.Reflection.MethodAttributes.Public);

            foreach (var entityProperty in entityType.GetProperties())
            {
                typeBuilder.DefineField(entityProperty.Name, entityProperty.PropertyType, System.Reflection.FieldAttributes.Public);
            }

            return typeBuilder.CreateType();
        }
    }
}
