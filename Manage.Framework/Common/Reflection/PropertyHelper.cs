
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Manage.Framework
{
    /// <summary>
    /// 反射属性助手
    /// </summary>
    public class PropertyHelper
    {
        /// <summary>
        /// set缓存池
        /// </summary>
        private static Dictionary<PropertyInfo, DynamicMethodHelper> _setPool = new Dictionary<PropertyInfo, DynamicMethodHelper>();
        /// <summary>
        /// get缓存池
        /// </summary>
        private static Dictionary<PropertyInfo, DynamicMethodHelper> _getPool = new Dictionary<PropertyInfo, DynamicMethodHelper>();
        private static object lockhelper = new object();
        /// <summary>
        /// 增加属性
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="assemblyName">程序集名</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="returnType">返回类型</param>
        /// <param name="parameterTypes">参数类型</param>
        public static void AddProperties(Type t, string assemblyName, string propertyName, Type returnType, params Type[] parameterTypes)
        {
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(t.Assembly.GetName(), AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder mb = ab.DefineDynamicModule(t.Assembly.GetName() + ".dll");
            TypeBuilder typeBuilder = mb.DefineType(t.Name, TypeAttributes.Public);

            //创建字段
            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, returnType, FieldAttributes.Private);
            //创建属性
            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, returnType, null);

            MethodAttributes GetSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            //创建get方法
            MethodBuilder currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, GetSetAttr, returnType, Type.EmptyTypes);
            ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
            currGetIL.Emit(OpCodes.Ldarg_0);
            currGetIL.Emit(OpCodes.Ldfld, field);
            currGetIL.Emit(OpCodes.Ret);
            property.SetGetMethod(currGetPropMthdBldr);

            //创建set方法
            MethodBuilder currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName, GetSetAttr, null, parameterTypes);
            ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
            currSetIL.Emit(OpCodes.Ldarg_0);
            currSetIL.Emit(OpCodes.Ldarg_1);
            currSetIL.Emit(OpCodes.Stfld, field);
            currSetIL.Emit(OpCodes.Ret);
            property.SetSetMethod(currSetPropMthdBldr);
        }
        /// <summary>
        /// 快速设置属性值
        /// </summary>
        /// <param name="pi">属性片段</param>
        /// <param name="instance">实体</param>
        /// <param name="value">属性值</param>
        /// <param name="value"></param>
        public static void FastSetValue(PropertyInfo pi, object instance, object[] value)
        {
            lock (lockhelper)
            {
                if (!_setPool.ContainsKey(pi))
                {
                    _setPool.Add(pi, new DynamicMethodHelper(pi.GetSetMethod()));
                }
                ((DynamicMethodHelper)_setPool[pi]).Execute(instance, value);
            }
        }
        /// <summary>
        /// 快速获取属性值
        /// </summary>
        /// <param name="pi">属性片段</param>
        /// <param name="instance">实体</param>
        /// <returns></returns>
        public static object FastGetValue(PropertyInfo pi, object instance)
        {
            lock (lockhelper)
            {
                if (!_getPool.ContainsKey(pi))
                {
                    _getPool.Add(pi, new DynamicMethodHelper(pi.GetGetMethod()));
                }
                return ((DynamicMethodHelper)_getPool[pi]).Execute(instance, null);
            }
        }
    }
}
