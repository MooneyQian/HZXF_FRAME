
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Manage.Framework.Cache.Core
{
    /// <summary>
    /// 自定序列化绑定操作
    /// </summary>
    class Binder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type tyType = null;

            string sShortAssemblyName = assemblyName.Split(',')[0];
            Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly ayAssembly in ayAssemblies)
            {
                if (sShortAssemblyName == ayAssembly.FullName.Split(',')[0])
                {
                    tyType = ayAssembly.GetType(typeName);
                    break;
                }
            }
            return tyType;
        }
    }
}
